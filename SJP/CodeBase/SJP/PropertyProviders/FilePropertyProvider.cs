// *********************************************** 
// NAME             : FilePropertyProvider.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 11 Feb 2011
// DESCRIPTION  	: Implemenation of PropertyProvider class to provide properties stored in 
//                    xml file.
// ************************************************


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using SJP.Common.EventLogging;

namespace SJP.Common.PropertyManager.PropertyProviders
{
    /// <summary>
    /// This class provides a method of extracting property definitions
    /// from an Xml file.  The Xml filepath/name is defined in the 
    /// application web config file. The class inherits from the general
    /// properties class which implements the IPropertyProvider interface. 
    /// </summary>
    public class FilePropertyProvider : Properties
    {
        
        #region Constructors
        /// <summary>
        /// Property provider to access properties stored in xml file
        /// </summary>
        public FilePropertyProvider()
        {
            propertyDictionary = new Dictionary<string,string>();
            
            intVersion = 0;

        }
        #endregion

       
        #region Public Methods

        /// <summary>
        /// IsNewVersion method determines if the new version of the properties is available
        /// </summary>
        public bool IsNewVersion()
        {
            try
            {
                int latestVersion = GetVersion();

                if (latestVersion == Version)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                    SJPTraceLevel.Error,
                    "An error has occurred manipulating the Properties Xml File");

                Trace.Write(oe);

                throw new SJPException(
                    "Exception trying to manipulate the Property Xml File : " + e.Message,
                    true,
                    SJPExceptionIdentifier.PSInvalidPropertyFile);

            }

        }

        /// <summary>
        /// Loads properties in to memory from property file 
        /// </summary>
        /// <returns>Null if no new properties exists, or a property provider if new properties are available</returns>
        public override IPropertyProvider Load()
        {
            strApplicationID = ConfigurationManager.AppSettings["propertyservice.applicationid"];

            strGroupID = ConfigurationManager.AppSettings["propertyservice.groupid"];

            if (Version == 0)
            {

                try
                {
                    // Get unique provider settings ( filename
                    string providerFile = (string)ConfigurationManager.AppSettings["propertyservice.providers.fileprovider.filepath"];

                    // populate properties
                    // open the xml file
                    XmlNodeList xNodelist;
                    XmlDocument xdLookup = new XmlDocument();
                    xdLookup.Load(providerFile);

                    // step through xml file and load all properties.

                    // first select the properties for this application
                    xNodelist = xdLookup.DocumentElement.SelectNodes("//property[@AID=\"" + ApplicationID + "\"]");
                    foreach (XmlNode node in xNodelist)
                    {
                        AddProperty(node);
                    }

                    // Next select the properties for this GroupId
                    xNodelist = xdLookup.DocumentElement.SelectNodes("//property[@GID=\"" + GroupID + "\"]");
                    foreach (XmlNode node in xNodelist)
                    {
                        AddProperty(node);
                    }

                    // Next select the global properties
                    xNodelist = xdLookup.DocumentElement.SelectNodes("//property[@GID=\"\"]");
                    foreach (XmlNode node in xNodelist)
                    {
                        AddProperty(node);
                    }

                    // find latest version from property table
                    intVersion = GetVersion();

                    return this;
                }
                catch (FileNotFoundException e)
                {
                    OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Error,
                        "An error has occurred manipulating the Properties Xml File");

                    Trace.Write(oe);

                    throw new SJPException(
                        "Exception trying to manipulate the Property Xml File : " + e.Message,
                        true,
                        SJPExceptionIdentifier.PSInvalidPropertyFile);
                }
                catch (ArgumentException e)
                {
                    OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Error,
                        "An error has occurred manipulating the Properties Xml File");

                    Trace.Write(oe);

                    throw new SJPException(
                        "Exception trying to manipulate the Property Xml File : " + e.Message,
                        true,
                        SJPExceptionIdentifier.PSInvalidPropertyFile);

                }
                catch (SJPException e)
                {
                    OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Error,
                        "An error has occurred manipulating the Properties Xml File");

                    Trace.Write(oe);

