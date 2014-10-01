// *********************************************** 
// NAME             : JourneyInputAdapter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Feb 2012
// DESCRIPTION  	: Journey Input page adapter responsible for populating inptut controls, journey request in session,
//                    validating the controls
// ************************************************
// 

using System;
using System.Collections.Generic;
using SJP.Common.LocationService;
using SJP.Common.Extenders;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPMobile.Controls;
using SJP.Common.PropertyManager;
using SJP.Common;

namespace SJP.UserPortal.SJPMobile.Adapters
{
    /// <summary>
    /// Journey Input page adapter responsible for populating inptut controls, journey request in session,
    /// validating the controls
    /// </summary>
    public class JourneyInputAdapter
    {
        #region Private Fields

        private JourneyInputControl journeyInputControl;
        private AccessibleStopsControl accessibleStopsControl;

        private SessionHelper sessionHelper;
        private CookieHelper cookieHelper;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public JourneyInputAdapter()
        {
            sessionHelper = new SessionHelper();
            cookieHelper = new CookieHelper();
        }

        /// <summary>
        /// Constructor - Initialises the adapter with page controls necessary for journey
        /// </summary>
        public JourneyInputAdapter(JourneyInputControl journeyInputControl)
            : this()
        {
            this.journeyInputControl = journeyInputControl;
        }

