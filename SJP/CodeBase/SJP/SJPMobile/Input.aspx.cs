// *********************************************** 
// NAME             : Input.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Feb 2012
// DESCRIPTION  	: Journey Input page
// ************************************************
// 
                
using System;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.Web;
using SJP.UserPortal.SJPMobile.Controls;
using SJP.UserPortal.JourneyControl;
using SJP.Common.PropertyManager;
using SJP.UserPortal.SJPMobile.Adapters;
using System.Collections.Generic;
using SJP.Reporting.Events;
using SJP.UserPortal.SessionManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// Journey Input page
    /// </summary>
    public partial class Input : SJPPageMobile
    {
        #region Private members

        private JourneyInputAdapter journeyInputAdapter;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Input()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileInput;
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Init
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            journeyInputControl.OnPlanJourney += new PlanJourney(journeyInputControl_OnPlanJourney);

            ((SJPMobile)Master).ButtonNext.Click += new EventHandler(showMap_Click);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            journeyInputAdapter = new JourneyInputAdapter(journeyInputControl);

            if (!IsPostBack)
            {
                // If is a landing page request, then ensure controls are told to re-validate/update. 
                // This is needed as we don't want the previously "selected" details changing if the user is returning from
                // the results page.
                journeyInputAdapter.PopulateInputControls(IsLandingPageRequest());
            }
            else
            {
                if (((SJPMobile)Master).PageScriptManager.IsInAsyncPostBack)
                {
                    logPageEntry = false;
                }

                // Ensure the input control continues to be in the correct planner mode
                journeyInputAdapter.UpdateInputControls();
            }

            // If landing page auto plan is set, then submit the journey request only if there were no messages to display
            if (IsLandingPageAutoPlanRequest() && !IsSessionMessages())
            {
                SubmitRequest(journeyInputControl.PlannerMode);
            }

            // Reset all landing page flags as they've been used
            ClearLandingPageFlags();

            // Add javascripts specific for this page
            AddJavascript("Input.js");
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();

            SetupMapLink();

            if (((SJPMobile)Master).PageScriptManager.IsInAsyncPostBack)
            {
                // Log an event as a result of partial page update
                PageEntryEvent logPage = new PageEntryEvent(Common.PageId.MobileInputPartialUpdate, SJPSessionManager.Current.Session.SessionID, false);
                Logger.Write(logPage);
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Plan journey button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void journeyInputControl_OnPlanJourney(object sender, PlanJourneyEventArgs e)
        {
            SubmitRequest(e.PlannerMode);
        }

        /// <summary>
        /// SJPMobile Master page Next button click handler to show the map page, default logic is in the master
        /// page and custom logic added here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void showMap_Click(object sender, EventArgs e)
        {
            // Capture all currently entered details before moving to the map page,
            // any invalid data can be ignored
            journeyInputAdapter.ValidateAndBuildSJPRequest(journeyInputControl.PlannerMode, true);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets up the resources and content
        /// </summary>
        private void SetupResources()
        {
            waitControl.LoadingMessageLabel.Text = GetResourceMobile("JourneyInput.LoadingMessage.Text");
        }

        /// <summary>
        /// Sets up the display of the map link, click logic is in master page
        /// </summary>
        private void SetupMapLink()
        {
            if (Properties.Current["Map.Input.Enabled.Switch"].Parse(true))
            {
                ((SJPMobile)Master).DisplayNext = true;
            }
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPMobile)this.Master).DisplayMessage(sjpMessage);
        }

        /// <summary>
        /// Submits the journey request. 
        /// If successful, then sets the page to transfer to, otherwise the current page processing continues
        /// </summary>
        /// <param name="plannerMode"></param>
        private void SubmitRequest(SJPJourneyPlannerMode plannerMode)
        {
            List<SJPMessage> messages = new List<SJPMessage>();

            if (journeyInputAdapter.SubmitRequest(plannerMode, ref messages, (SJPPage)Page))
            {
                switch (plannerMode)
                {
                    case SJPJourneyPlannerMode.PublicTransport:
                        // Set transfer to Summary page
                        SetPageTransfer(PageId.MobileSummary);

                        // Set the query string values for the Summary page,
                        // this allows the result for the correct request to be loaded
                        AddQueryStringForPage(PageId.MobileSummary);
                        break;

                    case SJPJourneyPlannerMode.Cycle:
                        // Set transfer to Summary page
                        SetPageTransfer(PageId.MobileSummary);

                        // Set the query string values for the Summary page,
                        // this allows the result for the correct request to be loaded
                        AddQueryStringForPage(PageId.MobileSummary);
                        break;
                }
            }
            else
            {
                // Show any submit validation error messages
                foreach (SJPMessage message in messages)
                {
                    DisplayMessage(message);
                }
            }
        }

        #endregion
    }
}