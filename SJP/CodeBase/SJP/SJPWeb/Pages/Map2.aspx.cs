// *********************************************** 
// NAME             : Map2.aspx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 May 2012
// DESCRIPTION  	: Map page for Google maps
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.Common.LocationService;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// Map page for Google maps
    /// </summary>
    public partial class Map2 : SJPPage
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Map2()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MapGoogle;
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
            if (!Page.IsPostBack)
            {
                SetupControls();
            }

            AddJavascript("Map2.js");
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupPage();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up the page 
        /// </summary>
        private void SetupPage()
        {
            SJPWeb master = (SJPWeb)this.Master;

            // Hide all london 2012 branding
            master.DisplayHeader = false;
            master.DisplayFooter = false;
            master.DisplaySideBarLeft = false;
            master.DisplaySideBarRight = false;
        }

        /// <summary>
        /// Sets up the controls on the page
        /// </summary>
        private void SetupControls()
        {
            JourneyHelper journeyHelper = new JourneyHelper();

            // Journey leg to be shown
            string journeyRequestHash = string.Empty;
            Journey journeyOutward = null;
            Journey journeyReturn = null;
            JourneyLeg leg = null;

            // Get journey leg from querystring/session
            bool legFound = journeyHelper.GetJourneyLeg(out leg);

            // Get journey (could have arrived with no session), to determine outward or return direction
            bool journeysFound = journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            bool isReturn = (journeyOutward == null);

            // No leg specified in querystring, assume it'll be the first leg for outward journey or last for the return
            if (!legFound && journeysFound)
            {
                leg = !isReturn ? journeyOutward.JourneyLegs.FirstOrDefault() : journeyReturn.JourneyLegs.LastOrDefault();
            }

            // Build and set journey coordinates
            if (leg != null)
            {
                MapHelper mapHelper = new MapHelper(Global.SJPResourceManager);
                
                // Get map points and add to the hidden fields
                List<LatitudeLongitude> coords = mapHelper.GetJourneyLegPoints(
                    leg,
                    isReturn,
                    true,
                    DebugHelper.ShowDebug);

                // At least two coords should be found
                if (coords != null && coords.Count >= 2)
                {
                    mapStartLocationCoordinate.Value = coords.FirstOrDefault().ToString();
                    mapEndLocationCoordinate.Value = coords.LastOrDefault().ToString();
                    mapTravelMode.Value = string.Empty;

                    // For debugging 
                    if (DebugHelper.ShowDebug)
                    {
                        if (leg.Mode != SJPModeType.Walk)
                        {
                            //See for acceptable values https://developers.google.com/maps/documentation/javascript/directions#TravelModes
                            mapTravelMode.Value = "DRIVING";
                            mapTravelMode.Visible = true;
                        }
                    }
                }
                else
                {
                    // No coords found, possibly latlongs failed to be converted. 
                    // Log error, and transfer to error page
                    SetPageTransfer(PageId.Error);
                }
            }
            else
            {
                // No journey found, invalid scenario, send to error page
                SetPageTransfer(PageId.Error);
            }
        }

        #endregion

    }
}