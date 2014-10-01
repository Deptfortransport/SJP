// *********************************************** 
// NAME             : LegInstructionAdapter.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Helper class to construct journey leg details and instructions
// ************************************************


using System;
using System.Text;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.Common.Web
{
    /// <summary>
    /// Helper class to construct journey leg details
    /// </summary>
    public class LegInstructionAdapter
    {
        #region Private Fields

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private SJPResourceManager resourceManager = null;

        private JourneyLeg journeyLeg;
        private JourneyLeg previousJourneyLeg;
        private JourneyLeg nextJourneyLeg;

        private const string dateTimeFormat = "HH:mm";

        // Text strings
        private string space = " ";
        private string TXT_Leave = string.Empty;
        private string TXT_Depart = string.Empty;
        private string TXT_Arrive = string.Empty;
        private string TXT_CheckIn = string.Empty;
        private string TXT_TakeThe;
        private string TXT_Take;
        private string TXT_Or;
        private string TXT_Towards;
        private string TXT_WalkTo;
        private string TXT_WalkTo_Allowance;
        private string TXT_WalkInterchangeTo;
        private string TXT_TransferTo;
        private string TXT_TaxiTo;
        private string TXT_To;
        private string TXT_Then;
        private string TXT_Km;
        private string TXT_Drive;
        private string TXT_Cycle;
        private string TXT_Mins;
        private string TXT_Min;
        private string TXT_AllowTimeForExit_Outward;
        private string TXT_AllowTimeForExit_Outward_Accessible;
        private string TXT_AllowTimeForExit_Outward_Queue;
        private string TXT_AllowTimeForEntrance_Outward;
        private string TXT_AllowTimeForEntrance_Outward_Accessible;
        private string TXT_AllowTimeForEntrance_Outward_Queue;
        private string TXT_AllowTimeForExit_Return;
        private string TXT_AllowTimeForExit_Return_Accessible;
        private string TXT_AllowTimeForExit_Return_Queue;
        private string TXT_AllowTimeForEntrance_Return;
        private string TXT_AllowTimeForEntrance_Return_Accessible;
        private string TXT_AllowTimeForEntrance_Return_Queue;

        // Indicates if the text should be setup in a printer friendly mode
        private bool isPrinterFriendly = false;

        // Indicates if mobile specific content is used
        private bool isMobile = false;

        #endregion

        #region Constructor
        /// <summary>
        /// LegInstructionHelper constructor 
        /// </summary>
        /// <param name="journeyLeg">JourneyLeg object </param>
        public LegInstructionAdapter(JourneyLeg journeyLeg, JourneyLeg previousJourneyLeg, JourneyLeg nextJourneyLeg,
            bool printerFriendly, SJPResourceManager resourceManager, bool isMobile)
        {
            this.journeyLeg = journeyLeg;
            this.previousJourneyLeg = previousJourneyLeg;
            this.nextJourneyLeg = nextJourneyLeg;
            this.isPrinterFriendly = printerFriendly;
            this.resourceManager = resourceManager;
            this.isMobile = isMobile;

            InitialiseText();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the journey leg start location name
        /// </summary>
        public string GetLegStartLocation()
        {
            return journeyLeg.LegStart.Location.DisplayName;
        }

        /// <summary>
        /// Returns the journey leg end location name
        /// </summary>
        public string GetLegEndLocation()
        {
            return journeyLeg.LegEnd.Location.DisplayName;
        }

        /// <summary>
        /// Returns the previous journey leg end location
        /// </summary>
        public string GetPreviousEndLocation()
        {
            if (previousJourneyLeg == null)
            {
                return string.Empty;
            }
            else
            {
                return previousJourneyLeg.LegEnd.Location.DisplayName;
            }
        }

        /// <summary>
        /// Constructs and retuns the journey leg detail.
        /// Overload for the current journey leg mode.
        /// </summary>
        /// <returns></returns>
        public string GetLegDetail()
        {
            return GetLegDetail(journeyLeg.Mode);
        }

        /// <summary>
        /// Constructs and retuns the journey leg detail.
        /// Overload for a specific mode (instead of the current journey leg mode)
        /// </summary>
        /// <returns></returns>
        public string GetLegDetail(SJPModeType mode)
        {
            string legDetail = string.Empty;

            switch (mode)
            {

                case SJPModeType.CheckIn:
                    legDetail = GetCheckInInstruction();
                    break;

                case SJPModeType.CheckOut:
                    legDetail = GetCheckOutInstruction();
                    break;

                case SJPModeType.Car:
                    legDetail = GetCarInstruction(ShowCarLeg);
                    break;

                case SJPModeType.Cycle:
                    legDetail = GetCycleInstruction(ShowCycleLeg);
                    break;

                case SJPModeType.Taxi:
                    legDetail = GetTexiInstruction();
                    break;

                case SJPModeType.Walk:
                    legDetail = GetWalkInstruction();
                    break;

                case SJPModeType.WalkInterchange:
                    legDetail = GetWalkInterchangeInstruction();
                    break;

                case SJPModeType.Transfer:
                    legDetail = GetTransferInstruction();
                    break;

                case SJPModeType.TransitShuttleBus:
                case SJPModeType.TransitRail:
                    legDetail = GetTransitInstruction();
                    break;

                default:
                    legDetail = GetLegInstruction();
                    break;

            }

            return legDetail;
        }

        /// <summary>
        /// Constructs and retuns the journey leg check constraint detail
        /// </summary>
        /// <returns></returns>
        public string GetLegCheckConstraintDetail(bool isReturn)
        {
            StringBuilder interchangeDetail = new StringBuilder();

            if ((journeyLeg.JourneyDetails != null) && (journeyLeg.JourneyDetails.Count > 0))
            {
                #region Check constraint text for a leg

                if (journeyLeg.JourneyDetails[0] is PublicJourneyInterchangeDetail)
                {
                    PublicJourneyInterchangeDetail pjid = (PublicJourneyInterchangeDetail)journeyLeg.JourneyDetails[0];

                    if (pjid.CheckConstraints != null)
                    {
                        foreach (ICJP.CheckConstraint cc in pjid.CheckConstraints)
                        {
                            //string minuteString = cc.averageDelay > 1 ?
                            //            TXT_Mins : TXT_Min;
                            //string time = cc.averageDelay + minuteString;

                            // Not used but left in case instruction changes in future
                            //string checkProcess = resourceManager.GetString(CurrentLanguage.Value, RG, RC,
                            //            string.Format("JourneyOutput.Text.CheckConstraint.{0}", cc.checkProcess));
                                                        
                            #region Security check constraint
                            // Display a check constraint for a "security" check constraint
                            if (((cc.checkProcess == ICJP.CheckProcess.securityCheck)|| (cc.checkProcess == ICJP.CheckProcess.baggageSecurityCheck))
                                &&
                                (!isReturn))
                            {
                                // Outward journey, entering venue
                                interchangeDetail.Append(TXT_AllowTimeForEntrance_Outward_Queue);
                            }
                            else if (((cc.checkProcess == ICJP.CheckProcess.securityCheck) || (cc.checkProcess == ICJP.CheckProcess.baggageSecurityCheck))
                                    &&
                                    (isReturn))
                            {
                                // Return journey, entering venue
                                interchangeDetail.Append(TXT_AllowTimeForEntrance_Return_Queue);
                            }
                            #endregion
                            #region Egress check constraint
                            // Display a check constraint for an "egress" check constraint
                            else if ((cc.checkProcess == ICJP.CheckProcess.egress)
                                    &&
                                    (!isReturn))
                            {
                                // Outward journey, exit venue
                                interchangeDetail.Append(TXT_AllowTimeForExit_Outward_Queue);
                            }
                            else if (cc.checkProcess == ICJP.CheckProcess.egress)
                            {
                                // Return journey, exit venue
                                interchangeDetail.Append(TXT_AllowTimeForExit_Return_Queue);
                            }
                            #endregion
                        }
                    }
                }

                #endregion
            }

            return interchangeDetail.ToString();
        }

        /// <summary>
        /// Constructs and returns the journey leg allow time detail
        /// </summary>
        /// <param name="isReturn"></param>
        /// <param name="isAccessibleJourney"></param>
        /// <returns></returns>
        public string GetLegAllowTimeDetail(bool isReturn, bool firstLeg, bool lastLeg, bool fromVenue, bool toVenue, bool isAccessibleJourney)
        {
            StringBuilder interchangeDetail = new StringBuilder();

            if (!isReturn)
            {
                if (firstLeg && fromVenue)
                {
                    // Outward journey, exit venue
                    if (isAccessibleJourney)
                        interchangeDetail.Append(TXT_AllowTimeForExit_Outward_Accessible);
                    else
                        interchangeDetail.Append(TXT_AllowTimeForExit_Outward);
                }
                else if (lastLeg && toVenue)
                {
                    // Outward journey, entering venue
                    if (isAccessibleJourney)
                        interchangeDetail.Append(TXT_AllowTimeForEntrance_Outward_Accessible);
                    else
                        interchangeDetail.Append(TXT_AllowTimeForEntrance_Outward);
                }
            }
            else
            {
                if (firstLeg && fromVenue)
                {
                    // Return journey, exit venue
                    if (isAccessibleJourney)
                        interchangeDetail.Append(TXT_AllowTimeForExit_Return_Accessible);
                    else
                        interchangeDetail.Append(TXT_AllowTimeForExit_Return);
                }
                else if (lastLeg && toVenue)
                {
                    // Return journey, entering venue
                    if (isAccessibleJourney)
                        interchangeDetail.Append(TXT_AllowTimeForEntrance_Return_Accessible);
                    else
                        interchangeDetail.Append(TXT_AllowTimeForEntrance_Return);
                }
            }

            return interchangeDetail.ToString();
        }

        /// <summary>
        /// Returns instruction time for the current journey leg (for frequency legs only)
        /// </summary>
        /// <returns></returns>
        public string GetLegTimeDetail(bool isPJFrequencyLeg)
        {
            StringBuilder output = new StringBuilder();

            if (journeyLeg.Mode != SJPModeType.Transfer)
            {
                if (isPJFrequencyLeg)
                {
                    output.Append(GetFrequencyDurationText());
                }
            }
            
            return output.ToString();
        }

        /// <summary>
        /// Returns the current journey leg duration as a string in 0 days, 0 hr, 0 mins format
        /// </summary>
        /// <returns></returns>
        public string GetLegModeDetail(bool isFirstLeg, bool isLastLeg, bool isPJFrequencyLeg, bool isAccessibleJourney)
        {
            StringBuilder output = new StringBuilder();

            // Add service number to start of mode detail text if required
            string serviceNumberText = GetServiceNumberText();
            if (!string.IsNullOrEmpty(serviceNumberText) && journeyLeg.Mode == SJPModeType.Bus)
            {
                bool showServiceNumber = Properties.Current["DetailsLegControl.ShowServiceNumber.Switch"].Parse(true);
                if (showServiceNumber)
                {
                    output.Append(serviceNumberText);
                    output.Append("<br />");
                }
            }
            
            // Add mode detail text
            if (journeyLeg.Mode == SJPModeType.Transfer)
            {
                output.Append(GetModeTransferDetailText(isFirstLeg, isLastLeg, isAccessibleJourney));
            }
            else if (journeyLeg.Mode == SJPModeType.Walk 
                || journeyLeg.Mode == SJPModeType.WalkInterchange)
            {
                // Walk and interchange leg do not show time below mode icon
            }
            else
            {
                output.Append(GetDurationText(isPJFrequencyLeg));
            }

            return output.ToString();
        }

        /// <summary>
        /// Returns the instruction text "Arrive" or "Check-in" if the 
        /// previous leg is neither a frequency leg or walking leg, otherwise
        /// a space is returned.
        /// </summary>
        /// <returns></returns>
        public string GetArriveLabelText(bool isFirstLeg, bool isLastLeg, bool isPreviousPJTimedLeg, bool isPreviousCarLeg, bool isPreviousCycleLeg, bool showInterchange)
        {
            if (isLastLeg)
            {
                return TXT_Arrive;
            }
            else if ((isFirstLeg) && (journeyLeg.Mode == SJPModeType.Air))
            {
                return TXT_CheckIn;
            }
            else
            {
                if (((isPreviousPJTimedLeg) || (isPreviousCarLeg) || (isPreviousCycleLeg))
                    && (!showInterchange))
                {
                    return TXT_Arrive;
                }
                else if (showInterchange)
                {
                    return TXT_Arrive;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns the instruction text "Leave" for the first leg
        /// Returns the instruction text "Depart" if the specified 
        /// leg is neither a frequency leg or walking leg, otherwise
        /// a space is returned.
        /// </summary>
        /// <returns></returns>
        public string GetDepartLabelText(bool isFirstLeg, bool isPJTimedLeg, bool isPJFrequencyLeg, bool isPJContinuousLeg,
            bool isCarLeg, bool isCycleLeg)
        {
            if ((isFirstLeg) && (journeyLeg.Mode != SJPModeType.Air))
            {
                if (journeyLeg.StartTime != DateTime.MinValue)
                {
                    return TXT_Leave;
                }
            }
            else if ((isPJTimedLeg) || (isCarLeg) || (isCycleLeg))
            {
                return TXT_Depart;
            }
            else if (isPJFrequencyLeg || isPJContinuousLeg)
            {
                if (journeyLeg.StartTime != DateTime.MinValue)
                {
                    return TXT_Depart;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returnes formated arrival time in HH:mm format. 
        /// This is the arrive time for the "Current journey leg"
        /// </summary>
        /// <returns></returns>
        public string GetArriveTime(bool isFirstLeg, bool isLastLeg)
        {
            // Return time for all legs other that first, 
            // but if its a one leg journey than leg will be true for first and last flags, so display time
            if ((!isFirstLeg)
                || (isFirstLeg && isLastLeg))
            {
                if ((journeyLeg != null) && (journeyLeg.EndTime != DateTime.MinValue))
                {
                    DateTime dateTime = journeyLeg.EndTime;

                    if (dateTime.Second >= 30)
                        dateTime = dateTime.AddMinutes(1);

                    return dateTime.ToString(dateTimeFormat);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returnes formated arrival time in HH:mm format. 
        /// This is the arrive time for the "Previous journey leg"
        /// </summary>
        /// <returns></returns>
        public string GetPreviousArriveTime(bool isFirstLeg, bool isPreviousPJTimedLeg, bool isPreviousCarLeg)
        {
            if (!isFirstLeg)   
            {
                if (isPreviousPJTimedLeg || isPreviousCarLeg)
                {
                    if ((previousJourneyLeg != null) && (previousJourneyLeg.EndTime != DateTime.MinValue))
                    {
                        DateTime dateTime = previousJourneyLeg.EndTime;

                        if (dateTime.Second >= 30)
                            dateTime = dateTime.AddMinutes(1);

                        return dateTime.ToString(dateTimeFormat);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returnes formated departure time in HH:mm format.
        /// This is the depart time for the "Current journey leg"
        /// </summary>
        /// <returns></returns>
        public string GetDepartTime(bool isFirstLeg, bool isLastLeg, bool isPJTimedLeg, bool isCarLeg)
        {
            // Return time for all legs other that last, 
            // but if its a one leg journey than leg will be true for first and last flags, so display time
            if ((!isLastLeg && journeyLeg.Mode != SJPModeType.Walk)
                || (isFirstLeg && journeyLeg.Mode == SJPModeType.Walk)
                || (isFirstLeg && isLastLeg)
                || (isPJTimedLeg)
                || (isCarLeg))
            {
                if (journeyLeg.StartTime != DateTime.MinValue)
                {
                    DateTime dateTime = journeyLeg.StartTime;

                    if (dateTime.Second >= 30)
                        dateTime = dateTime.AddMinutes(1);

                    return dateTime.ToString(dateTimeFormat);
                }
            }

            return string.Empty;
        }
        
        #endregion

        #region Private Methods

        /// <summary>
        /// Method which initialises the text string used by this control
        /// </summary>
        private void InitialiseText()
        {
            Language language = CurrentLanguage.Value;

            TXT_Leave = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Leave").ToLower();
            TXT_Depart = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Depart").ToLower();
            TXT_Arrive = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Arrive").ToLower();
            TXT_CheckIn = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.CheckIn").ToLower();
            TXT_Take = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Take");
            TXT_TakeThe = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.TakeThe");
            TXT_Or = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Or");
            TXT_Towards = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Towards");
            TXT_WalkTo = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.WalkTo");
            TXT_WalkTo_Allowance = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.WalkTo.Allowance");
            TXT_WalkInterchangeTo = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.WalkInterchangeTo");
            TXT_TransferTo = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.TransferTo");
            TXT_TaxiTo = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.TaxiTo");
            TXT_To = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.To");
            TXT_Then = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Then");
            TXT_Km = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Km");
            TXT_Drive = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Drive");
            TXT_Cycle = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Cycle");
            TXT_Mins = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Minutes");
            TXT_Min = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Minute");

            TXT_AllowTimeForExit_Outward = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Outward");
            TXT_AllowTimeForExit_Outward_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Outward");
            TXT_AllowTimeForExit_Outward_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Outward");
            TXT_AllowTimeForEntrance_Outward = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Outward");
            TXT_AllowTimeForEntrance_Outward_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Outward");
            TXT_AllowTimeForEntrance_Outward_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Outward");
            TXT_AllowTimeForExit_Return = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Return");
            TXT_AllowTimeForExit_Return_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Return");
            TXT_AllowTimeForExit_Return_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Return");
            TXT_AllowTimeForEntrance_Return = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Return");
            TXT_AllowTimeForEntrance_Return_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Return");
            TXT_AllowTimeForEntrance_Return_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Return");

            // Update text for mobile where required
            if (isMobile)
            {
                TXT_AllowTimeForExit_Outward = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Outward.Mobile");
                TXT_AllowTimeForExit_Outward_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Outward.Mobile");
                TXT_AllowTimeForExit_Outward_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Outward.Mobile");
                TXT_AllowTimeForEntrance_Outward = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Outward.Mobile");
                TXT_AllowTimeForEntrance_Outward_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Outward.Mobile");
                TXT_AllowTimeForEntrance_Outward_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Outward.Mobile");
                TXT_AllowTimeForExit_Return = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.ExitFromVenue.Return.Mobile");
                TXT_AllowTimeForExit_Return_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.ExitFromVenue.Return.Mobile");
                TXT_AllowTimeForExit_Return_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.ExitFromVenue.Return.Mobile");
                TXT_AllowTimeForEntrance_Return = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.EntranceToVenue.Return.Mobile");
                TXT_AllowTimeForEntrance_Return_Accessible = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Accessible.EntranceToVenue.Return.Mobile");
                TXT_AllowTimeForEntrance_Return_Queue = resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.AllowTimeFor.Queue.EntranceToVenue.Return.Mobile");
            }
        }

        /// <summary>
        /// Constructs leg instruction
        /// </summary>
        /// <returns></returns>
        private string GetLegInstruction()
        {
            StringBuilder sbInstruction = new StringBuilder();

            // Leg instruction may have multiple Services for each JourneyDetail 
            // (should only be 1 (Public)JourneyDetail, but handle for multiple!)
            if (journeyLeg.JourneyDetails != null)
            {
                int index = 0;

                foreach (JourneyDetail jd in journeyLeg.JourneyDetails)
                {
                    if (jd is PublicJourneyDetail)
                    {
                        PublicJourneyDetail pjd = (PublicJourneyDetail)jd;

                        if (pjd.Services.Count > 0)
                        {

                            bool frequentBusOrUnderround = false;
                            // if frequent bus/ under ground don't display 'the' and the time as part of the instruction
                            if((pjd is PublicJourneyFrequencyDetail && pjd.Mode == SJPModeType.Bus)
                                         || pjd.Mode == SJPModeType.Underground)
                            {
                                frequentBusOrUnderround = true;
                            }

                            foreach (ServiceDetail sd in pjd.Services)
                            {
                                #region Build Instruction using service details

                                // Take the
                                if (index == 0)
                                {
                                    // if frequent bus/ under ground don't display 'the' 
                                    if (frequentBusOrUnderround)
                                    {
                                        sbInstruction.Append(TXT_Take);
                                    }
                                    else
                                    {
                                        sbInstruction.Append(TXT_TakeThe);
                                    }
                                }
                                else
                                {
                                    sbInstruction.Append(TXT_Or);
                                    // if frequent bus/ under ground don't display 'the' 
                                    if (frequentBusOrUnderround)
                                    {
                                        sbInstruction.Append(TXT_Take);
                                    }
                                    else
                                    {
                                        sbInstruction.Append(TXT_TakeThe.ToLower());
                                    }
                                }

                                // if frequent bus/ underground don't display the time as part of the instruction 
                                if (!frequentBusOrUnderround)
                                {
                                    
                                    // 12:00 
                                    if (journeyLeg.StartTime != DateTime.MinValue)
                                    {
                                        sbInstruction.Append(journeyLeg.StartTime.ToString(dateTimeFormat));
                                        sbInstruction.Append(space);
                                    }
                                }

                                #region Service operator

                                string londonUnderGroundOprator = Properties.Current["JourneyDetails.LondonUnderground.Operator"].ToLower().Trim();
                                string londonDLROprator = Properties.Current["JourneyDetails.LondonDLR.Operator"].ToLower().Trim();

                                // Service opertor name, e.g. East midlands train
                                if (!string.IsNullOrEmpty(sd.OperatorName))
                                {
                                    // If operator is london underground or london DLR don't show the operator name
                                    if (sd.OperatorName.ToLower().Trim() != londonUnderGroundOprator 
                                        && sd.OperatorName.ToLower().Trim() != londonDLROprator)
                                    {
                                        sbInstruction.Append(sd.OperatorName);
                                    }
                                }
                                else
                                {
                                    // No service opertor name, append mode
                                    sbInstruction.Append(journeyLeg.Mode.ToString().ToLower());
                                }

                                #endregion

                                #region Service number

                                // Service number, e.g. 768
                                // but not for train as its not useful
                                if (!string.IsNullOrEmpty(sd.ServiceNumber)
                                    && (journeyLeg.Mode != SJPModeType.Rail))
                                {
                                    string serviceNumberClass = sd.ServiceNumber.ToLower().Replace("&", "and").Replace(" ", "");
                                    bool applyServiceColorClass = Properties.Current["JourneyDetails.LondonUnderground.Service.ColorCode"].Parse(false);

                                    // Decorate the service number with html code in case of underground
                                    // if mode is underground and the operator is London underground color code the service number
                                    // if mode is tram and the operator is DLR color code the service number
                                    if (!string.IsNullOrEmpty(sd.OperatorName)
                                        && applyServiceColorClass
                                        && ((journeyLeg.Mode == SJPModeType.Underground && sd.OperatorName.ToLower().Trim() == londonUnderGroundOprator)
                                            || (journeyLeg.Mode == SJPModeType.Tram && sd.OperatorName.ToLower().Trim() == londonDLROprator))
                                        )
                                    {
                                        sbInstruction.Append(string.Format("<span class='{0}'>{1}</span>", serviceNumberClass, sd.ServiceNumber));
                                    }
                                    else
                                    {
                                        sbInstruction.Append(space);
                                        sbInstruction.Append(sd.ServiceNumber);
                                    }
                                }

                                #endregion

                                // Towards
                                sbInstruction.Append(space);
                                sbInstruction.Append(TXT_Towards);

                                // Service destination or Leg end
                                if (!string.IsNullOrEmpty(sd.DestinationBoard))
                                {
                                    sbInstruction.Append(sd.DestinationBoard);
                                }
                                else
                                {
                                    sbInstruction.Append(journeyLeg.LegEnd.Location.DisplayName);
                                }

                                // Increment index here too in case there are multiple services
                                index++;

                                #endregion
                            }
                        }
                        else
                        {
                            sbInstruction.Append(GetSimpleInstruction(index));
                        }
                    }
                    else
                    {
                        sbInstruction.Append(GetSimpleInstruction(index));
                    }

                    sbInstruction.Append(space);
                    index++;
                }
            }
            else
            {
                // Set simple instruction, should never reach here because there will always be 1 JourneyDetail
                sbInstruction.Append(GetSimpleInstruction(0));
            }
            
            return sbInstruction.ToString().Trim();
        }

        /// <summary>
        /// Constructs transit shuttle instruction
        /// </summary>
        /// <returns></returns>
        private string GetTransitInstruction()
        {
            StringBuilder sbInstruction = new StringBuilder();

            string transitMode = resourceManager.GetString(CurrentLanguage.Value, string.Format("TransportMode.{0}", journeyLeg.Mode.ToString())).ToLower();

            sbInstruction.Append(TXT_TakeThe);
            sbInstruction.Append(transitMode);
            sbInstruction.Append(space);
            sbInstruction.Append(TXT_Towards);
            sbInstruction.Append(space);
            sbInstruction.Append(journeyLeg.LegEnd.Location.DisplayName);
                        
            return sbInstruction.ToString();
        }

        /// <summary>
        /// Constructs walk instruction
        /// </summary>
        /// <returns></returns>
        private string GetWalkInstruction()
        {
            // Do not show the interchange allowance comment for mobile
            return string.Format(TXT_WalkTo, 
                GetDurationText(false),
                journeyLeg.LegEnd.Location.DisplayName,
                isMobile ? string.Empty : TXT_WalkTo_Allowance);
        }

        /// <summary>
        /// Constructs walk interchange instruction
        /// </summary>
        /// <returns></returns>
        private string GetWalkInterchangeInstruction()
        {
            return string.Format(TXT_WalkInterchangeTo, 
                journeyLeg.LegEnd.Location.DisplayName,
                GetDurationText(false));
        }

        /// <summary>
        /// Constructs transfer instruction
        /// </summary>
        /// <returns></returns>
        private string GetTransferInstruction()
        {
            return string.Format("{0}{1}", TXT_TransferTo, journeyLeg.LegEnd.Location.DisplayName);
        }

        /// <summary>
        /// Constructs taxi instruction
        /// </summary>
        /// <returns></returns>
        private string GetTexiInstruction()
        {
            return string.Format("{0}{1}", TXT_TaxiTo, journeyLeg.LegEnd.Location.DisplayName);
        }

        /// <summary>
        /// Constructs and returns car leg detail summary
        /// </summary>
        /// <returns></returns>
        private string GetCarInstruction(bool showCarLeg)
        {
            StringBuilder sbInstruction = new StringBuilder();

            // Get distance as Km
            FormattedJourneyDetail fjd = new FormattedJourneyDetail();
            string km = fjd.ConvertMetresToKm(journeyLeg.Distance);

            // Default to leg end location name
            string location = GetLegEndLocation();

            // Drive will be to a car park, the journey planner should have correctly set the end location to be 
            // the car park name, so no need to do anything different here
            
            // Build instruction

            if ((!showCarLeg) || (isPrinterFriendly))
            {
                sbInstruction.Append(TXT_Drive);
                sbInstruction.Append(space);
            }

            sbInstruction.Append(km);
            sbInstruction.Append(TXT_Km);
            sbInstruction.Append(space);
            sbInstruction.Append(TXT_To);
            sbInstruction.Append(location);
            
            return sbInstruction.ToString();
        }
        
        /// <summary>
        /// Constructs and returns cycle leg detail summary
        /// </summary>
        /// <returns></returns>
        private string GetCycleInstruction(bool showCycleLeg)
        {
            StringBuilder sbInstruction = new StringBuilder();

            // Get distance as Km
            FormattedJourneyDetail fjd = new FormattedJourneyDetail();
            string km = fjd.ConvertMetresToKm(journeyLeg.Distance);

            // Default to leg end location name
            string location = GetLegEndLocation();

            // Cycle journey will be to a cycle park, the journey planner should have correctly set the end location to be 
            // the cycle park name, so no need to do anything different here

            // Build instruction

            if ((!showCycleLeg) || (isPrinterFriendly))
            {
                sbInstruction.Append(TXT_Cycle);
                sbInstruction.Append(space);
            }

            if (journeyLeg.Distance > 0)
            {
                sbInstruction.Append(km);
                sbInstruction.Append(TXT_Km);
                sbInstruction.Append(space);
            }

            sbInstruction.Append(TXT_To);
            sbInstruction.Append(location);

            return sbInstruction.ToString();
        }

        /// <summary>
        /// GetCheckOutInstruction
        /// </summary>
        /// <returns></returns>
        private string GetCheckOutInstruction()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetCheckInInstruction
        /// </summary>
        /// <returns></returns>
        private string GetCheckInInstruction()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs a simple instruction in format "Take the StartTime Mode towards LegEndDisplayName
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetSimpleInstruction(int index)
        {
            StringBuilder sbInstruction = new StringBuilder();

            sbInstruction.Append((index > 0) ? space + TXT_Or : string.Empty);
            sbInstruction.Append((index > 0) ? TXT_TakeThe.ToLower() : TXT_TakeThe);
            sbInstruction.Append(journeyLeg.StartTime.ToString(dateTimeFormat));
            sbInstruction.Append(space);
            sbInstruction.Append(journeyLeg.Mode.ToString().ToLower());
            sbInstruction.Append(space);
            sbInstruction.Append(TXT_Towards);
            sbInstruction.Append(space);
            sbInstruction.Append(journeyLeg.LegEnd.Location.DisplayName);
            
            return sbInstruction.ToString();
        }

        #region Methods for GetLegModeDetail

        /// <summary>
        /// Returns string containing duration text 
        /// </summary>
        /// <returns></returns>
        private string GetDurationText(bool isPJFrequencyLeg)
        {
            if (isPJFrequencyLeg)
            {
                // Frequency now shown in the leg instruction area, see GetLegTimeDetail().
                // Only show Max in the mode detail area. Commented out incase this is changed in future
                //if (!string.IsNullOrEmpty(GetFrequencyDurationText())
                //{
                //    output.Append(GetFrequencyDurationText());
                //    output.Append("<br />");
                //}
                //output.Append(GetMaxJourneyDurationText());
                //output.Append("<br />");
                //output.Append(GetTypicalDurationText());
                return GetMaxJourneyDurationText();
            }
            else
            {
                Language language = CurrentLanguage.Value;

                // Get the duration of the current leg
                TimeSpan duration = journeyLeg.Duration;

                // In seconds
				double durationInSeconds = duration.TotalSeconds;

				// Get the minutes
				double durationInMinutes = durationInSeconds / 60;

				// Check to see if seconds is less than 30 seconds.
				if( durationInSeconds < 31)
				{
				    return "< 30 " + resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Seconds");
                }
                else
                {
                    // Round to the nearest minute
                    durationInMinutes = Round(durationInMinutes);

                    // Calculate the number of hours in the minute
                    int hours = (int)durationInMinutes / 60;

                    // Get the minutes (afer the hours has been subracted so always < 60)
                    int minutes = (int)durationInMinutes % 60;

                    // If greater than 1 hour - retrieve "hours", if 1 or less, retrieve "hour"
                    string hourString = hours > 1 ?
                        resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Hours") :
                        resourceManager.GetString(language, RG, RC, "JourneyOutput.Text.Hour");

                    // If greater than 1 minute - retrive "minutes", if 1 or less, retrieve "minute"
                    string minuteString = minutes > 1 ?
                        TXT_Mins : TXT_Min;

                    string formattedString = string.Empty;

                    if (hours > 0)
                        formattedString += hours + hourString + " ";

                    formattedString += minutes + minuteString;

                    return formattedString;
                }
            }
        }

        /// <summary>
        ///  Gets the mode of transport text
        /// </summary>
        /// <param name="sJPModeType">SJP mode type enumeration value</param>
        /// <returns></returns>
        private string GetModeText(SJPModeType sJPModeType)
        {
            string resourceManagerKey = "TransportMode." + sJPModeType.ToString();
            return resourceManager.GetString(CurrentLanguage.Value, resourceManagerKey);
                    
        }

        /// <summary>
        /// Returns formatted string containing the maximum duration of
        /// the frequency leg, or empty string if leg is
        /// not a frequency leg.
        /// </summary>
        /// <returns>Formatted string containing the maximum duration or empty string</returns>
        private string GetMaxJourneyDurationText()
        {
            if ((journeyLeg.JourneyDetails != null) && (journeyLeg.JourneyDetails.Count > 0))
            {
                if (journeyLeg.JourneyDetails[0] is PublicJourneyFrequencyDetail)
                {
                    PublicJourneyFrequencyDetail frequencyDetail = (PublicJourneyFrequencyDetail)journeyLeg.JourneyDetails[0];

                    string TXT_maxDurationText = resourceManager.GetString(CurrentLanguage.Value, RG, RC, "JourneyOutput.Text.DurationMax");
                    
                    return string.Format(TXT_maxDurationText, 
                        frequencyDetail.MaxDuration + 
                        (frequencyDetail.MaxDuration > 1 ? TXT_Mins : TXT_Min));
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns formatted string containing the frequency of
        /// the frequency leg, or empty string if leg (i.e. 'Every 1 min' ) is
        /// not a frequency leg.
        /// </summary>
        /// <returns></returns>
        private string GetFrequencyDurationText()
        {
            if ((journeyLeg.JourneyDetails != null) && (journeyLeg.JourneyDetails.Count > 0))
            {
                if (journeyLeg.JourneyDetails[0] is PublicJourneyFrequencyDetail)
                {
                    PublicJourneyFrequencyDetail frequencyDetail = (PublicJourneyFrequencyDetail)journeyLeg.JourneyDetails[0];

                    string TXT_typicalFrequencyText = resourceManager.GetString(CurrentLanguage.Value, RG, RC, "JourneyOutput.Text.FrequencyDuration");

                    return string.Format("<span class='frequencyduration'>{0}</span>", string.Format(TXT_typicalFrequencyText,
                        frequencyDetail.Frequency +
                        (frequencyDetail.Frequency > 1 ? TXT_Mins : TXT_Min)));
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns formatted string containing the typical duration of
        /// the frequency leg, or empty string if leg is
        /// not a frequency leg.
        /// </summary>
        /// <returns>Formatted string containing the typical duration or empty string</returns>        
        private string GetTypicalDurationText()
        {
            if ((journeyLeg.JourneyDetails != null) && (journeyLeg.JourneyDetails.Count > 0))
            {
                if (journeyLeg.JourneyDetails[0] is PublicJourneyFrequencyDetail)
                {
                    PublicJourneyFrequencyDetail frequencyDetail = (PublicJourneyFrequencyDetail)journeyLeg.JourneyDetails[0];

                    string TXT_typicalDurationText = resourceManager.GetString(CurrentLanguage.Value, RG, RC, "JourneyOutput.Text.DurationTypical");

                    return string.Format(TXT_typicalDurationText,
                        frequencyDetail.TypicalDuration +
                        (frequencyDetail.TypicalDuration > 1 ? TXT_Mins : TXT_Min));
                }
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Returns a string containing the service number for the frequency leg
        /// </summary>
        private string GetServiceNumberText()
        {
            if ((journeyLeg.JourneyDetails != null) && (journeyLeg.JourneyDetails.Count > 0)
                && (journeyLeg.Mode == SJPModeType.Bus || journeyLeg.Mode == SJPModeType.Coach
                || journeyLeg.Mode == SJPModeType.TransitShuttleBus))
            {
                if (journeyLeg.JourneyDetails[0] is PublicJourneyDetail)
                {
                    PublicJourneyDetail joureneyDetail = (PublicJourneyDetail)journeyLeg.JourneyDetails[0];

                    StringBuilder serviceDetailsText = new StringBuilder();
                    for (int count = 0; count < joureneyDetail.Services.Count; count++)
                    {
                        serviceDetailsText.Append(joureneyDetail.Services[count].ServiceNumber);
                        if (count < joureneyDetail.Services.Count - 1)
                        {
                            serviceDetailsText.Append(",");
                        }
                    }

                    if (!string.IsNullOrEmpty(serviceDetailsText.ToString()))
                    {
                        return string.Format("<span class='servicenumber'>{0}</span>", serviceDetailsText.ToString());
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns string containing transfer mode text 
        /// </summary>
        private string GetModeTransferDetailText(bool isFirstLeg, bool isLastLeg, bool isAccessibleJourney)
        {
            string transferModeText = string.Empty;

            // Only show text is not an accessible journey 
            // (because accessible journey may have manually added a transfer from/to venue leg to start/end of journey,
            // which we don't to show any text for)
            if (isFirstLeg && !isAccessibleJourney)
            {
                string TXT_transferModeFrom = resourceManager.GetString(CurrentLanguage.Value, RG, RC, "JourneyOutput.Text.TransferMode.From");

                transferModeText = string.Format(TXT_transferModeFrom, journeyLeg.LegStart.Location.DisplayName);
            }
            else if (isLastLeg && !isAccessibleJourney)
            {
                string TXT_transferModeTo = resourceManager.GetString(CurrentLanguage.Value, RG, RC, "JourneyOutput.Text.TransferMode.To");

                transferModeText = string.Format(TXT_transferModeTo, journeyLeg.LegEnd.Location.DisplayName);
            }

            return transferModeText;
        }

        /// <summary>
        /// Rounds the given double to the nearest int.
        /// If double is 0.5, then rounds up.
        /// Using this instead of Math.Round because Math.Round
        /// ALWAYS returns the even number when rounding a .5 -
        /// this is not behaviour we want.
        /// </summary>
        /// <param name="valueToRound">Value to round.</param>
        /// <returns>Nearest integer</returns>
        private static int Round(double valueToRound)
        {
            // Get the decimal point
            double valueFloored = Math.Floor(valueToRound);
            double remain = valueToRound - valueFloored;

            if (remain >= 0.5)
                return (int)Math.Ceiling(valueToRound);
            else
                return (int)Math.Floor(valueToRound);
        }

        #endregion

        /// <summary>
        /// Returns true if the detail car leg should be visible
        /// </summary>
        private bool ShowCarLeg
        {
            get
            {
                bool show = false;

                // Show if its a car leg
                if (journeyLeg != null)
                {
                    if (journeyLeg.Mode == SJPModeType.Car)
                    {
                        // And it contains RoadJourneyDetails
                        if ((journeyLeg.JourneyDetails != null) && (journeyLeg.JourneyDetails.Count > 0))
                        {
                            if (journeyLeg.JourneyDetails[0] is RoadJourneyDetail)
                            {
                                show = true;
                            }
                        }
                    }
                }

                return show;
            }
        }

        /// <summary>
        /// Returns true if the detail cycle leg should be visible
        /// </summary>
        private bool ShowCycleLeg
        {
            get
            {
                bool show = false;

                // Show if its a cycle leg
                if (journeyLeg != null)
                {
                    if (journeyLeg.Mode == SJPModeType.Cycle)
                    {
                        // And it contains CycleJourneyDetails
                        if ((journeyLeg.JourneyDetails != null) && (journeyLeg.JourneyDetails.Count > 0))
                        {
                            if (journeyLeg.JourneyDetails[0] is CycleJourneyDetail)
                            {
                                show = true;
                            }
                        }
                    }
                }

                return show;
            }
        }

        #endregion
    }
}