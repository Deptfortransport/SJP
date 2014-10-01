// *********************************************** 
// NAME             : PublicJourneyTimedDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: PublicJourneyTimedDetail class representing the detail of a
// timed journey leg, e.g. rail
// ************************************************
// 
                
using System;
using TransportDirect.JourneyPlanning.CJPInterface;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// PublicJourneyTimedDetail class representing the detail of a
    /// timed journey leg
    /// </summary>
    [Serializable()]
    public class PublicJourneyTimedDetail : PublicJourneyDetail
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PublicJourneyTimedDetail()
        {
        }
        
        /// <summary>
        /// Takes a CJP leg and creates a new instance of this class populated with
        /// the leg information
        /// </summary>
        /// <param name="leg">A journey leg passed back from the CJP</param>
        public PublicJourneyTimedDetail(Leg leg, Leg previousLeg, Leg subsequentLeg)
            : base(leg, previousLeg, subsequentLeg)
        {
            // Leg duration calculated from leg arrival and departure times
            durationSecs = (int)((leg.alight.arriveTime.Ticks - leg.board.departTime.Ticks) / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Takes a CJP Stop Event and creates a new instance of this class populated with
        /// the StopEvent information
        /// </summary>
        /// <param name="cjpStopEvent">A journey leg passed back from the CJP</param>
        public PublicJourneyTimedDetail(StopEvent cjpStopEvent, SJPLocation startLocation, SJPLocation endLocation)
            : base(cjpStopEvent)
        {
            // Duration calculated from arrival and departure times

            // Get the stop where user boards, this should be found in the calling stops before our stop,
            // or the stop itself
            Event board = JourneyLeg.GetStopEvent(cjpStopEvent, startLocation, true);

            // Get the stop where user alights, this should be found in the calling stops after our stop,
            // or the stop itself
            Event alight = JourneyLeg.GetStopEvent(cjpStopEvent, endLocation, false);
                        
            durationSecs = (int)((alight.arriveTime.Ticks - board.departTime.Ticks) / TimeSpan.TicksPerSecond);

            if (durationSecs < 0)
            {
                durationSecs = 0;
            }
        }

        #endregion
    }
}
