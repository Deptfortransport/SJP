// *********************************************** 
// NAME             : UndergroundNewsHandler.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 05 Mar 2012
// DESCRIPTION  	: Factory that allows the ServiceDiscovery to create an instance of the UndergroundNewsHandler class
// ************************************************
// 

using System;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.UndergroundNews
{
    /// <summary>
    /// Factory that allows the ServiceDiscovery to create an instance of the UndergroundNewsHandler class
    /// </summary>
    public class UndergroundNewsHandlerFactory : IServiceFactory
    {
        #region Private members

        private IUndergroundNewsHandler current;
        private const string DataChangeNotificationGroup = "UndergroundNews";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UndergroundNewsHandlerFactory()
        {
            Update();
            RegisterForChangeNotification();
        }

        #endregion

        #region IServiceFactory members

        /// <summary>
        /// Method used by the ServiceDiscovery to get the instance of the UndergroundNewsHandler
        /// </summary>
        /// <returns>The current instance of the UndergroundNewsHandler</returns>
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
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Warning, "DataChangeNotificationService was not present when initialising UndergroundNews"));
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
                        "UndergroundNews - Reloading cache following event raised by data change notification service"));

                Update();
            }
        }

        /// <summary>
        /// Updates the underground news data
        /// </summary>
        private void Update()
        {
            IUndergroundNewsHandler newHandler = new UndergroundNewsHandler();
            current = newHandler;
        }

        #endregion
    }
}