// *********************************************** 
// NAME             : W3CWebLogReader.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Reader for W3C Format Web Logs
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;
using SJP.Reporting.Events;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Reader for W3C Format Web Logs
    /// </summary>
    public class W3CWebLogReader : IWebLogReader
    {
        #region Private members

        // Define the field names to look for and the order to look for them in
        private readonly string[] mandatoryFieldNames = { "date", "time", "c-ip", "cs-uri-stem", "cs-uri-query", "sc-bytes", "sc-status", "cs-host", "cs(User-Agent)", "cs(Cookie)" };

        // Define the field number of date in the fieldNames array
        private readonly int dateFieldPosition = 0;

        // Define the field number of time in the fieldNames array
        private readonly int timeFieldPosition = 1;

        // Define the field number of c-ip in the fieldNames array
        private readonly int clientIPFieldPosition = 2;

        // Define the field number of cs-uri-stem in the fieldNames array
        private readonly int uriFieldPosition = 3;

        // Define the field number of cs-uri-query in the fieldNames array
        private readonly int uriQueryFieldPosition = 4;

        // Define the field number of sc-bytes in the fieldNames array
        private readonly int bytesFieldPosition = 5;

        // Define the field number of sc-status in the fieldNames array
        private readonly int statusFieldPosition = 6;

        // Define the field number of sc-status in the fieldNames array
        private readonly int hostFieldPosition = 7;

        // Define the field number of cs(User-Agent) in the fieldNames array
        private readonly int userAgentFieldPosition = 8;

        // Define the field number of cs(Cookie) in the fieldNames array
        private readonly int cookieFieldPosition = 9;

        // Identifier that prefixes the field names definitions in the log file
        private readonly string fieldDefinitionMarker = "#Fields: ";

        // Prefix of web log lines that do not include web log data.
        private readonly string nonDataPrefix = "#";

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor.
        /// </summary>
        public W3CWebLogReader()
            : base()
        { }

        #endregion

        #region Private methods

        /// <summary>
        /// Determines positions of web log entry fields.
        /// This method caters for situations where order of fields 
        /// written to web logs are change within the same web log.
        /// </summary>
        /// <param name="actualFields">Array of fields to get positions for.</param>
        /// <param name="expectedFields">Array of expected fields.</param>
        /// <param name="fieldPositions">Used to return field positions.</param>
        /// <exception cref="TDException">A field position could not be allocated to one or more of the actual fields passed.</exception>
        private void GetFieldPositions(string[] actualFields, string[] expectedFields, int[] fieldPositions)
        {

            // Set existing positions to an invalid position value.
            for (int a = 0; a < expectedFields.Length; a++)
                fieldPositions[a] = -1;

            // Assign a field position to each actual field, based on the expected fields.
            for (int i = 0; i < actualFields.Length; i++)
            {
                for (int j = 0; j < expectedFields.Length; j++)
                {
                    if (String.Compare(actualFields[i], expectedFields[j]) == 0)
                    {
                        fieldPositions[j] = i;
                    }
                }
            }

            // Validate that a field position was allocated to all actual fields.
            for (int k = 0; k < fieldPositions.Length; k++)
            {
                if (fieldPositions[k] == -1)
                {
                    StringBuilder fieldRequirementMessage = new StringBuilder(100);

                    for (int fieldNum = 0; fieldNum < expectedFields.Length; fieldNum++)
                        fieldRequirementMessage.Append(expectedFields[fieldNum] + ", ");

                    throw new SJPException(String.Format(Messages.W3CReader_MissingFields, fieldPositions[k], fieldRequirementMessage.ToString()), false, 
                        SJPExceptionIdentifier.RDPWebLogReaderMissingFields);
                }
            }
        }

        /// <summary>
        /// Splits a web log line into its component field names using given
        /// character to perform split.
        /// </summary>
        /// <param name="webLogLine">Web log line containing field names.</param>
        /// <param name="splitOn">Char to use to split web log line into field names.</param>
        /// <returns>Array of field names.</returns>
        private string[] GetFieldNames(string webLogLine, char splitOn)
        {
            string[] webLogLineSplit = webLogLine.Split(splitOn);

            // Take account of text at start of field definitions that should be ignored.
            string[] webLogLineFieldNames = new String[webLogLineSplit.Length - 1];

            for (int x = 1; x < webLogLineSplit.Length; x++)
                webLogLineFieldNames[x - 1] = webLogLineSplit[x];

            return webLogLineFieldNames;
        }
        
        /// <summary>
        /// Returns true if web log reader is configured for handling hourly rotation.
        /// Defaults to true if no configuration is configured or if the configuration has invalid value.
        /// </summary>
        private bool HourlyRotation
        {
            get
            {
                string rolloverPeriod = Properties.Current[Keys.WebLogReaderRolloverPeriod];

                if (rolloverPeriod != null)
                {
                    if (0 == String.Compare(LogRolloverPeriods.Daily.ToString(), rolloverPeriod))
                        return false;
                    else if (0 == String.Compare(LogRolloverPeriods.Hourly.ToString(), rolloverPeriod))
                        return true;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Returns true if web log reader is configured for using local time. This setting should
        /// correspond to that set in IIS. Log files are named using the current time; if UseLocalTime
        /// is false, the current time as GMT (also known as UTC) should be used.
        /// </summary>
        private bool UseLocalTime
        {
            get
            {
                string time = Properties.Current[Keys.WebLogReaderUseLocalTime];
                bool parsedTime;

                // If null or empty string, return false (ie use GMT)
                if (time == null || time.Length == 0)
                    return false;
                else
                {
                    // Value should be boolean
                    try
                    {
                        parsedTime = bool.Parse(time);
                    }
                    catch (FormatException e)
                    {
                        // The value didn't correspond to either true or false. Log a warning and
                        // then return false
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, String.Format(Messages.W3CReader_InvalidUseLocalTimeValue, time), e));
                        return false;
                    }
                    return parsedTime;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Processes the workload of a web log in W3C Format.
        /// Logs WorkLoadEvent events for each entry read from web log
        /// that meets the specification passed.
        /// </summary>
        /// <param name="filePath">
        /// Full filepath to web log to process.
        /// </param>
        /// <param name="dataSpecification">
        /// Specification that must be met for entry to be given a workload event.
        /// </param>
        /// <returns>
        /// Number of workload events logged for the file processed.
        /// </returns>
        /// <exception cref="TDException">
        /// Thrown if error when processing the web log.
        /// </exception>
        public int ProcessWorkload(string filePath, WebLogDataSpecification dataSpecification)
        {
            bool fieldDefinitionsFound = false;
            StreamReader fileForHeader = null;
            StreamReader file = null;
            String webLogLine = string.Empty;
            int[] fieldPositions = new int[this.mandatoryFieldNames.Length];
            System.IFormatProvider formatProvider =
                new System.Globalization.CultureInfo("en-GB", false);

            // Create a hash table to store aggregated workload event counts for each minute.
            // Initialise hash table with maximum entries/minutes possible for configured rotation period.
            int maxWorkloadEvents;
            int errorsCaughtCount = 0;
            int maxPartnerCount = 120;
            StringBuilder errorString = new StringBuilder();

            if (this.HourlyRotation)
                maxWorkloadEvents = 60 * maxPartnerCount; // Number of minutes in an hour * partners.
            else
                maxWorkloadEvents = 1440 * maxPartnerCount; // Number of minutes in a day * partners.
            Hashtable workloadData = null;
            try
            {
                workloadData = new Hashtable(maxWorkloadEvents);
            }
            catch (Exception exception)
            {
                throw new SJPException(
                    String.Format(Messages.W3CReader_FailedAllocatingMemoryForData, exception.Message), false,
                    SJPExceptionIdentifier.RDPWebLogReaderFailedStoringWebLogData);
            }

            try
            {
                // Open web log file for processing.
                using (fileForHeader = File.OpenText(filePath))
                {

                    // Before processing web log data entries, must find the field definitions.
                    while (((webLogLine = fileForHeader.ReadLine()) != null) && (!fieldDefinitionsFound))
                    {
                        if (Regex.IsMatch(webLogLine, fieldDefinitionMarker))
                        {
                            // Get the field names (this may be a subset of the fields required to process line)
                            // Split the fields based on a space. (This assumes that fields do not include spaces.)
                            string[] webLogLineFieldNames = GetFieldNames(webLogLine, ' ');

                            // Get the positions of the relevant fields needed to process the line.
                            GetFieldPositions(webLogLineFieldNames, this.mandatoryFieldNames, fieldPositions);

                            fieldDefinitionsFound = true;
                        }
                    }

                    // Without field definitions it is not possible to process file!
                    if (!fieldDefinitionsFound)
                        throw new SJPException(Messages.W3CReader_NoFieldTokens, false, SJPExceptionIdentifier.RDPWebLogReaderNoFieldTokens);
                }

                // Reset to top of file (in case 1st definitions are not at top)
                file = File.OpenText(filePath);
                webLogLine = file.ReadLine();

                // Process the web log file (allowing for field definitions to be redefined).
                do
                {
                    if (0 == String.Compare(webLogLine, 0, this.nonDataPrefix, 0, 1))
                    {
                        // Non-data fields found.

                        if (Regex.IsMatch(webLogLine, fieldDefinitionMarker))
                        {
                            // Field definitions found (ie they have been redefined).
                            string[] webLogLineFieldNames = GetFieldNames(webLogLine, ' ');
                            GetFieldPositions(webLogLineFieldNames, this.mandatoryFieldNames, fieldPositions);
                        }

                    }
                    else
                    {
                        // Data fields found.

                        string[] webLogLineDataFields = webLogLine.Split(' ');
                        try
                        {
                            W3CWebLogData webLogData =
                            new W3CWebLogData(webLogLineDataFields[fieldPositions[uriFieldPosition]],
                                              Int32.Parse(webLogLineDataFields[fieldPositions[bytesFieldPosition]]),
                                              Int32.Parse(webLogLineDataFields[fieldPositions[statusFieldPosition]]),
                                              webLogLineDataFields[fieldPositions[clientIPFieldPosition]],
                                              webLogLineDataFields[fieldPositions[uriQueryFieldPosition]],
                                              webLogLineDataFields[fieldPositions[dateFieldPosition]],
                                              webLogLineDataFields[fieldPositions[timeFieldPosition]],
                                              webLogLineDataFields[fieldPositions[hostFieldPosition]],
                                              webLogLineDataFields[fieldPositions[userAgentFieldPosition]],
                                              webLogLineDataFields[fieldPositions[cookieFieldPosition]]);

                            // Store data if meets specification.
                            if (webLogData.MeetsSpecification(dataSpecification))
                            {
                                try
                                {
                                    // Key name is based on the web log data time logged.
                                    string key = webLogData.DateTimeLogged.ToString("g", formatProvider) + ";" + webLogData.PartnerId;
                                    int count;
                                    if (workloadData.ContainsKey(key))
                                    {
                                        int currentCount = Convert.ToInt32(((string)workloadData[key]).Split(';')[0], formatProvider);
                                        // Increment count for this log time.
                                        count = currentCount + 1;
                                    }
                                    else
                                    {
                                        count = 1;
                                    }

                                    //add the partner id (concatenate because of hashtable)
                                    workloadData[key] = count + ";" + webLogData.PartnerId;
                                }
                                catch (Exception exception)
                                {
                                    throw new SJPException(String.Format(Messages.W3CReader_FailureStoringData, exception.Message), false,
                                        SJPExceptionIdentifier.RDPWebLogReaderFailedStoringWebLogData);
                                }


                                // Log the user experience visitor data
                                if (webLogData.UserExperienceVisitor(dataSpecification))
                                {
                                    try
                                    {
                                        string sessionId = webLogData.GetSessionId(dataSpecification);

                                        if (!string.IsNullOrEmpty(sessionId))
                                        {

                                            WebLogDataWriter.WriteUserExperienceVisitorData(
                                                dataSpecification.UserExperienceVisitorUserAgent,
                                                string.Empty, sessionId, webLogData.Host,
                                                webLogData.UserAgent, 0,
                                                webLogData.DateTimeLogged, webLogData.DateTimeLogged);
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        throw new SJPException(String.Format(Messages.W3CReader_FailureStoringUserExperienceMonitoringData, exception.Message), false,
                                            SJPExceptionIdentifier.RDPWebLogReaderFailedStoringWebLogData);
                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            errorsCaughtCount++;
                            errorString.Append(webLogLine);

                            if (errorsCaughtCount == 10)
                            {
                                throw new SJPException(String.Format(Messages.W3CReader_FailureReadingWebLogFile, errorString.ToString(), exception.Message), false,
                                    SJPExceptionIdentifier.RDPWebLogReaderFailedReadingWebLog);
                            }
                        }

                    }

                } while ((webLogLine = file.ReadLine()) != null);
            }
            catch (SJPException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new SJPException(String.Format(Messages.W3CReader_FailureReadingWebLogFile, webLogLine, exception.Message), false,
                    SJPExceptionIdentifier.RDPWebLogReaderFailedReadingWebLog);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }


            // Log the workload events using the TD Event logging service.
            int eventsLogged = 0;
            try
            {
                System.Collections.IDictionaryEnumerator dataEnumerator = workloadData.GetEnumerator();

                while (dataEnumerator.MoveNext())
                {
                    //Seperate the value and the partner id
                    string[] values = ((string)dataEnumerator.Value).Split(';');


                    string[] enumeratorKey = ((string)dataEnumerator.Key).Split(';');


                    Trace.Write(new WorkloadEvent(DateTime.ParseExact((string)enumeratorKey[0], "g", formatProvider), Convert.ToInt32(values[0]), Convert.ToInt32(values[1])));
                    eventsLogged++;
                }
            }
            catch (Exception exception)
            {
                throw new SJPException(String.Format(Messages.W3CReader_FailureStoringData, exception.Message), false,
                    SJPExceptionIdentifier.RDPWebLogReaderFailedStoringWebLogData);
            }


            return eventsLogged;
        }

        /// <summary>
        /// Returns the filenames of web logs that should be treated
        /// as active and not be processed.
        /// File names are provided in W3C extended Log File format.
        /// </summary>
        /// <returns>Filenames of active web logs.</returns>
        public string[] GetActiveWebLogFileNames()
        {
            bool hourly = this.HourlyRotation;
            bool useLocal = this.UseLocalTime;

            if (SJPTraceSwitch.TraceVerbose)
            {
                if (hourly)
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, Messages.W3CReader_RolloverHourly));
                else
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, Messages.W3CReader_RolloverDaily));
            }

            if (SJPTraceSwitch.TraceVerbose)
            {
                if (useLocal)
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, Messages.W3CReader_LocalTime));
                else
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, Messages.W3CReader_UtcTime));
            }


            DateTime now;
            if (useLocal)
                now = DateTime.Now;
            else
                now = DateTime.UtcNow;

            string year = now.Year.ToString().Substring(2, 2);
            string month = now.Month.ToString();
            string day = now.Day.ToString();
            string hour = now.Hour.ToString();

            if (month.Length == 1)
                month = "0" + month;

            if (day.Length == 1)
                day = "0" + day;

            if (hour.Length == 1)
                hour = "0" + hour;

            string[] filenames = new string[2];

            if (!hourly)
            {
                filenames[0] = "ex" + year + month + day + ".log";
            }
            else
            {
                // If hourly then return log file for current hour. 
                //
                // Also return log file for next hour in
                // case IIS rolls to next hour before processing is started.
                //  - if situation occurs, then web log that was eliminated which was 
                // not active will be processed by web log reader on it's next run.

                DateTime next = now.AddMinutes(60);
                string nextYear = next.Year.ToString().Substring(2, 2);
                string nextMonth = next.Month.ToString();
                string nextDay = next.Day.ToString();
                string nextHour = next.Hour.ToString();

                if (nextMonth.Length == 1)
                    nextMonth = "0" + nextMonth;

                if (nextDay.Length == 1)
                    nextDay = "0" + nextDay;

                if (nextHour.Length == 1)
                    nextHour = "0" + nextHour;

                filenames[0] = "ex" + year + month + day + hour + ".log";
                filenames[1] = "ex" + nextYear + nextMonth + nextDay + nextHour + ".log";
            }

            return filenames;
        }

        #endregion
    }
}
