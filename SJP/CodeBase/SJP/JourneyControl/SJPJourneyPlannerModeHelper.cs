// *********************************************** 
// NAME             : SJPJourneyPlannerModeHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Feb 2012
// DESCRIPTION  	: SJPJourneyPlannerMode helper class
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// SJPJourneyPlannerMode helper class
    /// </summary>
    public class SJPJourneyPlannerModeHelper
    {
        /// <summary>
        /// Parses a query string journey planner moder value into an SJPJourneyPlannerMode
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static SJPJourneyPlannerMode GetSJPJourneyPlannerModeQS(string qsPlannerMode)
        {
            try
            {
                string plannerMode = qsPlannerMode.ToUpper().Trim();

                switch (plannerMode)
                {
                    case "PT":
                        return SJPJourneyPlannerMode.PublicTransport;
                    case "RS":
                        return SJPJourneyPlannerMode.RiverServices;
                    case "PR":
                        return SJPJourneyPlannerMode.ParkAndRide;
                    case "BB":
                        return SJPJourneyPlannerMode.BlueBadge;
                    case "CY":
                        return SJPJourneyPlannerMode.Cycle;
                    default:
                        throw new Exception("Failed to parse SJPJourneyPlannerMode");
                }
            }
            catch
            {
                // Any exception, default to PublicTransport
                return SJPJourneyPlannerMode.PublicTransport;
            }
        }

        /// <summary>
        /// Returns a query string representation of the SJPJourneyPlannerMode
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static string GetSJPJourneyPlannerModeQS(SJPJourneyPlannerMode plannerMode)
        {
            switch (plannerMode)
            {
                case SJPJourneyPlannerMode.RiverServices:
                    return "RS";
                case SJPJourneyPlannerMode.ParkAndRide:
                    return "PR";
                case SJPJourneyPlannerMode.BlueBadge:
                    return "BB";
                case SJPJourneyPlannerMode.Cycle:
                    return "CY";
                default:
                    return "PT";
            }
        }
    }
}
