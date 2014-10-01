// *********************************************** 
// NAME             : LatitudeLongitude.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: LatitudeLongitude class to hold an SJP representation of the LatitudeLongitude coordinate
// ************************************************
// 
                
using System;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// LatitudeLongitude class to hold an SJP representation of the LatitudeLongitude coordinate
    /// </summary>
    [Serializable()]
    public class LatitudeLongitude
    {
        #region Private members

        private double latitude;
        private double longitude;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LatitudeLongitude()
        {
            this.latitude = 0;
            this.longitude = 0;
        }

        /// <summary>
        /// Alternate constructor
        /// </summary>
        /// <param name="easting">latitude value</param>
        /// <param name="northing">longitude value</param>
        public LatitudeLongitude(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Constructor, from string
        /// </summary>
        public LatitudeLongitude(string gridRef)
        {
            latitude = 0;
            longitude = 0;

            if (!string.IsNullOrEmpty(gridRef))
            {
                // Assume gridref string is in format 123456,123456
                string[] parts = gridRef.Split(',');

                if (parts.Length == 2)
                {
                    if (!double.TryParse(parts[0], out latitude))
                    {
                        latitude = 0;
                    }
                    if (!double.TryParse(parts[1], out longitude))
                    {
                        longitude = 0;
                    }
                }
            }
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

        #region Public methods

        /// <summary>
        /// Returns a hash code of this LatitudeLongitude. 
        /// </summary>
        /// <returns></returns>
        public int GetSJPHashCode()
        {
            // Does not use native GetHashCode as this can return different hash codes 
            // for instances of the "same" object.

            // string, int, etc return the same hashcode if they have the same value
            int hashCode = latitude.GetHashCode() ^ longitude.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Overriden ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0},{1}", latitude, longitude);
        }

        #endregion
    }
}
