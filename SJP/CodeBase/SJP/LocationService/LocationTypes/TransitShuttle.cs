// *********************************************** 
// NAME             : TransitShuttle.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Apr 2011
// DESCRIPTION  	: Represents the transit shuttle between SJP car park and SJP venue
// ************************************************
                
                
using System;
using System.Collections.Generic;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Represents the transit shuttle between SJP car park and SJP venue
    /// </summary>
    [Serializable]
    public class TransitShuttle
    {
        #region Private Fields

        private string id;
        private bool toVenue;
        private ParkingInterchangeMode modeOfTransport;
        private int transitDuration;
        private bool isScheduledService;
        private bool isPRMOnly;
        private string venueGateToUse;
        private int serviceFrequency;
        private TimeSpan serviceStartTime;
        private TimeSpan serviceEndTime;
        private Dictionary<Language, string> transferNotes = new Dictionary<Language, string>();
        private List<SJPParkAvailability> availabilities = new List<SJPParkAvailability>(); 

        #endregion

        #region Public Properties

        /// <summary>
        /// The unique ID of this shuttle service.
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Is this service To a Venue (false means a service from a venue)
        /// </summary>
        public bool ToVenue
        {
            get { return toVenue; }
            set { toVenue = value; }
        }

        /// <summary>
        /// Type of transport for Parking Interchange
        /// </summary>
        public ParkingInterchangeMode ModeOfTransport
        {
            get { return modeOfTransport; }
            set { modeOfTransport = value; }
        }

        /// <summary>
        /// The number of minutes this transit shuttle service takes to reach the venue.
        /// </summary>
        public int TransitDuration
        {
            get { return transitDuration; }
            set { transitDuration = value; }
        }

        /// <summary>
        /// Is the service scheduled 
        /// </summary>
        public bool IsScheduledService
        {
            get { return isScheduledService; }
            set { isScheduledService = value; }
        }

        /// <summary>
        /// Is this service for Persons of Reduced Mobility
        /// </summary>
        public bool IsPRMOnly
        {
            get { return isPRMOnly; }
            set { isPRMOnly = value; }
        }

        /// <summary>
        /// The NaPTAN of the venue gate to use after alighting from the shuttle service
        /// </summary>
        public string VenueGateToUse
        {
            get { return venueGateToUse; }
            set { venueGateToUse = value; }
        }

        /// <summary>
        /// The frequency of the service in minutes. 
        /// </summary>
        public int ServiceFrequency
        {
            get { return serviceFrequency; }
            set { serviceFrequency = value; }
        }

        /// <summary>
        /// The time of the first service of the day.
        /// </summary>
        public TimeSpan ServiceStartTime
        {
            get { return serviceStartTime; }
            set { serviceStartTime = value; }
        }

        /// <summary>
        /// The time of the last service of the day.
        /// </summary>
        public TimeSpan ServiceEndTime
        {
            get { return serviceEndTime; }
            set { serviceEndTime = value; }
        }

        /// <summary>
        /// The transit notes
        /// </summary>
        public Dictionary<Language, string> TransferNotes
        {
            get { return transferNotes; }
            set { transferNotes = value; }
        }

        /// <summary>
        /// The availablity conditions for the transit shuttle
        /// </summary>
        public List<SJPParkAvailability> Availability
        {
            get { return availabilities; }
            set { availabilities = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the transfer text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="langauge"></param>
        /// <param name="isToVenue"></param>
        public void AddTransferText(string text, Language langauge)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (transferNotes.ContainsKey(langauge))
                {
                    transferNotes[langauge] = text;
                }
                else
                {
                    transferNotes.Add(langauge, text);
                }
            }
        }

        /// <summary>
        /// Returns the transfer text for the language, string.empty if doesnt exist
        /// </summary>
        public string GetTransferText(Language langauge)
        {
            if (transferNotes.ContainsKey(langauge))
            {
                return transferNotes[langauge];
            }

            return string.Empty;
        }

        #endregion
    }
}
