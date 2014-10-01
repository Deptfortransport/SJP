// *********************************************** 
// NAME             : JourneyPlannerInputAdapter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 04 Apr 2011
// DESCRIPTION  	: JourneyPlanner Input page adapter responsible for populating inptut controls, journey request in session,
//                    validating the controls
// ************************************************


using System;
using System.Collections.Generic;
using SJP.Common.LocationService;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPWeb.Controls;

namespace SJP.UserPortal.SJPWeb.Adapters
{
    /// <summary>
    /// JourneyPlanner Input page adapter responsible for populating inptut controls, journey request in session,
    /// validating the controls and initiating a journey request
    /// </summary>
    public class JourneyPlannerInputAdapter
    {
        #region Private Fields
        
        private LocationControl from;
        private LocationControl to;
        private EventDateControl eventControl;
        private JourneyOptionTabContainer journeyOptions;
        
        private SessionHelper sessionHelper;
        private CookieHelper cookieHelper;

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public JourneyPlannerInputAdapter()
        {
            sessionHelper = new SessionHelper();
            cookieHelper = new CookieHelper();
        }

        /// <summary>
        /// Constructor - Initialises the adapter with page controls necessary for journey
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="eventControl"></param>
        /// <param name="journeyOptions"></param>
        public JourneyPlannerInputAdapter(LocationControl from,
            LocationControl to, EventDateControl eventControl,
            JourneyOptionTabContainer journeyOptions) : this()
        {
            this.from = from;
            this.to = to;
            this.eventControl = eventControl;
            this.journeyOptions = journeyOptions;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Populates journey input page controls from session
        /// </summary>
        public void PopulateInputControls(bool isLandingPage)
        {
            ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

            // Set default location types (To-venue);
            from.TypeOfLocation = SJPLocationType.Unknown;
            to.TypeOfLocation = SJPLocationType.Venue;

            if (sjpJourneyRequest != null)
            {
                // If landing page, then do not want to resolve location as it would have already
                // been done (or if null/invalid then the control will say when Validate is called)
                from.ResolveLocation = !isLandingPage;
                from.Location = sjpJourneyRequest.Origin;
                to.ResolveLocation = !isLandingPage;
                to.Location = sjpJourneyRequest.Destination;

                #region Ensure location inputs are in a valid input scenario

                // If landing page, then ensure location controls type of location is valid
                if (isLandingPage)
                {
                    // Origin wasn't supplied and Destination was but is not a venue, then
                    // From must be a venue input
                    if (sjpJourneyRequest.Origin == null
                        && sjpJourneyRequest.Destination != null
                        && sjpJourneyRequest.Destination.TypeOfLocation != SJPLocationType.Venue)
                    {
                        from.TypeOfLocation = SJPLocationType.Venue;
                    }
                    // Destination wasn't supplied and Origin was but is a venue, then
                    // To is venue input (if outward required) or is text input (if return only)
                    else if (sjpJourneyRequest.Destination == null
                        && sjpJourneyRequest.Origin != null
                        && sjpJourneyRequest.Origin.TypeOfLocation == SJPLocationType.Venue)
                    {
                        if (sjpJourneyRequest.IsOutwardRequired)
                        {
                            to.TypeOfLocation = SJPLocationType.Venue;
                        }
                        else if (sjpJourneyRequest.IsReturnRequired)
                        {
                            to.TypeOfLocation = SJPLocationType.Unknown;
                        }
                    }
                    // Origin and Destination was not supplied, and return only required
                    else if (sjpJourneyRequest.Origin == null
                        && sjpJourneyRequest.Destination == null
                        && !sjpJourneyRequest.IsOutwardRequired)
                    {
                        from.TypeOfLocation = SJPLocationType.Venue;
                        to.TypeOfLocation = SJPLocationType.Unknown;
                    }
                }

                #endregion

                // If landing page, then want to ensure the dates are validated and updated if necessary
                eventControl.ForceUpdate = isLandingPage;
                eventControl.OurwardDateTime = sjpJourneyRequest.OutwardDateTime;
                eventControl.ReturnDateTime = sjpJourneyRequest.ReturnDateTime;

                eventControl.IsOutwardRequired = sjpJourneyRequest.IsOutwardRequired;
                eventControl.IsReturnRequired = sjpJourneyRequest.IsReturnRequired;

                // If outward journey not required, then hide the outward date control
                if (!sjpJourneyRequest.IsOutwardRequired)
                {
                    eventControl.ShowReturnDateOnly(true);
                }

                journeyOptions.PlannerMode = sjpJourneyRequest.PlannerMode;

                // Populate options within the tabs
                if (sjpJourneyRequest.AccessiblePreferences != null)
                {
                    SJPAccessiblePreferences accessPref = sjpJourneyRequest.AccessiblePreferences;

                    journeyOptions.PublicJourneyTab.ExcludeUnderGround = accessPref.DoNotUseUnderground;
                    journeyOptions.PublicJourneyTab.FewerInterchanges = accessPref.RequireFewerInterchanges;

                    if (sjpJourneyRequest.AccessiblePreferences.RequireSpecialAssistance
                        && sjpJourneyRequest.AccessiblePreferences.RequireStepFreeAccess)
                    {
                        journeyOptions.PublicJourneyTab.StepFreeAndAssistance = true;
                    }
                    else if (sjpJourneyRequest.AccessiblePreferences.RequireSpecialAssistance)
                    {
                        journeyOptions.PublicJourneyTab.Assistance = accessPref.RequireSpecialAssistance;
                    }
                    else if (sjpJourneyRequest.AccessiblePreferences.RequireStepFreeAccess)
                    {
                        journeyOptions.PublicJourneyTab.StepFree = accessPref.RequireStepFreeAccess;
                    }
                }
            }
        }

        #region Validate and Build SJPJourneyRequest

        /// <summary>
        /// Validates the input page controls and populates a SJPJourneyRequest object using input page controls,
        /// and adds to the session
        /// </summary>
        /// <returns>True if the input page controls are in valid state</returns>
        public bool ValidateAndBuildSJPRequest(SJPJourneyPlannerMode plannerMode)
        {
            bool valid = false;

            if (from != null && to != null && eventControl != null)
            {
                bool validFrom = from.Validate();
                bool validTo = to.Validate();
                bool validDate = eventControl.Validate();

                valid = validFrom && validTo && validDate;

                // Locations and dates are valid, construct an SJPJourneyRequest
                if (valid)
                {
                    JourneyRequestHelper jrh = new JourneyRequestHelper();

                    // Check if this should be treated as a return only journey request,
                    // by the abscene of the outward required in the event date control
                    bool isReturnOnly = (!eventControl.IsOutwardRequired && eventControl.IsReturnRequired);

                    ISJPJourneyRequest sjpJourneyRequest = jrh.BuildSJPJourneyRequest(
                        from.Location,
                        to.Location,
                        eventControl.OurwardDateTime,
                        eventControl.ReturnDateTime,
                        eventControl.IsOutwardRequired,
                        eventControl.IsReturnRequired,
                        isReturnOnly,
                        plannerMode,
                        GetAccessiblePreferences());

                    // Commit journey request to session
                    sessionHelper.UpdateSession(sjpJourneyRequest);

                    // Persist the sjp journey request details in the cookie.
                    // This aids in recovery following session timeout, or when user 
                    // returns to site after a period of time (new session)
                    cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);
                }
            }

            return valid;
        }

