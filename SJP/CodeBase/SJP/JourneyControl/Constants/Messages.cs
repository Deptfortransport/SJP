// *********************************************** 
// NAME             : Messages.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: Messages class containing messages used in JourneyControl project
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Messages class containing messages used in JourneyControl project
    /// </summary>
    public class Messages
    {
        // Used for "internal" logging messages
        // so no point in putting them in resource file ...
        public const string CJP = "CJP";
        public const string CTP = "CTP";

        public const string JourneyWebText = "JourneyWeb";
        public const string CJPText = "CJP";
        public const string TTBOText = "TTBO";
        public const string RoadEngineText = "Road Engine";
        public const string CTPText = "Road Engine";
        public const string StopEventText = "Stop Event";
        
        public const string LogMessage = "{0} returned message with Type[{1}] Text[{2}] MajorCode[{3}] MinorCode[{4}] for JourneyReferenceNumber[{5}]";

        public const string ROAD_NoNodeToid = "No Node or Node TOID was found for a StopoverSection of name[{0}] and type[{1}]";

        // Resource keys for messages to be displayed in UI
        public const string JourneyWebTooFarAhead = "JourneyOutput.Message.JourneyWebTooFarAhead";
        public const string JourneyWebNoResults = "JourneyOutput.Message.JourneyWebNoResults";

        public const string CJPInternalError = "JourneyOutput.Message.CJPInternalError";

        public const string CJPNoResults = "JourneyOutput.Message.CJPNoResults";
        public const string CJPPartialResults_Outward = "JourneyOutput.Message.CJPPartialResults.OutwardFound";
        public const string CJPPartialResults_Return = "JourneyOutput.Message.CJPPartialResults.ReturnFound";

        public const string CJPNoResults_Car = "JourneyOutput.Message.CJPNoResults.Car";
        public const string CJPPartialResults_Outward_Car = "JourneyOutput.Message.CJPPartialResults.OutwardFound.Car";
        public const string CJPPartialResults_Return_Car = "JourneyOutput.Message.CJPPartialResults.ReturnFound.Car";

        public const string CJPNoResults_WheelchairAssistance = "JourneyOutput.Message.CJPNoResults.WheelchairAssistance";
        public const string CJPPartialResults_Outward_WheelchairAssistance = "JourneyOutput.Message.CJPPartialResults.OutwardFound.WheelchairAssistance";
        public const string CJPPartialResults_Return_WheelchairAssistance = "JourneyOutput.Message.CJPPartialResults.ReturnFound.WheelchairAssistance";

        public const string CJPNoResults_Wheelchair = "JourneyOutput.Message.CJPNoResults.Wheelchair";
        public const string CJPPartialResults_Outward_Wheelchair = "JourneyOutput.Message.CJPPartialResults.OutwardFound.Wheelchair";
        public const string CJPPartialResults_Return_Wheelchair = "JourneyOutput.Message.CJPPartialResults.ReturnFound.Wheelchair";

        public const string CJPNoResults_Assistance = "JourneyOutput.Message.CJPNoResults.Assistance";
        public const string CJPPartialResults_Outward_Assistance = "JourneyOutput.Message.CJPPartialResults.OutwardFound.Assistance";
        public const string CJPPartialResults_Return_Assistance = "JourneyOutput.Message.CJPPartialResults.ReturnFound.Assistance";

        public const string CTPInternalError = "JourneyOutput.Message.CyclePlannerInternalError";
        public const string CTPPartialReturn = "JourneyOutput.Message.CyclePlannerPartialReturn";
        public const string CTPNoResults = "JourneyOutput.Message.CyclePlannerNoResults";

        public const string CJPStopEventInternalError = "JourneyOutput.Message.CJPStopEventInternalError";
        public const string CJPStopEventPartialReturn = "JourneyOutput.Message.CJPStopEventPartialReturn";
        public const string CJPStopEventNoResults = "JourneyOutput.Message.CJPStopEventNoResults";
        public const string CJPStopEventNoEarlierResults = "JourneyOutput.Message.CJPStopEventNoEarlierResults";
        public const string CJPStopEventNoLaterResults = "JourneyOutput.Message.CJPStopEventNoLaterResults";
        public const string CJPOvernightJourneyRejected = "JourneyOutput.Message.CJPOvernightJourneyRejected";

        public const string SJPCycleParkClosedForJourneyLeave = "JourneyOutput.Message.CycleParkIsClosed.Leave";
        public const string SJPCycleParkClosedForJourneyArrive = "JourneyOutput.Message.CycleParkIsClosed.Arrive";

        // Used in Road/Cycle journeys
        public const string ROAD_NUMBER_FERRY = "FERRY";
        public const string ROAD_NAME_SLIP_ROAD = "SLIP ROAD";
    }

}
