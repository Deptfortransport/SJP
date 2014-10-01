// *********************************************** 
// NAME             : NPTGDataFactory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 Jun 2011
// DESCRIPTION  	: Factory class for the NPTGData
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.Common.DataServices.NPTG
{
    /// <summary>
    /// NPTGDataFactory class
    /// </summary>
    public class NPTGDataFactory : IServiceFactory
    {
        private NPTGData current;

        #region Implementation of IServiceFactory
        /// <summary>
        /// Standard constructor. Initialises the NPTGData.
        /// </summary>
        public NPTGDataFactory()
        {
            current = new NPTGData();
        }

        /// <summary>
        /// Returns the current NPTGData object
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            return current;
        }

        #endregion
    }
}
