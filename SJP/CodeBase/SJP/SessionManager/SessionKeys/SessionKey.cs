// *********************************************** 
// NAME             : SessionKey.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: SessionKey class containing definition of keys used in Session class
// ************************************************
// 

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// Global session key repository where each key must be defined 
    /// prior to being used by the Session class.
    /// </summary>
    public class SessionKey
    {
        // Key Definitions
        // To add new key definitions, follow the example format:
        //      public static readonly BoolKey KeyResetJourney = new BoolKey("ResetJourney");

        public static readonly PageIdKey NextPageId = new PageIdKey("NextPageId");
        public static readonly BoolKey Transferred = new BoolKey("Transferred");

        // Used for persistent cookie support
        public static readonly StringKey CookieTest = new StringKey("CookieTest");
        public static readonly StringKey CookieSupport = new StringKey("CookieSupport");
        public static readonly StringKey CookieOverride = new StringKey("CookieOverride");
        public static readonly StringKey CookieVisitorLogged = new StringKey("CookieVisitorLogged");

        // Used for recovery handling
        public static readonly BoolKey IsSessionInitialised = new BoolKey("IsSessionInitialised");
        public static readonly BoolKey IsSessionTimeout = new BoolKey("IsSessionTimeout");
        public static readonly BoolKey IsErrorPage = new BoolKey("IsErrorPage");

        // Used for landing page
        public static readonly BoolKey IsLandingPage = new BoolKey("IsLandingPage");
        public static readonly BoolKey IsLandingPageAutoPlan = new BoolKey("IsLandingPageAutoPlan");

        // Used to show navigation (headers and footers)
        public static readonly BoolKey IsNavigationNotRequired = new BoolKey("IsNavigationNotRequired");

        // Used to allow debug information to be shown on site
        public static readonly BoolKey IsDebugMode = new BoolKey("IsDebugMode");
    }
}
