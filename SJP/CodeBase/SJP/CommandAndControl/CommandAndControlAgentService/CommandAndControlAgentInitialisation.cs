// *********************************************** 
// NAME             : CommandAndControlAgentInitialisation.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Implementation of initialisation logic
// ************************************************
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SJP.Common;
using SJP.Common.ServiceDiscovery;
using SJP.Common.PropertyManager;
using System.Diagnostics;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;
using SJP.Common.DatabaseInfrastructure;

namespace SJP.UserPortal.CCAgent
{
    public class CommandAndControlAgentInitialisation : IServiceInitialisation
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

                throw new SJPException(message, true, SJPExceptionIdentifier.CCAgentInitialisationFailed);
            }


            #region Enable EventLogging
            // Enable Event Logging
            List<string> errors = new List<string>();
            try
            {

                IEventPublisher[] customPublishers = new IEventPublisher[0];
                // Create and add SJPTraceListener instance to the listener collection	
                Trace.Listeners.Add(new SJPTraceListener(Properties.Current, customPublishers, errors));
            }
            catch (SJPException sjpEx)
            {
                #region Log and throw errors
                // Create message string
                StringBuilder message = new StringBuilder(100);
                message.Append(sjpEx.Message); // prepend with existing exception message

                // Append all messages returned by TDTraceListener constructor
                foreach (string error in errors)
                {
                    message.Append(error);
                    message.Append(" ");
                }

                // Log message using .NET default trace listener
                Trace.WriteLine(message.ToString() + "ExceptionID:" + sjpEx.Identifier.ToString());

                // rethrow exception - use the initial exception id as the id
                throw new SJPException(message.ToString(), sjpEx, false, sjpEx.Identifier);

                #endregion
            }
            #endregion

            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, "INITIALISATION Started CCAgentInitialisation"));
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Added to service cache - Properties and Event Logging"));
            // Add other services dependent on Properties and EventLogging
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: DataChangeNotification"));
            serviceCache.Add(ServiceDiscoveryKey.DataChangeNotification, new DataChangeNotificationFactory());
        }
        #endregion
    
    }
}
