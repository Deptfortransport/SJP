// *********************************************** 
// NAME             : Travelcard.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Jan 2012
// DESCRIPTION  	: Travelcard class representing a travelcard scheme, 
// with included/excluded zones,
// valid/invalid routes
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// Travelcard class representing a travelcard scheme
    /// </summary>
    [Serializable()]
    public class Travelcard
    {
        #region Private members

        private string id = string.Empty;
        private string name = string.Empty;
        private DateTime validFrom = DateTime.MinValue;
        private DateTime validTo = DateTime.MaxValue;

        private List<string> zonesIncluded = new List<string>();
        private List<string> zonesExcluded = new List<string>();

        private List<string> routesIncluded = new List<string>();
        private List<string> routesExcluded = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Travelcard()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Travelcard(string id, string name, DateTime validFrom, DateTime validTo)
        {
            this.id = id;
            this.name = name;
            this.validFrom = validFrom;
            this.validTo = validTo;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Travelcard id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Read/Write. Travelcard name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Read/Write. Travelcard valid from
        /// </summary>
        public DateTime ValidFrom
        {
            get { return validFrom; }
            set { validFrom = value; }
        }

        /// <summary>
        /// Read/Write. Travelcard valid to
        /// </summary>
        public DateTime ValidTo
        {
            get { return validTo; }
            set { validTo = value; }
        }

        /// <summary>
        /// Read/Write. Zones included list
        /// </summary>
        public List<string> ZonesIncluded
        {
            get { return zonesIncluded; }
            set { zonesIncluded = value; }
        }

        /// <summary>
        /// Read/Write. Zones excluded list
        /// </summary>
        public List<string> ZonesExcluded
        {
            get { return zonesExcluded; }
            set { zonesExcluded = value; }
        }

        /// <summary>
        /// Read/Write. Routes included list
        /// </summary>
        public List<string> RoutesIncluded
        {
            get { return routesIncluded; }
            set { routesIncluded = value; }
        }

        /// <summary>
        /// Read/Write. Routes excluded list
        /// </summary>
        public List<string> RoutesExcluded
        {
            get { return routesExcluded; }
            set { routesExcluded = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method to add a route to the routes list
        /// </summary>
        public void AddZone(string zoneId, bool isExcluded)
        {
            if (!string.IsNullOrEmpty(zoneId))
            {
                // Check zone doesnt exist in either list
                if (!zonesIncluded.Contains(zoneId.ToUpper())
                    && !zonesExcluded.Contains(zoneId.ToUpper()))
                {
                    if (isExcluded)
                    {
                        zonesExcluded.Add(zoneId.ToUpper());
                    }
                    else
                    {
                        zonesIncluded.Add(zoneId.ToUpper());
                    }
                }
            }
        }
        
        /// <summary>
        /// Method to add a route to the routes list
        /// </summary>
        public void AddRoute(string routeId, bool isExcluded)
        {
            if (!string.IsNullOrEmpty(routeId))
            {
                // Check route doesnt exist in either list
                if (!routesIncluded.Contains(routeId.ToUpper())
                    && !routesExcluded.Contains(routeId.ToUpper()))
                {
                    if (isExcluded)
                    {
                        routesExcluded.Add(routeId.ToUpper());
                    }
                    else
                    {
                        routesIncluded.Add(routeId.ToUpper());
                    }
                }
            }
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
            sb.Append(string.Format("From[{0}] ", validFrom.ToString()));
            sb.Append(string.Format("To[{0}] ", validTo.ToString()));

            sb.Append("ZonesInc[");
            foreach (string s in zonesIncluded)
            {
                sb.Append(string.Format("{0};", s));
            }
            sb.Append("] ");

            sb.Append("ZonesExc[");
            foreach (string s in zonesExcluded)
            {
                sb.Append(string.Format("{0};", s));
            }
            sb.Append("] ");

            sb.Append("RoutesInc[");
            foreach (string s in routesIncluded)
            {
                sb.Append(string.Format("{0};", s));
            }
            sb.Append("] ");

            sb.Append("RoutesExc[");
            foreach (string s in routesExcluded)
            {
                sb.Append(string.Format("{0};", s));
            }
            sb.Append("] ");
            
            return sb.ToString();
        }

        #endregion
    }
}
