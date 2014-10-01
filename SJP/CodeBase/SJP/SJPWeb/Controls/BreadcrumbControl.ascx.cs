// *********************************************** 
// NAME             : BreadcrumbControl.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Breadcrumb control
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common.ResourceManager;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Breadcrumb control
    /// </summary>
    public partial class BreadcrumbControl : System.Web.UI.UserControl
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
            // Set label text
            lblBreadcrumbTitle.Text = Global.SJPResourceManager.GetString(CurrentLanguage.Value, "BreadcrumbControl.lblBreadcrumbTitle.Text");

            SetupBreadcrumbLinks();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to navigate site map from current node and add hyperlinks to this control
        /// </summary>
        private void SetupBreadcrumbLinks()
        {
            // Control to add links to
            PlaceHolder linksPlaceholder = breadcrumbLinks;

            // Used to keep track of urls for links
            List<string> urls = new List<string>();
            List<string> urlDescs = new List<string>();

            // Current page
            SiteMapNode node = SiteMap.CurrentNode;

            // Navigate back up the link hierachy to capture all links/descriptions
            do
            {
                urls.Add(node.Url);

                // Display link text
                if (string.IsNullOrEmpty(node.ResourceKey))
                {
                    urlDescs.Add(node.Title);
                }
                else
                {   
                    // Resource key available, get from SJPResourcemanager
                    urlDescs.Add(
                        Global.SJPResourceManager.GetString(CurrentLanguage.Value,
                            SJPResourceManager.GROUP_SITEMAP, node.ResourceKey, node.ResourceKey + ".Breadcrumb.Title"));
                }

                node = node.ParentNode;
            }
            while (node != null);

            #region Build breadcrumbs

            // Now reverse through the found links found and setup the breadcrumb trail
            for (int i = urls.Count-1; i >= 0; i--)
            {
                // London2012 requires links to be in a list (so css can correctly be applied)
                using (HtmlGenericControl listItem = new HtmlGenericControl(HtmlTextWriterTag.Li.ToString().ToLower()))
                {
                    // Current page breadcrumb is not an hyperlink
                    if (i == 0)
                    {
                        using (Label label = new Label())
                        {
                            label.Text = urlDescs[i];

                            listItem.Controls.Add(label);
                        }
                    }
                    else
                    {
                        using (HyperLink link = new HyperLink())
                        {
                            link.NavigateUrl = urls[i];
                            link.Text = urlDescs[i];

                            listItem.Controls.Add(link);
                        }
                    }

                    // Don't add a seperator to current page breadcrumb
                    if (i != 0)
                    {
                        using (HtmlGenericControl seperator = new HtmlGenericControl())
                        {
                            seperator.InnerText = " > ";

                            listItem.Controls.Add(seperator);
                        }
                    }

                    linksPlaceholder.Controls.Add(listItem);
                }
            }

            #endregion
        }

        #endregion
    }
}