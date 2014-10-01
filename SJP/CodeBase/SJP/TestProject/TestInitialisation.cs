// *********************************************** 
// NAME             : TestInitialisation.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: Initialisation for Test projects
// ************************************************
// 

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.EventPublishers;
using SJP.UserPortal.CoordinateConvertorProvider;
using SJP.UserPortal.CyclePlannerService;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;
using SJP.UserPortal.SessionManager;

using Logger = System.Diagnostics.Trace;

namespace SJP.TestProject
{
    class TestInitialisation : IServiceInitialisation
    {
        public void Populate(Dictionary<string, IServiceFactory> serviceCache)
        {
            serviceCache.Add(ServiceDiscoveryKey.PropertyService, new PropertyServiceFactory());

            #region Enable EventLogging
            // Enable Event Logging
            List<string> errors = new List<string>();
            try
            {
                // No custom publishers
                IEventPublisher[] customPublishers = new IEventPublisher[2];

                // Create custom database publishers which will be used to publish 
                // custom events Note: id passed in constructors
                // must match those defined in the properties.
                customPublishers[0] = new SJPCustomEventPublisher("SJPDB", SqlHelperDatabase.ReportStagingDB);
                customPublishers[1] = new SJPOperationalEventPublisher("OPDB", SqlHelperDatabase.ReportStagingDB);

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

            serviceCache.Add(ServiceDiscoveryKey.DataChangeNotification, new DataChangeNotificationFactory());

            serviceCache.Add(ServiceDiscoveryKey.LocationService, new LocationServiceFactory());

            serviceCache.Add(ServiceDiscoveryKey.CJP, new CJPFactory());

            serviceCache.Add(ServiceDiscoveryKey.CJPManager, new CjpManagerFactory());

            serviceCache.Add(ServiceDiscoveryKey.CTP, new CyclePlannerFactory());

            serviceCache.Add(ServiceDiscoveryKey.CyclePlannerManager, new CyclePlannerManagerFactory());

            serviceCache.Add(ServiceDiscoveryKey.StopEventManager, new StopEventManagerFactory());

            serviceCache.Add(ServiceDiscoveryKey.CoordinateConvertor, new CoordinateConvertorFactory());

            serviceCache.Add(ServiceDiscoveryKey.RetailerHandoffSchema, new RetailHandoffSchemaFactory());

            serviceCache.Add(ServiceDiscoveryKey.SessionManager, new SJPSessionFactory());

            try
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "TestInitialisation completed"));
            }
            catch
            {
                // Ignore
            }
        }
    }
}
