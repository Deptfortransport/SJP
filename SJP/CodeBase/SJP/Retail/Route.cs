// *********************************************** 
// NAME             : Route.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Jan 2012
// DESCRIPTION  	: Route class representing a route between two ends of zones and/or stops
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common;
using SJP.Common.LocationService;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// Route class representing a route between two ends of zones and/or stops
    /// </summary>
    [Serializable()]
    public class Route
    {
        #region Private members

        private string id = string.Empty;
        private string name = string.Empty;

        private Dictionary<string, Zone> zonesEndA = new Dictionary<string, Zone>();
        private Dictionary<string, Zone> zonesEndB = new Dictionary<string, Zone>();
        
        private List<string> stopsEndA = new List<string>();
        private List<string> stopsEndB = new List<string>();
                
        private List<SJPModeType> modesIncluded = new List<SJPModeType>();
        private List<SJPModeType> modesExcluded = new List<SJPModeType>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Route()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Route(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Route id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Read/Write. Route name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Read/Write. Zones at End A of the route
        /// </summary>
        public Dictionary<string, Zone> ZonesEndA
        {
            get { return zonesEndA; }
            set { zonesEndA = value; }
        }

        /// <summary>
        /// Read/Write. Zones at End B of the route
        /// </summary>
        public Dictionary<string, Zone> ZonesEndB
        {
            get { return zonesEndB; }
            set { zonesEndB = value; }
        }

        /// <summary>
        /// Read/Write. Stops at End A of the route
        /// </summary>
        public List<string> StopsEndA
        {
            get { return stopsEndA; }
            set { stopsEndA = value; }
        }

        /// <summary>
        /// Read/Write. Stops at End B of the route
        /// </summary>
        public List<string> StopsEndB
        {
            get { return stopsEndB; }
            set { stopsEndB = value; }
        }

        /// <summary>
        /// Read/Write. Modes included list
        /// </summary>
        public List<SJPModeType> ModesIncluded
        {
            get { return modesIncluded; }
            set { modesIncluded = value; }
        }

        /// <summary>
        /// Read/Write. Modes excluded list
        /// </summary>
        public List<SJPModeType> ModesExcluded
        {
            get { return modesExcluded; }
            set { modesExcluded = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method to add a zone to the route ends list
        /// </summary>
        public void AddRouteEndZone(Zone zone, bool isEndA)
        {
            if (zone != null)
            {
                // Check zone doesnt exist in either list
                if (!zonesEndA.ContainsKey(zone.Id)
                    && !zonesEndB.ContainsKey(zone.Id))
                {
                    if (isEndA)
                    {
                        zonesEndA.Add(zone.Id, zone);
                    }
                    else
                    {
                        zonesEndB.Add(zone.Id, zone);
                    }
                }
            }
        }

        /// <summary>
        /// Method to add a stop naptan to the route ends list
        /// </summary>
        public void AddRouteEndStop(string naptan, bool isEndA)
        {
            if (!string.IsNullOrEmpty(naptan))
            {
                // Check naptan doesnt exist in either list
                if (!stopsEndA.Contains(naptan.ToUpper())
                    && !stopsEndB.Contains(naptan.ToUpper()))
                {
                    if (isEndA)
                    {
                        stopsEndA.Add(naptan.ToUpper());
                    }
                    else
                    {
                        stopsEndB.Add(naptan.ToUpper());
                    }
                }
            }
        }

        /// <summary>
        /// Method to add a mode to the included or excluded modes list
        /// </summary>
        public void AddRouteMode(SJPModeType mode, bool isExcluded)
        {
            // Check mode doesnt exist in either list
            if (mode != SJPModeType.Unknown
                && !modesIncluded.Contains(mode)
                && !modesExcluded.Contains(mode))
            {
                if (isExcluded)
                {
                    modesExcluded.Add(mode);
                }
                else
                {
                    modesIncluded.Add(mode);
                }
            }
        }

        /// <summary>
        /// Returns true if the supplied parameters are in and valid for this route
        /// </summary>
        public bool InRoute(SJPLocation location1, SJPLocation location2, SJPModeType mode)
        {
            bool result = false;

            if (location1 != null && location2 != null)
            {
                bool inRouteEndLocation1 = false;
                bool inRouteEndLocation2 = false;

                // Check if location1 is in EndA
                inRouteEndLocation1 = IsInRouteEndZone(true, location1, mode);

                if (!inRouteEndLocation1)
                    inRouteEndLocation1 = IsInRouteEndStop(true, location1);

                if (inRouteEndLocation1)
                {
                    // ... and location2 is in EndB
                    inRouteEndLocation2 = IsInRouteEndZone(false, location2, mode);

                    if (!inRouteEndLocation2)
                        inRouteEndLocation2 = IsInRouteEndStop(false, location2);
                }
                else
                {
                    // Otherwise check if location1 is in EndB
                    inRouteEndLocation1 = IsInRouteEndZone(false, location1, mode);

                    if (!inRouteEndLocation1)
                        inRouteEndLocation1 = IsInRouteEndStop(false, location1);

                    if (inRouteEndLocation1)
                    {
                        // ... and location2 is in EndA
                        inRouteEndLocation2 = IsInRouteEndZone(true, location2, mode);

                        if (!inRouteEndLocation2)
                            inRouteEndLocation2 = IsInRouteEndStop(true, location2);
                    }
                }

                if (inRouteEndLocation1 && inRouteEndLocation2)
                {
                    if (mode != SJPModeType.Unknown)
                    {
                        // Locations are at opposite route ends, check mode is not in the excluded list
                        if (modesExcluded.Count > 0)
                        {
                            result = !modesExcluded.Contains(mode);
                        }
                        // Otherwise, assume mode is ok as it may not have been explicitly added to the included list
                        else
                        {
                            result = true;
                        }
                    }
                }
                // Else locations are not in opposite route ends
                
            }

            return result;
        }

        /// <summary>
        /// Overridden ToString method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("Id[{0}] ", id));
            sb.Append(string.Format("Name[{0}] ", name));

            sb.Append("ZonesEndA[");
            foreach (string s in zonesEndA.Keys)
            {
                sb.Append(string.Format("{0};", s));
            }
            sb.Append("] ");

            sb.Append("ZonesEndB[");
            foreach (string s in zonesEndB.Keys)
            {
                sb.Append(string.Format("{0};", s));
            }
            sb.Append("] ");

            sb.Append("StopsEndA[");
            if (stopsEndA.Count < 10)
            {
                foreach (string s in stopsEndA)
                {
                    sb.Append(string.Format("{0};", s));
                }
            }
            else
            {
                sb.Append(string.Format("Count:{0}", stopsEndA.Count));
            }
            sb.Append("] ");

            sb.Append("StopsEndB[");
            if (stopsEndB.Count < 10)
            {
                foreach (string s in stopsEndB)
                {
                    sb.Append(string.Format("{0};", s));
                }
            }
            else
            {
                sb.Append(string.Format("Count:{0}", stopsEndB.Count));
            }
            sb.Append("] ");

            sb.Append("ModesInc[");
            foreach (SJPModeType m in modesIncluded)
            {
                sb.Append(string.Format("{0};", m.ToString()));
            }
            sb.Append("] ");

            sb.Append("ModesExc[");
            foreach (SJPModeType m in modesExcluded)
            {
                sb.Append(string.Format("{0};", m.ToString()));
            }
            sb.Append("] ");

            return sb.ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method checks if location is within the specified route end zones
        /// </summary>
        private bool IsInRouteEndZone(bool endA, SJPLocation location, SJPModeType mode)
        {
            bool inRouteEndLocation = false;

            Zone[] zones = ((endA) ? zonesEndA.Values : zonesEndB.Values).ToArray();

            foreach (Zone zone in zonesEndA.Values)
            {
                if (!inRouteEndLocation)
                {
                    inRouteEndLocation = zone.InZone(location.GridRef, location.Naptan.FirstOrDefault(), mode);
                    break;
                }
            }

            return inRouteEndLocation;
        }

        /// <summary>
        /// Method checks if location is within the specified route end stops
        /// </summary>
        private bool IsInRouteEndStop(bool endA, SJPLocation location)
        {
            List<string> stops = (endA) ? stopsEndA : stopsEndB;

            if ((location.Naptan != null) && (location.Naptan.Count > 0))
                return stops.Contains(location.Naptan.FirstOrDefault().ToUpper());
            else
                return false;
        }

        #endregion
    }
}
