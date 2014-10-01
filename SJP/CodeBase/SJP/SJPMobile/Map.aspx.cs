// *********************************************** 
// NAME             : Map.aspx.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 Feb 2012
// DESCRIPTION  	: Map page
// ************************************************
// 
                
using System;
using SJP.Common;
using SJP.Common.LocationService;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPMobile.Adapters;

namespace SJP.UserPortal.SJPMobile
{
    /// <summary>
    /// Map page
    /// </summary>
    public partial class Map : SJPPageMobile
    {
        #region Variables

        /// <summary>
        /// Enum used for map mode
        /// </summary>
        private enum MapMode
        {
            Location,
            LocationCurrent,
            Journey,
            WalkLeg
        }

        private MapMode mapMode = MapMode.LocationCurrent;
        private MapMode incomingMapMode = MapMode.Location; // Not location current in case the user doesn't select "my location"

        private string locationName = null;
        private string locationCoordinate = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Map()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.MobileMap;
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
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMapMode();

            // Get the hidden values set by the javascript, before this page's code updates the values
            SetSelectedMapLocation(); 
                        
            // Check if showing a journey
            if (mapMode == MapMode.Journey)
            {
                GetJourney();
            }
            // Show a specific location
            else if (mapMode == MapMode.Location)
            {
                GetLocation();
            }
            // Show walk leg
            else if (mapMode == MapMode.WalkLeg)
            {
                GetWalkLeg();
            }

            // Add javascripts specific for this page
            AddJavascript("Map.js");
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();

            SetupControlVisibility();

            // Set the hidden field to the map mode to use, allows map javascript to setup map accordingly
            modeType.Value = mapMode.ToString();
        }

        #endregion
        
        #region Event handlers

