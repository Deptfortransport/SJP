// *********************************************** 
// NAME             : DataServices.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Apr 2011
// DESCRIPTION  	: DataServices helps with cache property driven query data loads
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SJP.Common.ResourceManager;
using SJP.Common.EventLogging;
using System.Diagnostics;
using SJP.Common.DatabaseInfrastructure;
using System.Data.SqlClient;
using SJP.Common.PropertyManager;
using System.Globalization;
using System.Web.UI.WebControls;

namespace SJP.Common.DataServices
{
    /// <summary>
    /// DataServices helps with cache property driven query data loads
    /// </summary>
    public class DataServices : IDataServices
    {
        #region Private Static Fields
        /// <summary>
        /// Maintains a flag indicating of cache has been loaded.
        /// </summary>
        private static volatile bool CacheIsLoaded = false;

        private static object cacheLockObj = new object();

        /// <summary>
        /// Holds dataset ID as key and hash/array as value.
        /// </summary>
        private static Dictionary<DataServiceType,ICollection> cache = null;
        #endregion



        #region Constructor
        /// <summary>
        /// Loads the cache if not already loaded in class variables.
        /// </summary>
        public DataServices()
        {
            if (CacheIsLoaded == false)
            {
               
                lock (cacheLockObj)  // Lock DataServices type at this stage.
                {
                    // Test again in case it got loaded when lock was acquired.
                    if (CacheIsLoaded == false)
                    {
                        cache = new Dictionary<DataServiceType,ICollection>();
                        CacheIsLoaded = true;
                        LoadDataCache();
                    }
                } // lock
            } // If cache is not loaded
        }
        #endregion

        #region Public Interface
        /// <summary>
        /// Returns an Array-List type object from cache, performs type checking.
        /// </summary>
        /// <param name="item">Enumeration must refer to Array-List type.</param>
        /// <returns>Structure could contain a list of date and dropdown items.</returns>
        public ArrayList GetList(DataServiceType item)
        {
            object o = cache[item];
            if (o.GetType() != typeof(ArrayList)) // Check type for completeness
            {
                string message = "Illegal type, GetList";
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error, message
                    );
                Trace.Write(oe);
                throw new SJPException(message, false, SJPExceptionIdentifier.DSMethodIllegalType);
            }
            else
                return (ArrayList)((ArrayList)o).Clone(); // Shallow copy.
        }

        /// <summary>
        /// Method that populates a list control with items using dataservices.
        /// </summary>
        /// <param name="dataSet">The dataset that contains dropdown items.</param>
        /// <param name="control">The control, eg DropDownList, to be populated.</param>
        /// <param name="rm">The resource manager to use for looking up lang strings</param>
        public void LoadListControl(DataServiceType dataSet, ListControl control, SJPResourceManager rm, Language language)
        {
            ArrayList list;

            object o = cache[dataSet];
            if (o.GetType() != typeof(ArrayList)) // Check type for completeness
            {
                string message = "Illegal type, GetList";
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error, message
                    );
                Trace.Write(oe);
                throw new SJPException(message, false, SJPExceptionIdentifier.DSMethodIllegalType);
            }

            list = (ArrayList)o;

            string temp; DSDropItem itemCache;


            control.Items.Clear();
            temp = "DataServices." + dataSet.ToString() + ".";

            for (int i = 0; i < list.Count; i++)
            {
                ListItem itemWeb = new ListItem();
                itemCache = (DSDropItem)list[i];

                // Get corresponding text from resource content.
                itemWeb.Text = rm.GetString(language, temp + itemCache.ResourceID);

                itemWeb.Value = itemCache.ItemValue;

                control.Items.Add(itemWeb); // Append this item to the list.

                if (itemCache.IsSelected)
                {
                    if (control is DropDownList)
                    {
                        ((DropDownList)control).SelectedIndex = i;
                    }
                    else if (control is RadioButtonList)
                    {
                        ((RadioButtonList)control).SelectedIndex = i;
                    }
                    else
                    {
                        itemWeb.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the display text for a data service type value
        /// </summary>
        public string GetText(DataServiceType type, string value, SJPResourceManager rm, Language language)
        {
            ArrayList list = GetList(type);

            string temp = "DataServices." + type.ToString() + ".";

            foreach (DSDropItem item in list)
            {
                if (item.ItemValue == value)
                {
                    return rm.GetString(language, temp + item.ResourceID);
                }
            }

            return string.Empty;
        }

        #endregion

        #region Private Methods

        #region Load Data Cache
        /// <summary>
        /// Loads the datacache from databases.
        /// </summary>
        private void LoadDataCache()
        {
            using (SqlHelper sql = new SqlHelper())
            {
                SqlDataReader reader;

                IPropertyProvider currProps = Properties.Current;

                int i, max = (int)DataServiceType.DataServiceTypeEnd;

                String parType, parDB, parQuery, parTemp, previousDB = "~#@|<>,.";

                for (i = 0; i < max; i++)
                {
                    parTemp = "SJP.UserPortal.DataServices." + ((DataServiceType)i).ToString();

                    if (SJPTraceSwitch.TraceVerbose)
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure,
                            SJPTraceLevel.Verbose, ((DataServiceType)i).ToString()));

                    parType = currProps[parTemp + ".type"];
                    parDB = currProps[parTemp + ".db"];
                    parQuery = currProps[parTemp + ".query"];

                    // Ignore nulls
                    if ((parType == null) || (parDB == null) || (parQuery == null))
                    {
                        string message = "Parameter contains nulls for " + parTemp;
                        OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                            SJPTraceLevel.Warning, message);
                        Trace.Write(oe);
                        continue;
                    }

                    // Re-open the database if it is different from previous.
                    if (String.Compare(previousDB, parDB, true /* cast-insensitive */, new CultureInfo("en-GB")) != 0)
                    {
                        SqlHelperDatabase db = FindDatabase(parDB);

                        try // On error log then throw.
                        {
                            if (sql.ConnIsOpen)
                                sql.ConnClose();
                            sql.ConnOpen(db);

                            if (SJPTraceSwitch.TraceVerbose)
                                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure,
                                    SJPTraceLevel.Verbose, "Accissing " + db.ToString()));
                            previousDB = parDB;
                        }
                        catch (SqlException eSQL)
                        {
                            string message = "Error accessing database";
                            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Error, message
                                                            );
                            Trace.Write(oe);
                            throw new SJPException(message, eSQL, false, SJPExceptionIdentifier.DSDataAccessError);
                        }
                    } // if the database name is different.

