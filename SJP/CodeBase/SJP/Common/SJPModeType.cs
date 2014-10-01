// *********************************************** 
// NAME             : SJPModeType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Mar 2011
// DESCRIPTION  	: Enumeration for SJPModeType
// ************************************************
// 
                
using System;

namespace SJP.Common
{
    /// <summary>
    /// SJPModeType enum
    /// </summary>
    [Serializable()]
    public enum SJPModeType
    {
        Air = 0,
        Bus = 1,
        Car = 2,
        Coach = 3,
        Cycle = 4,
        Drt = 5,
        Ferry = 6,
        Metro = 7,
        Rail = 8,
        RailReplacementBus = 9,
        Taxi = 10,
        Tram = 11,
        Underground = 12,
        Walk = 13,
        CheckIn = 14,
        CheckOut = 15,
        Transfer = 16,
        Unknown = 17,
        // Modes for internal SJP use
        TransitShuttleBus = 18,
        TransitRail = 19,
        Queue = 20,
        WalkInterchange = 21,
        CableCar = 22,
        EuroTunnel = 23
    }
}
