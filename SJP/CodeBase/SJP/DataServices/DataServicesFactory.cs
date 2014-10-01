// *********************************************** 
// NAME             : DataServicesFactory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 07 Apr 2011
// DESCRIPTION  	: Factory class for the DataServices component
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ResourceManager;
using SJP.Common.ServiceDiscovery;

namespace SJP.Common.DataServices
{
    /// <summary>
    /// Factory class for the DataServices component
    /// </summary>
    public class DataServicesFactory : IServiceFactory
    {
        #region Private Fields
        private DataServices current;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rm"></param>
        public DataServicesFactory()
        {
            current = new DataServices();

        }
        #endregion

        #region IServiceFactory Interface Members
        /// <summary>
        /// Returns instance of DataServices object
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            return current;
        }
        #endregion
    }
}
