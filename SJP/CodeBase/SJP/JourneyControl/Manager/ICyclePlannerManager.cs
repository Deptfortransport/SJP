// *********************************************** 
// NAME             : ICyclePlannerManager.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: Interface for CyclePlannerManager
// ************************************************
// 

namespace SJP.UserPortal.JourneyControl
{
    public interface ICyclePlannerManager
    {
        /// <summary>
        /// CallCyclePlanner handles the orchestration of the various calls to the Cycle planner.
        /// </summary>
        /// <param name="request">Encapsulates cycle journey parameters</param>
        /// <param name="sessionID">Used for logging purposes only</param>
        /// <param name="referenceTransaction">True is this an SLA-monitoring transaction</param>
        /// <param name="language">Two-character ISO id ("en" or "cy") of the current UI language</param>
        /// <returns>The results of the enquiry, including any error messages</returns>
        ISJPJourneyResult CallCyclePlanner(ISJPJourneyRequest request,
            string sessionId,
            bool referenceTransaction,
            string language);
    }
}
