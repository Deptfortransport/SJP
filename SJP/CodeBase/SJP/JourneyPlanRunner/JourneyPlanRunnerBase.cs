// *********************************************** 
// NAME             : JourneyPlanRunnerBase.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: Abstract base class for JourneyPlanRunner's.
//                    Validates user input and creates journey request.
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;

namespace SJP.UserPortal.JourneyPlanRunner
{
    /// <summary>
    /// Abstract base class validates user input and creates journey request.
    /// </summary>
    public abstract class JourneyPlanRunnerBase : IJourneyPlanRunner
    {
        #region Constants

        // Resource string keys to add for any validation messages to be shown in UI
        protected const string DATE_TIME_IS_BEFORE_EVENT = "ValidateAndRun.DateTimeIsBeforeEvent";
        protected const string DATE_TIME_IS_AFTER_EVENT = "ValidateAndRun.DateTimeIsAfterEvent";
        protected const string OUTWARD_DATE_NOT_VALID = "ValidateAndRun.DateNotValid";
        protected const string RETURN_DATE_NOT_VALID = "ValidateAndRun.DateNotValid";
        protected const string OUTWARD_DATE_IS_AFTER_RETURN_DATE = "ValidateAndRun.OutwardDateIsAfterReturnDate";
        protected const string DATE_TIME_IS_IN_THE_PAST = "ValidateAndRun.DateTimeIsInThePast";
        protected const string DATE_TIME_IS_NOT_VALID_CYCLE_PARK = "CycleJourneyLocations.CycleParkNoneFound.Text"; // Reuse error message on control

        protected const string ORIGIN_INVALID = "ValidateAndRun.InvalidOrigin";
        protected const string DESTINATION_INVALID = "ValidateAndRun.InvalidDestination";
        protected const string ATLEAST_ONE_LOCATION_VENUE = "ValidateAndRun.AtleastOneLocationShouleBeVenue";
        protected const string ORIGIN_DESTINATION_VENUES_OVERLAPS = "ValidateAndRun.OriginAndDestinationOverlaps";
        protected const string ORIGIN_DESTINATION_VENUES_ARE_SAME = "ValidateAndRun.OriginAndDestinationAreSame";

        protected const string CYCLE_PLANNER_UNAVAILABLE = "ValidateAndRun.CyclePlannerUnavailableKey";
        protected const string LOCATION_HAS_NO_POINT = "ValidateAndRun.LocationHasNoPoint";
        protected const string DISTANCE_BETWEEN_LOCATIONS_TOO_GREAT = "ValidateAndRun.DistanceBetweenLocationsTooGreat";
        protected const string DISTANCE_TO_VENUE_LOCATION_TOO_GREAT = "ValidateAndRun.DistanceToVenueLocationTooGreat";
        protected const string DISTANCE_FROM_VENUE_LOCATION_TOO_GREAT = "ValidateAndRun.DistanceFromVenueLocationTooGreat";
        protected const string LOCATION_IN_INVALID_CYCLE_AREA = "ValidateAndRun.LocationInInvalidCycleArea";
        protected const string LOCATION_POINTS_NOT_IN_SAME_CYCLE_AREA = "ValidateAndRun.LocationPointsNotInSameCycleArea";

        #endregion

        #region Protected variables

        protected bool foundNonLocationValidationError = false;
        protected Dictionary<string, SJPMessage> listErrors;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyPlanRunnerBase()
        {
            listErrors = new Dictionary<string, SJPMessage>();
        }

        #endregion

        #region IJourneyPlanRunner Public Properties

        /// <summary>
        /// Read only. Messages raised by the validation
        /// </summary>
        public List<SJPMessage> Messages
        {
            get { return listErrors.Values.ToList(); }
        }

        #endregion

        #region IJourneyPlanRunner Interface Members

        /// <summary>
        /// Abstract method classes must implement to perform the request validation and invoke the journey planning
        /// </summary>
        public abstract bool ValidateAndRun(ISJPJourneyRequest journeyRequest, string language, bool submitRequest);

