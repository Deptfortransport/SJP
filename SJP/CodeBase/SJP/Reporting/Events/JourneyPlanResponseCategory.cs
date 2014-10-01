// *********************************************** 
// NAME             : JourneyPlanResponseCategory      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: Enumeration containing response classifiers for JourneyPlanResultEvent
// ************************************************
// 

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Enumeration containing response classifiers for <c>JourneyPlanResultEvent</c>
    /// </summary>
    public enum JourneyPlanResponseCategory : int
    {
        Results = 1,
        ZeroResults,
        Failure
    }
}
