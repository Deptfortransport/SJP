// *********************************************** 
// NAME             : TravelNewsHelper.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 17 May 2011
// DESCRIPTION  	: TravelNewsHelper class providing helper methods for travel news
// ************************************************


using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.TravelNews;
using SJP.UserPortal.TravelNews.SessionData;
using SJP.UserPortal.TravelNews.TravelNewsData;
using System.Collections.Generic;
using SJP.Common.LocationService;
using SJP.UserPortal.JourneyControl;
using System.Text;
using LS = SJP.Common.LocationService;


namespace SJP.Common.Web
{
    /// <summary>
    /// TravelNewsHelper class providing helper methods for travel news
    /// </summary>
    public class TravelNewsHelper
    {
        #region Public const

        public const string NewsViewMode_LUL = "LUL";
        public const string NewsViewMode_Venue = "Venue";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TravelNewsHelper()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns true if the travel news service is available and has travel news items
        /// </summary>
        /// <returns></returns>
        public bool TravelNewsAvailable()
        {
            ITravelNewsHandler tnDataServices = SJPServiceDiscovery.Current.Get<ITravelNewsHandler>(ServiceDiscoveryKey.TravelNews);

            return tnDataServices.IsTravelNewsAvaliable;
        }

        /// <summary>
        /// Returns true if the travel news service is available and has travel news items
        /// </summary>
        /// <returns></returns>
        public string TravelNewsUnavailableText()
        {
            ITravelNewsHandler tnDataServices = SJPServiceDiscovery.Current.Get<ITravelNewsHandler>(ServiceDiscoveryKey.TravelNews);

            return tnDataServices.TravelNewsUnavailableText;
        }

        /// <summary>
        /// Updates the travel news state to have default values for SJP
        /// </summary>
        /// <returns></returns>
        public TravelNewsState GetDefaultTravelNewsState(TravelNewsState currentNewsState)
        {
            currentNewsState = new TravelNewsState();
            currentNewsState.SetDefaultState();

            // Ensure default is for all venue naptans
            currentNewsState.SearchNaptans = GetVenueNaptans();

            // Ensure default start date is not before Games start date
            DateTime startDate = Properties.Current["JourneyPlanner.Validate.Games.StartDate"].Parse(DateTime.Now.Date);

            if (currentNewsState.SelectedDate < startDate)
            {
                currentNewsState.SelectedDate = startDate;
            }

            return currentNewsState;
        }

        /// <summary>
        /// Retrieves the current travel news state
        /// </summary>
        /// <returns></returns>
        public TravelNewsState GetTravelNewsState(bool readFromQueryString)
        {
            TravelNewsPageState tnPageState = SJPSessionManager.Current.TravelNewsPageState;

            TravelNewsState currentNewsState = tnPageState.TravelNewsState;

            if (currentNewsState == null)
            {
                currentNewsState = GetDefaultTravelNewsState(currentNewsState);               
            }

            if (readFromQueryString)
            {
                currentNewsState = UpdateTravelNewsStateFromQueryString(currentNewsState, true);
            }

            // Save the travel news state back in to session
            SetTravelNewsState(currentNewsState);

            return currentNewsState;
        }

        /// <summary>
        /// Retrieves the current travel news state, updated for mobile
        /// </summary>
        /// <param name="readFromQueryString"></param>
        /// <returns></returns>
        public TravelNewsState GetTravelNewsStateForMobile(bool readFromQueryString)
        {
            TravelNewsPageState tnPageState = SJPSessionManager.Current.TravelNewsPageState;

            TravelNewsState currentNewsState = tnPageState.TravelNewsState;

            if (currentNewsState == null)
            {
                currentNewsState = GetDefaultTravelNewsState(currentNewsState);

                // Ensure travel news state is as per SJP Mobile requirements
                // - PT only
                // - Today only
                // - All regions
                // - Venue and non-venue incidents
                // - Active incidents only
                currentNewsState.SelectedTransport = TransportType.PublicTransport;
                currentNewsState.SelectedDate = DateTime.Now.Date;
                currentNewsState.SelectedRegion = TravelNewsRegion.All.ToString();
                currentNewsState.SelectedVenuesFlag = false;
                currentNewsState.SearchNaptans = new List<string>(); // Reset naptans as mobile shouldn't restrict on them
                currentNewsState.SelectedIncidentActive = IncidentActiveStatus.Active.ToString();
            }
            
            if (readFromQueryString)
            {
                currentNewsState = UpdateTravelNewsStateFromQueryString(currentNewsState, false);

                // Ensure if venue naptans provided in URL, the news state is told to use them,
                // as mobile does not have a filter on venues checkbox
                if (currentNewsState.SearchNaptans.Count > 0)
                {
                    currentNewsState.SelectedVenuesFlag = true;
                }
            }

            // Save the travel news state back in to session
            SetTravelNewsState(currentNewsState);

            return currentNewsState;
        }

