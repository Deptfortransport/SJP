// *********************************************** 
// NAME             : IEventFilter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Interface for an event filter
// ************************************************

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Interface for an event filter.
    /// </summary>
    public interface IEventFilter
    {
        /// <summary>
        /// Used to determine whether an event should be published.
        /// </summary>
        /// <param name="logEvent">LogEvent to be published.</param>
        /// <returns><c>true</c> if <c>LogEvent</c> should be published.</returns>
        bool ShouldLog(LogEvent logEvent);

    }
}
