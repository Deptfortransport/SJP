// *********************************************** 
// NAME             : DetailsLegsControl.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 06 Apr 2011
// DESCRIPTION  	: The DetailsLegsControl uses the journey to build up the legs 
//                    to display using DetailsLegControls
// ************************************************


using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;
using SJP.UserPortal.SJPWeb.Adapters;

namespace SJP.UserPortal.SJPWeb.Controls
{
    #region Public Events

    // Delegate for selected leg details being expanded/collapsed
    public delegate void OnSelectedJourneyLegDetailsChange(object sender, JourneyLegEventArgs e);

    #endregion

    /// <summary>
    /// The DetailsLegsControl uses the journey to build up the legs 
    //  to display using DetailsLegControls
    /// </summary>
    public partial class DetailsLegsControl : System.Web.UI.UserControl
    {
        #region Public Events

        // Selected journey detail leg event declaration
        public event OnSelectedJourneyLegDetailsChange SelectedJourneyLegDetailsHandler;

        #endregion

        #region Private Fields

        private ISJPJourneyRequest journeyRequest = null;

        private List<JourneyLeg> journeyLegs;
        private RoutingDetail journeyRoutingDetail;

        private RetailerHelper retailerHelper = new RetailerHelper();

        private bool showAccessibleFeatures;
        private bool showAccessibleInfoRail = true; // Could set these in Properties in the future
        private bool showAccessibleInfoCoach = true;
        private bool showAccessibleInfoFerry = true;

        // Indicates if the journey leg has a "detail" element, whether to expand or collapse
        private bool legDetailExpanded = false;

        // Indicates if the control should be rendered in a printer friendly mode
        private bool isPrinterFriendly = false;

        // Indicates if the control should be rendered in accessible friendly mode
        private bool isAccessibleFriendly = false;

        // Track outward/return journey Id for cycle GPX link
        private int journeyId = -1;
        private bool isReturn = false;

