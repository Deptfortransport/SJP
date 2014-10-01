// *********************************************** 
// NAME             : HeaderControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 07 Feb 2012
// DESCRIPTION  	: Header control
// ************************************************
// 

using System;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPMobile.Controls
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
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupLogoImage();

            // Display correct language version
            pnlHeaderEn.Visible = (CurrentLanguage.Value == Language.English);
            pnlHeaderFr.Visible = !pnlHeaderEn.Visible;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to setup the logo
        /// </summary>
        private void SetupLogoImage()
        {
            // Currently not implmented
        }
        
        #endregion

    }
}