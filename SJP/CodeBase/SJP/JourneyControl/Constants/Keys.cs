// *********************************************** 
// NAME             : Keys.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: Journey Control Keys class defining property keys used in the project
// ************************************************
// 

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Journey Control Keys class defining property keys used in the project
    /// </summary>
    public class Keys
    {
        // Logging
        public const string LogNoJourneyResponses = "JourneyControl.Log.NoJourneyResponses";
        public const string LogJourneyWebFailures = "JourneyControl.Log.JourneyWebFailures";
        public const string LogTTBOFailures = "JourneyControl.Log.TTBOFailures";
        public const string LogCJPFailures = "JourneyControl.Log.CJPFailures";
        public const string LogRoadEngineFailures = "JourneyControl.Log.RoadEngineFailures";
        public const string LogCyclePlannerFailures = "JourneyControl.Log.CyclePlannerFailures";
        public const string LogDisplayableMessages = "JourneyControl.Log.DisplayableMessages";

        // Success/Error codes for CJP
        public const string CodeCjpOK = "JourneyControl.Code.CJP.OK"; // 0
        public const string CodeCjpNoPublicJourney = "JourneyControl.Code.CJP.NoPublicJourney"; //18;
        public const string CodeCjpJourneysRejected = "JourneyControl.Code.CJP.JourneysRejected"; //19;
        public const string CodeCjpAwkwardOvernightRejected = "JourneyControl.Code.CJP.AwkwardOvernightRejected"; //30;

        public const string CodeCjpRoadEngineOK = "JourneyControl.Code.CJP.RoadEngineOK"; //100;
        public const string CodeCjpRoadEngineMin = "JourneyControl.Code.CJP.RoadEngineMin"; //100;
        public const string CodeCjpRoadEngineMax = "JourneyControl.Code.CJP.RoadEngineMax"; //199;

        public const string CodeJourneyWebMajorNoResults = "JourneyControl.Code.JourneyWeb.MajorNoResults"; //1;
        public const string CodeJourneyWebMinorPast = "JourneyControl.Code.JourneyWeb.MinorPast"; //1;
        public const string CodeJourneyWebMinorFuture = "JourneyControl.Code.JourneyWeb.MinorFuture"; //2;
        public const string CodeJourneyWebMajorGeneral = "JourneyControl.Code.JourneyWeb.MajorGeneral"; //9;
        public const string CodeJourneyWebMinorDisplayable = "JourneyControl.Code.JourneyWeb.MinorDisplayable"; //2;

        public const string CodeTTBOMajorOK = "JourneyControl.Code.TTBO.MajorOK"; //0;
        public const string CodeTTBOMinorOK = "JourneyControl.Code.TTBO.MinorOK"; //1;
        public const string CodeTTBOMinorNoResults = "JourneyControl.Code.TTBO.MinorNoResults"; //1;
        public const string CodeTTBONoTimetableServiceFound = "JourneyControl.Code.TTBO.NoTimetableServiceFound"; //302;

        // Success/Error codes for CTP
        public const string CodeCTPOK = "JourneyControl.Code.CTP.OK"; //100;
        public const string CodeCTPUndeterminedError = "JourneyControl.Code.CTP.UndeterminedError"; //1;
        public const string CodeCTPSystemException = "JourneyControl.Code.CTP.SystemException"; //2;
        public const string CodeCTPOperationNotSupported = "JourneyControl.Code.CTP.OperationNotSupported"; //3;
        public const string CodeCTPInvalidRequest = "JourneyControl.Code.CTP.InvalidRequest"; //4;
        public const string CodeCTPErrorConnectingToDatabase = "JourneyControl.Code.CTP.ErrorConnectingToDatabase"; //5;
        public const string CodeCTPNoJourneyCouldBeFound = "JourneyControl.Code.CTP.NoJourneyCouldBeFound"; //6;

        // CJP properties
        public const string TimeoutMillisecs_CJP = "JourneyControl.CJP.TimeoutMillisecs";

        public const string JourneyRequest_AlgorithmPublic = "JourneyControl.CJP.JourneyRequest.Algorithm.Public";
        public const string JourneyRequest_AlgorithmPublic_MinChanges = "JourneyControl.CJP.JourneyRequest.Algorithm.Public.MinChanges";
        public const string JourneyRequest_DrtIsRequired = "JourneyControl.CJP.JourneyRequest.DrtIsRequired";
        public const string JourneyRequest_Sequence = "JourneyControl.CJP.JourneyRequest.Sequence";
        public const string JourneyRequest_Sequence_RiverServicePlannerMode = "JourneyControl.CJP.JourneyRequest.Sequence.RiverServicePlannerMode";
        public const string JourneyRequest_InterchangeSpeed = "JourneyControl.CJP.JourneyRequest.InterchangeSpeed";
        public const string JourneyRequest_WalkingSpeed = "JourneyControl.CJP.JourneyRequest.WalkingSpeed.MetresPerMin";
        public const string JourneyRequest_WalkingSpeed_Assistance = "JourneyControl.CJP.JourneyRequest.WalkingSpeed.Assistance.MetresPerMin";
        public const string JourneyRequest_WalkingSpeed_StepFree = "JourneyControl.CJP.JourneyRequest.WalkingSpeed.StepFree.MetresPerMin";
        public const string JourneyRequest_WalkingSpeed_StepFreeAssistance = "JourneyControl.CJP.JourneyRequest.WalkingSpeed.StepFreeAssistance.MetresPerMin";
        public const string JourneyRequest_MaxWalkingTime = "JourneyControl.CJP.JourneyRequest.MaxWalkingTime.Minutes";
        public const string JourneyRequest_MaxWalkingDistance_Assistance = "JourneyControl.CJP.JourneyRequest.WalkingDistance.Assistance.Metres";
        public const string JourneyRequest_MaxWalkingDistance_StepFree = "JourneyControl.CJP.JourneyRequest.WalkingDistance.StepFree.Metres";
        public const string JourneyRequest_MaxWalkingDistance_StepFreeAssistance = "JourneyControl.CJP.JourneyRequest.WalkingDistance.StepFreeAssistance.Metres";
        public const string JourneyRequest_RoutingGuideInfluenced = "JourneyControl.CJP.JourneyRequest.RoutingGuideInfluenced";
        public const string JourneyRequest_RoutingGuideCompliantJourneysOnly = "JourneyControl.CJP.JourneyRequest.RoutingGuideCompliantJourneysOnly";
        public const string JourneyRequest_RouteCodes = "JourneyControl.CJP.JourneyRequest.RouteCodes";
        public const string JourneyRequest_OlympicRequest = "JourneyControl.CJP.JourneyRequest.OlympicRequest";
        public const string JourneyRequest_TravelDemandPlanSwitch = "JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Switch";
        public const string JourneyRequest_TravelDemandPlanOff = "JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Off";
        public const string JourneyRequest_TravelDemandPlanOutward = "JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Outward";
        public const string JourneyRequest_TravelDemandPlanReturn = "JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Return";
        public const string JourneyRequest_TravelDemandPlanOutward_Accessible_DoNotUseUnderground = "JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Outward.Accessible.DoNotUseUnderground";
        public const string JourneyRequest_TravelDemandPlanReturn_Accessible_DoNotUseUnderground = "JourneyControl.CJP.JourneyRequest.TravelDemandPlan.Return.Accessible.DoNotUseUnderground";
        public const string JourneyRequest_DontForceCoach_OriginDestinationLondon = "JourneyControl.CJP.JourneyRequest.DontForceCoach.OriginDestinationLondon";
        public const string JourneyRequest_DontForceCoach_Accessible_OriginDestinationLondon = "JourneyControl.CJP.JourneyRequest.DontForceCoach.Accessible.OriginDestinationLondon";
        public const string JourneyRequest_DontForceCoach_Accessible_FewerChanges = "JourneyControl.CJP.JourneyRequest.DontForceCoach.Accessible.FewerChanges";

        public const string JourneyRequest_AlgorithmPrivate = "JourneyControl.CJP.JourneyRequest.Algorithm.Private";
        public const string JourneyRequest_AvoidMotorways = "JourneyControl.CJP.JourneyRequest.AvoidMotorways";
        public const string JourneyRequest_AvoidFerries = "JourneyControl.CJP.JourneyRequest.AvoidFerries";
        public const string JourneyRequest_AvoidTolls = "JourneyControl.CJP.JourneyRequest.AvoidTolls";
        public const string JourneyRequest_DrivingSpeed = "JourneyControl.CJP.JourneyRequest.DrivingSpeed.KmPerHour";
        public const string JourneyRequest_DoNotUseMotorways = "JourneyControl.CJP.JourneyRequest.DoNotUseMotorways";
        public const string JourneyRequest_FuelConsumption = "JourneyControl.CJP.JourneyRequest.FuelConsumption.MetresPerLitre";
        public const string JourneyRequest_FuelPrice = "JourneyControl.CJP.JourneyRequest.FuelPrice.PencePerLitre";
        public const string JourneyRequest_RemoveAwkwardOvernight = "JourneyControl.CJP.JourneyRequest.RemoveAwkwardOvernight";

        public const string TrapezeRegions = "JourneyControl.Notes.TrapezeRegions";
        public const string NullTollLink = "JourneyControl.NullTollLink.Value";
        public const string CongestionLevel = "JourneyControl.CongestionWarning.Value";        
        public const string Toid_Prefix = "JourneyControl.ToidPrefix";

        // CTP properties
        public const string TimeoutMillisecs_CyclePlanner = "JourneyControl.CyclePlanner.TimeoutMillisecs";

        public const string JourneyRequest_IncludeToids = "JourneyControl.CyclePlanner.JourneyRequest.IncludeToids";

        public const string JourneyRequest_PenaltyFunction_Algorithm = "JourneyControl.CyclePlanner.PenaltyFunction.Algorithm";
        public const string JourneyRequest_PenaltyFunction_DLLPath = "JourneyControl.CyclePlanner.PenaltyFunction.{0}.DllPath";
        public const string JourneyRequest_PenaltyFunction_DLL = "JourneyControl.CyclePlanner.PenaltyFunction.{0}.Dll";
        public const string JourneyRequest_PenaltyFunction_Prefix = "JourneyControl.CyclePlanner.PenaltyFunction.{0}.Prefix";
        public const string JourneyRequest_PenaltyFunction_Suffix = "JourneyControl.CyclePlanner.PenaltyFunction.{0}.Suffix";

        public const string JourneyRequest_UserPreferences_Count = "JourneyControl.CyclePlanner.UserPreference.NumberOfPreferences";
        public const string JourneyRequest_UserPreferences_Index = "JourneyControl.CyclePlanner.UserPreference.Preference.{0}";

        public const string JourneyResultSetting_IncludeToids = "JourneyControl.CyclePlanner.JourneyResultSetting.IncludeToids";
        public const string JourneyResultSetting_IncludeGeometry = "JourneyControl.CyclePlanner.JourneyResultSetting.IncludeGeometry";
        public const string JourneyResultSetting_IncludeText = "JourneyControl.CyclePlanner.JourneyResultSetting.IncludeText";
        public const string JourneyResultSetting_PointSeperator = "JourneyControl.CyclePlanner.JourneyResultSetting.PointSeperator";
        public const string JourneyResultSetting_EastingNorthingSeperator = "JourneyControl.CyclePlanner.JourneyResultSetting.EastingNorthingSeperator";

        public const string JourneyResult_IncludeLatLongs = "JourneyControl.CyclePlanner.JourneyResult.IncludeLatitudeLongitude";
                
        // StopEvent properties
        public const string StopEventRequest_TimeoutMillisecs_CJP = "JourneyControl.StopEvents.CJP.TimeoutMillisecs";
        public const string StopEventRequest_IncludeLocationFilter = "JourneyControl.StopEvents.JourneyRequest.IncludeLocationFilter";
        public const string StopEventRequest_RealTimeRequired = "JourneyControl.StopEvents.JourneyRequest.RealTimeRequired";
        public const string StopEventRequest_RangeSequence = "JourneyControl.StopEvents.JourneyRequest.Range";
        public const string StopEventRequest_RangeSequence_Replan = "JourneyControl.StopEvents.JourneyRequest.Replan.Range";
    }
}
