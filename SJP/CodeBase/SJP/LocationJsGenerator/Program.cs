// *********************************************** 
// NAME             : Program.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 04 Mar 2011
// DESCRIPTION  	: Entry point for LocationJSGenerator
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.LocationService;
using System.IO;
using SJP.Common.ServiceDiscovery;
using System.Diagnostics;
using SJP.Common.EventLogging;

namespace SJP.Common.LocationJsGenerator
{
    /// <summary>
    ///  Entry point for LocationJSGenerator
    /// </summary>
    class Program
    {
        /// <summary>
        ///  Entry point method for LocationJSGenerator
        /// </summary>
        /// <param name="args"></param>
        static int Main(string[] args)
        {
            int exitCode = -1;
            try
            {

                // Initialise service discovery
                SJPServiceDiscovery.Init(new Initialisation());

                // Initialise Javascript generator settings
                JSGeneratorSettings settings = new JSGeneratorSettings(args);

                JSGenerator generator = new JSGenerator();

                // Call to create Javascripts - SJPWeb
                generator.CreateScripts(JSGeneratorMode.SJPWeb);

                // Call to create Javascripts - SJPMobile
                generator.CreateScripts(JSGeneratorMode.SJPMobile);

                exitCode = 0;
            }
            catch (SJPException sjpex)
            {
                exitCode = (int)sjpex.Identifier;
            }
            catch (Exception ex)
            {
                string message = string.Format("Error generating location javascripts : {0}", ex.StackTrace);

                Trace.Write(
                  new OperationalEvent(
                      SJPEventCategory.Business,
                      SJPTraceLevel.Error,
                      message));

                exitCode = -1;
            }

            Console.WriteLine("Location Javascript Generator exited with code :{0}", exitCode);

            return exitCode;
 
        }

        
       
    }
}
