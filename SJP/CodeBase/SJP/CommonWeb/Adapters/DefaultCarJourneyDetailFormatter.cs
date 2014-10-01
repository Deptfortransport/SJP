// *********************************************** 
// NAME             : DefaultCarJourneyDetailFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Provides a default implementation for formatting
// car journey details for output to a web page adopted from Transport Direct portal.
// ************************************************


using System.Text;
using SJP.Common.ResourceManager;
using SJP.UserPortal.JourneyControl;
using TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.Common.Web
{
    /// <summary>
    /// Provides a default implementation for formatting
    /// car journey details for output to a web page.
    /// </summary>
    public class DefaultCarJourneyDetailFormatter : CarJourneyDetailFormatter
    {
                

        #region Constructors

        /// <summary>
        /// Constructs a formatter.
        /// </summary>
        /// <param name="journeyLeg">Selected journey details leg</param>
        /// <param name="outward">True if journey details are for outward journey, false if return</param>
        /// <param name="currentLanguage">Language for returned text</param>
        public DefaultCarJourneyDetailFormatter(
            JourneyLeg journeyLeg,
            Language currentLanguage,
            SJPResourceManager resourceManager
            )
            : base(journeyLeg, currentLanguage, false, resourceManager)
        {
        }
                

        #endregion Constructors

        #region Public methods

        protected override string AddContinueFor(RoadJourneyDetail detail,
            bool nextDetailHasJunctionExitJunction, string routeText)
        {
            //convert metres to miles
            string milesDistance = ConvertMetresToMileage(detail.Distance);
            string distanceInKm = ConvertMetresToKm(detail.Distance);
            string distanceInMiles = string.Empty;
            string kmDistance = string.Empty;
            string FerryCrossing = GetResourceString("RouteText.FerryCrossing");

            
            distanceInMiles = "<span class=\"mileshide\">" + milesDistance + space + miles + "</span>";
            kmDistance = "<span class=\"kmsshow\">" + distanceInKm + space + "km" + "</span>";
            

            //check if this text should be added
            if (detail.IsFerry)
            {
                //no need to add the "continues for..." message in these situations
                routeText = FerryCrossing;

                return routeText;
            }

            else if (
                (detail.Distance <= immediateTurnDistance) &&

                !((detail.JunctionAction == JunctionType.Entry) &&
                detail.IsJunctionSection && detail.Roundabout &&
                (detail.PlaceName != null && detail.PlaceName.Length > 0) &&
                nextDetailHasJunctionExitJunction) &&

                !(detail.IsJunctionSection &&
                ((detail.JunctionAction == JunctionType.Exit) ||
                (detail.JunctionAction == JunctionType.Merge)))
                )
            {
                //no need to add the "continues for..." message in these situations
                return routeText;

            }

            else if ((detail.IsSlipRoad) && (detail.Distance < slipRoadDistance) &&

                !((detail.JunctionAction == JunctionType.Entry) &&
                detail.IsJunctionSection && detail.Roundabout &&
                (detail.PlaceName != null && detail.PlaceName.Length > 0) &&
                nextDetailHasJunctionExitJunction) &&

                !(detail.IsJunctionSection &&
                ((detail.JunctionAction == JunctionType.Exit) ||
                (detail.JunctionAction == JunctionType.Merge)) &&
                ((detail.JunctionNumber != null) && (detail.JunctionNumber.Length > 0))))
            {
                //no need to add the "continues for..." message in these situations
                return routeText;
            }
            else
            {
                //in all other cases add "continues for..." to the end of the instruction 
                routeText = routeText + continueFor + space + kmDistance;

            }

            return routeText;
        }

        

        #endregion Public methods

        #region Protected methods

        /// <summary>
        /// A hook method called by processRoadJourney to process each road journey
        /// instruction. The returned string array contains formatted details for
        /// step number, total distance, directions and arrival time in that order
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into roadJourney.Details</param>
        /// <returns>details for each instruction</returns>
        protected override FormattedJourneyDetail ProcessJourneyDetail(int journeyDetailIndex, bool showCongestionCharge)
        {
            FormattedJourneyDetail details = new FormattedJourneyDetail();

            // Note that the Accumulated Distance is updated in GetDirections, therefore the TotalDistance
            // displayed is the total prior to the current instruction rather than after it	
            RoadJourneyDetail roaddetail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;
            RoadJourneyDetail previousroaddetail;

            if (journeyDetailIndex > 0)
            {
                previousroaddetail = journey.JourneyDetails[journeyDetailIndex - 1] as RoadJourneyDetail;
            }
            else
            {
                previousroaddetail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;
            }

            details.Step = GetCurrentStepNumber();

            details.TotalDistance = TotalDistance;

            if (roaddetail.IsStopOver)
            {
                details.TotalDistance = null;
            }

            details.Instruction = GetDirections(journeyDetailIndex, showCongestionCharge);
            details.ArriveTime = GetArrivalTime(journeyDetailIndex);

            details.HighTrafficLevel = roaddetail.CongestionLevel;
           
           details.Distance = GetDistance(journeyDetailIndex);
           

            return details;
        }

        /// <summary>
        /// A hook method called by processRoadJourney to add the first element to the  ordered list 
        /// of journey instructions. The returned string array contains step number, a "Leave from ..." 
        /// instruction and "not applicable" indicators for accumulated distance and arrival time.
        /// </summary>
        /// <returns>details for first instruction</returns>
        protected override FormattedJourneyDetail AddFirstDetailLine()
        {
            FormattedJourneyDetail details = new FormattedJourneyDetail();

            //step number
            details.Step = GetCurrentStepNumber();
            
            //accumulated distance
            details.TotalDistance = null;

            
            details.Instruction = leaveFrom + " <b>" + journey.LegStart.Location.DisplayName + "</b>";
            

            //arrival time
            details.ArriveTime = null;
            
            //distance
            details.Distance = null;
           

            return details;

        }

        /// <summary>
        /// A hook method called by processRoadJourney to add the last element to the 
        /// ordered list of journey instructions. The returned string array contains step number,
        /// an "arrive at ..." instruction and arrival time, and an empty string for accumulated distance
        /// </summary>
        /// <returns>details for last instruction</returns>
        protected override FormattedJourneyDetail AddLastDetailLine()
        {
            FormattedJourneyDetail details = new FormattedJourneyDetail();
            StringBuilder stringDetails = new StringBuilder();

            int journeyDetailIndex = journey.JourneyDetails.Count - 1;

            //step number
            details.Step = GetCurrentStepNumber();

            details.TotalDistance = TotalDistance;

            //instruction
            stringDetails.Append(arriveAt);
            stringDetails.Append("<b>");
            stringDetails.Append(journey.LegEnd.Location.DisplayName);
            stringDetails.Append("</b>");
            stringDetails.Append(" &nbsp; ");
           

            details.Instruction = stringDetails.ToString();

            //arrival time
            details.ArriveTime = GetArrivalTime(journeyDetailIndex);

            details.Distance = GetDistance(journeyDetailIndex);

            return details;

        }



        /// <summary>
        /// Returns formatted string of the road name for the supplied
        /// instruction. Overrides default implmentation to enclose road
        /// name in HTML bold element.
        /// </summary>
        /// <param name="detail">Details of journey instruction</param>
        /// <returns>The road name</returns>
        protected override string FormatRoadName(RoadJourneyDetail detail)
        {
            //Regex regexMotorway = new Regex(motorwaynumber);

            //Check for a motorway
            if (detail.RoadNumber != null)
            {

                if ((detail.RoadNumber.StartsWith("M") | (detail.RoadNumber.EndsWith("(M)"))) && (detail.RoadName != string.Empty))
                {
                    return "<span class=\"mwayLabel\">" + detail.RoadNumber + "</span>" + "<b>" + " (" + detail.RoadName + ")" + "</b> ";
                }
                else if ((detail.RoadNumber.StartsWith("M")) | (detail.RoadNumber.EndsWith("(M)")))
                {
                    return "<span class=\"mwayLabel\">" + detail.RoadNumber + "</span>";
                }
                else
                {
                    return "<b>" + base.FormatRoadName(detail) + "</b>";
                }
            }
            else
            {
                return " ";
            }
        }

        #endregion Protected methods
    }	
}