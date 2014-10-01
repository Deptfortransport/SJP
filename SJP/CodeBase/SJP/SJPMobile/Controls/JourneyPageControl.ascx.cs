// *********************************************** 
// NAME             : JourneyPageControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 14 Feb 2012
// DESCRIPTION  	: JourneyPage control to allow paging between the journeys
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.UserPortal.JourneyControl;
using SJP.Common.Extenders;
using SJP.Common.Web;
using SJP.Common.ResourceManager;
using System.Collections.Specialized;
using SJP.UserPortal.ScreenFlow;
using SJP.Common.LocationService;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// JourneyPage control to allow paging between the journeys
    /// </summary>
    public partial class JourneyPageControl : System.Web.UI.UserControl
    {
        #region Public Events

        // Show journey event declaration
        public event OnShowJourney ShowJourneyHandler;
        
        #endregion

        #region Private Fields

        private ISJPJourneyRequest journeyRequest = null;
        private ISJPJourneyResult journeyResult = null;
        private Journey journey;

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetupControls();
            }
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handler for the previous and next journey button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPreviousNext_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                int journeyId = ((Button)sender).CommandArgument.Parse(1);

                // Raise event to tell subscribers to that show journey button has been selected
                if (ShowJourneyHandler != null)
                {
                    ShowJourneyHandler(sender, new JourneyEventArgs(journeyId));
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise method
        /// </summary>
        public void Initialise(ISJPJourneyRequest journeyRequest, ISJPJourneyResult journeyResult, Journey journey)
        {
            this.journeyRequest = journeyRequest;
            this.journeyResult = journeyResult;
            this.journey = journey;
        }

        /// <summary>
        /// Refresh method to re-populate the controls, e.g. if initialise called after Page_Load
        /// </summary>
        public void Refresh()
        {
            SetupControls();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method which setup the controls
        /// </summary>
        private void SetupControls()
        {
            if (journeyRequest != null && journeyResult != null && journey != null)
            {
                if (journeyResult.OutwardJourneys.Count <= 1)
                {
                    // Only 1 journey in the result, no need to do paging
                    btnPrevious.Visible = false;
                    btnNext.Visible = false;
                    lnkPrevious.Visible = false;
                    lnkNext.Visible = false;
                }
                else
                {
                    // Determine which journey (index) is being displayed and set the previous/next buttons accordingly
                    int journeyId = journey.JourneyId;

                    List<Journey> journeys = journeyResult.OutwardJourneys;

                    // Sort the journeys to be same as displayed on the summary page:
                    if (journeyRequest.Destination is SJPVenueLocation)
                    {
                        journeys.Sort(JourneyComparer.SortJourneyArriveBy);
                    }
                    else
                    {
                        journeys.Sort(JourneyComparer.SortJourneyLeaveAfter);
                    }

                    // Assume journeys are sorted, and displayed on the summary page in the same order 
                    // they are found in this journeyresult
                    for (int i = 0; i < journeys.Count; i++)
                    {
                        Journey j = journeys[i];

                        // Index found for journey being displayed
                        if (j.JourneyId == journeyId)
                        {
                            // No previous journeys if index is at the start
                            btnPrevious.Visible = (i > 0);
                            lnkPrevious.Visible = (i > 0);

                            // No next journeys if index is at the end
                            btnNext.Visible = (i < journeys.Count - 1);
                            lnkNext.Visible = (i < journeys.Count - 1);

                            // Set the journey id to display on previous/next button click
                            if (btnPrevious.Visible)
                            {
                                string jid = journeys[i - 1].JourneyId.ToString();

                                btnPrevious.CommandArgument = jid;
                                lnkPrevious.NavigateUrl = GetPageLinkNavigateURL(journeyResult.JourneyRequestHash, jid);
                            }
                            if (btnNext.Visible)
                            {
                                string jid = journeys[i + 1].JourneyId.ToString();

                                btnNext.CommandArgument = jid;
                                lnkNext.NavigateUrl = GetPageLinkNavigateURL(journeyResult.JourneyRequestHash, jid);
                            }

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to setup the label text
        /// </summary>
        private void SetupResources()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            //btnNext.Text = page.GetResourceMobile("JourneyDetail.JourneyPaging.Next.Text");
            btnNext.ToolTip = page.GetResourceMobile("JourneyDetail.JourneyPaging.Next.ToolTip");
            //btnPrevious.Text = page.GetResourceMobile("JourneyDetail.JourneyPaging.Previous.Text");
            btnPrevious.ToolTip = page.GetResourceMobile("JourneyDetail.JourneyPaging.Previous.ToolTip");

            //lnkNext.Text = page.GetResourceMobile("JourneyDetail.JourneyPaging.Next.Text");
            lnkNext.ToolTip = page.GetResourceMobile("JourneyDetail.JourneyPaging.Next.ToolTip");
            //lnkPrevious.Text = page.GetResourceMobile("JourneyDetail.JourneyPaging.Previous.Text");
            lnkPrevious.ToolTip = page.GetResourceMobile("JourneyDetail.JourneyPaging.Previous.ToolTip");

            if (journey != null)
            {
                string TXT_Changes = page.GetResource(
                    SJPResourceManager.GROUP_JOURNEYOUTPUT, SJPResourceManager.COLLECTION_JOURNEY, "JourneyOutput.HeaderRow.Changes.Text");


                SummaryInstructionAdapter adapter = new SummaryInstructionAdapter(Global.SJPResourceManager, true);

                lblHeading.Text = string.Format("{0}: {1}, {2}",
                    page.GetResourceMobile("JourneyDetail.JourneyPaging.Heading.Text"),
                    string.Format("{0} {1}",
                        journey.InterchangeCount.ToString(),
                        TXT_Changes),
                    adapter.GetJourneyTime(journey.StartTime, journey.EndTime));
            }
        }

        /// <summary>
        /// Builds a url for displaying a specific journey on the current page
        /// </summary>
        /// <returns></returns>
        private string GetPageLinkNavigateURL(string journeyRequestHash, string journeyIdOutward)
        {
            SessionHelper sessionHelper = new SessionHelper();
            LandingPageHelper landingHelper = new LandingPageHelper();
            URLHelper urlHelper = new URLHelper();

            // Query string parameters
            NameValueCollection queryString = new NameValueCollection();

            queryString[QueryStringKey.JourneyRequestHash] = journeyRequestHash;
            queryString[QueryStringKey.JourneyIdOutward] = journeyIdOutward;

            // Landing Page
            Dictionary<string, string> dictLandingPageJO = landingHelper.BuildJourneyRequestForQueryString(
                sessionHelper.GetSJPJourneyRequest(journeyRequestHash));

            foreach (KeyValuePair<string, string> kvp in dictLandingPageJO)
            {
                queryString.Add(kvp.Key, kvp.Value);
            }

            // Get page transfer details for this page
            SJPPageMobile page = (SJPPageMobile)Page;
            PageTransferDetail transferDetail = page.GetPageTransferDetail(page.PageId);
            string transferUrl = transferDetail.PageUrl;

            // Append query string values set
            transferUrl = urlHelper.AddQueryStringParts(transferUrl, queryString);

            return transferUrl;
        }

        #endregion
    }
}