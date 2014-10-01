// *********************************************** 
// NAME             : LogEvent.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Abstract class representing a log event.
// ************************************************            
                
using System;
using System.Text;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Class representing a logging event.
    /// Has serializable attribute to allow events to be published on MSMQs
    /// </summary>
    [Serializable]
    public abstract class LogEvent
    {
        #region Private Fields
        private DateTime time;

        private StringBuilder publishedBy;
        private string className;
        private bool auditPublishersOff;
        #endregion

        #region Abstract Properties
        /// <summary>
        /// Gets the File Formatter.
        /// </summary>
        abstract public IEventFormatter FileFormatter { get; }

        /// <summary>
        /// Gets the Event Log Formatter.
        /// </summary>
        abstract public IEventFormatter EventLogFormatter { get; }

        /// <summary>
        /// Gets the Email Formatter.
        /// </summary>
        abstract public IEventFormatter EmailFormatter { get; }

        /// <summary>
        /// Gets the Console Formatter.
        /// </summary>
        abstract public IEventFormatter ConsoleFormatter { get; }

        /// <summary>
        /// Gets the Filter.
        /// </summary>
        abstract public IEventFilter Filter { get; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets and sets the string containing the list of publisher class types that
        /// have published the event.
        /// </summary>
        public string PublishedBy
        {
            get { return publishedBy.ToString(); }

            set { publishedBy.Append(value + " "); }
        }

        /// <summary>
        /// Gets and sets audit publisher indicator.
        /// Has value <c>true</c> if auditing is performed on event, otherwise <c>false</c>.
        /// </summary>
        public bool AuditPublishersOff
        {
            get { return auditPublishersOff; }
            set { auditPublishersOff = value; }
        }

        /// <summary>
        /// Gets the time at which the event was logged.
        /// </summary>
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        
        /// <summary>
        /// Gets and sets the class name. The class name does not include the full namespace path.
        /// The class name is used as a mechanism for associating events with publishers in SJPTraceListener.
        /// </summary>
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor used to create a log event.
        /// </summary>
        protected LogEvent()
        {
            this.time = DateTime.Now;

            this.publishedBy = new StringBuilder();

            this.AuditPublishersOff = true; // turn auditing off by default as it incurs runtime hit
        }

        #endregion

    }
}
