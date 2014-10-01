// *********************************************** 
// NAME             : ICycleJourneyPlanRunnerCaller.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Apr 2011
// DESCRIPTION  	: Provides interface to invoke Cycle Planner manager
// ************************************************
// 

using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Provides interface to invoke Cycle Planner manager
    /// </summary>
    interface ICycleJourneyPlanRunnerCaller
    {
        ///<summary>
        ///Invoke the SJP Cycle Planner Manager for a new journey request
        ///</summary>
        /// <param name="journeyRequest">Journey request</param>
        /// <param name="sessionInfo">Current sjp session information</param>
        /// <param name="lang">Two-character ISO id ("en" or "fr") of the current UI language</param>
        void InvokeCyclePlannerManager(ISJPJourneyRequest journeyRequest, ISJPSession sessionInfo, string lang);
    }
}