        /// <summary>
        /// Populates a new SJPJourneyRequest object using the supplied SJPJourneyRequest, 
        /// with the supplied parameters, and adds to the session
        /// </summary>
        /// <returns></returns>
        public bool ValidateAndBuildSJPRequestForReplan(ISJPJourneyRequest sjpJourneyRequest,
            bool replanOutwardRequired, bool replanReturnRequired,
            DateTime replanOutwardDateTime, DateTime replanReturnDateTime,
            bool replanOutwardArriveBefore, bool replanReturnArriveBefore,
            List<Journey> outwardJourneys, List<Journey> returnJourneys,
            bool retainOutwardJourneys, bool retainReturnJourneys,
            bool retainOutwardJourneysWhenNoResults, bool retainReturnJourneysWhenNoResults)
        {
            bool valid = false;

            if (sjpJourneyRequest != null)
            {
                JourneyRequestHelper jrh = new JourneyRequestHelper();

                // Create a new request. 
                // Must create a new request as it's hash will be different, and should allow the 
                // "browser back" to still function, and support multi-tabbing (assumption!)
                ISJPJourneyRequest sjpJourneyRequestReplan = jrh.BuildSJPJourneyRequestForReplan(
                    sjpJourneyRequest,
                    replanOutwardRequired, replanReturnRequired,
                    replanOutwardDateTime, replanReturnDateTime,
                    replanOutwardArriveBefore, replanReturnArriveBefore,
                    outwardJourneys, returnJourneys,
                    retainOutwardJourneys, retainReturnJourneys,
                    retainOutwardJourneysWhenNoResults, retainReturnJourneysWhenNoResults
                    );
                                
                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequestReplan);

                // Don't persist the sjp journey request details in the cookie, as this is a replan
                // and the original request should still be used (when user leaves site, or goes back
                // to input page)

                valid = true;
            }

