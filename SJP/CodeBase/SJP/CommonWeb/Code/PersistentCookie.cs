// *********************************************** 
// NAME             : PersistentCookie.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Represents the persistent cookie that SJP will use
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SJP.Common.Web
{
    /// <summary>
    /// Allows access to the persistent cookie
    /// </summary>
    public static class PersistentCookie
    {
        #region Private Constants

        // Custom datetime pattern based on ISO 8601, to resolution of seconds
        private const string dateTimeFormat = "yyyyMMddTHHmmZ";

        private const string CookieName = "SJP";
        private const string KeyUniqueId = "UID";
        private const string KeyLastVisitedDateTime = "DT";
        private const string KeyLastPageVisited = "LP";

        private const string KeyCurrentLanguage = "L";
        private const string KeyCurrentFontSize = "FS";
        private const string KeyCurrentAccessibleStyle = "AS";
        private const string KeyCurrentSiteMode = "SM";

        private const string KeyJourneyOriginId = "O";
        private const string KeyJourneyOriginType = "TYO";
        private const string KeyJourneyOriginName = "ON";
        private const string KeyJourneyDestinationId = "D";
        private const string KeyJourneyDestinationType = "TYD";
        private const string KeyJourneyDestinationName = "DN";

        private const string KeyJourneyDateTimeOutward = "DTO";
        private const string KeyJourneyDateTimeReturn = "DTR";
        private const string KeyJourneyOutwardRequired = "RO";
        private const string KeyJourneyReturnRequired = "RR";

        private const string KeyJourneyAccessibleOption = "GN"; // GNAT accessible option
        private const string KeyJourneyFewerInterchanges = "FC"; // Fewer interchanges journey

        private const string KeyJourneyPlannerMode = "PM";

        private const string KeyDoNotRedirect = "DNR";
        #endregion

        #region Public Static Properties

        /// <summary>
        /// Returns true if the persistent cookie is available in the current request
        /// </summary>
        public static bool IsPersistentCookieAvailable
        {
            get
            {
                try
                {
                    // Check for the cookie in the current httpcontext
                    HttpContext context = HttpContextHelper.GetCurrent();

                    if ((context != null) && (context.Request.Cookies[CookieName] != null))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Expiry date of the cookie
        /// </summary>
        public static DateTime Expires
        {
            get { return Cookie.Expires; }
            set { Cookie.Expires = value; }
        }

        /// <summary>
        /// Gets or sets the Domain of the cookie
        /// </summary>
        public static string Domain
        {
            get { return Cookie.Domain; }
            set { Cookie.Domain = value; }
        }

        /// <summary>
        /// Gets the Name of the cookie. Returns string.Empty by default
        /// </summary>
        public static string Name
        {
            get { return Cookie.Name; }
        }

        #region Custom properties

        /// <summary>
        /// Gets or sets the unique id. Returns string.Empty by default
        /// </summary>
        public static string UniqueId
        {
            get { return GetCookieValueString(KeyUniqueId, string.Empty); }
            set { SetCookieValueString(KeyUniqueId, value); }
        }

        /// <summary>
        /// Gets or sets the last visited date time. Returns Now by default
        /// </summary>
        public static DateTime LastVisitedDateTime
        {
            get { return GetCookieValueDateTime(KeyLastVisitedDateTime, DateTime.Now.ToUniversalTime()); }
            set { SetCookieValueDateTime(KeyLastVisitedDateTime, value); }
        }

        /// <summary>
        /// Gets or sets the Last page visited. Returns string.Empty by default
        /// </summary>
        public static string LastPageVisited
        {
            get { return GetCookieValueString(KeyLastPageVisited, string.Empty); }
            set { SetCookieValueString(KeyLastPageVisited, value); }
        }

        /// <summary>
        /// Gets or sets the current language. Note that English will be returned
        /// by default if nothing can be found.
        /// </summary>
        public static Language CurrentLanguage
        {
            get { return GetCookieValueLanguage(KeyCurrentLanguage, Language.English); }
            set { SetCookieValueLanguage(KeyCurrentLanguage, value); }
        }

        /// <summary>
        /// Gets or sets the current font size. Note that Normal will be returned
        /// by default if nothing can be found.
        /// </summary>
        public static FontSize CurrentFontSize
        {
            get { return GetCookieValueFontSize(KeyCurrentFontSize, FontSize.Normal); }
            set { SetCookieValueFontSize(KeyCurrentFontSize, value); }
        }

        /// <summary>
        /// Gets or sets the current accessible style. Note that Normal will be returned
        /// by default if nothing can be found.
        /// </summary>
        public static AccessibleStyle CurrentAccessibleStyle
        {
            get { return GetCookieValueAccessibleStyle(KeyCurrentAccessibleStyle, AccessibleStyle.Normal); }
            set { SetCookieValueAccessibleStyle(KeyCurrentAccessibleStyle, value); }
        }

        /// <summary>
        /// Gets or sets the current site mode. Note that Olympics will be returned
        /// by default if nothing can be found.
        /// </summary>
        public static SiteMode CurrentSiteMode
        {
            get { return GetCookieValueSiteMode(KeyCurrentSiteMode, CurrentSite.SiteModeDefault); }
            set { SetCookieValueSiteMode(KeyCurrentSiteMode, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey origin id. Returns empty string by default
        /// </summary>
        public static string JourneyOriginId
        {
            get { return GetCookieValueString(KeyJourneyOriginId, string.Empty); }
            set { SetCookieValueString(KeyJourneyOriginId, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey origin type. Returns empty string by default
        /// </summary>
        public static string JourneyOriginType
        {
            get { return GetCookieValueString(KeyJourneyOriginType, string.Empty); }
            set { SetCookieValueString(KeyJourneyOriginType, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey origin name. Returns empty string by default
        /// </summary>
        public static string JourneyOriginName
        {
            get { return GetCookieValueString(KeyJourneyOriginName, string.Empty); }
            set { SetCookieValueString(KeyJourneyOriginName, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey destination id. Returns empty string by default
        /// </summary>
        public static string JourneyDestinationId
        {
            get { return GetCookieValueString(KeyJourneyDestinationId, string.Empty); }
            set { SetCookieValueString(KeyJourneyDestinationId, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey destination type. Returns empty string by default
        /// </summary>
        public static string JourneyDestinationType
        {
            get { return GetCookieValueString(KeyJourneyDestinationType, string.Empty); }
            set { SetCookieValueString(KeyJourneyDestinationType, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey destination type. Returns empty string by default
        /// </summary>
        public static string JourneyDestinationName
        {
            get { return GetCookieValueString(KeyJourneyDestinationName, string.Empty); }
            set { SetCookieValueString(KeyJourneyDestinationName, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey date time outward. Returns current date time by default
        /// </summary>
        public static DateTime JourneyDateTimeOutward
        {
            get { return GetCookieValueDateTime(KeyJourneyDateTimeOutward, DateTime.Now.ToUniversalTime()); }
            set { SetCookieValueDateTime(KeyJourneyDateTimeOutward, value); }
        }

        /// <summary>
        /// Gets or sets the users current journey date time return. Returns current date time by default
        /// </summary>
        public static DateTime JourneyDateTimeReturn
        {
            get { return GetCookieValueDateTime(KeyJourneyDateTimeReturn, DateTime.Now.ToUniversalTime()); }
            set { SetCookieValueDateTime(KeyJourneyDateTimeReturn, value); }
        }

        /// <summary>
        /// Gets or sets if the outward journy is required
        /// </summary>
        public static bool JourneyOutwardRequired
        {
            get { return GetCookieValueBool(KeyJourneyOutwardRequired, true); }
            set { SetCookieValueBool(KeyJourneyOutwardRequired, value); }
        }

        /// <summary>
        /// Gets or sets if the return journey is required
        /// </summary>
        public static bool JourneyReturnRequired
        {
            get { return GetCookieValueBool(KeyJourneyReturnRequired, false); }
            set { SetCookieValueBool(KeyJourneyReturnRequired, value); }
        }

        /// <summary>
        /// Gets or sets the users journey planner mode. Returns empty string by default
        /// </summary>
        public static string JourneyPlannerMode
        {
            get { return GetCookieValueString(KeyJourneyPlannerMode, string.Empty); }
            set { SetCookieValueString(KeyJourneyPlannerMode, value); }
        }

        /// <summary>
        /// Gets or sets the users journey accessible option. Returns empty string by default
        /// </summary>
        public static string JourneyAccessibleOption
        {
            get { return GetCookieValueString(KeyJourneyAccessibleOption, string.Empty); }
            set { SetCookieValueString(KeyJourneyAccessibleOption, value); }
        }

        /// <summary>
        /// Gets or sets the users journey fewer interchanges option. Returns false by default
        /// </summary>
        public static bool JourneyFewerInterchanges
        {
            get { return GetCookieValueBool(KeyJourneyFewerInterchanges, false); }
            set { SetCookieValueBool(KeyJourneyFewerInterchanges, value); }
        }

        /// <summary>
        /// Gets or sets the do not redirect to mobile option. Returns false by default
        /// </summary>
        public static bool DoNotRedirect
        {
            get { return GetCookieValueBool(KeyDoNotRedirect, false); }
            set { SetCookieValueBool(KeyDoNotRedirect, value); }
        }

        #endregion

        #endregion

        #region Private Static Methods

        #region Common get/set cookie methods

        /// <summary>
        /// Gets a string value from the cookie, or default if null
        /// </summary>
        private static string GetCookieValueString(string key, string defaultValue)
        {
            //Persist an instance of this cookie:
            HttpCookie cookie = Cookie;
            string value = cookie[key];

            if (value == null)
            {
                cookie[key] = defaultValue;
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Sets a string value to the cookie
        /// </summary>
        private static void SetCookieValueString(string key, string value)
        {
            //Persist an instance of this cookie:
            HttpCookie cookie = Cookie;

            cookie[key] = value;

            //Make sure the cookie is written back to the browser...
            Cookie = cookie;
        }

        /// <summary>
        /// Gets a bool value from the cookie, or default if null
        /// </summary>
        private static bool GetCookieValueBool(string key, bool defaultValue)
        {
            string value = GetCookieValueString(key, defaultValue.ToString());

            bool returnValue = false;

            if (Boolean.TryParse(value, out returnValue))
            {
                return returnValue;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets a bool value to the cookie
        /// </summary>
        private static void SetCookieValueBool(string key, bool value)
        {
            SetCookieValueString(key, value.ToString());
        }

        /// <summary>
        /// Gets an int value from the cookie, or default if null
        /// </summary>
        private static int GetCookieValueInt(string key, int defaultValue)
        {
            string value = GetCookieValueString(key, defaultValue.ToString());

            int returnValue;

            if (int.TryParse(value, out returnValue))
            {
                return returnValue;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets an int value to the cookie
        /// </summary>
        private static void SetCookieValueInt(string key, int value)
        {
            SetCookieValueString(key, value.ToString());
        }

        /// <summary>
        /// Gets a datetime value from the cookie, or default if null
        /// </summary>
        private static DateTime GetCookieValueDateTime(string key, DateTime defaultValue)
        {
            // Retrieves datetime from cookie and parses to LocalTime

            //Persist an instance of this cookie:
            HttpCookie cookie = Cookie;
            string value = cookie[key];

            if (string.IsNullOrEmpty(value))
            {
                cookie[key] = defaultValue.ToString(dateTimeFormat);
                DateTime dateTime = DateTime.ParseExact(cookie[key], dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                return dateTime.ToLocalTime();
            }
            else
            {
                DateTime dateTime = DateTime.ParseExact(value, dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                return dateTime.ToLocalTime();
            }
        }

        /// <summary>
        /// Sets a datetime value to the cookie
        /// </summary>
        private static void SetCookieValueDateTime(string key, DateTime value)
        {
            // Saves datetime to cookie as a UniversalTime

            //Persist an instance of this cookie:
            HttpCookie cookie = Cookie;

            cookie[key] = value.ToUniversalTime().ToString(dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);

            //Make sure the cookie is written back to the browser...
            Cookie = cookie;
        }

        #endregion

        #region Custom get/set cookie methods

        /// <summary>
        /// Gets a Language value from the cookie, or default if null
        /// </summary>
        private static Language GetCookieValueLanguage(string key, Language defaultValue)
        {
            string value = GetCookieValueString(key, defaultValue.ToString());

            if (Enum.IsDefined(typeof(Language), value))
            {
                return (Language)Enum.Parse(typeof(Language), value);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets a Language value to the cookie
        /// </summary>
        private static void SetCookieValueLanguage(string key, Language value)
        {
            SetCookieValueString(key, value.ToString());
        }

        /// <summary>
        /// Gets a FontSize value from the cookie, or default if null
        /// </summary>
        private static FontSize GetCookieValueFontSize(string key, FontSize defaultValue)
        {
            string value = GetCookieValueString(key, defaultValue.ToString());

            if (Enum.IsDefined(typeof(FontSize), value))
            {
                return (FontSize)Enum.Parse(typeof(FontSize), value);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets a FontSize value to the cookie
        /// </summary>
        private static void SetCookieValueFontSize(string key, FontSize value)
        {
            SetCookieValueString(key, value.ToString());
        }

        /// <summary>
        /// Gets a AccessibleStyle value from the cookie, or default if null
        /// </summary>
        private static AccessibleStyle GetCookieValueAccessibleStyle(string key, AccessibleStyle defaultValue)
        {
            string value = GetCookieValueString(key, defaultValue.ToString());

            if (Enum.IsDefined(typeof(AccessibleStyle), value))
            {
                return (AccessibleStyle)Enum.Parse(typeof(AccessibleStyle), value);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets a AccessibleStyle value to the cookie
        /// </summary>
        private static void SetCookieValueAccessibleStyle(string key, AccessibleStyle value)
        {
            SetCookieValueString(key, value.ToString());
        }

        /// <summary>
        /// Gets a SiteMode value from the cookie, or default if null
        /// </summary>
        private static SiteMode GetCookieValueSiteMode(string key, SiteMode defaultValue)
        {
            string value = GetCookieValueString(key, defaultValue.ToString());

            if (Enum.IsDefined(typeof(SiteMode), value))
            {
                return (SiteMode)Enum.Parse(typeof(SiteMode), value);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets a SiteMode value to the cookie
        /// </summary>
        private static void SetCookieValueSiteMode(string key, SiteMode value)
        {
            SetCookieValueString(key, value.ToString());
        }

        #endregion

        #endregion

        #region Private Static Properties
        /// <summary>
        /// Read/Write property to read/write data to persistent cookie
        /// </summary>
        private static HttpCookie Cookie
        {
            get
            {
                //Get the current context:
                HttpContext context = HttpContextHelper.GetCurrent();

                HttpCookie currentCookie = null;
                
                currentCookie = context.Request.Cookies[CookieName];
                

                //Now check for the cookie:
                if (currentCookie == null)
                {
                    HttpCookie newCookie = new HttpCookie(CookieName);
                    newCookie.Expires = DateTime.Now.AddMonths(6);
                    return newCookie;
                }
                else
                {
                    return currentCookie;
                }
            }
            set
            {
                //Get the current context:
                HttpContext context = HttpContextHelper.GetCurrent();
                context.Response.AppendCookie(value);
            }
        }

        #endregion
    }
}
