// *********************************************** 
// NAME             : CJPFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 30 Mar 2011
// DESCRIPTION  	: Factory class used be ServiceDiscovery to return an instance of the CJP
// ************************************************
// 
                
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Factory class used be ServiceDiscovery to return an instance of the CJP
    /// </summary>
    public class CJPFactory : IServiceFactory
    {
        #region Constructor

        /// <summary>
		/// Constructor.
		/// </summary>
		public CJPFactory()
		{
		}

        #endregion

        #region IServiceFactory Members

        /// <summary>
		///  Method used by the ServiceDiscovery to get an instance of CJP
		/// </summary>
		/// <returns>A new instance of a CJP.</returns>
		public Object Get()
		{
			return new TransportDirect.JourneyPlanning.CJP.CJP();
        }

        #endregion
    }
}
