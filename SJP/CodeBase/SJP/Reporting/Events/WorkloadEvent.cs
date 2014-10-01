// *********************************************** 
// NAME             : WorkloadEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging workload data.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging workload data.
    /// </summary>
    [Serializable()]
    public class WorkloadEvent : SJPCustomEvent
    {
        #region Private members

        private DateTime requested;
		private int numberRequested;
        private int partnerId;

		private static WorkloadEventFileFormatter fileFormatter = new WorkloadEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a WorkloadEvent class. A WorkloadEvent class is used
        /// to capture web page request data.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <remarks>
        /// A workload event is used to capture the number of one or more web page
        /// requests in a given time frame. Note that earlier versions of this
        /// class only allowed capture of a single web page request. A decision was made
        /// to aggregate requests to improve performance.
        /// </remarks>
        /// <param name="timeRequested">DateTime at which the web page request were made.</param>
        /// <param name="numberRequested">The number of web page requests that were made in the give <c>timeRequested</c>.</param>
        public WorkloadEvent(DateTime timeRequested, int numberRequested)
            : base(string.Empty, false)
        {
            this.requested = timeRequested;
            this.numberRequested = numberRequested;
        }

        /// <summary>
        /// Constructor for a WorkloadEvent class. A WorkloadEvent class is used
        /// to capture web page request data.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <remarks>
        /// A workload event is used to capture the number of one or more web page
        /// requests in a given time frame. Note that earlier versions of this
        /// class only allowed capture of a single web page request. A decision was made
        /// to aggregate requests to improve performance.
        /// </remarks>
        /// <param name="timeRequested">DateTime at which the web page request were made.</param>
        /// <param name="numberRequested">The number of web page requests that were made in the give <c>timeRequested</c>.</param>
        /// <param name="partnerId">Id of the partner site</param>
        public WorkloadEvent(DateTime timeRequested, int numberRequested, int partnerId)
            : base(string.Empty, false)
        {
            this.requested = timeRequested;
            this.numberRequested = numberRequested;
            this.partnerId = partnerId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the date/time at which the request for the resource was made.
        /// </summary>
        public DateTime Requested
        {
            get { return requested; }
        }

        /// <summary>
        /// Gets the number of web pages requested at the time stored in <c>Requested</c>.
        /// </summary>
        public int NumberRequested
        {
            get { return numberRequested; }
        }

        /// <summary>
        /// read only property: Id of the partner site
        /// </summary>
        public int PartnerId
        {
            get { return partnerId; }
        }

        /// <summary>
        /// Provides an event formatter for publishing the event using a file publisher.
        /// </summary>
        override public IEventFormatter FileFormatter
        {
            get { return fileFormatter; }
        }

        #endregion
    }
}
