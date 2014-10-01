// *********************************************** 
// NAME             : JourneyPlannerHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 03 Feb 2012
// DESCRIPTION  	: JourneyPlanner helper class to submit a journey request to be planned
// ************************************************
// 

using SJP.Common.EventLogging;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.JourneyPlanRunner;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.Web
{
    /// <summary>
    /// JourneyPlanner helper class to submit a journey request to be planned
    /// </summary>
    public class JourneyPlannerHelper
    {
        #region Private members

        private SessionHelper sessionHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyPlannerHelper()
        {
            sessionHelper = new SessionHelper();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Submit a journey request. 
        /// Retrieves the SJPJourneyRequest from session to submit using an IJourneyPlanRunner class.
        /// Any validation or error messages are added to the session
        /// </summary>
        /// <param name="submitRequest">submitRequest flag indicates if the request should 
        /// be validated only or validated and submitted</param>
        /// <returns></returns>
        public bool SubmitRequest(SJPJourneyPlannerMode plannerMode, bool submitRequest)
        {
            bool valid = false;

            try
            {
                IJourneyPlanRunner runner = null;

                if (plannerMode == SJPJourneyPlannerMode.PublicTransport ||
                    plannerMode == SJPJourneyPlannerMode.BlueBadge ||
                    plannerMode == SJPJourneyPlannerMode.ParkAndRide ||
                    plannerMode == SJPJourneyPlannerMode.RiverServices)
                {
                    #region Public transport and Car journey planning

                    runner = new JourneyPlanRunner();

                    #endregion
                }
                else if (plannerMode == SJPJourneyPlannerMode.Cycle)
                {
                    #region Cycle journey planning

                    runner = new CycleJourneyPlanRunner();

                    #endregion
                }
                else
                {
                    // Should never reach here as the above if statements cover all planner modes
                    throw new SJPException("Unexpected planner mode was detected, unable to initiate journey planning", false, SJPExceptionIdentifier.SWUndefinedPlannerMode);
                }

                #region Call Validate and Run

                // Get the journey request to submit
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

                // If the journey result already exists for this request, then save
                // performance by not submitting to journey planners.
                // This is safe because the JourneyRequestHash can be used to check if a request
                // contains identical "user entered parameters".
                bool resultExists = sessionHelper.DoesSJPJourneyResultExist(sjpJourneyRequest.JourneyRequestHash, true);

                if (resultExists)
                {
                    // Journey has already been planned and the result is in the session result manager.
                    valid = true;
                }
                else if (runner != null)
                {
                    // Initiate the journey planning
                    valid = runner.ValidateAndRun(sjpJourneyRequest, CurrentLanguage.Culture, submitRequest);

                    // If failed to initiate journey planning, check for any validation error messages,
                    // add to session for page to display
                    if (!valid)
                    {
                        sessionHelper.AddMessages(runner.Messages);
                    }
                }

                if (valid && submitRequest)
                {
                    // Successfully submitted the request, update request and save. This is done to support
                    // potential repeated identical landing page requests
                    sjpJourneyRequest.JourneyRequestSubmitted = true;
                    sessionHelper.UpdateSession(sjpJourneyRequest);
                }

                #endregion
            }
            catch (SJPException sjpEx)
            {
                if (!sjpEx.Logged)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, sjpEx.Message));
                }

                // Add message to be displayed
                sessionHelper.AddMessage(new SJPMessage("ValidateAndRun.ErrorMessage.General1", SJPMessageType.Error));
                sessionHelper.AddMessage(new SJPMessage("ValidateAndRun.ErrorMessage.General2", SJPMessageType.Error));

                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Submit a stop event request.
        /// Retrieves the StopEvent SJPJourneyRequest from session to submit using an IJourneyPlanRunner class.
        /// Any validation or error messages are added to the session
        /// </summary>
        /// <param name="submitRequest">submitRequest flag indicates if the request should 
        /// be validated only or validated and submitted</param>
        /// <returns></returns>
        public bool SubmitStopEventRequest(bool submitRequest)
        {
            bool valid = false;

            try
            {
                IJourneyPlanRunner runner = new StopEventRunner();

                #region Call Validate and Run

                // Get the stop event request to submit
                ISJPJourneyRequest sjpStopEventRequest = sessionHelper.GetSJPStopEventRequest();

                // If the result already exists for this stop event request, then save
                // performance by not submitting to journey planners.
                // This is safe because the JourneyRequestHash can be used to check if a request
                // contains identical "user entered parameters"
                bool resultExists = sessionHelper.DoesSJPStopEventResultExist(sjpStopEventRequest.JourneyRequestHash, true);

                if (resultExists)
                {
                    // Journey has already been planned and the result is in the session result manager.
                    valid = true;
                }
                else
                {
                    // Initiate the journey planning
                    valid = runner.ValidateAndRun(sjpStopEventRequest, CurrentLanguage.Culture, submitRequest);

                    // If failed to initiate journey planning, check for any validation error messages,
                    // add to session for page to display
                    if (!valid)
                    {
                        sessionHelper.AddMessages(runner.Messages);
                    }
                }

                if (valid && submitRequest)
                {
                    // Successfully submitted the request, update request and save. This is done to support
                    // potential repeated identical landing page requests
                    sjpStopEventRequest.JourneyRequestSubmitted = true;
                    sessionHelper.UpdateSessionStopEvent(sjpStopEventRequest);
                }

                #endregion
            }
            catch (SJPException sjpEx)
            {
                if (!sjpEx.Logged)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, sjpEx.Message));
                }

                // Add message to be displayed
                sessionHelper.AddMessage(new SJPMessage("ValidateAndRun.ErrorMessage.General1", SJPMessageType.Error));
                sessionHelper.AddMessage(new SJPMessage("ValidateAndRun.ErrorMessage.General2", SJPMessageType.Error));

                valid = false;
            }

            return valid;
        }
        #endregion
    }
}
