// *********************************************** 
// NAME             : SJPJourneyRequest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 Mar 2011
// DESCRIPTION  	: SJPJourneyRequest class
// ************************************************
// 


using System;
using System.Collections.Generic;
using System.Text;
using SJP.Common;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// SJPJourneyRequest class
    /// </summary>
    [Serializable()]
    public class SJPJourneyRequest : ISJPJourneyRequest
    {
        #region Private members

        /// <summary>
        /// Hash of only the SJPJourneyRequest properties used in journey planning.
        /// e.g. location, dates, times, but not request id
        /// Used when comparing two SJPJourneyRequest objects.
        /// </summary>
        private string journeyRequestHash = string.Empty;
        private bool journeyRequestSubmitted = false;

        // Locations
        private SJPLocation origin = null;
        private SJPLocation destination = null;
        private SJPLocation returnOrigin = null;
        private SJPLocation returnDestination = null;
        private string locationInputMode = string.Empty;

        // Datetimes
        private DateTime outwardDateTime;
        private DateTime returnDateTime;
        private bool outwardArriveBefore;
        private bool returnArriveBefore;
        private bool isOutwardRequired = true;
        private bool isReturnRequired;
        private bool isReturnOnly;

        // Modes
        private SJPJourneyPlannerMode plannerMode = SJPJourneyPlannerMode.PublicTransport;
        private List<SJPModeType> modes = new List<SJPModeType>();

        // Public specific
        private SJPPublicAlgorithmType publicAlgorithm;
        private int sequence;
        private int interchangeSpeed;
        private int walkingSpeed;
        private int maxWalkingTime;
        private int maxWalkingDistance;
        private bool routingGuideInfluenced;
        private bool routingGuideCompliantJourneysOnly;
        private string routeCodes = string.Empty;
        private bool olympicRequest;
        private string travelDemandPlanOutward = string.Empty;
        private string travelDemandPlanReturn = string.Empty;
        private bool filteringStrict = false;
        private bool dontForceCoach = false;
        private bool removeAwkwardOvernight = false;
        private SJPAccessiblePreferences accessiblePreferences = null;

        // Car specific
        private bool avoidMotorways;
        private bool avoidFerries;
        private bool avoidTolls;
        private List<string> avoidRoads = new List<string>();
        private List<string> includeRoads = new List<string>();
        private SJPPrivateAlgorithmType privateAlgorithm;
        private int drivingSpeed;
        private bool doNotUseMotorways;
        private string fuelConsumption = string.Empty;
        private string fuelPrice = string.Empty;

        // Cycle specific
        private string cycleAlgorithm;
        private string penaltyFunction;
        private List<SJPUserPreference> userPreferences = new List<SJPUserPreference>();

        // Journey parts
        private Journey outwardJourneyPart;
        private Journey returnJourneyPart;

        // Replan
        private bool isReplan = false;

        // Replan - Datetimes
        private DateTime replanOutwardDateTime = DateTime.MinValue;
        private DateTime replanReturnDateTime = DateTime.MinValue;
        private bool replanOutwardArriveBefore = true;
        private bool replanReturnArriveBefore = false;
        private bool replanIsOutwardRequired = true;
        private bool replanIsReturnRequired = false;
        
        // Replan - Journeys to retain
        private List<Journey> replanOutwardJourneys = null;
        private List<Journey> replanReturnJourneys = null;
        private bool replanRetainOutwardJourneys = false;
        private bool replanRetainReturnJourneys = false;
        private bool replanRetainOutwardJourneysWhenNoResults = false;
        private bool replanRetainReturnJourneysWhenNoResults = false;

        // Replan - Journey paging
        private string replanJourneyRequestHashEarlier;
        private string replanJourneyRequestHashLater;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPJourneyRequest()
        {
        }

        #endregion
                
        #region Public properties

        /// <summary>
        /// Read/Write. The JourneyRequest hash value
        /// </summary>
        public string JourneyRequestHash
        {
            get { return journeyRequestHash; }
            set { journeyRequestHash = value; }
        }

        /// <summary>
        /// Read/Write. Journey request submitted value.
        /// Indicates if this SJPJourneyRequest was submitted to the journey planner.
        /// This value must be set to true when it has been submitted, as it is used with
        /// the landing page functionality to determine if it is considered as a "new" landing 
        /// page request
        /// </summary>
        public bool JourneyRequestSubmitted
        {
            get { return journeyRequestSubmitted; }
            set { journeyRequestSubmitted = value; }
        }

        #region Locations

        /// <summary>
        /// Read/Write. Origin Location for the journey request
        /// </summary>
        public SJPLocation Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        /// <summary>
        /// Read/Write. Destination Location for the journey request
        /// </summary>
        public SJPLocation Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        /// <summary>
        /// Read/Write. Return Origin Location for the journey request (if different from the Outward Destination)
        /// </summary>
        public SJPLocation ReturnOrigin
        {
            get
            {
                // returnOrigin was explicitly set, so use that
                if (returnOrigin != null)
                {
                    return returnOrigin;
                }
                // this is a return only journey, so the origin is used as the "origin of the return journey",
                // because of the way the input page allows entering of the "return only journey" locations
                else if ((isReturnOnly) && (isReturnRequired))
                {
                    return origin;
                }
                // else the destination is used as the "origin of the return journey"
                else
                {
                    return destination;
                }
            }
            set { returnOrigin = value; }
        }

        /// <summary>
        /// Read/Write. Return Destination Location for the journey request (if different from the Outward Origin)
        /// </summary>
        public SJPLocation ReturnDestination 
        {
            get
            {
                // returnDestination was explicitly set, so use that
                if (returnDestination != null)
                {
                    return returnDestination;
                }
                // this is a return only journey, so the destination is used as the "destination of the return journey",
                // because of the way the input page allows entering of the "return only journey" locations
                else if ((isReturnOnly) && (isReturnRequired))
                {
                    return destination;
                }
                // else the origin is used as the "destination of the return journey"
                else
                {
                    return origin;
                }
            }
            set { returnDestination = value; }
        }

        /// <summary>
        /// Read/Write. Location input mode
        /// </summary>
        public string LocationInputMode
        {
            get { return locationInputMode; }
            set { locationInputMode = value; }
        }

        #endregion

        #region Dates/times

        /// <summary>
        /// Read/Write. Outward DateTime
        /// </summary>
        public DateTime OutwardDateTime
        {
            get { return outwardDateTime; }
            set { outwardDateTime = value; }
        }

        /// <summary>
        /// Read/Write. Return DateTime
        /// </summary>
        public DateTime ReturnDateTime
        {
            get { return returnDateTime; }
            set { returnDateTime = value; }
        }
        
        /// <summary>
        /// Read/Write. Outward arrive before time flag
        /// </summary>
        public bool OutwardArriveBefore
        {
            get { return outwardArriveBefore; }
            set { outwardArriveBefore = value; }
        }

        /// <summary>
        /// Read/Write. Return arrive before time flag
        /// </summary>
        public bool ReturnArriveBefore
        {
            get { return returnArriveBefore; }
            set { returnArriveBefore = value; }
        }

        /// <summary>
        /// Read/Write. Indicates if outward journey is required
        /// </summary>
        public bool IsOutwardRequired
        {
            get { return isOutwardRequired; }
            set { isOutwardRequired = value; }
        }

        /// <summary>
        /// Read/Write. Indicates if return journey is required
        /// </summary>
        public bool IsReturnRequired
        {
            get { return isReturnRequired; }
            set { isReturnRequired = value; }
        }

        /// <summary>
        /// Read/Write. Indicates if this request must be treated as a return journey only, IsReturnRequired must be set to true.
        /// When true, then the Origin and Destination are treated as the ReturnOrigin and ReturnDestination respectively
        /// </summary>
        public bool IsReturnOnly
        {
            get { return isReturnOnly; }
            set { isReturnOnly = value; }
        }

        #endregion

        #region Modes

        /// <summary>
        /// Read/Write. Planner Mode of the journey request
        /// </summary>
        public SJPJourneyPlannerMode PlannerMode 
        {
            get { return plannerMode; }
            set { plannerMode = value; }
        }

        /// <summary>
        /// Read/Write. Modes to submit to the journey planner
        /// </summary>
        public List<SJPModeType> Modes 
        {
            get { return modes; }
            set { modes = value; }
        }

        #endregion

        #region Public specific

        /// <summary>
        /// Read/Write. Public journey type to plan
        /// </summary>
        public SJPPublicAlgorithmType PublicAlgorithm
        {
            get { return publicAlgorithm; }
            set { publicAlgorithm = value; }
        }

        /// <summary>
        /// Read/Write. Number of journeys to find
        /// </summary>
        public int Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }

        /// <summary>
        /// Read/Write. Interchange speed (int [-3 to 3]), corresponds to the walking speed
        /// </summary>
        /// <example>0 = normal</example>
        public int InterchangeSpeed
        {
            get { return interchangeSpeed; }
            set { interchangeSpeed = value; }
        }

        /// <summary>
        /// Read/Write. Walking speed (metres per minute [<=46 to >=117]), also used to calculate max walk distance
        /// </summary>
        /// <example>80 = normal</example>
        public int WalkingSpeed
        {
            get { return walkingSpeed; }
            set { walkingSpeed = value; }
        }

        /// <summary>
        /// Read/Write. Max walking time (minutes), used to calculate max walk distance if set
        /// </summary>
        public int MaxWalkingTime
        {
            get { return maxWalkingTime; }
            set { maxWalkingTime = value; }
        }

        /// <summary>
        /// Read/Write. Max walking distance (metres), used in preference to max walking time if set
        /// </summary>
        public int MaxWalkingDistance
        {
            get { return maxWalkingDistance; }
            set { maxWalkingDistance = value; }
        }

        /// <summary>
        /// Read/Write. Routing guide influenced
        /// </summary>
        public bool RoutingGuideInfluenced
        {
            get { return routingGuideInfluenced; }
            set { routingGuideInfluenced = value; }
        }

        /// <summary>
        /// Read/Write. Routing guide compliant journeys only
        /// </summary>
        public bool RoutingGuideCompliantJourneysOnly
        {
            get { return routingGuideCompliantJourneysOnly; }
            set { routingGuideCompliantJourneysOnly = value; }
        }

        /// <summary>
        /// Read/Write. Route codes
        /// </summary>
        public string RouteCodes
        {
            get { return routeCodes; }
            set { routeCodes = value; }
        }

        /// <summary>
        /// Read/Write. Indicates the journey planner should perform olympic games journey planning
        /// </summary>
        public bool OlympicRequest
        {
            get { return olympicRequest; }
            set { olympicRequest = value; }
        }
        
        /// <summary>
        /// Read/Write. Influences the validation performed by the journey planner - For Outward journey
        /// </summary>
        public string TravelDemandPlanOutward
        {
            get { return travelDemandPlanOutward; }
            set { travelDemandPlanOutward = value; }
        }

        /// <summary>
        /// Read/Write. Influences the validation performed by the journey planner - For Return journey
        /// </summary>
        public string TravelDemandPlanReturn
        {
            get { return travelDemandPlanReturn; }
            set { travelDemandPlanReturn = value; }
        }

        /// <summary>
        /// Read/Write.
        /// If true, then journey planners will be Strict when planning a route using accessibility options. 
        /// If false, then journey planner can return a non-compliant route if compliant not found
        /// </summary>
        public bool FilteringStrict
        {
            get { return filteringStrict; }
            set { filteringStrict = value; }
        }

        /// <summary>
        /// If true, then even if force coach value is on in the CJP settings, do not perform force coach. 
        /// If false, then the force coach value in the CJP settings is used
        /// </summary>
        public bool DontForceCoach
        {
            get { return dontForceCoach; }
            set { dontForceCoach = value; }
        }

        /// <summary>
        /// Read/Write. Remove Awkwad Overnight Journeys flag
        /// 
        /// </summary>
        public bool RemoveAwkwardOvernight
        {
            get { return removeAwkwardOvernight; }
            set { removeAwkwardOvernight = value; }
        }



        /// <summary>
        /// Read/Write. Accessible option preferences to be passed to the journey planner
        /// </summary>
        public SJPAccessiblePreferences AccessiblePreferences
        {
            get { return accessiblePreferences; }
            set { accessiblePreferences = value; }
        }

        #endregion

        #region Car specific

        /// <summary>
        /// Read/Write. Avoid motorways
        /// </summary>
        public bool AvoidMotorways 
        { 
            get { return avoidMotorways; }
            set { avoidMotorways = value; }
        }

        /// <summary>
        /// Read/Write. Avoid ferries
        /// </summary>
        public bool AvoidFerries 
        { 
            get { return avoidFerries; }
            set { avoidFerries = value; }
        }

        /// <summary>
        /// Read/Write. Avoid tolls
        /// </summary>
        public bool AvoidTolls 
        { 
            get { return avoidTolls; }
            set { avoidTolls = value; }
        }

        /// <summary>
        /// Read/Write. List of roads to avoid
        /// </summary>
        public List<string> AvoidRoads 
        { 
            get { return avoidRoads; }
            set { avoidRoads = value; }
        }

        /// <summary>
        /// Read/Write. List of roads to include
        /// </summary>
        public List<string> IncludeRoads 
        { 
            get { return includeRoads; }
            set { includeRoads = value; }
        }

        /// <summary>
        /// Read/Write. Private (road) journey type to plan
        /// </summary>
        public SJPPrivateAlgorithmType PrivateAlgorithm 
        { 
            get { return privateAlgorithm; }
            set { privateAlgorithm = value; }
        }

        /// <summary>
        /// Read/Write. Maximum driving speed to use (km per hour)
        /// </summary>
        /// <example>112</example>
        public int DrivingSpeed 
        { 
            get { return drivingSpeed; }
            set { drivingSpeed = value; }
        }

        /// <summary>
        /// Read/Write. Do not use motorways at all
        /// </summary>
        public bool DoNotUseMotorways 
        { 
            get { return doNotUseMotorways; }
            set { doNotUseMotorways = value; }
        }

        /// <summary>
        /// Read/Write. Fuel consumption value to use (usage of fuel in metres per litre)
        /// </summary>
        /// <example>13807</example>
        public string FuelConsumption 
        { 
            get { return fuelConsumption; }
            set { fuelConsumption = value; }
        }

        /// <summary>
        /// Read/Write. Fuel price value to use (10ths of pence per litre)
        /// </summary>
        /// <example>1150</example>
        public string FuelPrice 
        { 
            get { return fuelPrice; }
            set { fuelPrice = value; }
        }

        #endregion

        #region Cycle specific

        /// <summary>
        /// Read/write. Cycle algorithm used to identify the penality function used by the cycle planner
        /// </summary>
        public string CycleAlgorithm
        {
            get { return cycleAlgorithm; }
            set { cycleAlgorithm = value; }
        }

        /// <summary>
        /// Read/write. Penalty function used by the cycle planner.
        /// </summary>
        /// <example>
        /// Call C:\CyclePlanner\td.cp.CyclePenaltyFunctions.v2.dll, TransportDirect.JourneyPlanning.CyclePenaltyFunctions.QuietestV912
        /// </example>
        public string PenaltyFunction
        {
            get { return penaltyFunction; }
            set { penaltyFunction = value; }
        }

        /// <summary>
        /// Read/Write. Journey parameters in an SJPUserPreference key/value pairs array used by the cycle planner
        /// </summary>
        /// <remarks>
        /// This is a list of preferences that are used in the request sent to the Atkins CTP.
        /// Keys are (int)0 to (int)14, with values as defined in the CTP interface documentation.
        /// </remarks>
        public List<SJPUserPreference> UserPreferences 
        {
            get { return userPreferences; }
            set { userPreferences = value; }
        }

        #endregion

        #region Journey Parts

        /// <summary>
        /// Read/Write. Outward journey containing legs to add to the journeys planned for this request
        /// </summary>
        public Journey OutwardJourneyPart 
        {
            get { return outwardJourneyPart; }
            set { outwardJourneyPart = value; }
        }

        /// <summary>
        /// Read/Write. Return journey containing legs to add to the journeys planned for this request
        /// </summary>
        public Journey ReturnJourneyPart 
        {
            get { return returnJourneyPart; }
            set { returnJourneyPart = value; } 
        }

        #endregion

        #region Replan

        /// <summary>
        /// Read/Write. Replan flag, indicates to journey planner to perform a replan and to use replan values
        /// </summary>
        public bool IsReplan
        {
            get { return isReplan; }
            set { isReplan = value; }
        }
        
        #region Replan - Datetimes

        /// <summary>
        /// Read/Write. Replan Outward DateTime
        /// </summary>
        public DateTime ReplanOutwardDateTime 
        {
            get { return replanOutwardDateTime; }
            set { replanOutwardDateTime = value; } 
        }

        /// <summary>
        /// Read/Write. Replan Return DateTime
        /// </summary>
        public DateTime ReplanReturnDateTime
        {
            get { return replanReturnDateTime; }
            set { replanReturnDateTime = value; }
        }

        /// <summary>
        /// Read/Write. Replan Outward arrive before time flag
        /// </summary>
        public bool ReplanOutwardArriveBefore
        {
            get { return replanOutwardArriveBefore; }
            set { replanOutwardArriveBefore = value; }
        }

        /// <summary>
        /// Read/Write. Replan Return arrive before time flag
        /// </summary>
        public bool ReplanReturnArriveBefore
        {
            get { return replanReturnArriveBefore; }
            set { replanReturnArriveBefore = value; }
        }

        /// <summary>
        /// Read/Write. Replan Indicates if outward journey is required
        /// </summary>
        public bool ReplanIsOutwardRequired
        {
            get { return replanIsOutwardRequired; }
            set { replanIsOutwardRequired = value; }
        }

        /// <summary>
        /// Read/Write. Replan Indicates if return journey is required
        /// </summary>
        public bool ReplanIsReturnRequired
        {
            get { return replanIsReturnRequired; }
            set { replanIsReturnRequired = value; }
        }

        #endregion

        #region Replan - Journeys

        /// <summary>
        /// Read/Write. Replan Outward Journeys to add to the result
        /// </summary>
        public List<Journey> ReplanOutwardJourneys  
        {
            get { return replanOutwardJourneys; }
            set { replanOutwardJourneys = value; }
        }

        /// <summary>
        /// Read/Write. Replan Return Journeys to add to the result
        /// </summary>
        public List<Journey> ReplanReturnJourneys
        {
            get { return replanReturnJourneys; }
            set { replanReturnJourneys = value; }
        }

        /// <summary>
        /// RetainOutwardJourneys flag, indicates to journey planner to retain the outward journeys if supplied
        /// </summary>
        public bool ReplanRetainOutwardJourneys
        {
            get { return replanRetainOutwardJourneys; }
            set { replanRetainOutwardJourneys = value; }
        }

        /// <summary>
        /// RetainReturnJourneys flag, indicates to journey planner to retain the return journeys if supplied
        /// </summary>
        public bool ReplanRetainReturnJourneys
        {
            get { return replanRetainReturnJourneys; }
            set { replanRetainReturnJourneys = value; }
        }

        /// <summary>
        /// RetainOutwardJourneysWhenNoResults flag, indicates to journey planner to retain the outward journeys if supplied,
        /// when the journey planner fails to plan a journey (e.g. to allow previous results to be retained).
        /// This flag onle becomes applicable if RetainOutwardJourneys is false
        /// </summary>
        public bool ReplanRetainOutwardJourneysWhenNoResults
        {
            get { return replanRetainOutwardJourneysWhenNoResults; }
            set { replanRetainOutwardJourneysWhenNoResults = value; }
        }

        /// <summary>
        /// RetainReturnJourneysWhenNoResults flag, indicates to journey planner to retain the return journeys if supplied,
        /// when the journey planner fails to plan a journey (e.g. to allow previous results to be retained).
        /// This flag onle becomes applicable if RetainReturnJourneys is false
        /// </summary>
        public bool ReplanRetainReturnJourneysWhenNoResults
        {
            get { return replanRetainReturnJourneysWhenNoResults; }
            set { replanRetainReturnJourneysWhenNoResults = value; }
        }

        #endregion

        #region Replan - Journey Paging

        /// <summary>
        /// Read/Write. Hash of the SJPJourneyRequest considered as the earlier journey request.
        /// </summary>
        public string ReplanJourneyRequestHashEarlier
        {
            get { return replanJourneyRequestHashEarlier; }
            set { replanJourneyRequestHashEarlier = value; }
        }

        /// <summary>
        /// Read/Write. Hash of the SJPJourneyRequest considered as the later journey request 
        /// </summary>
        public string ReplanJourneyRequestHashLater
        {
            get { return replanJourneyRequestHashLater; }
            set { replanJourneyRequestHashLater = value; }
        }
        
        #endregion

        #endregion

        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a hashcode of user changeable values
        /// </summary>
        /// <remarks>The hash returns does not include request id</remarks>
        /// <returns></returns>
        public int GetSJPHashCode()
        {
            int hashCode = 0;

            // Locations
            if (origin != null)
            {
                hashCode = origin.GetSJPHashCode(); 
            }
            if (destination != null)
            {
                hashCode = hashCode ^ destination.GetSJPHashCode();
            }
            if (returnOrigin != null)
            {
                hashCode = hashCode ^ returnOrigin.GetSJPHashCode();
            }
            if (returnDestination != null)
            {
                hashCode = hashCode ^ returnDestination.GetSJPHashCode();
            }
            if (!string.IsNullOrEmpty(locationInputMode))
            {
                hashCode = hashCode ^ locationInputMode.GetHashCode();
            }

            // Dates/times
            hashCode = hashCode ^ outwardArriveBefore.GetHashCode() ^ returnArriveBefore.GetHashCode() ^
                outwardDateTime.GetHashCode() ^ returnDateTime.GetHashCode();
            
            // Do + here because of the nature of true/false hashcodes and bitwise exclusive ^
            // Could be any value i think, as long as out and ret are different and don't cancel each other out
            int outretRequired = (isOutwardRequired) ? 7 : 0; 
            outretRequired += (isReturnRequired) ? 13 : 0;
            hashCode = hashCode ^ outretRequired;
            
            // Modes
            if (modes != null)
            {
                foreach (SJPModeType mode in modes)
                {
                    hashCode = hashCode ^ mode.GetHashCode();
                }
            }

            // Accessible preferences
            if (accessiblePreferences != null)
            {
                // Do + here because of the nature of true/false hashcodes and bitwise exclusive ^
                hashCode = hashCode ^ (filteringStrict.GetHashCode() + accessiblePreferences.GetSJPHashCode());
            }

            // Force coach (to allow testing because its a configurable property)
            hashCode = hashCode ^ dontForceCoach.GetHashCode();

            // Cycle penalty function algorithm
            if (!string.IsNullOrEmpty(penaltyFunction))
            {
                hashCode = hashCode ^ penaltyFunction.GetHashCode();
            }

            // Journey parts (for river service journey requests)
            if ((outwardJourneyPart != null) && (outwardJourneyPart.JourneyLegs != null) && (outwardJourneyPart.JourneyLegs.Count > 0))
            {
                hashCode = hashCode ^ outwardJourneyPart.GetHashCode();
            }

            if ((returnJourneyPart != null) && (returnJourneyPart.JourneyLegs != null) && (returnJourneyPart.JourneyLegs.Count > 0))
            {
                hashCode = hashCode ^ returnJourneyPart.GetHashCode();
            }

            // Replan - Dates/times
            hashCode = hashCode ^ isReplan.GetHashCode() ^ replanOutwardArriveBefore.GetHashCode() ^ replanReturnArriveBefore.GetHashCode() ^
                replanIsOutwardRequired.GetHashCode() ^ replanIsReturnRequired.GetHashCode() ^
                replanOutwardDateTime.GetHashCode() ^ replanReturnDateTime.GetHashCode();

            // Replan - Journeys
            if ((replanOutwardJourneys != null) && (replanOutwardJourneys.Count > 0))
            {
                hashCode = hashCode ^ replanOutwardJourneys.Count.GetHashCode();
            }
            if ((replanReturnJourneys != null) && (replanReturnJourneys.Count > 0))
            {
                hashCode = hashCode ^ replanReturnJourneys.Count.GetHashCode();
            }

            // Replan - Journey paging
            // Do not include replanJourneyRequestHashEarlier and replanJourneyRequestHashLater
            // in the hashcode as they do not alter this request, they are only pointers to 
            // other journey requests

            return hashCode;
        }

        /// <summary>
        /// Returns a formatted string representing the contents of this SJPJourneyRequest
        /// </summary>
        /// <returns></returns>
        public string ToString(bool htmlLineBreaks)
        {
            string linebreak = (htmlLineBreaks) ? "<br />" : "\r\n";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("journeyRequestHash: {0} {1}", journeyRequestHash, linebreak));
            sb.AppendLine(string.Format("journeyRequestSubmitted: {0} {1}", journeyRequestSubmitted, linebreak));
            sb.AppendLine(linebreak);

            #region Locations
            // Locations
            if (origin is SJPVenueLocation)
            {
                sb.AppendLine(string.Format("origin: {0} {1}", ((SJPVenueLocation)origin).ToString(htmlLineBreaks), linebreak));
            }
            else
            {
                sb.AppendLine(string.Format("origin: {0} {1}", origin.ToString(htmlLineBreaks), linebreak));
            }
            if (destination is SJPVenueLocation)
            {
                sb.AppendLine(string.Format("destination: {0} {1}", ((SJPVenueLocation)destination).ToString(htmlLineBreaks), linebreak));
            }
            else
            {
                sb.AppendLine(string.Format("destination: {0} {1}", destination.ToString(htmlLineBreaks), linebreak));
            }
            if (returnOrigin != null)
            {
                if (returnOrigin is SJPVenueLocation)
                {
                    sb.AppendLine(string.Format("returnOrigin: {0} {1}", ((SJPVenueLocation)returnOrigin).ToString(htmlLineBreaks), linebreak));
                }
                else
                {
                    sb.AppendLine(string.Format("returnOrigin: {0} {1}", returnOrigin.ToString(htmlLineBreaks), linebreak));
                }
            }
            if (returnDestination != null)
            {
                if (returnDestination is SJPVenueLocation)
                {
                    sb.AppendLine(string.Format("returnDestination: {0} {1}", ((SJPVenueLocation)returnDestination).ToString(htmlLineBreaks), linebreak));
                }
                else
                {
                    sb.AppendLine(string.Format("returnDestination: {0} {1}", returnDestination.ToString(htmlLineBreaks), linebreak));
                }
            }
            #endregion

            #region Datetimes
            // Datetimes
            sb.AppendLine(string.Format("outwardDateTime: {0} {1}", outwardDateTime.ToString(), linebreak));
            sb.AppendLine(string.Format("returnDateTime: {0} {1}", returnDateTime.ToString(), linebreak));
            sb.AppendLine(string.Format("outwardArriveBefore: {0} {1}", outwardArriveBefore.ToString(), linebreak));
            sb.AppendLine(string.Format("returnArriveBefore: {0} {1}", returnArriveBefore.ToString(), linebreak));
            sb.AppendLine(string.Format("isOutwardRequired: {0} {1}", isOutwardRequired.ToString(), linebreak));
            sb.AppendLine(string.Format("isReturnRequired: {0} {1}", isReturnRequired.ToString(), linebreak));
            sb.AppendLine(string.Format("isReturnOnly: {0} {1}", isReturnOnly.ToString(), linebreak));
            #endregion

            #region Modes
            // Modes
            sb.AppendLine(string.Format("plannerMode: {0} {1}", plannerMode.ToString(), linebreak));
            sb.Append("modes: ");
            foreach (SJPModeType mode in modes)
            {
                sb.Append(mode.ToString());
                sb.Append(" ");
            }
            sb.AppendLine(linebreak);
            sb.AppendLine(linebreak);
            #endregion

            #region Public specific
            // Public specific
            sb.AppendLine(string.Format("publicAlgorithm: {0} {1}", publicAlgorithm.ToString(), linebreak));
            sb.AppendLine(string.Format("sequence: {0} {1}", sequence.ToString(), linebreak));
            sb.AppendLine(string.Format("interchangeSpeed: {0} {1}", interchangeSpeed.ToString(), linebreak));
            sb.AppendLine(string.Format("walkingSpeed: {0} {1}", walkingSpeed.ToString(), linebreak));
            sb.AppendLine(string.Format("maxWalkingTime: {0} {1}", maxWalkingTime.ToString(), linebreak));
            sb.AppendLine(string.Format("maxWalkingDistance: {0} {1}", maxWalkingDistance.ToString(), linebreak));
            sb.AppendLine(string.Format("routingGuideInfluenced: {0} {1}", routingGuideInfluenced.ToString(), linebreak));
            sb.AppendLine(string.Format("routingGuideCompliantJourneysOnly: {0} {1}", routingGuideCompliantJourneysOnly.ToString(), linebreak));
            sb.AppendLine(string.Format("routeCodes: {0} {1}", routeCodes.ToString(), linebreak));
            sb.AppendLine(string.Format("olympicRequest: {0} {1}", olympicRequest.ToString(), linebreak));
            sb.AppendLine(string.Format("travelDemandPlanOutward: {0} {1}", travelDemandPlanOutward.ToString(), linebreak));
            sb.AppendLine(string.Format("travelDemandPlanReturn: {0} {1}", travelDemandPlanReturn.ToString(), linebreak));
            sb.AppendLine(string.Format("filteringStrict: {0} {1}", filteringStrict.ToString(), linebreak));
            sb.AppendLine(string.Format("dontForceCoach: {0} {1}", dontForceCoach.ToString(), linebreak));
            sb.Append("accessiblePreferences: ");
            sb.Append(string.Format("[accessible: {0}] ", accessiblePreferences.Accessible));
            sb.Append(string.Format("[doNotUseUnderground: {0}] ", accessiblePreferences.DoNotUseUnderground));
            sb.Append(string.Format("[requireSpecialAssistance: {0}] ", accessiblePreferences.RequireSpecialAssistance));
            sb.Append(string.Format("[requireStepFreeAccess: {0}] ", accessiblePreferences.RequireStepFreeAccess));
            sb.Append(string.Format("[fewerInterchanges: {0}]", accessiblePreferences.RequireFewerInterchanges));
            sb.AppendLine(linebreak);
            #endregion

            #region Car specific
            // Car specific
            sb.AppendLine(linebreak);
            sb.AppendLine(string.Format("avoidMotorways: {0} {1}", avoidMotorways.ToString(), linebreak));
            sb.AppendLine(string.Format("avoidFerries: {0} {1}", avoidFerries.ToString(), linebreak));
            sb.AppendLine(string.Format("avoidTolls: {0} {1}", avoidTolls.ToString(), linebreak));
            sb.Append("avoidRoads: ");
            foreach (string road in avoidRoads)
            {
                sb.Append(road);
                sb.Append(" ");
            }
            sb.AppendLine(linebreak);
            sb.Append("includeRoads: ");
            foreach (string road in includeRoads)
            {
                sb.Append(road);
                sb.Append(" ");
            }
            sb.AppendLine(linebreak);

            sb.AppendLine(string.Format("privateAlgorithm: {0} {1}", privateAlgorithm.ToString(), linebreak));
            sb.AppendLine(string.Format("drivingSpeed: {0} {1}", drivingSpeed.ToString(), linebreak));
            sb.AppendLine(string.Format("doNotUseMotorways: {0} {1}", doNotUseMotorways.ToString(), linebreak));
            sb.AppendLine(string.Format("fuelConsumption: {0} {1}", fuelConsumption.ToString(), linebreak));
            sb.AppendLine(string.Format("fuelPrice: {0} {1}", fuelPrice.ToString(), linebreak));
            sb.AppendLine(linebreak);
            #endregion

            #region Cycle specific
            // Cycle specific
            sb.AppendLine(string.Format("cycleAlgorithm: {0} {1}", cycleAlgorithm.ToString(), linebreak));
            sb.AppendLine(string.Format("penaltyFunction: {0} {1}", penaltyFunction.ToString(), linebreak));
            sb.Append("userPreferences: ");
            foreach (SJPUserPreference up in userPreferences)
            {
                sb.Append(string.Format("[{0},{1}] ", up.PreferenceKey, up.PreferenceValue));
            }
            sb.AppendLine(linebreak);
            #endregion

            #region Replan
            // Replan 
            sb.AppendLine(string.Format("isReplan: {0} {1}", isReplan.ToString(), linebreak));
            sb.AppendLine(string.Format("replanOutwardDateTime: {0} {1}", replanOutwardDateTime.ToString(), linebreak));
            sb.AppendLine(string.Format("replanReturnDateTime: {0} {1}", replanReturnDateTime.ToString(), linebreak));
            sb.AppendLine(string.Format("replanOutwardArriveBefore: {0} {1}", replanOutwardArriveBefore.ToString(), linebreak));
            sb.AppendLine(string.Format("replanReturnArriveBefore: {0} {1}", replanReturnArriveBefore.ToString(), linebreak));
            sb.AppendLine(string.Format("replanIsOutwardRequired: {0} {1}", replanIsOutwardRequired.ToString(), linebreak));
            sb.AppendLine(string.Format("replanIsReturnRequired: {0} {1}", replanIsReturnRequired.ToString(), linebreak));
            sb.AppendLine(linebreak);
            #endregion

            return sb.ToString();
        }

        #endregion

        #region Private methods

        #endregion
    }
}
