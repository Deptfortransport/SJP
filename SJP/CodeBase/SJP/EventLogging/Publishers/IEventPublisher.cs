// *********************************************** 
// NAME             : IEventPublisher.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Interface for an Event Publisher
// ************************************************


namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Interface for an Event Publisher.
    /// </summary>
    public interface IEventPublisher
    {
        #region Properties
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        string Identifier { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Publishes a <c>LogEvent</c> to a sink.
        /// </summary>
        /// <param name="logEvent"><c>LogEvent</c> to publish.</param>
        /// <exception cref="SJP.Common.SJPException"><c>LogEvent</c> was not successfully written to the sink.</exception>
        void WriteEvent(LogEvent logEvent);
        #endregion
    }
}
