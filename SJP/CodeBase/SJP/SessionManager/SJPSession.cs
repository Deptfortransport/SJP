// *********************************************** 
// NAME             : SJPSession.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: SJPSession wrapper class providing a number of indexers that ensure any 
// data saved to the session is done so in a type safe manner.
// ************************************************
// 
                
using System;
using System.Web;
using SJP.Common;

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// SJPSession wrapper class forwards all calls to System.Web.Session
    /// </summary>
    public class SJPSession : ISJPSession
    {
        #region Private variables

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
                return (int)HttpContext.Current.Session[GetFullID(key)];
            }
            set
            {
                HttpContext.Current.Session[GetFullID(key)] = value;
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
                return (string)HttpContext.Current.Session[GetFullID(key)];
            }
            set
            {
                HttpContext.Current.Session[GetFullID(key)] = value;
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
                return (double)HttpContext.Current.Session[GetFullID(key)];
            }
            set
            {
                HttpContext.Current.Session[GetFullID(key)] = value;
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
                return (DateTime)HttpContext.Current.Session[GetFullID(key)];
            }
            set
            {
                HttpContext.Current.Session[GetFullID(key)] = value;
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
                if (HttpContext.Current.Session[GetFullID(key)] == null)
                {
                    return false;
                }
                return (bool)HttpContext.Current.Session[GetFullID(key)];
            }
            set
            {
                HttpContext.Current.Session[GetFullID(key)] = value;
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
                return (PageId)HttpContext.Current.Session[GetFullID(key)];
            }
            set
            {
                HttpContext.Current.Session[GetFullID(key)] = value;
            }
        }
                
        /// <summary>
        /// This is the string representation of the Session ID
        /// </summary>
        public string SessionID
        {
            get { return HttpContext.Current.Session.SessionID; }
        }

        #endregion
    }
}
