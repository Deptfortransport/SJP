// *********************************************** 
// NAME             : ContentProvider.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Allows interaction with the database containing language/group specific page content
// ************************************************


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.ResourceManager
{
    /// <summary>
    /// Singleton class to encapsulate interaction with the content database.
    /// </summary>
    public sealed class ContentProvider
    {
        #region Private Enums

        /// <summary>
        /// Enum to interact with the data reader containing content. Enum removes the 
        /// need for "magic numbers" in the code, while preventing the performance 
        /// problems associated with accessing fields by string name.
        /// </summary>
        private enum DataReaderFieldIndex
        {
            ControlName = 0,
            PropertyName = 1,
            Value = 2,
        }

        #endregion

        #region Private Static Fields

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static ContentProvider instance;
        /// <summary>
        /// Lock to allow thread safe interaction with singleton instance.
        /// </summary>
        private static readonly object instanceLock = new object();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Property to allow singleton instance to be obtained. Thread safe.
        /// </summary>
        public static ContentProvider Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ContentProvider();
                    }

                    return instance;
                }
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Given a group name, and language, a list of  sql parameter 
        /// details needed to make the correct database call (to retrieve content) 
        /// is returned. 
        /// </summary>
        /// <param name="groupName">The group name of the set of content being queried</param>
        /// <param name="language">The language being queried</param>
        /// <returns>A List of SQL parameters needed to perform the query</returns>
        private static List<SqlParameter> GetParameters(string groupName, Language language)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
                        
            parameters.Add(new SqlParameter("@Language", LanguageHelper.GetLanguageString(language)));
            parameters.Add(new SqlParameter("@Group", groupName));
                        
            return parameters;
        }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Given a group name, language and request Uri, a collection of relavent 
        /// content (in the form of control names and properties) is returned.
        /// </summary>
        /// <param name="groupName">The name of the group to retrieve content for</param>
        /// <param name="language">The language of the returned content</param>
        /// <returns></returns>
        internal static ControlPropertyCollection GetControlPropertyCollection(string groupName, Language language)
        {
            //The following field looks overly complex! However, it is required to
            //be this way. See notes in the class ControlPropertyCollection for
            //an explanation:
            Dictionary<string, Dictionary<string, string>> contentDictionary = new Dictionary<string, Dictionary<string, string>>();
            SqlHelper sqlHelper = null;
            SqlDataReader reader = null;

            try
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                        string.Format("ContentProvider - Loading content for Group[{0}] and Language[{1}]", groupName, language.ToString())));

                using (sqlHelper = new SqlHelper())
                {
                    List<SqlParameter> parameters = GetParameters(groupName, language);

                    sqlHelper.ConnOpen(SqlHelperDatabase.ContentDB);

                    using (reader = sqlHelper.GetReader("GetContent", parameters))
                    {
                        IEnumerator readerEnumerator = reader.GetEnumerator();

                        //Note that the reader returns three fields, 
                        //ControlName, PropertyName and Value. These are described 
                        //in the enum DataReaderFieldIndex to avoid 'magic numbers'
                        //in the code.
                        while (reader.Read())
                        {
                            //Note that we convert keys to lower case:
                            string controlName = reader.GetString((int)DataReaderFieldIndex.ControlName).ToLower();
                            string propertyName = reader.GetString((int)DataReaderFieldIndex.PropertyName).ToLower();
                            string value = reader.GetString((int)DataReaderFieldIndex.Value);

                            //Check to see if information about the current control 
                            //exists. If not, create a new dictionary.
                            if (!contentDictionary.ContainsKey(controlName))
                            {
                                contentDictionary.Add(controlName, new Dictionary<string, string>());
                            }

                            //Add the control property and value.
                            contentDictionary[controlName].Add(propertyName, value);
                        }
                    }
                }

                return new ControlPropertyCollection(contentDictionary);
            }
            catch (Exception ex)
            {
                // Log exceptions
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                        string.Format("ContentProvider - Error occurred loading content for Group[{0}] and Language[{1}]. Error[{2}]", 
                        groupName, language.ToString(), ex.Message), ex));

                throw;
            }
            finally
            {
                sqlHelper = null;
                reader = null;
            }
        }

        #endregion

        #region Private Fields

        //Dictionary to hold all content providers for each group:
        private readonly Dictionary<string, ContentGroup> contentGroupProviders = new Dictionary<string, ContentGroup>();

        //Used for data change notification
        private const string DataChangeNotificationGroup = "Content";
        private bool receivingChangeNotifications;

        #endregion

        #region Private Constructor

        /// <summary>
        /// Private constructor required by singleton implementation
        /// </summary>
        private ContentProvider()
        {
            // Setup data change notification
            receivingChangeNotifications = RegisterForChangeNotification();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers an event handler with the data change notification service
        /// </summary>
        private bool RegisterForChangeNotification()
        {
            IDataChangeNotification notificationService;
            try
            {
                notificationService = SJPServiceDiscovery.Current.Get<DataChangeNotification>(ServiceDiscoveryKey.DataChangeNotification);
            }
            catch (SJPException e)
            {
                // If the SDInvalidKey exception is thrown, return false as the notification service
                // hasn't been initialised.
                // Otherwise, rethrow the exception that was received.
                if (e.Identifier == SJPExceptionIdentifier.SDInvalidKey)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Warning,
                        "ContentProvider - DataChangeNotification service was not present when initialising"));
                    return false;
                }
                else
                    throw;
            }
            catch
            {
                throw;
            }

            notificationService.Changed += new ChangedEventHandler(this.DataChangedNotificationReceived);
            return true;
        }

        #endregion

        #region Event handler

        /// <summary>
        /// Used by the Data Change Notification service to clear the data if it is changed in the DB
        /// </summary>
        private void DataChangedNotificationReceived(object sender, ChangedEventArgs e)
        {
            if (e.GroupId == DataChangeNotificationGroup)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info,
                        "ContentProvider - Clearing cached content following event raised by data change notification service"));

                // Clear the data for each content group
                foreach (ContentGroup contentGroup in contentGroupProviders.Values)
                {
                    contentGroup.ClearControlProperties();
                }
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Allows content for a particular group to be obtained.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public ContentGroup this[string groupName]
        {
            get
            {
                //Groups are loaded on demand:
                if (!contentGroupProviders.ContainsKey(groupName))
                {
                    contentGroupProviders.Add(groupName, new ContentGroup(groupName));
                }

                return contentGroupProviders[groupName];
            }
        }

        #endregion
    }
}
