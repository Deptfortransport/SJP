// *********************************************** 
// NAME             : JourneyPlannerWidget.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Right hand journey planner promo widget
// ************************************************


using System;
using SJP.Common.Web;
using SJP.UserPortal.ScreenFlow;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Right hand journey planner promo widget
    /// </summary>
    public partial class JourneyPlannerWidget : System.Web.UI.UserControl
    {
        #region Page_Init, Page_Load, Page_PreRender
        /// <summary>
        /// Page load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            setupResources();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Sets up resource contents
        /// </summary>
        private void setupResources()
        {
            SJPPage page = (SJPPage)Page;

            widgetHeading.Text = page.GetResource("JourneyPlannerWidget.WidgetHeading.Text");

            jpPromoImage.ImageUrl = page.ImagePath + page.GetResource("JourneyPlannerWidget.JPPromoImage.ImageUrl");
            jpPromoImage.AlternateText = page.GetResource("JourneyPlannerWidget.JPPromoImage.AlternateText");
            jpPromoImage.ToolTip = page.GetResource("JourneyPlannerWidget.JPPromoImage.AlternateText");

            jpPromoContent.Text = page.GetResource("JourneyPlannerWidget.JPPromoContent.Text");

            PageTransferDetail ptd = page.GetPageTransferDetail(Common.PageId.JourneyPlannerInput);

            if (ptd != null)
                jpLink.NavigateUrl = ResolveClientUrl(ptd.PageUrl);

            jpLinkText.Text = page.GetResource("JourneyPlannerWidget.JPLink.Text");
            jpLink.ToolTip = page.GetResource("JourneyPlannerWidget.JPLink.ToolTip");
        }
        #endregion
    }
}