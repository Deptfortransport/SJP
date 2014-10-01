// *********************************************** 
// NAME             : QueuePublisher.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Publishes events to a queue
// ************************************************ 
                
                
using System;
using System.Messaging;

namespace SJP.Common.EventLogging
{
    /// <summary>
	/// Publishes events to a queue.
	/// </summary>
	public sealed class QueuePublisher : IEventPublisher, IDisposable
    {
        #region Private Fields
        private MessageQueue queue;
		private string identifier;
        #endregion

        #region Constructors
        #endregion

        #region Public Properties

        /// <summary>
		/// Gets the identifier.
		/// </summary>
		public string Identifier
		{
			get {return identifier;}
		}

        #endregion

        #region Public Methods
        /// <summary>
		/// Create a publisher that sends event details to a message queue.
		/// It is assumed that all parameters have been pre-validated.
		/// </summary>
		/// <param name="identifier">Identifier of publisher.</param>
		/// <param name="priority">Priority of messages published.</param>
		/// <param name="path">Path to the queue.</param>
		/// <param name="recoverable"><c>true</c> if messages should be recoverable otherwise <c>false</c>.</param>
		public QueuePublisher(string identifier, MessagePriority priority, string path, bool recoverable)
		{
			
			this.identifier = identifier;
			this.queue = new MessageQueue(path);

			this.queue.DefaultPropertiesToSend.Recoverable = recoverable;
			this.queue.DefaultPropertiesToSend.Priority = priority;
			this.queue.Formatter = new BinaryMessageFormatter();
			
		}
		
		/// <summary>
		/// Sends the given log event to the queue.
		/// </summary>
		/// <param name="logEvent"><c>LogEvent</c> to publish.</param>
		/// <exception cref="SJP.Common.SJPException">Log Event was not successfully written to the queue.</exception>
		public void WriteEvent(LogEvent logEvent)
		{
			try
			{
				queue.Send(logEvent, logEvent.ClassName);
			}
			catch(MessageQueueException mqe)
			{
				// thrown by Send if the path property has not been
				// set or an error occured when accessing a
				// Message Queuing API.
				string message = String.Format(Messages.QueuePublisherWriteEvent, "QueuePath:" + queue.Path);

				throw new SJPException(message, mqe, false, SJPExceptionIdentifier.ELSQueuePublisherWritingEvent);
			}
			catch(Exception e)
			{
				string message = String.Format(Messages.QueuePublisherWriteEvent, "QueuePath:" + queue.Path);

				throw new SJPException(message, e, false, SJPExceptionIdentifier.ELSQueuePublisherWritingEvent);
			}
        }

        /// <summary>
        /// Overloaded dispose method to clean up resources
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Code to dispose the managed resources of the class
                queue.Dispose();
            }
            // Code to dispose the un-managed resources of the class

        }

        /// <summary>
        /// Dispose method to clean up resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

       
    }
}

