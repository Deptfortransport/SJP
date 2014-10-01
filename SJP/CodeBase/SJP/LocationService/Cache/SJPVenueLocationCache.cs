// *********************************************** 
// NAME             : SJPVenueLocationCache.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 21 Feb 2011
// DESCRIPTION  	: Helper class to provide methods to obtain 
//                    Olympic Venue Location Information these are cached in memory
//                    to avoid repeated database calls.
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// SJPVenueLocationCache helper class to provide methods to obtain 
    /// Olympic Venue Location Information these are cached in memory 
    /// to avoid repeated database calls.
    /// </summary>
    static class SJPVenueLocationCache
    {
        #region Private members

        private static readonly object dataInitialisedLock = new object();
        private static bool dataInitialised = false;
        private static List<SJPLocation> venuesList = new List<SJPLocation>();
        private static List<SJPLocation> venueLocations = new List<SJPLocation>();
        /// <summary>
        /// Venue Cycle parks
        /// </summary>
        private static List<SJPVenueCyclePark> cycleParks = new List<SJPVenueCyclePark>();
        private static Dictionary<string, List<string>> venueCycleParks = new Dictionary<string, List<string>>();
        
        /// <summary>
        /// Venue Car parks
        /// </summary>
        private static List<SJPVenueCarPark> carParks = new List<SJPVenueCarPark>();
        private static Dictionary<string, List<string>> venueCarParks = new Dictionary<string, List<string>>();
        
        /// <summary>
        /// Venue River services data -  Dictionary<VenueNaPTAN, List<SJPVenueCyclePark>>
        /// </summary>
        private static Dictionary<string, List<SJPVenueRiverService>> venueRiverServices = new Dictionary<string, List<SJPVenueRiverService>>();

        /// <summary>
        /// Venue pier navigation path data -  Dictionary<VenueNaPTAN, List<SJPVenueCyclePark>>
        /// </summary>
        private static Dictionary<string, List<SJPPierVenueNavigationPath>> pierVenueNavigationPaths = new Dictionary<string, List<SJPPierVenueNavigationPath>>();

        /// <summary>
        /// Venue gate data -  Dictionary <VenueGateNaPTAN, List<SJPVenueGate>>
        /// </summary>
        private static Dictionary<string, List<SJPVenueGate>> venueGates = new Dictionary<string, List<SJPVenueGate>>();

        /// <summary>
        /// Venue gate navigation path data -  Dictionary <VenueGateNaPTAN, List<SJPVenueGateNavigationPath>>
        /// </summary>
        private static Dictionary<string, List<SJPVenueGateNavigationPath>> venueGateNavigationPaths = new Dictionary<string, List<SJPVenueGateNavigationPath>>();

        /// <summary>
        /// Venue gate check constraint -  Dictionary <VenueGateNaPTAN, List<SJPVenueGateCheckConstraint>>
        /// </summary>
        private static Dictionary<string, List<SJPVenueGateCheckConstraint>> venueGateCheckConstraints = new Dictionary<string, List<SJPVenueGateCheckConstraint>>();

        /// <summary>
        /// Venue access data - Dictionary<VenueNaPTAN, List<SJPVenueAccess>>
        /// </summary>
        private static Dictionary<string, List<SJPVenueAccess>> venueAccessData = new Dictionary<string, List<SJPVenueAccess>>();

        #endregion

        #region Constructor

        /// <summary>
        /// Static Constructor
        /// </summary>
        static SJPVenueLocationCache()
        {
            LoadVenues();
        }

        #endregion

        #region Private methods

        #region Venues

        /// <summary>
        /// Populates the Venue cache by retrieveing the source data from the database.
        /// </summary>
        private static void PopulateVenuesData()
        {
            // Build Venue Locations List
            using (SqlHelper helper = new SqlHelper())
            {
                // Temp lists before assigning to the static lists
                List<SJPLocation> tmpVenuesList = new List<SJPLocation>();
                List<SJPLocation> tmpVenueLocations = new List<SJPLocation>();

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venues location data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the summary venue List
                    using (SqlDataReader listDR = helper.GetReader("GetVenuesList", paramList))
                    {
                        while (listDR.Read())
                        {
                            // Add to venues list
                            tmpVenuesList.Add(
                                new SJPLocation(
                                    listDR["DisplayName"].ToString(),
                                    SJPLocationType.Venue,
                                    SJPLocationType.Venue,
                                    listDR["Naptan"].ToString().ToUpper()));
                        }
                    }

                    // Read and populate the detailed venue location array
                    using (SqlDataReader locationsDR = helper.GetReader("GetVenueLocations", paramList))
                    {
                        while (locationsDR.Read())
                        {
                            List<string> toids = new List<string>();
                            toids.Add(locationsDR["NearestTOID"].ToString());

                            List<string> naptans = new List<string>();
                            naptans.Add(locationsDR["Naptan"].ToString());

                            // Read venue values
                            string name = locationsDR["Name"].ToString();
                            string displayName = locationsDR["DisplayName"].ToString();
                            string locality = locationsDR["LocalityID"].ToString();
                            string parentId = locationsDR["ParentID"].ToString();
                            float easting = (float)Convert.ToDouble(locationsDR["Easting"].ToString());
                            float northing = (float)Convert.ToDouble(locationsDR["Northing"].ToString());
                            OSGridReference osgr = new OSGridReference(easting, northing);
                            float nearestEasting = (float)Convert.ToDouble(locationsDR["NearestPointE"].ToString());
                            float nearestNorthing = (float)Convert.ToDouble(locationsDR["NearestPointN"].ToString());
                            OSGridReference nearestOsgr = new OSGridReference(nearestEasting, nearestNorthing);
                            int adminAreaID = (int)Convert.ToInt32((locationsDR["AdminAreaID"] != DBNull.Value) ? locationsDR["AdminAreaID"].ToString() : "0");
                            int districtID = (int)Convert.ToInt32((locationsDR["DistrictID"] != DBNull.Value) ? locationsDR["DistrictID"].ToString() : "0");
                            
                            // Read venue addtional data values
                            int cycleToVenueDistance = Convert.ToInt32(locationsDR["CycleToVenueDistance"].ToString());
                            string venueMapURL = locationsDR["VenueMapURL"].ToString();
                            string travelNewsRegion = locationsDR["VenueTravelNewsRegion"].ToString();
                            string venueWalkingRoutesMapURL = locationsDR["VenueWalkingRoutesMapURL"].ToString();
                            bool isGNAT = Convert.ToBoolean(locationsDR["AccesibleJourneyToVenue"].ToString()); // isGNAT
                            bool useNaPTAN = Convert.ToBoolean(locationsDR["UseNaPTANforJourneyPlanning"].ToString());
                            string riverServiceAvailable = locationsDR["VenueRiverServiceAvailable"].ToString();
                            string venueGroupID = locationsDR["VenueGroupID"].ToString();
                            string venueGroupName = locationsDR["VenueGroupName"].ToString();
                                                       
                            // Create venue location
                            SJPVenueLocation sjpLocation = new SJPVenueLocation(name, displayName, locality, toids, naptans, parentId,
                                SJPLocationType.Venue, osgr, nearestOsgr, isGNAT, useNaPTAN,
                                adminAreaID, districtID, naptans[0]);

                            // Set the additional venue location specific values
                            sjpLocation.VenueMapUrl = venueMapURL;
                            sjpLocation.VenueTravelNewsRegion = travelNewsRegion;
                            sjpLocation.VenueWalkingRoutesUrl = venueWalkingRoutesMapURL;
                            sjpLocation.CycleToVenueDistance = cycleToVenueDistance;
                            sjpLocation.VenueRiverServiceAvailable = RiverServiceAvailableTypeHelper.GetRiverServiceAvailableType(riverServiceAvailable);
                            sjpLocation.VenueGroupID = venueGroupID;
                            sjpLocation.VenueGroupName = venueGroupName;
                                                        
                            // Add to venues locations list
                            tmpVenueLocations.Add(sjpLocation);
                        }
                    }

                   
                    // Assign to static lists
                    venuesList = tmpVenuesList;
                    venueLocations = tmpVenueLocations;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue locations in cache [{0}]", venueLocations.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venues location data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venues location data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        #endregion

        #region Access

        /// <summary>
        /// Populates venue access data for the SJP venue locations
        /// </summary>
        private static void PopulateVenueAccessData()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                //Dictionary<VenueNaPTAN, List<SJPVenueAccess>>
                Dictionary<string, List<SJPVenueAccess>> tmpVenueAccessData = new Dictionary<string, List<SJPVenueAccess>>();
                List<SJPVenueAccess> tmpVenueAccessList = null;

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue access data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the venue access list stations data
                    using (SqlDataReader venueAccessDR = helper.GetReader("GetVenueAccessData", paramList))
                    {
                        while (venueAccessDR.Read())
                        {
                            // Read Values
                            string venueNaptan = venueAccessDR["VenueNaPTAN"].ToString();
                            string venueName = venueAccessDR["VenueName"].ToString();

                            DateTime accessFrom = venueAccessDR.IsDBNull(venueAccessDR.GetOrdinal("AccessFrom")) ?
                                DateTime.MinValue : Convert.ToDateTime(venueAccessDR["AccessFrom"]);
                            DateTime accessTo = venueAccessDR.IsDBNull(venueAccessDR.GetOrdinal("AccessTo")) ?
                                DateTime.MaxValue : Convert.ToDateTime(venueAccessDR["AccessTo"]);

                            TimeSpan accessToVenueDuration = venueAccessDR.IsDBNull(venueAccessDR.GetOrdinal("AccessDuration")) ?
                                TimeSpan.Zero : venueAccessDR.GetTimeSpan(venueAccessDR.GetOrdinal("AccessDuration"));

                            string stationNaptan = venueAccessDR["StationNaPTAN"].ToString();
                            string stationName = venueAccessDR["StationName"].ToString();

                            string transferText = venueAccessDR["TransferText"].ToString();
                            Language transferLanguage = LanguageHelper.ParseLanguage(venueAccessDR["TransferLanguage"].ToString());
                            bool transferToVenue = venueAccessDR.GetBoolean(venueAccessDR.GetOrdinal("TransferToVenue"));

                            // Create data objects
                            SJPVenueAccess sjpVenueAccess = new SJPVenueAccess(venueNaptan, venueName, accessFrom, accessTo, accessToVenueDuration);
                            SJPVenueAccessStation sjpVenueAccessStation = new SJPVenueAccessStation(stationNaptan, stationName);

                            // Update the temp data list
                            if (!string.IsNullOrEmpty(venueNaptan))
                            {
                                #region Get/Set venue access list

                                // Create new list if venue naptan hasnt been setup
                                if (!tmpVenueAccessData.ContainsKey(venueNaptan))
                                {
                                    tmpVenueAccessData.Add(venueNaptan, new List<SJPVenueAccess>());
                                }

                                tmpVenueAccessList = tmpVenueAccessData[venueNaptan];

                                #endregion

                                #region Get/Set venue access and station

                                bool vaExists = false;
                                bool vasExists = false;

                                // Check if the SJPVenueAccess has already been created.
                                // Because a VenueAccess object can have multiple stations for an access period, 
                                // need to update the existing object if available
                                foreach (SJPVenueAccess va in tmpVenueAccessList)
                                {
                                    if (va.AccessFrom == accessFrom
                                        && va.AccessTo == accessTo)
                                    {
                                        // Overwrite the local new VenueAccess object
                                        sjpVenueAccess = va;

                                        // Flag the venue access already existed
                                        vaExists = true;
                                    }
                                }

                                // Check if the SJPVenueAccess has the SJPVenueAccessStation
                                // Because a SJPVenueAccessStation object can have multiple transfer texts (e.g. language, direction), 
                                // need to update the existing object if available
                                foreach (SJPVenueAccessStation vas in sjpVenueAccess.Stations)
                                {
                                    if (vas.StationNaPTAN.Equals(stationNaptan))
                                    {
                                        // Overwrite the local new VenueAccessStation object
                                        sjpVenueAccessStation = vas;

                                        // Flag the venue access station already existed
                                        vasExists = true;
                                    }
                                }

                                #endregion

                                // Update the transfer text for this venue station
                                sjpVenueAccessStation.AddTransferText(transferText, transferLanguage, transferToVenue);

                                #region Update temp list

                                if (!vasExists)
                                {
                                    // Because this is a new station, add it to the venue access data
                                    sjpVenueAccess.Stations.Add(sjpVenueAccessStation);
                                }

                                // Because this is a new venue access, add it to the tmp list cache
                                if (!vaExists)
                                {
                                    tmpVenueAccessList.Add(sjpVenueAccess);
                                }

                                #endregion
                            }
                        }
                    }

                    // Assign to static lists
                    venueAccessData = tmpVenueAccessData;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue access data in cache [{0}]", venueAccessData.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue access data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue access data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        #endregion

        #region Cycle parks

        /// <summary>
        /// Populates cycle parks for the SJP venue locations in a dictionary with dictionary key as venue NaPTAN
        /// </summary>
        /// <param name="location"></param>
        private static void PopulateVenueCycleParks()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                //  Temp lists to use while populating
                List<SJPVenueCyclePark> tmpCycleParks = new List<SJPVenueCyclePark>();
                Dictionary<string, List<string>> tmpVenueCycleParks = new Dictionary<string, List<string>>();

                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue cycle park data"  ));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                   
                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);
                    
                    // Read and populate the detailed venue cycle park data
                    using (SqlDataReader cycleParkDR = helper.GetReader("GetAllVenueCycleParks", paramList))
                    {
                        while (cycleParkDR.Read())
                        { 
                            SJPVenueCyclePark venuePark = new SJPVenueCyclePark();
                            // Read Values
                            venuePark.ID = cycleParkDR["CycleParkID"].ToString();
                            venuePark.Name = cycleParkDR["CycleParkName"].ToString();
                            venuePark.VenueNaPTAN = cycleParkDR["VenueServed"].ToString();
                            venuePark.CycleParkMapUrl = cycleParkDR["CycleParkMapURL"].ToString();
                            venuePark.StorageType = cycleParkDR.IsDBNull(cycleParkDR.GetOrdinal("StorageType")) ? 
                                CycleStorageType.Loops : (CycleStorageType) Enum.Parse(typeof(CycleStorageType), cycleParkDR["StorageType"].ToString());

                            float eastingTo = (float)Convert.ToDouble(cycleParkDR["CycleToEasting"].ToString());
                            float northingTo = (float)Convert.ToDouble(cycleParkDR["CycleToNorthing"].ToString());
                            venuePark.CycleToGridReference = new OSGridReference(eastingTo, northingTo);

                            float eastingFrom = (float)Convert.ToDouble(cycleParkDR["CycleFromEasting"].ToString());
                            float northingFrom = (float)Convert.ToDouble(cycleParkDR["CycleFromNorthing"].ToString());
                            venuePark.CycleFromGridReference = new OSGridReference(eastingFrom, northingFrom);

                            venuePark.NumberOfSpaces = cycleParkDR.IsDBNull(cycleParkDR.GetOrdinal("NumberOfSpaces")) ? 
                                0 : Convert.ToInt32(cycleParkDR["NumberOfSpaces"].ToString());

                            venuePark.VenueGateEntranceNaPTAN = cycleParkDR["VenueEntranceGate"].ToString();
                            venuePark.WalkToGateDuration = cycleParkDR.IsDBNull(cycleParkDR.GetOrdinal("WalkToGateDuration")) ? 
                                new TimeSpan() : cycleParkDR.GetTimeSpan(cycleParkDR.GetOrdinal("WalkToGateDuration"));

                            venuePark.VenueGateExitNaPTAN = cycleParkDR["VenueExitGate"].ToString();
                            venuePark.WalkFromGateDuration = cycleParkDR.IsDBNull(cycleParkDR.GetOrdinal("WalkFromGateDuration")) ?
                                new TimeSpan() : cycleParkDR.GetTimeSpan(cycleParkDR.GetOrdinal("WalkFromGateDuration"));


                            // Store in cycleparks list
                            tmpCycleParks.Add(venuePark);

                            // Store in venue's cycleparks list
                            if (tmpVenueCycleParks.ContainsKey(venuePark.VenueNaPTAN))
                            {
                                tmpVenueCycleParks[venuePark.VenueNaPTAN].Add(venuePark.ID);
                            }
                            else
                            {
                                List<string> cycleParkList = new List<string>();
                                cycleParkList.Add(venuePark.ID);
                                tmpVenueCycleParks.Add(venuePark.VenueNaPTAN, cycleParkList);
                            }
                        }
                    }

                    foreach (SJPVenueCyclePark cyclePark in tmpCycleParks)
                    {
                        // load cycle park availability
                        PopulateVenueCycleParkAvaiability(cyclePark);
                    }

                    // Assign to static lists
                    cycleParks = tmpCycleParks;
                    venueCycleParks = tmpVenueCycleParks;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue cycle parks in cache [{0}]", cycleParks.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue cycle park data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue cycle park data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates cycle par availability conditions
        /// </summary>
        /// <param name="cyclePark"></param>
        private static void PopulateVenueCycleParkAvaiability(SJPVenueCyclePark cyclePark)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                List<SJPParkAvailability> availability = new List<SJPParkAvailability>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue cycle park availability data for: " + cyclePark.Name));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@CycleParkID", cyclePark.ID));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue cycle parks availability data
                    using (SqlDataReader parkAvailabilityDR = helper.GetReader("GetCycleVenueParkAvailability", paramList))
                    {
                        while (parkAvailabilityDR.Read())
                        {
                            
                            // Read Values
                            DateTime fromDate = parkAvailabilityDR.GetDateTime(parkAvailabilityDR.GetOrdinal("FromDate"));
                            DateTime toDate = parkAvailabilityDR.GetDateTime(parkAvailabilityDR.GetOrdinal("ToDate"));
                            TimeSpan dailyOpeningTime = parkAvailabilityDR.GetTimeSpan(parkAvailabilityDR.GetOrdinal("DailyOpeningTime"));
                            TimeSpan dailyClosingTime = parkAvailabilityDR.GetTimeSpan(parkAvailabilityDR.GetOrdinal("DailyClosingTime"));
                            DaysOfWeek daysOpen = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), parkAvailabilityDR["DaysOfWeek"].ToString());

                            SJPParkAvailability parkAvailability = new SJPParkAvailability(fromDate,toDate,dailyOpeningTime,dailyClosingTime,daysOpen);
                            availability.Add(parkAvailability);
                        }
                    }

                    cyclePark.Availability = availability;
                    
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue cycle park availability data completed for: " + cyclePark.Name));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venues cycle park availability data for {0}: {1}", cyclePark.Name, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        #endregion

        #region Car parks

        /// <summary>
        /// Populates car parks for the SJP venue locations in a dictionary with key as venue NaPTAN
        /// </summary>
        /// <param name="location"></param>
        private static void PopulateVenueCarParks()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                //  Temp lists to use while populating
                List<SJPVenueCarPark> tmpCarParks = new List<SJPVenueCarPark>();
                Dictionary<string, List<string>> tmpVenueCarParks = new Dictionary<string, List<string>>();
                
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue car park data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue car parks data
                    using (SqlDataReader carParkDR = helper.GetReader("GetAllVenueCarParks", paramList))
                    {
                        while (carParkDR.Read())
                        {
                            SJPVenueCarPark venuePark = new SJPVenueCarPark();
                            // Read Values
                            venuePark.ID = carParkDR["CarParkID"].ToString();
                            venuePark.Name = carParkDR["CarParkName"].ToString();
                            venuePark.VenueNaPTAN = carParkDR["VenueServed"].ToString();
                            venuePark.MapOfSiteUrl = carParkDR["MapOfSiteUrl"].ToString();
                            venuePark.InterchangeDuration = carParkDR.IsDBNull(carParkDR.GetOrdinal("InterchangeDuration")) ? 
                                0 : Convert.ToInt32(carParkDR["InterchangeDuration"].ToString());
                            venuePark.CoachSpaces = carParkDR.IsDBNull(carParkDR.GetOrdinal("CoachSpaces")) ? 
                                0 : Convert.ToInt32(carParkDR["CoachSpaces"].ToString());
                            venuePark.CarSpaces = carParkDR.IsDBNull(carParkDR.GetOrdinal("CarSpaces")) ? 
                                0 : Convert.ToInt32(carParkDR["CarSpaces"].ToString());
                            venuePark.DisabledSpaces = carParkDR.IsDBNull(carParkDR.GetOrdinal("DisabledSpaces")) ? 
                                0 : Convert.ToInt32(carParkDR["DisabledSpaces"].ToString());
                            venuePark.BlueBadgeSpaces = carParkDR.IsDBNull(carParkDR.GetOrdinal("BlueBadgeSpaces")) ? 
                                0 : Convert.ToInt32(carParkDR["BlueBadgeSpaces"].ToString());
                            venuePark.DriveFromToid = carParkDR["DriveFromToid"].ToString();
                            venuePark.DriveToToid = carParkDR["DriveToToid"].ToString();

                            // Store in carparks list
                            tmpCarParks.Add(venuePark);

                            // Store in venue's carparks list
                            if (tmpVenueCarParks.ContainsKey(venuePark.VenueNaPTAN))
                            {
                                tmpVenueCarParks[venuePark.VenueNaPTAN].Add(venuePark.ID);
                            }
                            else
                            {
                                List<string> carParkList = new List<string>();
                                carParkList.Add(venuePark.ID);
                                tmpVenueCarParks.Add(venuePark.VenueNaPTAN, carParkList);
                            }
                        }
                    }

                    foreach (SJPVenueCarPark carPark in tmpCarParks)
                    {
                        // load car park availability
                        PopulateVenueCarParkAvailability(carPark);
                        // load car park transit shuttles
                        PopulateVenueCarParkTransitShuttles(carPark);
                        // load car park infrmation
                        PopulateVenueCarParkInformation(carPark);
                    }
                                        
                    // Assign to static lists
                    carParks = tmpCarParks;
                    venueCarParks = tmpVenueCarParks;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue car parks in cache [{0}]", carParks.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue car park data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue car park data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates transit shuttles data for the car park
        /// </summary>
        /// <param name="carPark">SJP Car Park</param>
        private static void PopulateVenueCarParkTransitShuttles(SJPVenueCarPark carPark)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                List<TransitShuttle> carParkTransitShuttles = new List<TransitShuttle>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park transit shuttle data for: " + carPark.Name));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@CarParkID", carPark.ID));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue car park transit shuttles data
                    using (SqlDataReader parkTransitShuttlesDR = helper.GetReader("GetVenueCarParkTransitShuttles", paramList))
                    {
                        while (parkTransitShuttlesDR.Read())
                        {
                            TransitShuttle transitShuttle = new TransitShuttle();
                            transitShuttle.ID = parkTransitShuttlesDR["TransitShuttleID"].ToString();
                            transitShuttle.ToVenue = Convert.ToBoolean(parkTransitShuttlesDR["ToVenue"].ToString());
                            transitShuttle.TransitDuration = parkTransitShuttlesDR.IsDBNull(parkTransitShuttlesDR.GetOrdinal("TransitDuration")) ?
                                0 : Convert.ToInt32(parkTransitShuttlesDR["TransitDuration"].ToString());
                            transitShuttle.VenueGateToUse = parkTransitShuttlesDR["VenueGateNaPTAN"].ToString();
                            transitShuttle.IsPRMOnly = Convert.ToBoolean(parkTransitShuttlesDR["IsPRMOnly"].ToString());
                            transitShuttle.IsScheduledService = Convert.ToBoolean(parkTransitShuttlesDR["IsScheduledService"].ToString());
                            transitShuttle.ServiceFrequency = parkTransitShuttlesDR.IsDBNull(parkTransitShuttlesDR.GetOrdinal("ServiceFrequency")) ?
                                0 : Convert.ToInt32(parkTransitShuttlesDR["ServiceFrequency"].ToString());
                            transitShuttle.ServiceStartTime = parkTransitShuttlesDR.GetTimeSpan(parkTransitShuttlesDR.GetOrdinal("FirstServiceOfDay"));
                            transitShuttle.ServiceEndTime = parkTransitShuttlesDR.GetTimeSpan(parkTransitShuttlesDR.GetOrdinal("LastServiceOfDay"));
                            transitShuttle.ModeOfTransport = (ParkingInterchangeMode)Enum.Parse(typeof(ParkingInterchangeMode), parkTransitShuttlesDR["ModeOfTransport"].ToString(), true);

                            carParkTransitShuttles.Add(transitShuttle);
                        }
                    }

                    foreach (TransitShuttle transitShuttle in carParkTransitShuttles)
                    {
                        // load car park transit shuttle transfers
                        PopulateVenueCarParkTransitShuttleTransfers(transitShuttle);

                        // load transit shuttle availability
                        PopulateVenueCarParkTransitShuttleAvailability(transitShuttle);
                    }

                    carPark.TransitShuttles = carParkTransitShuttles;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park transit shuttle data completed for: " + carPark.Name));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venues car park transit shuttle data for {0}: {1}", carPark.Name, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates transit shuttles transfer data for an TransitShuttle
        /// </summary>
        /// <param name="carPark">SJP Car Park</param>
        private static void PopulateVenueCarParkTransitShuttleTransfers(TransitShuttle transitShuttle)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park transit shuttle transfer data for: " + transitShuttle.ID));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@TransitShuttleID", transitShuttle.ID));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue car park transit shuttle transfer data
                    using (SqlDataReader dr = helper.GetReader("GetVenueCarParkTransitShuttleTransfers", paramList))
                    {
                        while (dr.Read())
                        {
                            string transferText = dr["TransferText"].ToString();
                            Language transferLanguage = LanguageHelper.ParseLanguage(dr["TransferLanguage"].ToString());

                            // Update the transfer text for this transit shuttle
                            transitShuttle.AddTransferText(transferText, transferLanguage);
                        }
                    }

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park transit shuttle transfer data completed for: " + transitShuttle.ID));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venues car park transit shuttle transfer data for {0}: {1}", transitShuttle.ID, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }
        

        /// <summary>
        /// Populates transit shuttles availability data for a TransitShuttle
        /// </summary>
        /// <param name="carPark">SJP Car Park</param>
        private static void PopulateVenueCarParkTransitShuttleAvailability(TransitShuttle transitShuttle)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                List<SJPParkAvailability> availability = new List<SJPParkAvailability>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park transit shuttle availability data for: " + transitShuttle.ID));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@TransitShuttleID", transitShuttle.ID));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue car park transit shuttle transfer data
                    using (SqlDataReader dr = helper.GetReader("GetTransitShuttleAvailability", paramList))
                    {
                        while (dr.Read())
                        {
                            // Read Values
                            DateTime fromDate = dr.GetDateTime(dr.GetOrdinal("FromDate"));
                            DateTime toDate = dr.GetDateTime(dr.GetOrdinal("ToDate"));
                            TimeSpan dailyStartTime = dr.GetTimeSpan(dr.GetOrdinal("DailyStartTime"));
                            TimeSpan dailyEndTime = dr.GetTimeSpan(dr.GetOrdinal("DailyEndTime"));
                            DaysOfWeek daysOpen = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), dr["DaysOfWeek"].ToString());

                            SJPParkAvailability shuttleAvailability = new SJPParkAvailability(fromDate, toDate, dailyStartTime, dailyEndTime, daysOpen);
                            availability.Add(shuttleAvailability);

                        }

                        transitShuttle.Availability = availability;
                    }

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park transit shuttle availability data completed for: " + transitShuttle.ID));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue car park transit shuttle availability data for {0}: {1}", transitShuttle.ID, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }
        /// <summary>
        /// Populates cycle par availability conditions
        /// </summary>
        /// <param name="carPark"></param>
        private static void PopulateVenueCarParkAvailability(SJPVenueCarPark carPark)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                List<SJPParkAvailability> availability = new List<SJPParkAvailability>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park availability data for: " + carPark.Name));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@CarParkID", carPark.ID));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue car park availability
                    using (SqlDataReader parkAvailabilityDR = helper.GetReader("GetVenueCarParkAvailability", paramList))
                    {
                        while (parkAvailabilityDR.Read())
                        {

                            // Read Values
                            DateTime fromDate = parkAvailabilityDR.GetDateTime(parkAvailabilityDR.GetOrdinal("FromDate"));
                            DateTime toDate = parkAvailabilityDR.GetDateTime(parkAvailabilityDR.GetOrdinal("ToDate"));
                            TimeSpan dailyOpeningTime = parkAvailabilityDR.GetTimeSpan(parkAvailabilityDR.GetOrdinal("DailyOpeningTime"));
                            TimeSpan dailyClosingTime = parkAvailabilityDR.GetTimeSpan(parkAvailabilityDR.GetOrdinal("DailyClosingTime"));
                            DaysOfWeek daysOpen = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), parkAvailabilityDR["DaysOfWeek"].ToString());

                            SJPParkAvailability parkAvailability = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
                            availability.Add(parkAvailability);
                        }
                    }

                    carPark.Availability = availability;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park availability data completed for: " + carPark.Name));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venues car park availability data for {0}: {1}", carPark.Name, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }
        
        /// <summary>
        /// Populates venur car park information
        /// </summary>
        /// <param name="carPark"></param>
        private static void PopulateVenueCarParkInformation(SJPVenueCarPark carPark)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park information for: " + carPark.Name));

                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(new SqlParameter("@CarParkID", carPark.ID));

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue car park availability
                    using (SqlDataReader parkInformationDR = helper.GetReader("GetVenueCarParkInformation", paramList))
                    {
                        while (parkInformationDR.Read())
                        {
                            // Read Values
                            string informationText = parkInformationDR["InformationText"].ToString();
                            Language language = LanguageHelper.ParseLanguage(parkInformationDR["CultureCode"].ToString());

                            // Update the information text for this car park
                            carPark.AddInformationText(informationText, language);
                        }
                    }

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "Loading Venue car park information data completed for: " + carPark.Name));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venues car park information data for {0}: {1}", carPark.Name, ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }
        #endregion

        #region River services

        /// <summary>
        /// Populates river services for the SJP venue locations in a dictionary with dictionary key as venue NaPTAN
        /// </summary>
        /// <param name="location"></param>
        private static void PopulateVenueRiverServices()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                // Dictionary<VenueNaPTAN, List<SJPVenueRiverService>> tmpVenueRiverServices 
                Dictionary<string, List<SJPVenueRiverService>> tmpVenueRiverServices = new Dictionary<string, List<SJPVenueRiverService>>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue river services data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue river services
                    using (SqlDataReader riverServicesDR = helper.GetReader("GetAllVenueRiverSerivces", paramList))
                    {
                        while (riverServicesDR.Read())
                        {
                            SJPVenueRiverService riverService = new SJPVenueRiverService();
                            // Read Values
                            riverService.VenueNaPTAN = riverServicesDR["VenueNaPTAN"].ToString();
                            riverService.VenuePierNaPTAN = riverServicesDR["VenuePierNaPTAN"].ToString();
                            riverService.RemotePierNaPTAN = riverServicesDR["RemotePierNaPTAN"].ToString();
                            riverService.VenuePierName = riverServicesDR["VenuePierName"].ToString();
                            riverService.RemotePierName = riverServicesDR["RemotePierName"].ToString();


                            if (tmpVenueRiverServices.ContainsKey(riverService.VenueNaPTAN))
                            {
                                tmpVenueRiverServices[riverService.VenueNaPTAN].Add(riverService);
                            }
                            else
                            {
                                List<SJPVenueRiverService> riverServiceList = new List<SJPVenueRiverService>();
                                riverServiceList.Add(riverService);
                                tmpVenueRiverServices.Add(riverService.VenueNaPTAN, riverServiceList);
                            }
                        }
                    }

                    // Assign to static lists
                    venueRiverServices = tmpVenueRiverServices;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue river services in cache [{0}]", venueRiverServices.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue river services data completed for"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue river services data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates river services pier navigation paths for the SJP venue locations
        /// </summary>
        private static void PopulateVenuePierNavigationPaths()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                //Dictionary<VenueNaPTAN, List<SJPPierVenueNavigationPath>>
                Dictionary<string, List<SJPPierVenueNavigationPath>> tmpPierVenueNavigationPaths = new Dictionary<string, List<SJPPierVenueNavigationPath>>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue river services pier navigation path data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue river services
                    using (SqlDataReader navigationPathsDR = helper.GetReader("GetPierToVenueNavigationPaths", paramList))
                    {
                        while (navigationPathsDR.Read())
                        {
                            SJPPierVenueNavigationPath pierVenueNavigationPath = new SJPPierVenueNavigationPath();
                            // Read Values
                            pierVenueNavigationPath.VenueNaPTAN = navigationPathsDR["VenueNaPTAN"].ToString();
                            pierVenueNavigationPath.NavigationID = navigationPathsDR["NavigationID"].ToString();
                            pierVenueNavigationPath.FromNaPTAN = navigationPathsDR["FromNaPTAN"].ToString();
                            pierVenueNavigationPath.ToNaPTAN = navigationPathsDR["ToNaPTAN"].ToString();
                            pierVenueNavigationPath.DefaultDuration = navigationPathsDR.GetTimeSpan(navigationPathsDR.GetOrdinal("DefaultDuration")); 
                            pierVenueNavigationPath.Distance = Convert.ToInt32(navigationPathsDR["Distance"].ToString());

                            string transferText = navigationPathsDR["TransferText"].ToString();
                            Language transferLanguage = LanguageHelper.ParseLanguage(navigationPathsDR["TransferLanguage"].ToString());

                            pierVenueNavigationPath.AddTransferText(transferText, transferLanguage);


                            if (tmpPierVenueNavigationPaths.ContainsKey(pierVenueNavigationPath.VenueNaPTAN))
                            {
                                // Check if an object has already been created for this 
                                // (so that transfer text can be assigned to that object)
                                List<SJPPierVenueNavigationPath> pierVenueNavigationPathList = tmpPierVenueNavigationPaths[pierVenueNavigationPath.VenueNaPTAN];

                                SJPPierVenueNavigationPath result = pierVenueNavigationPathList.Find(delegate(SJPPierVenueNavigationPath pvnp) { return pvnp.NavigationID == pierVenueNavigationPath.NavigationID; });

                                if (result != null)
                                {
                                    // pierVenueNavigationPath object already exists, add navigation path transfer text to it
                                    result.AddTransferText(transferText, transferLanguage);
                                }
                                else
                                {
                                    // Its a new pierVenueNavigationPath, add it to the existing list
                                    pierVenueNavigationPathList.Add(pierVenueNavigationPath);                                    
                                }
                            }
                            else
                            {
                                // Its a new pierVenueNavigationPath for a new Venue, add it to a new list
                                List<SJPPierVenueNavigationPath> pierVenueNavigationPathList = new List<SJPPierVenueNavigationPath>();
                                pierVenueNavigationPathList.Add(pierVenueNavigationPath);
                                tmpPierVenueNavigationPaths.Add(pierVenueNavigationPath.VenueNaPTAN, pierVenueNavigationPathList);
                            }
                        }
                    }

                    // Assign to static lists
                    pierVenueNavigationPaths = tmpPierVenueNavigationPaths;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue river services pier navigation paths in cache [{0}]", pierVenueNavigationPaths.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue river services pier navigation path data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue river services data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        #endregion

        #region Gate Paths

        /// <summary>
        /// Populates venue gate data for the SJP venue locations
        /// </summary>
        private static void PopulateVenueGates()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                //Dictionary<VenueGateNaPTAN, List<SJPVenueGate>>
                Dictionary<string, List<SJPVenueGate>> tmpVenueGates = new Dictionary<string, List<SJPVenueGate>>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue gates data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue river services
                    using (SqlDataReader venueGateDR = helper.GetReader("GetVenueGates", paramList))
                    {
                        while (venueGateDR.Read())
                        {
                            SJPVenueGate venueGate = new SJPVenueGate();
                            // Read Values
                            venueGate.GateNaPTAN = venueGateDR["EntranceNaPTAN"].ToString();
                            venueGate.GateName = venueGateDR["EntranceName"].ToString();
                            float easting = (float)Convert.ToDouble(venueGateDR["Easting"].ToString());
                            float northing = (float)Convert.ToDouble(venueGateDR["Northing"].ToString());
                            OSGridReference osgr = new OSGridReference(easting, northing);
                            venueGate.GateGridRef = osgr;
                            venueGate.AvailableFrom = venueGateDR.IsDBNull(venueGateDR.GetOrdinal("AvailableFrom")) ?
                                DateTime.MinValue : Convert.ToDateTime(venueGateDR["AvailableFrom"]);
                            venueGate.AvailableTo = venueGateDR.IsDBNull(venueGateDR.GetOrdinal("AvailableTo")) ?
                                DateTime.MaxValue : Convert.ToDateTime(venueGateDR["AvailableTo"]);

                            List<SJPVenueGate> venueGateList = new List<SJPVenueGate>();
                            venueGateList.Add(venueGate);
                            tmpVenueGates.Add(venueGate.GateNaPTAN, venueGateList);
                        }
                    }

                    // Assign to static lists
                    venueGates = tmpVenueGates;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue gates in cache [{0}]", venueGates.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue gates data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue gate data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates venue gate navigation path data for the SJP venue locations
        /// </summary>
        private static void PopulateVenueGateNavigationPaths()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                //Dictionary<VenueGateNaPTAN, List<SJPVenueGate>>
                Dictionary<string, List<SJPVenueGateNavigationPath>> tmpVenueGateNavigationPaths = new Dictionary<string, List<SJPVenueGateNavigationPath>>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue gate navigation path data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue river services
                    using (SqlDataReader venueGateNavigationPathsDR = helper.GetReader("GetVenueGateNavigationPaths", paramList))
                    {
                        while (venueGateNavigationPathsDR.Read())
                        {
                            SJPVenueGateNavigationPath venueGateNavigationPath = new SJPVenueGateNavigationPath();
                            // Read Values
                            venueGateNavigationPath.GateNaPTAN = venueGateNavigationPathsDR["GateNaptan"].ToString();
                            venueGateNavigationPath.NavigationPathID = venueGateNavigationPathsDR["NavigationPathId"].ToString();
                            venueGateNavigationPath.NavigationPathName = venueGateNavigationPathsDR["NavigationPathName"].ToString();
                            venueGateNavigationPath.FromNaPTAN = venueGateNavigationPathsDR["FromNaptan"].ToString();
                            venueGateNavigationPath.ToNaPTAN = venueGateNavigationPathsDR["ToNaptan"].ToString();
                            venueGateNavigationPath.TransferDistance = Convert.ToInt32(venueGateNavigationPathsDR["TransferDistance"]);
                            venueGateNavigationPath.TransferDuration = venueGateNavigationPathsDR.GetTimeSpan(venueGateNavigationPathsDR.GetOrdinal("TransferDuration"));
                            
                            if (tmpVenueGateNavigationPaths.ContainsKey(venueGateNavigationPath.GateNaPTAN))
                            {
                                tmpVenueGateNavigationPaths[venueGateNavigationPath.GateNaPTAN].Add(venueGateNavigationPath);
                            }
                            else
                            {
                                List<SJPVenueGateNavigationPath> venueGateNavigationPathList = new List<SJPVenueGateNavigationPath>();
                                venueGateNavigationPathList.Add(venueGateNavigationPath);
                                tmpVenueGateNavigationPaths.Add(venueGateNavigationPath.GateNaPTAN, venueGateNavigationPathList);
                            }
                        }
                    }

                    // Assign to static lists
                    venueGateNavigationPaths = tmpVenueGateNavigationPaths;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue gate navigation paths in cache [{0}]", venueGateNavigationPaths.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue gate navigation path data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue gate navigation path data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        /// <summary>
        /// Populates venue gate check constraint data for the SJP venue locations
        /// </summary>
        private static void PopulateVenueGateCheckConstraints()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                //Dictionary<VenueGateNaPTAN, List<SJPVenueGateCheckConstraint>>
                Dictionary<string, List<SJPVenueGateCheckConstraint>> tmpVenueGateCheckConstraints = new Dictionary<string, List<SJPVenueGateCheckConstraint>>();
                try
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue gate check constraint data"));

                    List<SqlParameter> paramList = new List<SqlParameter>();

                    helper.ConnOpen(SqlHelperDatabase.GazetteerDB);

                    // Read and populate the detailed venue river services
                    using (SqlDataReader venueGateCheckConstraintsDR = helper.GetReader("GetVenueGateCheckConstraints", paramList))
                    {
                        while (venueGateCheckConstraintsDR.Read())
                        {
                            SJPVenueGateCheckConstraint venueGateCheckConstraint = new SJPVenueGateCheckConstraint();
                            // Read Values
                            venueGateCheckConstraint.GateNaPTAN = venueGateCheckConstraintsDR["GateNaptan"].ToString();
                            venueGateCheckConstraint.CheckConstraintID = venueGateCheckConstraintsDR["CheckConstraintId"].ToString();
                            venueGateCheckConstraint.CheckConstraintName = venueGateCheckConstraintsDR["CheckConstraintName"].ToString();
                            venueGateCheckConstraint.IsEntry = Convert.ToBoolean(venueGateCheckConstraintsDR["Entry"]);
                            venueGateCheckConstraint.Process = venueGateCheckConstraintsDR["Process"].ToString();
                            venueGateCheckConstraint.Congestion = venueGateCheckConstraintsDR["Congestion"].ToString();
                            venueGateCheckConstraint.AverageDelay = venueGateCheckConstraintsDR.GetTimeSpan(venueGateCheckConstraintsDR.GetOrdinal("AverageDelay"));

                            if (tmpVenueGateCheckConstraints.ContainsKey(venueGateCheckConstraint.GateNaPTAN))
                            {
                                tmpVenueGateCheckConstraints[venueGateCheckConstraint.GateNaPTAN].Add(venueGateCheckConstraint);
                            }
                            else
                            {
                                List<SJPVenueGateCheckConstraint> venueGateCheckConstraintList = new List<SJPVenueGateCheckConstraint>();
                                venueGateCheckConstraintList.Add(venueGateCheckConstraint);
                                tmpVenueGateCheckConstraints.Add(venueGateCheckConstraint.GateNaPTAN, venueGateCheckConstraintList);
                            }
                        }
                    }

                    // Assign to static lists
                    venueGateCheckConstraints = tmpVenueGateCheckConstraints;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("Venue gate check constraints in cache [{0}]", venueGateCheckConstraints.Count)));
                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "Loading Venue gate check constraint data completed"));
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error occurred attempting to load Venue gate check constraint data: {0}", ex.Message);

                    Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, ex));
                }
            }
        }

        #endregion
                
        #endregion

        #region Query Methods

        /// <summary>
        /// Searches for an Venue by matching the supplied searchString.
        /// </summary>
        /// <param name="searchString">The NaPTAN/Location ID to search for</param>
        /// <returns>SJPLocation for the venue or null if no match found</returns>
        internal static SJPLocation GetVenueLocation(string location)
        {
            SJPLocation result = null;

            SJPLocation sjpLocation = venueLocations.Find(delegate(SJPLocation loc){return loc.ID == location;});

            if (sjpLocation != null)
            {
                // Return a new copy of the venue location from the cache, as the application may change the "selected"
                // values of the object and we dont want the cached objects being altered
                result = (SJPLocation)sjpLocation.Clone();
            }

            return result;
        }

        /// <summary>
        /// Returns a list of summary data for all Venues.
        /// </summary>
        /// <returns>SJPLocation list for venues with location objects only containing NaPTAN and DisplayName</returns>
        internal static List<SJPLocation> GetVenuesList()
        {
            return venuesList;
        }

        /// <summary>
        /// Returns a list of summary data for all Venues.
        /// </summary>
        /// <returns>SJPLocation list for venues with location objects</returns>
        internal static List<SJPLocation> GetVenuesLocations()
        {
            return venueLocations;
        }

        /// <summary>
        /// Returns the cycle park for specified id
        /// </summary>
        /// <param name="cycleParkId"></param>
        /// <returns></returns>
        internal static SJPVenueCyclePark GetSJPVenueCyclePark(string cycleParkId)
        {
            if (!string.IsNullOrEmpty(cycleParkId))
            {
                SJPVenueCyclePark result = cycleParks.Find(delegate(SJPVenueCyclePark cyclepark) { return cyclepark.ID == cycleParkId; });
                return result;
            }

            return null;
        }

        /// <summary>
        /// Returns all the cycle parks for specified SJP venue
        /// </summary>
        /// <param name="venueNaPTAN"></param>
        /// <returns></returns>
        internal static List<SJPVenueCyclePark> GetVenueCycleParks(List<string> venueNaPTANs)
        {
            if (venueNaPTANs != null)
            {
                List<SJPVenueCyclePark> result = new List<SJPVenueCyclePark>();

                // Find all cycle parks for the requested venue naptans
                foreach (string venueNaPTAN in venueNaPTANs)
                {
                    if (venueCycleParks.ContainsKey(venueNaPTAN))
                    {
                        SJPVenueCyclePark cyclePark = null;
                        foreach (string cycleParkId in venueCycleParks[venueNaPTAN])
                        {
                            cyclePark = GetSJPVenueCyclePark(cycleParkId);

                            if ((cyclePark != null) && (!result.Contains(cyclePark)))
                            {
                                result.Add(cyclePark);
                            }
                        }
                    }
                }

                // Only return result if cycle parks were found
                if (result.Count > 0)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the car park for specified id
        /// </summary>
        /// <param name="carParkId"></param>
        /// <returns></returns>
        internal static SJPVenueCarPark GetSJPVenueCarPark(string carParkId)
        {
            if (!string.IsNullOrEmpty(carParkId))
            {
                SJPVenueCarPark result = carParks.Find(delegate(SJPVenueCarPark carpark) { return carpark.ID == carParkId; });
                return result;
            }

            return null;
        }

        /// <summary>
        /// Returns all the car parks for specified SJP venue
        /// </summary>
        /// <param name="venueNaPTAN"></param>
        /// <returns></returns>
        internal static List<SJPVenueCarPark> GetVenueCarParks(List<string> venueNaPTANs)
        {
            if (venueNaPTANs != null)
            {
                List<SJPVenueCarPark> result = new List<SJPVenueCarPark>();

                // Find all car parks for the requested venue naptans
                foreach (string venueNaPTAN in venueNaPTANs)
                {
                    if (venueCarParks.ContainsKey(venueNaPTAN))
                    {
                        SJPVenueCarPark carPark = null;
                        foreach (string carParkId in venueCarParks[venueNaPTAN])
                        {
                            carPark = GetSJPVenueCarPark(carParkId);

                            if ((carPark != null) && (!result.Contains(carPark)))
                            {
                                result.Add(carPark);
                            }
                        }
                    }
                }

                // Only return result if car parks were found
                if (result.Count > 0)
                {
                    return result;
                }
            }

            return null;
        }
        
        /// <summary>
        /// Returns all the river services for specified SJP venue
        /// </summary>
        /// <param name="venueNaPTAN"></param>
        /// <returns></returns>
        internal static List<SJPVenueRiverService> GetVenueRiverServices(string venueNaPTAN)
        {
            if (!string.IsNullOrEmpty(venueNaPTAN))
            {
                if (venueRiverServices.ContainsKey(venueNaPTAN))
                {
                    List<SJPVenueRiverService> result = new List<SJPVenueRiverService>();

                    result = venueRiverServices[venueNaPTAN];
                    
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all the pier venue navigation paths for th specified SJP venue
        /// </summary>
        /// <param name="venueNaPTAN"></param>
        /// <returns></returns>
        internal static List<SJPPierVenueNavigationPath> GetVenuePierNavigationPaths(string venueNaPTAN)
        {
            if (!string.IsNullOrEmpty(venueNaPTAN))
            {
                if (pierVenueNavigationPaths.ContainsKey(venueNaPTAN))
                {
                    List<SJPPierVenueNavigationPath> result = new List<SJPPierVenueNavigationPath>();

                    result = pierVenueNavigationPaths[venueNaPTAN];
                    
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a venue gate for the specified SJP venue gate naptan
        /// </summary>
        /// <param name="gateNaPTAN"></param>
        /// <returns></returns>
        internal static SJPVenueGate GetSJPVenueGate(string gateNaPTAN)
        {
            if (!string.IsNullOrEmpty(gateNaPTAN))
            {
                if (venueGates.ContainsKey(gateNaPTAN))
                {
                    SJPVenueGate result = new SJPVenueGate();

                    result = venueGates[gateNaPTAN][0];

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all of the check constraints for the specified SJP venue gate
        /// </summary>
        /// <param name="gateNaPTAN"></param>
        /// <returns></returns>
        internal static List<SJPVenueGateCheckConstraint> GetVenueGateCheckConstraints(string gateNaPTAN)
        {
            if (!string.IsNullOrEmpty(gateNaPTAN))
            {
                if (venueGateCheckConstraints.ContainsKey(gateNaPTAN))
                {
                    List<SJPVenueGateCheckConstraint> result = new List<SJPVenueGateCheckConstraint>();

                    result = venueGateCheckConstraints[gateNaPTAN];

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all of the navigation paths for the specified SJP venue gate
        /// </summary>
        /// <param name="gateNaPTAN"></param>
        /// <returns></returns>
        internal static List<SJPVenueGateNavigationPath> GetVenueGateNavigationPaths(string gateNaPTAN)
        {
            if (!string.IsNullOrEmpty(gateNaPTAN))
            {
                if (venueGateNavigationPaths.ContainsKey(gateNaPTAN))
                {
                    List<SJPVenueGateNavigationPath> result = new List<SJPVenueGateNavigationPath>();

                    result = venueGateNavigationPaths[gateNaPTAN];

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all of the venue access stations for the specified SJP venue naptan
        /// </summary>
        /// <param name="venueNaPTAN"></param>
        /// <returns></returns>
        internal static List<SJPVenueAccess> GetVenueAccessData(string venueNaPTAN)
        {
            if (!string.IsNullOrEmpty(venueNaPTAN))
            {
                if (venueAccessData.ContainsKey(venueNaPTAN))
                {
                    List<SJPVenueAccess> result = new List<SJPVenueAccess>();

                    result = venueAccessData[venueNaPTAN];

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads all Venues data
        /// </summary>
        internal static void LoadVenues()
        {
            // Make load threadsafe
            if (!dataInitialised)
            {
                lock (dataInitialisedLock)
                {
                    if (!dataInitialised)
                    {
                        // Load data
                        PopulateVenuesData();
                        PopulateVenueCycleParks();
                        PopulateVenueCarParks();
                        PopulateVenueRiverServices();
                        PopulateVenuePierNavigationPaths();
                        PopulateVenueGates();
                        PopulateVenueGateCheckConstraints();
                        PopulateVenueGateNavigationPaths();
                        PopulateVenueAccessData();

                        // Set to true here (prevents repeated attempts to load if it fails)
                        dataInitialised = true;
                    }
                }
            }
        }

        #endregion
    }
}
