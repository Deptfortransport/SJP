// *********************************************** 
// NAME             : StopAccessibilityLinks.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: StopAccessibilityLinks class for loading the stop accessibility links
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.DataServices.StopAccessibilityLinks
{
    /// <summary>
    /// StopAccessibilityLinks class for loading the stop accessibility links
    /// </summary>
    public class StopAccessibilityLinks
    {
        #region Private members

        // Stores StopAccessibilityLink data, in list because multiple links can exist for a naptan, with differing
        // with effect from and until dates
        private Dictionary<string, List<StopAccessibilityLink>> stopAccessibilityLinksCache = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public StopAccessibilityLinks()
        {
            LoadData();
        }

        #endregion

        #region Private methods
       
        /// <summary>
        /// Loads the data and performs pre processing
        /// </summary>
        private void LoadData()
        {
            Dictionary<string, List<StopAccessibilityLink>> tmpStopAccessibilityLinksCache = new Dictionary<string, List<StopAccessibilityLink>>();
            
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    // Initialise a SqlHelper and connect to the database.
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info, "Loading StopAccessibilityLinks data started"));
                   
                    helper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    #region Load data

                    //Retrieve the reference data into a flat table of results
                    SqlDataReader reader = helper.GetReader("GetStopAccessibilityLinks", new List<SqlParameter>());

                    while (reader.Read())
                    {
                        //Read in each StopAccessibilityLink
                        string stopNaPTAN = reader.GetString(reader.GetOrdinal("StopNaPTAN"));

                        string stopOperator = reader.IsDBNull(reader.GetOrdinal("StopOperator")) ?
                            string.Empty : reader.GetString(reader.GetOrdinal("StopOperator"));
                        string stopAccessbilityURL = reader.IsDBNull(reader.GetOrdinal("LinkUrl")) ?
                            string.Empty : reader.GetString(reader.GetOrdinal("LinkUrl"));
                        DateTime dateWEF = reader.IsDBNull(reader.GetOrdinal("WEFDate")) ?
                            DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("WEFDate"));
                        DateTime dateWEU = reader.IsDBNull(reader.GetOrdinal("WEUDate")) ?
                            DateTime.MaxValue : reader.GetDateTime(reader.GetOrdinal("WEUDate"));

                        StopAccessibilityLink sal = new StopAccessibilityLink(
                            stopNaPTAN.Trim().ToUpper(),
                            stopOperator.Trim().ToUpper(), 
                            stopAccessbilityURL, 
                            dateWEF, dateWEU);

                        // Add to the local cache
                        if (!tmpStopAccessibilityLinksCache.ContainsKey(stopNaPTAN))
                        {
                            tmpStopAccessibilityLinksCache.Add(stopNaPTAN, new List<StopAccessibilityLink>());
                        }

                        tmpStopAccessibilityLinksCache[stopNaPTAN].Add(sal);
                    }

                    reader.Close();

                    #region Update the cahce
                    
                    // Replace the cache with the new dictionary
                    stopAccessibilityLinksCache = tmpStopAccessibilityLinksCache;
                                                            
                    #endregion

                    #endregion

                    // Record the fact that the data was loaded.
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose, 
                        string.Format("StopAccessibilityLinks data loaded into cache: Links[{0}]", stopAccessibilityLinksCache.Count)));

                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose, "Loading StopAccessibilityLinks data completed"));

                }
                catch (Exception e)
                {
                    // Catching the base Exception class because we don't want any possibility
                    // of this raising any errors outside of the class in case it causes the
                    // application to fall over. As long as the exception doesn't occur in 
                    // the final block of code, which copies the new data into the module-level
                    // hashtables and arraylists, the object will still be internally consistant,
                    // although the data will be inconsistant with that stored in the database.
                    // One exception to this: if this is the first time LoadData has been run,
                    // the exception should be raised.
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "An error occurred whilst attempting to load the StopAccessibilityLinks data.", e));
                    if ((stopAccessibilityLinksCache == null) || (stopAccessibilityLinksCache.Count == 0))
                    {
                        throw;
                    }
                }
                finally
                {
                    //close the database connection
                    if (helper.ConnIsOpen)
                        helper.ConnClose();
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the accessibility URL for the requested NaPTAN and Operator code, and the date to be shown for
        /// </summary>
        /// <returns>URL as string applicable for the date specified, empty is returned if none found</returns>
        public string GetAccessibilityURL(string naptan, string operatorCode, DateTime date)
        {
            string result = string.Empty;

            #region Validate inputs

            // Check and update naptan
            if (string.IsNullOrEmpty(naptan))
            {
                return result;
            }
            else
            {
                naptan = naptan.Trim().ToUpper();
            }

            // Check operator code was supplied
            if (operatorCode == null)
            {
                operatorCode = string.Empty;
            }
            else
            {
                operatorCode = operatorCode.Trim().ToUpper();
            }

            #endregion

            // Check cache was loaded (may have errored during initialisation,
            if (stopAccessibilityLinksCache != null)
            {
                // Check cache contains the naptan
                if (stopAccessibilityLinksCache.ContainsKey(naptan))
                {
                    List<StopAccessibilityLink> links = stopAccessibilityLinksCache[naptan];

                    foreach (StopAccessibilityLink link in links)
                    {
                        if (link.IsValidForDate(date))
                        {
                            // No need to match operator code anymore
                            result = link.StopAccessibilityURL;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
