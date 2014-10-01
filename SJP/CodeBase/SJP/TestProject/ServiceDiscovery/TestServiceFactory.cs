// *********************************************** 
// NAME             : TestServiceFactory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Jun 2011
// DESCRIPTION  	: Factory implementation to test the service discovery
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.ServiceDiscovery
{
    /// <summary>
    /// Factory implementation to test the service discovery
    /// </summary>
    public class TestServiceFactory: IServiceFactory
    {
        #region Private Fields
        private TestService current;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rm"></param>
        public TestServiceFactory()
        {
            current = new TestService();

        }
        #endregion

        #region IServiceFactory Interface Members
        /// <summary>
        /// Returns instance of TestService object
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            return current;
        }
        #endregion
    }
}
