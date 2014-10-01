// *********************************************** 
// NAME             : DefaultCycleJourneyDetailFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Provides a default implementation for formatting
//                    cycle journey details for output to a web page.
// ************************************************


using System.Collections.Generic;
using SJP.Common.ResourceManager;
using SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using SJP.UserPortal.JourneyControl;

namespace SJP.Common.Web
{
    /// <summary>
    /// Provides a default implementation for formatting
    /// cycle journey details for output to a web page.
    /// </summary>
    public class DefaultCycleJourneyDetailFormatter : CycleJourneyDetailFormatter
    {
        

        #region Constructors

        /// <summary>
        /// Constructs a formatter.
        /// </summary>
        /// <param name="cycleJourney">The specific cycle journey to display</param>
        /// <param name="journeyViewState">The related journey view state, used specifically for congestion zone</param>
        /// <param name="outward">Indicates if this is an outward journey</param>
        /// <param name="roadUnits">The road units to display the journey distance in</param>
        /// <param name="print">Indicates if the instructions are displayed on a printer friendly page</param>
        /// <param name="showAllDetails">Indicates if the attributes are set to visible or hidden by default</param>
        /// <param name="isCJPUser">When true, outputs additional direction values</param>
        public DefaultCycleJourneyDetailFormatter(
            JourneyLeg cycleJourney,
            Language currentLanguage,
            SJPResourceManager resourceManager)
            : base(cycleJourney, currentLanguage, false, resourceManager, true)
        {
            
        }

        #endregion Constructors

        
        #region Override Protected methods

        /// <summary>
        /// A hook method called by ProcessJourney to process each cycle journey
        /// instruction. The returned string array contains formatted details for
        /// step number, total distance, directions and arrival time in that order
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into cycleJourney.Details</param>
        /// <returns>details for each instruction</returns>
        protected override FormattedJourneyDetail ProcessJourneyDetail(int journeyDetailIndex, bool showCongestionCharge)
        {
            FormattedCycleJourneyDetail details = new  FormattedCycleJourneyDetail();

            // Note that the Accumulated Distance is updated in GetDirections, therefore the TotalDistance
            // displayed is the total prior to the current instruction rather than after it	
            CycleJourneyDetail cycleDetail = journey.JourneyDetails[journeyDetailIndex] as CycleJourneyDetail;

            details.Step = GetCurrentStepNumber();

            details.TotalDistance = TotalDistance;

            if (cycleDetail.StopoverSection)
            {
                details.TotalDistance = null;
            }

            //is the journey a return and was congestion charge shown for the outward already?
            details.Instruction = GetDirections(journeyDetailIndex, showCongestionCharge);
            details.PathName = GetCyclePathName(journeyDetailIndex);
            details.PathImageUrl = GetCyclePathImage(journeyDetailIndex);
            details.PathImageText = GetCyclePathImageText(journeyDetailIndex);
            details.ManoeuvreImage = GetManoeuvreImageUrl();
            details.ManoeuvreImageText = GetManoeuvreImageText();
            details.CycleInstruction = GetCycleInstructionDetails(journeyDetailIndex);
            details.CycleAttributes = GetCycleAttributeDetails(journeyDetailIndex, true);
            details.ArriveTime = GetArrivalTime(journeyDetailIndex);
            details.Distance = GetDistance(journeyDetailIndex);
            details.DurationActual = GetDurationActual(journeyDetailIndex);
            details.DistanceActual = GetDistanceActual(journeyDetailIndex);
            details.TOIDs = GetTOIDs(journeyDetailIndex);
            details.OSGRs = GetOSGRs(journeyDetailIndex);

            return details;
        }

        /// <summary>
        /// A hook method called by ProcessCycleJourney to add the first element to the ordered list 
        /// of journey instructions. The returned string array contains step number, a "Leave from ..." 
        /// instruction and "not applicable" indicators for accumulated distance and arrival time.
        /// </summary>
        /// <returns>details for first instruction</returns>
        protected override FormattedJourneyDetail AddFirstDetailLine()
        {
            FormattedCycleJourneyDetail details = new FormattedCycleJourneyDetail();

            //step number
            details.Step = GetCurrentStepNumber();

            //accumulated distance
            details.TotalDistance = TotalDistance;

            //instruction
            details.Instruction = leaveFrom + " <b>" + journey.LegStart.Location.DisplayName + "</b> &nbsp;";

            //cycle path name
            details.PathName = string.Empty;

            //cycle path image
            details.PathImageUrl = string.Empty;

            details.ManoeuvreImage = string.Empty;

            //cycle instruction text
            details.CycleInstruction = string.Empty;

            //cycle attribute details
            details.CycleAttributes = string.Empty;

            //arrival time
            details.ArriveTime = null;
                      

            return details;
        }

