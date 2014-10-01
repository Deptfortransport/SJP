// *********************************************** 
// NAME             : JourneyPlanRunnerCaller.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: Implementation of IJourneyPlanRunnerCaller class provides 
//                    interface methods to invoke CJP
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.UserPortal.JourneyControl;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.SessionManager;
using SJP.Common.EventLogging;
using System.Diagnostics;
using SJP.UserPortal.StateServer;
using Logger = System.Diagnostics.Trace;
using SJP.Common.DatabaseInfrastructure;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Summary description for JourneyPlanRunnerCaller.
    /// </summary>
    public class JourneyPlanRunnerCaller : MarshalByRefObject, IJourneyPlanRunnerCaller
    {
        #region Delegates

        private delegate void DelegateInvokeCJPManager(ISJPJourneyRequest journeyRequest, string sessionID, string lang);
        
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public JourneyPlanRunnerCaller()
        {
        }

        #endregion

        #region IJourneyPlanRunnerCaller Members

        /// <summary>
        /// Invoke the CJP Manager 
        /// </summary>
        /// <param name="journeyRequest">The validated user's Journey Request</param>
        /// <param name="sessionInfo">Information on the user's session</param>
        /// <param name="lang">The language of the current UI culture</param>
        public void InvokeCJPManager(ISJPJourneyRequest journeyRequest, ISJPSession sessionInfo, string lang)
        {
            DelegateInvokeCJPManager theDelegate = new DelegateInvokeCJPManager(InvokeCJP);
            theDelegate.BeginInvoke(journeyRequest, sessionInfo.SessionID, lang, null, null);
        }

        
        #endregion

        #region Private Methods 

        /// <summary>
        /// Invoke the CJP Manager 
        /// </summary>
        /// <param name="journeyRequest">The validated user's Journey Request</param>
        /// <param name="sessionID">Information on the user's sessionID</param>
        /// <param name="lang">The language of the current UI culture</param>
        private void InvokeCJP(ISJPJourneyRequest journeyRequest, string sessionID, string lang)
        {
            try
            {
                //Call the CJP Manager
                CallCJP(journeyRequest, sessionID, lang);
            }
            catch (Exception e)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("JourneyPlanRunnerCaller - Exception Message: [{0}], Stack Trace: [{1}]", e.Message, e.StackTrace), e));
            }
        }

        #endregion

        #region Methods to call the CJP

        /// <summary>
        /// Call the CJP Manager
        /// </summary>
        /// <param name="journeyRequest">The journey request for submission to the CJP</param>
        /// <param name="sessionID">User's sessionID</param>
        /// <param name="lang">Language of the journey detail expected</param>
        public void CallCJP(ISJPJourneyRequest journeyRequest, string sessionID, string lang)
        {
            try
            {
                // Get a CJP Manager from the service discovery
                ICJPManager cjpManager = SJPServiceDiscovery.Current.Get<ICJPManager>(ServiceDiscoveryKey.CJPManager);

                #region Plan journeys

                // Indicate that we are NOT an SLA monitoring transaction
                bool referenceTransation = false;

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("JourneyPlanRunnerCaller - Calling CJPManager to plan journeys for SessionId[{0}] and JourneyRequestHash[{1}]",
                            sessionID,
                            journeyRequest.JourneyRequestHash)));
                }

                ISJPJourneyResult journeyResult = cjpManager.CallCJP(
                    journeyRequest,
                    sessionID,
                    referenceTransation,
                    lang);

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("JourneyPlanRunnerCaller - CJPManager has returned for SessionId[{0}] and JourneyRequestHash[{1}]",
                            sessionID,
                            journeyRequest.JourneyRequestHash)));
                }

                #endregion

                #region Save journey to session

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("JourneyPlanRunnerCaller - Saving journey result to the state server for SessionId[{0}], JourneyRequestHash[{1}], JourneyReferenceNumber[{2}]",
                            sessionID,
                            journeyRequest.JourneyRequestHash,
                            SqlHelper.FormatRef(journeyResult.JourneyReferenceNumber))));
                }

                // Get an instance of the StateServer and directly add the journey result.
                
                // IMPORTANT - Relying on only this code in the Solution creating/adding/updating the 
                // SJPResultManager object. Therefore there should never be an issue/race condition 
                // of the UI thread and JourneyPlanning thread overriding the result object in the StateServer.

                // Possible SJPJourneyResult race condition. If two JourneyPlanning threads detect 
                // there is no result manager object in session, both create a new one, and both then save, 
                // thus losing one journey set.
                using (SJPStateServer stateServer = new SJPStateServer())
                {
                    // Lock the ResultManager - prevents any other process altering the data
                    stateServer.Lock(sessionID, new string[] { SessionManagerKey.KeyResultManager.ID });

                    // What happens if we can't get a lock on the SessionManagerKey.KeyResultManager? 
                    // How do we handle the jouney result? This could happen is the UI is going through a session/page 
                    // cycle and is checking if the SJPResultManager contains journeys.

                    // Get the SJPResultManager
                    object objResultManager = stateServer.Read(sessionID, SessionManagerKey.KeyResultManager.ID);
                    
                    if (objResultManager == null)
                    {
                        // First time user has planned journey so ok if it didnt exist, create a new instance
                        objResultManager = new SJPResultManager();
                    }

                    SJPResultManager resultManager = (SJPResultManager)objResultManager;

                    // Insert the journey result 
                    resultManager.AddSJPJourneyResult(journeyResult);
                                       
                    // And save, this will release the lock
                    stateServer.Save(sessionID, SessionManagerKey.KeyResultManager.ID, resultManager);
                
                } // StateServer will be disposed, any outstanding locks will be gracefully released during this process
                  // but ensure above code does this to avoid any delays

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("JourneyPlanRunnerCaller - Finished saving journey result to the state server for SessionId[{0}], JourneyRequestHash[{1}], JourneyReferenceNumber[{2}]",
                            sessionID,
                            journeyRequest.JourneyRequestHash,
                            SqlHelper.FormatRef(journeyResult.JourneyReferenceNumber))));
                }

                #endregion

            }
            catch (Exception ex)
            {
                Logger.Write(new OperationalEvent(
                    SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("JourneyPlanRunnerCaller - Exception was thrown planning the journey and/or saving to sesion for SessionId[{0}] and JourneyRequestHash[{1}]. Error Message[{2}], see exception for further details.",
                        sessionID,
                        journeyRequest.JourneyRequestHash,
                        ex.Message), 
                    ex));
            }
        }
        
        #endregion
    }
}
