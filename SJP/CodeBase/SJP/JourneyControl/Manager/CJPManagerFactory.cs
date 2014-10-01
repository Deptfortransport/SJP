// *********************************************** 
// NAME             : CJPManagerFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: Factory class to return instances of CJPManager
// ************************************************
// 
                
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Factory used by Service Discovery to create a CJPManager.
    /// </summary>
    public class CjpManagerFactory : IServiceFactory
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CjpManagerFactory()
        {
        }

        #endregion

        #region IServiceFactory methods

        /// <summary>
        ///  Method used by the ServiceDiscovery to get an
        ///  instance of the CJPManager class. A new object
        ///  is instantiated each time because a single shared
        ///  instance would not be thread-safe.
        /// </summary>
        /// <returns>A new instance of a ICJPManager.</returns>
        public Object Get()
        {
            ICJPManager manager = new CJPManager();
            return manager;
        }

        #endregion
    }
}
