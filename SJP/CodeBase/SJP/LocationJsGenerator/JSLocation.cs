// *********************************************** 
// NAME             : JSLocation.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Represents SJP location to use in javascript generation
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.LocationService;

namespace SJP.Common.LocationJsGenerator
{
    /// <summary>
    /// Represents SJP location to use in javascript generation
    /// </summary>
    class JSLocation
    {
        #region Private Fields
        private string displayName;
        private SJPLocationType locType ;
        private string dataId;
        private string alias;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor initialises sjp location using displayname, location type and dataId
        /// </summary>
        /// <param name="displayName">Display name</param>
        /// <param name="locType">SJP location type</param>
        /// <param name="dataId">SJP location identifier</param>
        public JSLocation(string displayName, SJPLocationType locType, string dataId)
        {
            this.displayName = displayName;
            this.locType = locType;
            this.dataId = dataId;
        }

        /// <summary>
        /// Constructor initialises sjp location using displayname, location type, dataId and alias
        /// </summary>
        /// <param name="displayName">Display name</param>
        /// <param name="locType">SJP location type</param>
        /// <param name="dataId">SJP location identifier</param>
        /// <param name="alias">Alias for the location</param>
        public JSLocation(string displayName, SJPLocationType locType, string dataId, string alias)
            : this(displayName,locType,dataId)
        {
            this.alias = alias;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Read only property determining display name for the location
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// Read only property determining the location identifier
        /// </summary>
        public string DataId
        {
            get { return dataId; }
        }

        /// <summary>
        /// Read only property determining the alias for SJP location
        /// </summary>
        public string Alias
        {
            get { return alias; }
        }

        /// <summary>
        /// Read only property determining type of location
        /// </summary>
        public SJPLocationType LocationType
        {
            get { return locType; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the location object properties serialised as Javascript array
        /// </summary>
        /// <returns>String representing location object serialised as Javascript array</returns>
        public string GetJSArrayString()
        {
            if (string.IsNullOrEmpty(alias))
            {
                return string.Format("[\"{0}\",\"{1}\",\"{2}\"]", displayName, dataId, locType);
            }

            return string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\"]", displayName, dataId, locType, alias);
        }

        #endregion

    }

}
