// *********************************************** 
// NAME             : RetailHandoffSchemaFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: Factory class for returning RetailHandoffSchema class
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
    /// Factory class for returning RetailHandoffSchema class
    /// </summary>
    public class RetailHandoffSchemaFactory: IServiceFactory
    {
        #region Private members

        // Singleton instance of RetailHandoffSchema
        RetailHandoffSchema current;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetailHandoffSchemaFactory()
        {
            current = new RetailHandoffSchema();
        }

        #endregion

        #region IServiceFactory

        /// <summary>
        /// Returns a singleton instance of RetailHandoffSchema
        /// </summary>
        /// <returns>An instance of RetailHandoffSchema</returns>
        public object Get()
        {
            return current;
        }

        #endregion
    }
}
