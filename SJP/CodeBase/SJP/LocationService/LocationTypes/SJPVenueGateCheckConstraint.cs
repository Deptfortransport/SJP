// *********************************************** 
// NAME             : SJPVenueGateCheckConstraints.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 11 May 2011
// DESCRIPTION  	: Represents SJP venue gate check constraints (i.e delays caused by queues)
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
    public class SJPVenueGateCheckConstraint
    {
        #region Private Fields
        private string checkConstraintID;
        private string checkConstraintName;
        private bool isEntry;
        private string process;
        private string congestion;
        private TimeSpan averageDelay;
        private string gateNaptan;
        #endregion

        #region Public Properties
        /// <summary>
        /// The unique ID of this check constraint
        /// </summary>
        public string CheckConstraintID
        {
            get { return checkConstraintID; }
            set { checkConstraintID = value; }
        }

        /// <summary>
        /// The name of this check constraint
        /// </summary>
        public string CheckConstraintName
        {
            get { return checkConstraintName; }
            set { checkConstraintName = value; }
        }

        /// <summary>
        /// Indicates whether this constraint applies to journeys going to the venue (true)
        /// or from the venue (false)
        /// </summary>
        public bool IsEntry
        {
            get { return isEntry; }
            set { isEntry = value; }
        }

        /// <summary>
        /// The reason behind any delay (e.g. Security Checks)
        /// </summary>
        public string Process
        {
            get { return process; }
            set { process = value; }
        }

        /// <summary>
        /// The type of delay (e.g Queueing)
        /// </summary>
        public string Congestion
        {
            get { return congestion; }
            set { congestion = value; }
        }        
        
        /// <summary>
        /// The length of the dealy in minutes
        /// </summary>
        public TimeSpan AverageDelay
        {
            get { return averageDelay; }
            set { averageDelay = value; }
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