        /// <summary>
        /// A hook method called by ProcessCycleJourney to add the last element to the 
        /// ordered list of journey instructions. The returned string array contains step number,
        /// an "arrive at ..." instruction and arrival time, and an empty string for accumulated distance
        /// </summary>
        /// <returns>details for last instruction</returns>
        protected override FormattedJourneyDetail AddLastDetailLine()
        {
            FormattedCycleJourneyDetail details = new FormattedCycleJourneyDetail();

            //step number
            details.Step = GetCurrentStepNumber();

            int journeyDetailIndex = journey.JourneyDetails.Count - 1;

            //accumulated distance
            details.TotalDistance = TotalDistance;

            //instruction
            details.Instruction = arriveAt + "<b>" + journey.LegEnd.Location.DisplayName + "</b> &nbsp;";

            //cycle path name
            details.PathName = string.Empty;

            //cycle path image
            details.PathImageUrl = string.Empty;

            details.ManoeuvreImage = string.Empty;

            //cycle instruction text
            details.CycleInstruction = string.Empty;

            //cycle attribute details
            details.CycleAttributes = string.Empty;

            //arrival time
            details.ArriveTime = GetArrivalTime(journeyDetailIndex);
            

            return details;
        }

        protected override string AddContinueFor(CycleJourneyDetail detail,
            bool nextDetailHasJunctionExitJunction, string routeText)
        {
            //convert metres to miles
            string milesDistance = ConvertMetresToMileage(detail.Distance);
            string distanceInKm = ConvertMetresToKm(detail.Distance);
            string distanceInMiles = string.Empty;
            string kmDistance = string.Empty;
            string ferryCrossing = GetResourceString("RouteText.FerryCrossing");

            //Switches the default display of either Miles or Kms depending on roadUnits
            
            distanceInMiles = "<span class=\"mileshide\">" + milesDistance + space + miles + "</span>";
            kmDistance = "<span class=\"kmsshow\">" + distanceInKm + space + "km" + "</span>";
            

            //check if this text should be added
            if (detail.Ferry)
            {
                //no need to add the "continues for..." message in these situations
                routeText = ferryCrossing;

                return routeText;
            }

            else if (
                (detail.Distance <= immediateTurnDistance) &&

                !((detail.JunctionType == JunctionType.Entry) &&
                detail.JunctionSection &&
                (detail.PlaceName != null && detail.PlaceName.Length > 0) &&
                nextDetailHasJunctionExitJunction) &&

                !(detail.JunctionSection &&
                ((detail.JunctionType == JunctionType.Exit) ||
                (detail.JunctionType == JunctionType.Merge)))
                )
            {
                //no need to add the "continues for..." message in these situations
                return routeText;

            }

            else if ((detail.SlipRoad) && (detail.Distance < slipRoadDistance) &&

                !((detail.JunctionType == JunctionType.Entry) &&
                detail.JunctionSection &&
                (detail.PlaceName != null && detail.PlaceName.Length > 0) &&
                nextDetailHasJunctionExitJunction) &&

                !(detail.JunctionSection &&
                ((detail.JunctionType == JunctionType.Exit) ||
                (detail.JunctionType == JunctionType.Merge)) &&
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

        /// <summary>
        /// Returns formatted string of the road name for the supplied
        /// instruction. Overrides default implmentation to enclose road
        /// name in HTML bold element.
        /// </summary>
        /// <param name="detail">Details of journey instruction</param>
        /// <returns>The road name</returns>
        protected override string FormatRoadName(CycleJourneyDetail detail)
        {
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

        /// <summary>
        /// A hook method called by ProcessCycleJourney to filter the details returned to only include
        /// the instructions at the start and end index
        /// </summary>
        /// <returns>Returns an ordered list of journey instructions where each 
        /// element is a string array of details (e.g, road name, distance, directions), filtered 
        /// to only include instructions between the start and end indexs</returns>
        protected override List<FormattedJourneyDetail> FilterCycleJourneyDetails(List<FormattedJourneyDetail> details, int startIndex, int endIndex)
        {
            List<FormattedJourneyDetail> filteredDetails = new List<FormattedJourneyDetail>();

            // The details array passed in will have the actual instructions, 
            // plus a first and last instruction which says leave/arrive at location.
            // The start/end index supplied do not "know" about these additional instructions, 
            // therefore, we need to include these instructions when applicable.

            for (int i = 0; i < details.Count; i++)
            {
                FormattedJourneyDetail detail = details[i];

                int stepNumber = detail.Step;

                // start index is "actual" first instruction, so also need to add the "leaving from" direction
                if ((startIndex == 1) && (stepNumber == 1))
                {
                    filteredDetails.Add(detail);
                }
                // end index is "actual" last instruction, so also need to add the "arrive at" direction
                else if ((endIndex == details.Count - 2) && (stepNumber == details.Count))
                {
                    filteredDetails.Add(detail);
                }
                // this detail is within the range asked for, so add to filtered array
                else if ((stepNumber >= startIndex + 1) && (stepNumber <= endIndex + 1))
                {
                    filteredDetails.Add(detail);
                }
            }

            return filteredDetails;
        }

        #endregion
    }
}