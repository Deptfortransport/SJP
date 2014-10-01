// *********************************************** 
// NAME             : MapHelper.cs      
// AUTHOR           : David Lane
// DATE CREATED     : 21 Mar 2012
// DESCRIPTION  	: MapHelper class providing helper methods for map display
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;
using SJP.Common.LocationService;
using SJP.Common.ResourceManager;
using SJP.UserPortal.JourneyControl;
using Logger = System.Diagnostics.Trace;
using System.Collections.Specialized;

namespace SJP.Common.Web
{
    /// <summary>
    /// MapHelper class providing helper methods for map display
    /// </summary>
    public class MapHelper
    {
        #region Private members

        private SJPResourceManager rm;
        private LocationHelper locationHelper = null;
        private char mapPointsSeperator = '|';

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MapHelper(SJPResourceManager rm)
        {
            this.rm = rm;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Builds the Map SJP page url, adding the journey to be used
        /// </summary>
        /// <returns></returns>
        public string BuildMapURL(string url, string journeyRequestHash, int journeyId, int journeyLegIndex, bool isReturn)
        {
            string mapUrl = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                URLHelper urlHelper = new URLHelper();

                NameValueCollection nvc = new NameValueCollection();

                if (!string.IsNullOrEmpty(journeyRequestHash))
                {
                    nvc.Add(QueryStringKey.JourneyRequestHash, journeyRequestHash);
                }

                if (isReturn)
                {
                    nvc.Add(QueryStringKey.JourneyIdReturn, journeyId.ToString());
                }
                else
                {
                    nvc.Add(QueryStringKey.JourneyIdOutward, journeyId.ToString());
                }

                if (journeyLegIndex >= 0)
                {
                    if (isReturn)
                    {
                        nvc.Add(QueryStringKey.JourneyLegIdReturn, journeyLegIndex.ToString());
                    }
                    else
                    {
                        nvc.Add(QueryStringKey.JourneyLegIdOutward, journeyLegIndex.ToString());
                    }
                }

                mapUrl = urlHelper.AddQueryStringParts(url, nvc);
            }

            return mapUrl;
        }

        /// <summary>
        /// Builds a list journey map coordinates for the map javascript to display on the map
        /// </summary>
        public List<LatitudeLongitude> GetJourneyLegPoints(JourneyLeg journeyLeg, bool isReturn, bool useLegNavigationPath, bool isDebug)
        {
            if (locationHelper == null)
                locationHelper = new LocationHelper();

            List<LatitudeLongitude> coords = new List<LatitudeLongitude>();

            if (journeyLeg != null)
            {
                // Currently only showing map for walk leg
                if ((journeyLeg.Mode == SJPModeType.Walk) || (isDebug))
                {
                    LatitudeLongitude latlong = null;

                    // If the useLegNavigationPath, then
                    // - for return journeys the start coord is the last coord in the nav path (or start location if null)
                    // - for outward journeys the end coord is the first coord in the nav path (or end location if null)

                    // Start coordinate
                    if (isReturn)
                    {
                        latlong = GetNavigationPathCoordinate(journeyLeg, isReturn);
                    }
                    if (latlong == null)
                    {
                        latlong = locationHelper.GetLatitudeLongitudeCoordinate(journeyLeg.LegStart.Location.GridRef);
                    }

                    if (latlong != null)
                        coords.Add(latlong);
                    latlong = null;

                    // End coordinate
                    if (!isReturn)
                    {
                        latlong = GetNavigationPathCoordinate(journeyLeg, isReturn);
                    }
                    if (latlong == null)
                    {
                        latlong = locationHelper.GetLatitudeLongitudeCoordinate(journeyLeg.LegEnd.Location.GridRef);
                    }

                    if (latlong != null)
                        coords.Add(latlong);
                    latlong = null;


                    if (isDebug)
                    {
                        // If both coords are the same (navigation paths may contain the same coords as the leg start/end),
                        // clear and use the location coords instead
                        if (coords.Count == 2)
                        {
                            if (coords[0].Latitude == coords[1].Latitude
                                && coords[0].Longitude == coords[1].Longitude)
                            {
                                coords.Clear();
                                coords.Add(locationHelper.GetLatitudeLongitudeCoordinate(journeyLeg.LegStart.Location.GridRef));
                                coords.Add(locationHelper.GetLatitudeLongitudeCoordinate(journeyLeg.LegEnd.Location.GridRef));
                            }
                        }
                    }
                }
            }

            return coords;
        }

