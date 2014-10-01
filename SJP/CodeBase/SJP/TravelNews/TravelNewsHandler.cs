// *********************************************** 
// NAME             : TravelNewsHandler.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: TravelNewsHandler class that makes travel news available to clients
// ************************************************
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.UserPortal.TravelNews.SessionData;
using SJP.UserPortal.TravelNews.TravelNewsData;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.TravelNews
{
    /// <summary>
    /// TravelNewsHandler class that makes travel news available to clients.
    /// This class should not be used/instanciated directly, only use the copy in the TDServiceDiscovery.
    /// </summary>
    [Serializable()]
    public class TravelNewsHandler : ITravelNewsHandler
    {
        #region Private members

        private DataTable dataTableGrid;
        private HeadlineItem[] headlines;
        // key: incident id, value: List<venue naptans>
        private Dictionary<string, List<string>> incidentVenues;

        private bool travelNewsAvailable = false;
        private string travelNewsUnavailableText = string.Empty;
        private DateTime travelNewsLastUpdated = DateTime.MinValue;

        #endregion

        #region Contructor

        /// <summary>
        /// Get the data and store in this class instance
        /// </summary>
        public TravelNewsHandler()
        {
            #region Load from database

            try
            {
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    //open connection to TransientPortalDB
                    sqlHelper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    #region Load Travel News Venues

                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                        "TravelNews - Loading travel news venues"));

                    // Temp dictionary to set up the incidents and the venues affected by it
                    Dictionary<string, List<string>> tmpIncidentVenues = new Dictionary<string, List<string>>();

                    // Get the incidents which are affected by venues
                    using (SqlDataReader dr = sqlHelper.GetReader(Keys.travelNewsVenuesProc, new List<SqlParameter>()))
                    {
                        while (dr.Read())
                        {
                            // Read Values
                            string venueNaptan = dr["VenueNaPTAN"].ToString();
                            string incidentId = dr["UID"].ToString();

                            // Update the temp data list
                            if (!string.IsNullOrEmpty(venueNaptan))
                            {
                                // Create new list if incident hasnt been setup
                                if (!tmpIncidentVenues.ContainsKey(incidentId))
                                {
                                    tmpIncidentVenues.Add(incidentId, new List<string>());
                                }

                                // Add affected venue naptan to the incident
                                tmpIncidentVenues[incidentId].Add(venueNaptan);
                            }
                        }
                    }

                    // Assign to the class list
                    incidentVenues = tmpIncidentVenues;

                    #endregion

                    DataSet dataSet;

                    #region Load Travel News Headlines

                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                        "TravelNews - Loading travel news headlines"));

                    //use stored procedure "TravelNewsHeadlines" to return data
                    dataSet = sqlHelper.GetDataSet(Keys.travelNewsHeadlinesProc);
                    //store the resultant data table
                    DataTable dataTableHeadlines = dataSet.Tables[0];

                    DataRow[] rows = dataTableHeadlines.Select();

                    headlines = new HeadlineItem[rows.Length];

                    int i = 0;
                    foreach (DataRow row in rows)
                    {
                        headlines[i++] = BuildHeadlineItem(row);
                    }

                    #endregion

                    #region Load Travel News

                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                        "TravelNews - Loading travel news items"));

                    //use stored procedure TravelNewsGrid to fill dataset
                    dataSet = sqlHelper.GetDataSet(Keys.travelNewsAllProc);
                    //store the resultant data table
                    dataTableGrid = dataSet.Tables[0];
                    
                    #endregion

                    // Update the last updated date time
                    travelNewsLastUpdated = DateTime.Now;
                }
            }
            catch (SqlException sqlEx)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error, "SQLNUM[" + sqlEx.Number + "] :" + sqlEx.Message + ":");

                throw new SJPException("SqlException caught : " + sqlEx.Message, sqlEx,
                    false, SJPExceptionIdentifier.TNSQLHelperError);
            }
            catch (SJPException sjpEx)
            {
                string message = "Error Calling Stored Procedure : TravelNewsHeadlines " + sjpEx.Message;
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error, "SJPException :" + sjpEx.Message + ":");

                throw new SJPException(message, sjpEx, false, SJPExceptionIdentifier.TNSQLHelperStoredProcedureFailure);
            }

            #endregion

            if (dataTableGrid.Rows.Count == 1 && (string)dataTableGrid.Rows[0][Keys.uidColumn] == Values.UidUnavailable)
            {
                travelNewsUnavailableText = (string)dataTableGrid.Rows[0][Keys.headlineTextColumn];
            }
            else
            {
                travelNewsAvailable = true;
            }

            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                string.Format("TravelNews - Load completed with Headlines[{0}] Items[{1}].",
                    (dataTableGrid != null) ? dataTableGrid.Rows.Count : 0,
                    (headlines != null) ? headlines.Length : 0)));
        }

        #endregion

        #region ITravelNewsHandler methods

        /// <summary>
        /// Returns whether the travel news is available
        /// </summary>
        public bool IsTravelNewsAvaliable
        {
            get { return travelNewsAvailable; }
        }

        /// <summary>
        /// Returns the travel news unavailable text
        /// </summary>
        public string TravelNewsUnavailableText
        {
            get { return travelNewsUnavailableText; }
        }

        /// <summary>
        /// Returns the travel news last updated date time 
        /// </summary>
        public DateTime TravelNewsLastUpdated 
        {
            get { return travelNewsLastUpdated; }
        }
        
        /// <summary>
        /// Returns headline data with default filtering set to all severe ones
        /// </summary>
        /// <returns></returns>
        public HeadlineItem[] GetHeadlines()
        {
            return headlines;
        }
                
        /// <summary>
        /// Returns headline data with given filtering using 
        /// the same filtering rules used by GetDetails, which 
        /// may differ from those used by other GetHeadlines() methods
        /// </summary>
        /// <returns></returns>
        public HeadlineItem[] GetHeadlines(TravelNewsState travelNewsState, string regionsToFilter)
        {
            DataRow[] rows = dataTableGrid.Select(
                GetFilterExpression(travelNewsState, regionsToFilter), 
                Keys.severityLevelColumn + " ASC, " + Keys.startDateTimeColumn + " DESC");

            List<HeadlineItem> hlItems = new List<HeadlineItem>();

            HeadlineItem hlItem = null;
            bool valid = true;

            foreach (DataRow row in rows)
            {
                hlItem = BuildHeadlineItem(row);

                // Filter using venue
                valid = HeadlineItemValidForVenue(hlItem, travelNewsState);

                if (valid)
                    hlItems.Add(hlItem);
            }

            return hlItems.ToArray();
        }

        /// <summary>
        /// Returns filtered/sorted data grid data
        /// </summary>
        /// <remarks>
        /// Modified sort expression to include the following sort order
        /// Severity - showing the most severe incidents first
        /// Motorway incidents to appear before other road type and PT incidents
        /// Unplanned or planned - Unplanned incidents will be shown first
        /// Open incidents will be shown first, then closed and then pending
        /// Open and close incidents will be shown with newest one first
        /// Pending incidents will be show ascending in incident start time order - ignoring date
        /// </remarks>
        /// <param name="travelNewsState">Current display settings (drop-down values)</param>
        /// <returns></returns>
        public TravelNewsItem[] GetDetails(TravelNewsState travelNewsState)
        {
            string filterExpression = GetFilterExpression(travelNewsState, string.Empty);
            dataTableGrid.CaseSensitive = false;

            // Gets all the rows according to filterExpression
            DataRow[] filteredrows = dataTableGrid.Select(filterExpression);

            DataTable sortingTable = dataTableGrid.Clone();

            //importing all the filtered rows to our sortingTable
            foreach (DataRow filteredRow in filteredrows)
            {
                sortingTable.ImportRow(filteredRow);
            }

            DataRow[] rows = GetSortedRows(sortingTable);

            List<TravelNewsItem> tnItems = new List<TravelNewsItem>();

            bool valid = true;

            foreach (DataRow row in rows)
            {
                TravelNewsItem tempNewsItem = BuildTravelNewsItem(row);

                // Filter using the search phrase
                valid = TravelNewsItemValidForSearchPhrase(tempNewsItem, travelNewsState);
                
                // Filter using venue
                valid = valid && TravelNewsItemValidForVenue(tempNewsItem, travelNewsState);

                if (valid)
                    tnItems.Add(tempNewsItem);
            }

            return tnItems.ToArray();

        }

        /// <summary>
        /// Returns the TravelNewsItem associated with the given uid
        /// </summary>
        /// <param name="travelNewsState">Current display settings (drop-down values)</param>
        /// <returns></returns>
        public TravelNewsItem GetDetailsByUid(string uid)
        {
            string filterExpression = Keys.uidColumn + " = '" + uid + "'";
            DataRow[] rows = dataTableGrid.Select(filterExpression);

            // create an item if 1 row returned. Otherwise item set to Nul;
            TravelNewsItem item = (rows.Length == 1) ? BuildTravelNewsItem(rows[0]) : null;

            return item;

        }

        /// <summary>
        /// Returns children of given travel news ID
        /// </summary>
        /// <param name="uid">ID of parent to find children for</param>
        /// <returns></returns>
        public TravelNewsItem[] GetChildrenDetailsByUid(string uid)
        {
            string filterExpression = Keys.IncidentParentColumn + " = '" + uid + "'";
            DataRow[] rows = dataTableGrid.Select(filterExpression);
            ArrayList tnItems = new ArrayList();

            foreach (DataRow row in rows)
            {
                TravelNewsItem tempNewsItem = BuildTravelNewsItem(row);
                if (tempNewsItem != null)
                {
                    tnItems.Add(tempNewsItem);
                }
            }
            return (TravelNewsItem[])tnItems.ToArray(typeof(TravelNewsItem));
        }

        /// <summary>
        /// Returns filtered/sorted travel news items for web
        /// </summary>
        /// <param name="travelNewsState"></param>
        /// <param name="olympicIncidents"></param>
        /// <returns></returns>
        public TravelNewsItem[] GetDetailsForWeb(TravelNewsState travelNewsState, bool olympicIncidents)
        {
            string filterExpression = GetFilterExpression(travelNewsState, string.Empty);
            dataTableGrid.CaseSensitive = false;

            // Gets all the rows according to filterExpression
            DataRow[] filteredrows = dataTableGrid.Select(filterExpression);

            DataTable sortingTable = dataTableGrid.Clone();

            // Import all the filtered rows to the sortingTable
            foreach (DataRow filteredRow in filteredrows)
            {
                sortingTable.ImportRow(filteredRow);
            }

            // Get rows sorted, this will be by 
            // - Olympic incidents, 
            // - Olympic severity, 
            // - Severity, 
            // - Region (London, Southwest, Others)
            DataRow[] sortedRows = GetSortedRowsOlympic(sortingTable);

            List<TravelNewsItem> travelNewsItems = new List<TravelNewsItem>();

            bool valid = true;

            foreach (DataRow row in sortedRows)
            {
                TravelNewsItem tempNewsItem = BuildTravelNewsItem(row);

                // Filter for olympic incident
                if (olympicIncidents)
                    valid = tempNewsItem.OlympicIncident;
                else
                    valid = !tempNewsItem.OlympicIncident;

                // Filter using the search phrase
                valid = valid && TravelNewsItemValidForSearchPhrase(tempNewsItem, travelNewsState);

                // Filter using venue
                valid = valid && TravelNewsItemValidForVenue(tempNewsItem, travelNewsState);

                if (valid)
                    travelNewsItems.Add(tempNewsItem);
            }

            return travelNewsItems.ToArray();
        }

        /// <summary>
        /// Returns filtered/sorted travel news items for web, grouped by the SeverityLevel
        /// </summary>
        public Dictionary<SeverityLevel, List<TravelNewsItem>> GetDetailsForWebGroupedBySeverity(TravelNewsState travelNewsState, bool olympicIncidents)
        {
            Dictionary<SeverityLevel, List<TravelNewsItem>> newsBySeverity = new Dictionary<SeverityLevel, List<TravelNewsItem>>();

            List<TravelNewsItem> travelNewsItems = new List<TravelNewsItem>();

            // Get all news items for the state
            travelNewsItems.AddRange(GetDetailsForWeb(travelNewsState, olympicIncidents));

            // Group by severity
            foreach (var group in travelNewsItems.GroupBy(tn => tn.SeverityLevel))
            {
                newsBySeverity.Add(group.Key, group.ToList());
            }

            return newsBySeverity;
        }

        /// <summary>
        /// Returns filtered/sorted travel news items for mobile
        /// </summary>
        public TravelNewsItem[] GetDetailsForMobile(TravelNewsState travelNewsState)
        {
            string filterExpression = GetFilterExpression(travelNewsState, string.Empty);
            dataTableGrid.CaseSensitive = false;

            // Gets all the rows according to filterExpression
            DataRow[] filteredrows = dataTableGrid.Select(filterExpression);

            DataTable sortingTable = dataTableGrid.Clone();

            // Import all the filtered rows to the sortingTable
            foreach (DataRow filteredRow in filteredrows)
            {
                sortingTable.ImportRow(filteredRow);
            }

            // Get rows sorted, this will be by 
            // - Olympic incidents, 
            // - Olympic severity, 
            // - Severity, 
            // - Region (London, Southwest, Others)
            DataRow[] sortedRows = GetSortedRowsOlympic(sortingTable);

            List<TravelNewsItem> travelNewsItems = new List<TravelNewsItem>();

            bool valid = true;

            foreach (DataRow row in sortedRows)
            {
                TravelNewsItem tempNewsItem = BuildTravelNewsItem(row);

                // Filter using the search phrase
                valid = TravelNewsItemValidForSearchPhrase(tempNewsItem, travelNewsState);

                // Filter using venue
                valid = valid && TravelNewsItemValidForVenue(tempNewsItem, travelNewsState);

                if (valid)
                    travelNewsItems.Add(tempNewsItem);
            }
            
            return travelNewsItems.ToArray();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Build a new TravelNewsItem from a given DataRow.
        /// </summary>
        /// <param name="headlineRow">DataRow containing Travel news info. This object must have a specific compatible format with appropriate column name and type</param>
        /// <returns>a new populated TravelNewsItem</returns>
        private TravelNewsItem BuildTravelNewsItem(DataRow travelNewsRow)
        {

            TravelNewsItem item = new TravelNewsItem();
            try
            {
                item.Uid = (string)travelNewsRow[Keys.uidColumn];
                item.SeverityLevel = Parsing.ParseSeverityLevel((byte)travelNewsRow[Keys.severityLevelColumn]);
                item.SeverityDescription = (string)travelNewsRow[Keys.severityDescriptionColumn];
                if (travelNewsRow[Keys.publicTransportOperatorColumn] != DBNull.Value)
                    item.PublicTransportOperator = (string)travelNewsRow[Keys.publicTransportOperatorColumn];
                item.Operator = (string)travelNewsRow[Keys.operatorColumn];
                item.ModeOfTransport = (string)travelNewsRow[Keys.modeOfTransportColumn];
                if (travelNewsRow[Keys.regionsColumn] != DBNull.Value)
                    item.Regions = (string)travelNewsRow[Keys.regionsColumn];
                item.Location = (string)travelNewsRow[Keys.locationColumn];
                item.RegionsLocation = (string)travelNewsRow[Keys.regionsLocationColumn];
                item.IncidentType = (string)travelNewsRow[Keys.incidentTypeColumn];
                item.HeadlineText = (string)travelNewsRow[Keys.headlineTextColumn];
                item.DetailText = (string)travelNewsRow[Keys.detailTextColumn];
                item.IncidentStatus = (string)travelNewsRow[Keys.incidentStatusColumn];

                item.RoadType = (string)travelNewsRow[Keys.roadTypeColumn];

                decimal tempEast = (decimal)travelNewsRow[Keys.eastingColumn];
                decimal tempNorth = (decimal)travelNewsRow[Keys.northingColumn];
                item.Easting = (int)tempEast;
                item.Northing = (int)tempNorth;

                item.ReportedDateTime = (DateTime)travelNewsRow[Keys.reportedDateTimeColumn];
                item.StartDateTime = (DateTime)travelNewsRow[Keys.startDateTimeColumn];
                item.StartToNowMinDiff = (int)travelNewsRow[Keys.startToNowMinDiffColumn];
                item.LastModifiedDateTime = (DateTime)travelNewsRow[Keys.lastModifiedDateTimeColumn];
                if (travelNewsRow[Keys.clearedDateTimeColumn] != DBNull.Value)
                    item.ClearedDateTime = (DateTime)travelNewsRow[Keys.clearedDateTimeColumn];
                if (travelNewsRow[Keys.expiryDateTimeColumn] != DBNull.Value)
                    item.ExpiryDateTime = (DateTime)travelNewsRow[Keys.expiryDateTimeColumn];

                // setting up planned incident property
                item.PlannedIncident = (bool)travelNewsRow[Keys.plannedIncidentColumn];

                // members for travel news hierarchy, times of roadworks & rss functionality
                if (travelNewsRow[Keys.IncidentParentColumn] != DBNull.Value)
                {
                    item.IncidentParent = (string)travelNewsRow[Keys.IncidentParentColumn];
                }
                if (travelNewsRow[Keys.CarriagewayDirectionColumn] != DBNull.Value)
                {
                    item.CarriagewayDirection = (string)travelNewsRow[Keys.CarriagewayDirectionColumn];
                }
                if (travelNewsRow[Keys.RoadNumberColumn] != DBNull.Value)
                {
                    item.RoadNumber = (string)travelNewsRow[Keys.RoadNumberColumn];
                }

                if (travelNewsRow[Keys.DayMaskColumn] != DBNull.Value)
                {
                    if ((string)travelNewsRow[Keys.DayMaskColumn] != "")
                    {
                        item.DayMask = (string)travelNewsRow[Keys.DayMaskColumn];
                    }
                }

                if (travelNewsRow[Keys.DailyStartTimeColumn] != DBNull.Value)
                {
                    item.DailyStartTime = (TimeSpan)travelNewsRow[Keys.DailyStartTimeColumn];
                }
                if (travelNewsRow[Keys.DailyEndTimeColumn] != DBNull.Value)
                {
                    item.DailyEndTime = (TimeSpan)travelNewsRow[Keys.DailyEndTimeColumn];
                }

                if (travelNewsRow[Keys.ItemChangeStatusColumn] != DBNull.Value)
                {
                    item.ItemChangeStatus = (string)travelNewsRow[Keys.ItemChangeStatusColumn];
                }

                if (travelNewsRow[Keys.IncidentActiveStatusColumn] != DBNull.Value)
                {
                    if ((bool)travelNewsRow[Keys.IncidentActiveStatusColumn])
                    { item.IncidentActiveStatus = IncidentActiveStatus.Active; }
                    else
                    { item.IncidentActiveStatus = IncidentActiveStatus.Inactive; }
                }

                // Olympic related values
                item.OlympicSeverityLevel = Parsing.ParseSeverityLevel((byte)travelNewsRow[Keys.severityLevelOlympicColumn]);
                item.OlympicSeverityDescription = (string)travelNewsRow[Keys.severityDescriptionOlympicColumn];
                item.OlympicTravelAdvice = (string)travelNewsRow[Keys.travelAdviceOlympicTextColumn];
                
                // Olympic venues affected (if exists)
                if (incidentVenues.ContainsKey(item.Uid))
                {
                    item.OlympicVenuesAffected = incidentVenues[item.Uid];
                    item.OlympicIncident = item.OlympicVenuesAffected.Count > 0;
                }

            }

            catch (InvalidCastException)
            {
                OperationalEvent oe = new OperationalEvent(
                    SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error,
                    "At least one column name in the TravelNews DB hasn't got a consistent type with the one expected. Check these types.");
                Logger.Write(oe);
            }
            catch (IndexOutOfRangeException)
            {
                OperationalEvent oe = new OperationalEvent(
                    SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error,
                    "At least one column name in the TravelNews DB is not consistent with the ones expected. Check these column names.");
                Logger.Write(oe);
            }

            catch (Exception exc)
            {
                OperationalEvent oe = new OperationalEvent(
                    SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error,
                    string.Format("An exception was thrown while building a TravelNewsItem. Reason: {0}", exc.Message));
                Logger.Write(oe);

            }

            return item;
        }

        /// <summary>
        /// Build a new HeadlineItem from a given DataRow.
        /// </summary>
        /// <param name="headlineRow">DataRow containing headlines info. This object must have a specific compatible format with appropriate column name and type</param>
        /// <returns>a new populated HeadlineItem</returns>
        private HeadlineItem BuildHeadlineItem(DataRow headlineRow)
        {
            HeadlineItem item = new HeadlineItem();

            try
            {
                item.Uid = (string)headlineRow[Keys.uidColumn];
                item.HeadlineText = (string)headlineRow[Keys.headlineTextColumn];
                item.SeverityLevel = Parsing.ParseSeverityLevel((byte)headlineRow[Keys.severityLevelColumn]);
                // if value of ModeOfTransport column is equal to road, then assign TransportType road, otherwise assign Public transport
                item.TransportType = ((string)headlineRow[Keys.modeOfTransportColumn] == Values.Road) ? TransportType.Road : TransportType.PublicTransport;
                item.Regions = (string)headlineRow[Keys.regionsColumn];
                item.DelayTypes = GetDelayTypes((byte)headlineRow[Keys.severityLevelColumn], (int)headlineRow[Keys.startToNowMinDiffColumn]);

                // Olympic venues affected (if exists)
                if (incidentVenues.ContainsKey(item.Uid))
                {
                    item.OlympicVenuesAffected = incidentVenues[item.Uid];
                }
            }
            catch (InvalidCastException)
            {
                OperationalEvent oe = new OperationalEvent(
                    SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error,
                    "At least one column name in the TravelNews DB hasn't got a consistent type with the one expected. Check these types.");
                Logger.Write(oe);
            }
            catch (IndexOutOfRangeException)
            {
                OperationalEvent oe = new OperationalEvent(
                    SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error,
                    "At least one column name in the TravelNews DB is not consistent with the ones expected. Check these column names.");
                Logger.Write(oe);
            }

            catch (Exception exc)
            {
                OperationalEvent oe = new OperationalEvent(
                    SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error,
                    string.Format("An exception was thrown while building a HeadlineItem. Reason: {0}", exc.Message));
                Logger.Write(oe);

            }
            return item;
        }

        /// <summary>
        /// Returns an array with Delay Type filters applicable for the given severity and timespan
        /// </summary>
        /// <param name="severityValue">severity level</param>
        /// <param name="startToNowMinDiffValue">difference in minutes between</param>
        /// <returns></returns>
        private DelayType[] GetDelayTypes(byte severityValue, int startToNowMinDiffValue)
        {
            bool isSevere = false;
            bool isRecent = false;

            if (((int)severityValue) < Convert.ToInt32(Enum.Format(typeof(SeverityLevel), SeverityLevel.VerySevere, "d")))
                isSevere = true;


            if (startToNowMinDiffValue < Values.IntRecentCritera)
                isRecent = true;


            if (isSevere && isRecent)
            {
                return new DelayType[] { DelayType.Major, DelayType.Recent };
            }
            if (isSevere)
            {
                return new DelayType[] { DelayType.Major };
            }
            if (isRecent)
            {
                return new DelayType[] { DelayType.Recent };
            }

            return new DelayType[0];

        }

        /// <summary>
        /// Builds and returns the filter expression to filter the grid data table
        /// </summary>
        /// <param name="regionsToFilter">Regions to filter when the travel news state selected regions set to 'All'</param>
        private string GetFilterExpression(TravelNewsState travelNewsState, string regionsToFilter)
        {
            // Array list object to hold each filter. Why use a string array? This is much tidier and
            // has a very small performance overhead and we just turn the array list to an array at the 
            // end of the process. No fixed array size, counters and empty objects!
            ArrayList filterList = new ArrayList();
            string filterExpression = string.Empty;

            // Severity column filter
            filterList.Add(Keys.severityLevelColumn + " <> " + Convert.ToInt32(Enum.Format(typeof(SeverityLevel), SeverityLevel.Critical, "d")));

            if (travelNewsState.SelectedSeverityFilter == SeverityFilter.CriticalIncidents)
            {
                // Critical incidents filter - on click through from critical incident DN states "...show all critical child 
                // entries for display." so we can ignore all other filter parameters & just select all "Serious" incidents.
                filterList.Add(Keys.severityLevelColumn + " = " + Enum.Format(typeof(SeverityLevel), SeverityLevel.Serious, "d"));
            }
            else
            // Not a critical incident click through therefore build filter as normal
            {
                #region Transport type filter

                // Modes of transport filter
                switch (travelNewsState.SelectedTransport)
                {
                    case (TransportType.All):
                        break;
                    case (TransportType.PublicTransport):
                        filterList.Add(Keys.modeOfTransportColumn + " <> '" + Values.Road + "'");
                        break;
                    default:
                        filterList.Add(Keys.modeOfTransportColumn + " = '" + Converting.ToString(travelNewsState.SelectedTransport) + "'");
                        break;
                }

                #endregion

                #region Region filter

                // Selected Region Filter
                switch (travelNewsState.SelectedRegion)
                {
                    case (""):
                    case ("All"):
                        if (!string.IsNullOrEmpty(regionsToFilter))
                        {
                            string regionFilterExpression = GetRegionsFilter(regionsToFilter);
                            if (!string.IsNullOrEmpty(regionFilterExpression))
                            {
                                filterList.Add(regionFilterExpression);
                            }
                        }
                        break;
                    default:
                        filterList.Add(Keys.regionsColumn + " LIKE '%" + travelNewsState.SelectedRegion + "%'");
                        break;
                }

                #endregion

                #region Delays filter

                switch (travelNewsState.SelectedDelays)
                {
                    case (DelayType.Major):
                        filterList.Add(Keys.severityLevelColumn + " <= " + Convert.ToInt32(Enum.Format(typeof(SeverityLevel), SeverityLevel.Severe, "d")));
                        break;
                    case (DelayType.Recent):
                        filterList.Add(Keys.startToNowMinDiffColumn + " < " + Values.RecentCriteria);
                        break;
                }

                #endregion

                #region Planned incident filter

                // Modes of incidentType filter
                switch (travelNewsState.SelectedIncidentType)
                {
                    case (IncidentType.All):
                        break;
                    case (IncidentType.Planned):
                        filterList.Add(Keys.plannedIncidentColumn + " = 1");
                        break;
                    case (IncidentType.Unplanned):
                        filterList.Add(Keys.plannedIncidentColumn + " = 0");
                        break;
                }

                #endregion

                #region Date filter

                // Filter for travelNewsState.SelectedDate
                if (travelNewsState.SelectedDate != null)
                {
                    filterList.Add(Keys.startDateTimeColumn + " <= " + "'" + travelNewsState.SelectedDate.ToString("yyyy-MM-dd") + " 23:59:59'" + " AND " + Keys.expiryDateTimeColumn + " >= " + "'" + travelNewsState.SelectedDate.ToString("yyyy-MM-dd") + " 00:00:00'");
                }

                #endregion

                #region Active incidents only filter

                switch (travelNewsState.SelectedIncidentActive)
                {

                    case ("Active"): //IncidentActiveStatus.Active
                        filterList.Add(Keys.IncidentActiveStatusColumn + " = 1");
                        break;
                    case ("Inactive"): //IncidentActiveStatus.Inactive
                        filterList.Add(Keys.IncidentActiveStatusColumn + " = 0");
                        break;
                    case ("All"):
                    default:
                        // Do not filter on active status
                        break;
                }

                #endregion

                #region Search phrase filter

                // Clear search tokens from page state
                travelNewsState.SearchTokens = null;

                // If a search expression has been entered then parse it
                if (travelNewsState.SearchPhrase.Length > 0)
                {
                    travelNewsState.SelectedRegion = "All";
                    TravelNewsSearchParser parser = new TravelNewsSearchParser();
                    ArrayList tokens = parser.GetSearchTokens(travelNewsState.SearchPhrase);

                    // Search is done outside of the datatable.select filter expression, using regular expressions.
                    // So add the token keys to the travel news state to be used later
                    travelNewsState.SearchTokens = tokens;
                }

                #endregion
            }

            // Compile complete list of filters.
            foreach (string filter in filterList)
            {
                // If filter express is not empty then add an AND
                // onto the end of the filter.
                if (filterExpression != string.Empty)
                {
                    filterExpression += " AND ";
                }

                // Add the new filter in.
                filterExpression += filter;
            }

            return filterExpression;
        }

        /// <summary>
        /// Builds a region filter expression from the comma delimited string of regions
        /// </summary>
        /// <param name="regionsToFilter">Comma delimited string of regions</param>
        /// <returns>Filter sub expression to filter the grid data table</returns>
        private string GetRegionsFilter(string regionsToFilter)
        {
           
            // Build a new search filter based on these phrases
            StringBuilder filter = new StringBuilder();

            if (!string.IsNullOrEmpty(regionsToFilter))
            {
                // Start of filter expression
                filter.Append("(");

                // Headline expression
                StringBuilder filterExpression = new StringBuilder();

                foreach (string token in regionsToFilter.Split(new char[]{','}))
                {
                    if (filterExpression.Length > 0)
                    {
                        // We have added one headline statement so add an OR statement
                        filterExpression.Append(" OR ");
                    }

                    filterExpression.Append(Keys.regionsColumn);
                    filterExpression.Append(" LIKE '%");
                    filterExpression.Append(token);
                    filterExpression.Append("%'");
                }

                // Add headline to the filter
                filter.Append(filterExpression.ToString().ToLower());

                // Or express between hadline and detail
                filter.Append(" ) ");
            }

            return filter.ToString();
        }

        /// <summary>
        /// Returns sorted rows based on 
        /// Severity, Road type (motorways first), Incident status (open first)
        /// </summary>
        /// <param name="sortingTable"> Table with filtered travel news data</param>
        /// <returns>Sorted data rows</returns>
        private DataRow[] GetSortedRows(DataTable sortingTable)
        {
            sortingTable = GetUpdatedSortingTable(sortingTable);

            string sortExpression = string.Format("{0} ASC, {1} ASC, {2} ASC, {3} ASC, {4} ASC, {5} DESC",
                Keys.severityLevelColumn,
                "RoadTypeSortOrder",
                Keys.plannedIncidentColumn,
                "IncidentSortOrder",
                "TimeColumn",
                Keys.startDateTimeColumn
                );

            return sortingTable.Select(null, sortExpression);
        }

        /// <summary>
        /// Returns sorted rows based on 
        /// Olympic Severity, Severity, Road type (motorways first), Incident status (open first)
        /// </summary>
        /// <param name="sortingTable"> Table with filtered travel news data</param>
        /// <returns>Sorted data rows</returns>
        private DataRow[] GetSortedRowsOlympic(DataTable sortingTable)
        {
            sortingTable = GetUpdatedSortingTable(sortingTable);

            string sortExpression = string.Format("{0} DESC, {1} ASC, {2} ASC, {3} ASC, {4} ASC, {5} ASC, {6} ASC, {7} ASC, {8} DESC",
                "OlympicIncident",
                Keys.severityLevelOlympicColumn,
                Keys.severityLevelColumn,
                "RegionSortOrder",
                "RoadTypeSortOrder",
                Keys.plannedIncidentColumn,
                "IncidentSortOrder",
                "TimeColumn",
                Keys.startDateTimeColumn
                );

            return sortingTable.Select(null, sortExpression);
        }

        /// <summary>
        /// Manually adds columns and data to table to allow sorting based on 
        /// Road type (motorways first), Incident status (open first), Olympic incident
        /// </summary>
        /// <param name="sortingTable"></param>
        /// <returns></returns>
        private DataTable GetUpdatedSortingTable(DataTable sortingTable)
        {
            // Creating new columns to enable sorting on IncidentStatus and DateTime
            sortingTable.Columns.Add(new DataColumn("TimeColumn", typeof(DateTime)));
            sortingTable.Columns.Add(new DataColumn("IncidentSortOrder", typeof(int)));
            sortingTable.Columns.Add(new DataColumn("RoadTypeSortOrder", typeof(int)));
            sortingTable.Columns.Add(new DataColumn("OlympicIncident", typeof(int)));
            sortingTable.Columns.Add(new DataColumn("RegionSortOrder", typeof(int)));

            string london = TravelNewsRegion.London.ToString().ToLower();
            string southwest = TravelNewsRegion.SouthWest.ToString().ToLower();

            // fill new columns with TimeColumn as todays date with no time if incident is
            // of type open or close and with the time as StartDateTime Column's time if incident is pending
            // IncidentSortOrder column will be 0 if Open, 2 if pending and 1 if closed to enable sorting in open, closed, pending order
            foreach (DataRow srow in sortingTable.Rows)
            {
                // Incident open status
                switch (srow[Keys.incidentStatusColumn].ToString().Trim())
                {
                    case "O":
                        //Even if marked as open by trafficlink still treat as pending if not actually active at current time
                        if ((bool)srow[Keys.IncidentActiveStatusColumn])
                        {
                            srow["IncidentSortOrder"] = 0;
                            srow["TimeColumn"] = DateTime.Today.Date;
                        }
                        else
                        {
                            goto case "P";
                        }
                        break;
                    case "P":
                        DateTime stDate = DateTime.Parse(srow[Keys.startDateTimeColumn].ToString());
                        srow["IncidentSortOrder"] = 2;
                        srow["TimeColumn"] = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, stDate.Hour, stDate.Minute, stDate.Second);
                        break;
                    case "C":
                        srow["IncidentSortOrder"] = 1;
                        srow["TimeColumn"] = DateTime.Today.Date;
                        break;
                }

                // Road type
                if (srow[Keys.roadTypeColumn].ToString().Trim().ToUpper() == "M")
                {
                    srow["RoadTypeSortOrder"] = 0;
                }
                else
                    srow["RoadTypeSortOrder"] = 1;

                // Olympic incident if incident has venues affected
                if (incidentVenues.ContainsKey(srow[Keys.uidColumn].ToString()))
                {
                    srow["OlympicIncident"] = 1;
                }
                else
                {
                    srow["OlympicIncident"] = 0;
                }

                // Region (London, South West, others) - specific for olympic news sorting
                string regions = srow[Keys.regionsColumn].ToString().Trim().Replace(" ", string.Empty).ToLower();
                if (regions.Contains(london))
                {
                    srow["RegionSortOrder"] = 0;
                }
                else if (regions.Contains(southwest))
                {
                    srow["RegionSortOrder"] = 1;
                }
                else
                {
                    srow["RegionSortOrder"] = 2;
                }
            }

            return sortingTable;
        }
        
        /// <summary>
        /// Method checks if the search phrase in the travel news state applies to the travel news item.
        /// If it does, the item is updated with the phrase "highlighted",
        /// Else null is returned indicating the news item is not vaild
        /// otherwise 
        /// </summary>
        /// <param name="tempNewsItem"></param>
        /// <returns></returns>
        private bool TravelNewsItemValidForSearchPhrase(TravelNewsItem tempNewsItem, TravelNewsState travelNewsState)
        {
            // Filter list again based on search terms
            // Search tokens should have been set in the method GetFilterExpression!
            if (travelNewsState.SearchTokens != null)
            {
                int tokenCount = travelNewsState.SearchTokens.Count;

                // Check travel news contains required search terms
                // if not then set it to null and it wont be
                // returned from this procedure
                string combinedItemDetail = tempNewsItem.Location.ToUpper() + " " + tempNewsItem.DetailText.ToUpper();
                combinedItemDetail = combinedItemDetail.Replace("(M)", "M");

                //combinedItemDetail.con
                bool fullMatch = false;
                int locatedTokens = 0;

                foreach (string token in travelNewsState.SearchTokens)
                {
                    string newToken = token;
                    //string newToken = token; //.Replace("(M)", "M");
                    Regex wordMatch = new Regex(@"\b" + newToken + @"\b", RegexOptions.IgnoreCase);

                    if (wordMatch.IsMatch(combinedItemDetail))
                    {
                        // We have matched a token against the text so we need to
                        // see what type of token it is and replace the text in the 
                        // travel news with the correct highlighted text.

                        // Create regex's to match the tokens to road names
                        Regex roadMatch = new Regex(@"[aA][0-9]+[mM]");

                        // If the token is a road name in this format Ann(M) then we need
                        // to feed a suitable regex into the match evaluator to highlight it.
                        if (roadMatch.IsMatch(newToken))
                        {
                            // If the token is in that format then we need to modify the token
                            // before feeding it into the highlight evaluator
                            newToken = newToken.ToUpper().Replace("M", "\\(M\\)");
                        }

                        SearchTokenMatchEvaluator c = new SearchTokenMatchEvaluator();
                        Regex ex = new Regex(@newToken, RegexOptions.IgnoreCase);
                        MatchEvaluator evaluator = new MatchEvaluator(c.TokenHighlighter);
                        string newLocation = ex.Replace(tempNewsItem.Location, evaluator);
                        string newDetailText = ex.Replace(tempNewsItem.DetailText, evaluator);
                        tempNewsItem.Location = newLocation;
                        tempNewsItem.DetailText = newDetailText;
                        locatedTokens++;
                    }
                }

                fullMatch = (locatedTokens == tokenCount);

                if (!fullMatch)
                {
                    // Search phrase not matched
                    return false;
                }
            }

            // Search phrase found (or not specified), so a match
            return true;
        }

        /// <summary>
        /// Method checks if the venues in the travel news state applies to the travel news item.
        /// If venues not specified, then true by default
        /// </summary>
        /// <param name="tempNewsItem"></param>
        /// <returns></returns>
        private bool TravelNewsItemValidForVenue(TravelNewsItem tempNewsItem, TravelNewsState travelNewsState)
        {
            if (travelNewsState.SelectedVenuesFlag && travelNewsState.SearchNaptans.Count > 0)
            {
                string[] matches = (tempNewsItem.OlympicVenuesAffected.Intersect(travelNewsState.SearchNaptans, StringComparer.InvariantCultureIgnoreCase)).ToArray();

                return (matches.Length > 0);
            }

            return true;
        }

        /// <summary>
        /// Method checks if the venues in the travel news state applies to the headline item.
        /// If venues not specified, then true by default
        /// </summary>
        /// <param name="tempNewsItem"></param>
        /// <returns></returns>
        private bool HeadlineItemValidForVenue(HeadlineItem tempHeadlineItem, TravelNewsState travelNewsState)
        {
            if (travelNewsState.SelectedVenuesFlag && travelNewsState.SearchNaptans.Count > 0)
            {
                string[] matches = (tempHeadlineItem.OlympicVenuesAffected.Intersect(travelNewsState.SearchNaptans, StringComparer.InvariantCultureIgnoreCase)).ToArray();

                return (matches.Length > 0);
            }

            return true;
        }

        #endregion
    }
}
