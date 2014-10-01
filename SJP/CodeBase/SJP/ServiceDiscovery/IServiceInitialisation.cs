// *********************************************** 
// NAME             : IServiceInitialisation.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Interface for the Service Initialisation classes
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.ServiceDiscovery
{
    /// <summary>
    /// Interface for the Service Initialisation classes
    /// </summary>
    public interface IServiceInitialisation
    {
        void Populate(Dictionary<string, IServiceFactory> serviceCache);
    }
}
