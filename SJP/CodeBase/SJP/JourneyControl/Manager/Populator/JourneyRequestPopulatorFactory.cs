// *********************************************** 
// NAME             : JourneyRequestPopulatorFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 30 Mar 2011
// DESCRIPTION  	: Factory class to return an instance of a JourneyRequestPopulator
// ************************************************
// 

using SJP.Common;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Factory class to return an instance of a JourneyRequestPopulator
    /// </summary>
    public class JourneyRequestPopulatorFactory
    {
        #region Public Static methods

        /// <summary>
        /// Instantiates and returns a JourneyRequestPopulator of the appropriate
        /// subclass to create the CJP or CTP requests for a specific ISJPJourneyRequest. 
        /// </summary>
        public static JourneyRequestPopulator GetPopulator(ISJPJourneyRequest request)
        {
            JourneyRequestPopulator populator = null;

            // Cycle request
            if (request.Modes.Contains(SJPModeType.Cycle))
            {
                populator = new CyclePlannerRequestPopulator(request);
            }
            // Car request
            else if (request.Modes.Count == 1 && request.Modes[0] == SJPModeType.Car)
            {
                populator = new MultiModalJourneyRequestPopulator(request);
            }
            // PT (and Car) request
            else
            {
                populator = new MultiModalJourneyRequestPopulator(request);
            }
            
            return populator;
        }

        #endregion
    }
}
