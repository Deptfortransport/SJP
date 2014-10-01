// *********************************************** 
// NAME             : XmlTransferTask.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: Transfer Task to retrieve and transfer an xml file from a Web source and a File source
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using Logger = System.Diagnostics.Trace;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SJP.DataLoader
{
    /// <summary>
    /// Transfer Task to retrieve and transfer an xml file from a Web source and a File source
    /// </summary>
    public class XmlTransferTask : TransferTask
    {
        #region Private struct

        /// <summary>
        /// Struct to hold file locations to transfer from
        /// </summary>
        private struct FileLocation
        {
            public string locationId;
            public string locationPath;

            public FileLocation(string locationId, string locationPath)
            {
                this.locationId = locationId;
                this.locationPath = locationPath;
            }
        }

        #endregion

        #region Protected members

        protected string Prop_Locations = "DataLoader.Transfer.{0}.Locations";
        protected string Prop_Location = "DataLoader.Transfer.{0}.Location.{1}";
        protected string Prop_Location_Username = "DataLoader.Transfer.{0}.Location.{1}.Username";
        protected string Prop_Location_Password = "DataLoader.Transfer.{0}.Location.{1}.Password";
        protected string Prop_Location_Domain = "DataLoader.Transfer.{0}.Location.{1}.Domain";
        protected string Prop_Location_Proxy = "DataLoader.Transfer.{0}.Location.{1}.Proxy";

        // File name to save at
        protected string Prop_FileName = "DataLoader.Transfer.{0}.FileName.Save";
                        
        #endregion

        #region Private members

        private string transferLocation = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public XmlTransferTask(string dataName, string dataDirectory)
            : base(dataName, dataDirectory)
        {
            DataTransferName = "XmlTransferTask";

            Prop_Locations = string.Format(Prop_Locations, dataName);
            Prop_FileName = string.Format(Prop_FileName, dataName);
        }

        #endregion

        #region Overridden protected methods

        /// <summary>
        /// Run class to call the transfer and logging methods in sequence to transfer the file
        /// </summary>
        public override int Run()
        {
            return base.Run();
        }

        /// <summary>
        /// Override the base class PerformTask method.
        /// </summary>
        /// <returns></returns>
        protected override int PerformTask()
        {
            // Assume success
            int result = 0;

            List<FileLocation> locations = GetDataTransferLocations();

            if ((locations != null) && (locations.Count > 0))
            {
                // Get file
                XmlDocument xmlDoc = GetFile(locations);

                // Document found
                if (xmlDoc != null)
                {
                    // Declare file and directory to save at
                    string fileName = DataDirectory + Properties.Current[Prop_FileName];

                    try
                    {
                        // Save the file
                        using (StreamWriter sw = new StreamWriter(fileName, false))
                        {
                            xmlDoc.Save(sw);

                            string message = string.Format("File saved to [{0}]", fileName);
                            Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info, message));
                        }
                    }
                    catch (Exception ex)
                    {
                        result = (int)SJPExceptionIdentifier.DLDataLoaderXmlFileSaveError;

                        string message = string.Format("Error saving transfered file [{0}] to [{1}]. Message: {2}", transferLocation, fileName, ex.Message);
                        Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
                    }
                }
                else
                {
                    result = (int)SJPExceptionIdentifier.DLDataLoaderXmlFileTransferError;

                    // If reached here, then failed to get the document from any of the locations
                    string message = string.Format("Error transfering file failed for {0}", DataName);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
                }
            }
            else
            {
                result = (int)SJPExceptionIdentifier.DLDataLoaderFileTransferLocationsNotFound;

                string message = string.Format("No transfer file locations have been configured for {0}", DataName);
                Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
            }
            
            //If we have reached this point then the transfer was completed successfully
            return result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns the list of data transfer locations from the Propreties 
        /// </summary>
        /// <returns></returns>
        private List<FileLocation> GetDataTransferLocations()
        {
            // Get data file locations to transfer from
            List<FileLocation> locations = new List<FileLocation>();

            // Read property containing the location keys
            string locationsKeys = Properties.Current[Prop_Locations];

            if (!string.IsNullOrEmpty(locationsKeys))
            {
                string[] locationKeys = locationsKeys.Trim().Split(',');

                foreach (string key in locationKeys)
                {
                    // Read each property for the location
                    string location = Properties.Current[string.Format(Prop_Location, DataName, key)];

                    if (!string.IsNullOrEmpty(location))
                    {
                        locations.Add(new FileLocation(key, location.Trim()));
                    }
                }
            }

            return locations;
        }

        /// <summary>
        /// Returns the first XmlDocument retrieved from the supplied list of locations
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        private XmlDocument GetFile(List<FileLocation> locations)
        {
            // Attempt to get the Xml document 
            foreach (FileLocation fileLocation in locations)
            {
                try
                {
                    XmlDocument xmlDoc = null;

                    if (fileLocation.locationPath.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Load from http source
                        xmlDoc = GetFileFromFromHttp(fileLocation.locationPath,
                            Properties.Current[string.Format(Prop_Location_Username, DataName, fileLocation.locationId)],
                            Properties.Current[string.Format(Prop_Location_Password, DataName, fileLocation.locationId)],
                            Properties.Current[string.Format(Prop_Location_Domain, DataName, fileLocation.locationId)],
                            Properties.Current[string.Format(Prop_Location_Proxy, DataName, fileLocation.locationId)]);
                    }
                    else
                    {
                        // Load from a folder source
                        xmlDoc = new XmlDocument();
                        xmlDoc.Load(fileLocation.locationPath);
                    }

                    if (xmlDoc != null)
                    {
                        // Flag which location retrieved from for logging
                        transferLocation = fileLocation.locationPath;

                        string message = string.Format("File transfered from [{0}]", fileLocation.locationPath);
                        Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info, message));

                        // Document retrieved, return
                        return xmlDoc;
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error transfering file from [{0}]. Error: {1}", fileLocation.locationPath, ex.Message);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, message));
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the XML file from an HTTP source
        /// </summary>
        /// <param name="HttpPath">The HTTP Source</param>
        /// <returns>An Xml Document containing the Xml from the Http Source</returns>
        public XmlDocument GetFileFromFromHttp(string httpPath, string username, string password, string domain, string proxy)
        {
            // Override automatic validation of SSL server certificates.
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertficate;

            // Set up the request
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(httpPath);

            // Only add credentials if passed in
            if (!string.IsNullOrEmpty(username))
            {
                if (string.IsNullOrEmpty(password))
                {
                    password = string.Empty;
                }

                if (string.IsNullOrEmpty(domain))
                {
                    domain = string.Empty;
                }

                webRequest.Credentials = new NetworkCredential(username, password, domain);
                webRequest.PreAuthenticate = true;
            }

            // Only add proxy if passed in
            if (!string.IsNullOrEmpty(proxy))
            {
                WebProxy webProxy = new WebProxy(proxy, true);
                webRequest.Proxy = webProxy;
            }
                                    
            // Get the response
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                        
            // Read in the Xml doc returned
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.ValidationType = ValidationType.None;
            settings.ProhibitDtd = false;

            using (XmlReader reader = XmlReader.Create(webResponse.GetResponseStream(), settings))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);

                return xmlDoc;
            }
        }

        /// <summary>
        /// Validates the SSL server certificate. Returns true always (i.e. invalid certificates are ok)
        /// </summary>
        private static bool ValidateServerCertficate(
                object sender,
                X509Certificate cert,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        #endregion
    }
}
