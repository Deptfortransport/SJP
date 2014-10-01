// *********************************************** 
// NAME             : RetailerHandoffEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: File formatter for the RetailerHandoffEvent
// ************************************************
// 

using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// File formatter for the RetailerHandoffEvent
    /// </summary>
    public class RetailerHandoffEventFileFormatter : IEventFormatter
    {
        #region Private members

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public RetailerHandoffEventFileFormatter()
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

            if (logEvent is RetailerHandoffEvent)
            {
                RetailerHandoffEvent rhe = (RetailerHandoffEvent)logEvent;

                string tab = "\t";

                output.Append("SJP-RHE" + tab);
                output.Append(rhe.Time.ToString(dateTimeFormat) + tab);
                output.Append("RetailerId: ");
                output.Append(rhe.RetailerId + tab);
                output.Append("LoggedOn: ");
                output.Append(rhe.UserLoggedOn.ToString());
                
                if (rhe.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output.Append(tab);
                    output.Append(rhe.SessionId);
                }
            }
            return output.ToString();
        }

        #endregion
    }
}
