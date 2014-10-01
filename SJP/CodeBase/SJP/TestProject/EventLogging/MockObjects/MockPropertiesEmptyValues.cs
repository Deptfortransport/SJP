using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Summary description for MockPropertiesEmptyValues.
    /// </summary>
    public class MockPropertiesEmptyValues : IPropertyProvider
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

        public MockPropertiesEmptyValues()
        {
            current = new Dictionary<string,string>();

            current[Keys.QueuePublishers] = "";
            current[Keys.DefaultPublisher] = ""; // bad
            current[Keys.FilePublishers] = "";
            current[Keys.EmailPublishers] = "";
            current[Keys.CustomPublishers] = "";
            current[Keys.EventLogPublishers] = "";
            current[Keys.ConsolePublishers] = "";

            current[Keys.OperationalTraceLevel] = ""; //bad

            current[Keys.OperationalEventVerbosePublishers] = "";  // bad
            current[Keys.OperationalEventInfoPublishers] = "";     // bad
            current[Keys.OperationalEventWarningPublishers] = "";  // bad
            current[Keys.OperationalEventErrorPublishers] = "";    // bad

            // current[Keys.CustomEvents] = ""; //bad

            current[Keys.CustomEventsLevel] = "";  // bad

            current[Keys.CustomEvents] = "";           


        }

        /// <summary>
        /// Sets the properties to enabled test validation errors raised during logging property validator tests
        /// </summary>
        public void SetPropertiesForValidationErrors()
        {
            current = new Dictionary<string, string>();

            current[Keys.QueuePublishers] = "Queue1 Queue2";
            current[Keys.DefaultPublisher] = "File1";
            current[Keys.FilePublishers] = "File1 File2";
            current[Keys.EmailPublishers] = "Email1 Email2";
            current[Keys.CustomPublishers] = "CustomPublisher1 CustomPublisher2";
            current[Keys.EventLogPublishers] = "EventLog1 EventLog2";
            current[Keys.ConsolePublishers] = "Console1 Console2";

            current[Keys.OperationalTraceLevel] = ""; //bad

            current[Keys.OperationalEventVerbosePublishers] = "";  // bad
            current[Keys.OperationalEventInfoPublishers] = "";     // bad
            current[Keys.OperationalEventWarningPublishers] = "";  // bad
            current[Keys.OperationalEventErrorPublishers] = "";    // bad

            // current[Keys.CustomEvents] = ""; //bad

            current[Keys.CustomEventsLevel] = "";  // bad

            current[Keys.CustomEvents] = "";      
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
