// *********************************************** 
// NAME             : SorryPage.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 19 Mar 2012
// DESCRIPTION  	: SorryPage page displayed when server is busy 
// (this is used to create the static html page displayed on the sorry server, rather than serving
// this expensive aspx)
// ************************************************
// 
                
using System;
using SJP.Common;
using SJP.Common.Web;
using SJP.UserPortal.ScreenFlow;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// SorryPage page displayed when server is busy
    /// </summary>
    public partial class SorryPage : SJPPageMobile
    {
        #region Private members

        // Urls shown to user
        private string URL_Default = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SorryPage()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileSorry;
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
            PageTransferDetail ptd = GetPageTransferDetail(Common.PageId.MobileDefault);

            if (ptd != null)
            {
                URL_Default = ResolveClientUrl(ptd.PageUrl);
            }
        }

        /// <summary>
        ///  Loads resources for page
        /// </summary>
        private void SetupResources()
        {
            titleMessage.InnerHtml = GetResourceMobile("Sorry.HeadingTitle.Text");
            lblMessage.Text = string.Format(GetResourceMobile("Sorry.Message.Text"), URL_Default);
        }

        #endregion
    }
}