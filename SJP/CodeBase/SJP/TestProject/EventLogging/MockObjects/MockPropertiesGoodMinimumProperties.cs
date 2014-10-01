using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;
using System.IO;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Summary description for MockPropertiesGoodMinimumProperties.
    /// </summary>
    public class MockPropertiesGoodMinimumProperties : IPropertyProvider
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


        

        public MockPropertiesGoodMinimumProperties()
        {
            current = new Dictionary<string,string>();

            current[Keys.QueuePublishers] = "";
            current[Keys.DefaultPublisher] = "File1";
            current[Keys.FilePublishers] = "File1";
            current[Keys.EmailPublishers] = "";
            current[Keys.CustomPublishers] = "";
            current[Keys.EventLogPublishers] = "";
            current[Keys.ConsolePublishers] = "";

            current[String.Format(Keys.FilePublisherDirectory, "File1")] = @".\File1";
            current[String.Format(Keys.FilePublisherRotation, "File1")] = "10";

            current[Keys.EmailPublishersSmtpServer] = "localhost";

            current[Keys.OperationalTraceLevel] = "Warning";

            current[Keys.OperationalEventVerbosePublishers] = "File1";
            current[Keys.OperationalEventInfoPublishers] = "File1";
            current[Keys.OperationalEventWarningPublishers] = "File1";
            current[Keys.OperationalEventErrorPublishers] = "File1";

            current[Keys.CustomEvents] = "";

            current[Keys.CustomEventsLevel] = "On";

            // create necessary publisher sinks
            DirectoryInfo di = new DirectoryInfo(current[String.Format(Keys.FilePublisherDirectory, "File1")].ToString());
            di.Create();

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
