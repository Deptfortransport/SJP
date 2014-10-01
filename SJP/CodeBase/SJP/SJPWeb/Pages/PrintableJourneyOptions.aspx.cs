// *********************************************** 
// NAME             : PrintableJourneyOptions.aspx      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: PrintableJourneyOptions page
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPWeb.Adapters;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// PrintableJourneyOptions page
    /// </summary>
    public partial class PrintableJourneyOptions : SJPPage
    {
        #region Variables

        private string journeyRequestHash = null;
        private Journey journeyOutward = null;
        private Journey journeyReturn = null;

        private bool detailOutwardExpanded = false;
        private bool detailReturnExpanded = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintableJourneyOptions()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.PrintableJourneyOptions;
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
            AddStyleSheet("SJPPrint.css");

            // If javascript is enabled, then the JourneyOptions page may not have had the chance to 
            // sync the selected journey to session (i.e. no postbacks) - so query string (containing
            // or not containing journey id's here) and session should be synched
            SynchSelectedJourneys();

            JourneyResultHelper resultHelper = new JourneyResultHelper();

            if (resultHelper.IsJourneyResultAvailable)
            {
                BindJourneyResult(resultHelper.JourneyRequest, resultHelper.JourneyResult);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Binds journey result to outward and return detail summary result controls
        /// </summary>
        /// <param name="journeyRequest">ISJPJourneyRequest object</param>
        /// <param name="journeyResult">ISJPJourneyResult object</param>
        private void BindJourneyResult(ISJPJourneyRequest journeyRequest, ISJPJourneyResult journeyResult)
        {
            if (journeyResult != null)
            {
                // Determine if the details control should render in accessible mode
                bool accessible = (CurrentStyle.AccessibleStyleValue != AccessibleStyle.Normal);

                JourneyHelper journeyHelper = new JourneyHelper();

                // Retrieve selected journeys (using query string first, then session)
                journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

                journeyHelper.GetJourneyLegDetailExpanded(out detailOutwardExpanded, out detailReturnExpanded);

                // Assume journeys must exist if showing printer friendly page
                if (journeyResult.OutwardJourneys != null)
                {
                    if (journeyResult.OutwardJourneys.Count > 0)
                    {
                        // Get journey id selected or 0 for do not show any selected
                        int jid = (journeyOutward != null) ? journeyOutward.JourneyId : 0;

                        outwardSummaryControl.Initialise(journeyRequest, journeyResult, false, jid, detailOutwardExpanded, true, accessible);
                        outwardSummaryControl.Visible = true;
                    }
                }

                if (journeyResult.ReturnJourneys != null)
                {
                    if (journeyResult.ReturnJourneys.Count > 0)
                    {
                        // Get journey id selected or 0 for do not show any selected
                        int jid = (journeyReturn != null) ? journeyReturn.JourneyId : 0;

                        returnSummaryControl.Initialise(journeyRequest, journeyResult, true, jid, detailReturnExpanded, true, accessible);
                        returnSummaryControl.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the session with the selected journey ids
        /// </summary>
        private void SynchSelectedJourneys()
        {
            // Query string may have no journey ids if user has collapsed all journeys, therefore update
            // session with none selected

            JourneyHelper journeyHelper = new JourneyHelper();

            int journeyIdOutward = journeyHelper.GetJourneySelectedQueryString(true);
            int journeyIdReturn = journeyHelper.GetJourneySelectedQueryString(false);

            journeyHelper.SetJourneySelected(true, journeyIdOutward);
            
            journeyHelper.SetJourneySelected(false, journeyIdReturn);
        }

        #endregion

    }
}