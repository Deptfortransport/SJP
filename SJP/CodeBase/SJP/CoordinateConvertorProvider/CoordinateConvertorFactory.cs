// *********************************************** 
// NAME             : CoordinateConvertorFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Factory class for CoordinateConvertor
// ************************************************
//            

using SJP.Common.ServiceDiscovery;
using System;

namespace SJP.UserPortal.CoordinateConvertorProvider
{
    /// <summary>
    /// Factory class for CoordinateConvertor.
    /// </summary>
    public class CoordinateConvertorFactory : IServiceFactory, IDisposable
    {
        #region Private members

        private ICoordinateConvertor current;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CoordinateConvertorFactory()
        {
            current = new CoordinateConvertor();
        }

        #endregion

        #region IServiceFactory Members
        /// <summary>
        /// Method used by the ServiceDiscovery to get the instance of the CoordinateConvertor.
        /// </summary>
        /// <returns>The current instance of the CoordinateConvertor.</returns>
        public object Get()
        {
            return current;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~CoordinateConvertorFactory()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (current != null)
                {
                    current.Dispose();
                    current = null;
                }

            }
        }

        #endregion
    }
}
