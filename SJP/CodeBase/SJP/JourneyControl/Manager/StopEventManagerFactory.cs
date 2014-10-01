// *********************************************** 
// NAME             : StopEventManagerFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Factory class for StopEventManager
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Factory class for StopEventManager
    /// </summary>
    public class StopEventManagerFactory : IServiceFactory
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public StopEventManagerFactory()
        {
        }

        #endregion

        #region IServiceFactory methods

        /// <summary>
        /// Implementation of IServiceFactory. Returns a new StopEventManager every time it's requested.
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            IStopEventManager manager = new StopEventManager();
            return manager;
        }

        #endregion
    }
}
