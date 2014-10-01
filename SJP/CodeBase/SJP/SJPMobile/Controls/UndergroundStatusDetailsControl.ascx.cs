// *********************************************** 
// NAME             : UndergroundStatusDetailsControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 01 May 2012
// DESCRIPTION  	: A template for holding the details of underground status details
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common.ResourceManager;
using SJP.UserPortal.UndergroundNews;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// A template for holding the details of underground status details
    /// </summary>
    public partial class UndergroundStatusDetailsControl : System.Web.UI.UserControl
    {
        #region Variables

        private SJPResourceManager RM = Global.SJPResourceManager;

        private UndergroundStatusItem usi = null;

        #endregion

        #region Page Load
        
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
            SetupControls();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="tni"></param>
        public void Initialise(UndergroundStatusItem usi)
        {
            this.usi = usi;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Setsup the controls
        /// </summary>
        public void SetupControls()
        {
            if (usi != null)
            {
                // Detail page header text
                undergroundNewsTitle.InnerText = ((SJPPageMobile)Page).GetResourceMobile("TravelNewsDetail.Heading.Text");
                closeNewsItem.Text = ((SJPPageMobile)Page).GetResourceMobile("JourneyInput.Back.Text");
                                
                // Add the display text
                undergroundServiceHeadlineLbl.InnerText = usi.LineName;
                statusDescriptionLbl.Text = usi.StatusDescription;
                statusDetailLbl.Text = usi.LineStatusDetails;

                // Set the line color
                string undergroundLineClass = usi.LineName.ToLower().Replace("&", "and").Replace(" ", "");

                if (!undergroundServiceHeadlineLbl.Attributes["class"].Contains(undergroundLineClass))
                {
                    undergroundServiceHeadlineLbl.Attributes["class"] += string.Format(" " + undergroundLineClass);
                }

            }
        }

        #endregion
    }
}