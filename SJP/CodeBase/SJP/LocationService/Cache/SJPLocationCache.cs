// *********************************************** 
// NAME             : SJPLocationCache.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 21 Feb 2011
// DESCRIPTION  	: Helper class to provide methods to obtain 
//                    Location Information for non-Olympic Venue locations
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// SJPLocationCache helper class to provide methods to obtain 
    //  Location Information for non-Olympic Venue locations
    /// </summary>
    static class SJPLocationCache
    {
        #region Private members

        // Stored procs - All must return the same column names as a common ReadLocation method is used
        private const string SP_GetLocations = "GetLocations";
        private const string SP_GetUnknownLocation = "GetUnknownLocation";
        private const string SP_GetNaptanLocation = "GetNaptanLocation";
        private const string SP_GetGroupLocation = "GetGroupLocation";
        private const string SP_GetLocalityLocation = "GetLocalityLocation";
        private const string SP_GetLocalityLocations = "GetLocalityLocations";
        private const string SP_GetPostcodeLocation = "GetPostcodeLocation";
        private const string SP_GetPostcodeLocations = "GetPostcodeLocations";

        private const string SP_GetAlternativeSuggestionList = "GetAlternativeSuggestionList";

        // Used for load
        private static readonly object dataInitialisedLock = new object();
        private static bool dataLocationsInitialised = false;
        private static bool dataPostcodesInitialised = false;

        // Load/Use flags
        private static bool useLocationCache = false;
        private static bool usePostcodeCache = false;

        // Location caches
        private static List<SJPLocation> locations = new List<SJPLocation>();
        private static List<SJPLocation> postcodeLocations = new List<SJPLocation>();

        #endregion

        #region Constructor

        /// <summary>
        /// Static Constructor
        /// </summary>
        static SJPLocationCache()
        {
            LoadLocations();
            LoadPostcodes();
        }

        #endregion

        #region Private methods

        #region Locations

        /// <summary>
        /// Populates the Locations cache by retrieveing the source data from the database.
        /// </summary>
        private static void PopulateLocationsData()
        {
            // Build Locations List
            using (SqlHelper helper = new SqlHelper())
            {
                // Temp lists before assigning to the static lists
                List<SJPLocation> tmpLocations = new List<SJPLocation>();

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Locations data"));

                    #region Load locations

                    SJPLocation sjpLocation = null;

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue location array
                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetLocations, paramList))
                    {
                        while (locationsDR.Read())
                        {
                            sjpLocation = ReadLocation(locationsDR, SJPLocationType.Unknown);
                                                        
                            tmpLocations.Add(sjpLocation);
                        }
                    }

                    // Assign to static lists
                    locations = tmpLocations;

                    #endregion

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Locations in cache [{0}]", locations.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Locations data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Locations data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        #region Unknown

        /// <summary>
        /// Populates a single Unknown location (not cached) by retrieving the source data from the database.
        /// </summary>
        /// <returns></returns>
        private static SJPLocation PopulateUnknownData(string location)
        {
            // Load Unknown location from database
            SJPLocation sjpLocation = new SJPLocation();

            if (!string.IsNullOrEmpty(location))
            {
                using (SqlHelper helper = new SqlHelper())
                {
                    try
                    {
                        #region Load unknown

                        // If its a postcode, then remove any spaces to allow search matching in database query
                        // as raw data includes spaces
                        if (location.IsValidPostcode())
                        {
                            location = location.Replace(" ", string.Empty);
                        }

                        List<SqlParameter> paramList = new List<SqlParameter>();
                        paramList.Add(new SqlParameter("@searchstring", location));

                        helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                        using (SqlDataReader locationsDR = helper.GetReader(SP_GetUnknownLocation, paramList))
                        {
                            if (locationsDR.HasRows)
                            {
                                locationsDR.Read();

                                sjpLocation = ReadLocation(locationsDR, SJPLocationType.Unknown);

                                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                        string.Format("Read Unknown location data for location searchstring[{0}]", location)));

                            }
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Error occurred attempting to load Unknown location[{0}] location data: {1}", location, ex.Message);

                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                    }
                }
            }

            return sjpLocation;
        }
                
        #endregion

        #region NaPTAN

        /// <summary>
        /// Populates a single NaPTAN location (not cached) by retrieving the source data from the database.
        /// </summary>
        /// <returns></returns>
        private static SJPLocation PopulateNaPTANData(string naptanId)
        {
            // Load NaPTAN location from database
            SJPLocation sjpLocation = new SJPLocation();
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    #region Load naptan

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@naptan", naptanId));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetNaptanLocation, paramList))
                    {
                        if (locationsDR.HasRows)
                        {
                            locationsDR.Read();

                            sjpLocation = ReadLocation(locationsDR, SJPLocationType.Station);
                            
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                    string.Format("Read NaPTAN location data for naptan[{0}]", naptanId)));

                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load NaptanId[{0}] location data: {1}", naptanId, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }

            return sjpLocation;
        }

        #endregion

        #region Group

        /// <summary>
        /// Populates a single Group location (not cached) by retrieving the source data from the database.
        /// </summary>
        /// <returns></returns>
        private static SJPLocation PopulateGroupData(string groupId)
        {
            // Load group location from database
            SJPLocation sjpLocation = new SJPLocation();
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    #region Load locality

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@groupID", groupId));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetGroupLocation, paramList))
                    {
                        if (locationsDR.HasRows)
                        {
                            locationsDR.Read();

                            sjpLocation = ReadLocation(locationsDR, SJPLocationType.StationGroup);
                            
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                    string.Format("Read Group location data for group[{0}]", groupId)));

                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load GroupId[{0}] location data: {1}", groupId, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }

            return sjpLocation;
        }

        #endregion

        #region Locality

        /// <summary>
        /// Populates a single Locality location (not cached) by retrieving the source data from the database.
        /// </summary>
        /// <returns></returns>
        private static SJPLocation PopulateLocalityData(string localityId)
        {
            // Load locality location from database
            SJPLocation sjpLocation = new SJPLocation();
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    #region Load locality

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@localityID", localityId));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetLocalityLocation, paramList))
                    {
                        if (locationsDR.HasRows)
                        {
                            locationsDR.Read();

                            sjpLocation = ReadLocation(locationsDR, SJPLocationType.Locality);
                            
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                    string.Format("Read Locality location data for locality[{0}]", localityId)));

                        }
                    }
                    
                    #endregion
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load LocalityId[{0}] location data: {1}", localityId, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }

            return sjpLocation;
        }

        /// <summary>
        /// Returns a list of Locality locations (not cached) by retrieving the source data from the database
        /// where localitys are within a coordinate square
        /// </summary>
        /// <param name="osgrMin"></param>
        /// <param name="osgrMax"></param>
        /// <returns></returns>
        private static List<SJPLocation> PopulateLocalityLocationsUsingCoordinate(OSGridReference osgrMin, OSGridReference osgrMax)
        {
            List<SJPLocation> locations = new List<SJPLocation>();

            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    #region Get localitys using coordinate square

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@eastingMin", osgrMin.Easting));
                    paramList.Add(new SqlParameter("@eastingMax", osgrMax.Easting));
                    paramList.Add(new SqlParameter("@northingMin", osgrMin.Northing));
                    paramList.Add(new SqlParameter("@northingMax", osgrMax.Northing));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Get locality locations using the coordinate square
                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetLocalityLocations, paramList))
                    {
                        SJPLocation sjpLocation = null;

                        if (locationsDR.HasRows)
                        {
                            while (locationsDR.Read())
                            {
                                sjpLocation = ReadLocation(locationsDR, SJPLocationType.Locality);

                                locations.Add(sjpLocation);
                            }
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Locality locations data for coordinate square [{0}][{1}]", osgrMin.ToString(), osgrMax.ToString());

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }


            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                    string.Format("Read Locality location data count[{0}] for localitys in cooridinate square [{1}][{2}]", locations.Count, osgrMin.ToString(), osgrMax.ToString())));

            return locations;
        }

        #endregion
                
        #endregion

        #region Postcodes

        /// <summary>
        /// Populates the Postcode locations cache by retrieveing the source data from the database.
        /// </summary>
        private static void PopulatePostcodesData()
        {
            // Build Postcode Locations List
            using (SqlHelper helper = new SqlHelper())
            {
                // Temp lists before assigning to the static lists
                List<SJPLocation> tmpPostcodeLocations = new List<SJPLocation>();

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Postcodes location data"));

                    #region Load postcodes

                    SJPLocation sjpLocation = null;

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    
                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);
                    
                    // Read and populate the detailed venue location array
                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetPostcodeLocations, paramList))
                    {
                        while (locationsDR.Read())
                        {
                            sjpLocation = ReadLocation(locationsDR, SJPLocationType.Postcode);

                            // Add to locations list
                            tmpPostcodeLocations.Add(sjpLocation);
                        }
                    }

                    // Assign to static lists
                    postcodeLocations = tmpPostcodeLocations;

                    #endregion

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Postcode locations in cache [{0}]", postcodeLocations.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Postcodes location data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Postcodes location data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates a single Postcode location (not cached) by retrieving the source data from the database.
        /// </summary>
        /// <returns></returns>
        private static SJPLocation PopulatePostcodeData(string postcode)
        {
            // Load postcode location from database
            SJPLocation sjpLocation = new SJPLocation();
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    #region Load postcode

                    // If its a postcode, then remove any spaces to allow search matching in database query
                    // as raw data includes spaces
                    if (postcode.IsValidPostcode())
                    {
                        postcode = postcode.Replace(" ", string.Empty);
                    }

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@postcode", postcode));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetPostcodeLocation, paramList))
                    {
                        if (locationsDR.HasRows)
                        {
                            locationsDR.Read();

                            sjpLocation = ReadLocation(locationsDR, SJPLocationType.Postcode);
                            
                            Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                    string.Format("Read Postcode location data for postcode[{0}]", postcode)));
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Postcode[{0}] location data: {1}", postcode, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }

            return sjpLocation;
        }

        #endregion

        /// <summary>
        /// Uses the SqlDataReader to build an SJPLocation (does not advance the reader)
        /// </summary>
        private static SJPLocation ReadLocation(SqlDataReader locationsDR, SJPLocationType sjpLocationType)
        {
            // Return object
            SJPLocation sjpLocation = null;

            // Read Values
            string dataSetID = (locationsDR["DATASETID"] != DBNull.Value) ? locationsDR["DATASETID"].ToString() : string.Empty;
            string name = (locationsDR["Name"] != DBNull.Value) ? locationsDR["Name"].ToString() : string.Empty;
            string displayName = (locationsDR["DisplayName"] != DBNull.Value) ? locationsDR["DisplayName"].ToString() : string.Empty;
            string locality = (locationsDR["LocalityID"] != DBNull.Value) ? locationsDR["LocalityID"].ToString() : string.Empty;
            string parentId = (locationsDR["ParentID"] != DBNull.Value) ? locationsDR["ParentID"].ToString() : string.Empty;

            float easting = (float)Convert.ToDouble(locationsDR["Easting"].ToString());
            float northing = (float)Convert.ToDouble(locationsDR["Northing"].ToString());
            OSGridReference osgr = new OSGridReference(easting, northing);
            float nearestEasting = (float)Convert.ToDouble((locationsDR["NearestPointE"] != DBNull.Value) ? locationsDR["NearestPointE"].ToString() : easting.ToString());
            float nearestNorthing = (float)Convert.ToDouble((locationsDR["NearestPointN"] != DBNull.Value) ? locationsDR["NearestPointN"].ToString() : northing.ToString());
            OSGridReference nearestOsgr = new OSGridReference(nearestEasting, nearestNorthing);

            int adminAreaID = (int)Convert.ToInt32((locationsDR["AdminAreaID"] != DBNull.Value) ? locationsDR["AdminAreaID"].ToString() : "0");
            int districtID = (int)Convert.ToInt32((locationsDR["DistrictID"] != DBNull.Value) ? locationsDR["DistrictID"].ToString() : "0");

            string naptan = (locationsDR["Naptan"] != DBNull.Value) ? locationsDR["Naptan"].ToString() : string.Empty;
            List<string> naptans = new List<string>();
            if (!string.IsNullOrEmpty(naptan))
                naptans.AddRange(naptan.Split(new char[] {','}));

            string toid = (locationsDR["NearestTOID"] != DBNull.Value) ? locationsDR["NearestTOID"].ToString() : string.Empty;
            List<string> toids = new List<string>();
            if (!string.IsNullOrEmpty(toid))
                toids.Add(toid);

            SJPLocationType sjpLocationTypeActual = SJPLocationType.Unknown;

            // Only read the location if parameter value is not known
            if (sjpLocationType == SJPLocationType.Unknown)
            {
                string type = (locationsDR["Type"] != DBNull.Value) ? locationsDR["Type"].ToString() : string.Empty;
                sjpLocationType = SJPLocationTypeHelper.GetSJPLocationType(type);
                sjpLocationTypeActual = SJPLocationTypeHelper.GetSJPLocationTypeActual(type);
            }

            // Create naptan location
            sjpLocation = new SJPLocation(
                name, displayName, locality, toids, naptans, parentId,
                sjpLocationType, sjpLocationTypeActual, osgr, nearestOsgr, false, false, 
                adminAreaID, districtID, dataSetID);

            // Update GNAT flag
            sjpLocation.IsGNAT = SJPGNATLocationCache.IsGNAT(sjpLocation.ID,false,false);

            return sjpLocation;
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Searches for any location where the DisplayName is an exact match to the supplied search string. 
        /// </summary>
        /// <param name="location">The postcode to search for</param>
        /// <returns>SJPLocation or null if no match found</returns>
        internal static SJPLocation GetUnknownLocation(string location)
        {
            if (useLocationCache)
            {
                // Find unknown location in cache (search on display name)
                string locationUpper = location.ToUpper();

                SJPLocation result = locations.Find(delegate(SJPLocation loc) { return loc.DisplayName.ToUpper() == locationUpper; });
                return result;
            }
            else
            {
                // Load unknown location from database
                SJPLocation result = PopulateUnknownData(location);
                return result;
            }
        }

        /// <summary>
        /// Searches for a Station Location where the Naptan is an exact match to the supplied search string. 
        /// </summary>
        /// <param name="naptan">The naptan of the station to search for</param>
        /// <returns>SJPLocation or null if no match found</returns>
        internal static SJPLocation GetNaptanLocation(string naptan)
        {
            if (useLocationCache)
            {
                // Find naptan location in cache (each location in cache will only contain one (or none) naptan)
                SJPLocation result = locations.Find(delegate(SJPLocation loc) { return loc.Naptan.Contains(naptan); });
                return result;
            }
            else
            {
                // Load naptan location from database
                SJPLocation result = PopulateNaPTANData(naptan);
                return result;
            }
        }

        /// <summary>
        /// Searches for a Station Group where the Group ID is an exact match with the supplied search string. 
        /// </summary>
        /// <param name="groupID">The ID of the group to search for</param>
        /// <returns>SJPLocation or null if no match found</returns>
        internal static SJPLocation GetGroupLocation(string groupID)
        {
            if (useLocationCache)
            {
                // Find group location in cache
                SJPLocation result = locations.Find(delegate(SJPLocation loc) { return loc.DataSetID == groupID; });
                return result;
            }
            else
            {
                // Load group location from database
                SJPLocation result = PopulateGroupData(groupID);
                return result;
            }
        }

        /// <summary>
        /// Searches for a Locality that is an exact match to the supplied search string. 
        /// </summary>
        /// <param name="localityID">The ID of the locality to search for</param>
        /// <returns>SJPLocation or null if no match found</returns>
        internal static SJPLocation GetLocalityLocation(string localityID)
        {
            if (useLocationCache)
            {
                // Find locality location in cache
                SJPLocation result = locations.Find(delegate(SJPLocation loc) { return loc.ID == localityID; });
                return result;
            }
            else
            {
                // Load locality location from database
                SJPLocation result = PopulateLocalityData(localityID);
                return result;
            }
        }

        /// <summary>
        /// Searches for a Locality closest to the supplied coordinate
        /// </summary>
        /// <returns>SJPLocation or null if none found</returns>
        internal static SJPLocation GetLocalityLocationForCoordinate(OSGridReference osgr)
        {
            // Found locality location object
            SJPLocation result = null;

            #region Find closest locality location

            List<SJPLocation> sjpLocationsFound = new List<SJPLocation>();

            // Set an area to narrow down the localities
            int paddingEastingMetres = Properties.Current[Keys.CoordinateLocation_LocalitySearch_Padding_Easting].Parse(50000);
            int paddingNorthingMetres = Properties.Current[Keys.CoordinateLocation_LocalitySearch_Padding_Northing].Parse(50000);

            OSGridReference osgrMin = new OSGridReference(osgr.Easting - paddingEastingMetres, osgr.Northing - paddingNorthingMetres);
            OSGridReference osgrMax = new OSGridReference(osgr.Easting + paddingEastingMetres, osgr.Northing + paddingNorthingMetres);

            // Find all locality locations closest to coordinate
            if (useLocationCache)
            {
                // Load from cache
                sjpLocationsFound = locations.FindAll(delegate(SJPLocation loc) {
                    return (loc.TypeOfLocation == SJPLocationType.Locality
                        && loc.GridRef.Easting > osgrMin.Easting && loc.GridRef.Easting < osgrMax.Easting)
                        && (loc.GridRef.Northing > osgrMin.Northing && loc.GridRef.Northing < osgrMax.Northing);
                });
            }
            else
            {
                // Load from database
                sjpLocationsFound = PopulateLocalityLocationsUsingCoordinate(osgrMin, osgrMax);
            }

            // Track the shortest locality location distance from the coordinate
            int shortestDistance = Int32.MaxValue;
            int locDistance = 0;

            // Find the closest locality location to the coordinate
            foreach (SJPLocation loc in sjpLocationsFound)
            {
                locDistance = loc.GridRef.DistanceFrom(osgr);

                if (locDistance < shortestDistance)
                {
                    // Closer location found
                    shortestDistance = locDistance;
                    result = loc;
                }
            }

            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                    string.Format("Closest Locality location found for coordinate[{0}] is id[{1}] displayname[{2}] distance[{3}metres]",
                        osgr.ToString(),
                        (result != null) ? result.ID : string.Empty,
                        (result != null) ? result.DisplayName : string.Empty,
                        shortestDistance
                    )));

            #endregion

            return result;
        }


        /// <summary>
        /// Searches for a Postcode that is an exact match to the supplied search string. 
        /// </summary>
        /// <param name="postcode">The postcode to search for</param>
        /// <returns>SJPLocation or null if no match found</returns>
        internal static SJPLocation GetPostcodeLocation(string postcode)
        {
            if (usePostcodeCache)
            {
                // Find postcode location in cache
                SJPLocation result = postcodeLocations.Find(delegate(SJPLocation loc)
                {
                    return loc.ID == postcode.ToUpper().Replace(" ", "");
                });
                return result;
            }
            else
            {
                // Load postcode location from database
                SJPLocation result = PopulatePostcodeData(postcode);
                return result;
            }
        }

        /// <summary>
        /// Searches for SJPLocations that are a close match to the supplied search string. 
        /// This method should only be called if a previous call to GetSJPLocation was unsuccesful.
        /// </summary>
        /// <param name="searchString">The name to search for</param>
        /// <returns>SJPLocation[], contains suitable alternatives</returns>
        internal static List<SJPLocation> GetAlternativeSJPLocations(string searchString)
        {
            // Enforce a limit to search for, e.g. a single letter "a" search could return 1000's of records
            int searchLimit = Properties.Current[Keys.Max_SearchLocationsLimit].Parse(1000);

            // Number of locations to return
            int searchShowLimit = Properties.Current[Keys.Max_SearchLocationsShow].Parse(20);

            // Use "in cache" ambiguity search logic,
            // this is different to the database stored proc ambiguity search, and better
            bool searchInCache = Properties.Current[Keys.Search_Cache_Locations].Parse(true);
                        
            List<SJPLocation> altLocations = new List<SJPLocation>();
            if (useLocationCache && searchInCache)
            {
                altLocations.AddRange(GetAlternativeSJPLocationsFromCache(searchString, searchLimit));
            }
            else
            {
                altLocations.AddRange(GetAlternativeSJPLocationsFromDB(searchString, searchLimit));
            }

            // Sort/limit the results
            return SortAndFilterAlternativeSJPLocations(altLocations, searchShowLimit);
        }

        #region Alternative SJP Locations helpers

        /// <summary>
        /// Searches for SJPLocations that are a close match to the supplied search string from cached SJPLocation store
        /// </summary>
        /// <param name="searchString">The name to search for</param>
        /// <param name="searchLimit">Maximum number of search results to return</param>
        /// <returns>SJPLocation[], contains suitable alternatives</returns>
        private static List<SJPLocation> GetAlternativeSJPLocationsFromCache(string searchString, int searchLimit)
        {
            Levenstein levenstein = new Levenstein();

            #region Varibles

            double simindexlimit_NoCommonWords = Properties.Current[Keys.SimilarityIndex_NoCommonWords].Parse(0.5);
            double simindexlimit_NoCommonWordsAndSpace = Properties.Current[Keys.SimilarityIndex_NoCommonWordsAndSpace].Parse(0.5);
            double simindexlimit_IndividualWords = Properties.Current[Keys.SimilarityIndex_IndividualWords].Parse(0.65);

            bool simIndex_ChildLocalityAtEnd = Properties.Current[Keys.SimilarityIndex_ChildLocalityAtEnd].Parse(true);
            
            // contains the matches with the whole search string matched
            Dictionary<SJPLocation, double> levensteinMatchWhole = new Dictionary<SJPLocation, double>();

            // contains the matches when matches  done using part of the search string words matched
            Dictionary<SJPLocation, double> levensteinMatchPart = new Dictionary<SJPLocation, double>();

            // contains the matches when matches  done using part of the search string words matched
            Dictionary<SJPLocation, double> levensteinMatchChildLocality = new Dictionary<SJPLocation, double>();

            // Get the common words from properties
            List<string> commonWords = new List<string>(Properties.Current[Keys.CommonWords].Split(new char[] { ',' }));

            // Remove the common symbols
            commonWords.AddRange(new string[] { ",", ".", "and", "&", "-", "(", ")", "'" });

            #endregion

            // normalise the spaces between word by removing extra spaces
            searchString = System.Text.RegularExpressions.Regex.Replace(searchString, @"\s+", " ");

            // Strip the common words first from both search string and location display name
            string search = StripCommonWords(searchString.ToLower(), commonWords);

            foreach (SJPLocation location in SJPLocationCache.locations)
            {
                bool childLocality = false;

                if (location.TypeOfLocation == SJPLocationType.Locality && !string.IsNullOrEmpty(location.Parent))
                {
                    childLocality = simIndex_ChildLocalityAtEnd;
                }

                // Strip the common words first from both search string and location display name
                string toMatch = StripCommonWords(location.DisplayName.ToLower(), commonWords);

                #region Perform similarity check and add

                // Get the similarity index with the common words stripped
                double simInd = levenstein.GetSimilarity(search, toMatch);

                // Get the similarity index with the common words stripped and the spaced between words
                double simIndNoSpace = levenstein.GetSimilarity(search.Replace(" ", ""), toMatch.Replace(" ", ""));

                // if the location name starts with the search string specified add the location with similarity index as 1
                if (toMatch.StartsWith(search))
                {
                    if (childLocality)
                    {
                        // if child locality put the locations in a separate dictionary
                        if (!levensteinMatchChildLocality.ContainsKey(location))
                        {
                            levensteinMatchChildLocality.Add(location, 0);
                        }
                    }
                    else
                    {
                        if (!levensteinMatchWhole.ContainsKey(location))
                        {
                            levensteinMatchWhole.Add(location, 1);
                        }
                    }
                }

                // if similarity index with only common words stripped is more than 0.5 add the location as match
                else if (simInd > simindexlimit_NoCommonWords)
                {
                    if (childLocality)
                    {
                        // if child locality put the locations in a separate dictionary
                        if (!levensteinMatchChildLocality.ContainsKey(location))
                        {
                            levensteinMatchChildLocality.Add(location, 0);
                        }
                    }
                    else
                    {
                        if (!levensteinMatchWhole.ContainsKey(location))
                        {
                            levensteinMatchWhole.Add(location, simInd);
                        }
                    }
                }
                // We had similarity index less than 0.5 with only common words stripped
                // Lets test if the similarity index is greater than 0.5 with common words and spaces stripped
                else if (simIndNoSpace > simindexlimit_NoCommonWordsAndSpace)
                {
                    if (childLocality)
                    {
                        // if child locality put the locations in a separate dictionary
                        if (!levensteinMatchChildLocality.ContainsKey(location))
                        {
                            levensteinMatchChildLocality.Add(location, 0);
                        }
                    }
                    else
                    {
                        if (!levensteinMatchWhole.ContainsKey(location))
                        {
                            levensteinMatchWhole.Add(location, simInd);
                        }
                    }
                }
                else // this is painful as we have to break the word in tokens and look for the similarity index
                {
                    string[] toMatchTokens = toMatch.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] searchTokens = search.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string st in searchTokens)
                    {
                        foreach (string token in toMatchTokens)
                        {
                            simInd = levenstein.GetSimilarity(st, token);
                                                       
                            // for individual words matching we have to have similarity index higher.
                            // Set it to 65% 
                            if (simInd > simindexlimit_IndividualWords)
                            {
                                if (childLocality)
                                {
                                    // if child locality put the locations in a separate dictionary
                                    if (!levensteinMatchChildLocality.ContainsKey(location))
                                    {
                                        levensteinMatchChildLocality.Add(location, 0);
                                    }
                                }
                                else
                                {
                                    if (!levensteinMatchPart.ContainsKey(location))
                                    {
                                        levensteinMatchPart.Add(location, simInd);
                                    }
                                }
                            }
                            
                        }
                    }
                }

                #endregion
            }

            // Sort the dictionaries and get the first specific number of locations defined by search limit
            Dictionary<SJPLocation, double> filtered = levensteinMatchWhole.OrderByDescending(x => x.Value)
                .Concat(levensteinMatchPart.OrderByDescending(x => x.Value))
                .Concat(levensteinMatchChildLocality.OrderBy(x=>x.Key.DisplayName)) // show locations of type locality with parent locality at very end
                .Take(searchLimit)
                .ToDictionary(x => x.Key, x => x.Value);
            
            return filtered.Keys.ToList();
        }

        /// <summary>
        /// Searches for SJPLocations that are a close match to the supplied search string in database
        /// </summary>
        /// <param name="searchString">The name to search for</param>
        /// <param name="searchLimit">Maximum number of search results to return</param>
        /// <returns>SJPLocation[], contains suitable alternatives</returns>
        private static List<SJPLocation> GetAlternativeSJPLocationsFromDB(string searchString, int searchLimit)
        {
            List<SJPLocation> altLocations = new List<SJPLocation>();
            List<SqlParameter> paramList = new List<SqlParameter>();
            SqlParameter searchparam = new SqlParameter("@searchstring", (System.Data.SqlTypes.SqlString)searchString);
            SqlParameter limitparam = new SqlParameter("@maxRecords", (System.Data.SqlTypes.SqlInt32)searchLimit);
            paramList.Add(searchparam);
            paramList.Add(limitparam);
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);
                    using (SqlDataReader locationsDR = helper.GetReader(SP_GetAlternativeSuggestionList, paramList))
                    {
                        while (locationsDR.Read())
                        {
                            SJPLocation tempLocation = new SJPLocation();

                            tempLocation.DisplayName = locationsDR["DisplayName"].ToString();
                            tempLocation.TypeOfLocation = SJPLocationTypeHelper.GetSJPLocationType(locationsDR["Type"].ToString());
                            tempLocation.TypeOfLocationActual = SJPLocationTypeHelper.GetSJPLocationTypeActual(locationsDR["Type"].ToString());

                            switch (tempLocation.TypeOfLocation)
                            {
                                case SJPLocationType.Venue:
                                    tempLocation.ID = locationsDR["Naptan"].ToString();
                                    break;
                                case SJPLocationType.Station:
                                    tempLocation.ID = locationsDR["Naptan"].ToString();
                                    break;
                                case SJPLocationType.StationGroup:
                                    tempLocation.ID = locationsDR["DATASETID"].ToString();
                                    break;
                                case SJPLocationType.Locality:
                                    tempLocation.ID = locationsDR["LocalityID"].ToString();
                                    break;
                                case SJPLocationType.Postcode:
                                    tempLocation.ID = locationsDR["DisplayName"].ToString();
                                    break;
                                default:
                                    tempLocation.ID = locationsDR["DisplayName"].ToString();
                                    break;
                            }
                            altLocations.Add(tempLocation);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to read Alternative suggestions locations data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }

            return altLocations;
        }

        /// <summary>
        /// Sorts and limits the number of the provided SJPLocations
        /// </summary>
        /// <param name="altLocations"></param>
        /// <returns></returns>
        private static List<SJPLocation> SortAndFilterAlternativeSJPLocations(List<SJPLocation> altLocations, int searchShowLimit)
        {
            List<SJPLocation> sortedLocations = new List<SJPLocation>();

            // Limits to apply for location types if more than max show limit exceeded
            int searchShowLimitGroup = Properties.Current[Keys.Max_SearchLocationsShowLimit_GroupStations].Parse(100);
            int searchShowLimitRail = Properties.Current[Keys.Max_SearchLocationsShowLimit_RailStations].Parse(5);
            int searchShowLimitCoach = Properties.Current[Keys.Max_SearchLocationsShowLimit_CoachStations].Parse(2);
            int searchShowLimitTram = Properties.Current[Keys.Max_SearchLocationsShowLimit_TramStations].Parse(5);
            int searchShowLimitFerry = Properties.Current[Keys.Max_SearchLocationsShowLimit_FerryStations].Parse(100);
            int searchShowLimitAirport = Properties.Current[Keys.Max_SearchLocationsShowLimit_AirportStations].Parse(100);

            if ((altLocations != null) && (altLocations.Count > 0))
            {
                // Only apply the loc type limits if more than show limit number of locations
                bool applySortLimits = (altLocations.Count > searchShowLimit);

                // Sort order is  as follows:
                // 1. Exchange groups (group stations)
                // 2. Rail stations
                // 3. Localities
                // 4. Coach stations
                // 5. TMU (tram, metros, underground)
                // 6. Ferry
                // 7. Airport
                // 8. Other (all other types)

                // Assume the locations provided have already been placed in an initial order, 
                // e.g. the "closer" matches are first
                // Therefore we want to preserve the locations order, but grouped as above

                #region Filter the location types

                // Extract the locations for each group type
                List<SJPLocation> locStationGroups = altLocations.FindAll(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationGroup; });
                List<SJPLocation> locStationsRail = altLocations.FindAll(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationRail; });
                List<SJPLocation> locLocalities = altLocations.FindAll(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.Locality; });
                List<SJPLocation> locStationsCoach = altLocations.FindAll(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationCoach; });
                List<SJPLocation> locStationsTMU = altLocations.FindAll(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationTMU; });
                List<SJPLocation> locStationsFerry = altLocations.FindAll(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationFerry; });
                List<SJPLocation> locStationsAirport = altLocations.FindAll(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationAirport; });
                List<SJPLocation> locOther = altLocations.FindAll(delegate(SJPLocation loc) { return (loc.TypeOfLocationActual == SJPLocationType.Unknown)
                                                                                                     || (loc.TypeOfLocationActual == SJPLocationType.Venue)
                                                                                                     || (loc.TypeOfLocationActual == SJPLocationType.Postcode)
                                                                                                     || (loc.TypeOfLocationActual == SJPLocationType.Station);
                                                                                            });
                #endregion

                #region Add the locations to the sorted list

                // - Exchange Groups
                if (applySortLimits && (locStationGroups.Count > searchShowLimitGroup))
                {
                    sortedLocations.AddRange(locStationGroups.Take(searchShowLimitGroup));
                }
                else
                {
                    sortedLocations.AddRange(locStationGroups);
                }

                // - Rail Stations
                if (applySortLimits && (locStationsRail.Count > searchShowLimitRail))
                {
                    sortedLocations.AddRange(locStationsRail.Take(searchShowLimitRail));
                }
                else
                {
                    sortedLocations.AddRange(locStationsRail);
                }

                // - Localities 
                // These are used to fill out the list if needed, therefore skip over for now and insert last
                int localitiesIndex = sortedLocations.Count;
                
                // - Coach stations
                if (applySortLimits && (locStationsCoach.Count > searchShowLimitCoach))
                {
                    sortedLocations.AddRange(locStationsCoach.Take(searchShowLimitCoach));
                }
                else
                {
                    sortedLocations.AddRange(locStationsCoach);
                }

                // - TMU
                if (applySortLimits && (locStationsTMU.Count > searchShowLimitTram))
                {
                    sortedLocations.AddRange(locStationsTMU.Take(searchShowLimitTram));
                }
                else
                {
                    sortedLocations.AddRange(locStationsTMU);
                }

                // - Ferry
                if (applySortLimits && (locStationsFerry.Count > searchShowLimitFerry))
                {
                    sortedLocations.AddRange(locStationsFerry.Take(searchShowLimitFerry));
                }
                else
                {
                    sortedLocations.AddRange(locStationsFerry);
                }

                // - Airport
                if (applySortLimits && (locStationsAirport.Count > searchShowLimitAirport))
                {
                    sortedLocations.AddRange(locStationsAirport.Take(searchShowLimitAirport));
                }
                else
                {
                    sortedLocations.AddRange(locStationsAirport);
                }

                // - Other
                sortedLocations.AddRange(locOther);

                // Insert the localities, upto the maximum number
                if ((locLocalities.Count > 0) && (sortedLocations.Count < searchShowLimit))
                {
                    if (locLocalities.Count > (searchShowLimit - sortedLocations.Count))
                    {
                        sortedLocations.InsertRange(localitiesIndex, locLocalities.Take(searchShowLimit - sortedLocations.Count));
                    }
                    else
                    {
                        sortedLocations.InsertRange(localitiesIndex, locLocalities);
                    }
                }

                #endregion
            }

            // Return the sorted locations, using the limit in case the above logic added too many
            return sortedLocations.Take(searchShowLimit).ToList();
        }

        /// <summary>
        /// Strips out common occurring words from the target string
        /// </summary>
        /// <param name="targetString">Target string from which common words need removing</param>
        /// <param name="commonWords">List of common words to be removed from the target string</param>
        /// <returns>Target string with all the common words removed i.e coach, station, rail, etc..</returns>
        private static string StripCommonWords(string targetString, List<string> commonWords)
        {
            foreach (string word in commonWords)
            {
                targetString = targetString.ToLower().Replace(word.ToLower(), "").Trim();
            }

            return targetString;
        }

        #endregion

        /// <summary>
        /// Loads all Locations data
        /// </summary>
        internal static void LoadLocations()
        {
            // Make load threadsafe
            if (!dataLocationsInitialised)
            {
                lock (dataInitialisedLock)
                {
                    // Check if locations should be cached, potentially a very large dataset so
                    // make it switchable
                    useLocationCache = Properties.Current[Keys.Cache_LoadLocations].Parse(false);

                    if (!dataLocationsInitialised)
                    {
                        // Load data if use cache flag has been set
                        if (useLocationCache)
                        {
                            PopulateLocationsData();
                        }

                        // Set to true here (prevents repeated attempts to load if it fails)
                        dataLocationsInitialised = true;
                    }
                }
            }
        }

        /// <summary>
        /// Loads all Postcode data
        /// </summary>
        internal static void LoadPostcodes()
        {
            // Make load threadsafe
            if (!dataPostcodesInitialised)
            {
                lock (dataInitialisedLock)
                {
                    // Check if postcodes shoule be cached, potentially a very large dataset so
                    // make it switchable
                    usePostcodeCache = Properties.Current[Keys.Cache_LoadPostcodes].Parse(false);

                    if (!dataPostcodesInitialised)
                    {
                        // Load data if use cache flag has been set
                        if (usePostcodeCache)
                        {
                            PopulatePostcodesData();
                        }

                        // Set to true here (prevents repeated attempts to load if it fails)
                        dataPostcodesInitialised = true;
                    }
                }
            }
        }
        #endregion
    }
}
