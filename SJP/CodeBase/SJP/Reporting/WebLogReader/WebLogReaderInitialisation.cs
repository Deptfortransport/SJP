// *********************************************** 
// NAME             : WebLogReaderInitialisation.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Initialisation class for WebLogReader applications
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.EventPublishers;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Initialisation class for WebLogReader applications
    /// </summary>
    public class WebLogReaderInitialisation : IServiceInitialisation
    {
        #region Interface members

        /// <summary>
        /// Populates sevice cache with services needed by web log reader component.
        /// Also validates properties needed by web log reader services.
        /// </summary>
        /// <param name="serviceCache"></param>
        public void Populate(Dictionary<string, IServiceFactory> serviceCache)
        {
            string logPath = ConfigurationManager.AppSettings["DefaultLogFilepath"];
            string logFilePath = logPath + "\\" + ConfigurationManager.AppSettings["propertyservice.applicationid"] + ".txt";

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
                    throw new SJPException(String.Format(Messages.Init_DotNETTraceListenerFailed, exception.Message), true,
                        SJPExceptionIdentifier.RDPWebLogReaderDefaultLoggerFailed);
                }
                #endregion

                // Add services to service cache that are needed to support TD Trace Listening.
                try
                {
                    serviceCache.Add(ServiceDiscoveryKey.PropertyService, new PropertyServiceFactory());
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(String.Format(Messages.Init_ServiceAddFailed, exception.Message));
                    logTextListener.Flush();
                    throw new SJPException(String.Format(Messages.Init_ServiceAddFailed, exception.Message), true,
                        SJPExceptionIdentifier.RDPWebLogReaderTDServiceAddFailed);
                }

                #region Enable EventLogging
                // Enable Event Logging
                List<string> errors = new List<string>();
                try
                {
                    // Create custom publisher
                    IEventPublisher[] customPublishers = new IEventPublisher[2];

                    customPublishers[0] = new SJPCustomEventPublisher("SJPDB", SqlHelperDatabase.ReportStagingDB);
                    customPublishers[1] = new SJPOperationalEventPublisher("OPDB", SqlHelperDatabase.ReportStagingDB);

                    Trace.Listeners.Add(new SJPTraceListener(Properties.Current, customPublishers, errors));
                }
                catch (SJPException sjpEx)
                {
                    #region Log and throw errors
                    // Create message string
                    StringBuilder message = new StringBuilder(100);
                    message.Append(sjpEx.Message); // prepend with existing exception message

                    // Append all messages returned by TraceListener constructor
                    foreach (string error in errors)
                    {
                        message.Append(error);
                        message.Append(" ");
                    }

                    // Log message using .NET default trace listener
                    Trace.WriteLine(message.ToString() + "ExceptionID:" + sjpEx.Identifier.ToString());

                    // rethrow exception - use the initial exception id as the id
                    throw new SJPException(
                        string.Format(Messages.Init_TraceListenerFailed,
                            sjpEx.Identifier.ToString("D"), message.ToString()),
                        false, SJPExceptionIdentifier.RDPWebLogReaderTDTraceInitFailed);

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

            // Validate web log reader specific properties.
            List<string> propertyErrors = new List<string>();
            WebLogReaderPropertyValidator validator = new WebLogReaderPropertyValidator(Properties.Current);
            bool isWebLogFolders = validator.ValidateProperty(Keys.WebLogReaderWebLogFolders, propertyErrors);
            if (isWebLogFolders)
            {

                validator.ValidateProperty(Keys.WebLogReaderArchiveDirectory, propertyErrors);
                validator.ValidateProperty(Keys.WebLogReaderLogDirectory, propertyErrors);

            }

            validator.ValidateProperty(Keys.WebLogReaderNonPageMinimumBytes, propertyErrors);
            validator.ValidateProperty(Keys.WebLogReaderWebPageExtensions, propertyErrors);
            validator.ValidateProperty(Keys.WebLogReaderClientIPExcludes, propertyErrors);
            validator.ValidateProperty(Keys.WebLogReaderUserExperienceVisitorUserAgent, propertyErrors);

            if (propertyErrors.Count > 0)
            {
                StringBuilder message = new StringBuilder(100);
                foreach (string error in propertyErrors)
                    message.Append(error + ",");

                Trace.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, String.Format(Messages.Init_ReaderProperties, message.ToString())));

                throw new SJPException(String.Format(Messages.Init_ReaderProperties, message.ToString()), true, SJPExceptionIdentifier.RDPWebLogReaderInvalidProperties);
            }

        }

        #endregion
    }
}
