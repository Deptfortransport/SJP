// *********************************************** 
// NAME             : ISJPJourneyRequest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 Mar 2011
// DESCRIPTION  	: Interface for SJPJourneyRequest class
// ************************************************
// 


using System;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Interface for SJPJourneyRequest class
    /// </summary>
    public interface ISJPJourneyRequest
    {
        /// <summary>
        /// Hash of only the SJPJourneyRequest properties used in journey planning.
        /// e.g. location, dates, times, but not request id
        /// Used when comparing two SJPJourneyRequest objects.
        /// </summary>
        string JourneyRequestHash { get; set; }

        /// <summary>
        /// Indicates if this SJPJourneyRequest was submitted to the journey planner.
        /// This value must be set to true when it has been submitted, as it is used with
        /// the landing page functionality to determine if it is considered as a "new" landing 
        /// page request
        /// </summary>
        bool JourneyRequestSubmitted { get; set; }

        /// <summary>
        /// Returns a hashcode of user changeable values
        /// </summary>
        int GetSJPHashCode();

        #region Locations

        /// <summary>
        /// Origin Location for the journey request
        /// </summary>
        SJPLocation Origin { get; set; }

        /// <summary>
        /// Destination Location for the journey request
        /// </summary>
        SJPLocation Destination { get; set; }

        /// <summary>
        /// Return Origin Location for the journey request (if different from the Outward Destination)
        /// </summary>
        SJPLocation ReturnOrigin { get; set; }

        /// <summary>
        /// Return Destination Location for the journey request (if different from the Outward Origin)
        /// </summary>
        SJPLocation ReturnDestination { get; set; }

        /// <summary>
        /// Location input mode
        /// </summary>
        string LocationInputMode { get; set; }

        #endregion

        #region Dates and times
        /// <summary>
        /// Outward DateTime
        /// </summary>
        DateTime OutwardDateTime { get; set; }

        /// <summary>
        /// Return DateTime
        /// </summary>
        DateTime ReturnDateTime { get; set; }

        /// <summary>
        /// Outward arrive before time flag
        /// </summary>
        bool OutwardArriveBefore { get; set; }

        /// <summary>
        /// Return arrive before time flag
        /// </summary>
        bool ReturnArriveBefore { get; set; }

        /// <summary>
        /// Indicates if outward journey is required
        /// </summary>
        bool IsOutwardRequired { get; set; }

        /// <summary>
        /// Indicates if return journey is required
        /// </summary>
        bool IsReturnRequired { get; set; }

        /// <summary>
        /// Indicates if this request must be treated as a return journey only, IsReturnRequired must be set to true.
        /// When true, then the Origin and Destination are treated as the ReturnOrigin and ReturnDestination respectively
        /// </summary>
        bool IsReturnOnly { get; set; }

        #endregion        

        #region Modes

        /// <summary>
        /// Planner Mode of the journey request
        /// </summary>
        SJPJourneyPlannerMode PlannerMode { get; set; }

        /// <summary>
        /// Modes to submit to the journey planner
        /// </summary>
        List<SJPModeType> Modes { get; set; }

        #endregion

        #region Public specific

        /// <summary>
        /// Public journey type to plan
        /// </summary>
        SJPPublicAlgorithmType PublicAlgorithm { get; set; }

        /// <summary>
        /// Number of journeys to find
        /// </summary>
        int Sequence { get; set; }

        /// <summary>
        /// Interchange speed (int [-3 to 3]), corresponds to the walking speed
        /// </summary>
        /// <example>0 = normal</example>
        int InterchangeSpeed { get; set; }

        /// <summary>
        /// Walking speed (metres per minute [lessthan46 to greaterthan117]), also used to calculate max walk distance
        /// </summary>
        /// <example>80 = normal</example>
        int WalkingSpeed { get; set; }

        /// <summary>
        /// Max walking time (minutes), used to calculate max walk distance if set
        /// </summary>
        int MaxWalkingTime { get; set; }

        /// <summary>
        /// Max walking distance (metres), used in preference to max walking time if set
        /// </summary>
        int MaxWalkingDistance { get; set; }

        /// <summary>
        /// Routing guide influenced
        /// </summary>
        bool RoutingGuideInfluenced { get; set; }

        /// <summary>
        /// Routing guide compliant journeys only
        /// </summary>
        bool RoutingGuideCompliantJourneysOnly { get; set; }

        /// <summary>
        /// Route codes
        /// </summary>
        string RouteCodes { get; set; }

        /// <summary>
        /// Indicates the journey planner should perform olympic games journey planning
        /// </summary>
        bool OlympicRequest { get; set; }
                        
        /// <summary>
        /// Influences the validation performed by the journey planner - For Outward journey
        /// </summary>
        string TravelDemandPlanOutward  { get; set; }
        
        /// <summary>
        /// Influences the validation performed by the journey planner - For Return journey
        /// </summary>
        string TravelDemandPlanReturn  { get; set; }
        
        /// <summary>
        /// If true, then journey planners will be Strict when planning a route using accessibility options. 
        /// If false, then journey planner can return a non-compliant route if compliant not found
        /// </summary>
        bool FilteringStrict { get; set; }

        /// <summary>
        /// If true, then even if force coach value is on in the CJP settings, do not perform force coach. 
        /// If false, then the force coach value in the CJP settings is used
        /// </summary>
        bool DontForceCoach { get; set; }

        /// <summary>
        /// Remove Awkward Overnight Journeys
        /// </summary>
        bool RemoveAwkwardOvernight { get; set; }

        /// <summary>
        /// Accessible option preferences to be passed to the journey planner
        /// </summary>
        SJPAccessiblePreferences AccessiblePreferences { get; set; }

        #endregion

        #region Car specific

        /// <summary>
        /// Avoid motorways
        /// </summary>
        bool AvoidMotorways { get; set; }

        /// <summary>
        /// Avoid ferries
        /// </summary>
        bool AvoidFerries { get; set; }

        /// <summary>
        /// Avoid tolls
        /// </summary>
        bool AvoidTolls { get; set; }

        /// <summary>
        /// List of roads to avoid
        /// </summary>
        List<string> AvoidRoads { get; set; }

        /// <summary>
        /// List of roads to include
        /// </summary>
        List<string> IncludeRoads { get; set; }

        /// <summary>
        /// Private (road) journey type to plan
        /// </summary>
        SJPPrivateAlgorithmType PrivateAlgorithm { get; set; }

        /// <summary>
        /// Maximum driving speed to use (km per hour)
        /// </summary>
        /// <example>112</example>
        int DrivingSpeed { get; set; }

        /// <summary>
        /// Do not use motorways at all
        /// </summary>
        bool DoNotUseMotorways { get; set; }

        /// <summary>
        /// Fuel consumption value to use (usage of fuel in metres per litre)
        /// </summary>
        /// <example>13807</example>
        string FuelConsumption { get; set; }

        /// <summary>
        /// Fuel price value to use (10ths of pence per litre)
        /// </summary>
        /// <example>1150</example>
        string FuelPrice { get; set; }

        #endregion

        #region Cycle specific

        /// <summary>
        /// Cycle algorithm used to identify the penality function used by the cycle planner
        /// </summary>
        string CycleAlgorithm { get; set; }

        /// <summary>
        /// Penalty function used by the cycle planner.
        /// </summary>
        /// <example>
        /// Call C:\CyclePlanner\td.cp.CyclePenaltyFunctions.v2.dll, TransportDirect.JourneyPlanning.CyclePenaltyFunctions.QuietestV912
        /// </example>
        string PenaltyFunction { get; set; }

        /// <summary>
        /// Journey parameters in a SJPUserPreference key/value pairs array used by the cycle planner
        /// </summary>
        /// <remarks>
        /// This is a list of preferences that are used in the request sent to the Atkins CTP.
        /// Keys are (int)0 to (int)14, with values as defined in the CTP interface documentation.
        /// </remarks>
        List<SJPUserPreference> UserPreferences { get; set; }

        #endregion

        #region Journey Parts

        /// <summary>
        /// Outward journey containing legs to add to the journeys planned for this request
        /// </summary>
        Journey OutwardJourneyPart { get; set; }

        /// <summary>
        /// Return journey containing legs to add to the journeys planned for this request
        /// </summary>
        Journey ReturnJourneyPart { get; set; }

        #endregion

        #region Replan

        /// <summary>
        /// Replan flag, indicates to journey planner to perform a replan and to use replan values
        /// </summary>
        bool IsReplan { get; set; }
        
        #region Replan - Datetimes

        /// <summary>
        /// Replan Outward DateTime
        /// </summary>
        DateTime ReplanOutwardDateTime { get; set; }

        /// <summary>
        /// Replan Return DateTime
        /// </summary>
        DateTime ReplanReturnDateTime { get; set; }

        /// <summary>
        /// Replan Outward arrive before time flag
        /// </summary>
        bool ReplanOutwardArriveBefore { get; set; }

        /// <summary>
        /// Replan Return arrive before time flag
        /// </summary>
        bool ReplanReturnArriveBefore { get; set; }

        /// <summary>
        /// Replan Indicates if outward journey is required
        /// </summary>
        bool ReplanIsOutwardRequired { get; set; }

        /// <summary>
        /// Replan Indicates if return journey is required
        /// </summary>
        bool ReplanIsReturnRequired { get; set; }

        #endregion

        #region Replan - Journeys

        /// <summary>
        /// Replan Outward Journeys to add to the result
        /// </summary>
        List<Journey> ReplanOutwardJourneys { get; set; }

        /// <summary>
        /// Replan Return Journeys to add to the result
        /// </summary>
        List<Journey> ReplanReturnJourneys { get; set; }

        /// <summary>
        /// RetainOutwardJourneys flag, indicates to journey planner to retain the outward journeys if supplied
        /// </summary>
        bool ReplanRetainOutwardJourneys { get; set; }

        /// <summary>
        /// RetainReturnJourneys flag, indicates to journey planner to retain the return journeys if supplied
        /// </summary>
        bool ReplanRetainReturnJourneys { get; set; }

        /// <summary>
        /// RetainOutwardJourneysWhenNoResults flag, indicates to journey planner to retain the outward journeys if supplied,
        /// when the journey planner fails to plan a journey (e.g. to allow previous results to be retained).
        /// This flag onle becomes applicable if RetainOutwardJourneys is false
        /// </summary>
        bool ReplanRetainOutwardJourneysWhenNoResults { get; set; }

        /// <summary>
        /// RetainReturnJourneysWhenNoResults flag, indicates to journey planner to retain the return journeys if supplied,
        /// when the journey planner fails to plan a journey (e.g. to allow previous results to be retained).
        /// This flag onle becomes applicable if RetainReturnJourneys is false
        /// </summary>
        bool ReplanRetainReturnJourneysWhenNoResults { get; set; }

        #endregion

        #region Replan - Journey Paging

        /// <summary>
        /// Hash of the SJPJourneyRequest considered as the earlier journey request 
        /// </summary>
        string ReplanJourneyRequestHashEarlier { get; set; }

        /// <summary>
        /// Hash of the SJPJourneyRequest considered as the later journey request 
        /// </summary>
        string ReplanJourneyRequestHashLater { get; set; }

        #endregion

        #endregion

    }
}
