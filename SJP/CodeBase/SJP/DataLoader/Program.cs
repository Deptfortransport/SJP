// *********************************************** 
// NAME             : Program.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: Entry point for the Data Loader console app
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logger = System.Diagnostics.Trace;
using SJP.Common.EventLogging;
using SJP.Common;
using SJP.Common.ServiceDiscovery;

namespace SJP.DataLoader
{
    /// <summary>
    /// Entry point for the Data Loader console app
    /// </summary>
    class Program
    {
        /// <summary>
        /// /// <summary>
        /// The main entry point for the application
        /// </summary>
        /// <returns>
        /// Exit Code: 
        /// Zero signals success.
        /// Greater than zero signals failure.
        /// </returns>
        /// </summary>
        /// <param name="args"></param>
        public static int Main(string[] args)
        {
            int returnCode = 0;

            // Expect dataName and (optional) noTransfer args
            string dataName = string.Empty;
            bool dataTransfer = true;
            bool dataLoad = true;
            bool isHelp = false;
            bool isTest = false;

            Console.WriteLine();

            try
            {
                // Read arguments supplied
                if (args.Length > 0)
                {
                    foreach (string arg in args)
                    {
                        switch (arg.ToLower().Trim())
                        {
                            case "/help":
                            case "/?":
                                isHelp = true;
                                break;
                            case "/test":
                                isTest = true;
                                break;
                            case "/notransfer":
                                dataTransfer = false;
                                break;
                            case "/noload":
                                dataLoad = false;
                                break;
                            default:
                                if (!string.IsNullOrEmpty(dataName))
                                {
                                    throw new SJPException(
                                        string.Format("Unexpected argument supplied [{0}], only one dataName can be supplied", arg), 
                                        false, 
                                        SJPExceptionIdentifier.DLDataLoaderInvalidArgument);
                                }
                                dataName = arg;
                                break;
                        }
                    }
                }

                // Run with validated arguments
                returnCode = Run(dataName, dataTransfer, dataLoad, isHelp, isTest);
            }
            catch (SJPException sjpEx)
            {
                // Log error (cannot assume that SJP listener has been initialised)
                if (!sjpEx.Logged)
                {
                    Logger.Write(string.Format(Messages.Loader_Failed, sjpEx.Message, sjpEx.Identifier));
                }
                
                Console.WriteLine(string.Format(Messages.Loader_Failed, sjpEx.Message, sjpEx.Identifier));

                returnCode = (int)sjpEx.Identifier;
            }
            catch (Exception ex)
            {
                // Any unhandled exceptions
                Logger.Write(string.Format(Messages.Loader_UnhandledError, ex.Message, ex.StackTrace));
                Console.WriteLine(string.Format(Messages.Loader_UnhandledError, ex.Message, ex.StackTrace));

                returnCode = (int)SJPExceptionIdentifier.DLDataLoaderUnexpectedException;
            }

            if (!isHelp)
            {
                string returnCodeText = (returnCode != 0) ? ((SJPExceptionIdentifier)returnCode).ToString() : string.Empty;
                
                Console.WriteLine();
                Console.WriteLine(string.Format(Messages.Loader_Exit, returnCode, returnCodeText));
            }

            return returnCode;
        }

        /// <summary>
        /// Runs the data loader
        /// </summary>
        /// <returns>Return code passed to client.</returns>
        private static int Run(string dataName, bool dataTransfer, bool dataLoad, bool isHelp, bool isTest)
        {
            // Assume success
            int returnCode = 0;

            try
            {
                // Show help message
                if (isHelp)
                {
                    Console.WriteLine(Messages.Init_Usage);
                }
                // Start test
                else if (isTest)
                {
                    Console.WriteLine(Messages.Loader_Starting);

                    SJPServiceDiscovery.Init(new DataLoaderInitialisation());

                    if (SJPTraceSwitch.TraceInfo)
                        Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, Messages.Loader_TestSucceeded));

                    Console.WriteLine();
                    Console.WriteLine(Messages.Loader_TestSucceeded);
                }
                // Data name supplied, load data
                else if (!string.IsNullOrEmpty(dataName))
                {
                    Console.WriteLine(Messages.Loader_Starting);

                    SJPServiceDiscovery.Init(new DataLoaderInitialisation());

                    if (SJPTraceSwitch.TraceInfo)
                        Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, Messages.Loader_Started));

                    // Use the controller to perform the load
                    DataLoaderController controller = new DataLoaderController();
                    returnCode = controller.Load(dataName, dataTransfer, dataLoad);

                    if ((SJPTraceSwitch.TraceInfo) && (returnCode == 0))
                        Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, Messages.Loader_Completed));
                }
                // No Data name supplied, exit
                else
                {
                    Console.WriteLine(Messages.Loader_NoArg);
                }
            }
            catch (SJPException sjpEx)
            {
                if (!sjpEx.Logged)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                        string.Format(Messages.Loader_Failed, sjpEx.Message, sjpEx.Identifier)));
                }

                returnCode = (int)sjpEx.Identifier;
            }
            
            return returnCode;
        }
    }
}