// *********************************************** 
// NAME             : Key.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Key class containing a number of seperate classes where each
// defines a KeyType for a specific data type that can be saved
// within a session.  
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.SessionManager
{
    // Before any new data types can be stored by the session manager
    // they must have a class entered into this file which corresponds to
    // the new data type.  This class must implement the IKey interface.

    #region Abstract key definition

    /// <summary>
    /// Base functionality that are used by almost every key
    /// </summary>
    public class AbstractKey : IKey
    {
        protected string KeyConst = string.Empty;

        #region IKey interface

        /// <summary>
        /// Get ID string representing TypePrefix + KeyName 
        /// </summary>
        public string ID
        {
            get { return KeyConst; }
        }

        #endregion

        #region Public override methods

        /// <summary>
        /// A string representation of this key.
        /// </summary>
        /// <returns>The string representation of this key</returns>
        public override string ToString()
        {
            return KeyConst;
        }

        /// <summary>
        /// Returns a hash code suitable for being used in hash tables or other types
        /// of hashed collections
        /// </summary>
        /// <returns>A suitable hash code</returns>
        public override int GetHashCode()
        {
            return KeyConst.GetHashCode();
        }

        #endregion
    }

    #endregion

    #region Key definitions

    #region Common type keys

    /// <summary>
    /// DateKey class for the DateTime data type
    /// </summary>
    public class DateKey : AbstractKey
    {
        public DateKey(string keyName)
        {
            KeyConst = "dte@" + keyName;
        }
    }

    /// <summary>
    /// DoubleKey class for the Double data type
    /// </summary>
    public class DoubleKey : AbstractKey
    {
        public DoubleKey(string keyName)
        {
            KeyConst = "dbl@" + keyName;
        }
    }

    /// <summary>
    /// IntKey class for the int32 data type
    /// </summary>
    public class IntKey : AbstractKey
    {
        public IntKey(string keyName)
        {
            KeyConst = "int@" + keyName;
        }
    }

    /// <summary>
    /// StringKey class for the string data type
    /// </summary>
    public class StringKey : AbstractKey
    {
        public StringKey(string keyName)
        {
            KeyConst = "str@" + keyName;
        }
    }

    /// <summary>
    /// BoolKey class for the bool data type
    /// </summary>
    public class BoolKey : AbstractKey
    {
        public BoolKey(string keyName)
        {
            KeyConst = "bool@" + keyName;
        }

    }

    #endregion

    #region Custom type keys

    /// <summary>
    /// PageIdKey class for the pageId data type
    /// </summary>
    public class PageIdKey : AbstractKey
    {
        public PageIdKey(string keyName)
        {
            KeyConst = "pgId@" + keyName;
        }
    }

    /// <summary>
    /// Key for saving objects in deferred storage
    /// </summary>
    public class DeferredKey : AbstractKey
    {
        public DeferredKey(string keyName)
        {
            KeyConst = "def@" + keyName;
        }

        /// <summary>
        /// So this keytype can be extended without changing the KeyConst.
        /// </summary>
        protected DeferredKey()
        {
        }
    }

    #endregion

    #endregion
}
