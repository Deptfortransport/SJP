using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.Web
{
    public class FormattedCycleJourneyDetail : FormattedJourneyDetail
    {
        #region Private Fields
        private string pathName = string.Empty;
        private string pathImageUrl = string.Empty;
        private string pathImageText = string.Empty;
        private string cycleInstruction = string.Empty;
        private string cycleAttributes = string.Empty;
        private string manoeuvreImage = string.Empty;
        private string manoeuvreImageText = string.Empty;
        #endregion

        #region Public Properties
        /// <summary>
        /// Cycle Path Name
        /// </summary>
        public string PathName 
        {
            get { return pathName; }
            set { pathName = value; }
        }

        /// <summary>
        /// Cycle Path image 
        /// </summary>
        public string PathImageUrl
        {
            get { return pathImageUrl; }
            set { pathImageUrl = value; }
        }

        /// <summary>
        /// Cycle Path Image alternate text
        /// </summary>
        public string PathImageText
        {
            get { return pathImageText; }
            set { pathImageText = value; }
        }

        /// <summary>
        /// Get/Set complex manoeuvre image path
        /// </summary>
        public string ManoeuvreImage
        {
            get { return manoeuvreImage; }
            set { manoeuvreImage = value; }
        }

        /// <summary>
        /// Get/Set complex manoeuvre image text
        /// </summary>
        public string ManoeuvreImageText
        {
            get { return manoeuvreImageText; }
            set { manoeuvreImageText = value; }
        }

        /// <summary>
        /// Specific instruction related to cycle journey
        /// </summary>
        public string CycleInstruction 
        {
            get { return cycleInstruction; }
            set { cycleInstruction = value; } 
        }

        /// <summary>
        /// Attribute text associated with the cycle journey
        /// </summary>
        public string CycleAttributes 
        {
            get { return cycleAttributes; }
            set { cycleAttributes = value; }
        }
        #endregion
    }
}
