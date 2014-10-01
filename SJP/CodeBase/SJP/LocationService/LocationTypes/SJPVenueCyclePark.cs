// *********************************************** 
// NAME             : SJPVenueCyclePark.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: The class represents the SJP venue cycle park
// ************************************************
                
using System;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// The class represents the SJP venue cycle park
    /// </summary>
    [Serializable]
    public class SJPVenueCyclePark : SJPPark
    {
        #region Private Fields

        private string cycleParkMapUrl = string.Empty;
        private int numberOfSpaces = 0;
        private OSGridReference cycleToGridRef = null;
        private OSGridReference cycleFromGridRef = null;
        private CycleStorageType storageType;
        private TimeSpan walkToGateDuration = new TimeSpan();
        private string venueGateEntranceNaPTAN = string.Empty;
        private TimeSpan walkFromGateDuration = new TimeSpan();
        private string venueGateExitNaPTAN = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// A URL pointing to a map of the cycle park site
        /// </summary>
        public string CycleParkMapUrl
        {
            get { return cycleParkMapUrl; }
            set { cycleParkMapUrl = value; }
        }

        /// <summary>
        /// The number of spaces situated at this cycle park
        /// </summary>
        public int NumberOfSpaces
        {
            get { return numberOfSpaces; }
            set { numberOfSpaces = value; }
        }

        /// <summary>
        /// The co-ordinate that will be used in cycle journey plans to the cycle park
        /// </summary>
        public OSGridReference CycleToGridReference
        {
            get { return cycleToGridRef; }
            set { cycleToGridRef = value; }
        }

        /// <summary>
        /// The co-ordinate that will be used in cycle journey plans from the cycle park
        /// </summary>
        public OSGridReference CycleFromGridReference
        {
            get { return cycleFromGridRef; }
            set { cycleFromGridRef = value; }
        }

        /// <summary>
        /// The type fo the cycle storage the cycle park have
        /// </summary>
        public CycleStorageType StorageType
        {
            get { return storageType; }
            set { storageType = value; }
        }

        /// <summary>
        /// The average number of minutes needed to walk from this cycle park to the Olympic venue gate
        /// </summary>
        public TimeSpan WalkToGateDuration
        {
            get { return walkToGateDuration; }
            set { walkToGateDuration = value; }
        }

        /// <summary>
        /// The average number of minutes needed to walk from the Olympic venue gate to this cycle park
        /// </summary>
        public TimeSpan WalkFromGateDuration
        {
            get { return walkFromGateDuration; }
            set { walkFromGateDuration = value; }
        }

        /// <summary>
        /// The NaPTAN code for the Venue Gate Entrance at the Olympic Venue with which this cycle park is associated
        /// </summary>
        public string VenueGateEntranceNaPTAN
        {
            get { return venueGateEntranceNaPTAN; }
            set { venueGateEntranceNaPTAN = value; }
        }
        
        /// <summary>
        /// The NaPTAN code for the Venue Gate Exit at the Olympic Venue with which this cycle park is associated
        /// </summary>
        public string VenueGateExitNaPTAN
        {
            get { return venueGateExitNaPTAN; }
            set { venueGateExitNaPTAN = value; }
        }

        #endregion
    }
}
