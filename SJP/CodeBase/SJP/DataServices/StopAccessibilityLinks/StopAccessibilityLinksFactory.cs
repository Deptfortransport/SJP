// *********************************************** 
// NAME             : StopAccessibilityLinksFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 June 2011
// DESCRIPTION  	: Factory class for StopAccessibilityLinks
// ************************************************
// 

using SJP.Common.ServiceDiscovery;

namespace SJP.Common.DataServices.StopAccessibilityLinks
{
    /// <summary>
    /// Factory class for StopAccessibilityLinks
    /// </summary>
    public class StopAccessibilityLinksFactory : IServiceFactory
    {
        private StopAccessibilityLinks current;

        #region Implementation of IServiceFactory

        /// <summary>
        /// Standard constructor. Initialises the StopAccessibilityLinks.
        /// </summary>
        public StopAccessibilityLinksFactory()
        {
            current = new StopAccessibilityLinks();
        }

        /// <summary>
        /// Returns the current StopAccessibilityLinks object
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            return current;
        }

        #endregion
    }
}
