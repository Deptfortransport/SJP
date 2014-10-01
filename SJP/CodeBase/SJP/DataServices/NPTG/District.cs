// *********************************************** 
// NAME             : District.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 Jun 2011
// DESCRIPTION  	: Represents District in NPTG data areas
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DataServices.NPTG
{
    /// <summary>
    /// Represents District in NPTG data areas
    /// </summary>
    public class District
    {
        #region Private Fields
        private int districtCode;
        private string districtName;
        private int administrativeAreaCode;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor - Represents District in NPTG data areas
        /// </summary>
        /// <param name="districtCode"></param>
        /// <param name="districtName"></param>
        /// <param name="administrativeAreaCode"></param>
        public District(int districtCode, string districtName, int administrativeAreaCode)
        {
            this.districtCode = districtCode;
            this.districtName = districtName;
            this.administrativeAreaCode = administrativeAreaCode;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// District Code
        /// </summary>
        public int DistrictCode
        {
            get { return districtCode; }
        }

        /// <summary>
        /// District Name
        /// </summary>
        public string DistrictName
        {
            get { return districtName; }
        }
        
        /// <summary>
        /// Administrative area code
        /// </summary>
        public int AdministrativeAreaCode
        {
            get { return administrativeAreaCode; }
        }

        /// <summary>
        /// District Name with District Code, and Admin Code
        /// </summary>
        public string DistrictNameDebug
        {
            get { return string.Format("{0} - d[{1}], ad[{2}]", districtName, districtCode, administrativeAreaCode); }
        }
        #endregion


    }
}
