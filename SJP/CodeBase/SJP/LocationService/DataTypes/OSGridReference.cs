// *********************************************** 
// NAME             : OSGridReference.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 22 Feb 2011
// DESCRIPTION  	: Public class to hold a pair of co-ordinates
//                    as a single object.
// ************************************************
// 

using System;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Public class to hold a pair of co-ordinates
    /// </summary>
    [Serializable()]
    public class OSGridReference
    {
        #region Private members

        private float easting;
        private float northing;

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OSGridReference()
        {
            easting = 0;
            northing = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OSGridReference(float gridRefEasting, float gridRefNorthing)
        {
            easting = gridRefEasting;
            northing = gridRefNorthing;
        }

        /// <summary>
        /// Constructor, from string
        /// </summary>
        public OSGridReference(string gridRef)
        {
            easting = 0;
            northing = 0;

            if (!string.IsNullOrEmpty(gridRef))
            {
                // Assume gridref string is in format 123456,123456
                string[] parts = gridRef.Split(',');

                if (parts.Length == 2)
                {
                    if (!float.TryParse(parts[0], out easting))
                    {
                        easting = 0;
                    }
                    if (!float.TryParse(parts[1], out northing))
                    {
                        northing = 0;
                    }
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write. Easting
        /// </summary>
        public float Easting
        {
            get { return easting ;}
            set { easting = value ;}
        }

        /// <summary>
        /// Read/Write. Northing
        /// </summary>
        public float Northing
        {
            get { return northing ;}
            set { northing = value ;}
        }
        
        /// <summary>
        /// Read only. Returns true if both the Easting & Northing co-ords are both greater than 0 (valid).
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (Easting > 0 && Northing > 0);
            }
        }
                
        #endregion

        #region Public Methods

        /// <summary>
        /// Get Distance in metres between the current instance 
        /// and the supplied other OSGR
        /// </summary>
        /// <param name="osgr">supplied OSGR</param>
        /// <returns>distance in metres</returns>
        public int DistanceFrom(OSGridReference osgr)
        {
            return (int)(Math.Sqrt(
                Math.Pow((this.Easting - osgr.Easting), 2)
                + Math.Pow((this.Northing - osgr.Northing), 2)));
        }

        /// <summary>
        /// Returns a hash code of this OSGridReference. 
        /// </summary>
        /// <returns></returns>
        public int GetSJPHashCode()
        {
            // Does not use native GetHashCode as this can return different hash codes 
            // for instances of the "same" object.

            // string, int, etc return the same hashcode if they have the same value
            int hashCode = easting.GetHashCode() ^ northing.GetHashCode();
                        
            return hashCode;
        }

        /// <summary>
        /// Overriden ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0},{1}", easting, northing);
        }

        #endregion
    }
}
