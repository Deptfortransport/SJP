using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;

namespace SJP.TestProject.EventLogging.MockObjects
{
    /// <summary>
    /// Summary description for MockPropertiesEmpty.
    /// </summary>
    public class MockPropertiesEmpty : IPropertyProvider
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

        public MockPropertiesEmpty()
        {
            current = new Dictionary<string,string>();

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
            get { return String.Empty; }
        }

        public string GroupID
        {
            get { return String.Empty; }
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
