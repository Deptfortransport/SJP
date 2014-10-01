// *********************************************** 
// NAME             : GNATMapsWidget.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: DiscriptionPlaceholder
// ************************************************
// 
                
using System;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.SJPWeb.Code;

namespace SJP.UserPortal.SJPWeb.Controls
{
    public partial class GNATMapsWidget : System.Web.UI.UserControl
    {

        #region Private Fields
        private GenericPromoType promoType = GenericPromoType.GBGNATMap;
        #endregion

        #region Properties
        /// <summary>
        /// Read/Write property determining the type of promo
        /// </summary>
        public GenericPromoType PromoType
        {
            get { return promoType; }
            set { promoType = value; }
        }
        #endregion

        #region Page_Init, Page_Load, Page_PreRender
        /// <summary>
        /// Page PreRender event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupGNATMapsLinks();

            // only check the property switch if the control is visible
            if (this.Visible)
            {
                this.Visible = Properties.Current[string.Format("GenericPromoWidget.{0}.Visible", promoType)].Parse(true);
            }
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        /// <summary>
        /// Set up venue related link in the context of the mode of journey choosed
        /// i.e. if park and ride journey is being plan show maps of park and ride locations
        /// </summary>
        private void SetupGNATMapsLinks()
        {
            SJPPage page = (SJPPage)Page;

            widgetHeadingLink.Text = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoHeadingLink.Text", promoType));

            widgetHeadingLink.ToolTip = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoHeadingLink.ToolTip", promoType));

            widgetHeadingLink.NavigateUrl = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoHeadingLink.Url", promoType));

            gnatImageLink.NavigateUrl = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoImageLink.Url", promoType));

            gnatImage.ImageUrl = page.ImagePath + page.GetResource(string.Format("GenericPromoWidget.{0}.PromoImage.ImageUrl", promoType));

            gnatImage.AlternateText = gnatImage.ToolTip = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoImage.AlternateText", promoType));

            pdfLink.NavigateUrl = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoButtonLink.Url", promoType));

            pdfLink.ToolTip = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoButtonLink.ToolTip", promoType));

            pdfDownloadButton.Text = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoButton.Text", promoType));

        }
        #endregion
    }
}