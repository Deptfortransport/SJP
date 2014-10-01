// *********************************************** 
// NAME             : FileMonitoringItem.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Implementation of FileMonitoringItem class
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
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
    public class FileMonitoringItem : MonitoringItem
    {

        private string fullFilePath;
        private DateTime fileModifiedDateTime;
        private DateTime fileCreatedDateTime;
        private string fileSize;
        private string fileProductVersion;

        /// <summary>
        /// Read Only. Full path of the file to monitor.
        /// </summary>
        /// <remarks></remarks>
        public string FullFilePath
        {
            get
            {
                return fullFilePath;
            }
        }

        /// <summary>
        /// Read Only. File last modified date/time as at last check.
        /// </summary>
        /// <remarks></remarks>
        public DateTime FileModifiedDateTime
        {
            get
            {
                return fileModifiedDateTime;
            }
            set
            {
                fileModifiedDateTime = value;
            }
        }

        /// <summary>
        /// Read Only. File product version as at last check.
        /// </summary>
        /// <remarks></remarks>
        public string FileProductVersion
        {
            get
            {
                return fileProductVersion;
            }
            set
            {
                fileProductVersion = value;
            }
        }

        /// <summary>
        /// Read Only. File created date/time as at last check.
        /// </summary>
        /// <remarks></remarks>
        public DateTime FileCreatedDateTime
        {
            get
            {
                return fileCreatedDateTime;
            }
            set
            {
                fileCreatedDateTime = value;
            }
        }

        public string FileSize
        {
            get
            {
                return fileSize;
            }
            set
            {
                fileSize = value;
            }
        }

        public FileMonitoringItem(int MonitoringItemID, int CheckInterval, string FullFilePath, string Description, string RedCondition)
        {
            monitoringItemID = MonitoringItemID;
            checkInterval = CheckInterval;
            fullFilePath = FullFilePath;
            description = Description;
            redCondition = RedCondition;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlThread)]
        public override void ReCheck(SqlHelper helper)
        {
            //Initialise local variables to default values
            fileModifiedDateTime = new DateTime();
            fileModifiedDateTime = DateTime.Now;
            fileCreatedDateTime = new DateTime();
            fileCreatedDateTime = DateTime.Now;
            fileSize = "";
            fileProductVersion = "";

            //Capture the datetime for this check
            this.LastCheckTime = DateTime.Now;

            try
            {
                //Get the File measure
                FileInfo targetFile = new FileInfo(fullFilePath);
                System.Diagnostics.FileVersionInfo finfo = FileVersionInfo.GetVersionInfo(fullFilePath);
                StringBuilder tempValue = new StringBuilder();
                fileModifiedDateTime = targetFile.LastWriteTime;
                fileCreatedDateTime = targetFile.CreationTime;
                fileSize = (targetFile.Length / 1024).ToString() + " Kb";
                fileProductVersion = finfo.ProductVersion;
                tempValue.Append("Version: ");
                tempValue.Append(fileProductVersion);
                tempValue.Append(" Last Modified: ");
                tempValue.Append(fileModifiedDateTime);
                tempValue.Append(" Size(Kb): ");
                tempValue.Append(fileSize);
                valueAtLastCheck =  tempValue.ToString();
            }
            catch (Exception e)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                "An error occurred while querying for File measure: " + this.Description + " - " + e.Message));
                valueAtLastCheck = "Unable to check - file may not be present";
            }
            //Write it to the DB
            #region AddFileResult
            List<SqlParameter> paramList = new List<SqlParameter>();
            SqlParameter MonitoringItemID = new SqlParameter("MonitoringItemID", (System.Data.SqlTypes.SqlInt32)monitoringItemID);
            SqlParameter SJP_Server = new SqlParameter("SJP_Server", (System.Data.SqlTypes.SqlString)System.Environment.MachineName);
            SqlParameter Description = new SqlParameter("Description", (System.Data.SqlTypes.SqlString)description);
            SqlParameter CheckTime = new SqlParameter("CheckTime", (System.Data.SqlTypes.SqlDateTime)lastCheckTime);
            SqlParameter ValueAtCheck = new SqlParameter("ValueAtCheck", (System.Data.SqlTypes.SqlString)valueAtLastCheck);
            SqlParameter ItemIsInRed = new SqlParameter("IsInRed", (System.Data.SqlTypes.SqlBoolean)IsInRed);
            SqlParameter FileCreatedDateTime = new SqlParameter("FileCreatedDateTime", (System.Data.SqlTypes.SqlDateTime)fileCreatedDateTime);
            SqlParameter FileModifiedDateTime = new SqlParameter("FileModifiedDateTime", (System.Data.SqlTypes.SqlDateTime)fileModifiedDateTime);
            SqlParameter FileSize = new SqlParameter("FileSize", (System.Data.SqlTypes.SqlString)fileSize);
            SqlParameter FileProductVersion = new SqlParameter("FileProductVersion", (System.Data.SqlTypes.SqlString)fileProductVersion);
            paramList.Add(MonitoringItemID);
            paramList.Add(SJP_Server);
            paramList.Add(Description);
            paramList.Add(CheckTime);
            paramList.Add(ValueAtCheck);
            paramList.Add(ItemIsInRed);
            paramList.Add(FileCreatedDateTime);
            paramList.Add(FileModifiedDateTime);
            paramList.Add(FileSize);
            paramList.Add(FileProductVersion);

            if (helper.Execute("InsertFileMonitoringResult", paramList) != 1)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                    "CCAgent - Could not add File monitoring item result with description " + this.Description));
            }
            #endregion
        }
    }
}
