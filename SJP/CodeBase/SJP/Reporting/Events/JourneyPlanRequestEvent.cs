// *********************************************** 
// NAME             : JourneyPlanRequestEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging journey plan request data.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;
using SJP.Common;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging journey plan request data.
    /// </summary>
    [Serializable()]
    public class JourneyPlanRequestEvent : SJPCustomEvent
    {
        #region Private members

        private string journeyPlanRequestId;
        private List<SJPModeType> modes;

        private static JourneyPlanRequestEventFileFormatter fileFormatter = new JourneyPlanRequestEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a <c>JourneyPlanRequestEvent</c> class. 
        /// A <c>JourneyPlanRequestEvent</c> is used
        /// to log journey request transaction data using the Event Service.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <param name="sessionId">The session id used to perform the journey request.</param>
        /// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or not (false).</param>
        /// <param name="journeyPlanRequestId">Identifier used to identify the journey request.</param>
        /// <param name="modes">The modes of transport specified in the journey plan request.</param>
        public JourneyPlanRequestEvent(string journeyPlanRequestId,
                                       List<SJPModeType> modes,
                                       bool userLoggedOn,
                                       string sessionId)
            : base(sessionId, userLoggedOn)
        {
            this.modes = modes;
            this.journeyPlanRequestId = journeyPlanRequestId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the journey request modes data.
        /// </summary>
        public List<SJPModeType> Modes
        {
            get { return modes; }
        }

        /// <summary>
        /// Gets the journey plan request identifier.
        /// </summary>
        public string JourneyPlanRequestId
        {
            get { return journeyPlanRequestId; }
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
