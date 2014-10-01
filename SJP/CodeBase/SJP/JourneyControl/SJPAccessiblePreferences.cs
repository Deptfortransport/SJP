// *********************************************** 
// NAME             : SJPAccessiblePreferences.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: SJPAccessiblePreferences class containing options for accessible journey requests
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// SJPAccessiblePreferences class containing options for accessible journey requests
    /// </summary>
    [Serializable()]
    public class SJPAccessiblePreferences
    {
         #region Private members

        private bool doNotUseUnderground = false;
        private bool requireSpecialAssistance = false;
        private bool requireStepFreeAccess = false;
        private bool requireFewerInterchanges = false;
        private bool blueBadge = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SJPAccessiblePreferences()
        {
        }
        
        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Accessible
        /// </summary>
        public bool Accessible
        {
            get {
                // if any of the accessible condition is met the accessible preference is selected
                return DoNotUseUnderground || RequireSpecialAssistance || RequireStepFreeAccess || RequireFewerInterchanges || BlueBadge;
            }
        }

        /// <summary>
        /// Read/Write. DoNotUseUnderground
        /// </summary>
        public bool DoNotUseUnderground
        {
            get { return doNotUseUnderground; }
            set { doNotUseUnderground = value; }
        }

        /// <summary>
        /// Read/Write. RequireSpecialAssistance
        /// </summary>
        public bool RequireSpecialAssistance
        {
            get { return requireSpecialAssistance; }
            set { requireSpecialAssistance = value; }
        }

        /// <summary>
        /// Read/Write. RequireStepFreeAccess
        /// </summary>
        public bool RequireStepFreeAccess
        {
            get { return requireStepFreeAccess; }
            set { requireStepFreeAccess = value; }
        }

        /// <summary>
        /// Read/Write. RequireFewerInterchanges
        /// </summary>
        public bool RequireFewerInterchanges
        {
            get { return requireFewerInterchanges; }
            set { requireFewerInterchanges = value; }
        }

        /// <summary>
        /// Read/Write. Blue Badge
        /// </summary>
        public bool BlueBadge
        {
            get { return blueBadge; }
            set { blueBadge = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Populates this SJPAccessiblePreferences object using the accessible option string provided (ignores case).
        /// Values can be "u" not underground, "a" assistance, "w" wheelchair, "wa" wheelchair and assistance, "b" blue badge
        /// </summary>
        /// <param name="accessibleOption"></param>
        public void PopulateAccessiblePreference(string accessibleOption)
        {
            if (!string.IsNullOrEmpty(accessibleOption))
            {
                string accessibleOptions = accessibleOption.ToUpper().Trim();

                switch (accessibleOptions)
                {
                    case "U":
                        doNotUseUnderground = true;
                        break;
                    case "A":
                        requireSpecialAssistance = true;
                        break;
                    case "W":
                        requireStepFreeAccess = true;
                        break;
                    case "WA":
                        requireStepFreeAccess = true;
                        requireSpecialAssistance = true;
                        break;
                    case "B":
                        blueBadge = true;
                        break;
                    default:
                        doNotUseUnderground = false;
                        requireStepFreeAccess = false;
                        requireSpecialAssistance = false;
                        break;
                }
            }
        }
        
        /// <summary>
        /// Retrieves a string value representing the accessible option of this SJPAccessiblePreferences object.
        /// Default is string.Empty if no accessible preference set
        /// </summary>
        /// <returns></returns>
        public string GetAccessiblePreferenceString()
        {
            if (requireStepFreeAccess && requireSpecialAssistance)
                return "WA";
            else if (requireSpecialAssistance)
                return "A";
            else if (requireStepFreeAccess)
                return "W";
            else if (doNotUseUnderground)
                return "U";
            else if (blueBadge)
                return "B";

            return string.Empty;
        }

        /// <summary>
        /// Returns a hash code of this SJPAccessiblePreferences. 
        /// </summary>
        /// <returns></returns>
        public int GetSJPHashCode()
        {
            // Does not use native GetHashCode as this can return different hash codes 
            // for instances of the "same" object.

            // string, int, etc return the same hashcode if they have the same value
            // Do + here because of the nature of true/false hashcodes and bitwise exclusive ^
            int hashCode = 0;

            if (Accessible) 
                hashCode += 1;
            if (doNotUseUnderground)
                hashCode += 2;
            if (requireSpecialAssistance)
                hashCode += 4;
            if (requireStepFreeAccess)
                hashCode += 8;
            if (requireFewerInterchanges)
                hashCode += 16;
            if (blueBadge)
                hashCode += 32;

            return hashCode;
        }

        /// <summary>
        /// Returns a deep clone of this SJPAccessiblePreferences
        /// </summary>
        /// <returns></returns>
        public SJPAccessiblePreferences Clone()
        {
            SJPAccessiblePreferences target = new SJPAccessiblePreferences();

            target.DoNotUseUnderground = this.doNotUseUnderground;
            target.requireSpecialAssistance = this.requireSpecialAssistance;
            target.requireStepFreeAccess = this.requireStepFreeAccess;
            target.requireFewerInterchanges = this.requireFewerInterchanges;
            target.blueBadge = this.blueBadge;

            return target;
        }

        #endregion
    }
}