        /// <summary>
        /// Builds a list of (two) coordinates for the map javascript to display on the map
        /// </summary>
        public string GetWalkLegMobile(Journey journey, bool startIsVenue)
        {
            if (locationHelper == null)
                locationHelper = new LocationHelper();

            StringBuilder builder = new StringBuilder();

            // We're looking for the walk leg that either begins a journey to a venue or ends a journey
            // from a venue
            JourneyLeg walkLeg = null;
            if (startIsVenue)
            {
                // Look for a walk leg at the end of the journey
                if (journey.JourneyLegs[journey.JourneyLegs.Count - 1].Mode == SJPModeType.Walk)
                {
                    walkLeg = journey.JourneyLegs[journey.JourneyLegs.Count - 1];

                    // Start
                    LatitudeLongitude latlong = GetNavigationPathCoordinate(walkLeg, true);
                    string displayText = walkLeg.LegStart.Location.DisplayName;

                    if (latlong == null)
                    {
                        latlong = locationHelper.GetLatitudeLongitudeCoordinate(walkLeg.LegStart.Location.GridRef);
                    }

                    AddMapPoint(builder, latlong, displayText, true, false);

                    // End
                    latlong = locationHelper.GetLatitudeLongitudeCoordinate(walkLeg.LegEnd.Location.GridRef);
                    displayText = walkLeg.LegEnd.Location.DisplayName;
                    AddMapPoint(builder, latlong, displayText, true, false);
                }
            }
            else
            {
                // Look for a walk leg at the start of the journey
                if (journey.JourneyLegs[0].Mode == SJPModeType.Walk)
                {
                    walkLeg = journey.JourneyLegs[0];

                    // Start
                    LatitudeLongitude latlong = locationHelper.GetLatitudeLongitudeCoordinate(walkLeg.LegStart.Location.GridRef);
                    string displayText = walkLeg.LegStart.Location.DisplayName;
                    AddMapPoint(builder, latlong, displayText, true, false);

                    // End
                    latlong = GetNavigationPathCoordinate(walkLeg, false);

                    if (latlong == null)
                    {
                        latlong = locationHelper.GetLatitudeLongitudeCoordinate(walkLeg.LegEnd.Location.GridRef);
                    }

                    displayText = walkLeg.LegEnd.Location.DisplayName;
                    AddMapPoint(builder, latlong, displayText, true, false);
                }
            }

            return builder.ToString().TrimEnd(mapPointsSeperator);
        }

