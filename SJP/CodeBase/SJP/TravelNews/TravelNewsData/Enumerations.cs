// *********************************************** 
// NAME             : Enumerations.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Contains classes to convert travel news data between internal and external values
// ************************************************
// 


namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// KeyValue
    /// </summary>
    internal class KeyValue
    {
        public const string All = "All";
        public const string PublicTransport = "Public Transport";
        public const string Road = "Road";
        public const string Recent = "Recent";
        public const string Full = "Full";
        public const string Summary = "Summary";
        public const string Major = "Very Severe";
        public const string Unplanned = "Unplanned";
        public const string Planned = "Planned";

    }

    /// <summary>
    /// TransportType
    /// </summary>
    public enum TransportType
    {
        All,
        PublicTransport,
        Road
    }

    /// <summary>
    /// DelayType
    /// </summary>
    public enum DelayType
    {
        All,
        Major,
        Recent
    }

    /// <summary>
    /// IncidentType enum for show news control filter 
    /// </summary>
    public enum IncidentType
    {
        All,
        Unplanned,
        Planned
    }

    /// <summary>
    /// DisplayType
    /// </summary>
    public enum DisplayType
    {
        Full,
        Summary
    }

    /// <summary>
    /// SeverityLevel
    /// </summary>
    public enum SeverityLevel
    {
        Critical = 0,
        Serious = 1,
        VerySevere = 2,
        Severe = 3,
        Medium = 4,
        Slight = 5,
        VerySlight = 6
    }

    /// <summary>
    /// SeverityFilter
    /// </summary>
    public enum SeverityFilter
    {
        Default,
        CriticalIncidents
    }

    /// <summary>
    /// IncidentActiveStatus enum
    /// </summary>
    public enum IncidentActiveStatus : byte
    {
        Inactive,
        Active
    }

    /// <summary>
    /// Enumeration used by Travel News page when determining 
    /// whether to display details in a table or a map
    /// </summary>
    public enum TravelNewsViewType
    {
        Details,
        Map
    }

    /// <summary>
    /// Enum to spcify travel news regions
    /// </summary>
    public enum TravelNewsRegion
    {
        All,
        EastAnglia,
        EastMidlands,
        London,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest,
        WestMidlands,
        YorkshireandHumber,
        Scotland,
        Wales
    }
}
