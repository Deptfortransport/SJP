// *********************************************** 
// NAME             : TravelNews.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 15 Apr 2011
// DESCRIPTION  	: TravelNews page displaying travel news incidents
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.DataServices;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.Reporting.Events;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.SJPWeb.Controls;
using SJP.UserPortal.TravelNews;
using SJP.UserPortal.TravelNews.SessionData;
using SJP.UserPortal.TravelNews.TravelNewsData;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// TravelNews page displaying travel news incidents
    /// </summary>
    public partial class TravelNews : SJPPage
    {
        #region Private members
        private const string DATEFORMAT = "dd/MM/yyyy";
        private DateTime selectedDate = DateTime.Now;

        private TravelNewsState currentNewsState = null;

        private TravelNewsHelper tnHelper = null;

        private bool updateRequired = true;

        // Severity Level group to be expanded
        private SeverityLevel severityLevelToExpand = SeverityLevel.Critical;
        private SeverityLevel selectedSeverityLevelToExpand = SeverityLevel.Critical;
        private bool useSelectedSeverity = false;

        // For non-js expand news button clicks
        private bool showOlympicTn = true;
        private bool showOtherTn = false;
        
        private string venueDropDownDefaultItem = string.Empty;
        LocationService locationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TravelNews()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.TravelNews;
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

        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            tnHelper = new TravelNewsHelper();

            locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            SetupResources();

            InitialiseControls();

            #region Get current travel news state

            if (!IsPostBack)
            {
                // Populate filter according to the travel news state returned from the travel news session adapter
                currentNewsState = tnHelper.GetTravelNewsState(true);

                PopulateFilter();
            }
            else
            {
                currentNewsState = tnHelper.GetTravelNewsState(false);

                if (((SJPWeb)Master).PageScriptManager.IsInAsyncPostBack)
                {
                    logPageEntry = false;
                }
            }

            #endregion

            // Sync the interactive all uk map links to the correct travel news region
            if (Request.QueryString.AllKeys.Contains("SelectedRegion"))
            {
                byte selectedRegionId = Request.QueryString["SelectedRegion"].Parse<byte>(0);
                currentNewsState.SelectedRegion = ((TravelNewsRegion)selectedRegionId).ToString();
            }

            // Add jquery ui css
            AddStyleSheet("jquery-ui-1.8.13.css");

            // Add javascripts specific for this page
            AddJavascript("common.js");
            AddJavascript("jquery-ui-1.8.13.min.js");
            AddJavascript("TravelNews.js");
            AddJavascript("jquery.ui.selectmenu.js");
            AddJavascript("jquery.dateentry.min.js");
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Ensure region dropdown and map are same as the currentlyNewsState
            regionDrop.SelectedIndex = regionDrop.Items.IndexOf(regionDrop.Items.FindByValue(currentNewsState.SelectedRegion));

            if (currentNewsState.SelectedRegion.ToLower().Trim() != "all")
                ukImageMap.SelectedRegionId = regionDrop.SelectedIndex.ToString();

            BindTravelNewsData();

            // Save the update news state back in to session
            tnHelper.SetTravelNewsState(currentNewsState);

            SetupRefreshLink();
            
            if (((SJPWeb)Master).PageScriptManager.IsInAsyncPostBack)
            {
                // Log an event as a result of partial page update
                PageEntryEvent logPage = new PageEntryEvent(Common.PageId.TravelNewsPartialUpdate, SJPSessionManager.Current.Session.SessionID, false);
                Logger.Write(logPage);
            }
        }

        #endregion

        #region Controls Event Handlers

        #region Populate travel news repeater(s)

        /// <summary>
        /// Travel news repeater ItemDataBound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TravelNews_DataBound(object sender, RepeaterItemEventArgs e)
        {

            SJPPage page = (SJPPage)Page;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                KeyValuePair<SeverityLevel, List<TravelNewsItem>> keyValue = (KeyValuePair<SeverityLevel, List<TravelNewsItem>>)e.Item.DataItem;
                ImageButton severityButton = e.Item.FindControlRecursive<ImageButton>("severityButton");

                Label severityHeading = e.Item.FindControlRecursive<Label>("severityHeading");

                severityHeading.Text = page.GetResource(string.Format("TravelNews.SeverityHeading.{0}.Text", keyValue.Key));
                
                HtmlGenericControl tnContainer = e.Item.FindControlRecursive<HtmlGenericControl>("tnContainer");
                HtmlGenericControl tnHeading = e.Item.FindControlRecursive<HtmlGenericControl>("tnHeading");

                HiddenField tnSeverity = e.Item.FindControlRecursive<HiddenField>("tnSeverity");

                tnSeverity.Value = keyValue.Key.ToString();

                severityButton.CommandArgument = keyValue.Key.ToString();
                severityButton.ImageUrl = ImagePath + GetResource("TravelNews.SeverityButton.collapsed.ImageUrl");
                severityButton.AlternateText = GetResource("TravelNews.SeverityButton.AlternateText");
                severityButton.ToolTip = GetResource("TravelNews.SeverityButton.ToolTip");

                if (keyValue.Key == severityLevelToExpand)
                {
                    if (tnContainer.Attributes["class"].Contains("collapsed"))
                    {
                        tnContainer.Attributes["class"] = tnContainer.Attributes["class"].Replace("collapsed", "expanded");
                        tnHeading.Attributes["class"] += " active";

                        // Enure non-js button is set accordingly
                        severityButton.ImageUrl = ImagePath + GetResource("TravelNews.SeverityButton.expanded.ImageUrl");
                        otherSeverityButton.ImageUrl = ImagePath + GetResource("TravelNews.SeverityButton.expanded.ImageUrl"); 
                    }
                }
                else
                {
                    tnContainer.Attributes["class"] = tnContainer.Attributes["class"].Replace("expanded", "collapsed");
                    tnHeading.Attributes["class"] = tnHeading.Attributes["class"].Replace("active", "");
                }

                TravelNewsItemControl travelNewsItemControl = e.Item.FindControlRecursive<TravelNewsItemControl>("travelnewsItems");
                travelNewsItemControl.Bind(keyValue.Value, this);

                if (!string.IsNullOrEmpty(currentNewsState.SelectedIncident) && (keyValue.Value.Where(item => item.Uid == currentNewsState.SelectedIncident).Count() > 0))
                {
                    // add a link reference to scrollpage to selected news story
                    AnchorLink = currentNewsState.SelectedIncident;
                }
 
            }
        }

        #endregion

        /// <summary>
        /// ImageButton_Command event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName.Equals("ShowSeverity"))
            {
                try
                {
                    selectedSeverityLevelToExpand = (SeverityLevel)Enum.Parse(typeof(SeverityLevel), e.CommandArgument.ToString(), true);
                    
                    useSelectedSeverity = true;
                    showOtherTn = true;
                    showOlympicTn = false;
                }
                catch (Exception ex)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    "TravelNews: Unable to parse the selected severity heading argument" + ex.Message));
                }
            }
            else if (e.CommandName.Equals("ShowOtherTN"))
            {
                showOtherTn = true;
                showOlympicTn = false;
            }
            else if (e.CommandName.Equals("ShowOlympicTN"))
            {
                showOlympicTn = true;
                showOtherTn = false;
            }

            // Update olympic expanded status (other news is done in the Bind method)
            if (!showOlympicTn && showOtherTn)
            {
                if (olympicImpactContainer.Attributes["class"].Contains("expanded"))
                {
                    olympicImpactContainer.Attributes["class"] = olympicImpactContainer.Attributes["class"].Replace("expanded", "collapsed");
                }
            }
            else
            {
                if (olympicImpactContainer.Attributes["class"].Contains("collapsed"))
                {
                    olympicImpactContainer.Attributes["class"] = olympicImpactContainer.Attributes["class"].Replace("collapsed", "expanded");
                }
            }
        }

        #region Filter change events

        /// <summary>
        /// Handler for travel news region change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RegionChange(object sender, EventArgs e)
        {
            updateRequired = false;

            regionDrop.SelectedIndex = int.Parse(ukImageMap.SelectedRegionId);
            currentNewsState.SelectedRegion = regionDrop.SelectedValue;
        }

        /// <summary>
        /// Handler for travel news region drop down change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RegionDropChange(object sender, EventArgs e)
        {
            updateRequired = false;

            ukImageMap.SelectedRegionId = regionDrop.SelectedIndex.ToString();
            currentNewsState.SelectedRegion = regionDrop.SelectedValue;
        }
        
        /// <summary>
        /// Outward date input box text change event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TNDate_Changed(object sender, EventArgs e)
        {
            updateRequired = false;

            DateTime travelDate = DateTime.MinValue;

            if (DateTime.TryParse(tnDate.Text.Trim(), out travelDate))
            {
                currentNewsState.SelectedDate = travelDate;
            }
        }

        #endregion

        /// <summary>
        /// 'Apply Filter' button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FilterNews_Click(object sender, EventArgs e)
        {
            UpdateTravelNewsState();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initialises travel news controls
        /// </summary>
        private void InitialiseControls()
        {
            InitCalendar();

            InitImageMap();

            if (!IsPostBack)
            {
                // load regions drop down
                IDataServices dataServices = SJPServiceDiscovery.Current.Get<IDataServices>(ServiceDiscoveryKey.DataServices);

                dataServices.LoadListControl(DataServiceType.NewsRegionDrop, regionDrop, Global.SJPResourceManager, CurrentLanguage.Value);

                PopulateVenueDropDown();
            }

            #region Check if Travel news is available

            if (!Properties.Current["TravelNews.Enabled.Switch"].Parse(true))
            {
                updatePanel.Visible = false;

                DisplayMessage(new SJPMessage("TravelNews.lblUnavailable.Text", SJPMessageType.Error));
            }
            // Check travel news service has items to display
            else if (!tnHelper.TravelNewsAvailable())
            {
                updatePanel.Visible = false;

                DisplayMessage(new SJPMessage(tnHelper.TravelNewsUnavailableText(), string.Empty, 0, 0, SJPMessageType.Error));
            }
            else
            {
                updatePanel.Visible = true;
            }

            #endregion
        }

        /// <summary>
        /// Sets up caledar control default dates
        /// </summary>
        private void InitCalendar()
        {
            #region Set calendar dates

            // Set the start to end dates to display for the calendar. This will be the period for which
            // journeys can be planned for the games
            DateTime startDate = Properties.Current["JourneyPlanner.Validate.Games.StartDate"].Parse(DateTime.Now.Date);
            DateTime endDate = Properties.Current["JourneyPlanner.Validate.Games.EndDate"].Parse(DateTime.Now.Date);

            if (startDate < DateTime.Now.Date)
            {
                startDate = DateTime.Now.Date;
            }

            calendarEndDate.Value = endDate.ToString(DATEFORMAT);
            calendarStartDate.Value = startDate.ToString(DATEFORMAT);

            #endregion
        }

        /// <summary>
        /// Initialises interactive image map
        /// </summary>
        private void InitImageMap()
        {
            ukImageMap.InitialiseFromProperties("UKRegionImageMap", Global.SJPResourceManager);
        }

        /// <summary>
        /// Sets up travel news resource contents
        /// </summary>
        private void SetupResources()
        {
            sjpHeading.InnerText = GetResource(string.Format("{0}.Heading.Text", pageId.ToString()));
            tnFilterHeading.Text = GetResource("TravelNews.TNFilterHeading.Text");
            lblRegion.Text = GetResource("TravelNews.LblRegion.Text");
            lblInclude.Text = GetResource("TravelNews.LblInclude.Text");
            publicTransportNews.Text = GetResource("TravelNews.PublicTransportNews.Text");
            roadNews.Text = GetResource("TravelNews.RoadNews.Text");
            lblTNPhrase.Text = GetResource("TravelNews.LblTNPhrase.Text");
            lblTNDate.Text = GetResource("TravelNews.LblTNDate.Text");
            lblNoIncidents.Text = GetResource("TravelNews.lblNoIncidents.Text");
            lblFilterNews.Text = GetResource("TravelNews.lblFilterNews.Text");

            filterNews.Text = GetResource("TravelNews.FilterNews.Text");
            filterNews.ToolTip = GetResource("TravelNews.FilterNews.ToolTip");

            loading.ImageUrl = ImagePath + GetResource("TravelNews.Loading.Imageurl");
            loading.ToolTip = loading.AlternateText = Server.HtmlDecode(GetResource("TravelNews.Loading.AlternateText"));

            loadingMessage.Text = Server.HtmlDecode(GetResource("TravelNews.loadingMessage.Text"));

            longWaitMessage.Text = Server.HtmlDecode(GetResource("TravelNews.LongWaitMessage.Text"));
            longWaitMessageLink.Text = Server.HtmlDecode(GetResource("TravelNews.LongWaitMessageLink.Text"));
            longWaitMessageLink.ToolTip = Server.HtmlDecode(GetResource("TravelNews.LongWaitMessageLink.ToolTip"));

            // Calendar Resources
            calendar_ButtonText.Value = GetResource("Calendar.ButtonText");
            calendar_NextText.Value = GetResource("Calendar.NextText");
            calendar_PrevText.Value = GetResource("Calendar.PrevText");
            calendar_DayNames.Value = GetResource("Calendar.DayNames");
            calendar_MonthNames.Value = GetResource("Calendar.MonthNames");

            venueDropDownDefaultItem = GetResource("TravelNews.VenueDropdown.DefaultItem.Text");
            lblVenue.Text = GetResource("TravelNews.lblVenue.Text");

            olympicImpactHeading.Text = GetResource("TravelNews.OlympicTravelNewsHeader.Text");
            otherImpactHeading.Text = GetResource("TravelNews.OtherTravelNewsHeader.Text");

            olympicSeverityButton.ImageUrl = ImagePath + GetResource("TravelNews.SeverityButton.collapsed.ImageUrl");
            olympicSeverityButton.AlternateText = GetResource("TravelNews.SeverityButton.AlternateText");
            olympicSeverityButton.ToolTip = GetResource("TravelNews.SeverityButton.ToolTip");

            otherSeverityButton.ImageUrl = ImagePath + GetResource("TravelNews.SeverityButton.collapsed.ImageUrl");
            otherSeverityButton.AlternateText = GetResource("TravelNews.SeverityButton.AlternateText");
            otherSeverityButton.ToolTip = GetResource("TravelNews.SeverityButton.ToolTip");
        }

        /// <summary>
        /// Sets up the auto-refresh link displayed on the page
        /// </summary>
        private void SetupRefreshLink()
        {
            if (Properties.Current["TravelNews.AutoRefresh.Enabled.Switch"].Parse(false))
            {
                refreshLinkDiv.Visible = Properties.Current["TravelNews.AutoRefresh.ShowRefreshLink.Switch"].Parse(false)
                    || DebugHelper.ShowDebug;

                // Build refresh url based on current filter selection, which should be reflected by the news state
                PageTransferDetail ptd = GetPageTransferDetail(Common.PageId.TravelNews);

                // Build the auto refresh url
                string refreshUrl = tnHelper.BuildTravelNewsUrl(ptd.PageUrl, currentNewsState, true, true, true, true, true, string.Empty);

                // Check if auto refresh is required for page by its existence in the request url,
                // and add to page
                if (tnHelper.GetTravelNewsAutoRefresh(true))
                {
                    int refreshSeconds = Properties.Current["TravelNews.AutoRefresh.Refresh.Seconds"].Parse(30);

                    // And add to the page for auto-refresh
                    this.AddAutoRefresh(refreshSeconds, refreshUrl);

                    // Set the refresh link to stop the auto-refresh
                    refreshUrl = tnHelper.BuildTravelNewsUrl(ptd.PageUrl, currentNewsState, false, true, true, true, true, string.Empty);
                    refreshLink.NavigateUrl = refreshUrl;
                    refreshLink.Text = GetResource("TravelNews.AutoRefreshLink.Stop.Text");
                    refreshLink.ToolTip = GetResource("TravelNews.AutoRefreshLink.Stop.ToolTip");
                }
                else
                {
                    // Auto-refresh currently not started
                    // Set the refresh link to start the auto-refresh
                    refreshLink.NavigateUrl = refreshUrl;
                    refreshLink.Text = GetResource("TravelNews.AutoRefreshLink.Start.Text");
                    refreshLink.ToolTip = GetResource("TravelNews.AutoRefreshLink.Start.ToolTip");
                }
            }
            else
            {
                // Hide the refresh link
                refreshLinkDiv.Visible = false;
            }
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);
        }

        /// <summary>
        /// Populates the travel news filter parameters using current travel news state
        /// </summary>
        private void PopulateFilter()
        {
            // Set the region
            regionDrop.SelectedIndex = regionDrop.Items.IndexOf(regionDrop.Items.FindByValue(currentNewsState.SelectedRegion));

            // Set the mode of transport
            publicTransportNews.Checked = (currentNewsState.SelectedTransport == TransportType.PublicTransport
                || currentNewsState.SelectedTransport == TransportType.All);

            roadNews.Checked = (currentNewsState.SelectedTransport == TransportType.Road
                || currentNewsState.SelectedTransport == TransportType.All);

            // Set travel news search phrase
            tnPhrase.Text = currentNewsState.SearchPhrase;

            // Set the travel news date
            tnDate.Text = currentNewsState.SelectedDate.ToString(DATEFORMAT);

            // Set the venues selected flag
            useVenues.Checked = currentNewsState.SelectedVenuesFlag;

            // Set the Venues drop down
            if (currentNewsState.SearchNaptans.Count > 0)
            {
                if (currentNewsState.SearchNaptans.Count == 1)
                {
                    venueDropdown.SelectedValue = currentNewsState.SearchNaptans.First();
                }
                else
                {
                    venueDropdown.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Gets the travel news state stored in travel news page state and updates it
        /// </summary>
        private void UpdateTravelNewsState()
        {
            // Selected Date
            DateTime selectedDate = DateTime.MinValue;

            if (DateTime.TryParse(tnDate.Text, out selectedDate))
            {
                currentNewsState.SelectedDate = selectedDate;
            }

            // Selected Region
            currentNewsState.SelectedRegion = regionDrop.SelectedValue;

            // Mode of Transport
            if (publicTransportNews.Checked && roadNews.Checked)
            {
                currentNewsState.SelectedTransport = TransportType.All;
            }
            else if (publicTransportNews.Checked)
            {
                currentNewsState.SelectedTransport = TransportType.PublicTransport;
            }
            else if (roadNews.Checked)
            {
                currentNewsState.SelectedTransport = TransportType.Road;
            }
            else
            {
                // Neither check box selected, force both as selected
                currentNewsState.SelectedTransport = TransportType.All;
                publicTransportNews.Checked = true;
                roadNews.Checked = true;
            }

            // Search Phrase
            currentNewsState.SearchPhrase = tnPhrase.Text.Trim();

            // Search Naptans flag
            currentNewsState.SelectedVenuesFlag = useVenues.Checked;

            // Search Naptans
            currentNewsState.SearchNaptans = new List<string>();
            currentNewsState.SelectedAllVenuesFlag = false;

            if (venueDropdown.SelectedItem.Text != venueDropDownDefaultItem)
            {
                currentNewsState.SearchNaptans.Add(venueDropdown.SelectedItem.Value);
                currentNewsState.SelectedAllVenuesFlag = false;
            }
            else
            {
                currentNewsState.SearchNaptans.AddRange(tnHelper.GetVenueNaptans());
                currentNewsState.SelectedAllVenuesFlag = true;
            }

        }

        /// <summary>
        /// Sets the affected aread of travel news story
        /// </summary>
        /// <param name="newsItem">Travel News story</param>
        /// <param name="affectedLocation"></param>
        /// <param name="affectedlocationText"></param>
        private void SetupAffectedLocation(TravelNewsItem newsItem, Image affectedLocation)
        {
            TravelNewsItem tnItem = newsItem;

            string image = string.Empty;

            if (newsItem.ModeOfTransport.ToLower().Trim() == "road")
            {
                if (!string.IsNullOrEmpty(newsItem.RoadType.ToString()))
                {
                    string roadType = newsItem.RoadType.Trim().Replace(" ", "");
                    image = GetResource(string.Format("TravelNews.AffectedLocation.Road.{0}.Url",
                        roadType));

                    affectedLocation.AlternateText = GetResource(string.Format("TravelNews.AffectedLocation.Road.{0}.AlternateText",
                       roadType));
                    affectedLocation.ToolTip = GetResource(string.Format("TravelNews.AffectedLocation.Road.{0}.AlternateText",
                       roadType));
                }
            }
            else
            {
                string modeOfTransport = newsItem.ModeOfTransport.Replace(" ", "");

                image = GetResource(string.Format("TravelNews.AffectedLocation.{0}.Url",
                    modeOfTransport));

                if (string.IsNullOrEmpty(image))
                {
                    image = GetResource("TravelNews.AffectedLocation.PublicTransport.Url");
                }

                string affectedLocationAltText = GetResource(string.Format("TravelNews.AffectedLocation.{0}.AlternateText",
                   modeOfTransport));

                if (string.IsNullOrEmpty(affectedLocationAltText))
                {
                    affectedLocationAltText = GetResource("TravelNews.AffectedLocation.PublicTransport.AlternateText");
                }
                affectedLocation.AlternateText = affectedLocationAltText;
                affectedLocation.ToolTip = affectedLocationAltText;
            }

            if (string.IsNullOrEmpty(image))
            {
                affectedLocation.Visible = false;
            }
            else
            {
                affectedLocation.ImageUrl = ImagePath + image;
                affectedLocation.Visible = true;
            }
        }

        /// <summary>
        /// Gets the travel news data using current travel news state and binds it to travel news repeater
        /// </summary>
        private void BindTravelNewsData()
        {
            if (updateRequired)
            {
                // Set the no incidents label to true then turn it off if incidents exist
                lblNoIncidents.Visible = true;

                // Get Travel news for the currentNewsState
                ITravelNewsHandler travelNewsHandler = SJPServiceDiscovery.Current.Get<ITravelNewsHandler>(ServiceDiscoveryKey.TravelNews);

                #region Olympic travel news

                // Olympic News items
                TravelNewsItem[] olympicNewsItems = travelNewsHandler.GetDetailsForWeb(currentNewsState, true);
                if ((olympicNewsItems != null) && (olympicNewsItems.Length > 0))
                {
                    olympicImpactTravelNewsItems.Bind(olympicNewsItems, this);
                    tnOlympicTravelNews.Visible = true;
                    lblNoIncidents.Visible = false;
                }
                else
                {
                    tnOlympicTravelNews.Visible = false;
                }
                
                #endregion

                #region Other travel news

                // Other news items
                Dictionary<SeverityLevel, List<TravelNewsItem>> travelNewsItems = travelNewsHandler.GetDetailsForWebGroupedBySeverity(currentNewsState, false);
                if ((travelNewsItems != null) && (travelNewsItems.Count > 0))
                {
                    // Olympic travel news always expanded by default, 
                    // so only set expanded for other news items when olympic news not found

                    // If incident selected, expand that severity
                    if (!IsPostBack
                        && !string.IsNullOrEmpty(currentNewsState.SelectedIncident)
                        && !Request.QueryString.AllKeys.Contains("SelectedRegion"))
                    {
                        TravelNewsItem selectedItem = travelNewsHandler.GetDetailsByUid(currentNewsState.SelectedIncident);
                        severityLevelToExpand = selectedItem.SeverityLevel;
                    }
                    else if (olympicNewsItems == null || olympicNewsItems.Length == 0)
                    {
                        // Otherwise expand first item severity (should be the highest sev as items should already be sorted
                        severityLevelToExpand = travelNewsItems.Keys.First();

                        if (useSelectedSeverity)
                            severityLevelToExpand = selectedSeverityLevelToExpand;
                    }
                    // For non-js button click
                    else if (!showOlympicTn && showOtherTn)
                    {
                        // Otherwise expand first item severity (should be the highest sev as items should already be sorted
                        severityLevelToExpand = travelNewsItems.Keys.First();

                        if (useSelectedSeverity)
                            severityLevelToExpand = selectedSeverityLevelToExpand;
                    }

                    lblNoIncidents.Visible = false;
                    tnOtherTravelNews.Visible = true;

                    travelNewsRepeater.DataSource = travelNewsItems;
                    travelNewsRepeater.DataBind();
                }
                else
                {
                    tnOtherTravelNews.Visible = false;
                }

                #endregion

                lblFilterNews.Visible = false;

                if (DebugHelper.ShowDebug)
                {
                    // For debug
                    int dbgOlympicActive = 0;
                    int dbgOlympicInActive = 0;
                    int dbgOtherActive = 0;
                    int dbgOtherInActive = 0;

                    if ((olympicNewsItems != null) && (olympicNewsItems.Length > 0))
                    {
                        List<TravelNewsItem> liOlympicNewsItems = new List<TravelNewsItem>(olympicNewsItems);

                        dbgOlympicActive = liOlympicNewsItems.Count(delegate(TravelNewsItem tni) { return tni.IncidentActiveStatus == IncidentActiveStatus.Active; });
                        dbgOlympicInActive = liOlympicNewsItems.Count(delegate(TravelNewsItem tni) { return tni.IncidentActiveStatus == IncidentActiveStatus.Inactive; });
                    }

                    if ((travelNewsItems != null) && (travelNewsItems.Count > 0))
                    {
                        foreach (KeyValuePair<SeverityLevel, List<TravelNewsItem>> kvp in travelNewsItems)
                        {
                            dbgOtherActive += kvp.Value.Count(delegate(TravelNewsItem tni) { return tni.IncidentActiveStatus == IncidentActiveStatus.Active; });
                            dbgOtherInActive += kvp.Value.Count(delegate(TravelNewsItem tni) { return tni.IncidentActiveStatus == IncidentActiveStatus.Inactive; });
                        }
                    }

                    lblDebug.Visible = true;

                    lblDebug.Text = string.Format("Olympic[Active:{0}/Inactive:{1}] Other[Active:{2}/Inactive:{3}]. <br />Filter Date[{4}] Transport[{5}] Region[{6}] Active[{7}]",
                        dbgOlympicActive,
                        dbgOlympicInActive,
                        dbgOtherActive,
                        dbgOtherInActive,
                        currentNewsState.SelectedDate,
                        currentNewsState.SelectedTransport,
                        currentNewsState.SelectedRegion,
                        currentNewsState.SelectedIncidentActive);
                }
            }
            else
            {
                // Display apply filter message and hide the news
                lblNoIncidents.Visible = false;
                lblFilterNews.Visible = true;
                tnOlympicTravelNews.Visible = false;
                tnOtherTravelNews.Visible = false;
            }
        }

        /// <summary>
        /// Populates the Venue dropdown
        /// </summary>
        private void PopulateVenueDropDown()
        {
            List<SJPLocation> venues = locationService.GetSJPVenueLocations();

            venueDropdown.Items.Clear();

            if (Properties.Current["LocationControl.VenueGrouping.Switch"].Parse(true))
            {
                Dictionary<string, List<SJPVenueLocation>> groupedVenues = new Dictionary<string, List<SJPVenueLocation>>();

                // To group items for which there is no group specified.
                // These items will be shown last in the list
                string defaultGroupName = "NOGROUP";

                groupedVenues.Add(defaultGroupName, new List<SJPVenueLocation>());

                foreach (SJPLocation loc in venues)
                {
                    SJPVenueLocation venue = (SJPVenueLocation)loc;

                    if (string.IsNullOrEmpty(venue.VenueGroupName) && !string.IsNullOrEmpty(venue.Parent))
                    {
                        // If the group name is empty but parent is not empty get the group name from the parent

                        SJPLocation parentVenue = venues.SingleOrDefault(v => v.ID == venue.Parent);

                        if (parentVenue != null)
                        {
                            SJPVenueLocation pvl = (SJPVenueLocation)parentVenue;
                            if (!groupedVenues.Keys.Contains(pvl.VenueGroupName))
                            {
                                groupedVenues.Add(pvl.VenueGroupName, new List<SJPVenueLocation>());
                            }
                            groupedVenues[pvl.VenueGroupName].Add(venue);
                        }
                        else // no suitable parent found add it as no group
                        {
                            groupedVenues[defaultGroupName].Add(venue);
                        }
                    }
                    else if (!string.IsNullOrEmpty(venue.VenueGroupName))
                    {
                        // group name specified add it will group name
                        if (!groupedVenues.Keys.Contains(venue.VenueGroupName))
                        {
                            groupedVenues.Add(venue.VenueGroupName, new List<SJPVenueLocation>());
                        }
                        groupedVenues[venue.VenueGroupName].Add(venue);
                    }
                    else // No grouping specified add it as no group
                    {
                        groupedVenues[defaultGroupName].Add(venue);
                    }
                }

                foreach (string venueGroupName in groupedVenues.Keys)
                {
                    foreach (SJPVenueLocation vl in groupedVenues[venueGroupName].OrderBy(v => v.DisplayName))
                    {
                        ListItem vItem = new ListItem();
                        vItem.Text = vl.DisplayName;
                        vItem.Value = vl.ID;

                        if (venueGroupName != defaultGroupName)
                            vItem.Attributes.Add("OptionGroup", venueGroupName);

                        venueDropdown.Items.Add(vItem);
                    }
                }
            }
            else
            {
                venueDropdown.DataSource = venues;
                venueDropdown.DataTextField = "DisplayName";
                venueDropdown.DataValueField = "Id";
                venueDropdown.DataBind();
            }

            venueDropdown.Items.Insert(0, new ListItem(venueDropDownDefaultItem));
        }

        #endregion
    }
}