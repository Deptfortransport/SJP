// *********************************************** 
// NAME             : LocationServiceFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: Factory class for returning LocationService
// ************************************************
// 
                
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Factory class for returning LocationService
    /// </summary>
    public class LocationServiceFactory : IServiceFactory
    {
        #region Private members

        private LocationService current;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocationServiceFactory()
        {
            current = new LocationService();
        }

        #endregion

        #region IServiceFactory methods

        /// <summary>
        ///  Method used by the ServiceDiscovery to get the
        ///  instance of the LocationService.
        /// </summary>
        /// <returns>The current instance of the LocationService.</returns>
        public Object Get()
        {
            return current;
        }

        #endregion
    }
}
