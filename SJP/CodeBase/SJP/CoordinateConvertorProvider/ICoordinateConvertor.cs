// *********************************************** 
// NAME             : ICoordinateConvertor.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: Interface for CoordinateConvertor
// ************************************************
// 

using SJP.Common.LocationService;
using System;

namespace SJP.UserPortal.CoordinateConvertorProvider
{
    /// <summary>
    /// Interface for CoordinateConvertor
    /// </summary>
    public interface ICoordinateConvertor : IDisposable
    {
        /// <summary>
        /// Converts an OSGR into a LatitudeLongitude by calling the CoordinateConvertor web service 
        /// </summary>
        LatitudeLongitude GetLatitudeLongitude(OSGridReference osgr);

        /// <summary>
        /// Converts the OSGRs into LatitudeLongitudes by calling the CoordinateConvertor web service
        /// </summary>
        LatitudeLongitude[] GetLatitudeLongitude(OSGridReference[] osgrs);

        /// <summary>
        /// Converts a LatitudeLongitude into an OSGR by calling the CoordinateConverter web service
        /// </summary>
        OSGridReference GetOSGridReference(LatitudeLongitude latlong);

        /// <summary>
        /// Converts the LatitudeLongitudes into OSGRs by calling the CoordinateConverter web service
        /// </summary>
        OSGridReference[] GetOSGridReference(LatitudeLongitude[] latlongs);
    }
}
