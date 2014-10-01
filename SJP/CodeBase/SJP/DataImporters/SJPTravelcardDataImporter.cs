// *********************************************** 
// NAME             : SJPTravelcardDataImporter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 16 Jan 2012
// DESCRIPTION  	: DataGateway import task to import SJP Travelcard data
// ************************************************
// 

using System;
using System.Diagnostics;
using System.Globalization;
using TransportDirect.Common.Logging;
using TransportDirect.Common.PropertyService.Properties;
using TransportDirect.Datagateway.Framework;

namespace SJP.DataImporters
{
    /// <summary>
    /// DataGateway import task to import SJP Travelcard data
    /// </summary>
    public class SJPTravelcardDataImporter : DatasourceImportTask
    {
        #region Private Fields

        private const string propertyKey = "datagateway.sqlimport.SJPTravelcardData.Name";
        private const string propertyFeedName = "datagateway.sqlimport.SJPTravelcardData.feedname";
		private string importPropertyName = string.Empty;				
		private const string logDescription = "SJP Travelcard Data Import" ;
		private string importFilename = string.Empty;

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
        public SJPTravelcardDataImporter
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
		
		#endregion
    }
}
