// *********************************************** 
// NAME             : SJPWeb.Master.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: SJPWeb Master page
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.SessionManager;

namespace SJP.UserPortal.SJPWeb
{
    /// <summary>
    /// SJPWeb Master Page
    /// </summary>
    public partial class SJPWeb : System.Web.UI.MasterPage
    {
        #region Private members

        // List of local messages to be displayed on page
        protected List<SJPMessage> localMessages = new List<SJPMessage>();

        protected bool displayHeader = true;
        protected bool displayFooter = true;
        protected bool displaySideBarLeft = true;
        protected bool displaySideBarRight = true;

        protected SJPPage page;

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Init
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            page = ((SJPPage)Page);

            // Add class name to SJPPageContent div so content pages can override the styles
            SJPPageContent.Attributes["class"] = SJPPageContent.Attributes["class"] + " " + page.PageId.ToString().ToLower();

            #region Add style sheets

            //SJP.Common.Web.AccessibleStyle accessibleStyle = CurrentStyle.AccessibleStyleValue;

            // Default styles needed to add London2012 components for pages
            // (Replicating London2012 behaviour for adding stylesheets based on current accessible style)
            //if (accessibleStyle == AccessibleStyle.Normal)
            //{
            //    page.AddStyleSheetLondon2012("colours.css");
            //}
            //if (accessibleStyle == AccessibleStyle.Normal || accessibleStyle == AccessibleStyle.Dyslexia)
            //{
            //    page.AddStyleSheetLondon2012("main.css");
            //}
            //page.AddStyleSheetLondon2012(CurrentStyle.AccessibleStyleSheet);
            
            //page.AddStyleSheetLondon2012("additions.css");
            //page.AddStyleSheetLondon2012("merge.css"); // From templates provided by London2012, shouldnt be needed for SJP pages
            //page.AddStyleSheetLondon2012(CurrentStyle.FontSizeStyleSheet);

            // Default styles needed for an SJP Page, placed in the Init method so they are added first
            // before any sub page adds their style sheets
            //page.AddStyleSheet("SJPColours.css");
            //page.AddStyleSheet("SJPFonts.css");
            //page.AddStyleSheet("SJPBackgrounds.css");
            //page.AddStyleSheet("SJPMain.css");
            //page.AddStyleSheet("SJPPrint.css", "print");

            //switch (accessibleStyle)
            //{
            //    case AccessibleStyle.Dyslexia:
            //        page.AddStyleSheet("SJPDyslexia.css");
            //        break;
            //    case AccessibleStyle.HighVis:
            //        page.AddStyleSheet("SJPHighVis.css");
            //        break;
            //}

            #endregion

            #region Add style sheets GTW
            
            // Default styles needed to add London2012 components for pages
            page.AddStyleSheetLondon2012("london2012base.css");
            page.AddStyleSheetLondon2012("london2012accessibility.css");
            if (CurrentLanguage.Value == Language.French)
            {
                page.AddStyleSheetLondon2012("lang-fr.css");
            }
            page.AddStyleSheetLondon2012("pink.css");
            page.AddStyleSheetLondon2012("skin-pink.css");
            page.AddStyleSheetLondon2012("skin-pink-override.css");


            // Default styles needed for an SJP Page, placed in the Init method so they are added first
            // before any sub page adds their style sheets
            page.AddStyleSheet("SJPColours.css");
            page.AddStyleSheet("SJPFonts.css");
            page.AddStyleSheet("SJPBackgrounds.css");
            page.AddStyleSheet("SJPMain.css");
            page.AddStyleSheet("SJPAccessibility.css");
            page.AddStyleSheet("SJPPrint.css", "print");
            
            #endregion

            #region Add javascript files

            // Default javascript needed to add London2012 components for pages
            //page.AddJavascriptLondon2012("jquery.min.js");
            //page.AddJavascriptLondon2012("jquery.pngfix.js");
            //page.AddJavascriptLondon2012("jquery.smoothdivscroll-0.9.js");
            //page.AddJavascriptLondon2012("theming.js");

            #endregion

            #region Add javascript files GTW

            // Default javascript needed to add London2012 components for pages
            page.AddJavascriptLondon2012("jquery.js");
                    
            string javascriptBlock = string.Format("downloadJSAtOnload(document,'{0}london2012base.js?{1}','634702744014030724',false);",
                ResolveClientUrl(page.JavascriptPathLondon2012),
                DateTime.Now.ToString("ddMMyyyyHHmmsss"));

            page.AddJavascriptBlockToPage("london2012base", javascriptBlock);

            #endregion
        }

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
            #region Add London2012 class

            // Add the french class if needed (for London2012 styling)
            if (CurrentLanguage.Value == Language.French)
            {
                if (!formSJP.Attributes["class"].Contains("fr"))
                    formSJP.Attributes["class"] = formSJP.Attributes["class"] + " fr";
            }
            else
            {
                formSJP.Attributes["class"] = formSJP.Attributes["class"].Replace(" fr", string.Empty);
            }

            // Add the paralympic class if needed (for London2012 styling)
            if (page.SiteModeDisplay == SiteMode.Paralympics)
            {
                if (!formSJP.Attributes["class"].Contains("paralympic"))
                    formSJP.Attributes["class"] = formSJP.Attributes["class"] + " paralympic";
            }
            else
            {
                formSJP.Attributes["class"] = formSJP.Attributes["class"].Replace(" paralympic", string.Empty);
            }

            #endregion

            SetupResources();

            SetupMessages();

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
                        lblMessage.Text = sjpMessage.MessageText;
                        lblMessage.CssClass = sjpMessage.Type.ToString().ToLower(); // "warning" or "error" or "info"
                    }
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
        /// Read/Write. Display the SideBarLeft. Default is true
        /// </summary>
        public bool DisplaySideBarLeft
        {
            get { return displaySideBarLeft; }
            set { displaySideBarLeft = value; }
        }

        /// <summary>
        /// Read/Write. Display the SideBarRight. Default is true
        /// </summary>
        public bool DisplaySideBarRight
        {
            get { return displaySideBarRight; }
            set { displaySideBarRight = value; }
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
        /// Determines if the venue map widget needs showing in right side bar on input page
        /// </summary>
        public bool ShowVenueMapOnInputPage
        {
            set { sideBarRightControl.ShowVenueMapOnInputPage = value; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads and sets common page resources
        /// </summary>
        private void SetupResources()
        {
            if (sjpMainHeading != null)
                sjpMainHeading.InnerText = page.GetResource("Heading.Text");
           
            if (sjpHeading != null)
                sjpHeading.InnerText = page.GetResource(string.Format("{0}.Heading.Text", page.PageId.ToString()));

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

                if (pageState.Messages.Count > 0)
                {
                    // Get messages
                    BuildMessages(pageState.Messages, ref messagesToDisplay);

                    // Clear to avoid messages being displayed again
                    pageState.ClearMessages();
                }

                // Check this page's message list
                if (localMessages.Count > 0)
                {
                    // Get messages
                    BuildMessages(localMessages, ref messagesToDisplay);
                }

                // Display all messages in repeater
                rptrMessages.DataSource = messagesToDisplay.Values;
                rptrMessages.DataBind();

                pnlMessages.Visible = messagesToDisplay.Count > 0;
            }
            
        }
                

        /// <summary>
        /// Sets the visibility of common controls on the page
        /// </summary>
        private void DisplayControls()
        {
            headerControl.Visible = displayHeader;
            footerControl.Visible = displayFooter;
            sideBarLeftControl.Visible = displaySideBarLeft;
            sideBarRightControl.Visible = displaySideBarRight;
        }

        #endregion
    }
}
