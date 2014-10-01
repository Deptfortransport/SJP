// *********************************************** 
// NAME             : SJPVenueGateNavigationPaths.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 11 May 2011
// DESCRIPTION  	: Represents the routes between venue gates and venues
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Represents SJP venue gate check constraints (i.e delays caused by queues)
    /// </summary>
    [Serializable]
    public class SJPVenueGateNavigationPath
    {
        #region Private Fields
        private string navigationPathID;
        private string navigationPathName;
        private string fromNaPTAN;
        private string toNaPTAN;
        private TimeSpan transferDuration;
        private int transferDistance;
        private string gateNaptan;
        #endregion

        #region Public Properties
        /// <summary>
        /// The unique ID of this navigation path
        /// </summary>
        public string NavigationPathID
        {
            get { return navigationPathID; }
            set { navigationPathID = value; }
        }

        /// <summary>
        /// The name of this navigation path
        /// </summary>
        public string NavigationPathName
        {
            get { return navigationPathName; }
            set { navigationPathName = value; }
        }

        /// <summary>
        /// The naptan at the start of this navigation path
        /// </summary>
        public string FromNaPTAN
        {
            get { return fromNaPTAN; }
            set { fromNaPTAN = value; }
        }

        /// <summary>
        /// The naptan at the end of this navigation path
        /// </summary>
        public string ToNaPTAN
        {
            get { return toNaPTAN; }
            set { toNaPTAN = value; }
        }

        /// <summary>
        /// The duration of this transfer in minutes
        /// </summary>
        public TimeSpan TransferDuration
        {
            get { return transferDuration; }
            set { transferDuration = value; }
        }

        /// <summary>
        /// The distance of this transfer in metres
        /// </summary>
        public int TransferDistance
        {
            get { return transferDistance; }
            set { transferDistance = value; }
        }

        /// <summary>
        /// The Venue Gate of this constraint
        /// </summary>
        public string GateNaPTAN
        {
            get { return gateNaptan; }
            set { gateNaptan = value; }
        } 
        #endregion
    }
}


