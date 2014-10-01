// *********************************************** 
// NAME             : SJPVenueCarPark.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Apr 2011
// DESCRIPTION  	: Represents SJP venue car park location
// ************************************************
                
using System;
using System.Collections.Generic;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Represents SJP venue car park location
    /// </summary>
    [Serializable]
    public class SJPVenueCarPark : SJPPark
    {
        #region Private Fields

        private string mapOfSiteUrl;
        private int interchangeDuration = 0;
        private int coachSpaces = 0;
        private int carSpaces = 0;
        private int disabledSpaces = 0;
        private int blueBadgeSpaces = 0;
        private string driveToToid = string.Empty;
        private string driveFromToid = string.Empty;
        private List<TransitShuttle> transitShuttles = new List<TransitShuttle>();
        private Dictionary<Language, string> informationText = new Dictionary<Language, string>();

        #endregion

        #region Public Properties

        /// <summary>
        /// A URL pointing to a map of the car park site
        /// </summary>
        public string MapOfSiteUrl
        {
            get { return mapOfSiteUrl; }
            set { mapOfSiteUrl = value; }
        }

        /// <summary>
        /// The average number of minutes needed to interchange from this car park to a transit shuttle service to or from the Olympic venue.
        /// </summary>
        public int InterchangeDuration
        {
            get { return interchangeDuration; }
            set { interchangeDuration = value; }
        }

        /// <summary>
        /// The number of coach spaces situated at this car park.
        /// </summary>
        public int CoachSpaces
        {
            get { return coachSpaces; }
            set { coachSpaces = value; }
        }

        /// <summary>
        /// The number of standard car spaces situated at this car 
        /// </summary>
        public int CarSpaces
        {
            get { return carSpaces; }
            set { carSpaces = value; }
        }

        /// <summary>
        /// The number of disabled spaces situated at this car park.
        /// </summary>
        public int DisabledSpaces
        {
            get { return disabledSpaces; }
            set { disabledSpaces = value; }
        }

        /// <summary>
        /// The number of blue badge spaces situated at this car park. 
        /// </summary>
        public int BlueBadgeSpaces
        {
            get { return blueBadgeSpaces; }
            set { blueBadgeSpaces = value; }
        }

        /// <summary>
        /// The co-ordinate that will be used in car journey plans from the car park
        /// </summary>
        public string DriveToToid
        {
            get { return driveToToid; }
            set { driveToToid = value; }
        }

        /// <summary>
        /// The co-ordinate that will be used in car journey plans to the car park
        /// </summary>
        public string DriveFromToid
        {
            get { return driveFromToid; }
            set { driveFromToid = value; }
        }

        /// <summary>
        /// Transite shuttle services between SJP Venue and the SJP Car park
        /// </summary>
        public List<TransitShuttle> TransitShuttles
        {
            get { return transitShuttles; }
            set { transitShuttles = value; }
        }

        /// <summary>
        /// The information text
        /// </summary>
        public Dictionary<Language, string> InformationText
        {
            get { return informationText; }
            set { informationText = value; }
        }

        /// <summary>
        /// Adds the information text
        /// </summary>
        /// <param name="text">The text to add</param>
        /// <param name="langauge">The culture code of the text being added (FR or EN)</param>
        public void AddInformationText(string text, Language langauge)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (informationText.ContainsKey(langauge))
                {
                    informationText[langauge] = text;
                }
                else
                {
                    informationText.Add(langauge, text);
                }
            }
        }

        /// <summary>
        /// Returns the information text for the language or string.empty if it doesnt exist
        /// </summary>
        /// <param name="langauge">The culture code of the text being requested (FR or EN)</param>
        public string GetTransferText(Language language)
        {
            if (informationText.ContainsKey(language))
            {
                return informationText[language];
            }

            return string.Empty;
        }

        #endregion
    }
}
