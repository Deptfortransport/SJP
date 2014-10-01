// *********************************************** 
// NAME             : SiteRedirectHelper.cs      
// AUTHOR           : David Lane
// DATE CREATED     : 05 July 2012
// DESCRIPTION  	: SiteRedirectHelper class to redirect SJP Main to Mobile if appropriate
// ************************************************
// 
using System;
using System.Collections.Generic;
using Logger = System.Diagnostics.Trace;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.Events;
using SJP.UserPortal.ScreenFlow;
using SJP.UserPortal.SessionManager;

namespace SJP.Common.Web
{
    /// <summary>
    /// SiteRedirectHelper class to redirect SJP Main to Mobile if appropriate
    /// </summary>
    public class SiteRedirectHelper
    {
        #region local vars

        private static string regexB = string.Empty;
        private static string regexV = string.Empty;
        private static object synchLock = new object();
        private static bool initialised = false;
        
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Top level method to redirect Main to Mobile if appropriate
        /// </summary>
        /// <param name="request">the http request</param>
        /// <param name="response">the http response</param>
        /// <param name="context">the http context</param>
        /// <param name="session">the asp session</param>
        /// <param name="pageId">the page Id</param>
        public static void PerformSiteRedirect(HttpRequest request, HttpResponse response, HttpContext context, HttpSessionState session, PageId pageId)
        {
            StoreDoNotRedirectParameterInCookie(request);

            if (CheckForMobileDevice(request))
            {
                if (!DoNotRedirect(request))
                {
                    if (IsNewSession(context, session, request))
                    {
                        RedirectToMobile(request, response, pageId);
                    }
                    else if (IsTimedOutSession(context, session, request))
                    {
                        RedirectToMobile(request, response, pageId);
                    }
                }
            }
        }

        #endregion

        #region Private Static Methods
        /// <summary>
        /// Adds the do not redirect querystring parameter to the cookie
        /// </summary>
        /// <param name="request">the http request</param>
        private static void StoreDoNotRedirectParameterInCookie(HttpRequest request)
        {
            if (request.QueryString[QueryStringKey.DoNotRedirect] != null)
            {
                CookieHelper helper = new CookieHelper();
                bool flag = false;
                if (bool.TryParse(request.QueryString[QueryStringKey.DoNotRedirect], out flag))
                {
                    helper.UpdateDoNotRedirectToCookie(flag);
                }
            }
        }

