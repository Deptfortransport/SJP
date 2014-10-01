// *********************************************** 
// NAME             : RepeatVisitorEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: File formatter for the RepeatVisitorEvent
// ************************************************
// 

using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// File formatter for the RepeatVisitorEvent
    /// </summary>
    public class RepeatVisitorEventFileFormatter : IEventFormatter
    {
        #region Private members

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RepeatVisitorEventFileFormatter()
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
            StringBuilder output = new StringBuilder();

            if (logEvent is RepeatVisitorEvent)
            {
                RepeatVisitorEvent rve = (RepeatVisitorEvent)logEvent;

                string tab = "\t";

                output.Append("SJP-RVE" + tab);
                output.Append(rve.Time.ToString(dateTimeFormat) + tab);
                output.Append(rve.RepeatVisitorType.ToString() + tab);
                output.Append("ThemeId: ");
                output.Append(rve.ThemeId.ToString() + tab);
                output.Append("Domain: ");
                output.Append(rve.Domain + tab);
                output.Append("LastPageVisited: ");
                output.Append(rve.LastPageVisted + tab);
                output.Append("LastVisitedDateTime: ");
                output.Append(rve.LastVisitedDateTime.ToString(dateTimeFormat) + tab);
                output.Append("UserAgent: ");
                output.Append(rve.UserAgent + tab);
                output.Append("SessionIdOld: ");
                output.Append(rve.SessionIdOld);

                if (rve.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output.Append(tab);
                    output.Append("SessionIdNew: " + tab);
                    output.Append(rve.SessionId);
                }
            }
            return output.ToString();
        }

        #endregion
    }
}
