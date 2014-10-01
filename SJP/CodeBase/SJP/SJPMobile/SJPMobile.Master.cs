// *********************************************** 
// NAME             : SJPMobile.Master.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Feb 2012
// DESCRIPTION  	: SJPMobile Master page
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Web.UI;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.SessionManager;
using System.Web.UI.WebControls;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// SJPMobile Master page
    /// </summary>
    public partial class SJPMobile : System.Web.UI.MasterPage
    {
        #region Private members

        // Page
        private SJPPageMobile page = null;

        // List of local messages to be displayed on page
        protected List<SJPMessage> localMessages = new List<SJPMessage>();

        protected bool displayHeader = true;
        protected bool displayFooter = true;
        protected bool displayNavigation = true;
        protected bool displayBack = true;
        protected bool displayNext = false;

        protected PageId pageIdNextButton = PageId.Empty;

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Init
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            page = ((SJPPageMobile)Page);

            // Add class name to SJPPageContent div so content pages can override the styles
            SJPPageContent.Attributes["class"] = SJPPageContent.Attributes["class"] + " " + page.PageId.ToString().ToLower();

            #region Add style sheets

            // Add stylesheets declared in properties (these should be absolute URLs)
            string cssfileKeyString = Properties.Current["StyleSheet.Files.Keys"];

            if (!string.IsNullOrEmpty(cssfileKeyString))
            {
                string[] cssfileKeys = cssfileKeyString.Split(',');

                foreach (string cssfileKey in cssfileKeys)
                {
                    string cssfile = Properties.Current[string.Format("StyleSheet.File.{0}", cssfileKey)];

                    if (!string.IsNullOrEmpty(cssfile))
                    {
                        page.AddStyleSheet(cssfile);
                    }
                }
            }

            // Default styles needed for an SJP Page, placed in the Init method so they are added first
            // before any sub page adds their style sheets
            page.AddStyleSheet("SJPMain1.css");

            #endregion

            #region Add javascript files

            // Add js files declared in properties (these should be absolute URLs)
            string jsfileKeyString = Properties.Current["Javscript.Files.Keys"];

            if (!string.IsNullOrEmpty(jsfileKeyString))
            {
                string[] jsfileKeys = jsfileKeyString.Split(',');

                foreach (string jsfileKey in jsfileKeys)
                {
                    string jsfile = Properties.Current[string.Format("Javscript.File.{0}",jsfileKey)];

                    if (!string.IsNullOrEmpty(jsfile))
                    {
                        page.AddJavascript(jsfile);
                    }
                }
            }

            page.AddJavascript("Common.js");

            #endregion
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Add the default device stylesheet, 
            // javascript will override the path to be the device specific if needed
            page.AddStyleSheet("Device/Default.css");
        }
        
        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();

            SetupMessages();

            SetupNext();

            SetupBack();

            DisplayControls();
        }

        #endregion

        #region Events

        /// <summary>
        /// Event handler for rptrMessages_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptrMessages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                SJPMessage sjpMessage = (SJPMessage)e.Item.DataItem;

                if (sjpMessage != null)
                {
                    Label lblMessage = (Label)e.Item.FindControl("lblMessage");

                    if (lblMessage != null)
                    {
                        string message = sjpMessage.MessageText;

                        // Temporary, this might change - remove any <strong> tags (because messages might be
                        // shared from SJP Web which does need them)
                        message = message.Replace("<strong>", string.Empty);
                        message = message.Replace("</strong>", string.Empty);

                        // Temporary, all messages are of error type
                        lblMessage.Text = message;
                        lblMessage.CssClass = SJPMessageType.Error.ToString().ToLower();
                            //sjpMessage.Type.ToString().ToLower(); // "warning" or "error" or "info"
                    }
                }
            }
        }

        /// <summary>
        /// Back button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void backBtn_Click(object sender, EventArgs e)
        {
            switch (page.PageId)
            {
                case PageId.MobileDirection:
                    page.SetPageTransfer(PageId.MobileDetail);
                    page.AddQueryStringForPage(PageId.MobileDetail);
                    break;
                case PageId.MobileDetail:
                    page.SetPageTransfer(PageId.MobileSummary);
                    page.AddQueryStringForPage(PageId.MobileSummary);
                    break;
                case PageId.MobileMap:
                    // Displaying for journey, from the detail page
                    if (IsNavigationForDetail())
                    {
                        page.SetPageTransfer(PageId.MobileDetail);
                        page.AddQueryStringForPage(PageId.MobileDetail);
                    }
                    // Displaying for location, from the summary page
                    else if (IsNavigationForSummary())
                    {
                        page.SetPageTransfer(PageId.MobileSummary);
                        page.AddQueryStringForPage(PageId.MobileSummary);
                    }
                    // Displaying for location, from the input page
                    else
                    {
                        page.SetPageTransfer(PageId.MobileInput);
                    }
                    break;
                case PageId.MobileTravelNews:
                    // Displaying for journey, from the detail page
                    if (IsNavigationForDetail())
                    {
                        page.SetPageTransfer(PageId.MobileDetail);
                        page.AddQueryStringForPage(PageId.MobileDetail);
                    }
                    // Displaying for journey, from the summary page
                    else if (IsNavigationForSummary())
                    {
                        page.SetPageTransfer(PageId.MobileSummary);
                        page.AddQueryStringForPage(PageId.MobileSummary);
                    }
                    else
                    {
                        page.SetPageTransfer(PageId.MobileDefault);
                    }
                    break;
                case PageId.MobileTravelNewsDetail:
                    page.SetPageTransfer(PageId.MobileTravelNews);

                    if (IsNavigationForLondonUndergroundNews())
                    {
                        page.AddQueryString(QueryStringKey.NewsMode, TravelNewsHelper.NewsViewMode_LUL);
                    }

                    break;
                case PageId.MobileAccessibilityOptions:
                    page.SetPageTransfer(PageId.MobileInput);
                    break;
                case PageId.MobileInput:
                case PageId.MobileSummary:
                case PageId.MobileError:
                case PageId.MobilePageNotFound:
                case PageId.MobileSorry:
                default:
                    page.SetPageTransfer(PageId.MobileDefault);
                    page.AddQueryStringForPage(PageId.MobileDefault);
                    break;
            }
        }

        /// <summary>
        /// Next button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void nextBtn_Click(object sender, EventArgs e)
        {
            // Specific next page has been specified
            if (pageIdNextButton != PageId.Empty)
            {
                page.SetPageTransfer(pageIdNextButton);
                page.AddQueryStringForPage(pageIdNextButton);
            }
            else
            {
                // Use current page to determine next page transfer
                switch (page.PageId)
                {
                    case PageId.MobileInput:
                        page.SetPageTransfer(PageId.MobileMap);
                        page.AddQueryStringForPage(PageId.MobileMap);
                        break;
                    case PageId.MobileSummary:
                        page.SetPageTransfer(PageId.MobileMap);
                        page.AddQueryStringForPage(PageId.MobileMapSummary);
                        break;
                    case PageId.MobileDetail:
                        page.SetPageTransfer(PageId.MobileDirection);
                        page.AddQueryStringForPage(PageId.MobileDirection);
                        break;
                    case PageId.MobileDirection:
                        page.SetPageTransfer(PageId.MobileMap);
                        page.AddQueryStringForPage(PageId.MobileMapJourney);
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method to add an SJPMessage to display on the page
        /// </summary>
        /// <param name="sjpMessage"></param>
        public void DisplayMessage(SJPMessage sjpMessage)
        {
            if (sjpMessage != null)
            {
                localMessages.Add(sjpMessage);
            }
        }

        /// <summary>
        /// Builds the SJPMessages list to show, retrieving actual text from Resource manager, and filtering 
        /// out duplicate messages
        /// </summary>
        /// <param name="sjpMessages"></param>
        /// <param name="messagesToDisplay"></param>
        public void BuildMessages(List<SJPMessage> sjpMessages, ref Dictionary<string, SJPMessage> messagesToDisplay)
        {
            SJPMessage message = null;

            foreach (SJPMessage sjpMessage in sjpMessages)
            {
                // For each message, get the display text, and add it to the display list.
                // Ensure duplicate messages are not added twice
                if (!messagesToDisplay.ContainsKey(sjpMessage.MessageResourceId))
                {
                    string messageText = string.Empty;

                    // Try to get a mobile-specific version
                    messageText = page.GetResourceMobile(sjpMessage.MessageResourceId);

                    if (!string.IsNullOrEmpty(sjpMessage.MessageResourceGroup) && !string.IsNullOrEmpty(sjpMessage.MessageResourceCollection))
                    {
                        messageText = page.GetResource(sjpMessage.MessageResourceGroup, sjpMessage.MessageResourceCollection, sjpMessage.MessageResourceId);
                    }

                    // Not got the SJP messages text with the resource collection and group specified for the message
                    // Try get the message text with the generic resource collection
                    if (string.IsNullOrEmpty(messageText))
                    {
                        messageText = page.GetResource(sjpMessage.MessageResourceId);
                    }

                    // Not got the SJP message text from the resource, check if message text was directly specified
                    if ((string.IsNullOrEmpty(messageText)) && (!string.IsNullOrEmpty(sjpMessage.MessageText)))
                    {
                        messageText = sjpMessage.MessageText;
                    }

                    if (!string.IsNullOrEmpty(messageText))
                    {
                        try
                        {
                            // Check if need to format the string with args for the message
                            if (sjpMessage.MessageArgs != null && sjpMessage.MessageArgs.Count > 0)
                            {
                                messageText = string.Format(messageText, sjpMessage.MessageArgs.ToArray());
                            }
                        }
                        catch
                        {
                            // Ignore any exceptions, we still have some sort of message to display
                        }

                        message = new SJPMessage(messageText, string.Empty, -1, -1, sjpMessage.Type);

                        string key = sjpMessage.MessageResourceId;

                        if (string.IsNullOrEmpty(key))
                        {
                            key = messageText;
                        }

                        messagesToDisplay.Add(sjpMessage.MessageResourceId, message);

                    }
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Display the Header. Default is true
        /// </summary>
        public bool DisplayHeader
        {
            get { return displayHeader; }
            set { displayHeader = value; }
        }

        /// <summary>
        /// Read/Write. Display the Footer. Default is true
        /// </summary>
        public bool DisplayFooter
        {
            get { return displayFooter; }
            set { displayFooter = value; }
        }

        /// <summary>
        /// Read/Write. Display the Navigation panel containing the Back and Next buttons. Default is true
        /// </summary>
        public bool DisplayNavigation
        {
            get { return displayNavigation; }
            set { displayNavigation = value; }
        }

        /// <summary>
        /// Read/Write. Display the Back button. Default is true
        /// </summary>
        public bool DisplayBack
        {
            get { return displayBack; }
            set { displayBack = value; }
        }

        /// <summary>
        /// Read/Write. Display the Next button. Default is false
        /// </summary>
        public bool DisplayNext
        {
            get { return displayNext; }
            set { displayNext = value; }
        }

        /// <summary>
        /// Read only property exposing local display messages, 
        /// so content pages can use them to bind to custom error message display
        /// </summary>
        public List<SJPMessage> PageMessages
        {
            get { return localMessages; }
        }

        /// <summary>
        /// exposes the script manager to content pages
        /// </summary>
        public ScriptManager PageScriptManager
        {
            get { return ScriptManager1; }
        }

        /// <summary>
        /// Next button control
        /// </summary>
        public System.Web.UI.WebControls.LinkButton ButtonNext
        {
            get { return nextBtn; }
            set { nextBtn = value; }
        }

        /// <summary>
        /// Read/Write to specify a page for the next button click to go to, 
        /// instead of using the default logic for current page
        /// </summary>
        public PageId ButtonNextPage
        {
            get { return pageIdNextButton; }
            set { pageIdNextButton = value; }
        }

        /// <summary>
        /// Div containing the Back and Next controls
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlGenericControl BackDiv
        {
            get { return sjpBackDiv; }
            set { sjpBackDiv = value; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads and sets common page resources
        /// </summary>
        private void SetupResources()
        {
            // Page heading title
            if (sjpHeading != null)
            {
                sjpHeading.InnerText = page.GetResourceMobile(string.Format("{0}.Heading.Text", page.PageId.ToString()));

                // No heading text, check for screenreader heading text (to allow a page to contain a h1 element)
                if (string.IsNullOrEmpty(sjpHeading.InnerText))
                {
                    sjpHeading.InnerText = page.GetResourceMobile(string.Format("{0}.Heading.ScreenReader.Text", page.PageId.ToString()));

                    // No screenreader heading text, hide the heading div
                    if (string.IsNullOrEmpty(sjpHeading.InnerText))
                    {
                        sjpHeadingDiv.Visible = false;
                    }
                    else
                    {
                        // Apply screen reader class to prevent the heading from being visible
                        string sjpHeadingDivClass = sjpHeadingDiv.Attributes["class"];
                        if (!sjpHeadingDivClass.Contains("screenReaderOnly"))
                        {
                            sjpHeadingDiv.Attributes["class"] = "screenReaderOnly";
                        }
                        sjpHeading.Attributes["class"] = "screenReaderOnly";
                    }
                }
            }

            // Site version
            hdnSiteVersion.Value = Properties.Current["Site.VersionNumber"];
        }

        /// <summary>
        /// Loads any messages currently in the Session
        /// </summary>
        private void SetupMessages()
        {
            if (pnlMessages != null && rptrMessages != null)
            {
                // Messages to be displayed on page
                Dictionary<string, SJPMessage> messagesToDisplay = new Dictionary<string, SJPMessage>();

                // Check session for messages
                InputPageState pageState = SJPSessionManager.Current.PageState;

                // Check this page's message list
                if (localMessages.Count > 0)
                {
                    // Get messages
                    BuildMessages(localMessages, ref messagesToDisplay);

                    // Clear to avoid messages being displayed again
                    pageState.ClearMessages();
                }
                else if (pageState.Messages.Count > 0)
                {
                    // Get messages
                    BuildMessages(pageState.Messages, ref messagesToDisplay);

                    // Clear to avoid messages being displayed again
                    pageState.ClearMessages();
                }

                // Display all messages in repeater
                rptrMessages.DataSource = messagesToDisplay.Values;
                rptrMessages.DataBind();

                pnlMessages.Visible = messagesToDisplay.Count > 0;
            }
        }

        /// <summary>
        /// Sets up the back button
        /// </summary>
        private void SetupBack()
        {
            navigationTitle.InnerHtml = page.GetResourceMobile("JourneyInput.Back.Text");

            switch (page.PageId)
            {
                case PageId.MobileDirection:
                    backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileDetail.Text");
                    backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileDetail.ToolTip");
                    break;
                case PageId.MobileDetail:
                    backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileSummary.Text");
                    backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileSummary.ToolTip");
                    break;
                case PageId.MobileAccessibilityOptions:
                    backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileInput.Text");
                    backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileInput.ToolTip");
                    break;
                case PageId.MobileInput:
                case PageId.MobileSummary:
                    backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileDefault.Text");
                    backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileDefault.ToolTip");
                    break;
                case PageId.MobileMap:
                    if (IsNavigationForDetail())
                    {
                        backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileDetail.Text");
                        backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileDetail.ToolTip");
                    }
                    else if (IsNavigationForSummary())
                    {
                        backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileSummary.Text");
                        backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileSummary.ToolTip");
                    }
                    else
                    {
                        backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileInput.Text");
                        backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileInput.ToolTip");
                    }
                    break;
                case PageId.MobileTravelNews:
                    if (IsNavigationForDetail())
                    {
                        backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileDetail.Text");
                        backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileDetail.ToolTip");
                    }
                    else if (IsNavigationForSummary())
                    {
                        backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileSummary.Text");
                        backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileSummary.ToolTip");
                    }
                    else
                    {
                        backBtn.Text = page.GetResourceMobile("JourneyInput.Back.MobileDefault.Text");
                        backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.MobileDefault.ToolTip");
                    }
                    break;
                default:
                    backBtn.Text = page.GetResourceMobile("JourneyInput.Back.Text");
                    backBtn.ToolTip = page.GetResourceMobile("JourneyInput.Back.ToolTip");
                    break;
            }

            backBtnNonJS.Text = backBtn.Text;
            backBtnNonJS.ToolTip = backBtn.ToolTip;
        }

        /// <summary>
        /// Sets up the next button
        /// </summary>
        private void SetupNext()
        {
            navigationTitle.InnerHtml = page.GetResourceMobile("JourneyInput.Next.Text");

            // Specific next page has been specified
            if (pageIdNextButton != PageId.Empty)
            {
                nextBtn.Text = page.GetResourceMobile(string.Format("JourneyInput.Next.{0}.Text", pageIdNextButton.ToString()));
                nextBtn.ToolTip = page.GetResourceMobile(string.Format("JourneyInput.Next.{0}.ToolTip", pageIdNextButton.ToString()));
                
                if (string.IsNullOrEmpty(nextBtn.Text))
                {
                    nextBtn.Text = page.GetResourceMobile("JourneyInput.Next.Text");
                    nextBtn.ToolTip = page.GetResourceMobile("JourneyInput.Next.ToolTip");
                }
            }
            else
            {
                // Use current page to determine next page
                switch (page.PageId)
                {
                    case PageId.MobileInput:
                    case PageId.MobileSummary:
                    case PageId.MobileSummaryPartialUpdate:
                    case PageId.MobileSummaryResult:
                    case PageId.MobileSummaryWait:
                        nextBtn.Text = page.GetResourceMobile("JourneyInput.Next.MobileMap.Text");
                        nextBtn.ToolTip = page.GetResourceMobile("JourneyInput.Next.MobileMap.ToolTip");
                        break;
                    case PageId.MobileDetail:
                        nextBtn.Text = page.GetResourceMobile("JourneyInput.Next.MobileDirection.Text");
                        nextBtn.ToolTip = page.GetResourceMobile("JourneyInput.Next.MobileDirection.ToolTip");
                        break;
                    case PageId.MobileDirection:
                        nextBtn.Text = page.GetResourceMobile("JourneyInput.Next.MobileMapJourney.Text");
                        nextBtn.ToolTip = page.GetResourceMobile("JourneyInput.Next.MobileMapJourney.ToolTip");
                        break;
                    default:
                        nextBtn.Text = page.GetResourceMobile("JourneyInput.Next.Text");
                        nextBtn.ToolTip = page.GetResourceMobile("JourneyInput.Next.ToolTip");
                        break;
                }
            }

            nextBtnNonJS.Text = nextBtn.Text;
            nextBtnNonJS.ToolTip = nextBtn.ToolTip;
        }

        /// <summary>
        /// Sets the visibility of common controls on the page
        /// </summary>
        private void DisplayControls()
        {
            // Only display the navigation (header and footer) controls if required,
            // session default is false if key missing, but default is to show navigation
            bool navigationRequired = !SJPSessionManager.Current.Session[SessionKey.IsNavigationNotRequired];

            headerControl.Visible = navigationRequired && displayHeader;
            footerControl.Visible = navigationRequired && displayFooter;

            contentNavigation.Visible = displayNavigation;
            backBtn.Visible = displayBack;
            nextBtn.Visible = displayNext;

            backBtnNonJS.Visible = displayBack;
            nextBtnNonJS.Visible = displayNext;
        }

        /// <summary>
        /// If page querystring contains a journey request hash
        /// </summary>
        /// <returns></returns>
        private bool IsNavigationForSummary()
        {
            return (page.QueryStringContains(QueryStringKey.JourneyRequestHash));
        }

        /// <summary>
        /// If page querystring contains a journey request hash and journey id
        /// </summary>
        /// <returns></returns>
        private bool IsNavigationForDetail()
        {
            return (page.QueryStringContains(QueryStringKey.JourneyRequestHash)
                    && 
                    (page.QueryStringContains(QueryStringKey.JourneyIdOutward) || page.QueryStringContains(QueryStringKey.JourneyIdReturn))
                   );
        }

        // If page querystring containg a travel news mode for london underground
        private bool IsNavigationForLondonUndergroundNews()
        {
            return (page.QueryStringContains(QueryStringKey.NewsMode));

        }

        #endregion
    }
}
