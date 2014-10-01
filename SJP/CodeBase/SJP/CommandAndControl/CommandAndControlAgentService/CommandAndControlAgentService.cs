// *********************************************** 
// NAME             : CommandAndControlAgentInitialisation.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Command&Control Agent service main class
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using System.Data.SqlClient;
using SJP.Common.ServiceDiscovery;
using SJP.Common.ServiceDiscovery.Initialisation;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.CCAgent
{
    public partial class CommandAndControlAgentService : ServiceBase
    {

        //Used for data change notification
        private const string DataChangeNotificationGroup = "CommandControl";
        private bool receivingChangeNotifications;
        private bool changeNotificationPending = false;

        /// <summary>
        /// Thread to poll & call the "RecheckDueItems" method at regular intervals defined in CCAgent.OverallPollIntervalSeconds property.
        /// </summary>
        private Thread pollingThread = null;
        /// <summary>
        /// Time service will sleep between polls as defined in the CCAgent.OverallPollIntervalSeconds property
        /// </summary>
        private int ServicePollingInterval;

        /// <summary>
        /// Array to contain all the enabled checksum items to be monitored
        /// </summary>
        private List<ChecksumMonitoringItem> ChecksumMonitoringItems;
        /// <summary>
        /// Array to contain all the enabled database items to be monitored
        /// </summary>
        private List<DatabaseMonitoringItem> DatabaseMonitoringItems;
        /// <summary>
        /// Array to contain all the enabled checksum items to be monitored
        /// </summary>
        private List<WMIMonitoringItem> WMIMonitoringItems;
        /// <summary>
        /// Array to contain all the enabled checksum items to be monitored
        /// </summary>
        private List<FileMonitoringItem> FileMonitoringItems;


        /// <summary>
        /// Constructor
        /// </summary>
        public CommandAndControlAgentService()
        {
            InitializeComponent();

        }


        /// <summary>
        /// Registers an event handler with the data change notification service
        /// </summary>
        private bool RegisterForChangeNotification()
        {
            IDataChangeNotification notificationService;
            try
            {
                notificationService = SJPServiceDiscovery.Current.Get<DataChangeNotification>(ServiceDiscoveryKey.DataChangeNotification);
            }
            catch (SJPException e)
            {
                // If the SDInvalidKey exception is thrown, return false as the notification service
                // hasn't been initialised.
                // Otherwise, rethrow the exception that was received.
                if (e.Identifier == SJPExceptionIdentifier.SDInvalidKey)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Warning,
                        "CCAgent - DataChangeNotification service was not present when initialising"));
                    return false;
                }
                else
                    throw;
            }
            catch
            {
                throw;
            }

            notificationService.Changed += new ChangedEventHandler(this.DataChangedNotificationReceived);
            return true;
        }

        /// <summary>
        /// Used by the Data Change Notification service to clear & reload the data if it is changed in the DB
        /// </summary>
        private void DataChangedNotificationReceived(object sender, ChangedEventArgs e)
        {
            if (e.GroupId == DataChangeNotificationGroup)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info,
                        "Event raised by data change notification service - flagging monitoring items for refresh"));
                // flag the monitoring items for reload
                changeNotificationPending = true;
            }
        }

        /// <summary>
        /// Log service start, set up CommandAndControlAgentInitialisation, load monitoring items and start a thread to poll and recheck due monitoring items
        /// </summary>
        protected override void OnStart(string[] args)
        {
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info,
            "CCAgent - service starting, setting up new thread to poll and monitor..."));

            if (pollingThread == null)
            {
                pollingThread = new Thread(new ThreadStart(Poll));
            }
            pollingThread.Start();
        }

        /// <summary>
        /// Log service stop, stop the thread and dispose of any resources used.
        /// </summary>
        protected override void OnStop()
        {
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info,
            "CCAgent - service stopping, aborting monitoring thread"));
            pollingThread.Abort();
        }

        /// <summary>
        /// Method to poll and call the "RecheckDueItems" method at regular intervals defined in CCAgent.OverallPollIntervalSeconds property.
        /// </summary>
        private void Poll()
        {
            #if Debug
                Thread.Sleep(10000); //DEBUG - sleep for 10 secs so i can attach!
            #endif
            // Initialise the service discovery
            SJPServiceDiscovery.Init(new CommandAndControlAgentInitialisation());

            // Setup data change notification
            receivingChangeNotifications = RegisterForChangeNotification();

            //Log thread starting
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
            "CCAgent - Beginning poll for rechecks..."));

            //Initial load of monitoring items 
            LoadMonitoringItems();

            IPropertyProvider properties = Properties.Current;

            do
            // Loops until killed by OnStop.
            {
                if (changeNotificationPending)
                {
                    //reload monitoring items if needed
                    LoadMonitoringItems();
                }

                //Get poll interval again in case it changed (default to 30 secs)
                ServicePollingInterval = Properties.Current["CCAgent.OverallPollIntervalSeconds"].Parse<int>(30);

                //Check those monitoring items which are due
                RecheckDueItems();
                // Sleep for configured interval
                Thread.Sleep(ServicePollingInterval*1000);
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info,
                "CCAgent - poll interval reached after " + ServicePollingInterval.ToString() + " - rechecking any due items"));
            }
            while (1 == 1);
        }

        /// <summary>
        /// Method which goes through all monitoring items in the loaded arrays, updating values for 
        /// any items due for a re-check according to their check interval property.
        /// </summary>
        private void RecheckDueItems()
        {
            try
            {
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    // Create a connection to the database to use for rechecking all the monitoring measures
                    IPropertyProvider currProps = Properties.Current;
                    string connectionString = currProps[SqlHelperDatabase.CommandControlDB.ToString()];
                    sqlHelper.ConnOpen(SqlHelperDatabase.CommandControlDB);

                    //Recheck all items required


                    //DB
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    "CCAgent - Rechecking database monitoring items due for update..."));
                    foreach (DatabaseMonitoringItem dbItem in DatabaseMonitoringItems)
                    {
                        if (dbItem.CheckRequired) { dbItem.ReCheck(sqlHelper); }
                    }

                    //WMI
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    "CCAgent - Rechecking WMI monitoring items due for update..."));
                    foreach (WMIMonitoringItem WMIItem in WMIMonitoringItems)
                    {
                        if (WMIItem.CheckRequired) { WMIItem.ReCheck(sqlHelper); }
                    }

                    //File
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    "CCAgent - Rechecking file monitoring items due for update..."));
                    foreach (FileMonitoringItem FileItem in FileMonitoringItems)
                    {
                        if (FileItem.CheckRequired) { FileItem.ReCheck(sqlHelper); }
                    }

                    //Checksum - let's check these last as they take longest!
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                        "CCAgent - Rechecking checksum monitoring items due for update..."));
                    foreach (ChecksumMonitoringItem chkItem in ChecksumMonitoringItems)
                    {
                        if (chkItem.CheckRequired) { chkItem.ReCheck(sqlHelper); }
                    }

                }
            }
            catch (SqlException sqlEx)
            {
                string message = string.Format("SqlException during Recheck of monitoring items - Error: {0}",sqlEx.Message);
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));
            }
            finally
            {
            }
        }

        /// <summary>
        /// Method which loads all required monitoring items into the arrays from the database.
        /// </summary>
        public void LoadMonitoringItems()
        {
            try 
            {

                //Clear the lists first
                WMIMonitoringItems = new List<WMIMonitoringItem>();
                ChecksumMonitoringItems = new List<ChecksumMonitoringItem>();
                FileMonitoringItems = new List<FileMonitoringItem>();
                DatabaseMonitoringItems = new List<DatabaseMonitoringItem>();

                using (SqlHelper helper = new SqlHelper())
                {
                    // Create a connection to the database
                    IPropertyProvider currProps = Properties.Current;
                    string connectionString = currProps[SqlHelperDatabase.CommandControlDB.ToString()];
                    helper.ConnOpen(SqlHelperDatabase.CommandControlDB);

                    #region getWMIItems
                    try
                    {
                        List<SqlParameter> paramList = new List<SqlParameter>();

                        SqlDataReader dr = helper.GetReader("GetWMIMonitoringItems", paramList);
                        //clear it down first
                        WMIMonitoringItems.Clear();

                        //now get the new data in
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                if ((bool)dr["Enabled"])
                                {
                                    //if enabled add it to the array - ignore any not marked enabled
                                    WMIMonitoringItem currItem = new WMIMonitoringItem((int)dr["ItemID"], (int)dr["CheckInterval"],
                                    dr["WQLQuery"].ToString(), dr["Description"].ToString(), dr["RedCondition"].ToString());

                                    WMIMonitoringItems.Add(currItem);
                                }
                            }
                        }
                        dr.Dispose();
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = string.Format("SqlException during load of WMI monitoring items - Error: {0}", sqlEx.Message);
                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));
                    }
                    #endregion

                    #region getChecksumItems
                    try
                    {
                        List<SqlParameter> paramList = new List<SqlParameter>();

                        SqlDataReader dr = helper.GetReader("GetChecksumMonitoringItems", paramList);
                        //clear it down first
                        ChecksumMonitoringItems.Clear();

                        //now get the new data in
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                if ((bool)dr["Enabled"])
                                {
                                    //if enabled add it to the array - ignore any not marked enabled
                                    ChecksumMonitoringItem currItem = new ChecksumMonitoringItem((int)dr["ItemID"], 
                                    (int)dr["CheckInterval"], dr["ChecksumRootPath"].ToString(), dr["Description"].ToString(), 
                                    dr["ExtensionsToIgnore"].ToString(), dr["RedCondition"].ToString());
                                    ChecksumMonitoringItems.Add(currItem);
                                }
                            }
                        }
                        dr.Dispose();
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = string.Format("SqlException during load of checksum monitoring items - Error: {0}", sqlEx.Message);
                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));
                    }
                    #endregion

                    #region getDatabaseItems
                    try
                    {
                        List<SqlParameter> paramList = new List<SqlParameter>();

                        SqlDataReader dr = helper.GetReader("GetDatabaseMonitoringItems", paramList);
                        //clear it down first
                        DatabaseMonitoringItems.Clear();

                        //now get the new data in
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                if ((bool)dr["Enabled"])
                                {
                                    //if enabled add it to the array - ignore any not marked enabled
                                    DatabaseMonitoringItem currItem = new DatabaseMonitoringItem((int)dr["ItemID"], (int)dr["CheckInterval"],
                                    dr["SQLHelperDatabaseTarget"].ToString(), dr["SQLQuery"].ToString(), dr["Description"].ToString(), dr["RedCondition"].ToString());

                                    DatabaseMonitoringItems.Add(currItem);
                                }
                            }
                        }
                        dr.Dispose();
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = string.Format("SqlException during load of database monitoring items - Error: {0}", sqlEx.Message);
                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));
                    }
                    #endregion

                    #region getFileItems
                    try
                    {
                        List<SqlParameter> paramList = new List<SqlParameter>();

                        SqlDataReader dr = helper.GetReader("GetFileMonitoringItems", paramList);
                        //clear it down first
                        FileMonitoringItems.Clear();

                        //now get the new data in
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                if ((bool)dr["Enabled"])
                                {
                                    //if enabled add it to the array - ignore any not marked enabled
                                    FileMonitoringItem currItem = new FileMonitoringItem((int)dr["ItemID"], (int)dr["CheckInterval"],
                                    dr["FullFilePath"].ToString(), dr["Description"].ToString(), dr["RedCondition"].ToString());

                                    FileMonitoringItems.Add(currItem);
                                }
                            }
                        }
                        dr.Dispose();
                    }
                    catch (SqlException sqlEx)
                    {
                        string message = string.Format("SqlException during load of file monitoring items - Error: {0}", sqlEx.Message);
                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));
                    }
                    #endregion
                }

            }
            catch (Exception unexpected)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                    "Unexpected exception occurred during load of file monitoring items", unexpected));
            }

            finally
            {

            }
        }
    }
}
