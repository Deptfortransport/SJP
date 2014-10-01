// *********************************************** 
// NAME             : InternalRequestEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: InternalRequestEvent
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
    /// Defines a custom event class for capturing internal request 
    /// event data in the CJP (ie calls to TTBO/road engine)
    /// </summary>
    [Serializable]
    public class InternalRequestEvent : CustomEvent
    {
        private string internalRequestId;
        private string sessionId;
        private DateTime submitted;
        private InternalRequestType requestType;
        private string functionType;
        private bool success;
        private bool refTransaction;

        /// <summary>
        /// Defines the formatter class that formats event data for use by a file publisher.
        /// Used by all instances of the JourneyWebRequestEvent class.
        /// </summary>
        private static IEventFormatter fileFormatter = new InternalRequestEventFileFormatter();

        #region Constructor

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="sessionId">Identifier to uniquely identify the session under which this journey web request was made.</param>
        /// <param name="internalRequestId">Identifier to uniquely identify this request.</param>
        /// <param name="submitted">Time that journey web request was submitted.</param>
        /// <param name="requestType">The type of request</param>
        /// <param name="functionType">The function code. Maximum two characters</param>
        /// <param name="success">True if request was successful, otherwise false.</param>
        /// <param name="refTransaction">True if request was triggered by a reference transaction, otherwise false.</param>
        public InternalRequestEvent(string sessionId,
            string internalRequestId,
            DateTime submitted,
            InternalRequestType requestType,
            string functionType,
            bool success,
            bool refTransaction)
            : base()
        {
            this.sessionId = sessionId;
            this.internalRequestId = internalRequestId;
            this.submitted = submitted;

            if (functionType.Length != 2)
            {
                // Throw/log an error
                SJPException e = new SJPException(String.Format("The function type [{0}] is invalid. Function types must be exactly two characters long.", functionType), true, SJPExceptionIdentifier.ELSInvalidFunctionType);
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, e.Message));
                throw e;
            }

            this.functionType = functionType;
            this.success = success;
            this.requestType = requestType;
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
        public string InternalRequestId
        {
            get { return internalRequestId; }
        }

        /// <summary>
        /// Gets the time that the journey web request was submitted.
        /// </summary>
        public DateTime Submitted
        {
            get { return submitted; }
        }

        /// <summary>
        /// Gets the region code.
        /// </summary>
        public InternalRequestType RequestType
        {
            get { return requestType; }
        }

        /// <summary>
        /// Gets the function type code
        /// </summary>
        public string FunctionType
        {
            get { return functionType; }
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