        /// <summary>
        /// Retrieves the current travel news id
        /// </summary>
        /// <param name="readFromQueryString"></param>
        /// <returns></returns>
        public string GetTravelNewsId(bool readFromQueryString)
        {
            string newsId = string.Empty;

            if (readFromQueryString)
            {
                if (HttpContext.Current != null)
                {
                    NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                    if (queryString != null)
                    {
                        newsId = queryString[QueryStringKey.NewsId];
                    }
                }
            }
            else
            {
                TravelNewsPageState tnPageState = SJPSessionManager.Current.TravelNewsPageState;

                TravelNewsState currentNewsState = tnPageState.TravelNewsState;

                if (currentNewsState == null)
                {
                    currentNewsState = GetDefaultTravelNewsState(currentNewsState);
                }

                newsId = currentNewsState.SelectedIncident;
            }

            return newsId;
        }

        /// <summary>
        /// Retrieves the current travel news mode
        /// </summary>
        /// <param name="readFromQueryString"></param>
        /// <returns></returns>
        public string GetTravelNewsMode(bool readFromQueryString)
        {
            string newsMode = string.Empty;

            if (readFromQueryString)
            {
                if (HttpContext.Current != null)
                {
                    NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                    if (queryString != null)
                    {
                        newsMode = queryString[QueryStringKey.NewsMode];
                    }
                }
            }

            return newsMode;
        }

        /// <summary>
        /// Retrieves the current travel news mode
        /// </summary>
        /// <param name="readFromQueryString"></param>
        /// <returns></returns>
        public bool GetTravelNewsAutoRefresh(bool readFromQueryString)
        {
            bool newsAutoRefresh = false;

            if (readFromQueryString)
            {
                if (HttpContext.Current != null)
                {
                    NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                    if (queryString != null)
                    {
                        try
                        {
                            newsAutoRefresh = (queryString[QueryStringKey.NewsRefresh] == "1");
                        }
                        catch
                        {
                            // Ignore exceptions, unrecognised value, defaults to false
                        }
                    }
                }
            }

            return newsAutoRefresh;
        }

        /// <summary>
        /// Saves the current travel news state to session
        /// </summary>
        /// <param name="currentNewsState"></param>
        public void SetTravelNewsState(TravelNewsState currentNewsState)
        {
            TravelNewsPageState tnPageState = SJPSessionManager.Current.TravelNewsPageState;

            if (currentNewsState == null)
            {
                currentNewsState = GetDefaultTravelNewsState(currentNewsState);
            }

            tnPageState.TravelNewsState = currentNewsState;

            SJPSessionManager.Current.TravelNewsPageState = tnPageState;
        }

