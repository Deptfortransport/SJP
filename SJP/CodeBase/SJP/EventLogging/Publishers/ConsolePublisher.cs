// *********************************************** 
// NAME             : ConsolePublisher.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Publishes events to the console
// ************************************************
                
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Publishes events to the console.
    /// </summary>
    public class ConsolePublisher : IEventPublisher
    {
        #region Private Fields
        private string identifier;
        private string streamSetting; 

        #endregion

        #region Constructors

        /// <summary>
        /// Create a publisher that sends event details to the console.
        /// It is assumed that all parameters have been pre-validated.
        /// </summary>
        /// <param name="identifier">Unique identifier for console publishers.</param>
        /// <param name="streamSetting">Takes values 'Error' or 'Out' to indicate type of console stream to publish to.</param>
        public ConsolePublisher(string identifier, string streamSetting)
        {
            this.identifier = identifier;
            this.streamSetting = streamSetting;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the identifier of the ConsolePublisher.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Writes a log event to the console.
        /// </summary>
        /// <param name="logEvent">Log Event to write details for.</param>
        public void WriteEvent(LogEvent logEvent)
        {
            try
            {
                string formatString = logEvent.ConsoleFormatter.AsString(logEvent);

                if (streamSetting == Keys.ConsolePublisherOutputStream)
                {
                    Console.Out.WriteLine(formatString);
                }
                else if (streamSetting == Keys.ConsolePublisherErrorStream)
                {
                    Console.Error.WriteLine(formatString);
                }
            }
            catch (System.IO.IOException ioe)
            {
                String message = Messages.ConsolePublisherWriteEvent;
                throw new SJPException(message, ioe, false, SJPExceptionIdentifier.ELSConsolePublisherWritingEvent);
            }
            catch (Exception e)
            {
                String message = Messages.ConsolePublisherWriteEvent;
                throw new SJPException(message, e, false, SJPExceptionIdentifier.ELSConsolePublisherWritingEvent);
            }

        }
        #endregion
    }
}
