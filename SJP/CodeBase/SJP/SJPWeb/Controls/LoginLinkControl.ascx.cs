// *********************************************** 
// NAME             : LoginLinkControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Apr 2011
// DESCRIPTION  	: LoginLinkControl control displaying a link to the login page (currently London2012)
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.UserPortal.ScreenFlow;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// LoginLinkControl control displaying a link to the login page
    /// </summary>
    public partial class LoginLinkControl : System.Web.UI.UserControl
    {
        #region Private members

        private string loginURL = string.Empty;
        private string loginReturnToURL = string.Empty;

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
            SetupLoginURL();

            SetupReturnToURL();
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetLinkText();

            SetLoginLink();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the login page url
        /// </summary>
        private void SetupLoginURL()
        {
            IPageController pageController = SJPServiceDiscovery.Current.Get<IPageController>(ServiceDiscoveryKey.PageController);

            PageTransferDetail ptd = pageController.GetPageTransferDetails(PageId.Login);

            if (ptd != null)
            {
                loginURL = ResolveClientUrl(ptd.PageUrl);
            }
        }

        /// <summary>
        /// Sets the return to page url
        /// </summary>
        private void SetupReturnToURL()
        {
            // Get the current page Raw URL. Use this as it will contain any query string parameters
            loginReturnToURL = Server.UrlEncode(Request.Url.AbsoluteUri);
        }

        /// <summary>
        /// Method to set link text
        /// </summary>
        private void SetLinkText()
        {
            // Set the text
            loginLink.Text = Global.SJPResourceManager.GetString(CurrentLanguage.Value, "Header.Login.Link.Text");
        }

        /// <summary>
        /// Method to set link url
        /// </summary>
        private void SetLoginLink()
        {
            // Set the link for skip to
            string loginURLFull = string.Format("{0}?returl={1}",
                loginURL,
                loginReturnToURL);

            loginLink.NavigateUrl = loginURLFull;
        }

        #endregion

        #region Public properties
        
        /// <summary>
        /// Read/Write. URL to return to after login has been completed
        /// </summary>
        /// <remarks>Defaults to current page Request.RawURL</remarks>
        public string LoginReturnToURL
        {
            get { return loginReturnToURL; }
            set { loginReturnToURL = value; }
        }

        #endregion
    }
}