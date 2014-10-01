// *********************************************** 
// NAME             : SJPStopAccessibilityLinksDataImporter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 June 2011
// DESCRIPTION  	: DataGateway import task to import SJP stop accessibility links data
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportDirect.Datagateway.Framework;
using TransportDirect.Common.PropertyService.Properties;
using System.Globalization;
using TransportDirect.Common.Logging;
using System.Diagnostics;
using TransportDirect.Common;
using System.Xml;
using TransportDirect.Common.DatabaseInfrastructure;
using System.IO;

namespace SJP.DataImporters
{
    /// <summary>
    /// DataGateway import task to import SJP stop accessibility links data
    /// </summary>
    public class SJPStopAccessibilityLinksDataImporter : DatasourceImportTask
    {
        #region Private Fields

        private string importPropertyName = string.Empty;
        private const string propertyKey = "datagateway.sqlimport.SJPStopAccessibilityLinksData.Name";
        private const string propertyFeedName = "datagateway.sqlimport.SJPStopAccessibilityLinksData.feedname";
        
        private const string logDescription = "SJP Stop Accessibility Links Data Import";
		
        private bool boolValidfeedname = false;
        private string csvFileName = string.Empty;

        #region Error Messages

        private const string errorCsvConversion = "Unable to produce xml file from given csv file: {0} update failed for feed";
        private const string errorCsvTDError = "The following error occurred while converting csv file {0} to xml: {1}";
        private const string errorInvalidFeedName = "Invalid feed name: {0}. Update failed for feed";
        private const string messageCsvLoadSuccessful = "The csv file has been loaded correctly";
        
