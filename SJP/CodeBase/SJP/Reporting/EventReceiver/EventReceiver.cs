// *********************************************** 
// NAME             : EventReceiver.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: EventReceiver class builds a list of MSMQs 
// and then sets up an event handler to receive 
// messages received by those queues.
// Logs the messages to relevant logger as specified
// in the properties
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;
using System.Threading;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Reporting.Events;
using TDPEL = TransportDirect.Common.Logging;
using TDPCJPE = TransportDirect.ReportDataProvider.CJPCustomEvents;

namespace SJP.Reporting.EventReceiver
{
    /// <summary>
    /// Monitors queues it is responsible for and 
    /// logs them using the event logging service
    /// </summary>
    public class EventReceiver : IDisposable
    {
        #region Private members

        private List<MessageQueue> messageQueueList;
        private ReceiveCompletedEventHandler receiveEventHandler;
        private bool recover = false;
        private bool disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public EventReceiver()
        {
            receiveEventHandler = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts message queue polling.
        /// </summary>
        public void Run()
        {
            this.Run(Properties.Current);
        }

        /// <summary>
        /// Starts message queue polling.
        /// This prototype is used for unit testing to allow mock properties to be passed in.
        /// </summary>
        /// <param name="pp">Properties.</param>
        public void Run(IPropertyProvider pp)
        {
            InitQueueList(pp);
            SetupQueues();	// has effect of starting queue polling				
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initialise the internal list of queues to support.
        /// The list of queues is obtained from the properties.
        /// </summary>
        /// <param name="pp">Properties used to initialise queues.</param>
        private void InitQueueList(IPropertyProvider properties)
        {
            messageQueueList = new List<MessageQueue>();

            string[] idList = new string[0];

            if (!string.IsNullOrEmpty(properties[Keys.ReceiverQueue]))
            {
                idList = properties[Keys.ReceiverQueue].Split(' ');
            }

            string queue = null;

            foreach (string id in idList)
            {
                if (id != " ")
                {
                    queue = properties[string.Format(Keys.ReceiverQueuePath, id)];

                    messageQueueList.Add(new MessageQueue(queue));
                }
            }

            if (SJPTraceSwitch.TraceVerbose)
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, String.Format(Messages.Service_EstablishedQueues, messageQueueList.Count)));

        }

        /// <summary>
        /// Setup the queues in our list to receive messages.
        /// </summary>
        /// <exception cref="TDException">
        /// Failure to set up a queue.
        /// </exception> 
        private void SetupQueues()
        {
            if (SJPTraceSwitch.TraceVerbose)
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, Messages.Service_SettingQueueHandlers));

            int numQueues = messageQueueList.Count;

            receiveEventHandler = new ReceiveCompletedEventHandler(Receive);

            try
            {
                foreach (TraceListener listener in Trace.Listeners)
                {
                    if (listener is SJPTraceListener)
                        ((SJPTraceListener)listener).DefaultPublisherCalled += new DefaultPublisherCalledEventHandler(OnDefaultPublisherCalled);

                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("My event", string.Format("Unable to subscribe to the event. Reason:[{0}]", e.Message), EventLogEntryType.Error);
            }
            for (int i = 0; i < numQueues; i++)
            {
                try
                {
                    messageQueueList[i].Formatter = new BinaryMessageFormatter();
                    messageQueueList[i].ReceiveCompleted += receiveEventHandler;
                    messageQueueList[i].BeginReceive();

                    // the recover variable is set to true 
                    // if the first Set up succeeds
                    recover = true;
                }
                catch (MessageQueueException messageQueueException)
                {
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, 
                        string.Format(Messages.Service_FailedSettingHandler, messageQueueException.Message) + 
                        string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                    // if first setup fails recover still set to false
                    // we don't want to recover
                    if (!recover)
                        throw new SJPException(String.Format(Messages.Service_FailedSettingHandler, messageQueueException.Message), messageQueueException, true, SJPExceptionIdentifier.RDPEventReceiverQueueReceiveInitFailed);
                    else
                        RecoverFromRemoteException();
                }
            }
        }

