// *********************************************** 
// NAME             : TravelcardCatalogue.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Jan 2012
// DESCRIPTION  	: TravelcardCatalogue loads and holds a list of Travelcards
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SJP.Common.DatabaseInfrastructure;
using Logger = System.Diagnostics.Trace;
using SJP.Common.EventLogging;
using SJP.Common;
using System.Drawing;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// TravelcardCatalogue loads and holds a list of Travelcards
    /// </summary>
    public class TravelcardCatalogue : ITravelcardCatalogue
    {
        #region Private members
                
        // Travelcards. Key is the travelcard id, value is instance of Travelcard.
        private Dictionary<string, Travelcard> travelcards;
        private Dictionary<string, Zone> zones;
        private Dictionary<string, Route> routes;

        // Stored procedures that returns the travelcard data
        private const string SP_GetTravelcards = "GetTravelcards";
        private const string SP_GetRoutes = "GetRoutes";
        private const string SP_GetRouteEnds = "GetRouteEnds";
        private const string SP_GetZones = "GetZones";
        private const string SP_GetZoneStops = "GetZoneStops";
        private const string SP_GetZoneModes = "GetZoneModes";
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Clients should not use this constructor to create instances.
        /// Instead the Current property should be used to obtain a singleton instance.
        /// </summary>
        public TravelcardCatalogue()
        {
            travelcards = new Dictionary<string, Travelcard>();

            LoadZonesData();
            LoadRoutesData();
            LoadTravelcardsData();
        }

        #endregion

        #region ITravelcardCatalogue Public methods

        /// <summary>
        /// Method determines if the supplied journey leg has an applicable travelcard
        /// </summary>
        public bool HasTravelcard(JourneyLeg journeyLeg)
        {
            if (journeyLeg != null)
            {
                DateTime startTime = journeyLeg.StartTime;
                DateTime endTime = journeyLeg.EndTime;

                return HasTravelcard(journeyLeg, startTime, endTime);
            }

            return false;
        }

        /// <summary>
        /// Method determines if the supplied journey leg with specified start and end datetimes
        /// has an applicable travelcard
        /// </summary>
        public bool HasTravelcard(JourneyLeg journeyLeg, DateTime startTime, DateTime endTime)
        {
            bool result = false;

            if (journeyLeg != null)
            {
                // Only check for non-walk legs
                if (journeyLeg.Mode != SJPModeType.Walk)
                {
                    foreach (Travelcard travelcard in travelcards.Values)
                    {
                        // Is the leg date valid for the travelcard applicable dates
                        if ((startTime >= travelcard.ValidFrom)
                            && (endTime <= travelcard.ValidTo))
                        {
                            bool isLegStartInZone = false;
                            bool isLegEndInZone = false;

                            bool isLegInIncludedRoute = false;
                            bool isLegInExcludedRoute = false;

                            #region Check zones

                            // Are the leg locations in any of the included zones
                            foreach (string zoneId in travelcard.ZonesIncluded)
                            {
                                if (zones.ContainsKey(zoneId))
                                {
                                    Zone zone = zones[zoneId];

                                    if (!isLegStartInZone)
                                        isLegStartInZone = zone.InZone(journeyLeg.LegStart.Location.GridRef, journeyLeg.LegStart.Location.Naptan.FirstOrDefault(), journeyLeg.Mode);

                                    if (!isLegEndInZone)
                                        isLegEndInZone = zone.InZone(journeyLeg.LegEnd.Location.GridRef, journeyLeg.LegEnd.Location.Naptan.FirstOrDefault(), journeyLeg.Mode);
                                }
                            }

                            // Both leg locations are in the travelcard included zones
                            if (isLegStartInZone && isLegEndInZone)
                            {
                                // Are either of the leg locations in any of the exclude zones
                                foreach (string zoneId in travelcard.ZonesExcluded)
                                {
                                    if (zones.ContainsKey(zoneId))
                                    {
                                        Zone zone = zones[zoneId];

                                        if (isLegStartInZone)
                                            isLegStartInZone = !zone.InZone(journeyLeg.LegStart.Location.GridRef, journeyLeg.LegStart.Location.Naptan.FirstOrDefault(), journeyLeg.Mode);

                                        if (isLegEndInZone)
                                            isLegEndInZone = !zone.InZone(journeyLeg.LegEnd.Location.GridRef, journeyLeg.LegEnd.Location.Naptan.FirstOrDefault(), journeyLeg.Mode);
                                    }
                                }
                            }

                            #endregion

                            #region Check routes

                            // Are the leg locations in any of the included routes
                            foreach (string routeId in travelcard.RoutesIncluded)
                            {
                                if (routes.ContainsKey(routeId))
                                {
                                    Route route = routes[routeId];

                                    if (!isLegInIncludedRoute)
                                        isLegInIncludedRoute = route.InRoute(journeyLeg.LegStart.Location, journeyLeg.LegEnd.Location, journeyLeg.Mode);
                                }
                            }

                            // Are the leg locations in any of the excluded routes
                            foreach (string routeId in travelcard.RoutesExcluded)
                            {
                                if (routes.ContainsKey(routeId))
                                {
                                    Route route = routes[routeId];

                                    if (!isLegInExcludedRoute)
                                        isLegInExcludedRoute = route.InRoute(journeyLeg.LegStart.Location, journeyLeg.LegEnd.Location, journeyLeg.Mode);
                                }
                            }

                            #endregion

                            // Determine if leg is valid for the travelcard
                            if (isLegStartInZone && isLegEndInZone && !isLegInExcludedRoute)
                            {
                                // Leg locations are in zones and not in an excluded route
                                result = true;
                            }
                            else if (isLegInIncludedRoute && !isLegInExcludedRoute)
                            {
                                // Leg is in an included route
                                result = true;
                            }
                        }

                    }
                }
            }

            return result;
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Loads the database tables that constitute the travelcards catalogue and builds the
        /// zones dictionary
        /// </summary>
        private void LoadZonesData()
        {
            try
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "TravelcardCatalogue - Loading Zones data"));

                // Temp dictionary
                Dictionary<string, Zone> tmpZones = new Dictionary<string, Zone>();

                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    sqlHelper.ConnOpen(SqlHelperDatabase.TransientPortalDB);
                                        
                    #region Zones

                    // Load zones data
                    using (SqlDataReader dr = sqlHelper.GetReader(SP_GetZones, new List<SqlParameter>()))
                    {
                        Zone zone = null;
                        Point point;

                        while (dr.Read())
                        {
                            // Zone
                            string id = (dr["ZoneID"] != DBNull.Value) ? dr["ZoneID"].ToString().ToUpper() : string.Empty;
                            string name = (dr["ZoneName"] != DBNull.Value) ? dr["ZoneName"].ToString() : string.Empty;

                            // Zone area
                            string areaId = (dr["ZoneAreaID"] != DBNull.Value) ? dr["ZoneAreaID"].ToString() : string.Empty;
                            bool isOuterZoneArea = (dr["IsOuterZoneArea"] != DBNull.Value) ?
                                    Convert.ToBoolean(dr["IsOuterZoneArea"].ToString()) : true;

                            // Zone area polygon point
                            int easting = (int)Convert.ToInt32((dr["Easting"] != DBNull.Value) ? dr["Easting"].ToString() : "0");
                            int northing = (int)Convert.ToInt32((dr["Northing"] != DBNull.Value) ? dr["Northing"].ToString() : "0");

                            // Create zone (retrieve existing one if exists)
                            if (tmpZones.ContainsKey(id))
                                zone = tmpZones[id];
                            else
                                zone = new Zone(id, name);

                            // Update zone with polygon point
                            if (easting > 0 && northing > 0)
                            {
                                point = new Point(easting, northing);

                                zone.AddZonePoint(point, isOuterZoneArea);
                            }

                            // Update temp dictionary
                            if (!tmpZones.ContainsKey(id))
                            {
                                tmpZones.Add(id, zone);
                            }
                        }
                    }

                    // Load stops for zones data
                    if (tmpZones.Count > 0)
                    {
                        using (SqlDataReader dr = sqlHelper.GetReader(SP_GetZoneStops, new List<SqlParameter>()))
                        {
                            Zone zone = null;

                            while (dr.Read())
                            {
                                // Zone stop
                                string id = (dr["ZoneID"] != DBNull.Value) ? dr["ZoneID"].ToString().ToUpper() : string.Empty;
                                string stopNaptan = (dr["NaPTAN"] != DBNull.Value) ? dr["NaPTAN"].ToString() : string.Empty;
                                bool isStopExcluded = (dr["IsExcluded"] != DBNull.Value) ?
                                    Convert.ToBoolean(dr["IsExcluded"].ToString()) : true;

                                // Retrieve zone
                                if (tmpZones.ContainsKey(id))
                                {
                                    zone = tmpZones[id];

                                    // Update zone
                                    zone.AddZoneStop(stopNaptan, isStopExcluded);
                                }
                            }
                        }
                    }

                    // Load modes for zones data
                    if (tmpZones.Count > 0)
                    {
                        using (SqlDataReader dr = sqlHelper.GetReader(SP_GetZoneModes, new List<SqlParameter>()))
                        {
                            Zone zone = null;

                            while (dr.Read())
                            {
                                // Zone mode
                                string id = (dr["ZoneID"] != DBNull.Value) ? dr["ZoneID"].ToString().ToUpper() : string.Empty;
                                SJPModeType modeOfTransport = (dr["ModeOfTransport"] != DBNull.Value) ? 
                                    (SJPModeType)Enum.Parse(typeof(SJPModeType), dr["ModeOfTransport"].ToString(), true) : SJPModeType.Unknown;
                                bool isModeExcluded = (dr["IsExcluded"] != DBNull.Value) ? 
                                    Convert.ToBoolean(dr["IsExcluded"].ToString()) : true;

                                // Retrieve zone
                                if (tmpZones.ContainsKey(id))
                                {
                                    zone = tmpZones[id];

                                    // Update zone
                                    zone.AddZoneMode(modeOfTransport, isModeExcluded);
                                }
                            }
                        }
                    }

                    foreach (Zone zone in tmpZones.Values)
                    {

                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                string.Format("TravelcardCatalogue - Zone cached: {0}", zone.ToString())));
                    }

                    #endregion
                }

                // Assign to static list
                zones = tmpZones;

                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("TravelcardCatalogue - Zones in cache [{0}]", zones.Count)));
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "TravelcardCatalogue - Loading Zones data completed"));
            }
            catch (SqlException sqlEx)
            {
                string message = string.Format("SqlException during TravelcardCatalogue Zones load: {0}", sqlEx.Message);
                
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));

                throw new SJPException(message, sqlEx, true, SJPExceptionIdentifier.RETravelcardCataglogueSQLCommandFailed);
            }
        }

        /// <summary>
        /// Loads the database tables that constitute the travelcards catalogue and builds the
        /// routes dictionary
        /// </summary>
        private void LoadRoutesData()
        {
            try
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "TravelcardCatalogue - Loading Routes data"));

                // Temp dictionary
                Dictionary<string, Route> tmpRoutes = new Dictionary<string, Route>();

                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    sqlHelper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    #region Routes

                    // Load routes data
                    using (SqlDataReader dr = sqlHelper.GetReader(SP_GetRoutes, new List<SqlParameter>()))
                    {
                        Route route = null;

                        while (dr.Read())
                        {
                            // Route
                            string id = (dr["RouteID"] != DBNull.Value) ? dr["RouteID"].ToString() : string.Empty;
                            string name = (dr["RouteName"] != DBNull.Value) ? dr["RouteName"].ToString() : string.Empty;

                            // Route mode
                            SJPModeType modeOfTransport = (dr["ModeOfTransport"] != DBNull.Value) ?
                                    (SJPModeType)Enum.Parse(typeof(SJPModeType), dr["ModeOfTransport"].ToString(), true) : SJPModeType.Unknown;
                            bool isModeExcluded = (dr["IsExcluded"] != DBNull.Value) ?
                                Convert.ToBoolean(dr["IsExcluded"].ToString()) : true;
                                                        
                            // Create route (retrieve existing one if exists)
                            if (tmpRoutes.ContainsKey(id))
                                route = tmpRoutes[id];
                            else
                                route = new Route(id, name);
                            
                            // Update route with transport mode
                            route.AddRouteMode(modeOfTransport, isModeExcluded);

                            // Update temp dictionary
                            if (!tmpRoutes.ContainsKey(id))
                            {
                                tmpRoutes.Add(id, route);
                            }
                        }
                    }

                    // Load route ends for route data
                    if (tmpRoutes.Count > 0)
                    {
                        using (SqlDataReader dr = sqlHelper.GetReader(SP_GetRouteEnds, new List<SqlParameter>()))
                        {
                            Route route = null;
                            Zone zone = null;
                            while (dr.Read())
                            {
                                // Route
                                string id = (dr["RouteID"] != DBNull.Value) ? dr["RouteID"].ToString() : string.Empty;
                                string name = (dr["RouteName"] != DBNull.Value) ? dr["RouteName"].ToString() : string.Empty;

                                // Route end - stop
                                string stopNaptan = (dr["NaPTAN"] != DBNull.Value) ? dr["NaPTAN"].ToString() : string.Empty;
                                bool isStopEndA = (dr["StopIsEndA"] != DBNull.Value) ?
                                    Convert.ToBoolean(dr["StopIsEndA"].ToString()) : true;

                                // Route end - zone
                                string zoneId = (dr["ZoneID"] != DBNull.Value) ? dr["ZoneID"].ToString() : string.Empty;
                                bool isZoneEndA = (dr["ZoneIsEndA"] != DBNull.Value) ?
                                    Convert.ToBoolean(dr["ZoneIsEndA"].ToString()) : true;

                                // Retrieve route
                                if (tmpRoutes.ContainsKey(id))
                                {
                                    route = tmpRoutes[id];
                                    
                                    // Update route
                                    route.AddRouteEndStop(stopNaptan, isStopEndA);

                                    // Retrieve zone and assign object to route 
                                    // (to simplify later query "InRoute" checks on a Travelard Route)
                                    if (zones.ContainsKey(zoneId))
                                    {
                                        zone = zones[zoneId];

                                        route.AddRouteEndZone(zone, isZoneEndA);
                                    }
                                }
                            }
                        }
                    }

                    foreach (Route route in tmpRoutes.Values)
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                        string.Format("TravelcardCatalogue - Route cached: {0}", route.ToString())));
                    }
                    
                    #endregion
                }

                // Assign to static list
                routes = tmpRoutes;

                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("TravelcardCatalogue - Routes in cache [{0}]", routes.Count)));
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "TravelcardCatalogue - Loading Routes data completed"));
            }
            catch (SqlException sqlEx)
            {
                string message = string.Format("SqlException during TravelcardCatalogue Routes load: {0}", sqlEx.Message);

                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));

                throw new SJPException(message, sqlEx, true, SJPExceptionIdentifier.RETravelcardCataglogueSQLCommandFailed);
            }
        }

        /// <summary>
        /// Loads the database tables that constitute the travelcards catalogue and builds the
        /// travelcards dictionary
        /// </summary>
        private void LoadTravelcardsData()
        {
            try
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, "TravelcardCatalogue - Loading Travelcards data"));

                // Temp dictionary
                Dictionary<string, Travelcard> tmpTravelcards = new Dictionary<string, Travelcard>();

                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    sqlHelper.ConnOpen(SqlHelperDatabase.TransientPortalDB);

                    #region Travelcards

                    // Load travelcards data
                    using (SqlDataReader dr = sqlHelper.GetReader(SP_GetTravelcards, new List<SqlParameter>()))
                    {
                        Travelcard travelcard = null;

                        while (dr.Read())
                        {
                            // Travelcard
                            string id = (dr["TravelCardID"] != DBNull.Value) ? dr["TravelCardID"].ToString() : string.Empty;
                            string name = (dr["TravelCardName"] != DBNull.Value) ? dr["TravelCardName"].ToString() : string.Empty;
                            DateTime validFrom = dr.IsDBNull(dr.GetOrdinal("ValidFrom")) ? DateTime.MinValue : Convert.ToDateTime(dr["ValidFrom"]);
                            DateTime validTo = dr.IsDBNull(dr.GetOrdinal("ValidTo")) ? DateTime.MaxValue : Convert.ToDateTime(dr["ValidTo"]);

                            // Travelcard zones
                            string zoneId = (dr["ZoneID"] != DBNull.Value) ? dr["ZoneID"].ToString() : string.Empty;
                            bool isZoneExcluded = (dr["ZoneExcluded"] != DBNull.Value) ?
                                    Convert.ToBoolean(dr["ZoneExcluded"].ToString()) : true;

                            // Travelcard routes
                            string routeId = (dr["RouteID"] != DBNull.Value) ? dr["RouteID"].ToString() : string.Empty;
                            bool isRouteExcluded = (dr["RouteExcluded"] != DBNull.Value) ?
                                    Convert.ToBoolean(dr["RouteExcluded"].ToString()) : true;

                            // Create travelcard (retrieve existing one if exists)
                            if (tmpTravelcards.ContainsKey(id))
                                travelcard = tmpTravelcards[id];
                            else
                                travelcard = new Travelcard(id, name, validFrom, validTo);

                            // Update
                            travelcard.AddZone(zoneId, isZoneExcluded);
                            travelcard.AddRoute(routeId, isRouteExcluded);

                            // Update temp dictionary
                            if (!tmpTravelcards.ContainsKey(id))
                            {
                                tmpTravelcards.Add(id, travelcard);
                            }
                        }
                    }

                    foreach (Travelcard travelcard in tmpTravelcards.Values)
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose,
                                        string.Format("TravelcardCatalogue - Travelcard cached: {0}", travelcard.ToString())));
                    }

                    #endregion
                }

                // Assign to static list
                travelcards = tmpTravelcards;

                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Verbose, string.Format("TravelcardCatalogue - Travelcards in cache [{0}]", travelcards.Count)));
                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Info, "TravelcardCatalogue - Loading Travelcards data completed"));
            }
            catch (SqlException sqlEx)
            {
                string message = string.Format("SqlException during TravelcardCatalogue Travelcards load: {0}", sqlEx.Message);

                Logger.Write(new OperationalEvent(SJPEventCategory.Database, SJPTraceLevel.Error, message, sqlEx));

                throw new SJPException(message, sqlEx, true, SJPExceptionIdentifier.RETravelcardCataglogueSQLCommandFailed);
            }
        }

        #endregion
    }
}
