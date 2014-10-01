// *********************************************** 
// NAME             : CyclePlannerFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: Factory class used be ServiceDiscovery to return an instance of the CyclePlanner
// ************************************************
// 
                
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.CyclePlannerService
{
    /// <summary>
    /// Factory class used be ServiceDiscovery to return an instance of the CyclePlanner
    /// </summary>
    public class CyclePlannerFactory : IServiceFactory
    {
        #region constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CyclePlannerFactory()
        {
        }

        #endregion

        #region IServiceFactory Members

        /// <summary>
        /// Method used by the ServiceDiscovery to get the instance of the CyclePlanner.
        /// </summary>
        /// <returns>An instance of the CyclePlanner.</returns>
        public Object Get()
        {
            return new CyclePlanner();
        }

        #endregion
    }
}
