// *********************************************** 
// NAME             : SJPGNATLocation.cs      
// AUTHOR           : Mark Turner
// DATE CREATED     : 19 May 2011
// DESCRIPTION  	: Represents a GNAT location
// ************************************************


using System;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    ///  Represents a GNAT location
    /// </summary>
    [Serializable]
    public class SJPGNATLocation : SJPLocation
    {
        #region Private Fields

        private bool stepFreeAccess = false;
        private bool assistanceAvailable = false;
        private string operatorCode = string.Empty;
        private string countryCode = string.Empty;
        private SJPGNATLocationType gnatStopType = SJPGNATLocationType.Bus;
        
        #endregion

        #region Constructors
        /// <summary>
        /// SJPVenueLocation Constructor
        /// </summary>
        /// <param name="locationName"></param>
        /// <param name="locationDisplayName"></param>
        /// <param name="locationLocality"></param>
        /// <param name="toids"></param>
        /// <param name="naptans"></param>
        /// <param name="locationParent"></param>
        /// <param name="locationType"></param>
        /// <param name="gridReference"></param>
        /// <param name="cycleGridReference"></param>
        /// <param name="isGNATStation"></param>
        /// <param name="useNaptan"></param>
        /// <param name="sourceID"></param>
        /// <param name="countryCode"></param>
        /// <param name="adminAreaCode"></param>
        /// <param name="districtCode"></param>
        public SJPGNATLocation(string locationDisplayName, SJPLocationType locationType, string identifier, 
            bool stepFree, bool assistance, string operatorCode, 
            string countryCode, int adminAreaCode, int districtCode, SJPGNATLocationType gnatStopType)                              
            : base(locationDisplayName, locationType, locationType, identifier)
        {
            stepFreeAccess = stepFree;
            assistanceAvailable = assistance;
            this.operatorCode = operatorCode;
            this.countryCode = countryCode;
            this.gnatStopType = gnatStopType;
            
            AdminAreaCode = adminAreaCode;
            DistrictCode = districtCode;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Whether this location has Step Free Access
        /// </summary>
        public bool StepFreeAccess
        {
            get { return stepFreeAccess; }
            set { stepFreeAccess = value; }
        }

        /// <summary>
        /// Whether Assistance is available at this location
        /// </summary>
        public bool AssistanceAvailable
        {
            get { return assistanceAvailable; }
            set { assistanceAvailable = value; }
        }

        /// <summary>
        /// NPTG Operator code for the location
        /// </summary>
        public string OperatorCode
        {
            get { return operatorCode; }
            set { operatorCode = value; }
        }

        /// <summary>
        /// NPTG Country code for the location
        /// </summary>
        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
                
        /// <summary>
        /// TYPE of the stop this GNAT Location represents
        /// </summary>
        public SJPGNATLocationType GNATStopType
        {
            get { return gnatStopType; }
            set { gnatStopType = value; }
        }
        
        #endregion

        #region Protected methods override
        
        /// <summary>
        /// Returns a formatted string representing the contents of this SJPVenueLocation
        /// </summary>
        /// <returns></returns>
        public override string ToString(bool htmlLineBreaks)
        {
            string linebreak = (htmlLineBreaks) ? "<br />" : "\r\n";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(base.ToString(htmlLineBreaks));

            sb.AppendLine(string.Format("stepFreeAccess: {0} {1}", stepFreeAccess.ToString(), linebreak));
            sb.AppendLine(string.Format("assistanceAvailable: {0} {1}", assistanceAvailable.ToString(), linebreak));
            sb.AppendLine(string.Format("operatorCode: {0} {1}", operatorCode.ToString(), linebreak));
            sb.AppendLine(string.Format("countryCode: {0} {1}", countryCode.ToString(), linebreak));
            sb.AppendLine(string.Format("gnatStopType: {0} {1}", gnatStopType.ToString(), linebreak));
            
            return sb.ToString();
        }

        #endregion
    }
}