        /// <summary>
        /// Set the location we have as the from / to location and return to input page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void useLocationBtn_Click(object sender, EventArgs e)
        {
            // Set the journey request in session with the location we have then return to input.aspx
            SJPLocation location = null;

            // Retrieve the journey request
            JourneyResultHelper resultHelper = new JourneyResultHelper();
            ISJPJourneyRequest journeyRequest = resultHelper.JourneyRequest;

            if (incomingMapMode == MapMode.LocationCurrent)
            {
                // Use the hidden location values set by the javascript,
                // should be set by the page_load
                if (!string.IsNullOrEmpty(locationCoordinate))
                {
                    LocationHelper locHelper = new LocationHelper();

                    location = locHelper.GetSJPLocation(locationCoordinate, locationName, SJPLocationType.CoordinateLL);

                    if (location != null)
                    {
                        JourneyInputAdapter adapter = new JourneyInputAdapter();

                        // Work out which location to overwrite
                        if ((journeyRequest.Origin != null && (journeyRequest.Origin is SJPVenueLocation))
                            && (journeyRequest.Destination != null && (journeyRequest.Destination is SJPVenueLocation)))
                        {
                            // Both origin and destination are venues, 
                            // therefore update origin (assume its non-venue to venue request)
                            adapter.ValidateAndUpdateSJPRequestOriginOrDestination(location, true);
                        }
                        else if ((journeyRequest.Origin == null)
                                || (!(journeyRequest.Origin is SJPVenueLocation)))
                        {
                            // origin is null or non-venue
                            adapter.ValidateAndUpdateSJPRequestOriginOrDestination(location, true);
                        }
                        else if ((journeyRequest.Destination == null)
                                || (!(journeyRequest.Destination is SJPVenueLocation)))
                        {
                            // destination is null or non-venue
                            adapter.ValidateAndUpdateSJPRequestOriginOrDestination(location, false);
                        }

                        // Transfer to input page
                        SetPageTransfer(PageId.MobileInput);
                    }
                    else
                    {
                        // If location not found, and as this will be a lat/long search type, 
                        // add message to session and return back to input page
                        SJPMessage message = new SJPMessage("LocationControl.ambiguityText.noLocationUKFound.Text", SJPMessageType.Error);

                        SessionHelper sessionHelper = new SessionHelper();
                        sessionHelper.AddMessage(message);

                        // Transfer to input page
                        SetPageTransfer(PageId.MobileInput);
                    }                    
                }
            }
            else
            {
                // Current location was not selected on map

                // Use what was previously set by doing nothing
                SetPageTransfer(PageId.MobileInput);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            loadingImage.ImageUrl = ImagePath + GetResourceMobile("JourneySummary.LoadingImage.Imageurl");
            loadingImage.AlternateText = Server.HtmlDecode(GetResourceMobile("JourneySummary.LoadingImage.AlternateText"));
            loadingImage.ToolTip = Server.HtmlDecode(GetResourceMobile("JourneySummary.LoadingImage.ToolTip"));

            loadingMessage.Text = Server.HtmlDecode(GetResourceMobile("JourneyMap.MapLoading.Text"));

            lblMapInfoNonJS.Text = GetResourceMobile("JourneyMap.MapInfo.NonJavascript");

            useLocationBtn.Text = Server.HtmlDecode(page.GetResourceMobile("JourneyMap.UseLocation.Text"));
            useLocationBtn.ToolTip = Server.HtmlDecode(page.GetResourceMobile("JourneyMap.UseLocation.ToolTip"));

            currentLocationButton.ToolTip = page.GetResource("LocationControl.CurrentLocation.Tooltip");
            
            viewJourneyBtn.Text = GetResourceMobile("JourneyMap.ViewJourney.Text");
            viewJourneyBtn.ToolTip = GetResourceMobile("JourneyMap.ViewJourney.ToolTip");
        }

        /// <summary>
        /// Sets up the controls on the page
        /// </summary>
        private void SetupControls()
        {
        }

        /// <summary>
        /// Sets up the control visibilities
        /// </summary>
        private void SetupControlVisibility()
        {
            if (mapMode == MapMode.Journey)
            {
                useLocationBtn.Visible = false;
            }
            else if (mapMode == MapMode.WalkLeg)
            {
                useLocationBtn.Visible = false;
            }
        }

        /// <summary>
        /// Sets the pages current map mode
        /// </summary>
        private void SetMapMode()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            if (!string.IsNullOrEmpty(modeType.Value))
            {
                try
                {
                    // Get the hidden field map mode, in case the javascript has altered the mode
                    incomingMapMode = (MapMode)Enum.Parse(typeof(MapMode), modeType.Value, true);
                }
                catch { }
            }

            // journey request hash, journey id and show walk - mapping walk leg
            if (page.QueryStringContains(QueryStringKey.JourneyRequestHash)
                && page.QueryStringContains(QueryStringKey.JourneyIdOutward)
                && page.QueryStringContains(QueryStringKey.ShowWalk))
            {
                mapMode = MapMode.WalkLeg;
            }
            // journey request hash and journey id, then displaying for journey
            else if (page.QueryStringContains(QueryStringKey.JourneyRequestHash)
                && page.QueryStringContains(QueryStringKey.JourneyIdOutward))
            {
                mapMode = MapMode.Journey;
            }
            else
            {
                // If there is a journey request, then displaying map for a location in the request
                JourneyResultHelper resultHelper = new JourneyResultHelper();

                ISJPJourneyRequest journeyRequest = resultHelper.JourneyRequest;

                if (journeyRequest != null)
                {
                    mapMode = MapMode.Location;
                }
                else
                {
                    // No journey request, so display for current location
                    mapMode = MapMode.LocationCurrent;
                }
            }
        }

        /// <summary>
        /// Sets the location coordinate and value from the hidden location values
        /// </summary>
        private void SetSelectedMapLocation()
        {
            if (!string.IsNullOrEmpty(mapLocationCoordinate.Value))
            {
                locationCoordinate = mapLocationCoordinate.Value;
            }

            if (!string.IsNullOrEmpty(mapLocationName.Value))
            {
                locationName = mapLocationName.Value;
            }
        }

