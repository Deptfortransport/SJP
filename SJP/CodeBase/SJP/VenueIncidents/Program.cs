using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common;
using Logger = System.Diagnostics.Trace;
using SJP.Common.ServiceDiscovery;

namespace SJP.VenueIncidents
{
    class Program
    {
        public static int Main(string[] args)
        {
            int returnCode = 0;

            try
            {
                SJPServiceDiscovery.Init(new VenueIncidentsInitialisation());
                VenueIncidents target = new VenueIncidents();
                target.GenerateTravelNewsAlertFile();
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

            return returnCode;
        }
    }
}
