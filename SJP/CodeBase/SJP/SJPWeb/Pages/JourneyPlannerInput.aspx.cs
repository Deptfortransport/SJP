// *********************************************** 
// NAME             : JourneyPlannerInput.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Journey planner input page. Contains controls for capturing locations, dates,
// and options for journey planning.
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.Reporting.Events;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.SJPWeb.Adapters;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// Journey Planner Input page
    /// </summary>
    public partial class JourneyPlannerInput : SJPPage
    {
        #region Private Fields

        private const string DATEFORMAT = "ddMMyyyyhhmm";
        JourneyPlannerInputAdapter adapter;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyPlannerInput()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.JourneyPlannerInput;
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupResources();
            SetupAssociatedControlsLabel();
            SetupJourneySelectControl();
            
            adapter = new JourneyPlannerInputAdapter(locationFrom, locationTo, eventControl, journeyOptions);

            if (!IsPostBack)
            {
                // If is a landing page request, then ensure controls are told to re-validate/update. 
                // This is needed as we don't want the previously "selected" details changing if the user is returning from
                // the results page.
                adapter.PopulateInputControls(IsLandingPageRequest());
                                
                // Set the venue for the journey options tab to use
                if (locationTo.TypeOfLocation == SJPLocationType.Venue) // This will be a journey to a venue location
                {
                    journeyOptions.Venue = locationTo.Location;
                    journeyOptions.IsOriginVenue = false;
                }
                else if (locationFrom.TypeOfLocation == SJPLocationType.Venue) // This will be a journey from a venue location
                {
                    journeyOptions.Venue = locationFrom.Location;
                    journeyOptions.IsOriginVenue = true;
                }
            }
            else
            {
                
                if (((SJPWeb)Master).PageScriptManager.IsInAsyncPostBack)
                {
                    logPageEntry = false;
                }

                UpdateJourneyOptionsControl();
            }

            // If landing page auto plan is set, then submit the journey request only if there were no messages to display
            if (IsLandingPageAutoPlanRequest() && !IsSessionMessages())
            {
                SubmitRequest(journeyOptions.PlannerMode);
            }

            // Reset all landing page flags as they've been used
            ClearLandingPageFlags();

            // Add jquery ui css
            AddStyleSheet("jquery-ui-1.8.13.css");
            AddStyleSheet("jquery.qtip.min.css");

            // Add javascripts specific for this page
            AddJavascript("jquery-ui-1.8.13.min.js");
            AddJavascript("JourneyPlannerInput.js");
            AddJavascript("jquery.ui.selectmenu.js");
            AddJavascript("jquery.dateentry.min.js");
            AddJavascript("jquery.qtip.min.js");

            SetupDebugInformation();
        }
                
        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (((SJPWeb)Master).PageScriptManager.IsInAsyncPostBack)
            {
                // Log an event as a result of partial page update
                PageEntryEvent logPage = new PageEntryEvent(Common.PageId.JourneyInputPartialUpdate, SJPSessionManager.Current.Session.SessionID, false);
                Logger.Write(logPage);
            }

            SetupLocationsControl();

            SetupMessages();
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the bubble event generated from the journey option tabs when submit buttons gets clicked on tab
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if(source is IJourneyOptionsTab)
            {
                IJourneyOptionsTab plannerTab = source as IJourneyOptionsTab;

                SubmitRequest(plannerTab.PlannerMode);
            }
            return true;
        }

        /// <summary>
        /// Event handler for TravelBetweenVenues_Click
        /// </summary>
        protected void TravelBetweenVenues_Click(object sender, EventArgs e)
        {
            LocationInputMode currentInputMode = GetLocationInputMode();
            LocationInputMode previousInputMode = currentInputMode;

            if(!string.IsNullOrEmpty(previousLocationInputMode.Value))
            {
                previousInputMode = previousLocationInputMode.Value.Parse(currentInputMode);
            }

            switch (currentInputMode)
            {
                case LocationInputMode.VenueToVenue:
                    // In case the FromToToggle button was selected when in VenueToVenue mode, 
                    // then default to ToVenue, otherwise revert back to FromVenue or ToVenue
                    if (currentInputMode != previousInputMode)
                    {
                        ToggleLocationControl(previousInputMode);
                        previousLocationInputMode.Value = LocationInputMode.VenueToVenue.ToString();
                    }
                    else
                    {
                        ToggleLocationControl(LocationInputMode.ToVenue);
                        previousLocationInputMode.Value = LocationInputMode.VenueToVenue.ToString();
                    }
                    break;
                case LocationInputMode.FromVenue:
                case LocationInputMode.ToVenue:
                default:
                    ToggleLocationControl(LocationInputMode.VenueToVenue);
                    previousLocationInputMode.Value = currentInputMode.ToString();
                    break;
            }
        }

        /// <summary>
        /// Event handler for TravelFromVenue_Click
        /// </summary>
        protected void TravelFromToToggle_Click(object sender, EventArgs e)
        {
            LocationInputMode currentInputMode = GetLocationInputMode();

            switch (currentInputMode)
            {
                case LocationInputMode.VenueToVenue:
                    ToggleLocationControl(LocationInputMode.VenueToVenue);
                    previousLocationInputMode.Value = currentInputMode.ToString();
                    break;
                case LocationInputMode.FromVenue:
                    ToggleLocationControl(LocationInputMode.ToVenue);
                    previousLocationInputMode.Value = currentInputMode.ToString();
                    break;
                case LocationInputMode.ToVenue:
                    ToggleLocationControl(LocationInputMode.FromVenue);
                    previousLocationInputMode.Value = currentInputMode.ToString();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler for rptrMessages_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptrMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                SJPMessage sjpMessage = (SJPMessage)e.Item.DataItem;

                if (sjpMessage != null)
                {
                    Label lblMessage = (Label)e.Item.FindControl("lblMessage");

                    if (lblMessage != null)
                    {
                        lblMessage.Text = sjpMessage.MessageText;
                        lblMessage.CssClass = sjpMessage.Type.ToString().ToLower(); // "warning" or "error" or "info"
                    }
                }
            }
        }
         
        #endregion

        #region Private methods

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            fromLocationLabel.Text = GetResource("JourneyPlannerInput.fromLocationLabel.Text");
            toLocationLabel.Text = GetResource("JourneyPlannerInput.toLocationLabel.Text");
                       
            locationFrom_Information.ImageUrl = ImagePath + GetResource("JourneyPlannerInput.LocationFromInformation.ImageUrl");
            locationFrom_Information.AlternateText = GetResource("JourneyPlannerInput.LocationFromInformation.AlternateText");
            
            locationTo_Information.ImageUrl = ImagePath + GetResource("JourneyPlannerInput.LocationToInformation.ImageUrl");
            locationTo_Information.AlternateText = GetResource("JourneyPlannerInput.LocationToInformation.AlternateText");

            travelFromToVenueToggle.ImageUrl = ImagePath + GetResource("JourneyPlannerInput.TravelFromToVenueToggle.ImageUrl");
            travelFromToVenueToggle.AlternateText = GetResource("JourneyPlannerInput.TravelFromToVenueToggle.AlternateText");

            // Calendar Resources
            calendar_ButtonText.Value = GetResource("Calendar.ButtonText");
            calendar_NextText.Value = GetResource("Calendar.NextText");
            calendar_PrevText.Value = GetResource("Calendar.PrevText");
            calendar_DayNames.Value = GetResource("Calendar.DayNames");
            calendar_MonthNames.Value = GetResource("Calendar.MonthNames");     
        }
        
        /// <summary>
        /// Sets the accociated controls for from and to labels
        /// </summary>
        private void SetupAssociatedControlsLabel()
        {
            fromLocationLabel.AssociatedControlID = locationFrom.ControlToAssociateLabel;
            toLocationLabel.AssociatedControlID = locationTo.ControlToAssociateLabel;
        }

        /// <summary>
        /// Sets up the JourneySelectControl
        /// </summary>
        private void SetupJourneySelectControl()
        {
            journeySelectControl.Initialise(true);
        }

        /// <summary>
        /// Loads resource strings for labels/controls - Locations links
        /// </summary>
        private void SetupLocationsControl()
        {
            LocationInputMode currentInputMode = GetLocationInputMode();

            #region Set locations text dependent on location input mode

            travelBetweenVenues.Text = GetResource("JourneyPlannerInput.travelBetweenVenues.Text");

            // Venue-to-venue locations text
            if (currentInputMode == LocationInputMode.VenueToVenue)
            {
                lblLocations.Text = GetResource("JourneyPlannerInput.locationLabel.travelBetweenVenues.Text");

                travelBetweenVenues.Text = GetResource("JourneyPlannerInput.travelBetweenVenues.StartAgain.Text");
                travelFromToVenueToggle.ToolTip = GetResource("JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.VenueToVenue");

                locationFrom_Information.ToolTip = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");
                tooltip_information_from.Title = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");
                locationTo_Information.ToolTip = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");
                tooltip_information_to.Title = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");
                
                travelBetweenVenuesDiv.Visible = true;
                travelFromToToggleDiv.Visible = true;
            }
            // From-venue locations text
            else if (currentInputMode == LocationInputMode.FromVenue)
            {
                lblLocations.Text = GetResource("JourneyPlannerInput.locationLabel.travelFromVenue.Text");

                travelFromToVenueToggle.ToolTip = GetResource("JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.FromVenue");

                locationFrom_Information.ToolTip = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");
                tooltip_information_from.Title = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");
                locationTo_Information.ToolTip = GetResource("JourneyPlannerInput.LocationFromInformation.ToolTip");
                tooltip_information_to.Title = GetResource("JourneyPlannerInput.LocationFromInformation.ToolTip");

                travelBetweenVenuesDiv.Visible = true;
                travelFromToToggleDiv.Visible = true;
            }
            // To-venue locations text
            else
            {
                lblLocations.Text = GetResource("JourneyPlannerInput.locationLabel.Text");

                travelFromToVenueToggle.ToolTip = GetResource("JourneyPlannerInput.TravelFromToVenueToggle.ToolTip.ToVenue");

                locationFrom_Information.ToolTip = GetResource("JourneyPlannerInput.LocationFromInformation.ToolTip");
                tooltip_information_from.Title = GetResource("JourneyPlannerInput.LocationFromInformation.ToolTip");
                locationTo_Information.ToolTip = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");
                tooltip_information_to.Title = GetResource("JourneyPlannerInput.LocationToInformation.ToolTip");

                travelBetweenVenuesDiv.Visible = true;
                travelFromToToggleDiv.Visible = true;
            }

            #endregion

            // If the location inputs are ambiguous, then don't show the toggle location links
            if (locationFrom.Status == LocationStatus.Ambiguous
                || locationTo.Status == LocationStatus.Ambiguous)
            {
                travelBetweenVenuesDiv.Visible = false;
                travelFromToToggleDiv.Visible = false;
            }
        }

        /// <summary>
        /// Updates the Journey options control with venue location and datetimes selected
        /// </summary>
        private void UpdateJourneyOptionsControl()
        {
            #region Update Journey Options control with Venue and Datetimes

            // Set the venue for the journey options tab to use
            if (!string.IsNullOrEmpty(venueID.Value))
            {
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                List<SJPLocation> venues = locationService.GetSJPVenueLocations();

                SJPLocation venue = null;
                if (venues != null)
                {
                    venue = venues.SingleOrDefault(vl => vl.ID == venueID.Value);
                }

                journeyOptions.Venue = venue;
            }
            else
            {
                // Set the venue for the journey options tab to use
                if (locationTo.TypeOfLocation == SJPLocationType.Venue) // This will be a journey to a venue location
                {
                    locationTo.Validate();
                    journeyOptions.Venue = locationTo.Location;
                    journeyOptions.IsOriginVenue = false;
                }
                else if (locationFrom.TypeOfLocation == SJPLocationType.Venue) // This will be a journey from a venue location
                {
                    locationFrom.Validate();
                    journeyOptions.Venue = locationFrom.Location;
                    journeyOptions.IsOriginVenue = true;
                }
            }

            DateTime outDateTime = DateTime.MinValue;
            if (!DateTime.TryParse(calendarOutward.Value, out outDateTime))
            {
                outDateTime = eventControl.OurwardDateTime;
            }
            journeyOptions.OutwardDateTime = outDateTime;

            DateTime retDateTime = DateTime.MinValue;
            if (!DateTime.TryParse(calendarOutward.Value, out retDateTime))
            {
                outDateTime = eventControl.ReturnDateTime;
            }
            journeyOptions.ReturnDateTime = retDateTime;

            #endregion
        }

        /// <summary>
        /// Returns the current LocationInputMode based on the state of the From and To location controls
        /// </summary>
        /// <returns></returns>
        private LocationInputMode GetLocationInputMode()
        {
            #region Get current location input mode

            LocationInputMode currentInputMode = LocationInputMode.ToVenue;

            if (locationFrom.TypeOfLocation == SJPLocationType.Venue
                && locationTo.TypeOfLocation == SJPLocationType.Venue)
            {
                currentInputMode = LocationInputMode.VenueToVenue;
            }
            else if (locationFrom.TypeOfLocation == SJPLocationType.Venue
                && locationTo.TypeOfLocation != SJPLocationType.Venue)
            {
                currentInputMode = LocationInputMode.FromVenue;
            }

            #endregion

            return currentInputMode;
        }

        /// <summary>
        /// Toggles the From/To location controls to be Venue-to-venue, From-venue, To-venue
        /// </summary>
        private void ToggleLocationControl(LocationInputMode locationInputMode)
        {
            // Determine current mode (to allow correct reassigning of locations
            LocationInputMode currentInputMode = GetLocationInputMode();

            #region Get currently entered locations

            // Retain the selected locations, to allow inserting back in if possible
            SJPLocation from = locationFrom.Location;
            SJPLocation to = locationTo.Location;

            // Validate incase the location object hasnt been retrieved from the cache
            if (locationFrom.Location == null && locationFrom.Status == LocationStatus.Unspecified)
            {
                if (locationFrom.Validate())
                {
                    from = locationFrom.Location;
                }
                else if (locationFrom.TypeOfLocation == SJPLocationType.Venue)
                {
                    // Issue where constant switching loses the non-venue location 
                    // - this is because the validate works on locationtype, so try and validate as a non-venue
                    // because the location could have non-venue values in its hidden settings preserved from 
                    // a previous location input mode switch
                    if (locationFrom.Validate(SJPLocationType.Unknown))
                    {
                        from = locationFrom.Location;
                    }
                }
            }

            // Validate incase the location object hasnt been retrieved from the cache
            if (locationTo.Location == null && locationTo.Status == LocationStatus.Unspecified)
            {
                if (locationTo.Validate())
                {
                    to = locationTo.Location;
                }
                else if (locationTo.TypeOfLocation == SJPLocationType.Venue)
                {
                    // Issue where constant switching loses the non-venue location 
                    // - this is because the validate works on locationtype, so try and validate as a non-venue
                    // because the location could have non-venue values in its hidden settings preserved from 
                    // a previous location input mode switch
                    if (locationTo.Validate(SJPLocationType.Unknown))
                    {
                        to = locationTo.Location;
                    }
                }
            }

            #endregion
            
            #region Set for new location input mode

            switch (locationInputMode)
            {
                case LocationInputMode.VenueToVenue:
                    
                    locationFrom.Reset(SJPLocationType.Venue);
                    locationTo.Reset(SJPLocationType.Venue);

                    eventControl.ShowReturnDateOnly(false);
                    break;
            
                case LocationInputMode.FromVenue:
            
                    locationFrom.Reset(SJPLocationType.Venue);
                    locationTo.Reset(SJPLocationType.Unknown);

                    eventControl.ShowReturnDateOnly(true);
                    break;
            
                case LocationInputMode.ToVenue:
                default:
                    
                    locationFrom.Reset(SJPLocationType.Unknown);
                    locationTo.Reset(SJPLocationType.Venue);

                    eventControl.ShowReturnDateOnly(false);
                    break;
            }

            #endregion

            #region Set currently entered locations for the new location input mode (where possible)

            // Reassign locations based on the current to the new location input mode.
            // The following has a lot of switches and if's because of the various permutations in the 
            // order the user can switch between the location input modes. Re-Factor at your own risk!
            switch (currentInputMode)
            {
                case LocationInputMode.VenueToVenue:

                    switch (locationInputMode)
                    {
                        // Retain the currently selected venue in the From/To venue mode
                        case LocationInputMode.ToVenue:

                            if ((to != null && to.TypeOfLocation == SJPLocationType.Venue)
                                && (from != null && from.TypeOfLocation != SJPLocationType.Venue))
                            {
                                locationTo.Location = to;
                                locationFrom.Location = from;
                            } 
                            // Handle sceneario where From -> Venue -> To
                            else if ((to != null && to.TypeOfLocation != SJPLocationType.Venue)
                                && (from != null && from.TypeOfLocation == SJPLocationType.Venue))
                            {
                                locationTo.Location = from;
                                locationFrom.Location = to;
                            }
                            else if (to != null && to.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationTo.Location = to;
                            }
                            break;
                        case LocationInputMode.FromVenue:

                            if ((to != null && to.TypeOfLocation != SJPLocationType.Venue)
                                && (from != null && from.TypeOfLocation == SJPLocationType.Venue))
                            {
                                locationTo.Location = to;
                                locationFrom.Location = from;
                            }
                            // Handle sceneario where To -> Venue -> From
                            else if ((to != null && to.TypeOfLocation == SJPLocationType.Venue)
                                && (from != null && from.TypeOfLocation != SJPLocationType.Venue))
                            {
                                locationTo.Location = from;
                                locationFrom.Location = to;
                            }
                            else if (from != null && from.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationFrom.Location = from;
                            }
                            break;
                        case LocationInputMode.VenueToVenue:

                            // Switching the venue locations around
                            if (to != null && to.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationFrom.Location = to;
                            }
                            if (from != null && from.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationTo.Location = from;
                            }
                            break;
                    }
                    break;

                case LocationInputMode.FromVenue:

                    switch (locationInputMode)
                    {
                        case LocationInputMode.VenueToVenue:
                            // Retain the currently selected venue
                            if (from != null && from.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationFrom.Location = from;
                            }
                            else if (to != null && to.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationFrom.Location = to;
                            }
                            break;
                        case LocationInputMode.ToVenue:
                            // Switch the currently input locations around
                            if (from != null && from.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationTo.Location = from; // To is now the venue
                            }
                            if (to != null && to.TypeOfLocation != SJPLocationType.Venue)
                            {
                                locationFrom.Location = to; // From is now the not-venue
                            }
                            break;
                    }
                    break;

                case LocationInputMode.ToVenue:
                default:

                    switch (locationInputMode)
                    {
                        case LocationInputMode.VenueToVenue:
                            // Retain the currently selected venue
                            if (to != null && to.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationTo.Location = to;
                            }
                            else if (from != null && from.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationTo.Location = from;
                            }
                            break;
                        case LocationInputMode.FromVenue:
                            // Switch the currently input locations around
                            if (from != null && from.TypeOfLocation != SJPLocationType.Venue)
                            {
                                locationTo.Location = from; // To is now the not-venue
                            }
                            if (to != null && to.TypeOfLocation == SJPLocationType.Venue)
                            {
                                locationFrom.Location = to; // From is now the venue
                            }
                            break;
                    }
                    break;
            }

            #endregion

            // Ensure journey options control is working with the latest venue
            UpdateJourneyOptionsControl();
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);
        }

        /// <summary>
        /// Submits the request. If successful, then sets the page to transfer to, otherwise this
        /// page is loaded
        /// </summary>
        /// <param name="plannerMode"></param>
        private void SubmitRequest(SJPJourneyPlannerMode plannerMode)
        {
            // Check locations/dates are valid and setup the journey request.
            // Any errors and this page will be displayed again, with errors shown
            if (adapter.ValidateAndBuildSJPRequest(plannerMode))
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
                    else
                    {
                        DisplayMessage(new SJPMessage("JourneyPlannerInput.NoAccessibleVenueError.Text", SJPMessageType.Error));
                        
                        // Persist selected options tab 
                        journeyOptions.PlannerMode = plannerMode;
                        
                        // Stop the code to process the journey request further
                        return;
                    }
                }

                #endregion

                #region Check for The Mall venue

                // Check for The Mall venue
                string mallNaptan = Properties.Current["JourneyPlannerInput.TheMallNaptan"];
                if (locationFrom.Location != null)
                {
                    if (locationFrom.Location is SJPVenueLocation)
                    {
                        if (locationFrom.Location.Naptan.Contains(mallNaptan))
                        {
                            DisplayMessage(new SJPMessage("JourneyPlannerInput.MallMessage.Text", SJPMessageType.Error));
                            return;
                        }
                    }
                }

                if (locationTo.Location != null)
                {
                    if (locationTo.Location is SJPVenueLocation)
                    {
                        if (locationTo.Location.Naptan.Contains(mallNaptan))
                        {
                            DisplayMessage(new SJPMessage("JourneyPlannerInput.MallMessage.Text", SJPMessageType.Error));
                            return;
                        }
                    }
                }

                #endregion

                #region Submit the request

                JourneyPlannerHelper journeyPlannerHelper = new JourneyPlannerHelper();

                if (plannerMode == SJPJourneyPlannerMode.PublicTransport && validAccessibleLocations)
                {
                    // Attempt to submit the request and plan the journey
                    if (journeyPlannerHelper.SubmitRequest(plannerMode, true))
                    {
                        // Set transfer to Journey Options page
                        SetPageTransfer(PageId.JourneyOptions);

                        // Set the query string values for the JourneyOptions page,
                        // this allows the result for the correct request to be loaded
                        AddQueryStringForPage(PageId.JourneyOptions);
                    }
                }
                else if (plannerMode == SJPJourneyPlannerMode.PublicTransport && !validAccessibleLocations)
                {
                    // Validate page details before moving on
                    if (journeyPlannerHelper.SubmitRequest(plannerMode, false))
                    {
                        // Transfer to further accessibility page
                        SetPageTransfer(PageId.AccessibilityOptions);

                        // Add the query string values to allow JourneyLocations page 
                        // to load the details for the correct request
                        AddQueryStringForPage(PageId.AccessibilityOptions);
                    }
                }
                else // All other planner modes require further input
                {
                    // Validate page details before moving on
                    if (journeyPlannerHelper.SubmitRequest(plannerMode, false))
                    {
                        // Transfer to journey locations page
                        SetPageTransfer(PageId.JourneyLocations);

                        // Add the query string values to allow JourneyLocations page 
                        // to load the details for the correct request
                        AddQueryStringForPage(PageId.JourneyLocations);
                    }
                }

                #endregion
            }
            else
            {
                DisplayMessage(new SJPMessage("JourneyPlannerInput.ValidationError.Text", SJPMessageType.Error));

                // Display specific error message

                // Invalid date is found by ValidateAndBuildSJPRequest, and therefore will not reach the Submit validate and run
                if (!eventControl.Validate())
                {
                    DisplayMessage(new SJPMessage("ValidateAndRun.DateNotValid", SJPMessageType.Error));
                }
            }

            // Persist selected options tab if the Submit validation fails and this page is displayed again
            journeyOptions.PlannerMode = plannerMode;
        }

        /// <summary>
        /// Loads any messages currently in the Session
        /// </summary>
        private void SetupMessages()
        {
            SJPWeb master =(SJPWeb)Master;
            if (pnlMessages != null && rptrMessages != null)
            {
                // Messages to be displayed on page
                Dictionary<string, SJPMessage> messagesToDisplay = new Dictionary<string, SJPMessage>();

                // Check session for messages
                InputPageState pageState = SJPSessionManager.Current.PageState;

                if (pageState.Messages.Count > 0)
                {
                    // Get messages
                    master.BuildMessages(pageState.Messages, ref messagesToDisplay);
                                        
                    // Clear to avoid messages being displayed again
                    pageState.ClearMessages();
                }

                // Check this page's message list
                if (master.PageMessages.Count > 0)
                {
                    // Get messages
                    master.BuildMessages(master.PageMessages, ref messagesToDisplay);
                }

                // if the venue overlap message is displaying show venue map in right hand bar
                if (messagesToDisplay.ContainsKey("ValidateAndRun.OriginAndDestinationOverlaps"))
                {
                    master.ShowVenueMapOnInputPage = true;
                }

                // Display all messages in repeater
                rptrMessages.DataSource = messagesToDisplay.Values;
                rptrMessages.DataBind();

                pnlMessages.Visible = messagesToDisplay.Count > 0;
            }

        }

        /// <summary>
        /// Adds debug information to page (where possible)
        /// </summary>
        private void SetupDebugInformation()
        {
            if (DebugHelper.ShowDebug)
            {
                try
                {
                    debugInfoDiv.Visible = true;

                    CookieHelper ch = new CookieHelper();

                    lblDebugInfo.Text = string.Format("Cookie: {0} <br />", ch.GetCookieString());
                }
                catch (Exception ex)
                {
                    // Any exceptions in outputting debug info, display it so it can be fixed in the future 
                    lblDebugInfo.Text = string.Format("Message: {0} <br />StackTrace: {1}<br />",
                        ex.Message,
                        ex.StackTrace);
                }
            }
        }

        #endregion
    }
}
