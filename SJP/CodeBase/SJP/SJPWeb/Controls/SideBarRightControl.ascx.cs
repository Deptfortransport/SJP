// *********************************************** 
// NAME             : SideBarRightControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Side bar right control
// ************************************************
// 


using System;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Side bar right control
    /// </summary>
    public partial class SideBarRightControl : System.Web.UI.UserControl
    {
        #region Private Fields
        private bool showVenueMapOnInputPage = false;
        #endregion

        #region Public Properties
        /// <summary>
        /// Determines whether to show or hide the venue map widget on the input page.
        /// </summary>
        public bool ShowVenueMapOnInputPage
        {
            set { showVenueMapOnInputPage = value; }
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
            ShowHideWidgets();
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Show or hide the widgets based on the setting in the proeprties database
        /// </summary>
        private void ShowHideWidgets()
        {
            SJPPage page = (SJPPage)Page;

            // Show hide configuration in the properties database
            bool venueMapsWidgetVisible = Properties.Current[string.Format("SideBarRightControl.{0}.VenueMapsWidget.Visible", page.PageId)].Parse(false);
            

            topTipsWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.TopTipsWidget.Visible",page.PageId)].Parse(false);
            faqWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.FAQWidget.Visible", page.PageId)].Parse(false);
            walkingWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.WalkingWidget.Visible", page.PageId)].Parse(false);
            gamesTravelCardWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.GamesTravelCardWidget.Visible", page.PageId)].Parse(false);
            accessibleTravelWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.AccessibleTravelWidget.Visible", page.PageId)].Parse(false);
            journeyPlannerWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.JourneyPlannerWidget.Visible", page.PageId)].Parse(false);
            travelNewsWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.TravelNewsWidget.Visible", page.PageId)].Parse(false);
            gbGNATMapWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.GBGNATMapWidget.Visible", page.PageId)].Parse(false);
            seGNATMapWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.SEGNATMapWidget.Visible", page.PageId)].Parse(false);
            londonGNATMapWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.LondonGNATMapWidget.Visible", page.PageId)].Parse(false);
            travelNewsInfoWidget.Visible = Properties.Current[string.Format("SideBarRightControl.{0}.TravelNewsInfoWidget.Visible", page.PageId)].Parse(false);

            SessionHelper sessionHelper = new SessionHelper();
            ISJPJourneyRequest request = sessionHelper.GetSJPJourneyRequest();

            //Only show the venue map on input page when showVenueMapOnInputPage is true;
            if (page.PageId == Common.PageId.JourneyPlannerInput)
            {
                if (request != null && showVenueMapOnInputPage)
                {
                    venueMapsWidget.Visible = venueMapsWidgetVisible;
                }
                else // Do not show venue Maps Widget by default on input page
                {
                    venueMapsWidget.Visible = false;
                }
            }
            else // for the other pages check the value set in the content configuration
            {
                venueMapsWidget.Visible = venueMapsWidgetVisible;
            }

            if (page.PageId == Common.PageId.JourneyLocations)
            {
               

                if (request != null)
                {
                    if (request.PlannerMode == SJPJourneyPlannerMode.RiverServices)
                    {
                        accessibleTravelWidget.Visible = false;
                        gamesTravelCardWidget.Visible = true;
                    }
                    else if (request.PlannerMode == SJPJourneyPlannerMode.BlueBadge)
                    {
                        accessibleTravelWidget.Visible = true;
                        gamesTravelCardWidget.Visible = false;
                    }
                    else
                    {
                        // Turn off both the accessible travel and games travel widgets for other planner modes
                        accessibleTravelWidget.Visible = false;
                        gamesTravelCardWidget.Visible = false;
                    }
                }
                else
                {
                    // Can not get the journey planer mode
                    // Turn off the accessible travel and games travel widgets 
                    accessibleTravelWidget.Visible = false;
                    gamesTravelCardWidget.Visible = false;
                }

            }

            // reset showVenueMapOnInputPage to false
            showVenueMapOnInputPage = false;

        }
        #endregion
    }
}