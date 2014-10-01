// *********************************************** 
// NAME             : CustomEventFilter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Class used to determine whether CustomEvents should be published
// ************************************************


namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Class used to determine whether CustomEvents should be published.
    /// </summary>
    public class CustomEventFilter : IEventFilter
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomEventFilter()
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether the given <c>LogEvent</c> should be published.
        /// </summary>
        /// <param name="logEvent">The <c>LogEvent</c> to test for.</param>
        /// <returns>Returns <c>true</c> if the <c>LogEvent</c> should be logged, otherwise <c>false</c>. Always returns <c>false</c> if the log event passed is not a <c>CustomEvent</c>.</returns>
        public bool ShouldLog(LogEvent logEvent)
        {
            CustomEvent ce = (CustomEvent)logEvent;
            
            if (CustomEventSwitch.On(ce.ClassName))
                return true;
            else
                return false;
        }
        #endregion
    }
}
