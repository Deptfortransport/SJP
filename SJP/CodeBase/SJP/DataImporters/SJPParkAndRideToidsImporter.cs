// *********************************************** 
// NAME             : SJPParkAndRideToidsImporter.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 10 Oct 2011
// DESCRIPTION  	: DataGateway import task to import SJP park and ride toids
// ************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TransportDirect.Datagateway.Framework;
using System.Globalization;
using TransportDirect.Common.PropertyService.Properties;
using System.Diagnostics;
using TransportDirect.Common.Logging;
using TransportDirect.Common.DatabaseInfrastructure;
using TransportDirect.Common;

namespace SJP.DataImporters
{
    /// <summary>
    /// DataGateway import task to import SJP park and ride toids
    /// </summary>
    public class SJPParkAndRideToidsImporter : DatasourceImportTask
    {
        #region Private Fields
        private const string propertyKey = "datagateway.sqlimport.SJPParkAndRideToids.Name";
        private const string propertyFeedName = "datagateway.sqlimport.SJPParkAndRideToids.feedname";
        private string importPropertyName = string.Empty;
        private const string logDescription = "SJP Park And Ride Toids Import";
        private string importFilename = string.Empty;
        private string csvFileName = string.Empty;
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
        public SJPParkAndRideToidsImporter
            (string feed, string params1, string params2, string utility,
            string processingDirectory)
            : base(feed, params1, params2, utility, processingDirectory)
        {
            importPropertyName = Properties.Current[propertyKey];
            xmlschemaLocationKey = string.Format(CultureInfo.InvariantCulture, xmlschemaLocationKey, importPropertyName);
            xmlNamespaceKey = string.Format(CultureInfo.InvariantCulture, xmlNamespaceKey, importPropertyName);
            databaseKey = string.Format(CultureInfo.InvariantCulture, databaseKey, importPropertyName);
            storedProcedureKey = string.Format(CultureInfo.InvariantCulture, storedProcedureKey, importPropertyName);

            // Check the feed name
            if (!dataFeed.Equals(Properties.Current[propertyFeedName]))
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
                logDesc = String.Format(CultureInfo.InvariantCulture, "{0} {1} update failed for feed : {2} ", logDescription, retCode.ToString(), dataFeed.ToString());
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error,
                    logDesc));
            }
            else
            {
                // Log success				
                logDesc = String.Format(CultureInfo.InvariantCulture, "{0}  update succeeded for feed {1}", logDescription, dataFeed);
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Info,
                    logDesc));
            }
        }

        #endregion

        #region Run and Convert methods
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
            
            try
            {
                // check file name is not null 
                if (csvFileName == null)
                {
                    string errorMessage = "No source file found for import";
                    throw new TDException(errorMessage, false, TDExceptionIdentifier.DGInputXMLFileNotFound);
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
                    return false;
                }

                using (XmlCsvReader CsvXmlConverter = new XmlCsvReader(myImportFileName, xmlDoc.NameTable))
                {
                    CsvXmlConverter.FirstRowHasColumnNames = true;

                    CsvXmlConverter.RootName = "SJPParkAndRideToidList";
                    CsvXmlConverter.RowName = "SJPParkAndRideToids";

                    xmlDoc.Load(CsvXmlConverter);


                    Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Verbose, "Csv Load Successful"));

                    // Now appending namespace attribute to the top level element
                    AddAttributeToXmlDoc(xmlDoc, xmlNameSpace, (string)Properties.Current["datagateway.sqlimport.SJPParkAndRideToids.xmlnamespace"]);

                    // Now appending namespace:xsi attribute 		
                    AddAttributeToXmlDoc(xmlDoc, xmlNamespaceXs, (string)Properties.Current["datagateway.sqlimport.SJPParkAndRideToids.xmlnamespacexsi"]);

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
                }

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
                xmlDoc = null;
            }
        }

        /// <summary>
		/// Adds attributes to the xml document 
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <param name="attributeName"></param>
		/// <param name="attributeValue"></param>
		protected virtual void AddAttributeToXmlDoc(XmlDocument xmlDoc, string attributeName , string attributeValue)
		{
			try
			{
				if (xmlDoc!= null)
				{
					// creating a new attribute 
					XmlAttribute Xmlatt = xmlDoc.CreateAttribute(attributeName);
					// assigining a value to attribute 
					Xmlatt.Value = attributeValue;

					// now appending attribute to the collection class
					XmlAttributeCollection attrColl = xmlDoc.DocumentElement.Attributes ; 
					attrColl.Append(Xmlatt);
				}
			}
			catch (TDException tdEx)
			{
				string errorMessage = "Problem adding attribute " + tdEx.Message.ToString(CultureInfo.CurrentCulture);
				throw new TDException(errorMessage, false, TDExceptionIdentifier.ZSAddAtrributeFails );
			}
		}

        /// <summary>
        /// Override of Run method which imports the data.
        /// </summary>
        public override int Run(string filename)
        {
            int returnCode = 0;

            try
            {
                LogStart();

                csvFileName = filename;
                // check if conversion is successful  
                if (!Convert())
                {
                    returnCode = (int)TDExceptionIdentifier.SNBCsvConversionFailed;
                    Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error,"Error in Csv Conversion ", csvFileName.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    // now call perform task to import data to the database 
                    importFile = ProcessingDirectory + "\\" + csvFileName;
                    returnCode = PerformTask();
                }
               
            }
            catch (TDException)
            {
                Trace.Write(new OperationalEvent(TDEventCategory.Business, TDTraceLevel.Error, "Error with CsvTD ", csvFileName.ToString(CultureInfo.InvariantCulture)));

                returnCode = (int)TDExceptionIdentifier.SNBImportFailed;
            }
            finally
            {
                LogFinish(returnCode);
            }

            return returnCode;
        }
        
        #endregion
    }
}


