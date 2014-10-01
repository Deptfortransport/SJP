// *********************************************** 
// NAME             : ICyclePlanner.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: Interface for the CyclePlanner class
// ************************************************
// 

using SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;

namespace SJP.UserPortal.CyclePlannerService
{
    /// <summary>
    /// Interface for the CyclePlanner class
    /// </summary>
    public interface ICyclePlanner
    {
        /// <summary>
        /// Method that returns a Cycle Journey for the supplied parameters
        /// </summary>
        CyclePlannerResult CycleJourneyPlan(CyclePlannerRequest cyclePlannerRequest);
    }
}
