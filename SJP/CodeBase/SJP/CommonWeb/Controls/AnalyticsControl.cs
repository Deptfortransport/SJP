// *********************************************** 
// NAME             : AnalyticsControl.ascx      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Apr 2011
// DESCRIPTION  	: AnalyticsControl control for placing the Google Analytics and Advert tags on to a page
// ************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using System.Web.UI.WebControls;

namespace SJP.Common.Web
{
    public class AnalyticsControl :  UserControl
    {
        #region Private members

        private SJPResourceManager resourceManager = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AnalyticsControl(SJPResourceManager resourceManager)
            : base()
        {
            this.resourceManager = resourceManager;
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender
        
        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadAnalyticsTag();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to load the analytics tag
        /// </summary>
        private void LoadAnalyticsTag()
        {
            bool includeAnalyticsTag = Properties.Current["Analytics.Tag.Include.Switch"].Parse(false);
            bool includeAdvertsTag = Properties.Current["Adverts.Tag.Include.Switch"].Parse(false);

            #region Analytics

            if (includeAnalyticsTag)
            {
                SJPPage page = (SJPPage)this.Page;

                #region Get tag content

                Language language = CurrentLanguage.Value;

                // Get tag specific for page
                string analyticsTagHostContent = resourceManager.GetString(language,
                    SJPResourceManager.GROUP_ANALYTICS, page.PageId.ToString(), "Analytics.Tag.Host");

                string analyticsTagTrackerContent = resourceManager.GetString(language,
                    SJPResourceManager.GROUP_ANALYTICS, page.PageId.ToString(), "Analytics.Tag.Tracker");

                if (string.IsNullOrEmpty(analyticsTagHostContent))
                {
                    // No tag specific for page, load default tag
                    analyticsTagHostContent = resourceManager.GetString(language,
                        SJPResourceManager.GROUP_ANALYTICS, PageId.Default.ToString(), "Analytics.Tag.Host");
                }

                if (string.IsNullOrEmpty(analyticsTagTrackerContent))
                {
                    // No tag specific for page, load default tag
                    analyticsTagTrackerContent = resourceManager.GetString(language,
                        SJPResourceManager.GROUP_ANALYTICS, PageId.Default.ToString(), "Analytics.Tag.Tracker");
                }

                #endregion

                // Add tags onto the page, using the literal control as they should be positioned at bottom of the page
                if (!string.IsNullOrEmpty(analyticsTagHostContent))
                {
                    using (Literal litAnalyticsTagHost = new Literal())
                    {
                        litAnalyticsTagHost.Text = analyticsTagHostContent;

                        this.Controls.Add(litAnalyticsTagHost);
                    }
                }

                if (!string.IsNullOrEmpty(analyticsTagTrackerContent))
                {
                    using (Literal litAnalyticsTagTracker = new Literal())
                    {
                        litAnalyticsTagTracker.Text = analyticsTagTrackerContent;

                        this.Controls.Add(litAnalyticsTagTracker);
                    }
                }
            }

            #endregion

            #region Adverts

            if (includeAdvertsTag)
            {
                SJPPage page = (SJPPage)this.Page;

                #region Get advert content

                Language language = CurrentLanguage.Value;

                // Get tag specific for page
                string advertsTagServiceContent = resourceManager.GetString(language,
                    SJPResourceManager.GROUP_ANALYTICS, page.PageId.ToString(), "Adverts.Tag.Service");

                string advertsTagPlaceholderContent = resourceManager.GetString(language,
                    SJPResourceManager.GROUP_ANALYTICS, page.PageId.ToString(), "Adverts.Tag.Placeholders");

                if (string.IsNullOrEmpty(advertsTagServiceContent))
                {
                    // No tag specific for page, load default tag
                    advertsTagServiceContent = resourceManager.GetString(language,
                        SJPResourceManager.GROUP_ANALYTICS, PageId.Default.ToString(), "Adverts.Tag.Service");
                }

                if (string.IsNullOrEmpty(advertsTagPlaceholderContent))
                {
                    // No tag specific for page, load default tag
                    advertsTagPlaceholderContent = resourceManager.GetString(language,
                        SJPResourceManager.GROUP_ANALYTICS, PageId.Default.ToString(), "Adverts.Tag.Placeholders");
                }

                #endregion

                // Add tags onto the page, using the literal control
                if (!string.IsNullOrEmpty(advertsTagServiceContent))
                {
                    using (Literal litAdvertsTagService = new Literal())
                    {
                        litAdvertsTagService.Text = advertsTagServiceContent;

                        this.Controls.Add(litAdvertsTagService);
                    }
                }

                if (!string.IsNullOrEmpty(advertsTagPlaceholderContent))
                {
                    using (Literal litAdvertsTagPlaceholder = new Literal())
                    {
                        litAdvertsTagPlaceholder.Text = advertsTagPlaceholderContent;

                        this.Controls.Add(litAdvertsTagPlaceholder);
                    }
                }
            }

            #endregion

            if (!includeAnalyticsTag && !includeAdvertsTag)
            {
                // Don't display any tag
                this.Visible = false;
            }
        }

        #endregion
    }
}
