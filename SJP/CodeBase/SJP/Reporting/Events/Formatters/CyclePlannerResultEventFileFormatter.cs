﻿// *********************************************** 
// NAME             : CyclePlannerResultEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: Class which defines a custom event for logging a Cycle planner result to a file
// ************************************************
// 

using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// Class which defines a custom event for logging a Cycle planner result to a file
    /// </summary>
    public class CyclePlannerResultEventFileFormatter : IEventFormatter
    {
        #region Private members

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CyclePlannerResultEventFileFormatter()
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

            if (logEvent is CyclePlannerResultEvent)
            {
                CyclePlannerResultEvent cpre = (CyclePlannerResultEvent)logEvent;

                output.Append("SJP-CPResE\t");
                output.Append(cpre.Time.ToString(dateTimeFormat) + "\t");
                output.Append("RequestID[" + cpre.CyclePlannerRequestId + "]\t");
                output.Append("ResponseCat[" + cpre.ResponseCategory + "]\t");
                output.Append("LoggedOn[" + cpre.UserLoggedOn + "]");

                if (cpre.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output.Append("\tSessionID[" + cpre.SessionId + "]");
                }
            }
            return output.ToString();
        }

        #endregion
    }
}
