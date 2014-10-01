// *********************************************** 
// NAME             : LocationHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 03 Feb 2012
// DESCRIPTION  	: Location helper class for methods related to locations
// ************************************************
// 

using System;
using SJP.Common.EventLogging;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.CoordinateConvertorProvider;
using SJP.UserPortal.JourneyControl;
using Logger = System.Diagnostics.Trace;
using LS = SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;
using System.Collections.Generic;
using System.Text;

namespace SJP.Common.Web
{
    public class LocationHelper
    {
        #region Private members

        private SessionHelper sessionHelper;
        
        // Used to populate locations
        private LS.LocationService locationService = null;

        private List<SJPLocation> venues = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationHelper()
        {
            sessionHelper = new SessionHelper();

            locationService = SJPServiceDiscovery.Current.Get<LS.LocationService>(ServiceDiscoveryKey.LocationService);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Uses the query string location id and locationtype to return an SJPLocation
        /// </summary>
        /// <returns>Null if failed</returns>
        public SJPLocation GetSJPLocation(string locationId, string locationName, SJPLocationType locationType)
        {
            try
            {
                if (locationType == SJPLocationType.CoordinateLL)
                {
                    // Use coordinate service to obtain an easting northing coordinate
                    OSGridReference osgr = GetEastingNorthingCoordinate(new LatitudeLongitude(locationId));

                    return locationService.GetSJPLocationForCoordinate(locationName, osgr);
                }
                else if (locationType == SJPLocationType.CoordinateEN)
                {
                    OSGridReference osgr = new OSGridReference(locationId);

                    return locationService.GetSJPLocationForCoordinate(locationName, osgr);
                }
                else
                {
                    return locationService.GetSJPLocation(locationId, locationType);
                }
            }
            catch
            {
                // Any exception, then its an unrecognised value, so default to unknown
                return null;
            }
        }

        /// <summary>
        /// Checks if the location is accessible, if accessible journey required.
        /// Retrieves the SJPJourneyRequest from session to check
        /// </summary>
        /// <returns>True if its ok to plan accessible journey for location</returns>
        public bool CheckAccessibleLocation(bool isOrigin)
        {
            ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();
            SJPAccessiblePreferences accessiblePreference = sjpJourneyRequest.AccessiblePreferences;

            if (accessiblePreference.Accessible)
            {
                // Check to see if the location is accessible,
                // only when special assistance or step free access required
                if (accessiblePreference.RequireSpecialAssistance || accessiblePreference.RequireStepFreeAccess)
                {
                    LS.SJPLocation location = null;

                    // Only check for non-venues
                    if ((isOrigin) && (sjpJourneyRequest.Origin != null) && !(sjpJourneyRequest.Origin is LS.SJPVenueLocation))
                    {
                        location = sjpJourneyRequest.Origin;
                    }
                    else if ((!isOrigin) && (sjpJourneyRequest.Destination != null) && !(sjpJourneyRequest.Destination is LS.SJPVenueLocation))
                    {
                        location = sjpJourneyRequest.Destination;
                    }


                    if (location != null)
                    {
                        LS.LocationService locationService = SJPServiceDiscovery.Current.Get<LS.LocationService>(ServiceDiscoveryKey.LocationService);

                        // Check 1) Is location admin area/district in a GNAT area?
                        if (locationService.IsGNATAdminArea(location.AdminAreaCode, location.DistrictCode,
                            accessiblePreference.RequireStepFreeAccess,
                            accessiblePreference.RequireSpecialAssistance))
                        {
                            // Location is in a defined GNAT area, return True
                            return true;
                        }
                        // Check 2) Is location GNAT? 
                        // Only allow GNAT look up for locations with 1 Naptan.
                        // If naptan is not provided, possibly a locality, return False
                        // If more than one naptan provided, possibly an exchange group, return False
                        else if (location.Naptan.Count == 1)
                        {
                            return locationService.IsGNAT(location.Naptan[0],
                                        accessiblePreference.RequireStepFreeAccess,
                                        accessiblePreference.RequireSpecialAssistance);
                        }
                        else
                        {
                            // Location is not GNAT
                            return false;
                        }
                    }
                }
            }

            // Otherwise valid to plan the accessible journey
            return true;
        }

        /// <summary>
        /// Checks if the venue location is accessible, if accessible journey required
        /// </summary>
        /// <remarks>
        /// AccessibleNaptans must be populated in the relevant venue locations prior call to this method
        /// </remarks>
        /// <returns>True if its ok to plan accessible journey for venue</returns>
        public bool CheckAccessibleLocationForVenue(bool isOrigin)
        {
            ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();
            SJPAccessiblePreferences accessiblePreference = sjpJourneyRequest.AccessiblePreferences;

            if (accessiblePreference.Accessible)
            {
                if ((isOrigin) && (sjpJourneyRequest.Origin != null))
                {
                    if (sjpJourneyRequest.Origin is LS.SJPVenueLocation)
                    {
                        LS.SJPVenueLocation originVenueLocation = sjpJourneyRequest.Origin as LS.SJPVenueLocation;

                        // Check the IsGNAT property to see if the venue is accessible
                        if (!originVenueLocation.IsGNAT)
                        {
                            // Not GNAT and if accessible naptans list is null or empty return false
                            if (originVenueLocation.AccessibleNaptans != null)
                            {
                                return originVenueLocation.AccessibleNaptans.Count > 0;
                            }
                            else
                                return false;
                        }
                    }
                }
                else if ((!isOrigin) && (sjpJourneyRequest.Destination != null))
                {
                    if (sjpJourneyRequest.Destination is LS.SJPVenueLocation)
                    {
                        LS.SJPVenueLocation destinationVenueLocation = sjpJourneyRequest.Destination as LS.SJPVenueLocation;

                        // Check the IsGNAT property to see if the venue is accessible
                        if (!destinationVenueLocation.IsGNAT)
                        {
                            // Not GNAT and if accessible naptans list is null or empty return false
                            if (destinationVenueLocation.AccessibleNaptans != null)
                            {
                                return destinationVenueLocation.AccessibleNaptans.Count > 0;
                            }
                            else
                                return false;
                        }
                    }
                }
            }

            // Otherwise valid to plan the accessible journey
            return true;
        }
        
        /// <summary>
        /// Method which gets an easting northing coordinate for a latitude longitudes
        /// </summary>
        /// <returns>null if fails or OSGridReference</returns>
        public OSGridReference GetEastingNorthingCoordinate(LatitudeLongitude latlong)
        {
            OSGridReference osgr = null;

            // Only perform the conversion if latitude longitude supported
            bool enabled = Properties.Current["LandingPage.Location.Coordinate.LatitudeLongitude.Switch"].Parse(false);
            
            if (enabled)
            {
                string logMessge = string.Empty;
                bool success = false;

                if (SJPTraceSwitch.TraceVerbose)
                {
                    logMessge = string.Format("Calling CoordinateConvertor to convert latitude/longitude location coordinate[{0}]",
                        (latlong != null) ? latlong.ToString() : string.Empty);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
                }

                try
                {
                    // Get the service used to convert the coordinates
                    ICoordinateConvertor coordinateConvertor = SJPServiceDiscovery.Current.Get<ICoordinateConvertor>(ServiceDiscoveryKey.CoordinateConvertor);

                    // Call web service
                    osgr = coordinateConvertor.GetOSGridReference(latlong);

                    // For logging
                    success = true;
                }
                catch (Exception ex)
                {
                    string message = string.Format("Calling CoordinateConvertor to convert latitude/longitude location coordinate[{0}] threw an exception: {1}",
                        (latlong != null) ? latlong.ToString() : string.Empty,
                        ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, message, ex));
                }

                if (SJPTraceSwitch.TraceVerbose)
                {
                    logMessge = string.Format("Calling CoordinateConvertor to convert latitude/longitude location coordinate completed with success[{0}] latlong[{1}] eastnorth[{2}]",
                        success,
                        (latlong != null) ? latlong.ToString() : string.Empty,
                        (osgr != null) ? osgr.ToString() : string.Empty);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
                }
            }

            return osgr;
        }

