// *********************************************** 
// NAME             : DatabaseLoadTask.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: Import an XML file into a database using a specified stored procedure
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.DataLoader
{
    /// <summary>
    /// Import an XML file into a database using a specified stored procedure
    /// </summary>
    public class DatabaseLoadTask : LoadTask
    {
        #region Protected members

        protected string Prop_xmlValidate = "DataLoader.Load.{0}.Xml.Validate";
        protected string Prop_xmlSchema = "DataLoader.Load.{0}.Xml.Schema";
        protected string Prop_xmlNamespace = "DataLoader.Load.{0}.Xml.Namespace";
        protected string Prop_xmlNamespaceXsi = "DataLoader.Load.{0}.Xml.NamespaceXsi";

        protected string Prop_database = "DataLoader.Load.{0}.Database";
        protected string Prop_databaseStoredProcedure = "DataLoader.Load.{0}.DatabaseStoredProcedure";
        protected string Prop_databaseTimeout = "DataLoader.Load.{0}.DatabaseTimeout";
                
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseLoadTask(string dataName, string dataDirectory)
            : base(dataName, dataDirectory)
        {
            DataLoaderName = "DatabaseLoadTask";

            Prop_xmlValidate = string.Format(Prop_xmlValidate, dataName);
            Prop_xmlSchema = string.Format(Prop_xmlSchema, dataName);
            Prop_xmlNamespace = string.Format(Prop_xmlNamespace, dataName);
            Prop_xmlNamespaceXsi = string.Format(Prop_xmlNamespaceXsi, dataName);

            Prop_database = string.Format(Prop_database, dataName);
            Prop_databaseStoredProcedure = string.Format(Prop_databaseStoredProcedure, dataName);
            Prop_databaseTimeout = string.Format(Prop_databaseTimeout, dataName);
        }

        #endregion

        #region Overridden protected methods

        /// <summary>
        /// Run class to call the load and logging methods in sequence to load the file into the database.
        /// </summary>
        /// <param name="dataFile">The XML data file name (not including path)</param>
        public override int Run(string dataFile)
        {
            return base.Run(dataFile);
        }

        /// <summary>
        /// Override the base class PerformTask method.
        /// </summary>
        /// <returns></returns>
        protected override int PerformTask()
        {
            // Assume success
            int result = 0;

            // Read the xml file into a string
            string xmlString = LoadXML();

            if (!string.IsNullOrEmpty(xmlString))
            {
                //Validate the xml file against the xsd schema specified in the xml file
                if (ValidateXML())
                {
                    //Update the xml to remove attributes that SQL Server will get confused by
                    xmlString = UpdateXML(xmlString);

                    //Upload the xml into the database
                    result = ImportDataIntoSQLServer(xmlString);
                }
                else
                {
                    result = (int)SJPExceptionIdentifier.DLDataLoaderXmlSchemaValidationFailed;
                }
            }
            else
            {
                result = (int)SJPExceptionIdentifier.DLDataLoaderXmlFileReadError;
            }

            //If we have reached this point then the load was successful
            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Extracts the XML string from the given file.
        /// </summary>
        /// <returns></returns>
        private string LoadXML()
        {
            if (SJPTraceSwitch.TraceVerbose)
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                    string.Format("Opening stream to XML file [{0}]", file)));
            }

            string xmlString = string.Empty;
            try
            {
                using (StreamReader streamReader = new StreamReader(file))
                {
                    xmlString = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error reading file [{0}]. Message: {1}", file, ex.Message);
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
            }

            return xmlString;
        }
        
        /// <summary>
        /// Validates the XML against the XSD schema which should
        /// reside in the same path as the actual XML file.
        /// </summary>
        /// <returns></returns>
        protected bool ValidateXML()
        {
            bool validate = Properties.Current[Prop_xmlValidate].Parse(false);

            if (validate)
            {
                try
                {
                    if (SJPTraceSwitch.TraceVerbose)
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                            "Setting up XML validation engine"));
                    }

                    string schemaLocation = Properties.Current[Prop_xmlSchema];
                    string xmlNS = Properties.Current[Prop_xmlNamespace];

                    if (string.IsNullOrEmpty(schemaLocation))
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                            "XML schema file not specified in Properties"));
                        throw new SJPException("XML schema file not specified in Properties", true, SJPExceptionIdentifier.DLDataLoaderXmlSchemaFileUnspecified);
                    }
                    if (string.IsNullOrEmpty(xmlNS))
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                            "XML namespace not specified in Properties"));
                        throw new SJPException("XML namespace not specified in Properties", true, SJPExceptionIdentifier.DLDataLoaderXmlNamespaceUnspecified);
                    }

                    XmlSchemaSet schemaSet = new XmlSchemaSet();
                    schemaSet.Add(xmlNS, schemaLocation);

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ValidationType = ValidationType.Schema;
                    settings.Schemas = schemaSet;
                    settings.ValidationEventHandler += new ValidationEventHandler(ValidationEventHandler);

                    using (XmlReader xmlValidator = XmlReader.Create(file, settings))
                    {
                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose,
                                "Validating XML "));
                        }

                        //Chek the XML document
                        while (xmlValidator.Read())
                        {
                            if (xmlValidator.NodeType == XmlNodeType.EntityReference)
                            {
                                xmlValidator.ResolveEntity();
                            }
                        }
                    }

                    // If reached here, then xml valid
                    return true;
                }
                catch (Exception ex)
                {
                    // Any parsing errors are raised as an SJPExceptopn in the ValidationEventHandler
                    // which is caught here, so the error needs to be simply logged and false returned
                    string message = string.Format("XML schema validation failed [{0}]. Message: {1}", file, ex.Message);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
                }

                // If reached here, then xml failed validation
                return false;
            }
            else
            {
                // Not validating xml
                return true;
            }
        }

        /// <summary>
        /// Any anomalies found in the XML document will be raised through here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            // Cause an error to let the caller know that the XML file is invalid
            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                "XML parser error: " + args.Message));
            throw new SJPException("XML parser error: " + args.Message, true, SJPExceptionIdentifier.DLDataLoaderXmlSchemaValidationFailed);
        }

        /// <summary>
        /// Removes the first occurance of an attribute from an XML string
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private string RemoveAttribute(string xmlString, string attributeName)
        {
            int start = xmlString.IndexOf(attributeName + "=\"");

            if (start > -1)
            {
                int end = xmlString.IndexOf("\"", start + attributeName.Length + 2);
                return xmlString.Substring(0, start - 1) + xmlString.Substring(end + 1);
            }

            return xmlString;
        }
        
        /// <summary>
        /// Update the xml to remove attributes that will confuse SQL Server
        /// This must be done with string manipulation because the XML DOM will not
        /// allow the removal of the xmlns attribute
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        protected string UpdateXML(string xmlString)
        {
            string ret = xmlString;
            ret = RemoveAttribute(ret, "xmlns");
            ret = RemoveAttribute(ret, "xsi:schemaLocation");
			ret = RemoveAttribute(ret, "xmlns:xs");

            // Replace the ’ as sql xml parser throws error
            ret = ret.Replace('’', '\'');

            return ret;
        }
        
        /// <summary>
        /// Import the XML into the database. The XML string is passed directly
        /// into a stored procedure. The stored procedure does all the work
        /// extracing the appropriate data from the XML into the database tables.
        /// </summary>
        /// <param name="XMLString"></param>
        protected int ImportDataIntoSQLServer(string XMLString)
        {
            int result = 0;

            string database = Properties.Current[Prop_database];
            string storedProcedureName = Properties.Current[Prop_databaseStoredProcedure];
            int commandTimeOut = Properties.Current[Prop_databaseTimeout].Parse(30000);
            			
            if (string.IsNullOrEmpty(database))
            {
                result = (int)SJPExceptionIdentifier.DLDataLoaderDatabaseUnspecified;
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                    "Database not specified in Properties"));
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                result = (int)SJPExceptionIdentifier.DLDataLoaderStoredProcedureUnspecified;
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                    "Stored procedure not specified in Properties"));
            }

            if ( result == 0 ) 
            {
                SqlHelperDatabase datasource = database.Parse(SqlHelperDatabase.DefaultDB);

                //Object for managing database calling
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    //Open the database connection
                    try
                    {
                        Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info, 
                            string.Format("Connecting to database[{0}]", datasource.ToString())));
                        sqlHelper.ConnOpen(datasource);
                    }
                    catch (Exception e)
                    {
                        result = (int)SJPExceptionIdentifier.DLDataLoaderDatabaseConnectionError;
                        Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                            "Database connection error: " + e.Message));
                    }

                    if (result == 0)
                    {
                        //Call the stored procedure
                        try
                        {
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(new SqlParameter("@XML", XMLString));
                            
                            if (SJPTraceSwitch.TraceVerbose)
                            {
                                foreach (SqlParameter param in parameters)
                                {
                                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Verbose, 
                                        string.Format("Element:: \t{0}:\t{1}", param.ParameterName, param.Value)));
                                }
                            }

                            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info, 
                                string.Format("Calling stored procedure[{0}] on database[{1}]", storedProcedureName, database)));

                            sqlHelper.Execute(storedProcedureName, parameters, commandTimeOut);
                        }
                        catch (SqlException sqlEx)
                        {
                            string message = string.Format("Stored procedure[{0}] calling error: {1}", storedProcedureName, sqlEx.Message);

                            result = (int)SJPExceptionIdentifier.DLDataLoaderStoredProcedureError;
                            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("Stored procedure[{0}] calling error: {1}", storedProcedureName, ex.Message);

                            result = (int)SJPExceptionIdentifier.DLDataLoaderStoredProcedureError;
                            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
