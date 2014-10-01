// *********************************************** 
// NAME             : TravelNewsWidget.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 May 2011
// DESCRIPTION  	: Travel news widget to be shown in the right hand section
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.SJPWeb.Adapters;
using SJP.UserPortal.TravelNews;
using SJP.UserPortal.TravelNews.SessionData;
using SJP.UserPortal.TravelNews.TravelNewsData;
using System.Text;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Travel news widget to be shown in the right hand section
    /// </summary>
    public partial class TravelNewsWidget : System.Web.UI.UserControl
    {
        #region Private members
        
        private bool isAvailable = true;
        private string travelNewsPageURL = string.Empty;
        private URLHelper urlHelper = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Read only property determining if the widget is available
        /// </summary>
        public bool IsAvailable
        {
            get { return isAvailable; }
        }

        /// <summary>
        /// Tracks the current set of headlines showing on the page
        /// </summary>
        public int CurrentPageIndex
        {
            get
            {
                if (ViewState["CurrentTNPage"] == null)
                {
                    return 1;
                }
                else
                {
                    return (int)ViewState["CurrentTNPage"];
                }
            }
            set { ViewState["CurrentTNPage"] = value; }
        }

        #endregion

        #region  Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            urlHelper = new URLHelper();

            SetupTravelNewsPageURL();

            SJPPage page = (SJPPage)Page;
            page.AddJavascript("TravelNewsWidget.js");
            page.AddJavascript("jquery.scrollTo.js");
            page.AddJavascript("jquery.serialScroll.js");
        }

        /// <summary>
        /// Page PreRender event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();
            BindTravelNewsHeadlines();
        }
        
        #endregion

        #region Control Event Handlers

        /// <summary>
        /// TravelNewsHeadlines container repeater data bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TravelNewsHeadlines_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl pageItem = e.Item.FindControlRecursive<HtmlGenericControl>("pageItem");
                currentPage.Text = CurrentPageIndex.ToString();
                if (e.Item.ItemIndex == CurrentPageIndex - 1)
                {
                    pageItem.Attributes["class"] = pageItem.Attributes["class"] + " active";
                }
                else
                {
                    pageItem.Attributes["class"] = pageItem.Attributes["class"].Replace(" active", "");
                }
                Repeater travelNewsHeadlineItems = e.Item.FindControlRecursive<Repeater>("travelNewsHeadlineItems");
                if (travelNewsHeadlineItems != null)
                {
                    IGrouping<int, HeadlineItem> group = e.Item.DataItem as IGrouping<int, HeadlineItem>;
                    travelNewsHeadlineItems.DataSource = group.ToList();
                    travelNewsHeadlineItems.DataBind();
                }
            }
        }

        /// <summary>
        /// TravelNewsHeadlines items repeater data bound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TravelNewsHeadlineItems_DataBound(object sender, RepeaterItemEventArgs e)
        {
            SJPPage page = (SJPPage)Page;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                HeadlineItem headline = e.Item.DataItem as HeadlineItem;

                HyperLink tnItemLink = e.Item.FindControlRecursive<HyperLink>("tnItemLink");
                if (tnItemLink != null)
                {
                    tnItemLink.NavigateUrl = urlHelper.AddQueryStringPart(travelNewsPageURL, QueryStringKey.NewsId, headline.Uid);

                    tnItemLink.NavigateUrl += string.Format("#{0}", headline.Uid);
                    tnItemLink.Text = headline.HeadlineText;
                    tnItemLink.ToolTip = page.GetResource("TravelNewsWidget.HeadlineLink.ToolTip");

                    if (DebugHelper.ShowDebug)
                    {
                        StringBuilder venues = new StringBuilder();

                        foreach (string s in headline.OlympicVenuesAffected)
                        {
                            if (venues.Length > 0)
                            {
                                venues.Append(", ");
                            }

                            venues.Append(s);
                        }

                        string debugText = string.Format("{0}<br /><span class=\"debug\">Sev[{1}] Reg[{2}] Venues[{3}]</span>",
                            tnItemLink.Text,
                            headline.SeverityLevel,
                            headline.Regions,
                            venues.ToString()
                            );

                        tnItemLink.Text = debugText;
                    }
                }
            }
        }

        /// <summary>
        /// Headlines paging previous button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Prev_Click(object sender, EventArgs e)
        {
            CurrentPageIndex -= 1;
        }

        /// <summary>
        /// Headlines paging next button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Next_Click(object sender, EventArgs e)
        {
            CurrentPageIndex += 1;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up resource content for controls
        /// </summary>
        private void SetupResources()
        {
            SJPPage page = (SJPPage)Page;
            
            IPageController pageController = SJPServiceDiscovery.Current.Get<IPageController>(ServiceDiscoveryKey.PageController);

            widgetHeading.NavigateUrl = travelNewsPageURL;
            widgetHeading.Text = page.GetResource("TravelNewsWidget.WidgetHeading.Text");
            widgetHeading.ToolTip = page.GetResource("TravelNewsWidget.WidgetHeading.Text");

            tnMoreLink.NavigateUrl = travelNewsPageURL;
            tnMoreLink.Text = page.GetResource("TravelNewsWidget.MoreLink.Text");
            tnMoreLink.ToolTip = page.GetResource("TravelNewsWidget.MoreLink.ToolTip");
             
            // Paging buttons
            prev.ImageUrl = page.ImagePath + page.GetResource("TravelNewsWidget.PrevButton.ImageUrl");
            prev.ToolTip = prev.AlternateText = page.GetResource("TravelNewsWidget.PrevButton.AlternateText");

            next.ImageUrl = page.ImagePath + page.GetResource("TravelNewsWidget.NextButton.ImageUrl");
            next.ToolTip = next.AlternateText = page.GetResource("TravelNewsWidget.NextButton.AlternateText");
        }

        /// <summary>
        /// Sets up the travel news page url to be used in linking headline items in the control
        /// </summary>
        private void SetupTravelNewsPageURL()
        {
            IPageController pageController = SJPServiceDiscovery.Current.Get<IPageController>(ServiceDiscoveryKey.PageController);
            PageTransferDetail pageTransferDetail = pageController.GetPageTransferDetails(Common.PageId.TravelNews);

            if (pageTransferDetail != null)
            {
                travelNewsPageURL = pageTransferDetail.PageUrl;
            }
        }

        /// <summary>
        /// Gets the travel news headlines and binds it to the travel news side bar widget repeater
        /// </summary>
        private void BindTravelNewsHeadlines()
        {
            SJPPage page = (SJPPage)Page;

            TravelNewsHelper travelNewsHelper = new TravelNewsHelper();

            // Use the default state always, otherwise the users actions on another page, e.g. TravelNews, 
            // may affect the headline incidents shown
            TravelNewsState travelNewsState = new TravelNewsState();
            travelNewsState = travelNewsHelper.GetDefaultTravelNewsState(travelNewsState);

            bool filterForJourneyVenues = false;

            //On result page filter the travel news items using selected venues
            if (page.PageId != Common.PageId.JourneyPlannerInput)
            {
                SessionHelper sessionHelper = new SessionHelper();
                ISJPJourneyRequest journeyRequest = sessionHelper.GetSJPJourneyRequest();

                SJPVenueLocation venueOrigin = null;
                SJPVenueLocation venueDestination = null;

                if (journeyRequest.Origin is SJPVenueLocation)
                    venueOrigin = (SJPVenueLocation)journeyRequest.Origin;
                if (journeyRequest.Destination is SJPVenueLocation)
                    venueDestination = (SJPVenueLocation)journeyRequest.Destination;
                
                if (Properties.Current["TravelNewsWidget.JourneyBasedFilter.UseVenueNaptan"].Parse(false))
                {
                    #region Use venue(s) naptan

                    // Reset naptans and add the venue specific range
                    travelNewsState.SearchNaptans.Clear();

                    if (venueOrigin != null)
                    {
                        travelNewsState.SearchNaptans.AddRange(venueOrigin.Naptan);
                        
                        filterForJourneyVenues = true;
                    }

                    if (venueDestination != null)
                    {
                        travelNewsState.SearchNaptans.AddRange(venueDestination.Naptan);

                        filterForJourneyVenues = true;
                    }

                    #endregion
                }

                if (Properties.Current["TravelNewsWidget.JourneyBasedFilter.UseVenueRegion"].Parse(false))
                {
                    #region Use venue region

                    // Only set region if there is one venue, use destination first
                    if (venueDestination != null && venueOrigin == null
                        && !string.IsNullOrEmpty(venueDestination.VenueTravelNewsRegion))
                    {
                        travelNewsState.SelectedRegion = venueDestination.VenueTravelNewsRegion;
                    }
                    else if (venueOrigin != null && venueDestination == null
                        && !string.IsNullOrEmpty(venueOrigin.VenueTravelNewsRegion))
                    {
                        travelNewsState.SelectedRegion = venueOrigin.VenueTravelNewsRegion;
                    }

                    #endregion
                }

                if (Properties.Current["TravelNewsWidget.JourneyBasedFilter.UseJourneyDate"].Parse(false))
                {
                    travelNewsState.SelectedDate = journeyRequest.OutwardDateTime;
                }
            }
            
            // Get the travel news headlines to show
            ITravelNewsHandler travelNewsService = SJPServiceDiscovery.Current.Get<ITravelNewsHandler>(ServiceDiscoveryKey.TravelNews);

            string regionFilterAndSort = Properties.Current["TravelNewsWidget.Regions.FilterAndSort"];

            HeadlineItem[] headlines = travelNewsService.GetHeadlines(travelNewsState, regionFilterAndSort);

            // If the selected region is all sort the headline items using the region sort order specified using property
            if (travelNewsState.SelectedRegion == "All")
            {
                TravelNewsHeadlineComparer comparer = new TravelNewsHeadlineComparer(regionFilterAndSort);

                List<HeadlineItem> headlinesToSort = headlines.ToList();

                headlinesToSort.Sort(comparer);

                headlines = headlinesToSort.ToArray();
            }
            
            // Group items                       
            var headlinesGroups = headlines.Select((hl, index) => new { hl, index })
                                    .GroupBy(g => g.index / Properties.Current["TravelNewsWidget.Headlines.Count"].Parse(6), i => i.hl);
                        

            // Only show paging if more than 1 page
            if (headlinesGroups.Count() > 1)
            {
                tnPaging.Visible = true;

                pageTotal.Text = headlinesGroups.Count().ToString();

                // Validate and set the current page index if not correct
                if (CurrentPageIndex < 1)
                    CurrentPageIndex = 1;
                if (CurrentPageIndex > headlinesGroups.Count())
                    CurrentPageIndex = headlinesGroups.Count();

                #region Display/hide paging buttons

                prev.Style[HtmlTextWriterStyle.Display] = "block";
                next.Style[HtmlTextWriterStyle.Display] = "block";

                if (CurrentPageIndex == 1)
                {
                    prev.Style[HtmlTextWriterStyle.Display] = "none";
                }

                if ((CurrentPageIndex == headlinesGroups.Count())
                    || (headlinesGroups.Count() == 1))
                {
                    next.Style[HtmlTextWriterStyle.Display] = "none";
                }
                #endregion
            }
            else
            {
                tnPaging.Visible = false;
            }

            if ((headlines.Length > 0) && (travelNewsService.IsTravelNewsAvaliable))
            {
                travelNewsHeadlines.Visible = true;
                lblNoIncidents.Visible = false;

                // Travel news available and incidents found
                travelNewsHeadlines.DataSource = headlinesGroups;
                travelNewsHeadlines.DataBind();
            }
            else if (travelNewsService.IsTravelNewsAvaliable)
            {
                // Travel news available but no incidents found for travel news state
                travelNewsHeadlines.Visible = false;
                lblNoIncidents.Visible = true;
                tnPaging.Visible = false;

                // Set specific no incidents message
                if (filterForJourneyVenues)
                {
                    lblNoIncidents.Text = page.GetResource("TravelNewsWidget.NoIncidents.ForJourney.Text");
                }
                else
                {
                    // No incidents
                    lblNoIncidents.Text = page.GetResource("TravelNewsWidget.NoIncidents.Text");
                }
                
                isAvailable = false;
            }
            else
            {
                // No headlines to show, or travel news not available hide the control
                isAvailable = false;
                this.Visible = false;
            }
        }

        #endregion
    }
}

