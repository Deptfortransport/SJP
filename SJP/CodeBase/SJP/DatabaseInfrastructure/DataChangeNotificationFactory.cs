// *********************************************** 
// NAME             : DataChangeNotificationFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Mar 2011
// DESCRIPTION  	: DataChangeNotificationFactory class that allows the
// ServiceDiscovery to create an instance of the DataChangeNotification class.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.Common.DatabaseInfrastructure
{
    /// <summary>
    /// DataChangeNotificationFactory class that allows the
    /// ServiceDiscovery to create an instance of the DataChangeNotification class.
    /// </summary>
    public class DataChangeNotificationFactory : IServiceFactory, IDisposable
    {
        #region Private members
        
		private DataChangeNotification current;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
		public DataChangeNotificationFactory()
		{
			current = new DataChangeNotification();
		}

        #endregion

        #region IServiceFactory methods

        /// <summary>
        /// Method used by the ServiceDiscovery to get the
        /// instance of the DataChangeNotification.
        /// </summary>
        /// <returns>The current instance of the DataChangeNotification.</returns>
		public object Get()
		{
			return current;
        }

        #endregion

        #region IDisposable methods

        ~DataChangeNotificationFactory()
        {
            //calls a protected method 
            //the false tells this method
            //not to bother with managed
            //resources
            this.Dispose(false);
        }

        public void Dispose()
        {
            //calls the same method
            //passed true to tell it to
            //clean up managed and unmanaged 
            this.Dispose(true);

            //as dispose has been correctly
            //called we don't need the 
            //'backup' finaliser
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //check this hasn't been called already
            //remember that Dispose can be called again
            if (!disposed)
            {
                //this is passed true in the regular Dispose
                if (disposing)
                {
                    // Dispose managed resources here.

                    #region Dispose managed resources

                    if (current != null)
                    {
                        current.Dispose();
                    }

                    #endregion
                }

                //both regular Dispose and the finaliser
                //will hit this code
                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        #endregion
    }
}
