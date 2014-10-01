// *********************************************** 
// NAME             : DataAccessorString.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 14 Mar 2011
// DESCRIPTION  	: Custom DataAccessor class which allows a String key constructor
// ************************************************
// 

using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Soss.Client;

namespace SJP.UserPortal.StateServer
{
    /// <summary>
    /// String key DataAccessor class
    /// </summary>
    [Serializable]
    public class DataAccessorString : DataAccessor
    {
        #region Constructor

        /// <summary>
        /// Constructor to create a StateServerKey using the string key value
        /// </summary>
        /// <param name="applicationName">Application name for the key</param>
        /// <param name="key">Key as a string</param>
        /// <param name="lockWhenReading">Flag indicating default lock behaviour when reading data object</param>
        public DataAccessorString(string applicationName, string key, bool lockWhenReading) 
            : base(CreateStateServerKey(applicationName, key), lockWhenReading) 
        {
        }

        /// <summary>
        /// Constructor. Call base DataAccessor object constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DataAccessorString(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Private Static methods

        static HashAlgorithm hashCodeProvider = null;

        /// <summary>
        /// Method which creates a new instance of a StateServerKey, 
        /// using a hash of the string key
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static StateServerKey CreateStateServerKey(string applicationName, string key)
        {
            // This is somewhat simplistic in that it doesn't take into consideration 
            // hash value collisions nor the fact that StateServer reserves the all-zeros key for its own use.
            if (hashCodeProvider == null)
            {
                hashCodeProvider = new MD5CryptoServiceProvider();
                hashCodeProvider.Initialize();
            }

            lock (hashCodeProvider)
            {
                byte[] bytes = hashCodeProvider.ComputeHash(Encoding.Unicode.GetBytes(key));

                return new StateServerKey(applicationName, bytes, key);
            }
        }

        #endregion       
    }
}
