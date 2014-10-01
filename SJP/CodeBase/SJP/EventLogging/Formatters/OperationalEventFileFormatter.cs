// *********************************************** 
// NAME             : OperationalEventFileFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Formats operational events for publishing by file.
// ************************************************
                
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Formats operational events for publishing by file.
    /// </summary>
    public class OperationalEventFileFormatter : IEventFormatter
    {
        #region Private Fields
        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";
        #endregion

        #region Private Static Fields

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static OperationalEventFileFormatter instance;

        /// <summary>
        /// Lock to allow thread safe interaction with singleton instance.
        /// </summary>
        private static readonly object instanceLock = new object();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Property to allow singleton instance to be obtained. Thread safe.
        /// </summary>
        public static OperationalEventFileFormatter Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OperationalEventFileFormatter();
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
        private OperationalEventFileFormatter()
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Formats tht given log event.
        /// </summary>
        /// <param name="logEvent">Log Event to format.</param>
        /// <returns>A formatted string representing the log event.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Empty;

            if (logEvent is OperationalEvent)
            {
                OperationalEvent oe = (OperationalEvent)logEvent;

                output =
                    "SJP-OP\t" +
                    oe.Time.ToString(dateTimeFormat) + "\t" +
                    oe.Message + "\t" +
                    oe.Category + "\t" +
                    oe.Level + "\t" +
                    oe.MachineName + "\t" +
                    oe.TypeName + "\t" +
                    oe.MethodName + "\t" +
                    oe.AssemblyName;

                if (oe.Target != null)
                {
                    output += ("\t" + oe.Target.ToString());
                }

                if (oe.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output += ("\t" + oe.SessionId);
                }
            }
            return output;
        }
        #endregion
    }
}
