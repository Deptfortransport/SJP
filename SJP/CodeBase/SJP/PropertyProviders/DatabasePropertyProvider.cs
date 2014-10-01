// *********************************************** 
// NAME             : DatabasePropertyProvider.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Property provider implementation to get properties stored in database
// ************************************************
// 


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using SJP.Common.EventLogging;

using Logger = System.Diagnostics.Trace;

namespace SJP.Common.PropertyManager.PropertyProviders
{
    /// <summary>
    /// Property provider implementation to get properties stored in database
    /// </summary>
    class DatabasePropertyProvider: Properties
    {
        #region Private Fields
        private int INITIAL_VERSION = -1;

        private int KEY = 0;
        private int VALUE = 1;
       
        private string PROP_APPID = "propertyservice.applicationid";
        private string PROP_GRPID = "propertyservice.groupid";
        private string PROP_VERSION = "propertyservice.version";
        
        private string PROP_CONSTR = "propertyservice.providers.databaseprovider.connectionstring";

        private string SPROC_SELAPP = "SelectApplicationProperties";
        private string SPROC_SELGRP = "SelectGroupProperties";
        private string SPROC_SELGLB = "SelectGlobalProperties";
        private string SPROC_GETVER = "GetVersion";

        private string AID = "@AID";
        private string GID = "@GID";

        #endregion

        #region Private variables
        /// <summary>
        /// The connection string in unencrypted form
        /// </summary>
        private string connectionString;
       
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a database property provider which reads its initial settings from App settings.
        /// It will also load the decryption engine from service discovery and use that to decrypt
        /// the connection string if nessecary.
        /// </summary>
        public DatabasePropertyProvider()
        {
            // Read General ConfigurationSettings (AppID Group ID property assembly and class)
            strApplicationID = ConfigurationManager.AppSettings[PROP_APPID];
            strGroupID = ConfigurationManager.AppSettings[PROP_GRPID];

            // Initialise property dictionary.
            propertyDictionary = new Dictionary<string,string>();
            
            // Get connection string
            connectionString = ConfigurationManager.ConnectionStrings[PROP_CONSTR].ConnectionString;
            
            // Ensure initial version
            intVersion = INITIAL_VERSION;

        }
        #endregion

        #region Public methods
        /// <summary>
        /// Checks if there is a newer version of the properties available.
        /// </summary>
        public bool IsNewVersion()
        {
            // create the db connection object
            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                SqlDataReader dr = null;
                // Initialise the sql command
                using (SqlCommand cm = new SqlCommand())
                {
                    try
                    {

                        cm.Connection = dbConnection;

                        cm.CommandText = SPROC_GETVER;

                        cm.CommandType = CommandType.StoredProcedure;

                        // Open the connection

                        dbConnection.Open();
                        // Execute the command
                        dr = cm.ExecuteReader(CommandBehavior.CloseConnection);

                        // read the current version - this should _always_ exist
                        dr.Read();

                        // transform value to int
                        int currentVersion = Convert.ToInt32(dr.GetString(0), CultureInfo.CurrentCulture.NumberFormat);

                        // If value differs from what we previously had - there is a new version available
                        if (currentVersion != Version)
                            return true;
                        else
                            return false;
                    }
                    catch (InvalidOperationException e)
                    {
                        OperationalEvent oe =
                            new OperationalEvent(SJPEventCategory.Database,
                            SJPTraceLevel.Error,
                            "An error occurred accessing the Version property in the Properties Database" + e.Message);
                        Logger.Write(oe);
                        throw new SJPException(
                            "Exception trying to get the version property from property Database: " + e.Message,
                            true,
                            SJPExceptionIdentifier.PSInvalidVersion);
                    }
                    catch (SqlException e)
                    {
                        OperationalEvent oe =
                            new OperationalEvent(SJPEventCategory.Database,
                            SJPTraceLevel.Error,
                            "An error has occurred accessing the Version property in the Properties Database :- " + e.Message);
                        Logger.Write(oe);
                        throw new SJPException(
                            "Exception trying to get the version property from property Database: " + e.Message,
                            true,
                            SJPExceptionIdentifier.PSInvalidVersion);
                    }
                }
            }

        }

