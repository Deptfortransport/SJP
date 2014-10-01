// *********************************************** 
// NAME             : RoutingDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 03 Jan 2012
// DESCRIPTION  	: RoutingDetail class to hold information about a routing rules used in a journey.
//                  : This class should be updated in future as required when the routing detail is to be used in jouney output
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// RoutingDetail class to hold information about a routing rules used in a journey
    /// </summary>
    [Serializable()]
    public class RoutingDetail
    {
        #region Private members

        private List<string> routingRuleIDs = new List<string>();
        private List<string> routingReasons = new List<string>();
        private List<string> routingStops = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RoutingDetail()
        { 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RoutingDetail(string[] routingRuleIDs,
                             string[] routingReasons,
                             string[] routingStops)
        {
            if ((routingRuleIDs != null) && (routingRuleIDs.Length > 0))
            {
                this.routingRuleIDs.AddRange(routingRuleIDs);
            }

            if ((routingReasons != null) && (routingReasons.Length > 0))
            {
                this.routingReasons.AddRange(routingReasons);
            }

            if ((routingStops != null) && (routingStops.Length > 0))
            {
                this.routingStops.AddRange(routingStops);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. RoutingRuleIDs
        /// </summary>
        public List<string> RoutingRuleIDs
        {
            get { return routingRuleIDs; }
            set { routingRuleIDs = value; }
        }

        /// <summary>
        /// Read/Write. RoutingReasons
        /// </summary>
        public List<string> RoutingReasons
        {
            get { return routingReasons; }
            set { routingReasons = value; }
        }

        /// <summary>
        /// Read/Write. RoutingStops
        /// </summary>
        public List<string> RoutingStops
        {
            get { return routingStops; }
            set { routingStops = value; }
        }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Returns routingRuleIDs as a string
        /// </summary>
        public string ToStringRoutingRuleIDs()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in routingRuleIDs)
            {
                sb.Append(s);
                sb.Append(",");
            }

            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Returns routingReasons as a string
        /// </summary>
        public string ToStringRoutingReasons()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in routingReasons)
            {
                sb.Append(s);
                sb.Append(",");
            }

            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Returns routingStops as a string
        /// </summary>
        public string ToStringRoutingStops()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in routingStops)
            {
                sb.Append(s);
                sb.Append(",");
            }

            return sb.ToString().TrimEnd(',');
        }

        #endregion
    }
}
