// *********************************************** 
// NAME             : W3CWebLogData.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Defines the data within a W3C Web Log entry and methods to validate the data against a specification.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Defines the data within a W3C Web Log entry and methods to validate the
    /// data against a specification.
    /// </summary>
    public struct W3CWebLogData
    {
        private string uriStem;
        private int bytesSent;
        private int protocolStatus;
        private string clientIP;
        private string uriQuery;
        private string date;
        private string time;
        private string host;
        private string userAgent;
        private string cookie;

        /// <summary>
        /// Gets the datetime that the web log data was logged.
        /// </summary>
        /// <remarks>
        /// Derivation of data into DateTime type is performed in the property
        /// getter, rather than constuctor, since this property will only
        /// be called for a subset of web log entries.
        /// The seconds component is always defaulted to zero. This is because web log
        /// data is aggregated at a resolution of 1 minute and therefore seconds data is irrelevant.
        /// </remarks>
        public DateTime DateTimeLogged
        {
            get
            {
                // Split date and time data into integer components.
                int year = int.Parse(this.date.Substring(0, 4));
                int month = int.Parse(this.date.Substring(5, 2));
                int day = int.Parse(this.date.Substring(8, 2));
                int hour = int.Parse(this.time.Substring(0, 2));
                int minute = int.Parse(this.time.Substring(3, 2));
                int second = 0; // Default to zero - see remarks.

                // Create a datetime based on integer components.
                DateTime dateTimeLogged = new DateTime(year, month, day, hour, minute, second);

                // IIS always logs using Greenwich Meantime (GMT).
                // If the date is in DayLightSavingTime 
                // (also known as British Summer Time (BST))
                // then an hour must be added to get the 'real' local time.
                // (Since GMT is one hour behind BST.)
                if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dateTimeLogged))
                {
                    // Web log entry made in BST so add an hour: 
                    dateTimeLogged = dateTimeLogged.AddHours(1);
                }

                return dateTimeLogged;
            }
        }

        /// <summary>
        /// Constructs a W3C web log entry.
        /// </summary>
        /// <param name="uriStem">The URI stem data string.</param>
        /// <param name="bytesSent">The number of bytes sent.</param>
        /// <param name="protocolStatus">The HTTP protocol status number.</param>
        /// <param name="clientIP">The client IP address.</param>
        /// <param name="uriQuery">The URI query.</param>
        /// <param name="date">The date (as a string).</param>
        /// <param name="time">The time (as a string).</param>
        /// <param name="host">The host name (as a string).</param>
        public W3CWebLogData(string uriStem,
                             int bytesSent,
                             int protocolStatus,
                             string clientIP,
                             string uriQuery,
                             string date,
                             string time,
                             string host,
                             string userAgent,
                             string cookie)
        {
            this.uriStem = uriStem;
            this.bytesSent = bytesSent;
            this.protocolStatus = protocolStatus;
            this.clientIP = clientIP;
            this.uriQuery = uriQuery;
            this.date = date;
            this.time = time;
            this.host = host;
            this.userAgent = userAgent;
            this.cookie = cookie;
        }

        private bool CheckProtocolStatus(List<StatusCode> statusCodes)
        {
            // Assume protocol status code is not valid unless it is in a status code range
            bool valid = false;

            foreach (StatusCode statusCode in statusCodes)
            {
                if ((this.protocolStatus >= statusCode.MinProtocolStatusCode) && (this.protocolStatus <= statusCode.MaxProtocolStatusCode))
                {
                    valid = true;
                    break;
                }
            }

            return valid;
        }

        private bool CheckQueryString(string queryIgnoreMarker)
        {
            bool ok = true;

            if (Regex.IsMatch(this.uriQuery, queryIgnoreMarker))
            {
                ok = false;
            }

            return ok;
        }

        private bool CheckBytesSent(int minSize)
        {
            return (bytesSent >= minSize);
        }

        private bool CheckClientIP(string[] clientIPExcludes)
        {
            bool ok = true;

            for (int i = 0; i < clientIPExcludes.Length; i++)
            {
                if (Regex.IsMatch(this.clientIP, clientIPExcludes[i]))
                {
                    ok = false;
                    break;
                }
            }

            return ok;
        }

        private bool CheckFileType(string[] fileExtensions, string noFileExtensionMarker, bool allowNoFileExtension)
        {
            string[] uriStemSplit = this.uriStem.Split('.');
            bool ok = false;

            if (uriStemSplit.Length == 2)
            {
                // URI Stem includes a file extension.
                for (int i = 0; i < fileExtensions.Length; i++)
                {
                    // don't compare when value is no fileextensionmarker
                    if (fileExtensions[i] != noFileExtensionMarker)
                    {
                        if (Regex.IsMatch(uriStemSplit[1], fileExtensions[i]))
                        {
                            ok = true;
                            break;
                        }
                    }
                }
            }
            else if (uriStemSplit.Length == 1)
            {
                // URI Stem does NOT include a file extension.
                if (allowNoFileExtension)
                    ok = true;
            }

            return ok;
        }

        /// <summary>
        /// Determines whether the web log entry meets the requirements defined
        /// by the specification passed.
        /// </summary>
        /// <param name="spec">Specification to test.</param>
        /// <returns>True if specification has been met, else false.</returns>
        public bool MeetsSpecification(WebLogDataSpecification specification)
        {

            return (CheckProtocolStatus(specification.ProtocolStatusCode) &&
                    (CheckFileType(specification.WebPageFileExtensions, specification.NoFileExtensionMarker, specification.AllowNoFileExtension) || CheckBytesSent(specification.MinNonWebPageSize)) &&
                    CheckQueryString(specification.QueryIgnoreMarker) &&
                    CheckClientIP(specification.ClientIPExcludes));

        }

        /// <summary>
        /// Determines whether the web log entry is for the UserExperienceMonitoring service.
        /// Returns true if the UserAgent log data contains a value (from properties) for the UEM service
        /// </summary>
        /// <param name="spec">Specification containing user agent string to search for</param>
        /// <returns>True if user agent string found</returns>
        public bool UserExperienceVisitor(WebLogDataSpecification specification)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                if (!string.IsNullOrEmpty(specification.UserExperienceVisitorUserAgent))
                {
                    // Check if user agent contains the specified UEM string
                    if (userAgent.ToLower().Contains(specification.UserExperienceVisitorUserAgent))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the session id from the web log entry, looking for the session id in the cookie string
        /// with the specification containing the start and end strings for which the session id is in,
        /// e.g. "ASP.NET_SessionId=q3jmjq45f0dddvnx3t3undmn;"
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public string GetSessionId(WebLogDataSpecification specification)
        {
            string sessionId = string.Empty;

            if (!string.IsNullOrEmpty(cookie))
            {
                if (!string.IsNullOrEmpty(specification.CookieSessionIdStartMarker) &&
                    !string.IsNullOrEmpty(specification.CookieSessionIdEndMarker))
                {
                    if (cookie.Contains(specification.CookieSessionIdStartMarker))
                    {
                        // Extract the string from the cookie where the session id starts
                        string sessionPart = cookie.Substring(
                            (cookie.IndexOf(specification.CookieSessionIdStartMarker) + (specification.CookieSessionIdStartMarker.Length)));

                        if (!string.IsNullOrEmpty(sessionPart))
                        {
                            // Extract the session id from the string to where the session id ends
                            if (sessionPart.Contains(specification.CookieSessionIdEndMarker))
                            {
                                sessionId = sessionPart.Substring(0, sessionPart.IndexOf(specification.CookieSessionIdEndMarker));
                            }
                            else
                            {
                                sessionId = sessionPart;
                            }
                        }
                    }
                }
            }

            return sessionId;
        }

        /// <summary>
        /// Read only property returning the partnerId from the partnercatalogue depending on the hostname.
        /// To allow Web log reader to produce single reports for sites using multiple hostnames any hostname
        /// with a known DisplayName returns the parent partner Id for that Partner.
        /// </summary>
        public int PartnerId
        {
            get
            {
                // SJP has no concept of partner, so always return 0
                int id = 0;
                
                return id;
            }
        }

        /// <summary>
        /// Read only. UserAgent
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
        }

        /// <summary>
        /// Read only. Host
        /// </summary>
        public string Host
        {
            get { return host; }
        }

    }
}
