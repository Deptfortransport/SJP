// *********************************************** 
// NAME             : AccessibilityOptions.aspx.cs      
// AUTHOR           : David Lane
// DATE CREATED     : 02 Apr 2012
// DESCRIPTION  	: Accessibility Options page
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
    /// AccessibilityOptions page
    /// </summary>
    public partial class AccessibilityOptions : SJPPageMobile
    {
        #region Private members

        private JourneyInputAdapter journeyInputAdapter;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AccessibilityOptions()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileAccessibilityOptions;
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
            accessibleStopsControl.OnPlanJourney += new PlanJourney(accessibleStopsControl_OnPlanJourney);
            accessibleStopsControl.OnDisplayMessage += new DisplayMessage(accessibleStopsControl_OnDisplayMessage);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            journeyInputAdapter = new JourneyInputAdapter(accessibleStopsControl);

            // Add javascripts specific for this page
            AddJavascript("AccessibilityOptions.js");
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Plan journey button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void accessibleStopsControl_OnPlanJourney(object sender, PlanJourneyEventArgs e)
        {
            SubmitRequest(e.PlannerMode);
        }

        /// <summary>
        /// Display message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void accessibleStopsControl_OnDisplayMessage(object sender, DisplayMessageEventArgs e)
        {
            DisplayMessage(e.Message);
        }

        #endregion

        #region Private methods
        
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
                // Should always be public transport on this page

                // Set transfer to Summary page
                SetPageTransfer(PageId.MobileSummary);

                // Set the query string values for the Summary page,
                // this allows the result for the correct request to be loaded
                AddQueryStringForPage(PageId.MobileSummary);
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