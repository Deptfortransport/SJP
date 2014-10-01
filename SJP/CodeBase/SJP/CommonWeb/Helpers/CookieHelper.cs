// *********************************************** 
// NAME             : CookieHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: CookieHelper class to detect and process cookies, raising RepeatVisitorEvents
// ************************************************
// 

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Reporting.Events;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.Common.Web
{
    /// <summary>
    /// CookieHelper class to detect and process cookies, raising RepeatVisitorEvents
    /// </summary>
    public class CookieHelper
    {
        #region Private members

        #region Constants

        // Name of the test cookie sent to the browser when checking for Cookie support
        private const string COOKIE_TESTCOOKIE = "SJPTestCookie";
                
        private const string Property_RepeatVistorSwitch = "Cookie.RepeatVisitor.Switch";
        private const string Property_UserAgentHeader = "Cookie.UserAgent.HeaderKey";
        private const string Property_UserAgentRobotPattern = "Cookie.UserAgent.Robot.Pattern";
        private const string Property_UserAgentRobotRegExp = "Cookie.UserAgent.Robot.RegularExpression";
        private const string Property_CookieExpiryTime = "Cookie.ExpiryTimeSpan.Seconds";

        // Name of the session parameter indicating cookie test is being done.
        // Needed because we assume cookies are supported and can only confirm on a subsequent user request
        //public static readonly StringKey CookieTest = new StringKey("CookieTest");

        // Name of the session parameter to hold the Cookies support field, used for new/repeatvisitor events
        //public static readonly StringKey CookieSupport = new StringKey("CookieSupport");

        // Name of the session parameter indicating this user should not have cookies sent to 
        // e.g. if is a "robot" user, and therefore don't want raise a new/repeatvisitor event
        //public static readonly StringKey CookieOverride = new StringKey("CookieOverride");

        #endregion

        private ISJPSession session = null;
        private PageId pageId = PageId.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CookieHelper()
        {
            this.session = SJPSessionManager.Current.Session;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pageId">PageId is used for Visitor event logging and updating the cookie's last page visited value</param>
        public CookieHelper(PageId pageId)
            : this()
        {
            this.pageId = pageId;
        }

        #endregion

        #region Public methods
                
        /// <summary>
        /// This method checks whether the user has indicated that Cookies are enabled. The default 
        /// setting is false. It places a test Cookie in the response and then checks to see if it is
        /// available in a subsequent request, deleting it if it is. This is then used as an indication 
        /// of the browser's Cookie support. 
        /// IsCookieSettingKnown is stored in the session and cannot be reset.
        /// </summary>
        public void DetectCookies()
        {
            // First page hit only, check if we should send cookie based on the requests User Agent
            if (session[SessionKey.CookieOverride] == null)
            {
                #region Allowed to add cookie test

                bool cookieOverride = true;

                // Check property to see if we want to detect repeat visitors,
                // By default we want to detect repeat visitors
                cookieOverride = Properties.Current[Property_RepeatVistorSwitch].Parse(true);

                if (cookieOverride)
                {
                    // Determie if we should place a cookie for this user
                    cookieOverride = IsPersistentCookieAllowed();

                    if (!cookieOverride)
                    {
                        // Raise a Robot visitor event
                        RepeatVisitorEvent rve = new RepeatVisitorEvent(RepeatVisitorType.VisitorRobot, string.Empty,
                            SJPSessionManager.Current.Session.SessionID, DateTime.Now, pageId.ToString(),
                            string.Empty, HttpContext.Current.Request.Headers[Properties.Current[Property_UserAgentHeader]]);

                        Logger.Write(rve);
                    }
                }

                session[SessionKey.CookieOverride] = cookieOverride.ToString();

                #endregion
            }

            // Only do cookie test if we're allowed to give this user a cookie
            if ((bool.Parse((string)session[SessionKey.CookieOverride])))
            {
                #region Cookie detection test

                // Check whether the test cookie has been returned
                if (HttpContext.Current.Request.Cookies[COOKIE_TESTCOOKIE] != null)
                {
                    // Remove the temporary cookie test value as it is no longer needed as we've established user allows cookies
                    session[SessionKey.CookieTest] = null; 

                    // Remove and then add in case the test cookie has persisted from a previous session
                    session[SessionKey.CookieSupport] = "true";

                    // And delete the test cookie as no longer needed
                    HttpContext.Current.Response.Cookies[COOKIE_TESTCOOKIE].Expires = DateTime.Now.AddDays(-1);
                }
                else if (session[SessionKey.CookieTest] != null)
                {
                    // As the temporary cookie test value is in the session, and we've come to this logic, 
                    // user does not support cookies
                    session[SessionKey.CookieTest] = null;
                    session[SessionKey.CookieSupport] = "false";
                }


                // If session doesnt have the cookie support flag, then must be first time user has accessed.
                // So place the Test cookie to be checked on subsequent page request.
                if (session[SessionKey.CookieSupport] == null)
                {
                    HttpCookie testCookie = new HttpCookie(COOKIE_TESTCOOKIE);
                    HttpContext.Current.Response.AppendCookie(testCookie);

                    // Add temporary value to session so it knows we're performing the cookie test on the next page request
                    session[SessionKey.CookieTest] = "In progress";

                    // Add the flag to session. By default, cookies are supported. 
                    // Needs to be added and true so we can raise new/repeat visitor event on first page hit
                    session[SessionKey.CookieSupport] = "true";

                    // Add flag to indicate visitor logging. By default, not logged.
                    // Needs to be added and true so we can raise new/repeat visitor event on first page hit
                    session[SessionKey.CookieVisitorLogged] = "false";
                }

                #endregion
            }
        }

        /// <summary>
        /// Method to check for persistent cookie and raise repeat visitor event report logs
        /// </summary>
        public void ProcessPersistentCookie()
        {
            bool cookiesEnabled = ((session[SessionKey.CookieSupport] != null) && (bool.Parse((string)session[SessionKey.CookieSupport])));
            bool cookieVisitorLogged = ((session[SessionKey.CookieVisitorLogged] != null) && (bool.Parse((string)session[SessionKey.CookieVisitorLogged])));

            // Do the processing
            if ((cookiesEnabled) && (PersistentCookie.IsPersistentCookieAvailable))
            {
                // If the cookie has the required information, then user has visited the site before (Repeat visitor)
                // otherwise its a new visitor!
                string cookieDomain = HttpContext.Current.Request.Url.Host; // Can't use the cookie value as it'll be null - not sure why it doesnt retain the value
                string cookieUniqueID = PersistentCookie.UniqueId;
                string cookieLastPageVisited = PersistentCookie.LastPageVisited;
                DateTime cookieLastVisitedDateTime = PersistentCookie.LastVisitedDateTime;

                string userAgentHeader = HttpContext.Current.Request.Headers[Properties.Current[Property_UserAgentHeader]];

                if ((string.IsNullOrEmpty(cookieUniqueID)) && (!cookieVisitorLogged))
                {
                    // Raise new visitor event (this is now done in the outer if below but left here just in case)
                    RepeatVisitorEvent rve = new RepeatVisitorEvent(RepeatVisitorType.VisitorNew, string.Empty,
                        SJPSessionManager.Current.Session.SessionID,
                        cookieLastVisitedDateTime, cookieLastPageVisited, cookieDomain, userAgentHeader);

                    Logger.Write(rve);

                    session[SessionKey.CookieVisitorLogged] = "true";
                }
                else if ((cookieUniqueID != SJPSessionManager.Current.Session.SessionID) && (!cookieVisitorLogged))
                {
                    // Raise repeat visitor event
                    RepeatVisitorEvent rve = new RepeatVisitorEvent(RepeatVisitorType.VisitorRepeat, cookieUniqueID,
                        SJPSessionManager.Current.Session.SessionID,
                        cookieLastVisitedDateTime, cookieLastPageVisited, cookieDomain, userAgentHeader);

                    Logger.Write(rve);

                    session[SessionKey.CookieVisitorLogged] = "true";
                }
                // No need to raise an event if visitor continues to use the site

                // Update the cookie with latest information
                UpdatePersistentCookie();
            }
            else if ((cookiesEnabled) && (!cookieVisitorLogged))
            {
                // No PersistentCookie for user, and they've not had a repeat visitor event logged, so it must
                // be a new visitor

                string cookieDomain = HttpContext.Current.Request.Url.Host;
                string userAgentHeader = HttpContext.Current.Request.Headers[Properties.Current[Property_UserAgentHeader]];

                // Raise new visitor event
                RepeatVisitorEvent rve = new RepeatVisitorEvent(RepeatVisitorType.VisitorNew, string.Empty,
                    SJPSessionManager.Current.Session.SessionID,
                    DateTime.Now, string.Empty, cookieDomain, userAgentHeader);

                Logger.Write(rve);

                session[SessionKey.CookieVisitorLogged] = "true";
            }
        }

        /// <summary>
        /// Updates any site preferences to cookie
        /// </summary>
        public void UpdateSitePreferencesToCookie()
        {
            bool cookiesEnabled = ((session[SessionKey.CookieSupport] != null) && (bool.Parse((string)session[SessionKey.CookieSupport])));

            if (cookiesEnabled)
            {
                PersistentCookie.CurrentSiteMode = CurrentSite.SiteModeValue;
            }
        }

        /// <summary>
        /// Updates the do not redirect flag to the cookie
        /// </summary>
        public void UpdateDoNotRedirectToCookie(bool flagValue)
        {
            bool cookiesEnabled = ((session[SessionKey.CookieSupport] != null) && (bool.Parse((string)session[SessionKey.CookieSupport])));

            if (cookiesEnabled)
            {
                PersistentCookie.DoNotRedirect = flagValue;
            }
        }

        /// <summary>
        /// Updates the cookie with journey request information
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        public void UpdateJourneyRequestToCookie(ISJPJourneyRequest sjpJourneyRequest)
        {
            bool cookiesEnabled = ((session[SessionKey.CookieSupport] != null) && (bool.Parse((string)session[SessionKey.CookieSupport])));

            if ((cookiesEnabled) && (sjpJourneyRequest != null))
            {
                // Outward location
                if (sjpJourneyRequest.Origin != null)
                {
                    PersistentCookie.JourneyOriginId = sjpJourneyRequest.Origin.ID;
                    PersistentCookie.JourneyOriginType = sjpJourneyRequest.Origin.TypeOfLocation.ToString();

                    // Only save name for coordinate locations
                    if (sjpJourneyRequest.Origin.TypeOfLocation == LocationService.SJPLocationType.CoordinateEN)
                    {
                        PersistentCookie.JourneyOriginName = sjpJourneyRequest.Origin.Name.SubstringFirst(30);
                    }
                }

                // Destination location
                if (sjpJourneyRequest.Destination != null)
                {
                    PersistentCookie.JourneyDestinationId = sjpJourneyRequest.Destination.ID;
                    PersistentCookie.JourneyDestinationType = sjpJourneyRequest.Destination.TypeOfLocation.ToString();

                    // Only save name for coordinate locations
                    if (sjpJourneyRequest.Destination.TypeOfLocation == LocationService.SJPLocationType.CoordinateEN)
                    {
                        PersistentCookie.JourneyDestinationName = sjpJourneyRequest.Destination.Name.SubstringFirst(30);
                    }
                }

                // Dates
                PersistentCookie.JourneyDateTimeOutward = sjpJourneyRequest.OutwardDateTime;
                PersistentCookie.JourneyDateTimeReturn = sjpJourneyRequest.ReturnDateTime;
                PersistentCookie.JourneyOutwardRequired = sjpJourneyRequest.IsOutwardRequired;
                PersistentCookie.JourneyReturnRequired = sjpJourneyRequest.IsReturnRequired;

                // Accessible option
                PersistentCookie.JourneyAccessibleOption = sjpJourneyRequest.AccessiblePreferences.GetAccessiblePreferenceString();
                PersistentCookie.JourneyFewerInterchanges = sjpJourneyRequest.AccessiblePreferences.RequireFewerInterchanges;

                // Planner mode
                PersistentCookie.JourneyPlannerMode = sjpJourneyRequest.PlannerMode.ToString();
            }
        }

        /// <summary>
        /// Uses values from the Cookie to return the do not redirect flag value, defaults to false
        /// </summary>
        /// <returns></returns>
        public bool RetrieveDoNotRedirect()
        {
            //bool cookiesEnabled = ((session[SessionKey.CookieSupport] != null) && (bool.Parse((string)session[SessionKey.CookieSupport])));

            //if (cookiesEnabled)
            //{
            //    return PersistentCookie.DoNotRedirect;
            //}
            //else
            //{
            //    return false;
            //}

            // Just try and get the value, if it fails then it's false
            bool dnr = PersistentCookie.DoNotRedirect;
            return dnr;
        }

        /// <summary>
        /// Uses values from the Cookie to return a populated SJPJourneyRequest.
        /// Returns null if cookie is not available 
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyRequest RetrieveJourneyRequestFromCookie()
        {
            ISJPJourneyRequest sjpJourneyRequest = null;

            // Load request from cookie if property switched on
            if (Properties.Current["Cookie.LoadJourneyRequest.Switch"].Parse(true))
            {

                bool cookiesEnabled = ((session[SessionKey.CookieSupport] != null) && (bool.Parse((string)session[SessionKey.CookieSupport])));

                if ((cookiesEnabled) && (PersistentCookie.IsPersistentCookieAvailable))
                {
                    // Read values from cookie
                    string originId = PersistentCookie.JourneyOriginId;

                    // Do a cursory check if origin value has been set, no point carrying on if it doesnt exist,
                    // e.g. If user has a cookie but never actually planned a journey (and therefore
                    // journey request values would never have been set)
                    if (!string.IsNullOrEmpty(originId))
                    {
                        string originType = PersistentCookie.JourneyOriginType;
                        string originName = PersistentCookie.JourneyOriginName;
                        string destinationId = PersistentCookie.JourneyDestinationId;
                        string destinationType = PersistentCookie.JourneyDestinationType;
                        string destinationName = PersistentCookie.JourneyDestinationName;

                        DateTime outwardDateTime = ValidateDateTime(PersistentCookie.JourneyDateTimeOutward);
                        DateTime returnDateTime = ValidateDateTime(PersistentCookie.JourneyDateTimeReturn);
                        bool outwardRequired = PersistentCookie.JourneyOutwardRequired;
                        bool returnRequired = PersistentCookie.JourneyReturnRequired;

                        string accessibleOption = PersistentCookie.JourneyAccessibleOption;
                        bool fewerInterchanges = PersistentCookie.JourneyFewerInterchanges;

                        string plannerMode = PersistentCookie.JourneyPlannerMode;

                        try
                        {
                            JourneyRequestHelper jrh = new JourneyRequestHelper();

                            sjpJourneyRequest = jrh.BuildSJPJourneyRequest(
                                originId, originType, originName,
                                destinationId, destinationType, destinationName,
                                outwardDateTime, returnDateTime, 
                                outwardRequired, returnRequired,
                                (!outwardRequired && returnRequired),
                                accessibleOption, fewerInterchanges, plannerMode);
                        }
                        catch (SJPException sjpEx)
                        {
                            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                                string.Format("Error building SJPJourneyRequest from Cookie values"), sjpEx));
                        }
                    }
                }
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Retrieves the cookie values as strings
        /// </summary>
        /// <returns></returns>
        public string GetCookieString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("isPersistentCookieAvailable[{0}] ", PersistentCookie.IsPersistentCookieAvailable));
            sb.AppendLine(string.Format("expires[{0}] ", PersistentCookie.Expires));
            sb.AppendLine(string.Format("domain[{0}] ", PersistentCookie.Domain));
            sb.AppendLine(string.Format("name[{0}] ", PersistentCookie.Name));

            sb.AppendLine(string.Format("uniqueId[{0}] ", PersistentCookie.UniqueId));
            sb.AppendLine(string.Format("lastVisitedDateTime[{0}] ", PersistentCookie.LastVisitedDateTime));
            sb.AppendLine(string.Format("lastPageVisited[{0}] ", PersistentCookie.LastPageVisited));
            sb.AppendLine(string.Format("currentLanguage[{0}] ", PersistentCookie.CurrentLanguage));
            sb.AppendLine(string.Format("currentFontSize[{0}] ", PersistentCookie.CurrentFontSize));
            sb.AppendLine(string.Format("currentAccessibleStyle[{0}] ", PersistentCookie.CurrentAccessibleStyle));

            sb.AppendLine(string.Format("originId[{0}] ", PersistentCookie.JourneyOriginId));
            sb.AppendLine(string.Format("originType[{0}] ", PersistentCookie.JourneyOriginType));
            sb.AppendLine(string.Format("destinationId[{0}] ", PersistentCookie.JourneyDestinationId));
            sb.AppendLine(string.Format("destinationType[{0}] ", PersistentCookie.JourneyDestinationType));
            sb.AppendLine(string.Format("outwardDateTime[{0}] ", PersistentCookie.JourneyDateTimeOutward));
            sb.AppendLine(string.Format("returnDateTime[{0}] ", PersistentCookie.JourneyDateTimeReturn));
            sb.AppendLine(string.Format("outwardRequired[{0}] ", PersistentCookie.JourneyOutwardRequired));
            sb.AppendLine(string.Format("returnRequired[{0}] ", PersistentCookie.JourneyReturnRequired));
            sb.AppendLine(string.Format("accessibleOption[{0}] ", PersistentCookie.JourneyAccessibleOption));
            sb.AppendLine(string.Format("fewerInterchanges[{0}] ", PersistentCookie.JourneyFewerInterchanges));
            sb.AppendLine(string.Format("plannerMode[{0}] ", PersistentCookie.JourneyPlannerMode));
            
            return sb.ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to check if the request subbmitted has a User Agent pattern that we want to give a Persistent Cookie.
        /// Gets patterns from properties to compare User Agent header against.
        /// </summary>
        /// <returns>Returns true if User Agent header does not contain any of the patterns</returns>
        private bool IsPersistentCookieAllowed()
        {
            // Get the property values
            string userAgentHeaderKey = Properties.Current[Property_UserAgentHeader];
            string userAgentPatternsProperty = Properties.Current[Property_UserAgentRobotPattern];
            string userAgentRegExpsProperty = Properties.Current[Property_UserAgentRobotRegExp];

            if ((!string.IsNullOrEmpty(userAgentHeaderKey)))
            {
                // Get the User Agent request value
                string userAgentHeader = HttpContext.Current.Request.Headers[userAgentHeaderKey];

                if (!string.IsNullOrEmpty(userAgentHeader))
                {
                    #region Check regular expression
                    if (!string.IsNullOrEmpty(userAgentRegExpsProperty))
                    {
                        // Get the patterns to test against
                        string[] userAgentRegExps = userAgentRegExpsProperty.Split(' ');

                        // Compare, if we match any reg exp then not allowed to send cookie to user
                        foreach (string userAgentRegExp in userAgentRegExps)
                        {
                            if (Regex.IsMatch(userAgentHeader, userAgentRegExp))
                                return false;
                        }
                    }
                    #endregion

                    #region Check patterns
                    if (!string.IsNullOrEmpty(userAgentPatternsProperty))
                    {
                        // Get the patterns to test against
                        string[] userAgentPatterns = userAgentPatternsProperty.Split(' ');

                        // Compare, if we match any pattern then not allowed to send cookie to user
                        foreach (string userAgentPattern in userAgentPatterns)
                        {
                            if (userAgentHeader.ToLower().Contains(userAgentPattern.ToLower()))
                                return false;
                        }
                    }
                    #endregion
                }
            }

            // Been through all the patterns and the user agent header does not match any, so ok to send cookie
            return true;
        }

        /// <summary>
        /// Method to add values to the Persistent cookie
        /// </summary>
        private void UpdatePersistentCookie()
        {
            PersistentCookie.UniqueId = SJPSessionManager.Current.Session.SessionID;
            PersistentCookie.LastVisitedDateTime = DateTime.Now.ToUniversalTime();
            PersistentCookie.LastPageVisited = this.pageId.ToString();

            #region Set the expiry date
            double expirySeconds = 0;
            int timeoutSeconds = 0;
            try
            {
                expirySeconds = Double.Parse(Properties.Current[Property_CookieExpiryTime]);
                timeoutSeconds = HttpContext.Current.Session.Timeout * 60;
            }
            catch
            {
                // Log an error
                OperationalEvent oe = new OperationalEvent(
                        SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                        "Error setting cookie expiry value: attempting to read property Cookie.ExpiryTimeSpan.Seconds, and Session.Timeout value");

                Logger.Write(oe);

                expirySeconds = (double)15778463; // Default the expiry to 6 months (15778463 seconds)
                timeoutSeconds = 1200; // Default session timeout to 20 minutes
            }

            // Set the expiry to be a property value + the session timeout value
            PersistentCookie.Expires = DateTime.Now.AddSeconds(expirySeconds + timeoutSeconds);

            #endregion
        }

        /// <summary>
        /// Checks the datetime provided is now or later, updateing the date and/or time parts to now if required
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DateTime ValidateDateTime(DateTime dateTime)
        {
            // Get Now ignoring seconds
            DateTime tmpNow = DateTime.Now;
            DateTime now = new DateTime(tmpNow.Year, tmpNow.Month, tmpNow.Day, tmpNow.Hour, tmpNow.Minute, 0);

            DateTime updated = dateTime;

            if (dateTime < now)
            {
                // Update change the date part only
                updated = new DateTime(now.Year, now.Month, now.Day, dateTime.Hour, dateTime.Minute, 0);

                // Check again to make sure the time part is also later than now
                if (updated < now)
                {
                    updated = now;
                }
            }

            return updated;
        }

        #endregion
    }
}