        /// <summary>
        /// Builds a travel news landing page url
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="travelNewsState"></param>
        /// <param name="autoRefresh"></param>
        /// <returns></returns>
        public string BuildTravelNewsUrl(string pageUrl, TravelNewsState travelNewsState, 
            bool autoRefresh, bool includeVenuesFlag, bool includeTransportType, bool includeRegion, bool includeDate, string newsMode)
        {
            string url = string.Empty;
            NameValueCollection queryString = new NameValueCollection();

            if (travelNewsState != null)
            {
                // Build all the travel news landing page values

                // News mode (i.e. Travel News or LUL)
                if (!string.IsNullOrEmpty(newsMode) && (newsMode == NewsViewMode_LUL))
                {
                    queryString.Add(QueryStringKey.NewsMode, newsMode);
                }
                else if (!string.IsNullOrEmpty(travelNewsState.SelectedIncident))
                {
                    queryString.Add(QueryStringKey.NewsId, travelNewsState.SelectedIncident);
                }
                else
                {

                    // Naptans
                    if (travelNewsState.SearchNaptans != null && travelNewsState.SearchNaptans.Count > 0)
                    {
                        StringBuilder naptans = new StringBuilder();

                        // If all venues selected, add All rather than all the naptans
                        if (travelNewsState.SelectedAllVenuesFlag)
                        {
                            naptans.Append("all");
                        }
                        else
                        {
                            foreach (string naptan in travelNewsState.SearchNaptans)
                            {
                                if (naptans.Length > 0)
                                {
                                    naptans.Append(",");
                                }
                                naptans.Append(naptan);
                            }
                        }
                        queryString.Add(QueryStringKey.NewsNaptan, naptans.ToString());
                    }

                    // Naptan selected flag (only set if not default, i.e. not selected)
                    if (includeVenuesFlag && !travelNewsState.SelectedVenuesFlag)
                    {
                        queryString.Add(QueryStringKey.NewsNaptanFlag, "0");
                    }


                    // Region
                    if (includeRegion && !string.IsNullOrEmpty(travelNewsState.SelectedRegion))
                    {
                        TravelNewsRegionParser tnrp = new TravelNewsRegionParser();
                        TravelNewsRegion region = tnrp.GetTravelNewsRegion(travelNewsState.SelectedRegion);
                        
                        queryString.Add(QueryStringKey.NewsRegion, tnrp.GetTravelNewsRegionForQueryString(region));
                    }

                    // Transport Type
                    if (includeTransportType && travelNewsState.SelectedTransport != TransportType.All)
                    {
                        queryString.Add(QueryStringKey.NewsTransportType, GetTransportType(travelNewsState.SelectedTransport));
                    }

                    // Search
                    if (!string.IsNullOrEmpty(travelNewsState.SearchPhrase))
                    {
                        queryString.Add(QueryStringKey.NewsSearch, travelNewsState.SearchPhrase.Trim());
                    }

                    // Date
                    if (includeDate && travelNewsState.SelectedDate != DateTime.MinValue)
                    {
                        queryString.Add(QueryStringKey.NewsDate, travelNewsState.SelectedDate.ToString("yyyyMMdd"));
                    }
                }

                // Auto refresh
                if (autoRefresh)
                {
                    queryString.Add(QueryStringKey.NewsRefresh, "1");
                }
                else 
                {
                    queryString.Add(QueryStringKey.NewsRefresh, "0");
                }
            }

            if (!string.IsNullOrEmpty(pageUrl))
            {
                URLHelper urlHelper = new URLHelper();
                url = pageUrl;
                url = urlHelper.AddQueryStringParts(url, queryString);
            }

            return url;
        }

