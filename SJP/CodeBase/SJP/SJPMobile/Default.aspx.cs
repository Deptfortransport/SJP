// *********************************************** 
// NAME             : Default.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Feb 2012
// DESCRIPTION  	: Default SJP Mobile page. Transfers user to actual "SJP Mobile Home" page
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPMobile.Adapters;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// Default page
    /// </summary>
    public partial class _Default : SJPPageMobile
    {
        #region Private members

        private JourneyInputAdapter journeyInputAdapter;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public _Default()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileDefault;
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
            journeyInputAdapter = new JourneyInputAdapter();

            SetupResources();

            SetupControls();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Publit Transport mode button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void publicTransportModeBtn_Click(object sender, EventArgs e)
        {
            UpdateJourneyPlannerMode(SJPJourneyPlannerMode.PublicTransport);

            SetPageTransfer(PageId.MobileInput);
        }

        /// <summary>
        /// Cycle mode button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cycleModeBtn_Click(object sender, EventArgs e)
        {
            UpdateJourneyPlannerMode(SJPJourneyPlannerMode.Cycle);

            SetPageTransfer(PageId.MobileInput);
        }

        /// <summary>
        /// Travel news button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void travelNewsBtn_Click(object sender, EventArgs e)
        {
            SetPageTransfer(PageId.MobileTravelNews);
        }
                
        #endregion

        #region Private methods

        /// <summary>
        /// Setups resources labels and controls
        /// </summary>
        private void SetupResources()
        {
            publicTransportModeBtn.Text = string.Format("<span>{0}</span>", GetResourceMobile("Default.PublicTransportModeButton.Text"));
            publicTransportModeBtn.ToolTip = GetResourceMobile("Default.PublicTransportModeButton.ToolTip");

            cycleModeBtn.Text = string.Format("<span>{0}</span>", GetResourceMobile("Default.CycleModeButton.Text"));
            cycleModeBtn.ToolTip = GetResourceMobile("Default.CycleModeButton.ToolTip");

            travelNewsBtn.Text = string.Format("<span>{0}</span>", GetResourceMobile("Default.TravelNewsButton.Text"));
            travelNewsBtn.ToolTip = GetResourceMobile("Default.TravelNewsButton.ToolTip");

            // Non javascript buttons as link buttons fail when JS disabled
            publicTransportModeBtnNonJS.Text = GetResourceMobile("Default.PublicTransportModeButton.Text");
            publicTransportModeBtnNonJS.ToolTip = GetResourceMobile("Default.PublicTransportModeButton.ToolTip");

            cycleModeBtnNonJS.Text = GetResourceMobile("Default.CycleModeButton.Text");
            cycleModeBtnNonJS.ToolTip = GetResourceMobile("Default.CycleModeButton.ToolTip");

            travelNewsBtnNonJS.Text = GetResourceMobile("Default.TravelNewsButton.Text");
            travelNewsBtnNonJS.ToolTip = GetResourceMobile("Default.TravelNewsButton.ToolTip");
        }

        /// <summary>
        /// Setup the controls
        /// </summary>
        private void SetupControls()
        {
            // Hide back next navigation buttons
            ((SJPMobile)Master).DisplayNavigation = false;

            // Cycle Planner functionality turned off
            if (!Properties.Current["CyclePlanner.Enabled.Switch"].Parse(true))
            {
                cycleModeBtn.Visible = false;
                cycleModeBtnNonJS.Visible = false;
            }

            // Travel News (and Underground news) functionality turned off
            if ((!Properties.Current["TravelNews.Enabled.Switch"].Parse(true))
                && (!Properties.Current["UndergroundNews.Enabled.Switch"].Parse(true)))
            {
                travelNewsBtn.Visible = false;
                travelNewsBtnNonJS.Visible = false;
            }
        }

        /// <summary>
        /// Updates the journey request (if exists) and cookie with the planner mode
        /// </summary>
        /// <param name="mode"></param>
        private void UpdateJourneyPlannerMode(SJPJourneyPlannerMode mode)
        {
            // Journey request will only exist in session if it is a returning user 
            // (i.e. has cookie, or page landed), so update that with planner mode,
            // or create an empty journey request with the mode
            journeyInputAdapter.ValidateAndUpdateSJPRequestForPlannerMode(mode);
        }

        #endregion
    }
}