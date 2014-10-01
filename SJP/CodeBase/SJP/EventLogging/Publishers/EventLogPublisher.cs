// *********************************************** 
// NAME             : EventLogPublisher.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Publishes events to the Event Log
// ************************************************
        
using System;
using System.Diagnostics;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Publishes events to the Event Log.
    /// </summary>
    public sealed class EventLogPublisher : IEventPublisher, IDisposable
    {
        #region Private Fields
        private string identifier;
        private EventLog eventLog;
        private const int OperationalEventId = 1;
        private const int CustomEventId = 2;
        
        /// <summary>
        /// Lock to allow thread safe interaction.
        /// </summary>
        private readonly object instanceLock = new object();
        #endregion

        #region Constructors
        #endregion 

        #region Public Properties
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a publisher that writes event details to the Event Log.
        /// It is assumed that all parameters have been pre-validated.
        /// This constructor will additionally create an Event Source for the
        /// Event Log if one does not already exist.
        /// </summary>
        /// <param name="identifier">Identifier of event log.</param>
        /// <param name="name">Name of the event log to publish to.</param>
        /// <param name="source">Name of the source for the event log.</param>
        /// <param name="machine">Name of the machine on which the event log resides.</param>
        /// <exception cref="SJP.Common.SJPException">Error creating the class.</exception>
        public EventLogPublisher(string identifier, string name, string source, string machine)
        {
            try
            {
                this.identifier = identifier;

                // Create a source for the event log if it does not already have one.
                if (!EventLog.SourceExists(source, machine))
                {
                    EventSourceCreationData sourceData = new EventSourceCreationData(source, name);
                    EventLog.CreateEventSource(sourceData);
                }

                // Associate EventLog instance with publisher properties.
                eventLog = new EventLog(name, machine, source);

            }
            catch (ArgumentException ae)
            {
                throw new SJPException(
                    String.Format(Messages.EventLogPublisherConstructor, ae.Message, name, source, machine),
                    ae, false, SJPExceptionIdentifier.ELSEventLogConstructor);
            }
            catch (Exception e)
            {
                throw new SJPException(
                    String.Format(Messages.EventLogPublisherConstructor, e.Message, name, source, machine), 
                    e, false, SJPExceptionIdentifier.ELSEventLogConstructor);
            }
        }

       
        /// <summary>
        /// Writes the given log event to the EventLog.
        /// </summary>
        /// <param name="logEvent"><c>LogEvent</c> to write the log entry for.</param>
        /// <exception cref="SJP.Common.SJPException">Log Event was not successfully written to the event log.</exception>
        public void WriteEvent(LogEvent logEvent)
        {
            lock (instanceLock)
            {
                try
                {
                    if (logEvent is OperationalEvent)
                    {
                        OperationalEvent operationalEvent = (OperationalEvent)logEvent;

                        // Derive entry type based on event level.
                        EventLogEntryType entryType;
                        switch (operationalEvent.Level)
                        {
                            case SJPTraceLevel.Error:
                                entryType = EventLogEntryType.Error;
                                break;
                            case SJPTraceLevel.Warning:
                                entryType = EventLogEntryType.Warning;
                                break;
                            default:
                                entryType = EventLogEntryType.Information;
                                break;
                        }

                        eventLog.WriteEntry(operationalEvent.EventLogFormatter.AsString(operationalEvent), entryType, OperationalEventId);

                    }
                    else
                    {
                        // All custom events are logged as Information entry type.
                        eventLog.WriteEntry(logEvent.EventLogFormatter.AsString(logEvent), EventLogEntryType.Information, CustomEventId);

                    }

                }
                catch (ArgumentException ae)
                {
                    throw new SJPException(
                        String.Format(Messages.EventLogPublisherWriteEvent, this.eventLog.Log, this.eventLog.Source, this.eventLog.MachineName),
                        ae, false, SJPExceptionIdentifier.ELSEventLogPublisherWritingEvent);
                }
                catch (InvalidOperationException ioe)
                {
                    throw new SJPException(
                        String.Format(Messages.EventLogPublisherWriteEvent, this.eventLog.Log, this.eventLog.Source, this.eventLog.MachineName), 
                        ioe, false, SJPExceptionIdentifier.ELSEventLogPublisherWritingEvent);
                }
                catch (System.ComponentModel.Win32Exception we)
                {
                    throw new SJPException(
                        String.Format(Messages.EventLogPublisherWriteEvent, this.eventLog.Log, this.eventLog.Source, this.eventLog.MachineName), 
                        we, false, SJPExceptionIdentifier.ELSEventLogPublisherWritingEvent);
                }
                catch (SystemException se)
                {
                    throw new SJPException(
                        String.Format(Messages.EventLogPublisherWriteEvent, this.eventLog.Log, this.eventLog.Source, this.eventLog.MachineName), 
                        se, false, SJPExceptionIdentifier.ELSEventLogPublisherWritingEvent);
                }
                catch (Exception e)
                {
                    throw new SJPException(
                        String.Format(Messages.EventLogPublisherWriteEvent, this.eventLog.Log, this.eventLog.Source, this.eventLog.MachineName), 
                        e, false, SJPExceptionIdentifier.ELSEventLogPublisherWritingEvent);
                }
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
                eventLog.Dispose();
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
