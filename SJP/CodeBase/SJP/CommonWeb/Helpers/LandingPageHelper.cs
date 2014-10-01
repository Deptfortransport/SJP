// *********************************************** 
// NAME             : LandingPageHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 13 Apr 2011
// DESCRIPTION  	: LandingPageHelper class providing methods to aid in page landing
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using Logger = System.Diagnostics.Trace;
using LS = SJP.Common.LocationService;

namespace SJP.Common.Web
{
    /// <summary>
    /// LandingPageHelper class providing methods to aid in page landing
    /// </summary>
    public class LandingPageHelper
    {
        #region Private members

        // Used to hold all original query string values in a request
        private NameValueCollection queryString = new NameValueCollection();
        private bool isMobile = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LandingPageHelper()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LandingPageHelper(NameValueCollection queryString)
            : this()
        {
            if (queryString != null)
            {
                this.queryString = queryString;
            }
        }

        /// <summary>
        /// Constructor that sets isMobile property
        /// </summary>
        public LandingPageHelper(NameValueCollection queryString, bool isMobile)
            : this()
        {
            if (queryString != null)
            {
                this.queryString = queryString;
            }

            this.isMobile = isMobile;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks the query string to determine if there are values which would indicate this is a
        /// landing page request
        /// </summary>
        /// <returns></returns>
        public bool IsLandingPageRequest(PageId pageId, List<PageId> bookmarkJourneyPageIds, bool pageTransfered, bool isPostback)
        {
            bool result = false;

            if (bookmarkJourneyPageIds == null)
                bookmarkJourneyPageIds = new List<PageId>();

            // Check for any parameters considered as a landing page parameter
            if (ContainsLandingPageParameters(queryString)) 
            {
                // If auto plan is provided, and isn't a postback, then user has manually created 
                // landing page url as SJP does not add
                if ((queryString[QueryStringKey.AutoPlan] != null) && (!isPostback))
                {
                    result = true;
                }
                // If there isnt a journey request hash, and isn't a postback, then user has manually 
                // created landing page url as SJP does add to all pages other than JourneyPlannerInput
                else if ((queryString[QueryStringKey.JourneyRequestHash] == null) && (!isPostback))
                {
                    result = true;
                }
                // If there is a journey request hash, and page id is a bookmark journey page (e.g. JourneyOptions), 
                // and page hasn't been  transferred (i.e. transitioned from another page on site), and isn't a postback,
                // then likely user has bookmarked the journey (e.f. on JourneyOptions page), and is returning using that link
                else if ((queryString[QueryStringKey.JourneyRequestHash] != null) &&
                         (bookmarkJourneyPageIds.Contains(pageId)) && (!pageTransfered) && (!isPostback))
                {
                    result = true;
                }
                // If there is a journey request hash, and page id is a bookmark journey page (e.g. JourneyOptions), 
                // and page has been transferred (i.e. transitioned from another page on site), and isn't a postback,
                // then likely user has submitted a journey from the input page - this isn't a landing page request. 
                // To check if the user has submitted "another" landing page URL, test the journey 
                // request hash doesn't exist in their session - thus confirming it is a new landing page request
                else if ((queryString[QueryStringKey.JourneyRequestHash] != null) &&
                         (bookmarkJourneyPageIds.Contains(pageId)) && (pageTransfered) && (!isPostback))
                {
                    string journeyRequestHash = queryString[QueryStringKey.JourneyRequestHash];

                    SessionHelper sessionHelper = new SessionHelper();

                    ISJPJourneyRequest sjpJourneyRequest = sessionHelper.GetSJPJourneyRequest(journeyRequestHash);

                    if (sjpJourneyRequest == null)
                    {
                        result = true;
                    }
                    // If the request is in session, but hasn't been submitted, then likely user has submitted the same
                    // landing page url twice, without planning the journey, therefore treat as a new landing page request
                    else if (!sjpJourneyRequest.JourneyRequestSubmitted)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns true if the Landing Page Auto Plan flag is found in the query string
        /// </summary>
        /// <returns></returns>
        public bool IsAutoPlan()
        {
            bool result = false;

            if ((queryString.Count > 0) && (queryString[QueryStringKey.AutoPlan] != null))
            {
                result = GetBoolean(queryString[QueryStringKey.AutoPlan]);
            }

            return result;
        }

        /// <summary>
        /// Returns the Landing Page Partner value from the query string
        /// </summary>
        /// <returns></returns>
        public string GetPartner()
        {
            string result = string.Empty;

            if ((queryString.Count > 0) && (queryString[QueryStringKey.Partner] != null))
            {
                result = queryString[QueryStringKey.Partner];

                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Trim().ToUpper();
                }

                // Database column length limit
                if (result.Length > 50)
                {
                    result = result.Substring(0, 50);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns true if the query string contains any parameters used for landing page
        /// </summary>
        /// <returns></returns>
        public bool ContainsLandingPageParameters(NameValueCollection queryString)
        {
            if (queryString != null)
            {
                if (!string.IsNullOrEmpty(queryString[QueryStringKey.OriginId]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.OriginType]))
                    return true;
                // Not checking name because it is only used with the location id and type
                //else if (!string.IsNullOrEmpty(queryString[QueryStringKey.OriginName]))
                //    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.DestinationId]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.DestinationType]))
                    return true;
                // Not checking name because it is only used with the location id and type
                //else if (!string.IsNullOrEmpty(queryString[QueryStringKey.DestinationName]))
                //    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.OutwardDate]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.OutwardTime]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.ReturnDate]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.ReturnTime]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.OutwardRequired]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.ReturnRequired]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.PlannerMode]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.Partner]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.AccessibleOption]))
                    return true;
                else if (!string.IsNullOrEmpty(queryString[QueryStringKey.FewestChanges]))
                    return true;
            }

            // No landing page parameters
            return false;
        }

        /// <summary>
        /// Builds a dictionary of QueryStringKey strings and values which represent the 
        /// SJPJourneyRequest that can be appended to a page url, which then becomes a 
        /// landing page url for the current request
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        /// <returns></returns>
        public Dictionary<string,string> BuildJourneyRequestForQueryString(ISJPJourneyRequest sjpJourneyRequest)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (sjpJourneyRequest != null)
            {
                // Origin location
                result.Add(QueryStringKey.OriginId, sjpJourneyRequest.Origin.ID);
                result.Add(QueryStringKey.OriginType, GetSJPLocationType(sjpJourneyRequest.Origin.TypeOfLocation));

                // Origin location name for coordinate locations only
                if (sjpJourneyRequest.Origin.TypeOfLocation == SJPLocationType.CoordinateEN)
                {
                    result.Add(QueryStringKey.OriginName, HttpContext.Current.Server.UrlEncode(sjpJourneyRequest.Origin.Name.SubstringFirst(30)));
                }

                // Destination location
                result.Add(QueryStringKey.DestinationId, sjpJourneyRequest.Destination.ID);
                result.Add(QueryStringKey.DestinationType, GetSJPLocationType(sjpJourneyRequest.Destination.TypeOfLocation));

                // Destination location name for coordinate locations only
                if (sjpJourneyRequest.Destination.TypeOfLocation == SJPLocationType.CoordinateEN)
                {
                    result.Add(QueryStringKey.DestinationName, HttpContext.Current.Server.UrlEncode(sjpJourneyRequest.Destination.Name.SubstringFirst(30)));
                }

                // Datetimes
                result.Add(QueryStringKey.OutwardDate, sjpJourneyRequest.OutwardDateTime.ToString("yyyyMMdd"));
                result.Add(QueryStringKey.OutwardTime, sjpJourneyRequest.OutwardDateTime.ToString("HHmm"));

                // Outward journey not required, determines if input page is shown in "plan return journey only" mode
                if (!sjpJourneyRequest.IsOutwardRequired)
                {
                    result.Add(QueryStringKey.OutwardRequired, GetBoolean(sjpJourneyRequest.IsOutwardRequired));
                }

                // Return date time and is required
                if (sjpJourneyRequest.IsReturnRequired)
                {
                    result.Add(QueryStringKey.ReturnDate, sjpJourneyRequest.ReturnDateTime.ToString("yyyyMMdd"));
                    result.Add(QueryStringKey.ReturnTime, sjpJourneyRequest.ReturnDateTime.ToString("HHmm"));
                    result.Add(QueryStringKey.ReturnRequired, GetBoolean(sjpJourneyRequest.IsReturnRequired));
                }

                // Planner mode
                result.Add(QueryStringKey.PlannerMode, GetPlannerMode(sjpJourneyRequest.PlannerMode));

                // Accessible preferences and only if required)
                if (sjpJourneyRequest.AccessiblePreferences != null && sjpJourneyRequest.AccessiblePreferences.Accessible)
                {
                    result.Add(QueryStringKey.AccessibleOption, GetAccessiblePreferences(sjpJourneyRequest.AccessiblePreferences));

                    if (sjpJourneyRequest.AccessiblePreferences.RequireFewerInterchanges)
                    {
                        result.Add(QueryStringKey.FewestChanges, GetBoolean(sjpJourneyRequest.AccessiblePreferences.RequireFewerInterchanges));
                    }
                }

                // DO NOT ADD AUTOPLAN FLAG
            }

            return result;
        }

        /// <summary>
        /// Uses values from the Query string to return a populated SJPJourneyRequest.
        /// Returns null if no query string values available. 
        /// Returns request with as much populated values as it can create.
        /// </summary>
        /// <returns></returns>
        public ISJPJourneyRequest RetrieveJourneyRequestFromQueryString()
        {
            ISJPJourneyRequest sjpJourneyRequest = null;

            // Error message keys
            string invalidLocationsKey = isMobile ? "Landing.Message.InvalidLocations.Mobile.Text" : "Landing.Message.InvalidLocations.Text";
            string invalidDestinationKey = isMobile ? "Landing.Message.InvalidDestination.Mobile.Text" : "Landing.Message.InvalidDestination.Text";
            string invalidOriginKey = isMobile ? "Landing.Message.InvalidOrigin.Mobile.Text" : "Landing.Message.InvalidOrigin.Text";

            if ((queryString != null) && (queryString.Count > 0))
            {
                // Validation messages
                List<SJPMessage> messages = new List<SJPMessage>();
                
                // Firsty message, want to display this first if any other messages are added
                messages.Add(new SJPMessage("Landing.Message.CheckTravelOptions.Text", SJPMessageType.Error));

                #region Read values

                // Read values from query string
                string originId = queryString[QueryStringKey.OriginId];
                string originType = queryString[QueryStringKey.OriginType];
                string originName = queryString[QueryStringKey.OriginName];
                string destinationId = queryString[QueryStringKey.DestinationId];
                string destinationType = queryString[QueryStringKey.DestinationType];
                string destinationName = queryString[QueryStringKey.DestinationName];

                string outwardDate = queryString[QueryStringKey.OutwardDate];
                string outwardTime = queryString[QueryStringKey.OutwardTime];
                string returnDate = queryString[QueryStringKey.ReturnDate];
                string returnTime = queryString[QueryStringKey.ReturnTime];

                string outwardRequired = queryString[QueryStringKey.OutwardRequired];
                string returnRequired = queryString[QueryStringKey.ReturnRequired];

                string plannerMode = queryString[QueryStringKey.PlannerMode];

                string accessibleOption = queryString[QueryStringKey.AccessibleOption];
                string fewestChanges = queryString[QueryStringKey.FewestChanges];

                #endregion

                #region Parse and validate values

                // Parse values to be used in building the request
                SJPLocationType originLocationType = GetSJPLocationType(originType, messages);
                SJPLocationType destinationLocationType = GetSJPLocationType(destinationType, messages);

                // Outward required, assume true
                bool isOutwardRequired = true;
                if (!string.IsNullOrEmpty(outwardRequired))
                {
                    isOutwardRequired = GetBoolean(outwardRequired);
                }

                // Return required 
                bool isReturnRequired = GetBoolean(returnRequired);

                // Locations
                LocationHelper locationHelper = new LocationHelper();

                SJPLocation originLocation = locationHelper.GetSJPLocation(HttpContext.Current.Server.UrlDecode(originId), 
                    HttpContext.Current.Server.UrlDecode(originName), originLocationType);
                SJPLocation destinationLocation = locationHelper.GetSJPLocation(HttpContext.Current.Server.UrlDecode(destinationId), 
                    HttpContext.Current.Server.UrlDecode(destinationName), destinationLocationType);

                #region Validate location types and direction required

                // Both origin and destination was supplied, check they are not both non-venues
                if ((!string.IsNullOrEmpty(originId)) && (!string.IsNullOrEmpty(destinationId)))
                {
                    // Test they are not both non-venues
                    if ((originLocation != null) && (destinationLocation != null))
                    {
                        if (originLocation.TypeOfLocation != SJPLocationType.Venue
                            && destinationLocation.TypeOfLocation != SJPLocationType.Venue)
                        {
                            messages.Add(new SJPMessage(invalidLocationsKey, SJPMessageType.Error));

                            // Because both are not a venue, null one location to ensure location control renders correctly
                            // with a venue dropdown
                            if (isOutwardRequired)
                            {
                                destinationLocation = null;
                            }
                            else
                            {
                                originLocation = null;
                            }
                        }
                    }
                    else if ((originLocation == null) && (destinationLocation != null))
                    {
                        // No origin location, and destination is not a venue
                        if (destinationLocation.TypeOfLocation != SJPLocationType.Venue)
                        {
                            messages.Add(new SJPMessage(invalidLocationsKey, SJPMessageType.Error));
                        }
                    }
                    else if ((originLocation != null) && (destinationLocation == null))
                    {
                        // No destination location, and origin is not a venue
                        if (originLocation.TypeOfLocation != SJPLocationType.Venue)
                        {
                            messages.Add(new SJPMessage(invalidLocationsKey, SJPMessageType.Error));
                        }
                    }
                    else if ((originLocation == null) && (destinationLocation == null))
                    {
                        // Neither destination was resolved correctly
                        messages.Add(new SJPMessage(invalidLocationsKey, SJPMessageType.Error));
                    }
                }
                // Only destination was supplied, check it is a venue
                else if ((!string.IsNullOrEmpty(destinationId)) && (isOutwardRequired))
                {
                    // Test destination is a venue
                    if ((destinationLocation == null) ||
                        ((destinationLocation != null) && (destinationLocation.TypeOfLocation != SJPLocationType.Venue)))
                    {
                        destinationLocation = null;

                        messages.Add(new SJPMessage(invalidDestinationKey, SJPMessageType.Error));
                    }
                }
                // Only origin was supplied (in a "return only" journey), check it is a venue
                else if ((!string.IsNullOrEmpty(originId)) && (!isOutwardRequired) && (isReturnRequired))
                {
                    // Test origin is a venue
                    if ((originLocation == null) ||
                        ((originLocation != null) && (originLocation.TypeOfLocation != SJPLocationType.Venue)))
                    {
                        originLocation = null;

                        messages.Add(new SJPMessage(invalidOriginKey, SJPMessageType.Error));
                    }
                }

                // Confirm location and required direction are for a valid scenario, 
                // i.e. if origin location was supplied for a "return only" journey, and it isn't a venue,
                // then make it an outward journey required rather than losing the origin location which might be required
                if (originLocation != null
                    && originLocation.TypeOfLocation != SJPLocationType.Venue
                    && !isOutwardRequired)
                {
                    isOutwardRequired = true;
                }

                #endregion

                // Datetimes
                DateTime outwardDateTime = GetDateTime(outwardDate, outwardTime, true, messages);
                DateTime returnDateTime = DateTime.MinValue;

                if (isReturnRequired)
                {
                    returnDateTime = GetDateTime(returnDate, returnTime, false, messages);
                }
                                
                // Planner mode
                SJPJourneyPlannerMode sjpPlannerMode = GetPlannerMode(plannerMode, messages);

                // Accessible preferences
                SJPAccessiblePreferences accessiblePreferences = GetAccessiblePreferences(accessibleOption, messages);

                bool isFewestChangesRequired = GetBoolean(fewestChanges);
                accessiblePreferences.RequireFewerInterchanges = isFewestChangesRequired;

                #endregion

                try
                {
                    JourneyRequestHelper jrh = new JourneyRequestHelper();

                    sjpJourneyRequest = jrh.BuildSJPJourneyRequest(
                        originLocation, destinationLocation, 
                        outwardDateTime, returnDateTime, 
                        isOutwardRequired, isReturnRequired, (!isOutwardRequired && isReturnRequired),
                        sjpPlannerMode,
                        accessiblePreferences);
                }
                catch (SJPException sjpEx)
                {
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                        string.Format("Error building SJPJourneyRequest from QueryString values"), sjpEx));
                }

                // Add any messages to display
                SessionHelper sessionHelper = new SessionHelper();

                if (messages.Count > 1) // Default message is always added (see above) so check for any more validations
                {
                    sessionHelper.AddMessages(messages);
                }
            }


            return sjpJourneyRequest;
        }

        #endregion

        #region Private methods
        
        #region Parse methods

        /// <summary>
        /// Parses the query string location type value into an SJPLocationType
        /// </summary>
        /// <returns>Default is SJPLocationType.Unknown </returns>
        private SJPLocationType GetSJPLocationType(string qsLocationType, List<SJPMessage> messages)
        {
            try
            {
                // If location type hasn't been specified, try making it a Venue as this is most likely 
                // how London2012 will land to us
                if (string.IsNullOrEmpty(qsLocationType))
                {
                    return SJPLocationType.Venue;
                }


                return SJPLocationTypeHelper.GetSJPLocationTypeQS(qsLocationType);
            }
            catch
            {
                // Any exception, then its an unrecognised value, so default to unknown
                return SJPLocationType.Unknown;
            }
        }

        /// <summary>
        /// Parses the query string location type value into an SJPLocationType
        /// </summary>
        /// <returns>Default is SJPLocationType.Unknown </returns>
        private string GetSJPLocationType(SJPLocationType locationType)
        {
            return SJPLocationTypeHelper.GetSJPLocationTypeQS(locationType);
        }

        /// <summary>
        /// Uses the query string date and time values to return a DateTime
        /// </summary>
        /// <param name="date">Date in format yyyymmdd</param>
        /// <param name="time">Time in format hhmm</param>
        /// <returns>Default DateTime as now</returns>
        private DateTime GetDateTime(string qsDate, string qsTime, bool isOutward, List<SJPMessage> messages)
        {
            #region Set initial datetime, and the configured start and end datetimes

            // Use the configured start and end date to default to if querystring values are outside this range
            DateTime defStartDate = Properties.Current["JourneyPlanner.Validate.Games.StartDate"].Parse(DateTime.Now.Date);
            DateTime defEndDate = Properties.Current["JourneyPlanner.Validate.Games.EndDate"].Parse(DateTime.Now.Date);

            // Get the configured default outward selected time value
            string[] outTimeparts = Properties.Current["EventDateControl.DropDownTime.Outward.Default"].Split(new char[] { ':' });
            int defOutMinute = outTimeparts[1].Parse(0);
            int defOutHour = outTimeparts[0].Parse(0);

            // Get the configured default return selected time value
            string[] retTimeparts = Properties.Current["EventDateControl.DropDownTime.Return.Default"].Split(new char[] { ':' });
            int defRetMinute = retTimeparts[1].Parse(0);
            int defRetHour = retTimeparts[0].Parse(0);
                        
            // Set start using the configured start date and time
            DateTime start = new DateTime(defStartDate.Year, defStartDate.Month, defStartDate.Day,
                    isOutward ? defOutHour : defRetHour,
                    isOutward ? defOutMinute : defRetMinute,
                    0);

            // Set end using the configured end date and time
            DateTime end = new DateTime(defEndDate.Year, defEndDate.Month, defEndDate.Day,
                    isOutward ? defOutHour : defRetHour,
                    isOutward ? defOutMinute : defRetMinute,
                    0);
            
            // Get now (ignoring seconds)
            DateTime dtNow = DateTime.Now;
            DateTime now = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, dtNow.Hour, dtNow.Minute, 0);

            // Initial defaults to now
            DateTime dtInitial = now;

            // If now earlier than configured Start datetime, set to default Start
            if (dtInitial < defStartDate)
            {
                dtInitial = start;
            }
            // If "today", then update initial to either be the configured Start time, or Now time, whichever is later
            if ((dtInitial.Date == now.Date) && (now.TimeOfDay < start.TimeOfDay))
            {
                dtInitial = new DateTime(now.Year, now.Month, now.Day, start.Hour, start.Minute, 0);
            }
            // If initial after configured End datetime, set to End
            if (dtInitial > defEndDate)
            {
                dtInitial = end;
            }
                        
            #endregion

            try
            {
                // Set the initial datetime values
                int year = dtInitial.Year;
                int month = dtInitial.Month;
                int day = dtInitial.Day;
                int hour = dtInitial.Hour;
                int minute = dtInitial.Minute;
                int second = dtInitial.Second;
                
                // Build up the datetime using querystring, any errors then return the default datetime 
                if (!string.IsNullOrEmpty(qsDate))
                {
                    year = Convert.ToInt32(qsDate.Substring(0, 4));
                    month = Convert.ToInt32(qsDate.Substring(4, 2));
                    day = Convert.ToInt32(qsDate.Substring(6, 2));
                }
                if (!string.IsNullOrEmpty(qsTime))
                {
                    hour = Convert.ToInt32(qsTime.Substring(0, 2));
                    minute = Convert.ToInt32(qsTime.Substring(2, 2));
                }

                DateTime datetime = new DateTime(year, month, day, hour, minute, 0);

                // Anything before configured Start is an invalid datetime, so default to that
                if (datetime < defStartDate)
                {
                    datetime = dtInitial;
                    
                    messages.Add(new SJPMessage("Landing.Message.InvalidDateInPast.Text", SJPMessageType.Error));
                }
                // Or datetime is for "today", and is before time now for today
                else if ((datetime.Date == now.Date) && (datetime.TimeOfDay < now.TimeOfDay))
                {
                    datetime = dtInitial;

                    messages.Add(new SJPMessage("Landing.Message.InvalidDateInPast.Text", SJPMessageType.Error));
                }
                // Anything later then the configured End date is an invalid datetime, so default to end date
                else if (datetime > defEndDate)
                {
                    datetime = end;

                    messages.Add(new SJPMessage("Landing.Message.InvalidDateInPast.Text", SJPMessageType.Error));
                }
                
                return datetime;
            }
            catch
            {
                messages.Add(new SJPMessage("Landing.Message.InvalidDate.Text", SJPMessageType.Error));

                // Any exception, then return the min datetime. The date control, journey plan runner will
                // validate/update it if needed
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Uses the query string boolean flag values to return a Boolean
        /// </summary>
        /// <param name="flag">Default to false</param>
        private bool GetBoolean(string qsFlag)
        {
            try
            {
                // Check for 0 or 1
                if (qsFlag.Trim() == "0")
                {
                    return false;
                }
                else if (qsFlag.Trim() == "1")
                {
                    return true;
                }
                else
                {
                    return Convert.ToBoolean(qsFlag.Trim().ToLower());
                }
            }
            catch
            {
                // Any exception, return false
                return false;
            }
        }

        /// <summary>
        /// Returns a query string representation of a Bool
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string GetBoolean(bool flag)
        {
            if (flag)
                return "1";
            else
                return "0";
        }

        /// <summary>
        /// Parses the query string planner mode value into an SJPJourneyPlannerMode
        /// </summary>
        /// <param name="qsPlannerMode"></param>
        /// <returns></returns>
        private SJPJourneyPlannerMode GetPlannerMode(string qsPlannerMode, List<SJPMessage> messages)
        {
            return SJPJourneyPlannerModeHelper.GetSJPJourneyPlannerModeQS(qsPlannerMode);            
        }

        /// <summary>
        /// Returns a query string representation of the SJPJourneyPlannerMode
        /// </summary>
        /// <param name="qsPlannerMode"></param>
        /// <returns></returns>
        private string GetPlannerMode(SJPJourneyPlannerMode plannerMode)
        {
            return SJPJourneyPlannerModeHelper.GetSJPJourneyPlannerModeQS(plannerMode);
        }

        /// <summary>
        /// Parses the query string accessible options value into an SJPAccessiblePreferences
        /// </summary>
        /// <param name="qsAccessibleOptions"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        private SJPAccessiblePreferences GetAccessiblePreferences(string qsAccessibleOption, List<SJPMessage> messages)
        {
            SJPAccessiblePreferences sjpAccessiblePreference = new SJPAccessiblePreferences();

            sjpAccessiblePreference.PopulateAccessiblePreference(qsAccessibleOption);
                        
            return sjpAccessiblePreference;
        }

        /// <summary>
        /// Returns a query string representation of the SJPAccessiblePreferences
        /// </summary>
        /// <param name="qsPlannerMode"></param>
        /// <returns></returns>
        private string GetAccessiblePreferences(SJPAccessiblePreferences sjpAccessiblePreference)
        {
            if (sjpAccessiblePreference != null)
            {
                return sjpAccessiblePreference.GetAccessiblePreferenceString();
            }

            return string.Empty;
        }

        #endregion

        #endregion
    }
}