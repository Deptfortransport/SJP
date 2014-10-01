// *********************************************** 
// NAME             : SessionHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 13 Apr 2011
// DESCRIPTION  	: SessionHelper class to provide helper methods for updating the session
// ************************************************
// 

using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;

namespace SJP.Common.Web
{
    /// <summary>
    /// SessionHelper class to provide helper methods for updating the session
    /// </summary>
    public class SessionHelper
    {
        #region Private Fields

        private ISJPSessionManager sessionManager;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionHelper()
        {
            sessionManager = (SJPSessionManager)SJPSessionManager.Current;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Detects if the session has timed-out.
        /// </summary>
        /// <param name="context">the http context</param>
        /// <param name="session">the asp session</param>
        /// <param name="request">the http request</param>
        /// <returns>true/false</returns>
        public static bool DetectSessionTimeout(HttpContext context, HttpSessionState session, HttpRequest request)
        {
            // The Request and Response appear to both share the 
            // same cookie collection.  If a cookie is set in the Reponse, it is 
            // also immediately visible to the Request collection.  This just means that 
            // since the ASP.Net_SessionID is set in the Session HTTPModule (which 
            // has already run), that we can't use our own code to see if the cookie was 
            // actually sent by the agent with the request using the collection. Check if 
            // the given page supports session or not (this tested as reliable indicator 
            // if EnableSessionState is true), should not care about a page that does 
            // not need session
            if (context.Session != null)
            {
                // The IsNewSession is more advanced then simply checking if 
                // a cookie is present, it does take into account a session timeout
                if (session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must 
                    // have timed out (can't use the cookie collection because even on first 
                    // request it already contains the cookie (request and response
                    // seem to share the collection))
                    string cookieHeader = request.Headers["Cookie"];
                    if ((null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        // Is a session timeout
                        return true;
                    }
                }
            }

            // Not a session timeout, it is a new or returning user
            return false;
        }

        /// <summary>
        /// Check if this is a new session
        /// </summary>
        /// <param name="context">the http context</param>
        /// <param name="session">the asp session</param>
        /// <param name="request">the http request</param>
        /// <returns>true/false</returns>
        public static bool DetectNewSession(HttpContext context, HttpSessionState session, HttpRequest request)
        {
            // The Request and Response appear to both share the 
            // same cookie collection.  If a cookie is set in the Reponse, it is 
            // also immediately visible to the Request collection.  This just means that 
            // since the ASP.Net_SessionID is set in the Session HTTPModule (which 
            // has already run), that we can't use our own code to see if the cookie was 
            // actually sent by the agent with the request using the collection. Check if 
            // the given page supports session or not (this tested as reliable indicator 
            // if EnableSessionState is true), should not care about a page that does 
            // not need session
            if (context.Session != null)
            {
                // The IsNewSession is more advanced then simply checking if 
                // a cookie is present, it does take into account a session timeout
                if (session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must 
                    // have timed out (can't use the cookie collection because even on first 
                    // request it already contains the cookie (request and response
                    // seem to share the collection))
                    string cookieHeader = request.Headers["Cookie"];
                    if ((null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        // Is a session timeout
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // A new user
                return true;
            }
        }

        /// <summary>
        /// Check if this is an active session
        /// </summary>
        /// <param name="context">the http context</param>
        /// <param name="session">the asp session</param>
        /// <param name="request">the http request</param>
        /// <returns>true/false</returns>
        public static bool DetectActiveSession(HttpContext context, HttpSessionState session, HttpRequest request)
        {
            // The Request and Response appear to both share the 
            // same cookie collection.  If a cookie is set in the Reponse, it is 
            // also immediately visible to the Request collection.  This just means that 
            // since the ASP.Net_SessionID is set in the Session HTTPModule (which 
            // has already run), that we can't use our own code to see if the cookie was 
            // actually sent by the agent with the request using the collection. Check if 
            // the given page supports session or not (this tested as reliable indicator 
            // if EnableSessionState is true), should not care about a page that does 
            // not need session
            if (context.Session != null)
            {
                // The IsNewSession is more advanced then simply checking if 
                // a cookie is present, it does take into account a session timeout
                if (session.IsNewSession)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                // A new user
                return false;
            }
        }

        /// <summary>
        /// Retrieves the SJPJourneyRequest from session, using the 
        /// JourneyRequestHash in the current InputPageState. Returns null if not found
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyRequest GetSJPJourneyRequest()
        {
            ISJPJourneyRequest sjpJourneyRequest = null;

            InputPageState pageState = sessionManager.PageState;

            if (!string.IsNullOrEmpty(pageState.JourneyRequestHash))
            {
                string requestHash = pageState.JourneyRequestHash;

                sjpJourneyRequest = sessionManager.RequestManager.GetSJPJourneyRequest(requestHash);
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Retrieves the SJPJourneyRequest from session, using the 
        /// JourneyRequestHash provided. Returns null if not found
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyRequest GetSJPJourneyRequest(string journeyRequestHash)
        {
            ISJPJourneyRequest sjpJourneyRequest = null;

            if (!string.IsNullOrEmpty(journeyRequestHash))
            {
                sjpJourneyRequest = sessionManager.RequestManager.GetSJPJourneyRequest(journeyRequestHash);
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Retrieves the SJPJourneyResult from session, using the 
        /// JourneyRequestHash provided. Returns null if not found
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyResult GetSJPJourneyResult(string journeyRequestHash)
        {
            ISJPJourneyResult sjpJourneyResult = null;

            if (!string.IsNullOrEmpty(journeyRequestHash))
            {
                sjpJourneyResult = sessionManager.ResultManager.GetSJPJourneyResult(journeyRequestHash);
            }

            return sjpJourneyResult;
        }

        /// <summary>
        /// Checks the session if an SJPJourneyResult exists for the supplied journeyRequestHash,
        /// and the result contains journeys
        /// </summary>
        /// <param name="journeyRequestHash"></param>
        /// <returns></returns>
        public bool DoesSJPJourneyResultExist(string journeyRequestHash, bool removeInvalidResult)
        {
            bool resultExists = false;

            if (!string.IsNullOrEmpty(journeyRequestHash))
            {
                // Check for journey result
                resultExists = sessionManager.ResultManager.DoesResultExist(journeyRequestHash);

                // Check journeys exist in the result, otherwise
                if (resultExists)
                {
                    ISJPJourneyRequest journeyRequest = sessionManager.RequestManager.GetSJPJourneyRequest(journeyRequestHash);
                    ISJPJourneyResult journeyResult = sessionManager.ResultManager.GetSJPJourneyResult(journeyRequestHash);

                    // Journeys must exist, or if a message for journey exists, then some part must have failed.
                    // Or if its Venue-to-Venue, then remove because of a bug when "switching locations from/to",
                    // the request equates to the same journey request hash code, which therefore incorrectly states 
                    // the result exists but it could be for the "previous" request (better to be safe so remove!)
                    if ((journeyResult.OutwardJourneys.Count + journeyResult.ReturnJourneys.Count == 0)
                        || (journeyResult.Messages.Count > 0)
                        || (journeyRequest.LocationInputMode == LocationInputMode.VenueToVenue.ToString()))
                    {
                        // No journeys, so it probably failed last time it was planned
                        resultExists = false;

                        // Remove the "bad" result
                        if (removeInvalidResult)
                        {
                            sessionManager.ResultManager.RemoveSJPJourneyResult(journeyRequestHash);
                        }
                    }
                }
            }

            return resultExists;
        }

        /// <summary>
        /// Updates the sesssion with the SJPJourneyRequest, 
        /// and also sets the session PageState to indicate current SJPJourneyRequest to use
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        public void UpdateSession(ISJPJourneyRequest sjpJourneyRequest)
        {
            // Add journey request into the session
            sessionManager.RequestManager.AddSJPJourneyRequest(sjpJourneyRequest);

            // Add to page state so current/next page knows which request it "should" be working with.
            // Request identifier is added to URL to also indicate which request to use, so this value 
            // provides a fallback incase it is missing, helps in certain scenarios (e.g. in tabbed browsing)
            sessionManager.PageState.JourneyRequestHash = sjpJourneyRequest.JourneyRequestHash;

            // Reset selected journeys
            sessionManager.PageState.JourneyIdOutward = -1;
            sessionManager.PageState.JourneyIdReturn = -1;

            // Reset leg detail expanded flags
            sessionManager.PageState.JourneyLegDetailExpandedOutward = false;
            sessionManager.PageState.JourneyLegDetailExpandedReturn = false;
        }

        /// <summary>
        /// Updates the sesssion with the journey request hash, 
        /// and also sets the session PageState to indicate current SJPJourneyRequest to use
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        public void UpdateSession(string journeyRequestHash)
        {
            // Add to page state so current/next page knows which request it "should" be working with.
            // Request identifier is added to URL to also indicate which request to use, so this value 
            // provides a fallback incase it is missing, helps in certain scenarios (e.g. in tabbed browsing)
            sessionManager.PageState.JourneyRequestHash = journeyRequestHash;
        }

        #region Stop Event Methods

        /// <summary>
        /// Retrieves the Stop Event SJPJourneyRequest from session, using the 
        /// request hash in the current InputPageState. Returns null if not found
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyRequest GetSJPStopEventRequest()
        {
            ISJPJourneyRequest sjpJourneyRequest = null;

            InputPageState pageState = sessionManager.PageState;

            if (!string.IsNullOrEmpty(pageState.StopEventRequestHash))
            {
                string requestHash = pageState.StopEventRequestHash;

                sjpJourneyRequest = sessionManager.StopEventRequestManager.GetSJPJourneyRequest(requestHash);
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Retrieves the Stop Event SJPJourneyRequest from session, using the 
        /// request hash provided. Returns null if not found
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyRequest GetSJPStopEventRequest(string stopEventRequestHash)
        {
            ISJPJourneyRequest sjpJourneyRequest = null;

            if (!string.IsNullOrEmpty(stopEventRequestHash))
            {
                sjpJourneyRequest = sessionManager.StopEventRequestManager.GetSJPJourneyRequest(stopEventRequestHash);
            }

            return sjpJourneyRequest;
        }

        /// <summary>
        /// Retrieves the Stop Event SJPJourneyResult from session, using the 
        /// request hash provided. Returns null if not found
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyResult GetSJPStopEventResult(string stopEventRequestHash)
        {
            ISJPJourneyResult sjpJourneyResult = null;

            if (!string.IsNullOrEmpty(stopEventRequestHash))
            {
                sjpJourneyResult = sessionManager.StopEventResultManager.GetSJPJourneyResult(stopEventRequestHash);
            }

            return sjpJourneyResult;
        }

        /// <summary>
        /// Checks the session if an SJPJourneyResult exists for the supplied request hash,
        /// and the result contains journeys
        /// </summary>
        /// <param name="journeyRequestHash"></param>
        /// <returns></returns>
        public bool DoesSJPStopEventResultExist(string stopEventRequestHash, bool removeInvalidResult)
        {
            bool resultExists = false;

            if (!string.IsNullOrEmpty(stopEventRequestHash))
            {
                // Check for journey result
                resultExists = sessionManager.StopEventResultManager.DoesResultExist(stopEventRequestHash);

                // Check journeys exist in the result, otherwise
                if (resultExists)
                {
                    ISJPJourneyResult departureResult = sessionManager.StopEventResultManager.GetSJPJourneyResult(stopEventRequestHash);

                    if (departureResult.OutwardJourneys.Count + departureResult.ReturnJourneys.Count == 0)
                    {
                        // No journeys, so it probably failed last time it was planned
                        resultExists = false;

                        // Remove the "bad" result
                        if (removeInvalidResult)
                        {
                            sessionManager.StopEventResultManager.RemoveSJPJourneyResult(stopEventRequestHash);
                        }
                    }
                }
            }

            return resultExists;
        }

        /// <summary>
        /// Updates the sesssion with a Stop Event SJPJourneyRequest, 
        /// and also sets the session PageState to indicate current Stop Event SJPJourneyRequest to use
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        public void UpdateSessionStopEvent(ISJPJourneyRequest sjpStopEventRequest)
        {
            // Add journey request into the session
            sessionManager.StopEventRequestManager.AddSJPJourneyRequest(sjpStopEventRequest);

            // Add to page state so current/next page knows which request it "should" be working with.
            // Request identifier is added to URL to also indicate which request to use, so this value 
            // provides a fallback incase it is missing, helps in certain scenarios (e.g. in tabbed browsing)
            sessionManager.PageState.StopEventRequestHash = sjpStopEventRequest.JourneyRequestHash;

            // Reset selected journeys
            sessionManager.PageState.StopEventJourneyIdOutward = -1;
            sessionManager.PageState.StopEventJourneyIdReturn = -1;

        }

        #endregion

        #region Earlier/Later Methods

        /// <summary>
        /// Method to reset all the Earlier and Later link flags in session
        /// </summary>
        /// <param name="isRiver"></param>
        public void ResetEarlierLaterLinkFlags(bool isRiver)
        {
            if (isRiver)
            {
                sessionManager.PageState.ShowEarlierLinkOutwardRiver = true;
                sessionManager.PageState.ShowLaterLinkOutwardRiver = true;
                sessionManager.PageState.ShowEarlierLinkReturnRiver = true;
                sessionManager.PageState.ShowLaterLinkReturnRiver = true;
            }
        }

        /// <summary>
        /// Method to update the earlier link flag in session
        /// </summary>
        public void UpdateEarlierLinkFlag(bool isOutward, bool isRiver, bool show)
        {
            if (isRiver)
            {
                if (isOutward)
                {
                    sessionManager.PageState.ShowEarlierLinkOutwardRiver = show;
                }
                else
                {
                    sessionManager.PageState.ShowEarlierLinkReturnRiver = show;
                }
            }
        }

        /// <summary>
        /// Method to update the earlier link flag in session
        /// </summary>
        public void UpdateLaterLinkFlag(bool isOutward, bool isRiver, bool show)
        {
            if (isRiver)
            {
                if (isOutward)
                {
                    sessionManager.PageState.ShowLaterLinkOutwardRiver = show;
                }
                else
                {
                    sessionManager.PageState.ShowLaterLinkReturnRiver = show;
                }
            }
        }

        #endregion

        #region Messages

        /// <summary>
        /// Adds SJPMessages to session
        /// </summary>
        /// <param name="errorMessages"></param>
        public void AddMessage(SJPMessage errorMessage)
        {
            sessionManager.PageState.AddMessage(errorMessage);
        }

        /// <summary>
        /// Adds SJPMessages to session
        /// </summary>
        /// <param name="errorMessages"></param>
        public void AddMessages(List<SJPMessage> errorMessages)
        {
            sessionManager.PageState.AddMessages(errorMessages);
        }

        /// <summary>
        /// Clears SJPMessages in session
        /// </summary>
        public void ClearMessages()
        {
            sessionManager.PageState.ClearMessages();
        }

        #endregion
        
        #endregion
    }
}