                    // Execute query and read data.
                    try // On error log then throw.
                    {
                        ICollection dataset; reader = sql.GetReader(parQuery);

                        switch (parType)
                        {
                            case "1": dataset = ReadList(reader); break;
                            case "2": dataset = ReadDateList(reader); break;
                            case "3": dataset = ReadDrop(reader); break;

                            default:
                                string message = "Type is unknown";
                                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                                    SJPTraceLevel.Error, message
                                                        );
                                Trace.Write(oe);
                                throw new SJPException(message, false, SJPExceptionIdentifier.DSUnknownType);
                        }

                        reader.Close();
                        cache.Add((DataServiceType)i, dataset);

                        if (SJPTraceSwitch.TraceVerbose)
                            Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure,
                                SJPTraceLevel.Verbose, "   Size " + dataset.Count.ToString()));
                    }
                    catch (SqlException eSQL)
                    {
                        string message = "Error executing query";
                        OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                            SJPTraceLevel.Error, message
                                        );
                        Trace.Write(oe);
                        throw new SJPException(message, eSQL, false, SJPExceptionIdentifier.DSQueryExecuteError);
                    }
                } // For each enum

                if (sql.ConnIsOpen)
                    sql.ConnClose();
            }
        }

       
        #endregion

        #region Collection Readers
        /// <summary>
        /// Reads Array-List data for the cache.
        /// </summary>
        /// <param name="reader">Contains items to read.</param>
        /// <returns>Oject to be appended to the list of cached items.</returns>
        private static ICollection ReadList(SqlDataReader reader)
        {
            ArrayList arrayList = new ArrayList(); string temp;

            while (reader.Read())
            {
                temp = reader.GetString(0).Trim();
                arrayList.Add(temp);
            }
            return arrayList;
        }

        /// <summary>
        /// Reads a table with date values in to an array list
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private ICollection ReadDateList(SqlDataReader reader)
        {
            ArrayList arrayList = new ArrayList(); 
            DateTime temp;

            while (reader.Read())
            {
                temp = reader.GetDateTime(0);
                arrayList.Add(temp);
            }
            return arrayList;
        }

        /// <summary>
        /// Reads data for the dropdown-list for the cache.
        /// </summary>
        /// <param name="reader">Contains items to read.</param>
        /// <returns>List of DSDropItem Oject to be appended to the list of cached items.</returns>
        private static ICollection ReadDrop(SqlDataReader reader)
        {
            string resourceID, itemValue; 
            bool isSelected;
            ArrayList itemList = new ArrayList();

            while (reader.Read())
            {
                resourceID = reader.GetString(0).Trim();
                itemValue = reader.GetString(1);
                isSelected = reader.GetInt32(2) != 0;

                if (itemValue != null)
                    itemValue = itemValue.Trim();
        
                DSDropItem item = new DSDropItem(
                    resourceID, itemValue, isSelected
                        );

                itemList.Add(item);
            }

            return itemList;
        }

        #endregion

        #region Find Database
        /// <summary>
        /// Converts a string to SqlHelperDatabase enumeration value.
        /// </summary>
        /// <param name="DBName"></param>
        /// <returns></returns>
        private static SqlHelperDatabase FindDatabase(string DBName)
        {
            int i, db = -1, max = (int)SqlHelperDatabase.SqlHelperDatabaseEnd;

            for (i = 0; i < max; i++) // Find matching enum strings
            {
                if (String.Compare(((SqlHelperDatabase)i).ToString(), DBName, true, new CultureInfo("en-GB")) == 0)
                {
                    db = i;
                    break;
                }
            } // for each db in SqlHelper

            if (db == -1)
            {
                string message = "SqlHelper database not found";
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure,
                    SJPTraceLevel.Error, message);
                Trace.Write(oe);
                throw new SJPException(message, false, SJPExceptionIdentifier.DSSQLHelperDatabaseNotFound);
            }

            return (SqlHelperDatabase)db;
        }

        #endregion

        #endregion
    }
}