        // Journey request hash
        private string journeyRequestHash;

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or Sets the journey leg objects of the journey to be displayed
        /// </summary>
        public List<JourneyLeg> JourneyLegs
        {
            get
            {
                return journeyLegs;
            }

            set
            {
                journeyLegs = value;
            }
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
            foreach (RepeaterItem item in legsDetailView.Items)
            {
                DetailsLegControl legControl = item.FindControlRecursive<DetailsLegControl>("legControl");
                if (legControl != null)
                {
                    legControl.SelectedJourneyLegDetailHandler += new OnSelectedJourneyLegDetailChange(SelectedJourneyLegDetailEventHandler);
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
        }

        #endregion

        #region Control Event Handlers
        /// <summary>
        /// legsDetailView repeater data bound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegsDetailView_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.SelectedItem)
            {

                // Current leg
                JourneyLeg currentLeg = e.Item.DataItem as JourneyLeg;

                // Previous leg
                JourneyLeg previousLeg = null;
                if (e.Item.ItemIndex > 0)
                    previousLeg = journeyLegs[e.Item.ItemIndex - 1];

                // Next leg 
                JourneyLeg nextLeg = null;
                if (e.Item.ItemIndex < (journeyLegs.Count - 1))
                    nextLeg = journeyLegs[e.Item.ItemIndex + 1];

                #region Determine if accessible info text should be shown

                // Show accessible info text if showing accessible features, and for all
                // instances of Rail, Coach, and Ferry mode
                bool showAccessibleInfo = false;

                if (showAccessibleFeatures)
                {
                    if ((currentLeg.Mode == Common.SJPModeType.Rail) && (showAccessibleInfoRail))
                    {
                        showAccessibleInfo = true;
                    }
                    else if ((currentLeg.Mode == Common.SJPModeType.Coach) && (showAccessibleInfoCoach))
                    {
                        showAccessibleInfo = true;
                    }
                    else if ((currentLeg.Mode == Common.SJPModeType.Ferry) && (showAccessibleInfoFerry))
                    {
                        showAccessibleInfo = true;
                    }
                }

                #endregion

                #region Get the retailers

                List<Retailer> retailers = retailerHelper.GetRetailersForJourneyLeg(journeyLegs, e.Item.ItemIndex);
                bool isCoachAndRailRetailer = false;

                if (retailers.Count > 0)
                    isCoachAndRailRetailer = retailerHelper.IsJourneyLegForCombinedCoachRailRetailer(journeyLegs, e.Item.ItemIndex);

                bool isTravelcardRetailer = retailerHelper.IsJourneyLegCoveredByTravelcard(journeyLegs, e.Item.ItemIndex);
                
                #endregion

                // Initialise the Leg control
                DetailsLegControl legControl = e.Item.FindControlRecursive<DetailsLegControl>("legControl");
                legControl.Initialise(journeyRequest, previousLeg, currentLeg, nextLeg, 
                    retailers, isCoachAndRailRetailer, isTravelcardRetailer,
                    showAccessibleFeatures, showAccessibleInfo, legDetailExpanded, isPrinterFriendly, isAccessibleFriendly, 
                    journeyId, e.Item.ItemIndex, isReturn, journeyRequestHash);
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                #region Debug - Routing Details

                // Only show the routing details in debug mode (this may be changed in future)
                if ((journeyRoutingDetail != null) && (DebugHelper.ShowDebug))
                {
                    Label lblRoutingDetails = e.Item.FindControlRecursive<Label>("lblRoutingDetails");

                    if (lblRoutingDetails != null)
                    {
                        StringBuilder sbRouting = new StringBuilder();

                        if (journeyRoutingDetail.RoutingRuleIDs.Count > 0)
                        {
                            sbRouting.Append(string.Format("routingRuleIds[{0}]<br/>",
                                journeyRoutingDetail.ToStringRoutingRuleIDs()));
                        }

                        if (journeyRoutingDetail.RoutingReasons.Count > 0)
                        {
                            sbRouting.Append(string.Format("routingReasons[{0}]<br/>",
                                journeyRoutingDetail.ToStringRoutingReasons()));
                        }

                        if (journeyRoutingDetail.RoutingStops.Count > 0)
                        {
                            sbRouting.Append(string.Format("routingStops[{0}]<br/>",
                                journeyRoutingDetail.ToStringRoutingStops()));
                        }

                        if (sbRouting.Length > 0 )
                        {
                            lblRoutingDetails.Text = string.Format("<span class=\"debug\">{0}</span>", sbRouting.ToString());
                            lblRoutingDetails.Visible = true;
                        }
                    }
                }

                #endregion
            }
        }

        /// <summary>
        /// Event handler for SelectedJourneyLegHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedJourneyLegDetailEventHandler(object sender, JourneyLegEventArgs e)
        {
            // Raise event to tell subscribers selected journey leg detail has changed
            if (SelectedJourneyLegDetailsHandler != null)
            {
                SelectedJourneyLegDetailsHandler(sender, e);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise method
        /// </summary>
        public void Initialise(ISJPJourneyRequest journeyRequest, List<JourneyLeg> journeyLegs, RoutingDetail journeyRoutingDetail,
            bool showAccessibleFeatures, bool legDetailExpanded,
            bool printerFriendly, bool accessibleFriendly, 
            int journeyId, bool isReturn, string journeyRequestHash)
        {
            this.journeyRequest = journeyRequest;
            this.journeyLegs = journeyLegs;
            this.journeyRoutingDetail = journeyRoutingDetail;
            this.showAccessibleFeatures = showAccessibleFeatures;
            this.legDetailExpanded = legDetailExpanded;
            this.isPrinterFriendly = printerFriendly;
            this.isAccessibleFriendly = accessibleFriendly;
            this.journeyId = journeyId;
            this.isReturn = isReturn;
            this.journeyRequestHash = journeyRequestHash;
            
            legsDetailView.DataSource = journeyLegs;
            legsDetailView.DataBind();
        }

        #endregion
    }
}