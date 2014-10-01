// *********************************************** 
// NAME             : SJPTravelNewsDataImporter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Feb 2012
// DESCRIPTION  	: DataGateway import task to import SJP Travel News data
// ************************************************
// 

using System;
using System.Diagnostics;
using System.Globalization;
using TransportDirect.Common.Logging;
using TransportDirect.Common.PropertyService.Properties;
using TransportDirect.Datagateway.Framework;
using TransportDirect.Common;
using System.Security.Permissions;

namespace SJP.DataImporters
{
    public class SJPTravelNewsDataImporter: DatasourceImportTask
    {
        #region Private Fields

        private const string propertyKey = "datagateway.sqlimport.SJPTravelNewsData.Name";
        private const string propertyFeedName = "datagateway.sqlimport.SJPTravelNewsData.feedname";
		private string importPropertyName = string.Empty;				
		private const string logDescription = "SJP Travel News Data Import" ;
		
        #endregion

		#region Constructor

		/// <summary>
		/// Default contructor 
		/// </summary>
		/// <param name="feed">Import feedname</param>
		/// <param name="params1">Addition information1</param>
		/// <param name="params2">Addition information1</param>
		/// <param name="utility">Utility to run</param>
		/// <param name="processingDirectory">Directory location of Gateway processing directory</param>
        public SJPTravelNewsDataImporter
			(string feed, string params1, string params2, string utility, string processingDirectory) 
            : base(feed, params1, params2, utility, processingDirectory)
		{
            importPropertyName = Properties.Current[propertyKey]; 				    
			xmlschemaLocationKey = string.Format(CultureInfo.InvariantCulture, xmlschemaLocationKey, importPropertyName);
			xmlNamespaceKey = string.Format(CultureInfo.InvariantCulture, xmlNamespaceKey, importPropertyName);
			databaseKey = string.Format(CultureInfo.InvariantCulture, databaseKey, importPropertyName);
			storedProcedureKey = string.Format(CultureInfo.InvariantCulture, storedProcedureKey, importPropertyName);		

			// Check the feed name
			if ( !dataFeed.Equals(Properties.Current[propertyFeedName] ) ) 
			{
				Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error, logDescription + " unexpected feed name: [" + dataFeed + "]"));
			}
		}

		#endregion

		#region Overridden protected methods

		/// <summary>
		/// Override of LogStart method which logs to the log file before data import.
		/// </summary>
		protected override void LogStart()
		{	
			string logDesc = String.Format(CultureInfo.InvariantCulture, "{0} update begun for feed {1}", logDescription, dataFeed); 
			Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info, logDesc));
		}

		/// <summary>
		/// Override of LogFinish method which logs to the log file after data import.
		/// </summary>
		protected override void LogFinish(int retCode)
		{	
			string logDesc = string.Empty;
			if (retCode != 0) 
			{
				// Log failure							 				
				logDesc = String.Format(CultureInfo.InvariantCulture, "{0} {1} update failed for feed : {2} ", logDescription, retCode.ToString(),  dataFeed.ToString()); 
				Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error, logDesc));
			}
			else
			{
				// Log success				
				logDesc = String.Format(CultureInfo.InvariantCulture,"{0} update succeeded for feed {1}", logDescription, dataFeed); 
				Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info, logDesc));			
			}
		}

        /// <summary>
        /// Override. Imports the given file into the database, and runs the import utility argument if exists
        /// </summary>
        /// <param name="filename">The XML import file's location path.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public override int Run(string filename)
        {
            // Call base class run method.
            int statusCode = base.Run(filename);

            try
            {
                //Only run the import utility (batch file) if succeeded so far
                if (statusCode == 0)
                {
                    #region BatchFileProcessing

                    // Call the batch file passed in the import utility argument
                    if (!string.IsNullOrEmpty(importUtility))
                    {
                        string logDesc = string.Empty;
                        if (TDTraceSwitch.TraceVerbose)
                        {
                            logDesc = String.Format(CultureInfo.InvariantCulture, "{0} - calling import utility {1}", logDescription, importUtility);
                            Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info, logDesc));
                        }

                        Console.WriteLine();
                        Console.WriteLine(string.Format("Calling {0}", importUtility));

                        using (Process p = new Process())
                        {
                            p.StartInfo.UseShellExecute = true;
                            p.StartInfo.CreateNoWindow = true;
                            p.StartInfo.FileName = importUtility;
                            p.StartInfo.Arguments = filename.ToString();
                            p.Start();
                            p.WaitForExit();

                            statusCode = p.ExitCode;
                        }
                        
                        Console.WriteLine();

                        if (TDTraceSwitch.TraceVerbose)
                        {
                            logDesc = String.Format(CultureInfo.InvariantCulture, "{0} - calling import utility completed with return code {1}", logDescription, statusCode.ToString());
                            Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info, logDesc));
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error occurred during {0} processing of import utility {1}. Message: {2}", logDescription, importUtility, ex.Message);
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error, message));

                statusCode = (int)TDExceptionIdentifier.DGUnexpectedException;
            }

            return statusCode;
        }
		
		#endregion
    }
}
