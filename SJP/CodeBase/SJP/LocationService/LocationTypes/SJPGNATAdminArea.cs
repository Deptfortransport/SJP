// *********************************************** 
// NAME             : SJPGNATAdminArea.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Nov 2011
// DESCRIPTION  	: Represents an SJP GNAT Administrative Area, 
//                    indicating if the admin area (and district) have accessible flags
// ************************************************

using System;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Represents an SJP GNAT Administrative Area
    /// </summary>
    [Serializable()]
    public class SJPGNATAdminArea
    {
        #region Private Fields
        
        private int administrativeAreaCode = -1;
        private int districtCode = -1;
        private bool stepFreeAccess = false;
        private bool assistanceAvailable = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPGNATAdminArea()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPGNATAdminArea(int administrativeAreaCode, int districtCode, bool stepFreeAccess, bool assistanceAvailable)
        {
            this.administrativeAreaCode = administrativeAreaCode;
            this.districtCode = districtCode;
            this.stepFreeAccess = stepFreeAccess;
            this.assistanceAvailable = assistanceAvailable;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Administrative Area Code
        /// </summary>
        public int AdministrativeAreaCode
        {
            get { return administrativeAreaCode; }
        }

        /// <summary>
        /// District Code
        /// </summary>
        public int DistrictCode
        {
            get { return districtCode; }
        }

        /// <summary>
        /// Whether Step Free Access is available for this Admin Area (and District)
        /// </summary>
        public bool StepFreeAccess
        {
            get { return stepFreeAccess; }
        }

        /// <summary>
        /// Whether Assistance is available for this Admin Area (and District)
        /// </summary>
        public bool AssistanceAvailable
        {
            get { return assistanceAvailable; }
        }

        #endregion
    }
}
