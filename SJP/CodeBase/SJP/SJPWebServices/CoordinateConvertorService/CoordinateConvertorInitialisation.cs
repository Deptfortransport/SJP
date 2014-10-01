// *********************************************** 
// NAME             : CoordinateConvertorInitialisation.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: CoordinateConvertorInitialisation class for CoordinateConvertor web service
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;

namespace SJP.WebService.CoordinateConvertorService
{
    /// <summary>
    /// CoordinateConvertorInitialisation class for CoordinateConvertor web service
    /// </summary>
    public class CoordinateConvertorInitialisation : IServiceInitialisation
    {
        /// <summary>
        /// Sets up services.
        /// </summary>
        /// <param name="serviceCache">Service cache to add services to.</param>
        /// <exception cref="SJPException">
        /// One or more services fail to initialise.
        /// </exception>
        public void Populate(Dictionary<string, IServiceFactory> serviceCache)
        {
            string logPath = ConfigurationManager.AppSettings["DefaultLogFilepath"];
            string logFilePath = logPath + "\\" + ConfigurationManager.AppSettings["propertyservice.applicationid"] + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";

            using (Stream logFile = File.Create(logFilePath))
            {

                #region Initialise .NET trace listener
                // Initialise .NET trace listener to record ONLY errors until SJP TraceListener is intialised. 
                TextWriterTraceListener logTextListener = null;
                try
                {

                    logTextListener = new TextWriterTraceListener(logFile);
                    Trace.Listeners.Add(logTextListener);
                    Trace.WriteLine(Messages.Init_InitialisationStarted);

                }
                catch (Exception exception) // catch all in this situation.
                {
                    throw new SJPException(String.Format(Messages.Init_DotNETTraceListenerFailed, exception.Message), false, SJPExceptionIdentifier.CCServiceTraceInitFailed);
                }
                #endregion

                // Add services to service cache that are needed to support Trace Listening.
                try
                {
                    serviceCache.Add(ServiceDiscoveryKey.PropertyService, new PropertyServiceFactory());
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(String.Format(Messages.Init_ServiceAddFailed, exception.Message));
                    logTextListener.Flush();

                    throw new SJPException(String.Format(Messages.Init_ServiceAddFailed, exception.Message), true, SJPExceptionIdentifier.CCServiceServiceAddFailed);
                }

                #region Enable EventLogging
                // Enable Event Logging
                List<string> errors = new List<string>();

                try
                {
                    IEventPublisher[] customPublishers = new IEventPublisher[0];

                    // Create and add SJPTraceListener instance to the listener collection	
                    Trace.Listeners.Add(new SJPTraceListener(Properties.Current, customPublishers, errors));
                }
                catch (SJPException)
                {
                    #region Log and throw errors

                    // Create error message to log to default listener.
                    Trace.WriteLine(String.Format(Messages.Init_TraceListenerFailed, "Reasons follow."));
                    StringBuilder message = new StringBuilder(100);
                    foreach (string error in errors)
                        message.Append(error + ",");
                    Trace.WriteLine(message.ToString());

                    throw new SJPException(String.Format(Messages.Init_TraceListenerFailed, message.ToString()), true, SJPExceptionIdentifier.CCServiceTraceInitFailed);

                    #endregion
                }
                finally
                {
                    // Remove other listeners.
                    logTextListener.Flush();
                    Trace.Listeners.Remove(logTextListener);
                    logTextListener = null;
                    Trace.Listeners.Remove("System.Diagnostics.DefaultTraceListener");
                }
                #endregion
            }

            if (SJPTraceSwitch.TraceVerbose)
            {
                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, Messages.Init_Completed));
            }
        }
    }
}