// *********************************************** 
// NAME             : SorryPage.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 15 Apr 2011
// DESCRIPTION  	: SorryPage page displayed when server is busy 
// (this is used to create the static html page displayed on the sorry server, rather than serving
// this expensive aspx)
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.Web;
using SJP.UserPortal.ScreenFlow;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// SorryPage page displayed when server is busy 
    /// </summary>
    public partial class SorryPage : SJPPage
    {
        #region Private members

        // Urls shown to user
        private string URL_JourneyPlannerInput = string.Empty;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SorryPage()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.Sorry;
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
            DisplayControls();
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupURLs();

            SetupResources();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the urls
        /// </summary>
        private void SetupURLs()
        {
            PageTransferDetail ptd = GetPageTransferDetail(Common.PageId.JourneyPlannerInput);

            // SJP homepage
            if (ptd != null)
            {
                URL_JourneyPlannerInput = ResolveClientUrl(ptd.PageUrl);
            }
        }

        /// <summary>
        ///  Loads resources for page
        /// </summary>
        private void SetupResources()
        {
            lblMessage1.Text = GetResource("Sorry.Message1.Text");
            lblMessage2.Text = GetResource("Sorry.Message2.Text");
            lblMessage3.Text = string.Format(GetResource("Sorry.Message3.Text"), URL_JourneyPlannerInput);
        }

        /// <summary>
        /// Sets the visibility of controls on the page
        /// </summary>
        private void DisplayControls()
        {
            // Don't display the sidebars
            ((SJPWeb)this.Master).DisplaySideBarLeft = false;
            ((SJPWeb)this.Master).DisplaySideBarRight = false;
        }

        #endregion
    }
}