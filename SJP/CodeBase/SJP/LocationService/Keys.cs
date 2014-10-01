// *********************************************** 
// NAME             : Keys.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: Location Service Keys class defining property keys used in the project
// ************************************************
// 

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Location Service Keys class defining property keys used in the project
    /// </summary>
    public class Keys
    {
        public const string Cache_LoadLocations = "LocationService.Cache.LoadLocations";
        public const string Cache_LoadPostcodes = "LocationService.Cache.LoadPostcodes";

        public const string NaptanPrefix_Airport = "LocationService.NaptanPrefix.Airport";
        public const string NaptanPrefix_Coach = "LocationService.NaptanPrefix.Coach";
        public const string NaptanPrefix_Rail = "LocationService.NaptanPrefix.Rail";

        public const string Search_Cache_Locations = "LocationService.SearchLocations.InCache";
        public const string Max_SearchLocationsLimit = "LocationService.SearchLocationsLimit.Count.Max";
        public const string Max_SearchLocationsShow = "LocationService.SearchLocationsShow.Count.Max";
        public const string Max_SearchLocationsShowLimit_GroupStations = "LocationService.SearchLocationsShow.GroupStationsLimit.Count";
        public const string Max_SearchLocationsShowLimit_RailStations = "LocationService.SearchLocationsShow.RailStationsLimit.Count";
        public const string Max_SearchLocationsShowLimit_CoachStations = "LocationService.SearchLocationsShow.CoachStationsLimit.Count";
        public const string Max_SearchLocationsShowLimit_TramStations = "LocationService.SearchLocationsShow.TramStationsLimit.Count";
        public const string Max_SearchLocationsShowLimit_FerryStations = "LocationService.SearchLocationsShow.FerryStationsLimit.Count";
        public const string Max_SearchLocationsShowLimit_AirportStations = "LocationService.SearchLocationsShow.AirportStationsLimit.Count";

        public const string CommonWords = "LocationService.CommonWords";
        public const string SimilarityIndex_NoCommonWords = "LocationService.SimilarityIndex.NoCommonWords.Min";
        public const string SimilarityIndex_NoCommonWordsAndSpace = "LocationService.SimilarityIndex.NoCommonWordsAndSpace.Min";
        public const string SimilarityIndex_IndividualWords = "LocationService.SimilarityIndex.IndividualWords.Min";
        public const string SimilarityIndex_ChildLocalityAtEnd = "LocationService.SimilarityIndex.ChildLocalityAtEnd";

        public const string CoordinateLocation_LocalitySearch_Padding_Easting = "LocationService.CoordinateLocation.LocalitySearch.EastingPadding.Metres";
        public const string CoordinateLocation_LocalitySearch_Padding_Northing = "LocationService.CoordinateLocation.LocalitySearch.NorthingPadding.Metres";
        public const string CoordinateLocation_AppendLocalityName = "LocationService.CoordinateLocation.LocationName.AppendLocalityName.Switch";
    }
}
