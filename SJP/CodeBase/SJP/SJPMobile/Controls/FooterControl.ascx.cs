// *********************************************** 
// NAME             : FooterControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 07 Feb 2012
// DESCRIPTION  	: Footer control
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.Web;
using SJP.Common.ResourceManager;
using SJP.Common.PropertyManager;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// Footer control
    /// </summary>
    public partial class FooterControl : System.Web.UI.UserControl
    {
        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Init
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            lnkbtnLanguageEn.Click += new EventHandler(lnkbtnLanguage_Click);
            lnkbtnLanguageFr.Click += new EventHandler(lnkbtnLanguage_Click);
        }

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Page PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetLanguageLinkText();

            SetupBack();

            SetControlVisibility();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to set the language link based on the current culture
        /// </summary>
        private void SetLanguageLinkText()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            // Set the text
            string english = page.GetResourceMobile("HeaderControl.Language.Link.Text.En");
            string english_tooltip = page.GetResourceMobile("HeaderControl.Language.Link.ToolTip.En");
            string french = page.GetResourceMobile("HeaderControl.Language.Link.Text.Fr");
            string french_tooltip = page.GetResourceMobile("HeaderControl.Language.Link.ToolTip.Fr");

            lnkbtnLanguageEn.Text = english;
            lnkbtnLanguageEn.ToolTip = english_tooltip;
            lnkbtnLanguageFr.Text = french;
            lnkbtnLanguageFr.ToolTip= french_tooltip;
        }

        /// <summary>
        /// Method to display and hide controls
        /// </summary>
        private void SetControlVisibility()
        {
            // Display correct language version
            pnlFooterEn.Visible = (CurrentLanguage.Value == Language.English);
            pnlFooterFr.Visible = !pnlFooterEn.Visible;

            // Display language link
            liLanguageLinkEn.Visible = Properties.Current["Header.Link.Language.Visible.Switch"].Parse(true);
            liLanguageLinkFr.Visible = Properties.Current["Header.Link.Language.Visible.Switch"].Parse(true);

            // Display cookie link if required
            DateTime cookieLinkDateTime = Properties.Current["Cookie.CookiePolicy.Hyperlink.VisibleFrom.Date"].Parse(DateTime.Now.AddYears(1));
            DateTime todayDate = DateTime.Now.Date;

            lnkCookiePolicyEn.Visible = todayDate >= cookieLinkDateTime;
            lblSeperatorCookieEn.Visible = lnkCookiePolicyEn.Visible;
            lnkCookiePolicyFr.Visible = todayDate >= cookieLinkDateTime;
            lblSeperatorCookieFr.Visible = lnkCookiePolicyFr.Visible;
        }

        /// <summary>
        /// Sets up the back button
        /// </summary>
        private void SetupBack()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            switch (page.PageId)
            {
                case PageId.MobileDetail:
                case PageId.MobileDirection:
                    backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileSummary.Text");
                    backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileSummary.ToolTip");
                    break;
                default:
                    backBtn.Visible = false;
                    backDiv.Visible = false;
                    break;
            }

            backBtnNonJS.Text = backBtn.Text;
            backBtnNonJS.ToolTip = backBtn.ToolTip;
            backBtnNonJS.Visible = backBtn.Visible;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for lnkbtnLanguage_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnLanguage_Click(object sender, EventArgs e)
        {
            //It is only possible to switch language by clicking the link, 
            //so perform a straight switch, but not here... goto SJPPage::OnInit
            CurrentLanguage.Value = (CurrentLanguage.Value == Language.English ? Language.French : Language.English);

            Server.Transfer(Request.RawUrl);
        }

        /// <summary>
        /// Back button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void backBtn_Click(object sender, EventArgs e)
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            switch (page.PageId)
            {
                case PageId.MobileDetail:
                case PageId.MobileDirection:
                    page.SetPageTransfer(PageId.MobileSummary);
                    page.AddQueryStringForPage(PageId.MobileSummary);
                    break;
                default:
                    // Do nothing, link shouldn't be displayed
                    break;
            }
        }

        #endregion
    }
}