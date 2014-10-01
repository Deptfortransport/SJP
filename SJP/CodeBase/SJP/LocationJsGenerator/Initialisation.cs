// *********************************************** 
// NAME             : Initialisation.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 04 Mar 2011
// DESCRIPTION  	: Service Discovery cache initialisation 
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;
using SJP.Common.PropertyManager;
using System.Diagnostics;
using SJP.Common.EventLogging;

namespace SJP.Common.LocationJsGenerator
{
    /// <summary>
    /// Initialisation class for service discovery
    /// Initialises services required fro LocationJsGenerator
    /// </summary>
    class Initialisation : IServiceInitialisation
    {
        #region Public Methods
        /// <summary>
        /// Method to initialise Service Discovery cache with relevant services
        /// </summary>
        /// <param name="serviceCache">Service cache to initialise</param>
        public void Populate(Dictionary<string, IServiceFactory> serviceCache)
        {
            try
            {
                serviceCache.Add(ServiceDiscoveryKey.PropertyService, new PropertyServiceFactory());
            }
            catch (Exception ex)
            {
                string message = string.Format("Initialisation failed : {0}", ex.StackTrace);

                Trace.Write(
                  new OperationalEvent(
                      SJPEventCategory.Business,
                      SJPTraceLevel.Error,
                      message));

                throw new SJPException(message, true, SJPExceptionIdentifier.LJSGenInitialisationFailed);
            }
        }

        #endregion
    }
}
