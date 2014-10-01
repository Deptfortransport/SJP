// *********************************************** 
// NAME             : LatitudeLongitude.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: LatitudeLongitude class, is returned by the web service to the caller
// ************************************************
// 
                
using System;

namespace SJP.WebService.CoordinateConvertorService
{
    /// <summary>
    /// LatitudeLongitude class, is returned by the web service to the caller
    /// </summary>
    [Serializable()]
    public class LatitudeLongitude
    {
        #region Private members

        // Private members
        private double latitude;
        private double longitude;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LatitudeLongitude()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write Property. Latitude
        /// </summary>
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        /// <summary>
        /// Read/Write Property. Longitude
        /// </summary>
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        #endregion
    }
}