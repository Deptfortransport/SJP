// *********************************************** 
// NAME             : GenericPromoWidget.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Generic promo widget user class to represent promos with similar layout
// ************************************************


using System;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.SJPWeb.Code;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Generic promo widget user class to represent promos with similar layout
    /// </summary>
    public partial class GenericPromoWidget : System.Web.UI.UserControl
    {
        #region Private Fields
        private GenericPromoType promoType = GenericPromoType.FAQ;
        private SJPPage page = null;
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
            page = (SJPPage)Page;
            SetupPromo();
            
            // only check the property switch if the control is visible
            if (this.Visible)
            {
                this.Visible = Properties.Current[string.Format("GenericPromoWidget.{0}.Visible",promoType)].Parse(true);
            }

            if (promoType == GenericPromoType.GBGNATMap
                || promoType == GenericPromoType.LondonGNATMap
                || promoType == GenericPromoType.SEGNATMap)
            {
                promoImageLink.Target = "_blank";
                promoHeadingLink.Target = "_blank";
                promoButtonLink.Target = "_blank";
            }
        }

        
        #endregion

        #region Private Methods
        /// <summary>
        ///  Set up generice promo content from the content database
        /// </summary>
        private void SetupPromo()
        {
            promoHeadingLink.Text = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoHeadingLink.Text",promoType));

            promoHeadingLink.ToolTip = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoHeadingLink.ToolTip", promoType));

            promoHeadingLink.NavigateUrl = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoHeadingLink.Url", promoType));

            promoImageLink.NavigateUrl = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoImageLink.Url", promoType));

            promoImage.ImageUrl = page.ImagePath + page.GetResource(string.Format("GenericPromoWidget.{0}.PromoImage.ImageUrl", promoType));

            promoImage.AlternateText = promoImage.ToolTip = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoImage.AlternateText", promoType));

            promoContent.Text = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoContent.Text", promoType));
            
            promoButtonLink.NavigateUrl = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoButtonLink.Url", promoType));

            promoButtonLink.ToolTip = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoButtonLink.ToolTip", promoType));

            promoButton.Text = page.GetResource(string.Format("GenericPromoWidget.{0}.PromoButton.Text", promoType));
        }
        #endregion
    }
}