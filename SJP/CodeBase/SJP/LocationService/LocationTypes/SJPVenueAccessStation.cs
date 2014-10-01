// *********************************************** 
// NAME             : SJPVenueAccessStation.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: SJPVenueAccessStation class to hold details of an accessible station to use 
// when a venue's NaPTAN is not suitable for an accessible journey
// ************************************************
// 
                
using System;
using System.Collections.Generic;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// SJPVenueAccessStation class to hold details of an accessible station to use 
    /// when a venue's NaPTAN is not suitable for an accessible journey
    /// </summary>
    [Serializable]
    public class SJPVenueAccessStation
    {
        #region Private Fields

        private string stationNaPTAN = string.Empty;
        private string stationName = string.Empty;
        private Dictionary<Language, string> transferToVenue = new Dictionary<Language, string>();
        private Dictionary<Language, string> transferFromVenue = new Dictionary<Language, string>();
        
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SJPVenueAccessStation()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPVenueAccessStation(string stationNaPTAN, string stationName)
        {
            this.stationNaPTAN = stationNaPTAN;
            this.stationName = stationName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The unique NaPTAN ID of this station
        /// </summary>
        public string StationNaPTAN
        {
            get { return stationNaPTAN; }
            set { stationNaPTAN = value; }
        }

        /// <summary>
        /// The display name of this station. 
        /// </summary>
        public string StationName
        {
            get { return stationName; }
            set { stationName = value; }
        }

        /// <summary>
        /// The transfer descriptions to the venue 
        /// </summary>
        public Dictionary<Language, string> TransferToVenue
        {
            get { return transferToVenue; }
            set { transferToVenue = value; }
        }

        /// <summary>
        /// The transfer descriptions from the venue 
        /// </summary>
        public Dictionary<Language, string> TransferFromVenue
        {
            get { return transferFromVenue; }
            set { transferFromVenue = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the transfer text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="langauge"></param>
        /// <param name="isToVenue"></param>
        public void AddTransferText(string text, Language langauge, bool isToVenue)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (isToVenue)
                {
                    if (transferToVenue.ContainsKey(langauge))
                    {
                        transferToVenue[langauge] = text;
                    }
                    else
                    {
                        transferToVenue.Add(langauge, text);
                    }
                }
                else
                {
                    if (transferFromVenue.ContainsKey(langauge))
                    {
                        transferFromVenue[langauge] = text;
                    }
                    else
                    {
                        transferFromVenue.Add(langauge, text);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the transfer text for the language and direction, string.empty if doesnt exist
        /// </summary>
        /// <param name="text"></param>
        /// <param name="langauge"></param>
        /// <param name="isToVenue"></param>
        public string GetTransferText(Language langauge, bool isToVenue)
        {
            if (isToVenue)
            {
                if (transferToVenue.ContainsKey(langauge))
                {
                    return transferToVenue[langauge];
                }
            }
            else
            {
                if (transferFromVenue.ContainsKey(langauge))
                {
                    return transferFromVenue[langauge];
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
