// *********************************************** 
// NAME             : RetailerCatalogue.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Mar 2011
// DESCRIPTION  	: RetailerCatalogue class implements a catalogue that holds 
// details of retailers capable of selling tickets for a given operator and mode of transport. 
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// RetailerCatalogue class implements a catalogue that holds 
    /// details of retailers capable of selling tickets for a given operator and mode of transport
    /// </summary>
    public class RetailerCatalogue : IRetailerCatalogue
    {
        #region Private members

        /// <summary>
        /// Table of retailers. Key is instance of RetailerKey, value is instance of Retailer.
        /// </summary>
        private Dictionary<RetailerKey, IList<Retailer>> retailerLookUp;

        /// <summary>
        /// Table of retailers. Key is retailer id, value is instance of Retailer.
        /// </summary>
        private Dictionary<string, Retailer> retailers;

        /// <summary>
        /// Name of the stored procedure that returns the retailers table
        /// </summary>
        private const string retailerTableStoredProc = "GetRetailers";

        /// <summary>
        /// Name of the stored procedure that returns the retailers lookup and partner relations table
        /// </summary>
        private const string retailerLookupTableStoredProc = "GetRetailerLookup";

        /// <summary>
        /// Value used to identify test retailers and load if property switch set
        /// </summary>
        private const string TestRetailer_Prefix = "TEST";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Clients should not use this constructor to create instances.
        /// Instead the Current property should be used to obtain a singleton instance.
        /// </summary>
        public RetailerCatalogue()
        {
            retailerLookUp = new Dictionary<RetailerKey, IList<Retailer>>();
            retailers = new Dictionary<string, Retailer>();
            
            LoadData();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns an immutable arraylist containing all Retailer instances
        /// </summary>
        /// <return>An IList of Retailer instances</return>
        public IList<Retailer> GetRetailers()
        {
            IList<Retailer> listRetailers = new List<Retailer>(retailers.Values);

            return listRetailers;
        }

        /// <summary>
        /// Returns an immutable arraylist containing 1 or more Retailer instances that are capable
        /// of selling tickets for the suppplied operator code and travel mode.
        /// Null is returned if no retailers are found or an invalid combination of parameters
        /// is used.
        /// </summary>
        /// <param name="operatorCode">The operator code. A null value indicates the operator
        /// code is not significant</param>
        /// <param name="mode">The mode of transport</param>
        /// <return>An IList of Retailer instances</return>
        public IList<Retailer> FindRetailers(string operatorCode, SJPModeType mode)
        {
            if (string.IsNullOrEmpty(operatorCode))
            {
                operatorCode = RetailerKey.IgnoreOperatorCode;
            }
            
            // In some cases the CJP returns operator codes with leading or 
            // trailing ":" and "*" characters. These are not included in the RetailerCatalogue
            // so need to be removed before we lookup the operator.
            if (operatorCode.StartsWith("*") || operatorCode.StartsWith(":"))
            {
                operatorCode = operatorCode.Substring(1);
            }
            
            if (operatorCode.EndsWith("*") || operatorCode.EndsWith(":"))
            {
                operatorCode = operatorCode.Substring(0, operatorCode.Length - 1);
            }
            
            RetailerKey key = new RetailerKey(operatorCode, mode);

            if (retailerLookUp.ContainsKey(key))
            {
                return retailerLookUp[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a retailer for the supplied id
        /// </summary>
        /// <param name="id">The id of the required retailer</param>
        /// <return>A retailer for the supplied id or null if not found</return>
        public Retailer FindRetailer(string id)
        {
            if (!string.IsNullOrEmpty(id) && retailers.ContainsKey(id))
            {
                return retailers[id];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Loads the database tables that constitute the retailer catalogue and builds the
        /// retailerLookUp dictionary. 
        /// This method reads all rows from the Retailers and RetailerLookup tables and constructs
        /// a mapping in the retailerLookUp dictionary.
        /// Each unique operator code and mode combination from the RetailerLookup table represents
        /// an entry in retailerLookUp. The value for each key (instance of RetailerKey)
        /// is a list containing 1 or more Retailer instances. This represents the n:m relationship
        /// between a single key and many retailers. One instance of Retailer will exist for each
        /// row in the Retailer table and will potentially be referenced from 1 or more lists. 
        /// </summary>
        private void LoadData()
        {
            SqlDataReader dataReader = null;
            
            // Temp dictionary
            Dictionary<RetailerKey, List<Retailer>> tmpRetailerLookup = new Dictionary<RetailerKey, List<Retailer>>();
                                    
            try
            {
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    // Determine whether to show test retailers
                    bool loadTestRetailers = Properties.Current["Retail.Retailers.ShowTestRetailers.Switch"].Parse(false);

                    sqlHelper.ConnOpen(SqlHelperDatabase.DefaultDB);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                        "RetailerCatalogue - Loading retailer data"));

                    // Load all retailers
                    LoadRetailers(sqlHelper, loadTestRetailers);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                        "RetailerCatalogue - Loading retailer lookup data"));

                    // Load lookup table
                    dataReader = sqlHelper.GetReader(retailerLookupTableStoredProc, new List<SqlParameter>());

                    while (dataReader.Read())
                    {
                        // Read values
                        string operCode = dataReader.GetString(0).Trim();
                        string mode = dataReader.GetString(1).Trim();
                        string retailerId = dataReader.GetString(2).Trim();
                        SJPModeType sjpModeType = (SJPModeType)Enum.Parse(typeof(SJPModeType), mode);
                        RetailerKey key = new RetailerKey(operCode.Trim(), sjpModeType);

                        // Only load the test retailers if switch set 
                        if ((!retailerId.ToUpper().StartsWith(TestRetailer_Prefix))
                            ||
                            (retailerId.ToUpper().StartsWith(TestRetailer_Prefix) && loadTestRetailers)
                           )
                        {
                            // If this is the first occurrence of this key, create a new
                            // list to hold the retailers
                            if (!tmpRetailerLookup.ContainsKey(key))
                            {
                                tmpRetailerLookup.Add(key, new List<Retailer>());
                            }

                            // Update retailer modes
                            retailers[retailerId].Modes.Add(sjpModeType);

                            // Add the retailer object to the lookup list 
                            tmpRetailerLookup[key].Add(retailers[retailerId]);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                string message = string.Format("SqlException during RetailerCatalogue load: {0}",sqlEx.Message);
                
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));
                
                throw new SJPException(message, sqlEx, true, SJPExceptionIdentifier.RERetailerCatalogueSQLCommandFailed);
            }
            finally
            {
                // Release database resources
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }

            // Convert all lists in lookup table to immutable lists
            foreach (RetailerKey retailerKey in tmpRetailerLookup.Keys)
            {
                IList<Retailer> retailerList = tmpRetailerLookup[retailerKey].AsReadOnly();
                                
                retailerLookUp.Add(retailerKey, retailerList);               
            }
        }

        /// <summary>
        /// Loads the contents of the Retailers table into the dictionary.
        /// </summary>
        /// <param name="SqlHelper">Database connection object for open connection</param>
        private void LoadRetailers(SqlHelper sqlHelper, bool loadTestRetailers)
        {
            SqlDataReader dataReader = null;
            try
            {
                // Execute stored proc
                dataReader = sqlHelper.GetReader(retailerTableStoredProc, new List<SqlParameter>());

                while (dataReader.Read())
                {
                    // Parse the retailers returned
                    string id = dataReader["RetailerId"].ToString().Trim();
                    string name = dataReader["Name"].ToString().Trim();
                    string websiteURL = string.Empty;
                    string handoffURL = string.Empty;
                    string displayURL = string.Empty;
                    string phoneNumber = string.Empty;
                    string phoneNumberDisplay = string.Empty;
                    string resourceKey = string.Empty;
                    
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("WebsiteURL")))
                    {
                        websiteURL = dataReader["WebsiteURL"].ToString().Trim();
                    }
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("HandoffURL")))
                    {
                        handoffURL = dataReader["HandoffURL"].ToString().Trim();
                    }
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("DisplayURL")))
                    {
                        displayURL = dataReader["DisplayURL"].ToString().Trim();
                    }
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("PhoneNumber")))
                    {
                        phoneNumber = dataReader["PhoneNumber"].ToString().Trim();
                    }
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("PhoneNumberDisplay")))
                    {
                        phoneNumberDisplay = dataReader["PhoneNumberDisplay"].ToString().Trim();
                    }
                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("ResourceKey")))
                    {
                        resourceKey = dataReader["ResourceKey"].ToString().Trim();
                    }

                    // LoadData() will update modes
                    Retailer retailer = new Retailer(id, new List<SJPModeType>(), name, websiteURL, handoffURL, displayURL, phoneNumber, phoneNumberDisplay, resourceKey);

                    // Only load the test retailers if switch set 
                    if ((!retailer.Id.ToUpper().StartsWith(TestRetailer_Prefix))
                        ||
                        (retailer.Id.ToUpper().StartsWith(TestRetailer_Prefix) && loadTestRetailers)
                       )
                    {
                        // Add retailer to the dictionary
                        retailers.Add(id, retailer);

                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                            string.Format("RetailerCatalogue - Retailer cached: {0}", retailer.ToString())));
                    }
                }
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        #endregion
    }
}
