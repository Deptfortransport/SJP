// *********************************************** 
// NAME             : EventReceiverService.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: EventReceiverService windows service for consuming SJPCustomEvents published to a message queue
// ************************************************
// 

using System;
using System.Diagnostics;
using System.ServiceProcess;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;

namespace SJP.Reporting.EventReceiver
{
    /// <summary>
    /// EventReceiverService windows service for consuming SJPCustomEvents published to a message queue
    /// </summary>
    public partial class EventReceiverService : ServiceBase
    {
        #region Private members
        
        private const string TestParameter = "/test";

        private EventReceiver eventReceiver;
        private bool testMode;
        private bool servicesInitialised;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public EventReceiverService()
        {
            InitializeComponent();

            // Set up service name - this must match the AID used in the properties, and in ProjectInstaller.cs.
            this.ServiceName = "EventReceiver";

            eventReceiver = null;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Called when service is started.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            testMode = false;
            servicesInitialised = false;
            eventReceiver = null;

            // Initialise Services
            try
            {
                SJPServiceDiscovery.Init(new EventReceiverInitialisation());
                servicesInitialised = true;
                AutoLog = false; // Disable default logging (will be using SJP Logging Service).
            }
            catch (SJPException sjpEx)
            {
                // Log error to default trace listener in case TD Listener failed to create.
                EventLog.WriteEntry(String.Format(Messages.Init_Failed, sjpEx.Message), EventLogEntryType.Error, 1);
                AutoLog = false; // Prevents .NET runtime from making entry to say service has started successfully.
            }

            if (servicesInitialised)
            {
                if (SJPTraceSwitch.TraceVerbose)
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, Messages.Init_Completed));
            }

            if ((servicesInitialised) && (args.Length == 1))
            {

                if (args[0].Equals(TestParameter))
                {
                    testMode = true;

                    if (SJPTraceSwitch.TraceWarning)
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, Messages.Service_StartedTestMode));
                }
                else
                {
                    if (SJPTraceSwitch.TraceWarning)
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, Messages.Service_BadParam));
                }
            }


            if (servicesInitialised)
            {
                if (testMode)
                {
                    // do nothing...
                }
                else
                {
                    // Start receiving events...
                    try
                    {
                        eventReceiver = new EventReceiver();
                        eventReceiver.Run();
                    }
                    catch (SJPException sjpEx)
                    {
                        Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Service_FailedRunning, sjpEx.Message)));
                    }
                }
            }
        }

        /// <summary>
        /// Called when service is stopped
        /// </summary>
        protected override void OnStop()
        {
            if (servicesInitialised)
            {
                if (SJPTraceSwitch.TraceVerbose)
                    Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, 
                        Messages.Service_Stopping));
            }

            if (eventReceiver != null)
            {
                eventReceiver.Dispose();
                eventReceiver = null;
            }
        }

        #endregion
    }
}
