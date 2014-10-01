// *********************************************** 
// NAME             : TravelNewsHandlerFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: Factory that allows the ServiceDiscovery to create an instance of the TravelNewsHandler class.
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.TravelNews
{
    /// <summary>
    /// Factory used by Service Discovery to create a TravelNewsHandler.
    /// </summary>	
    public class TravelNewsHandlerFactory : IServiceFactory
    {
        #region Private members

        private ITravelNewsHandler current;
        private const string DataChangeNotificationGroup = "TravelNews";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TravelNewsHandlerFactory()
        {
            Update();
            RegisterForChangeNotification();
        }

        #endregion

        #region IServiceFactory members

        /// <summary>
        /// Method used by the ServiceDiscovery to get the instance of the TravelNewsHandler.
        /// </summary>
        /// <returns>The current instance of the TravelNewsHandler.</returns>
        public Object Get()
        {
            return current;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Registers an event handler with the data change notification service
        /// </summary>
        private bool RegisterForChangeNotification()
        {
            IDataChangeNotification notificationService;
            try
            {
                notificationService = SJPServiceDiscovery.Current.Get<IDataChangeNotification>(ServiceDiscoveryKey.DataChangeNotification);
            }
            catch (SJPException e)
            {
                // If the SDInvalidKey Exception is thrown, return false as the notification service
                // hasn't been initialised.
                // Otherwise, rethrow the exception that was received.
                if (e.Identifier == SJPExceptionIdentifier.SDInvalidKey)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Warning, "DataChangeNotificationService was not present when initialising TravelNews"));
                    return false;
                }
                else
                    throw;
            }
            catch
            {
                throw;
            }

            notificationService.Changed += new ChangedEventHandler(this.DataChangedNotificationReceived);
            return true;
        }

        /// <summary>
        /// Event handler for data change notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataChangedNotificationReceived(object sender, ChangedEventArgs e)
        {
            if (e.GroupId == DataChangeNotificationGroup)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                        "TravelNews - Reloading cache following event raised by data change notification service"));

                Update();
            }
        }

        /// <summary>
        /// Updates the travel news data
        /// </summary>
        private void Update()
        {
            ITravelNewsHandler newTravelNewsHandler = new TravelNewsHandler();
            current = newTravelNewsHandler;
        }

        #endregion
    }
}