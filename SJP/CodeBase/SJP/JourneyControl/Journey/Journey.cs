// *********************************************** 
// NAME             : Journey.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Journey class encapsulating journey 
// data common to all types of journey. This class is used as the wrapper to all SJP Journeys
// regardless of type (i.e. public, road, cycle). The class contains an 
// array of JourneyLegs which contain the detailed elements of each journey type. The JourneyLegs
// in turn contain JourneyDetails which provide further details for the different journey 
// types (public, road, cycle)
// ************************************************
// 


using System;
using System.Collections.Generic;
using System.Text;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using CPWS = SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Journey class encapsulating journey 
    /// data common to all types of journey
    /// </summary>
    [Serializable()]
    public class Journey
    {
        #region Private members

        protected int journeyId = 0;
        protected DateTime journeyDate;
        protected List<JourneyLeg> journeyLegs = new List<JourneyLeg>();
        protected RoutingDetail routingDetail = new RoutingDetail();
        protected TimeSpan duration = new TimeSpan(0);
        protected int interchangeCount = -1;
        protected bool accessibleJourney = false;
        protected bool valid = true;
        protected List<SJPMessage> messages = new List<SJPMessage>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Journey()
        {
        }

        #region Specific CJP journey type constructors

        /// <summary>
        /// Constructor for adding a CJP PublicJourney
        /// </summary>
        public Journey(int journeyId, ICJP.PublicJourney cjpPublicJourney, bool outward, 
            SJPLocation originLocation, SJPLocation destinationLocation,
            bool accessible, Journey journeyPart, Language language)
        {
            this.journeyId = journeyId;
            
            #region Set JourneyDate

            // Default the request time, this will be updated and used later by the using the cjp journey 
            // or the venue time used for accessible journey
            DateTime requestTime = DateTime.MinValue;

            if (cjpPublicJourney != null)
            {
                // Set journeyDate from the first non-walk leg
                foreach (ICJP.Leg leg in cjpPublicJourney.legs)
                {
                    if (leg.mode != ICJP.ModeType.Walk)
                    {
                        journeyDate = DateTimeHelper.GetDateTime(leg.board.departTime, DateTimeHelper.DateTimeType.Date);
                        break;
                    }
                }
            }
            else if (journeyPart != null) // Used to create river service journeys where no PT journey was planned
            {
                journeyDate = journeyPart.JourneyDate;
            }
            else if (accessible) // Used to create accessible transfer journey where no PT journey was planned
            {
                // Because the accessible journey may have started from an accessible naptan, check for this and use that
                // as the request time instead
                requestTime = GetVenueLocationDateTime(requestTime, outward, accessible, originLocation, destinationLocation);
                
                journeyDate = requestTime.Date;
            }

            #endregion

            #region Setup journey legs

            #region Add Return journey leg parts provided

            // For return journey, if a journey part has been supplied (currently only for River Service planner mode),
            // then must add the legs (which should be a ferry leg), and the transit legs to get from the Venue
            // (where the ferry leg starts) to Venue pier
            // This includes adding where data available:
            //      1) venue to venue gate
            //      2) venue gate to venue pier
            //      3) venue pier ferry leg(s)
            if (journeyPart != null && !outward && journeyPart.JourneyLegs.Count > 0)
            {
                // Legs to add
                JourneyLeg pierInterchangeLeg = null;
                JourneyLeg gateJourneyLeg = null;

                SJPLocation interchangeStartLocation = null;
                SJPLocation interchangeEndLocation = null;
                SJPLocation gateEndLocation = null;

                // Used to create transit from venue legs, this should be the venue pier location
                SJPLocation startLocation = journeyPart.JourneyLegs[0].LegStart.Location;

                // Request time is now the start time of the first journey leg
                DateTime requestStartTime = journeyPart.JourneyLegs[0].StartTime;

                #region Set interchange leg details

                // Set the start for the interchange leg (assume starts at venue origin)
                interchangeStartLocation = originLocation;

                // Set the end for the interchange leg (assume this is the venue pier)
                interchangeEndLocation = startLocation;

                // Get the venue to pier navigation path, this will give us a duration and the gate used
                SJPPierVenueNavigationPath navigationPath = GetPierNavigationForVenueLocation(originLocation, startLocation, false);

                #endregion

                #region Set gate leg details

                // Set the end for the gate leg (assume ends at start location of first journey leg)
                gateEndLocation = startLocation;

                // Identify the gate the navigation path ends at
                string venueGateNaPTAN = string.Empty;

                if (navigationPath != null)
                    venueGateNaPTAN = navigationPath.FromNaPTAN;

                // Get the venue gate and path details for the pier
                SJPVenueGate gate = GetVenueGate(venueGateNaPTAN);
                SJPVenueGateCheckConstraint gateCheckConstraint = GetVenueGateCheckConstraint(gate, false);
                SJPVenueGateNavigationPath gateNavigationPath = GetVenueGateNavigationForVenueLocation(destinationLocation, gate, false);

                // If gate details exists, update the end of gate leg to be the gate
                if (gate != null)
                {
                    // Gate leg now starts at the gate
                    gateEndLocation = new SJPLocation();
                    gateEndLocation.Name = gate.GateName;
                    gateEndLocation.DisplayName = gate.GateName;
                    gateEndLocation.GridRef = gate.GateGridRef;
                    gateEndLocation.Naptan.Add(gate.GateNaPTAN);

                    // Interchange leg now starts at the gate
                    interchangeStartLocation = gateEndLocation;
                }

                #endregion

                #region Gate leg

                // If a venue to gate is required
                if (gate != null)
                {
                    // Set the duration to be navigation path + check constraint times
                    int durationMins = (gateNavigationPath != null) ?
                        Convert.ToInt32(gateNavigationPath.TransferDuration.TotalMinutes) : 0;

                    durationMins = durationMins +
                        ((gateCheckConstraint != null) ?
                         Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalMinutes) : 0);

                    // Create gate leg
                    gateJourneyLeg = JourneyLeg.Create(SJPModeType.Walk,
                        originLocation, gateEndLocation,
                        requestStartTime, durationMins, true);

                    gateJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(gate, gateCheckConstraint, gateNavigationPath));

                    // Update the request time to be the end of gate leg
                    requestStartTime = gateJourneyLeg.EndTime;
                }

                #endregion

                #region Interchange leg

                // Check for an interchange from gate
                if (navigationPath != null)
                {
                    // If this is the first or last leg, we have to include a depart/arrive time
                    DateTime startTime = DateTime.MinValue;
                    if (gate == null)
                    {
                        startTime = requestStartTime;
                    }

                    pierInterchangeLeg = JourneyLeg.Create(SJPModeType.Walk,
                                    interchangeStartLocation, interchangeEndLocation,
                                    startTime, DateTime.MinValue);

                    pierInterchangeLeg.JourneyDetails.Add(GetPublicJourneyDetail(Convert.ToInt32(navigationPath.DefaultDuration.TotalMinutes)));
                }

                #endregion
                
                #region Add journey legs

                // Add legs
                if (gateJourneyLeg != null)
                    journeyLegs.Add(gateJourneyLeg);
                if (pierInterchangeLeg != null)
                    journeyLegs.Add(pierInterchangeLeg);

                #endregion

                #region Ferry leg(s)

                foreach (JourneyLeg leg in journeyPart.JourneyLegs)
                {
                    journeyLegs.Add(leg);
                }

                #endregion

                #region Check if Ferry to PT time difference is acceptable

                if (cjpPublicJourney != null)
                {
                    // Check if PT journey starts at the ferry pier within an acceptable time period
                    int allowedPeriodMins = Properties.Current["JourneyControl.RiverService.Interchange.Max.Minutes"].Parse(60);

                    DateTime ferryEndTime = journeyPart.JourneyLegs[journeyPart.JourneyLegs.Count - 1].EndTime;
                    DateTime ptJourneyStartTime = cjpPublicJourney.legs[0].board.departTime;

                    TimeSpan timeDifference = ptJourneyStartTime.Subtract(ferryEndTime);

                    if (timeDifference.TotalMinutes > allowedPeriodMins)
                    {
                        // Time difference is above allowed period, mark journey as invalid. 
                        // Setting as invalid informs the SJPJourneyResult manager not to add the journey
                        valid = false;
                    }
                }

                #endregion
            }

            #endregion

            #region Add Return accessible journey leg

            // For return journey, if an accessible journey was planned from a venue origin which required
            // an accessible naptan to be used, then the PT journey needs an a journey leg added which 
            // takes the user from the venue to the accessible naptan

            if (originLocation is SJPVenueLocation)
            {
                SJPVenueLocation venue = (SJPVenueLocation)originLocation;

                if ((venue.AccessibleNaptans != null) && (venue.AccessibleNaptans.Count > 0))
                {
                    // If no cjp journey and accessible journey, then assume caller intentionally wants the accessible leg added
                    if (((cjpPublicJourney != null) && (cjpPublicJourney.legs != null) && (cjpPublicJourney.legs.Length > 0))
                        || (accessible))
                    {
                        #region Set location and datetime

                        // Get the start location (from the first journey leg or it is the destination location)
                        SJPLocation startLocation = destinationLocation;

                        if (cjpPublicJourney != null)
                        {
                            JourneyLeg journeyLeg = JourneyLeg.Create(cjpPublicJourney.legs[0]);

                            startLocation = journeyLeg.LegStart.Location;

                            // Request time is the start time of the first journey leg
                            requestTime = journeyLeg.StartTime;
                        }

                        #endregion

                        // Identify if the start location naptan is the same as venue accessible naptan
                        foreach (string startNaptan in startLocation.Naptan)
                        {
                            // Journey starts at the accessible naptan, add the accessible journey leg
                            SJPVenueAccess venueAccess = null;
                            SJPVenueAccessStation venueAccessStation = null;

                            GetVenueAccess(venue, startNaptan, requestTime, false, out venueAccess, out venueAccessStation);

                            // Compare naptans to make sure they're the same,
                            // or if only SJPVenueAccess found, then allow leg to be added (this is to fix an issue where
                            // CJP result does not end at the accessible NaPTAN, but close to it, e.g. the station entrance)
                            if ((venue.AccessibleNaptans.Exists(delegate(string s) { return s == startNaptan; }))
                                || (venueAccess != null))
                            {
                                #region Access leg

                                if (venueAccessStation != null)
                                {
                                    // Access station found, build leg

                                    // Legs to add
                                    JourneyLeg accessLeg = null;

                                    // Set the start for the access leg (assume starts at the venue destination)
                                    SJPLocation accessStartLocation = originLocation;

                                    // Set the end for the access leg (assume this is the start location of the PT journey)
                                    SJPLocation accessEndLocation = startLocation;

                                    #region Amend leg times

                                    // Amend time
                                    DateTime requestStartTime = DateTime.MinValue;
                                    DateTime requestEndTime = DateTime.MinValue;

                                    // Start time is the request time less the transfer duration
                                    if (requestTime != DateTime.MinValue)
                                        requestStartTime = requestTime.Subtract(venueAccess.AccessToVenueDuration);
                                    
                                    // No cjp journey, then need to set the end time based to be the request time passed in
                                    if (cjpPublicJourney == null)
                                        requestEndTime = requestTime;

                                    #endregion

                                    // If this is the first/last leg, include a depart/arrive time
                                    accessLeg = JourneyLeg.Create(SJPModeType.Transfer,
                                                        accessStartLocation, accessEndLocation,
                                                        requestStartTime, requestEndTime);

                                    accessLeg.JourneyDetails.Add(GetPublicJourneyDetail(venueAccess, venueAccessStation, SJPModeType.Transfer, language, false));

                                    // Add journey leg
                                    journeyLegs.Add(accessLeg);
                                    break;
                                }

                                #endregion
                            }
                        }
                    }
                }
            }

            #endregion

            #region Add PT journey legs

            // Each cjpPublicJourney leg is converted in to a JourneyLeg

            if (cjpPublicJourney != null)
            {
                int legIndex = 0;

                foreach (ICJP.Leg cjpLeg in cjpPublicJourney.legs)
                {
                    switch (cjpLeg.mode)
                    {
                        // These modes are not represented by their own JourneyLeg
                        case ICJP.ModeType.CheckIn:
                        case ICJP.ModeType.CheckOut:
                        case ICJP.ModeType.Transfer:
                            {
                                break;
                            }
                        default:
                            {
                                // Create the JourneyLeg
                                JourneyLeg journeyLeg = JourneyLeg.Create(cjpLeg);

                                // Populate the journey details for this leg
                                if (cjpLeg.mode == ICJP.ModeType.Air)
                                {
                                    // There should always be a CheckIn/Transfer leg before this and a
                                    // CheckOut/Transfer leg after it.
                                    ICJP.Leg cjpPreviousLeg = null;
                                    ICJP.Leg cjpSubsequentLeg = null;

                                    if (legIndex != 0)
                                    {
                                        cjpPreviousLeg = cjpPublicJourney.legs[legIndex - 1];
                                    }

                                    if (legIndex < (cjpPublicJourney.legs.Length - 1))
                                    {
                                        cjpSubsequentLeg = cjpPublicJourney.legs[legIndex + 1];
                                    }

                                    journeyLeg.PopulateJourneyDetails(cjpLeg, cjpPreviousLeg, cjpSubsequentLeg);
                                }
                                else
                                {
                                    journeyLeg.PopulateJourneyDetails(cjpLeg, null, null);
                                }

                                // Add to the journey legs list
                                journeyLegs.Add(journeyLeg);
                                break;
                            }
                    }

                    legIndex++;
                }
            }

            #endregion

            #region Add Outward journey leg parts provided
            
            // For outward journey, if a journey part has been supplied (currently only for River Service planner mode),
            // then must add the legs (which should be a ferry leg), and the transit legs to get from the Venue pier
            // (where the ferry leg finishes) to Venue
            // This includes adding where data available:
            //      1) ferry leg(s) to venue pier
            //      2) venue pier to venue gate
            //      3) venue gate to venue
            if (journeyPart != null && outward && journeyPart.JourneyLegs.Count > 0)
            {
                #region Check if PT to Ferry time difference is acceptable

                if (journeyLegs.Count > 0)
                {
                    // Check if PT journey ends at the ferry pier within an acceptable time period
                    int allowedPeriodMins = Properties.Current["JourneyControl.RiverService.Interchange.Max.Minutes"].Parse(60);

                    DateTime ptJourneyEndTime = journeyLegs[journeyLegs.Count - 1].EndTime;
                    DateTime ferryStartTime = journeyPart.StartTime;

                    TimeSpan timeDifference = ferryStartTime.Subtract(ptJourneyEndTime);

                    if (timeDifference.TotalMinutes > allowedPeriodMins)
                    {
                        // Time difference is above allowed period, mark journey as invalid. 
                        // Setting as invalid informs the SJPJourneyResult manager not to add the journey
                        valid = false;
                    }
                }

                #endregion

                #region Ferry leg(s)

                foreach (JourneyLeg leg in journeyPart.JourneyLegs)
                {
                    journeyLegs.Add(leg);
                }

                #endregion

                // Legs to add
                JourneyLeg pierInterchangeLeg = null;
                JourneyLeg gateJourneyLeg = null;

                SJPLocation interchangeStartLocation = null;
                SJPLocation interchangeEndLocation = null;
                SJPLocation gateStartLocation = null;

                // Used to create transit to venue legs, this should be the venue pier location
                SJPLocation endLocation = journeyLegs[journeyLegs.Count - 1].LegEnd.Location;

                // Request time is now the end time of the last journey leg
                DateTime requestStartTime = journeyLegs[journeyLegs.Count - 1].EndTime;

                #region Set interchange leg details

                // Set the start for the interchange leg (assume starts at the end location of last journey leg)
                interchangeStartLocation = endLocation;

                // Set the end for the interchange leg (assume this is the venue destination)
                interchangeEndLocation = destinationLocation;

                // Get the pier to venue navigation path, this will give us a duration and the gate used
                SJPPierVenueNavigationPath navigationPath = GetPierNavigationForVenueLocation(destinationLocation, endLocation, true);
                
                #endregion

                #region Set gate leg details

                // Set the start for the gate leg (assume starts at end location of last journey leg)
                gateStartLocation = endLocation;

                // Identify the gate the navigation path ends at
                string venueGateNaPTAN = string.Empty;

                if (navigationPath != null)
                    venueGateNaPTAN = navigationPath.ToNaPTAN;

                // Get the venue gate and path details for the pier`
                SJPVenueGate gate = GetVenueGate(venueGateNaPTAN);
                SJPVenueGateCheckConstraint gateCheckConstraint = GetVenueGateCheckConstraint(gate, true);
                SJPVenueGateNavigationPath gateNavigationPath = GetVenueGateNavigationForVenueLocation(destinationLocation, gate, true);

                // If gate details exists, update the start of gate leg to be the gate
                if (gate != null)
                {
                    // Gate leg now starts at the gate
                    gateStartLocation = new SJPLocation();
                    gateStartLocation.Name = gate.GateName;
                    gateStartLocation.DisplayName = gate.GateName;
                    gateStartLocation.GridRef = gate.GateGridRef;
                    gateStartLocation.Naptan.Add(gate.GateNaPTAN);

                    // Interchange leg now ends at the gate
                    interchangeEndLocation = gateStartLocation;
                }

                #endregion

                #region Interchange leg

                // Check for an interchange to gate
                if (navigationPath != null)
                {
                    // This is not the first or last leg, so don't have to include a depart/arrive time
                    pierInterchangeLeg = JourneyLeg.Create(SJPModeType.Walk,
                                    interchangeStartLocation, interchangeEndLocation,
                                    DateTime.MinValue, DateTime.MinValue);

                    pierInterchangeLeg.JourneyDetails.Add(GetPublicJourneyDetail(Convert.ToInt32(navigationPath.DefaultDuration.TotalMinutes)));

                    // Check for an interchange to gate, and amend time
                    requestStartTime = requestStartTime.Add(navigationPath.DefaultDuration);
                }

                #endregion

                #region Gate leg

                // If a gate to venue is required
                if (gate != null)
                {
                    // Set the duration to be navigation path + check constraint times
                    int durationMins = (gateNavigationPath != null) ?
                        Convert.ToInt32(gateNavigationPath.TransferDuration.TotalMinutes) : 0;

                    durationMins = durationMins +
                        ((gateCheckConstraint != null) ?
                         Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalMinutes) : 0);

                    // Create gate leg
                    gateJourneyLeg = JourneyLeg.Create(SJPModeType.Walk,
                        gateStartLocation, destinationLocation,
                        requestStartTime, durationMins, false);

                    gateJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(gate, gateCheckConstraint, gateNavigationPath));
                }

                #endregion

                #region Add journey legs

                // Add legs
                if (pierInterchangeLeg != null)
                    journeyLegs.Add(pierInterchangeLeg);
                if (gateJourneyLeg != null)
                    journeyLegs.Add(gateJourneyLeg);

                #endregion
            }

            #endregion

            #region Add Outward accessible journey leg 

            // For outward journey, if an accessible journey was planned to a venue destination which required
            // an accessible naptan to be used, then the PT journey needs a journey leg added which 
            // takes the user from the accessible naptan to the venue

            if (destinationLocation is SJPVenueLocation)
            {
                SJPVenueLocation venue = (SJPVenueLocation)destinationLocation;

                if ((venue.AccessibleNaptans != null) && (venue.AccessibleNaptans.Count > 0))
                {
                    // If no journey legs and accessible journey, then assume caller intentionally wants the accessible leg added
                    if ((journeyLegs.Count > 0) || (accessible))
                    {
                        #region Set location and datetime

                        // Get the end location (from the last journey leg or it is the origin location)
                        SJPLocation endLocation = (journeyLegs.Count > 0) ?
                            journeyLegs[journeyLegs.Count - 1].LegEnd.Location :
                            originLocation;

                        // Request time is the end time of the last journey leg
                        if (journeyLegs.Count > 0)
                            requestTime = journeyLegs[journeyLegs.Count - 1].EndTime;

                        #endregion

                        // Identify if the end location naptan is the same as venue accessible naptan
                        foreach (string endNaptan in endLocation.Naptan)
                        {
                            // Journey ends at the accessible naptan, add the accessible journey leg
                            SJPVenueAccess venueAccess = null;
                            SJPVenueAccessStation venueAccessStation = null;
                                                                                    
                            GetVenueAccess(venue, endNaptan, requestTime, true, out venueAccess, out venueAccessStation);

                            // Compare naptans to make sure they're the same,
                            // or if only SJPVenueAccess found, then allow leg to be added (this is to fix an issue where
                            // CJP result does not end at the accessible NaPTAN, but close to it, e.g. the station entrance)
                            if ((venue.AccessibleNaptans.Exists(delegate(string s) { return s == endNaptan; }))
                                || (venueAccess != null))
                            {
                                #region Access leg

                                if (venueAccessStation != null)
                                {
                                    // Access station found, build leg

                                    // Legs to add
                                    JourneyLeg accessLeg = null;

                                    // Set the start for the access leg (assume starts at the end location of last journey leg)
                                    SJPLocation accessStartLocation = endLocation;

                                    // Set the end for the access leg (assume this is the venue destination)
                                    SJPLocation accessEndLocation = destinationLocation;

                                    #region Amend leg times

                                    // Amend time
                                    DateTime requestStartTime = DateTime.MinValue;
                                    DateTime requestEndTime = DateTime.MinValue;

                                    // No journey legs, then need to set the start time to be the request time passed in
                                    if (journeyLegs.Count == 0)
                                        requestStartTime = requestTime;

                                    // End time is the request time plus the transfer duration
                                    requestEndTime = requestTime.Add(venueAccess.AccessToVenueDuration);

                                    #endregion

                                    // If this is the first/last leg, include a depart/arrive time
                                    accessLeg = JourneyLeg.Create(SJPModeType.Transfer,
                                                        accessStartLocation, accessEndLocation,
                                                        requestStartTime, requestEndTime);

                                    accessLeg.JourneyDetails.Add(GetPublicJourneyDetail(venueAccess, venueAccessStation, SJPModeType.Transfer, language, true));

                                    // Add journey leg
                                    journeyLegs.Add(accessLeg);
                                    break;
                                }

                                #endregion
                            }
                        }
                    }
                }
            }

            #endregion

            #endregion

            #region Set accessible journey flag

            accessibleJourney = accessible;

            #endregion

            #region Set routing detail

            if (cjpPublicJourney != null)
            {
                // SJP currently doesnt do anything useful with this information (just outputs for debug)
                routingDetail = new RoutingDetail(cjpPublicJourney.routingRuleIDs, cjpPublicJourney.routingReasons, cjpPublicJourney.routingStops);
            }

            #endregion

            // SJP is not interested in Grid References for a public journey

            // SJP is not interested in fares, pricing units, or routing guide sections for a public journey
        }

        /// <summary>
        /// Constructor for adding a CJP PrivateJourney (road journey)
        /// </summary>
        public Journey(int journeyId, ICJP.PrivateJourney cjpPrivateJourney, bool outward,
            SJPLocation originLocation, SJPLocation destinationLocation,
            DateTime requestTime, bool arriveBefore, Language language, bool accessible)
        {
            this.journeyId = journeyId;

            #region Set JourneyDate

            this.journeyDate = DateTimeHelper.GetDateTime(requestTime, DateTimeHelper.DateTimeType.Date);

            #endregion

            #region Setup journey legs

            SJPLocation startLocation = originLocation;
            SJPLocation endLocation = destinationLocation;

            #endregion

            # region Setup accessible flag for Blue Badge Journeys
            accessibleJourney = accessible;
            #endregion

            #region Update locations if car parks are involved in journey

            // A car journey may have been planned to or from a venue car park. If so, then journey locations 
            // are updated when constructing the journey because "transit to venue" legs are manually added
            SJPVenueCarPark originCarPark = GetCarParkForVenueLocation(originLocation);
            SJPVenueCarPark destinationCarPark = GetCarParkForVenueLocation(destinationLocation);

            // Because the car journey may have started from a car park time slot, check for this and use that
            // as the request time instead
            if (outward)
            {
                requestTime = GetVenueLocationDateTime(requestTime, outward, false, originLocation, destinationLocation);
            }

            // Update destination location to be the car park for the car journey leg
            if (outward && destinationCarPark != null)
            {
                endLocation = new SJPLocation();
                endLocation.ID = destinationCarPark.ID;
                endLocation.Name = destinationCarPark.Name + " Parking";
                endLocation.DisplayName = endLocation.Name;
            }
            // Update origin location to be the car park for the car journey leg
            if (!outward && originCarPark != null)
            {
                startLocation = new SJPLocation();
                startLocation.ID = originCarPark.ID;
                startLocation.Name = originCarPark.Name + " Parking";
                startLocation.DisplayName = startLocation.Name;
            }

            #endregion

            #region Add Return journey transit legs before car journey leg

            // For return journey, must add the legs to get from Venue to the Car park (where the car journey starts)
            // This includes adding where data available:
            //      1) venue to venue gate
            //      2) transit shuttle from venue gate
            //      3) transit shutttle to car park
            if ((!outward) && (originCarPark != null))
            {
                if (originCarPark.TransitShuttles != null)
                {
                    // Legs to add
                    JourneyLeg carParkInterchangeLeg = null;
                    JourneyLeg shuttleJourneyLeg = null;
                    JourneyLeg gateJourneyLeg = null;

                    SJPLocation shuttleStartLocation = null;
                    SJPLocation shuttleEndLocation = null;

                    bool validReturnShuttleFound = false;

                    foreach (TransitShuttle shuttle in originCarPark.TransitShuttles)
                    {
                        // Find shuttle from venue to car park
                        if (!outward && !shuttle.ToVenue)
                        {
                            bool dateValid = false;
                            bool timeValid = false;
                            
                            if (shuttle.Availability != null)
                            {
                                foreach (SJPParkAvailability availability in shuttle.Availability)
                                {
                                    List<DayOfWeek> daysValid = availability.GetDaysOfWeek();

                                    dateValid = IsDateValid(daysValid, availability.FromDate, availability.ToDate, requestTime);
                                    timeValid = IsTimeValid(availability.DailyOpeningTime, availability.DailyClosingTime, requestTime);

                                    if (dateValid && timeValid)
                                    {
                                        // Only add leg if it is not PRM only or request is blue badge
                                        if (AccessibleJourney || !shuttle.IsPRMOnly)
                                        {
                                            #region Set shuttle leg details

                                            // Set the end for a shuttle leg
                                            shuttleEndLocation = new SJPLocation();
                                            if (shuttle.ModeOfTransport == ParkingInterchangeMode.Shuttlebus)
                                            {
                                                shuttleEndLocation.Name = originCarPark.Name + " bus stop";
                                            }
                                            else
                                            {
                                                shuttleEndLocation.Name = originCarPark.Name;
                                            }
                                            shuttleEndLocation.DisplayName = shuttleEndLocation.Name;

                                            // Set the start for a shuttle leg (assume this in the venue origin)
                                            shuttleStartLocation = originLocation;

                                            #endregion

                                            #region Set gate leg details

                                            // Get the venue gate and path details for the transit shuttle
                                            SJPVenueGate gate = GetVenueGate(shuttle.VenueGateToUse);
                                            SJPVenueGateCheckConstraint gateCheckConstraint = GetVenueGateCheckConstraint(gate, false);
                                            SJPVenueGateNavigationPath gateNavigationPath = GetVenueGateNavigationForVenueLocation(originLocation, gate, false);

                                            // If gate details exists, update the shuttle leg to start at the gate
                                            if (gate != null)
                                            {
                                                // Shuttle leg now starts at the gate
                                                shuttleStartLocation = new SJPLocation();
                                                shuttleStartLocation.Name = gate.GateName;
                                                shuttleStartLocation.DisplayName = gate.GateName;
                                                shuttleStartLocation.GridRef = gate.GateGridRef;
                                                shuttleStartLocation.Naptan.Add(gate.GateNaPTAN);
                                            }

                                            #endregion

                                            #region Gate leg

                                            // If a venue to gate is required
                                            if (gate != null)
                                            {
                                                // Set the duration to be navigation path + check constraint times
                                                int durationMins = (gateNavigationPath != null) ?
                                                    Convert.ToInt32(gateNavigationPath.TransferDuration.TotalMinutes) : 0;

                                                durationMins = durationMins +
                                                    ((gateCheckConstraint != null) ?
                                                     Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalMinutes) : 0);

                                                // Create gate leg
                                                gateJourneyLeg = JourneyLeg.Create(SJPModeType.Walk,
                                                    originLocation, shuttleStartLocation,
                                                    requestTime, durationMins, false);

                                                gateJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(gate, gateCheckConstraint, gateNavigationPath));

                                                // Update the requestTime to be the end of gate leg
                                                requestTime = gateJourneyLeg.EndTime;
                                            }

                                            #endregion

                                            #region Interchange and Shuttle leg

                                            // Track the shuttle duration in case the interchange is merged in to the shuttle leg
                                            int shuttleDuration = shuttle.TransitDuration;

                                            // If an interchange is required, insert this after the shuttle
                                            if (originCarPark.InterchangeDuration > 0)
                                            {
                                                // Only disply a seperate interchange if mode is Rail Metro or Shuttlebus
                                                if (shuttle.ModeOfTransport == ParkingInterchangeMode.Shuttlebus || shuttle.ModeOfTransport == ParkingInterchangeMode.Rail
                                                    || shuttle.ModeOfTransport == ParkingInterchangeMode.Metro)
                                                {
                                                    // This is not the first or last leg, so don't have to include a depart/arrive time
                                                    carParkInterchangeLeg = JourneyLeg.Create(SJPModeType.WalkInterchange,
                                                        shuttleEndLocation, startLocation,
                                                        DateTime.MinValue, DateTime.MinValue);

                                                    carParkInterchangeLeg.JourneyDetails.Add(GetPublicJourneyDetail(originCarPark.InterchangeDuration));

                                                }
                                                else
                                                {
                                                    // Add the interchange time to the shuttle time as we will not be displaying the interchange
                                                    shuttleDuration += originCarPark.InterchangeDuration;

                                                    // Update shuttle end location to be start of the car leg
                                                    shuttleEndLocation.Name = startLocation.Name;
                                                    shuttleEndLocation.DisplayName = startLocation.DisplayName;
                                                }
                                            }

                                            // Create shuttle leg
                                            SJPModeType shuttleMode = SJPModeTypeHelper.GetSJPModeType(shuttle.ModeOfTransport);
                                            shuttleJourneyLeg = JourneyLeg.Create(
                                                shuttleMode,
                                                shuttleStartLocation, shuttleEndLocation,
                                                (shuttleMode != SJPModeType.Walk) ? requestTime : DateTime.MinValue,
                                                shuttleDuration, false);
                                            
                                            shuttleJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(shuttleDuration, shuttle, shuttleJourneyLeg.Mode, language));

                                            // Update the requestTime to be the end of shuttle leg
                                            if (shuttleJourneyLeg.Mode != SJPModeType.Walk)
                                            {
                                                requestTime = shuttleJourneyLeg.EndTime;
                                            }
                                            else
                                            {
                                                // If the shuttle mode is walk, then need to update the request time 
                                                // because the walk leg will have no actual start/end time, just a duration
                                                requestTime = requestTime.Add(shuttleJourneyLeg.Duration);
                                            }
                                                                                        
                                            #endregion

                                            #region Add journey legs

                                            // Add legs
                                            if (gateJourneyLeg != null)
                                                journeyLegs.Add(gateJourneyLeg);
                                            if (shuttleJourneyLeg != null)
                                                journeyLegs.Add(shuttleJourneyLeg);
                                            if (carParkInterchangeLeg != null)
                                                journeyLegs.Add(carParkInterchangeLeg);

                                            #endregion
                                        }

                                        validReturnShuttleFound = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (validReturnShuttleFound)
                        {
                            break;
                        }
                    }
                }
            }

            #endregion

            #region Add Car journey leg

            // The cjpPrivateJourney gets converted into one JourneyLeg
            JourneyLeg carJourneyLeg = JourneyLeg.Create(cjpPrivateJourney, startLocation, endLocation, requestTime, arriveBefore);

            // Populate the road journey details
            carJourneyLeg.PopulateJourneyDetails(cjpPrivateJourney.sections,
                outward ? destinationCarPark : originCarPark, language);

            // Add to the journey legs list
            journeyLegs.Add(carJourneyLeg);

            #endregion

            #region Add Outward journey transit legs after car journey leg

            // For outward journey, must add the legs to get from the Car park (where the car journey finishes) to Venue 
            // This includes adding where data available:
            //      1) car park to transit shuttle
            //      2) transit shuttle to venue gate
            //      3) venue gate to venue 
            if ((outward) && (destinationCarPark != null))
            {
                if (destinationCarPark.TransitShuttles != null)
                {
                    // Legs to add
                    JourneyLeg carParkInterchangeLeg = null;
                    JourneyLeg shuttleJourneyLeg = null;
                    JourneyLeg gateJourneyLeg = null;

                    SJPLocation shuttleStartLocation = null;
                    SJPLocation shuttleEndLocation = null;

                    // Request time is now the end time of the car journey leg
                    requestTime = carJourneyLeg.EndTime;

                    bool validOutwardShuttleFound = false;
                                                            
                    foreach (TransitShuttle shuttle in destinationCarPark.TransitShuttles)
                    {
                        // Find shuttle from car park to venue
                        if (outward && shuttle.ToVenue)
                        {
                            bool dateValid = false;
                            bool timeValid = false;

                            if (shuttle.Availability != null)
                            {

                                foreach (SJPParkAvailability availability in shuttle.Availability)
                                {
                                    List<DayOfWeek> daysValid = availability.GetDaysOfWeek();

                                    dateValid = IsDateValid(daysValid, availability.FromDate, availability.ToDate, requestTime);
                                    timeValid = IsTimeValid(availability.DailyOpeningTime, availability.DailyClosingTime, requestTime);

                                    if (dateValid && timeValid)
                                    {
                                        // Only add leg if it is not PRM only or request is blue badge
                                        if (AccessibleJourney || !shuttle.IsPRMOnly)
                                        {
                                            #region Set shuttle leg details

                                            // Set the start for a shuttle leg
                                            shuttleStartLocation = new SJPLocation();
                                            if (shuttle.ModeOfTransport == ParkingInterchangeMode.Shuttlebus)
                                            {
                                                shuttleStartLocation.Name = destinationCarPark.Name + " bus stop";
                                            }
                                            else
                                            {
                                                shuttleStartLocation.Name = destinationCarPark.Name;
                                            }
                                            shuttleStartLocation.DisplayName = shuttleStartLocation.Name;

                                            // Set the end for a shuttle leg (assume this in the venue destination)
                                            shuttleEndLocation = destinationLocation;

                                            #endregion

                                            #region Set gate leg details

                                            // Get the venue gate and path details for the transit shuttle
                                            SJPVenueGate gate = GetVenueGate(shuttle.VenueGateToUse);
                                            SJPVenueGateCheckConstraint gateCheckConstraint = GetVenueGateCheckConstraint(gate, true);
                                            SJPVenueGateNavigationPath gateNavigationPath = GetVenueGateNavigationForVenueLocation(destinationLocation, gate, true);

                                            // If gate details exists, update the shuttle leg to end at the gate
                                            if (gate != null)
                                            {
                                                // Shuttle leg now ends at the gate
                                                shuttleEndLocation = new SJPLocation();
                                                shuttleEndLocation.Name = gate.GateName;
                                                shuttleEndLocation.DisplayName = gate.GateName;
                                                shuttleEndLocation.GridRef = gate.GateGridRef;
                                                shuttleEndLocation.Naptan.Add(gate.GateNaPTAN);
                                            }

                                            #endregion

                                            #region Interchange and shuttle legs

                                            // Track the shuttle duration in case the interchange is merged in to the shuttle leg
                                            int shuttleDuration = shuttle.TransitDuration;

                                            // If an interchange to shuttle is required
                                            if (destinationCarPark.InterchangeDuration > 0)
                                            {
                                                // Only disply a seperate interchange if mode is Rail Metro or Shuttlebus
                                                if (shuttle.ModeOfTransport == ParkingInterchangeMode.Shuttlebus || shuttle.ModeOfTransport == ParkingInterchangeMode.Rail
                                                    || shuttle.ModeOfTransport == ParkingInterchangeMode.Metro)
                                                {
                                                    // This is not the first or last leg, so don't have to include a depart/arrive time
                                                    carParkInterchangeLeg = JourneyLeg.Create(SJPModeType.WalkInterchange,
                                                        endLocation, shuttleStartLocation,
                                                        DateTime.MinValue, DateTime.MinValue);

                                                    carParkInterchangeLeg.JourneyDetails.Add(GetPublicJourneyDetail(destinationCarPark.InterchangeDuration));

                                                    // Check for an interchange to shuttle, and amend time
                                                    requestTime = requestTime.Add(new TimeSpan(0, destinationCarPark.InterchangeDuration, 0));
                                                }
                                                else
                                                {
                                                    // Add the interchange time to the shuttle time as we will not be displaying the interchange
                                                    shuttleDuration += destinationCarPark.InterchangeDuration;

                                                    // Update shuttle start location to be end of the car leg
                                                    shuttleStartLocation.Name = endLocation.Name; ;
                                                    shuttleStartLocation.DisplayName = endLocation.DisplayName;
                                                }
                                            }

                                            // Update the requestTime to be at least the first shuttle service of the day
                                            if (requestTime.TimeOfDay < shuttle.ServiceStartTime)
                                            {
                                                requestTime = new DateTime(requestTime.Year, requestTime.Month, requestTime.Day);
                                                requestTime = requestTime.Add(shuttle.ServiceStartTime);
                                            }

                                            // Create shuttle leg
                                            SJPModeType shuttleMode = SJPModeTypeHelper.GetSJPModeType(shuttle.ModeOfTransport);
                                            shuttleJourneyLeg = JourneyLeg.Create(
                                                shuttleMode,
                                                shuttleStartLocation, shuttleEndLocation,
                                                (shuttleMode != SJPModeType.Walk) ? requestTime : DateTime.MinValue,
                                                shuttleDuration, false);

                                            shuttleJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(shuttleDuration, shuttle, shuttleJourneyLeg.Mode, language));

                                            #endregion

                                            #region Gate leg

                                            // If a gate to venue is required
                                            if (gate != null)
                                            {
                                                // Update the requestTime to be the end of shuttle leg
                                                if (shuttleJourneyLeg.Mode != SJPModeType.Walk)
                                                {
                                                    requestTime = shuttleJourneyLeg.EndTime;
                                                }
                                                else
                                                {
                                                    // If the shuttle mode is walk, then need to update the request time 
                                                    // because the walk leg will have no actual start/end time, just a duration
                                                    requestTime = requestTime.Add(shuttleJourneyLeg.Duration);
                                                }

                                                // Set the duration to be navigation path + check constraint times
                                                int durationMins = (gateNavigationPath != null) ?
                                                    Convert.ToInt32(gateNavigationPath.TransferDuration.TotalMinutes) : 0;

                                                durationMins = durationMins +
                                                    ((gateCheckConstraint != null) ?
                                                     Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalMinutes) : 0);

                                                // Create gate leg
                                                gateJourneyLeg = JourneyLeg.Create(SJPModeType.Walk,
                                                    shuttleEndLocation, destinationLocation,
                                                    requestTime, durationMins, false);

                                                gateJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(gate, gateCheckConstraint, gateNavigationPath));
                                            }

                                            #endregion

                                            #region Add journey legs

                                            // Add legs
                                            if (carParkInterchangeLeg != null)
                                                journeyLegs.Add(carParkInterchangeLeg);
                                            if (shuttleJourneyLeg != null)
                                                journeyLegs.Add(shuttleJourneyLeg);
                                            if (gateJourneyLeg != null)
                                                journeyLegs.Add(gateJourneyLeg);

                                            #endregion

                                            validOutwardShuttleFound = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (validOutwardShuttleFound)
                        {
                            break;
                        }
                    }
                }
            }

            #endregion

            #endregion
        }
                
        /// <summary>
        /// Constructor for adding a CTP Journey (cycle journey)
        /// </summary>
        public Journey(int journeyId, CPWS.Journey ctpJourney, bool outward,
            SJPLocation originLocation, SJPLocation destinationLocation,
            DateTime requestTime, bool arriveBefore)
        {
            this.journeyId = journeyId;

            #region Set JourneyDate

            this.journeyDate = DateTimeHelper.GetDateTime(requestTime, DateTimeHelper.DateTimeType.Date);

            #endregion

            #region Setup journey legs

            SJPLocation startLocation = originLocation;
            SJPLocation endLocation = destinationLocation;

            #region Update locations if cycle parks are involved in journey

            // A cycle journey may have been planned to or from a venue cycle park. If so, then journey locations 
            // are updated when constructing the journey because "transit to venue" legs are manually added
            SJPVenueCyclePark originCyclePark = GetCycleParkForVenueLocation(originLocation);
            SJPVenueCyclePark destinationCyclePark = GetCycleParkForVenueLocation(destinationLocation);

            // Because the cycle journey may have started from an amended cycle park time, check for this and use that
            // as the request time instead
            if (outward)
            {
                requestTime = GetVenueLocationDateTime(requestTime, outward, false, originLocation, destinationLocation);
            }

            // Update destination location to be the cycle park for the cycle journey leg
            if (outward && destinationCyclePark != null)
            {
                endLocation = new SJPLocation();
                endLocation.ID = destinationCyclePark.ID;
                endLocation.Name = destinationCyclePark.Name;
                endLocation.DisplayName = destinationCyclePark.Name;
                endLocation.GridRef = destinationCyclePark.CycleToGridReference;
            }
            // Update origin location to be the cycle park for the cycle journey leg
            // For SJPMobile, an outward journey can be from Venue (cycle park) to non-venue 
            if (originCyclePark != null)
            {
                startLocation = new SJPLocation();
                startLocation.ID = originCyclePark.ID;
                startLocation.Name = originCyclePark.Name;
                startLocation.DisplayName = originCyclePark.Name;
                startLocation.GridRef = originCyclePark.CycleFromGridReference;
            }

            #endregion

            #region Add journey transit legs before cycle journey leg

            // For journey starting at cycle park, must add the legs to get from Venue to the Cycle park 
            // (where the cycle journey starts)
            // This includes adding where data available:
            //      1) venue to venue gate
            //      2) venue gate to cycle park
            if (originCyclePark != null)
            {
                // Legs to add
                JourneyLeg cycleParkInterchangeLeg = null;
                JourneyLeg gateJourneyLeg = null;

                SJPLocation interchangeStartLocation = null;
                SJPLocation interchangeEndLocation = null;
                SJPLocation gateEndLocation = null;

                #region Set interchange leg details

                // Set the start for the interchange leg (assume starts at venue origin)
                interchangeStartLocation = originLocation;

                // Set the end for the interchange leg (assume this in the cycle park)
                interchangeEndLocation = startLocation;

                #endregion

                #region Set gate leg details

                // Set the end for the gate leg (assume ends at cycle park)
                gateEndLocation = startLocation;
                
                // Get the venue gate and path details for cycle park, use Exit naptan if it exists
                SJPVenueGate gate = GetVenueGate(!string.IsNullOrEmpty(originCyclePark.VenueGateExitNaPTAN) 
                    ? originCyclePark.VenueGateExitNaPTAN : originCyclePark.VenueGateEntranceNaPTAN);
                SJPVenueGateCheckConstraint gateCheckConstraint = GetVenueGateCheckConstraint(gate, false);
                SJPVenueGateNavigationPath gateNavigationPath = GetVenueGateNavigationForVenueLocation(originLocation, gate, false);

                // If gate details exists, update the end of gate leg to be the gate
                if (gate != null)
                {
                    // Gate leg now ends at the gate
                    gateEndLocation = new SJPLocation();
                    gateEndLocation.Name = gate.GateName;
                    gateEndLocation.DisplayName = gate.GateName;
                    gateEndLocation.GridRef = gate.GateGridRef;
                    gateEndLocation.Naptan.Add(gate.GateNaPTAN);

                    // Interchange leg now starts at the gate
                    interchangeStartLocation = gateEndLocation;
                }

                #endregion

                #region Gate leg

                // If a gate to venue is required
                if (gate != null)
                {
                    // Set the duration to be navigation path + check constraint times
                    int durationMins = (gateNavigationPath != null) ?
                        Convert.ToInt32(gateNavigationPath.TransferDuration.TotalMinutes) : 0;

                    durationMins = durationMins +
                        ((gateCheckConstraint != null) ?
                         Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalMinutes) : 0);

                    // Create gate leg
                    gateJourneyLeg = JourneyLeg.Create(SJPModeType.Walk,
                        originLocation, gateEndLocation,
                        requestTime, durationMins, false);

                    gateJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(gate, gateCheckConstraint, gateNavigationPath));

                    // Update the requestTime to be the end of gate leg
                    requestTime = gateJourneyLeg.EndTime;
                }

                #endregion

                #region Interchange leg

                // Use Walk From gate if available, otherwise use Walk To
                TimeSpan duration = (originCyclePark.WalkFromGateDuration.TotalMinutes > 0) ?
                    originCyclePark.WalkFromGateDuration : originCyclePark.WalkToGateDuration;

                // Check for an interchange from venue, and add leg
                if (duration.TotalMinutes > 0)
                {
                    // If this is the first or last leg, we have to include a depart/arrive time
                    DateTime startTime = DateTime.MinValue;
                    if (gate == null)
                    {
                        startTime = requestTime;
                    }

                    cycleParkInterchangeLeg = JourneyLeg.Create(SJPModeType.Walk,
                                    interchangeStartLocation, interchangeEndLocation,
                                    startTime, DateTime.MinValue);

                    cycleParkInterchangeLeg.JourneyDetails.Add(GetPublicJourneyDetail(Convert.ToInt32(duration.TotalMinutes)));

                    // Request time is now updated with the interchange journey duration
                    requestTime = requestTime.Add(duration);
                }

                #endregion

                #region Add journey legs

                // Add legs
                if (gateJourneyLeg != null)
                    journeyLegs.Add(gateJourneyLeg);
                if (cycleParkInterchangeLeg != null)
                    journeyLegs.Add(cycleParkInterchangeLeg);
                
                #endregion
            }

            #endregion

            #region Add Cycle journey leg

            // The ctpJourney gets converted into one JourneyLeg
            JourneyLeg cycleJourneyLeg = JourneyLeg.Create(ctpJourney, startLocation, endLocation, requestTime, arriveBefore);

            // Populate the cycle journey details
            cycleJourneyLeg.PopulateJourneyDetails(ctpJourney.sections);

            // Add to the journey legs list
            journeyLegs.Add(cycleJourneyLeg);

            // Check if cycle journey leg arrives/departs when the cycle park is closed
            if (originCyclePark != null)
            {
                // Cycle leg departing cycle park
                if (!originCyclePark.IsOpenForDateAndTime(cycleJourneyLeg.StartTime, true))
                {
                    // Get opening time to display in error
                    TimeSpan tsOpeningTime = originCyclePark.GetAvailabilityTimeForDate(cycleJourneyLeg.StartTime, false);
                    DateTime dtOpeningTime = new DateTime(cycleJourneyLeg.StartTime.Year, cycleJourneyLeg.StartTime.Month, cycleJourneyLeg.StartTime.Day, 
                        tsOpeningTime.Hours, tsOpeningTime.Minutes, 0);

                    messages.Add(new SJPMessage(string.Empty, JourneyControl.Messages.SJPCycleParkClosedForJourneyLeave,
                        SJP.Common.ResourceManager.SJPResourceManager.COLLECTION_JOURNEY,
                        SJP.Common.ResourceManager.SJPResourceManager.GROUP_JOURNEYOUTPUT,
                        new List<string>() { originCyclePark.Name, dtOpeningTime.ToString("HH:mm") },
                        0, 0, SJPMessageType.Info
                        ));
                }
            }
            else if (destinationCyclePark != null)
            {
                // Cycle leg arriving at cycle park
                if (!destinationCyclePark.IsOpenForDateAndTime(cycleJourneyLeg.EndTime, true))
                {
                    // Get closing time to display in error
                    TimeSpan tsClosingTime = destinationCyclePark.GetAvailabilityTimeForDate(cycleJourneyLeg.EndTime, true);
                    DateTime dtClosingTime = new DateTime(cycleJourneyLeg.EndTime.Year, cycleJourneyLeg.EndTime.Month, cycleJourneyLeg.EndTime.Day, 
                        tsClosingTime.Hours, tsClosingTime.Minutes, 0);
                    
                    messages.Add(new SJPMessage(string.Empty, JourneyControl.Messages.SJPCycleParkClosedForJourneyArrive,
                        SJP.Common.ResourceManager.SJPResourceManager.COLLECTION_JOURNEY,
                        SJP.Common.ResourceManager.SJPResourceManager.GROUP_JOURNEYOUTPUT,
                        new List<string>() { destinationCyclePark.Name, dtClosingTime.ToString("HH:mm") },
                        0, 0, SJPMessageType.Info
                        ));
                }
            }
            #endregion

            #region Add Outward journey transit legs after cycle journey leg

            // For outward journey, must add the legs to get from the Cycle park (where the cycle journey finishes) to Venue 
            // This includes adding where data available:
            //      1) cycle park to venue gate
            //      2) venue gate to venue
            if ((outward) && (destinationCyclePark != null))
            {
                // Legs to add
                JourneyLeg cycleParkInterchangeLeg = null;
                JourneyLeg gateJourneyLeg = null;

                SJPLocation interchangeStartLocation = null;
                SJPLocation interchangeEndLocation = null;
                SJPLocation gateStartLocation = null;

                // Request time is now the end time of the cycle journey leg
                requestTime = cycleJourneyLeg.EndTime;

                #region Set interchange leg details

                // Set the start for the interchange leg (assume starts at cycle park)
                interchangeStartLocation = endLocation;

                // Set the end for the interchange leg (assume this is the venue destination)
                interchangeEndLocation = destinationLocation;

                #endregion

                #region Set gate leg details

                // Set the start for the gate leg (assume starts at cycle park)
                gateStartLocation = endLocation;
                
                // Get the venue gate and path details for cycle park, use Entrance naptan
                SJPVenueGate gate = GetVenueGate(destinationCyclePark.VenueGateEntranceNaPTAN);
                SJPVenueGateCheckConstraint gateCheckConstraint = GetVenueGateCheckConstraint(gate, true);
                SJPVenueGateNavigationPath gateNavigationPath = GetVenueGateNavigationForVenueLocation(destinationLocation, gate, true);

                // If gate details exists, update the start of gate leg to be the gate
                if (gate != null)
                {
                    // Gate leg now starts at the gate
                    gateStartLocation = new SJPLocation();
                    gateStartLocation.Name = gate.GateName;
                    gateStartLocation.DisplayName = gate.GateName;
                    gateStartLocation.GridRef = gate.GateGridRef;
                    gateStartLocation.Naptan.Add(gate.GateNaPTAN);

                    // Interchange leg now ends at the gate
                    interchangeEndLocation = gateStartLocation;
                }

                #endregion

                #region Interchange leg

                // Check for an interchange to gate, and add leg
                if (destinationCyclePark.WalkToGateDuration.TotalMinutes > 0)
                {
                    // Check for an interchange to gate, and amend time, use Walk To gate duration
                    requestTime = requestTime.Add(destinationCyclePark.WalkToGateDuration);

                    // If This is not the first or last leg, so don't have to include a depart/arrive time
                    // If the gate is null this will be the first or last leg, in which case include a depart/arrive time
                    cycleParkInterchangeLeg = JourneyLeg.Create(SJPModeType.Walk,
                                    interchangeStartLocation, interchangeEndLocation,
                                    DateTime.MinValue, gate!=null ? DateTime.MinValue : requestTime);

                    cycleParkInterchangeLeg.JourneyDetails.Add(GetPublicJourneyDetail(Convert.ToInt32(destinationCyclePark.WalkToGateDuration.TotalMinutes)));

                    
                }

                #endregion

                #region Gate leg

                // If a gate to venue is required
                if (gate != null)
                {
                    // Set the duration to be navigation path + check constraint times
                    int durationMins = (gateNavigationPath != null) ?
                        Convert.ToInt32(gateNavigationPath.TransferDuration.TotalMinutes) : 0;

                    durationMins = durationMins +
                        ((gateCheckConstraint != null) ?
                         Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalMinutes) : 0);

                    // Create gate leg
                    gateJourneyLeg = JourneyLeg.Create(SJPModeType.Walk,
                        gateStartLocation, destinationLocation,
                        requestTime, durationMins, false);

                    gateJourneyLeg.JourneyDetails.Add(GetPublicJourneyDetail(gate, gateCheckConstraint, gateNavigationPath));
                }

                #endregion

                #region Add journey legs

                // Add legs
                if (cycleParkInterchangeLeg != null)
                    journeyLegs.Add(cycleParkInterchangeLeg);
                if (gateJourneyLeg != null)
                    journeyLegs.Add(gateJourneyLeg);

                #endregion
            }

            #endregion

            #endregion
        }

        /// <summary>
        /// Constructor for creating a journey using a CJP StopEvent
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="cjpStopEvent"></param>
        public Journey(int journeyId, ICJP.StopEvent cjpStopEvent,
            SJPLocation originLocation, SJPLocation destinationLocation)
        {
            this.journeyId = journeyId;

            #region Set JourneyDate

            DateTime stopEventDateTime = DateTime.Now;

            // Find date using the stop details
            if (cjpStopEvent.stop != null)
            {
                if ((cjpStopEvent.stop.arriveTime != null) && (cjpStopEvent.stop.arriveTime != DateTime.MinValue))
                {
                    stopEventDateTime = cjpStopEvent.stop.arriveTime;
                }
                else if ((cjpStopEvent.stop.departTime != null) && (cjpStopEvent.stop.departTime != DateTime.MinValue))
                {
                    stopEventDateTime = cjpStopEvent.stop.departTime;
                }
            }

            this.journeyDate = DateTimeHelper.GetDateTime(stopEventDateTime, DateTimeHelper.DateTimeType.Date);

            #endregion

            #region Setup journey legs

            // The cjpStopEvent gets converted into one JourneyLeg

            // Create the JourneyLeg
            JourneyLeg journeyLeg = JourneyLeg.Create(cjpStopEvent, originLocation, destinationLocation);

            // Populate the stop event journey details
            journeyLeg.PopulateJourneyDetails(cjpStopEvent, originLocation, destinationLocation);

            // Add to the journey legs list
            journeyLegs.Add(journeyLeg);
                        
            #endregion
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all the modes used in this journey
        /// </summary>
        /// <returns></returns>
        public SJPModeType[] GetUsedModes()
        {
            List<SJPModeType> modes = new List<SJPModeType>();
            
            SJPModeType mode = SJPModeType.Unknown;

            foreach (JourneyLeg leg in journeyLegs)
            {
                mode = leg.Mode;

                // Check for cable car mode override
                if (JourneyLeg.IsLegModeCableCar(leg))
                {
                    modes.Add(SJPModeType.CableCar);
                }
                // Mode for leg
                else if (!modes.Contains(leg.Mode))
                {
                    modes.Add(leg.Mode);
                }
                
                // Check for queue mode for any legs which contain a Check Constraint
                if (!modes.Contains(SJPModeType.Queue) && JourneyLeg.IsLegModeQueue(leg))
                {
                    modes.Add(SJPModeType.Queue);
                }
            }
            
            return modes.ToArray();
        }

        /// <summary>
        /// Returns true if the modes contains Car
        /// </summary>
        /// <returns></returns>
        public bool IsCarJourney()
        {
            List<SJPModeType> modes = new List<SJPModeType>(GetUsedModes());

            if (modes.Contains(SJPModeType.Car))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the modes contains Cycle
        /// </summary>
        /// <returns></returns>
        public bool IsCycleJourney()
        {
            List<SJPModeType> modes = new List<SJPModeType>(GetUsedModes());

            if (modes.Contains(SJPModeType.Cycle))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Public properties
                
        /// <summary>
        /// Read/Write. Unique JourneyId to identify this journey
        /// </summary>
        public int JourneyId
        {
            get { return journeyId; }
            set { journeyId = value; }
        }

        /// <summary>
        /// Read/Write. JourneyLegs that represents all of the legs of this journey,
        /// these are sorted in order
        /// </summary>
        public List<JourneyLeg> JourneyLegs
        {
            get { return journeyLegs; }
            set { journeyLegs = value; }
        }

        /// <summary>
        /// Read/Write. Date for the journey
        /// </summary>
        /// <remarks></remarks>
        public DateTime JourneyDate
        {
            get { return journeyDate; }
            set { journeyDate = value; }
        }

        /// <summary>
        /// Read only. Departure time for the journey
        /// </summary>
        public DateTime StartTime
        {
            get 
            {
                if (journeyLegs.Count == 0)
                    return DateTime.MinValue;
                else
                    return journeyLegs[0].StartTime;
            }
        }

        /// <summary>
        /// Read only. Arrival time for the journey
        /// </summary>
        public DateTime EndTime
        {
            get 
            {
                if (journeyLegs.Count == 0)
                    return DateTime.MinValue;
                else
                    return journeyLegs[journeyLegs.Count - 1].EndTime; 
            }
        }

        /// <summary>
        /// Read/Write. Duration of the journey, using the First leg and Last leg 
        /// of the journey
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                CalculateDuration();

                return duration;
            }
            set { duration = value; }
        }

        /// <summary>
        /// Read/Write. Number of changes within the journey
        /// </summary>
        public int InterchangeCount
        {
            get
            {
                CalculateInterchangeCount();

                return interchangeCount;
            }
            set { interchangeCount = value; }
        }

        /// <summary>
        /// Read/Write. Routing details used within the journey
        /// </summary>
        public RoutingDetail JourneyRoutingDetail
        {
            get { return routingDetail; }
            set { routingDetail = value; }
        }

        /// <summary>
        /// Read/Write. Indicates if this is an accessible journey
        /// </summary>
        public bool AccessibleJourney
        {
            get { return accessibleJourney; }
            set { accessibleJourney = value; }
        }

        /// <summary>
        /// Read/Write. Indicates if this is a valid journey, default is true.
        /// Currently only used for River Services planner mode
        /// </summary>
        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        /// <summary>
        /// Read/Write. Messages added by the journey during the create. 
        /// Should be shown in the UI
        /// </summary>
        public List<SJPMessage> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Calculates the journey duration
        /// </summary>
        private void CalculateDuration()
        {
            if (duration != new TimeSpan(0))
                return;

            if (journeyLegs.Count == 0)
                return;

            // Get the depart and arrival times for this journey
            DateTime departureTime = DateTimeHelper.GetDateTime(journeyLegs[0].StartTime, DateTimeHelper.DateTimeType.DateTimeMillisecond);
            DateTime arrivalTime = DateTimeHelper.GetDateTime(journeyLegs[journeyLegs.Count - 1].EndTime, DateTimeHelper.DateTimeType.DateTimeMillisecond);
                        
            // Find the difference between the two times, and save in class property
            duration = arrivalTime.Subtract(departureTime);
        }

        /// <summary>
        /// Returns the number of interchanges in the given journey.
        /// </summary>
        /// <param name="journey">Journey to find the number for interchanges for.</param>
        /// <returns>Number of interchanges</returns>
        private void CalculateInterchangeCount()
        {
            if (interchangeCount >= 0)
                return;

            int numberOfLegs = 0;

            // Find the number of legs, ignoring modes not considered an interchange (e.g. walk)
            foreach (JourneyLeg leg in journeyLegs)
            {
                if (IsVehicleUsed(leg))
                    numberOfLegs++;
            }

            int finalCount = numberOfLegs - 1;

            // Set the class property value, if negative count set to 0
            interchangeCount = (finalCount < 0) ? 0 : finalCount;
        }

        /// <summary>
        /// Determines if a vehicle was used in given leg
        /// </summary>
        /// <returns>True if a vehicle was used, false otherwise.</returns>
        private bool IsVehicleUsed(JourneyLeg leg)
        {
            return (leg.Mode != SJPModeType.Walk);
        }

        /// <summary>
        /// Checks if the date supplied is valid for From and To date range and DaysOfWeek list
        /// </summary>
        private bool IsDateValid(List<DayOfWeek> daysValid, DateTime fromDate, DateTime toDate, DateTime journeyDate)
        {
            bool valid = false;

            // Outward and return journey check
            if (fromDate.Date <= journeyDate.Date &&
                toDate.Date >= journeyDate.Date)
            {
                if (daysValid.Contains(journeyDate.DayOfWeek))
                {
                    valid = true;
                }
            }

            return valid;
        }

        /// <summary>
        /// Checks if the Time supplied is valid for Start and End Times
        /// </summary>
        private bool IsTimeValid(TimeSpan starttime, TimeSpan endtime, DateTime journeyTime)
        {
            bool valid = false;

            if (starttime.Hours <= journeyTime.Hour && endtime.Hours >= journeyTime.Hour)
            {
                valid = true;

                if (starttime.Hours == journeyTime.Hour)
                {
                    if (starttime.Minutes > journeyTime.Minute)
                    {
                        valid = false;
                    }
                }

                if (endtime.Hours == journeyTime.Hour)
                {
                    if (endtime.Minutes < journeyTime.Minute)
                    {
                        valid = false;
                    }
                }
            }

            return valid;
        }

        #region Methods used to manually add transit legs

        /// <summary>
        /// Returns an SJPVenueCarPark for the SJPLocation if available
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private SJPVenueCarPark GetCarParkForVenueLocation(SJPLocation location)
        {
            SJPVenueCarPark carPark = null;

            if (location is SJPVenueLocation)
            {
                SJPVenueLocation venueLocation = (SJPVenueLocation)location;

                if (!string.IsNullOrEmpty(venueLocation.SelectedSJPParkID))
                {
                    LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                    carPark = locationService.GetSJPVenueCarPark(venueLocation.SelectedSJPParkID);
                }
            }

            return carPark;
        }

        /// <summary>
        /// Returns an SJPVenueCyclePark for the SJPLocation if available
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private SJPVenueCyclePark GetCycleParkForVenueLocation(SJPLocation location)
        {
            SJPVenueCyclePark cyclePark = null;

            if (location is SJPVenueLocation)
            {
                SJPVenueLocation venueLocation = (SJPVenueLocation)location;

                if (!string.IsNullOrEmpty(venueLocation.SelectedSJPParkID))
                {
                    LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                    cyclePark = locationService.GetSJPVenueCyclePark(venueLocation.SelectedSJPParkID);
                }
            }

            return cyclePark;
        }

        /// <summary>
        /// Returns the Venue pier navigation path for the venue location provided if available
        /// </summary>
        /// <param name="venueLocation">Venue location to find navigation path for</param>
        /// <param name="venuePierLocation">Venue pier location to find navigation path to/from</param>
        private SJPPierVenueNavigationPath GetPierNavigationForVenueLocation(SJPLocation venueLocation, SJPLocation venuePierLocation, bool isToVenue)
        {
            SJPPierVenueNavigationPath navigationPath = null;

            // Check it is a venue location, otherwise no point looking
            if (venueLocation is SJPVenueLocation)
            {
                SJPVenueLocation venue = (SJPVenueLocation)venueLocation;

                // Get the navigation paths for venue
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                navigationPath = locationService.GetSJPVenuePierNavigationPaths(venue.Naptan, venuePierLocation.Naptan[0], isToVenue);
            }

            return navigationPath;
        }

        /// <summary>
        /// Returns an SJPVenueGate for the gate naptan if available
        /// </summary>
        private SJPVenueGate GetVenueGate(string venueGateNaptan)
        {
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            return locationService.GetSJPVenueGate(venueGateNaptan);
        }

        /// <summary>
        /// Returns an SJPVenueGateCheckConstraint for the SJPVenueGate if available
        /// </summary>
        private SJPVenueGateCheckConstraint GetVenueGateCheckConstraint(SJPVenueGate venueGate, bool isEntry)
        {
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
            
            return locationService.GetSJPVenueGateCheckConstraints(venueGate, isEntry);
        }

        /// <summary>
        /// Returns an SJPVenueGateNavigationPath for the SJPLocation from/to SJPVenueGate if available
        /// </summary>
        private SJPVenueGateNavigationPath GetVenueGateNavigationForVenueLocation(SJPLocation location, SJPVenueGate venueGate, bool isToVenue)
        {
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            return locationService.GetSJPVenueGateNavigationPaths(location, venueGate, isToVenue);
        }

        /// <summary>
        /// Returns the date time for the Venue location if it has one set,
        /// but only for the Outward journey End location or Return journey Start location
        /// (as these are currently the ones only set for a Car/Cycle park or some Accessible journeys),
        /// otherwise returns the requestTime passed
        /// </summary>
        private DateTime GetVenueLocationDateTime(DateTime requestTime, bool outward, bool accessible,
            SJPLocation startLocation, SJPLocation endLocation)
        {
            DateTime dateTime = requestTime;

            #region Use Venue location date time

            // For outward journey, use the End Venue location outward date time
            if ((outward) && (endLocation is SJPVenueLocation))
            {
                SJPVenueLocation endVenue = (SJPVenueLocation)endLocation;

                if (((!string.IsNullOrEmpty(endVenue.SelectedSJPParkID)) || (accessible))
                    && (endVenue.SelectedOutwardDateTime != DateTime.MinValue))
                {
                    dateTime = endVenue.SelectedOutwardDateTime;
                }
            }
            // For return journey, use the Start Venue location return date time
            else if ((!outward) && (startLocation is SJPVenueLocation))
            {
                SJPVenueLocation startVenue = (SJPVenueLocation)startLocation;

                if (((!string.IsNullOrEmpty(startVenue.SelectedSJPParkID)) || (accessible))
                    && (startVenue.SelectedReturnDateTime != DateTime.MinValue))
                {
                    dateTime = startVenue.SelectedReturnDateTime;
                }
            }

            #endregion

            return dateTime;
        }

        /// <summary>
        /// Populates the SJPVenueAccess and SJPVenueAccessStation for the venue and station naptan
        /// </summary>
        private void GetVenueAccess(SJPVenueLocation venue, string stationNaptan, DateTime datetime, bool isToVenue,
            out SJPVenueAccess venueAccess, out SJPVenueAccessStation venueAccessStation)
        {
            // Return values
            venueAccess = null;
            venueAccessStation = null;

            if (!string.IsNullOrEmpty(stationNaptan))
            {
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                List<SJPVenueAccess> venueAccessList = locationService.GetSJPVenueAccessData(venue.Naptan, datetime);

                if (venueAccessList != null)
                {
                    foreach (SJPVenueAccess va in venueAccessList)
                    {
                        if (va.Stations != null)
                        {
                            // Where there is only one station, then return it. 
                            // This is to overcome a problem where the CJP returns a journey not exactly at the accessible station
                            // NaPTAN - but cannot assume it is the correct one if there are multiple stations
                            if (va.Stations.Count == 1)
                            {
                                venueAccess = va;
                                venueAccessStation = va.Stations[0];
                            }
                            else
                            {
                                foreach (SJPVenueAccessStation vas in va.Stations)
                                {
                                    if (vas.StationNaPTAN.ToLower() == stationNaptan.ToLower())
                                    {
                                        // Station found exit
                                        venueAccess = va;
                                        venueAccessStation = vas;
                                        break;
                                    }
                                }
                            }
                        }

                        // Access and Station found exit
                        if (venueAccessStation != null)
                        {
                            break;
                        }
                    }
                }
            }
        }

        #region Get PublicJourneyDetail

        /// <summary>
        /// Returns a PublicJourneyDetail built using an SJP TransitShuttle
        /// </summary>
        /// <param name="shuttle"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private PublicJourneyDetail GetPublicJourneyDetail(int durationMins, TransitShuttle shuttle, SJPModeType mode, Language language)
        {
            // Use durationMins if supplied (>= 0), otherwise the shuttle duration
            durationMins = (durationMins >= 0) ?
                    durationMins : shuttle.TransitDuration; // In Mins
            int durationSecs = durationMins * 60; // In secs

            if (mode == SJPModeType.Walk
                || mode == SJPModeType.Cycle)
            {
                // Shuttles are a "continuous" leg for Walk
                PublicJourneyContinuousDetail pjcd = new PublicJourneyContinuousDetail();

                pjcd.Duration = durationSecs; // In secs
                pjcd.Mode = mode;

                string transferNote = shuttle.GetTransferText(language);

                if (!string.IsNullOrEmpty(transferNote))
                {
                    pjcd.DisplayNotes.Add(transferNote);
                }

                return pjcd;
            }
            else
            {
                // Shuttles are a "frequency" leg for Bus and Rail
                PublicJourneyFrequencyDetail pjfd = new PublicJourneyFrequencyDetail();
                pjfd.Frequency = shuttle.ServiceFrequency;
                pjfd.MinFrequency = shuttle.ServiceFrequency;
                pjfd.MaxDuration = durationMins;
                pjfd.TypicalDuration = durationMins;
                pjfd.Duration = durationSecs; // In Secs
                pjfd.Mode = mode;

                if (shuttle.ServiceStartTime > TimeSpan.MinValue)
                {
                    StringBuilder start = new StringBuilder();
                    if (shuttle.ServiceStartTime.Hours < 10)
                        start.Append("0");
                    start.Append(shuttle.ServiceStartTime.Hours.ToString());
                    start.Append(":");
                    if (shuttle.ServiceStartTime.Minutes < 10)
                        start.Append("0");
                    start.Append(shuttle.ServiceStartTime.Minutes.ToString());

                    StringBuilder end = new StringBuilder();
                    if (shuttle.ServiceEndTime.Hours < 10)
                        end.Append("0");
                    end.Append(shuttle.ServiceEndTime.Hours.ToString());
                    end.Append(":");
                    if (shuttle.ServiceEndTime.Minutes < 10)
                        end.Append("0");
                    end.Append(shuttle.ServiceEndTime.Minutes.ToString());

                    string vehicle = string.Empty;
                    switch (mode)
                    {
                        case SJPModeType.TransitShuttleBus:
                            vehicle = "shuttle bus";
                            break;
                        case SJPModeType.TransitRail:
                            vehicle = "rail service";
                            break;
                        case SJPModeType.Metro:
                            vehicle = "metro service";
                            break;
                        default:
                            vehicle = mode.ToString().ToLower();
                            break;
                    }
                    string note1 = string.Format("A {0} departs every {1} minutes, first service at {2} and last service at {3}",
                        vehicle,
                        shuttle.ServiceFrequency,
                        start.ToString(),
                        end.ToString());
                    pjfd.DisplayNotes.Add(note1);
                }

                string transferNote = shuttle.GetTransferText(language);

                if (!string.IsNullOrEmpty(transferNote))
                {
                    pjfd.DisplayNotes.Add(transferNote);
                }

                return pjfd;
            }
        }

        /// <summary>
        /// Returns a PublicJourneyDetail built for a duration
        /// </summary>
        /// <param name="shuttle"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private PublicJourneyDetail GetPublicJourneyDetail(int durationMinutes)
        {
            PublicJourneyContinuousDetail pjcd = new PublicJourneyContinuousDetail();

            pjcd.Mode = SJPModeType.Walk;
            pjcd.Duration = durationMinutes * 60; // In secs

            return pjcd;
        }

        /// <summary>
        /// Returns a PublicJourneyDetail built using an SJP VenueGate details
        /// </summary>
        /// <returns></returns>
        private PublicJourneyDetail GetPublicJourneyDetail(SJPVenueGate gate, SJPVenueGateCheckConstraint gateCheckConstraint, SJPVenueGateNavigationPath gateNavigationPath)
        {
            // Gates are a "interchange" legs
            PublicJourneyInterchangeDetail pjid = new PublicJourneyInterchangeDetail();

            pjid.Mode = SJPModeType.Walk;

            if (gateNavigationPath != null)
            {
                // Duration of gate to venue path
                
                // Set the duration to be navigation path + check constraint times
                int durationSecs = (gateNavigationPath != null) ?
                    Convert.ToInt32(gateNavigationPath.TransferDuration.TotalSeconds) : 0;

                durationSecs = durationSecs +
                    ((gateCheckConstraint != null) ?
                     Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalSeconds) : 0);

                pjid.Duration = durationSecs;
            }

            if (gateCheckConstraint != null)
            {
                // Create a CJP check constraint so UI handles correctly
                ICJP.CheckConstraint checkConstraint = new ICJP.CheckConstraint();
                checkConstraint.name = gateCheckConstraint.CheckConstraintName;
                checkConstraint.averageDelay = Convert.ToInt32(gateCheckConstraint.AverageDelay.TotalMinutes);
                checkConstraint.checkProcess = SJPCheckConstraintHelper.GetCJPCheckProcess(gateCheckConstraint.Process);
                checkConstraint.congestion = SJPCheckConstraintHelper.GetCJPCongestionReason(gateCheckConstraint.Congestion);

                pjid.CheckConstraints.Add(checkConstraint);
            }

            return pjid;
        }

        /// <summary>
        /// Returns a PublicJourneyDetail built using an SJP VenueAccess and VenueAccessStation details
        /// </summary>
        /// <returns></returns>
        private PublicJourneyDetail GetPublicJourneyDetail(SJPVenueAccess venueAccess, SJPVenueAccessStation venueAccessStation, 
            SJPModeType mode, Language language, bool isToVenue)
        {
            // VenueAccess are "continuos" legs
            PublicJourneyContinuousDetail pjcd = new PublicJourneyContinuousDetail();

            pjcd.Mode = mode;

            if (venueAccess != null)
            {
                // In secs
                pjcd.Duration = Convert.ToInt32(venueAccess.AccessToVenueDuration.TotalSeconds);
            }

            if (venueAccessStation != null) 
            {
                string transferText = venueAccessStation.GetTransferText(language, isToVenue);

                if (!string.IsNullOrEmpty(transferText))
                {
                    pjcd.DisplayNotes.Add(transferText);
                }
            }

            return pjcd;
        }

        #endregion

        #endregion

        #endregion
    }
}