        /// <summary>
        /// Returns a full list of venue naptans to show the travel news for
        /// </summary>
        /// <returns></returns>
        public List<string> GetVenueNaptans()
        {
            List<string> naptans = new List<string>();

            try
            {
                LS.LocationService locationService = SJPServiceDiscovery.Current.Get<LS.LocationService>(ServiceDiscoveryKey.LocationService);
                List<SJPLocation> venues = locationService.GetSJPVenueLocations();

                foreach (SJPLocation venue in venues)
                {
                    naptans.Add(venue.Naptan.FirstOrDefault());
                }

            }
            catch
            {
                // Ignore any exceptions
            }

            return naptans;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the travel news state with values from the querystring
        /// </summary>
        /// <param name="currentNewsState"></param>
        private TravelNewsState UpdateTravelNewsStateFromQueryString(TravelNewsState currentNewsState, bool resetNewsState)
        {
            #region Read from query string

            string travelNewsIncidentId = string.Empty;
            string travelNewsNaptanFlag = string.Empty;
            string travelNewsNaptan = string.Empty;
            string travelNewsRegion = string.Empty;
            string travelNewsTransportType = string.Empty;
            string travelNewsSearch = string.Empty;
            string travelNewsDate = string.Empty;
            string journeyRequestId = string.Empty;

            // Read travel news query string values
            if (HttpContext.Current != null)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                if (queryString != null)
                {
                    travelNewsIncidentId = queryString[QueryStringKey.NewsId];
                    travelNewsNaptanFlag = queryString[QueryStringKey.NewsNaptanFlag];
                    travelNewsNaptan = queryString[QueryStringKey.NewsNaptan];
                    travelNewsRegion = queryString[QueryStringKey.NewsRegion];
                    travelNewsTransportType = queryString[QueryStringKey.NewsTransportType];
                    travelNewsSearch = queryString[QueryStringKey.NewsSearch];
                    travelNewsDate = queryString[QueryStringKey.NewsDate];
                    journeyRequestId = queryString[QueryStringKey.JourneyRequestHash];

                    // Because showing for query string values, must clear all previously set travel news values
                    if (resetNewsState)
                        currentNewsState = GetDefaultTravelNewsState(currentNewsState);
                }
            }

            // Show incident only
            if (!string.IsNullOrEmpty(travelNewsIncidentId))
            {
                #region Set state for incident

                currentNewsState.SelectedIncident = travelNewsIncidentId;

                ITravelNewsHandler tnDataServices = SJPServiceDiscovery.Current.Get<ITravelNewsHandler>(ServiceDiscoveryKey.TravelNews);

                // Get the region, type of transport etc from the news Id and set it in current travel news state
                TravelNewsItem newsItem = tnDataServices.GetDetailsByUid(travelNewsIncidentId);

                if (newsItem != null)
                {
                    currentNewsState.SelectedRegion = newsItem.Regions;
                }

                #endregion
            }
            else
            {
                // Has come from journeys page, retrieve any venue naptans
                if (!string.IsNullOrEmpty(journeyRequestId))
                {
                    #region Get venue naptans for journey

                    JourneyHelper journeyHelper = new JourneyHelper();

                    string journeyRequestHash = null;
                    Journey journeyOutward = null;
                    Journey journeyReturn = null;

                    // Only get if there are journeys and the request exists
                    if (journeyHelper.GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn))
                    {
                        if ((journeyOutward != null) || (journeyReturn != null))
                        {
                            SessionHelper sessionHelper = new SessionHelper();

                            // Set the local variable and let the next if deal with it
                            travelNewsNaptan = GetTravelNewsVenueNaptans(sessionHelper.GetSJPJourneyRequest(journeyRequestHash));
                        }
                    }

                    #endregion
                }

                // Show incidents for venue naptan only
                if (!string.IsNullOrEmpty(travelNewsNaptan))
                {
                    #region Set state for venue naptans

                    string naptan = HttpContext.Current.Server.UrlDecode(travelNewsNaptan);

                    if (!string.IsNullOrEmpty(naptan))
                    {
                        if (naptan.Trim().ToLower() == "all")
                        {
                            currentNewsState.SearchNaptans = new List<string>();
                            currentNewsState.SearchNaptans.AddRange(GetVenueNaptans());
                            currentNewsState.SelectedAllVenuesFlag = true;
                        }
                        else
                        {
                            string[] naptans = naptan.Split(',');

                            currentNewsState.SearchNaptans = new List<string>();
                            currentNewsState.SearchNaptans.AddRange(naptans);
                            currentNewsState.SelectedAllVenuesFlag = false;
                        }

                        // Get the region and set it in current travel news state,
                        // only if there is one venue (otherwise it defaults to all)
                        if (currentNewsState.SearchNaptans.Count == 1)
                        {
                            LocationHelper locationHelper = new LocationHelper();

                            SJPLocation location = locationHelper.GetSJPLocation(
                                currentNewsState.SearchNaptans[0], string.Empty, SJPLocationType.Venue);

                            if ((location != null) && (location is SJPVenueLocation))
                            {
                                SJPVenueLocation venue = (SJPVenueLocation)location;

                                if (!string.IsNullOrEmpty(venue.VenueTravelNewsRegion))
                                    currentNewsState.SelectedRegion = venue.VenueTravelNewsRegion;
                            }
                        }
                    }

                    #endregion
                }

                // Venues selected flag (in case the user had selected a naptan but unchecked venues box)
                if (!string.IsNullOrEmpty(travelNewsNaptanFlag))
                {
                    #region Set state for venue selected flag

                    try
                    {
                        currentNewsState.SelectedVenuesFlag = !(travelNewsNaptanFlag.Trim() == "0");
                    }
                    catch
                    {
                        // Ignore exceptions, unrecognised value, default to true
                        currentNewsState.SelectedVenuesFlag = true;
                    }

                    #endregion
                }

                // Show incidents for region, 
                // Note: This overrides previous region set (for Venue, or All)
                if (!string.IsNullOrEmpty(travelNewsRegion))
                {
                    #region Set state for region

                    TravelNewsRegionParser tnps = new TravelNewsRegionParser();

                    TravelNewsRegion tnr = tnps.GetTravelNewsRegion(travelNewsRegion);

                    currentNewsState.SelectedRegion = tnps.GetTravelNewsRegion(tnr);

                    #endregion
                }

                // Show for transport type
                if (!string.IsNullOrEmpty(travelNewsTransportType))
                {
                    #region Set state for transport type

                    currentNewsState.SelectedTransport = GetTransportType(travelNewsTransportType);

                    #endregion
                }

                // Show for search phrase
                if (!string.IsNullOrEmpty(travelNewsSearch))
                {
                    #region Set state for search phrase

                    currentNewsState.SearchPhrase = HttpContext.Current.Server.UrlDecode(travelNewsSearch);

                    #endregion
                }

                // Show incidents for date
                // Note: This potentially overrides the Games start/end date validation
                if (!string.IsNullOrEmpty(travelNewsDate))
                {
                    #region Set state for date

                    try
                    {
                        int year = Convert.ToInt32(travelNewsDate.Substring(0, 4));
                        int month = Convert.ToInt32(travelNewsDate.Substring(4, 2));
                        int day = Convert.ToInt32(travelNewsDate.Substring(6, 2));

                        DateTime datetime = new DateTime(year, month, day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

                        currentNewsState.SelectedDate = datetime;
                    }
                    catch
                    {
                        // Ignore exceptions
                    }

                    #endregion
                }
            }

            #endregion

            return currentNewsState;
        }

