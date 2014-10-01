// *********************************************** 
// NAME             : HeadlineItem.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Represents a travel news headline
// ************************************************
// 
                
using System;
using System.Collections.Generic;

namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// Represents a travel news headline
    /// </summary>
    [Serializable()]
    public class HeadlineItem
    {
        #region Private members

        private string sUid = string.Empty;
        private string sHeadlineText = string.Empty;
        private SeverityLevel slSeverityLevel;
        private TransportType ttTransportType = TransportType.All;
        private string sRegions = string.Empty;
        private DelayType[] dltDelayTypes = new DelayType[0];
        private List<string> olympicVenuesAffected = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public HeadlineItem()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write.
        /// </summary>
        public string Uid
        {
            get { return sUid; }
            set { sUid = value; }
        }

        /// <summary>
        /// Read/Write.
        /// </summary>
        public string HeadlineText
        {
            get { return sHeadlineText; }
            set { sHeadlineText = value; }
        }

        /// <summary>
        /// Read/Write.
        /// </summary>
        public SeverityLevel SeverityLevel
        {
            get { return slSeverityLevel; }
            set { slSeverityLevel = value; }
        }

        /// <summary>
        /// Read/Write.
        /// </summary>
        public TransportType TransportType
        {
            get { return ttTransportType; }
            set { ttTransportType = value; }
        }

        /// <summary>
        /// Read/Write.
        /// </summary>
        public string Regions
        {
            get { return sRegions; }
            set { sRegions = value; }
        }

        /// <summary>
        /// Read/Write.
        /// </summary>
        public DelayType[] DelayTypes
        {
            get { return dltDelayTypes; }
            set { dltDelayTypes = value; }
        }

        /// <summary>
        /// OlympicVenuesAffected
        /// </summary>
        public List<string> OlympicVenuesAffected
        {
            get { return olympicVenuesAffected; }
            set { olympicVenuesAffected = value; }
        }

        #endregion
    }
}