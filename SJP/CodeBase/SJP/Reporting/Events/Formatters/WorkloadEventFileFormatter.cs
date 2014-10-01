// *********************************************** 
// NAME             : WorkloadEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: File formatter for the WorkloadEvent
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// File formatter for the WorkloadEvent
    /// </summary>
    public class WorkloadEventFileFormatter : IEventFormatter
    {
        #region Private members

        // Provide formats that will allow easy import into database if necessary.
		private const string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff000000";
		private const string RecordFormat = "WorkloadEvent,'{0}','{1}'";

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public WorkloadEventFileFormatter()
        {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Formats tht given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is WorkloadEvent)
            {
                WorkloadEvent we = (WorkloadEvent)logEvent;

                output = String.Format(RecordFormat,
                                       we.Requested.ToString(TimeFormat),
                                       we.NumberRequested.ToString());
            }
            return output;
        }

        #endregion
    }
}
