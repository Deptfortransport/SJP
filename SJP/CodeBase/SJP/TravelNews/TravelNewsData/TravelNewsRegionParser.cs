// *********************************************** 
// NAME             : TravelNewsRegionParser      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: TravelNewsRegionParser for parsing a text string into a TravelNewsRegion
// ************************************************
// 

namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// TravelNewsRegionParser for parsing a text string into a TravelNewsRegion
    /// </summary>
    public class TravelNewsRegionParser
    {
        #region Private members
                
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TravelNewsRegionParser()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the a Travel News region using spcified region string
        /// </summary>
        /// <param name="value">venue string</param>
        public TravelNewsRegion GetTravelNewsRegion(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // Make consistent
                string region = value.Trim().ToLower();
                region = region.Replace(" ", string.Empty);

                switch (region)
                {
                    case "eastanglia":
                    case "ea":
                        return TravelNewsRegion.EastAnglia;
                        
                    case "eastmidlands":
                    case "em":
                        return TravelNewsRegion.EastMidlands;
                        
                    case "london":
                    case "l":
                        return TravelNewsRegion.London;
                        
                    case "northeast":
                    case "ne":
                        return TravelNewsRegion.NorthEast;
                        
                    case "northwest":
                    case "nw":
                        return TravelNewsRegion.NorthWest;
                        
                    case "scotland":
                    case "s":
                        return TravelNewsRegion.Scotland;
                        
                    case "southeast":
                    case "se":
                        return TravelNewsRegion.SouthEast;
                        
                    case "southwest":
                    case "sw":
                        return TravelNewsRegion.SouthWest;
                        
                    case "wales":
                    case "w":
                        return TravelNewsRegion.Wales;
                        
                    case "westmidlands":
                    case "wm":
                        return TravelNewsRegion.WestMidlands;
                        
                    case "yorkshireandhumber":
                    case "yh":
                        return TravelNewsRegion.YorkshireandHumber;
                }
            }

            // Return All by default
            return TravelNewsRegion.All;
        }


        /// <summary>
        /// Gets the a region string using spcified TravelNewsRegion
        /// </summary>
        /// <param name="value">venue string</param>
        public string GetTravelNewsRegion(TravelNewsRegion value)
        {
            switch (value)
            {
                case TravelNewsRegion.EastAnglia:
                    return "East Anglia";

                case TravelNewsRegion.EastMidlands:
                    return "East Midlands";

                case TravelNewsRegion.London:
                    return "London";

                case TravelNewsRegion.NorthEast:
                    return "North East";

                case TravelNewsRegion.NorthWest:
                    return "North West";

                case TravelNewsRegion.Scotland:
                    return "Scotland";

                case TravelNewsRegion.SouthEast:
                    return "South East";

                case TravelNewsRegion.SouthWest:
                    return "South West";

                case TravelNewsRegion.Wales:
                    return "Wales";

                case TravelNewsRegion.WestMidlands:
                    return "West Midlands";

                case TravelNewsRegion.YorkshireandHumber:
                    return "Yorkshire and Humber";
            }

            // Return All by default
            return "All";
        }

        /// <summary>
        /// Gets the a region string for page landing using spcified TravelNewsRegion
        /// </summary>
        /// <param name="value">venue string</param>
        public string GetTravelNewsRegionForQueryString(TravelNewsRegion value)
        {
            switch (value)
            {
                case TravelNewsRegion.EastAnglia:
                    return "ea";

                case TravelNewsRegion.EastMidlands:
                    return "em";

                case TravelNewsRegion.London:
                    return "l";

                case TravelNewsRegion.NorthEast:
                    return "ne";

                case TravelNewsRegion.NorthWest:
                    return "nw";

                case TravelNewsRegion.Scotland:
                    return "s";

                case TravelNewsRegion.SouthEast:
                    return "se";

                case TravelNewsRegion.SouthWest:
                    return "sw";

                case TravelNewsRegion.Wales:
                    return "w";

                case TravelNewsRegion.WestMidlands:
                    return "wm";

                case TravelNewsRegion.YorkshireandHumber:
                    return "yh";
            }

            // Return All by default
            return "all";
        }

        #endregion
    }
}
