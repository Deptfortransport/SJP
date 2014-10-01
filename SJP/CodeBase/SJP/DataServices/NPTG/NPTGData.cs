// *********************************************** 
// NAME             : NPTGData.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 Jun 2011
// DESCRIPTION  	: Represents NPTG administrative area and district data
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.DatabaseInfrastructure;
using System.Diagnostics;
using SJP.Common.EventLogging;
using System.Data.SqlClient;

namespace SJP.Common.DataServices.NPTG
{
    /// <summary>
    /// Represents NPTG administrative area and district data
    /// </summary>
    public class NPTGData
    {
        #region Private Fields
        // Dictionary to store admin areas by country codes
        private Dictionary<string,List<AdminArea>> countryAdminAreasCache = new Dictionary<string, List<AdminArea>>();
        // Dictionary to store districts grouped by administrative area code
        private Dictionary<int, List<District>> adminAreaDistrictsCache = new Dictionary<int, List<District>>();
        #endregion

        #region Constructor
        
        /// <summary>
        /// NPTGData Constructor
        /// </summary>
        public NPTGData()
        {
            LoadData();
            
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all NPTG admin areas
        /// </summary>
        /// <returns></returns>
        public List<AdminArea> GetAllAdminAreas()
        {
            List<AdminArea> adminAreas = new List<AdminArea>();

            foreach (string countryCode in countryAdminAreasCache.Keys)
            {
                adminAreas.AddRange(countryAdminAreasCache[countryCode]);
            }

            return adminAreas;
        }

        /// <summary>
        /// Returns all NPTG districts
        /// </summary>
        /// <returns></returns>
        public List<District> GetAllDistricts()
        {
            List<District> districts = new List<District>();

            foreach(int areaCode in adminAreaDistrictsCache.Keys)
            {
                districts.AddRange(adminAreaDistrictsCache[areaCode]);
            }

            return districts;
        }

        /// <summary>
        /// Returns list of admin areas for specified country code
        /// </summary>
        /// <param name="CountryCode">Country Code</param>
        /// <returns>List of AdminArea objects</returns>
        public List<AdminArea> GetAdminAreas(string countryCode)
        {
            if (countryAdminAreasCache.Count > 0)
            {
                return countryAdminAreasCache[countryCode];
            }
            return new List<AdminArea>();
        }

        /// <summary>
        /// Returns list of districs for the specified administrative area code
        /// </summary>
        /// <param name="adminAreaCode">Administrative area code</param>
        /// <returns>List of Districs objects</returns>
        public List<District> GetDistricts(int adminAreaCode)
        {
            if (adminAreaDistrictsCache.Count > 0)
            {
                return adminAreaDistrictsCache[adminAreaCode];
            }

            return new List<District>();
        }


        /// <summary>
        /// Returns a specific Admin Area for the admin area code
        /// </summary>
        /// <param name="adminAreaCode">Admin area code</param>
        /// <returns></returns>
        public AdminArea GetAdminArea(int adminAreaCode)
        {
            // Check each country list of AdminAreas, and return if found
            foreach (string countryCode in countryAdminAreasCache.Keys)
            {
                if (countryAdminAreasCache[countryCode].SingleOrDefault(
                        area => area.CountryCode == countryCode
                        && area.AdministrativeAreaCode == adminAreaCode) != null)
                {
                    return countryAdminAreasCache[countryCode].SingleOrDefault(
                        area => area.CountryCode == countryCode
                        && area.AdministrativeAreaCode == adminAreaCode);
                }
                
            }
            return null;
        }

        /// <summary>
        /// Returns a specific Admin Area for the specified country code and admin area code
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <param name="adminAreaCode">Admin area code</param>
        /// <returns></returns>
        public AdminArea GetAdminArea(string countryCode, int adminAreaCode)
        {
            if (countryAdminAreasCache[countryCode] != null)
            {
                return countryAdminAreasCache[countryCode].SingleOrDefault(area => area.CountryCode == countryCode 
                    && area.AdministrativeAreaCode == adminAreaCode);
            }
            return null;
        }

        /// <summary>
        /// Returns a specific district based on the admin area code and district code provided
        /// </summary>
        /// <param name="adminAreaCode">Administrative area code</param>
        /// <param name="districtCode">District code</param>
        /// <returns></returns>
        public District GetDistrict(int adminAreaCode, int districtCode)
        {
            if (adminAreaDistrictsCache[adminAreaCode] != null)
            {
                return adminAreaDistrictsCache[adminAreaCode].SingleOrDefault(district => district.DistrictCode == districtCode
                    && district.AdministrativeAreaCode == adminAreaCode);
            }
            return null;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the data and performs pre processing
        /// </summary>
        private void LoadData()
        {
            LoadAdminArea();

            LoadDistricts();
        }

        /// <summary>
        /// Load NPTG district area
        /// </summary>
        private void LoadDistricts()
        {
            Dictionary<int, List<District>> localAdminAreaDistricts = new Dictionary<int, List<District>>();


            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    // Initialise a SqlHelper and connect to the database.
                    Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Info, "Loading NPTG districts data started"));

                    helper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    #region Load NPTG districts
                    //Retrieve the reference data into a flat table of results
                    SqlDataReader reader = helper.GetReader("GetNPTGDistricts", new List<SqlParameter>());


                    while (reader.Read())
                    {

                        int administrativeAreaCode =int.Parse(reader["AdministrativeAreaCode"].ToString().Trim());
                        string districtName = reader["DistrictName"].ToString().Trim();
                        int districtCode = int.Parse(reader["DistrictCode"].ToString().Trim());
                       
                        District district = new District(districtCode, districtName, administrativeAreaCode);

                        if (!localAdminAreaDistricts.ContainsKey(administrativeAreaCode))
                        {
                            localAdminAreaDistricts[administrativeAreaCode] = new List<District>();
                        }

                        localAdminAreaDistricts[administrativeAreaCode].Add(district);

                    }

                    reader.Close();

                    #region Update the dictionaries
                    Dictionary<int, List<District>> oldTable = new Dictionary<int, List<District>>();

                    if (adminAreaDistrictsCache != null)
                    {
                        oldTable = adminAreaDistrictsCache;
                    }

                    //replace the adminAreaDistrictsCache dictionary
                    adminAreaDistrictsCache = localAdminAreaDistricts;

                    //clear the old data storage 
                    oldTable.Clear();


                    #endregion

                    #endregion

                    // Record the fact that the data was loaded.
                    if (SJPTraceSwitch.TraceVerbose)
                    {
                        Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                            SJPTraceLevel.Verbose, "Loading NPTG admin area data completed"));
                    }

                }
                catch (Exception e)
                {

                    Trace.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "An error occurred whilst attempting to reload the districts data.", e));
                    if ((countryAdminAreasCache == null) || (countryAdminAreasCache.Count == 0))
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

        /// <summary>
        /// Load NPTG Admin areas
        /// </summary>
        private void LoadAdminArea()
        {
            Dictionary<string, List<AdminArea>> localCountryAdminAreas = new Dictionary<string, List<AdminArea>>();
          

            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    // Initialise a SqlHelper and connect to the database.
                    Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Info, "Loading NPTG admin area data started"));

                    helper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    #region Load NPTG admin areas
                    //Retrieve the reference data into a flat table of results
                    SqlDataReader reader = helper.GetReader("GetNPTGAdminAreas", new List<SqlParameter>());
                                       

                    while (reader.Read())
                    {

                        int administrativeAreaCode = int.Parse(reader["AdministrativeAreaCode"].ToString().Trim());
                        string areaName = reader["AreaName"].ToString().Trim();
                        string countryCode = reader["Country"].ToString().Trim();
                        string regionCode = reader["RegionCode"].ToString().Trim();

                        AdminArea adminArea = new AdminArea(administrativeAreaCode, areaName, countryCode, regionCode);

                        if (!localCountryAdminAreas.ContainsKey(countryCode))
                        {
                            localCountryAdminAreas[countryCode] = new List<AdminArea>();
                        }

                        localCountryAdminAreas[countryCode].Add(adminArea);

                    }

                    reader.Close();

                    #region Update the dictionaries
                    Dictionary<string, List<AdminArea>> oldTable = new Dictionary<string, List<AdminArea>>();

                    if (countryAdminAreasCache != null)
                    {
                        oldTable = countryAdminAreasCache;
                    }

                    //replace the countryAdminAreasCache dictionary
                    countryAdminAreasCache = localCountryAdminAreas;

                    //clear the old data storage 
                    oldTable.Clear();

                    
                    #endregion

                    #endregion

                    // Record the fact that the data was loaded.
                    if (SJPTraceSwitch.TraceVerbose)
                    {
                        Trace.Write(new OperationalEvent(SJPEventCategory.Business,
                            SJPTraceLevel.Verbose, "Loading NPTG admin area data completed"));
                    }

                }
                catch (Exception e)
                {
                    
                    Trace.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "An error occurred whilst attempting to reload the Admin area data.", e));
                    if ((countryAdminAreasCache == null) || (countryAdminAreasCache.Count == 0))
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

    }
}
