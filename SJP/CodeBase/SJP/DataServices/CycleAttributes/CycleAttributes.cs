// *********************************************** 
// NAME             : CycleAttributes.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Class which provides the collection of CycleAttributes which can be displayed on a page.
//                  : The UI will compare the attributes returned for a cycle journey against this collection,
//                  : to determine if and what text it can display for the cycle journey attribute.
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SJP.Common.EventLogging;
using SJP.Common.DatabaseInfrastructure;
using System.Data.SqlClient;

namespace SJP.Common.DataServices.CycleAttributes
{
    /// <summary>
    /// CycleAttributes class
    /// </summary>
    public class CycleAttributes : ICycleAttributes
    {
        #region Private members

        

        // Hashtable to store Cycle attribute data
        private Dictionary<int, CycleAttribute> cycleAttributesCache = new Dictionary<int, CycleAttribute>();

        // Hashtable to store Cycle infrastructure attribute data (used to control display of cycle path icon)
        private Dictionary<int, CycleAttribute> cycleInfrastructureAttributesCache = new Dictionary<int, CycleAttribute>();

        // Hashtable to store Cycle recommended route attribute data (used to control display of cycle route icon)
        private Dictionary<int, CycleAttribute> cycleRecommendedAttributesCache = new Dictionary<int, CycleAttribute>();

        // Variables to hold column headers for data
        private int cycleattributeIdColumnOrdinal;
        private int cycleattributeTypeColumnOrdinal;
        private int cycleattributeGroupColumnOrdinal;
        private int cycleattributeCategoryColumnOrdinal;
        private int cycleattributeResourceNameColumnOrdinal;
        private int cycleattributeMaskColumnOrdinal;
        private int cycleattributeCycleInfrastructureColumnOrdinal;
        private int cycleattributeCycleRecommendedColumnOrdinal;

        #endregion

        #region Constructor

        /// <summary>
        /// CycleAttributes Constructor
        /// </summary>
        public CycleAttributes()
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
            Dictionary<int, CycleAttribute> localCycleAttributesCache = new Dictionary<int, CycleAttribute>();
            Dictionary<int, CycleAttribute> localCycleInfrastructureAttributesCache = new Dictionary<int, CycleAttribute>();
            Dictionary<int, CycleAttribute> localCycleRecommendedAttributesCache = new Dictionary<int, CycleAttribute>();

            using (SqlHelper helper = new SqlHelper())
            {

                try
                {
                    // Initialise a SqlHelper and connect to the database.
                    Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Info, "Loading CycleAttributes data started"));
                   
                    helper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    #region Load cycle attributes into hashtables
                    //Retrieve the reference data into a flat table of results
                    SqlDataReader reader = helper.GetReader("GetCycleAttributes", new List<SqlParameter>());

                    //Assign the column ordinals
                    cycleattributeIdColumnOrdinal = reader.GetOrdinal("CycleAttributeId");
                    cycleattributeTypeColumnOrdinal = reader.GetOrdinal("Type");
                    cycleattributeGroupColumnOrdinal = reader.GetOrdinal("Group");
                    cycleattributeCategoryColumnOrdinal = reader.GetOrdinal("Category");
                    cycleattributeResourceNameColumnOrdinal = reader.GetOrdinal("ResourceName");
                    cycleattributeMaskColumnOrdinal = reader.GetOrdinal("Mask");
                    cycleattributeCycleInfrastructureColumnOrdinal = reader.GetOrdinal("CycleInfrastructure");
                    cycleattributeCycleRecommendedColumnOrdinal = reader.GetOrdinal("CycleRecommendedRoute");

                    while (reader.Read())
                    {
                        //Read in each cycle attribute
                        int attributeId = reader.GetInt32(cycleattributeIdColumnOrdinal);
                        string attributeType = reader.GetString(cycleattributeTypeColumnOrdinal);
                        string attributeGroup = reader.GetString(cycleattributeGroupColumnOrdinal);
                        string attributeCategory = reader.GetString(cycleattributeCategoryColumnOrdinal);
                        string attributeResourceName = reader.GetString(cycleattributeResourceNameColumnOrdinal);
                        Int64 attributeMask = Convert.ToInt64(reader.GetString(cycleattributeMaskColumnOrdinal), 16);
                        bool attributeCycleInfrastructure = reader.GetBoolean(cycleattributeCycleInfrastructureColumnOrdinal);
                        bool attributeCycleRecommended = reader.GetBoolean(cycleattributeCycleRecommendedColumnOrdinal);

                        CycleAttributeType cycleAttributeType = GetCycleAttributeType(attributeType);
                        CycleAttributeGroup cycleAttributeGroup = GetCycleAttributeGroup(attributeGroup);
                        CycleAttributeCategory cycleAttributeCategory = GetCycleAttributeCategory(attributeCategory);

                        CycleAttribute cycleAttribute = new CycleAttribute(attributeId, cycleAttributeType, cycleAttributeGroup, cycleAttributeCategory, attributeResourceName, attributeMask);

                        // Add to the local caches
                        localCycleAttributesCache.Add(attributeId, cycleAttribute);

                        if (attributeCycleInfrastructure)
                        {
                            localCycleInfrastructureAttributesCache.Add(attributeId, cycleAttribute);
                        }

                        if (attributeCycleRecommended)
                        {
                            localCycleRecommendedAttributesCache.Add(attributeId, cycleAttribute);
                        }
                    }

