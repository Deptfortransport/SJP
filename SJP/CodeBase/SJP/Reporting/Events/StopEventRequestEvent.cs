// *********************************************** 
// NAME             : StopEventRequestEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Defines a	custom event class for capturing StopEventRequest event data in CJP
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a	custom event class for capturing StopEventRequest event data in CJP
    /// </summary>
    [Serializable()]
    public class StopEventRequestEvent : SJPCustomEvent
    {
        #region Private members

        private string requestId = string.Empty;
		private DateTime submitted;
		private StopEventRequestType requestType = StopEventRequestType.Time;
		private bool isSuccess = false;

        private static StopEventRequestEventFileFormatter fileFormatter = new StopEventRequestEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a <c>StopEventRequestEvent</c> class. 
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        public StopEventRequestEvent(
			string requestId,
			DateTime submitted,
			StopEventRequestType requestType,
			bool success)
            : base(string.Empty, false)
        {
            this.requestId = requestId;
            this.submitted = submitted;
            this.requestType = requestType;
            this.isSuccess = success;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read only property. Holds the Request Id info.
        /// </summary>
        public string RequestId
        {
            get { return requestId; }
        }

        /// <summary>
        /// Read only property. Holds the submitted date and Time for the event.
        /// </summary>
        public DateTime Submitted
        {
            get { return submitted; }
        }

        /// <summary>
        /// Read only property. Holds the type of request (First, Last, Time)
        /// </summary>
        public StopEventRequestType RequestType
        {
            get { return requestType; }
        }



        /// <summary>
        /// Read only property. Holds if request was successful.
        /// </summary>
        public bool Success
        {
            get { return isSuccess; }
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
