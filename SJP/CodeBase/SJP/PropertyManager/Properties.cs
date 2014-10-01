// *********************************************** 
// NAME             : Properties.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Implementation of IPropertyProvider provides base class 
//                    for various property providers
// ************************************************
                
                
using System;
using System.Collections.Generic;
using SJP.Common.ServiceDiscovery;

namespace SJP.Common.PropertyManager
{
    /// <summary>
    /// Abstract property provider class 
    /// </summary>
    public abstract class Properties : IPropertyProvider
    {
       
       
        #region Protected Static Fields
        protected static string strApplicationID = string.Empty;
        protected static string strGroupID = string.Empty;
        #endregion
        
        #region Protected Fields
        protected bool boolSuperseded = false;
        protected int intVersion = 0;
        protected Dictionary<string, string> propertyDictionary;
       
        #endregion

        #region Events
        public event SupersededEventHandler Superseded;
        #endregion


        #region Implementation of IPropertyProvider

        /// <summary>
        /// Read Only Property determining if the property values are refreshed in memory from data store
        /// </summary>
        public bool IsSuperseded
        {
            get { return boolSuperseded; }
        }

        
        /// <summary>
        /// Read only property determing the version of the properties 
        /// </summary>
        public int Version
        {
            get { return intVersion; }
        }


        /// <summary>
        /// Indexed property returning the value of the property whose key is supplied
        /// </summary>
        /// <param name="key">Key to uniquely identify property</param>
        /// <returns>Value of the property</returns>
        public string this[string key]
        {
            get
            {
                if (propertyDictionary.ContainsKey(key))
                {
                    return propertyDictionary[key].ToString();
                }
                
                return null;
            }
        }

        /// <summary>
        /// Read only property returning the application id of the property store
        /// </summary>
        public string ApplicationID
        {
            get { return strApplicationID; }
        }

        /// <summary>
        /// Read only property returning the group id of the property store
        /// </summary>
        public string GroupID
        {
            get { return strGroupID; }
        }

        /// <summary>
        /// Static read only property giving access to current property provider cached in the Service Discover
        /// </summary>
        public static IPropertyProvider Current
        {
            get
            {


                return SJPServiceDiscovery.Current.Get<IPropertyProvider>(ServiceDiscoveryKey.PropertyService);


            }
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Abstract load method to load the properties data in memory from data store 
        /// </summary>
        /// <returns></returns>
        public abstract IPropertyProvider Load();
        #endregion

        #region Publid Methods
       
        /// <summary>
        /// Supersede method sets the flag indicating property data refreshed and also triggers 
        /// Superseded event
        /// </summary>
        public void Supersede()
        {
            boolSuperseded = true;

            // Raise the Superseded event
            if (Superseded != null)
                Superseded(this, EventArgs.Empty);
           
        }
                
        

        #endregion
    }
}
