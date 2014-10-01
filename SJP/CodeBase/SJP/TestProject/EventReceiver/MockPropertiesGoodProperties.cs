using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Reporting.EventReceiver;
using System.Messaging;

namespace SJP.TestProject.EventReceiver
{
    class MockPropertiesGoodProperties : IPropertyProvider
    {
        private Dictionary<string, string> current;

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

        public MockPropertiesGoodProperties()
        {
            current = new Dictionary<string, string>();

            current[Keys.ReceiverQueue] = "Queue1";
            current[string.Format(Keys.ReceiverQueuePath, "Queue1")] = Environment.MachineName + @"\Private$\ERTestQueue1$"; //@".\private$\mockpropertiesgoodproperties";
            current[string.Format(Keys.ReceiverQueuePath, "Queue2")] = Environment.MachineName + @"\Private$\ERTestQueue2$";

            current[Keys.TimeBeforeRecovery] = "1000";

            current["Logging.Publisher.Queue"] = "";
            current["Logging.Publisher.Email"] = "";
            current["Logging.Publisher.EventLog"] = "";
            current["Logging.Publisher.Console"] = "";

            current["Logging.Event.Operational.Info.Publishers"] = "F1";
            current["Logging.Event.Operational.Verbose.Publishers"] = "F1";
            current["Logging.Event.Operational.Warning.Publishers"] = "F1";
            current["Logging.Event.Operational.Error.Publishers"] = "F1";

            current["Logging.Event.Custom.Trace"] = "On";
            current["Logging.Event.Custom"] = "ROE";
            current["Logging.Event.Custom.ROE.Name"] = "ReceivedOperationalEvent";
            current["Logging.Event.Custom.ROE.Assembly"] = "td.reportdataprovider.tdpcustomevents";
            current["Logging.Event.Custom.ROE.Publishers"] = "F1";
            current["Logging.Event.Custom.ROE.Trace"] = "On";

            current["Logging.Publisher.File"] = "F1";
            current["Logging.Publisher.File.F1.Directory"] = @".\testout";
            current["Logging.Publisher.File.F1.Rotation"] = "1024";

            if (!MessageQueue.Exists(current[String.Format(Keys.ReceiverQueuePath, "Queue1")].ToString()))
            {
                using (MessageQueue newQueue1 = MessageQueue.Create(current[String.Format(Keys.ReceiverQueuePath, "Queue1")].ToString(), false)) {  }
            }

            if (!MessageQueue.Exists(current[String.Format(Keys.ReceiverQueuePath, "Queue2")].ToString()))
            {
                using (MessageQueue newQueue1 = MessageQueue.Create(current[String.Format(Keys.ReceiverQueuePath, "Queue2")].ToString(), false)) {  }
            }
        }


        // Required from interface - not actually used by the mock

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
    }
}
