// *********************************************** 
// NAME             : SkipToLinkControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Apr 2011
// DESCRIPTION  	: SkipToLinkControl control to provide a skip to content hyperlink
// ************************************************
// 
                
using System;
using System.Web.UI.HtmlControls;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// SkipToLinkControl control to provide a skip to content hyperlink
    /// </summary>
    public partial class SkipToLinkControl : System.Web.UI.UserControl
    {
        #region Private members

        private string skipToLinkResource = "Header.SkipTo.Link.Text";
        private string skipToURL = string.Empty;
        private string skipToLinkTag = "skip-navigation";
        
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
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupSkipToLink();
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Method to set link url
        /// </summary>
        private void SetupSkipToLink()
        {
            // Set the link for skip to
            string skipLink = string.Format("{0}#{1}",
                skipToURL,
                (skipToLinkTag.StartsWith("#")) ? skipToLinkTag.Substring(1) : skipToLinkTag);

            // Build anchor link control
            // (cannot use the asp:Hyperlink because it cannot add a plain "#abc", it resolves 
            // to a relative path, in this case the path to the Controls folder)
            using (HtmlGenericControl control = new HtmlGenericControl())
            {
                control.Attributes.Add("href", skipLink);
                control.InnerText = Global.SJPResourceManager.GetString(CurrentLanguage.Value, skipToLinkResource);
                control.TagName = "a";

                this.Controls.Add(control);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Skip to link text to display (this is the resource key to load)
        /// </summary>
        /// <remarks>Defaults to "Header.SkipTo.Link.Text"</remarks>
        public string SkipToLinkResource
        {
            get { return skipToURL; }
            set { skipToURL = value; }
        }

        /// <summary>
        /// Read/Write. Skip to URL (not including the skip to tag)
        /// </summary>
        /// <remarks>Defaults to empty url (current page)</remarks>
        public string SkipToURL
        {
            get { return skipToURL; }
            set { skipToURL = value; }
        }

        /// <summary>
        /// Read/Write. Skip to Tag (tag to append to the URL which represents an anchor to skip to)
        /// </summary>
        /// <remarks>Defaults to "skip-navigation"</remarks>
        public string SkipToLinkTag
        {
            get { return skipToLinkTag; }
            set { skipToLinkTag = value; }
        }

        #endregion
    }
}