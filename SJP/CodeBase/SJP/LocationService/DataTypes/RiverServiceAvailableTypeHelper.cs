// *********************************************** 
// NAME             : RiverServiceAvailableTypeHelper.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Dec 2011
// DESCRIPTION  	: Helper to convert the type of river service availability for a venue
// ************************************************

namespace SJP.Common.LocationService
{
    public class RiverServiceAvailableTypeHelper
    {
        /// <summary>
        /// Parses a database river service available type value into an RiverServiceAvailableType
        /// </summary>
        /// <param name="cjpModeType"></param>
        public static RiverServiceAvailableType GetRiverServiceAvailableType(string dbRiverServiceAvailableType)
        {
            string riverServiceAvailableType = dbRiverServiceAvailableType.ToUpper().Trim();

            switch (riverServiceAvailableType)
            {
                case "YES":
                    return RiverServiceAvailableType.Yes;
                case "MAYBE":
                    return RiverServiceAvailableType.Maybe;
                case "NO":
                default:
                    return RiverServiceAvailableType.No;
            }
        }
    }
}
