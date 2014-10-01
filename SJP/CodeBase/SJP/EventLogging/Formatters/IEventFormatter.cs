// *********************************************** 
// NAME             : IEventFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Interface for an Event Formatter.
//                    Concrete formatter classes should implement this inteface to 
//                    provide event publishers with a mechanism of retrieving relevant data 
//                    stored with an event for publishing. 
// ************************************************

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Interface for an Event Formatter.
    /// Concrete formatter classes should implement this inteface to 
    /// provide event publishers with a mechanism of retrieving relevant data 
    /// stored with an event for publishing. 
    /// </summary>
    public interface IEventFormatter
    {
        /// <summary>
        /// Returns relevant data that is stored in the <c>LogEvent</c> passed, 
        /// as a formatted string ready for publishing to a data sink.
        /// </summary>
        /// <param name="logEvent"><c>LogEvent</c> to whose data is to be formatted.</param>
        /// <returns>The event data formatted as a string.</returns>
        string AsString(LogEvent logEvent);
                

    }
}
