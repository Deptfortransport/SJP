// *********************************************** 
// NAME             : RiverServicesJourneyLocations.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 May 2011
// DESCRIPTION  	: River services journey locations options page
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.SJPWeb.Adapters;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// River services journey locations options page
    /// 
    /// Enables user to select River service routes and find the departure times for the servicer routes.
    /// Once user has selected a River service route and departur time it enables to plan journey to river service pier
    /// </summary>
    public partial class RiverServicesJourneyLocations : System.Web.UI.UserControl
    {
        #region Events

        public event EventHandler GoBack;

        #endregion

        #region Private Fields

        private const string NO_RESULT_INFO = "JourneyOptions.NoResultsFound.UserInfo";
        private const string NO_RESULT_ERROR = "JourneyOptions.NoResultsFound.Error";

        private SJPVenueLocation venue;
        private List<SJPVenueRiverService> riverServiceList;
        private DateTime dateTimeOutward = DateTime.MinValue;
        private DateTime dateTimeReturn = DateTime.MinValue;
        private bool refreshTagVisible = false;

        private Journey selectedOutwardJourney = null;
        private Journey selectedReturnJourney = null;

        private SJPPage page = null;

        private bool riverServiceRequestInProgress = false;

        private SessionHelper sessionHelper = new SessionHelper();
        
        #endregion

        #region Public Properties

        /// <summary>
        /// SJP venue location for which River services options required
        /// </summary>
        public SJPVenueLocation Venue
        {
            get { return venue; }
            set { venue = value; }
        }

        /// <summary>
        /// Read/Write. Outward date time to use
        /// </summary>
        public DateTime OutwardDateTime
        {
            get { return dateTimeOutward; }
            set { dateTimeOutward = value; }
        }

        /// <summary>
        /// Read/Write. Return date time to use
        /// </summary>
        public DateTime ReturnDateTime
        {
            get { return dateTimeReturn; }
            set { dateTimeReturn = value; }
        }

        /// <summary>
        /// Determines if the page having refresh meta tag enabled
        /// </summary>
        public bool RefreshTagVisible
        {
            get { return refreshTagVisible; }
            set { refreshTagVisible = value; }
        }

        /// <summary>
        /// Read only property to represent the selected stop event journey
        /// </summary>
        public Journey SelectedOutwardStopEventJourney
        {
            get
            {
                int journeyId = -1;
                if (SJPSessionManager.Current.PageState != null)
                {
                    journeyId = SJPSessionManager.Current.PageState.StopEventJourneyIdOutward;
                }
                JourneyResultHelper resultHelper = new JourneyResultHelper();

                bool stopEventResultAvailable = resultHelper.IsStopEventResultAvailable;

                if (stopEventResultAvailable)
                {
                    ISJPJourneyResult stopEventResult = resultHelper.StopEventResult;

                    if (stopEventResult.OutwardJourneys.Count > 0)
                    {
                        selectedOutwardJourney = stopEventResult.OutwardJourneys.SingleOrDefault(ser => ser.JourneyId == journeyId);
                    }
                }
                else
                {
                    selectedOutwardJourney = null;
                }

                return selectedOutwardJourney;
            }
        }

        /// <summary>
        /// Read only property to represent the selected stop event journey
        /// </summary>
        public Journey SelectedReturnStopEventJourney
        {
            get
            {
                int journeyId = -1;
                if (SJPSessionManager.Current.PageState != null)
                {
                    journeyId = SJPSessionManager.Current.PageState.StopEventJourneyIdReturn;
                }
                JourneyResultHelper resultHelper = new JourneyResultHelper();

                bool stopEventResultAvailable = resultHelper.IsStopEventResultAvailable;

                if (stopEventResultAvailable)
                {
                    ISJPJourneyResult stopEventResult = resultHelper.StopEventResult;
                    if (stopEventResult.ReturnJourneys.Count > 0)
                    {
                        selectedReturnJourney = stopEventResult.ReturnJourneys.SingleOrDefault(ser => ser.JourneyId == journeyId);
                    }
                }
                else
                {
                    selectedReturnJourney = null;
                }

                return selectedReturnJourney;
            }
        }

        /// <summary>
        /// Read only property to return the selected journey outward date time updated with any interchange times
        /// </summary>
        public DateTime SelectedOutwardDateTime
        {
            get
            {
                // Amend the selected service time with an interchange time
                DateTime selectedOutwardDateTime = GetInterchangeTime(selectedOutwardJourney != null ? selectedOutwardJourney.StartTime : DateTime.MinValue, true);

                return selectedOutwardDateTime;
            }
        }

        /// <summary>
        /// Read only property to return the selected journey return date time updated with any interchange times
        /// </summary>
        public DateTime SelectedReturnDateTime
        {
            get
            {
                // Amend the selected service time with an interchange time
                DateTime selectedReturnDateTime = GetInterchangeTime(selectedReturnJourney != null ? selectedReturnJourney.EndTime : DateTime.MinValue, false);

                return selectedReturnDateTime;
            }
        }

        /// <summary>
        /// Read only property determining if the stop event results are available
        /// </summary>
        public bool IsStopEventResultsAvailable
        {
            get
            {
                JourneyResultHelper resultHelper = new JourneyResultHelper();
                return resultHelper.IsStopEventResultAvailable;
            }
        }

        /// <summary>
        /// Read only Determines if the river service route result is visible
        /// </summary>
        public bool IsRiverServiceResultsVisible
        {
            get
            {
                JourneyResultHelper resultHelper = new JourneyResultHelper();

                // Check if the stop event results available 
                if (resultHelper.IsStopEventResultAvailable)
                {
                    ISJPJourneyResult stopEventResult = resultHelper.StopEventResult;

                    if (stopEventResult != null)
                    {
                        return ((stopEventResult.OutwardJourneys.Count > 0)
                                    || (stopEventResult.ReturnJourneys.Count > 0))
                                    && riverServiceResults.Visible;
                    }
                }
                
                return false;
            }
        }

        /// <summary>
        /// Read only Determines if the river service route departure board request is in progress
        /// </summary>
        public bool IsRiverServiceRequestInProgress
        {
            get { return riverServiceRequestInProgress; }
        }

        /// <summary>
        /// Read only property determining if the River services available for the venue on the date selected
        /// </summary>
        public bool IsRiverServiceAvailable
        {
            get { return (riverServiceList != null && riverServiceList.Count > 0); }
        }
        
        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Init Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            page = (SJPPage)Page;

            // Attach to selected journey changed events
            riverServiceResultOutward.SelectedJourneyHandler += new OnSelectedJourneyChange(riverServiceResultOutward_SelectedJourneyHandler);
            riverServiceResultReturn.SelectedJourneyHandler += new OnSelectedJourneyChange(riverServiceResultReturn_SelectedJourneyHandler);

            // Attach to the replan journey events
            riverServiceResultOutward.ReplanJourneyHandler += new OnReplanJourney(riverServiceResultOutward_ReplanJourneyHandler);
            riverServiceResultReturn.ReplanJourneyHandler += new OnReplanJourney(riverServiceResultReturn_ReplanJourneyHandler);
        }

        /// <summary>
        /// Page_Load Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupRiverServices();

            DisplayMessages();

            if (!IsPostBack)
            {
                InitWaitTimer();
            }
            else
            {
                JourneyResultHelper resultHelper = new JourneyResultHelper();

                // check if the stop event request call executing
                if (waitTimer.Enabled || RefreshTagVisible)
                {
                    int waitCount = UpdateWaitCount();

                    bool stopEventResultAvailable = resultHelper.IsStopEventResultAvailable;

                    if (stopEventResultAvailable)
                    {
                        #region Bind results

                        #region Show error messages

                        // Journeys have returned, display them
                        ISJPJourneyResult stopEventResult = resultHelper.StopEventResult;
                        if (stopEventResult.Messages.Count > 0)
                        {
                            bool isReplanError = false;
                                                        
                            // Journey Result returned with error messages
                            foreach (SJPMessage message in stopEventResult.Messages)
                            {
                                DisplayMessage(message);

                                #region Check for Replan error (earlier/later service request)

                                if ((message.MajorMessageNumber == Codes.SJPNoEarlierServicesOutward)
                                    || (message.MajorMessageNumber == Codes.SJPNoLaterServicesOutward)
                                    || (message.MajorMessageNumber == Codes.SJPNoEarlierServicesReturn)
                                    || (message.MajorMessageNumber == Codes.SJPNoLaterServicesReturn))
                                {
                                    isReplanError = true;

                                    switch (message.MajorMessageNumber)
                                    {
                                        case Codes.SJPNoEarlierServicesOutward:
                                            sessionHelper.UpdateEarlierLinkFlag(true, true, false);
                                            break;
                                        case Codes.SJPNoLaterServicesOutward:
                                            sessionHelper.UpdateLaterLinkFlag(true, true, false);
                                            break;
                                        case Codes.SJPNoEarlierServicesReturn:
                                            sessionHelper.UpdateEarlierLinkFlag(false, true, false);
                                            break;
                                        case Codes.SJPNoLaterServicesReturn:
                                            sessionHelper.UpdateLaterLinkFlag(false, true, false);
                                            break;
                                    }
                                }

                                #endregion
                            }

                            // Journey Result returned with error message... Add instruction for user
                            if (!isReplanError)
                            {
                                DisplayMessage(new SJPMessage(NO_RESULT_INFO, SJPMessageType.Info));
                            }
                        }

                        #endregion

                        // Display journeys if they exist
                        if ((stopEventResult.OutwardJourneys.Count > 0)
                            || (stopEventResult.ReturnJourneys.Count > 0))
                        {

                            // Journey result returned without errors, Bind journey result to journey result controls
                            BindStopEventResult(resultHelper.StopEventRequest, stopEventResult);
                            riverServiceRequestInProgress = false;
                        }
                        else
                        {
                            riverServiceResults.Visible = false;
                            riverServiceRoutes.Visible = true;
                            riverServiceRequestInProgress = false;

                        }

                        // Stop refresh
                        journeyProgress.Visible = false;
                        waitTimer.Enabled = false;
                        refreshTagVisible = false;

                        #endregion
                    }
                    else
                    {
                        // If results not available and have exceeded the wait count, then display error
                        int maxWaitCount = Properties.Current["RiverServicesJourneyLocations.Wait.RefreshCount.Max"].Parse(12);

                        riverServiceRequestInProgress = true;

                        if (waitCount > maxWaitCount)
                        {
                            DisplayMessage(new SJPMessage(NO_RESULT_ERROR, SJPMessageType.Error));
                            DisplayMessage(new SJPMessage(NO_RESULT_INFO, SJPMessageType.Info));

                            riverServiceRequestInProgress = false;

                            riverServiceResults.Visible = false;

                            // Stop refresh
                            //journeyProgress.Visible = false;
                            waitTimer.Enabled = false;
                            refreshTagVisible = false;
                        }
                    }
                }

            }

        }

        /// <summary>
        /// Page_PreRender event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();

            if (!IsPostBack)
            {
                SetupVenueMap();
                SetupRiverServicesDropDown();
            }

            // Only display dropdowns/labels if river services found
            if (riverServiceList != null && riverServiceList.Count > 0)
            {
                pnlRiverServices.Visible = true;
            }
            else
            {
                pnlRiverServices.Visible = false;
            }
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// FindDepartureTimes button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFindDepartureTimes_Click(object sender, EventArgs e)
        {
            // Builds and submits the StopEventRequest for the selected service route
            SubmitRiverServiceRequest();
        }

        /// <summary>
        /// Outward river service stop event result selected journey change event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void riverServiceResultReturn_SelectedJourneyHandler(object sender, JourneyEventArgs e)
        {
            int journeyId = e.JourneyId;

            InputPageState pageState = SJPSessionManager.Current.PageState;

            pageState.StopEventJourneyIdReturn = journeyId;
        }

        /// <summary>
        /// Return river service stop event result selected journey change event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void riverServiceResultOutward_SelectedJourneyHandler(object sender, JourneyEventArgs e)
        {
            int journeyId = e.JourneyId;

            InputPageState pageState = SJPSessionManager.Current.PageState;

            pageState.StopEventJourneyIdOutward = journeyId;
        }

        #region Replan journey events

        /// <summary>
        /// Replans the outward journey for the current request
        /// </summary>
        protected void riverServiceResultOutward_ReplanJourneyHandler(object sender, ReplanJourneyEventArgs e)
        {
            if (e != null)
            {
                ReplanRiverServiceRequest(true, e.IsEarlier);
            }
        }

        /// <summary>
        /// Replans the return journey for the current request
        /// </summary>
        protected void riverServiceResultReturn_ReplanJourneyHandler(object sender, ReplanJourneyEventArgs e)
        {
            if (e != null)
            {
                ReplanRiverServiceRequest(false, e.IsEarlier);
            }
        }

        #endregion

        /// <summary>
        /// Event handler for Back button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (GoBack != null)
            {
                GoBack(sender, EventArgs.Empty);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets the control to river service route selection mode
        /// </summary>
        public void Reset()
        {
            riverServiceRoutes.Visible = true;
            riverServiceResults.Visible = false;
            journeyProgress.Visible = false;
            riverServiceResultOutward.Visible = false;
            riverServiceResultReturn.Visible = false;
            riverServiceResultsHeading.Visible = false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up resource content for the controls
        /// </summary>
        private void SetupResources()
        {
            btnBack.Text = Server.HtmlDecode(page.GetResource("JourneyLocations.Back.Text"));
            routeSelection.Text = (dateTimeOutward != DateTime.MinValue) ?
                page.GetResource("RiverServicesJourneyLocations.RouteSelection.To.Text") :
                page.GetResource("RiverServicesJourneyLocations.RouteSelection.From.Text");

            btnFindDepartureTimes.Text = page.GetResource("RiverServicesJourneyLocations.BtnFindDepartureTimes.Text");
            btnFindDepartureTimes.ToolTip = page.GetResource("RiverServicesJourneyLocations.BtnFindDepartureTimes.ToolTip");

            loading.ImageUrl = page.ImagePath + page.GetResource("RiverServicesJourneyLocations.Loading.Imageurl");
            loading.AlternateText = Server.HtmlDecode(page.GetResource("RiverServicesJourneyLocations.Loading.AlternateText"));

            loadingMessage.Text = Server.HtmlDecode(page.GetResource("RiverServicesJourneyLocations.loadingMessage.Text"));

            riverServiceResultsHeading.Text = page.GetResource("RiverServicesJourneyLocations.RiverServiceResultsHeading.Text");

            if (venue != null)
            {
                usetheMap.Text = string.Format(page.GetResource("RiverServicesJourneyLocations.UseTheMap.Text"), venue.DisplayName);
            }
        }

        /// <summary>
        /// Sets any messages to show based on validation of river services
        /// </summary>
        private void DisplayMessages()
        {
            // Display message if no car parks found, must be placed in Load method 
            // so Master page can display it
            if (venue != null && (riverServiceList == null || riverServiceList.Count == 0))
            {
                SJPMessage sjpMessage = null;

                sjpMessage = new SJPMessage("RiverServicesJourneyLocations.RiverServiceNoneFound.Text", SJPMessageType.Error);

                ((SJPWeb)this.Page.Master).DisplayMessage(sjpMessage);
            }
        }

        /// <summary>
        /// Sets any messages to show based on validation of river services
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            if (sjpMessage != null)
            {
                ((SJPWeb)this.Page.Master).DisplayMessage(sjpMessage);
            }
        }

        /// <summary>
        /// Sets up venue map url using resource content
        /// </summary>
        private void SetupVenueMap()
        {
            if (venue != null)
            {
                SJPPage page = (SJPPage)Page;

                Language language = CurrentLanguage.Value;

                string venueMapUrl_ResourceId = "JourneyLocations." + venue.Naptan[0] + ".RiverServices.Url";
                string venueMapAlternateText_ResourceId = "JourneyLocations." + venue.Naptan[0] + ".RiverServices.AlternateText";

                venueMap.ImageUrl = page.ImagePath + Global.SJPResourceManager.GetString(language, venueMapUrl_ResourceId);
                venueMap.AlternateText = Global.SJPResourceManager.GetString(language, venueMapAlternateText_ResourceId);
                venueMap.ToolTip = venueMap.AlternateText;
            }
        }

        /// <summary>
        /// Sets up the car park for the provided venue
        /// </summary>
        private void SetupRiverServices()
        {
            // Venue should have been set by now
            if (venue != null)
            {
                // Get River Services
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                // River services list
                riverServiceList = locationService.GetSJPVenueRiverServices(venue.Naptan);

                if (Properties.Current["RiverServicesJourneyLocations.VenueMap.Clickable.Switch"].Parse(true))
                {
                    SetupClickableMapBullets(riverServiceList);
                }
            }
        }

        /// <summary>
        /// Sets up cycle park map bullet links
        /// The links rendered as absolute position style set in content database
        /// </summary>
        /// <param name="cycleParkList"></param>
        private void SetupClickableMapBullets(List<SJPVenueRiverService> riverServiceList)
        {
            if (riverServiceList != null)
            {
                Language language = CurrentLanguage.Value;

                mapBulletTarget.Value = routeSelectionOptions.ClientID;

                foreach (SJPVenueRiverService riverService in riverServiceList)
                {
                    using (HtmlAnchor bullet = new HtmlAnchor())
                    {
                        string bulletStyle_ResourceId = string.Format("JourneyLocations.RiverServices.{0}_{1}.Style", riverService.RemotePierNaPTAN, riverService.VenuePierNaPTAN);

                        string bulletStyle = Global.SJPResourceManager.GetString(language, bulletStyle_ResourceId);
                        if (!string.IsNullOrEmpty(bulletStyle))
                        {
                            bullet.Title = string.Format(page.GetResource("RiverServicesJourneyLocations.RouteSelectionOptions.Option.Text"), riverService.RemotePierName, riverService.VenuePierName);
                            bullet.Attributes["style"] = bullet.Attributes["style"] + bulletStyle + "display:none";
                            bullet.Attributes["class"] = "bullet " + riverService.RemotePierNaPTAN;
                            bullet.HRef = "#";
                            bullet.InnerHtml = "&nbsp;";
                            bullet.Name = string.Format("{0}:{1}", riverService.RemotePierNaPTAN, riverService.VenuePierNaPTAN);

                            mapBullets.Controls.Add(bullet);

                            if (Properties.Current["RiverServicesJourneyLocations.VenueMap.MapRoutes.Switch"].Parse(false))
                            {
                                // Create image to overlay on the map
                                using (Image img = new Image())
                                {
                                    string routeImage = page.GetResource(string.Format("JourneyLocations.RiverServices.{0}_{1}.ImageUrl", riverService.RemotePierNaPTAN, riverService.VenuePierNaPTAN));

                                    if (!string.IsNullOrEmpty(routeImage))
                                    {
                                        img.ToolTip = img.AlternateText = bullet.Title;
                                        img.ID = riverService.RemotePierNaPTAN;
                                        img.Attributes["style"] = "position:absolute;left:0px;top:0px;display:none;";
                                        img.CssClass = "riverRoute " + riverService.RemotePierNaPTAN;
                                        img.ImageUrl = page.ImagePath + routeImage;

                                        mapBullets.Controls.Add(img);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Binds venue car parks to preferredParkOptions dropdown
        /// </summary>
        private void SetupRiverServicesDropDown()
        {
            List<SJPVenueRiverService> riverServices = riverServiceList;
            SJPPage page = (SJPPage)Page;

            if (riverServices != null)
            {
                // If an outward date has not been supplied, then show the service text as from Venue Pier to Remote Pier,
                // but retain the values order to avoid having to change the logic
                bool isToVenue = (dateTimeOutward != DateTime.MinValue);
                
                // order the river services by remote pier name and build the river service route options
                foreach (SJPVenueRiverService riverService in riverServices.OrderBy(vrs => vrs.RemotePierName))
                {
                    ListItem service = new ListItem();
                    service.Text = 
                        string.Format(page.GetResource("RiverServicesJourneyLocations.RouteSelectionOptions.Option.Text"), 
                        isToVenue ? riverService.RemotePierName : riverService.VenuePierName,
                        isToVenue ? riverService.VenuePierName : riverService.RemotePierName);
                    service.Value = string.Format("{0}:{1}", riverService.RemotePierNaPTAN, riverService.VenuePierNaPTAN);

                    routeSelectionOptions.Items.Add(service);
                }

                routeSelectionOptions.DataBind();
            }
        }

        /// <summary>
        /// Updates the date time with the pier interchange time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DateTime GetInterchangeTime(DateTime dateTime, bool outward)
        {
            if (dateTime != DateTime.MinValue)
            {
                int interchangeMinutes = Properties.Current["JourneyControl.RiverService.Interchange.Minutes"].Parse(0);

                TimeSpan interchangeTime = new TimeSpan(0, interchangeMinutes, 0);

                if (outward)
                {
                    dateTime = dateTime.Subtract(interchangeTime);
                }
                else
                {
                    dateTime = dateTime.Add(interchangeTime);
                }
            }

            return dateTime;
        }

        /// <summary>
        /// Method to submit the river service request
        /// </summary>
        private void SubmitRiverServiceRequest()
        {
            // Clear any previous earlier/later link flags first time service request is submitted
            sessionHelper.ResetEarlierLaterLinkFlags(true);

            // Get the selected route and perform a Stop Event request to find the services
            string selectedRoute = routeSelectionOptions.SelectedValue;

            if (!string.IsNullOrEmpty(selectedRoute))
            {
                string remotePierNaPTAN = selectedRoute.Trim().Split(new char[] { ':' })[0];

                string venuePierNaPTAN = selectedRoute.Trim().Split(new char[] { ':' })[1];

                JourneyLocationsAdapter journeyLocationsAdapter = new JourneyLocationsAdapter();

                // Build the Stop Event request
                bool valid = journeyLocationsAdapter.ValidateAndBuildSJPStopEventRequest(venuePierNaPTAN, remotePierNaPTAN);

                if (valid)
                {
                    JourneyPlannerHelper journeyPlannerHelper = new JourneyPlannerHelper();

                    // Submit the Stop Event request
                    if (journeyPlannerHelper.SubmitStopEventRequest(true))
                    {
                        // Display/Hide appropriate controls
                        waitTimer.Enabled = true;
                        riverServiceRoutes.Visible = false;
                        riverServiceResults.Visible = true;
                        journeyProgress.Visible = true;
                        riverServiceResultOutward.Visible = false;
                        riverServiceResultReturn.Visible = false;
                        riverServiceResultsHeading.Visible = false;
                        riverServiceRequestInProgress = true;
                    }
                }
            }
        }

        /// <summary>
        /// Method to replan the river service request (for earlier or later) 
        /// </summary>
        private void ReplanRiverServiceRequest(bool isOutward, bool isEarlier)
        {
            // Get the request and result to be replanned
            ISJPJourneyRequest sjpStopEventRequest = sessionHelper.GetSJPStopEventRequest();
            ISJPJourneyResult sjpStopEventResult = sessionHelper.GetSJPStopEventResult(sjpStopEventRequest.JourneyRequestHash);

            #region Set the replan values

            bool replanOutwardRequired = false;
            bool replanReturnRequired = false;
            DateTime replanOutwardDateTime = DateTime.MinValue;
            DateTime replanReturnDateTime = DateTime.MinValue;

            int earlierIntervalMins = Properties.Current["JourneyOptions.Replan.Earlier.River.Interval.Minutes"].Parse(120);
            int laterIntervalMins = Properties.Current["JourneyOptions.Replan.Later.River.Interval.Minutes"].Parse(1);

            bool retainPreviousJourneys = Properties.Current["JourneyOptions.Replan.RetainPreviousJourneys.River.Switch"].Parse(false);

            // Only replan the selected journey direction

            if (isOutward)
            {
                replanOutwardRequired = true;

                DateTime dt = GetJourneyDateTime(sjpStopEventResult.OutwardJourneys, isEarlier);

                // If Earlier, then date time is 2 hours before earliest "arrive time" in all the journeys
                if (isEarlier)
                {
                    replanOutwardDateTime = dt.Subtract(new TimeSpan(0, earlierIntervalMins, 0));
                }
                // If Later, then date time is 1 minute after latest "leave time" in all the journeys
                else
                {
                    replanOutwardDateTime = dt.Add(new TimeSpan(0, laterIntervalMins, 0));
                }
            }
            else
            {
                replanReturnRequired = true;

                DateTime dt = GetJourneyDateTime(sjpStopEventResult.ReturnJourneys, isEarlier);

                // If Earlier, then date time is 2 hours before earliest "arrive time" in all the journeys
                if (isEarlier)
                {
                    replanReturnDateTime = dt.Subtract(new TimeSpan(0, earlierIntervalMins, 0));
                }
                // If Later, then date time is 1 minute after latest "leave time" in all the journeys
                else
                {
                    replanReturnDateTime = dt.Add(new TimeSpan(0, laterIntervalMins, 0));
                }
            }

            #endregion
            
            JourneyLocationsAdapter journeyLocationsAdapter = new JourneyLocationsAdapter();

            // Build the stop event replan request
            bool valid = journeyLocationsAdapter.ValidateAndBuildSJPStopEventRequestForReplan(sjpStopEventRequest,
                replanOutwardRequired, replanReturnRequired,
                replanOutwardDateTime, replanReturnDateTime,
                (retainPreviousJourneys || !replanOutwardRequired) ? sjpStopEventResult.OutwardJourneys : null,
                (retainPreviousJourneys || !replanReturnRequired) ? sjpStopEventResult.ReturnJourneys : null);
            
            if (valid)
            {
                // Submit the stop event replan request
                JourneyPlannerHelper journeyPlannerHelper = new JourneyPlannerHelper();

                if (journeyPlannerHelper.SubmitStopEventRequest(true))
                {
                    // Reset wait count
                    waitCount.Value = "0";

                    // Display/Hide appropriate controls
                    waitTimer.Enabled = true;
                    riverServiceRoutes.Visible = false;
                    riverServiceResults.Visible = true;
                    journeyProgress.Visible = true;
                    riverServiceResultOutward.Visible = false;
                    riverServiceResultReturn.Visible = false;
                    riverServiceResultsHeading.Visible = false;
                    riverServiceRequestInProgress = true;
                }
            }
        }

        /// <summary>
        /// Returns the earliest/latest datetime for the journeys
        /// If isEarlier, then the Arrive times are searched and the earliest arrive time is returned
        /// Else, then the latest Leave times are searched and the latest leave time is returned
        /// </summary>
        /// <param name="journeys"></param>
        /// <param name="isEarlier"></param>
        /// <returns></returns>
        private DateTime GetJourneyDateTime(List<Journey> journeys, bool isEarlier)
        {
            DateTime dt = DateTime.MinValue;

            if (journeys != null)
            {
                if (isEarlier)
                {
                    journeys.Sort(JourneyComparer.SortJourneyArriveBy);

                    // Last journey should have the earliest arrive by datetime
                    dt = journeys[journeys.Count - 1].EndTime;
                }
                else
                {
                    journeys.Sort(JourneyComparer.SortJourneyLeaveAfter);

                    // Last journey should have the latest leave after datetime
                    dt = journeys[journeys.Count - 1].StartTime;
                }
            }

            return dt;
        }

        #region Stop Event Result methods

        /// <summary>
        /// Binds the StopEvent departure boad results
        /// </summary>
        /// <param name="stopEventRequest"></param>
        /// <param name="stopEventResult"></param>
        private void BindStopEventResult(ISJPJourneyRequest stopEventRequest, ISJPJourneyResult stopEventResult)
        {
            InputPageState pageState = SJPSessionManager.Current.PageState;
            
            riverServiceResultOutward.Initialise(stopEventRequest, stopEventResult, false, 0, pageState.ShowEarlierLinkOutwardRiver, pageState.ShowLaterLinkOutwardRiver, false);
            riverServiceResultOutward.Visible = stopEventResult.OutwardJourneys.Count > 0;

            riverServiceResultReturn.Initialise(stopEventRequest, stopEventResult, true, 0, pageState.ShowEarlierLinkReturnRiver, pageState.ShowLaterLinkReturnRiver, false);
            riverServiceResultReturn.Visible = stopEventResult.ReturnJourneys.Count > 0;

            riverServiceResultsHeading.Visible = (riverServiceResultOutward.Visible || riverServiceResultReturn.Visible);
        }

        /// <summary>
        /// Updates the count of page wait refresh
        /// </summary>
        private int UpdateWaitCount()
        {
            // Read count from hidden field
            int count = waitCount.Value.Parse(0);

            // Increment
            count = count + 1;

            // Persist in hidden field
            waitCount.Value = count.ToString();

            return count;
        }

        /// <summary>
        /// Initialises wait timer
        /// </summary>
        private void InitWaitTimer()
        {
            int refreshSecs = Properties.Current["RiverServicesJourneyLocations.Wait.RefreshTime.Seconds"].Parse(5);

            waitTimer.Interval = refreshSecs * 1000; // In millisecs

            waitTimer.Enabled = false;
        }

        #endregion

        #endregion
    }
}