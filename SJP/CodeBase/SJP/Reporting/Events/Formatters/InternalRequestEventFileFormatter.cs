// *********************************************** 
// NAME             : InternalRequestEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 June 2011
// DESCRIPTION  	: Defines a file formatter for formatting
// InternalRequestEvent events using the core Event Service File Publisher.
// Typically this file formatter will not be used in production, but
// instead InternalRequestEvent events will be published to database via MSMQ
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
    /// Formats JourneyWebRequest events for publishing by a file publisher.
    /// </summary>
    public class InternalRequestEventFileFormatter : IEventFormatter
    {

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InternalRequestEventFileFormatter()
        { }

        /// <summary>
        /// Formats the given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is InternalRequestEvent)
            {
                InternalRequestEvent e = (InternalRequestEvent)logEvent;

                output = String.Format(Messages.InternalRequestEventFileFormat,
                    e.Submitted.ToString(dateTimeFormat),
                    e.Time.ToString(dateTimeFormat),
                    e.SessionId,
                    e.InternalRequestId,
                    e.RefTransaction,
                    e.RequestType.ToString(),
                    e.FunctionType,
                    e.Success.ToString());
            }
            return output;
        }
    }
}
