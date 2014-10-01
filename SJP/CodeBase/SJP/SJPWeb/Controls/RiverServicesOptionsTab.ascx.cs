// *********************************************** 
// NAME             : RiverServicesOptionsTab.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 May 2011
// DESCRIPTION  	: SJP River Services options tab
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// SJP River Services options tab
    /// </summary>
    public partial class RiverServicesOptionsTab : System.Web.UI.UserControl, IJourneyOptionsTab
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
            get { return JourneyControl.SJPJourneyPlannerMode.RiverServices; }
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

        #region Control Event Handlers

        /// <summary>
        /// River services next button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlanRiverServices(object sender, EventArgs e)
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
                riverServicesOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.Disabled.ImageUrl");
                riverServicesOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.Disabled.AlternateText");
                riverServicesOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.Disabled.ToolTip");
            }
            else
            {
                riverServicesOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.ImageUrl");
                riverServicesOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.AlternateText");
                riverServicesOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.ToolTip");
            }

            planRiverServices.Text = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.PlanRiverServices.Text"));
            planRiverServices.ToolTip = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.PlanRiverServices.ToolTip"));

            planRiverServices.Visible = true;
            planRiverServices.Enabled = !disabled && (venue != null);

            string URL_OpenInNewWindowImage = ResolveClientUrl(page.ImagePath + page.GetResource("OpenInNewWindow.Blue.URL"));
            string ALTTXT_OpenInNewWindow = page.GetResource("OpenInNewWindow.AlternateText");
            string TOOLTIP_OpenInNewWindow = page.GetResource("OpenInNewWindow.Text");

            string imgOpenInNewWindow = string.Format("<img src=\"{0}\" alt=\"{1}\" title=\"{2}\" />",
                URL_OpenInNewWindowImage, ALTTXT_OpenInNewWindow, TOOLTIP_OpenInNewWindow);

            if (disabled)
            {
                venueContent.InnerHtml = page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.Disable.Information");
                
                // Fully hide the submit button if the functionality is set to disabled
                planRiverServices.Visible = false;
            }

            if (venue != null)
            {
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
                List<SJPVenueRiverService> riverServicesList = locationService.GetSJPVenueRiverServices(venue.Naptan);

                // Determine the venue's river service availabilty
                RiverServiceAvailableType riverServiceAvailable = RiverServiceAvailableType.No;
                                
                if (venue is SJPVenueLocation)
                {
                    SJPVenueLocation venueLocation = (SJPVenueLocation)venue;
                    riverServiceAvailable = venueLocation.VenueRiverServiceAvailable;
                }

                if (!disabled)
                {
                    string displayhtml = string.Empty;

                    switch (riverServiceAvailable)
                    {
                        case RiverServiceAvailableType.Yes:
                            if (riverServicesList != null)
                            {
                                planRiverServices.Enabled = riverServicesList.Count > 0;
                                venueContent.InnerHtml = string.Format(page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.Available.Information"),
                                    venue.DisplayName);
                            }
                            else
                            {
                                planRiverServices.Enabled = false;
                                venueContent.InnerHtml = string.Format(
                                    isOriginVenue ? page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.From.Information") :
                                                    page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.To.Information"),
                                    venue.DisplayName,
                                    imgOpenInNewWindow);
                            }
                            break;

                        case RiverServiceAvailableType.Maybe:
                            planRiverServices.Enabled = false;
                            venueContent.InnerHtml = string.Format(
                                isOriginVenue ? page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.From.Information") :
                                                page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.MaybeAvailable.To.Information"),
                                venue.DisplayName,
                                imgOpenInNewWindow);
                            break;

                        case RiverServiceAvailableType.No:
                        default:
                            planRiverServices.Enabled = false;
                            venueContent.InnerHtml = string.Format(
                                isOriginVenue ? page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.NotAvailable.From.Information") :
                                                page.GetResource("JourneyOptionTabContainer.RiverServicesOptions.NotAvailable.To.Information"),
                                venue.DisplayName);
                            break;
                    }
                }
            }
            else if (!disabled)
            {
                venueContent.InnerHtml = page.GetResource("JourneyOptionTabContainer.SelectVenue.Information");
            }
            

            #region Set enabled/disabled style

            if (!planRiverServices.Enabled)
            {
                if (!planRiverServices.CssClass.Contains("btnDisabled"))
                {
                    planRiverServices.CssClass = planRiverServices.CssClass + " btnDisabled";
                }
            }
            else
            {
                if (planRiverServices.CssClass.Contains("btnDisabled"))
                {
                    planRiverServices.CssClass = planRiverServices.CssClass.Replace(" btnDisabled", string.Empty);
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