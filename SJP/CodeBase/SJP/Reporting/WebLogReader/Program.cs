// *********************************************** 
// NAME             : Program.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Entry point for the Web Log Reader Console app
// ************************************************
// 

using System;
using System.Diagnostics;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Entry point for the Web Log Reader Console app
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

            try
            {
                if (args.Length > 0)
                {
                    if (String.Compare(args[0], "/help", true) == 0)
                    {
                        Console.WriteLine(Messages.Init_Usage);
                        returnCode = 0;
                    }
                    else if (String.Compare(args[0], "/test", true) == 0)
                    {
                        SJPServiceDiscovery.Init(new WebLogReaderInitialisation());

                        returnCode = 0;

                        if (SJPTraceSwitch.TraceInfo)
                            Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, Messages.Reader_TestSucceeded));

                        Console.WriteLine(Messages.Reader_TestSucceeded);

                    }
                    else
                    {
                        Console.WriteLine(Messages.Reader_InvalidArg);
                        returnCode = (int)SJPExceptionIdentifier.RDPWebLogReaderInvalidArg;
                    }
                }
                else
                {
                    SJPServiceDiscovery.Init(new WebLogReaderInitialisation());

                    if (SJPTraceSwitch.TraceInfo)
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, Messages.Reader_Started));

                    returnCode = RunReader();

                    if ((SJPTraceSwitch.TraceInfo) && (returnCode == 0))
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, Messages.Reader_Completed));
                }
            }
            catch (SJPException sjpEx)
            {
                // Log error (cannot assume that TD listener has been initialised)
                if (!sjpEx.Logged)
                {
                    Console.Write(String.Format(Messages.Reader_Failed, sjpEx.Message, sjpEx.Identifier));
                    Trace.Write(String.Format(Messages.Reader_Failed, sjpEx.Message, sjpEx.Identifier));
                }

                returnCode = (int)sjpEx.Identifier;
            }

            return returnCode;
        }

        /// <summary>
        /// Runs the web log reader.
        /// </summary>
        /// <returns>Return code passed to client.</returns>
        private static int RunReader()
        {
            int returnCode = 0;

            try
            {
                WebLogReaderController controller = new WebLogReaderController(Properties.Current);
                returnCode = controller.Run();
            }
            catch (SJPException sjpEx)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Reader_Failed, sjpEx.Message, sjpEx.Identifier)));
                returnCode = (int)sjpEx.Identifier;
            }

            return returnCode;
        }
    }
}
