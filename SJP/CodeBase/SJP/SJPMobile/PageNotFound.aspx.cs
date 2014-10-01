// *********************************************** 
// NAME             : PageNotFound.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 19 Mar 2012
// DESCRIPTION  	: PageNotFound page for resources not found
// ************************************************

using System;
using SJP.Common;
using SJP.Common.Web;
using SJP.UserPortal.ScreenFlow;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// PageNotFound page for resources not found
    /// </summary>
    public partial class PageNotFound : SJPPageMobile
    {
        #region Private members

        // Urls shown to user
        private string URL_Homepage = string.Empty;
        private string URL_Sitemap = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PageNotFound()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobilePageNotFound;
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
            PageTransferDetail ptd = GetPageTransferDetail(Common.PageId.Homepage);

            // London2012 homepage
            if (ptd != null)
            {
                URL_Homepage = ResolveClientUrl(ptd.PageUrl);
            }

            ptd = GetPageTransferDetail(Common.PageId.Sitemap);

            // London2012 sitemap
            if (ptd != null)
            {
                URL_Sitemap = ResolveClientUrl(ptd.PageUrl);
            }
        }

        /// <summary>
        ///  Loads resources for page
        /// </summary>
        private void SetupResources()
        {
            titleMessage.InnerHtml = GetResourceMobile("PageNotFound.HeadingTitle.Text");
            lblMessage.Text = string.Format(GetResourceMobile("PageNotFound.Message.Text"), URL_Homepage, URL_Sitemap);
        }

        #endregion
    }
}