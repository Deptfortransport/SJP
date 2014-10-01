// *********************************************** 
// NAME             : ErrorPage.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 19 Mar 2012
// DESCRIPTION  	: Error page for application errors
// ************************************************

using System;
using SJP.Common;
using SJP.Common.Web;
using SJP.UserPortal.ScreenFlow;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// Error page for application errors
    /// </summary>
    public partial class ErrorPage : SJPPageMobile
    {
        #region Private members

        // Urls shown to user
        private string URL_Default = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorPage()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileError;
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

            // SJP homepage
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
            titleMessage.InnerHtml = GetResourceMobile("Error.HeadingTitle.Text");
            lblMessage.Text = string.Format(GetResourceMobile("Error.Message.Text"), URL_Default);
        }

        #endregion
    }
}