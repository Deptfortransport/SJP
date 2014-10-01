// *********************************************** 
// NAME             : WebLogReaderController.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Controller Class to process web logs
// ************************************************
// 

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using System.Collections.Generic;
using SJP.Common.Extenders;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Controller Class to process web logs
    /// </summary>
    public class WebLogReaderController
    {
        #region Private members

        private IPropertyProvider properties;
        private IWebLogReader reader;

        private readonly string logFileExtension = "log";

        /// <summary>
        /// Identifier used to prevent certain TD Portal web pages from being 
        /// logged as workload events, if used as URI query string.
        /// </summary>
        private readonly string ignoreMarker = "undvik=1";

        /// <summary>
        /// Range of HTTP status codes for which workload events should be logged.
        /// </summary>
        private readonly int minValidStatusCode = 100;
        private readonly int maxValidStatusCode = 599;

        /// <summary>
        /// Identifier used to specify that web log entries without an extension
        /// should be considered as a web page.
        /// </summary>
        private readonly string noFileExtensionMarker = "[none]";

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="properties">
        /// Property provider that supplies properties to use by the controller.
        /// </param>
        public WebLogReaderController(IPropertyProvider properties)
        {
            this.properties = properties;

            // Create a W3C web log reader to process web logs.
            reader = new W3CWebLogReader();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method to process Web Log Files.
        /// </summary>
        /// <returns>
        /// Success code:
        /// Zero if processing was successful.
        /// Greater than zero if unsuccessful.
        /// </returns>
        public int Run()
        {
            int returnCode = 0;

            // Determine configurable properties.
            string[] validFileExtensions = properties[Keys.WebLogReaderWebPageExtensions].Split(' ');
            int minNonPageSize = int.Parse(properties[Keys.WebLogReaderNonPageMinimumBytes]);
            string[] clientIPExcludes = null;
            if (properties[Keys.WebLogReaderClientIPExcludes].Length != 0)
                clientIPExcludes = properties[Keys.WebLogReaderClientIPExcludes].Split(' ');
            else
                clientIPExcludes = new string[0];

            List<StatusCode> statusCodes = GetStatusCodes();

            // User agent string for UserExperience visitor
            string userAgentUEV = properties[Keys.WebLogReaderUserExperienceVisitorUserAgent];
            string sessionIdStartMarker = properties[Keys.WebLogReaderCookieSessionIdStartMarker];
            string sessionIdEndMarker = properties[Keys.WebLogReaderCookieSessionIdEndMarker];

            // Create a web log entry spec based on the properties.
            WebLogDataSpecification spec =
                new WebLogDataSpecification(validFileExtensions,
                                            statusCodes,
                                            minNonPageSize,
                                            clientIPExcludes,
                                            this.ignoreMarker,
                                            this.noFileExtensionMarker,
                                            userAgentUEV,
                                            sessionIdStartMarker,
                                            sessionIdEndMarker);

            // Get the property to get All folders to read from
            string[] webLogsFolders = properties[Keys.WebLogReaderWebLogFolders].Split(' ');

            // For each folder Process as with single folder
            foreach (string folder in webLogsFolders)
            {

                // the current key is formed from the template and includes the name of the current folder.
                string logDirectoryKey = string.Format(Keys.WebLogReaderLogDirectory, folder);
                string archiveDirectoryKey = string.Format(Keys.WebLogReaderArchiveDirectory, folder);

                // Determine list of files to process.
                ArrayList logFileNames = GetLogFileNames(properties[logDirectoryKey]);
                if (logFileNames.Count == 0)
                {
                    if (SJPTraceSwitch.TraceWarning)
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, Messages.Controller_NoWebLogs));
                }

                if (SJPTraceSwitch.TraceInfo)
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, String.Format(Messages.Controller_NumLogs, logFileNames.Count)));

                int currentFileIndex = 0;
                int filesProcessed = 0;

                try
                {
                    int workloadEventsLogged = 0;

                    for (int i = 0; i < logFileNames.Count; i++)
                    {
                        // Process a web log file.
                        currentFileIndex = i;
                        workloadEventsLogged = reader.ProcessWorkload((string)logFileNames[i], spec);

                        // Move processed web log to archive directory.
                        ArchiveWebLog((string)logFileNames[i],
                            properties[logDirectoryKey],
                            properties[archiveDirectoryKey]);


                        filesProcessed++;

                        if (SJPTraceSwitch.TraceWarning)
                        {
                            if (workloadEventsLogged == 0)
                                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, String.Format(Messages.Controller_ZeroWorkloadEvents, (string)logFileNames[i])));
                            else
                                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, String.Format(Messages.Controller_NumWorkloadEvents, workloadEventsLogged, (string)logFileNames[i])));
                        }

                    }


                }
                catch (SJPException sjpEx)
                {

                    returnCode = (int)sjpEx.Identifier;

                    if (!sjpEx.Logged)
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure,
                            SJPTraceLevel.Error,
                            String.Format(Messages.Controller_FailedProcessingWebLog, logFileNames[currentFileIndex], sjpEx.Message, filesProcessed)));
                }

                if (SJPTraceSwitch.TraceInfo)
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, String.Format(Messages.Controller_NumLogsProcessed, filesProcessed)));

            }
            return returnCode;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Determines list of web log files to process. Excludes active files.
        /// </summary>
        /// <param name="logDir">Directory containing log files.</param>
        /// <returns>List of all file names.</returns>
        private ArrayList GetLogFileNames(string logDir)
        {
            // Determine list of all the web logs which have not been processed.
            string[] allFilepaths = Directory.GetFiles(logDir);
            ArrayList allLogFilepaths = new ArrayList();
            foreach (string fileName in allFilepaths)
            {
                // Only consider files with correct extension.
                string[] fileNameSplit = fileName.Split('.');

                if (fileNameSplit[1] == this.logFileExtension)
                    allLogFilepaths.Add(fileName);
            }

            // this code isn't working so just remove the latest log file from the list
            /**
            // Determine names of active web logs which should not be processed.
            string[] activeLogFilenames = reader.GetActiveWebLogFileNames();
            ArrayList activeLogFilepaths = new ArrayList();
            foreach (string fileName in activeLogFilenames)
            {
                // Add full paths to active file names.
                string activeLogFilepath = logDir + "\\" + fileName;
                activeLogFilepaths.Add(activeLogFilepath);
            }*/


            // Determine list of inactive logs.
            ArrayList inactiveLogs = new ArrayList();
            string lastFilename = string.Empty;
            allLogFilepaths.Sort(); 

            foreach (string fileName in allLogFilepaths)
            {
                inactiveLogs.Add(fileName);
                lastFilename = fileName;
            }

            // remove the current log file as it will be opened by IIS
            if (lastFilename != string.Empty)
            {
                inactiveLogs.Remove(lastFilename);
            }

            return inactiveLogs;
        }

        /// <summary>
        /// Reads the status code range values from the properties returning a list of status codes ranges
        /// </summary>
        /// <returns></returns>
        private List<StatusCode> GetStatusCodes()
        {
            List<StatusCode> statusCodes = new List<StatusCode>();

            string[] statusCodeRanges = properties[Keys.WebLogReaderValidStatusCodeRanges].Split(' ');

            int statusCodeMin = minValidStatusCode;
            int statusCodeMax = maxValidStatusCode;

            if (statusCodeRanges.Length > 0)
            {
                foreach (string range in statusCodeRanges)
                {
                    // the current key is formed from the template and includes the name of the current folder.
                    string strStatusCodeMin = properties[string.Format(Keys.WebLogReaderValidStatusCodeRangeMin, range)];
                    string strStatusCodeMax = properties[string.Format(Keys.WebLogReaderValidStatusCodeRangeMax, range)];

                    if (!string.IsNullOrEmpty(strStatusCodeMin))
                        statusCodeMin = strStatusCodeMin.Parse(minValidStatusCode);
                    else
                        statusCodeMin = minValidStatusCode;

                    if (!string.IsNullOrEmpty(strStatusCodeMax))
                        statusCodeMax = strStatusCodeMax.Parse(maxValidStatusCode);
                    else
                        statusCodeMax = maxValidStatusCode;

                    StatusCode statusCode = new StatusCode(statusCodeMin, statusCodeMax);

                    statusCodes.Add(statusCode);
                }
            }
            else
            {
                StatusCode statusCode = new StatusCode(statusCodeMin, statusCodeMax);

                statusCodes.Add(statusCode);
            }

            return statusCodes;
        }

        /// <summary>
        /// Method to move a processed web log from the WebLog Dir to the Archive Dir.
        /// Catches any exceptions and rethrows them as a TDException 
        /// </summary>
        /// <param name="fileName">log file name to move</param>
        /// <param name="logDir">dir to move from</param>
        /// <param name="archiveDir">dir to move to</param>
        /// <returns></returns>
        private void ArchiveWebLog(string fileName, string logDir, string archiveDir)
        {
            try
            {
                string[] splitFileName = fileName.Split('\\');
                File.Move(fileName, archiveDir + "\\" + splitFileName[splitFileName.Length - 1]);
            }
            catch (Exception ex)
            {
                throw new SJPException(String.Format(Messages.Controller_FailedArchivingFile, ex.Message), false, SJPExceptionIdentifier.RDPWebLogReaderArchiveFailed);
            }
        }

        #endregion
    }
}
