// *********************************************** 
// NAME             : CyclePlannerCall.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: CyclePlannerCall class which contains the call and invokes the Cycle planner service
// ************************************************
// 

using System;
using System.Threading;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.CyclePlannerService;
using CPWS = SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// CyclePlannerCall class which contains the call and invokes the Cycle planner service
    /// </summary>
    public class CyclePlannerCall
    {
        #region Private members

        private delegate CPWS.CyclePlannerResult JourneyPlanAsyncDelegate(CPWS.CyclePlannerRequest request);

        private CPWS.JourneyRequest request;
        private bool isReturn;
        private bool success;

        private int referenceNumber;
        private string sessionId;

        private JourneyPlanAsyncDelegate cyclePlannerDelegate;
        private IAsyncResult cyclePlannerDelegateASR;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">the JourneyRequest to be passed as input to the CTP</param>
        /// <param name="isReturn">true if for return journey, false for outward</param>
        /// <param name="referenceNumber">Used by CTP for logging</param>
        /// <param name="sessionId">Used by CTP for logging</param>
        public CyclePlannerCall(CPWS.JourneyRequest request, bool isReturn, int referenceNumber, string sessionId)
        {
            this.request = request;
            this.isReturn = isReturn;
            this.referenceNumber = referenceNumber;
            this.sessionId = sessionId;
        }

        #endregion

        #region Call Cycle Planner service

        /// <summary>
        /// Initiates an asynchronous call to the Cycle Planner service itself.
        /// </summary>
        /// <returns>
        /// A WaitHandle that will be used to determine when the call 
        /// has completed, or null if the call generated an exception.
        /// </returns>
        public WaitHandle InvokeCyclePlanner()
        {
            try
            {
                ICyclePlanner cyclePlanner = SJPServiceDiscovery.Current.Get<ICyclePlanner>(ServiceDiscoveryKey.CTP);
                cyclePlannerDelegate = new JourneyPlanAsyncDelegate(cyclePlanner.CycleJourneyPlan);
                cyclePlannerDelegateASR = cyclePlannerDelegate.BeginInvoke(request, null, null);
                return cyclePlannerDelegateASR.AsyncWaitHandle;
            }
            catch (Exception e)
            {
                string message = "CyclePlanner - InvokeCyclePlanner exception after attempting to call web service, for request " + request.requestID;

                OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message, e, sessionId);
                Logger.Write(oe);
                return null;
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read-only - the result of the Cycle Planner call (or null if not sucessful)
        /// </summary>
        public CPWS.CyclePlannerResult Result
        {
            get
            {
                try
                {
                    if (cyclePlannerDelegateASR.IsCompleted)
                    {
                        // Get the result
                        CPWS.CyclePlannerResult result = cyclePlannerDelegate.EndInvoke(cyclePlannerDelegateASR);

                        success = true;

                        return result;
                    }
                    else
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error,
                            "CyclePlanner - CyclePlannerCall has timed out for request: " + request.requestID,
                            null,
                            sessionId));

                        success = false;
                        return null;
                    }
                }
                catch (Exception e)
                {
                    string message = "CyclePlanner - CyclePlannerCall exception attempting to get and return CyclePlannerResult for request: " + request.requestID;

                    OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message, e, sessionId);
                    Logger.Write(oe);
                    success = false;
                    return null;
                }
            }
        }

        /// <summary>
        /// Read-only - the Request used to call the Cycle Planner service  
        /// </summary>
        public CPWS.JourneyRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Read-only - true if request was for return journey, false if outward 
        /// </summary>
        public bool IsReturnJourney
        {
            get { return isReturn; }
        }

        /// <summary>
        /// Read-only - true if Cycle Planner call was successful
        /// </summary>
        public bool IsSuccessful
        {
            get { return success; }
        }

        /// <summary>
        /// Read-only - the request-id 
        /// Query reference, plus 4-digit suffix to make unique per-request
        /// </summary>
        public string RequestId
        {
            get { return request.requestID; }
        }

        #endregion
    }
}
