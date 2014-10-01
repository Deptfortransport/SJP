// *********************************************** 
// NAME             : CyclePlannerManager.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: CyclePlannerManager class which constructs the Cycle planner calls 
// and waits for the results to be returned
// ************************************************
// 

using System;
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
using CPWS = SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// CyclePlannerManager class which constructs the Cycle planner calls 
    /// and waits for the results to be returned
    /// </summary>
    public class CyclePlannerManager : ICyclePlannerManager
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
        public CyclePlannerManager()
        {
        }

        #endregion

        #region ICyclePlannerManager methods

        /// <summary>
        /// CallCyclePlanner handles the orchestration of the various calls to the Cycle planner.
        /// </summary>
        /// <param name="request">Encapsulates cycle journey parameters</param>
        /// <param name="sessionID">Used for logging purposes only</param>
        /// <param name="referenceTransaction">True is this an SLA-monitoring transaction</param>
        /// <param name="language">Two-character ISO id ("en" or "cy") of the current UI language</param>
        /// <returns>The results of the enquiry, including any error messages</returns>
        public ISJPJourneyResult CallCyclePlanner(ISJPJourneyRequest request, string sessionId, bool referenceTransaction, string language)
        {
            this.sessionId = sessionId;
            this.referenceTransaction = referenceTransaction;
            this.language = language;

            return CallCyclePlanner(request);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Common private method called by CallCyclePlanner public method
        /// to do the real work of handling the Cycle planner call.
        /// </summary>
        private ISJPJourneyResult CallCyclePlanner(ISJPJourneyRequest request)
        {
            IPropertyProvider propertyProvider = Properties.Current;

            // Unique reference number for the set of calls in this journey request
            int journeyReferenceNumber = SqlHelper.GetRefNumInt();

            #region Determine if  log Requests/Responses
            
            // Log raw Cycle planner inputs and outputs only when in Verbose mode
            bool logAllRequests = SJPTraceSwitch.TraceVerbose;
            bool logAllResponses = SJPTraceSwitch.TraceVerbose;
                        
            #endregion

            #region Create cycle planner calls
            
            // Create the Cycle planner calls
            JourneyRequestPopulator populator = JourneyRequestPopulatorFactory.GetPopulator(request);
            
            CyclePlannerCall[] cyclePlannerCallList = populator.PopulateRequestsCTP(
                journeyReferenceNumber, 0, sessionId,
                referenceTransaction, userType, language);

            #endregion

            #region Invoke cycle planner calls

            // INVOKE THE CYCLE PLANNER CALLs
            WaitHandle[] wh = new WaitHandle[cyclePlannerCallList.Length];
            
            // Track number of calls
            int callCount = 0;
            bool cyclePlannerFailed = false;

            foreach (CyclePlannerCall cpCall in cyclePlannerCallList)
            {
                #region Log request
                
                // MIS event
                LogRequest(request, cpCall.RequestId, false, sessionId);

                if (logAllRequests)
                {
                    // Log request xml
                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                        "CyclePlanner - CyclePlannerManager, logging the cycle journey request to submit."));
                    
                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                        ConvertToXML(cpCall.Request, cpCall.RequestId)));
                }

                #endregion

                // Submit the journey request to the cycle planner
                wh[callCount] = cpCall.InvokeCyclePlanner();

                if (wh[callCount] == null)
                {
                    cyclePlannerFailed = true;
                }

                callCount++;
            }

            #endregion

            #region Setup the result object
            
            // The return object which will be populated with journeys and/or error messages
            SJPJourneyResult result = new SJPJourneyResult(
                    request.JourneyRequestHash, journeyReferenceNumber,
                    request.Origin, request.Destination,
                    request.ReturnOrigin, request.ReturnDestination,
                    request.OutwardDateTime, request.ReturnDateTime,
                    request.OutwardArriveBefore, request.ReturnArriveBefore,
                    request.AccessiblePreferences.Accessible,
                    LanguageHelper.ParseLanguage(language),
                    request.ReplanOutwardJourneys, request.ReplanReturnJourneys,
                    request.ReplanRetainOutwardJourneys, request.ReplanRetainReturnJourneys);
            
            #endregion

            #region Process cycle planner call results

            // Timeout value
            int cyclePlannerTimeOut = propertyProvider[Keys.TimeoutMillisecs_CyclePlanner].Parse(60000); // Default to 60secs
            // Failures count
            int cyclePlannerFailureCount = 0;

            // Wait for parallel Cycle planner calls to finish.
            // This method will return when either:
            //  - all calls have completed; or
            //  - the timeout period is exceeded.
            if (!cyclePlannerFailed)
            {
                #region Wait for Cycle planner calls to return
                
                //Added code change for multithreaded journey planning. The code uses WaitOne instead
                //of WaitAll for returning threads. The timeout is adjusted for remaining threads to 
                //ensure the overall timeout (cyclePlannerTimeOut) is enforced. 

                // ----- WaitOne code -------- 
                string message = string.Format("CyclePlanner - CyclePlannerManager Thread has returned Ref[{0}].", journeyReferenceNumber);
                int startTime, endTime;
                                
                foreach (ManualResetEvent mh in wh)
                {
                    startTime = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                    mh.WaitOne(cyclePlannerTimeOut, false);
                    endTime = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                    cyclePlannerTimeOut = cyclePlannerTimeOut - (endTime - startTime);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose, 
                        string.Format("{0} Remaining Timeout[{1}]", message, cyclePlannerTimeOut.ToString())));
                }
                // ----- End WaitOne code -------- 

                #endregion

                foreach (CyclePlannerCall cpCall in cyclePlannerCallList)
                {
                    bool thisCallFailed = true;

                    // Get the result from the call
                    CPWS.CyclePlannerResult cyclePlannerResult = cpCall.Result;
                    CPWS.JourneyResult journeyResult = null;

                    #region Add the journey to the result object

                    if (cpCall.IsSuccessful)
                    {
                        #region Convert result in to CTP JourneyResult
                        try
                        {
                            // Convert the result into a journey result so we can get at the cycle journeys
                            journeyResult = (CPWS.JourneyResult)cyclePlannerResult;

                            #region Log result
                            if (logAllResponses)
                            {
                                // Log result xml
                                Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                                    "CyclePlanner - CyclePlannerManager, logging the cycle journey result received."));

                                Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                                    ConvertToXML(journeyResult, cpCall.RequestId)));
                            }
                            #endregion
                        }
                        catch
                        {
                            #region Log error

                            // If there is an exception then result returned by the cycle planner was invalid
                            Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error,
                                         "CyclePlanner - CyclePlannerManager, exception occurred attempting to cast CyclePlannerResult into a JourneyResult, for request: " + cpCall.RequestId));

                            // Log any messages contained in the result 
                            if ((cyclePlannerResult != null) && (cyclePlannerResult.messages != null) && (cyclePlannerResult.messages.Length > 0))
                            {
                                foreach (CPWS.Message msg in cyclePlannerResult.messages)
                                {
                                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                                        string.Format("CyclePlanner - CyclePlannerResult included message Code[{0}] Message[{1}] for Request[{2}]",
                                            msg.code.ToString(),
                                            msg.description,
                                            cpCall.RequestId)));
                                }
                            }

                            #endregion
                        }

                        #endregion
                        
                        // Add the cycle journey to the result object, noting if it is ok
                        thisCallFailed = !result.AddResult(journeyResult, !cpCall.IsReturnJourney);
                    }

                    if (thisCallFailed)
                    {
                        cyclePlannerFailureCount++;
                    }

                    #endregion
                }
            } // All cycle planner calls processed

            #endregion

            #region Check for errors

            #region No journeys planned

            // If outward journeys were requested but no outward journeys were planned, and no errors were raised by CTP, 
            // then this would indicate CTP couldn't plan a journey for the locations
            if (request.IsOutwardRequired)
            {
                if (result.OutwardJourneys.Count == 0)
                {
                    if (result.JourneyValidError)
                    {
                        if (result.Messages.Count == 0)
                        {
                            result.AddMessageToArray(string.Empty, Messages.CTPNoResults, null, 0, 0, SJPMessageType.Warning);
                        }
                    }
                }
            }

            // If return journeys were requested but not planned, and no errors were raised by CTP,
            // then this would indicate CTP couldn't plan the return journey for the locations
            if (request.IsReturnRequired)
            {
                if (result.ReturnJourneys.Count == 0)
                {
                    if (result.JourneyValidError)
                    {
                        if (result.Messages.Count == 0)
                        {
                            result.AddMessageToArray(string.Empty, Messages.CTPNoResults, null, 0, 0, SJPMessageType.Warning);
                        }
                    }
                }
            }

            #endregion

            #region Calls failed

            // If couldn't invoke a cycle planner service call
            if (cyclePlannerFailed)
            {
                result.AddMessageToArray(string.Empty, Messages.CTPInternalError, null, Codes.CTPCallError, 0, SJPMessageType.Error);
            }
            else
            {
                // Check for multiple cycle planner call fails
                if (cyclePlannerFailureCount == cyclePlannerCallList.Length)
                {
                    if (result.Messages.Count == 0)
                    {
                        result.AddMessageToArray(string.Empty, Messages.CTPInternalError, null, Codes.CTPCallError, 0, SJPMessageType.Error);
                    }

                    cyclePlannerFailed = true;
                }
                else if (cyclePlannerFailureCount > 0)
                {
                    if (result.Messages.Count == 0)
                    {
                        result.AddMessageToArray(string.Empty, Messages.CTPPartialReturn, null, Codes.CTPCallError, 0, SJPMessageType.Warning);
                    }
                }
            }

            #endregion

            #endregion

            #region Log

            // Retrieve the formatted reference number from the first call object
            string requestId = ((CyclePlannerCall)cyclePlannerCallList[0]).RequestId;

            LogResponse(result, requestId, false, cyclePlannerFailed, sessionId);

            #endregion

            return result;
        }

        #region MIS Events

        /// <summary>
        /// Logs a Cycle Planner request event
        /// </summary>
        private void LogRequest(ISJPJourneyRequest request, string requestId, bool isLoggedOn, string sessionId)
        {
            // Log a request event.
            if (CustomEventSwitch.On("CyclePlannerRequestEvent"))
            {
                CyclePlannerRequestEvent cpre = new CyclePlannerRequestEvent(requestId,
                    isLoggedOn,
                    sessionId);
                Logger.Write(cpre);
            }
        }

        /// <summary>
        /// Logs a Cycle Planner result event
        /// </summary>
        private void LogResponse(ISJPJourneyResult result, string requestId, bool isLoggedOn, bool cyclePlannerFailed, string sessionId)
        {
            if (CustomEventSwitch.On("CyclePlannerResultEvent"))
            {
                JourneyPlanResponseCategory responseCategory = JourneyPlanResponseCategory.Results;

                if (cyclePlannerFailed)
                {
                    responseCategory = JourneyPlanResponseCategory.Failure;
                }
                else if (result.OutwardJourneys.Count == 0 && result.ReturnJourneys.Count == 0)
                {
                        responseCategory = JourneyPlanResponseCategory.ZeroResults;
                }

                CyclePlannerResultEvent cpre = new CyclePlannerResultEvent(requestId,
                    responseCategory,
                    isLoggedOn,
                    sessionId);
                Logger.Write(cpre);
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
