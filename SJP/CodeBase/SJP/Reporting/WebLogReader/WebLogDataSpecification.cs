// *********************************************** 
// NAME             : WebLogDataSpecification      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Structure used to define web log entry specification
// ************************************************
// 
                
using System;
using System.Collections.Generic;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Structure used to define web log entry specification.
    /// This specification defines the requirements that must be met in order
    /// for the web log reader to log a <code>WorkloadEvent</code> for a web log
    /// entry in the web log being processed.
    /// </summary>
    public struct WebLogDataSpecification
    {
        private string[] webPageFileExtensions;
        public string[] WebPageFileExtensions
        {
            get { return webPageFileExtensions; }
        }

        private List<StatusCode> protocolStatusCodes;
        public List<StatusCode> ProtocolStatusCode
        {
            get { return protocolStatusCodes; }
        }
        
        private int minNonWebPageSize;
        public int MinNonWebPageSize
        {
            get { return minNonWebPageSize; }
        }

        private string[] clientIPExcludes;
        public string[] ClientIPExcludes
        {
            get { return clientIPExcludes; }
        }

        private string queryIgnoreMarker;
        public string QueryIgnoreMarker
        {
            get { return queryIgnoreMarker; }
        }

        private string noFileExtensionMarker;
        public string NoFileExtensionMarker
        {
            get { return noFileExtensionMarker; }
        }

        private bool allowNoFileExtension;
        public bool AllowNoFileExtension
        {
            get { return allowNoFileExtension; }
        }

        private string userExperienceVisitorUserAgent;
        public string UserExperienceVisitorUserAgent
        {
            get { return userExperienceVisitorUserAgent; }
        }

        private string cookieSessionIdStartMarker;
        public string CookieSessionIdStartMarker
        {
            get { return cookieSessionIdStartMarker; }
        }

        private string cookieSessionIdEndMarker;
        public string CookieSessionIdEndMarker
        {
            get { return cookieSessionIdEndMarker; }
        }

        /// <summary>
        /// Creates a web log data specification.
        /// </summary>
        /// <param name="webPageFileExtensions">Array of file extensions that are considered to be a web page.</param>
        /// <param name="minStatusCode">Minimum HTTP status code.</param>
        /// <param name="maxStatusCode">Maximum HTTP status code.</param>
        /// <param name="minSize">Minimum size in bytes for pages that do NOT have a valid file extension.</param>
        /// <param name="clientIPExcludes">IP addresses to exclude.</param>
        /// <param name="queryIgnoreMarker">Marker string that if found in query string, signifies exclusion.</param>
        /// <param name="noFileExtensionMarker">Marker string used to specify that web log entries with no file extension should be considered as a web page.</param>
        public WebLogDataSpecification(string[] webPageFileExtensions,
                                       List<StatusCode> protocolStatusCodes,
                                       int minNonWebPageSize,
                                       string[] clientIPExcludes,
                                       string queryIgnoreMarker,
                                       string noFileExtensionMarker,
                                       string userExperienceVisitorUserAgent,
                                       string cookieSessionIdStartMarker,
                                       string cookieSessionIdEndMarker)
        {
            this.webPageFileExtensions = new String[webPageFileExtensions.Length];
            this.clientIPExcludes = new String[clientIPExcludes.Length];
            Array.Copy(webPageFileExtensions, this.webPageFileExtensions, webPageFileExtensions.Length);
            Array.Copy(clientIPExcludes, this.clientIPExcludes, clientIPExcludes.Length);
            this.protocolStatusCodes = protocolStatusCodes;
            this.minNonWebPageSize = minNonWebPageSize;
            this.queryIgnoreMarker = queryIgnoreMarker;
            this.noFileExtensionMarker = noFileExtensionMarker;
            this.userExperienceVisitorUserAgent = userExperienceVisitorUserAgent.ToLower();
            this.cookieSessionIdStartMarker = cookieSessionIdStartMarker;
            this.cookieSessionIdEndMarker = cookieSessionIdEndMarker;
            this.allowNoFileExtension = false;
            for (int i = 0; i < webPageFileExtensions.Length; i++)
            {
                if (webPageFileExtensions[i] == noFileExtensionMarker)
                {
                    this.allowNoFileExtension = true;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Structure used to define a min and max status code
    /// </summary>
    public struct StatusCode
    {
        private int minProtocolStatusCode;
        public int MinProtocolStatusCode
        {
            get { return minProtocolStatusCode; }
        }

        private int maxProtocolStatusCode;
        public int MaxProtocolStatusCode
        {
            get { return maxProtocolStatusCode; }
        }

        /// <summary>
        /// Creates a StatusCode
        /// </summary>
        /// <param name="minStatusCode"></param>
        /// <param name="maxStatusCode"></param>
        public StatusCode(int minStatusCode, int maxStatusCode)
        {
            this.minProtocolStatusCode = minStatusCode;
            this.maxProtocolStatusCode = maxStatusCode;
        }
    }

    /// <summary>
    /// Enumeration containing log rollover periods.
    /// ie the period after which IIS creates a new web log file.
    /// </summary>
    public enum LogRolloverPeriods : int
    {
        Hourly,
        Daily
    }
}
