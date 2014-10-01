// *********************************************** 
// NAME             : CycleOptionsTab.ascx      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: CycleOptionsTab.ascx
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
    /// <summary>
    /// CycleOptionsTab.ascx
    /// </summary>
    public partial class CycleOptionsTab : System.Web.UI.UserControl, IJourneyOptionsTab
    {
        #region Private Fields

        private bool disabled = false;
        private bool isOriginVenue = false;
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
            get { return JourneyControl.SJPJourneyPlannerMode.Cycle; }
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
        /// Read/Write. Is the Venue the origin venue (false be default)
        /// </summary>
        public bool IsOriginVenue
        {
            get { return isOriginVenue; }
            set { isOriginVenue = value; }
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

        #region Private Methods
        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            SJPPage page = (SJPPage)Page;
            if (disabled)
            {
                cycleOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.CycleOptions.Disabled.ImageUrl");
                cycleOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.CycleOptions.Disabled.AlternateText");
                cycleOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.CycleOptions.Disabled.ToolTip");
            }
            else
            {
                cycleOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.CycleOptions.ImageUrl");
                cycleOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.CycleOptions.AlternateText");
                cycleOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.CycleOptions.ToolTip");
            }

            planCycle.Text = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.CycleOptions.PlanCycle.Text"));
            planCycle.ToolTip = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.CycleOptions.PlanCycle.ToolTip"));

            planCycle.Visible = true;
            planCycle.Enabled = !disabled && (venue != null);

            if (disabled)
            {
                venueContent.InnerHtml = page.GetResource("JourneyOptionTabContainer.CycleOptions.Disable.Information");

                // Fully hide the submit button if the functionality is set to disabled
                planCycle.Visible = false;
            }

            if (venue != null)
            {
                
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                // Get cycle parks for the venue and its parent
                List<string> naptans = new List<string>(venue.Naptan);
                if (!string.IsNullOrEmpty(venue.Parent))
                {
                    naptans.Add(venue.Parent);
                }
                List<SJPVenueCyclePark> cycleParkList = locationService.GetSJPVenueCycleParks(naptans);

                if (!disabled)
                {
                    if (cycleParkList != null)
                    {
                        planCycle.Enabled = cycleParkList.Count > 0;
                        venueContent.InnerHtml = string.Format(
                            isOriginVenue ? page.GetResource("JourneyOptionTabContainer.CycleOptions.Available.From.Information") :
                                            page.GetResource("JourneyOptionTabContainer.CycleOptions.Available.To.Information"), 
                            venue.DisplayName);
                    }
                    else
                    {
                        planCycle.Enabled = false;
                        venueContent.InnerHtml = string.Format(page.GetResource("JourneyOptionTabContainer.CycleOptions.NotAvailable.Information"), venue.DisplayName);
                    }
                }
            }
            else if (!disabled)
            {
                venueContent.InnerHtml = page.GetResource("JourneyOptionTabContainer.SelectVenue.Information");
            }

            #region Set enabled/disabled style

            if (!planCycle.Enabled)
            {
                if (!planCycle.CssClass.Contains("btnDisabled"))
                {
                    planCycle.CssClass = planCycle.CssClass + " btnDisabled";
                }
            }
            else
            {
                if (planCycle.CssClass.Contains("btnDisabled"))
                {
                    planCycle.CssClass = planCycle.CssClass.Replace(" btnDisabled", string.Empty);
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

        #region Control Event Handlers
        protected void PlanCycleJourney(object sender, EventArgs e)
        {
            if (OnPlanJourney != null)
            {
                OnPlanJourney(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}