        /// <summary>
        /// DefaultPublisherCalledEventHandler event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDefaultPublisherCalled(object sender, DefaultPublisherCalledEventArgs e)
        {
            StopMessageQueues();

            LogEvent le = e.LogEvent;
            Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, string.Format(Messages.Service_FailureWhenPublishingEvent, le.ClassName)));
        }

        /// <summary>
        /// Closes and releases all queues. Then Re Init the queues.
        /// </summary>
        private void RecoverFromRemoteException()
        {
            // First Stop MessageQueues
            StopMessageQueues();

            // Sleep before trying to re init again.
            // This is if network not available for a long time
            // so it does not loop too many times

            int sleep = Convert.ToInt32(Properties.Current[Keys.TimeBeforeRecovery]);
            if (sleep == 0)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, Messages.Service_FailedRetrievingProperty));
                throw new SJPException(Messages.Service_FailedRetrievingProperty, true, SJPExceptionIdentifier.PSMissingProperty);
            }
            Thread.Sleep(sleep);
            // Then Re Init all queues
            Run();
        }

        /// <summary>
        /// Releases the message quests
        /// </summary>
        private void StopMessageQueues()
        {
            // releases all queues

            // Unregister the eventhandler from all the queues in our list
            // and the remove from the list
            while (messageQueueList.Count > 0)
            {
                messageQueueList[0].ReceiveCompleted -= receiveEventHandler;
                messageQueueList[0].Close();
                messageQueueList.RemoveAt(0);

            }
            MessageQueue.ClearConnectionCache();
        }

        /// <summary>
        /// Global event handler used for all queues.
        /// </summary>
        /// <remarks>
        /// Any exceptions caught are NOT re-thrown since component is
        /// running as a service and exceptions will not reach client.
        /// Events that do not relate to exceptions MUST NOT be logged
        /// - since recursion may result if event logged to same queue
        /// as that being serviced.
        /// </remarks>
        /// <param name="source">Source queue.</param>
        /// <param name="asyncResult">Event data.</param>
        private void Receive(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)source;
            Message msg = null;

            try
            {
                msg = mq.EndReceive(asyncResult.AsyncResult);

                LogEvent le = null;

                // ensure that message body is of correct class type
                if (msg.Body is TDPEL.LogEvent)
                {
                    le = EventParser.ParseTDPCJPLogEvent((TDPEL.LogEvent)msg.Body);
                } 
                else
                {
                    le = (LogEvent)msg.Body;
                }

                if (le != null)
                {
                    // Publish message body using the configured publisher.
                    // Wrap Operational Events received to allow Operational Events 
                    // generated by Event Receiver to be configured differently to those received.
                    if (le is OperationalEvent)
                        Trace.Write(new ReceivedOperationalEvent((OperationalEvent)le));
                    else
                        Trace.Write(le);
                }


                // Note: Only poll queue for next message if the previous message was processed successfully.
                // This prevents more messages being potentially lost if the problem repeats itself.
                try
                {
                    mq.BeginReceive();
                }
                catch (MessageQueueException messageQueueException)
                {
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_FailedPollingQueue, messageQueueException.Message)));
                }


            }
            catch (ArgumentNullException argumentNullException)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_FailureReceivingMessage, argumentNullException.Message) + string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                RecoverFromRemoteException();
            }
            catch (ArgumentException argumentException)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_FailureReceivingMessage, argumentException.Message) + string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                RecoverFromRemoteException();
            }
            catch (MessageQueueException messageQueueException)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_FailureReceivingMessage, messageQueueException.Message) + string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                RecoverFromRemoteException();
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_FailedExtractingMessageBody, invalidOperationException.Message) + string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                RecoverFromRemoteException();
            }
            catch (InvalidCastException)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_UnknownEventReceived) + string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                RecoverFromRemoteException();
            }
            catch (SJPException sjpEx)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_EventPublishFail, sjpEx.Message) + string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                RecoverFromRemoteException();
            }
            catch (Exception exception)
            {
                // Catch undocumented exceptions.
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_FailureWhenProcessingMessage, exception.Message) + string.Format(Messages.Service_RecoveryMessage, Properties.Current[Keys.TimeBeforeRecovery])));
                RecoverFromRemoteException();
            }

        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes of pool resources.
        /// Allows clients to dispose of resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // take off finalization queue to prevent dispose being called again.
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        /// <summary>
        /// Class destructor.
        /// </summary>
        ~EventReceiver()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of resources. 
        /// Can be called by clients (via Dispose()) or runtime (via destructor).
        /// </summary>
        /// <param name="disposing">
        /// True when called by clients.
        /// False when called by runtime.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose of any managed resources:	
                    StopMessageQueues();

                }

                // Dispose of any unmanaged resources:

            }

            this.disposed = true;
        }
                
        #endregion
    }
}
