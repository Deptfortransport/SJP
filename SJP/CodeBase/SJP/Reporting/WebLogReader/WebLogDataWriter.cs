// *********************************************** 
// NAME             : WebLogDataWriter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 July 2011
// DESCRIPTION  	: WebLogDataWriter class to log data directly to the Database. This is specifically used to log 
// data associated with the UserExperienceMonitoring visitor web log data
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.DatabaseInfrastructure;
using System.Data.SqlClient;
using SJP.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using SJP.Common.EventLogging;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// WebLogDataWriter class to log data directly to the Database. This is specifically used to log 
    /// data associated with the UserExperienceMonitoring visitor web log data
    /// </summary>
    public static class WebLogDataWriter
    {
        #region Private members

        private static SqlHelperDatabase targetDatabase;

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor
        /// </summary>
        static WebLogDataWriter()
        {
            targetDatabase = SqlHelperDatabase.ReportStagingDB;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Writes a WebLogReader data or a UserExperience visitor to the ReportStaging database
        /// </summary>
        public static void WriteUserExperienceVisitorData(
            string repeatVisitorType, 
            string sessionIdOld,
            string sessionIdNew,
            string domain,
            string userAgent,
            int themeId,
            DateTime datetime,
            DateTime lastVisitedDateTime  )

        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@RepeatVistorType", repeatVisitorType));
            parameters.Add(new SqlParameter("@SessionIdOld", sessionIdOld));
            parameters.Add(new SqlParameter("@SessionIdNew", sessionIdNew));
            parameters.Add(new SqlParameter("@DomainName", domain));
            parameters.Add(new SqlParameter("@UserAgent", userAgent));
            parameters.Add(new SqlParameter("@ThemeId", themeId));
            parameters.Add(new SqlParameter("@TimeLogged", datetime));
            parameters.Add(new SqlParameter("@LastVisited", lastVisitedDateTime));

            try
            {
                WriteToDB("AddRepeatVisitorEventWebLogReader", parameters, sessionIdNew);
            }
            catch (SJPException sjpEx)
            {
                throw new SJPException(String.Format("Error occurred attempting to write WebLogReader UserExperience visitor data to the database, exception: {0}", sjpEx.Message),
                    sjpEx, false, SJPExceptionIdentifier.RDPFailedPublishingRepeatVisitorEvent);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Executes the requested Stored Procedure with the parameters supplied.
        /// </summary>
        /// <param name="storedProcName">string containing the Stored Procedure Name</param>
        /// <param name="parameters">Hashtable containg the Stored Proc parameters</param>
        /// <exception cref="SJPException">Thrown when the stored Procedure return code is incorrect</exception>
        /// <exception cref="SJPException">Thrown when a SqlException is caught</exception>
        private static void WriteToDB(string storedProcName, List<SqlParameter> sqlParameters, string sessionId)
        {
            try
            {
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    sqlHelper.ConnOpen(targetDatabase);

                    sqlHelper.Execute(storedProcName, sqlParameters);
                }
            }
            catch (SqlException sqlEx)
            {
                // SQLHelper does not catch SqlException so catch here.
                throw new SJPException(String.Format("SQL Helper error when excuting stored procedure [{0}]. Message: [{1}]", storedProcName, sqlEx.Message), sqlEx, false,
                    SJPExceptionIdentifier.RDPSQLHelperStoredProcedureFailure);
            }
            catch (SqlTypeException ste)
            {
                throw new SJPException(String.Format("SQL Helper Type error when excuting stored procedure [{0}]. Message: [{1}]", storedProcName, ste.Message), ste, false,
                    SJPExceptionIdentifier.RDPSQLHelperStoredProcedureFailure);
            }
        }

        #endregion
    }
}
