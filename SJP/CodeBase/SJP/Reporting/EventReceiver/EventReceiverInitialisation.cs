// *********************************************** 
// NAME             : EventReceiverInitialisation.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: EventReceiver Initialisation class
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.EventPublishers;

namespace SJP.Reporting.EventReceiver
{
    /// <summary>
    /// EventReceiver Initialisation class
    /// </summary>
    public class EventReceiverInitialisation : IServiceInitialisation
    {
        #region Interface members

        /// <summary>
        /// IServiceInitialisation Populate method
        /// </summary>
        /// <param name="serviceCache"></param>
        public void Populate(Dictionary<string, IServiceFactory> serviceCache)
        {
            // Enable PropertyService
            try
            {
                serviceCache.Add(ServiceDiscoveryKey.PropertyService, new PropertyServiceFactory());
            }
            catch (Exception exception)
            {
                throw new SJPException(String.Format(Messages.Init_PropertyServiceFailed, exception.Message), false, SJPExceptionIdentifier.RDPEventReceiverInitFailed);
            }

            #region Enable EventLogging
            // Enable Event Logging
            List<string> errors = new List<string>();
            try
            {
                // Create custom publisher
                IEventPublisher[] customPublishers = new IEventPublisher[3];
                
                customPublishers[0] = new SJPCustomEventPublisher("SJPDB", SqlHelperDatabase.ReportStagingDB);
                customPublishers[1] = new SJPOperationalEventPublisher("OPDB", SqlHelperDatabase.ReportStagingDB);
                customPublishers[2] = new CJPCustomEventPublisher("CJPDB", SqlHelperDatabase.ReportStagingDB);
                
                // Create and add SJPTraceListener instance to the listener collection	
                Trace.Listeners.Add(new SJPTraceListener(Properties.Current, customPublishers, errors));
                Trace.Listeners.Remove("System.Diagnostics.DefaultTraceListener");
            }
            catch (SJPException sjpEx)
            {
                #region Log and throw errors
                // Create message string
                StringBuilder message = new StringBuilder(100);
                message.Append(sjpEx.Message); // prepend with existing exception message

                // Append all messages returned by TDTraceListener constructor
                foreach (string error in errors)
                {
                    message.Append(error);
                    message.Append(" ");
                }

                // Log message using .NET default trace listener
                Trace.WriteLine(message.ToString() + "ExceptionID:" + sjpEx.Identifier.ToString());

                // rethrow exception - use the initial exception id as the id
                throw new SJPException(string.Format(Messages.Init_TraceListenerFailed, sjpEx.Identifier.ToString("D"), message.ToString()), false, SJPExceptionIdentifier.RDPEventReceiverInitFailed);	

                #endregion
            }
            #endregion

            // Validate Properties which are required by services.
            List<string> propertyErrors = new List<string>();
            EventReceiverPropertyValidator validator = new EventReceiverPropertyValidator(Properties.Current);
            validator.ValidateProperty(Keys.ReceiverQueue, propertyErrors);
            if (propertyErrors.Count != 0)
            {
                StringBuilder message = new StringBuilder(100);
                foreach (string error in propertyErrors)
                    message.Append(error + ",");

                throw new SJPException(String.Format(Messages.Init_InvalidPropertyKeys, message.ToString()), true, SJPExceptionIdentifier.RDPEventReceiverInitFailed);
            }
        }

        #endregion
    }
}
