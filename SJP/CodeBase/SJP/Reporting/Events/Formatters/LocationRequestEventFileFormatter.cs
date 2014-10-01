// *********************************************** 
// NAME             : LocationRequestEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 June 2011
// DESCRIPTION  	: efines a file formatter for formatting
// LocationRequest events using the core Event Service File Publisher.
// Typically this file formatter will not be used in production, but
// instead LocationRequest events will be published to database via MSMQ
// This file formatter may be useful during development for debugging.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Formats LocationRequest events for publishing by a file publisher.
    /// </summary>
    public class LocationRequestEventFileFormatter : IEventFormatter
    {

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocationRequestEventFileFormatter()
        { }

        /// <summary>
        /// Formats the given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is LocationRequestEvent)
            {
                LocationRequestEvent e = (LocationRequestEvent)logEvent;

                output = String.Format(Messages.LocationRequestEventFileFormat,
                                       e.Time.ToString(dateTimeFormat),
                                       e.JourneyPlanRequestId,
                                       e.AdminAreaCode,
                                       e.RegionCode,
                                       e.PrepositionCategory.ToString());
            }
            return output;
        }
    }
}