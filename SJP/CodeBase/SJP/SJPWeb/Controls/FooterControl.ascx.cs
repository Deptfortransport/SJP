// *********************************************** 
// NAME             : FooterControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Footer control
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using SJP.Common.ResourceManager;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Footer control
    /// </summary>
    public partial class FooterControl : System.Web.UI.UserControl
    {
        #region Page_Init, Page_Load, Page_PreRender

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
            // Display correct language version
            pnlFooterEn.Visible = (CurrentLanguage.Value == Language.English);
            pnlFooterFr.Visible = !pnlFooterEn.Visible;

            #region Footer html from resource manager

            // Load footer html content
            using (LiteralControl footer = new LiteralControl())
            {
                SJPPage page =  (SJPPage)Page;
                
                footer.Text = page.GetResource(
                    SJPResourceManager.GROUP_HEADERFOOTER, 
                    SJPResourceManager.COLLECTION_DEFAULT,
                    (page.SiteModeDisplay == SiteMode.Olympics ? "Footer.Olympics.Html" : "Footer.Paralympics.Html"));


                if (pnlFooterEn.Visible)
                {
                    pnlFooterEn.Controls.Add(footer);
                }
                else
                {
                    pnlFooterFr.Controls.Add(footer);
                }

            }

            #endregion
        }

        #endregion
    }
}