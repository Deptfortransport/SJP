using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.UserPortal.SessionManager;
using SJP.Common;

namespace SJP.TestProject
{
    public class MockSJPSession : ISJPSession
    {
        #region Private variables
        private static Dictionary<object, object> sessionStore = new Dictionary<object, object>();
        #endregion

        #region Private methods

        /// <summary>
        /// Method to return the key id.
        /// Used to allow manipulation of key, e.g. adding a partion prefix value if 
        /// session partitioning is introduced future
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetFullID(IKey key)
        {
            return key.ID;
        }

        #endregion

        #region ISJPSession definitions
        // The following get methods take an instance of the appropriate Key,
        // they then attempt to find this key's ID property in the System.Web.Session data. 
        // A succesfully matched entry's value is returned to the calling class.
        // The set methods place a value into the session data as a string, along with the 
        // ID property of the given key. 

        /// <summary>
        /// This is a read/write property that adds data of type int32 to
        /// the ASP session data.
        /// </summary>
        /// <value>Int32</value>
        public int this[IntKey key]
        {
            get
            {
                return (int)sessionStore[GetFullID(key)];
            }
            set
            {
                sessionStore[GetFullID(key)] = value;
            }
        }

        /// <summary>
        /// This is a read/write property that adds data of type string to
        /// the ASP session data.
        /// </summary>
        /// <value>String</value>
        public string this[StringKey key]
        {
            get
            {
                return (string)sessionStore[GetFullID(key)];
            }
            set
            {
                sessionStore[GetFullID(key)] = value;
            }
        }

        /// <summary>
        /// This is a read/write property that adds data of type Double to
        /// the ASP session data.
        /// </summary>
        /// <value>double</value>
        public double this[DoubleKey key]
        {
            get
            {
                return (double)sessionStore[GetFullID(key)];
            }
            set
            {
                sessionStore[GetFullID(key)] = value;
            }
        }

        /// <summary>
        /// This is a read/write property that adds data of type DateTime to
        /// the ASP session data.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime this[DateKey key]
        {
            get
            {
                return (DateTime)sessionStore[GetFullID(key)];
            }
            set
            {
                sessionStore[GetFullID(key)] = value;
            }
        }

        /// <summary>
        /// This is a read/write property that adds data of type bool to
        /// the ASP session data.
        /// </summary>
        /// <value>bool</value>
        public bool this[BoolKey key]
        {
            get
            {
                if (sessionStore[GetFullID(key)] == null)
                {
                    return false;
                }
                return (bool)sessionStore[GetFullID(key)];
            }
            set
            {
                sessionStore[GetFullID(key)] = value;
            }
        }

        /// <summary>
        /// This is a read/write property that adds data of type PageID to
        /// the ASP session data.
        /// </summary>
        /// <value>PageId</value>
        public PageId this[PageIdKey key]
        {
            get
            {
                return (PageId)sessionStore[GetFullID(key)];
            }
            set
            {
                sessionStore[GetFullID(key)] = value;
            }
        }

        /// <summary>
        /// This is the string representation of the Session ID
        /// </summary>
        public string SessionID
        {
            get { return MockSJPSessionFactory.mockSessionId; }
        }

        #endregion
    }
}
