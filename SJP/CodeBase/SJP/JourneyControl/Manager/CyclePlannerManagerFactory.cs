// *********************************************** 
// NAME             : CyclePlannerManagerFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: Factory class to return instances of CyclePlannerManager
// ************************************************
// 

using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Factory used by Service Discovery to create a Cycle planner manager Stub.
    /// </summary>
    public class CyclePlannerManagerFactory : IServiceFactory
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CyclePlannerManagerFactory()
        {
        }

        #endregion

        #region IServiceFactory Members

        /// <summary>
        ///  Method used by the ServiceDiscovery to get an
        ///  instance of the CyclePlannerManager class. A new object
        ///  is instantiated each time because a single shared
        ///  instance would not be thread-safe.
        /// </summary>
        /// <returns>A new instance of a ICyclePlannerManager</returns>
        public Object Get()
        {
            ICyclePlannerManager manager = new CyclePlannerManager();
            return manager;
        }

        #endregion
    }
}