        #endregion
        
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
        public SJPStopAccessibilityLinksDataImporter
			(string feed, string params1, string params2, string utility,
			string processingDirectory) : base(feed, params1, params2, utility, processingDirectory)
		{
			importPropertyName = Properties.Current[propertyKey]; 				    
			xmlschemaLocationKey = string.Format(CultureInfo.InvariantCulture, xmlschemaLocationKey, importPropertyName);
			xmlNamespaceKey = string.Format(CultureInfo.InvariantCulture, xmlNamespaceKey, importPropertyName);
            xmlNamespaceXsiKey = string.Format(CultureInfo.InvariantCulture, xmlNamespaceXsiKey, importPropertyName);
			databaseKey = string.Format(CultureInfo.InvariantCulture, databaseKey, importPropertyName);
			storedProcedureKey = string.Format(CultureInfo.InvariantCulture, storedProcedureKey, importPropertyName);		

			// Check the feed name
            if (!dataFeed.Equals(Properties.Current[propertyFeedName]))
            {
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error, logDescription + " unexpected feed name: [" + dataFeed + "]"));
            }
            else
            {
                boolValidfeedname = true;
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
				logDesc = String.Format(CultureInfo.InvariantCulture,"{0}  update succeeded for feed {1}", logDescription, dataFeed); 
				Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info, logDesc));
			}
		}
		
		#endregion

        #region Overridden public methods

        /// <summary>
        /// Overidden method of the base run method. Validate the source data file and calls the
        /// Convert method to process the data. If conversion is successful the run method
        /// of the base class is called, otherwise the error code is returned
        /// </summary>
        /// <param name="filename">The name of the file containing the csv data to be validated</param>
        /// <returns>integer - return value</returns>
        public override int Run(string filename)
        {
            int returnCode = 0;

            try
            {
                LogStart();

                //go no further if the feedname is invalid
                if (!boolValidfeedname)
                {
                    Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error,
                        string.Format(CultureInfo.InvariantCulture, errorInvalidFeedName, dataFeed)));
                    returnCode = (int)TDExceptionIdentifier.DGUnexpectedFeedName;
                }
                else
                {
                    csvFileName = filename;
                    // check if conversion is successful  
                    if (!Convert())
                    {
                        Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error,
                            string.Format(CultureInfo.InvariantCulture, errorCsvConversion, csvFileName.ToString(CultureInfo.InvariantCulture))));
                        returnCode = (int)TDExceptionIdentifier.DGCSVtoXMLConversionFailed;
                    }
                    else
                    {
                        // now call perform task to import data to the database 
                        importFile = ProcessingDirectory + "\\" + csvFileName;
                        returnCode = PerformTask();
                    }
                }
            }
            catch (TDException tdex)
            {
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error,
                    string.Format(CultureInfo.InvariantCulture, errorCsvTDError, csvFileName.ToString(CultureInfo.InvariantCulture),
                    tdex.Message.ToString(CultureInfo.InvariantCulture))));

                returnCode = (int)TDExceptionIdentifier.DDGImportFailed;
            }
            finally
            {
                LogFinish(returnCode);
            }

            return returnCode;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Converts values in the csv file to xml format
        /// </summary>
        /// <returns>true if conversion is successful, false otherwise</returns>
        private bool Convert()
        {
            string fullFilePath = string.Empty;
            string outputXmlFileName = string.Empty;
            string outputXmlFilePath = string.Empty;

            string xmlNameSpace = @"xmlns";
            string xmlNamespaceXs = @"xmlns:xs";
            XmlDocument xmlDoc = new XmlDocument();

            System.Uri myImportFileName;
            XmlCsvReader CsvXmlConverter = new TransportDirect.Common.DatabaseInfrastructure.XmlCsvReader();

            try
            {
                // check file name is not null 
                if (csvFileName == null)
                {
                    string errorMessage = "No source file found for " + logDescription;
                    throw new TDException(errorMessage, false, TDExceptionIdentifier.DGFileCopyFileNotFound);
                }

                // getting full path 
                fullFilePath = ProcessingDirectory.ToString(CultureInfo.InvariantCulture) + @"\" + csvFileName;

                // getting output file name 
                outputXmlFileName = csvFileName.Substring(0, csvFileName.Length - 4).Trim() + ".xml";
                // getting output file path 
                outputXmlFilePath = ProcessingDirectory.ToString(CultureInfo.InvariantCulture) + @"\" + outputXmlFileName;

                myImportFileName = new System.Uri("file://" + fullFilePath);

                // check if file exists
                if (!File.Exists(fullFilePath))
                {
                    string errorMessage = "No file found at full path : " + fullFilePath;
                    throw new TDException(errorMessage, false, TDExceptionIdentifier.DGFileCopyFileNotFound);
                }
                CsvXmlConverter = new TransportDirect.Common.DatabaseInfrastructure.XmlCsvReader(myImportFileName, xmlDoc.NameTable);

                CsvXmlConverter.FirstRowHasColumnNames = true;
                CsvXmlConverter.RootName = "StopAccessibilityLinks";
                CsvXmlConverter.RowName = "StopAccessibilityLink";

                xmlDoc.Load(CsvXmlConverter);

                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Verbose, messageCsvLoadSuccessful));

                // Now appending namespace attribute to the top level element
                AddAttributeToXmlDoc(xmlDoc, xmlNameSpace, Properties.Current[xmlNamespaceKey]);

                // Now appending namespace:xsi attribute 		
                AddAttributeToXmlDoc(xmlDoc, xmlNamespaceXs, Properties.Current[xmlNamespaceXsiKey]);

                // make sure output file is not read-only 
                if (File.Exists(outputXmlFilePath))
                {
                    File.SetAttributes(outputXmlFilePath, FileAttributes.Normal);
                }

                //now save the file
                xmlDoc.Save(outputXmlFilePath);

                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Verbose,
                    " csv file has been converted successfully at: " + outputXmlFilePath.ToString(CultureInfo.InvariantCulture)));

                // Assigning xml file as input file now..
                csvFileName = outputXmlFileName;

                return true;
            }
            catch (TDException tdex)
            {
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error,
                    tdex.Message.ToString(CultureInfo.InvariantCulture) + ": " + " update failed for feed "));
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error,
                    ex.Message.ToString(CultureInfo.InvariantCulture)));
                return false;
            }
            finally
            {
                if ((CsvXmlConverter.ReadState != ReadState.Closed) && (CsvXmlConverter.ReadState == ReadState.EndOfFile))
                { CsvXmlConverter.Close(); }
                xmlDoc = null;
                CsvXmlConverter = null;
            }
        }

        /// <summary>
        /// Adds attributes to the xml document 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        protected virtual void AddAttributeToXmlDoc(XmlDocument xmlDoc, string attributeName, string attributeValue)
        {
            try
            {
                if (xmlDoc != null)
                {
                    // creating a new attribute 
                    XmlAttribute Xmlatt = xmlDoc.CreateAttribute(attributeName);
                    // assigining a value to attribute 
                    Xmlatt.Value = attributeValue;

                    // now appending attribute to the collection class
                    XmlAttributeCollection attrColl = xmlDoc.DocumentElement.Attributes;
                    attrColl.Append(Xmlatt);
                }
            }
            catch (TDException tdEx)
            {
                string errorMessage = "Problem adding attribute " + tdEx.Message.ToString(CultureInfo.CurrentCulture);
                throw new TDException(errorMessage, false, TDExceptionIdentifier.DGInputXMLFileHasInvalidCharacters);
            }
        }

        #endregion
    }
}
