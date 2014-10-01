// *********************************************** 
// NAME             : IStopEventRunnerCaller.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Provides interface to invoke Stop Event manager
// ************************************************
// 

using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Provides interface to invoke Stop Event manager
    /// </summary>
    public interface IStopEventRunnerCaller
    {
        ///<summary>
        ///Invoke the SJP Stop Event Manager for a new journey request
        ///</summary>
        /// <param name="journeyRequest">Journey request for stop event request</param>
        /// <param name="sessionInfo">Current sjp session information</param>
        /// <param name="lang">Two-character ISO id ("en" or "fr") of the current UI language</param>
        void InvokeStopEventManager(ISJPJourneyRequest journeyRequest, ISJPSession sessionInfo, string lang);
    }
}
