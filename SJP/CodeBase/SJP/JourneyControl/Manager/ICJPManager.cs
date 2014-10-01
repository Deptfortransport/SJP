// *********************************************** 
// NAME             : ICJPManager.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: Interface for CJPManager
// ************************************************
// 

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Interface for CJPManager
    /// </summary>
    public interface ICJPManager
    {
        /// <summary>
        /// CallCJP handles the orchestration of the various calls to the Journey planner.
        /// </summary>
        /// <param name="request">Encapsulates journey parameters</param>
        /// <param name="sessionID">Used for logging purposes only</param>
        /// <param name="referenceTransaction">True is this an SLA-monitoring transacation</param>
        /// <param name="language">Two-character ISO id ("en" or "cy") of the current UI language</param>
        /// <returns>The results of the enquiry, including any error messages</returns>
        ISJPJourneyResult CallCJP(ISJPJourneyRequest request,
                                    string sessionId,
                                    bool referenceTransaction,
                                    string language);
    }
}
