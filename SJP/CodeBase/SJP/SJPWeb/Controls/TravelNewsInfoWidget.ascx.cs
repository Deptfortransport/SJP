// *********************************************** 
// NAME             : TravelNewsInfoWidget.ascx.cs      
// AUTHOR           : Mark Danforth
// DATE CREATED     : 30 May 2012
// DESCRIPTION  	: Information widgte for the right side bar
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Right hand control travel news info widget
    /// </summary>
    public partial class TravelNewsInfoWidget : System.Web.UI.UserControl
    {
        /// <summary>
        /// Page load for the widget
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            setupResources();
        }

        #region Private Methods
        /// <summary>
        /// Sets up resource contents
        /// </summary>
        private void setupResources()
        {
            SJPPage page = (SJPPage)Page;

            widgetHeading.Text = page.GetResource("TravelNewsInfoWidget.WidgetHeading.Text");

            tnInfoContent.Text = page.GetResource("TravelNewsInfoWidget.Content.Text");
        }
        #endregion
    }
}