        /// <summary>
        /// Checks to see if a redirect should be blocked
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>true/false</returns>
        private static bool DoNotRedirect(HttpRequest request)
        {
            if (request.QueryString[QueryStringKey.DoNotRedirect] == null)
            {
                // Check for cookie 
                CookieHelper cookieHelper = new CookieHelper();

                // defaults to false if not present
                return cookieHelper.RetrieveDoNotRedirect();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks the http user agent for signs of a mobile device
        /// </summary>
        /// <param name="request">the http request</param>
        /// <returns>true/false</returns>
        private static bool CheckForMobileDevice(HttpRequest request)
        {
            Initialise();

            // The rest of the code in this method is covered by the UNLICENSE agreement so is 
            // part of the public domain and not owned by the project or customer (including the regex strings)
            string userAgent =  request.ServerVariables["HTTP_USER_AGENT"];

            if (!string.IsNullOrEmpty(userAgent))
            {
                // Regex values come from http://detectmobilebrowsers.com version taken on 3/7/2012
                Regex b = new Regex(regexB, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(regexV, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if ((b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4))))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Perform the transfer to the mobile site, to the appropriate page and with querystring parameters if appropriate
        /// </summary>
        /// <param name="request">the http request</param>
        /// <param name="response">the http response</param>
        /// <param name="pageId">the page Id to redirect from</param>
        private static void RedirectToMobile(HttpRequest request, HttpResponse response, PageId pageId)
        {
            /* Redirection rules:
             * - if coming from main input with no page landing params go to mobile menu page
             * - if coming from main input or results page with page landing params fo to mobile input page with
             *      persisted params. default is PT mode or if param indicates cycle then cycle mode
             * - if coming from main travel news go to mobile travel news, persist any params
             * - all othe pages / scenarios go to mobile menu page with no persisted params
             * - all redirects will include the RedirectTo param (rt=m)
             */

            // Which mobile page we redirect to depends on the page we're on now
            PageId targetPageId = PageId.MobileDefault;

            // always include the RedirectedTo querystring parameter when redirecting (for google analytics)
            string outgoingQueryString = "?" + QueryStringKey.RedirectedTo + "=m";
            string incomingQueryString = string.Empty;

            if (request.ServerVariables["QUERY_STRING"] != null)
            {
                incomingQueryString = request.ServerVariables["QUERY_STRING"];
            }

            // for logic see top of method
            switch (pageId)
            {
                case PageId.JourneyPlannerInput:
                    if (incomingQueryString != string.Empty)
                    {
                        targetPageId = PageId.MobileInput;
                        outgoingQueryString += "&" + incomingQueryString;
                    }
                    break;
                case PageId.JourneyOptions:
                    if (incomingQueryString != string.Empty)
                    {
                        targetPageId = PageId.MobileSummary;
                        outgoingQueryString += "&" + incomingQueryString;
                    }
                    break;
                case PageId.TravelNews:
                    targetPageId = PageId.MobileTravelNews;
                    if (incomingQueryString != string.Empty)
                    {
                        outgoingQueryString += "&" + incomingQueryString;
                    }
                    break;
            }

            // Get the page transfer details
            string url = string.Empty;
            try
            {
                // Get transfer details
                IPageController pageController = SJPServiceDiscovery.Current.Get<IPageController>(ServiceDiscoveryKey.PageController);
                PageTransferDetail transferDetail = pageController.GetPageTransferDetails(targetPageId);
                url = transferDetail.PageUrl;
            }
            catch (Exception ex)
            {
                string message = string.Format("An error occurred retrieving PageTransferDetails for PageId[{0}]. Page has not been declared in the sitemap configuration.", pageId);
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, message, ex));

                // This is serious as a page/control is attempting to go to requested page and it hasn't been setup
                throw;
            }

            // Include query parameters
            url = url + outgoingQueryString;

            // Log RedirectMobile MIS event
            LogMisEvent();

            // Do the redirect
            response.Redirect(url);
        }

        /// <summary>
        /// Log a page entry event to signify a transfer is happening
        /// </summary>
        private static void LogMisEvent()
        {
            // Log page entry event (to signify a re-direct)
            PageEntryEvent logPage = new PageEntryEvent(PageId.RedirectMobile, SJPSessionManager.Current.Session.SessionID, false);
            Logger.Write(logPage);
        }

        /// <summary>
        /// Retrieves the regex patterns
        /// </summary>
        private static void Initialise()
        {
            if (!initialised)
            {
                lock (synchLock)
                {
                    regexB = Properties.Current["SiteRedirector.RegexB"];
                    regexV = Properties.Current["SiteRedirector.RegexV"];

                    initialised = true;
                }
            }
        }

        /// <summary>
        /// Check if this is a new session
        /// </summary>
        /// <param name="context">the http context</param>
        /// <param name="session">the asp session</param>
        /// <param name="request">the http request</param>
        /// <returns>true/false</returns>
        private static bool IsNewSession(HttpContext context, HttpSessionState session, HttpRequest request)
        {
            return SessionHelper.DetectNewSession(context, session, request);
        }

        /// <summary>
        /// Check if this is an active session
        /// </summary>
        /// <param name="context">the http context</param>
        /// <param name="session">the asp session</param>
        /// <param name="request">the http request</param>
        /// <returns>true/false</returns>
        private static bool IsActiveSession(HttpContext context, HttpSessionState session, HttpRequest request)
        {
            return SessionHelper.DetectActiveSession(context, session, request);
        }

        /// <summary>
        /// check if this is a timed-out session
        /// </summary>
        /// <param name="context">the http context</param>
        /// <param name="session">the asp session</param>
        /// <param name="request">the http request</param>
        /// <returns>true/false</returns>
        private static bool IsTimedOutSession(HttpContext context, HttpSessionState session, HttpRequest request)
        {
            return SessionHelper.DetectSessionTimeout(context, session, request);
        }
        
        #endregion
    }
}
