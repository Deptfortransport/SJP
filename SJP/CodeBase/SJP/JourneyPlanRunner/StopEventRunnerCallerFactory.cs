// *********************************************** 
// NAME             : StopEventRunnerCallerFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Apr 2011
// DESCRIPTION  	: Factory class for StopEventRunnerCaller class
// ************************************************


using System;
using System.Diagnostics;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.JourneyPlanRunner
{
    // <summary>
    /// Factory class for StopEventRunnerCaller class
    /// </summary>
    public class StopEventRunnerCallerFactory : IServiceFactory
    {
        #region Constructors
        /// <summary>
        /// Constructor 
        /// </summary>
        public StopEventRunnerCallerFactory()
        {
        }
        #endregion

        #region IServiceFactory Members


        /// <summary>
        ///  This enables StopEventRunnerCaller to 
        ///  be used with Service Discovery, and returns
        ///  a new instance of StopEventRunnerCaller
        /// </summary>
        /// <returns>A new instance of StopEventRunnerCaller</returns>
        public object Get()
        {
            return new StopEventRunnerCaller();
            
        }

        #endregion
    }
}
