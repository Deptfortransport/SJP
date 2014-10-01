// *********************************************** 
// NAME             : RetailerHandoffEvent      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging retailer handoff event data.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging retailer handoff event data
    /// </summary>
    [Serializable()]
    public class RetailerHandoffEvent : SJPCustomEvent
    {
        #region Private members

        private string retailerId;

        private static RetailerHandoffEventFileFormatter fileFormatter = new RetailerHandoffEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a <c>RetailerHandoffEvent</c> class. 
        /// A <c>RetailerHandoffEvent</c> is used
        /// to log retailer handoff event data using the Event Service.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <param name="sessionId">The session id on which the page was entered.</param>
        /// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or not (false).</param>
        public RetailerHandoffEvent(string retailerId,
                                    string sessionId,
                                    bool userLoggedOn)
            : base(sessionId, userLoggedOn)
        {
            this.retailerId = retailerId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the retailer id
        /// </summary>
        public string RetailerId
        {
            get { return retailerId; }
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
