// *********************************************** 
// NAME             : CyclePlannerRequestEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: Class which defines a custom event for logging a Cycle planner request
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Class which defines a custom event for logging a Cycle planner request
    /// </summary>
    [Serializable()]
    public class CyclePlannerRequestEvent : SJPCustomEvent
    {
        #region Private members

        private string cyclePlannerRequestId;

        private static CyclePlannerRequestEventFileFormatter fileFormatter = new CyclePlannerRequestEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sessionId">The session id used to perform the cycle planner request.</param>
        /// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or not (false).</param>
        /// <param name="cyclePlannerRequestId">Identifier used to identify the cycle planner request.</param>
        public CyclePlannerRequestEvent(string cyclePlannerRequestId,
                                       bool userLoggedOn,
                                       string sessionId)
            : base(sessionId, userLoggedOn)
        {
            this.cyclePlannerRequestId = cyclePlannerRequestId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the cycle planner request identifier.
        /// </summary>
        public string CyclePlannerRequestId
        {
            get { return cyclePlannerRequestId; }
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
