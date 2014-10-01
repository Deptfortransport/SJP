// *********************************************** 
// NAME             : CanonicalControl.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Canonical Control
// ************************************************
// 


using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common.ResourceManager;

namespace SJP.Common.Web
{
    /// <summary>
    /// CanonicalControl. Uses the SiteMap to build up the canonical url to add to the Page's Head element
    /// </summary>
    public class CanonicalControl : WebControl
    {
        #region Private members

        private SJPResourceManager resourceManager = null;
                
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CanonicalControl(SJPResourceManager resourceManager) : base(HtmlTextWriterTag.Link)
        {
            this.resourceManager = resourceManager;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Overridden method that renders the canonical attribute to the page
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            // Current page
            SiteMapNode node = SiteMap.CurrentNode;

            if (node != null)
            {
                string url = string.Empty;

                if (resourceManager != null)
                {
                    url = resourceManager.GetString(CurrentLanguage.Value,
                                    SJPResourceManager.GROUP_SITEMAP, node.ResourceKey, node.ResourceKey + ".Canonical");
                }
                
                // If nothing specified in ResourceManager, use URL in sitemap
                if (string.IsNullOrEmpty(url))
                {
                    url = string.Format("http://{0}{1}", Page.Request.Url.Host, Page.ResolveClientUrl(node.Url));
                }

                // Write the attribute
                writer.AddAttribute(HtmlTextWriterAttribute.Href, url);
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "canonical");
            }
            
            base.AddAttributesToRender(writer);
        }

        #endregion
    }
}