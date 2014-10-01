// *********************************************** 
// NAME             : CJPStopEventCall.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Class responsible for calling the CJP for StopEvent calls
// ************************************************
// 

using System;
using System.Text;
using System.Threading;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using CJP = TransportDirect.JourneyPlanning.CJP;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Class responsible for calling the CJP for StopEvent calls
    /// </summary>
    public class CJPStopEventCall
    {
        #region Private members

        private delegate ICJP.CJPResult StopEventAsyncDelegate(ICJP.CJPRequest request);

        private ICJP.EventRequest eventRequest = null;
        private bool success = false;
        private bool isReturn = false;
		
        private StopEventAsyncDelegate cjpDelegate = null;
		private IAsyncResult cjpDelegateASR = null;

        #endregion

        #region Constructor

        /// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="request">request to submit</param>
        public CJPStopEventCall(ICJP.EventRequest request, bool isReturn)
		{
			eventRequest = request;
            this.isReturn = isReturn;
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
                cjpDelegate = new StopEventAsyncDelegate(cjp.JourneyPlan);
                cjpDelegateASR = cjpDelegate.BeginInvoke(eventRequest, null, null);
                return cjpDelegateASR.AsyncWaitHandle;
            }
            catch (Exception e)
            {
                string message = "StopEvent - InvokeCJP exception after attempting to call CJP, for request " + eventRequest.requestID;

                OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message, e);
                Logger.Write(oe);
                return null;
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Method that returns the StopEventResult associated with the CJP call.
        /// </summary>
        /// <returns>StopEventResult object returned by CJP. null if error!</returns>
        public ICJP.StopEventResult GetResult()
        {
            try
            {
                if (cjpDelegateASR.IsCompleted)
                {
                    success = true;
                    
                    ICJP.CJPResult result = cjpDelegate.EndInvoke(cjpDelegateASR);

                    if (result != null && result is ICJP.StopEventResult)
                    {
                        return (ICJP.StopEventResult)result;
                    }

                    if (result.messages.Length > 0)
                    {
                        StringBuilder messageBuilder = new StringBuilder();

                        foreach (ICJP.Message m in result.messages)
                        {
                            messageBuilder.AppendFormat("{0}:{1} \t", m.code, m.description);
                        }
                        
                        OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Warning, 
                            "StopEvent - " + eventRequest.requestID + " CJPCall come back with messages : " + messageBuilder.ToString());

                        Logger.Write(oe);

                        ICJP.StopEventResult seResult = new ICJP.StopEventResult();
                        seResult.messages = result.messages;

                        success = true;
                        return seResult;
                    }

                    success = false;
                    return null;
                }
                else
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error,
                        "StopEvent - CJPCall has timed out for request: " + eventRequest.requestID));

                    success = false;
                    return null;
                }
            }
            catch (Exception e)
            {
                string message = "StopEvent - CJPCall exception attempting to get and return StopEventResult for request: " + eventRequest.requestID;

                OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message, e);
                Logger.Write(oe);
                success = false;
                return null;
            }
        }
        
        /// <summary>
        /// Read-only - the EventRequest used to call the CJP  
        /// </summary>
        public ICJP.EventRequest Request
        {
            get { return eventRequest; }
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
        /// </summary>
        public string RequestId
        {
            get { return eventRequest.requestID; }
        }

        #endregion
    }
}
