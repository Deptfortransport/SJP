// *********************************************** 
// NAME             : JourneyLocations.aspx      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: JourneyLocations page
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.LocationService;
using SJP.Common.Web;
using SJP.Reporting.Events;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.SJPWeb.Adapters;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// JourneyLocations page
    /// </summary>
    public partial class JourneyLocations : SJPPage
    {
        #region Variables

        private ISJPJourneyRequest journeyRequest = null;
        private JourneyPlannerInputAdapter adapter;

        private bool logPageActionEvent = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyLocations()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.JourneyLocations;
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Init
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            riverServicesJourneyLocations.GoBack += new EventHandler(riverServicesJourneyLocations_GoBack);
        }
        
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            adapter = new JourneyPlannerInputAdapter();

            JourneyHelper journeyHelper = new JourneyHelper();
            SessionHelper sessionHelper = new SessionHelper();

            string journeyHash = journeyHelper.GetJourneyRequestHash();
            journeyRequest = sessionHelper.GetSJPJourneyRequest(journeyHash);

            SetupCycleJourneyLocations();
            SetupParkAndRideJourneyLocations();
            SetupRiverServicesJourneyLocations();

            AddStyleSheet("jquery-ui-1.8.13.css");
            AddStyleSheet("jquery.qtip.min.css");

            AddJavascript("jquery-ui-1.8.13.min.js");
            AddJavascript("Common.js");
            AddJavascript("JourneyLocations.js");
            AddJavascript("jquery.ui.selectmenu.js");
            AddJavascript("jquery.qtip.min.js");
        }

        /// <summary>
        /// Page_LoadComplete event hadler.
        /// Does the processing after all the sub control's page load event fired
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices
               && (riverServicesJourneyLocations.IsRiverServiceRequestInProgress
                 || riverServicesJourneyLocations.IsRiverServiceResultsVisible))
            {
                // Tracks if a page action event should be logged in the PreRender event of page
                logPageActionEvent = true;

                // Base page shouldn't log the page entry because the page action event is logged 
                // by this page (in posbacks only)
                if (IsPostBack)
                {
                    logPageEntry = false;
                }
            }
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();
            SetupSubmitButton();

            if (logPageActionEvent)
            {
                // Log in postbacks only because Results could have been available on first render of this page
                // and therefore don't need to log this page action event
                if (IsPostBack && !riverServicesJourneyLocations.IsRiverServiceRequestInProgress)
                {
                    // Log an event because user is still waiting for journey results
                    PageEntryEvent logPage = new PageEntryEvent(Common.PageId.JourneyLocationsStopEventWait, SJPSessionManager.Current.Session.SessionID, false);
                    Logger.Write(logPage);
                }
                else if (IsPostBack && riverServicesJourneyLocations.IsRiverServiceResultsVisible)
                {
                    // Log an event because journey results are now available to user
                    PageEntryEvent logPage = new PageEntryEvent(Common.PageId.JourneyLocationsStopEventResult, SJPSessionManager.Current.Session.SessionID, false);
                    Logger.Write(logPage);
                }
            }
            
            // Only show the back button on river services if the stop event results are visible
            if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices)
            {
                btnBack.Visible = riverServicesJourneyLocations.IsStopEventResultsAvailable
                    && IsPostBack
                    && riverServicesJourneyLocations.IsRiverServiceResultsVisible;
                
                // If the river services not found for the venue make the back button visible 
                // So user can navigate to previous page
                if (!riverServicesJourneyLocations.IsRiverServiceAvailable)
                {
                    btnBack.Visible = true;
                }
            }
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// Event handler for Back button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if(journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices
                && riverServicesJourneyLocations.IsRiverServiceResultsVisible)
            {
                riverServicesJourneyLocations.Reset();
            }
            else
            {
                SetPageTransfer(PageId.JourneyPlannerInput);
            }
        }

        /// <summary>
        /// Event handler for the river services back button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void riverServicesJourneyLocations_GoBack(object sender, EventArgs e)
        {
            SetPageTransfer(PageId.JourneyPlannerInput);
        }

        /// <summary>
        /// Event handler for plan journey button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPlanJourney_Click(object sender, EventArgs e)
        {
            SubmitRequest();
        }

        #endregion
               
        #region Private Methods

        /// <summary>
        /// Sets up Cycle Journey Locations control 
        /// </summary>
        private void SetupCycleJourneyLocations()
        {
            if (journeyRequest != null && journeyRequest.PlannerMode == SJPJourneyPlannerMode.Cycle)
            {
                SJPVenueLocation venue = null;

                if (journeyRequest.Destination is SJPVenueLocation)
                {
                    venue = (SJPVenueLocation)journeyRequest.Destination;
                }
                else if (journeyRequest.Origin is SJPVenueLocation)
                {
                    venue = (SJPVenueLocation)journeyRequest.Origin;
                }
    
                cycleJourneyLocations.Venue = venue;

                // Set travel dates to validate venue cycle park
                if (journeyRequest.IsOutwardRequired)
                {
                    cycleJourneyLocations.OutwardDateTime = journeyRequest.OutwardDateTime;
                }
                if (journeyRequest.IsReturnRequired)
                {
                    cycleJourneyLocations.ReturnDateTime = journeyRequest.ReturnDateTime;
                }

                // Set the selected value to be the specified value (allows previous choice to be retained when returning
                // to the locations page)
                cycleJourneyLocations.SelectedCycleRouteType = journeyRequest.CycleAlgorithm;

                cycleJourneyLocations.Visible = true;
            }
        }
        
        /// <summary>
        /// Sets up Park and Ride Locations control
        /// </summary>
        private void SetupParkAndRideJourneyLocations()
        {
            if (journeyRequest != null)
            {
                if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.ParkAndRide
                        || journeyRequest.PlannerMode == SJPJourneyPlannerMode.BlueBadge)
                {
                    SJPVenueLocation venue = null;

                    if (journeyRequest.Destination is SJPVenueLocation)
                    {
                        venue = (SJPVenueLocation)journeyRequest.Destination;
                    }
                    else if (journeyRequest.Origin is SJPVenueLocation)
                    {
                        venue = (SJPVenueLocation)journeyRequest.Origin;
                    }

                    parkAndRideJourneyLocations.Venue = venue;
                    
                    // Set travel dates to validate venue car park
                    if (journeyRequest.IsOutwardRequired)
                    {
                        parkAndRideJourneyLocations.OutwardDateTime = journeyRequest.OutwardDateTime;
                    }
                    if (journeyRequest.IsReturnRequired)
                    {
                        parkAndRideJourneyLocations.ReturnDateTime = journeyRequest.ReturnDateTime;
                    }

                    parkAndRideJourneyLocations.IsBlueBadge = journeyRequest.PlannerMode == SJPJourneyPlannerMode.BlueBadge;

                    parkAndRideJourneyLocations.Visible = true;
                }
            }
        }

        /// <summary>
        /// Sets up River services locations control
        /// </summary>
        private void SetupRiverServicesJourneyLocations()
        {
            if (journeyRequest != null)
            {
                if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices)
                {
                    SJPVenueLocation venue = null;

                    if (journeyRequest.Destination is SJPVenueLocation)
                    {
                        venue = (SJPVenueLocation)journeyRequest.Destination;
                    }
                    else if (journeyRequest.Origin is SJPVenueLocation)
                    {
                        venue = (SJPVenueLocation)journeyRequest.Origin;
                    }

                    riverServicesJourneyLocations.Venue = venue;

                    // Set travel dates
                    if (journeyRequest.IsOutwardRequired)
                    {
                        riverServicesJourneyLocations.OutwardDateTime = journeyRequest.OutwardDateTime;
                    }
                    if (journeyRequest.IsReturnRequired)
                    {
                        riverServicesJourneyLocations.ReturnDateTime = journeyRequest.ReturnDateTime;
                    }

                    riverServicesJourneyLocations.Visible = true;
                }
            }
        }

        /// <summary>
        /// Sets up control resource contents
        /// </summary>
        private void SetupResources()
        {
            btnBack.Text = Server.HtmlDecode(GetResource("JourneyLocations.Back.Text"));
            btnBack.ToolTip = Server.HtmlDecode(GetResource("JourneyLocations.Back.ToolTip"));
            btnPlanJourney.Text = Server.HtmlDecode(GetResource("JourneyLocations.PlanJourney.Text"));
            btnPlanJourney.ToolTip = Server.HtmlDecode(GetResource("JourneyLocations.PlanJourney.ToolTip"));

            if (journeyRequest != null)
            {
                if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.Cycle)
                    sjpHeading.InnerText = GetResource("JourneyLocations.Heading.Cycle.Text");
                else if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.ParkAndRide)
                    sjpHeading.InnerText = GetResource("JourneyLocations.Heading.ParkAndRide.Text");
                else if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.BlueBadge)
                    sjpHeading.InnerText = GetResource("JourneyLocations.Heading.BlueBadge.Text");
                else if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices)
                    sjpHeading.InnerText = GetResource("JourneyLocations.Heading.RiverServices.Text");
            }
        }

        /// <summary>
        /// Displays or hides the submit button
        /// </summary>
        private void SetupSubmitButton()
        {
            // Set to not visible be default
            btnPlanJourney.Visible = false;

            if (journeyRequest != null)
            {
                if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.ParkAndRide
                        || journeyRequest.PlannerMode == SJPJourneyPlannerMode.BlueBadge)
                {
                    btnPlanJourney.Visible = parkAndRideJourneyLocations.IsCarParksAvailable;
                }
                else if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.Cycle)
                {
                    btnPlanJourney.Visible = cycleJourneyLocations.IsCycleParksAvailable;
                }
                else if (journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices)
                {
                    btnPlanJourney.Visible = riverServicesJourneyLocations.IsStopEventResultsAvailable 
                        && IsPostBack
                        && riverServicesJourneyLocations.IsRiverServiceResultsVisible;
                }
            }
        }

        /// <summary>
        /// Submits the request.
        /// </summary>
        private void SubmitRequest()
        {
            if (journeyRequest != null)
            {
                bool validRequest = false;

                switch (journeyRequest.PlannerMode)
                {
                    case SJPJourneyPlannerMode.Cycle:
                        validRequest = adapter.ValidateAndUpdateSJPRequestForSJPPark(
                            cycleJourneyLocations.SelectedCyclePark,
                            cycleJourneyLocations.SelectedOutwardDateTime,
                            cycleJourneyLocations.SelectedReturnDateTime,
                            cycleJourneyLocations.SelectedCycleRouteType);
                        break;
                    case SJPJourneyPlannerMode.ParkAndRide:
                    case SJPJourneyPlannerMode.BlueBadge:
                        validRequest = adapter.ValidateAndUpdateSJPRequestForSJPPark(
                            parkAndRideJourneyLocations.SelectedCarPark,
                            parkAndRideJourneyLocations.SelectedOutwardDateTime,
                            parkAndRideJourneyLocations.SelectedReturnDateTime,
                            string.Empty);
                        break;
                    case SJPJourneyPlannerMode.RiverServices:
                        validRequest = adapter.ValidateAndUpdateSJPRequestForSJPRiverServices(
                            riverServicesJourneyLocations.SelectedOutwardStopEventJourney,
                            riverServicesJourneyLocations.SelectedOutwardDateTime,
                            riverServicesJourneyLocations.SelectedReturnStopEventJourney,
                            riverServicesJourneyLocations.SelectedReturnDateTime);
                        break;
                }

                JourneyPlannerHelper journeyPlannerHelper = new JourneyPlannerHelper();

                if (validRequest && journeyPlannerHelper.SubmitRequest(journeyRequest.PlannerMode, true))
                {
                    // Set transfer to Journey Options page
                    SetPageTransfer(PageId.JourneyOptions);

                    // Set the query string values for the JourneyOptions page,
                    // this allows the result for the correct request to be loaded
                    AddQueryStringForPage(PageId.JourneyOptions);
                }
                else
                {
                    DisplayMessage(new SJPMessage("JourneyLocations.ValidationError.Text", SJPMessageType.Error));
                }
            }
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);

            // Display the message seperator div (just a line seperator image)
            messageSeprator.Visible = true;
        }

        #endregion
    }
}
