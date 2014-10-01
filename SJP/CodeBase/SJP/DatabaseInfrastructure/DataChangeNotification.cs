// *********************************************** 
// NAME             : DataChangeNotification.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Mar 2011
// DESCRIPTION  	: DataChangeNotification class. Provides a polling service that
// checks for updates to ChangeNotificiation tables
// in nominated databases, and raises an event when
// a change occurs. In order to run, this service requires 
// at least the PollingInterval property to be present and
// hold a valid value, and for the Groups property to be
// present (can be null).
// NOTE: This code requires that the ChangeNotification
// table and GetChangeTable stored procedure have been
// created in the nominated database(s).
// ************************************************
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Timers;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.DatabaseInfrastructure
{
    /// <summary>
    /// DataChangeNotification class to provide a polling service that checks 
    /// for updates to ChangeNotificiation tables in nominated databases, 
    /// and raises an event when a change occurs
    /// </summary>
    public class DataChangeNotification : IDataChangeNotification, IDisposable
    {
        #region Private members

        /// <summary>
        /// Stored proceduce
        /// </summary>
        private string SP_GetChangeTable = "GetChangeTable";

        /// <summary>
        /// Properties
        /// </summary>
        private string PROP_PollingInterval = "DataNotification.PollingInterval.Seconds";
        private string PROP_Groups = "DataNotification.Groups";
        private string PROP_Database = "DataNotification.{0}.Database";
        private string PROP_Tables = "DataNotification.{0}.Tables";

        /// <summary>
        /// Hashtable that holds the sqlhelperDB object (key), and another
        /// hashtable (value) that contains a copy of the ChangeNotification table
        /// read from the database on initialisation.
        /// </summary>
        private Hashtable changeNotificationTables;

        /// <summary>
        /// Holds DataChangeNotificationGroup objects
        /// </summary>
        private ArrayList changeGroups;

        /// <summary>
        /// Timer that fires an elapsed event whenever the configurable PollingInterval
        /// elapses, in order to execute the Poll method (listener).
        /// </summary>
        private Timer pollingTimer = null;

        /// <summary>
        /// Configurable duration (secs) between elapsed events. Configured by property service.
        /// </summary>
        private int pollingInterval = 0;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// The class constructor exeutes the Initialise method, which uses the Property Service
        /// to: initialise the class with the Polling Interval and list of groups defined in the
        /// Properties Database; perform initialisation checks on the nominated Databases;
        /// instantiate required DataChangeNotificationGroup classes (one per group).
        /// The Constructor then starts the pollingTimer, which fires an elapsed event every
        /// polling interval, which is handled by the Poll Event Handler. 
        /// </summary>
        public DataChangeNotification()
        {
            //Instantiate a new timer
            pollingTimer = new Timer();

            // Execute Initialise method to perform initial data load
            Initialise();

            // Set up Polling via a timer using Polling Interval in properties DB
            // Polling interval in the database is in seconds, we need to convert to milliseconds
            pollingTimer.Interval = (double)pollingInterval * 1000;
            pollingTimer.AutoReset = false;

            // Initialisation code to connect to the Elapsed EventHandler
            pollingTimer.Elapsed += new ElapsedEventHandler(this.Poll);

            // Start timer
            pollingTimer.Start();

            // This exception will be thrown if there is no/null PollingInterval property.
            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info,
                "DataChangeNotification Polling started"));
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Groups being monitored for changes
        /// </summary>
        internal ArrayList ChangeGroups
        {
            get { return (ArrayList)this.changeGroups.Clone(); }
        }

        /// <summary>
        /// Interval between polling for changes (milliseconds)
        /// </summary>
        internal int PollInterval
        {
            get { return Convert.ToInt32(pollingTimer.Interval); }
        }

        /// <summary>
        /// Tables being monitored for changes
        /// </summary>
        internal Hashtable ChangeNotificationTables
        {
            get { return (Hashtable)changeNotificationTables.Clone(); }
        }

        #endregion

        #region Change Event

        /// <summary>
        /// Event raised when a change is detected
        /// </summary>
        public event ChangedEventHandler Changed;

        /// <summary>
        /// Raises the Changed event to notify clients of a change in the 
        /// database concerned, provided that the delegate exists.
        /// </summary>
        /// <param name="e"></param>
        private void OnChanged(ChangedEventArgs e)
        {
            // If delegate not null raise event
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Initialises the class by using the Property Service to get:
        /// 1) The list of groups, and their associated tables
        /// 2) The polling interval (in seconds)
        /// 3) The Database and nominated tables for each group
        /// It then creates one group object per group that stores the Database
        /// and Tables of the group (later used for checking for changes).
        /// </summary>
        protected void Initialise()
        {
            string[] groupsArray = null;
            int noOfGroups = 0;

            // Read properties
            string pollingIntervalString = Properties.Current[PROP_PollingInterval];
            string strAllGroups = Properties.Current[PROP_Groups];

            #region PollingInterval

            if (string.IsNullOrEmpty(pollingIntervalString))
            {
                // This exception will be thrown if there is no/null PollingInterval property.
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                    "DataChangeNotification PollingInterval not found in Properties service."));

                //Throw SJPException error
                throw new SJPException("DataChangeNotification PollingInterval not found in Properties service.",
                    true, SJPExceptionIdentifier.DSPollingIntervalNotFound);
            }

            try
            {
                // Cast and store PollingInterval from properties table in DB
                pollingInterval = Convert.ToInt32(pollingIntervalString);
                if (SJPTraceSwitch.TraceInfo)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info,
                        string.Format("DataChangeNotification PollingInterval set to [{0}] seconds", pollingInterval)));
                }
            }
            catch (FormatException e)
            {
                // This exception will be thrown if there is no/null PollingInterval property.
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                    "DataChangeNotification PollingInterval not a valid integer.", e));

                //Throw SJPException error
                throw new SJPException("DataChangeNotification PollingInterval not a valid integer.",
                    e, true, SJPExceptionIdentifier.DSPollingIntervalNotFound);
            }

            #endregion

            #region Groups

            // Validate list of groups
            if (strAllGroups == null)
            {
                // Raise error
                // This exception will be thrown if there is no/null Groups property.
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                    "DataChangeNotification Groups property is null or not found in Properties table."));

                //Throw SJPException error
                throw new SJPException("DataChangeNotification Groups property is null or not found in Properties table.",
                    true, SJPExceptionIdentifier.DSGroupsNotFound);

            }

            // If not null, split values & store in an array
            if (strAllGroups.Trim().Length > 0)
            {
                groupsArray = strAllGroups.Split(",".ToCharArray());

                ArrayList processedGroups = new ArrayList();
                
                // For logging
                StringBuilder groupsSB = new StringBuilder();

                //trim off whitespace
                foreach (string strGroup in groupsArray)
                {
                    string groupCur = strGroup.Trim();
                    if (groupCur.Length != 0 && !processedGroups.Contains(groupCur))
                    {
                        processedGroups.Add(groupCur);
                        
                        groupsSB.Append(groupCur);
                        groupsSB.Append(",");
                    }
                }

                // If the number of processed (trimmed) groups is not equal 
                // to the original number of groups from the DB, log a warning
                if ((groupsArray.Length != processedGroups.Count) && SJPTraceSwitch.TraceWarning)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database,
                        SJPTraceLevel.Warning, "DataChangeNotification Groups property contained zero-length or duplicate entries."));
                }

                // Copy the processed groups back into the groupsArray
                groupsArray = (string[])processedGroups.ToArray(typeof(string));
                noOfGroups = groupsArray.Length;

                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, 
                        string.Format("DataChangeNotification Groups[{0}]", groupsSB.ToString().TrimEnd(','))));
                }

            }
            else
            {
                // If groups property is present but null, initialise as null for now and continue
                groupsArray = new string[0];
            }

            #endregion

            #region Tables

            string currDatabaseName;
            string strTables;
            string[] currTables;
            SqlHelperDatabase currDatabase = new SqlHelperDatabase();
            DataChangeNotificationGroup currGroup;

            //Instance of SQLHelper to handle DB connection
            using (SqlHelper sql = new SqlHelper())
            {

                this.changeNotificationTables = new Hashtable();
                this.changeGroups = new ArrayList(noOfGroups);

                foreach (string s in groupsArray)
                {
                    // Read properties to determine Database for the group
                    currDatabaseName = Properties.Current[string.Format(PROP_Database, s)];

                    // Parse the database name to get a SqlHelperDatabase enum type
                    currDatabase = ParseDatabase(currDatabaseName);

                    // Read properties to determine Tables for the group
                    strTables = Properties.Current[string.Format(PROP_Tables, s)];

                    // If not null, split values & store in an array
                    if (strTables.Trim().Length > 0)
                    {
                        currTables = strTables.Split(",".ToCharArray());

                        ArrayList processedTables = new ArrayList();
                        
                        // For logging
                        StringBuilder tablesSB = new StringBuilder();

                        //trim off whitespace
                        foreach (string strTable in currTables)
                        {
                            // If the number of processed (trimmed) table is not equal 
                            // to the original number of tables from the DB, log a warning
                            string tableCur = strTable.Trim();
                            if (tableCur.Length != 0 && !processedTables.Contains(tableCur))
                            {
                                processedTables.Add(tableCur);
                                tablesSB.Append(tableCur);
                                tablesSB.Append(',');
                            }
                        }

                        if ((currTables.Length != processedTables.Count) && SJPTraceSwitch.TraceWarning)
                        {
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Warning, 
                                "DataChangeNotification Tables property contained zero-length or duplicate entries."));
                        }

                        // Copy the processed groups back into the groupsArray
                        currTables = (string[])processedTables.ToArray(typeof(string));
                        noOfGroups = currTables.Length;
                        
                        if (SJPTraceSwitch.TraceInfo)
                        {
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, 
                                string.Format("DataChangeNotification Change data for Group[{0}] Tables[{1}]",s, tablesSB.ToString().TrimEnd(','))));                            
                        }

                    }
                    else
                    {
                        // If groups property is present but null, initialise as null for now and continue
                        currTables = new string[0];
                    }

                    currTables = strTables.Split(",".ToCharArray());

                    // Create Group object and add to changeGroups list.
                    currGroup = new DataChangeNotificationGroup(s, currDatabase, currTables);
                    changeGroups.Add(currGroup);

                    //If we have not already gathered the change notification info for this
                    //data base
                    if (!changeNotificationTables.ContainsKey(currGroup.DataBase))
                    {
                        try
                        {
                            if (SJPTraceSwitch.TraceVerbose)
                            {
                                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, 
                                    string.Format("DataChangeNotification Opening database connection[{0}]", currGroup.DataBase.ToString())));
                            }

                            // Connect to DB and check for Stored Proc.
                            sql.ConnOpen(currGroup.DataBase);


                            if (SJPTraceSwitch.TraceVerbose)
                            {
                                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                    string.Format("DataChangeNotification Checking for stored procedure[{0}] in database[{1}]", SP_GetChangeTable, currGroup.DataBase.ToString())));
                            }

                            bool storedProcedureExists = (int)sql.GetScalar(
                                string.Format("SELECT Count(1) FROM sysobjects WHERE xtype = 'P' AND NAME = '{0}'", SP_GetChangeTable)) == 1;

                            if (storedProcedureExists)
                            {
                                changeNotificationTables.Add(currDatabase, ChangeNotificationData(sql, currDatabase));
                            }
                            else
                            {
                                // Log exception
                                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                                    string.Format("DataChangeNotification StoredProcedure [{0}] not found in database[{1}]", SP_GetChangeTable, s)));

                                // throw SJPException error
                                throw new SJPException(
                                    string.Format("DataChangeNotification StoredProcedure [{0}] not found in database[{1}]", SP_GetChangeTable, s),
                                    true, SJPExceptionIdentifier.DSStoredProcedureNotPresent);
                            }
                        }
                        catch (SJPException e)
                        {
                            // Log exception if it hasn't already been done
                            if (!e.Logged)
                                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, e.Message));

                            // Rethrow exception
                            throw;
                        }
                        catch (Exception e)
                        {
                            // Log exception
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                                string.Format("DataChangeNotification Unexpected error encountered: {0} ", e.Message)));

                            // throw SJPException error
                            throw new SJPException("DataChangeNotification Error initialising service, see inner exception", e,
                                true, SJPExceptionIdentifier.DSUnknownType);
                        }
                        finally
                        {
                            // Close DB connection
                            sql.ConnClose();
                        }
                    }
                }
            }

            #endregion
        }
        
        /// <summary>
        /// This method first checks whether, for a given group, the ChangeNotification
        /// Table in the group's database has changed. It does this by getting a copy of
        /// the latest ChangeNotification table and comparing it against the original, as
        /// at the initialisation (or last time the Poll method executed).
        /// In the event of a change, the method will then determine whether this group
        /// is affected by the change, and sets the group's RaiseEvent flag. It then
        /// iterates through the groups, raising events for those affected, and restarts the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Poll(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (SJPTraceSwitch.TraceVerbose)
                {
                    Logger.Write( new OperationalEvent( SJPEventCategory.Database, SJPTraceLevel.Info,
                        "DataChangeNotification Polling for data updates to ChangeNotification tables"));
                }

                Hashtable updatedChangeNotificationTables = new Hashtable();

                foreach (object database in changeNotificationTables.Keys)
                {
                    SqlHelperDatabase s;
                    try
                    {
                        s = (SqlHelperDatabase)database;
                    }
                    catch (InvalidCastException badCast)
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Warning, "An unexpected value was found in the Keys collection of the changeNotificationTables hashtable", badCast));
                        continue;
                    }
                    
                    // Check DB for changes to ChangeNotification table
                    using (SqlHelper sql = new SqlHelper())
                    {
                        SqlDataReader dReader = null;

                        try
                        {
                            sql.ConnOpen(s);
                            
                            // Create a new hashtable that references the values in memory
                            Hashtable existingChangeNotificationTables = (Hashtable)changeNotificationTables[s];
                                                        
                            // Store in temporary hashtable				
                            Hashtable currentChangeNotificationTables = ChangeNotificationData(sql, s);
                            
                            // Access data from changeNotificationTables for this DB
                            // Iterate through hashtable
                            foreach (string strTable in currentChangeNotificationTables.Keys)
                            {
                                // Compare Version against in-memory changeNotificationTables Version
                                if (existingChangeNotificationTables[strTable] == null ||
                                    !currentChangeNotificationTables[strTable].Equals(existingChangeNotificationTables[strTable]))
                                {
                                    if (SJPTraceSwitch.TraceInfo)
                                    {
                                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, 
                                            string.Format("DataChangeNotification Table[{0}] differs from last poll, OldVersion[{1}] NewVersion[{2}]", 
                                                strTable,
                                                existingChangeNotificationTables[strTable],
                                                currentChangeNotificationTables[strTable])));
                                    }

                                    // Version number of this table has changed
                                    // Determine which Group(s) contain these tables
                                    foreach (DataChangeNotificationGroup dcng in changeGroups)
                                    {
                                        dcng.RaiseEvent = dcng.RaiseEvent || dcng.IsAffected(s, strTable);
                                    }
                                }
                            }

                            //Completed comparision between old and new change data, setting
                            //old data to current
                            updatedChangeNotificationTables[s] = currentChangeNotificationTables;
                        }
                        catch (Exception unexpected)
                        {
                            // An error has occurred. Don't raise it further up, but log it as an error
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, 
                                "DataChangeNotification Error occurred whilst polling for data changes", unexpected));
                        }
                        finally
                        {
                            // Close database reader
                            if (dReader != null && !dReader.IsClosed)
                            {
                                dReader.Close();
                            }

                            // Close database connection
                            if (sql != null && sql.ConnIsOpen)
                            {
                                sql.ConnClose();
                            }
                        }
                    }
                }

                changeNotificationTables = updatedChangeNotificationTables;


                //Loop through groups and raise events where RaiseEvent = true
                foreach (DataChangeNotificationGroup dcng in changeGroups)
                {
                    if (dcng.RaiseEvent)
                    {
                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, 
                                string.Format("DataChangeNotification Raising event for Group[{0}]", dcng.GroupId)));
                        }

                        //Raise Event for this group
                        OnChanged(new ChangedEventArgs(dcng.GroupId));

                        dcng.RaiseEvent = false;
                    }
                }
            }
            catch (Exception unexpected)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, 
                    "DataChangeNotification Error occurred whilst polling the databases for change notifications", unexpected));
            }
            finally
            {
                if (!pollingTimer.AutoReset)
                    pollingTimer.Start();
            }

        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Parse the database name and return its associated SqlHelperDatabase enum type
        /// </summary>
        /// <param name="databaseName">The name of the SqlHelperDatabase to return</param>
        /// <returns>The SqlHelperDatabase represented by databaseName</returns>
        private SqlHelperDatabase ParseDatabase(string databaseName)
        {
            SqlHelperDatabase db;

            // Convert database name to enumerated value
            try
            {
                db = (SqlHelperDatabase)Enum.Parse(typeof(SqlHelperDatabase), databaseName);
            }
            catch (ArgumentNullException e)
            {
                // This exception will be thrown if currDatabaseName is null i.e. if properties haven't been set up!
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                    "Parameter currDatabaseName is null. The Database name in the Properties table was null or not found.", e));

                //Throw SJPException error
                throw new SJPException("Parameter currDatabaseName is null. The Database name in the Properties table was null or not found.",
                    e, true, SJPExceptionIdentifier.DSDatabaseNamePropertyNotPresent);
            }
            catch (ArgumentException e)
            {
                // currDatabaseName is not null, but is not a vaild value.
                // Exception thrown if currDatabaseName is not null, but an unexpected value
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error,
                    "Parameter currDatabaseName is an unexpected value. The Database name in the Properties table does not match one of the specified enums.", e));

                //Throw SJPException error
                throw new SJPException("Parameter currDatabaseName is an unexpected value. The Database name in the Properties table does not match one of the specified enums.",
                    e, true, SJPExceptionIdentifier.DSDatabaseNamePropertyNotValid);
            }

            return db;
        }

        /// <summary>
        /// Returns a hashtable of the ChangeNotificationTable data for the supplied db
        /// </summary>
        /// <param name="sql">SqlHelper connection</param>
        /// <param name="db">The db to get notification data for</param>
        private Hashtable ChangeNotificationData(SqlHelper sql, SqlHelperDatabase db)
        {
            SqlDataReader dReader = null;
            Hashtable tempHash = null;

            try
            {
                // Execute Stored Procedure to get data from ChangeNotification table
                tempHash = new Hashtable();
                dReader = sql.GetReader(SP_GetChangeTable, new List<SqlParameter>());

                // Store data from DataReader in a hashtable (table, version)
                while (dReader.Read())
                {
                    tempHash.Add(dReader.GetString(0), dReader.GetInt32(1));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                // Close the DataReader
                if ((dReader != null) && (!dReader.IsClosed))
                    dReader.Close();
            }
            return tempHash;
        }
        
        #endregion

        #region IDisposable methods

        ~DataChangeNotification()
        {
            //calls a protected method 
            //the false tells this method
            //not to bother with managed
            //resources
            this.Dispose(false);
        }

        public void Dispose()
        {
            //calls the same method
            //passed true to tell it to
            //clean up managed and unmanaged 
            this.Dispose(true);

            //as dispose has been correctly
            //called we don't need the 
            //'backup' finaliser
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //check this hasn't been called already
            //remember that Dispose can be called again
            if (!disposed)
            {
                //this is passed true in the regular Dispose
                if (disposing)
                {
                    // Dispose managed resources here.

                    #region Dispose managed resources

                    if (pollingTimer != null)
                    {
                        pollingTimer.Dispose();
                    }

                    #endregion
                }

                //both regular Dispose and the finaliser
                //will hit this code
                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        #endregion
    }
}
