// *********************************************** 
// NAME             : JourneyRequestHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 13 Apr 2011
// DESCRIPTION  	: JourneyRequestHelper class to provide helper methods for journey requests
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using JC = SJP.UserPortal.JourneyControl;
using Logger = System.Diagnostics.Trace;
using LS = SJP.Common.LocationService;

namespace SJP.Common.Web
{
    /// <summary>
    /// JourneyRequestHelper class to provide helper methods for journey requests
    /// </summary>
    public class JourneyRequestHelper
    {
        #region Private Fields

        private SJPLocation originLocation;
        private SJPLocation destinationLocation;
        private DateTime outwardDateTime;
        private DateTime returnDateTime;
        private bool outwardArriveBy = true; // Outward journeys always arrive by (unless "replanning", see Replan method above)
        private bool returnArriveBy = false; // Return journeys always leave at (unless "replanning", see Replan method above)
        private bool outwardRequired;
        private bool returnRequired;
        private bool returnOnly;
        private SJPJourneyPlannerMode plannerMode;
        private SJPAccessiblePreferences accessiblePreferences;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public JourneyRequestHelper()
        {
        }

        #endregion

        #region Public Methods

        #region Build new request

        /// <summary>
        /// Populates an SJPJourneyRequest object using parameters provided,
        /// overloaded to specify an outward and return arrive by flag
        /// </summary>
        public ISJPJourneyRequest BuildSJPJourneyRequest(SJPLocation originLocation,
            SJPLocation destinationLocation,
            DateTime outwardDateTime,
            DateTime returnDateTime,
            bool outwardArriveBy,
            bool returnArriveBy,
            bool outwardRequired,
            bool returnRequired,
            bool returnOnly,
            SJPJourneyPlannerMode plannerMode,
            SJPAccessiblePreferences accessiblePreferences)
        {
            this.originLocation = originLocation;
            this.destinationLocation = destinationLocation;
            this.outwardDateTime = outwardDateTime;
            this.returnDateTime = returnDateTime;
            this.outwardArriveBy = outwardArriveBy;
            this.returnArriveBy = returnArriveBy;
            this.outwardRequired = outwardRequired;
            this.returnRequired = returnRequired;
            this.returnOnly = returnOnly;   // Treat the origin location as the ReturnOrigin, and destination location as the ReturnDestination (assumes no outward journey is required)
            this.plannerMode = plannerMode;
            this.accessiblePreferences = accessiblePreferences;

            ISJPJourneyRequest sjpJourneyRequest = BuildSJPRequest();

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Populates an SJPJourneyRequest object using parameters provided
        /// </summary>
        public ISJPJourneyRequest BuildSJPJourneyRequest(SJPLocation originLocation,
            SJPLocation destinationLocation,
            DateTime outwardDateTime,
            DateTime returnDateTime,
            bool outwardRequired,
            bool returnRequired,
            bool returnOnly,
            SJPJourneyPlannerMode plannerMode,
            SJPAccessiblePreferences accessiblePreferences)
        {
            return this.BuildSJPJourneyRequest(
                originLocation,
                destinationLocation,
                outwardDateTime,
                returnDateTime,
                outwardArriveBy,
                returnArriveBy,
                outwardRequired,
                returnRequired,
                returnOnly,
                plannerMode,
                accessiblePreferences);
        }

        /// <summary>
        /// Populates an SJPJourneyRequest object using parameters provided. 
        /// Each string parameter is parsed into the correct type. 
        /// If any parameter fails parsing, then an SJPException is thrown or a null object is returned
        /// </summary>
        public ISJPJourneyRequest BuildSJPJourneyRequest(
            string originId,
            string originType,
            string originName,
            string destinationId,
            string destinationType,
            string destinationName,
            DateTime outwardDateTime,
            DateTime returnDateTime,
            bool outwardRequired,
            bool returnRequired,
            bool returnOnly,
            string accessibleOption,
            bool fewerInterchanges,
            string plannerMode)
        {
            try
            {
                LocationHelper locationHelper = new LocationHelper();

                SJPLocationType originLocationType = SJPLocationType.Unknown;
                SJPLocationType destinationLocationType = SJPLocationType.Unknown;
                                
                // Origin location
                originLocationType = (SJPLocationType)Enum.Parse(typeof(SJPLocationType), originType, true);
                this.originLocation = locationHelper.GetSJPLocation(originId, originName, originLocationType);

                // Destination location
                destinationLocationType = (SJPLocationType)Enum.Parse(typeof(SJPLocationType), destinationType, true);
                this.destinationLocation = locationHelper.GetSJPLocation(destinationId, destinationName, destinationLocationType);
                
                // Outward date - No need to validate here as the Date control, or the JourneyPlanRunner will check
                this.outwardDateTime = outwardDateTime;

                // Return date - No need to validate here as the Date control, or the JourneyPlanRunner will check
                this.returnDateTime = returnDateTime;

                // Outward/Return required
                this.outwardRequired = outwardRequired;
                this.returnRequired = returnRequired;
                this.returnOnly = returnOnly;   // Treat the origin location as the ReturnOrigin, and destination location as the ReturnDestination (assumes no outward journey is required)

                // Planner mode
                this.plannerMode = (SJPJourneyPlannerMode)Enum.Parse(typeof(SJPJourneyPlannerMode), plannerMode, true);
                
                // Accessible preferences
                this.accessiblePreferences = new SJPAccessiblePreferences();
                this.accessiblePreferences.PopulateAccessiblePreference(accessibleOption);
                this.accessiblePreferences.RequireFewerInterchanges = fewerInterchanges;
                
                // For SJPMobile, outward only journey From venue scenario
                if (outwardRequired && !returnRequired && !returnOnly)
                {
                    if ((originLocation != null && originLocation is SJPVenueLocation)
                        && (destinationLocation != null && !(destinationLocation is SJPVenueLocation)))
                    {
                        this.outwardArriveBy = false;
                    }
                }

                // Build the SJPJourneyRequest
                ISJPJourneyRequest sjpJourneyRequest = BuildSJPRequest();

                return sjpJourneyRequest;
            }
            catch (Exception ex)
            {
                throw new SJPException(
                    string.Format("Error building an SJPJourneyRequest from string values: {0}", ex.Message), 
                    false, SJPExceptionIdentifier.SWErrorBuildingJourneyRequestFromString, ex);
            }
        }

        #endregion

        #region Build replan request

        /// <summary>
        /// Populates an SJPJourneyRequest object using parameters provided
        /// </summary>
        public ISJPJourneyRequest BuildSJPJourneyRequestForReplan(ISJPJourneyRequest sjpJourneyRequest,
            bool replanOutwardRequired,
            bool replanReturnRequired,
            DateTime replanOutwardDateTime,
            DateTime replanReturnDateTime,
            bool replanOutwardArriveBefore,
            bool replanReturnArriveBefore,
            List<Journey> outwardJourneys, 
            List<Journey> returnJourneys,
            bool retainOutwardJourneys,
            bool retainReturnJourneys,
            bool retainOutwardJourneysWhenNoResults,
            bool retainReturnJourneysWhenNoResults)
        {
            this.originLocation = (SJPLocation)sjpJourneyRequest.Origin.Clone();
            this.destinationLocation = (SJPLocation)sjpJourneyRequest.Destination.Clone();
            this.outwardDateTime = sjpJourneyRequest.OutwardDateTime;
            this.returnDateTime = sjpJourneyRequest.ReturnDateTime;
            this.outwardRequired = sjpJourneyRequest.IsOutwardRequired;
            this.returnRequired = sjpJourneyRequest.IsReturnRequired;
            this.returnOnly = sjpJourneyRequest.IsReturnOnly;
            this.plannerMode = sjpJourneyRequest.PlannerMode;
            this.accessiblePreferences = sjpJourneyRequest.AccessiblePreferences.Clone();

            // Create the replan request based on the original
            ISJPJourneyRequest sjpJourneyRequestReplan = BuildSJPRequest();

            // Add the replan values
            sjpJourneyRequestReplan.IsReplan = true;
            sjpJourneyRequestReplan.ReplanIsOutwardRequired = replanOutwardRequired;
            sjpJourneyRequestReplan.ReplanIsReturnRequired = replanReturnRequired;
            sjpJourneyRequestReplan.ReplanOutwardDateTime = replanOutwardDateTime;
            sjpJourneyRequestReplan.ReplanReturnDateTime = replanReturnDateTime;
            sjpJourneyRequestReplan.ReplanOutwardArriveBefore = replanOutwardArriveBefore;
            sjpJourneyRequestReplan.ReplanReturnArriveBefore = replanReturnArriveBefore;
            sjpJourneyRequestReplan.ReplanOutwardJourneys = outwardJourneys;
            sjpJourneyRequestReplan.ReplanReturnJourneys = returnJourneys;
            sjpJourneyRequestReplan.ReplanRetainOutwardJourneys = retainOutwardJourneys;
            sjpJourneyRequestReplan.ReplanRetainReturnJourneys = retainReturnJourneys;
            sjpJourneyRequestReplan.ReplanRetainOutwardJourneysWhenNoResults = retainOutwardJourneysWhenNoResults;
            sjpJourneyRequestReplan.ReplanRetainReturnJourneysWhenNoResults = retainReturnJourneysWhenNoResults;
            
            // Update the journey request hash because replan values have been added
            sjpJourneyRequestReplan.JourneyRequestHash = sjpJourneyRequestReplan.GetSJPHashCode().ToString();

            return sjpJourneyRequestReplan;
        }

        #endregion

        #region Update existing request

        /// <summary>
        /// Updates an SJPJourneyRequest object with the planner mode parameter provided
        /// </summary>
        /// <param name="sjpJourneyRequest">Request to update</param>
        public ISJPJourneyRequest UpdateSJPJourneyRequestPlannerMode(ISJPJourneyRequest sjpJourneyRequest, SJPJourneyPlannerMode plannerMode)
        {
            if (sjpJourneyRequest != null)
            {
                sjpJourneyRequest.PlannerMode = plannerMode;

                // Update the journey request hash because the planner mode has changed.
                sjpJourneyRequest.JourneyRequestHash = sjpJourneyRequest.GetSJPHashCode().ToString();
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Updates an SJPJourneyRequest object with the accessible options provided
        /// </summary>
        /// <param name="sjpJourneyRequest">Request to update</param>
        public ISJPJourneyRequest UpdateSJPJourneyRequestAccessiblePreferences(ISJPJourneyRequest sjpJourneyRequest, SJPAccessiblePreferences accessiblePreferences)
        {
            if (sjpJourneyRequest != null)
            {
                // Update accessible preference in the existing request
                sjpJourneyRequest.AccessiblePreferences = accessiblePreferences;

                // And update the request properties which are dependent on the accessible preferences
                UpdateAccessibilePreferences((SJPJourneyRequest)sjpJourneyRequest, accessiblePreferences);

                UpdateAccessiblePublicParameters((SJPJourneyRequest)sjpJourneyRequest);

                // Update the journey request hash because the planner mode has changed.
                sjpJourneyRequest.JourneyRequestHash = sjpJourneyRequest.GetSJPHashCode().ToString();
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Updates an SJPJourneyRequest object using the Additional Venue parameters provided.
        /// The destination location is updated (if a venue), otherwise the origin (if a venue)
        /// </summary>
        /// <param name="sjpJourneyRequest">Request to update</param>
        public ISJPJourneyRequest UpdateSJPJourneyRequestVenue(ISJPJourneyRequest sjpJourneyRequest,
            SJPPark sjpPark, DateTime outwardDateTime, DateTime returnDateTime)
        {
            SJPVenueLocation venueLocation = null;
            bool isOriginLocation = false;

            if (sjpJourneyRequest.Destination is SJPVenueLocation)
            {
                venueLocation = sjpJourneyRequest.Destination as SJPVenueLocation;
                isOriginLocation = false;
            }
            else if (sjpJourneyRequest.Origin is SJPVenueLocation)
            {
                venueLocation = sjpJourneyRequest.Origin as SJPVenueLocation;
                isOriginLocation = true;
            }
            
            if (venueLocation != null)
            {
                // Set the park Id for the journey planner to use
                venueLocation.SelectedSJPParkID = sjpPark.ID;

                // Set the datetimes to use instead of the input page journey request datetimes
                venueLocation.SelectedOutwardDateTime = outwardDateTime;
                venueLocation.SelectedReturnDateTime = returnDateTime;

                // Clear any previous venue accessible details
                venueLocation.AccessibleNaptans = new List<string>();
                venueLocation.SelectedName = string.Empty;

                // Update request
                if (isOriginLocation)
                {
                    sjpJourneyRequest.Origin = venueLocation;
                }
                else
                {
                    sjpJourneyRequest.Destination = venueLocation;
                }

                // Update the journey request hash because the location has changed.
                sjpJourneyRequest.JourneyRequestHash = sjpJourneyRequest.GetSJPHashCode().ToString();
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Updates an SJPJourneyRequest object to use the specified Cycle route type penalty algorithm
        /// </summary>
        /// <param name="sjpJourneyRequest">Request to update</param>
        /// <param name="penaltyFunctionAlgortihm">Penalty function algorithm</param>
        /// <returns></returns>
        public ISJPJourneyRequest UpdateSJPJourneyRequestCycle(ISJPJourneyRequest sjpJourneyRequest,
            string penaltyFunctionAlgortihm)
        {
            if (!string.IsNullOrEmpty(penaltyFunctionAlgortihm))
            {
                // Set the correct cycle algorithm to use
                sjpJourneyRequest.CycleAlgorithm = penaltyFunctionAlgortihm;
                sjpJourneyRequest.PenaltyFunction = GetCycleAlgorithm(penaltyFunctionAlgortihm);

                // Clear any previous accessible preferences
                sjpJourneyRequest.AccessiblePreferences = new SJPAccessiblePreferences();

                // Update the journey request hash because the penalty function has changed.
                sjpJourneyRequest.JourneyRequestHash = sjpJourneyRequest.GetSJPHashCode().ToString();
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Updates an SJPJourneyRequest object using the Additional Venue parameters provided for river services
        /// </summary>
        public ISJPJourneyRequest UpdateSJPJourneyRequestRiverServices(ISJPJourneyRequest sjpJourneyRequest,
            Journey outwardStopEventJourney, Journey returnStopEventJourney, DateTime outwardDateTime, DateTime returnDateTime)
        {
            SJPVenueLocation venueLocation = null;
            SJPVenueLocation returnVenueLocation = null;

            bool updateRequest = false;

            #region Outward venue location

            if (outwardStopEventJourney != null)
            {
                venueLocation = sjpJourneyRequest.Destination as SJPVenueLocation;
                
                if (venueLocation != null)
                {
                    if (outwardStopEventJourney.JourneyLegs.Count > 0)
                    {
                        // Set the Pier NaPTAN and name for the journey planner to use
                        JourneyCallingPoint venuePier = outwardStopEventJourney.JourneyLegs[0].LegStart;
                        if (venuePier != null && venuePier.Location != null)
                        {
                            venueLocation.SelectedPierNaptan = venuePier.Location.Naptan.FirstOrDefault();
                            venueLocation.SelectedName = venuePier.Location.DisplayName;
                            venueLocation.SelectedGridReference = venuePier.Location.GridRef;
                        }
                    }

                    // Set the datetimes to use instead of the input page journey request datetimes
                    venueLocation.SelectedOutwardDateTime = outwardDateTime;
                    venueLocation.SelectedReturnDateTime = returnDateTime;

                    // Clear any previous venue accessible details
                    venueLocation.AccessibleNaptans =  new List<string>();
                    
                    // Update request
                    sjpJourneyRequest.Destination = venueLocation;
                    
                    // Add the stop event journey, it's legs will be appended to the journey planned
                    sjpJourneyRequest.OutwardJourneyPart = outwardStopEventJourney;

                    updateRequest = true;
                }
            }

            #endregion

            #region Return venue location

            if (returnStopEventJourney != null)
            {
                returnVenueLocation = sjpJourneyRequest.ReturnOrigin as SJPVenueLocation;
                
                if (returnVenueLocation != null)
                {
                    if (returnStopEventJourney.JourneyLegs.Count > 0)
                    {
                        // Set the Pier NaPTAN for the journey planner to use
                        JourneyCallingPoint venuePier = returnStopEventJourney.JourneyLegs[0].LegEnd;
                        if (venuePier != null && venuePier.Location != null)
                        {
                            returnVenueLocation.SelectedPierNaptan = venuePier.Location.Naptan.FirstOrDefault();
                            returnVenueLocation.SelectedName = venuePier.Location.DisplayName;
                            returnVenueLocation.SelectedGridReference = venuePier.Location.GridRef;
                        }
                    }

                    // Set the datetimes to use instead of the input page journey request datetimes
                    returnVenueLocation.SelectedOutwardDateTime = outwardDateTime;
                    returnVenueLocation.SelectedReturnDateTime = returnDateTime;

                    // Clear any previous venue accessible details
                    returnVenueLocation.AccessibleNaptans = new List<string>();

                    // Update request
                    sjpJourneyRequest.ReturnOrigin = returnVenueLocation;
                    
                    // Add the stop event journey, it's legs will be appended to the journey planned
                    sjpJourneyRequest.ReturnJourneyPart = returnStopEventJourney;

                    updateRequest = true;
                }
            }

            #endregion

            if (updateRequest)
            {
                IPropertyProvider pp = Properties.Current;

                // Update the number of PT journeys required
                sjpJourneyRequest.Sequence = pp[JC.Keys.JourneyRequest_Sequence_RiverServicePlannerMode].Parse(3);

                // Travel demand should be turned off for this journey request to pier
                sjpJourneyRequest.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanOff];
                sjpJourneyRequest.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanOff];

                // Clear any previous accessible preferences
                sjpJourneyRequest.AccessiblePreferences = new SJPAccessiblePreferences();

                // Update the journey request hash because the location has changed.
                sjpJourneyRequest.JourneyRequestHash = sjpJourneyRequest.GetSJPHashCode().ToString();
            }
            
            return sjpJourneyRequest;
        }


        /// <summary>
        /// Updates an SJPJourneyRequest object origin location using the user selected accessible GNAT stop location
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        /// <param name="gnatStop"></param>
        /// <returns></returns>
        public ISJPJourneyRequest UpdateSJPJourneyRequestForAccessiblePublicTransport(ISJPJourneyRequest sjpJourneyRequest, SJPGNATLocation gnatStop)
        {
            if (gnatStop != null)
            {
                LS.LocationService locationService = SJPServiceDiscovery.Current.Get<LS.LocationService>(ServiceDiscoveryKey.LocationService);
                                
                // Get fully populated SJPLocation from cache for the selected GNAT location
                SJPLocation location = locationService.GetSJPLocation(gnatStop.Naptan[0].ToString(), SJPLocationType.Station);

                // Update the appropriate location (assume its origin)
                if (sjpJourneyRequest.Origin.TypeOfLocation != SJPLocationType.Venue)
                    sjpJourneyRequest.Origin = location;
                else
                    sjpJourneyRequest.Destination = location;
                
                // Update the dont force coach rule because origin/destination has changed
                UpdateDontForceCoach((SJPJourneyRequest)sjpJourneyRequest);

                // Update the journey request hash because the origin/destination has changed.
                sjpJourneyRequest.JourneyRequestHash = sjpJourneyRequest.GetSJPHashCode().ToString();
            }

            return sjpJourneyRequest;
        }


        /// <summary>
        /// Updates an SJPJourneyRequest object origin or destination location 
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        /// <param name="location">the new location</param>
        /// <param name="updateOrigin">true for origin, false for destination</param>
        /// <returns></returns>
        public ISJPJourneyRequest UpdateSJPJourneyRequestOriginOrDestination(ISJPJourneyRequest sjpJourneyRequest, SJPLocation location, bool updateOrigin)
        {
            if (location != null)
            {
                if (updateOrigin)
                {
                    sjpJourneyRequest.Origin = location;
                }
                else
                {
                    sjpJourneyRequest.Destination = location;
                }

                // Update the journey request hash because the origin/destination has changed.
                sjpJourneyRequest.JourneyRequestHash = sjpJourneyRequest.GetSJPHashCode().ToString();
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Updates an SJPJourneyRequest object with the replan journey request hash
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        /// <param name="isEarlier"></param>
        /// <param name="replanJourneyRequestHash"></param>
        /// <returns></returns>
        public ISJPJourneyRequest UpdateSJPJourneyRequestEarlierLater(ISJPJourneyRequest sjpJourneyRequest, bool isEarlier, string replanJourneyRequestHash)
        {
            if (sjpJourneyRequest != null)
            {
                // Update the replan journey request hash for this request
                if (isEarlier)
                {
                    sjpJourneyRequest.ReplanJourneyRequestHashEarlier = replanJourneyRequestHash;
                }
                else
                {
                    sjpJourneyRequest.ReplanJourneyRequestHashLater = replanJourneyRequestHash;
                }

                // Do not update the journey request hash code as the replan journey request hash
                // has no bearing on this journey request parameters, it is only a pointer to the 
                // replan journey request
            }

            return sjpJourneyRequest;
        }
        
        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Constructs an SJP journey request
        /// </summary>
        private ISJPJourneyRequest BuildSJPRequest()
        {
            SJPJourneyRequest request = new SJPJourneyRequest();

            #region User entered parameters

            // Locations
            request.Origin = originLocation;
            request.Destination = destinationLocation;

            // Ensures the SJP hash code is different when the only difference is origin and destination
            // has been swapped around
            request.LocationInputMode = GetLocationInputMode();

            // Date/times
            request.OutwardDateTime = outwardDateTime;
            request.OutwardArriveBefore = outwardArriveBy; // Outward journeys always arrive by (unless "replanning", see Replan method above)
            
            request.ReturnDateTime = returnDateTime;
            request.ReturnArriveBefore = returnArriveBy; // Return journeys always leave at (unless "replanning", see Replan method above)

            request.IsOutwardRequired = outwardRequired;
            request.IsReturnRequired = returnRequired;
            request.IsReturnOnly = returnOnly; // Treat the origin location as the ReturnOrigin, and destination location as the ReturnDestination (assumes no outward journey is required)
                        
            // Modes
            request.PlannerMode = plannerMode;
            request.Modes = PopulateModes(plannerMode);

            #region Accessible preferences

            UpdateAccessibilePreferences(request, accessiblePreferences);

            #endregion

            #region Dont force coach

            UpdateDontForceCoach(request);

            #endregion
            
            #endregion

            #region Common parameters from properties

            // Populate request parameters from the properties service.
            // Populate values for all journey request types regardless of requested type. 
            // The journey planner managers will use only those values it needs

            IPropertyProvider pp = Properties.Current;

            #region Public

            // Public specific
            request.PublicAlgorithm = GetPublicAlgorithm(pp[JC.Keys.JourneyRequest_AlgorithmPublic]);

            request.Sequence = pp[JC.Keys.JourneyRequest_Sequence].Parse(3);
            request.InterchangeSpeed = pp[JC.Keys.JourneyRequest_InterchangeSpeed].Parse(0);
            request.WalkingSpeed = pp[JC.Keys.JourneyRequest_WalkingSpeed].Parse(80);
            request.MaxWalkingTime = pp[JC.Keys.JourneyRequest_MaxWalkingTime].Parse(30);
            request.RoutingGuideInfluenced = pp[JC.Keys.JourneyRequest_RoutingGuideInfluenced].Parse(false);
            request.RoutingGuideCompliantJourneysOnly = pp[JC.Keys.JourneyRequest_RoutingGuideCompliantJourneysOnly].Parse(false);
            request.RouteCodes = pp[JC.Keys.JourneyRequest_RouteCodes];
            request.OlympicRequest = pp[JC.Keys.JourneyRequest_OlympicRequest].Parse(true);
            request.RemoveAwkwardOvernight = pp[JC.Keys.JourneyRequest_RemoveAwkwardOvernight].Parse(false);
            
           
            // TDM rules
            if (pp[JC.Keys.JourneyRequest_TravelDemandPlanSwitch].Parse(true))
            {
                // Handle venue to non-venue for an outward journey request
                if ((request.IsOutwardRequired && !request.IsReturnRequired && !request.IsReturnOnly)
                    && ((request.Origin != null) && (request.Origin is SJPVenueLocation)
                         && (request.Destination != null) && !(request.Destination is SJPVenueLocation))
                   )
                {
                    // Then this a leaving venue, so use TDM for return journey
                    request.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanReturn];
                    request.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanReturn];
                }
                else
                {
                    request.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanOutward];
                    request.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanReturn];
                }
            }
            else
            {
                request.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanOff];
                request.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanOff];
            }

            // Override for accessible journeys
            UpdateAccessiblePublicParameters(request);

            #endregion

            #region Car

            // Car specific
            request.PrivateAlgorithm = GetPrivateAlgorithm(pp[JC.Keys.JourneyRequest_AlgorithmPrivate]);

            request.AvoidMotorways = pp[JC.Keys.JourneyRequest_AvoidMotorways].Parse(false);
            request.AvoidFerries = pp[JC.Keys.JourneyRequest_AvoidFerries].Parse(false);
            request.AvoidTolls = pp[JC.Keys.JourneyRequest_AvoidTolls].Parse(false);
            request.AvoidRoads = new List<string>();
            request.IncludeRoads = new List<string>();
            request.DrivingSpeed = pp[JC.Keys.JourneyRequest_DrivingSpeed].Parse(112);
            request.DoNotUseMotorways = pp[JC.Keys.JourneyRequest_DoNotUseMotorways].Parse(false);
            request.FuelConsumption = pp[JC.Keys.JourneyRequest_FuelConsumption];
            request.FuelPrice = pp[JC.Keys.JourneyRequest_FuelPrice];

            #endregion

            #region Cycle

            // Cycle specific

            #region Penalty function

            request.CycleAlgorithm = string.Empty;
            request.PenaltyFunction = GetCycleAlgorithm(string.Empty);

            #endregion

            #region User preferences

            // The ID of each user preference must match the IDs specified in the cycle planner configuration file.
            List<SJPUserPreference> userPreferences = new List<SJPUserPreference>();

            SJPUserPreference sjpUserPreference = new SJPUserPreference();
            
            // A property that denotes the size of the array of user preferences expected by the Atkins CTP
            int numOfProperties = Convert.ToInt32(pp[JC.Keys.JourneyRequest_UserPreferences_Count]);

            // Build the actual array of user preferences from properties
            // these are used in the request sent to the Atkins CTP.
            for (int i = 0; i < numOfProperties; i++)
            {
                // Override any preferences by User entered/chosen values
                switch (i)
                {
                    //case 5: // Max Speed
                    //    break;
                    //case 6:  // Avoid Time Based Restrictions
                    //    break;
                    //case 12: // Avoid Steep Climbs
                    //    break;
                    //case 13: // Avoid Unlit Roads
                    //    break;
                    //case 14: // Avoid Walking your bike
                    //    break;
                    default:
                        sjpUserPreference = new SJPUserPreference(i.ToString(),
                            pp[string.Format(JC.Keys.JourneyRequest_UserPreferences_Index, i.ToString())]);
                        break;
                }
                userPreferences.Add(sjpUserPreference);
            }

            request.UserPreferences = userPreferences;

            #endregion

            #endregion

            #endregion
            
            // All request values have been set, now update the journey request hash.
            // This determines the uniqueness of this journey request for this users session
            request.JourneyRequestHash = request.GetSJPHashCode().ToString();

            return request;
        }

        /// <summary>
        /// Used the SJPJourneyPlannerMode to return an SJPModeTypes array corresponding to that mode 
        /// </summary>
        private List<SJPModeType> PopulateModes(SJPJourneyPlannerMode plannerMode)
        {
            List<SJPModeType> modes = new List<SJPModeType>();

            switch (plannerMode)
            {
                case SJPJourneyPlannerMode.Cycle: // Cycle only journey
                    modes.Add(SJPModeType.Cycle);
                    break;
                case SJPJourneyPlannerMode.BlueBadge: // Car only journey
                    modes.Add(SJPModeType.Car);
                    break;
                case SJPJourneyPlannerMode.ParkAndRide: // Car only journey
                    modes.Add(SJPModeType.Car);
                    break;
                case SJPJourneyPlannerMode.RiverServices: // River PT journey
                    modes.Add(SJPModeType.Air);
                    modes.Add(SJPModeType.Bus);
                    modes.Add(SJPModeType.Coach);
                    modes.Add(SJPModeType.Ferry);
                    modes.Add(SJPModeType.Metro);
                    modes.Add(SJPModeType.Rail);
                    modes.Add(SJPModeType.Tram);
                    modes.Add(SJPModeType.Underground);
                    break;
                case SJPJourneyPlannerMode.PublicTransport: // Public Transport journey
                default: 
                    modes.Add(SJPModeType.Air);
                    modes.Add(SJPModeType.Bus);
                    modes.Add(SJPModeType.Coach);
                    modes.Add(SJPModeType.Ferry);
                    modes.Add(SJPModeType.Metro);
                    modes.Add(SJPModeType.Rail);
                    modes.Add(SJPModeType.Tram);

                    // Do not include underground if accessible preference underground flag set
                    if ((accessiblePreferences == null) || (!accessiblePreferences.DoNotUseUnderground))
                    {
                        modes.Add(SJPModeType.Underground);
                    }
                    break;
            }

            return modes;
        }

        /// <summary>
        /// Converts a string into a SJPPublicAlgorithmType. If unable to parse, Default is returned 
        /// and a warning logged
        /// </summary>
        private SJPPublicAlgorithmType GetPublicAlgorithm(string algorithm)
        {
            SJPPublicAlgorithmType algorithmType = SJPPublicAlgorithmType.Default;
            try
            {
                algorithmType = (SJPPublicAlgorithmType)Enum.Parse(typeof(SJPPublicAlgorithmType), algorithm, true);
            }
            catch
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                    string.Format("Failed to parse algorithm string[{0}] into an SJPPublicAlgorithmType, check property[{1}] contains a valid value for this algorithm type.",
                                   algorithm,
                                   JC.Keys.JourneyRequest_AlgorithmPublic)));
            }

            return algorithmType;
        }

