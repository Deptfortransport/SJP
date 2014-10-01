// *********************************************** 
// NAME             : JourneyCallingPoint.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: JourneyCallingPoint class encapsulate attributes of a 
// single point on a journey transport schedule
// ************************************************
// 
                
using System;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// JourneyCallingPoint class encapsulate attributes of a 
    /// single point on a journey transport schedule
    /// </summary>
    [Serializable()]
    public class JourneyCallingPoint
    {
        #region Private members

        private SJPLocation location;
        private DateTime arrivalDateTime;
        private DateTime departureDateTime;
        private JourneyCallingPointType type;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public JourneyCallingPoint()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyCallingPoint(SJPLocation location, DateTime arrivalDateTime, DateTime departureDateTime, JourneyCallingPointType type)
		{
			this.location = location;
			this.arrivalDateTime = arrivalDateTime;
			this.departureDateTime = departureDateTime;
			this.type = type;
		}

        #endregion
                
        #region Public properties
        
        /// <summary>
        /// Read/Write. SJPLocation of calling point
        /// </summary>
        public SJPLocation Location
        {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// Read/Write. Arrival DateTime at calling point
        /// </summary>
        public DateTime ArrivalDateTime
        {
            get { return arrivalDateTime; }
            set { arrivalDateTime = value; }
        }

        /// <summary>
        /// Read/Write. Departure DateTime from calling point
        /// </summary>
        public DateTime DepartureDateTime
        {
            get { return departureDateTime; }
            set { departureDateTime = value; }
        }

        /// <summary>
        /// Read/Write. Calling point type
        /// </summary>
        public JourneyCallingPointType Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion
    }
}
