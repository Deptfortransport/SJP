// *********************************************** 
// NAME             : SJPLocationType.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 22 Feb 2011
// DESCRIPTION  	: Enumeration of possible location types
// ************************************************
// 

using System;

namespace SJP.Common.LocationService
{
    [Serializable()]
    public enum SJPLocationType
    {
        Venue,
        Station,
        StationAirport,
        StationCoach,
        StationFerry,
        StationRail,
        StationTMU,
        StationGroup,
        Locality,
        Postcode,
        CoordinateEN, // Easting Northing
        CoordinateLL, // Latitude Longitude
        Unknown
    }
}