        /// <summary>
        /// Converts a string into a SJPPrivateAlgorithmType. If unable to parse, Fastest is returned 
        /// and a warning logged
        /// </summary>
        private SJPPrivateAlgorithmType GetPrivateAlgorithm(string algorithm)
        {
            SJPPrivateAlgorithmType algorithmType = SJPPrivateAlgorithmType.Fastest;
            try
            {
                algorithmType = (SJPPrivateAlgorithmType)Enum.Parse(typeof(SJPPrivateAlgorithmType), algorithm, true);
            }
            catch
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                    string.Format("Failed to parse algorithm string[{0}] into an SJPPrivateAlgorithmType, check property[{1}] contains a valid value for this algorithm type.",
                                   algorithm,
                                   JC.Keys.JourneyRequest_AlgorithmPrivate)));
            }

            return algorithmType;
        }

        /// <summary>
        /// Uses the provided cycle penalty function algorithm to build up a penalty function algorithm
        /// to use for cycle planning. Values are read from properties for the specified algorithm
        /// </summary>
        /// <param name="algorithm">Algorithm name corresponding to properties value, empty will use default algorithm</param>
        /// <returns></returns>
        private string GetCycleAlgorithm(string algorithm)
        {
            IPropertyProvider pp = Properties.Current;

            // penalty function must be formatted as 
            // "Call <location of penalty function assembly file>,<penalty function type name>"
            // e.g. "Call C:\CyclePlannerService\Services\RoadInterfaceHostingService\atk.cp.PenaltyFunctions.dll,
            // AtkinsGlobal.JourneyPlanning.PenaltyFunctions.Fastest"

            string algorithmToUse = algorithm;

            // Construct penalty function using the properties
            if (string.IsNullOrEmpty(algorithmToUse))
            {
                algorithmToUse = pp[JC.Keys.JourneyRequest_PenaltyFunction_Algorithm];
            }

            string dllPath = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLLPath, algorithmToUse)];
            string dll = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLL, algorithmToUse)];
            string prefix = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Prefix, algorithmToUse)];
            string suffix = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Suffix, algorithmToUse)];

            #region Validate

            // Validate penalty function values
            if (string.IsNullOrEmpty(dllPath) ||
                string.IsNullOrEmpty(dll) ||
                string.IsNullOrEmpty(prefix) ||
                string.IsNullOrEmpty(suffix))
            {
                throw new SJPException(
                    string.Format("Cycle planner penalty function property values for algorithm[{0}] were missing or invalid, check properties[{1}, {2}, {3}, and {4}] are available.",
                        algorithmToUse,
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLLPath, algorithmToUse),
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLL, algorithmToUse),
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Prefix, algorithmToUse),
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Suffix, algorithmToUse)),
                    false,
                    SJPExceptionIdentifier.PSMissingProperty);
            }

            if (!dllPath.EndsWith("\\"))
            {
                dllPath = dllPath + "\\";
            }

            if (!prefix.EndsWith("."))
            {
                prefix = prefix + ".";
            }

            #endregion

            string penaltyFunction = string.Format("Call {0}{1}, {2}{3}", dllPath, dll, prefix, suffix);

            return penaltyFunction;
        }

        /// <summary>
        /// Returns the current LocationInputMode based on the state of the From and To location
        /// </summary>
        /// <returns></returns>
        private string GetLocationInputMode()
        {
            if ((originLocation != null) && (destinationLocation != null))
            {
                LocationInputMode locationInputMode = LocationInputMode.ToVenue;

                if (originLocation.TypeOfLocation == SJPLocationType.Venue
                    && destinationLocation.TypeOfLocation == SJPLocationType.Venue)
                {
                    locationInputMode = LocationInputMode.VenueToVenue;
                }
                else if (originLocation.TypeOfLocation == SJPLocationType.Venue
                        && destinationLocation.TypeOfLocation != SJPLocationType.Venue)
                {
                    locationInputMode = LocationInputMode.FromVenue;
                }

                return locationInputMode.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Updates the accessible options based on the accessible preferences provided
        /// </summary>
        private void UpdateAccessibilePreferences(SJPJourneyRequest request, SJPAccessiblePreferences accessiblePreferences)
        {
            // Accessible preferences
            request.AccessiblePreferences = accessiblePreferences;
            request.FilteringStrict = accessiblePreferences.Accessible;

            // Set the venue accessible naptans
            if (accessiblePreferences.Accessible)
            {
                SJPVenueLocation originVenueLocation = null;
                SJPVenueLocation destinationVenueLocation = null;

                if (request.Origin != null && request.Origin is SJPVenueLocation)
                {
                    originVenueLocation = request.Origin as SJPVenueLocation;
                }

                if (request.Destination != null && request.Destination is SJPVenueLocation)
                {
                    destinationVenueLocation = request.Destination as SJPVenueLocation;
                }

                // Set the accessible naptans
                UpdateAccessibleLocations(originVenueLocation, destinationVenueLocation);

                // Assign updated locations back to request
                if (originVenueLocation != null)
                {
                    request.Origin = originVenueLocation;
                }

                if (destinationVenueLocation != null)
                {
                    request.Destination = destinationVenueLocation;
                }
            }
        }

        /// <summary>
        /// Updates the accessible naptans in the venue location for the naptans specified, valid for a datetime. List can be empty
        /// </summary>
        /// <returns></returns>
        private void UpdateAccessibleLocations(SJPVenueLocation originVenueLocation, SJPVenueLocation destinationVenueLocation)
        {
            LS.LocationService locationService = SJPServiceDiscovery.Current.Get<LS.LocationService>(ServiceDiscoveryKey.LocationService);

            #region Origin location

            // Origin location (in case it is a venue to venue accessible journey
            if (originVenueLocation != null)
            {
                List<SJPVenueAccess> venueAccessList = locationService.GetSJPVenueAccessData(originVenueLocation.Naptan, outwardDateTime);

                List<string> accessibleNaptans = new List<string>();
                string accessibleName = string.Empty;

                if ((venueAccessList != null) && (venueAccessList.Count > 0))
                {
                    foreach (SJPVenueAccess va in venueAccessList)
                    {
                        if (va.Stations != null)
                        {
                            foreach (SJPVenueAccessStation vas in va.Stations)
                            {
                                if (!accessibleNaptans.Contains(vas.StationNaPTAN))
                                {
                                    // Set station name (use first one found)
                                    if (string.IsNullOrEmpty(accessibleName))
                                    {
                                        accessibleName = vas.StationName;
                                    }

                                    // Add to the list of station accessible naptans to use
                                    accessibleNaptans.Add(vas.StationNaPTAN);
                                }
                            }
                        }
                    }
                }

                originVenueLocation.AccessibleNaptans = accessibleNaptans;
                originVenueLocation.SelectedName = accessibleName;
            }

            #endregion

            #region Destination location

            if (destinationVenueLocation != null)
            {
                List<SJPVenueAccess> venueAccessList = locationService.GetSJPVenueAccessData(destinationVenueLocation.Naptan, outwardDateTime);

                List<string> accessibleNaptans = new List<string>();
                string accessibleName = string.Empty;

                if ((venueAccessList != null) && (venueAccessList.Count > 0))
                {
                    foreach (SJPVenueAccess va in venueAccessList)
                    {
                        if (va.Stations != null)
                        {
                            foreach (SJPVenueAccessStation vas in va.Stations)
                            {
                                if (!accessibleNaptans.Contains(vas.StationNaPTAN))
                                {
                                    // Set station name (use first one found)
                                    if (string.IsNullOrEmpty(accessibleName))
                                    {
                                        accessibleName = vas.StationName;
                                    }
                                    
                                    // Add to the list of station accessible naptans to use
                                    accessibleNaptans.Add(vas.StationNaPTAN);
                                }
                            }
                        }
                    }

                    // Adjust the date time to take into account the transfer time
                    if ((outwardDateTime != DateTime.MinValue) && (outwardDateTime != DateTime.MaxValue))
                    {
                        destinationVenueLocation.SelectedOutwardDateTime = outwardDateTime.Subtract(venueAccessList[0].AccessToVenueDuration);
                    }
                    if ((returnDateTime != DateTime.MinValue) && (returnDateTime != DateTime.MaxValue))
                    {
                        destinationVenueLocation.SelectedReturnDateTime = returnDateTime.Add(venueAccessList[0].AccessToVenueDuration);
                    }
                }

                destinationVenueLocation.AccessibleNaptans = accessibleNaptans;
                destinationVenueLocation.SelectedName = accessibleName;
            }

            #endregion
        }

        /// <summary>
        /// Updates the public journey planning parameters based on the accessible preferences
        /// </summary>
        /// <param name="request"></param>
        private void UpdateAccessiblePublicParameters(SJPJourneyRequest request)
        {
            IPropertyProvider pp = Properties.Current;

            // Only update is accessible journey required
            if (request.AccessiblePreferences.Accessible)
            {
                // Walk speed, distance
                if (request.AccessiblePreferences.RequireSpecialAssistance && request.AccessiblePreferences.RequireStepFreeAccess)
                {
                    request.WalkingSpeed = pp[JC.Keys.JourneyRequest_WalkingSpeed_StepFreeAssistance].Parse(80);
                    request.MaxWalkingDistance = pp[JC.Keys.JourneyRequest_MaxWalkingDistance_StepFreeAssistance].Parse(3000);
                }
                else if (request.AccessiblePreferences.RequireSpecialAssistance)
                {
                    request.WalkingSpeed = pp[JC.Keys.JourneyRequest_WalkingSpeed_Assistance].Parse(80);
                    request.MaxWalkingDistance = pp[JC.Keys.JourneyRequest_MaxWalkingDistance_Assistance].Parse(3000);
                }
                else if (request.AccessiblePreferences.RequireStepFreeAccess)
                {
                    request.WalkingSpeed = pp[JC.Keys.JourneyRequest_WalkingSpeed_StepFree].Parse(80);
                    request.MaxWalkingDistance = pp[JC.Keys.JourneyRequest_MaxWalkingDistance_StepFree].Parse(3000);
                }

                // Override algorithm
                if (request.AccessiblePreferences.RequireFewerInterchanges)
                {
                    request.PublicAlgorithm = GetPublicAlgorithm(pp[JC.Keys.JourneyRequest_AlgorithmPublic_MinChanges]);
                }

                // TDM rules
                if (pp[JC.Keys.JourneyRequest_TravelDemandPlanSwitch].Parse(true))
                {
                    // If accessible preference set with only Do not use underground, then we still want TDM rule applied
                    if (request.AccessiblePreferences.DoNotUseUnderground
                        && !request.AccessiblePreferences.RequireSpecialAssistance
                        && !request.AccessiblePreferences.RequireStepFreeAccess)
                    {
                        request.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanOutward_Accessible_DoNotUseUnderground];
                        request.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanReturn_Accessible_DoNotUseUnderground];
                    }
                    else
                    {
                        request.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanOff];
                        request.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanOff];
                    }
                }
            }
        }

        /// <summary>
        /// Updates the dont force coach flag in the journey request
        /// </summary>
        /// <param name="request"></param>
        private void UpdateDontForceCoach(SJPJourneyRequest request)
        {
            IPropertyProvider pp = Properties.Current;

            #region Dont Force Coach

            // Dont force coach is set using Property switches, and for the following scenarios only

            // Default false to let CJP determine how to apply force coach rule
            request.DontForceCoach = false;

            // If PT journey...
            if (request.PlannerMode == SJPJourneyPlannerMode.PublicTransport)
            {
                int londonAdminAreaCode = pp["AccessibilityOptions.DistrictList.AdminAreaCode.London"].Parse(82);
                bool propertyDontForceCoach_OriginDestinationLondon = pp[JC.Keys.JourneyRequest_DontForceCoach_OriginDestinationLondon].Parse(false);
                bool propertyDontForceCoach_Accessible_OriginDestinationLondon = pp[JC.Keys.JourneyRequest_DontForceCoach_Accessible_OriginDestinationLondon].Parse(false);
                bool propertyDontForceCoach_Accessible_FewerChanges = pp[JC.Keys.JourneyRequest_DontForceCoach_Accessible_FewerChanges].Parse(false);

                // ... and both origin and destination are in london (adminarea = 82) and not accessible request, or
                if ((request.Origin != null && request.Origin.AdminAreaCode == londonAdminAreaCode)
                    && (request.Destination != null && request.Destination.AdminAreaCode == londonAdminAreaCode)
                    && !request.AccessiblePreferences.Accessible
                    && propertyDontForceCoach_OriginDestinationLondon)
                {
                    request.DontForceCoach = true;
                }
                // ... and both origin and destination are in london (adminarea = 82) and is accessible request (step free/assistance only), or
                else if ((request.Origin != null && request.Origin.AdminAreaCode == londonAdminAreaCode)
                    && (request.Destination != null && request.Destination.AdminAreaCode == londonAdminAreaCode)
                    && (request.AccessiblePreferences.RequireStepFreeAccess || request.AccessiblePreferences.RequireSpecialAssistance)
                    && propertyDontForceCoach_Accessible_OriginDestinationLondon)
                {
                    request.DontForceCoach = true;
                }
                // ... and fewer changes required
                else if (request.AccessiblePreferences.RequireFewerInterchanges
                    && propertyDontForceCoach_Accessible_FewerChanges)
                {
                    request.DontForceCoach = true;
                }
            }

            #endregion
        }

        #endregion
    }
}
