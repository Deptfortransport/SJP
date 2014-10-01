// *********************************************** 
// NAME             : SJPLocationTypeHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: SJPLocationTypeHelper class
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// SJPLocationTypeHelper class
    /// </summary>
    public class SJPLocationTypeHelper
    {
        /// <summary>
        /// Parses a database location type value into an SJPLocationType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static SJPLocationType GetSJPLocationType(string dbLocationType)
        {
            string locationType = dbLocationType.ToUpper().Trim();
    
            switch (locationType)
            {
                case "AIRPORT":
                case "COACH":
                case "FERRY":
                case "RAIL STATION":
                case "TMU":
                    return SJPLocationType.Station;
                case "LOCALITY":
                    return SJPLocationType.Locality;
                case "VENUEPOI":
                    return SJPLocationType.Venue;
                case "EXCHANGE GROUP":
                    return SJPLocationType.StationGroup;
                case "POSTCODE":
                    return SJPLocationType.Postcode;
                default:
                    throw new SJPException(
                        string.Format("Error parsing database Location Type value into an SJPLocationType, unrecognised value[{0}]", dbLocationType),
                        false, SJPExceptionIdentifier.LSErrorParsingLocationType);
            }
        }

        /// <summary>
        /// Parses a database location type value into an SJPLocationType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static SJPLocationType GetSJPLocationTypeActual(string dbLocationType)
        {
            string locationType = dbLocationType.ToUpper().Trim();

            switch (locationType)
            {
                case "AIRPORT":
                    return SJPLocationType.StationAirport;
                case "COACH":
                    return SJPLocationType.StationCoach;
                case "FERRY":
                    return SJPLocationType.StationFerry;
                case "RAIL STATION":
                    return SJPLocationType.StationRail;
                case "TMU":
                    return SJPLocationType.StationTMU;
                case "LOCALITY":
                    return SJPLocationType.Locality;
                case "VENUEPOI":
                    return SJPLocationType.Venue;
                case "EXCHANGE GROUP":
                    return SJPLocationType.StationGroup;
                case "POSTCODE":
                    return SJPLocationType.Postcode;
                default:
                    throw new SJPException(
                        string.Format("Error parsing database Location Type value into an SJPLocationType, unrecognised value[{0}]", dbLocationType),
                        false, SJPExceptionIdentifier.LSErrorParsingLocationType);
            }
        }

        /// <summary>
        /// Parses a query string location type value into an SJPLocationType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static SJPLocationType GetSJPLocationTypeQS(string qsLocationType)
        {
            string locationType = qsLocationType.ToUpper().Trim();

            switch (locationType)
            {
                case "S":
                    return SJPLocationType.Station;
                case "L":
                    return SJPLocationType.Locality;
                case "V":
                    return SJPLocationType.Venue;
                case "SG":
                    return SJPLocationType.StationGroup;
                case "P":
                    return SJPLocationType.Postcode;
                case "EN":
                    return SJPLocationType.CoordinateEN;
                case "LL":
                    return SJPLocationType.CoordinateLL;
                case "U":
                    return SJPLocationType.Unknown;
                default:
                    throw new SJPException(
                        string.Format("Error parsing database Location Type value into an SJPLocationType, unrecognised value[{0}]", qsLocationType),
                        false, SJPExceptionIdentifier.LSErrorParsingLocationType);
            }
        }

        /// <summary>
        /// Returns a query string representation of the SJPLocationType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static string GetSJPLocationTypeQS(SJPLocationType locationType)
        {
            switch (locationType)
            {
                case SJPLocationType.Station:
                    return "S";
                case SJPLocationType.Locality:
                    return "L";
                case SJPLocationType.Venue:
                    return "V";
                case SJPLocationType.StationGroup:
                    return "SG";
                case SJPLocationType.Postcode:
                    return "P";
                case SJPLocationType.CoordinateEN:
                    return "EN";
                case SJPLocationType.CoordinateLL:
                    return "LL";
                default:
                    return "U";
            }
        }
    }
}
