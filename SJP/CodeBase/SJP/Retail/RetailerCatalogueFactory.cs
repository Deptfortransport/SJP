// *********************************************** 
// NAME             : RetailerCatalogueFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Mar 2011
// DESCRIPTION  	: RetailerCatalogueFactory class that allows the
// ServiceDiscovery to create an instance of the RetailerCatalogue class.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// RetailerCatalogueFactory class that allows the
    /// ServiceDiscovery to create an instance of the RetailerCatalogue class.
    /// </summary>
    public class RetailerCatalogueFactory : IServiceFactory
    {
        #region Private members

        private IRetailerCatalogue current;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetailerCatalogueFactory()
        {
            current = new RetailerCatalogue();
        }

        #endregion

        #region IServiceFactory methods

        /// <summary>
        ///  Method used by the ServiceDiscovery to get the
        ///  instance of the RetailerCatalogue.
        /// </summary>
        /// <returns>The current instance of the RetailerCatalogue.</returns>
        public Object Get()
        {
            return current;
        }

        #endregion
    }
}
