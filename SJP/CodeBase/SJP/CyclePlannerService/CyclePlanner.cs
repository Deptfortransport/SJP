// *********************************************** 
// NAME             : CyclePlanner.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: CyclePlanner class to call the Cycle Trip Planner (CTP)
// ************************************************
// 

using System;
using System.ServiceModel;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using CPWS = SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.CyclePlannerService
{
    /// <summary>
    /// CyclePlanner class to call the Cycle Trip Planner (CTP)
    /// </summary>
    public class CyclePlanner : ICyclePlanner
    {
        #region Private members
                
        // Web service properties
        BasicHttpBinding serviceBinding = null;
        EndpointAddress serviceEndpoint = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CyclePlanner()
        {
            // Read configuration properites
            string serviceAddress = Properties.Current["CyclePlanner.WebService.URL"];
            int serviceTimeout = Properties.Current["CyclePlanner.WebService.Timeout.Seconds"].Parse<int>(60); // Default to 60 seconds

            if (string.IsNullOrEmpty(serviceAddress))
            {
                throw new SJPException("CyclePlanner - Property[CyclePlanner.WebService.URL] is missing or invalid, unable to initialise Cycle Planner Web Service",
                    false, SJPExceptionIdentifier.CYCyclePlannerWebServiceUrlNotValid);
            }

            //initialise the service
            serviceBinding = new BasicHttpBinding();
            serviceBinding.SendTimeout = new TimeSpan(0, 0, serviceTimeout);
            serviceBinding.ReceiveTimeout = new TimeSpan(0, 0, serviceTimeout);
            serviceBinding.MaxReceivedMessageSize = Properties.Current["CyclePlanner.WebService.MaxReceivedMessageSize"].Parse(1000000);
            serviceEndpoint = new EndpointAddress(serviceAddress);
        }

        #endregion

        #region ICyclePlanner methods

        /// <summary>
        /// Calls the Cycle planner web service
        /// </summary>
        /// <param name="cyclePlannerRequest"></param>
        /// <returns></returns>
        public CPWS.CyclePlannerResult CycleJourneyPlan(CPWS.CyclePlannerRequest cyclePlannerRequest)
        {
            if (SJPTraceSwitch.TraceVerbose)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                    string.Format("CyclePlanner - Web service CycleJounreyPlan call started. RequestID[{0}]", cyclePlannerRequest.requestID)));
            }

            // Declare the output object
            CPWS.CyclePlannerResult result = null;

            // Create the web service, place in using so it is disposed of correctly
            using (CPWS.CyclePlannerServiceSoapClient webServiceCyclePlanner = new CPWS.CyclePlannerServiceSoapClient(serviceBinding, serviceEndpoint))
            {
                DateTime callStartTime = DateTime.UtcNow;
                bool successful = false;

                try
                {
                    // Call webservice
                    result = webServiceCyclePlanner.JourneyPlan(cyclePlannerRequest);

                    // Set succesful here if we want to note down that the call produced successful results
                    successful = true;
                }
                catch (Exception ex)
                {
                    LogException(ex);
                }
                finally
                {
                    //log the callevent
                    LogCallEvent("CyclePlannerWebService.JourneyPlan", callStartTime, successful);
                }
            }

            if (SJPTraceSwitch.TraceVerbose)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                    string.Format("CyclePlanner - Web service CycleJourneyPlan call completed. RequestId[{0}]", cyclePlannerRequest.requestID)));
            }

            return (CPWS.CyclePlannerResult)result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method that logs an exception if one is detected when calling the web service, 
        /// and throws an SJPException
        /// </summary>
        /// <param name="we">Web exception</param>
        /// <returns>TD exception</returns>
        private void LogException(Exception ex)
        {
            string message = string.Format(
                "CyclePlanner - An exception has occured when trying to call the CyclePlannerWebService. Error: {0}",
                ex.Message);
                    

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message);
            Logger.Write(oe);

            //log an operational event and create new SJPException using the web exception 
            throw new SJPException(message, true, SJPExceptionIdentifier.CYCyclePlannerWebServiceCallError, ex);
        }

        /// <summary>
        /// Method that logs the web service call time, and whether it was successful
        /// </summary>
        /// <param name="callStartTime"></param>
        /// <param name="successful"></param>
        private void LogCallEvent(string webServiceMethod, DateTime callStartTime, bool successful)
        {
            string message = string.Format(
                "CyclePlanner - Web service call made to Method[{0}] Start[{1}] Successful[{2}]",
                webServiceMethod,
                callStartTime,
                successful);

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, message);
            Logger.Write(oe);
        }

        #endregion
    }
}
