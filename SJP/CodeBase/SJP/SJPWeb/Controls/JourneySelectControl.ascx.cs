// *********************************************** 
// NAME             : JourneySelectControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 15 Nov 2011
// DESCRIPTION  	: Displays list of previously planned journeys in session to allow user to switch easily
// ************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.SJPWeb.Adapters;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// JourneySelectControl
    /// </summary>
    public partial class JourneySelectControl : System.Web.UI.UserControl
    {
        #region Private class

        /// <summary>
        /// JourneyItem used to bind the journeys dropdown list
        /// </summary>
        private class JourneyItem
        {
            private string journeyRequestHash;
            private string journeyName;

            public JourneyItem(string journeyRequestHash, string journeyName)
            {
                this.journeyRequestHash = journeyRequestHash;
                this.journeyName = journeyName;
            }

            public string JourneyRequestHash
            {
                get { return journeyRequestHash; }
            }

            public string JourneyName
            {
                get { return journeyName; }
            }
        }

        #endregion

        #region Private members

        private bool isForJourneyRequests = false;

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page Load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DebugHelper.ShowDebug)
            {
                debugInfoDiv.Visible = true;

                SetupResources();

                // Must setup the journeys list each time as journeys may have changed
                SetupJourneysList();
            }
        }

        /// <summary>
        /// Page PreRender event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupSelectedJourney();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for show journey button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowJourney_Click(object sender, EventArgs e)
        {
            ShowJourney();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialise method
        /// </summary>
        /// <param name="isForJourneyRequests">Indicates if the journey requests should be used to build the journeys list</param>
        public void Initialise(bool isForJourneyRequests)
        {
            this.isForJourneyRequests = isForJourneyRequests;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up resource contents
        /// </summary>
        private void SetupResources()
        {
            SJPPage page = (SJPPage)Page;

            lblJourneys.Text = page.GetResource("JourneySelectControl.lblJourneys.Text");
            btnShowJourney.Text = page.GetResource("JourneySelectControl.btnShowJourney.Text");
            btnShowJourney.ToolTip = page.GetResource("JourneySelectControl.btnShowJourney.Text");
            lblJourneys.Text = "Previous";
            btnShowJourney.Text = "Show";
            btnShowJourney.ToolTip = "Show journey";
        }

        /// <summary>
        /// Sets up the journeys drop down list
        /// </summary>
        private void SetupJourneysList()
        {
            SJPSessionManager sessionManager = (SJPSessionManager)SJPSessionManager.Current;

            #region Get the journey items

            List<JourneyItem> journeyItems = new List<JourneyItem>();

            if (isForJourneyRequests)
            {
                SJPRequestManager requestManager = sessionManager.RequestManager;

                // Get all the results
                Dictionary<string, ISJPJourneyRequest> journeyRequests = requestManager.SJPJourneyRequests;

                if (journeyRequests != null)
                {
                    List<ISJPJourneyRequest> journeys = new List<ISJPJourneyRequest>(journeyRequests.Values);

                    journeyItems = GetJourneysData(journeys);
                }
            }
            else
            {
                SJPResultManager resultManager = sessionManager.ResultManager;

                // Get all the results
                Dictionary<string, ISJPJourneyResult> journeyResults = resultManager.SJPJourneyResults;

                if (journeyResults != null)
                {
                    List<ISJPJourneyResult> journeys = new List<ISJPJourneyResult>(journeyResults.Values);

                    journeyItems = GetJourneysData(journeys);
                }
            }

            #endregion

            if (journeyItems.Count > 0)
            {
                // Retain selected value
                string selectedValue = string.Empty;

                if (Page.IsPostBack && (!string.IsNullOrEmpty(journeysList.SelectedValue)))
                {
                    selectedValue = journeysList.SelectedValue;
                }

                // Update the journeys dropdown list
                journeysList.DataSource = journeyItems;
                journeysList.DataTextField = "journeyName";
                journeysList.DataValueField = "journeyRequestHash";

                journeysList.DataBind();

                // Set the selected value
                if (Page.IsPostBack && (!string.IsNullOrEmpty(selectedValue)))
                {
                    // Place in try catch because journey result may no longer exist 
                    // from the session because of new journey requests
                    try
                    {
                        journeysList.SelectedValue = selectedValue;
                    }
                    catch
                    {
                        // ignore exceptions
                    }
                }
            }
        }

        /// <summary>
        /// Returns the journeys data to bind to the journeys dropdown list
        /// </summary>
        /// <param name="journeys"></param>
        /// <returns></returns>
        private List<JourneyItem> GetJourneysData(List<ISJPJourneyRequest> journeyRequests)
        {
            List<JourneyItem> journeysData = new List<JourneyItem>();

            if (journeyRequests != null)
            {
                string journeyName = string.Empty;

                foreach (ISJPJourneyRequest jr in journeyRequests)
                {
                    journeyName = string.Format("{0} to {1} {2}",
                        jr.Origin.DisplayName, jr.Destination.DisplayName, jr.OutwardDateTime.ToString());

                    journeysData.Add(new JourneyItem(jr.JourneyRequestHash, journeyName));
                }
            }

            if (journeysData.Count == 0)
            {
                // Add an empty row
                journeysData.Add(new JourneyItem(string.Empty, "No journeys"));
            }

            return journeysData;
        }

        /// <summary>
        /// Returns the journeys data to bind to the journeys dropdown list
        /// </summary>
        /// <param name="journeys"></param>
        /// <returns></returns>
        private List<JourneyItem> GetJourneysData(List<ISJPJourneyResult> journeyResults)
        {
            List<JourneyItem> journeysData = new List<JourneyItem>();

            if (journeyResults != null)
            {
                string journeyName = string.Empty;

                foreach (ISJPJourneyResult jr in journeyResults)
                {
                    journeyName = string.Format("{0} to {1} {2}",
                        jr.Origin.DisplayName, jr.Destination.DisplayName, jr.OutwardDateTime.ToString());

                    journeysData.Add(new JourneyItem(jr.JourneyRequestHash, journeyName));
                }
            }

            if (journeysData.Count == 0)
            {
                // Add an empty row
                journeysData.Add(new JourneyItem(string.Empty, "No journeys"));
            }

            return journeysData;
        }

        /// <summary>
        /// Sets the currently selected journey in the journeys dropdown list
        /// </summary>
        private void SetupSelectedJourney()
        {
            if ((journeysList != null) && (journeysList.Items.Count > 0))
            {
                JourneyHelper journeyHelper = new JourneyHelper();
                string jrh = journeyHelper.GetJourneyRequestHash();
                
                // Place in try catch because journey result may no longer exist 
                // from the session because of new journey requests
                try
                {
                    journeysList.SelectedValue = jrh;
                }
                catch
                {
                    // ignore exceptions
                }
            }
        }

        /// <summary>
        /// Posts page to show the selected journey
        /// </summary>
        private void ShowJourney()
        {
            if ((journeysList != null) && (!string.IsNullOrEmpty(journeysList.SelectedValue)))
            {
                // Safest way to display the required journey is to perform a page landing,
                // which should go to the options page if loading from journey results,
                // or the input page if loading from journey requests

                SJPPage page = (SJPPage)Page;
                LandingPageHelper landingHelper = new LandingPageHelper();
                SessionHelper sessionHelper = new SessionHelper();
                URLHelper urlHelper = new URLHelper();

                // Page is either results or input page
                Common.PageId pageId = (isForJourneyRequests) ? Common.PageId.JourneyPlannerInput : Common.PageId.JourneyOptions;

                #region Query string

                // Have to duplicate the build querystring code as need to 
                // specify the selected journey request hash
                NameValueCollection redirectQueryString = new NameValueCollection();
                
                // Only add query string for results page
                if (!isForJourneyRequests)
                {
                    redirectQueryString[QueryStringKey.JourneyRequestHash] = journeysList.SelectedValue;

                    // Landing page querystring values
                    Dictionary<string, string> dictLandingPageJO = landingHelper.BuildJourneyRequestForQueryString(
                            sessionHelper.GetSJPJourneyRequest(journeysList.SelectedValue));

                    foreach (KeyValuePair<string, string> kvp in dictLandingPageJO)
                    {
                        redirectQueryString[kvp.Key] = kvp.Value;
                    }
                }

                #endregion

                // Get page transfer details
                PageTransferDetail transferDetail = page.GetPageTransferDetail(pageId);
                string transferUrl = transferDetail.PageUrl;

                // Append query string values set
                transferUrl = urlHelper.AddQueryStringParts(transferUrl, redirectQueryString);

                // Update session with selected journey request hash,
                // allows input page to load currently selected journey
                sessionHelper.UpdateSession(journeysList.SelectedValue);

                // And do the redirect here.
                // Can't use the Page SetPageTransfer functionality as we may already be on the JourneyOptions
                // page, and would require complicating the code logic on the JourneyOptions page to load 
                // the required result
                Response.Redirect(transferUrl);
            }
        }

        #endregion
    }
}