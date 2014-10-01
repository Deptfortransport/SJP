// *********************************************** 
// NAME             : CJPCall.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 30 Mar 2011
// DESCRIPTION  	: CJPCall class which contains the call and invokes the CJP
// ************************************************
// 


using System;
using System.Threading;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using CJP = TransportDirect.JourneyPlanning.CJP;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// CJPCall class which contains the call and invokes the CJP
    /// </summary>
    public class CJPCall
    {
        #region Private members

        private delegate ICJP.CJPResult JourneyPlanAsyncDelegate(ICJP.CJPRequest request);

        private ICJP.JourneyRequest request;
        private bool isReturn;
        private bool success;

        private int referenceNumber;
        private string sessionId;

        private JourneyPlanAsyncDelegate cjpDelegate;
        private IAsyncResult cjpDelegateASR;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">the JourneyRequest to be passed as input to the CJP</param>
        /// <param name="isReturn">true if for return journey, false for outward</param>
        /// <param name="referenceNumber">Used by CJP for logging</param>
        /// <param name="sessionId">Used by CJP for logging</param>
        public CJPCall(ICJP.JourneyRequest request, bool isReturn, int referenceNumber, string sessionId)
        {
            this.request = request;
            this.isReturn = isReturn;
            this.referenceNumber = referenceNumber;
            this.sessionId = sessionId;
        }

        #endregion

        #region Call CJP

        /// <summary>
        /// Initiates an asynchronous call to the CJP itself.
        /// </summary>
        /// <returns>
        /// A WaitHandle that will be used to determine when the call 
        /// has completed, or null if the call generated an exception.
        /// </returns>
        public WaitHandle InvokeCJP()
        {
            try
            {
                CJP.ICJP cjp = SJPServiceDiscovery.Current.Get<CJP.ICJP>(ServiceDiscoveryKey.CJP);
                cjpDelegate = new JourneyPlanAsyncDelegate(cjp.JourneyPlan);
                cjpDelegateASR = cjpDelegate.BeginInvoke(request, null, null);
                return cjpDelegateASR.AsyncWaitHandle;
            }
            catch (Exception e)
            {
                string message = "JourneyPlanner - InvokeCJP exception after attempting to call CJP, for request " + request.requestID;

                OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message, e, sessionId);
                Logger.Write(oe);
                return null;
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read-only - the result of the cjp call (or null if not sucessful)
        /// </summary>
        public ICJP.JourneyResult CJPResult
        {
            get
            {
                try
                {
                    if (cjpDelegateASR.IsCompleted)
                    {
                        success = true;
                        return (ICJP.JourneyResult)cjpDelegate.EndInvoke(cjpDelegateASR);
                    }
                    else
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error,
                            "JourneyPlanner - CJPCall has timed out for request: " + request.requestID,
                            null,
                            sessionId));

                        success = false;
                        return null;
                    }
                }
                catch (Exception e)
                {
                    string message = "JourneyPlanner - CJPCall exception attempting to get and return JourneyResult for request: " + request.requestID;

                    OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message, e, sessionId);
                    Logger.Write(oe);
                    success = false;
                    return null;
                }

            }
        }

        /// <summary>
        /// Read-only - the JourneyRequest used to call the CJP  
        /// </summary>
        public ICJP.JourneyRequest Request
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
        /// Read-only - true if CJP call was successful
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
