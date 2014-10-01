// *********************************************** 
// NAME             : StopEventRunnerCaller.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Apr 2011
// DESCRIPTION  	: Implementation of IStopEventRunnerCaller class provides 
//                    interface methods to invoke CJP Stop Event
// ************************************************


using System;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.StateServer;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Summary description for StopEventRunnerCaller.
    /// </summary>
    public class StopEventRunnerCaller : MarshalByRefObject, IStopEventRunnerCaller
    {
        #region Delegates

        private delegate void DelegateInvokeStopEventManager(ISJPJourneyRequest journeyRequest, string sessionID, string lang);

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public StopEventRunnerCaller()
        {
        }

        #endregion

        #region IStopEventRunnerCaller Members

        /// <summary>
        /// Invoke the StopEventManager 
        /// </summary>
        /// <param name="journeyRequest">The validated user's Journey Request</param>
        /// <param name="sessionInfo">Information on the user's session</param>
        /// <param name="lang">The language of the current UI culture</param>
        public void InvokeStopEventManager(ISJPJourneyRequest journeyRequest, ISJPSession sessionInfo, string lang)
        {
            DelegateInvokeStopEventManager theDelegate = new DelegateInvokeStopEventManager(InvokeStopEvent);
            theDelegate.BeginInvoke(journeyRequest, sessionInfo.SessionID, lang, null, null);
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Invoke the StopEventManager 
        /// </summary>
        /// <param name="journeyRequest">The validated user's Journey Request</param>
        /// <param name="sessionID">Information on the user's sessionID</param>
        /// <param name="lang">The language of the current UI culture</param>
        private void InvokeStopEvent(ISJPJourneyRequest journeyRequest, string sessionID, string lang)
        {
            try
            {
                //Call the StopEventManager
                CallStopEvent(journeyRequest, sessionID, lang);
            }
            catch (Exception e)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("StopEventRunnerCaller - Exception Message: [{0}], Stack Trace: [{1}]", e.Message, e.StackTrace), e));
            }
        }

        #endregion

        #region Methods to call the CJP

        /// <summary>
        /// Call the StopEventManager
        /// </summary>
        /// <param name="journeyRequest">The journey request for submission to the CJP StopEvent</param>
        /// <param name="sessionID">User's sessionID</param>
        /// <param name="lang">Language of the journey detail expected</param>
        public void CallStopEvent(ISJPJourneyRequest journeyRequest, string sessionID, string lang)
        {
            try
            {
                // Get a StopEventManager from the service discovery
                IStopEventManager stopEventManager = SJPServiceDiscovery.Current.Get<IStopEventManager>(ServiceDiscoveryKey.StopEventManager);

                #region Initiate stop event request

                // Indicate that we are NOT an SLA monitoring transaction
                bool referenceTransation = false;

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("StopEventRunnerCaller - Calling StopEventManager to return stop event journeys for SessionId[{0}] and JourneyRequestHash[{1}]",
                            sessionID,
                            journeyRequest.JourneyRequestHash)));
                }

                ISJPJourneyResult journeyResult = stopEventManager.CallCJP(
                    journeyRequest,
                    sessionID,
                    referenceTransation,
                    lang);

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("StopEventRunnerCaller - StopEventManager has returned for SessionId[{0}] and JourneyRequestHash[{1}]",
                            sessionID,
                            journeyRequest.JourneyRequestHash)));
                }

                #endregion

                #region Save stop event journey to session

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("StopEventRunnerCaller - Saving stop event journey result to the state server for SessionId[{0}], JourneyRequestHash[{1}], JourneyReferenceNumber[{2}]",
                            sessionID,
                            journeyRequest.JourneyRequestHash,
                            SqlHelper.FormatRef(journeyResult.JourneyReferenceNumber))));
                }

                // Get an instance of the StateServer and directly add the stop event journey result.

                // IMPORTANT - Relying on only this code in the Solution creating/adding/updating the 
                // SJPStopEventResultManager object. Therefore there should never be an issue/race condition 
                // of the UI thread and JourneyPlanning thread overriding the result object in the StateServer.

                // Possible SJPJourneyResult race condition. If two StopEvent threads detect 
                // there is no result manager object in session, both create a new one, and both then save, 
                // thus losing one journey set.
                using (SJPStateServer stateServer = new SJPStateServer())
                {
                    // Lock the ResultManager - prevents any other process altering the data
                    stateServer.Lock(sessionID, new string[] { SessionManagerKey.KeyStopEventResultManager.ID });

                    // What happens if we can't get a lock on the SessionManagerKey.KeyStopEventResultManager? 
                    // How do we handle the jouney result? This could happen if the UI is going through a session/page 
                    // cycle and is checking if the SJPStopEventResultManager contains journeys.

                    // Get the SJPResultManager
                    object objResultManager = stateServer.Read(sessionID, SessionManagerKey.KeyStopEventResultManager.ID);

                    if (objResultManager == null)
                    {
                        // First time user has performed stop event request so ok if it didnt exist, create a new instance
                        objResultManager = new SJPStopEventResultManager();
                    }

                    SJPStopEventResultManager resultManager = (SJPStopEventResultManager)objResultManager;

                    // Insert the stop event journey result 
                    resultManager.AddSJPJourneyResult(journeyResult);

                    // And save, this will release the lock
                    stateServer.Save(sessionID, SessionManagerKey.KeyStopEventResultManager.ID, resultManager);

                } // StateServer will be disposed, any outstanding locks will be gracefully released during this process
                // but ensure above code does this to avoid any delays

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("StopEventRunnerCaller - Finished saving stop event journey result to the state server for SessionId[{0}], JourneyRequestHash[{1}], JourneyReferenceNumber[{2}]",
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
                    string.Format("StopEventRunnerCaller - Exception was thrown planning the journey and/or saving to sesion for SessionId[{0}] and JourneyRequestHash[{1}]. Error Message[{2}], see exception for further details.",
                        sessionID,
                        journeyRequest.JourneyRequestHash,
                        ex.Message),
                    ex));
            }
        }

        #endregion
    }
}
