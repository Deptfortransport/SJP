// *********************************************** 
// NAME             : Messages.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Messages used by classes in the WebLogReader project.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Messages used by classes in the WebLogReader project
    /// </summary>
    public class Messages
    {
        // Initialisation Messages
        public const string Init_DotNETTraceListenerFailed = "Failed to initialise a default .NET trace listener. Reason:[{0}].";
        public const string Init_InitialisationStarted = "Initialisation of Web Log Reader started.";
        public const string Init_Completed = "Initialisation of Web Log Reader completed successfully.";
        public const string Init_ServiceAddFailed = "Failed to add a service to the cache: [{0}].";
        public const string Init_TraceListenerFailed = "Failed to initialise the Trace Listener class: {0} Message: {1}";
        public const string Init_Usage = "Usage: WebLogReader [/help|/test]";
        public const string Init_UnknownPropertyKey = "Attempt to validate unknown property key: [{0}].";
        public const string Init_ReaderProperties = "One or more errors found in Web Log Reader properties: [{0}]";

        // Reader Messages
        public const string Reader_Failed = "Web Log Reader failed. Reason:[{0}] Id:[{1}]";
        public const string Reader_TestSucceeded = "Web Log Reader was run in test mode and succeeded.";
        public const string Reader_InvalidArg = "Invalid argument/s passed to web log reader.";
        public const string Reader_Started = "Web Log Reader initialised successfully and is processing web log/s.";
        public const string Reader_Completed = "Web Log Reader completed successfully.";

        // Controller Messages
        public const string Controller_NoWebLogs = "Web Log Reader determined that there were no web log files to process.";
        public const string Controller_FailedProcessingWebLog = "Failed to process web log [{0}]. Reason: [{1}]. Prior to this failure, [{2}] web logs were processed successfully.";
        public const string Controller_FailedArchivingFile = "Failed to archive a processed web log file. Reason: [{0}]";
        public const string Controller_NumLogs = "Web Log Reader determined that there is/are [{0}] web log/s ready to process.";
        public const string Controller_NumLogsProcessed = "Web Log Reader successfully processed [{0}] web log/s.";
        public const string Controller_NumWorkloadEvents = "Web Log Reader logged [{0}] workload events for web log file [{1}].";
        public const string Controller_ZeroWorkloadEvents = "Web Log Reader logged zero workload events for web log file [{0}].";

        // Validate Messages

        public const string Validation_MissingWebLogsFolders = "Web log folders property specified in key [{0} does not exist or is empty.";
        public const string Validation_BadArchiveDir = "Web Log archive directory [{0}] specified in key [{1}] does not exist.";
        public const string Validation_BadLogDir = "Web Log log directory [{0}] specified in key [{1}] does not exist.";
        public const string Validation_NonPageMinimumBytesInvalid = "Non page minimum bytes value [{0}] specified in key [{1}] is invalid. Value must be zero or greater.";
        public const string Validation_TimeZoneInvalid = "Timezone setting of machine on which web logs reside is invalid. Time zone must be set to GMT.";
        public const string Validation_IPAddressInvalid = "Client IP Address Exclude [{0}] specified in key [{1}] is not in the correct format.";

        // W3C Reader messages
        public const string W3CReader_MissingFields = "Mandatory field [{0}] missing from web log file. File must contain the following fields in each entry: [{1}]";
        public const string W3CReader_NoFieldTokens = "No field name tokens have been included in web log file. These are used to determine which data maps to which fields.";
        public const string W3CReader_FailureStoringData = "Web Log Reader failure when storing workload event data for a web log file. Reason:[{0}]";
        public const string W3CReader_FailureStoringUserExperienceMonitoringData = "Web Log Reader failure when storing user experience monitoring data for a web log file. Reason:[{0}]";
        public const string W3CReader_FailureReadingWebLogFile = "Web Log Reader failure when reading a web log file entry [{0}]. Reason:[{1}]";
        public const string W3CReader_RolloverDaily = "Web Log Reader determined active web logs using Daily rollover.";
        public const string W3CReader_RolloverHourly = "Web Log Reader determined active web logs using Hourly rollover.";
        public const string W3CReader_LocalTime = "Web Log Reader determined active web logs using local machine time.";
        public const string W3CReader_UtcTime = "Web Log Reader determined active web logs using GMT.";
        public const string W3CReader_FailedAllocatingMemoryForData = "Web Log Reader failed to allocate memory to store the web log data. Reason:[{0}]";
        public const string W3CReader_InvalidUseLocalTimeValue = "The property value UseLocalTime ({0}) could not be converted to a boolean value. The WebLogReader will default to using GMT.";
        public const string W3CReader_LoggedUserExperienceVisitor = "Web Log Reader logged user experience visitor with sessionId[{0}]";

    }
}
