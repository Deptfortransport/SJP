// *********************************************** 
// NAME             : LandingPageEntryEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging landing page entry event data
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging landing page entry event data
    /// </summary>
    [Serializable()]
    public class LandingPageEntryEvent : SJPCustomEvent
    {
        #region Private members

        private static LandingPageEntryEventFileFormatter fileFormatter = new LandingPageEntryEventFileFormatter();

        private string partnerID;
		private LandingPageService serviceID;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a <c>LandingPageEntryEvent</c> class. 
        /// A <c>LandingPageEntryEvent</c> is used
        /// to log page entry transaction data using the Event Service.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <param name="partnerId">The partner id of the client making requests to the landing page.</param>
        /// <param name="eventType">The page identifier of the page entered.</param>
        /// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or not (false).</param>
        public LandingPageEntryEvent(string partnerId, LandingPageService serviceId, string sessionID, bool userLoggedOn)
            : base(sessionID, userLoggedOn)
        {
            this.partnerID = partnerId;
            this.serviceID = serviceId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read only - Gets the Partner ID
        /// </summary>
        public string PartnerID
        {
            get { return partnerID; }
        }

        /// <summary>
        /// Read only - Gets the Service ID
        /// </summary>
        public LandingPageService ServiceID
        {
            get { return serviceID; }
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
