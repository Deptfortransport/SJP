using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Summary description for MockProperties.
    /// </summary>
    public class MockPropertiesBadPublishers : IPropertyProvider
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

        public MockPropertiesBadPublishers(bool correctProps)
        {
            current = new Dictionary<string, string>();

            if (correctProps)
            {
               

                current[Keys.QueuePublishers] = "Queue3 Queue4";
                current[Keys.DefaultPublisher] = "File3";
                current[Keys.FilePublishers] = "File3 File4 File5";
                current[Keys.EmailPublishers] = "Email3 Email4";
                current[Keys.CustomPublishers] = "CustomPublisher3 CustomPublisher4 CustomPublisher5 CustomPublisher6";
                current[Keys.EventLogPublishers] = "EventLog3 EventLog4";
                current[Keys.ConsolePublishers] = "Console3 Console4";

                current[String.Format(Keys.QueuePublisherPath, "Queue3")] = Environment.MachineName + @"\Private$\Missing";
                current[String.Format(Keys.QueuePublisherDelivery, "Queue3")] = "Expresso";
                current[String.Format(Keys.QueuePublisherPriority, "Queue3")] = "Highly";
                current[String.Format(Keys.QueuePublisherPath, "Queue4")] = "";
                current[String.Format(Keys.QueuePublisherDelivery, "Queue4")] = "";
                current[String.Format(Keys.QueuePublisherPriority, "Queue4")] = "";

                current[String.Format(Keys.FilePublisherDirectory, "File3")] = @".\Missing";
                current[String.Format(Keys.FilePublisherRotation, "File3")] = "-9";
                current[String.Format(Keys.FilePublisherDirectory, "File4")] = @".\Missing";
                current[String.Format(Keys.FilePublisherRotation, "File4")] = "0";
                current[String.Format(Keys.FilePublisherDirectory, "File5")] = "";
                current[String.Format(Keys.FilePublisherRotation, "File5")] = "";

                current[String.Format(Keys.EmailPublisherTo, "Email3")] = "garypeaton@@hotmail.com";
                current[String.Format(Keys.EmailPublisherFrom, "Email3")] = "TDSupport3oops";
                current[String.Format(Keys.EmailPublisherSubject, "Email3")] = "ExactlyThirtySixCharactersLong......";
                current[String.Format(Keys.EmailPublisherPriority, "Email3")] = "Highly";
                current[String.Format(Keys.EmailPublisherTo, "Email4")] = "@hotmail.com";
                current[String.Format(Keys.EmailPublisherFrom, "Email4")] = "TDSupport4";
                current[String.Format(Keys.EmailPublisherSubject, "Email4")] = "reallyreallyreallyreallyreallyreallyreallyreallyreallyreallylongsubjectline";
                current[String.Format(Keys.EmailPublisherPriority, "Email4")] = "";

                current[String.Format(Keys.ConsolePublisherStream, "Console3")] = "Undefined";
                current[String.Format(Keys.ConsolePublisherStream, "Console4")] = "";

                current[Keys.EmailPublishersSmtpServer] = "";

                current[String.Format(Keys.CustomPublisherName, "CustomPublisher3")] = "TDPublisher3";
                current[String.Format(Keys.CustomPublisherName, "CustomPublisher4")] = "BadPublisher";
                current[String.Format(Keys.CustomPublisherName, "CustomPublisher5")] = "BadderPublisher";
                current[String.Format(Keys.CustomPublisherName, "CustomPublisher6")] = "";

                current[String.Format(Keys.EventLogPublisherName, "EventLog3")] = "VeryLongEventLogName";
                current[String.Format(Keys.EventLogPublisherSource, "EventLog3")] = "SourceName";
                current[String.Format(Keys.EventLogPublisherMachine, "EventLog3")] = "UnknownMachine";
                current[String.Format(Keys.EventLogPublisherName, "EventLog4")] = "Name";
                current[String.Format(Keys.EventLogPublisherSource, "EventLog4")] = "Source";
                current[String.Format(Keys.EventLogPublisherMachine, "EventLog4")] = "Machine";
            }
            else
            {
                current[Keys.QueuePublishers] = "Queue3 Queue4 Queue5";
                current[Keys.DefaultPublisher] = "File3";
                current[Keys.FilePublishers] = "File3 File4 File5 File6";
                current[Keys.EmailPublishers] = "Email3 Email4 Email5";
                current[Keys.CustomPublishers] = "CustomPublisher3 CustomPublisher4 CustomPublisher5 CustomPublisher6 CustomPublisher7";
                current[Keys.EventLogPublishers] = "EventLog3 EventLog4 EventLog5";
                current[Keys.ConsolePublishers] = "Console3 Console4 Console5";

                current[String.Format(Keys.QueuePublisherPath, "Queue3")] = Environment.MachineName + @"\Private$\Missing";
                current[String.Format(Keys.QueuePublisherDelivery, "Queue3")] = "Expresso";
                current[String.Format(Keys.QueuePublisherPriority, "Queue3")] = "Highly";
                current[String.Format(Keys.QueuePublisherPath, "Queue4")] = "";
                current[String.Format(Keys.QueuePublisherDelivery, "Queue4")] = "";
                current[String.Format(Keys.QueuePublisherPriority, "Queue4")] = "";

                current[String.Format(Keys.FilePublisherDirectory, "File3")] = @".\Missing";
                current[String.Format(Keys.FilePublisherRotation, "File3")] = "-9";
                current[String.Format(Keys.FilePublisherDirectory, "File4")] = @".\Missing";
                current[String.Format(Keys.FilePublisherRotation, "File4")] = "0";
                current[String.Format(Keys.FilePublisherDirectory, "File5")] = "";
                current[String.Format(Keys.FilePublisherRotation, "File5")] = "";

                current[String.Format(Keys.EmailPublisherTo, "Email3")] = "garypeaton@@hotmail.com";
                current[String.Format(Keys.EmailPublisherFrom, "Email3")] = "TDSupport3oops";
                current[String.Format(Keys.EmailPublisherSubject, "Email3")] = "ExactlyThirtySixCharactersLong......";
                current[String.Format(Keys.EmailPublisherPriority, "Email3")] = "Highly";
                current[String.Format(Keys.EmailPublisherTo, "Email4")] = "@hotmail.com";
                current[String.Format(Keys.EmailPublisherFrom, "Email4")] = "TDSupport4";
                current[String.Format(Keys.EmailPublisherSubject, "Email4")] = "reallyreallyreallyreallyreallyreallyreallyreallyreallyreallylongsubjectline";
                current[String.Format(Keys.EmailPublisherPriority, "Email4")] = "";

                current[String.Format(Keys.ConsolePublisherStream, "Console3")] = "Undefined";
                current[String.Format(Keys.ConsolePublisherStream, "Console4")] = "";

                current[Keys.EmailPublishersSmtpServer] = "";

                current[String.Format(Keys.CustomPublisherName, "CustomPublisher3")] = "TDPublisher3";
                current[String.Format(Keys.CustomPublisherName, "CustomPublisher4")] = "BadPublisher";
                current[String.Format(Keys.CustomPublisherName, "CustomPublisher5")] = "BadderPublisher";
                current[String.Format(Keys.CustomPublisherName, "CustomPublisher6")] = "";

                current[String.Format(Keys.EventLogPublisherName, "EventLog3")] = "VeryLongEventLogName";
                current[String.Format(Keys.EventLogPublisherSource, "EventLog3")] = "SourceName";
                current[String.Format(Keys.EventLogPublisherMachine, "EventLog3")] = "UnknownMachine";
                current[String.Format(Keys.EventLogPublisherName, "EventLog4")] = "Name";
                current[String.Format(Keys.EventLogPublisherSource, "EventLog4")] = "Source";
                current[String.Format(Keys.EventLogPublisherMachine, "EventLog4")] = "Machine";

            }


            // NOTE : the last publisher identifier does not have ANY other keys defined for it

            // NB EVENT PROPERTIES ARE NOT REQUIRED FOR THIS MOCK OBJECT

        }

        public MockPropertiesBadPublishers()
        {
            current = new Dictionary<string,string>();

            current[Keys.QueuePublishers] = "Queue3 Queue4 Queue5";
            current[Keys.DefaultPublisher] = "File3";
            current[Keys.FilePublishers] = "File3 File4 File5 File6";
            current[Keys.EmailPublishers] = "Email3 Email4 Email5";
            current[Keys.CustomPublishers] = "CustomPublisher3 CustomPublisher4 CustomPublisher5 CustomPublisher6 CustomPublisher7";
            current[Keys.EventLogPublishers] = "EventLog3 EventLog4 EventLog5";
            current[Keys.ConsolePublishers] = "Console3 Console4 Console5";

            current[String.Format(Keys.QueuePublisherPath, "Queue3")] = Environment.MachineName + @"\Private$\Missing";
            current[String.Format(Keys.QueuePublisherDelivery, "Queue3")] = "Expresso";
            current[String.Format(Keys.QueuePublisherPriority, "Queue3")] = "Highly";
            current[String.Format(Keys.QueuePublisherPath, "Queue4")] = "";
            current[String.Format(Keys.QueuePublisherDelivery, "Queue4")] = "";
            current[String.Format(Keys.QueuePublisherPriority, "Queue4")] = "";

            current[String.Format(Keys.FilePublisherDirectory, "File3")] = @".\Missing";
            current[String.Format(Keys.FilePublisherRotation, "File3")] = "-9";
            current[String.Format(Keys.FilePublisherDirectory, "File4")] = @".\Missing";
            current[String.Format(Keys.FilePublisherRotation, "File4")] = "0";
            current[String.Format(Keys.FilePublisherDirectory, "File5")] = "";
            current[String.Format(Keys.FilePublisherRotation, "File5")] = "";

            current[String.Format(Keys.EmailPublisherTo, "Email3")] = "garypeaton@@hotmail.com";
            current[String.Format(Keys.EmailPublisherFrom, "Email3")] = "TDSupport3oops";
            current[String.Format(Keys.EmailPublisherSubject, "Email3")] = "ExactlyThirtySixCharactersLong......";
            current[String.Format(Keys.EmailPublisherPriority, "Email3")] = "Highly";
            current[String.Format(Keys.EmailPublisherTo, "Email4")] = "@hotmail.com";
            current[String.Format(Keys.EmailPublisherFrom, "Email4")] = "TDSupport4";
            current[String.Format(Keys.EmailPublisherSubject, "Email4")] = "reallyreallyreallyreallyreallyreallyreallyreallyreallyreallylongsubjectline";
            current[String.Format(Keys.EmailPublisherPriority, "Email4")] = "";

            current[String.Format(Keys.ConsolePublisherStream, "Console3")] = "Undefined";
            current[String.Format(Keys.ConsolePublisherStream, "Console4")] = "";

            current[Keys.EmailPublishersSmtpServer] = "";

            current[String.Format(Keys.CustomPublisherName, "CustomPublisher3")] = "TDPublisher3";
            current[String.Format(Keys.CustomPublisherName, "CustomPublisher4")] = "BadPublisher";
            current[String.Format(Keys.CustomPublisherName, "CustomPublisher5")] = "BadderPublisher";
            current[String.Format(Keys.CustomPublisherName, "CustomPublisher6")] = "";

            current[String.Format(Keys.EventLogPublisherName, "EventLog3")] = "VeryLongEventLogName";
            current[String.Format(Keys.EventLogPublisherSource, "EventLog3")] = "SourceName";
            current[String.Format(Keys.EventLogPublisherMachine, "EventLog3")] = "UnknownMachine";
            current[String.Format(Keys.EventLogPublisherName, "EventLog4")] = "Name";
            current[String.Format(Keys.EventLogPublisherSource, "EventLog4")] = "Source";
            current[String.Format(Keys.EventLogPublisherMachine, "EventLog4")] = "Machine";


            // NOTE : the last publisher identifier does not have ANY other keys defined for it

            // NB EVENT PROPERTIES ARE NOT REQUIRED FOR THIS MOCK OBJECT

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
