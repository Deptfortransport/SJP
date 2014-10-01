using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.VenueIncidents
{
    class Messages
    {
        public const string Init_DotNETTraceListenerFailed = "Failed to initialise a default .NET trace listener. Reason:[{0}].";
        public const string Init_InitialisationStarted = "Initialisation of DataLoader started.";
        public const string Init_ServiceAddFailed = "Failed to add a service to the cache: [{0}].";
        public const string Init_TraceListenerFailed = "Failed to initialise the Trace Listener class: {0} Message: {1}";
        public const string Loader_Failed = "DataLoader failed. Reason: {0} Id: {1}";

        public const string Init_Usage = "Usage: sjp.venueincidents [/help]" +
            "\n[/help]       - Show usage help" +
            "\n\nCreates an XML document which indicates if travel news is available for a particular venue.";


    }
}
