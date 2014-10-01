// *********************************************** 
// NAME             : CurrentLanguage.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Interacts with the user's cookies to determine the current language for the site.
// ************************************************


using System.Globalization;
using System.Web;

namespace SJP.Common.Web
{
    /// <summary>
    /// Interacts with the user's cookies to determine the current language for the site.
    /// </summary>
    public static class CurrentLanguage
    {
        #region Private Constants

        /// <summary>
        /// The name of the session key to look at.
        /// </summary>
        private const string siteLanguageKeyName = "SiteLanguage";

        #endregion

        #region Private Fields

        private static readonly object valueLock = new object();

        #endregion

        #region Public Static Properties
        
        /// <summary>
        /// Gets and sets the value of the current language
        /// </summary>
        public static Language Value
        {
            get
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                return GetValue(context);
            }
            set
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                SetValue(context, value);
            }
        }

        /// <summary>
        /// Gets the CultureInfo based on the current language
        /// </summary>
        public static CultureInfo CurrentCultureInfo
        {
            get
            {
                switch (Value)
                {
                    case Language.French:
                        return new CultureInfo("fr-FR");
                    case Language.English:
                    default:
                        return new CultureInfo("en-GB");
                }
            }
        }

        /// <summary>
        /// Gets the current culture
        /// </summary>
        public static string Culture
        {
            get
            {
                switch (Value)
                {
                    case Language.French:
                        return LanguageHelper.GetLanguageString(Language.French);
                    case Language.English:
                    default:
                        return LanguageHelper.GetLanguageString(Language.English);
                }
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Updates the current language both in a session variable and in a cookie, so
        /// that the value can be read quickly in the same session, and can be recovered
        /// for future sessions, should the user have enabled cookies.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        private static void SetValue(HttpContext context, Language value)
        {
            lock (valueLock)
            {
                //First persist the value in a Cookie:
                PersistentCookie.CurrentLanguage = value;

                //Now persist in the session in case the user is not 
                //allowed to use Cookies:
                SetSessionValue(context, value);
            }
        }

        /// <summary>
        /// Reads the current language from the session if possible. If not, (because 
        /// the user has just started a new session) the value is read from a cookie 
        /// if possible. If this is not possible, english is returned by default.
        /// </summary>
        /// <returns></returns>
        private static Language GetValue(HttpContext context)
        {
            lock (valueLock)
            {
                if (context.Session != null)
                {
                    //Get the value from the session if possible:
                    object sessionLanguage = context.Session[siteLanguageKeyName];

                    if (sessionLanguage != null && sessionLanguage is Language)
                    {
                        return (Language)sessionLanguage;
                    }
                }

                //Could not get from session; now check from the cookie:
                //The cookie code will always return a value, even if
                //there is no cookie:
                Language value = PersistentCookie.CurrentLanguage;

                //Remember to store in the session before continuing...
                SetSessionValue(context, value);

                //Now return:
                return value;
            }
        }

        /// <summary>
        /// Sets the language value in the session
        /// </summary>
        /// <param name="context"></param>
        private static void SetSessionValue(HttpContext context, Language value)
        {
            if (context.Session != null)
            {
                context.Session[siteLanguageKeyName] = value;
            }
        }

        #endregion
    }
}
