// *********************************************** 
// NAME             : CommandAndControlAgentInitialisation.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Main entry point for starting the Command&ControlAgent Service
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SJP.UserPortal.CCAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application. Start an instance of CommandAndControlAgentService
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new CommandAndControlAgentService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
