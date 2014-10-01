// *********************************************** 
// NAME             : StopEventRequestEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: File formatter for the StopEventRequestEvent
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// Summary description for StopEventRequestEventFileFormatter.
    /// </summary>
    public class StopEventRequestEventFileFormatter : IEventFormatter
    {
        #region Private members

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public StopEventRequestEventFileFormatter()
        {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Formats the given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is StopEventRequestEvent)
            {
                StopEventRequestEvent e = (StopEventRequestEvent)logEvent;

                output = String.Format(Messages.StopEventRequestEventFileFormat,
                    e.Submitted.ToString(dateTimeFormat),
                    e.Time.ToString(dateTimeFormat),
                    e.RequestId,
                    e.RequestType.ToString(),
                    e.Success.ToString());

            }
            return output;
        }

        #endregion
    }
}
