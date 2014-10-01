// *********************************************** 
// NAME             : OperationalEventFilter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Filter class used to determine whether an OperationalEvent should be logged.
// ************************************************            


namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Filter class used to determine whether an OperationalEvent should be logged.
    /// </summary>
    public class OperationalEventFilter : IEventFilter
    {
        private SJPTraceLevelOverride overrideLevel;

        /// <summary>
        /// Default constructor. Will set override level to SJPTracelLevelOverride.None
        /// </summary>
        public OperationalEventFilter()
            : this(SJPTraceLevelOverride.None)
        {
        }

        /// <summary>
        /// Constructor allowing an override level to be specified
        /// </summary>
        /// <param name="overrideLevel"></param>
        public OperationalEventFilter(SJPTraceLevelOverride overrideLevel)
        {
            this.overrideLevel = overrideLevel;
        }

        /// <summary>
        /// Checks if the event trace level is less than or equal to the specified trace level
        /// </summary>
        /// <param name="traceLevel">Trace level to compare againse</param>
        /// <param name="eventLevel">Trace level to compare</param>
        /// <returns>true if the event trace level is less than or equal to the specified trace level</returns>
        private static bool CheckLevel(SJPTraceLevel traceLevel, SJPTraceLevel eventLevel)
        {
            return (eventLevel <= traceLevel);
        }

        /// <summary>
        /// Determines whether the given <c>LogEvent</c> should be published.
        /// </summary>
        /// <param name="logEvent">The <c>LogEvent</c> to test for.</param>
        /// <returns>Returns <c>true</c> if the <c>LogEvent</c> should be logged, otherwise <c>false</c>. Always returns <c>false</c> if the log event passed is not an <c>OperationalEvent</c>.</returns>
        public bool ShouldLog(LogEvent logEvent)
        {
            if (logEvent is OperationalEvent)
            {
                switch (overrideLevel)
                {
                    case SJPTraceLevelOverride.User:
                        return true;
                    default:
                        OperationalEvent oe = (OperationalEvent)logEvent;

                        if (SJPTraceSwitch.TraceVerbose)
                            return (CheckLevel(SJPTraceLevel.Verbose, oe.Level));
                        else if (SJPTraceSwitch.TraceInfo)
                            return (CheckLevel(SJPTraceLevel.Info, oe.Level));
                        else if (SJPTraceSwitch.TraceWarning)
                            return (CheckLevel(SJPTraceLevel.Warning, oe.Level));
                        else if (SJPTraceSwitch.TraceError)
                            return (CheckLevel(SJPTraceLevel.Error, oe.Level));
                        else
                            return false; // signifies all levels are off
                }
            }

            return false;
        }
    }
}
