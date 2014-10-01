// *********************************************** 
// NAME             : AdminArea.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 Jun 2011
// DESCRIPTION  	: Represents NPTG administrative areas
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DataServices.NPTG
{
    /// <summary>
    /// Represents NPTG administrative areas
    /// </summary>
    public class AdminArea
    {
        #region Private Fields
        private int administrativeAreaCode;
        private string areaName;
        private string countryCode;
        private string regionCode;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor - Represents NPTG administrative areas
        /// </summary>
        /// <param name="administrativeAreaCode"></param>
        /// <param name="areaName"></param>
        /// <param name="countryCode"></param>
        /// <param name="regionCode"></param>
        public AdminArea(int administrativeAreaCode, string areaName, string countryCode, string regionCode)
        {
            this.administrativeAreaCode = administrativeAreaCode;
            this.areaName = areaName;
            this.countryCode = countryCode;
            this.regionCode = regionCode;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// NPTG Administrative area code
        /// </summary>
        public int AdministrativeAreaCode
        {
            get { return administrativeAreaCode; }
        }

        /// <summary>
        /// NPTG Area Name
        /// </summary>
        public string AreaName
        {
            get { return areaName; }
        }

        /// <summary>
        /// Country Code
        /// </summary>
        public string CountryCode
        {
            get { return countryCode; }
        }

        /// <summary>
        /// Region Code
        /// </summary>
        public string RegionCode
        {
            get { return regionCode; }
        }

        /// <summary>
        /// NPTG Area Name with Admin Code, Country Code, and Region Code
        /// </summary>
        public string AreaNameDebug
        {
            get { return string.Format("{0} - ad[{1}], cc[{2}], reg[{3}]", areaName, administrativeAreaCode, countryCode, regionCode); }
        }
        #endregion
    }
}
