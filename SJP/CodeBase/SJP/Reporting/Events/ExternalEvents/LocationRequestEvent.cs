// *********************************************** 
// NAME             : LocationRequestEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 June 2011
// DESCRIPTION  	: Defines a custom event class
// for capturing Location Request event data in
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
    /// Defines the LocationRequestEvent class used for capturing
    /// Location Request event data within the CJP.
    /// </summary>
    [Serializable]
    public class LocationRequestEvent : CustomEvent
    {
        private string journeyPlanRequestId;
        private string adminAreaCode;
        private string regionCode;
        private JourneyPrepositionCategory prepositionCategory;

        /// <summary>
        /// Defines the formatter class that formats event data for use by a file publisher.
        /// Used by all instances of the LocationRequestEvent class.
        /// </summary>
        private static IEventFormatter fileFormatter = new LocationRequestEventFileFormatter();

        #region Constructor

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <remarks>
        /// The constructor does NOT perfrom any validation on arguments.
        /// </remarks>
        /// <param name="journeyPlanRequestId">Identifier to uniquely identify this request.</param>
        /// <param name="prepositionCategory">The category of preposition for this request.</param>
        /// <param name="adminAreaCode">The administrative area code relating to the location request.</param>
        /// <param name="regionCode">The region code relating to the location request.</param>
        public LocationRequestEvent(string journeyPlanRequestId,
                                    JourneyPrepositionCategory prepositionCategory,
                                    string adminAreaCode,
                                    string regionCode)
            : base()
        {
            this.journeyPlanRequestId = journeyPlanRequestId;
            this.prepositionCategory = prepositionCategory;
            this.adminAreaCode = adminAreaCode;
            this.regionCode = regionCode;
        }

        #endregion

        /// <summary>
        /// Gets the journey plan request identifier.
        /// </summary>
        public string JourneyPlanRequestId
        {
            get { return journeyPlanRequestId; }
        }

        /// <summary>
        /// Gets the administrative area code.
        /// </summary>
        public string AdminAreaCode
        {
            get { return adminAreaCode; }
        }

        /// <summary>
        /// Gets the region code.
        /// </summary>
        public string RegionCode
        {
            get { return regionCode; }
        }

        /// <summary>
        /// Gets the request preposition category.
        /// </summary>
        public JourneyPrepositionCategory PrepositionCategory
        {
            get { return prepositionCategory; }
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
