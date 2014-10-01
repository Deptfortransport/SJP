// *********************************************** 
// NAME             : DefaultFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: A class which formats log events for publishing
// ************************************************               
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// A class which formats log events for publishing.
    /// Used when a more specific formatter is not available
    /// for a given <c>LogEvent</c> type.
    /// </summary>
    public class DefaultFormatter : IEventFormatter
    {
        #region Private Static Fields

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static DefaultFormatter instance;
        /// <summary>
        /// Lock to allow thread safe interaction with singleton instance.
        /// </summary>
        private static readonly object instanceLock = new object();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Property to allow singleton instance to be obtained. Thread safe.
        /// </summary>
        public static DefaultFormatter Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new DefaultFormatter();
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
        private DefaultFormatter()
        { }
        #endregion

       
        #region Public Methods
        /// <summary>
        /// Formats log event data into a string. The data included in the
        /// format string will exist for all <c>LogEvent</c> types.
        /// </summary>
        /// <param name="logEvent">The <c>LogEvent</c> to format.</param>
        /// <returns>A formatted string containing event data common across all event types.</returns>
        public string AsString(LogEvent logEvent)
        {
            string output = String.Format(Messages.DefaultFormatterOutput, logEvent.Time, logEvent.ClassName);
            return output;
        }
        #endregion


    }

}
