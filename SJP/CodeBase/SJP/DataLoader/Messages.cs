// *********************************************** 
// NAME             : Messages.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: Messages class used by the DataLoader
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.DataLoader
{
    /// <summary>
    /// Messages class used by the DataLoader
    /// </summary>
    public class Messages
    {
        // Initialisation Messages
        public const string Init_DotNETTraceListenerFailed = "Failed to initialise a default .NET trace listener. Reason:[{0}].";
        public const string Init_InitialisationStarted = "Initialisation of DataLoader started.";
        public const string Init_Completed = "Initialisation of DataLoader completed successfully.";
        public const string Init_ServiceAddFailed = "Failed to add a service to the cache: [{0}].";
        public const string Init_TraceListenerFailed = "Failed to initialise the Trace Listener class: {0} Message: {1}";
        public const string Init_Usage = "Usage: sjp.dataLoader [dataname] [/notransfer] [/noload] [/test] [/help]" +
            "\n[dataname]    - Name of the data (case sensitive), as defined in properties" +
            "\n[/notransfer] - Do not transfer the data file to the processing directory" + 
            "\n[/noload]     - Do not load the data file" + 
            "\n[/test]       - Run program startup test" +
            "\n[/help]       - Show usage help";

        // Loader Messages
        public const string Loader_UnhandledError = "DataLoader failed with unhandled exception. Message: {0} \n Exception: {1}";
        public const string Loader_Failed = "DataLoader failed. Reason: {0} Id: {1}";
        public const string Loader_TestSucceeded = "DataLoader was run in test mode and succeeded";
        public const string Loader_InvalidArg = "Invalid arguments passed to DataLoader";
        public const string Loader_NoArg = "No arguments passed to DataLoader";
        public const string Loader_Starting = "DataLoader starting";
        public const string Loader_Started = "DataLoader initialised successfully";
        public const string Loader_Completed = "DataLoader completed successfully";
        public const string Loader_Exit = "DataLoader exiting with statuscode {0} {1}";
    }
}
