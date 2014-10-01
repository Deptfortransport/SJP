// *********************************************** 
// NAME             : SJPGNATLocationType.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 09 June 2011
// DESCRIPTION  	: Enumeration of possible GNAT location types
// ************************************************
// 

using System;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Enumeration of possible GNAT location types
    /// </summary>
    [Serializable()]
    public enum SJPGNATLocationType
    {
        Bus,
        Rail,
        Coach,
        Tram,
        Underground,
        DLR,
        Ferry,
        Air,

    }
}