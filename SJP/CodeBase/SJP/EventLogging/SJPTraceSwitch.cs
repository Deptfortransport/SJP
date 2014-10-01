// *********************************************** 
// NAME             : SJPTraceSwitch.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Static class used to determine logging level for Operational Events
// ************************************************



namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Static class used to determine logging level for Operational Events.
    /// </summary>
    public class SJPTraceSwitch
    {
        #region Private Fields
        /// <summary>
        /// Indicates the level of logging applied for the whole application, e.g. error
        /// </summary>
        private static SJPTraceLevel currentLevel;
        #endregion

        #region Constructors
        /// <summary>
        /// Static constructor
        /// </summary>
        static SJPTraceSwitch()
        {
            currentLevel = SJPTraceLevel.Undefined;

            // Register listener for any changes in level and for initialisation of level
            SJPTraceListener.OperationalTraceLevelChange += new TraceLevelChangeEventHandler(TraceLevelChangeEventHandler);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Hanlder for OperationalTraceLevelChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="traceLevelEventArgs">TraceLevelEventArgs object</param>
        private static void TraceLevelChangeEventHandler(object sender, TraceLevelEventArgs traceLevelEventArgs)
        {
            currentLevel = traceLevelEventArgs.TraceLevel;
        }
        #endregion


        #region Private Methods
        /// <summary>
        /// Checks if the supplied trace level is less than or equal to current trace level
        /// </summary>
        /// <param name="traceLevel">SJP trace level</param>
        /// <returns>True if the supplied trace level is less than or equal to current level otherwise false</returns>
        private static bool CheckLevel(SJPTraceLevel traceLevel)
        {
            if (currentLevel == SJPTraceLevel.Undefined)
            {
                throw new SJPException(Messages.SJPTraceSwitchNotInitialised, false, SJPExceptionIdentifier.ELSTraceLevelUninitialised);
            }
            else
                return (traceLevel <= currentLevel);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets trace info indicator. <c>true</c> if informational events and above are being traced.
        /// </summary>
        public static bool TraceInfo
        {
            get { return CheckLevel(SJPTraceLevel.Info); }
        }

        /// <summary></summary>
        /// Gets trace error indicator. <c>true</c> if error events and above are being traced.
        /// </summary>
        public static bool TraceError
        {
            get { return CheckLevel(SJPTraceLevel.Error); }
        }

        /// <summary>
        /// Gets trace warning indicator. <c>true</c> if warning events and above are being traced.
        /// </summary>
        public static bool TraceWarning
        {
            get { return CheckLevel(SJPTraceLevel.Warning); }
        }

        /// <summary>
        /// Gets the verbose indicator. <c>true</c> if events at any level are being traced.
        /// </summary>
        public static bool TraceVerbose
        {
            get { return CheckLevel(SJPTraceLevel.Verbose); }
        }
        #endregion

    }
}
