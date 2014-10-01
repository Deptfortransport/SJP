// *********************************************** 
// NAME             : NoResultsEvent      
// AUTHOR           : Phil Scott
// DATE CREATED     : 15 Mar 2012
// DESCRIPTION      : Defines a custom event for logging No Results event data.
// ************************************************
// 

using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging No Results event data
    /// </summary>
    [Serializable()]
    public class NoResultsEvent : SJPCustomEvent
    {
        #region Private members

        private DateTime submitted;

        private static NoResultsEventFileFormatter fileFormatter = new NoResultsEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a <c>NoResultsEvent</c> class. 
        /// A <c>NoResultsEvent</c> is used
        /// to log no results event data using the Event Service.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <param name="sessionId">The session id on which the page was entered.</param>
        public NoResultsEvent(DateTime submitted,
               string sessionId,
               bool userLoggedOn)
            : base(sessionId,userLoggedOn)
        {
            this.submitted = submitted;
        }

        #endregion

        #region Public properties


        /// <summary>
        /// Provides an event formatter for publishing to files.
        /// </summary>
        override public IEventFormatter FileFormatter
        {
            get { return fileFormatter; }
        }


        /// <summary>
        /// Gets the date/time at which the reference transaction was submitted.
        /// </summary>
        public DateTime Submitted
        {
            get { return submitted; }
        }

        #endregion
    }
}
