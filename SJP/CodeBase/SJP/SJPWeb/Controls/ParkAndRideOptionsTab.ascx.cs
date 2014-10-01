// *********************************************** 
// NAME             : ParkAndRideOptionsTab      
// AUTHOR           : Amit Patel
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: ParkAndRideOptionsTab      
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    public partial class ParkAndRideOptionsTab : System.Web.UI.UserControl, IJourneyOptionsTab
    {
        #region Private Fields
        private bool disabled = false;
        private SJPLocation venue = null;
        private DateTime outwardDateTime;
        private DateTime returnDateTime;
        #endregion

        #region Events
        public event PlanJourney OnPlanJourney;
        #endregion

        #region Public Properties
        /// <summary>
        /// Read only property determining the planner mode represented by journey obtions tab
        /// </summary>
        public JourneyControl.SJPJourneyPlannerMode PlannerMode
        {
            get { return JourneyControl.SJPJourneyPlannerMode.ParkAndRide; }
        }

        /// <summary>
        /// Read/Write property if the tab is disabled
        /// </summary>
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }

        }

        /// <summary>
        /// Read/Write. SJP venue location
        /// </summary>
        public SJPLocation Venue
        {
            get { return venue; }
            set { venue = value; }
        }

        /// <summary>
        /// Read/Write outward date time selected for the journey
        /// </summary>
        public DateTime OutwardDateTime
        {
            get { return outwardDateTime; }
            set { outwardDateTime = value; }

        }

        /// <summary>
        /// Read/Write. Return date time selected for the journey
        /// </summary>
        public DateTime ReturnDateTime
        {
            get { return returnDateTime; }
            set { returnDateTime = value; }
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender
        /// <summary>
        /// Page_PreRender 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();
            SetupUpdateProgressPanel();
        }
        #endregion

        #region Control Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlanParkAndRideJourney(object sender, EventArgs e)
        {
            if (OnPlanJourney != null)
            {
                OnPlanJourney(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            SJPPage page = (SJPPage)Page;
            if (disabled)
            {
                parkAndRideOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.Disabled.ImageUrl");
                parkAndRideOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.Disabled.AlternateText");
                parkAndRideOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.Disabled.ToolTip");
            }
            else
            {
                parkAndRideOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.ImageUrl");
                parkAndRideOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.AlternateText");
                parkAndRideOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.ToolTip");
            }

            planParkAndRide.Text = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.PlanParkAndRide.Text"));
            planParkAndRide.ToolTip = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.PlanParkAndRide.ToolTip"));

            planParkAndRide.Visible = true;
            planParkAndRide.Enabled = !disabled && (venue != null);

            string URL_OpenInNewWindowImage = ResolveClientUrl(page.ImagePath + page.GetResource("OpenInNewWindow.Blue.URL"));
            string ALTTXT_OpenInNewWindow = page.GetResource("OpenInNewWindow.AlternateText");
            string TOOLTIP_OpenInNewWindow = page.GetResource("OpenInNewWindow.Text");

            string imgOpenInNewWindow = string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" />",
                URL_OpenInNewWindowImage, ALTTXT_OpenInNewWindow, TOOLTIP_OpenInNewWindow);

            if (disabled)
            {
                venueContent.InnerHtml = string.Format(
                    page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.Disable.Information"),
                    imgOpenInNewWindow);

                // Fully hide the submit button if the functionality is set to disabled
                planParkAndRide.Visible = false;
            }
            else if (venue != null)
            {
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                // Get car parks for the venue and its parent
                List<string> naptans = new List<string>(venue.Naptan);
                if (!string.IsNullOrEmpty(venue.Parent))
                {
                    naptans.Add(venue.Parent);
                }
                List<SJPVenueCarPark> carParkList = locationService.GetSJPVenueCarParks(naptans);

                if (!disabled)
                {
                    if ((carParkList != null) && (carParkList.Count > 0))
                    {
                        planParkAndRide.Enabled = true;
                        venueContent.InnerHtml = string.Format(
                            page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.Available.Information"), 
                            venue.DisplayName,
                            imgOpenInNewWindow);
                    }
                    else
                    {
                        planParkAndRide.Enabled = false;
                        venueContent.InnerHtml = string.Format(
                            page.GetResource("JourneyOptionTabContainer.ParkAndRideOptions.NotAvailable.Information"), 
                            venue.DisplayName,
                            imgOpenInNewWindow);
                    }
                }
            }
            else if (!disabled)
            {
                venueContent.InnerHtml = page.GetResource("JourneyOptionTabContainer.SelectVenue.Information");
            }            

            #region Set enabled/disabled style

            if (!planParkAndRide.Enabled)
            {
                if (!planParkAndRide.CssClass.Contains("btnDisabled"))
                {
                    planParkAndRide.CssClass = planParkAndRide.CssClass + " btnDisabled";
                }
            }
            else
            {
                if (planParkAndRide.CssClass.Contains("btnDisabled"))
                {
                    planParkAndRide.CssClass = planParkAndRide.CssClass.Replace(" btnDisabled", string.Empty);
                }
            }

            #endregion
        }

        /// <summary>
        /// Sets up update panel's progress panel
        /// </summary>
        private void SetupUpdateProgressPanel()
        {
            SJPPage page = (SJPPage)Page;
            Image loading = updateProgress.FindControlRecursive<Image>("loading");
            Label loadingMessage = updateProgress.FindControlRecursive<Label>("loadingMessage");

            if (loading != null)
            {
                loading.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.Loading.Imageurl");
                loading.AlternateText = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.Loading.AlternateText"));
            }

            if (loadingMessage != null)
            {
                loadingMessage.Text = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.loadingMessage.Text"));
            }
        }
        #endregion
    }
}