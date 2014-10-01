// *********************************************** 
// NAME             : JourneyPlanResultsEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: File formatter for the JourneyPlanResults event
// ************************************************
// 

using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// File formatter for the JourneyPlanResults event
    /// </summary>
    public class JourneyPlanResultsEventFileFormatter : IEventFormatter
    {
        #region Private members

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public JourneyPlanResultsEventFileFormatter()
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

            if (logEvent is JourneyPlanResultsEvent)
            {
                JourneyPlanResultsEvent jpre = (JourneyPlanResultsEvent)logEvent;

                string tab = "\t";

                output.Append("SJP-JPResE" + tab);
                output.Append(jpre.Time.ToString(dateTimeFormat) + tab);
                output.Append(jpre.JourneyPlanRequestId + tab);
                output.Append(jpre.ResponseCategory + tab);
                output.Append("LoggedOn: ");
                output.Append(jpre.UserLoggedOn);

                if (jpre.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output.Append(tab);
                    output.Append("SessionId: " + tab);
                    output.Append(jpre.SessionId);
                }
            }
            return output.ToString();
        }

        #endregion
    }
}
