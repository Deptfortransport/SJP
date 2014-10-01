// *********************************************** 
// NAME             : IServiceFactory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Service Discovery IServiceFactory interface
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.ServiceDiscovery
{
    /// <summary>
    /// Interface for the service factory classes.
    /// </summary>
    public interface IServiceFactory
    {
        object Get();
    }
}
