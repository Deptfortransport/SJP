// *********************************************** 
// NAME             : Keys.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: String Formatters for Properties file
// ************************************************
// 


namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// String Formatters for Properties file
    /// </summary>
    public class Keys
    {
        public const string WebLogReaderWebLogFolders = "WebLogReader.WebLogFolders";
        public const string WebLogReaderArchiveDirectory = "WebLogReader.{0}.ArchiveDirectory";
        public const string WebLogReaderLogDirectory = "WebLogReader.{0}.LogDirectory";
        public const string WebLogReaderNonPageMinimumBytes = "WebLogReader.NonPageMinimumBytes";
        public const string WebLogReaderWebPageExtensions = "WebLogReader.WebPageExtensions";
        public const string WebLogReaderClientIPExcludes = "WebLogReader.ClientIPExcludes";
        public const string WebLogReaderRolloverPeriod = "WebLogReader.RolloverPeriod";
        public const string WebLogReaderUseLocalTime = "WebLogReader.UseLocalTime";
        public const string WebLogReaderValidStatusCodeRanges = "WebLogReader.ValidStatusCode.Ranges";
        public const string WebLogReaderValidStatusCodeRangeMin = "WebLogReader.ValidStatusCode.{0}.Min";
        public const string WebLogReaderValidStatusCodeRangeMax = "WebLogReader.ValidStatusCode.{0}.Max";
        public const string WebLogReaderUserExperienceVisitorUserAgent = "WebLogReader.UserExperienceVisitor.UserAgent";
        public const string WebLogReaderCookieSessionIdStartMarker = "WebLogReader.Cookie.SessionId.StartMarker";
        public const string WebLogReaderCookieSessionIdEndMarker = "WebLogReader.Cookie.SessionId.EndMarker";
    }
}
