// *********************************************** 
// NAME             : TravelNewsTransportModeParser.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 08 Mar 2012
// DESCRIPTION  	: TravelNewsTransportModeParser for parsing a text string into a SJPModeType
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common;

namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// TravelNewsTransportModeParser for parsing a text string into a SJPModeType
    /// </summary>
    public static class TravelNewsTransportModeParser
    {
        #region Public methods

        /// <summary>
        /// Gets the an SJPModeType using spcified travel news mode of transport string
        /// </summary>
        /// <param name="value">venue string</param>
        public static SJPModeType GetSJPModeType(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // Mode of transport is a free text string according to the xml schema, 
                // so best attempt is to check for matching strings.
                value = value.ToLower().Trim().Replace(" ", string.Empty);

                if (value.Contains("air"))
                    return SJPModeType.Air;
                else if (value.Contains("bus"))
                    return SJPModeType.Bus;
                else if (value.Contains("cable"))
                    return SJPModeType.CableCar;
                else if (value.Contains("car"))
                    return SJPModeType.Car;
                else if (value.Contains("road"))
                    return SJPModeType.Car;
                else if (value.Contains("coach"))
                    return SJPModeType.Coach;
                else if (value.Contains("cycle"))
                    return SJPModeType.Cycle;
                else if (value.Contains("lightrail"))
                    return SJPModeType.Drt;
                else if (value.Contains("ferry"))
                    return SJPModeType.Ferry;
                else if (value.Contains("metro"))
                    return SJPModeType.Metro;
                else if (value.Contains("rail"))
                    return SJPModeType.Rail;
                else if (value.Contains("tram"))
                    return SJPModeType.Tram;
                else if (value.Contains("underground"))
                    return SJPModeType.Underground;
                else if (value.Contains("walk"))
                    return SJPModeType.Walk;
                else if (value.Contains("eurotunnel"))
                    return SJPModeType.EuroTunnel;
            }

            // Return default
            return SJPModeType.Unknown;
        }




        #endregion
    }
}
