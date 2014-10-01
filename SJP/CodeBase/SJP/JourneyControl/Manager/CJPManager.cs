// *********************************************** 
// NAME             : CJPManager.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: CJPManager class which constructs the CJP calls and waits for 
// the results to be returned
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
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyControl
{
    public class CJPManager : ICJPManager
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
        public CJPManager()
        {
        }

        #endregion

        #region ICJPManager methods

        /// <summary>
        /// CallCJP handles the orchestration of the various calls to the Journey planner.
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
        /// Method which performs the work for calling the journey planning
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ISJPJourneyResult CallCJP(ISJPJourneyRequest request)
        {
            IPropertyProvider propertyProvider = Properties.Current;

            // Unique reference number for the set of calls in this journey request
            int journeyReferenceNumber = SqlHelper.GetRefNumInt();

            #region Determine if  log Requests/Responses

            // Log raw CJP inputs and outputs only when in Verbose mode
            bool logAllRequests = SJPTraceSwitch.TraceVerbose;
            bool logAllResponses = SJPTraceSwitch.TraceVerbose;

            #endregion

            #region Create journey planner calls

            // Create the CJP calls
            JourneyRequestPopulator populator = JourneyRequestPopulatorFactory.GetPopulator(request);

            CJPCall[] cjpCallList = populator.PopulateRequestsCJP(
                journeyReferenceNumber, 0, sessionId,
                referenceTransaction, userType, language);

            #endregion

            #region Invoke journey planner calls

            // INVOKE THE CJP CALLs
            WaitHandle[] wh = new WaitHandle[cjpCallList.Length];

            // Track number of calls
            int callCount = 0;
            bool cjpFailed = false;

            foreach (CJPCall cjpCall in cjpCallList)
            {
                #region Log request

                // MIS event
                LogRequest(request, cjpCall.RequestId, false, sessionId);

                if (logAllRequests)
                {
                    // Log request xml
                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                        "JourneyPlanner - CJPManager, logging the journey request to submit."));

                    Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                        ConvertToXML(cjpCall.Request, cjpCall.RequestId)));
                }

                #endregion

                // Submit the journey request to the journey planner
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
                    request.JourneyRequestHash, journeyReferenceNumber,
                    request.Origin, request.Destination,
                    request.ReturnOrigin, request.ReturnDestination,
                    request.OutwardDateTime, request.ReturnDateTime,
                    request.OutwardArriveBefore, request.ReturnArriveBefore,
                    request.AccessiblePreferences.Accessible,
                    LanguageHelper.ParseLanguage(language.Substring(0, 2)),
                    request.ReplanOutwardJourneys, request.ReplanReturnJourneys,
                    request.ReplanRetainOutwardJourneys, request.ReplanRetainReturnJourneys);

            #endregion

            #region Process journey planner call results

            // Timeout value
            int cjpTimeOut = propertyProvider[Keys.TimeoutMillisecs_CJP].Parse(60000); // Default to 60secs
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
                string message = string.Format("JourneyPlanner - CJPManager Thread has returned Ref[{0}].", journeyReferenceNumber);
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

                foreach (CJPCall cjpCall in cjpCallList)
                {
                    bool thisCallFailed = true;

                    // Get the result from the call
                    ICJP.JourneyResult journeyResult = cjpCall.CJPResult;

                    #region Add the journey to the result object

                    if (cjpCall.IsSuccessful)
                    {
                        #region Log result
                        if (logAllResponses)
                        {
                            // Log result xml
                            Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                                "JourneyPlanner - CJPManager, logging the journey result received."));

                            Logger.Write(new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Verbose,
                                ConvertToXML(journeyResult, cjpCall.RequestId)));
                        }
                        #endregion

                        #region Special "no journeys found" scenarios

                        // Fix for Bug 122:
                        // It is possible the request submitted to the CJP could have the same origin and destination 
                        // locations. This can happen when the planner mode is RiverService, where the user
                        // has entered an origin pier, then selected a river service route to venue with the 
                        // same start pier as origin, and then initiates a PT plan to the pier- hence the journey becomes:
                        //      Pier A to Pier A to Pier B to Venue.
                        // If this happens, then the CJP will fail the PT journey plan, but because we may have the 
                        // successful "JourneyPart" for the river service route, we can return the JourneyPart built 
                        // into a journey.
                        bool canCreateJourneyUsingOnlyJourneyPart = UseJourneyPart(cjpCall.Request,
                            !cjpCall.IsReturnJourney ? request.OutwardJourneyPart : request.ReturnJourneyPart);

                        // Fix for Bug 195:
                        // Is is possible the request submitted to the CJP could have the same origin and destination
                        // locations. This can happen for an Accessible journey request where an "accessible naptan" was
                        // used as the To Venue location, and the From location is the same NaPTAN.
                        // If this happens, then the CJP will fail the journey plan, but because we add on a
                        // Transfer leg manually from the accessible NaPTAN, we can build and return a transfer only leg journey
                        bool canCreateJourneyUsingAccessibleTransfer = UseAccessibleTransfer(cjpCall.Request,
                            request.AccessiblePreferences.Accessible);

                        #endregion

                        // Add the journey to the result object, noting if it is ok
                        thisCallFailed = !result.AddResult(journeyResult, !cjpCall.IsReturnJourney,
                            !cjpCall.IsReturnJourney ? request.OutwardJourneyPart : request.ReturnJourneyPart,
                            canCreateJourneyUsingOnlyJourneyPart, canCreateJourneyUsingAccessibleTransfer);
                    }

                    if (thisCallFailed)
                    {
                        cjpFailureCount++;
                    }

                    #endregion
                }

                // Sort the journeys
                result.SortJourneys();

            } // All journey planner calls processed

            #endregion

            #region Check for errors

            string errorResourceId = string.Empty;

            #region No journeys planned

            // If journeys requested but no journeys were planned, and no errors were raised by CJP, 
            // then this would indicate CJP couldn't plan a journey for the locations. Display
            // the no journeys found error. 
            // Scenarios:
            // - Outward and Return request and both have no journeys found
            // - Or only Outward request which has no journeys found
            // - Or only Return request which has no journeys found.
            //
            // Partial journeys found (i.e. outward or return part failed), then the error message
            // is added in the Calls failed logic.
            //
            if ((result.OutwardJourneys.Count == 0 && result.ReturnJourneys.Count == 0)
                || (request.IsOutwardRequired && !request.IsReturnRequired && result.OutwardJourneys.Count == 0)
                || (request.IsReturnRequired && !request.IsOutwardRequired && result.ReturnJourneys.Count == 0))
            {
                if (result.JourneyValidError)
                {
                    if (result.Messages.Count == 0)
                    {
                        if (request.PlannerMode == SJPJourneyPlannerMode.ParkAndRide ||
                            request.PlannerMode == SJPJourneyPlannerMode.BlueBadge)
                        {
                            // Add Car specific message
                            errorResourceId = Messages.CJPNoResults_Car;
                        }
                        else
                        {
                            // Add PT specific message
                            if (request.AccessiblePreferences.RequireStepFreeAccess
                                && request.AccessiblePreferences.RequireSpecialAssistance)
                            {
                                errorResourceId = Messages.CJPNoResults_WheelchairAssistance;
                            }
                            else if (request.AccessiblePreferences.RequireStepFreeAccess)
                            {
                                errorResourceId = Messages.CJPNoResults_Wheelchair;
                            }
                            else if (request.AccessiblePreferences.RequireSpecialAssistance)
                            {
                                errorResourceId = Messages.CJPNoResults_Assistance;
                            }
                            else
                            {
                                errorResourceId = Messages.CJPNoResults;
                            }
                        }

                        result.AddMessageToArray(string.Empty, errorResourceId, null, 0, 0, SJPMessageType.Warning);
                    }
                }
            }


            #endregion

            #region Calls failed

            // If couldn't invoke a journey planner call
            if (cjpFailed)
            {
                result.AddMessageToArray(string.Empty, Messages.CJPInternalError, null, Codes.CjpCallError, 0, SJPMessageType.Error);
            }
            else
            {
                // All journey planner call fails
                if (cjpFailureCount == cjpCallList.Length)
                {
                    if (result.Messages.Count == 0)
                    {
                        result.AddMessageToArray(string.Empty, Messages.CJPInternalError, null, Codes.CjpCallError, 0, SJPMessageType.Error);
                    }

                    cjpFailed = true;
                }
                // Partial journey planner call failure
                else if (cjpFailureCount > 0)
                {
                    if (result.Messages.Count == 0)
                    {
                        // Outward planned, Return failed
                        if (result.OutwardJourneys.Count != 0 && result.ReturnJourneys.Count == 0)
                        {
                            #region Outward journeys planned ok

                            if (request.PlannerMode == SJPJourneyPlannerMode.ParkAndRide ||
                                request.PlannerMode == SJPJourneyPlannerMode.BlueBadge)
                            {
                                // Car specific message
                                errorResourceId = Messages.CJPPartialResults_Outward_Car;
                            }
                            else
                            {
                                // Add PT specific message
                                if (request.AccessiblePreferences.RequireStepFreeAccess
                                    && request.AccessiblePreferences.RequireSpecialAssistance)
                                {
                                    errorResourceId = Messages.CJPPartialResults_Outward_WheelchairAssistance;
                                }
                                else if (request.AccessiblePreferences.RequireStepFreeAccess)
                                {
                                    errorResourceId = Messages.CJPPartialResults_Outward_Wheelchair;
                                }
                                else if (request.AccessiblePreferences.RequireSpecialAssistance)
                                {
                                    errorResourceId = Messages.CJPPartialResults_Outward_Assistance;
                                }
                                else
                                {
                                    errorResourceId = Messages.CJPPartialResults_Outward;
                                }
                            }

                            #endregion
                        }
                        // Return planned, Outward failed
                        else
                        {
                            #region Return journeys planned ok

                            if (request.PlannerMode == SJPJourneyPlannerMode.ParkAndRide ||
                                request.PlannerMode == SJPJourneyPlannerMode.BlueBadge)
                            {
                                // Car specific message
                                errorResourceId = Messages.CJPPartialResults_Return_Car;
                            }
                            else
                            {
                                // Add PT specific message
                                if (request.AccessiblePreferences.RequireStepFreeAccess
                                    && request.AccessiblePreferences.RequireSpecialAssistance)
                                {
                                    errorResourceId = Messages.CJPPartialResults_Return_WheelchairAssistance;
                                }
                                else if (request.AccessiblePreferences.RequireStepFreeAccess)
                                {
                                    errorResourceId = Messages.CJPPartialResults_Return_Wheelchair;
                                }
                                else if (request.AccessiblePreferences.RequireSpecialAssistance)
                                {
                                    errorResourceId = Messages.CJPPartialResults_Return_Assistance;
                                }
                                else
                                {
                                    errorResourceId = Messages.CJPPartialResults_Return;
                                }
                            }

                            #endregion
                        }

                        result.AddMessageToArray(string.Empty, errorResourceId, null, Codes.CjpCallError, 0, SJPMessageType.Warning);
                    }
                }
            }

            #endregion

            #endregion

            #region Log

            // Retrieve the formatted reference number from the first call object
            string requestId = ((CJPCall)cjpCallList[0]).RequestId;

            LogResponse(result, requestId, false, cjpFailed, sessionId);

            #endregion

            #region Add existing journeys when No journeys planned

            bool addExistingOutwardJourneys = (result.OutwardJourneys.Count == 0
                && request.ReplanRetainOutwardJourneysWhenNoResults
                && request.ReplanOutwardJourneys != null
                && request.ReplanOutwardJourneys.Count > 0);

            bool addExistingReturnJourneys = (result.ReturnJourneys.Count == 0
                && request.ReplanRetainReturnJourneysWhenNoResults
                && request.ReplanReturnJourneys != null
                && request.ReplanReturnJourneys.Count > 0);

            if (addExistingOutwardJourneys || addExistingReturnJourneys)
            {
                result.AddJourneys(request.ReplanOutwardJourneys, request.ReplanReturnJourneys,
                    addExistingOutwardJourneys, addExistingReturnJourneys);
            }

            #endregion

            return result;
        }

        /// <summary>
        /// Returns true if the journey part can be used soley to create a journey. 
        /// This will only return true if the journey part is not null, and the CJP request has 
        /// an origin and destination which are the same.
        /// See Bug 122 for further details
        /// </summary>
        /// <param name="cjpRequest"></param>
        /// <param name="journeyPart"></param>
        /// <returns></returns>
        private bool UseJourneyPart(ICJP.JourneyRequest cjpRequest, Journey journeyPart)
        {
            bool result = false;

            if ((journeyPart != null) && (cjpRequest != null))
            {
                // Check the origin and destination is for the same location
                if ((cjpRequest.origin != null) && (cjpRequest.destination != null))
                {
                    if ((cjpRequest.origin.stops != null) && (cjpRequest.destination.stops != null)
                        && (cjpRequest.origin.stops.Length == cjpRequest.destination.stops.Length)
                        && (cjpRequest.origin.stops.Length >= 1))
                    {
                        if (cjpRequest.origin.stops[0].NaPTANID == cjpRequest.destination.stops[0].NaPTANID)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns true if the request is for an accessible journey, and the origin and destination
        /// are the same
        /// See Bug 195 for further details
        /// </summary>
        /// <param name="cjpRequest"></param>
        /// <param name="accessibleJourney"></param>
        /// <returns></returns>
        private bool UseAccessibleTransfer(ICJP.JourneyRequest cjpRequest, bool accessibleJourney)
        {
            bool result = false;

            if ((accessibleJourney) && (cjpRequest != null))
            {
                // Check the origin and destination is for the same location
                if ((cjpRequest.origin != null) && (cjpRequest.destination != null))
                {
                    if ((cjpRequest.origin.stops != null) && (cjpRequest.destination.stops != null)
                        && (cjpRequest.origin.stops.Length == cjpRequest.destination.stops.Length)
                        && (cjpRequest.origin.stops.Length >= 1))
                    {
                        if (cjpRequest.origin.stops[0].NaPTANID == cjpRequest.destination.stops[0].NaPTANID)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        #region MIS Events

        /// <summary>
        /// Logs a Journey Planner request event
        /// </summary>
        private void LogRequest(ISJPJourneyRequest request, string requestId, bool isLoggedOn, string sessionId)
        {
            if  (CustomEventSwitch.On("JourneyPlanRequestEvent"))
            {
                JourneyPlanRequestEvent jpre = new JourneyPlanRequestEvent(requestId,
                    request.Modes,
                    isLoggedOn,
                    sessionId);
                Logger.Write(jpre);
            }
        }

        /// <summary>
        /// Logs a Journey Planner result event
        /// </summary>
        private void LogResponse(ISJPJourneyResult result, string requestId, bool isLoggedOn, bool cjpFailed, string sessionId)
        {
            if  (CustomEventSwitch.On("JourneyPlanResultsEvent"))
            {
                JourneyPlanResponseCategory responseCategory = JourneyPlanResponseCategory.Results;

                if  (cjpFailed)
                {
                    responseCategory = JourneyPlanResponseCategory.Failure;
                }
                else if  (result.OutwardJourneys.Count  == 0 && result.ReturnJourneys.Count == 0)
                {
                    responseCategory = JourneyPlanResponseCategory.ZeroResults;
                }
                
                JourneyPlanResultsEvent jpre = new JourneyPlanResultsEvent(requestId,
                    responseCategory,
                    isLoggedOn,
                    sessionId);
                Logger.Write(jpre);
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
