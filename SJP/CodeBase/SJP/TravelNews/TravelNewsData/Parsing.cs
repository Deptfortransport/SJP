// *********************************************** 
// NAME             : Parsing.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Classes for converting travel 
// news data between internal and external values.
// ************************************************
// 

using System;

namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// Class for converting travel news data between internal and external values.
    /// </summary>
    sealed public class Parsing
    {
        /// <summary>
        /// Constructor - does nothing.
        /// </summary>
        private Parsing() { }

        /// <summary>
        /// Converts travel news incident severity level to SeverityLevel
        /// </summary>
        /// <param name="level">severity level</param>
        /// <returns>severity level as SeverityLevel</returns>
        public static SeverityLevel ParseSeverityLevel(byte level)
        {
            SeverityLevel sLevel = (SeverityLevel)level;
            return sLevel;
        }

        /// <summary>
        /// Parses travel news incident severity string in to travel news incident severity level
        /// </summary>
        /// <param name="level">severity level as string</param>
        /// <returns>severity level as SeverityLevel</returns>
        public static SeverityLevel ParseSeverityLevel(string level)
        {
            SeverityLevel sLevel = (SeverityLevel)Enum.Parse(typeof(SeverityLevel),level);
            return sLevel;
        }
        
        /// <summary>
        /// Converts travel news incident transport type to TransportType
        /// </summary>
        /// <param name="type">transport type</param>
        /// <returns>transport type as TransportType</returns>
        public static TransportType ParseTransportType(string type)
        {
            switch (type)
            {
                case KeyValue.PublicTransport:
                    return TransportType.PublicTransport;
                case KeyValue.Road:
                    return TransportType.Road;
                default:
                case KeyValue.All:
                    return TransportType.All;

            }
        }

        /// <summary>
        ///  Converts travel news incident delay type to DelayType
        /// </summary>
        /// <param name="type">delay type</param>
        /// <returns>delay type as DelayType</returns>
        public static DelayType ParseDelayType(string type)
        {
            switch (type)
            {
                case KeyValue.Major:
                    return DelayType.Major;
                case KeyValue.Recent:
                    return DelayType.Recent;
                default:
                case KeyValue.All:
                    return DelayType.All;
            }
        }

        /// <summary>
        ///  Converts travel news incidentType type to IncidentType
        /// </summary>
        /// <param name="type">incidentType type</param>
        /// <returns>incidentType type as IncidentType</returns>
        public static IncidentType ParseIncidentType(string type)
        {
            switch (type)
            {
                case KeyValue.Unplanned:
                    return IncidentType.Unplanned;
                case KeyValue.Planned:
                    return IncidentType.Planned;
                default:
                case KeyValue.All:
                    return IncidentType.All;
            }
        }

        /// <summary>
        /// Converts travel news incident display type to DisplayType
        /// </summary>
        /// <param name="type">display type</param>
        /// <returns>display type as DisplayType</returns>
        public static DisplayType ParseDisplayType(string type)
        {
            switch (type)
            {
                case KeyValue.Summary:
                    return DisplayType.Summary;
                case KeyValue.Full:
                default:
                    return DisplayType.Full;
            }
        }

    }
}
