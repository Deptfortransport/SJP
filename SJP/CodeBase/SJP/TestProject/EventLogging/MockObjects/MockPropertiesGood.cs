using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;
using System.Messaging;
using System.Diagnostics;
using System.IO;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Summary description for MockPropertiesGood.
    /// </summary>
    public class MockPropertiesGood : IPropertyProvider
    {
        private Dictionary<string,string> current;

        public void MockLevelChange1()
        {
            // change operational trace level
            current[Keys.OperationalTraceLevel] = "Off";

            // change global custom event level
            current[Keys.CustomEventsLevel] = "Off";
                      

            if (Superseded != null)
                Superseded(this, null);
        }

        public void MockLevelChange2()
        {
            // change operational trace level
            current[Keys.OperationalTraceLevel] = "Warning";

            // change global custom event level
            current[Keys.CustomEventsLevel] = "On";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent1")] = "Off";

           
            if (Superseded != null)
                Superseded(this, null);
        }

        public void MockLevelChange3()
        {
            // change operational trace level
            current[Keys.OperationalTraceLevel] = "Info";

            
            if (Superseded != null)
                Superseded(this, null);
        }

        public void MockValidPropertyChange()
        {
            // change operational trace level
            current[Keys.OperationalTraceLevel] = "Warning";

            // change individual custom events levels to valid values
            current[String.Format(Keys.CustomEventLevel, "CustomEvent1")] = "Off";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent2")] = "On";

            if (Superseded != null)
                Superseded(this, null);
        }

        public void MockInvalidPropertyChange()
        {
            // change operational trace level to invalid value
            current[Keys.OperationalTraceLevel] = "Undefined";

            // change global custom events level to invalid value
            current[Keys.CustomEventsLevel] = "Undefined";

            // change individual custom events levels to invalid values
            current[String.Format(Keys.CustomEventLevel, "CustomEvent1")] = "Undefined";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent2")] = "Undefined";
                        

            if (Superseded != null)
                Superseded(this, null);

        }

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


       

        public MockPropertiesGood()
        {
            current = new Dictionary<string,string>();

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
            current[String.Format(Keys.EmailPublisherFrom, "Email1")] = "SJPSupport1@slb.com";
            current[String.Format(Keys.EmailPublisherSubject, "Email1")] = "subject1";
            current[String.Format(Keys.EmailPublisherPriority, "Email1")] = "High";
            current[String.Format(Keys.EmailPublisherTo, "Email2")] = "kcheung@slb.com";
            current[String.Format(Keys.EmailPublisherFrom, "Email2")] = "kcheung@slb.com";
            current[String.Format(Keys.EmailPublisherSubject, "Email2")] = "ExactlyThirtyFiveCharactersLong....";
            current[String.Format(Keys.EmailPublisherPriority, "Email2")] = "Low";

            current[String.Format(Keys.ConsolePublisherStream, "Console1")] = "Error";
            current[String.Format(Keys.ConsolePublisherStream, "Console2")] = "Out";

            current[Keys.EmailPublishersSmtpServer] = "localhost";

            current[String.Format(Keys.CustomPublisherName, "CustomPublisher1")] = "SJPPublisher1";
            current[String.Format(Keys.CustomPublisherName, "CustomPublisher2")] = "SJPPublisher2";

            current[String.Format(Keys.EventLogPublisherName, "EventLog1")] = "ELName1";
            current[String.Format(Keys.EventLogPublisherSource, "EventLog1")] = "SJPSource1";
            current[String.Format(Keys.EventLogPublisherMachine, "EventLog1")] = Environment.MachineName;
            current[String.Format(Keys.EventLogPublisherName, "EventLog2")] = "ELName2";
            current[String.Format(Keys.EventLogPublisherSource, "EventLog2")] = "SJPSource2";
            current[String.Format(Keys.EventLogPublisherMachine, "EventLog2")] = Environment.MachineName;


            current[Keys.OperationalTraceLevel] = "Error";

            current[Keys.OperationalEventVerbosePublishers] = "Queue1 Queue2";
            current[Keys.OperationalEventInfoPublishers] = "File2";
            current[Keys.OperationalEventWarningPublishers] = "File2";
            current[Keys.OperationalEventErrorPublishers] = "Queue2 EventLog2";

            current[Keys.CustomEvents] = "CustomEvent1 CustomEvent2";

            current[String.Format(Keys.CustomEventName, "CustomEvent1")] = "CustomEventOne";
            current[String.Format(Keys.CustomEventAssembly, "CustomEvent1")] = "sjp.testproject";
            current[String.Format(Keys.CustomEventPublishers, "CustomEvent1")] = "CustomPublisher1 Queue1";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent1")] = "On";
            current[String.Format(Keys.CustomEventName, "CustomEvent2")] = "CustomEventTwo";
            current[String.Format(Keys.CustomEventAssembly, "CustomEvent2")] = "sjp.testproject";
            current[String.Format(Keys.CustomEventPublishers, "CustomEvent2")] = "CustomPublisher2";
            current[String.Format(Keys.CustomEventLevel, "CustomEvent2")] = "Off";

            current[Keys.CustomEventsLevel] = "On";

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

        /// <summary>
        /// Exposes the internal property store
        /// </summary>
        public Dictionary<string, string> PropStore
        {
            get { return current; }
        }

        // ---------------------------------------

    }
}
