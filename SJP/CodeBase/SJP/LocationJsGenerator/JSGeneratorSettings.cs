// *********************************************** 
// NAME             : JSGeneratorSettings.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Class to provide all the settings used in generating location javascripts
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;

namespace SJP.Common.LocationJsGenerator
{
    /// <summary>
    /// Class to provide all the settings used in generating location javascripts
    /// </summary>
    class JSGeneratorSettings
    {
        #region Private Fields
        private static string[] commandLineArgs = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandLineArguments">Command line arguments</param>
        public JSGeneratorSettings(string[] commandLineArguments)
        {
            commandLineArgs = commandLineArguments;
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// The location for writing the scripts to
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string JSLocation(JSGeneratorMode mode)
        {
            if (Properties.Current == null)
                return string.Empty;

            return Properties.Current["JSFolder." + mode];
        }

        #endregion

        #region Public static Properties
        
        /// <summary>
        /// Read only property determining the alias file location
        /// </summary>
        public static string AliasFile
        {
            get
            {
                if (Properties.Current == null)
                    return string.Empty;

                return Properties.Current["AliasFile"];
            }
        }

        /// <summary>
        /// Read only property determining the prefix name for the scripts
        /// </summary>
        public static string ScriptName
        {
            get
            {
                if (Properties.Current == null)
                    return string.Empty;

                return Properties.Current["ScriptName"];
            }
        }

        /// <summary>
        /// Read only property determining the connection string to be used to load the location data
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (Properties.Current == null)
                    return string.Empty;

                return Properties.Current["GazetteerDB"];
            }
        }

        /// <summary>
        /// Read only property determining the version string to use in generating the full name of the script files
        /// </summary>
        /// <remarks>
        /// If the version is provided through command line the property will return that otherwise the property 
        /// returns the date the script generated in yyyyMMdd format
        /// </remarks>
        public static string Version
        {
            get
            {
                string version = GetCommandLineValue("version");

                if (string.IsNullOrEmpty(version))
                    version = GetCommandLineValue("v");

                if (string.IsNullOrEmpty(version))
                {
                    version = DateTime.Now.ToString("yyyyMMdd");
                }

                return version;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a command line param value defined following a key i.e. /key keyvalue
        /// </summary>
        /// <param name="key">key for the value</param>
        /// <returns>command line param value</returns>
        private static string GetCommandLineValue(string key)
        {
            string keyValue = string.Empty;

            if (commandLineArgs != null)
            {

                if (commandLineArgs.Length > 1 && !string.IsNullOrEmpty(key.Trim()))
                {
                    if (commandLineArgs.Contains("/" + key))
                    {
                        string[] argsSubPart = commandLineArgs.SkipWhile(e => e != ("/" + key)).ToArray();

                        if (argsSubPart.Length > 1)
                        {
                            keyValue = argsSubPart[1];

                        }
                    }
                }
            }
                                 

            return keyValue;
        }

        #endregion


    }
}
