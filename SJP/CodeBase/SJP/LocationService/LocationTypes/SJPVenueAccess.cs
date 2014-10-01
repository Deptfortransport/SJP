// *********************************************** 
// NAME             : SJPVenueAccess.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 16 June 2011
// DESCRIPTION  	: SJPVenueAccess class to hold alternative NaPTANs to use for 
// a Venue if the venue's NaPTAN is not suitable for an accessible journey request
// ************************************************
// 
                
using System;
using System.Collections.Generic;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// SJPVenueAccess class to hold alternative NaPTANs to use for 
    /// a Venue if the venue's NaPTAN is not suitable for an accessible journey request
    /// </summary>
    [Serializable]
    public class SJPVenueAccess
    {
        #region Private Fields

        private string venueNaPTAN = string.Empty;
        private string venueName = string.Empty;
        private DateTime accessFrom = DateTime.MinValue;
        private DateTime accessTo = DateTime.MaxValue;
        private TimeSpan accessToVenueDuration = TimeSpan.Zero;
        private List<SJPVenueAccessStation> stations = new List<SJPVenueAccessStation>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SJPVenueAccess()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPVenueAccess(string venueNaPTAN, string venueName, DateTime accessFrom, DateTime accessTo, TimeSpan accessToVenueDuration)
        {
            this.venueNaPTAN = venueNaPTAN;
            this.venueName = venueName;
            this.accessFrom = accessFrom;
            this.accessTo = accessTo;
            this.accessToVenueDuration = accessToVenueDuration;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The unique NaPTAN ID of the venue.
        /// </summary>
        public string VenueNaPTAN
        {
            get { return venueNaPTAN; }
            set { venueNaPTAN = value; }
        }

        /// <summary>
        /// The display name of the venue. 
        /// </summary>
        public string VenueName
        {
            get { return venueName; }
            set { venueName = value; }
        }
                
        /// <summary>
        /// The first date the venue accessible station(s) are available. 
        /// </summary>
        public DateTime AccessFrom
        {
            get { return accessFrom; }
            set { accessFrom = value; }
        }

        /// <summary>
        /// The last date the venue accessible station(s) are available. 
        /// </summary>
        public DateTime AccessTo
        {
            get { return accessTo; }
            set { accessTo = value; }
        }

        /// <summary>
        /// The duraation to get to the venue
        /// </summary>
        public TimeSpan AccessToVenueDuration
        {
            get { return accessToVenueDuration; }
            set { accessToVenueDuration = value; }
        }

        /// <summary>
        /// The list of accessible stations to use for the venue. 
        /// </summary>
        public List<SJPVenueAccessStation> Stations
        {
            get { return stations; }
            set { stations = value; }
        }

        #endregion
    }
}
