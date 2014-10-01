// *********************************************** 
// NAME             : CurrentSite.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 11 Jun 2012
// DESCRIPTION  	: Interacts with the user's cookies to determine the current site mode
// ************************************************
// 

using System.Web;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using System;

namespace SJP.Common.Web
{
    /// <summary>
    /// Interacts with the user's cookies to determine the current site mode
    /// </summary>
    public static class CurrentSite
    {
        #region Private Constants

        /// <summary>
        /// The name of the session keys to look at.
        /// </summary>
        private const string siteModeKeyName = "SiteMode";

        #endregion

        #region Private Fields

        private static readonly object siteModeValueLock = new object();
        
        #endregion

        #region Public Static Properties
        
        /// <summary>
        /// Gets the default SiteMode for the application
        /// </summary>
        public static SiteMode SiteModeDefault
        {
            get
            {
                // Default site modeis Olympics, but this changes to Paralympics on configured date
                if (DateTime.Now < Properties.Current["Site.DefaultSiteMode.Switch.Date"].Parse(DateTime.MinValue))
                {
                    return SiteMode.Olympics;
                }
                else
                {
                    return SiteMode.Paralympics;
                }
            }
        }

        /// <summary>
        /// Gets (from session only) and sets (session and cookie) the value of the current SiteMode
        /// </summary>
        public static SiteMode SiteModeValue
        {
            get
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                return GetSiteModeValue(context, false);
            }
            set
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                SetSiteModeValue(context, value);
            }
        }

        /// <summary>
        /// Gets (from cookie only) the value of the current SiteMode
        /// </summary>
        public static SiteMode SiteModeValueTimeout
        {
            get
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                return GetSiteModeValue(context, true);
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Parses the site mode and returns a SiteMode value. Returns default for unrecognised value
        /// </summary>
        public static SiteMode ParseSiteMode(string siteMode)
        {
            switch (siteMode.ToLower().Trim())
            {
                case "o":
                    return SiteMode.Olympics;
                case "p":
                    return SiteMode.Paralympics;
                default:
                    return SiteModeDefault;
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Updates the current SiteMode both in a session variable and in a cookie, so
        /// that the value can be read quickly in the same session, and can be recovered
        /// for future sessions, should the user have enabled cookies.
        /// </summary>
        private static void SetSiteModeValue(HttpContext context, SiteMode value)
        {
            lock (siteModeValueLock)
            {
                //First persist the value in a Cookie:
                PersistentCookie.CurrentSiteMode = value;

                //Now persist in the session in case the user is not 
                //allowed to use Cookies:
                SetSessionValue(context, value);
            }
        }

        /// <summary>
        /// Reads the current SiteMode from the session if possible. If not, (because 
        /// the user has just started a new session) the value is read from a cookie 
        /// if possible. If this is not possible, default is returned.
        /// </summary>
        /// <returns></returns>
        private static SiteMode GetSiteModeValue(HttpContext context, bool useCookie)
        {
            lock (siteModeValueLock)
            {
                if (context.Session != null)
                {
                    //Get the value from the session if possible:
                    object sessionSiteMode = context.Session[siteModeKeyName];

                    if (sessionSiteMode != null && sessionSiteMode is SiteMode)
                    {
                        return (SiteMode)sessionSiteMode;
                    }
                }

                //Could not get from session; now check from the cookie:
                //The cookie code will always return a value, even if
                //there is no cookie:
                SiteMode value = SiteMode.Olympics;
                if (useCookie)
                    value = PersistentCookie.CurrentSiteMode;
                else
                    value = SiteModeDefault;

                //Remember to store in the session before continuing...
                SetSessionValue(context, value);

                //Now return:
                return value;
            }
        }
                
        /// <summary>
        /// Sets the SiteMode value in the session
        /// </summary>
        /// <param name="context"></param>
        private static void SetSessionValue(HttpContext context, SiteMode value)
        {
            if (context.Session != null)
            {
                context.Session[siteModeKeyName] = value;
            }
        }

        #endregion
    }
}
