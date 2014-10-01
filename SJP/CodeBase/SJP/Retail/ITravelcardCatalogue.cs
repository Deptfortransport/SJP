// *********************************************** 
// NAME             : ITravelcardCatalogue.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Jan 2012
// DESCRIPTION  	: Interface for the TravelcardCatalogue class
// ************************************************
// 

using SJP.UserPortal.JourneyControl;
using System;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// Interface for the TravelcardCatalogue
    /// </summary>
    public interface ITravelcardCatalogue
    {
        /// <summary>
        /// Method determines if the supplied journey leg has an applicable travelcard
        /// </summary>
        bool HasTravelcard(JourneyLeg journeyLeg);

        /// <summary>
        /// Method determines if the supplied journey leg with specified start and end datetimes
        /// has an applicable travelcard
        /// </summary>
        bool HasTravelcard(JourneyLeg journeyLeg, DateTime startTime, DateTime endTime);
    }
}
