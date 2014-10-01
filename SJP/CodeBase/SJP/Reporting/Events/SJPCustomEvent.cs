// *********************************************** 
// NAME             : SJPCustomEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Defines a base custom event class from which SJP Custom Events can derive.
// ************************************************
// 

using System;
using SJP.Common.EventLogging;
using EL = SJP.Common.EventLogging;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a base custom event class from which SJP Custom Events can derive.
    /// </summary>
    [Serializable()]
    public class SJPCustomEvent : CustomEvent
    {
        #region Private members

        private string sessionId;
        private bool userLoggedOn;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <remarks>
        /// Instances of this class will never be logged. 
        /// Only subclasses may be loggged.
        /// </remarks>
        /// <param name="sessionId">Session identifier that identifies session under which event was logged.</param>
        /// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or an unregistered user is logged on (false)</param>
        protected SJPCustomEvent(string sessionId, bool userLoggedOn)
            : base()
        {
            this.sessionId = sessionId;
            this.userLoggedOn = userLoggedOn;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the session identifier
        /// </summary>
        public string SessionId
        {
            get { return sessionId; }
        }

        /// <summary>
        /// Gets the user logged on flag
        /// </summary>
        public bool UserLoggedOn
        {
            get { return userLoggedOn; }
        }

        /// <summary>
        /// Provides an event formatter for publishing to files.
        /// </summary>
        override public IEventFormatter FileFormatter
        {
            get { return EL.DefaultFormatter.Instance; }
        }

        /// <summary>
        /// Provides an event formatting for publishing to email.
        /// </summary>
        override public IEventFormatter EmailFormatter
        {
            get { return EL.DefaultFormatter.Instance; }
        }

        /// <summary>
        /// Provides an event formatter for publishing to event logs
        /// </summary>
        override public IEventFormatter EventLogFormatter
        {
            get { return EL.DefaultFormatter.Instance; }
        }

        /// <summary>
        /// Provides an event formatter for publishing to console.
        /// </summary>
        override public IEventFormatter ConsoleFormatter
        {
            get { return EL.DefaultFormatter.Instance; }
        }

        #endregion
    }
}