        /// <summary>
        /// Method which gets a latitude longitude coordinate an easting northing
        /// </summary>
        /// <returns>null if fails or OSGridReference</returns>
        public LatitudeLongitude GetLatitudeLongitudeCoordinate(OSGridReference osgr)
        {
            LatitudeLongitude latlong = null;

            string logMessge = string.Empty;
                bool success = false;

                if (SJPTraceSwitch.TraceVerbose)
                {
                    logMessge = string.Format("Calling CoordinateConvertor to convert easting/northing location coordinate[{0}]",
                        (osgr != null) ? osgr.ToString() : string.Empty);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
                }

                try
                {
                    // Get the service used to convert the coordinates
                    ICoordinateConvertor coordinateConvertor = SJPServiceDiscovery.Current.Get<ICoordinateConvertor>(ServiceDiscoveryKey.CoordinateConvertor);

                    // Call web service
                    latlong = coordinateConvertor.GetLatitudeLongitude(osgr);

                    // For logging
                    success = true;
                }
                catch (Exception ex)
                {
                    string message = string.Format("Calling CoordinateConvertor to convert easting/northing location coordinate[{0}] threw an exception: {1}",
                        (osgr != null) ? osgr.ToString() : string.Empty,
                        ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, message, ex));
                }

                if (SJPTraceSwitch.TraceVerbose)
                {
                    logMessge = string.Format("Calling CoordinateConvertor to convert easting/northing location coordinate completed with success[{0}] latlong[{1}] eastnorth[{2}]",
                        success,
                        (latlong != null) ? latlong.ToString() : string.Empty,
                        (osgr != null) ? osgr.ToString() : string.Empty);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
                }
            

            return latlong;
        }

        /// <summary>
        /// Returns a string of names for venue naptans 
        /// </summary>
        /// <returns></returns>
        public string GetLocationNames(List<string> venueNaptans)
        {
            // Get all the venues and store locally to prevent repeated calls to the service
            if (venues == null)
            {
                venues = locationService.GetSJPVenueLocations();
            }

            StringBuilder sb = new StringBuilder();
            SJPLocation venue = null;

            foreach (string naptan in venueNaptans)
            {
                // Find the venue for the naptan
                venue = venues.Find(delegate(SJPLocation loc) { return loc.ID == naptan; });
                if (venue != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(venue.DisplayName);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