                    throw new SJPException(
                        "Exception trying to manipulate the Property Xml File : " + e.Message,
                        true,
                        SJPExceptionIdentifier.PSInvalidPropertyFile);

                }

            }

            // if changed version
            else if (IsNewVersion())
            {
                try
                {


                    FilePropertyProvider newInstance = new FilePropertyProvider();


                    // Call Load in new instantiated object and return it
                    return newInstance.Load();
                }
                catch (ArgumentException e)
                {
                    OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Error,
                        "An error has occurred manipulating the Properties Xml File");
                    
                    Trace.Write(oe);

                    throw new SJPException(
                        "Exception trying to manipulate the Property Xml File : " + e.Message,
                        true,
                        SJPExceptionIdentifier.PSInvalidPropertyFile);

                }
                catch (SJPException e)
                {
                    OperationalEvent oe =
                        new OperationalEvent(SJPEventCategory.Business,
                        SJPTraceLevel.Error,
                        "An error has occurred manipulating the Properties Xml File");
                    
                    Trace.Write(oe);

                    throw new SJPException(
                        "Exception trying to manipulate the Property Xml File : " + e.Message,
                        true,
                        SJPExceptionIdentifier.PSInvalidPropertyFile);

                }

            }
            else
            {
                return null;

            }

        }

        #endregion

        #region Private Methods

        
        /// <summary>
        /// Adds a property to the propertyTable
        /// </summary>
        /// <param name="node"></param>
        private void AddProperty(XmlNode node)
        {
            
            string key = node.Attributes["name"].InnerText;

            string strValue = node.InnerText;

            // if property already exists in the table dont add it.
            if (!propertyDictionary.ContainsKey(key))
                propertyDictionary.Add(key, strValue);

        }

        /// <summary>
        /// Function to test if string value represents an integer
        /// </summary>
        /// <param name="toTest"></param>
        /// <returns></returns>
        private static bool IsNumeric(string toTest)
        {
            // Attempt the conversion and if it errors it's not an integer
            try
            {
                Convert.ToInt32(toTest, CultureInfo.InvariantCulture);
                return true;
            }
            catch (System.FormatException)
            {
            }
            return false;
        }

        /// <summary>
        /// Reads the version from the currently saved properties file
        /// </summary>
        /// <returns></returns>
        private int GetVersion()
        {

            int latestVersion = 0;

            try
            {
                // Get unique provider settings ( filename
                string providerFile = (string)ConfigurationManager.AppSettings["propertyservice.providers.fileprovider.filepath"];

                XmlNode xnQuery;
                XmlDocument xdLookup = new XmlDocument();
                xdLookup.Load(providerFile);
                xnQuery = xdLookup.DocumentElement.SelectSingleNode("//property[@name=\"propertyservice.version\"]");
                latestVersion = Convert.ToInt32(xnQuery.InnerXml, CultureInfo.CurrentCulture.NumberFormat);
            }

            catch (NullReferenceException e)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                    SJPTraceLevel.Error,
                    "An error has occurred accessing the version property in the Properties Xml File : Null Reference");

                Trace.Write(oe);

                throw new SJPException(
                    "Exception trying to get the version property from property Xml file : " + e.Message,
                    false,
                    SJPExceptionIdentifier.PSInvalidVersion);
            }
            catch (XPathException e)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                    SJPTraceLevel.Error,
                    "An error has occurred accessing the version property in the Properties Xml File : incorrect XPath");

                Trace.Write(oe);

                throw new SJPException(
                    "Exception trying to get the version property from property Xml file : " + e.Message,
                    false,
                    SJPExceptionIdentifier.PSInvalidVersion);
            }
            catch (XmlException e)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business,
                    SJPTraceLevel.Error,
                    "An error has occurred accessing the version property in the Properties Xml File");

                Trace.Write(oe);

                throw new SJPException(
                    "Exception trying to get the version property from property Xml file : " + e.Message,
                    false,
                    SJPExceptionIdentifier.PSInvalidVersion);

            }
            catch (Exception e)
            {
                OperationalEvent oe =
                    new OperationalEvent(SJPEventCategory.Business,
                    SJPTraceLevel.Error,
                    "An error has occurred accessing the version property in the Properties Xml File");

                Trace.Write(oe);

                throw new SJPException(
                    "Exception trying to get the version property from property Xml file : " + e.Message,
                    false,
                    SJPExceptionIdentifier.PSInvalidVersion);

            }
            

            return latestVersion;

        }

        #endregion

    }
}
