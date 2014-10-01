// *********************************************** 
// NAME             : OperationalEventEventLogFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Formats operational events for publishing by Event Log
// ************************************************ 
                
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Formats operational events for publishing by Event Log.
    /// </summary>
    public class OperationalEventEventLogFormatter : IEventFormatter
    {
        #region Private Static Fields

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static OperationalEventEventLogFormatter instance;

        /// <summary>
        /// Lock to allow thread safe interaction with singleton instance.
        /// </summary>
        private static readonly object instanceLock = new object();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Property to allow singleton instance to be obtained. Thread safe.
        /// </summary>
        public static OperationalEventEventLogFormatter Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OperationalEventEventLogFormatter();
                    }

                    return instance;
                }
            }
        }

        #endregion

        #region Private Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        private OperationalEventEventLogFormatter()
        { }

        #endregion

        #region Public Methods
        /// <summary>
        /// Formats the given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is OperationalEvent)
            {
                OperationalEvent operationalEvent = (OperationalEvent)logEvent;

                // NOTE: first string MUST be category for TNG to parse. TNG does not parse any of the remaining text.
                output = operationalEvent.Category + Environment.NewLine +
                         "SJP OPERATIONAL EVENT" + Environment.NewLine +
                         "Time: " + operationalEvent.Time.ToString("yyyy-MM-ddTHH:mm:ss.fff") + Environment.NewLine +
                         "Category: " + operationalEvent.Category + Environment.NewLine +
                         "Level: " + operationalEvent.Level + Environment.NewLine +
                         "Message: " + operationalEvent.Message + Environment.NewLine +
                         "Machine: " + operationalEvent.MachineName + Environment.NewLine +
                         "Class logged: " + operationalEvent.TypeName + Environment.NewLine +
                         "Method logged: " + operationalEvent.MethodName + Environment.NewLine +
                         "Assembly logged: " + operationalEvent.AssemblyName + Environment.NewLine;

                if (operationalEvent.Target != null)
                {
                    output += "Target: " + operationalEvent.Target.ToString() + Environment.NewLine;
                }

                if (operationalEvent.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output += "Session Id: " + operationalEvent.SessionId + Environment.NewLine;
                }
            }

            return output;
        }

        #endregion
    }
}