        /// <summary>
        /// Constructor - Initialises the adapter with page controls necessary for accessible stop
        /// </summary>
        public JourneyInputAdapter(AccessibleStopsControl accessibleStopsControl)
            : this()
        {
            this.accessibleStopsControl = accessibleStopsControl;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Populates journey input page controls from session
        /// </summary>
        public void PopulateInputControls(bool isLandingPage)
        {
            ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

            if (journeyInputControl != null)
            {
                journeyInputControl.Reset();
            }

            if (sjpJourneyRequest != null)
            {
                if (journeyInputControl != null)
                {
                    // If landing page, then do not want to resolve location as it would have already
                    // been done (or if null/invalid then the control will say when Validate is called)
                    journeyInputControl.ResolveLocationFrom = !isLandingPage;
                    journeyInputControl.LocationFrom = sjpJourneyRequest.Origin;
                    journeyInputControl.ResolveLocationTo = !isLandingPage;
                    journeyInputControl.LocationTo = sjpJourneyRequest.Destination;

                    #region Ensure location inputs are in a valid input scenario

                    // If landing page, then ensure location controls type of location is valid
                    if (isLandingPage)
                    {
                        // logic moved to JourneyInputControl.LocationInputMode
                    }

                    #endregion

                    // ensure that at least one of the input controls is always venue only
                    switch (journeyInputControl.LocationInputMode)
                    {
                        case LocationInputMode.VenueToVenue:
                            // Retain existing location as venue only, giving preference to the desitnation input
                            if (journeyInputControl.LocationToVenueOnly)
                            {
                                journeyInputControl.LocationFromVenueOnly = false;
                                journeyInputControl.LocationToVenueOnly = true;
                            }
                            else if (journeyInputControl.LocationFromVenueOnly)
                            {
                                journeyInputControl.LocationFromVenueOnly = true;
                                journeyInputControl.LocationToVenueOnly = false;
                            }
                            break;
                        case LocationInputMode.FromVenue:
                            journeyInputControl.LocationFromVenueOnly = true;
                            journeyInputControl.LocationToVenueOnly = false;
                            break;
                        case LocationInputMode.ToVenue:
                        default:
                            journeyInputControl.LocationFromVenueOnly = false;
                            journeyInputControl.LocationToVenueOnly = true;
                            break;
                    }

                    // If landing page, then want to ensure the dates are validated and updated if necessary
                    journeyInputControl.OutwardDateTimeForceUpdate = true; // isLandingPage;
                    journeyInputControl.OutwardDateTime = sjpJourneyRequest.OutwardDateTime;
                    journeyInputControl.OutwardDateTimeArriveBy = sjpJourneyRequest.OutwardArriveBefore;

                    journeyInputControl.PlannerMode = sjpJourneyRequest.PlannerMode;

                    // Set the selected value to be the specified value (allows previous choice to be retained when returning
                    // to the locations page)
                    journeyInputControl.SelectedCycleJourneyType = sjpJourneyRequest.CycleAlgorithm;


                    // Populate options
                    if (sjpJourneyRequest.AccessiblePreferences != null)
                    {
                        if (journeyInputControl.AccessibleOptions != null)
                        {
                            SJPAccessiblePreferences accessPref = sjpJourneyRequest.AccessiblePreferences;

                            journeyInputControl.AccessibleOptions.ExcludeUnderGround = accessPref.DoNotUseUnderground;

                            if (sjpJourneyRequest.AccessiblePreferences.RequireSpecialAssistance
                                && sjpJourneyRequest.AccessiblePreferences.RequireStepFreeAccess)
                            {
                                journeyInputControl.AccessibleOptions.StepFreeAndAssistance = true;
                            }
                            else if (sjpJourneyRequest.AccessiblePreferences.RequireSpecialAssistance)
                            {
                                journeyInputControl.AccessibleOptions.Assistance = accessPref.RequireSpecialAssistance;
                            }
                            else if (sjpJourneyRequest.AccessiblePreferences.RequireStepFreeAccess)
                            {
                                journeyInputControl.AccessibleOptions.StepFree = accessPref.RequireStepFreeAccess;
                            }
                        }
                    }
                } // journeyInputControl != null
            }
        }

        /// <summary>
        /// Updates journey input controls from session for a page postback
        /// (only updates the planner mode)
        /// </summary>
        public void UpdateInputControls()
        {
            ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

            if (sjpJourneyRequest != null)
            {
                if (journeyInputControl != null)
                {
                    journeyInputControl.PlannerMode = sjpJourneyRequest.PlannerMode;
                }
            }
        }

        #region Validate and Build SJPJourneyRequest

        /// <summary>
        /// Validates the input page controls and populates a SJPJourneyRequest object using input page controls,
        /// and adds to the session
        /// </summary>
        /// <returns>True if the input page controls are in valid state</returns>
        public bool ValidateAndBuildSJPRequest(SJPJourneyPlannerMode plannerMode, bool ignoreInvalid)
        {
            bool valid = false;

            if (journeyInputControl != null)
            {
                // Validate inputs
                valid = journeyInputControl.Validate();

                // Locations and dates are valid, construct an SJPJourneyRequest,
                // If ignoreInvalid set, then caller requires a request in session built with the valid items,
                // e.g. if moving temporarily away from the input page to map and requires the selected items to be captured
                if (valid || ignoreInvalid)
                {
                    JourneyRequestHelper jrh = new JourneyRequestHelper();

                    ISJPJourneyRequest sjpJourneyRequest = jrh.BuildSJPJourneyRequest(
                        journeyInputControl.IsValidLocationFrom ? journeyInputControl.LocationFrom : null,
                        journeyInputControl.IsValidLocationTo ? journeyInputControl.LocationTo : null,
                        journeyInputControl.IsValidDate ? journeyInputControl.OutwardDateTime : DateTime.MinValue,
                        DateTime.MinValue, // SJPMobile does not do return journeys
                        journeyInputControl.OutwardDateTimeArriveBy,
                        false,
                        true,
                        false, // SJPMobile does not do return journeys
                        false,
                        plannerMode,
                        GetAccessiblePreferences(plannerMode));

                    // Commit journey request to session
                    sessionHelper.UpdateSession(sjpJourneyRequest);

                    // Persist the sjp journey request details in the cookie.
                    // This aids in recovery following session timeout, or when user 
                    // returns to site after a period of time (new session)
                    cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);
                }
            }
            else if (accessibleStopsControl != null)
            {
                // Use the existing journey request but update origin / destination with GNAT stop
                JourneyRequestHelper jrh = new JourneyRequestHelper();

                // Get the request from the session
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

                // Update origin / destination
                ValidateAndUpdateSJPRequestForAccessiblePublicTransport(accessibleStopsControl.TheLocation);

                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequest);

                // Persist the sjp journey request details in the cookie.
                // This aids in recovery following session timeout, or when user 
                // returns to site after a period of time (new session)
                cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);

