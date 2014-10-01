// *********************************************** 
// NAME             : ChecksumMonitoringItem.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Implementation of ChecksumMonitoringItem class
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.ChecksumUtils;
using System.Data.SqlClient;
using Logger = System.Diagnostics.Trace;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

namespace SJP.UserPortal.CCAgent
{
    public class ChecksumMonitoringItem : MonitoringItem
    {
        private static string servicePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private string checksumRootPath;
        private string extensionsToIgnore;

        /// <summary>
        /// Read - Only. Full path of root folder to run a checksum against.
        /// </summary>
        /// <remarks></remarks>
        public string ChecksumRootPath
        {
            get
            {
                return checksumRootPath;
            }

        }
        public string ExtensionsToIgnore
        {
            get
            {
                return extensionsToIgnore;
            }

        }

        public ChecksumMonitoringItem(int MonitoringItemID, int CheckInterval, string ChecksumRootPath, string Description, string ExtensionsToIgnore, string RedCondition)
        {
            monitoringItemID = MonitoringItemID;
            checkInterval = CheckInterval;
            checksumRootPath = ChecksumRootPath;
            description = Description;
            redCondition = RedCondition;
            extensionsToIgnore = ExtensionsToIgnore;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlThread)]
        public override void ReCheck(SqlHelper helper)
        {
            try
            {

                //'Create a new Process for the checksum app
                using (Process checksumProcess = new Process())
                {

                    //Call the checksum validator on the path
                    ProcessStartInfo checksumProcessInfo = new ProcessStartInfo("HashValidator.exe");
                    checksumProcessInfo.UseShellExecute = false;
                    checksumProcessInfo.CreateNoWindow = true;
                    checksumProcessInfo.Arguments = checksumRootPath.ToString() + " " + extensionsToIgnore.ToString();
                    checksumProcessInfo.WorkingDirectory = servicePath;
                    checksumProcess.StartInfo = checksumProcessInfo;
                    checksumProcess.Start();
                    checksumProcess.WaitForExit(180000);

                    if (checksumProcess.ExitCode == 0)
                    {
                        valueAtLastCheck = "Matched";
                    }
                    else
                    {
                        valueAtLastCheck = "Not Matched";
                    }

                    //'Check if the process has completed - if not kill the process
                    if (!checksumProcess.HasExited)
                    {
                        checksumProcess.Kill();
                    }
                }

                //Capture the datetime for this check
                lastCheckTime = DateTime.Now;

                //Write it to the DB
                #region AddChecksumResult
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

                if (helper.Execute("InsertChecksumMonitoringResult", paramList) != 1)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                        "CCAgent - Could not add Checksum monitoring item result with description " + this.Description));
                }
                #endregion
            }
            catch (Exception Ex)
            {
                string message = string.Format("Exception during Checksum recheck on " + checksumRootPath, Ex.Message);
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, Ex));

            }
            finally
            {
            }
        }

    }
}
