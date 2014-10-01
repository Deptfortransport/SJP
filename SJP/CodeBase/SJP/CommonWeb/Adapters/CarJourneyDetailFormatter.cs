// *********************************************** 
// NAME             : CarJourneyDetailFormatter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Wraps a RoadJourneyDetail object to provide easier
// formatting of car journey details for presentation purposes.
// Subclasses will implement formatting for specific purposes e.g.
// web page output, email output and so on. The code has been adopted from the 
// formating class inplemented for Transport Direct portal. The class uses Template
// pattern to allow formatting for different type of purposes.
// ************************************************


using System;
using System.Globalization;
using System.Text;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.UserPortal.JourneyControl;
using TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.Common.Web
{
    /// <summary>
    /// Wraps a RoadJourneyDetail object to provide easier
    /// formatting of car journey details for presentation purposes.
    /// Subclasses will implement formatting for specific purposes e.g.
    /// web page output, email output (if needed) and so on.
    /// </summary>
    public abstract class CarJourneyDetailFormatter : JourneyDetailFormatter
    {
        #region Instance Variables

        
        // Road distance in metres within which "immediate" text should be 
        // prepended to journey direction
        protected int immediateTurnDistance;

        // slip road distance limit parameter
        protected int slipRoadDistance;

        // string for through route
        protected string throughRoute = String.Empty;
        

        #region Strings that are used to generate the route text descriptions.

        // string for roundabout exit
        protected string roundaboutExitOne = String.Empty;
        protected string roundaboutExitTwo = String.Empty;
        protected string roundaboutExitThree = String.Empty;
        protected string roundaboutExitFour = String.Empty;
        protected string roundaboutExitFive = String.Empty;
        protected string roundaboutExitSix = String.Empty;
        protected string roundaboutExitSeven = String.Empty;
        protected string roundaboutExitEight = String.Empty;
        protected string roundaboutExitNine = String.Empty;
        protected string roundaboutExitTen = String.Empty;

        protected string continueString = String.Empty;
        protected string continueMiniRoundabout = String.Empty;
        protected string leftMiniRoundabout = String.Empty;
        protected string rightMiniRoundabout = String.Empty;
        protected string uTurnMiniRoundabout = String.Empty;
        protected string leftMiniRoundabout2 = String.Empty;
        protected string rightMiniRoundabout2 = String.Empty;
        protected string uTurnMiniRoundabout2 = String.Empty;

        protected string immediatelyTurnLeft = String.Empty;
        protected string immediatelyTurnRight = String.Empty;

        protected string turnLeftOne = String.Empty;
        protected string turnLeftTwo = String.Empty;
        protected string turnLeftThree = String.Empty;
        protected string turnLeftFour = String.Empty;

        protected string turnRightOne = String.Empty;
        protected string turnRightTwo = String.Empty;
        protected string turnRightThree = String.Empty;
        protected string turnRightFour = String.Empty;

        protected string turnLeftInDistance = String.Empty;
        protected string turnRightInDistance = String.Empty;

        protected string bearLeft = String.Empty;
        protected string bearRight = String.Empty;

        protected string immediatelyBearLeft = String.Empty;
        protected string immediatelyBearRight = String.Empty;

        protected string leaveFrom = String.Empty;
        protected string arriveAt = String.Empty;
        protected string notApplicable = String.Empty;

        protected string localRoad = String.Empty;

        //route text for motorway junctions
        protected string atJunctionLeave = String.Empty;
        protected string leaveMotorway = String.Empty;
        protected string towards = String.Empty;
        protected string continueFor = String.Empty;
        protected string miles = String.Empty;
        protected string turnLeftToJoin = String.Empty;
        protected string turnRightToJoin = String.Empty;
        protected string atJunctionJoin = String.Empty;
        protected string bearLeftToJoin = String.Empty;
        protected string bearRightToJoin = String.Empty;
        protected string join = String.Empty;
        protected string forText = String.Empty;
        protected string follow = String.Empty;
        protected string to = String.Empty;
        protected string routeTextFor = String.Empty;
        protected string continueText = String.Empty;
        protected string untilJunction = String.Empty;
        protected string onTo = String.Empty;

        //Strings for Del 7 Car Costing
        protected string enter = String.Empty;
        protected string exit = String.Empty;
        protected string end = String.Empty;
        protected string congestionZone = String.Empty;
        protected string charge = String.Empty;
        protected string certainTimes = String.Empty;
        protected string certainTimesNoCharge = String.Empty;
        protected string board = String.Empty;
        protected string departingAt = String.Empty;
        protected string toll = String.Empty;
        protected string notAvailable = String.Empty;

        protected string HighTraffic = String.Empty;
        protected string PlanStop = String.Empty;
        protected string FerryWait = String.Empty;
        protected string UnspecifedFerryWait = String.Empty;
        protected string IntermediateFerryWait = String.Empty;
        protected string WaitAtTerminal = String.Empty;

        protected string viaArriveAt = String.Empty;
        protected string leaveFerry = String.Empty;

        //Ambiguity text
        protected string straightOn = string.Empty;
        protected string atMiniRoundabout = string.Empty;
        protected string immediatelyTurnRightOnto = string.Empty;
        protected string immediatelyTurnLeftOnto = string.Empty;
        protected string whereRoadSplits = string.Empty;

        // park and ride specific
        protected string parkAndRide = string.Empty;
        protected string carParkText = string.Empty;

        // open new window icon
        protected string openNewWindowImageUrl = string.Empty;


        //London Congestion Charge Additional Text added for CCN0602
        protected string londonCCAdditionalText = String.Empty;
        protected bool londonCCzoneExtraTextVisible = true;


        #endregion

        #endregion Instance Variables


        #region Constructors
        /// <summary>
        /// Private default constructor method
        /// </summary>
        /// <param name="roadJourney">The specific road journey to display</param>
        /// <param name="outward">Whether the journey is an outward one</param>
        /// <param name="currentCulture">Culture for returned text</param>
        public CarJourneyDetailFormatter(
            JourneyLeg roadJourney,
            Language currentLanguage,
            bool print,
            SJPResourceManager resourceManager
            ) : base ( roadJourney, currentLanguage, print, resourceManager)
        {
            
            
            
            InitialiseRouteTextDescriptionStrings();

            // Get road distance in metres within which "immediate" text should be 
            // prepended to journey direction
            immediateTurnDistance = Properties.Current["Web.CarJourneyDetailsControl.ImmediateTurnDistance"].Parse(0);

            // Get slip road distance limit parameter
            slipRoadDistance = Properties.Current["Web.CarJourneyDetailsControl.SlipRoadDistance"].Parse(0);

            // property switch for CCN0602 London CCzone Extra Text - displayed when CCzone boundaries under review
            londonCCzoneExtraTextVisible = Properties.Current["CCN0602LondonCCzoneExtraTextVisible"].Parse(false);

        }

        #endregion Constructors
               

        

        #region Protected methods


        //protected abstract string AddContinueFor(RoadJourneyDetail detail, string routeText);
        protected abstract string AddContinueFor(RoadJourneyDetail detail,
            bool nextDetailHasJunctionExitJunction, string routeText);

        

        /// <summary>
        /// Returns formatted string of the road name for the supplied
        /// instruction
        /// </summary>
        /// <param name="detail">Details of journey instruction</param>
        /// <returns>The road name</returns>
        protected virtual string FormatRoadName(RoadJourneyDetail detail)
        {
            string roadName = detail.RoadName;
            string roadNumber = detail.RoadNumber;
            string result = String.Empty;

            if ((roadName == null || roadName.Length == 0) &&
                (roadNumber == null || roadNumber.Length == 0))
            {
                return localRoad;
            }

            if (roadNumber != null && roadNumber.Length != 0)
            {
                result += roadNumber;
            }

            if (roadName != null && roadName.Length != 0)
            {
                // Check to see if road number was empty and if so
                // then need to bracket the road name.

                if (roadNumber != null && roadNumber.Length != 0)
                {
                    result += space + "(" + roadName + ")";
                }
                else
                {
                    // No road number exists so just concatinate the road name
                    result += roadName;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns formatted string of the road name for the supplied
        /// instruction.
        /// </summary>
        /// <param name="detail">Array index into roadJourney.Details for journey instruction</param>
        /// <returns>The road name</returns>
        protected virtual string GetRoadName(int journeyDetailIndex)
        {
            RoadJourneyDetail detail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;
            string result = FormatRoadName(detail);
            return result;
        }


        /// <summary>
        /// Returns a formatted string of the arrival time for the current
        /// driving instruction in the format defined by the method FormatTime.
        /// The time is calculated by adding the current journey
        /// instruction duration to an accumulated journey time.
        /// </summary>
        /// <param name="detail">Array index into roadJourney.Details for journey instruction</param>
        /// <returns>Formatted arrival time</returns>
        protected override DateTime GetArrivalTime(int journeyDetailIndex)
        {
            RoadJourneyDetail detail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;
            //If it is a Stopover (unless a wait) We don't want to add the time

            if (detail.FerryEntry)
            {
                TimeSpan span = new TimeSpan(0, 0, (int)detail.Duration);
                DateTime arrivalTime = currentArrivalTime.Subtract(span);
                return arrivalTime;

            }

            else if (detail.IsStopOver && !detail.Wait)
            {
                return currentArrivalTime;
            }
            else
            {
                TimeSpan span = new TimeSpan(0, 0, (int)detail.Duration);
                DateTime arrivalTime = currentArrivalTime;
                currentArrivalTime = currentArrivalTime.Add(span);
                return arrivalTime;
            }
        }

       
        /// <summary>
        /// Returns a formatted string of the directions for the current
        /// driving instruction e.g. "Continue on to FOXHUNT GROVE".
        /// </summary>
        /// <param name="detail">Array index into journey.Details for journey instruction</param>
        /// <returns>Formatted string of the directions</returns>
        protected virtual string GetDirections(int journeyDetailIndex, bool showCongestionCharge)
        {
            //temp variables for this method
            string routeText = String.Empty;

            //assign the detail to be formatted as an instruction
            RoadJourneyDetail detail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;
            //last Detail, which is also needed for formatting logic			
            RoadJourneyDetail lastDetail = journey.JourneyDetails[journey.JourneyDetails.Count - 1] as RoadJourneyDetail;

            bool nextDetailHasJunctionExitJunction = false;

            if (detail != lastDetail)
            {
                RoadJourneyDetail nextDetail = detail;
                nextDetail = journey.JourneyDetails[journeyDetailIndex + 1] as RoadJourneyDetail;
                nextDetailHasJunctionExitJunction = ((nextDetail.IsJunctionSection) &&
                    ((nextDetail.JunctionAction == JunctionType.Exit) ||
                    (nextDetail.JunctionAction == JunctionType.Merge)));
            }



            //Returns specific text depending on the StopoverSection
            if (detail.IsStopOver)
            {
                if (detail.CongestionZoneEntry != null)
                {
                    routeText = CongestionEntry(detail, showCongestionCharge);
                }
                if (detail.CongestionZoneExit != null)
                {
                    routeText = CongestionExit(detail, showCongestionCharge);
                }
                if (detail.CongestionZoneEnd != null)
                {
                    routeText = CongestionEnd(detail, showCongestionCharge);
                }
                if (detail.FerryCheckIn != null)
                {
                    //We need to pass the previous detail so we can find out if it was 
                    //an UndefinedWait or not (as this changes the instruction passed to the user)
                    RoadJourneyDetail previousDetail = journey.JourneyDetails[journeyDetailIndex - 1] as RoadJourneyDetail;
                    routeText = FerryEntry(detail, previousDetail);
                }
                if (detail.TollEntry != null)
                {
                    routeText = TollEntry(detail);
                }
                if (detail.TollExit != null)
                {
                    routeText = TollExit(detail);
                }
               
                if (detail.FerryCheckOut == true)
                {
                    routeText = FerryExit(detail);
                }
                if (detail.Wait == true)
                {
                    RoadJourneyDetail previousDetail = journey.JourneyDetails[journeyDetailIndex - 1] as RoadJourneyDetail;
                    RoadJourneyDetail nextDetail = journey.JourneyDetails[journeyDetailIndex + 1] as RoadJourneyDetail;
                    routeText = WaitForFerry(detail, previousDetail, nextDetail);
                }
                if (detail.UndefindedWait == true)
                {
                    RoadJourneyDetail previousDetail = journey.JourneyDetails[journeyDetailIndex - 1] as RoadJourneyDetail;
                    RoadJourneyDetail nextDetail = journey.JourneyDetails[journeyDetailIndex + 1] as RoadJourneyDetail;
                    routeText = UndefindedFerryWait(detail, previousDetail, nextDetail);
                }
                if (detail.LimitedAccessRestriction)
                {
                    routeText = LimitedAccessRestriction(detail);
                }
            }
            else
            {
                //check turns and format accordingly			
                if (detail.Roundabout)
                    routeText = Roundabout(detail);
                else if (detail.ThroughRoute)
                    routeText = ThroughRoute(detail);
                else
                {
                    // Not a roundabout or a through route.  
                    if (detail.Angle == TurnAngle.Continue)
                        routeText = TurnAngleContinue(journeyDetailIndex);
                    else if (detail.Angle == TurnAngle.Bear)
                        routeText = TurnAngleBear(journeyDetailIndex);
                    else if (detail.Angle == TurnAngle.Turn)
                        routeText = TurnAngleTurn(journeyDetailIndex);
                }

                //check if the current road journey detail is a motorway junction section
                if (detail.IsJunctionSection)
                {
                    //add junction number to the instruction
                    routeText = FormatMotorwayJunction(detail, routeText);
                }

                //Add formatting place holder for where the road splits
                routeText += "{0}";

                //check if place name was returned
                bool placeNameExists = false;
                if (detail.PlaceName != null && detail.PlaceName.Length > 0)
                {
                    if ((detail.JunctionAction == JunctionType.Entry) && detail.IsJunctionSection &&
                        detail.Roundabout && (detail.Distance < immediateTurnDistance) &&
                        nextDetailHasJunctionExitJunction)
                    {
                        //don't add place name
                    }
                    else
                    {
                        routeText = AddPlaceName(detail, routeText);
                    }
                    placeNameExists = true;
                }

                //Add formatting place holder for where the road splits
                routeText += "{1}";

                if (detail.IsJunctionSection &&
                    ((detail.JunctionAction == JunctionType.Exit) ||
                    (detail.JunctionAction == JunctionType.Merge)) &&
                    (detail.Distance > immediateTurnDistance))
                {
                    //don't add continue for
                }
                else
                {
                    //Add 'continue for' 
                    routeText = AddContinueFor(detail, nextDetailHasJunctionExitJunction, routeText);
                }

                //Add formatting place holder for where the road splits
                routeText += "{2}";

                //check the next Detail providing we are not on the last detail already 

                if (detail != lastDetail)
                {
                    //next Detail, which is needed for formatting logic	
                    RoadJourneyDetail nextDetail = journey.JourneyDetails[journeyDetailIndex + 1] as RoadJourneyDetail;

                    if (!((detail.JunctionAction == JunctionType.Exit) ||
                        (detail.JunctionAction == JunctionType.Merge)))
                    {
                        //amend the current instruction if necessary
                        routeText = CheckNextDetail(nextDetail, routeText);
                    }
                }
                //Add formatting place holder for where the road splits
                routeText += "{3}";
                                                

                // Add the distance to the running count
                accumulatedDistance += detail.Distance;

                //Add where the road splits to the appropriate place


                //Fille in the place holders for where the road splits for Motorway Entry
                if ((detail.JunctionAction == JunctionType.Entry) &&
                    (detail.IsJunctionSection) && (detail.RoadSplits))
                {
                    if (detail.Roundabout)
                    {
                        //after place name
                        if (placeNameExists && (detail.Distance > immediateTurnDistance))
                        {
                            routeText = String.Format(routeText,
                                String.Empty, space + whereRoadSplits, String.Empty, String.Empty);
                        }
                        //after jno2
                        else if ((!placeNameExists) && (detail.Distance < immediateTurnDistance) &&
                            nextDetailHasJunctionExitJunction)
                        {
                            routeText = String.Format(routeText,
                                String.Empty, String.Empty, String.Empty, space + whereRoadSplits);
                        }
                        else routeText = String.Format(routeText, space + whereRoadSplits, String.Empty,
                                 String.Empty, String.Empty);
                    }
                    else
                    {
                        if (nextDetailHasJunctionExitJunction && (!placeNameExists) &&
                            (detail.Distance < immediateTurnDistance))
                        {
                            routeText = String.Format(routeText,
                                String.Empty, String.Empty, String.Empty, space + whereRoadSplits);
                        }
                        else routeText = String.Format(routeText, space + whereRoadSplits,
                                 String.Empty, String.Empty, String.Empty);
                    }
                }
                else routeText = String.Format(routeText, String.Empty,
                         String.Empty, String.Empty, String.Empty);
            }

            //return complete formatted instruction
            return routeText;

        }

       
        /// <summary>
        /// Return a high value motorway symbol next to the route text.
        /// </summary>
        /// <param name="routeText">Route directions</param>
        /// <returns></returns>
        protected virtual string AddHighValueMotorwaySymbol(string routeText)
        {
            routeText += space + GetResourceString("CarJourneyDetailsTableControl.HighValueMotorway.Image");
            return routeText;
        }

        /// <summary>
        /// Returns the distance in metres appended to the route text (if the isCJPUser flag is set)
        /// </summary>
        /// <param name="routeText"></param>
        /// <returns></returns>
        protected virtual string AddDirectionDistanceMetres(string routeText, int journeyDetailIndex)
        {
            if (this.isCJPUser)
            {
                RoadJourneyDetail detail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;

                string distanceMetres = "<span class=\"txterror\"> (" + detail.Distance + " metres)</span>";

                routeText += distanceMetres;
            }

            return routeText;
        }


        /// <summary>
        /// Generates the route text for the given RoadJourney where
        /// the RoadJourney type is "Roundabout". This method assumes
        /// that the type is "Roundabout".
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string Roundabout(RoadJourneyDetail detail)
        {
            string routeText = String.Empty;
            string thisRoad = FormatRoadName(detail);

            switch (detail.TurnCount)
            {
                case 1: routeText = roundaboutExitOne; break;
                case 2: routeText = roundaboutExitTwo; break;
                case 3: routeText = roundaboutExitThree; break;
                case 4: routeText = roundaboutExitFour; break;
                case 5: routeText = roundaboutExitFive; break;
                case 6: routeText = roundaboutExitSix; break;
                case 7: routeText = roundaboutExitSeven; break;
                case 8: routeText = roundaboutExitEight; break;
                case 9: routeText = roundaboutExitNine; break;
                case 10: routeText = roundaboutExitTen; break;
            }

            routeText += space + thisRoad;

            if (detail.RoadSplits && !detail.IsJunctionSection)
            {
                routeText += space + whereRoadSplits;
            }

            return routeText;
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail type is "ThroughRoute". This method assumes
        /// that the type is "ThroughRoute".
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string ThroughRoute(RoadJourneyDetail detail)
        {
            string roadName = FormatRoadName(detail);
            string routeText = String.Empty;

            //add "Follow..." to start of instruction			
            routeText = follow + space + roadName;

            if (detail.RoadSplits)
            {
                routeText += space + whereRoadSplits;
            }
            return routeText;
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'TollEntry'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string TollEntry(RoadJourneyDetail detail)
        {
            StringBuilder routeText = new StringBuilder(string.Empty);

            //Convert TollCost value to pounds & pence
            decimal pence = (Convert.ToDecimal(detail.TollCost)) / 100;
            string cost = string.Format("{0:C}", pence);

            //display company url as hyerplink if it exists
            if (detail.CompanyUrl.Length != 0 && print)
            {
                //add "Toll:" to start of instruction
                routeText.Append(toll);
                routeText.Append(space);
                routeText.Append("<a href=\"");
                routeText.Append("http://");
                routeText.Append(detail.CompanyUrl.Trim());
                routeText.Append("\" target=\"_blank\">");
                routeText.Append(detail.TollEntry);
                routeText.Append(space);
                routeText.Append(openNewWindowImageUrl);
                routeText.Append("</a>");
            }
            else
            {
                routeText.Append(toll);
                routeText.Append(space);
                routeText.Append(detail.TollEntry);
            }

            if (displayTollCharge)
            {
                //If price in pence is less than zero return the route text, otherwise
                //return route text inclusive of the charge.
                if (pence < 0)
                {
                    //IR2499 added in message when charge is unknown "Charge: Not Available"
                    routeText.Append(space);
                    routeText.Append("<b>");
                    routeText.Append(charge);
                    routeText.Append(notAvailable);
                    routeText.Append("</b>");
                }
                else if (pence == 0)
                {
                    routeText.Append(space);
                    routeText.Append("<b>");
                    routeText.Append(charge);
                    routeText.Append("£0.00");
                    routeText.Append("</b>");
                }
                else
                {
                    routeText.Append(space);
                    routeText.Append("<b>");
                    routeText.Append(charge);
                    routeText.Append(cost);
                    routeText.Append("</b>");
                }
            }

            return routeText.ToString();
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'TollEntry'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string TollExit(RoadJourneyDetail detail)
        {
            StringBuilder routeText = new StringBuilder(string.Empty);

            //Convert TollCost value to pounds & pence
            decimal pence = (Convert.ToDecimal(detail.TollCost)) / 100;
            string cost = string.Format("{0:C}", pence);

            //display company url as hyerplink if it exists
            if (detail.CompanyUrl.Length != 0 && print)
            {
                //add "Toll:" to start of instruction	
                routeText.Append(toll);
                routeText.Append(space);
                routeText.Append("<a href=\"");
                routeText.Append("http://");
                routeText.Append(detail.CompanyUrl.Trim());
                routeText.Append("\" target=\"_blank\">");
                routeText.Append(detail.TollExit);
                routeText.Append(space);
                routeText.Append(openNewWindowImageUrl);
                routeText.Append("</a>");
            }
            else
            {
                routeText.Append(toll);
                routeText.Append(space);
                routeText.Append(detail.TollExit);
            }

            if (displayTollCharge)
            {
                routeText.Append(space);
                routeText.Append("<b>");
                routeText.Append(charge);
            }

            return routeText.ToString();
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'FerryEntry'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string FerryEntry(RoadJourneyDetail detail, RoadJourneyDetail PreviousDetail)
        {

            bool previousInstruction = PreviousDetail.UndefindedWait;

            StringBuilder routeText = new StringBuilder(string.Empty);
            decimal pence = (Convert.ToDecimal(detail.TollCost)) / 100;
            string cost = string.Format("{0:C}", pence);

            TimeSpan span = new TimeSpan(0, 0, (int)detail.Duration);
            DateTime arrivalTime = currentArrivalTime;
            currentArrivalTime = currentArrivalTime.Add(span);
            string time = FormatDateTime(currentArrivalTime);

            //add "Board:" to start of instruction			
            //If previous instruction was an undefinedWait we don't want to display a time(as we don't know it)
            if (previousInstruction)
            {
                //display company url as hyerplink if it exists
                if (detail.CompanyUrl.Length != 0 && print)
                {
                    routeText.Append(board);
                    routeText.Append(space);
                    routeText.Append("<a href=\"");
                    routeText.Append("http://");
                    routeText.Append(detail.CompanyUrl.Trim());
                    routeText.Append("\" target=\"_blank\">");
                    routeText.Append(detail.FerryCheckIn);
                    routeText.Append(space);
                    routeText.Append(openNewWindowImageUrl);
                    routeText.Append("</a>");
                }
                else
                {
                    routeText.Append(board);
                    routeText.Append(space);
                    routeText.Append(detail.FerryCheckIn);
                }
            }
            else
            {
                //display company url as hyerplink if it exists
                if (detail.CompanyUrl.Length != 0 && print)
                {
                    routeText.Append(board);
                    routeText.Append(space);
                    routeText.Append("<a href=\"");
                    routeText.Append("http://");
                    routeText.Append(detail.CompanyUrl.Trim());
                    routeText.Append("\" target=\"_blank\">");
                    routeText.Append(detail.FerryCheckIn);
                    routeText.Append(space);
                    routeText.Append(openNewWindowImageUrl);
                    routeText.Append("</a>");
                    routeText.Append(space);
                    routeText.Append(departingAt);
                    routeText.Append(space);
                    routeText.Append(time);
                }
                else
                {
                    routeText.Append(board);
                    routeText.Append(space);
                    routeText.Append(detail.FerryCheckIn);
                    routeText.Append(space);
                    routeText.Append(departingAt);
                    routeText.Append(space);
                    routeText.Append(time);
                }
            }

            //If price in pence is less than zero return the route text, otherwise
            //return route text inclusive of the charge.
            if (pence < 0)
            {
                // when charge is unknown "Charge: Not Available"
                routeText.Append(";");
                routeText.Append(space);
                routeText.Append("<b>");
                routeText.Append(charge);
                routeText.Append(notAvailable);
                routeText.Append("</b>");
            }
            else if (pence == 0)
            {
                // when charge is unknown "Charge: Not Available"
                routeText.Append(";");
                routeText.Append(space);
                routeText.Append("<b>");
                routeText.Append(charge);
                routeText.Append("£0.00");
                routeText.Append("</b>");
            }
            else
            {
                routeText.Append(";");
                routeText.Append(space);
                routeText.Append("<b>");
                routeText.Append(charge);
                routeText.Append(cost);
                routeText.Append("</b>");
            }
            return routeText.ToString();
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'CongestionEntry'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string CongestionEntry(RoadJourneyDetail detail, bool showCongestionCharge)
        {
            StringBuilder routeText = new StringBuilder(string.Empty);
            decimal pence = (Convert.ToDecimal(detail.TollCost)) / 100;
            string cost = string.Format("{0:C}", pence);

            //display company name as hyperplink if it exists
            if (detail.CompanyUrl.Length != 0 && print)
            {
                routeText.Append(enter);
                routeText.Append(space);
                routeText.Append("<a href=\"");
                routeText.Append("http://");
                routeText.Append(detail.CompanyUrl.Trim());
                routeText.Append("\" target=\"_blank\">");
                routeText.Append(detail.CongestionZoneEntry);
                routeText.Append(space);
                routeText.Append(openNewWindowImageUrl);
                routeText.Append("</a>");
            }
            else
            {
                routeText.Append(enter);
                routeText.Append(space);
                routeText.Append(detail.CongestionZoneEntry);
            }

            // If there is a toll charge
            // Amended for IR 2639 - Check if we already have this C Charge in ViewState or 
            // there is a cost and showCongestionCharge
            if ((detail.TollCost > 0) && ((showCongestionCharge) ||
                (!journeyViewState.VisitedCongestionCompany.Contains(detail.CompanyUrl))))
            {
                //Viewstate contains this C Charge and we don't want to show it
                if (journeyViewState.VisitedCongestionCompany.Contains(detail.CompanyUrl) && (!showCongestionCharge))
                {
                    // No toll charge for this day/time
                    routeText.Append(fullstop);
                    routeText.Append(space);

                    if (displayCongestionCharge)
                    {
                        routeText.Append("<b>");
                        routeText.Append(charge);
                        routeText.Append("£0.00");
                        routeText.Append("</b>");
                        routeText.Append(space);
                    }

                    routeText.Append(certainTimesNoCharge);
                }
                else

                //We want to show it
                {
                    journeyViewState.VisitedCongestionCompany.Add(detail.CompanyUrl);
                    routeText.Append(fullstop);
                    routeText.Append(space);

                    if (displayCongestionCharge)
                    {
                        routeText.Append("<b>");
                        routeText.Append(charge);
                        routeText.Append(cost);
                        routeText.Append("</b>");
                        routeText.Append(space);
                        routeText.Append(certainTimes);
                    }
                    else
                    {
                        routeText.Append(certainTimesNoCharge);
                    }

                    journeyViewState.CongestionChargeAdded = true;

                }


            }

            else if ((detail.TollCost == 0) || (!showCongestionCharge))
            {
                // No toll charge for this day/time
                routeText.Append(fullstop);
                routeText.Append(space);

                if (displayCongestionCharge)
                {
                    routeText.Append("<b>");
                    routeText.Append(charge);
                    routeText.Append("£0.00");
                    routeText.Append("</b>");
                    routeText.Append(space);
                }

                routeText.Append(certainTimesNoCharge);
            }
            else
            {
                if (displayCongestionCharge)
                {
                    //in the event the charge is unavailable - IR2499
                    routeText.Append(space);
                    routeText.Append("<b>");
                    routeText.Append(charge);
                    routeText.Append(notAvailable);
                    routeText.Append("</b>");
                }
            }

            //display company name as hyperplink if it exists
            string ccURL = detail.CompanyUrl.ToLower();
            if (ccURL.StartsWith("www.tfl.gov.uk") && londonCCzoneExtraTextVisible)
            {
                routeText.Append(londonCCAdditionalText);
            }

            return routeText.ToString();
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'CongestionExit'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string CongestionExit(RoadJourneyDetail detail, bool showCongestionCharge)
        {
            StringBuilder routeText = new StringBuilder(string.Empty);
            decimal pence = (Convert.ToDecimal(detail.TollCost)) / 100;
            string cost = string.Format("{0:C}", pence);

            if (detail.CompanyUrl.Length != 0 && print)
            {
                //add "Exit" to start of instruction						
                routeText.Append(exit);
                routeText.Append(space);
                routeText.Append("<a href=\"");
                routeText.Append("http://");
                routeText.Append(detail.CompanyUrl.Trim());
                routeText.Append("\" target=\"_blank\">");
                routeText.Append(detail.CongestionZoneExit);
                routeText.Append(space);
                routeText.Append(openNewWindowImageUrl);
                routeText.Append("</a>");
            }
            else
            {
                //add "Exit" to start of instruction						
                routeText.Append(exit);
                routeText.Append(space);
                routeText.Append(detail.CongestionZoneExit);
            }


            // If there is a toll charge
            // Amended for IR 2639 - Check if we already have this C Charge in ViewState or 
            // there is a cost and showCongestionCharge
            if ((detail.TollCost > 0) && ((showCongestionCharge) ||
                (!journeyViewState.VisitedCongestionCompany.Contains(detail.CompanyUrl))))
            {
                //Viewstate contains this C Charge and we don't want to show it
                if (journeyViewState.VisitedCongestionCompany.Contains(detail.CompanyUrl) && (!showCongestionCharge))
                {
                    // No toll charge for this day/time
                    routeText.Append(fullstop);
                    routeText.Append(space);

                    if (displayCongestionCharge)
                    {
                        routeText.Append("<b>");
                        routeText.Append(charge);
                        routeText.Append("£0.00");
                        routeText.Append("</b>");
                        routeText.Append(space);
                    }

                    routeText.Append(certainTimesNoCharge);
                }
                else
                //We want to show it
                {
                    journeyViewState.VisitedCongestionCompany.Add(detail.CompanyUrl);
                    routeText.Append(fullstop);
                    routeText.Append(space);

                    if (displayCongestionCharge)
                    {
                        routeText.Append("<b>");
                        routeText.Append(charge);
                        routeText.Append(cost);
                        routeText.Append("</b>");
                        routeText.Append(space);
                        routeText.Append(certainTimes);
                    }
                    else
                    {
                        routeText.Append(certainTimesNoCharge);
                    }

                   // if (outward)
                    //    journeyViewState.CongestionChargeAdded = true;

                }
            }
            else if ((detail.TollCost == 0) || (!showCongestionCharge))
            {
                // No toll charge, so dont want to display the charge text
            }
            else
            {
                if (displayCongestionCharge)
                {
                    //in the event the charge is unavailable - IR2499
                    routeText.Append(space);
                    routeText.Append("<b>");
                    routeText.Append(charge);
                    routeText.Append(notAvailable);
                    routeText.Append("</b>");
                }
            }

            return routeText.ToString();

        }
        
        /// <summary>
        /// 
        /// THIS needs to be activated when ATKINS CJP changes in place
        /// 
        /// 
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'CongestionEnd'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string CongestionEnd(RoadJourneyDetail detail, bool showCongestionCharge)
        {
            StringBuilder routeText = new StringBuilder(string.Empty);
            decimal pence = (Convert.ToDecimal(detail.TollCost)) / 100;
            string cost = string.Format("{0:C}", pence);

            if (detail.CompanyUrl.Length != 0 && print)
            {
                //add "End" to start of instruction						
                routeText.Append(end);
                routeText.Append(space);
                routeText.Append("<a href=\"");
                routeText.Append("http://");
                routeText.Append(detail.CompanyUrl.Trim());
                routeText.Append("\" target=\"_blank\">");
                routeText.Append(detail.CongestionZoneEnd);
                routeText.Append(space);
                routeText.Append(openNewWindowImageUrl);
                routeText.Append("</a>");
            }
            else
            {
                //add "End" to start of instruction						
                routeText.Append(end);
                routeText.Append(space);
                routeText.Append(detail.CongestionZoneEnd);
            }


            // If there is a toll charge
            // Amended for IR 2639 - Check if we already have this C Charge in ViewState or 
            // there is a cost and showCongestionCharge
            if ((detail.TollCost > 0) && ((showCongestionCharge) ||
                (!journeyViewState.VisitedCongestionCompany.Contains(detail.CompanyUrl))))
            {
                //Viewstate contains this C Charge and we don't want to show it
                if (journeyViewState.VisitedCongestionCompany.Contains(detail.CompanyUrl) && (!showCongestionCharge))
                {
                    // No toll charge for this day/time
                    routeText.Append(fullstop);
                    routeText.Append(space);

                    if (displayCongestionCharge)
                    {
                        routeText.Append("<b>");
                        routeText.Append(charge);
                        routeText.Append("£0.00");
                        routeText.Append("</b>");
                        routeText.Append(space);
                    }

                    routeText.Append(certainTimesNoCharge);
                }
                else
                //We want to show it
                {
                    journeyViewState.VisitedCongestionCompany.Add(detail.CompanyUrl);
                    routeText.Append(fullstop);
                    routeText.Append(space);

                    if (displayCongestionCharge)
                    {
                        routeText.Append("<b>");
                        routeText.Append(charge);
                        routeText.Append(cost);
                        routeText.Append("</b>");
                        routeText.Append(space);
                        routeText.Append(certainTimes);
                    }
                    else
                    {
                        routeText.Append(certainTimesNoCharge);
                    }

                   journeyViewState.CongestionChargeAdded = true;

                }
            }
            else if ((detail.TollCost == 0) || (!showCongestionCharge))
            {
                // No toll charge, so dont want to display the charge text
            }
            else
            {
                if (displayCongestionCharge)
                {
                    //in the event the charge is unavailable - IR2499
                    routeText.Append(space);
                    routeText.Append("<b>");
                    routeText.Append(charge);
                    routeText.Append(notAvailable);
                    routeText.Append("</b>");
                }
            }

            return routeText.ToString();

        }
        
        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'FerryExit'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string FerryExit(RoadJourneyDetail detail)
        {
            TimeSpan span = new TimeSpan(0, 0, (int)detail.Duration);
            currentArrivalTime = currentArrivalTime.Add(span);

            string routeText = String.Empty;

            routeText = leaveFerry;
            return routeText;
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'Wait'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string WaitForFerry(RoadJourneyDetail detail, RoadJourneyDetail previousDetail, RoadJourneyDetail nextDetail)
        {
            string routeText = String.Empty;

            if (nextDetail.RoadNumber == "FERRY" && previousDetail.RoadNumber == "FERRY")
            {
                routeText = IntermediateFerryWait;
            }
            else
            {
                if (previousDetail.FerryCheckOut)
                {
                    routeText = WaitAtTerminal;
                }
                else
                {
                    routeText = FerryWait;
                }
            }
            return routeText;
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'UndefindedWait'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string UndefindedFerryWait(RoadJourneyDetail detail, RoadJourneyDetail previousDetail, RoadJourneyDetail nextDetail)
        {
            string routeText = String.Empty;

            if (nextDetail.RoadNumber == "FERRY" && previousDetail.RoadNumber == "FERRY")
            {
                routeText = IntermediateFerryWait;
            }
            else
            {
                if (previousDetail.FerryCheckOut)
                {
                    routeText = WaitAtTerminal;
                }
                else
                {
                    routeText = UnspecifedFerryWait;
                }
            }
            return routeText;
        }

        /// <summary>
        /// Generates the route text for the given RoadJourneyDetail where
        /// the RoadJourneyDetail is a StopoverSection of type 'LimitedAccessRestriction'.
        /// </summary>
        /// <param name="detail">Road Journey Detail to generate route text for.</param>
        /// <returns>Route text.</returns>
        protected virtual string LimitedAccessRestriction(RoadJourneyDetail detail)
        {
            TimeSpan span = new TimeSpan(0, 0, (int)detail.Duration);
            currentArrivalTime = currentArrivalTime.Add(span);

            return detail.Restriction;
        }

        /// <summary>
        /// Generate the text where the turn angle of the given
        /// RoadJourneyDetail is "Continue". This method assumes that
        /// the turn angle for the given detail is "Continue".
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into roadJourney.Details</param>
        /// <returns>Route text.</returns>
        protected virtual string TurnAngleContinue(int journeyDetailIndex)
        {
            RoadJourneyDetail detail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;

            string nextRoad = FormatRoadName(detail);
            string routeText = String.Empty;
            bool addedWhereRoadSplits = false;


            //When the CJP produces a JunctionDriveSection of type Merge (or Exit) 
            //without a junction number (regardless of whether the junction is indicated as ambiguous) 
            //for a Motorway junction (including slip roads) then ignore the turn count 
            //and the immediate parameter. 			
            if (detail.JunctionDriveSectionWithoutJunctionNo &&
                (detail.JunctionAction == JunctionType.Exit || detail.JunctionAction == JunctionType.Merge))
            {
                string straightOnInstruction =
                    ((detail.PlaceName == null || detail.PlaceName.Length == 0) &&
                    (detail.Distance > immediateTurnDistance) && detail.RoadSplits) ?
                        String.Empty : space + straightOn;

                if (detail.Direction == TurnDirection.Continue)
                    routeText = follow + space + nextRoad + straightOnInstruction;
                else if (detail.Direction == TurnDirection.Right)
                    routeText = follow + space + nextRoad + straightOnInstruction;
                else if (detail.Direction == TurnDirection.Left)
                    routeText = follow + space + nextRoad + straightOnInstruction;
            }
            else if (!detail.IsJunctionSection)
            {
                if ((detail.Direction == TurnDirection.Continue) ||
                    (detail.Direction == TurnDirection.Left) ||
                    (detail.Direction == TurnDirection.Right))
                {

                    if ((detail.PlaceName == null || detail.PlaceName.Length == 0) &&
                        (detail.Distance > immediateTurnDistance) && detail.RoadSplits)
                    {
                        routeText = follow + space + nextRoad;
                    }
                    else
                    {
                        routeText = follow + space + nextRoad + space + straightOn;

                    }
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutContinue)
                {
                    routeText = atMiniRoundabout + follow.ToLower(CultureInfo.CurrentCulture) +
                        space + nextRoad + space + straightOn;
                }
                else
                {
                    // Special case if no place name, < immediate distance and road splits
                    if ((detail.PlaceName == null || detail.PlaceName.Length == 0) &&
                        (detail.Distance < immediateTurnDistance) && detail.RoadSplits)
                    {
                        if (detail.Direction == TurnDirection.MiniRoundaboutLeft)
                        {
                            routeText = leftMiniRoundabout2 + space + whereRoadSplits + space + onTo + space + nextRoad;
                        }
                        else if (detail.Direction == TurnDirection.MiniRoundaboutRight)
                        {
                            routeText = rightMiniRoundabout2 + space + whereRoadSplits + space + onTo + space + nextRoad;
                        }
                        else if (detail.Direction == TurnDirection.MiniRoundaboutReturn)
                        {
                            routeText = uTurnMiniRoundabout2 + space + whereRoadSplits + space + onTo + space + nextRoad;
                        }
                        // Set flag so dont add "where road splits" again at end of string
                        addedWhereRoadSplits = true;
                    }
                    else
                    {
                        if (detail.Direction == TurnDirection.MiniRoundaboutLeft)
                        {
                            routeText = leftMiniRoundabout + space + nextRoad;
                        }
                        else if (detail.Direction == TurnDirection.MiniRoundaboutRight)
                        {
                            routeText =
                                rightMiniRoundabout + space + nextRoad;
                        }
                        else if (detail.Direction == TurnDirection.MiniRoundaboutReturn)
                        {
                            routeText =
                                uTurnMiniRoundabout + space + nextRoad;
                        }

                    }
                }
            }
            else if (detail.IsJunctionSection)
            {
                //Motorway entry 
                if ((detail.JunctionAction == JunctionType.Entry) &&
                    (detail.JunctionNumber != null) && (detail.JunctionNumber.Length > 0))
                {
                    routeText = join + space + nextRoad;
                    return routeText; //without where the road splits
                }

                if (detail.Direction == TurnDirection.Continue)
                {
                    routeText = (detail.RoadSplits) ? follow + space + nextRoad + space + straightOn :
                        join + space + nextRoad;
                }
                else if (detail.Direction == TurnDirection.Right)
                {
                    routeText = (detail.RoadSplits) ? follow + space + nextRoad + space + straightOn :
                        join + space + nextRoad;
                }
                else if (detail.Direction == TurnDirection.Left)
                {
                    routeText = (detail.RoadSplits) ? follow + space + nextRoad + space + straightOn :
                        join + space + nextRoad;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutContinue)
                {
                    routeText = (detail.RoadSplits) ?
                        atMiniRoundabout + follow.ToLower(CultureInfo.CurrentCulture) +
                        space + nextRoad + space + straightOn :
                        continueMiniRoundabout + space + nextRoad;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutLeft)
                {
                    routeText = leftMiniRoundabout + space + nextRoad;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutRight)
                {
                    routeText = rightMiniRoundabout + space + nextRoad;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutReturn)
                {
                    routeText = uTurnMiniRoundabout + space + nextRoad;
                }



            }
            //if detail does not include a motorway junction

            if (detail.RoadSplits && !addedWhereRoadSplits)
            {
                routeText += space + whereRoadSplits;
            }

            return routeText;
        }

        /// <summary>
        /// Generates the route text for the given RoadJourney where
        /// the TurnAngle for the given RoadJourney is "Bear".  This method
        /// assumes that the TurnAngle is "Bear".
        ///
        /// Note that for "counted turns" (TurnCount 1 - 4) the text is actually
        /// "Take the ..." which can be used for both "bear" and "turn".
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into roadJourney.Details</param>
        /// <returns>Route text.</returns>
        protected virtual string TurnAngleBear(int journeyDetailIndex)
        {
            RoadJourneyDetail detail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;

            int previousDistance = 0;
            if (journeyDetailIndex > 0)
            {
                previousDistance = journey.JourneyDetails[journeyDetailIndex - 1].Distance;
            };

            string nextRoad = FormatRoadName(detail);
            string routeText = String.Empty;

            //When the CJP produces a JunctionDriveSection of type Merge (or Exit) 
            //without a junction number (regardless of whether the junction is indicated as ambiguous) 
            //for a Motorway junction (including slip roads) then ignore the turn count 
            //and the immediate parameter. 
            if (detail.JunctionDriveSectionWithoutJunctionNo &&
                (detail.JunctionAction == JunctionType.Exit || detail.JunctionAction == JunctionType.Merge))
            {
                if (detail.Direction == TurnDirection.Continue)
                {
                    string straightOnInstruction = space + straightOn;

                    if (detail.Direction == TurnDirection.Continue)
                        routeText = follow + space + nextRoad + straightOnInstruction;
                    else if (detail.Direction == TurnDirection.Right)
                        routeText = follow + space + nextRoad + straightOnInstruction;
                    else if (detail.Direction == TurnDirection.Left)
                        routeText = follow + space + nextRoad + straightOnInstruction;
                }
                else
                {
                    if (detail.Direction == TurnDirection.Left)
                        routeText = bearLeft + space + nextRoad;
                    else if (detail.Direction == TurnDirection.Right)
                        routeText = bearRight + space + nextRoad;
                }

                if (detail.RoadSplits)
                    routeText += space + whereRoadSplits;

                return routeText;
            }

            // Check to see if this detail is a junction drive section
            if (detail.IsJunctionSection)
            {
                if (detail.Direction == TurnDirection.Left)
                {
                    //Motorway entry 
                    if ((detail.JunctionAction == JunctionType.Entry) &&
                        (detail.JunctionNumber != null) && (detail.JunctionNumber.Length > 0))
                    {
                        routeText = bearLeftToJoin;
                    }

                }
                else if (detail.Direction == TurnDirection.Right)
                {
                    //Motorway entry 
                    if ((detail.JunctionAction == JunctionType.Entry) &&
                        (detail.JunctionNumber != null) && (detail.JunctionNumber.Length > 0))
                    {
                        routeText = bearRightToJoin;
                    }

                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutLeft)
                    routeText = leftMiniRoundabout;
                else if (detail.Direction == TurnDirection.MiniRoundaboutRight)
                    routeText = rightMiniRoundabout;
                else if (detail.Direction == TurnDirection.Continue)
                    //check flag for ambiguous junction
                    routeText = (detail.RoadSplits) ? continueString : join;
                else if (detail.Direction == TurnDirection.MiniRoundaboutReturn)
                    routeText = uTurnMiniRoundabout;

                // Append the next road to the route text
                routeText += space + nextRoad;
            }
            //this detail is a drive section
            else
            {
                if (detail.Direction == TurnDirection.Left)
                {
                    switch (detail.TurnCount)
                    {
                        case 1:

                            // Check to see if the distance of this leg is less than the
                            // immediate turn distance.
                            if (journeyDetailIndex > 0)
                            {
                                if (previousDistance < immediateTurnDistance)
                                    routeText = immediatelyBearLeft;
                                else
                                    routeText = turnLeftOne;
                            }
                            else
                            {
                                routeText = turnLeftOne;
                            }

                            break;

                        case 2: routeText = turnLeftTwo; break;
                        case 3: routeText = turnLeftThree; break;
                        case 4: routeText = turnLeftFour; break;

                        default: routeText = bearLeft; break;
                    }
                }
                else if (detail.Direction == TurnDirection.Right)
                {
                    switch (detail.TurnCount)
                    {
                        case 1:

                            // Check to see if the distance of this leg is less than the
                            // immediate turn distance.
                            if (journeyDetailIndex > 0)
                            {
                                if (previousDistance < immediateTurnDistance)
                                    routeText = immediatelyBearRight;
                                else
                                    routeText = turnRightOne;
                            }
                            else
                            {
                                routeText = turnRightOne;
                            }
                            break;

                        case 2: routeText = turnRightTwo; break;
                        case 3: routeText = turnRightThree; break;
                        case 4: routeText = turnRightFour; break;

                        default: routeText = bearRight; break;
                    }
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutContinue)
                    routeText = atMiniRoundabout + ChangeFirstCharacterCapitalisation(follow, false);
                else if (detail.Direction == TurnDirection.MiniRoundaboutLeft)
                    routeText = leftMiniRoundabout;
                else if (detail.Direction == TurnDirection.MiniRoundaboutRight)
                    routeText = rightMiniRoundabout;
                else if (detail.Direction == TurnDirection.Continue)
                {
                    if ((detail.PlaceName == null || detail.PlaceName.Length == 0) &&
                        (detail.Distance > immediateTurnDistance) && detail.RoadSplits)
                    {
                        routeText = follow + space + to;
                    }
                    else
                    {
                        routeText = follow;
                    }
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutReturn)
                    routeText = uTurnMiniRoundabout;

                // Append the next road to the route text
                routeText += space + nextRoad;

                if ((detail.Direction == TurnDirection.Continue) ||
                    (detail.Direction == TurnDirection.MiniRoundaboutContinue))
                    routeText += space + straightOn;

                if (detail.RoadSplits)
                {
                    if ((detail.TurnCount > 4) &&
                        ((detail.Direction == TurnDirection.Left) || (detail.Direction == TurnDirection.Right)))
                        routeText = ChangeFirstCharacterCapitalisation(whereRoadSplits, true) + space +
                            ChangeFirstCharacterCapitalisation(routeText, false);
                    else routeText += space + whereRoadSplits;
                }

            }



            return routeText;
        }

        /// <summary>
        /// Generates the route text for the given RoadJourney where
        /// the TurnAngle for the given RoadJourney is "Turn".  This method
        /// assumes that the TurnAngle is "Turn".
        ///
        /// Note that for "counted turns" (TurnCount 1 - 4) the text is actually
        /// "Take the ..." which can be used for both "bear" and "turn".
        /// </summary>
        /// <param name="journeyDetailIndex">Array index into roadJourney.Details</param>
        /// <returns>Route text.</returns>
        protected virtual string TurnAngleTurn(int journeyDetailIndex)
        {

            RoadJourneyDetail detail = journey.JourneyDetails[journeyDetailIndex] as RoadJourneyDetail;

            int previousDistance = 0;
            if (journeyDetailIndex > 0)
            {
                previousDistance = journey.JourneyDetails[journeyDetailIndex - 1].Distance;
            };

            string routeText = String.Empty;
            string nextRoad = FormatRoadName(detail);

            //When the CJP produces a JunctionDriveSection of type Merge (or Exit) 
            //without a junction number (regardless of whether the junction is indicated as ambiguous) 
            //for a Motorway junction (including slip roads) then ignore the turn count 
            //and the immediate parameter. 
            if (detail.JunctionDriveSectionWithoutJunctionNo &&
                (detail.JunctionAction == JunctionType.Exit || detail.JunctionAction == JunctionType.Merge))
            {
                if (detail.Direction == TurnDirection.Continue)
                {
                    string straightOnInstruction = space + straightOn;

                    if (detail.Direction == TurnDirection.Continue)
                        routeText = follow + space + nextRoad + straightOnInstruction;
                    else if (detail.Direction == TurnDirection.Right)
                        routeText = follow + space + nextRoad + straightOnInstruction;
                    else if (detail.Direction == TurnDirection.Left)
                        routeText = follow + space + nextRoad + straightOnInstruction;

                    if (detail.RoadSplits)
                        routeText += space + whereRoadSplits;
                }
                else
                {
                    if (detail.Direction == TurnDirection.Left)
                        routeText = turnLeftInDistance + space + nextRoad;
                    else if (detail.Direction == TurnDirection.Right)
                        routeText = turnRightInDistance + space + nextRoad;

                    if (detail.RoadSplits)
                        routeText = ChangeFirstCharacterCapitalisation(whereRoadSplits, true) + space +
                            ChangeFirstCharacterCapitalisation(routeText, false);
                }



                return routeText;
            }

            // Check to see if this detail is a junction drive section
            if (detail.IsJunctionSection)
            {
                if (detail.Direction == TurnDirection.Left)
                {
                    //Motorway entry 
                    if ((detail.JunctionAction == JunctionType.Entry) &&
                        (detail.JunctionNumber != null) && (detail.JunctionNumber.Length > 0))
                    {
                        routeText = turnLeftToJoin;
                    }

                }
                else if (detail.Direction == TurnDirection.Right)
                {
                    //Motorway entry 
                    if ((detail.JunctionAction == JunctionType.Entry) &&
                        (detail.JunctionNumber != null) && (detail.JunctionNumber.Length > 0))
                    {
                        routeText = turnRightToJoin;
                    }
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutLeft)
                {
                    routeText = leftMiniRoundabout;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutRight)
                {
                    routeText = rightMiniRoundabout;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutReturn)
                {
                    routeText = uTurnMiniRoundabout;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutContinue)
                {
                    routeText = (detail.RoadSplits) ?
                        atMiniRoundabout + follow.ToLower(CultureInfo.CurrentCulture) +
                        space + nextRoad + space + straightOn :
                        continueString;
                }
                else if (detail.Direction == TurnDirection.Continue)
                {
                    routeText = continueString;
                }

                // Append the next road to the route text.
                routeText += space + nextRoad;
            }
            //this detail is a drive section
            else
            {

                if (detail.Direction == TurnDirection.Left)
                {
                    switch (detail.TurnCount)
                    {
                        case 1:

                            // Check to see if the distance of this leg is less than the
                            // immediate turn distance.
                            if (journeyDetailIndex > 0)
                            {
                                if (previousDistance < immediateTurnDistance)
                                    routeText = immediatelyTurnLeftOnto;
                                else
                                    routeText = turnLeftOne;
                            }
                            else
                            {
                                routeText = turnLeftOne;
                            }

                            break;

                        case 2: routeText = turnLeftTwo; break;
                        case 3: routeText = turnLeftThree; break;
                        case 4: routeText = turnLeftFour; break;

                        // Greater than 4 - assuming that the turn count is never 0 when
                        // turn angle is Turn.
                        default: routeText = turnLeftInDistance; break;
                    }
                }
                else if (detail.Direction == TurnDirection.Right)
                {
                    switch (detail.TurnCount)
                    {
                        case 1:

                            // Check to see if the distance of this leg is less than the
                            // immediate turn distance.
                            if (journeyDetailIndex > 0)
                            {
                                if (previousDistance < immediateTurnDistance)
                                    routeText = immediatelyTurnRightOnto;
                                else
                                    routeText = turnRightOne;
                            }
                            else
                            {
                                routeText = turnRightOne;
                            }

                            break;

                        case 2: routeText = turnRightTwo; break;
                        case 3: routeText = turnRightThree; break;
                        case 4: routeText = turnRightFour; break;

                        // Greater than 4 - assuming that the turn count is never 0 when
                        // turn angle is Turn.
                        default: routeText = turnRightInDistance; break;
                    }
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutLeft)
                {
                    routeText = leftMiniRoundabout;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutRight)
                {
                    routeText = rightMiniRoundabout;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutReturn)
                {
                    routeText = uTurnMiniRoundabout;
                }
                else if (detail.Direction == TurnDirection.MiniRoundaboutContinue)
                {
                    routeText = atMiniRoundabout + ChangeFirstCharacterCapitalisation(follow, false);
                }
                else if (detail.Direction == TurnDirection.Continue)
                {
                    routeText = follow;
                }

                // Append the next road to the route text.
                routeText += space + nextRoad;

                if ((detail.Direction == TurnDirection.Continue) ||
                    (detail.Direction == TurnDirection.MiniRoundaboutContinue))
                    routeText += space + straightOn;

                if (detail.RoadSplits)
                {
                    if ((detail.TurnCount > 4) &&
                        ((detail.Direction == TurnDirection.Left) || (detail.Direction == TurnDirection.Right)))
                        routeText = ChangeFirstCharacterCapitalisation(whereRoadSplits, true) + space +
                            ChangeFirstCharacterCapitalisation(routeText, false);
                    else routeText += space + whereRoadSplits;
                }
            }



            return routeText;
        }
        
        #endregion Protected methods

        #region Del 6.3.1 changes

        /// <summary>
        /// checks if a place name exists and formats the instruction accordingly
        /// </summary>
        /// <param name="detail">the RoadJourneyDetail being formatted </param>
        /// <param name="routeText">the existing instruction text </param>
        /// <returns>updated formatted string of the directions</returns>
        protected virtual string AddPlaceName(RoadJourneyDetail detail, string routeText)
        {
            //add "towards {placename}" to the end of the instruction 
            routeText = routeText + space + towards + " <b>" + detail.PlaceName + "</b>";

            return routeText;
        }


        /// <summary>
        /// processes the junction number and junction action values
        /// </summary>
        protected virtual string FormatMotorwayJunction(RoadJourneyDetail detail, string routeText)
        {
            string junctionText = String.Empty;

            //apply the junction number rules 
            switch (detail.JunctionAction)
            {

                case JunctionType.Entry:

                    //add a join motorway message to the instructional text
                    routeText = routeText + space + atJunctionJoin + " <b>"
                        + detail.JunctionNumber + "</b>";

                    break;

                case JunctionType.Exit:

                    //replace the normal instructional text with a leave motorway message
                    routeText = atJunctionLeave + " <b>" + detail.JunctionNumber + "</b> " + leaveMotorway;

                    break;

                case JunctionType.Merge:

                    //replace the normal instructional text with a leave motorway message
                    routeText = atJunctionLeave + " <b>" + detail.JunctionNumber + "</b> " + leaveMotorway;

                    break;

                default:

                    //no action required
                    return routeText;
            }

            //return amended route text
            return routeText;

        }

        /// <summary>
        /// Checks the subsequent RoadJourneyDetail
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="routeText"></param>
        /// <returns></returns>
        protected virtual string CheckNextDetail(RoadJourneyDetail nextDetail, string routeText)
        {
            //add "until junction number {no}" to end of current instruction
            if ((nextDetail.IsJunctionSection) && ((nextDetail.JunctionAction == JunctionType.Exit) || (nextDetail.JunctionAction == JunctionType.Merge)))
            {
                routeText = routeText + space + untilJunction + " <b>" + nextDetail.JunctionNumber + "</b>";
            }
            return routeText;
        }

        #endregion Del 6.3.1 changes

        #region Private methods

        /// <summary>
        /// Initialises all language sensitive text strings using
        /// the resource manager.
        /// </summary>
        private void InitialiseRouteTextDescriptionStrings()
        {

            throughRoute = GetResourceString
                ("RouteText.ThroughRoute");

            // string for roundabout exit
            roundaboutExitOne = GetResourceString
                ("RouteText.RoundaboutExitOne");
            roundaboutExitTwo = GetResourceString
                ("RouteText.RoundaboutExitTwo");
            roundaboutExitThree = GetResourceString
                ("RouteText.RoundaboutExitThree");
            roundaboutExitFour = GetResourceString
                ("RouteText.RoundaboutExitFour");
            roundaboutExitFive = GetResourceString
                ("RouteText.RoundaboutExitFive");
            roundaboutExitSix = GetResourceString
                ("RouteText.RoundaboutExitSix");
            roundaboutExitSeven = GetResourceString
                ("RouteText.RoundaboutExitSeven");
            roundaboutExitEight = GetResourceString
                ("RouteText.RoundaboutExitEight");
            roundaboutExitNine = GetResourceString
                ("RouteText.RoundaboutExitNine");
            roundaboutExitTen = GetResourceString
                ("RouteText.RoundaboutExitTen");

            continueString = GetResourceString
                ("RouteText.Continue");
            continueMiniRoundabout = GetResourceString
                ("RouteText.ContinueMiniRoundabout");
            leftMiniRoundabout = GetResourceString
                ("RouteText.LeftMiniRoundabout");
            rightMiniRoundabout = GetResourceString
                ("RouteText.RightMiniRoundabout");
            uTurnMiniRoundabout = GetResourceString
                ("RouteText.UTurnMiniRoundabout");
            leftMiniRoundabout2 = GetResourceString
                ("RouteText.LeftMiniRoundabout2");
            rightMiniRoundabout2 = GetResourceString
                ("RouteText.RightMiniRoundabout2");
            uTurnMiniRoundabout2 = GetResourceString
                ("RouteText.UTurnMiniRoundabout2");

            immediatelyTurnLeft = GetResourceString
                ("RouteText.ImmediatelyTurnLeft");
            immediatelyTurnRight = GetResourceString
                ("RouteText.ImmediatelyTurnRight");

            turnLeftOne = GetResourceString
                ("RouteText.TurnLeftOne");
            turnLeftTwo = GetResourceString
                ("RouteText.TurnLeftTwo");
            turnLeftThree = GetResourceString
                ("RouteText.TurnLeftThree");
            turnLeftFour = GetResourceString
                ("RouteText.TurnLeftFour");

            turnRightOne = GetResourceString
                ("RouteText.TurnRightOne");
            turnRightTwo = GetResourceString
                ("RouteText.TurnRightTwo");
            turnRightThree = GetResourceString
                ("RouteText.TurnRightThree");
            turnRightFour = GetResourceString
                ("RouteText.TurnRightFour");

            turnLeftInDistance = GetResourceString
                ("RouteText.TurnLeftInDistance");
            turnRightInDistance = GetResourceString
                ("RouteText.TurnRightInDistance");

            bearLeft = GetResourceString
                ("RouteText.BearLeft");
            bearRight = GetResourceString
                ("RouteText.BearRight");

            immediatelyBearLeft = GetResourceString
                ("RouteText.ImmediatelyBearLeft");
            immediatelyBearRight = GetResourceString
                ("RouteText.ImmediatelyBearRight");


            arriveAt = GetResourceString
                ("RouteText.ArriveAt");

            leaveFrom = GetResourceString
                ("RouteText.Leave");

            notApplicable = GetResourceString
                ("RouteText.NotApplicable");

            localRoad = GetResourceString
                ("RouteText.LocalRoad");

            //motorway instructions
            atJunctionLeave = GetResourceString
                ("RouteText.AtJunctionLeave");

            leaveMotorway = GetResourceString
                ("RouteText.LeaveMotorway");

            untilJunction = GetResourceString
                ("RouteText.UntilJunction");

            onTo = GetResourceString
                ("RouteText.OnTo");

            towards = GetResourceString
                ("RouteText.Towards");

            continueFor = GetResourceString
                ("RouteText.ContinueFor");

            miles = GetResourceString
                ("RouteText.Miles");

            turnLeftToJoin = GetResourceString
                ("RouteText.TurnLeftToJoin");

            turnRightToJoin = GetResourceString
                ("RouteText.TurnRightToJoin");

            atJunctionJoin = GetResourceString
                ("RouteText.AtJunctionJoin");

            bearLeftToJoin = GetResourceString
                ("RouteText.BearLeftToJoin");

            bearRightToJoin = GetResourceString
                ("RouteText.BearRightToJoin");

            join = GetResourceString
                ("RouteText.Join");

            forText = GetResourceString
                ("RouteText.For");

            follow = GetResourceString
                ("RouteText.Follow");

            to = GetResourceString
                ("RouteText.To");

            routeTextFor = GetResourceString
                ("RouteText.For");

            continueText = GetResourceString
                ("RouteText.Continue");

            //Del 7 Car Costing strings
            enter = GetResourceString
                ("RouteText.Enter");
            congestionZone = GetResourceString
                ("RouteText.CongestionCharge");
            charge = GetResourceString
                ("RouteText.Charge");
            certainTimes = GetResourceString
                ("RouteText.CertainTimes");
            certainTimesNoCharge = GetResourceString
                ("RouteText.CertainTimesNoCharge");
            board = GetResourceString
                ("RouteText.Board");
            departingAt = GetResourceString
                ("RouteText.DepartingAt");
            toll = GetResourceString
                ("RouteText.Toll");
            HighTraffic = GetResourceString
                ("RouteText.HighTraffic");
            PlanStop = GetResourceString
                ("RouteText.PlanStop");
            FerryWait = GetResourceString
                ("RouteText.WaitForFerry");
            viaArriveAt = GetResourceString
                ("RouteText.ViaArriveAt");
            leaveFerry = GetResourceString
                ("RouteText.LeaveFerry");
            exit = GetResourceString
                ("RouteText.Exit");
            end = GetResourceString
                ("RouteText.End");
            UnspecifedFerryWait = GetResourceString
                ("RouteText.UnspecifedWaitForFerry");
            IntermediateFerryWait = GetResourceString
                ("RouteText.IntermediateFerry");
            WaitAtTerminal = GetResourceString
                ("RouteText.WaitAtTerminal");
            notAvailable = GetResourceString
                ("RouteText.NotAvailable");

            straightOn = GetResourceString
                ("RouteText.StraightOn");
            atMiniRoundabout = GetResourceString
                ("RouteText.AtMiniRoundabout");
            immediatelyTurnRightOnto = GetResourceString
                ("RouteText.ImmediatelyTurnRightOnto");
            immediatelyTurnLeftOnto = GetResourceString
                ("RouteText.ImmediatelyTurnLeftOnto");
            whereRoadSplits = GetResourceString
                ("RouteText.WhereRoadSplits");

            // park and ride
            parkAndRide = GetResourceString
                ("ParkAndRide.Suffix");
            carParkText = GetResourceString
                ("ParkAndRide.CarkPark.Suffix");

            // open new window icon
            openNewWindowImageUrl = GetResourceString
                ("ExternalLinks.OpensNewWindowImage");

            //London Congestion Charge Additional Text added for CCN0602
            londonCCAdditionalText = GetResourceString
                ("RouteText.LondonCongestionChargeAdditionalText");

        }

       
        #endregion Private methods

    }
}