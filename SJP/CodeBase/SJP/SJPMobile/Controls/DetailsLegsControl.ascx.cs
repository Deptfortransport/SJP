// *********************************************** 
// NAME             : DetailsLegsControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2012
// DESCRIPTION  	: The DetailsLegsControl uses the journey to build up the legs 
//                    to display using DetailsLegControls
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SJP.Common.LocationService;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// The DetailsLegsControl uses the journey to build up the legs 
    /// to display using DetailsLegControls
    /// </summary>
    public partial class DetailsLegsControl : System.Web.UI.UserControl
    {
        #region Private Fields

        private ISJPJourneyRequest journeyRequest = null;

        private List<JourneyLeg> journeyLegs;

        private RetailerHelper retailerHelper = new RetailerHelper();

        private bool showAccessibleFeatures;
        private bool showAccessibleInfoRail = true; // Could set these in Properties in the future
        private bool showAccessibleInfoCoach = true;
        private bool showAccessibleInfoFerry = true;
        
        // Indicates if the control should be rendered in accessible friendly mode
        private bool isAccessibleFriendly = false;

        // If a refresh should be forced on the control (e.g. following a page postback)
        private bool refresh = false;

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or Sets the journey leg objects of the journey to be displayed
        /// </summary>
        public List<JourneyLeg> JourneyLegs
        {
            get { return journeyLegs; }
            set { journeyLegs = value; }
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
            SetupControls();
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
                    showAccessibleFeatures, showAccessibleInfo, isAccessibleFriendly);

                if (refresh)
                {
                    legControl.Refresh();
                }
            }
        }
               
        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise method
        /// </summary>
        public void Initialise(ISJPJourneyRequest journeyRequest, List<JourneyLeg> journeyLegs, 
            bool showAccessibleFeatures, bool isAccessibleFriendly)
        {
            this.journeyRequest = journeyRequest;
            this.journeyLegs = journeyLegs;
            this.showAccessibleFeatures = showAccessibleFeatures;
            this.isAccessibleFriendly = isAccessibleFriendly;
        }

        /// <summary>
        /// Refresh method to re-populate the controls, e.g. if initialise called after Page_Load
        /// </summary>
        public void Refresh()
        {
            this.refresh = true;

            SetupControls();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method which setup the controls
        /// </summary>
        private void SetupControls()
        {
            legsDetailView.DataSource = journeyLegs;
            legsDetailView.DataBind();

            SetVenueMapLink();
        }

        /// <summary>
        /// Sets up the map links for any Venue locations specified
        /// </summary>
        private void SetVenueMapLink()
        {
            if (journeyRequest != null)
            {
                if (journeyRequest.Origin != null && journeyRequest.Origin is SJPVenueLocation)
                {
                    originVenueMapControl.SetVenue((SJPVenueLocation)journeyRequest.Origin, journeyLegs.First().StartTime.Date);
                }

                if (journeyRequest.Destination != null && journeyRequest.Destination is SJPVenueLocation)
                {
                    destinationVenueMapControl.SetVenue((SJPVenueLocation)journeyRequest.Destination, journeyLegs.Last().EndTime.Date);
                }
            }
        }

        #endregion
    }
}