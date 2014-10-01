// *********************************************** 
// NAME             : OperationalEventEmailFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Formats operational events for publishing by email.
// ************************************************
                
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Formats operational events for publishing by email.
    /// </summary>
    public class OperationalEventEmailFormatter : IEventFormatter
    {
        
        #region Private Static Fields

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static OperationalEventEmailFormatter instance;

        /// <summary>
        /// Lock to allow thread safe interaction with singleton instance.
        /// </summary>
        private static readonly object instanceLock = new object();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Property to allow singleton instance to be obtained. Thread safe.
        /// </summary>
        public static OperationalEventEmailFormatter Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OperationalEventEmailFormatter();
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
        private OperationalEventEmailFormatter()
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
                OperationalEvent oe = (OperationalEvent)logEvent;

                output =
                    "SJP Operational Event\n\n" +
                    "Time: " + oe.Time + "\n" +
                    "Message: " + oe.Message + "\n" +
                    "Category: " + oe.Category + "\n" +
                    "Level: " + oe.Level + "\n";

                output += "Machine Name: ";
                if (oe.MachineName.Length > 0)
                {
                    output += (oe.MachineName + "\n");
                }
                else
                {
                    output += ("N/A\n");
                }

                output += "Type Name: ";
                if (oe.TypeName.Length > 0)
                {
                    output += (oe.TypeName + "\n");
                }
                else
                {
                    output += ("N/A\n");
                }

                output += "Method Name: ";
                if (oe.MethodName.Length > 0)
                {
                    output += (oe.MethodName + "\n");
                }
                else
                {
                    output += ("N/A\n");
                }

                output += "Assembly Name: ";
                if (oe.AssemblyName.Length > 0)
                {
                    output += (oe.AssemblyName + "\n");
                }
                else
                {
                    output += ("N/A\n");
                }

                output += "Target: ";
                if (oe.Target != null)
                {
                    output += (oe.Target.ToString() + "\n");
                }
                else
                {
                    output += ("N/A\n");
                }

                output += "Session: ";
                if (oe.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    output += oe.SessionId;
                }
                else
                {
                    output += ("N/A\n");
                }
            }

            return output;
        }
        #endregion
    }
}
