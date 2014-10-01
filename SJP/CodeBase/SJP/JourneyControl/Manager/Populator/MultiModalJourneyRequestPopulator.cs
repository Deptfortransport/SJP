// *********************************************** 
// NAME             : MultiModalJourneyRequestPopulator.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 30 Mar 2011
// DESCRIPTION  	: MultiModalJourneyRequestPopulator class responsible for populating CJP JourneyRequests
// for all multi-modal and car-only journeys.
// ************************************************
// 

using System;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;
using SJP.Common.EventLogging;
using System.Linq;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// MultiModalJourneyRequestPopulator class responsible for populating CJP JourneyRequests
    /// for all multi-modal and car-only journeys
    /// </summary>
    public class MultiModalJourneyRequestPopulator : JourneyRequestPopulator
    {
        #region Constructor

        /// <summary>
        /// Constructs a new MultiModalJourneyRequestPopulator
        /// </summary>
        /// <param name="request">ISJPJourneyRequest</param>
        public MultiModalJourneyRequestPopulator(ISJPJourneyRequest sjpJourneyRequest)
        {
            this.sjpJourneyRequest = sjpJourneyRequest;
            this.properties = Properties.Current;
            this.locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the CJPRequest objects needed to call the CJP for the current 
        /// ISJPJourneyRequest, and returns them encapsulated in an array of CJPCall objects.
        /// </summary>
        public override CJPCall[] PopulateRequestsCJP(int referenceNumber, int seqNo, string sessionId, bool referenceTransaction, int userType, string language)
        {
            List<CJPCall> cjpCalls = new List<CJPCall>();

            ICJP.JourneyRequest request = null;

            // Outward journey required (or its a replan outward journey required)
            if ((sjpJourneyRequest.IsOutwardRequired && !sjpJourneyRequest.IsReplan)
                || (sjpJourneyRequest.ReplanIsOutwardRequired && sjpJourneyRequest.IsReplan))
            {
                request = PopulateSingleRequest(sjpJourneyRequest, false,
                                                    referenceNumber, seqNo++,
                                                    sessionId, referenceTransaction,
                                                    userType, language);

                cjpCalls.Add(new CJPCall(request, false, referenceNumber, sessionId));
            }

            // Return journey required (or its a reaplan return journey required)
            if ((sjpJourneyRequest.IsReturnRequired && !sjpJourneyRequest.IsReplan)
                || (sjpJourneyRequest.ReplanIsReturnRequired && sjpJourneyRequest.IsReplan))
            {
                request = PopulateSingleRequest(sjpJourneyRequest, true,
                                                referenceNumber, seqNo++,
                                                sessionId, referenceTransaction,
                                                userType, language);

                cjpCalls.Add(new CJPCall(request, true, referenceNumber, sessionId));
            }

            return cjpCalls.ToArray();
        }


        /// <summary>
        /// Creates the CyclePlannerRequest objects needed to call the Cycle planner service for the current 
        /// ISJPJourneyRequest, and returns them encapsulated in an array of CyclePlannerCall objects.
        /// </summary>
        public override CyclePlannerCall[] PopulateRequestsCTP(int referenceNumber,
                                                   int seqNo,
                                                   string sessionId,
                                                   bool referenceTransaction,
                                                   int userType,
                                                   string language)
        {
            // MultiModalJourneyRequestPopulator does not support creating calls for the CTP
            throw new SJPException("MultiModalJourneyRequestPopulator does not support creating calls for the CTP", false, SJPExceptionIdentifier.JCUnsupportedJourneyRequestPopulator);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create a single fully-populated CJP JourneyRequest object
        /// for a single multimodal request for a specified date.
        /// </summary>
        private ICJP.JourneyRequest PopulateSingleRequest(ISJPJourneyRequest sjpJourneyRequest,
            bool returnJourney,
            int referenceNumber,
            int seqNo,
            string sessionId,
            bool referenceTransaction,
            int userType,
            string language)
        {
            #region Initialise the request

            ICJP.JourneyRequest cjpRequest = InitialiseNewRequest(referenceNumber, seqNo,
                                                                sessionId, referenceTransaction,
                                                                userType, language);

            #endregion

            #region Set locations and date/times, plus any TOIDs

            bool privateRequired = sjpJourneyRequest.Modes.Contains(SJPModeType.Car);
            bool publicRequired = (sjpJourneyRequest.Modes.Count > 1 || sjpJourneyRequest.Modes[0] != SJPModeType.Car);

            bool isForAccessibleJourney = sjpJourneyRequest.AccessiblePreferences.Accessible;

            DateTime arriveTime = DateTime.MinValue;
            DateTime departTime = DateTime.MinValue;

            if (returnJourney)
            {
                // Return arrive before time (or its a replan arrive before time)
                if ((sjpJourneyRequest.ReturnArriveBefore && !sjpJourneyRequest.IsReplan)
                    || (sjpJourneyRequest.ReplanReturnArriveBefore && sjpJourneyRequest.IsReplan))
                {
                    arriveTime = GetRequestDateTime(sjpJourneyRequest, false);

                    cjpRequest.depart = false;
                }
                else
                {
                    departTime = GetRequestDateTime(sjpJourneyRequest, false);

                    cjpRequest.depart = true;
                }

                cjpRequest.origin = PopulateRequestPlace(sjpJourneyRequest.ReturnOrigin, departTime,
                    publicRequired, privateRequired, true, returnJourney, isForAccessibleJourney);
                cjpRequest.destination = PopulateRequestPlace(sjpJourneyRequest.ReturnDestination, arriveTime,
                    publicRequired, privateRequired, false, returnJourney, isForAccessibleJourney);
            }
            else
            {
                cjpRequest.depart = !sjpJourneyRequest.OutwardArriveBefore;

                // Outward arrive before time (or its a replan arrive before time)
                if ((sjpJourneyRequest.OutwardArriveBefore && !sjpJourneyRequest.IsReplan)
                    || (sjpJourneyRequest.ReplanOutwardArriveBefore && sjpJourneyRequest.IsReplan))
                {
                    arriveTime = GetRequestDateTime(sjpJourneyRequest, true);

                    cjpRequest.depart = false;
                }
                else
                {
                    departTime = GetRequestDateTime(sjpJourneyRequest, true);

                    cjpRequest.depart = true;
                }

                cjpRequest.origin = PopulateRequestPlace(sjpJourneyRequest.Origin, departTime,
                    publicRequired, privateRequired, true, returnJourney, isForAccessibleJourney);
                cjpRequest.destination = PopulateRequestPlace(sjpJourneyRequest.Destination, arriveTime,
                    publicRequired, privateRequired, false, returnJourney, isForAccessibleJourney);
            }

            #endregion

            cjpRequest.serviceFilter = null; // No services filter for SJP
            cjpRequest.operatorFilter = null; // No operators filter for SJP
            cjpRequest.parkNRide = false; // Not a park and ride request

            #region Modes

            List<SJPModeType> modes = sjpJourneyRequest.Modes;

            cjpRequest.modeFilter = new ICJP.Modes();
            cjpRequest.modeFilter.include = true;

            if (modes.Contains(SJPModeType.Bus) && !modes.Contains(SJPModeType.Drt))
            {
                bool isDrtRequired = properties[Keys.JourneyRequest_DrtIsRequired].Parse(true);

                if (isDrtRequired)
                {
                    modes.Add(SJPModeType.Drt);
                }
            }

            cjpRequest.modeFilter.modes = GetModeArray(modes);

            #endregion

            if (privateRequired)
            {
                cjpRequest.privateParameters = SetPrivateParameters(sjpJourneyRequest);
            }

            if (publicRequired)
            {
                cjpRequest.publicParameters = SetPublicParameters(sjpJourneyRequest, privateRequired, returnJourney);
            }

            return cjpRequest;
        }

        /// <summary>
        /// Instantiates a CJP JourneyRequest and populates some common attributes. 
        /// </summary>
        private ICJP.JourneyRequest InitialiseNewRequest(int referenceNumber, int seqNo, string sessionId,
                                                        bool referenceTransaction, int userType,
                                                        string language)
        {
            ICJP.JourneyRequest cjpRequest = new ICJP.JourneyRequest();

            cjpRequest.requestID = SqlHelper.FormatRef(referenceNumber) + FormatSeqNo(seqNo);
            cjpRequest.referenceTransaction = referenceTransaction;
            cjpRequest.sessionID = sessionId;
            cjpRequest.language = language;
            cjpRequest.userType = userType;

            return cjpRequest;
        }

        /// <summary>
		/// Takes an SJPLocation, and convert it into a CJP RequestPlace
		/// </summary>
		/// <returns></returns>
        private ICJP.RequestPlace PopulateRequestPlace(SJPLocation location, DateTime dateTime, 
            bool publicRequired, bool privateRequired,
            bool isOrigin, bool isForReturnJourney, bool isForAccessibleJourney)
		{
            ICJP.RequestPlace requestPlace = new ICJP.RequestPlace();

            // Changeable values based on location type
            string name = location.DisplayName;
            List<string> toids = location.Toid;
            List<string> naptans = location.Naptan;
            OSGridReference gridRef = location.GridRef;

            // If Private journey required, then should use the venue car park to populate details
            // If Public journey required, then should use the venue naptans to populate details
            SetVenueLocationDetails(location, privateRequired, isOrigin, isForReturnJourney, isForAccessibleJourney,
                ref name, ref toids, ref naptans, ref gridRef);

            #region name

            requestPlace.givenName = name;

            #endregion

            #region locality

            requestPlace.locality = location.Locality;

            #endregion

            #region via

            // No via locations in SJP
            requestPlace.userSpecifiedVia = false;

            #endregion
                                    
            #region place type

            // request place type
            switch (location.TypeOfLocation)
            {
                case SJPLocationType.Venue:
                    // Because some venues have unrecognised NaPTANs, should use the coordinate instead.
                    // This gives a better chance of finding journeys
                    if (location.UseNaPTAN)
                    {
                        requestPlace.type = ICJP.RequestPlaceType.NaPTAN;
                    }
                    else
                    {
                        requestPlace.type = ICJP.RequestPlaceType.Coordinate;

                        // If UseNaPTAN is false, but we have an accessible journey request 
                        // that contains accessible naptans, then use that
                        if (isForAccessibleJourney)
                        {
                            if (location is SJPVenueLocation)
                            {
                                SJPVenueLocation venueLocation = (SJPVenueLocation)location;

                                if ((venueLocation.AccessibleNaptans != null) && (venueLocation.AccessibleNaptans.Count > 0))
                                {
                                    requestPlace.type = ICJP.RequestPlaceType.NaPTAN;
                                }
                            }
                        }
                    }
                    break;
                case SJPLocationType.Locality:
                    requestPlace.type = ICJP.RequestPlaceType.Locality;
                    break;
                case SJPLocationType.Postcode:
                    requestPlace.type = ICJP.RequestPlaceType.Coordinate;
                    break;
                case SJPLocationType.CoordinateEN:
                    requestPlace.type = ICJP.RequestPlaceType.Coordinate;
                    break;
                default:
                    requestPlace.type = ICJP.RequestPlaceType.NaPTAN;
                    break;
            }

            #endregion

            #region coordinate

            // coordinates
            requestPlace.coordinate = new ICJP.Coordinate();
            requestPlace.coordinate.easting = Convert.ToInt32(gridRef.Easting);
            requestPlace.coordinate.northing = Convert.ToInt32(gridRef.Northing);

            #endregion

            if	(publicRequired)
            {
                #region naptans

                List<ICJP.RequestStop> requestStops = new List<ICJP.RequestStop>();

                if (naptans.Count == 0)
				{
                    // Always need at least one naptan, even if it's a dummy empty string,
                    //  so that we've got somewhere to hang the time 

                    ICJP.RequestStop requestStop = new ICJP.RequestStop();
                    					
					requestStop.NaPTANID = string.Empty;

					if	((dateTime != null) && (dateTime != DateTime.MinValue))
					{
						requestStop.timeDate = dateTime;
					}

                    requestStops.Add(requestStop);
				}
				else
				{
                    foreach (string naptan in naptans)
                    {
                        ICJP.RequestStop requestStop = new ICJP.RequestStop();

                        requestStop.NaPTANID = naptan;

                        if ((dateTime != null) && (dateTime != DateTime.MinValue))
                        {
                            requestStop.timeDate = dateTime;
                        }

                        if (gridRef.IsValid)
                        {
                            requestStop.coordinate = new ICJP.Coordinate();
                            requestStop.coordinate.easting = Convert.ToInt32(gridRef.Easting);
                            requestStop.coordinate.northing = Convert.ToInt32(gridRef.Northing);
                        }

                        requestStops.Add(requestStop);
                    }
				}

                requestPlace.stops = requestStops.ToArray();

                #endregion
            }

			if	(privateRequired)
            {
                #region road points

                List<ICJP.ITN> requestRoadPoints = new List<ICJP.ITN>();

                #region Remove duplicate toids

                // Remove any duplicate toids
                List<string> editedToidList = new List<string>();

                foreach (string toid in toids)
                {
                    if (!editedToidList.Contains(toid))
                    {
                        editedToidList.Add(toid);
                    }
                }

                #endregion

                string toidPrefix = properties[Keys.Toid_Prefix];

				if	(toidPrefix == null) 
				{
					toidPrefix = string.Empty;
				}

                #region Add ITNs for toids

                foreach (string toid in editedToidList)
                {
                    ICJP.ITN itn = new ICJP.ITN();

                    // toids must have the toid prefix
                    if (!toid.StartsWith(toidPrefix))
                    {
                        itn.TOID = toidPrefix + toid;
                    }
                    else
                    {
                        itn.TOID = toid;
                    }

                    itn.node = false;		// ESRI supplies us with links, not nodes

                    if ((dateTime != null) && (dateTime != DateTime.MinValue))
                    {
                        itn.timeDate = dateTime;
                    }

                    // Add the ITN
                    requestRoadPoints.Add(itn);
                }

                #endregion

                requestPlace.roadPoints = requestRoadPoints.ToArray();

                #endregion
            }

            return requestPlace;
		}
        
        /// <summary>
        /// Create an array of ICJP.Mode from the given array of SJPModeType
        /// Modes are used within the CJPJourneyRequest Type
        /// </summary>
        private ICJP.Mode[] GetModeArray(List<SJPModeType> modes)
        {
            if (modes != null)
            {
                List<ICJP.Mode> modeResult = new List<ICJP.Mode>();
                
                foreach(SJPModeType sjpMode in modes)
                {
                    ICJP.Mode mode = new ICJP.Mode();
                    
                    mode.mode = SJPModeTypeHelper.GetCJPModeType(sjpMode);

                    modeResult.Add(mode);
                }

                return modeResult.ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updates the name, toids, naptans, if criteria to use Venue specific details are met, 
        /// if not then ref values remain unaltered.
        /// e.g. if for a Car journey and the Venue location has a Car Park Id specified
        /// e.g. if for a Public journey and the Venue river pier naptan is specified
        /// </summary>
        private void SetVenueLocationDetails(SJPLocation location, bool privateRequired,
            bool isOrigin, bool isForReturnJourney, bool isForAccessibleJourney,
            ref string name, ref List<string> toids, ref List<string> naptans, ref OSGridReference gridRef)
        {
            if (location is SJPVenueLocation)
            {
                SJPVenueLocation venueLocation = (SJPVenueLocation)location;
                
                // If Private journey required, then should use the venue car park to populate details
                if ((privateRequired) && (!string.IsNullOrEmpty(venueLocation.SelectedSJPParkID)))
                {
                    #region Update for Car park location
                                        
                    SJPVenueCarPark carPark = locationService.GetSJPVenueCarPark(venueLocation.SelectedSJPParkID);

                    if (carPark != null)
                    {
                        // For outward journey, destination will be to the "entrance" TOID of car park
                        if (!isForReturnJourney && !isOrigin)
                        {
                            name = carPark.Name;

                            if (!string.IsNullOrEmpty(carPark.DriveToToid))
                            {
                                toids = new List<string>(1);
                                toids.Add(carPark.DriveToToid.Trim());
                            }
                            else
                            {
                                // Should exist but log to inform support
                                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, 
                                    string.Format("Attempt to use CarPark[{0}][{1}] for journey planning is missing a DriveToToid", carPark.ID, carPark.Name)));
                            }
                        }
                        // For return journey, origin will be to the "exit" TOID of car park
                        else if (isForReturnJourney && isOrigin)
                        {
                            name = carPark.Name;

                            if (!string.IsNullOrEmpty(carPark.DriveFromToid))
                            {
                                toids = new List<string>(1);
                                toids.Add(carPark.DriveFromToid.Trim());
                            }
                            else
                            {
                                // Should exist but log to inform support
                                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                                    string.Format("Attempt to use CarPark[{0}][{1}] for journey planning is missing a DriveFromToid", carPark.ID, carPark.Name)));
                            }
                        }
                    }

                    #endregion
                }
                // If Public journey required, and pier naptan, then should use the venue river service to populate details
                else if (!string.IsNullOrEmpty(venueLocation.SelectedPierNaptan))
                {
                    #region Update for River Services Pier location

                    // Use pier naptans
                    List<string> pierNaPTANs = new List<string>() { venueLocation.SelectedPierNaptan };
                    naptans = pierNaPTANs;

                    // Use name provided
                    if (!string.IsNullOrEmpty(venueLocation.SelectedName))
                    {
                        name = venueLocation.SelectedName;
                    }

                    if (venueLocation.SelectedGridReference.IsValid)
                    {
                        gridRef = venueLocation.SelectedGridReference;
                    }

                    #endregion
                }
                else if (isForAccessibleJourney)
                {
                    #region Update for Accessible venue location

                    // Use the venue accessible naptans if they exist
                    if ((venueLocation.AccessibleNaptans != null) && (venueLocation.AccessibleNaptans.Count > 0))
                    {
                        naptans = venueLocation.AccessibleNaptans;
                    }

                    // If a name provided
                    if (!string.IsNullOrEmpty(venueLocation.SelectedName))
                    {
                        if (naptans.Count > 1)
                        {
                            // Set name to empty string in the event multiple accessible naptans are added,
                            // the cjp will then output the name rather than the one set here
                            name = string.Empty;
                        }
                        else
                        {
                            name = venueLocation.SelectedName;
                        }
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// Returns the Outward/Return date time to use in the request. 
        /// If the location is a Venue location which contains a "datetime to use", then this is returned
        /// but only for the Outward journey Destination location or Return journey Origin location
        /// (as these are currently the ones only set for a Car park journey),
        /// otherwise returns the journey request date time
        /// </summary>
        private DateTime GetRequestDateTime(ISJPJourneyRequest journeyRequest, bool outward)
        {
            DateTime dateTime = DateTime.MinValue;

            SJPVenueLocation venueLocation = null;

            if (outward)
            {
                dateTime = journeyRequest.OutwardDateTime;

                // For outward journey, if destination is Venue, then use it's datetime
                if (journeyRequest.Destination is SJPVenueLocation)
                {
                    venueLocation = (SJPVenueLocation)journeyRequest.Destination;
                }
            }
            else
            {
                dateTime = journeyRequest.ReturnDateTime;

                // For return journey, if origin is Venue, then use it's datetime
                if (journeyRequest.ReturnOrigin is SJPVenueLocation)
                {
                    venueLocation = (SJPVenueLocation)journeyRequest.ReturnOrigin;
                }
            }

            #region Use Venue location date time

            if (venueLocation != null)
            {
                // Currently only Park and Ride, Blue Badge, and River Service specify a Venue location datetime value,
                // and also a Public Transport Accessible journey
                if ((journeyRequest.PlannerMode == SJPJourneyPlannerMode.ParkAndRide
                    || journeyRequest.PlannerMode == SJPJourneyPlannerMode.BlueBadge
                    || journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices)
                    || (journeyRequest.PlannerMode == SJPJourneyPlannerMode.PublicTransport && journeyRequest.AccessiblePreferences.Accessible))
                {
                    if (outward)
                    {
                        if (venueLocation.SelectedOutwardDateTime != DateTime.MinValue)
                        {
                            dateTime = venueLocation.SelectedOutwardDateTime;
                        }
                    }
                    else
                    {
                        if (venueLocation.SelectedReturnDateTime != DateTime.MinValue)
                        {
                            dateTime = venueLocation.SelectedReturnDateTime;
                        }
                    }
                }
            }

            #endregion

            #region Replan datetime

            // If its a replan, then need to use the replan datetime
            // update replan datetime to take into account the "venue selected datetime" (accessible journey)
            if (journeyRequest.IsReplan)
            {
                if (outward)
                {
                    dateTime = journeyRequest.ReplanOutwardDateTime;
                }
                else
                {
                    dateTime = journeyRequest.ReplanReturnDateTime;
                }
            }

            #endregion

            return dateTime;
        }

        #region Private parameters

        /// <summary>
        /// Fill the CJP PrivateParameters fields for a single multimodal request.
        /// </summary>
        private ICJP.PrivateParameters SetPrivateParameters(ISJPJourneyRequest sjpJourneyRequest)
        {
            ICJP.PrivateParameters privateParameters = new ICJP.PrivateParameters();

            // Fixed parameters
            privateParameters.flowType = ICJP.FlowType.Congestion;
            privateParameters.vehicleType = ICJP.VehicleType.Car;

            // Changeable parameters
            privateParameters.avoidMotorway = sjpJourneyRequest.AvoidMotorways;
            privateParameters.avoidFerries = sjpJourneyRequest.AvoidFerries;
            privateParameters.avoidToll = sjpJourneyRequest.AvoidTolls;
            privateParameters.avoidRoads = GetRoads(sjpJourneyRequest.AvoidRoads);
            privateParameters.useRoads = GetRoads(sjpJourneyRequest.IncludeRoads);
            privateParameters.algorithm = GetPrivateAlgorithm(sjpJourneyRequest.PrivateAlgorithm);
            privateParameters.maxSpeed = sjpJourneyRequest.DrivingSpeed;
            privateParameters.banMotorway = sjpJourneyRequest.DoNotUseMotorways;
            privateParameters.fuelConsumption = Convert.ToInt32(Convert.ToDecimal((sjpJourneyRequest.FuelConsumption)));
            privateParameters.fuelPrice = Convert.ToInt32(Convert.ToDecimal((sjpJourneyRequest.FuelPrice)));

            // No via locations in SJP
            privateParameters.vias = new ICJP.RequestPlace[0];

            return privateParameters;
        }

        /// <summary>
        /// Create an array of Roads and populate the contents from an array of strings
        /// </summary>
        /// <param name="Roads">String array of road names</param>
        private ICJP.Road[] GetRoads(List<string> stringRoads)
        {
            List<ICJP.Road> roadResult = new List<ICJP.Road>();

            foreach(string strRoad in stringRoads)
            {
                if (!string.IsNullOrEmpty(strRoad))
                {
                    ICJP.Road road = new ICJP.Road();

                    road.roadNumber = strRoad;

                    roadResult.Add(road);
                }
            }

            return roadResult.ToArray();
        }

        /// <summary>
        /// Converts an SJPPrivateAlgorithmType into a CJP PrivateAlgorithmType
        /// </summary>
        /// <param name="sjpPrivateAlgorithm"></param>
        /// <returns></returns>
        private ICJP.PrivateAlgorithmType GetPrivateAlgorithm(SJPPrivateAlgorithmType sjpPrivateAlgorithm)
        {
            switch (sjpPrivateAlgorithm)
            {
                case SJPPrivateAlgorithmType.Cheapest:
                    return ICJP.PrivateAlgorithmType.Cheapest;
                case SJPPrivateAlgorithmType.MostEconomical:
                    return ICJP.PrivateAlgorithmType.MostEconomical;
                case SJPPrivateAlgorithmType.Shortest:
                    return ICJP.PrivateAlgorithmType.Shortest;
                default:
                    return ICJP.PrivateAlgorithmType.Fastest;
            }
        }

        #endregion

        #region Public Parameters

        /// <summary>
        /// Fill the PublicParameters fields for a single multimodal request.
        /// </summary>
        private ICJP.PublicParameters SetPublicParameters(ISJPJourneyRequest sjpJourneyRequest, bool privateRequired, bool returnJourney)
        {
            ICJP.PublicParameters parameters = new ICJP.PublicParameters();

            // Fixed parameters
            parameters.trunkPlan = false;
            parameters.intermediateStops = ICJP.IntermediateStopsType.All;
            parameters.rangeType = ICJP.RangeType.Sequence;

            // Changeable parameters
            parameters.algorithm = GetPublicAlgorithm(sjpJourneyRequest.PublicAlgorithm);
            parameters.interchangeSpeed = sjpJourneyRequest.InterchangeSpeed;
            parameters.walkSpeed = sjpJourneyRequest.WalkingSpeed;
            parameters.sequence = (privateRequired ? sjpJourneyRequest.Sequence - 1 : sjpJourneyRequest.Sequence);
            parameters.extraSequence = 0;
            parameters.extraInterval = DateTime.MinValue;
            parameters.extraCheckInTime = DateTime.MinValue;

            // Distance in metres, times in minutes, speeds in metres/min ...
            if (sjpJourneyRequest.MaxWalkingDistance > 0)
            {
                parameters.maxWalkDistance = sjpJourneyRequest.MaxWalkingDistance;
            }
            else
            {
                parameters.maxWalkDistance = sjpJourneyRequest.WalkingSpeed * sjpJourneyRequest.MaxWalkingTime;
            }

            // No via locations in SJP
            parameters.vias = new ICJP.RequestPlace[0];
            parameters.softVias = new ICJP.RequestPlace[0];
            parameters.notVias = new ICJP.RequestPlace[0];
            
            // Set up Routing guide specific values
            parameters.routingGuideInfluenced = sjpJourneyRequest.RoutingGuideInfluenced;
            parameters.rejectNonRGCompliantJourneys = sjpJourneyRequest.RoutingGuideCompliantJourneysOnly;
            parameters.routeCodes = sjpJourneyRequest.RouteCodes;

            // Set up Awkward Overnight Journey Rules
            parameters.removeAwkwardOvernight = sjpJourneyRequest.RemoveAwkwardOvernight;

            // Games specific
            parameters.olympicRequest = sjpJourneyRequest.OlympicRequest;
            if (returnJourney)
            {
                parameters.travelDemandPlan = sjpJourneyRequest.TravelDemandPlanReturn;
            }
            else
            {
                parameters.travelDemandPlan = sjpJourneyRequest.TravelDemandPlanOutward;
            }
            
            // Accessible parameters
            parameters.accessibilityOptions = GetAccessibilityOptions(sjpJourneyRequest);
            parameters.filtering = (sjpJourneyRequest.FilteringStrict) ? ICJP.FilterOptionEnumeration.strict : ICJP.FilterOptionEnumeration.permissive;

            // Force coach 
            parameters.dontForceCoach = sjpJourneyRequest.DontForceCoach;

            return parameters;
        }

        /// <summary>
        /// Converts an SJPPublicAlgorithmType into a CJP PublicAlgorithmType
        /// </summary>
        /// <param name="sjpPublicAlgorithm"></param>
        /// <returns></returns>
        private ICJP.PublicAlgorithmType GetPublicAlgorithm(SJPPublicAlgorithmType sjpPublicAlgorithm)
        {
            switch (sjpPublicAlgorithm)
            {
                case SJPPublicAlgorithmType.Fastest:
                    return ICJP.PublicAlgorithmType.Fastest;
                case SJPPublicAlgorithmType.Max1Change:
                    return ICJP.PublicAlgorithmType.Max1Change;
                case SJPPublicAlgorithmType.Max2Changes:
                    return ICJP.PublicAlgorithmType.Max2Changes;
                case SJPPublicAlgorithmType.Max3Changes:
                    return ICJP.PublicAlgorithmType.Max3Changes;
                case SJPPublicAlgorithmType.NoChanges:
                    return ICJP.PublicAlgorithmType.NoChanges;
                case SJPPublicAlgorithmType.MinChanges:
                    return ICJP.PublicAlgorithmType.MinChanges;
                default:
                    return ICJP.PublicAlgorithmType.Default;
            }
        }

        /// <summary>
        /// Gets the CJP AccessibilityOptions for the request
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        /// <returns></returns>
        private ICJP.AccessibilityOptions GetAccessibilityOptions(ISJPJourneyRequest sjpJourneyRequest)
        {
            SJPAccessiblePreferences accessiblePreferences = sjpJourneyRequest.AccessiblePreferences;

            // Accessible journey required
            if ((accessiblePreferences != null) && (accessiblePreferences.Accessible))
            {
                ICJP.AccessibilityOptions accessibilityOptions = new ICJP.AccessibilityOptions();
                                
                if (accessiblePreferences.DoNotUseUnderground)
                {
                    // Remove Underground from modes
                    // This will have already been done by the JourneyRequestHelper when setting the modes for request
                }
                if (accessiblePreferences.RequireStepFreeAccess)
                {
                    accessibilityOptions.wheelchairUse = ICJP.AccessibilityRequirement.Essential;
                }
                if (accessiblePreferences.RequireSpecialAssistance)
                {
                    accessibilityOptions.assistanceService = ICJP.AccessibilityRequirement.Essential;
                }
                                                
                return accessibilityOptions;
            }

            // Accessible journey options not required
            return null;
        }

        #endregion

        #endregion
    }
}