        /// <summary>
        /// Builds a list of journey map coordinate for the map javascript to display on the map
        /// </summary>
        public string GetJourneyMapPoints(Journey journey, string imagePath, bool mobile)
        {
            if (locationHelper == null)
                locationHelper = new LocationHelper();

            StringBuilder builder = new StringBuilder();

            // Build up a list of coordinates that the map can use to display the journey route
            // Map Javascript expects each point to be in format
            // lat,long,displaytext,pushpin,plotroute

            // Need to know if we're starting and / or ending at a venue
            string startMapLinkHtml = string.Empty;
            string endMapLinkHtml = string.Empty;
            string cycleParkImage = string.Empty;
            if (mobile)
            {
                cycleParkImage = "./Version/Images/cycle/CyclePark.png";
            }
            else
            {
                cycleParkImage = "../VersionGTW/Images/cycle/CyclePark.png";
            }

            if (journey.JourneyLegs[0].LegStart.Location is SJPVenueLocation)
            {
                SJPVenueLocation venue = (SJPVenueLocation)journey.JourneyLegs[0].LegStart.Location;
                string navigateUrl = GetVenueMapURL(venue, journey.JourneyLegs[0].StartTime.Date, imagePath);
                string link = string.Empty;
                if (mobile)
                {
                    link = "javascript:showOriginVenueMap()";
                }
                else
                {
                    link = venue.VenueMapUrl;
                }

                startMapLinkHtml =
                    string.Format(
                        "<div><div class=\"infoboxLocationImg\"><img src=\"{1}\" /></div><div class=\"infoboxLocationLink\"><a id=\"endLocationMapLink\" href=\"{2}\">View map of {0}</a></div></div>",
                        venue.DisplayName,
                        cycleParkImage,
                        link);
            }

            if (journey.JourneyLegs[journey.JourneyLegs.Count - 1].LegEnd.Location is SJPVenueLocation)
            {
                SJPVenueLocation venue = (SJPVenueLocation)journey.JourneyLegs[journey.JourneyLegs.Count - 1].LegEnd.Location;
                string navigateUrl = GetVenueMapURL(venue, journey.JourneyLegs[journey.JourneyLegs.Count - 1].EndTime.Date, imagePath);
                string link = string.Empty;
                if (mobile)
                {
                    link = "javascript:showDestinationVenueMap()";
                }
                else
                {
                    link = venue.VenueMapUrl;
                }

                endMapLinkHtml = 
                    string.Format(
                        "<div><div class=\"infoboxLocationImg\"><img src=\"{1}\" /></div><div class=\"infoboxLocationLink\"><a id=\"endLocationMapLink\" href=\"{2}\">View map of {0}</a></div></div>",
                        venue.DisplayName,
                        cycleParkImage,
                        link);
            }

            // Only include the cycle journey and the start and end points thereof
            for (int i = 0; i < journey.JourneyLegs.Count; i++)
            {
                JourneyLeg leg = journey.JourneyLegs[i];

                // Currently only showing route for cycle journey
                if (leg.Mode == SJPModeType.Cycle)
                {
                    // start point
                    if (i > 0)
                    {
                        JourneyLeg start = journey.JourneyLegs[i - 1];
                        AddMapPoint(builder, start.LegEnd.Location.LatitudeLongitudeCoordinate, "From: " + start.LegEnd.Location.DisplayName + startMapLinkHtml, true, true);
                    }
                    else
                    {
                        JourneyLeg start = journey.JourneyLegs[i];
                        AddMapPoint(builder, start.LegStart.Location.LatitudeLongitudeCoordinate, "From: " + start.LegStart.Location.DisplayName + startMapLinkHtml, true, true);
                    }

                    builder.Append(GetJourneyMapPointsForCycle(leg));

                    // end point
                    if (i < journey.JourneyLegs.Count - 1)
                    {
                        JourneyLeg end = journey.JourneyLegs[i + 1];
                        AddMapPoint(builder, end.LegStart.Location.LatitudeLongitudeCoordinate, "To: " + end.LegStart.Location.DisplayName + endMapLinkHtml, true, true);
                    }
                    else
                    {
                        JourneyLeg end = journey.JourneyLegs[i];
                        AddMapPoint(builder, end.LegEnd.Location.LatitudeLongitudeCoordinate, "To: " + end.LegEnd.Location.DisplayName + endMapLinkHtml, true, true);
                    }
                }
            }

            // Make sure last map point doesnt have the seperator
            return builder.ToString().TrimEnd(mapPointsSeperator);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method which returns the venue map url to use, either retrieving from resource manager, or from the 
        /// venue location itself
        /// </summary>
        /// <param name="venue"></param>
        /// <param name="journeyDateTime"></param>
        /// <returns></returns>
        private string GetVenueMapURL(SJPVenueLocation venue, DateTime journeyDateTime, string imagePath)
        {
            string mapUrl = string.Empty;
            Language language = CurrentLanguage.Value;
            string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
            string RC = SJPResourceManager.COLLECTION_JOURNEY;

            // Get map image url from resource manager, for the journey date
            mapUrl = rm.GetString(language, RG, RC, string.Format("Venue.VenueMapImage.{0}.Url.{1}",
                venue.Naptan.FirstOrDefault(),
                journeyDateTime.ToString("yyyyMMdd")));

            // Otherwise get default map image url from resource manager
            if (string.IsNullOrEmpty(mapUrl))
            {
                mapUrl = rm.GetString(language, RG, RC, string.Format("Venue.VenueMapImage.{0}.Url",
                venue.Naptan.FirstOrDefault()));
            }

            // Not in resource manager, use the map url in the venue object
            if (string.IsNullOrEmpty(mapUrl))
            {
                mapUrl = (venue.VenueMapUrl);
            }
            else
            {
                // Append the server image path to the url from resource manager
                mapUrl = imagePath + mapUrl;
            }

            // Don't want the initial "~/"
            mapUrl = mapUrl.Replace("~/", "");
            return mapUrl;
        }

        /// <summary>
        /// Returns map points for a cycle journey leg
        /// </summary>
        /// <param name="cycleLeg"></param>
        /// <returns></returns>
        private string GetJourneyMapPointsForCycle(JourneyLeg cycleLeg)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                JourneyLeg cycleJourney = cycleLeg;

                if (cycleJourney != null)
                {

                    // Check the latitude longitude coordinates were set correctly during the journey response processing from the Cycle Trip Planner.
                    // If at least one detail contains LatLong then assume ok.
                    bool latitudeLongitudesAreValid = false;

                    var detailWithPopulatedLatLong = cycleJourney.JourneyDetails
                                                    .Where(jd => jd.LatitudeLongitudes != null)
                                                    .Union(
                                                        cycleJourney.JourneyDetails
                                                            .Where(jd => jd.LatitudeLongitudes.Length > 0)
                                                    );
                    latitudeLongitudesAreValid = detailWithPopulatedLatLong.Count() > 0;

                    if (latitudeLongitudesAreValid)
                    {
                        #region Build map points for cycle leg

                        DefaultCycleJourneyDetailFormatter cycleDetailFormatter = new DefaultCycleJourneyDetailFormatter(
                            cycleJourney, CurrentLanguage.Value, rm);

                        // Get the cycle journey directions
                        List<FormattedJourneyDetail> journeyDetails = cycleDetailFormatter.GetJourneyDetails();

                        // Get the cycle journey detail objects
                        List<CycleJourneyDetail> cycleJourneyDetails = cycleJourney.JourneyDetails.Cast<CycleJourneyDetail>().ToList();

                        // Get the coordinates for the entire journey
                        List<LatitudeLongitude> gridReferences = GetAllLatitudeLongitudes(cycleJourney);


                        string direction = string.Empty;


                        // Loop through the directions (first and last are start and end locations)
                        for (int i = 0; i < journeyDetails.Count; i++)
                        {
                            FormattedCycleJourneyDetail cycleFormattedDetail = journeyDetails[i] as FormattedCycleJourneyDetail;
                            // Get the direction instruction
                            direction = cycleFormattedDetail.Instruction;


                            if (i == 0) // First direction
                            {
                                #region Start
                                // Set the lat/lon from First grid reference in journey
                                AddMapPoint(builder, gridReferences[0], string.Empty, false, true);

                                #endregion
                            }
                            else if (i == journeyDetails.Count - 1) // Last direction
                            {
                                #region End
                                // Set the lat/lon from Last grid reference in journey
                                AddMapPoint(builder, gridReferences[gridReferences.Count - 1], string.Empty, false, true);

                                #endregion
                            }
                            else
                            {
                                #region Directions
                                // Get the detail the instruction relates to
                                CycleJourneyDetail cjd = cycleJourneyDetails[i - 1];
                                bool writeDirection = true;

                                // Write the points contained for this CycleJourneyDetail
                                LatitudeLongitude[] coordinates = cjd.LatitudeLongitudes;

                                if (coordinates != null && coordinates.Length > 0)
                                {
                                    // loop through each coordinate there is for this detail, ignoring the last 
                                    // one in each array as this is used as the start for the next detail
                                    int items = (coordinates.Length == 1) ? 1 : (coordinates.Length - 1);

                                    for (int j = 0; j < items; j++)
                                    {
                                        if (writeDirection)
                                        {
                                            // For the first osgr on this detail, add the Direction instruction
                                            AddMapPoint(builder, coordinates[j], string.Empty, false, true);

                                            writeDirection = false;
                                        }
                                        else
                                        {
                                            AddMapPoint(builder, coordinates[j], string.Empty, false, true);
                                        }
                                    }
                                }

                                #endregion

                            }
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Map points create failed for CycleJourney. Message {0}.", ex.Message);

                OperationalEvent operationalEvent = new OperationalEvent
                    (SJPEventCategory.Infrastructure, SJPTraceLevel.Error, message, ex);
                Logger.Write(operationalEvent);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns all LatitudeLongitudes for the journey as an array of LatitudeLongitudes.
        /// </summary>
        /// <returns></returns>
        private List<LatitudeLongitude> GetAllLatitudeLongitudes(JourneyLeg journey)
        {
            // Temp array used to group together polylines
            List<LatitudeLongitude> gridReferences = new List<LatitudeLongitude>();

            foreach (JourneyDetail detail in journey.JourneyDetails)
            {
                // Get the geometry values for this detail
                LatitudeLongitude[] latlongs = detail.LatitudeLongitudes;

                if (latlongs != null && latlongs.Length > 0)
                {
                    gridReferences.AddRange(latlongs);
                }
            }

            return gridReferences;
        }

        /// <summary>
        /// Adds a LatitudeLongitude coordinate to the string builder 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="latlong"></param>
        /// <param name="displayText"></param>
        private void AddMapPoint(StringBuilder builder, LatitudeLongitude latlong, string displayText, bool addPushpin, bool includeInRoute)
        {
            if (latlong != null)
            {
                builder.Append(Math.Round(latlong.Latitude, 6));
                builder.Append("@");
                builder.Append(Math.Round(latlong.Longitude, 6));
                builder.Append("@");
                builder.Append(displayText);
                builder.Append("@");
                builder.Append(addPushpin.ToString().ToLower());
                builder.Append("@");
                builder.Append(includeInRoute.ToString().ToLower());
                builder.Append(mapPointsSeperator);
            }
        }

        /// <summary>
        /// Returns the coordinate from the navigation path of the journey leg
        /// </summary>
        /// <param name="journeyLeg"></param>
        /// <param name="isReturn"></param>
        /// <returns></returns>
        private LatitudeLongitude GetNavigationPathCoordinate(JourneyLeg journeyLeg, bool isReturn)
        {
            LatitudeLongitude latlong = null;
            
            // Currently only PublicJourneyInterchangeDetail will contain a navigation path (held in the geometry property)
            PublicJourneyInterchangeDetail pjid = null;

            if (journeyLeg.JourneyDetails != null)
            {
                JourneyDetail detail = journeyLeg.JourneyDetails[0];

                if (detail is PublicJourneyInterchangeDetail)
                {
                    pjid = (PublicJourneyInterchangeDetail)detail;
                }
            }

            if ((pjid != null) && (pjid.Geometry != null))
            {
                OSGridReference[] osgrs = pjid.GetAllOSGRGridReferences();

                if (osgrs != null && osgrs.Length > 0)
                {
                    if (!isReturn)
                    {
                        // Find the first coord in the navigation path
                        latlong = locationHelper.GetLatitudeLongitudeCoordinate(osgrs.FirstOrDefault());
                    }
                    else
                    {
                        // Find the last coord in the navigation path
                        latlong = locationHelper.GetLatitudeLongitudeCoordinate(osgrs.LastOrDefault());
                    }
                }
            }

            return latlong;
        }

        #endregion
    }
}
