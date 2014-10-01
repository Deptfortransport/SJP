// *********************************************** 
// NAME             : DebugHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 Oct 2011
// DESCRIPTION  	: DebugHelper class 
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;
using SJP.UserPortal.SessionManager;

namespace SJP.Common.Web
{
    /// <summary>
    /// DebugHelper class 
    /// </summary>
    public static class DebugHelper
    {
        private static bool showDebugInfo;

        /// <summary>
        /// Static constructor
        /// </summary>
        static DebugHelper()
        {
            showDebugInfo = Properties.Current["Debug.Information.Show.Switch"].Parse(false);
        }

        /// <summary>
        /// Returns the show debug information flag
        /// </summary>
        public static bool ShowDebug
        {
            get
            {
                // If property switch on and flag set in session (through landing page parameter!)
                if (showDebugInfo && SJPSessionManager.Current.Session[SessionKey.IsDebugMode])
                {
                    return true;
                }

                return false;
            }
        }
    }
}