// *********************************************** 
// NAME             : TravelNewsItem.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: TravelNewsItem class 
// ************************************************
// 
                
using System;
using System.Collections.Generic;

namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// TravelNewsItem class
    /// </summary>
    [Serializable()]
    public class TravelNewsItem
    {
        #region Private members

        private string sUid = string.Empty;
        private SeverityLevel slSeverityLevel;
        private string sSeverityDescription = string.Empty;
        private string sPublicTransportOperator = string.Empty;
        private string sOperator = string.Empty;
        private string sModeOfTransport = string.Empty;
        private string sRegions = string.Empty;
        private string sLocation = string.Empty;
        private string sRegionsLocation = string.Empty;
        private string sIncidentType = string.Empty;
        private string sHeadlineText = string.Empty;
        private string sDetailText = string.Empty;
        private string sIncidentStatus = string.Empty;
        private int iEasting = -1;
        private int iNorthing = -1;
        private DateTime dtReportedDateTime = DateTime.MinValue;
        private DateTime dtStartDateTime = DateTime.MinValue;
        private int iStartToNowMinDiff = 0;
        private DateTime dtLastModifiedDateTime = DateTime.MinValue;
        private DateTime dtClearedDateTime = DateTime.MinValue;
        private DateTime dtExpiryDateTime = DateTime.MinValue;
        private bool sPlannedIncident = false;
        private string sRoadType = string.Empty;

        // For travel news hierarchy, times of roadworks & rss functionality
        private string sIncidentParent = string.Empty;
        private string sCarriagewayDirection = string.Empty;
        private string sRoadNumber = string.Empty;

        //Default daymask to all days, tDailyStartTime to min and tDailyEndTime to max value respectively so
        //that any incidents with no values will just show according to previous criteria of expiry date etc.
        private string sDayMask = "SuMoTuWeThFrSa";
        private TimeSpan tDailyStartTime = TimeSpan.MinValue;
        private TimeSpan tDailyEndTime = TimeSpan.MaxValue;
        private string sItemChangeStatus = string.Empty;
        private IncidentActiveStatus iasIncidentActiveStatus;

        // Olympic related
        private SeverityLevel olympicSeverityLevel = SeverityLevel.VerySlight;
        private string olympicSeverityDescription = string.Empty;
        private string olympicTravelAdvice = string.Empty;
        private bool olympicIncident = false;
        private List<string> olympicVenuesAffected = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TravelNewsItem()
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
        public SeverityLevel SeverityLevel
        {
            get { return slSeverityLevel; }
            set { slSeverityLevel = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string SeverityDescription
        {
            get { return sSeverityDescription; }
            set { sSeverityDescription = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string PublicTransportOperator
        {
            get { return sPublicTransportOperator; }
            set { sPublicTransportOperator = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string Operator
        {
            get { return sOperator; }
            set { sOperator = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string ModeOfTransport
        {
            get { return sModeOfTransport; }
            set { sModeOfTransport = value; }
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
        public string Location
        {
            get { return sLocation; }
            set { sLocation = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string RegionsLocation
        {
            get { return sRegionsLocation; }
            set { sRegionsLocation = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string IncidentType
        {
            get { return sIncidentType; }
            set { sIncidentType = value; }
        }

        public string HeadlineText
        {
            get { return sHeadlineText; }
            set { sHeadlineText = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string DetailText
        {
            get { return sDetailText; }
            set { sDetailText = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string IncidentStatus
        {
            get { return sIncidentStatus; }
            set { sIncidentStatus = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public int Easting
        {
            get { return iEasting; }
            set { iEasting = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public int Northing
        {
            get { return iNorthing; }
            set { iNorthing = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public DateTime ReportedDateTime
        {
            get { return dtReportedDateTime; }
            set { dtReportedDateTime = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public DateTime StartDateTime
        {
            get { return dtStartDateTime; }
            set { dtStartDateTime = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public int StartToNowMinDiff
        {
            get { return iStartToNowMinDiff; }
            set { iStartToNowMinDiff = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public DateTime LastModifiedDateTime
        {
            get { return dtLastModifiedDateTime; }
            set { dtLastModifiedDateTime = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public DateTime ClearedDateTime
        {
            get { return dtClearedDateTime; }
            set { dtClearedDateTime = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public DateTime ExpiryDateTime
        {
            get { return dtExpiryDateTime; }
            set { dtExpiryDateTime = value; }
        }

        /// <summary>
        /// PlannedIncident to store Planned/Unplanned status of the news incident/// </summary>
        public bool PlannedIncident
        {
            get { return sPlannedIncident; }
            set { sPlannedIncident = value; }
        }

        /// <summary>
        /// Incident parent for hierarchy functionality
        /// </summary>
        public string IncidentParent
        {
            get { return sIncidentParent; }
            set { sIncidentParent = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string CarriagewayDirection
        {
            get { return sCarriagewayDirection; }
            set { sCarriagewayDirection = value; }
        }

        /// <summary>
        /// Read/Write. 
        /// </summary>
        public string RoadNumber
        {
            get { return sRoadNumber; }
            set { sRoadNumber = value; }
        }

        /// <summary>
        /// Daymask for times of roadworks functionality
        /// </summary>
        public string DayMask
        {
            get { return sDayMask; }
            set { sDayMask = value; }
        }

        /// <summary>
        /// Daily start time for times of roadworks functionality
        /// </summary>
        public TimeSpan DailyStartTime
        {
            get { return tDailyStartTime; }
            set { tDailyStartTime = value; }
        }

        /// <summary>
        /// Daily end time for times of roadworks functionality
        /// </summary>
        public TimeSpan DailyEndTime
        {
            get { return tDailyEndTime; }
            set { tDailyEndTime = value; }
        }

        /// <summary>
        /// Change status for rss functionality
        /// </summary>
        public string ItemChangeStatus
        {
            get { return sItemChangeStatus; }
            set { sItemChangeStatus = value; }
        }

        /// <summary>
        /// incident active status for times of roadworks functionality
        /// </summary>
        public IncidentActiveStatus IncidentActiveStatus
        {
            get { return iasIncidentActiveStatus; }
            set { iasIncidentActiveStatus = value; }
        }

        /// <summary>
        /// Type of road i.e M - Motorways
        /// </summary>
        public string RoadType
        {
            get { return sRoadType; }
            set { sRoadType = value; } 
        }

        /// <summary>
        /// Olympic incident flag
        /// </summary>
        public bool OlympicIncident
        {
            get { return olympicIncident; }
            set { olympicIncident = value; }
        }

        /// <summary>
        /// OlympicSeverityLevel
        /// </summary>
        public SeverityLevel OlympicSeverityLevel
        {
            get { return olympicSeverityLevel; }
            set { olympicSeverityLevel = value; }
        }

        /// <summary>
        /// OlympicSeverityDescription
        /// </summary>
        public string OlympicSeverityDescription
        {
            get { return olympicSeverityDescription; }
            set { olympicSeverityDescription = value; }
        }

        /// <summary>
        /// OlympicTravelAdvice
        /// </summary>
        public string OlympicTravelAdvice
        {
            get { return olympicTravelAdvice; }
            set { olympicTravelAdvice = value; }
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
