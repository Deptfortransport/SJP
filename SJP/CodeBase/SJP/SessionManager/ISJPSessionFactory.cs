// *********************************************** 
// NAME             : ISessionFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: ISJPSessionFactory interface for the service discovery session factory
// ************************************************
// 

using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// Interface for SJPSessionFactory
    /// </summary>
    public interface ISJPSessionFactory : IServiceFactory
    {
        void Remove();
    }
}
