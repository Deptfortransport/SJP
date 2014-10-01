using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;
using System.IO;
using System.Messaging;
using System.Diagnostics;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Summary description for MockPropertiesGoodPublishersBadEvents.
    /// </summary>
    public class MockPropertiesGoodPublishersBadEvents : IPropertyProvider
    {
        private Dictionary<string,string> current;

        public string this[string key]
        {
            get
            {
                if (current.ContainsKey(key))
                {
                    return (string)current[key];
                }
                else
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// Dummy Read only Indexer property that also takes a partner ID. 		
        /// Since this method was added to the interface and 
        /// hence every class which is implementing  IPropertyProvider interface must implement this method.
        /// </summary>
        public string this[string key, int partnerID]
        {
            get { return string.Empty; }
        }

        public MockPropertiesGoodPublishersBadEvents()
        {
            current = new Dictionary<string,string>();

            // (GOOD) PUBLISHER PROPERTIES START
            current[Keys.QueuePublishers] = "Queue1 Queue2";
            current[Keys.DefaultPublisher] = "File1";
            current[Keys.FilePublishers] = "File1 File2";
            current[Keys.EmailPublishers] = "Email1 Email2";
            current[Keys.CustomPublishers] = "CustomPublisher1 CustomPublisher2";
            current[Keys.EventLogPublishers] = "EventLog1 EventLog2";
            current[Keys.ConsolePublishers] = "Console1 Console2";

            current[String.Format(Keys.QueuePublisherPath, "Queue1")] = Environment.MachineName + @"\Private$\TestQueue1$";
            current[String.Format(Keys.QueuePublisherDelivery, "Queue1")] = "Express";
            current[String.Format(Keys.QueuePublisherPriority, "Queue1")] = "High";
            current[String.Format(Keys.QueuePublisherPath, "Queue2")] = Environment.MachineName + @"\Private$\TestQueue2$";
            current[String.Format(Keys.QueuePublisherDelivery, "Queue2")] = "Recoverable";
            current[String.Format(Keys.QueuePublisherPriority, "Queue2")] = "Normal";

            current[String.Format(Keys.FilePublisherDirectory, "File1")] = @".\File1";
            current[String.Format(Keys.FilePublisherRotation, "File1")] = "10";
            current[String.Format(Keys.FilePublisherDirectory, "File2")] = @".\File2";
            current[String.Format(Keys.FilePublisherRotation, "File2")] = "2000";

            current[String.Format(Keys.EmailPublisherTo, "Email1")] = "garypeaton@hotmail.com";
            current[String.Format(Keys.EmailPublisherFrom, "Email1")] = "TDSupport1@slb.com";
            current[String.Format(Keys.EmailPublisherSubject, "Email1")] = "subject1";
            current[String.Format(Keys.EmailPublisherPriority, "Email1")] = "High";
            current[String.Format(Keys.EmailPublisherTo, "Email2")] = "garypeaton@hotmail.com";
            current[String.Format(Keys.EmailPublisherFrom, "Email2")] = "TDSupport2@slb.com";
            current[String.Format(Keys.EmailPublisherSubject, "Email2")] = "ExactlyThirtyFiveCharactersLong....";
            current[String.Format(Keys.EmailPublisherPriority, "Email2")] = "Low";

            current[String.Format(Keys.ConsolePublisherStream, "Console1")] = "Error";
            current[String.Format(Keys.ConsolePublisherStream, "Console2")] = "Out";

            current[Keys.EmailPublishersSmtpServer] = "localhost";

            current[String.Format(Keys.CustomPublisherName, "CustomPublisher1")] = "TDPublisher1";
            current[String.Format(Keys.CustomPublisherName, "CustomPublisher2")] = "TDPublisher2";

            current[String.Format(Keys.EventLogPublisherName, "EventLog1")] = "ELName1";
            current[String.Format(Keys.EventLogPublisherSource, "EventLog1")] = "TDSource1";
            current[String.Format(Keys.EventLogPublisherMachine, "EventLog1")] = Environment.MachineName;
            current[String.Format(Keys.EventLogPublisherName, "EventLog2")] = "ELName2";
            current[String.Format(Keys.EventLogPublisherSource, "EventLog2")] = "TDSource2";
            current[String.Format(Keys.EventLogPublisherMachine, "EventLog2")] = Environment.MachineName;

            // (GOOD) PUBLISHER PROPERTIES END

            // (BAD) EVENT PROPERTIES START
            current[Keys.OperationalTraceLevel] = "ErrorOops";
            current[Keys.CustomEventsLevel] = "OnOops";

            current[Keys.OperationalEventVerbosePublishers] = "Queue1Oops Queue2Oops";
            current[Keys.OperationalEventInfoPublishers] = "File2Oops";
            current[Keys.OperationalEventWarningPublishers] = "Email1Oops";
            current[Keys.OperationalEventErrorPublishers] = "Email2Oops EventLog1Oops";

            current[Keys.CustomEvents] = "CustomEvent1 CustomEvent2 CustomEvent3 CustomEvent4 CustomEvent5";

            current[String.Format(Keys.CustomEventName, "CustomEvent1")] = "TDEventUnknown";
            current[String.Format(Keys.CustomEventAssembly, "CustomEvent1")] = "td.common.logging";
            current[String.Format(Keys.CustomEventPublishers, "CustomEvent1")] = "UnknownPub";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent1")] = "OnOops";
            current[String.Format(Keys.CustomEventName, "CustomEvent2")] = "TDEvent2";
            current[String.Format(Keys.CustomEventAssembly, "CustomEvent2")] = "Unknown";
            current[String.Format(Keys.CustomEventPublishers, "CustomEvent2")] = "File1";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent2")] = "Off";
            current[String.Format(Keys.CustomEventName, "CustomEvent3")] = "TDEvent3";
            current[String.Format(Keys.CustomEventAssembly, "CustomEvent3")] = "td.common.logging";
            current[String.Format(Keys.CustomEventPublishers, "CustomEvent3")] = "CustomPublisher1";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent3")] = "On";
            current[String.Format(Keys.CustomEventName, "CustomEvent4")] = "";
            current[String.Format(Keys.CustomEventAssembly, "CustomEvent4")] = "";
            current[String.Format(Keys.CustomEventPublishers, "CustomEvent4")] = "";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent4")] = "";
            // (BAD) EVENT PROPERTIES END

            // create necessary publisher sinks
            DirectoryInfo di1 = new DirectoryInfo(current[String.Format(Keys.FilePublisherDirectory, "File1")].ToString());
            di1.Create();
            DirectoryInfo di2 = new DirectoryInfo(current[String.Format(Keys.FilePublisherDirectory, "File2")].ToString());
            di2.Create();
            if (!MessageQueue.Exists(current[String.Format(Keys.QueuePublisherPath, "Queue1")].ToString()))
            {
                using (MessageQueue newQueue1 = MessageQueue.Create(current[String.Format(Keys.QueuePublisherPath, "Queue1")].ToString(), false)) { }
            }
            if (!MessageQueue.Exists(current[String.Format(Keys.QueuePublisherPath, "Queue2")].ToString()))
            {
                using (MessageQueue newQueue1 = MessageQueue.Create(current[String.Format(Keys.QueuePublisherPath, "Queue2")].ToString(), false)) { }
            }
            if (!EventLog.Exists(current[String.Format(Keys.EventLogPublisherName, "EventLog1")].ToString()))
            {
                EventSourceCreationData sourceData = new EventSourceCreationData(current[String.Format(Keys.EventLogPublisherSource, "EventLog1")].ToString(),
                                                                                 current[String.Format(Keys.EventLogPublisherName, "EventLog1")].ToString());
                EventLog.CreateEventSource(sourceData);
            }
            if (!EventLog.Exists(current[String.Format(Keys.EventLogPublisherName, "EventLog2")].ToString()))
            {
                EventSourceCreationData sourceData = new EventSourceCreationData(current[String.Format(Keys.EventLogPublisherSource, "EventLog2")].ToString(),
                                                                                 current[String.Format(Keys.EventLogPublisherName, "EventLog2")].ToString());
                EventLog.CreateEventSource(sourceData);
            }
        }

        // ---------------------------------------

        // Stuff required from interface - not actually
        // used by the mock

        public event SupersededEventHandler Superseded;

        // following definition gets rid of compiler warning
        public void Supersede()
        {
            Superseded(this, new EventArgs());
        }

        public string ApplicationID
        {
            get { return ""; }
        }

        public string GroupID
        {
            get { return ""; }
        }

        public bool IsSuperseded
        {
            get { return false; }
        }

        public int Version
        {
            get { return 0; }
        }

        // ---------------------------------------

    }
}
