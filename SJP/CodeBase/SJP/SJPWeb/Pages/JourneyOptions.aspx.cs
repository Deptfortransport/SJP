// *********************************************** 
// NAME             : JourneyOptions.aspx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: JourneyOptions page contains the logic and controls 
//                    to represent journey result options to user 
// ************************************************


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.Reporting.Events;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.SJPWeb.Adapters;
using SJP.UserPortal.SJPWeb.Controls;
using Logger = System.Diagnostics.Trace;
using SJPWC = SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Pages
{
    public partial class JourneyOptions : SJPPage
    {
        #region Variables
        
        private const string NO_RESULT_INFO = "JourneyOptions.NoResultsFound.UserInfo";
        private const string NO_RESULT_ERROR = "JourneyOptions.NoResultsFound.Error";

        // Used to log page action events
        private bool logPageActionEvent = false;
        private bool journeyResultAvailable = false;
                
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyOptions()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.JourneyOptions;
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupClientSideSettings();
            // Add jquery ui css
            AddStyleSheet("jquery-ui-1.8.13.css");
            AddStyleSheet("jquery.qtip.min.css");

            // Add javascripts
            AddJavascript("Common.js");
            AddJavascript("JourneyOptions.js");
            AddJavascript("jquery-ui-1.8.13.min.js");
            AddJavascript("jquery.qtip.min.js");
            AddJavascript("jquery.ui.selectmenu.js");

            // Attach to selected journey changed events
            outwardSummaryControl.SelectedJourneyHandler += new OnSelectedJourneyChange(SelectedOutwardJourneyChangeEvent);
            returnSummaryControl.SelectedJourneyHandler += new OnSelectedJourneyChange(SelectedReturnJourneyChangeEvent);

            outwardSummaryControl.SelectedJourneySummaryLegDetailsHandler += new OnSelectedJourneySummaryLegDetailsChange(SelectedOutwardJourneyLegChangeHandler);
            returnSummaryControl.SelectedJourneySummaryLegDetailsHandler += new OnSelectedJourneySummaryLegDetailsChange(SelectedReturnJourneyLegChangeHandler);

            // Attach to the replan journey events
            outwardSummaryControl.ReplanJourneyHandler += new OnReplanJourney(ReplanOutwardJourneyEvent);
            returnSummaryControl.ReplanJourneyHandler += new OnReplanJourney(ReplanReturnJourneyEvent);

            // Enable the page auto refresh to allow checking for journeys
            if (!IsPostBack)
            {
                SetupTimer();
            }

            longWaitMessageLink.NavigateUrl = Request.Url.ToString();

            JourneyResultHelper resultHelper = new JourneyResultHelper();

            if (waitTimer.Enabled)
            {
                int waitCount = UpdateWaitCount();

                // Tracks if a page action event should be logged in the PreRender event of page
                logPageActionEvent = true;
                
                // Base page shouldn't log the page entry because the page action event is logged 
                // by this page (in posbacks only)
                if (IsPostBack)
                {
                    logPageEntry = false;
                }

                journeyResultAvailable = resultHelper.IsJourneyResultAvailable;
                
                if (journeyResultAvailable)
                {
                    #region Bind results

                    // Journeys have returned, display them
                    ISJPJourneyResult journeyResult = resultHelper.JourneyResult;
                    if (journeyResult.Messages.Count > 0)
                    {
                        // Journey Result returned with error messages
                        foreach (SJPMessage message in journeyResult.Messages)
                        {
                            DisplayMessage(message);
                        }

                        // Journey Result returned with error message... Add instruction for user
                        DisplayMessage(new SJPMessage(NO_RESULT_INFO, SJPMessageType.Info));
                    }

                    // Display journeys if they exist
                    if ((journeyResult.OutwardJourneys.Count > 0)
                        || (journeyResult.ReturnJourneys.Count > 0))
                    {
                        // Journey result returned without errors, Bind journey result to journey result controls
                        BindJourneyResult(resultHelper.JourneyRequest, journeyResult);
                        BindTicketButton();
                        BindPrinterFriendlyButton();

                        // Display journey summary info text
                        journeyInfoDiv1.Visible = true;
                        journeyInfoDiv2.Visible = true;
                    }
                    else
                    {
                        // write a no results event
                        NoResultsEvent nre = new NoResultsEvent(DateTime.Now, SJPSessionManager.Current.Session.SessionID,false);
                        Logger.Write(nre);
                    }

                    // Stop refresh
                    journeyProgress.Visible = false;
                    waitTimer.Enabled = false;

                    #endregion
                }
                else
                {
                    // If results not available and have exceeded the wait count, then display error
                    int maxWaitCount = Properties.Current["JourneyOptions.Wait.RefreshCount.Max"].Parse(12);

                    if (waitCount > maxWaitCount)
                    {
                        DisplayMessage(new SJPMessage(NO_RESULT_ERROR, SJPMessageType.Error));
                        DisplayMessage(new SJPMessage(NO_RESULT_INFO, SJPMessageType.Info));
                        
                        // write a no results event
                        NoResultsEvent nre = new NoResultsEvent(DateTime.Now, SJPSessionManager.Current.Session.SessionID, false);
                        Logger.Write(nre);
                        
                        // Stop refresh
                        journeyProgress.Visible = false;
                        waitTimer.Enabled = false;
                    }
                }
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

            SetupDebugInformation();

            #region Log page action event for postbacks

            if (logPageActionEvent)
            {
                // Log in postbacks only because Results could have been available on first render of this page
                // and therefore don't need to log this page action event
                if (IsPostBack && !journeyResultAvailable)
                {
                    // Log an event because user is still waiting for journey results
                    PageEntryEvent logPage = new PageEntryEvent(Common.PageId.JourneyOptionsWait, SJPSessionManager.Current.Session.SessionID, false);
                    Logger.Write(logPage);
                }
                else if (IsPostBack && journeyResultAvailable)
                {
                    // Log an event because journey results are now available to user
                    PageEntryEvent logPage = new PageEntryEvent(Common.PageId.JourneyOptionsResult, SJPSessionManager.Current.Session.SessionID, false);
                    Logger.Write(logPage);
                }
            }

            #endregion
        }

        #endregion

        #region Control Event Handlers
        /// <summary>
        /// Tick event handler for the ajax wait timer
        /// WaitTimer simulates the wait page scenario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WaitTimer_Tick(object sender, EventArgs e)
        {
            
        }

       
        #endregion

        #region Event handlers

        /// <summary>
        /// Event handler for Tickets (Retail handoff) button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTickets_Click(object sender, EventArgs e)
        {
            // If there are multiple retailers for the seleced journeys,
            // then display the Retailers page, otherwise send directly 
            // to the RetailerHandoff page.
                        
            string journeyRequestHash = null;
            Journey journeyOutward = null;
            Journey journeyReturn = null;

            // Find any retailers which can ticket the journeys
            List<Retailer> retailers = RetailersForJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            if (retailers.Count > 1)
            {
                // Multiple retailers, send to Retailers page
                SetPageTransfer(PageId.Retailers);
                AddQueryStringForPage(PageId.Retailers);
            }
            else if (retailers.Count == 1)
            {
                // The Ticket button will have javascript attached 
                // to open a new window to do the retail handoff.
                // Only need to display message
                DisplayMessage(new SJPMessage("Retailers.Message.JourneyHandedOff.Text", SJPMessageType.Info));
            }

            // Ensure tickets button is still correct
            BindTicketButton();
        }

        /// <summary>
        /// Event handler for Back button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Check planner mode of request to determine where to return
            JourneyHelper journeyHelper = new JourneyHelper();

            string journeyRequestHash = journeyHelper.GetJourneyRequestHash();

            SessionHelper sessionHelper = new SessionHelper();

            ISJPJourneyRequest journeyRequest = sessionHelper.GetSJPJourneyRequest(journeyRequestHash);

            if ((journeyRequest != null) && (journeyRequest.PlannerMode != SJPJourneyPlannerMode.PublicTransport))
            {
                // Take to intermediate locations page
                SetPageTransfer(PageId.JourneyLocations);
            }
            else
            {
                // Default to input page
                SetPageTransfer(PageId.JourneyPlannerInput);
            }
        }

        #region Selected journey events

        /// <summary>
        /// Updates the selected journey in session
        /// </summary>
        /// <param name="journeyId"></param>
        protected void SelectedOutwardJourneyChangeEvent(object sender, JourneyEventArgs e)
        {
            JourneyHelper journeyHelper = new JourneyHelper();

            journeyHelper.SetJourneySelected(true, e.JourneyId);

            BindTicketButton();
            BindPrinterFriendlyButton();
        }

        /// <summary>
        /// Updates the selected journey in session
        /// </summary>
        /// <param name="journeyId"></param>
        protected void SelectedReturnJourneyChangeEvent(object sender, JourneyEventArgs e)
        {
            JourneyHelper journeyHelper = new JourneyHelper();

            journeyHelper.SetJourneySelected(false, e.JourneyId);

            BindTicketButton();
            BindPrinterFriendlyButton();
        }

        /// <summary>
        /// Updates the printer friendly button to indicate the displayed journey leg details
        /// </summary>
        /// <param name="journeyId"></param>
        protected void SelectedOutwardJourneyLegChangeHandler(object sender, JourneyLegEventArgs e)
        {
            if (e != null)
            {
                JourneyHelper journeyHelper = new JourneyHelper();

                journeyHelper.SetJourneyLegDetailExpanded(true, e.Expanded);
            }

            BindPrinterFriendlyButton();
        }

        /// <summary>
        /// Updates the printer friendly button to indicate the displayed journey leg details
        /// </summary>
        /// <param name="journeyId"></param>
        protected void SelectedReturnJourneyLegChangeHandler(object sender, JourneyLegEventArgs e)
        {
            if (e != null)
            {
                JourneyHelper journeyHelper = new JourneyHelper();

                journeyHelper.SetJourneyLegDetailExpanded(false, e.Expanded);
            }

            BindPrinterFriendlyButton();
        }

        #endregion

        #region Replan journey events

        /// <summary>
        /// Replans the outward journey for the current request
        /// </summary>
        protected void ReplanOutwardJourneyEvent(object sender, ReplanJourneyEventArgs e)
        {
            if (e != null)
            {
                ReplanJourney(true, e.IsEarlier);
            }
        }

        /// <summary>
        /// Replans the return journey for the current request
        /// </summary>
        protected void ReplanReturnJourneyEvent(object sender, ReplanJourneyEventArgs e)
        {
            if (e != null)
            {
                ReplanJourney(false, e.IsEarlier);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            btnBack.Text = Server.HtmlDecode(GetResource("JourneyOptions.Back.Text"));
            btnBack.ToolTip = Server.HtmlDecode(GetResource("JourneyOptions.Back.ToolTip"));
            btnTickets.Text = Server.HtmlDecode(GetResource("JourneyOptions.Tickets.Text"));
            btnTickets.ToolTip = Server.HtmlDecode(GetResource("JourneyOptions.Tickets.ToolTip"));

            loading.ImageUrl = ImagePath + GetResource("JourneyOptions.Loading.Imageurl");
            loading.ToolTip = loading.AlternateText = Server.HtmlDecode(GetResource("JourneyOptions.Loading.AlternateText"));

            loadingMessage.Text = Server.HtmlDecode(GetResource("JourneyOptions.loadingMessage.Text"));

            longWaitMessage.Text = Server.HtmlDecode(GetResource("JourneyOptions.LongWaitMessage.Text"));
            longWaitMessageLink.Text = Server.HtmlDecode(GetResource("JourneyOptions.LongWaitMessageLink.Text"));
            longWaitMessageLink.ToolTip = Server.HtmlDecode(GetResource("JourneyOptions.LongWaitMessageLink.ToolTip"));

            lblJourneyInfo1.Text = GetResource("JourneyOptions.JourneyInfo1.Text");
            lblJourneyInfo2.Text = GetResource("JourneyOptions.JourneyInfo2.Text");
            lnkJourneyFAQs.Text = GetResource("JourneyOptions.JourneyInfoFAQLink.Text");
            lnkJourneyFAQs.ToolTip = GetResource("JourneyOptions.JourneyInfoFAQLink.ToolTip");
            lnkJourneyFAQs.NavigateUrl = GetResource("JourneyOptions.JourneyInfoFAQLink.Url");

            tooltip_information.Title = GetResource("JourneyOptions.BookTicketsInfo.ToolTip");
            bookTicketsInfo.AlternateText = GetResource("JourneyOptions.BookTicketsInfo.AlternateText");
            bookTicketsInfo.ToolTip = GetResource("JourneyOptions.BookTicketsInfo.ToolTip");
            bookTicketsInfo.ImageUrl = ImagePath + GetResource("JourneyOptions.BookTicketsInfo.Imageurl");
            
            openInNewWindow.AlternateText = openInNewWindow.ToolTip = GetResource("OpenInNewWindow.AlternateText");
            openInNewWindow.ImageUrl = ImagePath + GetResource("OpenInNewWindow.URL");
        }

        /// <summary>
        /// Initialises the ajax timer control
        /// </summary>
        private void SetupTimer()
        {
            int refreshSecs = Properties.Current["JourneyOptions.Wait.RefreshTime.Seconds"].Parse(5);

            waitTimer.Interval = refreshSecs * 1000; // In millisecs
        }

        /// <summary>
        /// Updates the count of page wait refresh
        /// </summary>
        private int UpdateWaitCount()
        {
            // Read count from hidden field
            int count = waitCount.Value.Parse(0);

            // Increment
            count = count + 1;

            // Persist in hidden field
            waitCount.Value = count.ToString();

            return count;
        }
        
        /// <summary>
        /// Binds journey result to outward and return detail summary result controls
        /// </summary>
        /// <param name="journeyRequest">ISJPJourneyRequest object</param>
        /// <param name="journeyResult">ISJPJourneyResult object</param>
        private void BindJourneyResult(ISJPJourneyRequest journeyRequest, ISJPJourneyResult journeyResult)
        {
            if (journeyResult != null)
            {
                // Determine if journey should be expanded by default
                bool showExpanded =  Properties.Current["JourneyOptions.ShowJourneyExpanded.Switch"].Parse(true);
                bool showSingleExpanded = Properties.Current["JourneyOptions.ShowSingleJourneyExpanded.Switch"].Parse(true);

                // If the leg detail should be expanded
                bool detailOutwardExpanded = false;
                bool detailReturnExpanded = false;

                // Determine if the details control should render in accessible mode
                bool accessible = (CurrentStyle.AccessibleStyleValue != AccessibleStyle.Normal);

                JourneyHelper journeyHelper = new JourneyHelper();

                journeyHelper.GetJourneyLegDetailExpanded(out detailOutwardExpanded, out detailReturnExpanded);

                if (journeyResult.OutwardJourneys != null)
                {
                    if (journeyResult.OutwardJourneys.Count > 0)
                    {
                        // Default to not expanded
                        int jid = 0;

                        if (showExpanded) // If show default journey expanded
                        {
                            jid = journeyHelper.GetJourneySelected(true);
                        }
                        else if ((showSingleExpanded) && (journeyResult.OutwardJourneys.Count == 1)) // If there is only 1 journey, and show expanded is set
                        {
                            jid = journeyHelper.GetJourneySelected(true);
                        }

                        outwardSummaryControl.Initialise(journeyRequest, journeyResult, false, jid, detailOutwardExpanded, false, accessible);
                        outwardSummaryControl.Visible = true;
                    }
                }

                if (journeyResult.ReturnJourneys != null)
                {
                    if (journeyResult.ReturnJourneys.Count > 0)
                    {
                        // Default to not expanded
                        int jid = 0;

                        if (showExpanded) // If show default journey expanded
                        {
                            jid = journeyHelper.GetJourneySelected(false);
                        }
                        else if ((showSingleExpanded) && (journeyResult.ReturnJourneys.Count == 1)) // If there is only 1 journey, and show expanded is set
                        {
                            jid = journeyHelper.GetJourneySelected(false);
                        }

                        returnSummaryControl.Initialise(journeyRequest, journeyResult, true, jid, detailReturnExpanded, false, accessible);
                        returnSummaryControl.Visible = true;
                    }
                }                
            }
        }

        /// <summary>
        /// Displays the printer friendly button
        /// </summary>
        /// <param name="journeyResult"></param>
        private void BindPrinterFriendlyButton()
        {
            JourneyResultHelper resultHelper = new JourneyResultHelper();

            bool resultAvailables = resultHelper.IsJourneyResultAvailable;

            // Journeys have returned, display the printer friendly button
            ISJPJourneyResult journeyResult = resultHelper.JourneyResult;

            if ((resultAvailables) && (journeyResult != null) &&
                ((journeyResult.OutwardJourneys != null && journeyResult.OutwardJourneys.Count > 0)
                 || (journeyResult.ReturnJourneys != null && journeyResult.ReturnJourneys.Count > 0)
                )
               )
            {
                // Retrieve printer friendly page url
                string printerFriendlyUrl = PrinterFriendlyPageURL();
                
                if (!string.IsNullOrEmpty(printerFriendlyUrl))
                {
                    JourneyHelper journeyHelper = new JourneyHelper();

                    string journeyRequestHash = null;
                    Journey journeyOutward = null;
                    Journey journeyReturn = null;
                                        
                    bool detailOutwardExpanded = false;
                    bool detailReturnExpanded = false;

                    // Retrieve selected journeys
                    journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

                    journeyHelper.GetJourneyLegDetailExpanded(out detailOutwardExpanded, out detailReturnExpanded);

                    URLHelper urlHelper = new URLHelper();

                    if (!string.IsNullOrEmpty(journeyRequestHash))
                    {
                        printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyRequestHash, journeyRequestHash);
                    }
                    if (journeyOutward != null)
                    {
                        printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyIdOutward, journeyOutward.JourneyId.ToString());

                        if (detailOutwardExpanded)
                        {   
                            // Leg Detail should be expanded
                            printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyLegDetailOutward, "1");
                        }
                    }
                    if (journeyReturn != null)
                    {
                        printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyIdReturn, journeyReturn.JourneyId.ToString());

                        if (detailReturnExpanded)
                        {
                            // Leg Detail should be expanded
                            printerFriendlyUrl = urlHelper.AddQueryStringPart(printerFriendlyUrl, QueryStringKey.JourneyLegDetailReturn, "1");
                        }
                    }

                    // Find printer friendly button on master page
                    ContentPlaceHolder masterHeadingContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("contentHeading");

                    if (masterHeadingContentPlaceHolder != null)
                    {
                        SJPWC.LinkButton  btnPrinterFriendly = (SJPWC.LinkButton)masterHeadingContentPlaceHolder.FindControl("btnPrinterFriendly");

                        if (btnPrinterFriendly != null)
                        {
                            btnPrinterFriendly.Text = GetResource("JourneyOptions.PrinterFriendly.Text");
                            btnPrinterFriendly.ToolTip = GetResource("JourneyOptions.PrinterFriendly.ToolTip");
                            btnPrinterFriendly.OnClientClick = string.Format("openWindow('{0}')", printerFriendlyUrl);
                            btnPrinterFriendly.Visible = true;
                        }

                        #region Hyperlink (for non-javascript)

                        HyperLink lnkPrinterFriendly = (HyperLink)masterHeadingContentPlaceHolder.FindControl("lnkPrinterFriendly");

                        if (lnkPrinterFriendly != null)
                        {
                            lnkPrinterFriendly.Text = GetResource("JourneyOptions.PrinterFriendly.Text");
                            lnkPrinterFriendly.ToolTip = GetResource("JourneyOptions.PrinterFriendly.ToolTip");
                            lnkPrinterFriendly.NavigateUrl = printerFriendlyUrl;
                        }

                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Displays the buy ticket button, and enables it if the selected journey(s) have
        /// retailer(s) to allow travel tickets to be bought
        /// </summary>
        private void BindTicketButton()
        {
            string journeyRequestHash = null;
            Journey journeyOutward = null;
            Journey journeyReturn = null;
            
            // Find any retailers which can ticket the journeys
            List<Retailer> retailers = RetailersForJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            btnTickets.Visible = true;
            bookTicketsInfo.Visible = true;
            tooltip_information.Visible = true;

            // Disable the button
            if (retailers.Count == 0)
            {
                btnTickets.Attributes["disabled"] = "disabled";
            }
            else
            {
                btnTickets.Attributes.Remove("disabled");
            }
            
            // Don't display button for Car or Cycle journeys
            if (((journeyOutward != null) && (journeyOutward.IsCarJourney() || journeyOutward.IsCycleJourney()))
               || ((journeyReturn != null) && (journeyReturn.IsCarJourney() || journeyReturn.IsCycleJourney())))
            {
                btnTickets.Visible = false;
                bookTicketsInfo.Visible = false;
                tooltip_information.Visible = false;
            }

            // If there is only 1 retailer, then want to send directly 
            // to the RetailerHandoff page
            if (retailers.Count == 1)
            {
                string retailerHandoffUrl = RetailHandoffPageURL();

                if (!string.IsNullOrEmpty(retailerHandoffUrl))
                {
                    RetailerHelper retailerHelper = new RetailerHelper();

                    // Get the handoff url, this will load the aspx to build the handoff xml and post to retailer
                    string handoffUrl = retailerHelper.BuildRetailerHandoffURL(retailerHandoffUrl, retailers[0],
                        journeyRequestHash, journeyOutward, journeyReturn);

                    // Add handoff url
                    if (!string.IsNullOrEmpty(handoffUrl))
                    {
                        btnTickets.OnClientClick = string.Format("openWindow('{0}')", handoffUrl);
                    }
                }
                else
                {
                    // Don't display button
                    btnTickets.Visible = false;
                    bookTicketsInfo.Visible = false;
                    tooltip_information.Visible = false;
                }
            }
        }

        /// <summary>
        /// Returns a list of Retailers applicable to the journeys currently selected
        /// </summary>
        /// <param name="journeyRequestHash"></param>
        /// <param name="journeyOutward"></param>
        /// <param name="journeyReturn"></param>
        /// <returns></returns>
        private List<Retailer> RetailersForJourneys(out string journeyRequestHash, out  Journey journeyOutward, out Journey journeyReturn)
        {
            JourneyHelper journeyHelper = new JourneyHelper();
            RetailerHelper retailerHelper = new RetailerHelper();

            journeyRequestHash = null;
            journeyOutward = null;
            journeyReturn = null;

            // Retrieve selected journeys
            journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            // Find any retailers which can ticket the journeys
            List<Retailer> retailers = retailerHelper.GetRetailersForJourneys(journeyOutward, journeyReturn);
            
            return retailers;
        }

        /// <summary>
        /// Returns the retailer handoff page url, or empty string if not found
        /// </summary>
        private string RetailHandoffPageURL()
        {
            PageTransferDetail ptd = GetPageTransferDetail(PageId.RetailerHandoff);

            if (ptd != null)
            {
                return ResolveClientUrl(ptd.PageUrl);
            }
            else
            {
                // This shouldnt happen, but may do if page hasn't been setup correctly
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    "RetailerHandoff page url was not found, sitemap may be missing the page definition. Unable to perform retail handoff."));
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns the printer friendly page url, or empty string if not found
        /// </summary>
        private string PrinterFriendlyPageURL()
        {
            PageTransferDetail ptd = GetPageTransferDetail(PageId.PrintableJourneyOptions);

            if (ptd != null)
            {
                return ResolveClientUrl(ptd.PageUrl);
            }

            return string.Empty;
        }

        /// <summary>
        /// Method to replan the journey (for earlier or later) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplanJourney(bool isOutward, bool isEarlier)
        {
            JourneyPlannerInputAdapter journeyPlannerAdapter = new JourneyPlannerInputAdapter();
            JourneyHelper journeyHelper = new JourneyHelper();
            SessionHelper sessionHelper = new SessionHelper();
            LandingPageHelper landingHelper = new LandingPageHelper();
            URLHelper urlHelper = new URLHelper();

            // Get the request hash
            string jrh = journeyHelper.GetJourneyRequestHash();

            // Get the request and result to be replanned
            ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest(jrh);
            ISJPJourneyResult sjpJourneyResult = sessionHelper.GetSJPJourneyResult(jrh);

            #region Set the replan values

            bool replanOutwardRequired = false;
            bool replanReturnRequired = false;
            DateTime replanOutwardDateTime = DateTime.MinValue;
            DateTime replanReturnDateTime = DateTime.MinValue;
            bool replanOutwardArriveBefore = false;
            bool replanReturnArriveBefore = false;

            int earlierIntervalMins = Properties.Current["JourneyOptions.Replan.Earlier.Interval.Minutes"].Parse(1);
            int laterIntervalMins = Properties.Current["JourneyOptions.Replan.Later.Interval.Minutes"].Parse(1);

            bool retainPreviousJourneys = Properties.Current["JourneyOptions.Replan.RetainPreviousJourneys.Switch"].Parse(false);
            bool retainPreviousJourneysWhenNoResults = Properties.Current["JourneyOptions.Replan.RetainPreviousJourneysWhenNoResults.Switch"].Parse(false);

            // Only replan the selected journey direction
            
            if (isOutward)
            {
                replanOutwardRequired = true;

                DateTime dt = GetJourneyDateTime(sjpJourneyResult.OutwardJourneys, isEarlier);

                // If Earlier, then date time is 1 minute before earliest "arrive time" in all the journeys
                if (isEarlier)
                {
                    replanOutwardDateTime = dt.Subtract(new TimeSpan(0, earlierIntervalMins, 0));
                    replanOutwardArriveBefore = true;
                }
                // If Later, then date time is 1 minute after latest "leave time" in all the journeys
                else
                {
                    replanOutwardDateTime = dt.Add(new TimeSpan(0, laterIntervalMins, 0));
                    replanOutwardArriveBefore = false;
                }
            }
            else
            {
                replanReturnRequired = true;

                DateTime dt = GetJourneyDateTime(sjpJourneyResult.ReturnJourneys, isEarlier);

                // If Earlier, then date time is 1 minute before earliest "arrive time" in all the journeys
                if (isEarlier)
                {
                    replanReturnDateTime = dt.Subtract(new TimeSpan(0, earlierIntervalMins, 0));
                    replanReturnArriveBefore = true;
                }
                // If Later, then date time is 1 minute after latest "leave time" in all the journeys
                else
                {
                    replanReturnDateTime = dt.Add(new TimeSpan(0, laterIntervalMins, 0));
                    replanReturnArriveBefore = false;
                }
            }

            #endregion

            // Create the replan request
            journeyPlannerAdapter.ValidateAndBuildSJPRequestForReplan(sjpJourneyRequest,
                replanOutwardRequired, replanReturnRequired,
                replanOutwardDateTime, replanReturnDateTime,
                replanOutwardArriveBefore, replanReturnArriveBefore,
                sjpJourneyResult.OutwardJourneys, sjpJourneyResult.ReturnJourneys,
                (retainPreviousJourneys || !replanOutwardRequired), (retainPreviousJourneys || !replanReturnRequired),
                retainPreviousJourneysWhenNoResults, retainPreviousJourneysWhenNoResults // Ensure existing journeys are reshown if replan fails
                );

            #region Submit

            // Submit the request
            JourneyPlannerHelper journeyPlannerHelper = new JourneyPlannerHelper();

            if (journeyPlannerHelper.SubmitRequest(SJPJourneyPlannerMode.PublicTransport, true))
            {
                // And redirect to the options page
                PageId pageId = PageId.JourneyOptions;

                #region Query string

                // Have to duplicate the build querystring code as need to 
                // specify the selected journey request hash
                NameValueCollection redirectQueryString = new NameValueCollection();

                // Only add query string for results page
                redirectQueryString[QueryStringKey.JourneyRequestHash] = sessionHelper.GetSJPJourneyRequest().JourneyRequestHash;

                // Landing page querystring values
                Dictionary<string, string> dictLandingPageJO = landingHelper.BuildJourneyRequestForQueryString(
                        sessionHelper.GetSJPJourneyRequest());

                foreach (KeyValuePair<string, string> kvp in dictLandingPageJO)
                {
                    redirectQueryString[kvp.Key] = kvp.Value;
                }

                #endregion

                // Get page transfer details
                PageTransferDetail transferDetail = GetPageTransferDetail(pageId);
                string transferUrl = transferDetail.PageUrl;

                // Append query string values set
                transferUrl = urlHelper.AddQueryStringParts(transferUrl, redirectQueryString);

                // And do the redirect here.
                // Can't use the Page SetPageTransfer functionality as if already on the JourneyOptions
                // page, and would require complicating the code logic on the JourneyOptions page to load 
                // the required result
                Response.Redirect(transferUrl);
            }

            #endregion
        }

        /// <summary>
        /// Returns the earliest/latest datetime for the journeys
        /// If isEarlier, then the Arrive times are searched and the earliest arrive time is returned
        /// Else, then the latest Leave times are searched and the latest leave time is returned
        /// </summary>
        /// <param name="journeys"></param>
        /// <param name="isEarlier"></param>
        /// <returns></returns>
        private DateTime GetJourneyDateTime(List<Journey> journeys, bool isEarlier)
        {
            DateTime dt = DateTime.MinValue;

            if (journeys != null)
            {
                if (isEarlier)
                {
                    journeys.Sort(JourneyComparer.SortJourneyArriveBy);

                    // Last journey should have the earliest arrive by datetime
                    dt = journeys[journeys.Count - 1].EndTime;
                }
                else
                {
                    journeys.Sort(JourneyComparer.SortJourneyLeaveAfter);

                    // Last journey should have the latest leave after datetime
                    dt = journeys[journeys.Count - 1].StartTime;
                }
            }

            return dt;
        }

        /// <summary>
        /// Sets up hidden fields which can be used client side to enable retailer handoff and printer friendly
        /// </summary>
        private void SetupClientSideSettings()
        {
            PageTransferDetail retailerPage = GetPageTransferDetail(PageId.Retailers);
            JourneyHelper journeyHelper = new JourneyHelper();

            handoff.Value = RetailHandoffPageURL();
            retailer.Value = retailerPage.PageUrl;
            printerFriendlyUrl.Value = PrinterFriendlyPageURL();
            journeyKeys.Value = string.Format("{0},{1},{2},{3},{4},{5}", 
                QueryStringKey.JourneyRequestHash, QueryStringKey.JourneyIdOutward, QueryStringKey.JourneyIdReturn, 
                QueryStringKey.Retailer, QueryStringKey.JourneyLegDetailOutward, QueryStringKey.JourneyLegDetailReturn);
            journeyHash.Value = journeyHelper.GetJourneyRequestHash();
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);

            // Display the message seperator div (just a line seperator image)
            messageSeprator.Visible = true;
        }

        /// <summary>
        /// Adds debug information to page (where possible)
        /// </summary>
        private void SetupDebugInformation()
        {
            if (DebugHelper.ShowDebug)
            {
                try
                {
                    debugInfoDiv.Visible = true;

                    StringBuilder sbDebugText = new StringBuilder();

                    JourneyResultHelper resultHelper = new JourneyResultHelper();

                    // Output the request object, containing the parameters sent to the CJPManager
                    SJPJourneyRequest request = (SJPJourneyRequest)resultHelper.JourneyRequest;

                    sbDebugText.Append(request.ToString(true));

                    lblDebugInfo.Text = sbDebugText.ToString();

                    // Wait counter
                    loadingMessage.Text += string.Format("<br /><span class=\"debug\"> Waitcount[{0}]</span>", waitCount.Value.Parse(0));
                }
                catch (Exception ex)
                {
                    // Any exceptions in outputting debug info, display it so it can be fixed in the future 
                    lblDebugInfo.Text = string.Format("Message: {0} <br />StackTrace: {1}<br />",
                        ex.Message,
                        ex.StackTrace);
                }
            }
        }

        #endregion
    }
}
