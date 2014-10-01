// *********************************************** 
// NAME             : JourneyPlanResultsEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging journey plan results data.
// ************************************************
// 

using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging journey plan results data.
    /// </summary>
    [Serializable()]
    public class JourneyPlanResultsEvent : SJPCustomEvent
    {
        #region Private members

        private string journeyPlanRequestId;
        private JourneyPlanResponseCategory responseCategory;

        private static JourneyPlanResultsEventFileFormatter fileFormatter = new JourneyPlanResultsEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a <c>JourneyPlanResultsEvent</c> class. 
        /// A <c>JourneyPlanResultsEvent</c> is used
        /// to log journey results transaction data using the Event Service.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <param name="sessionId">The session id used to perform the journey request that produced the results.</param>
        /// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or not (false).</param>
        /// <param name="journeyPlanRequestId">Identifier used to identify the journey request that produced the results.</param>
        /// <param name="resultsCategory">The category of response returned by the journey request.</param>
        public JourneyPlanResultsEvent(string journeyPlanRequestId,
                                       JourneyPlanResponseCategory responseCategory,
                                       bool userLoggedOn,
                                       string sessionId)
            : base(sessionId, userLoggedOn)
        {
            this.responseCategory = responseCategory;
            this.journeyPlanRequestId = journeyPlanRequestId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the journey plan request identifier.
        /// </summary>
        public string JourneyPlanRequestId
        {
            get { return journeyPlanRequestId; }
        }

        /// <summary>
        /// Gets the journey response category.
        /// </summary>
        public JourneyPlanResponseCategory ResponseCategory
        {
            get { return responseCategory; }
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
