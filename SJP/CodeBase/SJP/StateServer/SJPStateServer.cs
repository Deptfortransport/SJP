// *********************************************** 
// NAME             : SJPStateServer.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: SJPStateServer class to save and retrieve objects from the state server
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Soss.Client;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;
using SJP.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace SJP.UserPortal.StateServer
{
    /// <summary>
    /// SJPStateServer class to save and retrieve objects from the state server
    /// </summary>
    public class SJPStateServer : ISJPStateServer, IDisposable
    {
        #region Private members

        #region Static members

        /// <summary>
        /// The application name will remain the same for each app domain during runtime 
        /// (unless the overload constructor is used).
        /// This variable stores the default application name so that it doesn't
        /// have to be looked up each time an instance of the class is created.
        /// </summary>
        private static string applicationName = string.Empty;

        /// <summary>
        /// Static values used in accessing objects on the StateServer
        /// </summary>
        private static bool staticValuesSet = false;
        private static int maxRetries = 0;
        private static int sleepIntervalMilliSecs = 0;
        private static int expiryTimeMins = 0;

        #endregion

        /// <summary>
        /// Dictionary used to track the DataAccessors used to create/update/read objects in the StateServer.
        /// This is done to preserve locks on the objects for the duration of the read/modify/update process
        /// on the life cycle of a page
        /// </summary>
        private Dictionary<string, DataAccessor> dataAccessors = new Dictionary<string, DataAccessor>();

        /// <summary>
        /// Application domain name used in accessing the correct state server storage area
        /// </summary>
        private string appName;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor.
        /// Application name used to access the state server storage area is read from properties service
        /// </summary>
        public SJPStateServer()
        {
            SetStaticValues();

            SetApplicationName(string.Empty);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appName">The Application name used to access the state server storage area</param>
        public SJPStateServer(string appName)
        {
            SetStaticValues();

            SetApplicationName(appName);
        }

        #endregion

        #region ISJPStateServer interface methods

        /// <summary>
        /// Saves the supplied deferree object to the state server. 
        /// The session id with the deferred key are joined to form the key to save the object.
        /// Deferree object must be serialiseable
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKey">Key for the deferree object</param>
        /// <param name="deferree">Object to be saved, must be serialiseable</param>
        public void Save(string sessionId, string deferredKey, object deferree)
        {
            string key = sessionId + deferredKey;

            // Get data accessor
            DataAccessor da = GetDataAccessor(key);

            try
            {
                // Used to ensure the object could be obtained, if not, its removed from the dictionary
                bool obtainedLock = false;

                #region Save

                // First try to do a read; another process may have a lock on this object, 
                // so it may take several tries to get a lock.
                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        if (da.Read(false) == null)
                        {
                            // New object, so lock not needed
                            da.Create(expiryTimeMins, deferree, false);

                            obtainedLock = true;
                        }
                        else
                        {
                            // Ensure we have a lock before update
                            da.AcquireOrUpdateLock(true);
                            da.Update(deferree, true);

                            obtainedLock = true;
                        }

                        // Create/Update method calls will release the locks on the objects when they finish,
                        // no need to manually release locks

                        break;
                    }
                    catch (ObjectLockedException)
                    {
                        // Wait and then try to do the locked save again
                        ObjectLockedWait(key, i + 1);
                    }
                }

                #endregion

                // Couldnt establish a lock on the object
                if (!obtainedLock)
                {
                    RemoveDataAccessor(key);

                    throw new SJPException(
                            string.Format(System.Globalization.CultureInfo.CurrentCulture,
                                Messages.FailedLocking, key),
                            false, SJPExceptionIdentifier.SSFailedLocking);
                }
            }
            // Catch all exceptions and rethrow as SJPException
            catch (Exception ex)
            {
                throw new SJPException(
                        string.Format(System.Globalization.CultureInfo.CurrentCulture,
                            Messages.ErrorSaving, key, ex.Message),
                        ex, false, SJPExceptionIdentifier.SSErrorSaving
                        );
            }
        }

        /// <summary>
        /// Reads a deferree object from the state server.
        /// The session id with the deferred key are joined to form the key to read the object.
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKey">Key for the deferree obect</param>
        /// <returns>Object from state server</returns>
        public object Read(string sessionId, string deferredKey)
        {
            object deferredObj = null;

            string key = sessionId + deferredKey;

            DataAccessor da = GetDataAccessor(key);

            try
            {
                // Used to ensure the object could be obtained, if not, its removed from the dictionary
                bool obtainedLock = false;

                // First try to do a read; another process may have a lock on this object, 
                // so it may take several tries to get a lock.
                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        // Read will ensure the lock is refreshed
                        deferredObj = Deserialize(da.Read(true));
                        obtainedLock = true;
                        break;
                    }
                    catch (ObjectLockedException)
                    {
                        // Wait and then try to do the locked read again
                        ObjectLockedWait(key, i + 1);
                    }
                    catch (ObjectNotFoundException)
                    {
                        // Ok if null as it probably hasnt been set, will return null to caller
                        obtainedLock = true;
                        break;
                    }
                }

                // Couldnt establish a lock on the object
                if (!obtainedLock)
                {
                    RemoveDataAccessor(key);

                    throw new SJPException(
                            string.Format(System.Globalization.CultureInfo.CurrentCulture,
                                Messages.FailedLocking, key),
                            false, SJPExceptionIdentifier.SSFailedLocking);
                }
            }
            // Catch all exceptions and rethrow as SJPException
            catch (Exception ex)
            {
                throw new SJPException(
                        string.Format(System.Globalization.CultureInfo.CurrentCulture,
                            Messages.ErrorReading, key, ex.Message),
                        ex, false, SJPExceptionIdentifier.SSErrorReading
                        );
            }

            return deferredObj;
        }

        /// <summary>
        /// Deletes a deferree object from the state server.
        /// The session id with the deferred key are joined to form the key to delete the object.
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKey">Key for the deferree object</param>
        public void Delete(string sessionId, string deferredKey)
        {
            string key = sessionId + deferredKey;

            DataAccessor da = GetDataAccessor(key);

            try
            {
                // Used to ensure the object could be obtained, if not, its removed from the dictionary
                bool obtainedLock = false;

                // First try to do a read; another process may have a lock on this object, 
                // so it may take several tries to get a lock.
                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        // Attempt to delete
                        da.Delete();

                        // Successfully deleted, remove data accessor reference
                        RemoveDataAccessor(key);

                        // Dispose of the accessor as its no longer needed. 
                        // This ensures any underlying locks are removed, not really needed to do here
                        // as delete was previously called but no harm done 
                        da.Dispose();
                        da = null;

                        obtainedLock = true;
                        break;
                    }
                    catch (ObjectLockedException)
                    {
                        // Wait and then try to do the locked read again
                        ObjectLockedWait(key, i + 1);
                    }
                    catch (ObjectNotFoundException)
                    {
                        // Ok if null as it probably hasnt been created
                        obtainedLock = true;
                        break;
                    }
                }

                // Couldnt establish a lock on the object
                if (!obtainedLock)
                {
                    RemoveDataAccessor(key);

                    string message = string.Format(System.Globalization.CultureInfo.CurrentCulture,
                        Messages.ErrorDeleting, key, Messages.DeleteWhenExpires);

                    OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Warning,
                                message
                                );
                    Logger.Write(oe);
                }
            }
            // Catch all exceptions and rethrow as SJPException
            catch (Exception ex)
            {
                throw new SJPException(
                        string.Format(System.Globalization.CultureInfo.CurrentCulture,
                            Messages.ErrorDeleting, key, ex.Message),
                        ex, false, SJPExceptionIdentifier.SSErrorDeleting
                        );
            }
        }

        /// <summary>
        /// Locks the derree object(s) in the state server.
        /// The session id with the deferred key(s) are joined to form the key for the object to lock
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKeys">Keys for the deferree objects</param>
        public void Lock(string sessionId, string[] deferredKeys)
        {
            string key = string.Empty;
            
            try
            {
                // Loop through all supplied keys and aquire a lock
                foreach (string deferredKey in deferredKeys)
                {
                    key = sessionId + deferredKey;

                    DataAccessor da = GetDataAccessor(key);

                    // Used to ensure the object could be obtained, if not, its removed from the dictionary
                    bool obtainedLock = false;

                    // First try to do a read; another process may have a lock on this object, 
                    // so it may take several tries to get a lock.
                    for (int i = 0; i < maxRetries; i++)
                    {
                        try
                        {
                            // Attempt to place lock
                            da.AcquireOrUpdateLock(true);
                            obtainedLock = true;
                            break;
                        }
                        catch (ObjectLockedException)
                        {
                            ObjectLockedWait(key, i + 1);
                        }
                        catch (ObjectNotFoundException)
                        {
                            // Ok if null as it probably hasnt been created
                            obtainedLock = true;
                            break;
                        }
                    }

                    // Couldnt establish a lock on the object
                    if (!obtainedLock)
                    {
                        RemoveDataAccessor(key);

                        // All keys requested couldnt be locked, throw exception so caller knows
                        throw new SJPException(
                                string.Format(System.Globalization.CultureInfo.CurrentCulture,
                                    Messages.FailedLocking, key),
                                false, SJPExceptionIdentifier.SSFailedLocking);
                    }
                }
                
            }
            // Catch all exceptions and rethrow as SJPException
            catch (Exception ex)
            {
                
                throw new SJPException(
                        string.Format(System.Globalization.CultureInfo.CurrentCulture,
                            Messages.ErrorLocking, key, ex.Message),
                        ex, false, SJPExceptionIdentifier.SSErrorLocking
                        );
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to set the state server static values required by the class
        /// </summary>
        private void SetStaticValues()
        {
            if (!staticValuesSet)
            {
                lock (this)
                {
                    // Check again in case another thread has completed this in the meantime
                    if (!staticValuesSet)
                    {
                        #region Read and parse state server values

                        string strMaxRetries = Properties.Current["StateServer.RetriesMax"];
                        string strSleepIntervalMilliSecs = Properties.Current["StateServer.SleepIntervalMilliSecs"];
                        string strExpiryTimeMins = Properties.Current["StateServer.ExpiryTimeMins"];

                        if (!Int32.TryParse(strMaxRetries, out maxRetries))
                        {
                            maxRetries = 20;

                            OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Error,
                                string.Format(System.Globalization.CultureInfo.CurrentCulture,
                                "Error setting StateServer static value for RetriesMax, missing or invalid property[{0}] ", "StateServer.RetriesMax"));
                            Logger.Write(oe);
                        }

                        if (!Int32.TryParse(strSleepIntervalMilliSecs, out sleepIntervalMilliSecs))
                        {
                            sleepIntervalMilliSecs = 250; // 0.25secs

                            OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Error,
                                string.Format(System.Globalization.CultureInfo.CurrentCulture,
                                "Error setting StateServer static value for SleepIntervalMilliSecs, missing or invalid property[{0}] ", "StateServer.SleepIntervalMilliSecs"));
                            Logger.Write(oe);
                        }

                        if (!Int32.TryParse(strExpiryTimeMins, out expiryTimeMins))
                        {
                            expiryTimeMins = 300; // 5hours

                            OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Error,
                                string.Format(System.Globalization.CultureInfo.CurrentCulture,
                                "Error setting StateServer static value for ExpiryTimeMins, missing or invalid property[{0}] ", "StateServer.ExpiryTimeMins"));
                            Logger.Write(oe);
                        }

                        // Check for valid timeout, 69905 is taken from state server help documentation
                        if ((expiryTimeMins < 0) || (expiryTimeMins > 69905))
                        {
                            expiryTimeMins = 300; // 5hours

                            OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Error,
                                string.Format(System.Globalization.CultureInfo.CurrentCulture,
                                "Error setting StateServer static value for ExpiryTimeMins, invalid property[{0}]. Value must be in the range 0 to 69905.", "StateServer.ExpiryTimeMins"));
                            Logger.Write(oe);
                        }

                        #endregion

                        // Set flag indicating static values are set
                        staticValuesSet = true;

                        #region Log values set

                        if (SJPTraceSwitch.TraceInfo)
                        {
                            OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Info,
                                string.Format("StateServer static values have been set: RetriesMax[{0}] SleepIntervalMilliSecs[{1}] ExpiryTimeMins[{2}]",
                                    maxRetries, sleepIntervalMilliSecs, expiryTimeMins));
                            Logger.Write(oe);
                        }

                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Sets the ApplicationName to be used for the StateServer.
        /// </summary>
        private void SetApplicationName(string appName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                lock (this)
                {
                    // Check again in case another thread has completed the lookup in the meantime
                    if (string.IsNullOrEmpty(applicationName))
                    {
                        #region Read and parse default state server application name

                        string defaultAppName = Properties.Current["StateServer.ApplicationName"];

                        if (string.IsNullOrEmpty(defaultAppName))
                        {
                            defaultAppName = "SJPStateServer";

                            OperationalEvent oe = new OperationalEvent(
                                        SJPEventCategory.Infrastructure,
                                        SJPTraceLevel.Error,
                                        string.Format("Error setting StateServer ApplicationName, missing or invalid property[{0}].", "StateServer.ApplicationName"));
                            Logger.Write(oe);
                        }

                        #endregion

                        // Set static value
                        applicationName = defaultAppName;

                        #region Log

                        if (SJPTraceSwitch.TraceInfo)
                        {
                            OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Info,
                                string.Format("StateServer default application name has been set: ApplicationName[{0}]", applicationName));
                            Logger.Write(oe);
                        }

                        #endregion
                    }
                }
            }

            if (string.IsNullOrEmpty(appName))
            {
                // No app name provided, use the default
                appName = applicationName;
            }

            // Set the objects
            this.appName = appName;
        }

        /// <summary>
        /// Returns a DataAccessor (of type DataAccessorString) from the private list, creating 
        /// a new object if required for the key
        /// </summary>
        /// <param name="key">DataAccessor string key</param>
        private DataAccessor GetDataAccessor(string key)
        {
            if (!dataAccessors.ContainsKey(key))
            {
                // Create accessor object for the state server object, ensure lock is applied when reading
                using (DataAccessor da = new DataAccessorString(appName, key, true))
                {
                    dataAccessors[key] = da;
                }
            }

            // Get the previously used accessor
            return dataAccessors[key];
        }

        /// <summary>
        /// Removes a DataAccessor from the private list
        /// </summary>
        /// <param name="key">DataAccessor string key</param>
        private void RemoveDataAccessor(string key)
        {
            if (dataAccessors.ContainsKey(key))
            {
                // Remove from dictionary, shouldnt retain accessor objects which failed, avoids potential future
                // access problems
                dataAccessors.Remove(key);
            }
        }

        /// <summary>
        /// Method to force a thread sleep. Logs an object locked message
        /// </summary>
        private void ObjectLockedWait(string key, int count)
        {
            if (SJPTraceSwitch.TraceVerbose)
            {
                OperationalEvent oe = new OperationalEvent(
                        SJPEventCategory.Infrastructure,
                        SJPTraceLevel.Verbose,
                        string.Format(System.Globalization.CultureInfo.CurrentCulture,
                            Messages.RetryAttemptObjectLocked, key, count));
                Logger.Write(oe);
            }

            // Wait and then try to do the lock action again
            System.Threading.Thread.Sleep(sleepIntervalMilliSecs);
        }

        /// <summary>
        /// Deserializes a byte array into an object
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private object Deserialize(byte[] bytes)
        {
            object deserializedObject = null;

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    // Deserialize the data
                    deserializedObject = (object)formatter.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error deserializing object read from StateServer, Message[{0}]", ex.Message);
                OperationalEvent oe = new OperationalEvent(
                                SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Error,
                                message,
                                ex);
                Logger.Write(oe);
                throw new SJPException(message, true, SJPExceptionIdentifier.SSErrorReading);
            }

            return deserializedObject;
        }

        #endregion
        
        #region IDisposable methods

        ~SJPStateServer()
        {
            //calls a protected method 
            //the false tells this method
            //not to bother with managed
            //resources
            this.Dispose(false);
        }

        public void Dispose()
        {
            //calls the same method
            //passed true to tell it to
            //clean up managed and unmanaged 
            this.Dispose(true);

            //as dispose has been correctly
            //called we don't need the 
            //'backup' finaliser
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //check this hasn't been called already
            //remember that Dispose can be called again
            if (!disposed)
            {
                //this is passed true in the regular Dispose
                if (disposing)
                {
                    // Dispose managed resources here.

                    #region Dispose managed resources

                    // Tidyup all data accessors containined in this instance
                    foreach (string key in dataAccessors.Keys)
                    {
                        if (dataAccessors[key] != null)
                        {
                            dataAccessors[key].Dispose();
                        }
                    }

                    #endregion
                }

                //both regular Dispose and the finaliser
                //will hit this code
                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        #endregion
    }
}
