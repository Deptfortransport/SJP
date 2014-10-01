// *********************************************** 
// NAME             : TravelNewsItemControl.ascx.cs      
// AUTHOR           : Mark Danforth
// DATE CREATED     : 30 May 2012
// DESCRIPTION  	: Travel news item control
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.UserPortal.TravelNews.TravelNewsData;
using SJP.Common.LocationService;
using SJP.Common.Web;
using System.Web.UI.HtmlControls;
using SJP.Common.ServiceDiscovery;
using SJP.Common;
using System.Text;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Control to encapsulate repeater functionality for travel news item lists
    /// </summary>
    public partial class TravelNewsItemControl : System.Web.UI.UserControl
    {
        #region Private members

        private SJPPage parentPage;
        private LocationService locationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Public constructor
        /// </summary>
        public TravelNewsItemControl()
        {
            locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page load method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // no implementation
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Binds the list item to the control and sets the page member to allow resources to be loaded
        /// </summary>
        /// <param name="travelNewsItems">List of TravelNewsItems</param>
        /// <param name="pageContext">The page context to use to get resources</param>
        public void Bind(IEnumerable<TravelNewsItem> travelNewsItems, SJPPage pageContext)
        {
            this.parentPage = pageContext;
            this.travelnewsItems.DataSource = travelNewsItems;
            this.travelnewsItems.DataBind();
        }

        #endregion

        #region Protected events

        /// <summary>
        /// Travel News Items repeater ItemDataBound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TravelNewsItems_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                TravelNewsItem newsItem = e.Item.DataItem as TravelNewsItem;

                HtmlAnchor summaryLink = e.Item.FindControlRecursive<HtmlAnchor>("summaryLink");
                Label summaryText = e.Item.FindControlRecursive<Label>("summaryText");
                Label lblNewsId = e.Item.FindControlRecursive<Label>("lblNewsId");
                Image affectedLocation = e.Item.FindControlRecursive<Image>("affectedLocation");

                SetupAffectedLocation(newsItem, affectedLocation);

                lblNewsId.Text = newsItem.Uid;

                summaryText.Text = Server.HtmlEncode(newsItem.HeadlineText);

                summaryLink.Name = string.Format("{0}", newsItem.Uid);

                Label locationText = e.Item.FindControlRecursive<Label>("locationText");
                locationText.Text = string.Format(parentPage.GetResource("TravelNews.locationText.Text"), newsItem.Location);

                Label detailText = e.Item.FindControlRecursive<Label>("detailText");
                detailText.Text = newsItem.DetailText; // Do not HtmlEncode because any found "search token" matched add a <span class=highlight> tag

                Label startDate = e.Item.FindControlRecursive<Label>("startDate");
                startDate.Text = string.Format(parentPage.GetResource("TravelNews.StartDate.Text"), newsItem.StartDateTime.ToString(parentPage.GetResource("TravelNews.StartDate.Format")));

                Label statusDate = e.Item.FindControlRecursive<Label>("statusDate");
                statusDate.Text = string.Format(parentPage.GetResource("TravelNews.StatusDate.Updated.Text"), newsItem.LastModifiedDateTime.ToString(parentPage.GetResource("TravelNews.StatusDate.Format")));

                if (newsItem.ClearedDateTime != DateTime.MinValue)
                {
                    if (DateTime.Now > newsItem.ClearedDateTime)
                    {
                        statusDate.Text = string.Format(parentPage.GetResource("TravelNews.StatusDate.Cleared.Text"), newsItem.ClearedDateTime.ToString(parentPage.GetResource("TravelNews.StatusDate.Format")));
                    }
                }

                Label incidentText = e.Item.FindControlRecursive<Label>("incidentText");
                incidentText.Text = Server.HtmlEncode(newsItem.IncidentType);

                Image incidentType = e.Item.FindControlRecursive<Image>("incidentType");
                TransportType tranportType = TransportType.PublicTransport;

                if (newsItem.ModeOfTransport.ToLower().Trim() == TransportType.Road.ToString().ToLower())
                {
                    tranportType = TransportType.Road;
                }

                incidentType.ImageUrl = parentPage.ImagePath + parentPage.GetResource(string.Format("TravelNews.IncidentType.{0}.{1}.Url",
                    tranportType,
                    newsItem.PlannedIncident ? "Planned" : "UnPlanned"));
                incidentType.AlternateText = parentPage.GetResource(string.Format("TravelNews.IncidentType.{0}.{1}.AlternateText",
                    tranportType,
                    newsItem.PlannedIncident ? "Planned" : "UnPlanned"));
                incidentType.ToolTip = parentPage.GetResource(string.Format("TravelNews.IncidentType.{0}.{1}.AlternateText",
                    tranportType,
                    newsItem.PlannedIncident ? "Planned" : "UnPlanned"));

                // Check for affected venues and include them in the ouptput
                List<SJPLocation> affectedLocations = locationService.GetSJPVenueLocations().AsQueryable().Where(vl => newsItem.OlympicVenuesAffected.Contains(vl.Naptan.FirstOrDefault())).Select(vl => vl).ToList();

                if (affectedLocations.Count > 0)
                {
                    e.Item.FindControlRecursive<Panel>("affectedVenuesDiv").Visible = true;
                    e.Item.FindControlRecursive<Label>("lblAffectedVenues").Text = parentPage.GetResource("TravelNews.lblAffectedVenues.Text");
                    Label affectedVenues = e.Item.FindControlRecursive<Label>("affectedVenues");

                    StringBuilder sbAffectedVenues = new StringBuilder();

                    bool showDebug = DebugHelper.ShowDebug;

                    foreach(SJPLocation loc in affectedLocations)
                    {
                        if (sbAffectedVenues.Length > 0)
                        {
                            sbAffectedVenues.Append(", ");
                        }
                        sbAffectedVenues.Append(string.Format("{0}{1}",
                            loc.DisplayName,
                            showDebug ? string.Format("<span class=\"debug\">[{0}]</span>", loc.Naptan.FirstOrDefault()) : string.Empty));
                    }
                    
                    affectedVenues.Text = sbAffectedVenues.ToString();
                }

                // check for and include any olympic travel advice
                if (!String.IsNullOrEmpty(newsItem.OlympicTravelAdvice))
                {
                    Label travelAdvice = e.Item.FindControlRecursive<Label>("travelAdvice");
                    travelAdvice.Visible = true;
                    travelAdvice.Text = newsItem.OlympicTravelAdvice;
                }

                
                if (DebugHelper.ShowDebug)
                {
                    e.Item.FindControlRecursive<HtmlGenericControl>("debugInfoDiv").Visible = true;
                                        
                    Label lblDebugInfo = e.Item.FindControlRecursive<Label>("lblDebugInfo");
                    lblDebugInfo.Text = string.Format("ID[{0}] Olym[{1}] OlymSev[{2}] Sev[{3}] Reg[{4}] Stus[{5}] Actv[{6}] DyMask[{7}] DyStrt[{8}] DyEnd[{9}] Trnspt[{10}]",
                            newsItem.Uid,
                            newsItem.OlympicIncident,
                            newsItem.OlympicSeverityDescription,
                            newsItem.SeverityDescription,
                            newsItem.Regions,
                            newsItem.IncidentStatus,
                            newsItem.IncidentActiveStatus.ToString(),
                            newsItem.DayMask,
                            newsItem.DailyStartTime.ToString(),
                            newsItem.DailyEndTime.ToString(),
                            newsItem.ModeOfTransport);
                }

            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the affected aread of travel news story
        /// </summary>
        /// <param name="newsItem">Travel News story</param>
        /// <param name="affectedLocation"></param>
        /// <param name="affectedlocationText"></param>
        private void SetupAffectedLocation(TravelNewsItem newsItem, Image affectedLocation)
        {
            TravelNewsItem tnItem = newsItem;

            string image = string.Empty;

            if (newsItem.ModeOfTransport.ToLower().Trim() == "road")
            {
                if (!string.IsNullOrEmpty(newsItem.RoadType.ToString()))
                {
                    string roadType = newsItem.RoadType.Trim().Replace(" ", "");
                    image = parentPage.GetResource(string.Format("TravelNews.AffectedLocation.Road.{0}.Url", roadType));

                    // Set image text
                    affectedLocation.AlternateText = parentPage.GetResource(string.Format("TravelNews.AffectedLocation.Road.{0}.AlternateText", roadType));
                    affectedLocation.ToolTip = parentPage.GetResource(string.Format("TravelNews.AffectedLocation.Road.{0}.AlternateText", roadType));
                    affectedLocation.CssClass = roadType.Replace("(", string.Empty).Replace(")", string.Empty);
                }
            }
            else
            {
                // Set the transport mode image
                SJPModeType mode = TravelNewsTransportModeParser.GetSJPModeType(tnItem.ModeOfTransport);
                
                image = parentPage.GetResource(string.Format("TransportMode.{0}.ImageUrl", mode.ToString()));
                
                if (string.IsNullOrEmpty(image))
                {
                    image = parentPage.GetResource("TravelNews.AffectedLocation.PublicTransport.Url");
                }

                // Use the news item mode first
                string affectedLocationAltText = parentPage.GetResource(string.Format("TravelNews.AffectedLocation.{0}.AlternateText", tnItem.ModeOfTransport));
                
                if (string.IsNullOrEmpty(affectedLocationAltText))
                {
                    // Otherwise use the mode type
                    affectedLocationAltText = parentPage.GetResource(string.Format("TransportMode.{0}", mode));

                    if (string.IsNullOrEmpty(affectedLocationAltText))
                        affectedLocationAltText = parentPage.GetResource("TravelNews.AffectedLocation.PublicTransport.AlternateText");
                }

                // Set image text
                affectedLocation.AlternateText = affectedLocationAltText;
                affectedLocation.ToolTip = affectedLocationAltText;
                affectedLocation.CssClass = mode.ToString().ToLower();
            }

            if (string.IsNullOrEmpty(image))
            {
                affectedLocation.Visible = false;
            }
            else
            {
                affectedLocation.ImageUrl = parentPage.ImagePath + image;
                affectedLocation.Visible = true;
            }
        }

        #endregion
    }
}