        /// <summary>
        /// Asks the property to either populate itself or check if there is new properties 
        /// and return a new instance. It is up to the called of this method to ensure that
        /// the old property provider properly gets superseeded.
        /// </summary>
        /// <returns>Null if no new properties exists, or a property provider if new properties is available.</returns>
        public override IPropertyProvider Load()
        {
            try
            {
                if (Version == INITIAL_VERSION)
                {
                    // For initial version, only need to populate my own properties
                    PopulateProperties();
                    // return myself
                    return this;
                }
                else if (IsNewVersion())
                {
                    // if there is a new version in the database - create new instance 
                    DatabasePropertyProvider newInstance = new DatabasePropertyProvider();
                    // Ensure new instance loads itself and return its prefered provider (itself in this case)
                    return newInstance.Load();
                }
            }
            catch (SqlException e)
            {
                string message = "An SQL error has occurred manipulating the Properties Database: SQLNUM=" + e.Number + " : Message " + e.Message;
                OperationalEvent oe =
                    new OperationalEvent(SJPEventCategory.Database,
                    SJPTraceLevel.Error,
                    message);
                Logger.Write(oe);

                throw new SJPException(
                    message,
                    true,
                    SJPExceptionIdentifier.PSDatabaseFailure);
            }
            catch (FormatException e)
            {
                OperationalEvent oe =
                    new OperationalEvent(SJPEventCategory.Database,
                    SJPTraceLevel.Error,
                    "An error has occurred manipulating the Properties Database");
                Logger.Write(oe);
                throw new SJPException(
                    "Exception trying to manipulate the Properties Database" + e.Message,
                    true,
                    SJPExceptionIdentifier.PSDatabaseFailure);
            }
            catch (InvalidOperationException e)
            {

                OperationalEvent oe =
                    new OperationalEvent(SJPEventCategory.Database,
                    SJPTraceLevel.Error,
                    "An error has occurred manipulating the Properties Database");
                Logger.Write(oe);
                throw new SJPException(
                    "Exception trying to manipulate the Properties Database" + e.Message,
                    true,
                    SJPExceptionIdentifier.PSDatabaseFailure);
            }

            // If no changes are needed - just return null
            return null;

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Populates the property lists with a new set of properties.
        /// It ensures that application properties are handled first, group secondly and the global ones last.
        /// </summary>
        private void PopulateProperties()
        {
            using(SqlConnection dbConnection = new SqlConnection(connectionString))
            {
           
                // Open connection - connection  is open while all the properties are being fetched.
                dbConnection.Open();

                // ----- Get Application Properties ------
                AddAll(SPROC_SELAPP, dbConnection, CreateParam(AID, ApplicationID));
                // ---- End Get Application properties	

                // ----- Get Group Properties --------
                AddAll(SPROC_SELGRP, dbConnection, CreateParam(GID, GroupID));
                // ------ End Get Group Properties ---------

                // ------ Get Global properties ------------
                AddAll(SPROC_SELGLB, dbConnection, null);
                // ------ End Global properties ------------

                // Populate Version
                intVersion = Convert.ToInt32(this[PROP_VERSION], CultureInfo.InvariantCulture.NumberFormat);

            }
            
        }

        /// <summary>
        /// Adds all properties (if they are unset previously) to the property table.
        /// All properties are decrypted and added to the decryption list before being added.
        /// </summary>
        /// <param name="storedProc">The stored procedure to call</param>
        /// <param name="con">The connection to use</param>
        /// <param name="param">An SQL parameter that should be included (or null)</param>
        private void AddAll(string storedProc, SqlConnection con, SqlParameter param)
        {
            //We need an exception to handle the content database:


            // The data reader
            SqlDataReader dr = null;
            try
            {
                // Execute the correct stored procedure
                dr = ExecuteCommand(storedProc, con, param);

                // For every returned answer
                while (dr.Read())
                {
                    // retrive key
                    string key = dr.GetString(KEY);
                    // retrive value
                    string val = dr.GetString(VALUE);

                    if (!propertyDictionary.ContainsKey(key))
                    {

                        propertyDictionary.Add(key, val);

                    }
                }
            }            
            finally
            {
                // If we have a data reader
                if (dr != null)
                {
                    // close it
                    dr.Close();
                }
            }

        }

        /// <summary>
        /// Creates a SQL Parameter with the given name and value.
        /// It will only create parameters with the NVarChar type and
        /// read only.
        /// </summary>
        /// <param name="name">The name of the parameter, ie "@GID"</param>
        /// <param name="val">The value of the parameter</param>
        /// <returns>A constructed SqlParameter object</returns>
        private SqlParameter CreateParam(string name, string val)
        {
            SqlParameter sp = new SqlParameter(name, SqlDbType.NVarChar);
            sp.Direction = ParameterDirection.Input;
            sp.Value = val;
            return sp;
        }

        /// <summary>
        /// Executes a stored procedure and returns the appropriate reader from that
        /// procedure.
        /// </summary>
        /// <param name="storedProc">The name of the procedure</param>
        /// <param name="con">The connection to use</param>
        /// <param name="param">The sql parameter if such exists</param>
        /// <param name="requestUrl">The requested url, used to deduce the theme/partner</param>
        /// <returns>A Sql data reader</returns>
        private SqlDataReader ExecuteCommand(string storedProc, SqlConnection con, SqlParameter param)
        {
            // The command
            using (SqlCommand cm = new SqlCommand())
            {
                    cm.Connection = con;

                    cm.CommandText = storedProc;
                    // As a stored procedure
                    cm.CommandType = CommandType.StoredProcedure;
                    // Only add the parameter is there is one
                    if (param != null)
                    {
                        cm.Parameters.Add(param);
                    }


                    // return the reader
                    return cm.ExecuteReader();
            }
            
        }

               

        #endregion
    }
}
