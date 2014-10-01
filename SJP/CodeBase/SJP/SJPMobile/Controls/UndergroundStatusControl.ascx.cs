// *********************************************** 
// NAME             : UndergroundStatusControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 08 Mar 2012
// DESCRIPTION  	: Control to display Underground status items
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.UserPortal.UndergroundNews;
using SJP.Common.Web;
using SJP.Common.ServiceDiscovery;
using System.Web.UI.HtmlControls;
using SJP.Common.ResourceManager;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// Control to display Underground status items
    /// </summary>
    public partial class UndergroundStatusControl : System.Web.UI.UserControl
    {
        #region Private members

        // Resource manager
        private SJPResourceManager RM = Global.SJPResourceManager;

        private bool show = true;
        private bool forceUnavailable = false;

        private const string dateTimeFormat = "dd/MM/yyyy HH:mm";

        #endregion

        #region Page_Load

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
            BindUndergroundStatusData();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialise method
        /// </summary>
        public void Initialise(bool show, bool forceUnavailable)
        {
            this.show = show;
            this.forceUnavailable = forceUnavailable;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Underground status repeater ItemDataBound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void undergroundStatusRptr_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                UndergroundStatusItem usi = e.Item.DataItem as UndergroundStatusItem;

                // Build an id based on name to allow javascript to search on id,
                string usiId = string.Format("{0}",
                     usi.LineName.Replace(" ", "_").ToLower());

                #region Summary view

                HiddenField statusLineId = e.Item.FindControlRecursive<HiddenField>("statusLineId");
                statusLineId.Value = usiId;

                HtmlGenericControl statusColorDiv = e.Item.FindControlRecursive<HtmlGenericControl>("statusColorDiv");
                Label statusLineLbl = e.Item.FindControlRecursive<Label>("statusLineLbl");
                Label statusDescriptionLbl = e.Item.FindControlRecursive<Label>("statusDescriptionLbl");

                // Set the status labels
                statusLineLbl.Text = usi.LineName;
                statusDescriptionLbl.Text = usi.StatusDescription;

                // Set the line colour div
                string undergroundLineClass = usi.LineName.ToLower().Replace("&", "and").Replace(" ", "");
                if (!statusColorDiv.Attributes["class"].Contains(undergroundLineClass))
                {
                    statusColorDiv.Attributes["class"] += string.Format(" " + undergroundLineClass);
                }

                // Set the status description css if there are details
                if (!string.IsNullOrEmpty(usi.LineStatusDetails))
                {
                    if (!statusDescriptionLbl.CssClass.Contains("highlight"))
                    {
                        statusDescriptionLbl.CssClass += string.Format(" highlight");
                    }
                }

                #endregion

                #region Detail view

                // Creates a new undergroundDetails 'page' and adds the control to a list of details pages
                UndergroundStatusDetailsControl undergroundStatusDetailsControl = (UndergroundStatusDetailsControl)Page.LoadControl("./Controls/UndergroundStatusDetailsControl.ascx");

                // Set the control id to be the status item id to allow javascript to find it
                undergroundStatusDetailsControl.ID = usiId;

                undergroundStatusDetailsControl.Initialise(usi);
                                
                // Add the detail control to the details page placeholder
                undergroundDetails.Controls.Add(undergroundStatusDetailsControl);

                #endregion

                #region Show Details button non-js

                Button showDetailsBtnNonJS = e.Item.FindControlRecursive<Button>("showDetailsBtnNonJS");

                showDetailsBtnNonJS.Text = "View";
                showDetailsBtnNonJS.ToolTip = "View";
                showDetailsBtnNonJS.CommandArgument = usiId;
                
                #endregion
            }
        }

        /// <summary>
        /// Show Details button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void showDetailsBtnNonJS_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                string newsId = ((Button)sender).CommandArgument;

                SJPPageMobile page = (SJPPageMobile)Page;

                page.SetPageTransfer(Common.PageId.MobileTravelNewsDetail);

                page.AddQueryString(QueryStringKey.NewsId, newsId);
                page.AddQueryString(QueryStringKey.NewsMode, TravelNewsHelper.NewsViewMode_LUL);
            }
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Gets the travel news data using current travel news state and binds it to travel news repeater
        /// </summary>
        private void BindUndergroundStatusData()
        {
            if (show)
            {
                // Get latest underground status items
                IUndergroundNewsHandler undergroundNewsHandler = SJPServiceDiscovery.Current.Get<IUndergroundNewsHandler>(ServiceDiscoveryKey.UndergroundNews);

                List<UndergroundStatusItem> items = undergroundNewsHandler.GetUndergroundStatusItems();

                // Clear the popups
                undergroundDetails.Controls.Clear();

                if ((items != null && items.Count > 0)
                    && !forceUnavailable)
                {
                    // Underground status available
                    undergroundStatusUnavailableDiv.Visible = false;
                    undergroundStatusDiv.Visible = true;

                    undergroundStatusRptr.DataSource = items;
                    undergroundStatusRptr.DataBind();

                    // Set the underground status updated date time
                    string lastUpdated = RM.GetString(CurrentLanguage.Value, "UndergroundNews.LastUpdated.Text");

                    undergroundStatusLastUpdatedLbl.Text = string.Format("{0}: {1}",
                        lastUpdated,
                        undergroundNewsHandler.UndergroundStatusLastUpdated.ToString(dateTimeFormat));
                }
                else
                {
                    // Underground status unavailable
                    undergroundStatusUnavailableDiv.Visible = true;
                    undergroundStatusDiv.Visible = false;

                    undergroundStatusUnavailableLbl.Text = RM.GetString(CurrentLanguage.Value, "UndergroundNews.Unavailable.Text");
                }
            }
            else
            {
                undergroundStatusUnavailableDiv.Visible = false;
                undergroundStatusDiv.Visible = false;
            }
        }

        #endregion
    }
}