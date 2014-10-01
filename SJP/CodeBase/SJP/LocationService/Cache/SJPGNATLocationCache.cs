// *********************************************** 
// NAME             : SJPGNATLocationCache.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 21 Feb 2011
// DESCRIPTION  	: Helper class to provide methods to obtain 
//                    GNAT Location Information. 
// ************************************************
//

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// SJPGNATLocationCache helper class to provide methods to obtain 
    /// GNAT Location Information.
    /// </summary>
    static class SJPGNATLocationCache
    {
        #region Private Constants

        private const string DLR_STATION_NAPTAN_PREFIX = "9400ZZ";
        private const string DISTRICT_ALL = "ALL";
        private const int DISTRICTCODE_ALL = 99999; // Number currently not used in District codes table

        #endregion

        #region Private members

        private static readonly object dataInitialisedLock = new object();
        private static bool dataInitialised = false;

        // GNAT Locations
        private static List<SJPGNATLocation> gnatList = new List<SJPGNATLocation>();

        // GNAT Admin Areas
        private static List<SJPGNATAdminArea> adminAreaList = new List<SJPGNATAdminArea>();

        #endregion

        #region Constructor

        /// <summary>
        /// Static Constructor
        /// </summary>
        static SJPGNATLocationCache()
        {
            LoadGNATStations();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Populates the GNAT locations cache by retrieveing the source data from the database.
        /// </summary>
        private static void PopulateGNATLocations()
        {
            // Build GNAT Locations List
            using (SqlHelper helper = new SqlHelper())
            {
                // Temp lists before assigning to the static lists
                List<SJPGNATLocation> tmpGNATList = new List<SJPGNATLocation>();

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading GNAT stations location data"));

                    #region Load GNAT stations

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);
                    List<SqlParameter> paramList = new List<SqlParameter>();

                    using (SqlDataReader listDR = helper.GetReader("GetGNATList", paramList))
                    {
                        while (listDR.Read())
                        {
                            tmpGNATList.Add(
                                new SJPGNATLocation(
                                    listDR["StopName"].ToString(),
                                    SJPLocationType.Station,
                                    listDR["StopNaptan"].ToString().ToUpper(),
                                    Convert.ToBoolean(listDR["WheelchairAccess"]),
                                    Convert.ToBoolean(listDR["AssistanceService"]),
                                    listDR["StopOperator"].ToString(),
                                    listDR["StopCountry"].ToString(),
                                    listDR["AdministrativeAreaCode"].ToString().Parse(0),
                                    listDR["NPTGDistrictCode"].ToString().Parse(0),
                                    listDR.IsDBNull(listDR.GetOrdinal("StopType")) ? 
                                        SJPGNATLocationType.Bus : (SJPGNATLocationType) Enum.Parse(typeof(SJPGNATLocationType), listDR["StopType"].ToString())
                                    ));
                        }
                    }

                    // Assign to static list
                    gnatList = tmpGNATList;

                    #endregion

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("GNAT stations location in cache [{0}]", gnatList.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading GNAT stations location data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load GNAT stations location data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates the GNAT admin areas cache by retrieveing the source data from the database.
        /// </summary>
        private static void PopulateGNATAdminAreas()
        {
            // Build GNAT Admin Areas List
            using (SqlHelper helper = new SqlHelper())
            {
                // Temp lists before assigning to the static lists
                List<SJPGNATAdminArea> tmpGNATList = new List<SJPGNATAdminArea>();

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading GNAT admin areas data"));

                    #region Load GNAT stations

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);
                    List<SqlParameter> paramList = new List<SqlParameter>();

                    using (SqlDataReader listDR = helper.GetReader("GetGNATAdminAreas", paramList))
                    {
                        while (listDR.Read())
                        {
                            int adminAreaCode = listDR["AdministrativeAreaCode"].ToString().Parse(0);
                            string districtCodeStr = listDR["DistrictCode"].ToString();
                            int districtCode = 0;
                            
                            // Check if district code is "All"
                            if (!string.IsNullOrEmpty(districtCodeStr))
                            {
                                if (districtCodeStr.ToUpper().Equals(DISTRICT_ALL))
                                {
                                    districtCode = DISTRICTCODE_ALL;
                                }
                                else
                                {
                                    districtCode = districtCodeStr.Parse(0);
                                }
                            }

                            // Add to the temp list
                            tmpGNATList.Add(
                                new SJPGNATAdminArea(
                                    adminAreaCode,
                                    districtCode,
                                    Convert.ToBoolean(listDR["StepFree"]),
                                    Convert.ToBoolean(listDR["Assistance"])
                                    ));
                        }
                    }

                    // Assign to static list
                    adminAreaList = tmpGNATList;

                    #endregion

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("GNAT Admin Areas in cache [{0}]", adminAreaList.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading GNAT admin areas data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load GNAT admin areas data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        #endregion
        
        #region Query Methods

        /// <summary>
        /// Returns true is the location NaPTAN is found in the GNAT stations list and has the required GNAT attributes
        /// </summary>
        /// <param name="naptan"></param>
        /// <returns></returns>
        internal static bool IsGNAT(string naptan, bool stepFreeAccess, bool assistanceAvailable)
        {
            SJPGNATLocation result = gnatList.Find(delegate(SJPGNATLocation loc) { return loc.ID == naptan; });
            if (result != null)
            {
                if (stepFreeAccess && assistanceAvailable)
                {
                    return result.StepFreeAccess && result.AssistanceAvailable; 
                }
                else if (stepFreeAccess)
                {
                    return result.StepFreeAccess;
                }
                else if (assistanceAvailable)
                {
                    return result.AssistanceAvailable;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true is the admin area and district code are found in the GNAT admin areas list 
        /// and has the required GNAT attributes
        /// </summary>
        /// <param name="adminAreaCode">Admin Area code</param>
        /// <param name="districtCode">District code</param>
        /// <param name="stepFreeAccess">Check step free access available in admin area/district</param>
        /// <param name="assistanceAvailable">Check assistance available in admin area/district</param>
        /// <returns></returns>
        internal static bool IsGNATAdminArea(int adminAreaCode, int districtCode, bool stepFreeAccess, bool assistanceAvailable)
        {
            // Find matching on both admin area and district code
            SJPGNATAdminArea result = adminAreaList.Find(delegate(SJPGNATAdminArea gnatAdminArea) 
                { return (gnatAdminArea.AdministrativeAreaCode == adminAreaCode)
                          && (gnatAdminArea.DistrictCode == districtCode); });

            // If no result found, check for admin area and the "All" district code
            if (result == null)
            {
                result = adminAreaList.Find(delegate(SJPGNATAdminArea gnatAdminArea)
                {
                    return (gnatAdminArea.AdministrativeAreaCode == adminAreaCode)
                            && (gnatAdminArea.DistrictCode == DISTRICTCODE_ALL);
                });
            }

            // Check for GNAT attributes
            if (result != null)
            {
                if (stepFreeAccess && assistanceAvailable)
                {
                    return result.StepFreeAccess && result.AssistanceAvailable;
                }
                else if (stepFreeAccess)
                {
                    return result.StepFreeAccess;
                }
                else if (assistanceAvailable)
                {
                    return result.AssistanceAvailable;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a list of summary data for all Venues.
        /// </summary>
        /// <returns>SJPGNATLocation list for GNAT stations</returns>
        internal static List<SJPGNATLocation> GetGNATList()
        {
            return gnatList;
        }

        /// <summary>
        /// Loads all GNAT data
        /// </summary>
        internal static void LoadGNATStations()
        {
            // Make load threadsafe
            if (!dataInitialised)
            {
                lock (dataInitialisedLock)
                {
                    if (!dataInitialised)
                    {
                        // Load data
                        PopulateGNATLocations();
                        PopulateGNATAdminAreas();

                        // Set to true here (prevents repeated attempts to load if it fails)
                        dataInitialised = true;
                    }
                }
            }
        }

        #endregion
    }
}
