// *********************************************** 
// NAME             : TestServiceFactoryInitialisation.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Jun 2011
// DESCRIPTION  	: Test service factory initialisation to help unit testing service discovery
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.ServiceDiscovery
{
    /// <summary>
    /// Test service factory initialisation to help unit testing service discovery
    /// </summary>
    class TestServiceFactoryInitialisation : IServiceInitialisation
    {
        #region Interface members

        /// <summary>
        /// IServiceInitialisation Populate method
        /// </summary>
        /// <param name="serviceCache"></param>
        public void Populate(Dictionary<string, IServiceFactory> serviceCache)
        {
            // Enable PropertyService
            serviceCache.Add("TestKey", new TestServiceFactory());

        }

        #endregion
    }
}
