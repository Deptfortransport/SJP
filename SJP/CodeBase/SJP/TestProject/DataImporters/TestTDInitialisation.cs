using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportDirect.Common.ServiceDiscovery;
using System.Collections;
using TransportDirect.Common.PropertyService.Properties;
using TransportDirect.Common.Logging;
using System.Diagnostics;
using TransportDirect.Common;

namespace SJP.TestProject.DataImporters
{
    class TestTDInitialisation : IServiceInitialisation
    {
        /// <summary>
        /// Populates sevice cache with services needed 
        /// </summary>
        /// <param name="serviceCache">Cache to populate.</param>
        public void Populate(Hashtable serviceCache)
        {
            // Add cryptographic scheme
            serviceCache.Add(ServiceDiscoveryKey.Crypto, new CryptoFactory());

            // Enable PropertyService					
            serviceCache.Add(ServiceDiscoveryKey.PropertyService, new PropertyServiceFactory());

           
            // Enable logging service.
            ArrayList errors = new ArrayList();
            try
            {
                IEventPublisher[] customPublishers = new IEventPublisher[0];

                Trace.Listeners.Add(new TDTraceListener(Properties.Current, customPublishers, errors));
            }
            catch (TDException)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                throw;
            }

        }
    }
}
