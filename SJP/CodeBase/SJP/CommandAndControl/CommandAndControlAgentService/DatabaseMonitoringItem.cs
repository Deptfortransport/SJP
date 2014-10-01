// *********************************************** 
// NAME             : DatabaseMonitoringItem.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Implementation of DatabaseMonitoringItem class
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using System.Data.SqlClient;
using Logger = System.Diagnostics.Trace;
using System.Security;
using System.Security.Permissions;

namespace SJP.UserPortal.CCAgent
{
    public class DatabaseMonitoringItem : MonitoringItem
    {
        private string sqlQuery;
        private string sqlHelperDatabaseTarget;

        /// <summary>
        /// Read - Only. Connection string to access database holding the required data for this monitoring item.
        /// </summary>
        /// <remarks></remarks>
        public string SQLHelperDatabaseTarget
        {
            get
            {
                return sqlHelperDatabaseTarget;
            }
        }

        /// <summary>
        /// Read - Only. SQL query to run to obtain required monitoring metric from the specified Database.
        /// </summary>
        /// <remarks></remarks>
        public string SQLQuery
        {
            get
            {
                return sqlQuery;
            }
        }

        public DatabaseMonitoringItem(int MonitoringItemID, int CheckInterval, string SQLHelperDatabaseTarget, string SQLQuery, string Description, string RedCondition)
        {
            monitoringItemID = MonitoringItemID;
            checkInterval = CheckInterval;
            sqlHelperDatabaseTarget = SQLHelperDatabaseTarget;
            sqlQuery = SQLQuery;
            description = Description;
            redCondition = RedCondition;
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlThread)]
        public override void ReCheck(SqlHelper helper)
        {
            //Need a second sqlHelper to connect to the DB we're checking
            using (SqlHelper monitorHelper = new SqlHelper())
            {
                try
                {
                    //Get the DB measure
                    SqlHelperDatabase targetDB = new SqlHelperDatabase();
                    targetDB = (SqlHelperDatabase)Enum.Parse(typeof(SqlHelperDatabase), sqlHelperDatabaseTarget, true);

                    monitorHelper.ConnOpen(targetDB);
                    StringBuilder tempValue = new StringBuilder();
                    SqlDataReader listDR = monitorHelper.GetReader(sqlQuery);
                    while (listDR.Read())
                    //Should only have one row - if more than one just return first. Return all column vals delimited by " | "
                    //though it is not expected this will be used to retrieve multiple columns...
                    {
                        for (int i = 0; i < listDR.FieldCount; i++)

                            if (tempValue.Length == 0)
                            {
                                tempValue.Append(listDR.GetString(i));
                            }
                            else
                            {
                                tempValue.Append(" | ");
                                tempValue.Append(listDR.GetString(i));
                            }
                    }
                    listDR.Dispose();
                    this.ValueAtLastCheck = tempValue.ToString();
                }
                catch (Exception Ex)
                {
                    string message = string.Format("Exception during check of DB monitoring item: " + this.Description, Ex.Message);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, Ex));
                    valueAtLastCheck = "Unable to perform DB query - relevant table/columns may not exist";
                }
                //Capture the datetime for this check
                this.LastCheckTime = DateTime.Now;
                try
                {
                    //Write it to the DB
                    #region AddDBResult
                    List<SqlParameter> paramList = new List<SqlParameter>();
                    SqlParameter MonitoringItemID = new SqlParameter("MonitoringItemID", (System.Data.SqlTypes.SqlInt32)monitoringItemID);
                    SqlParameter SJP_Server = new SqlParameter("SJP_Server", (System.Data.SqlTypes.SqlString)System.Environment.MachineName);
                    SqlParameter Description = new SqlParameter("Description", (System.Data.SqlTypes.SqlString)description);
                    SqlParameter CheckTime = new SqlParameter("CheckTime", (System.Data.SqlTypes.SqlDateTime)lastCheckTime);
                    SqlParameter ValueAtCheck = new SqlParameter("ValueAtCheck", (System.Data.SqlTypes.SqlString)valueAtLastCheck);
                    SqlParameter ItemIsInRed = new SqlParameter("IsInRed", (System.Data.SqlTypes.SqlBoolean)IsInRed);
                    paramList.Add(MonitoringItemID);
                    paramList.Add(SJP_Server);
                    paramList.Add(Description);
                    paramList.Add(CheckTime);
                    paramList.Add(ValueAtCheck);
                    paramList.Add(ItemIsInRed);

                    if (helper.Execute("InsertDatabaseMonitoringResult", paramList) != 1)
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                            "CCAgent - Could not add DB monitoring item result with description " + this.Description));
                    }
                    #endregion
                }
                catch (SqlException sqlEx)
                {
                    string message = string.Format("SqlException during save of DB monitoring item result: " + this.Description, sqlEx.Message);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));
                }
            }
        }

    }
}