                    reader.Close();

                    #region Update the hashtables
                    Dictionary<int, CycleAttribute> oldTable = new Dictionary<int, CycleAttribute>();

                    //Create reference to the existing cycleAttributesCache hashtable prior to replacing it				
                    //This is for multi-threading purposes to ensure there is always data to reference
                    if (cycleAttributesCache != null)
                    {
                        oldTable = cycleAttributesCache;
                    }

                    //replace the cycleAttributesCache hashtable
                    cycleAttributesCache = localCycleAttributesCache;

                    //clear the old data storage hashtable
                    oldTable.Clear();


                    // CycleInfrastructure cache
                    if (cycleInfrastructureAttributesCache != null)
                    {
                        oldTable = cycleInfrastructureAttributesCache;
                    }

                    cycleInfrastructureAttributesCache = localCycleInfrastructureAttributesCache;

                    oldTable.Clear();


                    // CycleRecommended cache
                    if (cycleRecommendedAttributesCache != null)
                    {
                        oldTable = cycleRecommendedAttributesCache;
                    }

                    cycleRecommendedAttributesCache = localCycleRecommendedAttributesCache;

                    oldTable.Clear();

                    #endregion

                    #endregion

                    // Record the fact that the data was loaded.
                    if (SJPTraceSwitch.TraceVerbose)
                    {
                        Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                            SJPTraceLevel.Verbose, "Loading CycleAttributes data completed"));
                    }

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
                    Trace.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "An error occurred whilst attempting to reload the CycleAttributes data.", e));
                    if ((cycleAttributesCache == null) || (cycleAttributesCache.Count == 0))
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

        #region Helpers
        /// <summary>
        /// Method which parses a string into a CycleAttributeType.
        /// </summary>
        /// <param name="attributeGroup">string attributeType</param>
        /// <returns>CycleAttributeType.None if unable to parse</returns>
        private CycleAttributeType GetCycleAttributeType(string attributeType)
        {
            CycleAttributeType cycleAttributeType = CycleAttributeType.None;

            try
            {
                if (!string.IsNullOrEmpty(attributeType))
                {
                    cycleAttributeType = (CycleAttributeType)Enum.Parse(typeof(CycleAttributeType), attributeType);
                }
            }
            catch
            {
                // If the attribute group could be parsed, log exception

                Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                   SJPTraceLevel.Info, "Error occurred attempting to parse a CycleAttributeType for string: " + attributeType));
            }

            return cycleAttributeType;
        }

        /// <summary>
        /// Method which parses a string into a CycleAttributeGroup.
        /// </summary>
        /// <param name="attributeGroup">string attributeGroup</param>
        /// <returns>CycleAttributeGroup.None if unable to parse</returns>
        private CycleAttributeGroup GetCycleAttributeGroup(string attributeGroup)
        {
            CycleAttributeGroup cycleAttributeGroup = CycleAttributeGroup.None;

            try
            {
                if (!string.IsNullOrEmpty(attributeGroup))
                {
                    cycleAttributeGroup = (CycleAttributeGroup)Enum.Parse(typeof(CycleAttributeGroup), attributeGroup);
                }
            }
            catch
            {
                // If the attribute group could be parsed, log exception

                Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                    SJPTraceLevel.Info, "Error occurred attempting to parse a CycleAttributeGroup for string: " + attributeGroup));
            }

            return cycleAttributeGroup;
        }

        /// <summary>
        /// Method which parses a string into a CycleAttributeCategory.
        /// </summary>
        /// <param name="attributeCategory">string attributeCategory</param>
        /// <returns>CycleAttributeCategory.None if unable to parse</returns>
        private CycleAttributeCategory GetCycleAttributeCategory(string attributeCategory)
        {
            CycleAttributeCategory cycleAttributeCategory = CycleAttributeCategory.None;

            try
            {
                if (!string.IsNullOrEmpty(attributeCategory))
                {
                    cycleAttributeCategory = (CycleAttributeCategory)Enum.Parse(typeof(CycleAttributeCategory), attributeCategory);
                }
            }
            catch
            {
                // If the attribute group could be parsed, log exception

                Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                    SJPTraceLevel.Info, "Error occurred attempting to parse a CycleAttributeCategory for string: " + attributeCategory));
            }

            return cycleAttributeCategory;
        }
        #endregion

        #endregion

        

        #region Public methods

        /// <summary>
        /// Returns an array of all CycleAttribute objects
        /// </summary>
        /// <returns>CycleAttribute[]</returns>
        public List<CycleAttribute> GetCycleAttributes(CycleAttributeType cycleAttributeType, CycleAttributeGroup cycleAttributeGroup)
        {
            List<CycleAttribute> cycleAttributeArray = new List<CycleAttribute>();

            if ((cycleAttributesCache != null) && (cycleAttributesCache.Count > 0))
            {
                foreach (int attributeId in cycleAttributesCache.Keys)
                {
                    CycleAttribute cycleAttribute = cycleAttributesCache[attributeId];
                    if ((cycleAttribute.CycleAttributeType == cycleAttributeType) &&
                            (cycleAttribute.CycleAttributeGroup == cycleAttributeGroup))
                    {
                        cycleAttributeArray.Add(cycleAttribute);
                    }

                }
            }

            return cycleAttributeArray;
        }

        /// <summary>
        /// Returns an array of all CycleAttributes 
        /// </summary>
        /// <returns>List of CycleAttribute objects</returns>
        public List<CycleAttribute> GetCycleAttributes()
        {
            List<CycleAttribute> cycleAttributeArray = new List<CycleAttribute>();

            if ((cycleAttributesCache != null) && (cycleAttributesCache.Count > 0))
            {
                foreach (int attributeId in cycleAttributesCache.Keys)
                {
                    CycleAttribute cycleAttribute = cycleAttributesCache[attributeId];

                    cycleAttributeArray.Add(cycleAttribute);

                }
            }

            return cycleAttributeArray;
        }

        /// <summary>
        /// Returns an array of CycleAttributes for the specified CycleAttributeGroup. Attributes returned will 
        /// be those attributes which have been defined as specific Cycle Infrastructure. 
        /// Can be used to determine when the Cycle (cycle path) icon is displayed
        /// </summary>
        /// <param name="cycleAttributeGroup">CycleAttributeGroup</param>
        /// <returns>List of CycleAttribute objects</returns>
        public List<CycleAttribute> GetCycleInfrastructureAttributes(CycleAttributeGroup cycleAttributeGroup)
        {
            List<CycleAttribute> cycleInfrastructureAttributeArray = new List<CycleAttribute>();

            if ((cycleInfrastructureAttributesCache != null) && (cycleInfrastructureAttributesCache.Count > 0))
            {
                foreach (int attributeId in cycleInfrastructureAttributesCache.Keys)
                {
                    CycleAttribute cycleAttribute = cycleInfrastructureAttributesCache[attributeId];

                    if (cycleAttribute.CycleAttributeGroup == cycleAttributeGroup)
                    {
                        cycleInfrastructureAttributeArray.Add(cycleAttribute);
                    }
                }
            }

            return cycleInfrastructureAttributeArray;
        }

        /// <summary>
        /// Returns an array of CycleAttributes for the specified CycleAttributeGroup. Attributes returned will 
        /// be those attributes which have been defined as Recommended cycle route attributes. 
        /// Can be used to determine when the Cycle (recommended route) icon is displayed
        /// </summary>
        /// <param name="cycleAttributeGroup">CycleAttributeGroup</param>
        /// <returns>List of CycleAttribute objects</returns>
        public List<CycleAttribute> GetCycleRecommendedAttributes(CycleAttributeGroup cycleAttributeGroup)
        {
            List<CycleAttribute> cycleRecommendedAttributeArray = new List<CycleAttribute>();

            if ((cycleRecommendedAttributesCache != null) && (cycleRecommendedAttributesCache.Count > 0))
            {
                foreach (int attributeId in cycleRecommendedAttributesCache.Keys)
                {
                    CycleAttribute cycleAttribute = cycleRecommendedAttributesCache[attributeId];

                    if (cycleAttribute.CycleAttributeGroup == cycleAttributeGroup)
                    {
                        cycleRecommendedAttributeArray.Add(cycleAttribute);
                    }
                }
            }

            return cycleRecommendedAttributeArray;
        }

        #endregion
    }
}
