// *********************************************** 
// NAME             : SJPWebInitialisation.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: SJPWebInitialisation class for initialising services needed for the SJPWeb project
// ************************************************
// 

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.DataServices;
using SJP.Common.DataServices.CycleAttributes;
using SJP.Common.DataServices.NPTG;
using SJP.Common.DataServices.StopAccessibilityLinks;
using SJP.Common.EventLogging;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.UserPortal.CoordinateConvertorProvider;
using SJP.UserPortal.CyclePlannerService;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.JourneyPlanRunner;
using SJP.UserPortal.Retail;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.SessionManager;
using SJP.UserPortal.TravelNews;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.ServiceDiscovery.Initialisation
{
    /// <summary>
    /// SJPWeb Initialisation class
    /// </summary>
    public class SJPWebInitialisation : IServiceInitialisation
    {        
        #region Interface members

        /// <summary>
        /// IServiceInitialisation Populate method
        /// </summary>
        /// <param name="serviceCache"></param>
        public void Populate(Dictionary<string, IServiceFactory> serviceCache)
        {
            // Enable PropertyService
            serviceCache.Add(ServiceDiscoveryKey.PropertyService, new PropertyServiceFactory());

            #region Enable EventLogging
            // Enable Event Logging
            List<string> errors = new List<string>();
            try
            {
                // Create custom email publisher
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

            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, "INITIALISATION Started SJPWebInitialisation"));

            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Added to service cache - Properties and Event Logging"));
                        
            // Add other services dependent on Properties and EventLogging

            // Session
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: SessionManager"));
            serviceCache.Add(ServiceDiscoveryKey.SessionManager, new SJPSessionFactory());

            // DataChangeNotification
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: DataChangeNotification"));
            serviceCache.Add(ServiceDiscoveryKey.DataChangeNotification, new DataChangeNotificationFactory());

            // LocationService
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: LocationService"));
            serviceCache.Add(ServiceDiscoveryKey.LocationService, new LocationServiceFactory());

            // PageController
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: PageController"));
            serviceCache.Add(ServiceDiscoveryKey.PageController, new PageControllerFactory());

            // CJP
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: CJP"));
            serviceCache.Add(ServiceDiscoveryKey.CJP, new CJPFactory());

            // CJPManager
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: CJPManger"));
            serviceCache.Add(ServiceDiscoveryKey.CJPManager, new CjpManagerFactory());

            // CTP
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: CTP"));
            serviceCache.Add(ServiceDiscoveryKey.CTP, new CyclePlannerFactory ());
                        
            // CyclePlannerManager
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: CyclePlannerManager"));
            serviceCache.Add(ServiceDiscoveryKey.CyclePlannerManager, new CyclePlannerManagerFactory());

            // StopEventManager
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: StopEventManager"));
            serviceCache.Add(ServiceDiscoveryKey.StopEventManager, new StopEventManagerFactory());

            // CoordinateConvertor
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: CoordinateConvertor"));
            serviceCache.Add(ServiceDiscoveryKey.CoordinateConvertor, new CoordinateConvertorFactory());

            // JourneyPlanRunnerCaller
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: JourneyPlanRunnerCaller"));
            serviceCache.Add(ServiceDiscoveryKey.JourneyPlanRunnerCaller, new JourneyPlanRunnerCallerFactory());

            // CycleJourneyPlanRunnerCaller
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: CycleJourneyPlanRunnerCaller"));
            serviceCache.Add(ServiceDiscoveryKey.CycleJourneyPlanRunnerCaller, new CycleJourneyPlanRunnerCallerFactory());

            // StopEventRunnerCaller
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: StopEventRunnerCaller"));
            serviceCache.Add(ServiceDiscoveryKey.StopEventRunnerCaller, new StopEventRunnerCallerFactory());

            // RetailerCatalogue
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: RetailerCatalogue"));
            serviceCache.Add(ServiceDiscoveryKey.RetailerCatalogue, new RetailerCatalogueFactory());

            // RetailHandoffSchema
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: RetailHandoffSchema"));
            serviceCache.Add(ServiceDiscoveryKey.RetailerHandoffSchema, new RetailHandoffSchemaFactory());

            // TravelcardCatalogue
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: TravelcardCatalogue"));
            serviceCache.Add(ServiceDiscoveryKey.TravelcardCatalogue, new TravelcardCatalogueFactory());

            // DataServices
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: DataServices"));
            serviceCache.Add(ServiceDiscoveryKey.DataServices, new DataServicesFactory());

            // CycleAttributes
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: CycleAttributes"));
            serviceCache.Add(ServiceDiscoveryKey.CycleAttributes, new CycleAttributesFactory());

            // StopAccessibilityLinks
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: StopAccessibilityLinks"));
            serviceCache.Add(ServiceDiscoveryKey.StopAccessibilityLinks, new StopAccessibilityLinksFactory());

            // TravelNews
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: TravelNewsHandler"));
            serviceCache.Add(ServiceDiscoveryKey.TravelNews, new TravelNewsHandlerFactory());

            // NPTGData
            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "INITIALISATION Adding to service cache: NPTGData"));
            serviceCache.Add(ServiceDiscoveryKey.NPTGData, new NPTGDataFactory());
            

            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, "INITIALISATION Completed SJPWebInitialisation"));
        }

        #endregion
    }
}
