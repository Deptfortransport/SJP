// *********************************************** 
// NAME             : MetaControl.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Meta control used to add meta tags to Page head
// ************************************************
// 

using System.Web.UI;
using System.Web.UI.WebControls;

namespace SJP.Common.Web
{
    /// <summary>
    /// MetaControl
    /// </summary>
    public class MetaControl : WebControl
    {
        #region Private members

        private string name;
        private string content;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MetaControl(string name, string content) 
            : base(HtmlTextWriterTag.Meta)
        {
            this.name = name;
            this.content = content;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Overridden method that renders the meta tag to the page head
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            // Write the attribute that adds the fav icon
            if ((!string.IsNullOrEmpty(name)) && (!string.IsNullOrEmpty(content)))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Name, name);
                writer.AddAttribute(HtmlTextWriterAttribute.Content, content);
            }

            base.AddAttributesToRender(writer);
        }

        #endregion
    }
}