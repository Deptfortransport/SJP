// *********************************************** 
// NAME             : CurrentStyle.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Apr 2011
// DESCRIPTION  	: Interacts with the user's cookies to determine the current style for the site.
// ************************************************
// 

using System.Web;

namespace SJP.Common.Web
{
    /// <summary>
    /// Interacts with the user's cookies to determine the current language for the site.
    /// </summary>
    public static class CurrentStyle
    {
        #region Private Constants

        /// <summary>
        /// The name of the session keys to look at.
        /// </summary>
        private const string siteFontSizeKeyName = "SiteFontSize";
        private const string siteAccessibleStyleKeyName = "SiteAccessibleStyle";

        #endregion

        #region Private Fields

        private static readonly object fontSizeValueLock = new object();
        private static readonly object accessibleStyleValueLock = new object();

        #endregion

        #region Public Static Properties

        #region Font size

        /// <summary>
        /// Gets the default font size style for the application
        /// </summary>
        public static FontSize FontSizeDefault
        {
            get
            {
                return FontSize.Normal;
            }
        }

        /// <summary>
        /// Gets and sets the value of the current font size
        /// </summary>
        public static FontSize FontSizeValue
        {
            get
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                return GetFontSizeValue(context);
            }
            set
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                SetFontSizeValue(context, value);
            }
        }
                

        /// <summary>
        /// Gets the current font size style sheet name
        /// </summary>
        public static string FontSizeStyleSheet
        {
            get
            {
                switch (FontSizeValue)
                {
                    case FontSize.Medium:
                        return "font-medium.css";
                    case FontSize.Large:
                        return "font-large.css";
                    default:
                        return "font-small.css";
                }
            }
        }

        #endregion

        #region Accessible style

        /// <summary>
        /// Gets the default accessible style for the application
        /// </summary>
        public static AccessibleStyle AccessibleStyleDefault
        {
            get
            {
                return AccessibleStyle.Normal;
            }
        }

        /// <summary>
        /// Gets and sets the value of the current accessible style
        /// </summary>
        public static AccessibleStyle AccessibleStyleValue
        {
            get
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                return GetAccessibleStyleValue(context);
            }
            set
            {
                HttpContext context = HttpContextHelper.GetCurrent();
                SetAccessibleStyleValue(context, value);
            }
        }


        /// <summary>
        /// Gets the current accessible stylesheet name. 
        /// Returns string.empty for default style
        /// </summary>
        public static string AccessibleStyleSheet
        {
            get
            {
                switch (AccessibleStyleValue)
                {
                    case AccessibleStyle.Dyslexia:
                        return "dyslexia.css";
                    case AccessibleStyle.HighVis:
                        return "high-vis.css";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        #endregion

        #region Public Static Methods
        
        /// <summary>
        /// Parses the Font size and returns a FontSize value. Returns default for unrecognised value
        /// </summary>
        public static FontSize ParseFontSize(string fontSize)
        {
            switch (fontSize.ToLower().Trim())
            {
                case "l":
                    return FontSize.Large;
                case "m":
                    return FontSize.Medium;
                default:
                    return FontSize.Normal;
            }
        }

        /// <summary>
        /// Parses the Accessible style and returns a AccessibleStyle value. Returns default for unrecognised value
        /// </summary>
        public static AccessibleStyle ParseAccessibleStyle(string accessibleStyle)
        {
            switch (accessibleStyle.ToLower().Trim())
            {
                case "h":
                    return AccessibleStyle.HighVis;
                case "d":
                    return AccessibleStyle.Dyslexia;
                default:
                    return AccessibleStyle.Normal;
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Updates the current font size both in a session variable and in a cookie, so
        /// that the value can be read quickly in the same session, and can be recovered
        /// for future sessions, should the user have enabled cookies.
        /// </summary>
        private static void SetFontSizeValue(HttpContext context, FontSize value)
        {
            lock (fontSizeValueLock)
            {
                //First persist the value in a Cookie:
                PersistentCookie.CurrentFontSize = value;

                //Now persist in the session in case the user is not 
                //allowed to use Cookies:
                SetSessionValue(context, value);
            }
        }

        /// <summary>
        /// Reads the current font size from the session if possible. If not, (because 
        /// the user has just started a new session) the value is read from a cookie 
        /// if possible. If this is not possible, Normal is returned by default.
        /// </summary>
        /// <returns></returns>
        private static FontSize GetFontSizeValue(HttpContext context)
        {
            lock (fontSizeValueLock)
            {
                if (context.Session != null)
                {
                    //Get the value from the session if possible:
                    object sessionFontSize = context.Session[siteFontSizeKeyName];

                    if (sessionFontSize != null && sessionFontSize is FontSize)
                    {
                        return (FontSize)sessionFontSize;
                    }
                }

                //Could not get from session; now check from the cookie:
                //The cookie code will always return a value, even if
                //there is no cookie:
                FontSize value = PersistentCookie.CurrentFontSize;

                //Remember to store in the session before continuing...
                SetSessionValue(context, value);

                //Now return:
                return value;
            }
        }

        /// <summary>
        /// Updates the current accessible style both in a session variable and in a cookie, so
        /// that the value can be read quickly in the same session, and can be recovered
        /// for future sessions, should the user have enabled cookies.
        /// </summary>
        private static void SetAccessibleStyleValue(HttpContext context, AccessibleStyle value)
        {
            lock (accessibleStyleValueLock)
            {
                //First persist the value in a Cookie:
                PersistentCookie.CurrentAccessibleStyle = value;

                //Now persist in the session in case the user is not 
                //allowed to use Cookies:
                SetSessionValue(context, value);
            }
        }

        /// <summary>
        /// Reads the current accessible style from the session if possible. If not, (because 
        /// the user has just started a new session) the value is read from a cookie 
        /// if possible. If this is not possible, Normal is returned by default.
        /// </summary>
        /// <returns></returns>
        private static AccessibleStyle GetAccessibleStyleValue(HttpContext context)
        {
            lock (accessibleStyleValueLock)
            {
                if (context.Session != null)
                {
                    //Get the value from the session if possible:
                    object sessionAccessibleStyle = context.Session[siteAccessibleStyleKeyName];

                    if (sessionAccessibleStyle != null && sessionAccessibleStyle is AccessibleStyle)
                    {
                        return (AccessibleStyle)sessionAccessibleStyle;
                    }
                }

                //Could not get from session; now check from the cookie:
                //The cookie code will always return a value, even if
                //there is no cookie:
                AccessibleStyle value = PersistentCookie.CurrentAccessibleStyle;

                //Remember to store in the session before continuing...
                SetSessionValue(context, value);

                //Now return:
                return value;
            }
        }

        /// <summary>
        /// Sets the font size value in the session
        /// </summary>
        /// <param name="context"></param>
        private static void SetSessionValue(HttpContext context, FontSize value)
        {
            if (context.Session != null)
            {
                context.Session[siteFontSizeKeyName] = value;
            }
        }

        /// <summary>
        /// Sets the accessible style value in the session
        /// </summary>
        /// <param name="context"></param>
        private static void SetSessionValue(HttpContext context, AccessibleStyle value)
        {
            if (context.Session != null)
            {
                context.Session[siteAccessibleStyleKeyName] = value;
            }
        }

        #endregion
    }
}