        /// <summary>
        /// Gets the journey from the session
        /// </summary>
        private void GetJourney()
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

            // Build and set journey coordinates
            if (journeyOutward != null)
            {
                MapHelper mapHelper = new MapHelper(Global.SJPResourceManager);

                // Get map points and add to the hidden journey map points field
                journeyPoints.Value = mapHelper.GetJourneyMapPoints(journeyOutward, ((SJPPageMobile)Page).ImagePath, true);

                SetVenueMapLink(journeyRequest, journeyOutward);
            }
            else
            {
                // No journey found, revert the map mode to location current
                mapMode = MapMode.LocationCurrent;
            }
        }

        /// <summary>
        /// Gets the walk leg from the session
        /// </summary>
        private void GetWalkLeg()
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

            // For walk leg only show start and end points - no route
            if (journeyOutward != null)
            {
                MapHelper mapHelper = new MapHelper(Global.SJPResourceManager);

                // Get map points and add to the hidden journey map points field
                if (journeyRequest.Origin.TypeOfLocation == SJPLocationType.Venue)
                {
                    journeyPoints.Value = mapHelper.GetWalkLegMobile(journeyOutward, true);
                }
                else
                {
                    journeyPoints.Value = mapHelper.GetWalkLegMobile(journeyOutward, false);
                }
            }
            else
            {
                // No journey found, revert the map mode to location current
                mapMode = MapMode.LocationCurrent;
            }
        }        
        /// <summary>
        /// Get the location to show from the journey request object
        /// </summary>
        private void GetLocation()
        {
            // Journey request
            JourneyResultHelper resultHelper = new JourneyResultHelper();
            ISJPJourneyRequest journeyRequest = resultHelper.JourneyRequest;
            
            SJPLocation location = null;

            if (journeyRequest != null)
            {
                // Work out which location to use
                if (journeyRequest.Origin != null && !(journeyRequest.Origin is SJPVenueLocation))
                {
                    // use the origin
                    location = journeyRequest.Origin;
                }
                else if (journeyRequest.Destination != null && !(journeyRequest.Destination is SJPVenueLocation))
                {
                    // use the destination
                    location = journeyRequest.Destination;
                }

                if (location != null)
                {
                    LocationHelper locationHelper = new LocationHelper();
                    
                    LatitudeLongitude latLong = locationHelper.GetLatitudeLongitudeCoordinate(location.GridRef);

                    if (latLong != null)
                    {
                        // Update hidden field to tell map javascript show the lat long location
                        mapLocationName.Value = location.DisplayName;
                        mapLocationCoordinate.Value = latLong.ToString();
                    }
                }
                else 
                {
                    // No location, revert the map mode to location current
                    mapMode = MapMode.LocationCurrent;
                }
            }
            else
            {
                // No journey request, revert the map mode to location current
                mapMode = MapMode.LocationCurrent;
            }
        }

        /// <summary>
        /// Sets up the map links for any Venue locations specified
        /// </summary>
        private void SetVenueMapLink(ISJPJourneyRequest journeyRequest, Journey journey)
        {
            if (journeyRequest != null)
            {
                if (journeyRequest.Origin != null && journeyRequest.Origin is SJPVenueLocation)
                {
                    originVenueMapControl.SetVenue((SJPVenueLocation)journeyRequest.Origin,
                        (journey != null) ? journey.StartTime.Date : DateTime.MinValue);
                }

                if (journeyRequest.Destination != null && journeyRequest.Destination is SJPVenueLocation)
                {
                    destinationVenueMapControl.SetVenue((SJPVenueLocation)journeyRequest.Destination,
                        (journey != null) ? journey.EndTime.Date : DateTime.MinValue);
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
