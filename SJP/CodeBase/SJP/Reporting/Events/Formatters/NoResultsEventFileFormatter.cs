//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SJP.Reporting.Events.Formatters
//{
//    class NoResultsEventFileFormatter
//    {
//    }
//}
// *********************************************** 
// NAME             : NoResultsEventFileFormatter.cs      
// AUTHOR           : Phil Scott
// DATE CREATED     : 15 Mar 2012
// DESCRIPTION      : File formatter for the NoResultsEvent
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
    public class NoResultsEventFileFormatter : IEventFormatter
    {

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NoResultsEventFileFormatter()
        { }

        /// <summary>
        /// Formats the given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is NoResultsEvent)
            {
                NoResultsEvent e = (NoResultsEvent)logEvent;

                output = String.Format(Messages.NoResultsEventFileFormat,
                    e.Submitted.ToString(dateTimeFormat),
                    e.Time.ToString(dateTimeFormat),
                    e.SessionId);
            }
            return output;
        }
    }
}
