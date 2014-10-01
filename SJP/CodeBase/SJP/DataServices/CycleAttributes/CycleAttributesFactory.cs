// *********************************************** 
// NAME             : CycleAttributesFactory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Factory class for the CycleAttributes
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.ServiceDiscovery;

namespace SJP.Common.DataServices.CycleAttributes
{
    /// <summary>
    /// CycleAttributesFactory class
    /// </summary>
    public class CycleAttributesFactory : IServiceFactory
    {
        private CycleAttributes current;

        #region Implementation of IServiceFactory
        /// <summary>
        /// Standard constructor. Initialises the CycleAttributes.
        /// </summary>
        public CycleAttributesFactory()
        {
            current = new CycleAttributes();
        }

        /// <summary>
        /// Returns the current CycleAttributes object
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            return current;
        }

        #endregion
    }
}
