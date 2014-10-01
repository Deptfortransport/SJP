// *********************************************** 
// NAME             : Keys.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Keys used within this project
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.TravelNews
{
    /// <summary>
    /// Keys used within this project
    /// </summary>
    internal class Keys
    {
        /// <summary>
        /// Constructor. Does nothing.
        /// </summary>
        private Keys() { }

        // stored procedures
        public const string travelNewsHeadlinesProc = "TravelNewsHeadlines";
        public const string travelNewsAllProc = "TravelNewsAll";
        public const string travelNewsVenuesProc = "TravelNewsVenues";

        // columns
        public const string uidColumn = "UID";
        public const string severityLevelColumn = "SeverityLevel";
        public const string severityDescriptionColumn = "SeverityDescription";
        public const string severityLevelOlympicColumn = "SeverityLevelOlympic";
        public const string severityDescriptionOlympicColumn = "SeverityDescriptionOlympic";
        public const string publicTransportOperatorColumn = "PublicTransportOperator";
        public const string operatorColumn = "Operator";
        public const string modeOfTransportColumn = "ModeOfTransport";
        public const string regionsColumn = "Regions";
        public const string locationColumn = "Location";
        public const string regionsLocationColumn = "RegionsLocation";
        public const string incidentTypeColumn = "IncidentType";
        public const string headlineTextColumn = "HeadlineText";
        public const string detailTextColumn = "DetailText";
        public const string travelAdviceOlympicTextColumn = "TravelAdviceOlympicText";
        public const string incidentStatusColumn = "IncidentStatus";
        public const string eastingColumn = "Easting";
        public const string northingColumn = "Northing";
        public const string reportedDateTimeColumn = "ReportedDateTime";
        public const string startDateTimeColumn = "StartDateTime";
        public const string startToNowMinDiffColumn = "StartToNowMinDiff";
        public const string lastModifiedDateTimeColumn = "LastModifiedDateTime";
        public const string clearedDateTimeColumn = "ClearedDateTime";
        public const string expiryDateTimeColumn = "ExpiryDateTime";

        // Key for planned incidents
        public const string plannedIncidentColumn = "PlannedIncident";

        // Key for road type
        public const string roadTypeColumn = "RoadType";

        // Keys for travel news hierarchy, times of roadworks & rss functionality
        public const string IncidentParentColumn = "IncidentParent";
        public const string CarriagewayDirectionColumn = "CarriagewayDirection";
        public const string RoadNumberColumn = "RoadNumber";
        public const string DayMaskColumn = "DayMask";
        public const string DailyStartTimeColumn = "DailyStartTime";
        public const string DailyEndTimeColumn = "DailyEndTime";
        public const string ItemChangeStatusColumn = "ItemChangeStatus";
        public const string IncidentActiveStatusColumn = "IncidentActiveStatus";
    }
}
