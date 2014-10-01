// *********************************************** 
// NAME             : JourneyHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 07 Apr 2011
// DESCRIPTION  	: JourneyHelper class providing helper methods for journeys
// ************************************************
// 

using System;
using System.Collections.Specialized;
using System.Web;
using SJP.Common.Extenders;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;

namespace SJP.Common.Web
{
    /// <summary>
    /// JourneyHelper class providing helper methods for journeys
    /// </summary>
    public class JourneyHelper
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyHelper()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Retrieves the current journey request hash
        /// </summary>
        /// <returns></returns>
        public string GetJourneyRequestHash()
        {
            string journeyRequestHash = string.Empty;

            // Read journey request hash from Request query string first
            if (HttpContext.Current != null)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                if (queryString != null)
                {
                    journeyRequestHash = queryString[QueryStringKey.JourneyRequestHash];
                }
            }

            // If journey request hash doesnt exist in the query string, then retrieve from session
            if (string.IsNullOrEmpty(journeyRequestHash))
            {
                journeyRequestHash = SJPSessionManager.Current.PageState.JourneyRequestHash;
            }

            return journeyRequestHash;
        }

        /// <summary>
        /// Gets the selected journeys using first query string values, and if not then the values in session
        /// </summary>
        /// <param name="journeyRequestHash"></param>
        /// <param name="journeyOutward"></param>
        /// <param name="journeyReturn"></param>
        /// <returns></returns>
        public bool GetJourneys(out string journeyRequestHash, out Journey journeyOutward, out Journey journeyReturn)
        {
            // Use query string first to identify journeys
            bool journeysFound = GetJourneysUsingQueryString(out journeyRequestHash, out journeyOutward, out journeyReturn);

            if (!journeysFound)
            {
                // Use session second to identify journeys where query string failed
                journeysFound = GetJourneysUsingSession(out journeyRequestHash, out journeyOutward, out journeyReturn);
            }

            return journeysFound;
        }

        /// <summary>
        /// Gets the selected journey Id in session
        /// </summary>
        /// <param name="outward"></param>
        /// <returns></returns>
        public int GetJourneySelected(bool outward)
        {
            if (outward)
            {
                return SJPSessionManager.Current.PageState.JourneyIdOutward;
            }
            else
            {
                return SJPSessionManager.Current.PageState.JourneyIdReturn;
            }
        }

        /// <summary>
        /// Gets the selected journey Id in query string 
        /// (returns 0 if value doesnt exist, journey ids start at 1)
        /// </summary>
        /// <param name="outward"></param>
        /// <returns></returns>
        public int GetJourneySelectedQueryString(bool outward)
        {
            int id = 0;

            if (HttpContext.Current != null)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                if (queryString != null)
                {
                    string journeyId = null;

                    if (outward)
                    {
                        journeyId = queryString[QueryStringKey.JourneyIdOutward];
                    }
                    else
                    {
                        journeyId = queryString[QueryStringKey.JourneyIdReturn];
                    }
                    
                    // Is parse fails, it is set to 0, so no need to check
                    Int32.TryParse(journeyId, out id);                   
                }
            }

            return id;
        }

        /// <summary>
        /// Updates the selected journey Id in session
        /// </summary>
        /// <param name="outward"></param>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public void SetJourneySelected(bool outward, int journeyId)
        {
            if (outward)
            {
                SJPSessionManager.Current.PageState.JourneyIdOutward = journeyId;
            }
            else
            {
                SJPSessionManager.Current.PageState.JourneyIdReturn = journeyId;
            }
        }

        /// <summary>
        /// Sets the outward/return journey leg detail expanded flag
        /// </summary>
        /// <returns></returns>
        public void SetJourneyLegDetailExpanded(bool outward, bool expanded)
        {
            if (outward)
            {
                SJPSessionManager.Current.PageState.JourneyLegDetailExpandedOutward = expanded;
            }
            else
            {
                SJPSessionManager.Current.PageState.JourneyLegDetailExpandedReturn = expanded;
            }
        }

        /// <summary>
        /// Gets the outward/return journey leg detail expanded flag from the query string or session
        /// </summary>
        /// <returns></returns>
        public bool GetJourneyLegDetailExpanded(out bool journeyLegDetailOutwardExpanded, out bool journeyLegDetailReturnExpanded)
        {
            bool found = false;

            journeyLegDetailOutwardExpanded = false;
            journeyLegDetailReturnExpanded = false;

            // Read from query string first
            if (HttpContext.Current != null)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                if (queryString != null)
                {
                    if (!string.IsNullOrEmpty(queryString[QueryStringKey.JourneyLegDetailOutward]))
                    {
                        if (queryString[QueryStringKey.JourneyLegDetailOutward].Trim() == "1")
                        {
                            journeyLegDetailOutwardExpanded = true;
                        }

                        found = true;
                    }

                    if (!string.IsNullOrEmpty(queryString[QueryStringKey.JourneyLegDetailReturn]))
                    {
                        if (queryString[QueryStringKey.JourneyLegDetailReturn].Trim() == "1")
                        {
                            journeyLegDetailReturnExpanded = true;
                        }

                        found = true;
                    }
                }
            }
            
            if (!found)
            {
                // Read from session
                journeyLegDetailOutwardExpanded = SJPSessionManager.Current.PageState.JourneyLegDetailExpandedOutward;
                journeyLegDetailReturnExpanded = SJPSessionManager.Current.PageState.JourneyLegDetailExpandedReturn;

                found = true;
            }

            return found;
        }

        /// <summary>
        /// Gets the selected journey leg using first query string values, and if not then the values in session
        /// </summary>
        /// <returns></returns>
        public bool GetJourneyLeg(out JourneyLeg journeyLeg)
        {
            string journeyRequestHash = string.Empty;
            Journey journeyOutward = null;
            Journey journeyReturn = null;
            
            journeyLeg = null;

            bool journeysFound = GetJourneys(out journeyRequestHash, out journeyOutward, out journeyReturn);

            if (journeysFound)
            {
                #region Retrieve journey leg from query string

                if (HttpContext.Current != null)
                {
                    NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                    if (queryString != null)
                    {
                        // Place in try catch in case journey or journey leg index doesnt exist
                        try
                        {
                            // Try outward journey leg first
                            string journeyLegIndex = queryString[QueryStringKey.JourneyLegIdOutward]; ;

                            int index = -1;

                            if (!string.IsNullOrEmpty(journeyLegIndex))
                            {
                                index = journeyLegIndex.Parse(-1);

                                // Journey leg index has been specified
                                if (index >= 0 && journeyOutward != null)
                                {
                                    journeyLeg = journeyOutward.JourneyLegs[index];
                                }
                            }
                            else
                            {
                                // Try return journey leg
                                journeyLegIndex = queryString[QueryStringKey.JourneyLegIdReturn];
                                
                                if (!string.IsNullOrEmpty(journeyLegIndex))
                                {
                                    index = journeyLegIndex.Parse(-1);

                                    // Journey leg index has been specified
                                    if (index >= 0 && journeyReturn != null)
                                    {
                                        journeyLeg = journeyReturn.JourneyLegs[index];
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // Ignore exceptions, as attempting to retrieve a journey leg only
                        }                        
                    }
                }

                #endregion
            }

            return (journeyLeg != null);
        }
        
        #region Stop Event Methods

        /// <summary>
        /// Retrieves the current stop event journey request hash
        /// </summary>
        /// <returns></returns>
        public string GetStopEventRequestHash()
        {
            string stopEventRequestHash = string.Empty;

            // Read journey request hash from Request query string first
            if (HttpContext.Current != null)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                if (queryString != null)
                {
                    // Stop event request hash is never added to query string, always retrieve from session
                    // stopEventRequestHash = queryString[QueryStringKey.StopEventRequestHash];
                }
            }

            // If journey request hash doesnt exist in the query string, then retrieve from session
            if (string.IsNullOrEmpty(stopEventRequestHash))
            {
                stopEventRequestHash = SJPSessionManager.Current.PageState.StopEventRequestHash;
            }

            return stopEventRequestHash;
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Uses the query string values to identify and retrieve the journey from
        /// session. Expects the query string to contain the journey request identifier,
        /// and the journey id's for outward/return journey.
        /// </summary>
        /// <returns>True if either outward or return journey was found</returns>
        private bool GetJourneysUsingQueryString(out string journeyRequestHash, out Journey journeyOutward, out Journey journeyReturn)
        {
            // Return objects
            journeyRequestHash = null;
            journeyOutward = null;
            journeyReturn = null;

            // Read journey request hash from Request query string
            if (HttpContext.Current != null)
            {
                NameValueCollection queryString = HttpContext.Current.Request.QueryString;

                if (queryString != null)
                {
                    // Read the journey request to get the journeys from
                    journeyRequestHash = queryString[QueryStringKey.JourneyRequestHash];

                    // Read the journey id(s) from the query string
                    string journeyIdOutward = queryString[QueryStringKey.JourneyIdOutward];
                    string journeyIdReturn = queryString[QueryStringKey.JourneyIdReturn];
                                        
                    // If query string contains values use that, otherwise look in session
                    if (!string.IsNullOrEmpty(journeyRequestHash))
                    {
                        SJPResultManager resultManager = SJPSessionManager.Current.ResultManager;

                        // Retrieve the journeys from session
                        ISJPJourneyResult journeyResult = resultManager.GetSJPJourneyResult(journeyRequestHash);

                        if (journeyResult != null)
                        {
                            if (!string.IsNullOrEmpty(journeyIdOutward))
                            {
                                journeyOutward = journeyResult.GetJourney(Convert.ToInt32(journeyIdOutward));
                            }

                            if (!string.IsNullOrEmpty(journeyIdReturn))
                            {
                                journeyReturn = journeyResult.GetJourney(Convert.ToInt32(journeyIdReturn));
                            }
                        }
                    }
                }
            }

            // Return true if either journey was found
            return ((journeyOutward != null) || (journeyReturn != null));
        }

        /// <summary>
        /// Uses the Session to identify and retrieve the journeys from
        /// session. Expects the InputPageState to contain the journey request identifier,
        /// and the journey id's for outward/return journey.
        /// </summary>
        /// <returns>True if either outward or return journey was found</returns>
        private bool GetJourneysUsingSession(out string journeyRequestHash, out Journey journeyOutward, out Journey journeyReturn)
        {
            InputPageState pageState = SJPSessionManager.Current.PageState;

            // Read the journey request from session
            journeyRequestHash = pageState.JourneyRequestHash;

            // Read the journey id(s) from session
            int journeyIdOutward = pageState.JourneyIdOutward;
            int journeyIdReturn = pageState.JourneyIdReturn;
                        

            // Journeys to find
            journeyOutward = null;
            journeyReturn = null;

            if (!string.IsNullOrEmpty(journeyRequestHash))
            {
                SJPResultManager resultManager = SJPSessionManager.Current.ResultManager;

                // Retrieve the journeys from session
                ISJPJourneyResult journeyResult = resultManager.GetSJPJourneyResult(journeyRequestHash);

                if (journeyResult != null)
                {
                    journeyOutward = journeyResult.GetJourney(journeyIdOutward);

                    journeyReturn = journeyResult.GetJourney(journeyIdReturn);
                }
            }

            // Return true if either journey was found
            return ((journeyOutward != null) || (journeyReturn != null));
        }

        #endregion
    }
}