                // Should alway be valid
                valid = true;
            }

            return valid;
        }

        /// <summary>
        /// Populates a new SJPJourneyRequest object using the supplied SJPJourneyRequest, 
        /// with the supplied parameters, and adds to the session
        /// </summary>
        /// <returns></returns>
        public bool ValidateAndBuildSJPRequestForReplan(ISJPJourneyRequest sjpJourneyRequest,
            DateTime replanOutwardDateTime,
            bool replanOutwardArriveBefore,
            List<Journey> outwardJourneys,
            bool retainOutwardJourneys,
            bool retainOutwardJourneysWhenNoResults
            )
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
                    true, false, // SJPMobile only supports outward journeys so cannot replan the return 
                    replanOutwardDateTime, DateTime.MinValue,
                    replanOutwardArriveBefore, false,
                    outwardJourneys, null,
                    retainOutwardJourneys, false,
                    retainOutwardJourneysWhenNoResults, false
                    );

                // Update the Journey request location with the earlier or later journey request hash
                sjpJourneyRequestReplan = jrh.UpdateSJPJourneyRequestEarlierLater(
                    sjpJourneyRequestReplan, !replanOutwardArriveBefore, sjpJourneyRequest.JourneyRequestHash);

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
        /// Updates the journey request (if exists) and cookie with the planner mode and accessible options
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public void ValidateAndUpdateSJPRequestForPlannerMode(SJPJourneyPlannerMode plannerMode)
        {
            JourneyRequestHelper journeyRequestHelper = new JourneyRequestHelper();
            
            // Get journey request from session (if it exists)
            ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

            if (sjpJourneyRequest != null)
            {
                journeyRequestHelper.UpdateSJPJourneyRequestPlannerMode(sjpJourneyRequest, plannerMode);

                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequest);

                // Persist the sjp journey request details in the cookie.
                // This aids in recovery following session timeout, or when user 
                // returns to site after a period of time (new session)
                cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);
            }
            else
            {
                // Build an empty request with planner mode  and accessible options specified
                sjpJourneyRequest = journeyRequestHelper.BuildSJPJourneyRequest(
                        null,
                        null,
                        DateTime.MinValue,
                        DateTime.MinValue, // SJPMobile does not do return journeys
                        true,
                        false, // SJPMobile does not do return journeys
                        false,
                        plannerMode,
                        GetAccessiblePreferences(plannerMode));

                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequest);

                // Persist the sjp journey request details in the cookie.
                // This aids in recovery following session timeout, or when user 
                // returns to site after a period of time (new session)
                cookieHelper.UpdateJourneyRequestToCookie(sjpJourneyRequest);
            }
        }

        /// <summary>
        /// Validates the venue for car/cycle parks and updates the sjp request stored in session
        /// </summary>
        /// <param name="sjpPark">SJP car or cycle park</param>
        public bool ValidateAndUpdateSJPRequestForSJPPark(SJPPark sjpPark,
            DateTime outwardDateTime, string cycleRouteType, SJPJourneyPlannerMode plannerMode)
        {
            if (sjpPark != null)
            {
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

                // Update the journey request location with the Park and datetimes 
                JourneyRequestHelper journeyRequestHelper = new JourneyRequestHelper();
                sjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestVenue(
                    sjpJourneyRequest, 
                    sjpPark, 
                    outwardDateTime,
                    DateTime.MinValue);

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

        /// <summary>
        /// Validates the location and updates the sjp request's destination or origin (stored in session)
        /// </summary>
        /// <param name="location">New location</param>
        /// <param name="updateOrigin">true for origin, false for destination</param>
        /// <returns></returns>
        public bool ValidateAndUpdateSJPRequestOriginOrDestination(SJPLocation location, bool updateOrigin)
        {
            if (location != null)
            {
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

                // Update the Journey request location with the accessible location
                JourneyRequestHelper journeyRequestHelper = new JourneyRequestHelper();
                sjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestOriginOrDestination(
                    sjpJourneyRequest, location, updateOrigin);

                // Commit journey request to session
                sessionHelper.UpdateSession(sjpJourneyRequest);

                // Do not persist in cookie as this method is only used by the maps page and therefore
                // a journey request isn't being submitted 
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates the replan journey request hash exists and updates the two journey requests so they are linked 
        /// as an earlier - later chain
        /// </summary>
        /// <param name="journeyRequestHash"></param>
        /// <param name="isEarlier"></param>
        /// <param name="replanJourneyRequestHash"></param>
        /// <returns></returns>
        public bool ValidateAndUpdateSJPRequestEarlierLater(string journeyRequestHash, bool isEarlier, string replanJourneyRequestHash)
        {
            bool valid = false;

            if (!string.IsNullOrEmpty(journeyRequestHash) && !string.IsNullOrEmpty(replanJourneyRequestHash))
            {
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest(journeyRequestHash);
                ISJPJourneyRequest replanSjpJourneyRequest = sessionHelper.GetSJPJourneyRequest(replanJourneyRequestHash);

                // Ensure both requests exist, otherwise don't update
                if (sjpJourneyRequest != null && replanSjpJourneyRequest != null)
                {
                    // Update the Journey request location with the earlier or later journey request hash
                    JourneyRequestHelper journeyRequestHelper = new JourneyRequestHelper();
                    
                    sjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestEarlierLater(
                        sjpJourneyRequest, isEarlier, replanJourneyRequestHash);

                    replanSjpJourneyRequest = journeyRequestHelper.UpdateSJPJourneyRequestEarlierLater(
                        replanSjpJourneyRequest, !isEarlier, journeyRequestHash);

                    // Commit journey request to session
                    sessionHelper.UpdateSession(sjpJourneyRequest);

                    valid = true;
                }
            }

            return valid;
        }

        #endregion

        #region Submit

        /// <summary>
        /// Submits the request. If successful, then sets the page to transfer to, otherwise the current
        /// page processing continues
        /// </summary>
        /// <param name="plannerMode"></param>
        public bool SubmitRequest(SJPJourneyPlannerMode plannerMode, ref List<SJPMessage> messages, SJPPage page)
        {
            bool submitValid = false;

            // Check locations/dates are valid and setup the journey request.
            // Any errors and the curret page will be displayed again, with errors shown
            if (ValidateAndBuildSJPRequest(plannerMode, false))
            {
                LocationHelper locationHelper = new LocationHelper();

                // Assume locations are accessible
                bool validAccessibleLocations = true;

                #region Validate accessible locations (if required)

                bool checkAccessibleLocations = Properties.Current["JourneyPlannerInput.CheckForGNATStation.Switch"].Parse(true);

                // Only check for accessible locations if the planner mode is public transport, and switch turned on
                if (plannerMode == SJPJourneyPlannerMode.PublicTransport && checkAccessibleLocations)
                {
                    // If origin/destination are venues and not accessible, then error
                    if (locationHelper.CheckAccessibleLocationForVenue(true) && locationHelper.CheckAccessibleLocationForVenue(false))
                    {
                        // Check accessible location for Origin
                        validAccessibleLocations = locationHelper.CheckAccessibleLocation(true);

                        // Check accessible location for Destination
                        validAccessibleLocations = validAccessibleLocations && locationHelper.CheckAccessibleLocation(false);
                    }
                }

                #endregion

                #region Check for The Mall venue

                // Check for The Mall venue
                string mallNaptan = Properties.Current["JourneyPlannerInput.TheMallNaptan"];
                if (journeyInputControl != null)
                {
                    if (journeyInputControl.LocationFrom is SJPVenueLocation)
                    {
                        if (journeyInputControl.LocationFrom.Naptan.Contains(mallNaptan))
                        {
                            messages.Add(new SJPMessage("JourneyPlannerInput.MallMessage.Text", SJPMessageType.Error));
                            return false;
                        }
                    }

                    if (journeyInputControl.LocationTo is SJPVenueLocation)
                    {
                        if (journeyInputControl.LocationTo.Naptan.Contains(mallNaptan))
                        {
                            messages.Add(new SJPMessage("JourneyPlannerInput.MallMessage.Text", SJPMessageType.Error));
                            return false;
                        }
                    }
                }

                #endregion

                #region Submit the request

                JourneyPlannerHelper journeyPlannerHelper = new JourneyPlannerHelper();

                switch (plannerMode)
                {
                    case SJPJourneyPlannerMode.PublicTransport:
                        if (validAccessibleLocations)
                        {
                            // Attempt to submit the request and plan the journey
                            if (journeyPlannerHelper.SubmitRequest(plannerMode, true))
                            {
                                submitValid = true;
                            }
                        }
                        else
                        {
                            // Validate page details before moving on
                            if (journeyPlannerHelper.SubmitRequest(plannerMode, false))
                            {
                                // Transfer to further accessibility page
                                page.SetPageTransfer(PageId.AccessibilityOptions);

                                // Add the query string values to allow JourneyLocations page 
                                // to load the details for the correct request
                                page.AddQueryStringForPage(PageId.AccessibilityOptions);
                            }
                        }
                        break;

                    case SJPJourneyPlannerMode.Cycle:
                        if (journeyInputControl == null)
                        {
                            submitValid = false;
                        }
                        else
                        {
                            bool valid = ValidateAndUpdateSJPRequestForSJPPark(
                                journeyInputControl.SelectedCyclePark,
                                journeyInputControl.SelectedCycleParkDateTime,
                                journeyInputControl.SelectedCycleJourneyType,
                                journeyInputControl.PlannerMode);

                            if (valid && journeyPlannerHelper.SubmitRequest(plannerMode, true))
                            {
                                submitValid = true;
                            }
                        }
                        break;
                }

                #endregion
            }
            else
            {
                // Add any control specific messages
                if (journeyInputControl != null)
                {
                    List<string> validationMessages = journeyInputControl.ValidationMessages;

                    foreach (string message in validationMessages)
                    {
                        messages.Add(new SJPMessage(message, string.Empty, 0, 0, SJPMessageType.Error));
                    }
                }

                // Add default message if needed
                if (messages.Count == 0)
                {
                    messages.Add(new SJPMessage("JourneyPlannerInput.ValidationError.Text", SJPMessageType.Error));
                }
            }

            if (journeyInputControl != null)
            {
                // Persist planner mode if the Submit validation fails and this page is displayed again
                journeyInputControl.PlannerMode = plannerMode;
            }

            return submitValid;
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Checks the active journey options tab and builds accessible preferences
        /// </summary>
        /// <returns></returns>
        private SJPAccessiblePreferences GetAccessiblePreferences(SJPJourneyPlannerMode plannerMode)
        {
            SJPAccessiblePreferences accessiblePreferences = new SJPAccessiblePreferences();

            if (plannerMode == SJPJourneyPlannerMode.PublicTransport)
            {
                if (journeyInputControl != null && journeyInputControl.AccessibleOptions != null)
                {
                    accessiblePreferences.DoNotUseUnderground = journeyInputControl.AccessibleOptions.ExcludeUnderGround;
                    accessiblePreferences.RequireFewerInterchanges = false;
                    accessiblePreferences.RequireSpecialAssistance = journeyInputControl.AccessibleOptions.Assistance;
                    accessiblePreferences.RequireStepFreeAccess = journeyInputControl.AccessibleOptions.StepFree;

                    if (journeyInputControl.AccessibleOptions.StepFreeAndAssistance)
                    {
                        accessiblePreferences.RequireSpecialAssistance = true;
                        accessiblePreferences.RequireStepFreeAccess = true;
                    }
                }
            }

            return accessiblePreferences;
        }

        #endregion
    }
}
