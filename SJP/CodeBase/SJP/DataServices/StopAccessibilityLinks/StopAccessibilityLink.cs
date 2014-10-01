// *********************************************** 
// NAME             : StopAccessibilityLink.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 June 2011
// DESCRIPTION  	: StopAccessibilityLink class
// ************************************************
// 
                
using System;

namespace SJP.Common.DataServices.StopAccessibilityLinks
{
    /// <summary>
    /// StopAccessibilityLink class
    /// </summary>
    [Serializable]
    public class StopAccessibilityLink
    {
        #region Private members

        private string stopNaPTAN;
        private string stopOperator;
        private string stopAccessibilityURL;
        private DateTime dateWEF;
        private DateTime dateWEU;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public StopAccessibilityLink()
        {
            this.stopNaPTAN = string.Empty;
            this.stopOperator = string.Empty;
            this.stopAccessibilityURL = string.Empty;
            this.dateWEF = DateTime.MinValue;
            this.dateWEU = DateTime.MaxValue;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StopAccessibilityLink(string stopNaPTAN, string stopOperator, string stopAccessibilityURL, DateTime dateWEF, DateTime dateWEU)
        {
            this.stopNaPTAN = stopNaPTAN;
            this.stopOperator = stopOperator;
            this.stopAccessibilityURL = stopAccessibilityURL;
            this.dateWEF = dateWEF;
            this.dateWEU = dateWEU;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Read only. NaPTAN
        /// </summary>
        public string StopNaPTAN
        {
            get { return stopNaPTAN; }
        }

        /// <summary>
        /// Read only. Operator code
        /// </summary>
        public string StopOperator
        {
            get { return stopOperator; }
        }

        /// <summary>
        /// Read only. URL
        /// </summary>
        public string StopAccessibilityURL
        {
            get { return stopAccessibilityURL; }
        }

        /// <summary>
        /// Read only. With Effect From date
        /// </summary>
        public DateTime DateWEF
        {
            get { return dateWEF; }
        }

        /// <summary>
        /// Read only. With Effect Until date
        /// </summary>
        public DateTime DateWEU
        {
            get { return dateWEU; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns true if this StopAccessibilityLink if valid for the date specified, 
        /// or if date is DateTime.MinValue, then returns true
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsValidForDate(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return true;
            }
            else
            {
                return ((date.Date >= dateWEF.Date) && (date.Date <= dateWEU.Date));
            }
        }

        #endregion
    }
}
