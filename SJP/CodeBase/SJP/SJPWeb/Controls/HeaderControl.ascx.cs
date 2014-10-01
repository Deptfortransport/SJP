// *********************************************** 
// NAME             : HeaderControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Header control
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using System.Web.UI;
using SJP.Common.ResourceManager;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Header Control
    /// </summary>
    public partial class HeaderControl : System.Web.UI.UserControl
    {

        #region Page_Init, Page_Load, Page_PreRender
        
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupStyleLinks();
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            DisplayControls();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to set the StyleLinkControl properties
        /// </summary>
        private void SetupStyleLinks()
        {
            // Commented out as style link control no longer used. Retained for future re-implementation
            //styleLinkControlFontSmall.StyleMode = StyleLinkControl.StyleLinkMode.FontSmall;
            //styleLinkControlFontMedium.StyleMode = StyleLinkControl.StyleLinkMode.FontMedium;
            //styleLinkControlFontLarge.StyleMode = StyleLinkControl.StyleLinkMode.FontLarge;
            //styleLinkControlAccessibleNormal.StyleMode = StyleLinkControl.StyleLinkMode.AccessibleNormal;
            //styleLinkControlAccessibleDyslexia.StyleMode = StyleLinkControl.StyleLinkMode.AccessibleDyslexia;
            //styleLinkControlAccessibleHighVis.StyleMode = StyleLinkControl.StyleLinkMode.AccessibleHighVis;
        }

        /// <summary>
        /// Displays or hides controls
        /// </summary>
        private void DisplayControls()
        {
            IPropertyProvider pp = Properties.Current;
            SJPPage page = (SJPPage)Page;

            bool showEnglish = (CurrentLanguage.Value == Language.English);
            bool showPara = (page.SiteModeDisplay == SiteMode.Paralympics);

            // Set link visibility
            liSkipToContentLinkEn.Visible = pp["Header.Link.SkipToContent.Visible.Switch"].Parse(true);
            liSkipToContentLinkEnPara.Visible = pp["Header.Link.SkipToContent.Visible.Switch"].Parse(true);

            liSkipToContentLinkFr.Visible = pp["Header.Link.SkipToContent.Visible.Switch"].Parse(true);
            liSkipToContentLinkFrPara.Visible = pp["Header.Link.SkipToContent.Visible.Switch"].Parse(true);

            liLanguageLinkEn.Visible = pp["Header.Link.Language.Visible.Switch"].Parse(true);
            liLanguageLinkEnPara.Visible = pp["Header.Link.Language.Paralympics.Visible.Switch"].Parse(true);

            liLanguageLinkFr.Visible = pp["Header.Link.Language.Visible.Switch"].Parse(true);
            liLanguageLinkFrPara.Visible = pp["Header.Link.Language.Paralympics.Visible.Switch"].Parse(true);

            // Display cookie link if required
            DateTime cookieLinkDateTime = Properties.Current["Cookie.CookiePolicy.Hyperlink.VisibleFrom.Date"].Parse(DateTime.Now.AddYears(1));
            DateTime todayDate = DateTime.Now.Date;

            liCookiesLinkEn.Visible = todayDate >= cookieLinkDateTime;
            liCookiesLinkEnPara.Visible = todayDate >= cookieLinkDateTime;
            liCookiesLinkFr.Visible = todayDate >= cookieLinkDateTime;
            liCookiesLinkFrPara.Visible = todayDate >= cookieLinkDateTime;

            #region Load navigation html from resource manager

            // Load header (primary) html content
            using (LiteralControl headerPrimary = new LiteralControl())
            {
                headerPrimary.Text = page.GetResource(
                    SJPResourceManager.GROUP_HEADERFOOTER,
                    SJPResourceManager.COLLECTION_DEFAULT,
                    (page.SiteModeDisplay == SiteMode.Olympics ?
                        "Header.Olympics.PrimaryContainer.Html" : "Header.Paralympics.PrimaryContainer.Html"));

                if (showEnglish)
                {
                    pnlHeaderPrimaryContainerEn.Controls.Add(headerPrimary);
                }
                else
                {
                    pnlHeaderPrimaryContainerFr.Controls.Add(headerPrimary);
                }
            }

            // Load header (secondary) html content
            using (LiteralControl headerSecondary = new LiteralControl())
            {
                headerSecondary.Text = page.GetResource(
                    SJPResourceManager.GROUP_HEADERFOOTER,
                    SJPResourceManager.COLLECTION_DEFAULT,
                    (page.SiteModeDisplay == SiteMode.Olympics ?
                        "Header.Olympics.SecondaryContainer.Html" : "Header.Paralympics.SecondaryContainer.Html"));

                if (showEnglish)
                {
                    pnlHeaderSecondaryContainerEn.Controls.Add(headerSecondary);
                }
                else
                {
                    pnlHeaderSecondaryContainerFr.Controls.Add(headerSecondary);
                }
            }

            #endregion

            #region Set panel visibility

            // Display correct language/site version
            pnlHeaderMastheadContainerEn.Visible = showEnglish && !showPara;
            pnlHeaderMastheadContainerEnPara.Visible = showEnglish && showPara;
            
            pnlHeaderMastheadContainerFr.Visible = !showEnglish && !showPara;
            pnlHeaderMastheadContainerFrPara.Visible = !showEnglish && showPara;

            pnlHeaderPrimaryContainerEn.Visible = showEnglish;
            pnlHeaderSecondaryContainerEn.Visible = showEnglish;

            pnlHeaderPrimaryContainerFr.Visible = !showEnglish;
            pnlHeaderSecondaryContainerFr.Visible = !showEnglish;

            #endregion
        }

        #endregion
    }
}