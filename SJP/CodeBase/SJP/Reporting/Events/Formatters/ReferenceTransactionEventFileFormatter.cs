// *********************************************** 
// NAME             : ReferenceTransactionEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Formats ReferenceTransactionEvent event data for file publisher.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// Formats ReferenceTransactionEvent event data for file publisher
    /// </summary>
    public class ReferenceTransactionEventFileFormatter : IEventFormatter
    {
         #region Private members

        // Provide formats that will allow easy import into database if necessary.
		private const string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff000000";
        private const string RecordFormat = "1,'{0}','{1}','{2}','{3}','{4}','{5}','{6}'";

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public ReferenceTransactionEventFileFormatter()
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

            if (logEvent is ReferenceTransactionEvent)
            {
                ReferenceTransactionEvent rte = (ReferenceTransactionEvent)logEvent;

                output = String.Format(RecordFormat,
                                       rte.EventType,
                                       rte.ServiceLevelAgreement.ToString(),
                                       rte.Submitted.ToString(TimeFormat),
                                       rte.SessionId,
                                       rte.Time.ToString(TimeFormat),
                                       rte.Successful.ToString(),
                                       rte.MachineName);
            }

            return output;
        }

        #endregion
    }
}
