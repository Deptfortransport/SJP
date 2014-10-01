// *********************************************** 
// NAME             : RepeatVisitorType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Defines types for Repeat visitor events.
// ************************************************
// 

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Enumeration containing classifiers for <c>RepeatVisitorEvent</c>.
    /// </summary>
    public enum RepeatVisitorType : int
    {
        VisitorUnknown = 1,
        VisitorNew,
        VisitorRepeat,
        VisitorRobot
    }
}