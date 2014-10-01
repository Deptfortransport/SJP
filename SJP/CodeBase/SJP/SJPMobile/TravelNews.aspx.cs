// *********************************************** 
// NAME             : TravelNews.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 08 Mar 2012
// DESCRIPTION  	: Travel news page
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using SJP.Common;
using SJP.Common.DataServices;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.Reporting.Events;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.TravelNews.SessionData;
using Logger = System.Diagnostics.Trace;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.TravelNews.TravelNewsData;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// Travel news page
    /// </summary>
    public partial class TravelNews : SJPPageMobile
    {
        #region Variables

        // Read from properties to control overall showing of news controls
        private bool showTravelNews = true;
        private bool showUndergroundStatus = true;

        private TravelNewsHelper tnHelper = null;
        private TravelNewsState currentNewsState = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TravelNews()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileTravelNews;
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

            currentNewsState = GetTravelNewsState();

            InitialiseControls();

            if (!Page.IsPostBack)
            {
                bool showTravelNewsDefault = SetupControls();

                // Display travel news or underground news
                SetupNewsControls(showTravelNewsDefault, !showTravelNewsDefault);
            }
            else
            {
                if (((SJPMobile)Master).PageScriptManager.IsInAsyncPostBack)
                {
                    logPageEntry = false;
                }
                UpdateNewsItems();
            }

            AddJavascript("News.js");

        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();

            // Save the update news state back in to session
            tnHelper.SetTravelNewsState(currentNewsState);

            SetupRefreshLink();

            if (((SJPMobile)Master).PageScriptManager.IsInAsyncPostBack)
            {
                // Log an event as a result of partial page update
                PageEntryEvent logPage = new PageEntryEvent(Common.PageId.MobileTravelNewsPartialUpdate, SJPSessionManager.Current.Session.SessionID, false);
                Logger.Write(logPage);
            }
        }

        #endregion
        
        #region Event handlers

        /// <summary>
        /// tnModeDrp_SelectedIndexChanged event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tnModeDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (newsModes.SelectedValue)
            {
                case TravelNewsHelper.NewsViewMode_LUL:
                    SetupNewsControls(false, true);
                    break;
                case TravelNewsHelper.NewsViewMode_Venue:
                    SetupNewsControls(true, false);
                    venueSelectControl.Populate(SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService), true);
                    break;
                default:
                    SetupNewsControls(true, false);
                    break;
            }

            newsModeHeading.InnerHtml = newsModes.SelectedItem.Text;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialises travel news controls
        /// </summary>
        private void InitialiseControls()
        {
            showTravelNews = Properties.Current["TravelNews.Enabled.Switch"].Parse(true);
            showUndergroundStatus = Properties.Current["UndergroundNews.Enabled.Switch"].Parse(true);

            if (!showTravelNews && !showUndergroundStatus)
            {
                updatePanel.Visible = false;

                DisplayMessage(new SJPMessage("TravelNews.lblUnavailable.Text", SJPMessageType.Error));
            }
            else
            {
                updatePanel.Visible = true;
            }
        }

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            switch (newsModes.SelectedValue)
            {
                case TravelNewsHelper.NewsViewMode_LUL:
                    providedByLbl.Text = GetResourceMobile("TravelNews.LondonUnderground.ProvidedBy.Text");
                    break;
                case TravelNewsHelper.NewsViewMode_Venue:
                default:
                    providedByLbl.Text = GetResourceMobile("TravelNews.ProvidedBy.Text");
                    break;
            }

            newsModeOptionsLegend.InnerText = GetResourceMobile("TravelNews.NewsModeOptionsLegend.Text");
            tnFilterBtnNonJS.Text = GetResourceMobile("TravelNews.FilterButtonNonJS.Text"); 

            waitControl.LoadingMessageLabel.Text = GetResourceMobile("TravelNews.LoadingMessage.Text");
        }

        /// <summary>
        /// Setup the controls on the page
        /// </summary>
        /// re
        private bool SetupControls()
        {
            bool showTravelNewsDefault = true;

            if (!IsPostBack)
            {
                // Populate the view news mode dropdown
                IDataServices dataServices = SJPServiceDiscovery.Current.Get<IDataServices>(ServiceDiscoveryKey.DataServices);

                dataServices.LoadListControl(DataServiceType.NewsViewMode, newsModes, Global.SJPResourceManager, CurrentLanguage.Value);
                newsModeHeading.InnerHtml = newsModes.SelectedItem.Text;

                // Populate the venues to select
                venueSelectControl.Populate(SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService), true);
            }

            // Default news view mode should be All travel news, 
            // but if travelnews switched off, and underground news on then set dropdown
            if (!showTravelNews && showUndergroundStatus)
            {
                try
                {
                    newsModes.SelectedIndex = newsModes.Items.IndexOf(newsModes.Items.FindByValue(TravelNewsHelper.NewsViewMode_LUL));
                    newsModeHeading.InnerHtml = newsModes.SelectedItem.Text;

                    showTravelNewsDefault = false;
                }
                catch
                {
                    // Ignore exceptions, option may have been removed in config
                }
            }

            // Populate the selected venue naptans field
            if (currentNewsState.SelectedVenuesFlag && currentNewsState.SearchNaptans.Count > 0)
            {
                StringBuilder naptans = new StringBuilder();
                foreach (string naptan in currentNewsState.SearchNaptans)
                {
                    if (naptans.Length > 0)
                        naptans.Append(",");

                    naptans.Append(naptan);
                }

                // Set the hidden field to allow javascript and next postback to detect which naptans are selected
                venueNaptans.Value = naptans.ToString();

                // Set the news view mode drop down to be venue
                try
                {
                    newsModes.SelectedIndex = newsModes.Items.IndexOf(newsModes.Items.FindByValue(TravelNewsHelper.NewsViewMode_Venue));
                    newsModeHeading.InnerHtml = newsModes.SelectedItem.Text;
                }
                catch
                {
                    // Ignore exceptions, option may have been removed in config
                }
            }

            // Check if query string indicates to show london underground lines
            if (!IsPostBack)
            {
                string newsMode = tnHelper.GetTravelNewsMode(true);

                // Only check for london undeground as default is travel news
                if (!string.IsNullOrEmpty(newsMode) && (newsMode.ToLower() == TravelNewsHelper.NewsViewMode_LUL.ToLower()))
                {
                    if (showUndergroundStatus)
                    {
                        try
                        {
                            newsModes.SelectedIndex = newsModes.Items.IndexOf(newsModes.Items.FindByValue(TravelNewsHelper.NewsViewMode_LUL));
                            newsModeHeading.InnerHtml = newsModes.SelectedItem.Text;

                            showTravelNewsDefault = false;
                        }
                        catch
                        {
                            // Ignore exceptions, option may have been removed in config
                        }
                    }
                }
            }

            return showTravelNewsDefault;
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
                PageTransferDetail ptd = GetPageTransferDetail(Common.PageId.MobileTravelNews);

                // Build the auto refresh url
                string refreshUrl = tnHelper.BuildTravelNewsUrl(ptd.PageUrl, currentNewsState, true, false, false, false, false, newsModes.SelectedValue);

                // Check if auto refresh is required for page by its existence in the request url,
                // and add to page
                if (tnHelper.GetTravelNewsAutoRefresh(true))
                {
                    int refreshSeconds = Properties.Current["TravelNews.AutoRefresh.Refresh.Seconds"].Parse(30);

                    // And add to the page for auto-refresh
                    this.AddAutoRefresh(refreshSeconds, refreshUrl);

                    // Set the refresh link to stop the auto-refresh
                    refreshUrl = tnHelper.BuildTravelNewsUrl(ptd.PageUrl, currentNewsState, false, false, false, false, false, newsModes.SelectedValue);

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
        /// Sets up the news controls on the page
        /// </summary>
        private void UpdateNewsItems()
        {
            Boolean showTravel = true;
            Boolean showUnderground = true;

            switch (newsModes.SelectedValue)
            {
                case TravelNewsHelper.NewsViewMode_LUL:
                    showTravel = false;
                    break;
                case TravelNewsHelper.NewsViewMode_Venue:
                    showUnderground = false;
                    break;
                default:
                    showUnderground = false;
                    break;
            }

            newsModeHeading.InnerHtml = newsModes.SelectedItem.Text;

            SetupNewsControls(showTravel, showUnderground);
        }

        /// <summary>
        /// Sets up the news controls on the page
        /// </summary>
        private void SetupNewsControls(bool showTravel, bool showUnderground)
        {
            // Update the travel news state
            UpdateTravelNewsState();

            travelNewsControl.Initialise(showTravel, !showTravelNews, currentNewsState);
            undergroundStatusControl.Initialise(showUnderground, !showUndergroundStatus);
        }
        
        /// <summary>
        /// Returns the current travel news state
        /// </summary>
        /// <returns></returns>
        private TravelNewsState GetTravelNewsState()
        {
            TravelNewsState tns = null;
            
            // Get travel news state from session
            if (!IsPostBack)
            {
                tns = tnHelper.GetTravelNewsStateForMobile(true);
            }
            else
            {
                tns = tnHelper.GetTravelNewsStateForMobile(false);
            }

            return tns;
        }

        /// <summary>
        /// Udpates the travel news state with user parameters
        /// </summary>
        /// <returns></returns>
        private void UpdateTravelNewsState()
        {
            #region Venue naptans

            // Venue naptans
            selectedVenue.Text = string.Empty;

            try
            {
                // Only set search naptans if news view mode is for venue
                if (newsModes.SelectedValue == TravelNewsHelper.NewsViewMode_Venue)
                {
                    LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
                    List<SJPLocation> venues = locationService.GetSJPVenueLocations();

                    // Specific venue(s) have been selected
                    if (!string.IsNullOrEmpty(venueNaptans.Value.Trim()))
                    {
                        currentNewsState.SearchNaptans = new List<string>(venueNaptans.Value.Split(','));
                        currentNewsState.SelectedVenuesFlag = true;

                        // Simple check for all venues, not accurate but should be ok
                        if (currentNewsState.SelectedAllVenuesFlag &&
                            tnHelper.GetVenueNaptans().Count == currentNewsState.SearchNaptans.Count)
                        {
                            currentNewsState.SelectedAllVenuesFlag = true;
                            selectedVenue.Text = GetResourceMobile("TravelNews.DisplayedFor.AllVenues");
                        }
                        else
                        {
                            currentNewsState.SelectedAllVenuesFlag = false;

                            LocationHelper locationHelper = new LocationHelper();
                            selectedVenue.Text = string.Format(GetResourceMobile("TravelNews.DisplayedFor.Venues"),
                                locationHelper.GetLocationNames(currentNewsState.SearchNaptans));
                        }
                    }
                    // No venues selected, so filter for all
                    else
                    {
                        currentNewsState.SearchNaptans = tnHelper.GetVenueNaptans();
                        currentNewsState.SelectedAllVenuesFlag = true;
                        currentNewsState.SelectedVenuesFlag = true;

                        selectedVenue.Text = GetResourceMobile("TravelNews.DisplayedFor.AllVenues");
                    }
                }
                else
                {
                    // Reset values which may have been set by page landing
                    currentNewsState.SearchNaptans = new List<string>();
                    currentNewsState.SelectedAllVenuesFlag = false;
                    currentNewsState.SelectedVenuesFlag = false;
                    currentNewsState.SelectedRegion = TravelNewsRegion.All.ToString();
                }
            }
            catch
            {
                // Ignore any exceptions
            }

            #endregion
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPMobile)this.Master).DisplayMessage(sjpMessage);
        }

        #endregion
    }
}
