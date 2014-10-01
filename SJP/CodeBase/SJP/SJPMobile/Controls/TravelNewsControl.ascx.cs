// *********************************************** 
// NAME             : TravelNewsControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 08 Mar 2012
// DESCRIPTION  	: Control to display Travel news data
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.UserPortal.TravelNews.TravelNewsData;
using SJP.Common.Web;
using SJP.UserPortal.TravelNews;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.TravelNews.SessionData;
using SJP.Common.ResourceManager;
using SJP.Common;
using System.Text;
using SJP.Common.LocationService;
using System.Web.UI.HtmlControls;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// Control to display Travel news data
    /// </summary>
    public partial class TravelNewsControl : System.Web.UI.UserControl
    {
        #region Private members

        // Resource manager
        private SJPResourceManager RM = Global.SJPResourceManager;

        private TravelNewsState travelNewsState = null;
        private bool show = true;
        private bool forceUnavailable = false;

        private const string dateTimeFormat = "dd/MM/yyyy HH:mm";

        private LocationHelper locationHelper = null;
        
        #endregion

        #region Page_Load

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            locationHelper = new LocationHelper();
        }
        
        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            newsDetails.Controls.Clear();
            BindTravelNewsData();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialise method
        /// </summary>
        public void Initialise(bool show, bool forceUnavailable, TravelNewsState travelNewsState)
        {
            this.show = show;
            this.forceUnavailable = forceUnavailable;
            this.travelNewsState = travelNewsState;
        }

        /// <summary>
        /// Refresh method to force update of the travel news controls
        /// </summary>
        public void Refresh()
        {
            BindTravelNewsData();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Travel news repeater ItemDataBound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void travelNewsRptr_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                TravelNewsItem tni = e.Item.DataItem as TravelNewsItem;

                Language language = CurrentLanguage.Value;

                #region Summary view

                HiddenField newsId = e.Item.FindControlRecursive<HiddenField>("newsId");
                Label newsHeadlineLbl = e.Item.FindControlRecursive<Label>("newsHeadlineLbl");
                Label newsUpdatedLbl = e.Item.FindControlRecursive<Label>("newsUpdatedLbl");
                Image newsTransportModeImg = e.Item.FindControlRecursive<Image>("newsTransportModeImg");

                // Set the travel news labels
                newsId.Value = tni.Uid;
                newsHeadlineLbl.Text = 
                    DebugHelper.ShowDebug ? 
                        string.Format("<span class=\"debug\">ID[{0}] Olym[{1}] OlymSev[{2}] Sev[{3}] Reg[{4}] Actv[{5}]</span><br />{6}",
                            tni.Uid,
                            tni.OlympicIncident,
                            tni.OlympicSeverityDescription,
                            tni.SeverityDescription,
                            tni.Regions,
                            tni.IncidentActiveStatus.ToString(),
                            Server.HtmlEncode(tni.HeadlineText))
                        : Server.HtmlEncode(tni.HeadlineText);
                     
                newsUpdatedLbl.Text = tni.LastModifiedDateTime.ToString(dateTimeFormat);
                
                // Set the transport mode image
                SJPModeType mode = TravelNewsTransportModeParser.GetSJPModeType(tni.ModeOfTransport);
                string transportModePath = RM.GetString(language, string.Format("TransportMode.{0}.ImageUrl", mode.ToString()));
                if (!string.IsNullOrEmpty(transportModePath))
                {
                    SJPPageMobile page = (SJPPageMobile)Page;
                    newsTransportModeImg.ImageUrl = page.ImagePath + transportModePath;
                    newsTransportModeImg.ToolTip = RM.GetString(language, string.Format("TransportMode.{0}", tni.ModeOfTransport));
                    newsTransportModeImg.AlternateText = RM.GetString(language, string.Format("TransportMode.{0}", tni.ModeOfTransport));
                    newsTransportModeImg.CssClass = mode.ToString().ToLower();
                }
                else
                {
                    newsTransportModeImg.Visible = false;
                }

                #endregion

                #region Detail view

                // Creates a new travelNewsDetails 'page' and adds the control to a list of details pages
                TravelNewsDetailControl travelNewsDetailControl = (TravelNewsDetailControl)Page.LoadControl("./Controls/TravelNewsDetailControl.ascx");

                travelNewsDetailControl.ID = tni.Uid;
                
                travelNewsDetailControl.Initialise(tni);

                // Add the detail control to the details page placeholder
                newsDetails.Controls.Add(travelNewsDetailControl);

                #endregion

                #region Show Details button non-js

                Button showDetailsBtnNonJS = e.Item.FindControlRecursive<Button>("showDetailsBtnNonJS");

                showDetailsBtnNonJS.Text = "View";
                showDetailsBtnNonJS.ToolTip = "View";
                showDetailsBtnNonJS.CommandArgument = tni.Uid;

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
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the travel news data using current travel news state and binds it to travel news repeater
        /// </summary>
        private void BindTravelNewsData()
        {
            if (show && travelNewsState != null)
            {
                // Get latest travel news items
                ITravelNewsHandler travelNewsHandler = SJPServiceDiscovery.Current.Get<ITravelNewsHandler>(ServiceDiscoveryKey.TravelNews);

                TravelNewsItem[] items = travelNewsHandler.GetDetailsForMobile(travelNewsState);

                if ((items != null && items.Length > 0) 
                    && travelNewsHandler.IsTravelNewsAvaliable
                    && !forceUnavailable)
                {
                    // Travel news available
                    travelNewsUnavailableDiv.Visible = false;
                    travelNewsDiv.Visible = true;

                    travelNewsRptr.DataSource = items;
                    travelNewsRptr.DataBind();

                    // Set the travel news updated date time
                    string lastUpdated = RM.GetString(CurrentLanguage.Value, "TravelNews.LastUpdated.Text");

                    travelNewsLastUpdatedLbl.Text = string.Format("{0}: {1}",
                        lastUpdated,
                        travelNewsHandler.TravelNewsLastUpdated.ToString(dateTimeFormat));
                }
                else if (travelNewsHandler.IsTravelNewsAvaliable 
                    && !forceUnavailable)
                {
                    // Travel news items not found
                    travelNewsUnavailableDiv.Visible = true;
                    travelNewsDiv.Visible = false;

                    // Travel news incidents not found for current news state,
                    // most likely no incidents for the selected venue naptans
                    if (travelNewsState.SearchNaptans.Count > 0)
                    {
                        string venues = locationHelper.GetLocationNames(travelNewsState.SearchNaptans);

                        if (!string.IsNullOrEmpty(venues))
                        {
                            if (travelNewsState.SelectedAllVenuesFlag)
                            {
                                travelNewsUnavailableLbl.Text = RM.GetString(CurrentLanguage.Value, "TravelNews.lblNoIncidents.AllVenues.Text");
                            }
                            else
                            {
                                travelNewsUnavailableLbl.Text = string.Format(
                                    travelNewsState.SearchNaptans.Count == 1 ?
                                        RM.GetString(CurrentLanguage.Value, "TravelNews.lblNoIncidents.Venue.Text") :
                                        RM.GetString(CurrentLanguage.Value, "TravelNews.lblNoIncidents.Venues.Text"),
                                    venues);
                            }
                        }
                        // Unrecognised or invalid naptans were provided
                        else
                        {
                            travelNewsUnavailableLbl.Text = string.Format(
                                RM.GetString(CurrentLanguage.Value, "TravelNews.lblNoIncidents.InvalidVenue.Text"),
                                travelNewsState.SearchNaptans[0]);
                        }
                    }
                    else
                    {
                        travelNewsUnavailableLbl.Text = RM.GetString(CurrentLanguage.Value, "TravelNews.lblNoIncidents.Text");
                    }
                }
                else
                {
                    // Travel news unavailable
                    travelNewsUnavailableDiv.Visible = true;
                    travelNewsDiv.Visible = false;

                    travelNewsUnavailableLbl.Text = RM.GetString(CurrentLanguage.Value, "TravelNews.lblUnavailable.Text");
                }
            }
            else
            {
                travelNewsUnavailableDiv.Visible = false;
                travelNewsDiv.Visible = false;
            }
        }

        #endregion
    }
}