            return valid;
        }

        #endregion

        #region Validate and Update SJP Request

        /// <summary>
        /// Validates the venue for car/cycle parks and updates the sjp request stored in session
        /// </summary>
        /// <param name="sjpPark">SJP car or cycle park</param>
        public bool ValidateAndUpdateSJPRequestForSJPPark(SJPPark sjpPark, 
            DateTime outwardDateTime, DateTime returnDateTime, string cycleRouteType)
        {
            if (sjpPark != null)
            {
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

                // Update the journey request location with the Park and datetimes 
                JourneyRequestHelper journeyRequestHelper = new JourneyRequestHelper();
                sjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestVenue(sjpJourneyRequest,
                    sjpPark, outwardDateTime, returnDateTime);

                if (!string.IsNullOrEmpty(cycleRouteType))
                {
                    // Update the selected cycle route penalty function algorithm
                    sjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestCycle(sjpJourneyRequest, cycleRouteType);
                }

                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequest);

                // Persist the sjp journey request details in the cookie.
                // This aids in recovery following session timeout, or when user 
                // returns to site after a period of time (new session)
                cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates the venue for river serivces and updates the sjp request stored in session
        /// </summary>
        /// <param name="outwardStopEvent">Selected outward river serivce </param>
        /// <param name="outwardDateTime">Outward river service date and time</param>
        /// <param name="returnStopEvent">Selected return river serivce </param>
        /// <param name="returnDateTime">Return river service date and time</param>
        /// <returns></returns>
        public bool ValidateAndUpdateSJPRequestForSJPRiverServices(Journey outwardStopEventJourney, DateTime outwardDateTime, 
            Journey returnStopEventJourney, DateTime returnDateTime)
        {
            if ((outwardStopEventJourney != null) || (returnStopEventJourney != null))
            {
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();
                                
                // Update the journey request location with the river service details
                JourneyRequestHelper journeyRequestHelper = new JourneyRequestHelper();
                sjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestRiverServices(sjpJourneyRequest,
                    outwardStopEventJourney, returnStopEventJourney, outwardDateTime, returnDateTime);
                
                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequest);

                // Persist the sjp journey request details in the cookie.
                // This aids in recovery following session timeout, or when user 
                // returns to site after a period of time (new session)
                cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Validates the GNAT location and updates the sjp request stored in session
        /// </summary>
        /// <param name="selectedLocation"></param>
        /// <returns></returns>
        public bool ValidateAndUpdateSJPRequestForAccessiblePublicTransport(SJPGNATLocation gnatStop)
        {
            if (gnatStop != null)
            {
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();


                // Update the Journey request location with the accessible location
                JourneyRequestHelper journeyRequestHelper = new JourneyRequestHelper();
                sjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestForAccessiblePublicTransport(
                    sjpJourneyRequest, gnatStop);
                
                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequest);

                // Persist the sjp journey request details in the cookie.
                // This aids in recovery following session timeout, or when user 
                // returns to site after a period of time (new session)
                cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);

                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Checks the active journey options tab and builds accessible preferences
        /// </summary>
        /// <returns></returns>
        private SJPAccessiblePreferences GetAccessiblePreferences()
        {
            SJPAccessiblePreferences accessiblePreferences = new SJPAccessiblePreferences();

            if (journeyOptions.ActiveTab is PublicJourneyOptionsTab)
            {
                accessiblePreferences.DoNotUseUnderground = journeyOptions.PublicJourneyTab.ExcludeUnderGround;
                accessiblePreferences.RequireFewerInterchanges = journeyOptions.PublicJourneyTab.FewerInterchanges;
                accessiblePreferences.RequireSpecialAssistance = journeyOptions.PublicJourneyTab.Assistance;
                accessiblePreferences.RequireStepFreeAccess = journeyOptions.PublicJourneyTab.StepFree;

                if (journeyOptions.PublicJourneyTab.StepFreeAndAssistance)
                {
                    accessiblePreferences.RequireSpecialAssistance = true;
                    accessiblePreferences.RequireStepFreeAccess = true;
                }
            }
            else if (journeyOptions.ActiveTab is BlueBadgeOptionsTab)
            {
                accessiblePreferences.BlueBadge = true;
            }

            return accessiblePreferences;
        }
        
        #endregion
    }
}
