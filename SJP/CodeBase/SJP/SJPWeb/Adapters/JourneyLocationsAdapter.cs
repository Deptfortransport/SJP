// *********************************************** 
// NAME             : JourneyLocationsAdapter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 17 May 2011
// DESCRIPTION  	: JourneyLocationsAdapter class providing helper methods for Journey Locations intermediate page
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using SJP.Common;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SJPWeb.Adapters
{
    /// <summary>
    /// JourneyLocationsAdapter class providing helper methods for Journey Locations intermediate page
    /// </summary>
    public class JourneyLocationsAdapter
    {
        #region Private members

        private SessionHelper sessionHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyLocationsAdapter()
        {
            sessionHelper = new SessionHelper();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validates the parameters and populates a StopEvent SJPJourneyRequest object using 
        /// the existing SJPJourneyRequest in session, and adds to the session
        /// </summary>
        /// <returns>True if stop event request built and added to session</returns>
        public bool ValidateAndBuildSJPStopEventRequest(string venuePierNaPTAN, string remotePierNaPTAN)
        {
            bool valid = false;

            // Assume existing journey request to base the stop event request on is already valid
            if (!string.IsNullOrEmpty(venuePierNaPTAN) && !string.IsNullOrEmpty(remotePierNaPTAN))
            {
                // Retrieve the journey request 
                ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest();

                // Build the stop event request to submit using the journey request
                ISJPJourneyRequest sjpStopEventRequest = BuildSJPStopEventRequest(sjpJourneyRequest, venuePierNaPTAN, remotePierNaPTAN);

                if (sjpStopEventRequest != null)
                {
                    // Commit stop event request to session
                    sessionHelper.UpdateSessionStopEvent(sjpStopEventRequest);

                    valid = true;
                }
            }

            return valid;
        }

        /// <summary>
        /// Populates a new StopEvent SJPJourneyRequest object using the supplied StopEvent SJPJourneyRequest, 
        /// with the supplied parameters, and adds to the session
        /// </summary>
        /// <returns></returns>
        public bool ValidateAndBuildSJPStopEventRequestForReplan(ISJPJourneyRequest sjpStopEventRequest,
            bool replanOutwardRequired, bool replanReturnRequired,
            DateTime replanOutwardDateTime, DateTime replanReturnDateTime,
            List<Journey> outwardJourneys, List<Journey> returnJourneys)
        {
            bool valid = false;

            if (sjpStopEventRequest != null)
            {
                // Create a new stop event request. 
                // Must create a new request as it's hash will be different, and should allow the 
                // "browser back" to still function, and support multi-tabbing (assumption!)
                ISJPJourneyRequest sjpJourneyRequestReplan = BuildSJPStopEventRequestForReplan(sjpStopEventRequest,
                    replanOutwardRequired, replanReturnRequired,
                    replanOutwardDateTime, replanReturnDateTime,
                    outwardJourneys, returnJourneys);

                // Commit stop event request to session
                sessionHelper.UpdateSessionStopEvent(sjpJourneyRequestReplan);

                valid = true;
            }

            return valid;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds the SJP stop event request using journey request and selecter river service route
        /// </summary>
        /// <param name="venuePierNaPTAN">NaPTAN of venue pier</param>
        /// <param name="remotePierNaPTAN">NaPTAN of remote pier</param>
        /// <returns>SJPJourneyRequest object</returns>
        private ISJPJourneyRequest BuildSJPStopEventRequest(ISJPJourneyRequest sjpJourneyRequest, string venuePierNaPTAN, string remotePierNaPTAN)
        {
            if (sjpJourneyRequest != null)
            {
                SJPLocation venue = null;
                bool isOriginVenue = false;

                if (sjpJourneyRequest.Destination is SJPVenueLocation)
                {
                    venue = sjpJourneyRequest.Destination;
                    isOriginVenue = false;
                }
                else if (sjpJourneyRequest.Origin is SJPVenueLocation)
                {
                    venue = sjpJourneyRequest.Origin;
                    isOriginVenue = true;
                }

                if (venue != null)
                {
                    List<SJPVenueRiverService> riverServiceList;

                    // Get River Services
                    LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                    // River services list
                    riverServiceList = locationService.GetSJPVenueRiverServices(venue.Naptan);

                    // Retrive the venue river service so it's details can be used for the locations in the request
                    SJPVenueRiverService venueRiverService = riverServiceList.Where(rs => rs.RemotePierNaPTAN == remotePierNaPTAN && rs.VenuePierNaPTAN == venuePierNaPTAN).FirstOrDefault();

                    if (venueRiverService != null)
                    {
                        ISJPJourneyRequest sjpStopEventRequest = new SJPJourneyRequest();

                        SJPLocation destination = null;
                        SJPLocation origin = null;

                        if (isOriginVenue)
                        {
                            // From Venue Pier to the Remote Pier
                            destination = new SJPLocation(venueRiverService.RemotePierName, SJPLocationType.Station, SJPLocationType.Unknown, venueRiverService.RemotePierNaPTAN);
                            origin = new SJPLocation(venueRiverService.VenuePierName, SJPLocationType.Station, SJPLocationType.Unknown, venueRiverService.VenuePierNaPTAN);
                            origin.Locality = venue.Locality; // Locality is needed by the CJP
                        }
                        else
                        {
                            // To Venue Pier from the Remote Pier
                            origin = new SJPLocation(venueRiverService.RemotePierName, SJPLocationType.Station, SJPLocationType.Unknown, venueRiverService.RemotePierNaPTAN);
                            destination = new SJPLocation(venueRiverService.VenuePierName, SJPLocationType.Station, SJPLocationType.Unknown, venueRiverService.VenuePierNaPTAN);
                            destination.Locality = venue.Locality; // Locality is needed by the CJP
                        }

                        sjpStopEventRequest.Origin = origin;
                        sjpStopEventRequest.Destination = destination;

                        // Adjust date times with Transit time to/from venue to venue pier
                        sjpStopEventRequest.OutwardDateTime = sjpJourneyRequest.OutwardDateTime.Subtract(GetTransitTime(venue, venuePierNaPTAN, true));
                        sjpStopEventRequest.OutwardArriveBefore = false; //sjpJourneyRequest.OutwardArriveBefore; Ignored for StopEvent requests
                        sjpStopEventRequest.ReturnDateTime = sjpJourneyRequest.ReturnDateTime.Add(GetTransitTime(venue, venuePierNaPTAN, false));
                        sjpStopEventRequest.ReturnArriveBefore = false; //sjpJourneyRequest.ReturnArriveBefore; Ignored for StopEvent requests
                        sjpStopEventRequest.IsOutwardRequired = sjpJourneyRequest.IsOutwardRequired;
                        sjpStopEventRequest.IsReturnRequired = sjpJourneyRequest.IsReturnRequired;
                        sjpStopEventRequest.IsReturnOnly = sjpJourneyRequest.IsReturnOnly;

                        sjpStopEventRequest.PlannerMode = sjpJourneyRequest.PlannerMode;

                        // Only in Ferry mode for the river stop event request
                        sjpStopEventRequest.Modes = new List<SJPModeType>() { SJPModeType.Ferry };

                        // All request values have been set, now update the journey request hash.
                        // This determines the uniqueness of this journey request for this users session
                        sjpStopEventRequest.JourneyRequestHash = sjpStopEventRequest.GetSJPHashCode().ToString();

                        return sjpStopEventRequest;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Populates a new Stop Event SJPJourneyRequest object using the supplied Stop Event SJPJourneyRequest, 
        /// with the supplied parameters
        /// </summary>
        private ISJPJourneyRequest BuildSJPStopEventRequestForReplan(ISJPJourneyRequest sjpStopEventRequest,
            bool replanOutwardRequired, bool replanReturnRequired,
            DateTime replanOutwardDateTime, DateTime replanReturnDateTime,
            List<Journey> outwardJourneys, List<Journey> returnJourneys)
        {
            if (sjpStopEventRequest != null)
            {
                ISJPJourneyRequest sjpStopEventRequestReplan = new SJPJourneyRequest();

                sjpStopEventRequestReplan.Origin = (SJPLocation)sjpStopEventRequest.Origin.Clone();
                sjpStopEventRequestReplan.Destination = (SJPLocation)sjpStopEventRequest.Destination.Clone();

                sjpStopEventRequestReplan.PlannerMode = sjpStopEventRequest.PlannerMode;
                
                // Only in Ferry mode for the river stop event request
                sjpStopEventRequestReplan.Modes = new List<SJPModeType>() { SJPModeType.Ferry };
                                
                sjpStopEventRequestReplan.OutwardDateTime = sjpStopEventRequest.OutwardDateTime;
                sjpStopEventRequestReplan.OutwardArriveBefore = sjpStopEventRequest.OutwardArriveBefore;
                sjpStopEventRequestReplan.ReturnDateTime = sjpStopEventRequest.ReturnDateTime;
                sjpStopEventRequestReplan.ReturnArriveBefore = sjpStopEventRequest.ReturnArriveBefore;
                sjpStopEventRequestReplan.IsOutwardRequired = sjpStopEventRequest.IsOutwardRequired;
                sjpStopEventRequestReplan.IsReturnRequired = sjpStopEventRequest.IsReturnRequired;
                sjpStopEventRequestReplan.IsReturnOnly = sjpStopEventRequest.IsReturnOnly;
                
                // Add the replan values
                sjpStopEventRequestReplan.IsReplan = true;
                sjpStopEventRequestReplan.ReplanIsOutwardRequired = replanOutwardRequired;
                sjpStopEventRequestReplan.ReplanIsReturnRequired = replanReturnRequired;
                sjpStopEventRequestReplan.ReplanOutwardDateTime = replanOutwardDateTime;
                sjpStopEventRequestReplan.ReplanReturnDateTime = replanReturnDateTime;
                sjpStopEventRequestReplan.ReplanOutwardJourneys = outwardJourneys;
                sjpStopEventRequestReplan.ReplanReturnJourneys = returnJourneys;

                if (!replanOutwardRequired && outwardJourneys.Count != 0)
                {
                    sjpStopEventRequestReplan.ReplanRetainOutwardJourneys = true;
                }
                if (!replanReturnRequired && returnJourneys.Count != 0)
                {
                    sjpStopEventRequestReplan.ReplanRetainReturnJourneys = true;
                }
                
                // All request values have been set, now update the journey request hash.
                // This determines the uniqueness of this journey request for this users session
                sjpStopEventRequestReplan.JourneyRequestHash = sjpStopEventRequestReplan.GetSJPHashCode().ToString();

                return sjpStopEventRequestReplan;
            }

            return null;
        }

        /// <summary>
        /// Returns the total transit time between the Venue pier and Venue
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetTransitTime(SJPLocation venue, string venuePierNaPTAN, bool isToVenue)
        {
            TimeSpan time = new TimeSpan();

            if (venue != null)
            {
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                string venueGateNaPTAN = string.Empty;

                #region Venue pier to/from the venue interchange

                SJPPierVenueNavigationPath navigationPath = locationService.GetSJPVenuePierNavigationPaths(venue.Naptan, venuePierNaPTAN, isToVenue);

                if (navigationPath != null)
                {
                    venueGateNaPTAN = navigationPath.ToNaPTAN;

                    time = time.Add(navigationPath.DefaultDuration);
                }

                #endregion

                #region Venue gate and check constraints

                // Get the venue gate and path details for cycle park
                SJPVenueGate gate = locationService.GetSJPVenueGate(venueGateNaPTAN);
                SJPVenueGateCheckConstraint gateCheckConstraint = locationService.GetSJPVenueGateCheckConstraints(gate, isToVenue);
                SJPVenueGateNavigationPath gateNavigationPath = locationService.GetSJPVenueGateNavigationPaths(venue, gate, isToVenue);

                if (gateCheckConstraint != null)
                {
                    time = time.Add(gateCheckConstraint.AverageDelay);
                }

                if (gateNavigationPath != null)
                {
                    time = time.Add(gateNavigationPath.TransferDuration);
                }

                #endregion
            }

            return time;
        }

        #endregion
    }
}