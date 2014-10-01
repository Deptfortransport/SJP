// *********************************************** 
// NAME             : OSGridReference.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: OSGridReference class, returned by the web service to the caller
// ************************************************
// 
                
using System;

namespace SJP.WebService.CoordinateConvertorService
{
    /// <summary>
    /// OSGridReference class, returned by the web service to the caller
    /// </summary>
    [Serializable()]
    public class OSGridReference
    {
        #region Private members

        // Private members
        private int easting;
        private int northing;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public OSGridReference()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write Property. Easting
        /// </summary>
        public int Easting
        {
            get { return easting; }
            set { easting = value; }
        }

        /// <summary>
        /// Read/Write Property. Northing
        /// </summary>
        public int Northing
        {
            get { return northing; }
            set { northing = value; }
        }

        #endregion
    }
}