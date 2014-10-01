// *********************************************** 
// NAME             : OperationalEventConsoleFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Formats operational events for publishing by console
// ************************************************       
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Formats operational events for publishing by console.
    /// </summary>
    public class OperationalEventConsoleFormatter : IEventFormatter
    {
        #region Private Static Fields

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static OperationalEventConsoleFormatter instance;

        /// <summary>
        /// Lock to allow thread safe interaction with singleton instance.
        /// </summary>
        private static readonly object instanceLock = new object();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Property to allow singleton instance to be obtained. Thread safe.
        /// </summary>
        public static OperationalEventConsoleFormatter Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OperationalEventConsoleFormatter();
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
        private OperationalEventConsoleFormatter()
        { }

        #endregion

        #region Public Methods
        /// <summary>
        /// Formats the given log event.
        /// </summary>
        /// <param name="logEvent">Log event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string header = "SJP-OP";
            string output = String.Empty;

            if (logEvent is OperationalEvent)
            {
                OperationalEvent operationalEvent = (OperationalEvent)logEvent;

                output =
                    header + " " +
                    operationalEvent.Time + " " +
                    operationalEvent.Message + " " +
                    operationalEvent.Category + " " +
                    operationalEvent.Level + " " +
                    operationalEvent.MachineName + " " +
                    operationalEvent.TypeName + " " +
                    operationalEvent.MethodName + " " +
                    operationalEvent.AssemblyName;

                if (operationalEvent.Target != null)
                {
                    output += " " + operationalEvent.Target.ToString();
                }

                if (operationalEvent.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output += " " + operationalEvent.SessionId;
                }
            }

            return output;
        }
        #endregion
    }
}
