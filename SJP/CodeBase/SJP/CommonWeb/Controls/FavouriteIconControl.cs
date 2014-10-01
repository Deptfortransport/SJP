// *********************************************** 
// NAME             : FavouriteIconControl.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Favourite icon control
// ************************************************
// 

using System.Web.UI;
using System.Web.UI.WebControls;

namespace SJP.Common.Web
{
    /// <summary>
    /// Favourite icon control
    /// </summary>
    public class FavouriteIconControl : WebControl
    {
        #region Private members

        private string iconName;
        private string iconPath;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="iconName"></param>
        public FavouriteIconControl(string iconPath, string iconName) 
            : base(HtmlTextWriterTag.Link)
        {
            this.iconPath = iconPath;
            this.iconName = iconName;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Overridden method that renders the appropriate fav icon attribute to the page
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            // Write the attribute that adds the fav icon
            if (!string.IsNullOrEmpty(iconPath))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, ResolveClientUrl(iconPath) + iconName);
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "icon");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, @"image/png");
            }

            base.AddAttributesToRender(writer);
        }

        #endregion
    }
}