        /// <summary>
        /// Builds a comma seprated list of venue naptans for the journey request
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        /// <returns></returns>
        private string GetTravelNewsVenueNaptans(ISJPJourneyRequest sjpJourneyRequest)
        {
            StringBuilder result = new StringBuilder();
            
            if (sjpJourneyRequest != null)
            {
                // Venue naptans
                List<string> naptans = new List<string>();

                if (sjpJourneyRequest.Origin is LocationService.SJPVenueLocation)
                {
                    naptans.Add(sjpJourneyRequest.Origin.Naptan.FirstOrDefault());
                    naptans.Add(sjpJourneyRequest.Origin.Parent);
                }
                if (sjpJourneyRequest.Destination is LocationService.SJPVenueLocation)
                {
                    naptans.Add(sjpJourneyRequest.Destination.Naptan.FirstOrDefault());
                    naptans.Add(sjpJourneyRequest.Destination.Parent);
                }


                foreach (string s in naptans)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (result.Length > 0)
                            result.Append(",");

                        result.Append(s);
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Returns a transport type string used for page landing
        /// </summary>
        /// <param name="transportType"></param>
        /// <returns></returns>
        private string GetTransportType(TransportType transportType)
        {
            switch (transportType)
            {
                case TransportType.PublicTransport:
                    return "pt";
                case TransportType.Road:
                    return "r";
                case TransportType.All:
                default:
                    return "all";
            }
        }

        /// <summary>
        /// Returns a TransportType for a string used for page landing
        /// </summary>
        /// <param name="transportType"></param>
        /// <returns></returns>
        private TransportType GetTransportType(string transportType)
        {
            switch (transportType.Trim().ToLower())
            {
                case "pt":
                    return TransportType.PublicTransport;
                case "r":
                    return TransportType.Road;
                case "all":
                default:
                    return TransportType.All;
            }
        }

        #endregion
    }
}