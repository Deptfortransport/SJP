// *********************************************** 
// NAME             : ReferenceTransactionEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging reference transaction data.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging reference transaction data.
    /// </summary>
    [Serializable()]
    public class ReferenceTransactionEvent : SJPCustomEvent
    {
        #region Private members

        private DateTime submitted;
		private string category;
		private bool serviceLevelAgreement;
		private bool successful;
        private string machineName;

		private static ReferenceTransactionEventFileFormatter fileFormatter = new ReferenceTransactionEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
		/// Constructor for a <c>ReferenceTransactionEvent</c> class. 
		/// A <c>ReferenceTransactionEvent</c> is used
		/// to log reference transaction data using the Event Service.
		/// This class must be serializable to allow logging to MSMQs.
		/// </summary>
		/// <param name="submitted">Date/Time that reference transaction was submitted, down to the millisecond.</param>
		/// <param name="sessionId">The session id used to perform the reference transaction.</param>
		/// <param name="category">The category of reference transaction.</param>
		/// <param name="successful">Indicates whether reference transaction returned the expected results.</param>
		/// <param name="serviceLevelAgreement">Indicates whether reference transaction is used to calculate if SLAs have been met.</param>
		public ReferenceTransactionEvent(string category,
										 bool serviceLevelAgreement,
										 DateTime submitted,
										 string sessionId,
                                         bool successful,
                                         string machineName)
            : base(sessionId, false)
		{
			this.submitted = submitted;
			this.category = category;
			this.serviceLevelAgreement = serviceLevelAgreement;
			this.successful = successful;
            this.machineName = machineName;
		}

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the date/time at which the reference transaction was submitted.
        /// </summary>
        public DateTime Submitted
        {
            get { return submitted; }
        }

        /// <summary>
        /// Gets the event category.
        /// </summary>
        public string EventType
        {
            get { return category; }
        }

        /// <summary>
        /// Gets the SLA indicator. True if transaction is used for SLA calculations.
        /// </summary>
        public bool ServiceLevelAgreement
        {
            get { return serviceLevelAgreement; }
        }

        /// <summary>
        /// Gets the success indicator. True if transaction completed successfully.
        /// </summary>
        public bool Successful
        {
            get { return successful; }
        }



        /// <summary>
        /// Gets the MachineName of the injector service.
        /// </summary>
        public string MachineName
        {
            get { return machineName; }
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
