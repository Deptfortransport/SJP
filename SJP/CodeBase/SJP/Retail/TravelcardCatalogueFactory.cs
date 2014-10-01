// *********************************************** 
// NAME             : TravelcardCatalogueFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 12 Jan 2012
// DESCRIPTION  	: TravelcardCatalogueFactory class that allows the
// ServiceDiscovery to create an instance of the TravelcardCatalogue class
// ************************************************
// 
                
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// TravelcardCatalogueFactory class that allows the
    /// ServiceDiscovery to create an instance of the TravelcardCatalogue class
    /// </summary>
    public class TravelcardCatalogueFactory : IServiceFactory
    {
        #region Private members

        private ITravelcardCatalogue current;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TravelcardCatalogueFactory()
        {
            current = new TravelcardCatalogue();
        }

        #endregion

        #region IServiceFactory methods

        /// <summary>
        ///  Method used by the ServiceDiscovery to get the
        ///  instance of the TravelcardCatalogue.
        /// </summary>
        /// <returns>The current instance of the TravelcardCatalogue.</returns>
        public Object Get()
        {
            return current;
        }

        #endregion
    }
}
