// *********************************************** 
// NAME             : ReceivedOperationalEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: A formatter that formats
// "received" operational events for file publishing.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// Formats "received" operational events for publishing by file.
    /// </summary>
    public class ReceivedOperationalEventFileFormatter : IEventFormatter
    {
        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ReceivedOperationalEventFileFormatter()
        {
        }

        /// <summary>
        /// Formats the given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is ReceivedOperationalEvent)
            {
                ReceivedOperationalEvent roe = (ReceivedOperationalEvent)logEvent;

                output =
                    "SJP-ROE\t" +
                    roe.WrappedOperationalEvent.Time.ToString(dateTimeFormat) + "\t" +
                    roe.WrappedOperationalEvent.Message + "\t" +
                    roe.WrappedOperationalEvent.Category + "\t" +
                    roe.WrappedOperationalEvent.Level + "\t" +
                    roe.WrappedOperationalEvent.MachineName + "\t" +
                    roe.WrappedOperationalEvent.TypeName + "\t" +
                    roe.WrappedOperationalEvent.MethodName + "\t" +
                    roe.WrappedOperationalEvent.AssemblyName;

                if (roe.WrappedOperationalEvent.Target != null)
                {
                    output += ("\t" + roe.WrappedOperationalEvent.Target.ToString());
                }

                if (roe.WrappedOperationalEvent.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output += ("\t" + roe.WrappedOperationalEvent.SessionId);
                }
            }
            return output;
        }
    }
}
