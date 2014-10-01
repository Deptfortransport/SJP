// *********************************************** 
// NAME             : ITravelNewsHandler.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Interface for classes that make travel news available to clients
// ************************************************
// 


using SJP.UserPortal.TravelNews.SessionData;
using SJP.UserPortal.TravelNews.TravelNewsData;
using System.Collections.Generic;
using System;

namespace SJP.UserPortal.TravelNews
{
    /// <summary>
    /// Interface for classes that make travel news available to clients
    /// </summary>
    public interface ITravelNewsHandler
    {
        /// <summary>
        /// Returns whether the travel news is available
        /// </summary>
        bool IsTravelNewsAvaliable { get; }

        /// <summary>
        /// Returns the travel news unavailable text
        /// </summary>
        string TravelNewsUnavailableText { get; }

        /// <summary>
        /// Returns the travel news last updated date time 
        /// </summary>
        DateTime TravelNewsLastUpdated { get; }

        /// <summary>
        /// Returns filtered/sorted data grid data
        /// </summary>
        /// <param name="travelNewsState">Current display settings (drop-down values)</param>
        /// <returns></returns>
        TravelNewsItem[] GetDetails(TravelNewsState travelNewsState);


        /// <summary>
        /// Returns the TravelNewsItem associated with the given uid.
        /// </summary>
        TravelNewsItem GetDetailsByUid(string uid);

        /// <summary>
        /// Returns headline data
        /// </summary>
        /// <returns></returns>
        HeadlineItem[] GetHeadlines();

        /// <summary>
        /// Returns headline data
        /// </summary>
        /// <returns></returns>
        HeadlineItem[] GetHeadlines(TravelNewsState travelNewsState, string regionsToFilter);

        /// <summary>
        /// Returns children of given travel news ID
        /// </summary>
        TravelNewsItem[] GetChildrenDetailsByUid(string uid);
        
        /// <summary>
        /// Returns filtered/sorted travel news items for web
        /// </summary>
        TravelNewsItem[] GetDetailsForWeb(TravelNewsState travelNewsState, bool olympicIncidents);

        /// <summary>
        /// Returns filtered/sorted travel news items for web, grouped by the SeverityLevel
        /// </summary>
        Dictionary<SeverityLevel, List<TravelNewsItem>> GetDetailsForWebGroupedBySeverity(TravelNewsState travelNewsState, bool olympicIncidents);

        /// <summary>
        /// Returns filtered/sorted travel news items for mobile
        /// </summary>
        TravelNewsItem[] GetDetailsForMobile(TravelNewsState travelNewsState);
    }
}
