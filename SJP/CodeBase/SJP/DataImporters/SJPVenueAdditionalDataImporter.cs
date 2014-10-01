// *********************************************** 
// NAME             : SJPVenueAdditionalDataImporter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 28 Apr 2011
// DESCRIPTION  	: DataGateway import task to import SJP venue additional data
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportDirect.Datagateway.Framework;
using TransportDirect.Common.PropertyService.Properties;
using System.Globalization;
using TransportDirect.Common.Logging;
using System.Diagnostics;

namespace SJP.DataImporters
{
    /// <summary>
    /// DataGateway import task to import SJP venue additional data
    /// </summary>
    public class SJPVenueAdditionalDataImporter: DatasourceImportTask
    {
        #region Private Fields
        private const string propertyKey = "datagateway.sqlimport.SJPAdditionalVenueData.Name";
        private const string propertyFeedName = "datagateway.sqlimport.SJPAdditionalVenueData.feedname";
		private string importPropertyName = string.Empty;				
		private const string logDescription = "SJP Additional Venue Data Import" ;
		private string importFilename = string.Empty ;		
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
        public SJPVenueAdditionalDataImporter
			(string feed, string params1, string params2, string utility,
			string processingDirectory) : base(feed, params1, params2, utility, processingDirectory)
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
			Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info, 
				logDesc));			
			
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
				Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error, 
					logDesc));
			}
			else
			{
				// Log success				
				logDesc = String.Format(CultureInfo.InvariantCulture,"{0}  update succeeded for feed {1}", logDescription, dataFeed); 
				Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info, 
					logDesc));			
			}
		}
		
		#endregion
	
		
	
    }
}
