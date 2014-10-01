// *********************************************** 
// NAME             : SqlTypeHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: SqlTypeHelper class containing static helper methods
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DatabaseInfrastructure
{
    /// <summary>
    /// SqlTypeHelper class containing static helper methods
    /// </summary>
    public class SqlTypeHelper
    {
        #region Static members
        
        public static readonly DateTime MinDateTime = new DateTime(1753, 1, 1, 12, 0, 0);
        public static readonly DateTime MaxDateTime = new DateTime(9999, 12, 31, 23, 59, 59);

        #endregion

        #region Constructor

        /// <summary>
        /// Private constructor
        /// </summary>
        private SqlTypeHelper()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// IsSqlDateTimeCompatible checks if the provided datetime is in the supported 
        /// sql range
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsSqlDateTimeCompatible(DateTime dateTime)
        {
            if (dateTime >= MinDateTime && dateTime <= MaxDateTime)
                return true;
            else
                return false;
        }

        #endregion
    }
}
