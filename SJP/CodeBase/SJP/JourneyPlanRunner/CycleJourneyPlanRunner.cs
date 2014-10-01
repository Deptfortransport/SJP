// *********************************************** 
// NAME             : CycleJourneyPlanRunner.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Apr 2011
// DESCRIPTION  	: Validates user input and initiate cycle journey request
// ************************************************
// 

using System.Linq;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.UserPortal.JourneyControl;
using Logger = System.Diagnostics.Trace;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using System.Collections.Generic;
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Validates user input and initiate cycle journey request
    /// </summary>
    public class CycleJourneyPlanRunner : JourneyPlanRunnerBase
    {
        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CycleJourneyPlanRunner()
            : base()
        {
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// ValidateAndRun
        /// </summary>
        /// <param name="journeyRequest">ISJPJourneyRequest</param>
        public override bool ValidateAndRun(ISJPJourneyRequest journeyRequest, string language, bool submitRequest)
        {
            if (journeyRequest == null)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "No journey request provided"));
                throw new SJPException("JourneyRequest object is null", true, SJPExceptionIdentifier.JPRInvalidSJPJourneyRequest);
            }

            #region Validations

            //
            // Perform planner available check
            //
            if (!PerformPlannerAvailableValidation())
            {
                // No need to perform any more validations
                return false;
            }

            //
            // Perform location validations
            //
            PerformLocationValidations(journeyRequest);

            //
            // Perform distance between locations check
            //
            PerformLocationsDistanceValidation(journeyRequest);

            //
            // Perform date validations
            //
            PerformDateValidations(journeyRequest);

            //
            // Perform cycle location is open on date validation
            //
            PerformLocationDateValidations(journeyRequest);

            #endregion

            if (listErrors.Count == 0)
            {
                if (submitRequest)
                {
                    // All input journey parameters were correctly formed so invoke the Cycle Planner Manager
                    InvokeCyclePlannerManager(journeyRequest, language);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        
        #endregion

        #region Validations

        /// <summary>
        /// Checks if the Cycle Planner is available
        /// </summary>
        private bool PerformPlannerAvailableValidation()
        {
            // Ensure cycle planner is available
            if (!Properties.Current["JourneyPlanner.Switch.CyclePlanner.Available"].Parse(true))
            {
                // Cycle planner is not availble. Add error
                SetValidationError(CYCLE_PLANNER_UNAVAILABLE);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks the cycle location is open on the date validation
        /// </summary>
        /// <param name="journeyRequest"></param>
        private void PerformLocationDateValidations(ISJPJourneyRequest journeyRequest)
        {
            SJPLocation fromLocation = journeyRequest.Origin;
            SJPLocation toLocation = journeyRequest.Destination;

            SJPVenueLocation venue = null;

            // Check destination is a venue, and has a max cycle distance value
            if (toLocation is SJPVenueLocation)
            {
                venue = (SJPVenueLocation)toLocation;
            }
            else if (fromLocation is SJPVenueLocation)
            {
                venue = (SJPVenueLocation)fromLocation;
            }

            if (venue != null)
            {
                // Selected cycle park
                string selectedCycleParkId = venue.SelectedSJPParkID;

                // Only validate if a cycle park has been selected
                if (!string.IsNullOrEmpty(selectedCycleParkId))
                {

                    LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                    DateTime outwardDateTime = journeyRequest.OutwardDateTime;
                    DateTime returnDateTime = journeyRequest.ReturnDateTime;

                    // Check if the users journey travel dates should be used in determining validaty of car parks
                    bool validateDate = Properties.Current["CycleJourneyLocations.JouneyDate.Validate.Switch"].Parse(true);
                    if (!validateDate)
                    {
                        outwardDateTime = DateTime.MinValue;
                        returnDateTime = DateTime.MinValue;
                    }

                    // Get cycle parks for the venue and its parent
                    List<string> naptans = new List<string>(venue.Naptan);
                    if (!string.IsNullOrEmpty(venue.Parent))
                    {
                        naptans.Add(venue.Parent);
                    }

                    // Cycle parks
                    List<SJPVenueCyclePark> cycleParkList = locationService.GetSJPVenueCycleParks(naptans, outwardDateTime, returnDateTime);

                    // Check the selected cycle park was returned (if it isn't, then not open for the dates provided)
                    if ((cycleParkList == null)
                        || (cycleParkList.SingleOrDefault(cp => cp.ID == selectedCycleParkId) == null))
                    {
                        List<string> msgArgs = new List<string>();
                        msgArgs.Add(venue.Name);

                        // Not open
                        SetValidationError(DATE_TIME_IS_NOT_VALID_CYCLE_PARK, msgArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Checks the distance between the locations to a configured value
        /// </summary>
        private void PerformLocationsDistanceValidation(ISJPJourneyRequest journeyRequest)
        {
            int maxJourneyDistanceMetres = Properties.Current["JourneyPlanner.Validate.Locations.CycleDistance.Metres"].Parse(20000);
            int maxDistanceKm = Convert.ToInt32(maxJourneyDistanceMetres / 1000);
            
            SJPLocation fromLocation = journeyRequest.Origin;
            SJPLocation toLocation = journeyRequest.Destination;

            // Check overall distance property betweeen from and to only
            if (fromLocation.CycleGridRef.DistanceFrom(toLocation.CycleGridRef) > maxJourneyDistanceMetres)
            {
                List<string> msgArgs = new List<string>();
                msgArgs.Add(maxDistanceKm.ToString());

                SetValidationError(DISTANCE_BETWEEN_LOCATIONS_TOO_GREAT, msgArgs);
            }
            // Check the venue's max cycle distance
            else
            {
                // Check destination is a venue, and has a max cycle distance value
                if (toLocation is SJPVenueLocation)
                {
                    maxDistanceKm = ((SJPVenueLocation)toLocation).CycleToVenueDistance;
                }

                // If origin is also a venue, take the smaller cycle distance
                if (fromLocation is SJPVenueLocation)
                {
                    if (((SJPVenueLocation)fromLocation).CycleToVenueDistance < maxDistanceKm)
                    {
                        maxDistanceKm = ((SJPVenueLocation)fromLocation).CycleToVenueDistance;
                    }
                }

                maxJourneyDistanceMetres = Convert.ToInt32(maxDistanceKm * 1000);

                // Validate locations distance
                if (fromLocation.CycleGridRef.DistanceFrom(toLocation.CycleGridRef) > maxJourneyDistanceMetres)
                {
                    // To location takes preference for the location name to display in the error 
                    // (as its typically a journey to a venue, but could be a journey from a venue)
                    string locationName = (toLocation is SJPVenueLocation) ? toLocation.DisplayName : fromLocation.DisplayName;
                    string msg = (toLocation is SJPVenueLocation) ? DISTANCE_TO_VENUE_LOCATION_TOO_GREAT : DISTANCE_FROM_VENUE_LOCATION_TOO_GREAT;

                    List<string> msgArgs = new List<string>();
                    msgArgs.Add(locationName);
                    msgArgs.Add(maxDistanceKm.ToString());

                    SetValidationError(msg, msgArgs);
                }
            }

        }
        
        #endregion
    }
}
