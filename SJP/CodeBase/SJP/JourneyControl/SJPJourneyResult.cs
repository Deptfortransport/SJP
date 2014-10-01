    // *********************************************** 
// NAME             : SJPJourneyResult.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 Mar 2011
// DESCRIPTION  	: SJPJourneyResult class
// ************************************************
// 


using System;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.CoordinateConvertorProvider;
using CPWS = SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// SJPJourneyResult class
    /// </summary>
    [Serializable()]
    public class SJPJourneyResult : ISJPJourneyResult
    {
        #region Private members

        // Values from journey request
        private string journeyRequestHash = string.Empty;

        // Values for the journey result
        private int journeyReferenceNumber = 0;
        
        // Messages returned by the journey planner
        private List<SJPMessage> messageList = new List<SJPMessage>();

        // Locations
        private SJPLocation originLocation = null;
        private SJPLocation destinationLocation = null;
        private SJPLocation returnOrigin = null;
        private SJPLocation returnDestination = null;

        // Dates
        private DateTime outwardDateTime = DateTime.MinValue;
        private DateTime returnDateTime = DateTime.MinValue;
        private bool outwardArriveBefore = false;
        private bool returnArriveBefore = false;

        // Journeys
        private int startJourneyId = 0;
        private List<Journey> outwardJourneys = new List<Journey>();
        private List<Journey> returnJourneys = new List<Journey>();
                
        // Journey success/error flag
        private bool correctNoJourneysReturned = false;
        
        // Accessible flag
        private bool accessible = false;

        // Language (for any language specific text to output)
        private Language language = Language.English;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SJPJourneyResult()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="journeyRequestHash">JourneyRequest hash value used for planning the journeys to be added to this result</param>
        /// <param name="journeyReferenceNumber">Journey reference number uniquely identifying the journeys containined in this result</param>
        /// <param name="originLocation">Origin location used in planning the journeys</param>
        /// <param name="destinationLocation">Destination location used in planning the journeys</param>
        /// <param name="outwardDateTime">Outward datetime used in planning the journeys</param>
        /// <param name="returnDateTime">Return datetime used in planning the journeys</param>
        /// <param name="accessible">Accessible flag indicates if the journey was planned with accessible paramters in the request</param>
        /// <param name="language">Language for any language specific content to output</param>
        public SJPJourneyResult(string journeyRequestHash, int journeyReferenceNumber,
            SJPLocation originLocation, SJPLocation destinationLocation,
            SJPLocation returnOriginLocation, SJPLocation returnDestinationLocation,
            DateTime outwardDateTime, DateTime returnDateTime,
            bool outwardArriveBefore, bool returnArriveBefore,
            bool accessible,
            Language language,
            List<Journey> outwardJourneysExisting, List<Journey> returnJourneysExisting,
            bool retainOutwardJourneys, bool retainReturnJourneys)
		{
            this.journeyRequestHash = journeyRequestHash;
            this.journeyReferenceNumber = journeyReferenceNumber;
            this.originLocation = originLocation;
            this.destinationLocation = destinationLocation;
            this.returnOrigin = returnOriginLocation;
            this.returnDestination = returnDestinationLocation;
            this.outwardDateTime = outwardDateTime;
            this.returnDateTime = returnDateTime;
            this.outwardArriveBefore = outwardArriveBefore;
            this.returnArriveBefore = returnArriveBefore;
            this.accessible = accessible;
            this.language = language;

            // Add existing journeys
            AddJourneys(outwardJourneysExisting, returnJourneysExisting,
                retainOutwardJourneys, retainReturnJourneys);
		}

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the CJP JourneyResult to this SJPJourneyResult.
        /// The outward flag determines where the information will be added.
        /// </summary>
        /// <param name="cjpResult">CJP result to build journeys for</param>
        /// <param name="outward">Is outward journey</param>
        /// <param name="journeyPart">Journey containing legs to join to CJP result journey</param>
        /// <param name="canUseJourneyPart">Indicates if there are no PT journeys, then can use the journey part to create a journey</param>
        /// <param name="canUseAccessibleTransfer">Indicates if there are no PT journey, then can use an acccessible transfer leg to create a journey</param>
        /// <returns>true if at least one journey was found, false otherwise</returns>
        public bool AddResult(ICJP.JourneyResult cjpResult, bool outward, Journey journeyPart, 
            bool canUseJourneyPart, bool canUseAccessibleTransfer)
        {
            if (cjpResult == null)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("Unexpected null CJP JourneyResult, journeyReferenceNumber[{0}]", journeyReferenceNumber.ToString()));
                Logger.Write(oe);
                return false;
            }

            #region Public journeys

            // Add public journeys
            if (cjpResult.publicJourneys != null)
            {
                foreach (ICJP.PublicJourney cjpPublicJourney in cjpResult.publicJourneys)
                {
                    if ((cjpPublicJourney.legs != null) && (cjpPublicJourney.legs.Length > 0))
                    {
                        // Create the SJP Journey
                        Journey journey = new Journey(GetJourneyId(), cjpPublicJourney, outward,
                            (outward ? originLocation : ReturnOrigin),
                            (outward ? destinationLocation : ReturnDestination),
                            accessible, journeyPart, language);

                        // Add journey, if it doesnt already exist (e.g. could exist if replaning journey retaining previous journeys)
                        if ((journey.Valid) && (!DoesJourneyExist(journey, outward)))
                        {
                            if (outward)
                            {
                                outwardJourneys.Add(journey);
                            }
                            else
                            {
                                returnJourneys.Add(journey);
                            }
                        }
                    }
                }
            }
            else if (journeyPart != null && canUseJourneyPart)
            {
                // Create the SJP Journey only using the JourneyPart
                Journey journey = new Journey(GetJourneyId(), null, outward,
                    (outward ? originLocation : ReturnOrigin),
                    (outward ? destinationLocation : ReturnDestination),
                    accessible, journeyPart, language);

                if (journey.Valid)
                {
                    if (outward)
                    {
                        outwardJourneys.Add(journey);
                    }
                    else
                    {
                        returnJourneys.Add(journey);
                    }
                }
            }
            else if (accessible && canUseAccessibleTransfer)
            {
                // Create the SJP Journey only using an accessible transfer (if the transfer exists)
                Journey journey = new Journey(GetJourneyId(), null, outward,
                    (outward ? originLocation : ReturnOrigin),
                    (outward ? destinationLocation : ReturnDestination),
                    accessible, journeyPart, language);

                if (journey.Valid)
                {
                    if (outward)
                    {
                        outwardJourneys.Add(journey);
                    }
                    else
                    {
                        returnJourneys.Add(journey);
                    }
                }
            }

            #endregion

            #region Private journeys (road)

            // Add private journeys
            if (cjpResult.privateJourneys != null)
            {
                foreach (ICJP.PrivateJourney cjpPrivateJourney in cjpResult.privateJourneys)
                {
                    // Create the SJP Journey
                    Journey journey = new Journey(GetJourneyId(), 
                        cjpPrivateJourney, outward,
                        (outward ? originLocation : ReturnOrigin),
                        (outward ? destinationLocation : ReturnDestination),
                        (outward ? outwardDateTime : returnDateTime),
                        (outward ? outwardArriveBefore : returnArriveBefore),
                        language, accessible);

                    if (outward)
                    {
                        outwardJourneys.Add(journey);
                    }
                    else
                    {
                        returnJourneys.Add(journey);
                    }
                }
            }
 
            #endregion

            // Convert CJP messages into SJP usable messages, and write to log
            AddCJPMessages(cjpResult.messages, outward);

            #region Return flag

            // Return if result was successfully processed
            bool resultValid = false;

            if (outward)
            {
                if (outwardJourneys.Count > 0)
                {
                    resultValid = true;
                }
            }
            else
            {
                if (returnJourneys.Count > 0)
                {
                    resultValid = true;
                }
            }
            
            return resultValid;

            #endregion
        }

        /// <summary>
        /// Adds the CTP CyclePlannerResult to this SJPJourneyResult.
        /// The outward flag determines where the information will be added.
        /// </summary>
        /// <returns>true if at least one journey was found, false otherwise</returns>
        public bool AddResult(CPWS.JourneyResult ctpResult, bool outward)
        {
            if (ctpResult == null)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("Unexpected null CTP JourneyResult, journeyReferenceNumber[{0}]", journeyReferenceNumber.ToString()));
                Logger.Write(oe);
                return false;
            }
                        
            #region Cycle journeys

            // Add cycle journeys
            if (ctpResult.journeys != null)
            {
                foreach (CPWS.Journey ctpJourney in ctpResult.journeys)
                {
                    // Create the SJP Journey
                    Journey journey = new Journey(GetJourneyId(),
                        ctpJourney, outward,
                        (outward ? originLocation : ReturnOrigin),
                        (outward ? destinationLocation : ReturnDestination),
                        (outward ? outwardDateTime : returnDateTime),
                        (outward ? outwardArriveBefore : returnArriveBefore));

                    // Populate the cycle journey latitude/longitude coordinates (needed for the GPX download)
                    if (Properties.Current[Keys.JourneyResult_IncludeLatLongs].Parse(false))
                    {
                        PopulateLatLongCoordinates(journey);
                    }

                    if (outward)
                    {
                        outwardJourneys.Add(journey);
                    }
                    else
                    {
                        returnJourneys.Add(journey);
                    }

                    // Add any journey specific messages
                    foreach (SJPMessage m in journey.Messages)
                    {
                        AddMessageToArray(m.MessageText, m.MessageResourceId, m.MessageArgs, m.MajorMessageNumber, m.MinorMessageNumber, m.Type);
                    }
                }
            }

            #endregion

            // Convert CTP messages into SJP usable messages, and write to log
            AddCTPMessages(ctpResult.messages, outward);

            #region Return flag

            // Return if result was successfully processed
            bool resultValid = false;

            if (outward)
            {
                if (outwardJourneys.Count > 0)
                {
                    resultValid = true;
                }
            }
            else
            {
                if (returnJourneys.Count > 0)
                {
                    resultValid = true;
                }
            }

            return resultValid;

            #endregion
        }

        /// <summary>
        /// Adds the CJP StopEvent to this SJPJourneyResult.
        /// The outward flag determines where the information will be added.
        /// </summary>
        /// <returns>true is at least one stop event journey was added, false otherwise</returns>
        public bool AddResult(ICJP.StopEventResult cjpStopEventResult, bool outward)
        {
            if (cjpStopEventResult == null)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("Unexpected null CJP StopEventResult, journeyReferenceNumber[{0}]", journeyReferenceNumber.ToString()));
                Logger.Write(oe);
                return false;
            }

            #region StopEvent results

            // Add stop event journeys
            if (cjpStopEventResult.stopEvents != null)
            {
                foreach (ICJP.StopEvent cjpStopEvent in cjpStopEventResult.stopEvents)
                {
                    // Create the SJP Journey from the Stop Event
                    Journey journey = new Journey(GetJourneyId(), 
                        cjpStopEvent,
                        (outward ? originLocation : ReturnOrigin),
                        (outward ? destinationLocation : ReturnDestination));

                    // Add journey, if it doesnt already exist (e.g. could exist if replaning journey retaining previous journeys)
                    if ((journey.Valid) && (!DoesJourneyExist(journey, outward)))
                    {
                        if (outward)
                        {
                            outwardJourneys.Add(journey);
                        }
                        else
                        {
                            returnJourneys.Add(journey);
                        }
                    }
                }
            }

            #endregion
                        
            // Convert CJP messages into SJP usable messages, and write to log
            AddCJPMessages(cjpStopEventResult.messages, outward);

            #region Return flag

            // Return if result was successfully processed
            bool resultValid = false;

            if (outward)
            {
                if (outwardJourneys.Count > 0)
                {
                    resultValid = true;
                }
            }
            else
            {
                if (returnJourneys.Count > 0)
                {
                    resultValid = true;
                }
            }

            return resultValid;

            #endregion
        }

        /// <summary>
        /// Adds a displayable SJPMessages to this result (to be shown on UI)
        /// </summary>
        public void AddMessageToArray(string description, string resourceId, List<string> msgArgs,
            int majorCode, int minorCode, SJPMessageType type)
        {
            foreach (SJPMessage msg in messageList)
            {
                if (msg.MessageResourceId == resourceId)
                {
                    return;
                }
            }

            if (msgArgs == null)
            {
                msgArgs = new List<string>();
            }

            messageList.Add(new SJPMessage(description, resourceId, 
                SJP.Common.ResourceManager.SJPResourceManager.COLLECTION_JOURNEY,
                SJP.Common.ResourceManager.SJPResourceManager.GROUP_JOURNEYOUTPUT,
                msgArgs,
                majorCode, minorCode, type));
        }

        /// <summary>
        /// Adds journeys provided to the private journey lists, updating the startJourneyId value
        /// </summary>
        public void AddJourneys(List<Journey> outwardJourneysExisting, List<Journey> returnJourneysExisting,
            bool retainOutwardJourneys, bool retainReturnJourneys)
        {
            List<int> journeyIds = new List<int>();

            if (retainOutwardJourneys && outwardJourneysExisting != null)
            {
                foreach (Journey j in outwardJourneysExisting)
                {
                    if (!journeyIds.Contains(j.JourneyId))
                    {
                        // Add to the journeys list
                        outwardJourneys.Add(j);

                        // Track the id
                        journeyIds.Add(j.JourneyId);
                    }
                    else
                    {
                        OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Warning,
                            string.Format("Duplicate journeyId[{0}] detected when adding existing journeys to this JourneyResult, journey has been discarded, journeyReferenceNumber[{1}]", j.JourneyId, journeyReferenceNumber.ToString()));
                        Logger.Write(oe);
                    }
                }
            }

            if (retainReturnJourneys && returnJourneysExisting != null)
            {
                foreach (Journey j in returnJourneysExisting)
                {
                    if (!journeyIds.Contains(j.JourneyId))
                    {
                        // Add to the journeys list
                        returnJourneys.Add(j);

                        // Track the id
                        journeyIds.Add(j.JourneyId);
                    }
                    else
                    {
                        OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Warning,
                            string.Format("Duplicate journeyId[{0}] detected when adding existing journeys to this JourneyResult, journey has been discarded, journeyReferenceNumber[{1}]", j.JourneyId, journeyReferenceNumber.ToString()));
                        Logger.Write(oe);
                    }
                }
            }

            if (journeyIds.Count > 0)
            {
                journeyIds.Sort();
                startJourneyId = journeyIds[journeyIds.Count - 1];
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read only. Indicates if no journeys were returned, but is not a Journey Planner error
        /// </summary>
        public bool JourneyValidError
        {
            get
            {
                return correctNoJourneysReturned;
            }
        }

        #endregion

        #region ISJPJourneyResult Public methods

        /// <summary>
        /// Returns the journey for the id requested. Null if not found.
        /// </summary>
        /// <param name="journeyId">Journey Id</param>
        /// <returns></returns>
        public Journey GetJourney(int journeyId)
        {
            Journey journey = null;

            // Check in outward journeys
            foreach (Journey j in outwardJourneys)
            {
                if (j.JourneyId == journeyId)
                {
                    journey = j;
                    break;
                }
            }

            // Check in return journeys
            if (journey == null)
            {
                foreach (Journey j in returnJourneys)
                {
                    if (j.JourneyId == journeyId)
                    {
                        journey = j;
                        break;
                    }
                }
            }

            return journey;
        }

        /// <summary>
        /// Sorts the journeys containined in this result 
        /// </summary>
        public void SortJourneys()
        {
            if ((outwardJourneys != null) && (outwardJourneys.Count > 1))
            {
                if (outwardArriveBefore)
                {
                    outwardJourneys.Sort(JourneyComparer.SortJourneyArriveBy);
                }
                else
                {
                    outwardJourneys.Sort(JourneyComparer.SortJourneyLeaveAfter);
                }
            }

            if ((returnJourneys != null) && (returnJourneys.Count > 1))
            {
                if (returnArriveBefore)
                {
                    returnJourneys.Sort(JourneyComparer.SortJourneyArriveBy);
                }
                else
                {
                    returnJourneys.Sort(JourneyComparer.SortJourneyLeaveAfter);
                }
            }
        }

        #endregion

        #region ISJPJourneyResult Public properties

        /// <summary>
        /// Read/Write. JourneyRequest hash value used for planning the journeys contained in this result
        /// </summary>
        public string JourneyRequestHash
        {
            get { return journeyRequestHash; }
            set { journeyRequestHash = value; }
        }

        /// <summary>
        /// Read/Write. Journey reference number uniquely identifying the journeys containined in this result
        /// </summary>
        public int JourneyReferenceNumber
        {
            get { return journeyReferenceNumber; }
            set { journeyReferenceNumber = value; }
        }

        /// <summary>
        /// Read/Write. Messages returned by the journey planner
        /// </summary>
        public List<SJPMessage> Messages
        {
            get { return messageList; }
            set { messageList = value; }
        }

        /// <summary>
        /// Read/Write. Origin Location for the journey result
        /// </summary>
        public SJPLocation Origin
        {
            get { return originLocation; }
            set { originLocation = value; }
        }

        /// <summary>
        /// Read/Write. Destination Location for the journey result
        /// </summary>
        public SJPLocation Destination
        {
            get { return destinationLocation; }
            set { destinationLocation = value; }
        }

        /// <summary>
        /// Read/Write. Return Origin Location for the journey request (if different from the Outward Destination)
        /// </summary>
        public SJPLocation ReturnOrigin
        {
            get { return (returnOrigin == null) ? destinationLocation : returnOrigin; }
            set { returnOrigin = value; }
        }

        /// <summary>
        /// Read/Write. Return Destination Location for the journey request (if different from the Outward Origin)
        /// </summary>
        public SJPLocation ReturnDestination
        {
            get { return (returnDestination == null) ? originLocation : returnDestination; }
            set { returnDestination = value; }
        }

        /// <summary>
        /// Read/Write. Outward datetime
        /// </summary>
        public DateTime OutwardDateTime
        {
            get { return outwardDateTime; }
            set { outwardDateTime = value; }
        }

        /// <summary>
        /// Read/Write. Return datetime
        /// </summary>
        public DateTime ReturnDateTime
        {
            get { return returnDateTime; }
            set { returnDateTime = value; }
        }

        /// <summary>
        /// Read/Write. Outward arrive before time flag
        /// </summary>
        public bool OutwardArriveBefore
        {
            get { return outwardArriveBefore; }
            set { outwardArriveBefore = value; }
        }

        /// <summary>
        /// Read/Write. Return arrive before time flag
        /// </summary>
        public bool ReturnArriveBefore
        {
            get { return returnArriveBefore; }
            set { returnArriveBefore = value; }
        }

        /// <summary>
        /// Read/Write. Outward journeys
        /// </summary>
        public List<Journey> OutwardJourneys
        {
            get { return outwardJourneys; }
            set { outwardJourneys = value; }
        }

        /// <summary>
        /// Read/Write. Return journeys
        /// </summary>
        public List<Journey> ReturnJourneys
        {
            get { return returnJourneys; }
            set { returnJourneys = value; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates a unique journey id for each journey within this SJPJourneyResult.
        /// </summary>
        /// <returns>The next integer a unary series, starting with 1</returns>
        private int GetJourneyId()
        {
            return ++startJourneyId;
        }

        /// <summary>
        /// Determines if the journey provided already exists in the journeys list.
        /// Tests against journey StartTime, EndTime, Changes count, and Legs count
        /// </summary>
        private bool DoesJourneyExist(Journey journey, bool outward)
        {
            List<Journey> journeys = outwardJourneys;

            if (!outward)
            {
                journeys = returnJourneys;
            }

            Journey result = journeys.Find(delegate(Journey j)
                {
                    return (j.StartTime == journey.StartTime)
                        && (j.EndTime == journey.EndTime)
                        && (j.InterchangeCount == journey.InterchangeCount)
                        && (j.JourneyLegs.Count == journey.JourneyLegs.Count);
                });

            return (result != null);
        }

        /// <summary>
        /// Method which updates the various Location and Detail OSGR coordiantes with Latitude Longitudes (which
        /// are needed by the GPX download page).
        /// </summary>
        private void PopulateLatLongCoordinates(Journey journey)
        {
            string logMessge = string.Empty;
            bool success = false;

            if (SJPTraceSwitch.TraceVerbose)
            {
                logMessge = "Populating latitude/longitude coordinates for journey";
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
            }

            try
            {
                // Get the service used to convert the coordinates
                ICoordinateConvertor coordinateConvertor = SJPServiceDiscovery.Current.Get<ICoordinateConvertor>(ServiceDiscoveryKey.CoordinateConvertor);

                foreach (JourneyLeg leg in journey.JourneyLegs)
                {
                    #region Leg start location
                    
                    if (leg.LegStart.Location.GridRef != null)
                    {
                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            logMessge = string.Format("Calling CoordinateConvertor for LegStart[{0}]", leg.LegStart.Location.Name);
                            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
                        }

                        // Get location osgr
                        OSGridReference osgr = leg.LegStart.Location.GridRef;
                        
                        // Call web service
                        LatitudeLongitude latLong = coordinateConvertor.GetLatitudeLongitude(osgr);

                        // Add the latlong coordinate to the location
                        leg.LegStart.Location.LatitudeLongitudeCoordinate = latLong;
                    }

                    #endregion

                    #region Leg end location

                    if (leg.LegEnd.Location.GridRef != null)
                    {
                        if (SJPTraceSwitch.TraceVerbose)
                        {
                            logMessge = string.Format("Calling CoordinateConvertor for LegEnd[{0}]", leg.LegEnd.Location.Name);
                            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
                        }

                        // Get location osgr
                        OSGridReference osgr = leg.LegEnd.Location.GridRef;

                        // Call web service
                        LatitudeLongitude latLong = coordinateConvertor.GetLatitudeLongitude(osgr);

                        // Add the latlong coordinate to the location
                        leg.LegEnd.Location.LatitudeLongitudeCoordinate = latLong;
                    }

                    #endregion

                    #region Details

                    if (leg.JourneyDetails != null)
                    {
                        foreach(JourneyDetail detail in leg.JourneyDetails)
                        {
                            OSGridReference[] osgrs = detail.GetAllOSGRGridReferences();

                            if (osgrs.Length > 0)
                            {
                                if (SJPTraceSwitch.TraceVerbose)
                                {
                                    logMessge = string.Format("Calling CoordinateConvertor for the journey detail. Number of OSGR coordinates [{0}]", osgrs.Length.ToString());
                                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
                                }

                                // Call web service
                                LatitudeLongitude[] latLongs = coordinateConvertor.GetLatitudeLongitude(osgrs);

                                // Add the latlong coordinates to the journey detail
                                detail.UpdateLatitudeLongitudeCoordinates(latLongs);
                            }
                        }
                    }

                    #endregion
                }

                // For logging
                success = true;
            }
            catch (Exception ex)
            {
                string message = "Populating latitude/longitude coordinates for journey threw an exception: " + ex.Message;

                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, message, ex));
            }

            if (SJPTraceSwitch.TraceVerbose)
            {
                logMessge = string.Format("Populating latitude/longitude coordinates for journey completed with success[{0}]", success);
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, logMessge));
            }
        }
        
        #region Messages

        /// <summary>
        /// Parses the array of messages returned by the CJP and logs them all 
        /// (configurable via a property), add those that are displayable 
        /// to an array for eventual presentation to the user. Note that  
        /// messages indicating a technical faiure by the CJP do not need
        /// anything adding to the message array here, because the absence 
        /// of any journeys will cause CJPManager to add an "unable to find 
        /// journeys at this time" msg to the array.  
        /// </summary>
        private void AddCJPMessages(ICJP.Message[] cjpMessages, bool outward)
        {
            if (cjpMessages == null || cjpMessages.Length == 0)
            {
                return;
            }

            #region Read logging flags

            IPropertyProvider properties = Properties.Current;

            // Default to false (prevents logs from filling up but sacrifices support - none of these 
            // should fail as properties will exist)
            bool logNoJourneyResponses = (properties[Keys.LogNoJourneyResponses].Parse(false));
            bool logJourneyWebFailures = (properties[Keys.LogJourneyWebFailures].Parse(false));
            bool logTTBOFailures = (properties[Keys.LogTTBOFailures].Parse(false));
            bool logCJPFailures = (properties[Keys.LogCJPFailures].Parse(false));
            bool logRoadEngineFailures = (properties[Keys.LogRoadEngineFailures].Parse(false));
            bool logDisplayableMessages = (properties[Keys.LogDisplayableMessages].Parse(false));

            #endregion

            foreach (ICJP.Message msg in cjpMessages)
            {
                if (msg.code == Codes.CjpOK || msg.code == Codes.CjpRoadEngineOK)
                {
                    // OK - no need to do anything
                    continue;
                }

                if (msg.code == Codes.CjpNoPublicJourney ||
                    msg.code == Codes.CjpJourneysRejected ||
                    msg.code == Codes.TTBONoTimetableServiceFound ||
                    msg.code == Codes.CjpAwkwardOvernightRemoved)
                {
                    // Flag that no journeys were found by the CJP,
                    // but also that this is not necessarily an error 
                    correctNoJourneysReturned = true;
                }

                ICJP.JourneyWebMessage journeyWebMessage = msg as ICJP.JourneyWebMessage;
                ICJP.TTBOMessage ttboMessage = msg as ICJP.TTBOMessage;

                if (journeyWebMessage != null)
                {
                    #region Process JourneyWebMessage

                    if (msg.code == Codes.JourneyWebMajorNoResults && 
                        journeyWebMessage.subClass == Codes.JourneyWebMinorFuture)
                    {
                        if (logDisplayableMessages)
                        {
                            LogMessage(JourneyControl.Messages.CJP, JourneyControl.Messages.JourneyWebText, msg.description, msg.code, journeyWebMessage.subClass);
                        }

                        // Add error message (UI can then display if it wants)
                        AddMessageToArray(string.Empty, JourneyControl.Messages.JourneyWebTooFarAhead, null, 
                            msg.code, journeyWebMessage.subClass, SJPMessageType.Error);
                    }
                    else if (msg.code == Codes.JourneyWebMajorGeneral && 
                             journeyWebMessage.subClass == Codes.JourneyWebMinorDisplayable)
                    {
                        if (logDisplayableMessages)
                        {
                            LogMessage(JourneyControl.Messages.CJP, JourneyControl.Messages.JourneyWebText, msg.description, msg.code, journeyWebMessage.subClass);
                        }

                        // Add error message (UI can then display if it wants)
                        AddMessageToArray(string.Empty, JourneyControl.Messages.JourneyWebNoResults, null, 
                            msg.code, journeyWebMessage.subClass, SJPMessageType.Error);
                    }
                    else
                    {
                        if (logJourneyWebFailures)
                        {
                            LogMessage(JourneyControl.Messages.CJP, JourneyControl.Messages.JourneyWebText, msg.description, msg.code, journeyWebMessage.subClass);
                        }
                    }

                    #endregion
                }
                if (ttboMessage != null)
                {
                    #region Process TTBOMessage

                    if (logTTBOFailures)
                    {
                        LogMessage(JourneyControl.Messages.CJP, JourneyControl.Messages.TTBOText, msg.description, msg.code, ttboMessage.subCode);
                    }

                    #endregion
                }
                else
                {
                    // Else it is just a normal CJP message
                    // Checl for specific codes and add a message for the UI

                    #region Process CJP and Road engine failures

                    if (msg.code == Codes.CjpAwkwardOvernightRemoved)
                    {
                        // Add error message (UI can then display if it wants)
                        AddMessageToArray(string.Empty, JourneyControl.Messages.CJPOvernightJourneyRejected, null,
                            msg.code, msg.code, SJPMessageType.Error);
                    }
                    else if (msg.code < Codes.CjpRoadEngineMin || msg.code > Codes.CjpRoadEngineMax)
                    {
                        if (logCJPFailures)
                        {
                            if (!this.correctNoJourneysReturned || logNoJourneyResponses)
                            {
                                LogMessage(JourneyControl.Messages.CJP, JourneyControl.Messages.CJPText, msg.description, msg.code, -1);
                            }
                        }
                    }
                    else if (logRoadEngineFailures)
                    {
                        LogMessage(JourneyControl.Messages.CJP, JourneyControl.Messages.RoadEngineText, msg.description, msg.code, -1);
                    }

                    #endregion
                }

            }
        }

        /// <summary>
        /// Logs messages returned by the CTP Cycle Planner Service, adding messages to the displayable message 
        /// list where required
        /// </summary>
        private void AddCTPMessages(CPWS.Message[] ctpMessages, bool outward)
        {
            if (ctpMessages == null || ctpMessages.Length == 0)
            {
                return;
            }

            bool logNoJourneyResponses = Properties.Current[Keys.LogNoJourneyResponses].Parse(false);
            bool logCyclePlannerFailures = Properties.Current[Keys.LogCyclePlannerFailures].Parse(false);

            foreach (CPWS.Message msg in ctpMessages)
            {
                if (msg.code == Codes.CTPOK) // OK - no need to do anything
                {
                    continue;
                }
                else if (msg.code == Codes.CTPNoJourneyCouldBeFound)
                {
                    // Flag that no cycle journeys were found by the Cycle Planner service,
                    // but also that this is not necessarily an error ...
                    this.correctNoJourneysReturned = true;

                    if (logNoJourneyResponses)
                    {
                        LogMessage(JourneyControl.Messages.CTP, JourneyControl.Messages.CTPText, msg.description, msg.code, -1);
                    }
                }
                else
                {
                    if (logCyclePlannerFailures)
                    {
                        LogMessage(JourneyControl.Messages.CTP, JourneyControl.Messages.CTPText, msg.description, msg.code, -1);
                    }
                }
            }
        }
        
        /// <summary>
        /// Logs a CJP/CTP message
        /// </summary>
        private void LogMessage(string plannerType, string messageType, string originalMessage, int majorCode, int minorCode)
        {
            string message = 
                string.Format(JourneyControl.Messages.LogMessage,
                            plannerType, 
                            messageType,
                            originalMessage,
                            ((majorCode >= 0) ? majorCode.ToString() : "-"),
                            ((minorCode >= 0) ? minorCode.ToString() : "-"),
                            journeyReferenceNumber.ToString());

            OperationalEvent operationalEvent = new OperationalEvent(SJPEventCategory.CJP, SJPTraceLevel.Error, message);

            Logger.Write(operationalEvent);
        }

        #endregion

        #endregion
    }
}
