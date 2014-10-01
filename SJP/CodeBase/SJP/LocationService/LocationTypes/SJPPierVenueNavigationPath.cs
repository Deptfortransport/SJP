// *********************************************** 
// NAME             : SJPPierVenueNavigationPath.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Apr 2011
// DESCRIPTION  	: Represents SJP peir venue navigation path
// ************************************************
                
                
using System;
using System.Collections.Generic;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Represents SJP peir venue navigation path
    /// </summary>
    [Serializable]
    public class SJPPierVenueNavigationPath
    {
        #region Private Fields

        private string navigationID;
        private TimeSpan defaultDuration;
        private int distance;
        private string toNaPTAN;
        private string fromNaPTAN;
        private string venueNaPTAN;
        private Dictionary<Language, string> navigationPathTransfers = new Dictionary<Language, string>();

        #endregion

        #region Public Properties
        /// <summary>
        /// The unique ID of this pier navigation path.
        /// </summary>
        public string NavigationID
        {
            get { return navigationID; }
            set { navigationID = value; }
        }

        /// <summary>
        /// The default time allowed for this section of a journey.
        /// </summary>
        public TimeSpan DefaultDuration
        {
            get { return defaultDuration; }
            set { defaultDuration = value; }
        }

        /// <summary>
        /// The distance in metres from the venue pier to the venue
        /// </summary>
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// To NaPTAN of the navigation path
        /// </summary>
        public string ToNaPTAN
        {
            get { return toNaPTAN; }
            set { toNaPTAN = value; }
        }

        /// <summary>
        /// From NaPTAN of the navigation path
        /// </summary>
        public string FromNaPTAN
        {
            get { return fromNaPTAN; }
            set { fromNaPTAN = value; }
        }

        /// <summary>
        /// NapTAN of the SJP venue location
        /// </summary>
        public string VenueNaPTAN
        {
            get { return venueNaPTAN; }
            set { venueNaPTAN = value; }
        }

        /// <summary>
        /// The display name of this pier navigation path. 
        /// This will be used to describe the transfer between the venue and pier
        /// </summary>
        public Dictionary<Language, string> NavigationPathTransfers
        {
            get { return navigationPathTransfers; }
            set { navigationPathTransfers = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the navigation path transfer text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="langauge"></param>
        /// <param name="isToVenue"></param>
        public void AddTransferText(string text, Language langauge)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (navigationPathTransfers.ContainsKey(langauge))
                {
                    navigationPathTransfers[langauge] = text;
                }
                else
                {
                    navigationPathTransfers.Add(langauge, text);
                }
            }
        }

        /// <summary>
        /// Returns the navigation path transfer text for the language, string.empty if doesnt exist
        /// </summary>
        public string GetTransferText(Language langauge)
        {
            if (navigationPathTransfers.ContainsKey(langauge))
            {
                return navigationPathTransfers[langauge];
            }

            return string.Empty;
        }

        #endregion
    }
}
