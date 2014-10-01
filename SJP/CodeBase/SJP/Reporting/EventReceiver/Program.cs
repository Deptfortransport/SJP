// *********************************************** 
// NAME             : Program.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Program class for main entry point for the application
// ************************************************
// 

using System.ServiceProcess;

namespace SJP.Reporting.EventReceiver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new EventReceiverService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
