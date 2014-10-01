// *********************************************** 
// NAME             : QueryStringKey.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: QueryStringKey class containing all query string values used in SJP urls
// ************************************************
// 

namespace SJP.Common.Web
{
    /// <summary>
    /// QueryStringKey class containing all query string values used in SJP urls
    /// </summary>
    public class QueryStringKey
    {
        #region External used

        #region Site Preferences

        /// <summary>
        /// Langauge to set the page to, values can be "en" or "fr"
        /// </summary>
        public const string Language = "l";

        /// <summary>
        /// Font size style to apply to the page, values can be "s", "m", "l"
        /// </summary>
        public const string FontSize = "f";

        /// <summary>
        /// Accessibility style to apply to the page, values can be "n", "d", "h"
        /// </summary>
        public const string AccessiblityStyle = "a";

        /// <summary>
        /// Navigation (Header and Footer) required, values can be "0" = false, "1" = true (default)
        /// </summary>
        public const string Navigation = "pn";

        /// <summary>
        /// Site mode, vales can be:
        /// "o" = olympics (default until end of olympics), 
        /// "p" = paralympics (default from end of olympics)
        /// </summary>
        public const string SiteMode = "sm";

        /// <summary>
        /// Do not redirect to site, values true or false
        /// </summary>
        public const string DoNotRedirect = "dnr";

        /// <summary>
        /// Redirected to site, values "w" (web) or "m" (mobile)
        /// </summary>
        public const string RedirectedTo = "rt";

        #endregion

        #region Journey

        /// <summary>
        /// Origin location Id, the SJPLocation.ID value
        /// </summary>
        public const string OriginId = "o";

        /// <summary>
        /// Origin location Type, SJPLocationType value, values can be "v", "s", "sg", "l", "p", "en", "ll", "u"
        /// </summary>
        public const string OriginType = "ot";

        /// <summary>
        /// Origin location Name, the display name for the location
        /// </summary>
        public const string OriginName = "on";

        /// <summary>
        /// Destination location Id, the SJPLocation.ID value
        /// </summary>
        public const string DestinationId = "d";

        /// <summary>
        /// Destination location Type, SJPLocationType value, values can be "v", "s", "sg", "l", "p", "en", "ll", "u"
        /// </summary>
        public const string DestinationType = "dt";

        /// <summary>
        /// Destination location Name, the display name for the location
        /// </summary>
        public const string DestinationName = "dn";

        /// <summary>
        /// Outward date, in format yyyymmdd
        /// </summary>
        public const string OutwardDate = "do";

        /// <summary>
        /// Outward time, in format hhmm
        /// </summary>
        public const string OutwardTime = "to";

        /// <summary>
        /// Return date, in format yyyymmdd
        /// </summary>
        public const string ReturnDate = "dr";

        /// <summary>
        /// Return time, in format hhmm
        /// </summary>
        public const string ReturnTime = "tr";

        /// <summary>
        /// Outward required, values can be "0" = false, "1" = true
        /// </summary>
        public const string OutwardRequired = "ro";

        /// <summary>
        /// Return required, values can be "0" = false, "1" = true
        /// </summary>
        public const string ReturnRequired = "rr";

        /// <summary>
        /// Journey planner mode, SJPJourneyPlannerMode value, values can be "pt", "rs", "pr", "bb", "cy"
        /// </summary>
        public const string PlannerMode = "pm";

        /// <summary>
        /// Initiate auto planning of the journey, "0" = false, "1" = true
        /// </summary>
        public const string AutoPlan = "p";

        /// <summary>
        /// Partner identifier, string
        /// </summary>
        public const string Partner = "pa";

        /// <summary>
        /// Accessible journey planning option, 
        /// values can be "u" not underground, "a" assistance, "w" wheelchair, "wa" wheelchair and assistance
        /// </summary>
        public const string AccessibleOption = "gn";

        /// <summary>
        /// Fewest changes journey planning option, "0" = false, "1" = true
        /// </summary>
        public const string FewestChanges = "fc";

        #endregion

        #region Travel News 

        /// <summary>
        /// Travel News story Item unique identifier value used on Travel news page
        /// </summary>
        public const string NewsId = "nid";

        /// <summary>
        /// Travel news venues selected flag to filter on venues, values can be 0 or 1 (default)
        /// </summary>
        public const string NewsNaptanFlag = "nvf";

        /// <summary>
        /// Travel news venue naptan to filter for venue, "all", or "8100xxx"
        /// </summary>
        public const string NewsNaptan = "nv";

        /// <summary>
        /// Travel news region value used on Travel news page, values can be "all", (default) or character e.g. "em" (East Midlands)
        /// </summary>
        public const string NewsRegion = "nr";

        /// <summary>
        /// Travel news transport types used on Travel news page, values can be "all", "pt" (Public Transport), "r" (Road)
        /// </summary>
        public const string NewsTransportType = "ntt";

        /// <summary>
        /// Travel news search phrase used on Travel news page, URL encoded text
        /// </summary>
        public const string NewsSearch = "ns";

        /// <summary>
        /// Travel news date, in format yyyymmdd, e.g. "20120725"
        /// </summary>
        public const string NewsDate = "nd";

        /// <summary>
        /// Travel news mode to display on travel news page diplay, values can be "tn" (Travel News) (default) or "lul" (London Underground)
        /// </summary>
        public const string NewsMode = "nm";

        /// <summary>
        /// Travel news auto refresh, values can be 0 (default) or 1 
        /// </summary>
        public const string NewsRefresh = "nar";

        #endregion

        #endregion

        #region Internal used

        /// <summary>
        /// Journey request identifier to allow pages to load the correct journey request or result from session
        /// </summary>
        public const string JourneyRequestHash = "jrh";

        /// <summary>
        /// Journey Id outward, to allow pages to determine which journey has been selected. 
        /// This value should be used with the JourneyRequestHash so the journey can be retrieved from the 
        /// correct journey result
        /// </summary>
        public const string JourneyIdOutward = "jio";

        /// <summary>
        /// Journey Id return, to allow pages to determine which journey has been selected. 
        /// This value should be used with the JourneyRequestHash so the journey can be retrieved from the 
        /// correct journey result
        /// </summary>
        public const string JourneyIdReturn = "jir";

        /// <summary>
        /// Retailer Id used in the RetailerHandoff page for identifying the retailer to handoff to
        /// </summary>
        public const string Retailer = "rtr";
                
        /// <summary>
        /// Journey Leg Detail outward, to allow printer friendly page to determine whether the journey leg "detail"
        /// element should be expanded. 
        /// </summary>
        public const string JourneyLegDetailOutward = "jdo";

        /// <summary>
        /// Journey Leg Detail return, to allow printer friendly page to determine whether the journey leg "detail"
        /// element should be expanded. 
        /// </summary>
        public const string JourneyLegDetailReturn = "jdr";

        /// <summary>
        /// Journey Leg Id outward, to allow map page to show a specific leg, 
        /// should be used with used JourneyRequeshHash and JourneyIdOutward
        /// </summary>
        public const string JourneyLegIdOutward = "jlo";

        /// <summary>
        /// Journey Leg Id return, to allow map page to show a specific leg
        /// should be used with used JourneyRequeshHash and JourneyIdReturn
        /// </summary>
        public const string JourneyLegIdReturn = "jlr";

        /// <summary>
        /// Show walk - parameter for mobile map page. 
        /// </summary>
        public const string ShowWalk = "shw";

        /// <summary>
        /// Debug flag, to allow debug information to be shown in the UI (used in conjunction with switch in properties)
        /// </summary>
        public const string DebugMode = "dbg";

        #endregion
    }
}
