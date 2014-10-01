// *********************************************** 
// NAME             : RepeatVisitorEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging repeat visitor event data.
// ************************************************
// 

using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging repeat visitor event data.
    /// </summary>
    [Serializable()]
    public class RepeatVisitorEvent : SJPCustomEvent
    {
        #region Private members

        private static RepeatVisitorEventFileFormatter fileFormatter = new RepeatVisitorEventFileFormatter();

        private RepeatVisitorType repeatVisitorType = RepeatVisitorType.VisitorUnknown;
        private string sessionIdOld = string.Empty;
        private DateTime lastVisitedDateTime = DateTime.MinValue;
        private string lastPageVisited = string.Empty;
        private string domain = string.Empty;
        private string userAgent = string.Empty;
        private int themeId = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the RepeatVisitorEvent to log events
        /// </summary>
        /// <param name="sessionIdOld"></param>
        /// <param name="sessionIdNew"></param>
        /// <param name="lastVisitedDateTime"></param>
        /// <param name="domain"></param>
        /// <param name="userAgent"></param>
        public RepeatVisitorEvent(RepeatVisitorType repeatVisitorType, string sessionIdOld, string sessionIdNew, DateTime lastVisitedDateTime,
            string lastPageVisted, string domain, string userAgent)
            : base(sessionIdNew, false)
        {
            this.repeatVisitorType = repeatVisitorType;
            this.sessionIdOld = sessionIdOld;
            this.lastVisitedDateTime = lastVisitedDateTime;
            this.lastPageVisited = lastPageVisted;
            this.domain = domain;
            this.userAgent = userAgent;
            this.themeId = 0;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the repeatVisitorType
        /// </summary>
        public RepeatVisitorType RepeatVisitorType
        {
            get { return repeatVisitorType; }
        }

        /// <summary>
        /// Gets the sessionIdOld
        /// </summary>
        public string SessionIdOld
        {
            get { return sessionIdOld; }
        }

        /// <summary>
        /// Gets the lastVisitedDateTime
        /// </summary>
        public DateTime LastVisitedDateTime
        {
            get { return lastVisitedDateTime; }
        }

        /// <summary>
        /// Gets the lastPageVisited
        /// </summary>
        public string LastPageVisted
        {
            get { return lastPageVisited; }
        }

        /// <summary>
        /// Gets the domain
        /// </summary>
        public string Domain
        {
            get { return domain; }
        }

        /// <summary>
        /// Gets the userAgent
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
        }

        /// <summary>
        /// Returns themeId
        /// </summary>
        public int ThemeId
        {
            get { return themeId; }
        }

        /// <summary>
        /// Provides an event formatter for publishing to files.
        /// </summary>
        override public IEventFormatter FileFormatter
        {
            get { return fileFormatter; }
        }

        #endregion
    }
}