        #endregion

        #region Protected Methods

        #region Validation methods

        /// <summary>
        /// Adds the error generated while validation into a list of errors 
        /// </summary>
        /// <param name="msgResourceID"></param>
        protected void SetValidationError(string msgResourceID)
        {
            if (!listErrors.ContainsKey(msgResourceID))
            {
                listErrors.Add(msgResourceID, new SJPMessage(msgResourceID, SJPMessageType.Error));
            }
        }

        /// <summary>
        /// Adds the error generated while validation into a list of errors 
        /// </summary>
        /// <param name="msgResourceID"></param>
        protected void SetValidationError(string msgResourceID, List<string> msgArgs)
        {
            if (!listErrors.ContainsKey(msgResourceID))
            {
                listErrors.Add(msgResourceID, new SJPMessage(
                    string.Empty, msgResourceID, string.Empty, string.Empty, msgArgs,
                    0, 0, SJPMessageType.Error));
            }
        }

        /// <summary>
        /// Validates outward and return datetime
        /// </summary>
        /// <param name="journeyRequest">Journey Request object</param>
        protected void PerformDateValidations(ISJPJourneyRequest journeyRequest)
        {
            DateTime outwardDateTime = journeyRequest.OutwardDateTime;
            DateTime returnDateTime = journeyRequest.ReturnDateTime;

            IPropertyProvider pp = Properties.Current;

            bool validateEventDatesRange = pp["JourneyPlanner.Validate.Switch.DatesInGamesDateRange"].Parse(true);
            DateTime eventStartDate = pp["JourneyPlanner.Validate.Games.StartDate"].Parse(DateTime.MinValue);
            DateTime eventEndDate = pp["JourneyPlanner.Validate.Games.EndDate"].Parse(DateTime.MinValue);

            #region Outward date validation

            if (journeyRequest.IsOutwardRequired)
            {
                if (outwardDateTime == DateTime.MinValue)
                {
                    SetValidationError(OUTWARD_DATE_NOT_VALID);
                }
                else if (outwardDateTime < DateTime.Now)
                {
                    SetValidationError(DATE_TIME_IS_IN_THE_PAST);
                }
                else
                {
                    // Check if the requested date is within the Events game duration
                    if (validateEventDatesRange)
                    {
                        if (outwardDateTime < eventStartDate)
                        {
                            SetValidationError(DATE_TIME_IS_BEFORE_EVENT);
                        }

                        if (outwardDateTime > eventEndDate)
                        {
                            SetValidationError(DATE_TIME_IS_AFTER_EVENT);
                        }
                    }
                }
            }

            #endregion

            #region Return date validation

            if (journeyRequest.IsReturnRequired)
            {
                if (returnDateTime == DateTime.MinValue)
                {
                    SetValidationError(RETURN_DATE_NOT_VALID);
                }

                if (journeyRequest.IsOutwardRequired 
                    && returnDateTime < outwardDateTime)
                {
                    SetValidationError(OUTWARD_DATE_IS_AFTER_RETURN_DATE);
                }

                if (returnDateTime < DateTime.Now)
                {
                    SetValidationError(DATE_TIME_IS_IN_THE_PAST);
                }

                // Check if the requested date is within the Events game duration
                if (validateEventDatesRange)
                {
                    if (returnDateTime < eventStartDate)
                    {
                        SetValidationError(DATE_TIME_IS_BEFORE_EVENT);
                    }

                    if (returnDateTime > eventEndDate)
                    {
                        SetValidationError(DATE_TIME_IS_AFTER_EVENT);
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Validates locations in the journey request
        /// </summary>
        /// <param name="journeyRequest">Journey Request object</param>
        protected void PerformLocationValidations(ISJPJourneyRequest journeyRequest)
        {
            SJPLocation origin = journeyRequest.Origin;
            SJPLocation destination = journeyRequest.Destination;

            IPropertyProvider pp = Properties.Current;

            bool validateOneLocationIsVenue = pp["JourneyPlanner.Validate.Switch.OneLocationIsVenue"].Parse(true);

            // Check at least one location is a venue
            if (validateOneLocationIsVenue)
            {
                if (origin.TypeOfLocation != SJPLocationType.Venue
                    && destination.TypeOfLocation != SJPLocationType.Venue)
                {
                    SetValidationError(ATLEAST_ONE_LOCATION_VENUE);
                }
            }

            // Check if both locations are venues, they are not located in the same place, i.e their parents are not the same
            if (origin.TypeOfLocation == SJPLocationType.Venue
                && destination.TypeOfLocation == SJPLocationType.Venue)
            {
                // Check they are not the same locations
                if ((origin.GridRef.Easting == destination.GridRef.Easting)
                    && (origin.GridRef.Northing == destination.GridRef.Northing)
                    && (origin.Naptan.Count >= 1)
                    && (destination.Naptan.Count >= 1)
                    && (origin.Naptan[0] == destination.Naptan[0]))
                {
                    SetValidationError(ORIGIN_DESTINATION_VENUES_ARE_SAME);
                }
                // Only check if parent exists for either venue
                else if ((!string.IsNullOrEmpty(origin.Parent)) || (!string.IsNullOrEmpty(destination.Parent)))
                {
                    if (origin.Parent == destination.Parent
                       || origin.Naptan.Contains(destination.Parent)
                       || destination.Naptan.Contains(origin.Parent))
                    {
                        SetValidationError(ORIGIN_DESTINATION_VENUES_OVERLAPS);
                    }
                }
            }
        }

        #endregion

        #region Invoke journey/stop event managers

        /// <summary>
        /// Invoke the CJP Manager (asynchronously)
        /// </summary>
        /// <param name="journeyRequest">User's validated Journey Request</param>
        protected void InvokeCJPManager(ISJPJourneyRequest journeyRequest, string language)
        {
            ISJPSessionManager sessionManager = SJPSessionManager.Current;

            // Obtain an instance of JourneyPlanRunnerCaller from ServiceDiscovery
            IJourneyPlanRunnerCaller journeyCaller = SJPServiceDiscovery.Current.Get<IJourneyPlanRunnerCaller>(ServiceDiscoveryKey.JourneyPlanRunnerCaller);

            journeyCaller.InvokeCJPManager(journeyRequest, sessionManager.Session, language);
        }

        /// <summary>
        /// Invoke the Cycle Planner Manager (asynchronously)
        /// </summary>
        /// <param name="journeyRequest">User's validated Journey Request</param>
        protected void InvokeCyclePlannerManager(ISJPJourneyRequest journeyRequest, string language)
        {
            ISJPSessionManager sessionManager = SJPSessionManager.Current;

            // Obtain an instance of CycleJourneyPlanRunnerCaller from ServiceDiscovery
            ICycleJourneyPlanRunnerCaller cycleJourneyCaller = SJPServiceDiscovery.Current.Get<ICycleJourneyPlanRunnerCaller>(ServiceDiscoveryKey.CycleJourneyPlanRunnerCaller);

            cycleJourneyCaller.InvokeCyclePlannerManager(journeyRequest, sessionManager.Session, language);
        }

        /// <summary>
        /// Invoke the Stop Event Manager (asynchronously)
        /// </summary>
        /// <param name="journeyRequest">Validated Journey Request for Stop Event request</param>
        protected void InvokeStopEventManager(ISJPJourneyRequest journeyRequest, string language)
        {
            ISJPSessionManager sessionManager = SJPSessionManager.Current;
            
            // Obtain an instance of StopEventRunnerCaller from ServiceDiscovery
            IStopEventRunnerCaller stopEventCaller = SJPServiceDiscovery.Current.Get<IStopEventRunnerCaller>(ServiceDiscoveryKey.StopEventRunnerCaller);

            stopEventCaller.InvokeStopEventManager(journeyRequest, sessionManager.Session, language);
        }

        #endregion

        #endregion

    }
}
