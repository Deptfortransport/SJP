// *********************************************** 
// NAME             : JourneyCallingPointType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: Enumeration of JourneyCallingPointType
// ************************************************
// 

using System;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Enumeration of JourneyCallingPointType
    /// </summary>
    [Serializable()]
    public enum JourneyCallingPointType
    {
        Origin,
        Destination,
        Board,
        Alight,
        CallingPoint,
        PassingPoint,
        OriginAndBoard,
        DestinationAndAlight
    }
}
