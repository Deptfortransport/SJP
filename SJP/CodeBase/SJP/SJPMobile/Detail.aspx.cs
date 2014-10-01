// *********************************************** 
// NAME             : Detail.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2012
// DESCRIPTION  	: Journey Detail page
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPMobile.Controls;
using SJP.Common.PropertyManager;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// Journey Detail page
    /// </summary>
    public partial class Detail : SJPPageMobile
    {
        #region Variables

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Detail()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileDetail;
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
            journeyPageControl.ShowJourneyHandler += new OnShowJourney(ShowJourneyEvent);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupControls();

            AddJavascript("Detail.js");
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
        
        #region Event handlers
        
        #region Show journey events

        /// <summary>
        /// Shows the selected journey
        /// </summary>
        protected void ShowJourneyEvent(object sender, JourneyEventArgs e)
        {
            if (e != null)
            {
                // Get the selected journey, and refresh the controls
                JourneyResultHelper resultHelper = new JourneyResultHelper();
                JourneyHelper journeyHelper = new JourneyHelper();

                // Persist selected journey to session (for any browser navigation)
                journeyHelper.SetJourneySelected(true, e.JourneyId);
                
                ISJPJourneyRequest journeyRequest = resultHelper.JourneyRequest;
                ISJPJourneyResult journeyResult = resultHelper.CheckJourneyResultAvailability();
                Journey journey = journeyResult.GetJourney(e.JourneyId);
                bool accessibleFriendly = (CurrentStyle.AccessibleStyleValue != AccessibleStyle.Normal);

                if (journey != null)
                {
                    // Display the journey pageing control
                    journeyPageControl.Initialise(journeyRequest, journeyResult, journey);
                    journeyPageControl.Refresh();

                    // Display the journey leg details
                    legsDetails.Initialise(journeyRequest, journey.JourneyLegs,
                        journey.AccessibleJourney, accessibleFriendly);
                    legsDetails.Refresh();
                }
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
        }

        /// <summary>
        /// Sets up the controls on the page
        /// </summary>
        private void SetupControls()
        {
            JourneyResultHelper resultHelper = new JourneyResultHelper();
            JourneyHelper journeyHelper = new JourneyHelper();

            // Journey request/result
            ISJPJourneyRequest journeyRequest = resultHelper.JourneyRequest;
            ISJPJourneyResult journeyResult = resultHelper.CheckJourneyResultAvailability();

            // Journey to be shown
            string journeyRequestHash = string.Empty;
            Journey journeyOutward = null;
            Journey journeyReturn = null;

            // Should only find an outward journey
            journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            // If arrived on page without user selecting a journey (e.g. Cycle may not show Summary page)
            if ((journeyResult != null) && (journeyResult.OutwardJourneys.Count == 1))
            {
                journeyOutward = journeyResult.OutwardJourneys[0];
            }

            // Determine if the details control should render in accessible mode
            bool accessibleFriendly = (CurrentStyle.AccessibleStyleValue != AccessibleStyle.Normal);

            if (journeyOutward != null)
            {
                // Display the journey pageing control
                journeyPageControl.Initialise(journeyRequest, journeyResult, journeyOutward);

                // Display the journey leg details
                legsDetails.Initialise(journeyRequest, journeyOutward.JourneyLegs, 
                    journeyOutward.AccessibleJourney, accessibleFriendly);

                if (journeyOutward.GetUsedModes().Contains(SJPModeType.Cycle))
                {
                    ((SJPMobile)Master).DisplayNext = true;
                }
                else
                {
                    // Travel News functionality available
                    if (Properties.Current["TravelNews.Enabled.Switch"].Parse(true))
                    {
                        ((SJPMobile)Master).DisplayNext = true;
                        ((SJPMobile)Master).ButtonNextPage = PageId.MobileTravelNews;   
                    }
                }
            }
        }
        
        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPMobile)this.Master).DisplayMessage(sjpMessage);
        }

        #endregion
    }
}