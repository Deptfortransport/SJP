// *********************************************** 
// NAME             : JourneyWebRequestEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Defines a custom event class
// for capturing Journey Web Request event data in
// the CJP.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;
using SJP.Common;
using EL = SJP.Common.EventLogging;

using Logger = System.Diagnostics.Trace;


namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines the JourneyWebRequestEvent class used for capturing
    /// Journey Web Request event data within the CJP.
    /// </summary>
    [Serializable]
    public class JourneyWebRequestEvent : CustomEvent
    {
        private string journeyWebRequestId;
        private string sessionId;
        private DateTime submitted;
        private JourneyWebRequestType requestType;
        private string regionCode;
        private bool success;
        private bool refTransaction;

        /// <summary>
        /// Defines the formatter class that formats event data for use by a file publisher.
        /// Used by all instances of the JourneyWebRequestEvent class.
        /// </summary>
        private static IEventFormatter fileFormatter = new JourneyWebRequestEventFileFormatter();

        #region Constructor

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor does NOT perfrom any validation on arguments.
        /// </remarks>
        /// <param name="sessionId">Identifier to uniquely identify the session under which this journey web request was made.</param>
        /// <param name="journeyWebRequestId">Identifier to uniquely identify this journey web request.</param>
        /// <param name="submitted">Time that journey web request was submitted.</param>
        /// <param name="regionCode">The region code relating to the request.</param>
        /// <param name="success">True if request was successful, otherwise false.</param>
        /// <param name="refTransaction">True if request was triggered by a reference transaction, otherwise false.</param>
        public JourneyWebRequestEvent(string sessionId,
                                      string journeyWebRequestId,
                                      DateTime submitted,
                                      JourneyWebRequestType requestType,
                                      string regionCode,
                                      bool success,
                                      bool refTransaction)
            : base()
        {
            this.sessionId = sessionId;
            this.journeyWebRequestId = journeyWebRequestId;
            this.submitted = submitted;
            this.requestType = requestType;
            this.success = success;
            this.regionCode = regionCode;
            this.refTransaction = refTransaction;
        }

        #endregion

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        public string SessionId
        {
            get { return sessionId; }
        }

        /// <summary>
        /// Gets the journey web request identifier.
        /// </summary>
        public string JourneyWebRequestId
        {
            get { return journeyWebRequestId; }
        }

        /// <summary>
        /// Gets the time that the journey web request was submitted.
        /// </summary>
        public DateTime Submitted
        {
            get { return submitted; }
        }

        /// <summary>
        /// Gets the type of request that was submitted
        /// </summary>
        public JourneyWebRequestType RequestType
        {
            get { return requestType; }
        }

        /// <summary>
        /// Gets the region code.
        /// </summary>
        public string RegionCode
        {
            get { return regionCode; }
        }

        /// <summary>
        /// Gets the success indicator.
        /// A value of true indicates that the journey web request was successful.
        /// </summary>
        public bool Success
        {
            get { return success; }
        }

        /// <summary>
        /// Gets the reference transaction indicator.
        /// A value of true indicates that a reference transaction 
        /// was used to trigger the journey web request.
        /// </summary>
        public bool RefTransaction
        {
            get { return refTransaction; }
        }
                
        /// <summary>
        /// Provides an event formatter for publishing to files.
        /// </summary>
        override public IEventFormatter FileFormatter
        {
            get { return fileFormatter; }
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
    }
}
