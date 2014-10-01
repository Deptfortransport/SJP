// *********************************************** 
// NAME             : StyleSheetControl.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Control to insert a style sheet ref link into the page
// ************************************************
// 

using System.Web.UI;
using System.Web.UI.WebControls;

namespace SJP.Common.Web
{
    /// <summary>
    /// StyleSheetControl to insert a style sheet ref link into the page
    /// </summary>
    public class StyleSheetControl : WebControl
    {
        #region Private members

        private string styleSheetName;
        private string styleSheetPath;
        private string styleMedia;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="styleSheetPath"></param>
        /// <param name="styleSheetName"></param>
        public StyleSheetControl(string styleSheetPath, string styleSheetName) 
            : base(HtmlTextWriterTag.Link)
        {
            this.styleSheetPath = styleSheetPath;
            this.styleSheetName = styleSheetName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="styleSheetPath"></param>
        /// <param name="styleSheetName"></param>
        /// <param name="media"></param>
        public StyleSheetControl(string styleSheetPath, string styleSheetName, string styleMedia)
            : base(HtmlTextWriterTag.Link)
        {
            this.styleSheetPath = styleSheetPath;
            this.styleSheetName = styleSheetName;
            this.styleMedia = styleMedia;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Overridden method that renders the appropriate style sheet attribute to the page
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            // Write the attribute that ensures
            if (!string.IsNullOrEmpty(styleSheetPath))
            {
                // If no stylesheetname, then assume path is absolute and includes the file name
                if (string.IsNullOrEmpty(styleSheetName))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, styleSheetPath);
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, ResolveClientUrl(styleSheetPath) + styleSheetName);
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, @"text/css");

                if (!string.IsNullOrEmpty(styleMedia))
                {
                    writer.AddAttribute("media", styleMedia.ToLower());
                }

                // Append an id to allow javascript to find this style sheet element
                if (styleSheetName.ToLower().Contains("default"))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "cssLink");
                }
            }

            base.AddAttributesToRender(writer);
        }

        #endregion
    }
}