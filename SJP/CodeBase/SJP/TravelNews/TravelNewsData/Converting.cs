// *********************************************** 
// NAME             : Converting.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Class for converting travel news data between internal and external values.
// ************************************************
// 


namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// Class for converting travel news data between internal and external values.
    /// </summary>
    sealed public class Converting
    {
        /// <summary>
        /// Constructor - does nothing.
        /// </summary>
        private Converting() { }

        /// <summary>
        /// Converts travel news incident transport type to string
        /// </summary>
        /// <param name="type">transport type</param>
        /// <returns>transport type as string</returns>
        public static string ToString(TransportType type)
        {
            switch (type)
            {
                case TransportType.PublicTransport:
                    return KeyValue.PublicTransport;
                case TransportType.Road:
                    return KeyValue.Road;
                case TransportType.All:
                default:
                    return KeyValue.All;
            }

        }
        /// <summary>
        /// Converts travel news incident delay type to string
        /// </summary>
        /// <param name="type">delay type</param>
        /// <returns>delay type as string</returns>
        public static string ToString(DelayType type)
        {
            switch (type)
            {
                case DelayType.Major:
                    return KeyValue.Major;
                case DelayType.Recent:
                    return KeyValue.Recent;
                default:
                case DelayType.All:
                    return KeyValue.All;
            }
        }

        /// <summary>
        /// Converts travel news incidentType type to string
        /// </summary>
        /// <param name="type">incidentType type</param>
        /// <returns>incidentType type as string</returns>
        public static string ToString(IncidentType type)
        {
            switch (type)
            {
                case IncidentType.Planned:
                    return KeyValue.Planned;
                case IncidentType.Unplanned:
                    return KeyValue.Unplanned;
                default:
                case IncidentType.All:
                    return KeyValue.All;
            }
        }
        /// <summary>
        /// Converts travel news incident display type to string
        /// </summary>
        /// <param name="type">display type</param>
        /// <returns>display type as string</returns>
        public static string ToString(DisplayType type)
        {
            switch (type)
            {
                case DisplayType.Full:
                    return KeyValue.Full;
                case DisplayType.Summary:
                default:
                    return KeyValue.Summary;
            }
        }

    }
}
