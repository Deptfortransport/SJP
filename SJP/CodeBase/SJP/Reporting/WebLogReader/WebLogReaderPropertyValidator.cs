// *********************************************** 
// NAME             : WebLogReaderPropertyValidator.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Validates properties read into the WebLogReaderController
// ************************************************
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SJP.Common;
using SJP.Common.PropertyManager;

namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Validates properties read into the WebLogReaderController
    /// </summary>
    public class WebLogReaderPropertyValidator : PropertyValidator
    {
        #region Constructor

        public WebLogReaderPropertyValidator(IPropertyProvider properties)
            : base(properties)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Used to Validate the assocaiated Properties.
        /// Pass in the the WebLogReader.Machine key and the method will 
        /// validate all the other properties for each value in the key.
        /// </summary>
        /// <param name="key">Property key name.</param>
        /// <param name="errors">Used to return any errors found in property value of the key passed.</param>
        /// <returns>True if property is valid, else false</returns>
        /// <exception cref="TDException">
        /// If unknown property key is passed.
        /// </exception>
        public override bool ValidateProperty(string key, List<string> errors)
        {
            if (key == Keys.WebLogReaderArchiveDirectory)
                return ValidateArchiveDirectory(key, errors);
            else if (key == Keys.WebLogReaderLogDirectory)
                return ValidateLogDirectory(key, errors);
            else if (key == Keys.WebLogReaderNonPageMinimumBytes)
                return ValidateNonPageMinimumBytes(key, errors);
            else if (key == Keys.WebLogReaderWebPageExtensions)
                return ValidateWebPageExtensions(key, errors);
            else if (key == Keys.WebLogReaderClientIPExcludes)
                return ValidateClientIPExclusions(key, errors);
            else if (key == Keys.WebLogReaderWebLogFolders)
                return ValidateWebLogFolders(key, errors);
            else if (key == Keys.WebLogReaderUserExperienceVisitorUserAgent)
                return ValidateExistence(key, Optionality.Mandatory, errors);
            else if (key == Keys.WebLogReaderCookieSessionIdStartMarker)
                return ValidateExistence(key, Optionality.Mandatory, errors);
            else if (key == Keys.WebLogReaderCookieSessionIdEndMarker)
                return ValidateExistence(key, Optionality.Mandatory, errors);
            else
            {
                throw new SJPException(String.Format(Messages.Init_UnknownPropertyKey, key), false, SJPExceptionIdentifier.RDPWebLogReaderUnknownPropertyKey);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Validates the property value of the key passed
        /// </summary>
        /// <param name="key">key neame that contains the value to validate</param>
        /// <param name="errors">Used to return any errors found</param>
        /// <returns>True if no errors found else false.</returns>
        private bool ValidateWebLogFolders(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (ValidateExistence(key, Optionality.Mandatory, errors))
            {
                string sFolders = properties[key];
                if (sFolders.Split(' ').Length == 0)
                    errors.Add(string.Format(Messages.Validation_MissingWebLogsFolders, key));
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates the property value of the key passed.
        /// </summary>
        /// <param name="key">Key name that contains value to validate.</param>
        /// <param name="errors">Used to return any errors found.</param>
        /// <returns>True if no errors found else false.</returns>
        private bool ValidateArchiveDirectory(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;



            // assumes that if arrived till here, WebLogFolders property is OK!
            string[] webLogFolders = Properties.Current[Keys.WebLogReaderWebLogFolders].Split(' ');

            foreach (string folder in webLogFolders)
            {
                string archiveDirectoryKey = string.Format(key, folder);

                if (ValidateExistence(archiveDirectoryKey, Optionality.Mandatory, errors))
                {
                    if (!Directory.Exists(properties[archiveDirectoryKey]))
                        errors.Add(String.Format(Messages.Validation_BadArchiveDir, properties[archiveDirectoryKey], archiveDirectoryKey));
                }
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates the property value of the key passed.
        /// </summary>
        /// <param name="key">Key name that contains value to validate.</param>
        /// <param name="errors">Used to return any errors found.</param>
        /// <returns>True if no errors found else false.</returns>
        private bool ValidateLogDirectory(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;

            // assumes that if arrived till here, WebLogFolders property is OK!
            string[] webLogFolders = Properties.Current[Keys.WebLogReaderWebLogFolders].Split(' ');

            foreach (string folder in webLogFolders)
            {
                string logDirectoryKey = string.Format(key, folder);

                if (ValidateExistence(logDirectoryKey, Optionality.Mandatory, errors))
                {

                    if (!Directory.Exists(properties[logDirectoryKey]))
                    {
                        errors.Add(String.Format(Messages.Validation_BadLogDir, properties[logDirectoryKey], logDirectoryKey));
                    }
                    else
                    {
                        // Ensure that the server on which logs reside is working to GMT.
                        ValidateLocalTimeZone(errors);
                    }
                }
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates the property value of the key passed.
        /// </summary>
        /// <param name="key">Key name that contains value to validate.</param>
        /// <param name="errors">Used to return any errors found.</param>
        /// <returns>True if no errors found else false.</returns>
        private bool ValidateNonPageMinimumBytes(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (ValidateExistence(key, Optionality.Mandatory, errors))
            {
                if (int.Parse(properties[key]) < 0)
                    errors.Add(String.Format(Messages.Validation_NonPageMinimumBytesInvalid, properties[key], key));
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates the property value of the key passed.
        /// </summary>
        /// <param name="key">Key name that contains value to validate.</param>
        /// <param name="errors">Used to return any errors found.</param>
        /// <returns>True if no errors found else false.</returns>
        private bool ValidateLocalTimeZone(List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (TimeZone.CurrentTimeZone.StandardName.ToString() != "GMT Standard Time")
                errors.Add(Messages.Validation_TimeZoneInvalid);

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates the property value of the key passed.
        /// </summary>
        /// <param name="key">Key name that contains value to validate.</param>
        /// <param name="errors">Used to return any errors found.</param>
        /// <returns>True if no errors found else false.</returns>
        private bool ValidateWebPageExtensions(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;

            ValidateExistence(key, Optionality.Mandatory, errors);

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Validates the property value of the key passed.
        /// </summary>
        /// <param name="key">Key name that contains value to validate.</param>
        /// <param name="errors">Used to return any errors found.</param>
        /// <returns>True if no errors found else false.</returns>
        private bool ValidateClientIPExclusions(string key, List<string> errors)
        {
            int errorsBefore = errors.Count;

            if (ValidateExistence(key, Optionality.Mandatory, errors))
            {
                if (properties[Keys.WebLogReaderClientIPExcludes].Length != 0)
                {
                    string[] ipList = properties[Keys.WebLogReaderClientIPExcludes].Split(' ');

                    foreach (string ip in ipList)
                    {
                        if (ip != " ")
                        {
                            if (!ValidateIPAddress(ip))
                                errors.Add(String.Format(Messages.Validation_IPAddressInvalid, ip, key));
                        }

                    }
                }
            }

            return (errorsBefore == errors.Count);
        }

        /// <summary>
        /// Checks that IP address passed is in correct format.
        /// </summary>
        /// <param name="ipAddress">IP Address to validate</param>
        /// <returns>True if IP address is valid, otherwise false.</returns>
        /// <remarks>
        /// IP Address passed must be in IPv4 Decimal format.
        /// </remarks>
        private bool ValidateIPAddress(string ipAddress)
        {
            bool valid = true;
            ArrayList ipParts = new ArrayList(ipAddress.Split(('.')));

            valid = (ipParts.Count == 4);

            if (valid)
            {
                for (int i = 0; i < ipParts.Count; i++)
                {
                    string ipPart = ipParts[i].ToString().Replace(" ", "");

                    while (ipPart.Length < 3)
                        ipPart = "0" + ipPart;

                    int ipPartAsInt = Convert.ToInt32(ipPart, 10);

                    if (ipPartAsInt >= 256)
                    {
                        valid = false;
                        break;
                    }
                }
            }

            return valid;
        }

        #endregion
    }
}
