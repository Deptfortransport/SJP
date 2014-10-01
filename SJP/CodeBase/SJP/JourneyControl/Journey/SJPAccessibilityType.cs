// *********************************************** 
// NAME             : SJPAccessibilityType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: SJPAccessibilityType enumeration
// ************************************************
// 
                
using System;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// SJPAccessibilityType enumeration
    /// </summary>
    [Serializable()]
    public enum SJPAccessibilityType
    {
        // Used for returning an accessibility type which is not parsed into any of the others, 
        // should not be displayed in UI output
        Unknown,

        // Service assistance (e.g. train service offers assistance with boarding)
        ServiceAssistanceBoarding,
        ServiceAssistanceWheelchair,
        ServiceAssistanceWheelchairBooked,
        ServiceAssistancePorterage,
        ServiceAssistanceOther,

        // Service specific (e.g. bus service has low floor)
        ServiceLowFloor,
        ServiceWheelchairBookingRequired,

        // Accessibility
        EscalatorFreeAccess,
        LiftFreeAccess,
        MobilityImpairedAccess,
        StepFreeAccess,
        WheelchairAccess,
        
        // Access features
        AccessLiftUp,
        AccessLiftDown,
        AccessStairsUp,
        AccessStairsDown,
        AccessSeriesOfStairs,
        AccessEscalatorUp,
        AccessEscalatorDown,
        AccessTravelator,
        AccessRampUp,
        AccessRampDown,
        AccessShuttle,
        AccessBarrier,
        AccessNarrowEntrance,
        AccessConfinedSpace,
        AccessQueueManagement,
        AccessNone,
        AccessUnknown,
        AccessOther,
        AccessOpenSpace,
        AccessStreet,
        AccessPavement,
        AccessFootpath,
        AccessPassage,
    }
}
