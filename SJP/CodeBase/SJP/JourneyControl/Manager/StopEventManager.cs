// *********************************************** 
// NAME             : StopEventManager.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: StopEventManager implementation of IStopEventManager interface
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Reporting.Events;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// StopEventManager implementation of IStopEventManager interface
    /// </summary>
    public class StopEventManager : IStopEventManager
    {
        #region Private members

        private string sessionId = string.Empty;
        private int userType = 0;
        private bool referenceTransaction = false;
        private string language = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public StopEventManager()
        {
        }

        #endregion

        #region IStopEventManager methods

        /// <summary>
        /// CallCJP handles the orchestration of the various calls to the Stop Event service.
        /// </summary>
        /// <param name="request">Encapsulates journey parameters</param>
        /// <param name="sessionID">Used for logging purposes only</param>
        /// <param name="referenceTransaction">True is this an SLA-monitoring transacation</param>
        /// <param name="language">Two-character ISO id ("en" or "cy") of the current UI language</param>
        /// <returns>The results of the enquiry, including any error messages</returns>
        public ISJPJourneyResult CallCJP(ISJPJourneyRequest request, string sessionId, bool referenceTransaction, string language)
        {
            this.sessionId = sessionId;
            this.referenceTransaction = referenceTransaction;
            this.language = language;

            return CallCJP(request);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method that submit a request to the CJP asynchronously and wait for it return if less than 
        /// timeout
        /// </summary>
        /// <param name="request">StopEvent request to submit</param>
        /// <returns>StopEventResult object returned by CJP</returns>
        private ISJPJourneyResult CallCJP(ISJPJourneyRequest request)
        {
            IPropertyProvider propertyProvider = Properties.Current;

            // Unique reference number for the set of calls in this stop event request
            int requestReferenceNumber = SqlHelper.GetRefNumInt();

            #region Determine if  log Requests/Responses

            // Log raw CJP inputs and outputs only when in Verbose mode
            bool logAllRequests = SJPTraceSwitch.TraceVerbose;
            bool logAllResponses = SJPTraceSwitch.TraceVerbose;

            #endregion

            #region Create journey planner calls

            // Create CJP calls
            StopEventRequestPopulator populator = new StopEventRequestPopulator(request);

            CJPStopEventCall[] cjpCallList = populator.PopulateRequestsCJPStopEvent(
                requestReferenceNumber, 0, sessionId,
                referenceTransaction, userType, language);

            #endregion
            
            #region Invoke stop event calls

            // Used for logging
            DateTime submitted = DateTime.Now;
            
            // INVOKE THE CJP CALLs
            WaitHandle[] wh = new WaitHandle[cjpCallList.Length];
                        
            // Track number of calls
            int callCount = 0;
            bool cjpFailed = false;

            foreach (CJPStopEventCall cjpCall in cjpCallList)
            {
                #region Log request
                                
                if (logAllRequests)
                {
                    // Log request xml
                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                        "StopEvent - StopEventPManager, logging the request to submit."));

                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                        ConvertToXML(cjpCall.Request, cjpCall.RequestId)));
                }

                #endregion

                // Submit the request
                wh[callCount] = cjpCall.InvokeCJP();

                if (wh[callCount] == null)
                {
                    cjpFailed = true;
                }

                callCount++;
            }

            #endregion

            #region Setup the result object

            // The return object which will be populated with journeys and/or error messages
            SJPJourneyResult result = new SJPJourneyResult(
                    request.JourneyRequestHash, requestReferenceNumber,
                    request.Origin, request.Destination,
                    request.ReturnOrigin, request.ReturnDestination,
                    request.OutwardDateTime, request.ReturnDateTime,
                    request.OutwardArriveBefore, request.ReturnArriveBefore,
                    (request.AccessiblePreferences == null) ? false : request.AccessiblePreferences.Accessible,
                    LanguageHelper.ParseLanguage(language),
                    request.ReplanOutwardJourneys, request.ReplanReturnJourneys,
                    request.ReplanRetainOutwardJourneys, request.ReplanRetainReturnJourneys);

            // Track how many journeys there are (to allow an error message to be shown if the replan did not find any new journeys)
            int journeysCountOutward = result.OutwardJourneys.Count;
            int journeysCountReturn = result.ReturnJourneys.Count;

            #endregion

            #region Process stop event call results

            // Timeout value
            int cjpTimeOut = propertyProvider[Keys.StopEventRequest_TimeoutMillisecs_CJP].Parse(30000); // Default to 30secs
            // Failures count
            int cjpFailureCount = 0;

            // Wait for parallel CJP calls to finish.
            // This method will return when either:
            //  - all calls have completed; or
            //  - the timeout period is exceeded.
            if (!cjpFailed)
            {
                #region Wait for CJP calls to return

                //Added code change for multithreaded journey planning. The code uses WaitOne instead
                //of WaitAll for returning threads. The timeout is adjusted for remaining threads to 
                //ensure the overall timeout (cjpTimeOut) is enforced. 

                // ----- WaitOne code -------- 
                string message = string.Format("StopEvent - StopEventManager Thread has returned Ref[{0}].", requestReferenceNumber);
                int startTime, endTime;

                foreach (ManualResetEvent mh in wh)
                {
                    startTime = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                    mh.WaitOne(cjpTimeOut, false);
                    endTime = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                    cjpTimeOut = cjpTimeOut - (endTime - startTime);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                        string.Format("{0} Remaining Timeout[{1}]", message, cjpTimeOut.ToString())));
                }
                // ----- End WaitOne code -------- 

                #endregion

                foreach (CJPStopEventCall cjpCall in cjpCallList)
                {
                    bool thisCallFailed = true;

                    // Get the result from the call
                    ICJP.StopEventResult stopEventResult = cjpCall.GetResult();

                    #region Add the stopEventResult to the result object

                    if (cjpCall.IsSuccessful)
                    {
                        #region Log result
                        if (logAllResponses)
                        {
                            // Log result xml
                            Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                                "StopEvent - StopEventManager, logging the stop event result received."));

                            Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                                ConvertToXML(stopEventResult, cjpCall.RequestId)));
                        }
                        #endregion

                        // Sort results
                        SortStopEventResult(stopEventResult, cjpCall.IsReturnJourney);

                        // Add the services to the result object, and noting if it is ok
                        thisCallFailed = !result.AddResult(
                            stopEventResult, 
                            !cjpCall.IsReturnJourney);
                    }

                    if (thisCallFailed)
                    {
                        cjpFailureCount++;
                    }

                    #endregion

                    #region MIS
                    
                    // determine which type is the request for logging
                    StopEventRequestType type = StopEventRequestType.Time;

                    if (cjpCall.Request is ICJP.FirstLastServiceRequest)
                    {
                        ICJP.FirstLastServiceRequest flReq = (ICJP.FirstLastServiceRequest)cjpCall.Request;
                        type = (flReq.type == ICJP.FirstLastServiceRequestType.First) ? StopEventRequestType.First : StopEventRequestType.Last;
                    }

                    // Retrieve the formatted reference number from the first call object
                    string requestId = cjpCall.RequestId;

                    LogResponse(requestId, submitted, type, thisCallFailed);

                    #endregion
                }

                // Sort the journeys
                result.SortJourneys();

            } // All calls processed
                        
            #endregion

            #region Check for errors

            #region No journeys planned

            // If outward services were requested but no outward services were found, and no errors were raised by CJP, 
            // then this would indicate CJP couldn't find services for the locations
            if (request.IsOutwardRequired)
            {
                if (result.OutwardJourneys.Count == 0)
                {
                    if (result.JourneyValidError)
                    {
                        if (result.Messages.Count == 0)
                        {
                            result.AddMessageToArray(string.Empty, Messages.CJPStopEventNoResults, null, 0, 0, SJPMessageType.Info);
                        }
                    }
                }
            }

            // If return services were requested but not planned, and no errors were raised by CJP,
            // then this would indicate CJP couldn't find services for the locations
            if (request.IsReturnRequired)
            {
                if (result.ReturnJourneys.Count == 0)
                {
                    if (result.JourneyValidError)
                    {
                        if (result.Messages.Count == 0)
                        {
                            result.AddMessageToArray(string.Empty, Messages.CJPStopEventNoResults, null, 0, 0, SJPMessageType.Info);
                        }
                    }
                }
            }

            #endregion

            #region Calls failed

            // If couldn't invoke a stop event call
            if (cjpFailed)
            {
                result.AddMessageToArray(string.Empty, Messages.CJPStopEventInternalError, null, Codes.CjpCallError, 0, SJPMessageType.Error);
            }
            else
            {
                // Check for multiple call fails
                if (cjpFailureCount == cjpCallList.Length)
                {
                    if (result.Messages.Count == 0)
                    {
                        result.AddMessageToArray(string.Empty, Messages.CJPStopEventInternalError, null, Codes.CjpCallError, 0, SJPMessageType.Error);
                    }

                    cjpFailed = true;
                }
                else if (cjpFailureCount > 0)
                {
                    if (result.Messages.Count == 0)
                    {
                        result.AddMessageToArray(string.Empty, Messages.CJPStopEventPartialReturn, null, Codes.CjpCallError, 0, SJPMessageType.Warning);
                    }
                }
                // If a replan, and no new journeys were added, display an approriate message
                else
                {
                    // Are there any new outward journeys in the replan
                    if ((request.ReplanIsOutwardRequired) && (journeysCountOutward > 0) 
                        && !(journeysCountOutward < result.OutwardJourneys.Count))
                    {
                        if (result.Messages.Count == 0)
                        {
                            // Earlier replan request
                            if (request.ReplanOutwardDateTime < request.OutwardDateTime)
                            {
                                result.AddMessageToArray(string.Empty, Messages.CJPStopEventNoEarlierResults, new List<string>() { request.OutwardDateTime.ToString("dd/MM/yyyy") }, Codes.SJPNoEarlierServicesOutward, 0, SJPMessageType.Warning);
                            }
                            else
                            {
                                result.AddMessageToArray(string.Empty, Messages.CJPStopEventNoLaterResults, new List<string>() { request.OutwardDateTime.ToString("dd/MM/yyyy") }, Codes.SJPNoLaterServicesOutward, 0, SJPMessageType.Warning);
                            }
                        }
                    }
                    
                    // Are there any new return journeys in the replan
                    if ((request.ReplanIsReturnRequired) && (journeysCountReturn > 0)
                        && !(journeysCountReturn < result.ReturnJourneys.Count))
                    {
                        if (result.Messages.Count == 0)
                        {
                            // Earlier replan request
                            if (request.ReplanReturnDateTime < request.ReturnDateTime)
                            {
                                result.AddMessageToArray(string.Empty, Messages.CJPStopEventNoEarlierResults, new List<string>() { request.ReturnDateTime.ToString("dd/MM/yyyy") }, Codes.SJPNoEarlierServicesReturn, 0, SJPMessageType.Warning);
                            }
                            else
                            {
                                result.AddMessageToArray(string.Empty, Messages.CJPStopEventNoLaterResults, new List<string>() { request.ReturnDateTime.ToString("dd/MM/yyyy") }, Codes.SJPNoLaterServicesReturn, 0, SJPMessageType.Warning);
                            }
                        }
                    }
                }
            }

            #endregion

            #endregion

            return result;
        }

        /// <summary>
        /// Method to sort the result by arrival/departure time
        /// </summary>
        /// <param name="sr">StopEventResult which  contains collection of StopEvent</param>
        private void SortStopEventResult(ICJP.StopEventResult sr, bool isDeparture)
        {
            try
            {
                if (sr.stopEvents != null && sr.stopEvents.Length != 0)
                {
                    List<ICJP.StopEvent> stopevents = new List<ICJP.StopEvent>(sr.stopEvents);

                    StopEventDateComparer comparer = new StopEventDateComparer(isDeparture);

                    stopevents.Sort(comparer);

                    sr.stopEvents = stopevents.ToArray();
                }
            }
            catch (ArgumentException aEx)
            {
                OperationalEvent oe = new OperationalEvent(
                    SJPEventCategory.ThirdParty,
                    SJPTraceLevel.Error, 
                    string.Format("Error occurred attempting to sort CJP StopEventResult, Message: {0}", aEx.Message));
                Logger.Write(oe);
            }
        }

        #region MIS Events

        /// <summary>
        /// Logs a Journey Planner result event
        /// </summary>
        private void LogResponse(string requestId, DateTime submitted, StopEventRequestType type, bool isSuccessful)
        {
            if (CustomEventSwitch.On("StopEventRequestEvent"))
            {
                StopEventRequestEvent stopEventRequestEvent = new StopEventRequestEvent(
                    requestId,
                    submitted,
                    type,
                    isSuccessful);

                Logger.Write(stopEventRequestEvent);
            }
        }

        #endregion

        /// <summary>
        /// Create an XML representtaion of the specified object,
        /// with leading whitespace trimmed, for logging purposes.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="requestId"></param>
        /// <returns>XML string, prefixed by request id</returns>
        private string ConvertToXML(object obj, string requestId)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xmls = new XmlSerializer(obj.GetType());
                xmls.Serialize(sw, obj);

                // strip out leading spaces to conserve space in logging ...
                Regex re = new Regex("\\r\\n\\s+");
                return (requestId + " " + re.Replace(sw.ToString(), "\r\n"));
            }
        }

        #endregion
    }
}
