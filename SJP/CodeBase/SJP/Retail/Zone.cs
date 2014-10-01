// *********************************************** 
// NAME             : Zone.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Jan 2012
// DESCRIPTION  	: Zone class for storing polygons defining the zone, including stop naptans, and transport modes
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SJP.Common;
using SJP.Common.LocationService;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// Zone class for storing polygons defining the zone, including stop naptans, and transport modes
    /// </summary>
    public class Zone
    {
        #region Private members

        private string id = string.Empty;
        private string name = string.Empty;

        private List<Point> outerZonePolygon = new List<Point>();
        private List<Point> innerZonePolygon = new List<Point>();
        
        private List<string> stopsIncluded = new List<string>();
        private List<string> stopsExcluded = new List<string>();

        private List<SJPModeType> modesIncluded = new List<SJPModeType>();
        private List<SJPModeType> modesExcluded = new List<SJPModeType>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Zone()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Zone(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Zone id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Read/Write. Zone name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Read/Write. Zone outer polygon
        /// </summary>
        public List<Point> OuterZonePolygon
        {
            get { return outerZonePolygon; }
            set { outerZonePolygon = value; }
        }

        /// <summary>
        /// Read/Write. Zone inner polygon
        /// </summary>
        public List<Point> InnerZonePolygon
        {
            get { return innerZonePolygon; }
            set { innerZonePolygon = value; }
        }

        /// <summary>
        /// Read/Write. Stops included list
        /// </summary>
        public List<string> StopsIncluded
        {
            get { return stopsIncluded; }
            set { stopsIncluded = value; }
        }

        /// <summary>
        /// Read/Write. Stops excluded list
        /// </summary>
        public List<string> StopsExcluded
        {
            get { return stopsExcluded; }
            set { stopsExcluded = value; }
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
        /// Method to add a point to the zone outer or inner polygon list
        /// </summary>
        public void AddZonePoint(Point point, bool isOuterPolygon)
        {
            // No validation, just add it
            if (isOuterPolygon)
            {
                outerZonePolygon.Add(point);
            }
            else
            {
                innerZonePolygon.Add(point);
            }
        }

        /// <summary>
        /// Method to add a stop naptan to the included or excluded stops list
        /// </summary>
        public void AddZoneStop(string naptan, bool isExcluded)
        {
            if (!string.IsNullOrEmpty(naptan))
            {
                // Check naptan doesnt exist in either list
                if (!stopsIncluded.Contains(naptan.ToUpper())
                    && !stopsExcluded.Contains(naptan.ToUpper()))
                {
                    if (isExcluded)
                    {
                        stopsExcluded.Add(naptan.ToUpper());
                    }
                    else
                    {
                        stopsIncluded.Add(naptan.ToUpper());
                    }
                }
            }
        }

        /// <summary>
        /// Method to add a mode to the included or excluded modes list
        /// </summary>
        public void AddZoneMode(SJPModeType mode, bool isExcluded)
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
        /// Returns true if the supplied parameters are in this zone
        /// </summary>
        public bool InZone(OSGridReference osgr, string naptan, SJPModeType mode)
        {
            Point point = new Point(Convert.ToInt32(osgr.Easting), Convert.ToInt32(osgr.Northing));

            return InZone(point, naptan, mode);
        }

        /// <summary>
        /// Returns true if the supplied parameters are in this zone
        /// </summary>
        public bool InZone(Point point, string naptan, SJPModeType mode)
        {
            // Is in this zone, if
            // - a) Point is inside Outer zone polygon, and
            // -    Point is outside Inner zone polygon
            // - OR
            // - b) NaPTAN (if supplied) is in Included list, and
            // -    NaPTAN (if supplied) is not in Excluded list
            // - AND
            // - c) Mode is not in Excluded list

            // Assume not in zone until proven
            bool result = false;


            if (point != null)
            {
                // Point in outer polygon
                result = PolygonHelper.PointInPolygon(point, outerZonePolygon.ToArray());

                if ((result) && (innerZonePolygon.Count > 0))
                {
                    // Point not in inner polygon
                    result = !PolygonHelper.PointInPolygon(point, innerZonePolygon.ToArray());
                }
            }

            if (!string.IsNullOrEmpty(naptan))
            {
                // If in zone polygon, check napan is not in the excluded list
                // because it might be a stop inside the zone polygon considered to not be part of this zone
                if ((result) && (stopsExcluded.Count > 0))
                {
                    result = !stopsExcluded.Contains(naptan.ToUpper());
                }
                // Otherwise, check it is in the included list 
                // because it might be a stop outside the zone polygon considered to be part of this zone
                else if (stopsIncluded.Count > 0)
                {
                    result = stopsIncluded.Contains(naptan.ToUpper());
                }
            }

            if (mode != SJPModeType.Unknown)
            {
                // If in zone (polygon or stop), check mode is not in the excluded list
                if ((result) && (modesExcluded.Count > 0))
                {
                    result = !modesExcluded.Contains(mode);
                }
                // Otherwise, assume mode is ok as it may not have been explicitly added to the included list
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

            sb.Append(string.Format("OuterPoints[{0}] ", outerZonePolygon.Count));
            sb.Append(string.Format("InnerPoints[{0}] ", innerZonePolygon.Count));

            sb.Append("StopsInc[");
            if (stopsIncluded.Count < 10)
            {
                foreach (string s in stopsIncluded)
                {
                    sb.Append(string.Format("{0};", s));
                }
            }
            else
            {
                sb.Append(string.Format("Count:{0}", stopsIncluded.Count));
            }
            sb.Append("] ");

            sb.Append("StopsExc[");
            if (stopsExcluded.Count < 10)
            {
                foreach (string s in stopsExcluded)
                {
                    sb.Append(string.Format("{0};", s));
                }
            }
            else
            {
                sb.Append(string.Format("Count:{0}", stopsExcluded.Count));
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
    }
}
