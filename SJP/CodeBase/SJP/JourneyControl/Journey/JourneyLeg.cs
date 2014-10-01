// *********************************************** 
// NAME             : JourneyLeg.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: JourneyLeg class containing details for a journey leg. The class contains 
// a list of journey details (public, road, cycle) for providing further information for specific
// journey types
// ************************************************
// 

using System;
using System.Linq;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using CPWS = SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using SJP.Common.PropertyManager;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// JourneyLeg class containing details for a journey leg, 
    /// including the start/end calling point, and start/end times
    /// </summary>
    [Serializable()]
    public class JourneyLeg
    {
        #region Private static members

        // Used for load
        private static readonly object dataInitialisedLock = new object();
        private static List<string> cableCarNaptans = null;

        #endregion

        #region Private members

        protected SJPModeType mode;
        protected JourneyCallingPoint legStart;
        protected JourneyCallingPoint legEnd;
        protected List<JourneyDetail> journeyDetails = new List<JourneyDetail>();
        protected TimeSpan duration = new TimeSpan(0);
        protected int distance = 0;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public JourneyLeg()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyLeg(SJPModeType mode, JourneyCallingPoint legStart, JourneyCallingPoint legEnd)
        {
            this.mode = mode;
            this.legStart = legStart;
            this.legEnd = legEnd;
        }

        #endregion

        #region Public Static methods

        #region Create for external types

        /// <summary>
		/// Factory method that creates a JourneyLeg for a CJP PublicJourney leg
		/// </summary>
		/// <param name="leg">The current CJP PublicJourney leg</param>
		/// <returns>JourneyLeg</returns>
        public static JourneyLeg Create(ICJP.Leg cjpLeg)
        {
            #region Leg mode

            SJPModeType mode = SJPModeTypeHelper.GetSJPModeType(cjpLeg.mode);

            #endregion

            #region Leg start

            // Leg start is where the user boards
            JourneyCallingPointType startType;

            if (cjpLeg.origin == null)
            {
                startType = JourneyCallingPointType.Board;
            }
            else
            {
                startType = ((cjpLeg.board.stop.NaPTANID == cjpLeg.origin.stop.NaPTANID) ?
                    JourneyCallingPointType.OriginAndBoard : JourneyCallingPointType.Board);
            }

            SJPLocation startLocation = new SJPLocation(cjpLeg.board);
            
            JourneyCallingPoint legStart = new JourneyCallingPoint(startLocation,
                                                        DateTimeHelper.GetDateTime(cjpLeg.board.arriveTime),
                                                        DateTimeHelper.GetDateTime(cjpLeg.board.departTime),
                                                        startType);

            #endregion

            #region Leg end

            // Leg end is where the user alights
            JourneyCallingPointType endType;

            if (cjpLeg.destination == null)
            {
                endType = JourneyCallingPointType.Alight;
            }
            else
            {
                endType = ((cjpLeg.alight.stop.NaPTANID == cjpLeg.destination.stop.NaPTANID) ? 
                    JourneyCallingPointType.DestinationAndAlight : JourneyCallingPointType.Alight);
            }

            SJPLocation endLocation = new SJPLocation(cjpLeg.alight);
                        
            JourneyCallingPoint legEnd = new JourneyCallingPoint(endLocation,
                                                        DateTimeHelper.GetDateTime(cjpLeg.alight.arriveTime),
                                                        DateTimeHelper.GetDateTime(cjpLeg.alight.departTime),
                                                        endType);
            #endregion

            return new JourneyLeg(mode, legStart, legEnd);
        }

        /// <summary>
        /// Factory method that creates a JourneyLeg for a CJP PrivateJourney
        /// </summary>
        /// <returns>JourneyLeg</returns>
        public static JourneyLeg Create(ICJP.PrivateJourney cjpPrivateJourney, 
            SJPLocation startLocation, SJPLocation endLocation,
            DateTime requestTime, bool arriveBefore)
        {
            #region Leg mode

            SJPModeType mode = SJPModeType.Car;

            #endregion

            #region Calculate Start and End times

            long totalDuration = 0;
            bool hasFerry = false;

            // Private journey doesn't contain start/end times, so have to total up the 
            // duration values for each leg to work out the overall start/end times.
            if (cjpPrivateJourney.sections != null)
            {
                foreach (ICJP.Section cjpSection in cjpPrivateJourney.sections)
                {
                    totalDuration += cjpSection.time.Ticks / TimeSpan.TicksPerSecond;

                    // Check for ferry, used in determining start time
                    if ((arriveBefore) && (!hasFerry))
                        hasFerry = RoadJourneyDetail.IsFerrySection(cjpSection);
                }
            }
            
            DateTime startTime;
            DateTime endTime;

            if (arriveBefore)
            {
                // Use the start time from the CJP journey if present - for arrive by journeys
                // this is not necessarily same as requestTime - eg where a ferry was used
                if (hasFerry && (cjpPrivateJourney.startTime != DateTime.MinValue))
                {
                    startTime = DateTimeHelper.GetDateTime(cjpPrivateJourney.startTime);
                    endTime = startTime.Add(new TimeSpan(0, 0, (int)totalDuration));
                }
                else
                {
                    endTime = requestTime;
                    startTime = endTime.Subtract(new TimeSpan(0, 0, (int)totalDuration));
                }
            }
            else
            {
                startTime = requestTime;
                endTime = startTime.Add(new TimeSpan(0, 0, (int)totalDuration));
            }

            #endregion

            #region Leg start

            JourneyCallingPoint legStart = new JourneyCallingPoint(startLocation,
                                                        DateTime.MinValue,
                                                        startTime,
                                                        JourneyCallingPointType.Board);

            #endregion

            #region Leg end
            
            JourneyCallingPoint legEnd = new JourneyCallingPoint(endLocation,
                                                        endTime,
                                                        DateTime.MaxValue,
                                                        JourneyCallingPointType.Alight);
            #endregion

            return new JourneyLeg(mode, legStart, legEnd);
        }
        
        /// <summary>
        /// Factory method that creates a JourneyLeg for a CTP Journey (cycle journey)
        /// </summary>
        /// <returns>JourneyLeg</returns>
        public static JourneyLeg Create(CPWS.Journey ctpJourney,
            SJPLocation startLocation, SJPLocation endLocation,
            DateTime requestTime, bool arriveBefore)
        {
            #region Leg mode

            SJPModeType mode = SJPModeType.Cycle;

            #endregion

            #region Calculate Start and End times

            long totalDuration = 0;
            bool hasFerry = false;

            // Private journey doesn't contain start/end times, so have to total up the 
            // duration values for each leg to work out the overall start/end times.
            if (ctpJourney.sections != null)
            {
                foreach (CPWS.Section cjpSection in ctpJourney.sections)
                {
                    totalDuration += cjpSection.time.Ticks / TimeSpan.TicksPerSecond;

                    // Check for ferry, used in determining start time
                    if ((arriveBefore) && (!hasFerry))
                        hasFerry = CycleJourneyDetail.IsFerrySection(cjpSection);
                }
            }

            DateTime startTime;
            DateTime endTime;

            if (arriveBefore)
            {
                // Use the start time from the CTP journey if present - for arrive by journeys
                // this is not necessarily same as requestTime - eg where a ferry was used
                if (hasFerry && (ctpJourney.startTime != DateTime.MinValue))
                {
                    startTime = DateTimeHelper.GetDateTime(ctpJourney.startTime);
                    endTime = startTime.Add(new TimeSpan(0, 0, (int)totalDuration));
                }
                else
                {
                    endTime = requestTime;
                    startTime = endTime.Subtract(new TimeSpan(0, 0, (int)totalDuration));
                }
            }
            else
            {
                startTime = requestTime;
                endTime = startTime.Add(new TimeSpan(0, 0, (int)totalDuration));
            }

            #endregion

            #region Leg start

            JourneyCallingPoint legStart = new JourneyCallingPoint(startLocation,
                                                        DateTime.MinValue,
                                                        startTime,
                                                        JourneyCallingPointType.Board);

            #endregion

            #region Leg end

            JourneyCallingPoint legEnd = new JourneyCallingPoint(endLocation,
                                                        endTime,
                                                        DateTime.MaxValue,
                                                        JourneyCallingPointType.Alight);
            #endregion

            return new JourneyLeg(mode, legStart, legEnd);
        }

        
        /// <summary>
        /// Factory method that creates a JourneyLeg for a CJP StopEvent leg
        /// </summary>
        /// <returns>JourneyLeg</returns>
        public static JourneyLeg Create(ICJP.StopEvent cjpStopEvent,
            SJPLocation startLocation, SJPLocation endLocation)
        {
            #region Leg mode

            SJPModeType mode = SJPModeTypeHelper.GetSJPModeType(cjpStopEvent.mode);

            #endregion

            #region Leg start

            // Leg start is where the user boards
            JourneyCallingPointType startType = JourneyCallingPointType.Board;

            // Get the stop where user boards, this should be found in the calling stops before our stop,
            // or the stop itself
            ICJP.Event board = GetStopEvent(cjpStopEvent, startLocation, true);

            SJPLocation start = new SJPLocation(board);

            JourneyCallingPoint legStart = new JourneyCallingPoint(start,
                                                        DateTimeHelper.GetDateTime(board.arriveTime),
                                                        DateTimeHelper.GetDateTime(board.departTime),
                                                        startType);

            #endregion

            #region Leg end

            // Leg end is where the user alights
            JourneyCallingPointType endType = JourneyCallingPointType.Alight;

            // Get the stop where user alights, this should be found in the calling stops after our stop,
            // or the stop itself
            ICJP.Event alight = GetStopEvent(cjpStopEvent, endLocation, false);
                        
            SJPLocation end = new SJPLocation(alight);

            JourneyCallingPoint legEnd = new JourneyCallingPoint(end,
                                                        DateTimeHelper.GetDateTime(alight.arriveTime),
                                                        DateTimeHelper.GetDateTime(alight.departTime),
                                                        endType);
            #endregion

            return new JourneyLeg(mode, legStart, legEnd);
        }

        #endregion

        #region Create for internal types

        /// <summary>
        /// Factory method that creates a JourneyLeg
        /// </summary>
        public static JourneyLeg Create(
            SJPModeType sjpModeType, SJPLocation startLocation, SJPLocation endLocation,
            DateTime requestTime, int durationMins, bool arriveBefore)
        {
            #region Leg mode

            SJPModeType mode = sjpModeType;

            #endregion

            #region Calculate Start and End times

            if (durationMins < 0)
                durationMins = 0;
            
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MinValue;

            if (requestTime != DateTime.MinValue)
            {
                if (arriveBefore)
                {
                    endTime = requestTime;
                    startTime = endTime.Subtract(new TimeSpan(0, durationMins, 0));
                }
                else
                {
                    startTime = requestTime;
                    endTime = startTime.Add(new TimeSpan(0, durationMins, 0));
                }
            }

            #endregion

            #region Leg start

            JourneyCallingPoint legStart = new JourneyCallingPoint(startLocation,
                                                        DateTime.MinValue,
                                                        startTime,
                                                        JourneyCallingPointType.Board);

            #endregion

            #region Leg end

            JourneyCallingPoint legEnd = new JourneyCallingPoint(endLocation,
                                                        endTime,
                                                        DateTime.MaxValue,
                                                        JourneyCallingPointType.Alight);
            #endregion

            return new JourneyLeg(mode, legStart, legEnd);
        }

        /// <summary>
        /// Factory method that creates a JourneyLeg
        /// </summary>
        public static JourneyLeg Create(
            SJPModeType sjpModeType, SJPLocation startLocation, SJPLocation endLocation,
            DateTime startTime, DateTime endTime)
        {
            #region Leg mode

            SJPModeType mode = sjpModeType;

            #endregion

            #region Leg start

            JourneyCallingPoint legStart = new JourneyCallingPoint(startLocation,
                                                        DateTime.MinValue, 
                                                        startTime,
                                                        JourneyCallingPointType.Board);

            #endregion

            #region Leg end

            DateTime departTime = DateTime.MaxValue;

            // For a walk, set the depart time to MinValue, this allows any subsequent legs
            // to handle journey time/durations correctly
            if (mode == SJPModeType.Walk)
            {
                departTime = DateTime.MinValue;
            }

            JourneyCallingPoint legEnd = new JourneyCallingPoint(endLocation,
                                                        endTime,
                                                        departTime,
                                                        JourneyCallingPointType.Alight);
            #endregion

            return new JourneyLeg(mode, legStart, legEnd);
        }

        #endregion

        #region Static helpers

        /// <summary>
        /// Finds the CJP.Event stop from the CJP StopEvent for the SJPLocation naptan required.
        /// Defaults to the Stop in the CJP StopEvent if not found
        /// </summary>
        public static ICJP.Event GetStopEvent(ICJP.StopEvent cjpStopEvent, SJPLocation location, bool beforeStop)
        {
            // Assume the stop itself is the stop to find
            ICJP.Event stop = cjpStopEvent.stop;

            // Stop looking for
            string naptan = location.Naptan[0];

            bool found = false;

            // Check the origin and intermediates to the stop itself
            if (beforeStop)
            {
                if (cjpStopEvent.origin != null)
                {
                    if (cjpStopEvent.origin.stop.NaPTANID == naptan)
                    {
                        stop = cjpStopEvent.origin;

                        found = true;
                    }
                }
                
                if ((!found) && (cjpStopEvent.previousIntermediates != null))
                {
                    foreach (ICJP.Event stopEvent in cjpStopEvent.previousIntermediates)
                    {
                        if (stopEvent.stop.NaPTANID == naptan)
                        {
                            stop = stopEvent;
                            break;
                        }
                    }
                }
            }
            // Check from the stop itself and intermeditate to destination
            else
            {
                if (cjpStopEvent.destination != null)
                {
                    if (cjpStopEvent.destination.stop.NaPTANID == naptan)
                    {
                        stop = cjpStopEvent.destination;

                        found = true;
                    }
                }
                
                if ((!found) && (cjpStopEvent.onwardIntermediates != null))
                {
                    foreach (ICJP.Event stopEvent in cjpStopEvent.onwardIntermediates)
                    {
                        if (stopEvent.stop.NaPTANID == naptan)
                        {
                            stop = stopEvent;
                            break;
                        }
                    }
                }
            }

            return stop;
        }

        /// <summary>
        /// Returns true if the leg starts and ends at Cable Car naptans
        /// </summary>
        /// <param name="leg"></param>
        /// <returns></returns>
        public static bool IsLegModeCableCar(JourneyLeg leg)
        {
            #region Setup cable car naptans static list
            if (cableCarNaptans == null)
            {
                lock (dataInitialisedLock)
                {
                    // Check again in case another thread set it up
                    if (cableCarNaptans == null)
                    {
                        // Get the naptans considered as cable car stops
                        string cableCarNaptansString = Properties.Current["JourneyDetails.Location.CableCar.Naptans"];

                        if (!string.IsNullOrEmpty(cableCarNaptansString))
                        {
                            cableCarNaptans = new List<string>(cableCarNaptansString.Split(','));
                        }
                        else
                            cableCarNaptans = new List<string>();
                    }
                }
            }
            #endregion

            if (leg.Mode == SJPModeType.Tram)
            {
                // If the journey leg starts and ends at any of the cable car naptans, 
                // then display the cable car icon
                if (cableCarNaptans.Contains(leg.LegStart.Location.Naptan.FirstOrDefault())
                    && cableCarNaptans.Contains(leg.LegEnd.Location.Naptan.FirstOrDefault()))
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Returns true if the leg contains check constraints which are considered as Queue
        /// </summary>
        /// <param name="leg"></param>
        /// <returns></returns>
        public static bool IsLegModeQueue(JourneyLeg leg)
        {
            if ((leg.JourneyDetails != null) && (leg.JourneyDetails.Count > 0))
            {
                foreach (JourneyDetail jd in leg.JourneyDetails)
                {
                    if (jd is PublicJourneyDetail)
                    {
                        PublicJourneyDetail pjd = (PublicJourneyDetail)jd;

                        if (pjd.HasCheckConstraint())
                        {
                            PublicJourneyInterchangeDetail pjid = (PublicJourneyInterchangeDetail)pjd;
                            if (pjid != null)
                            {
                                int showDurationSecs = Properties.Current["DetailsLegControl.CheckConstraint.ShowQueue.MinimumDuration.Seconds"].Parse(120);

                                // Queue only shown for check constraints with duration of 2mins or more
                                if (pjid.DurationCheckConstraints >= showDurationSecs)
                                {
                                    // This JourneyLeg contains a detail which contains check constraints,
                                    // and the check constraint duration is over the minimum amount,
                                    // so add the Queue mode type (the leg is actually a Walk type but UI
                                    // is required to display these as Queues but process as Walk (e.g. for retail handoff))
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region Public methods

        /// <summary>
        /// Popultes the JourneyDetails for this leg with the provided CJP PublicJourney leg
        /// </summary>
        /// <param name="leg">The current CJP PublicJourney leg</param>
        /// <param name="previousLeg">The previous CJP PublicJourney leg. This is necessary for flights only.</param>
        /// <param name="subsequentLeg">The subsequent CJP PublicJourney leg. This is necessary for flights only.</param>
        public void PopulateJourneyDetails(ICJP.Leg cjpLeg, ICJP.Leg cjpPreviousLeg, ICJP.Leg cjpSubsequentLeg)
        {
            PublicJourneyDetail pjd = PublicJourneyDetail.Create(cjpLeg, cjpPreviousLeg, cjpSubsequentLeg);

            if (pjd != null)
            {
                journeyDetails.Add(pjd);
            }
        }
        
        /// <summary>
        /// Populates the JourneyDetails for this leg with the provided CJP PrivateJourney Sections
        /// </summary>
        /// <param name="cjpSections">All the CJP PrivateJourney Sections for the journey</param>
        public void PopulateJourneyDetails(ICJP.Section[] cjpSections, SJPVenueCarPark carPark, Language language)
        {
            if (cjpSections != null)
            {
                int totalDuration = 0;
                int sectionCount = 0;

                foreach (ICJP.Section cjpSection in cjpSections)
                {
                    RoadJourneyDetail rjd;


                    // Add any Car Park Information text to the first section of the journey
                    if (sectionCount == 0)
                    {
                        rjd = new RoadJourneyDetail(cjpSection, carPark, language);
                    }
                    else
                    {
                        rjd = new RoadJourneyDetail(cjpSection, null, language);
                    }
                                                            
                    // For congestion zones, if Stopover item is an "End", and there is £0 charge, then 
                    // dont add the direction as we dont want it to be displayed
                    if (!((rjd.CongestionEnd) && (rjd.CongestionZoneCost <= 0)))
                    {
                        journeyDetails.Add(rjd);

                        //Get the total distance and total duration
                        if (cjpSection.GetType() == typeof(ICJP.DriveSection) ||
                            cjpSection.GetType() == typeof(ICJP.JunctionDriveSection) ||
                            cjpSection.GetType() == typeof(ICJP.StopoverSection))
                        {
                            distance += rjd.Distance;
                            totalDuration += Convert.ToInt32(rjd.Duration);

                            // SJP is not interested in total fuel cost for private journeys
                        }
                    }
                    sectionCount++;
                }

                // Set the object duration as we have the accurate value here
                duration = new TimeSpan(0, 0, totalDuration);
            }
        }

        /// <summary>
        /// Populates the JourneyDetails for this leg with the provided CTP Journey (cycle) Sections
        /// </summary>
        /// <param name="ctpSections">All the CTP Journey Sections for the cycle journey</param>
        public void PopulateJourneyDetails(CPWS.Section[] ctpSections)
        {
            if (ctpSections != null)
            {
                int totalDuration = 0;

                foreach (CPWS.Section ctpSection in ctpSections)
                {
                    CycleJourneyDetail cjd = new CycleJourneyDetail(ctpSection);

                    journeyDetails.Add(cjd);

                    //Get the total distance and total duration
                    if (ctpSection.GetType() == typeof(CPWS.DriveSection) ||
                        ctpSection.GetType() == typeof(CPWS.JunctionDriveSection) ||
                        ctpSection.GetType() == typeof(CPWS.StopoverSection))
                    {
                        distance += cjd.Distance;
                        totalDuration += Convert.ToInt32(cjd.Duration);

                    }
                }

                // Set the object duration as we have the accurate value here
                duration = new TimeSpan(0, 0, totalDuration);
            }
        }

        /// <summary>
        /// Popultes the JourneyDetails for this leg with the provided CJP StopEvent
        /// </summary>
        /// <param name="cjpStopEvent">The CJP StopEvent</param>
        public void PopulateJourneyDetails(ICJP.StopEvent cjpStopEvent, SJPLocation startLocation, SJPLocation endLocation)
        {
            PublicJourneyDetail pjd = PublicJourneyDetail.Create(cjpStopEvent, startLocation, endLocation);

            if (pjd != null)
            {
                journeyDetails.Add(pjd);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Transport mode for this leg
        /// </summary>
        public SJPModeType Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        /// Read/Write. Calling point at the start of this leg (boarding point).
        /// </summary>
        public JourneyCallingPoint LegStart
        {
            get { return legStart; }
            set { legStart = value; }
        }

        /// <summary>
        /// Read/Write. Calling point at the end of this leg (alighting point).
        /// </summary>
        public JourneyCallingPoint LegEnd
        {
            get { return legEnd; }
            set { legEnd = value; }
        }

        /// <summary>
        /// Read/Write. JourneyDetails data applying to this journey leg. 
        /// e.g. A public journey would have one PublicJourneyDetail, where as a road journey
        /// could have multiple RoadJourneyDetails
        /// </summary>
        public List<JourneyDetail> JourneyDetails
        {
            get { return journeyDetails; }
            set { journeyDetails = value; }
        }

        /// <summary>
        /// Read only. Departure time for the start of this leg. 
        /// If CheckInTime is set in this leg's journey details, then this is returned
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                if ((journeyDetails.Count > 0) && (journeyDetails[0] is PublicJourneyDetail))
                {
                    PublicJourneyDetail pjd = (PublicJourneyDetail)journeyDetails[0];

                    if ((pjd.CheckInTime != null) && (pjd.CheckInTime != DateTime.MinValue))
                    {
                        return pjd.CheckInTime;
                    }
                }
                return legStart.DepartureDateTime;
            }
        }

        /// <summary>
        /// Read only. Arrival time for the end of this leg.
        /// If ExitTime is set in this leg's journey details, then this is returned
        /// </summary>
        public DateTime EndTime
        {
            get 
            {
                if ((journeyDetails.Count > 0) && (journeyDetails[journeyDetails.Count - 1] is PublicJourneyDetail))
                {
                    PublicJourneyDetail pjd = (PublicJourneyDetail)journeyDetails[journeyDetails.Count - 1];

                    if ((pjd.ExitTime != null) && (pjd.ExitTime != DateTime.MinValue))
                    {
                        return pjd.ExitTime;
                    }
                }
                return legEnd.ArrivalDateTime; 
            }
        }

        /// <summary>
        /// Read/Write. Duration of the journey leg
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                CalculateDuration();

                return duration;
            }
            set { duration = value; }
        }

        /// <summary>
        /// Read/Write. Distance of the journey leg.
        /// </summary>
        /// <remarks>
        /// For a PT journey leg, this value is not available.
        /// For a Road journey leg, this value is the total distance for all JourneyDetails contained.
        /// For a Cycle journey leg, this value is the total distance for all JourneyDetails contained.
        /// </remarks>
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Calculates the journey duration
        /// </summary>
        private void CalculateDuration()
        {
            if (duration != new TimeSpan(0))
                return;

            // Get duration from JourneyDetails list (otherwise calculate from this legs JourneyCallingPoint)
            if (journeyDetails.Count > 0)
            {
                int durationSecs = 0;

                foreach (JourneyDetail detail in journeyDetails)
                {
                    // Add up all the durations for the journey details belonging to this leg to give a total
                    durationSecs += detail.Duration;
                }

                duration = new TimeSpan(0, 0, durationSecs);
            }
            else
            {
                long durationTicks = (legEnd.ArrivalDateTime.Ticks - legStart.DepartureDateTime.Ticks);

                duration = new TimeSpan(durationTicks);
            }
        }

        #endregion
    }
}
