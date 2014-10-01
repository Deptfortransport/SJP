// *********************************************** 
// NAME             : PublicJourneyDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: PublicJourneyDetail class containing details specific for a public journey leg
// ************************************************
// 

using System;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;
using LS = SJP.Common.LocationService;
using SJP.Common.LocationService;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// PublicJourneyDetail class containing details specific for a public journey leg
    /// </summary>
    [Serializable()]
    public class PublicJourneyDetail : JourneyDetail
    {
        #region Private members

        // Static values
        private static string airPrefix = null;
        private static string coachPrefix = null;
        private static string railPrefix = null;
        private static List<string> trapezeRegions = null;

        // Public journey detail information
        private string region = string.Empty;
        private List<ServiceDetail> serviceDetails = new List<ServiceDetail>();
        private List<int> vehicleFeatures = new List<int>();
        private List<string> displayNotes = new List<string>();
        private bool isTrapezeRegion = false;

        // Accessible information
        private List<SJPAccessibilityType> serviceAccessibility = new List<SJPAccessibilityType>();
        private List<SJPAccessibilityType> stopBoardAccessibility = new List<SJPAccessibilityType>();
        private List<SJPAccessibilityType> stopAlightAccessibility = new List<SJPAccessibilityType>();

        // Used for flight specific
        private DateTime checkInTime = DateTime.MinValue;
        private DateTime exitTime = DateTime.MinValue;
        private string transferDescription = string.Empty;

        #endregion

        #region Constructor Static
        
        /// <summary>
        /// Static Constructor - loads static values
        /// </summary>
        static PublicJourneyDetail()
        {
            // Property provider
            IPropertyProvider properties = Properties.Current; 
            
            // Set prefix values
            airPrefix = properties[LS.Keys.NaptanPrefix_Airport];
            coachPrefix = properties[LS.Keys.NaptanPrefix_Coach];
            railPrefix = properties[LS.Keys.NaptanPrefix_Rail];

            // Set trapeze regions
            try
            {
                // initialise in case this errors
                trapezeRegions = new List<string>();

                string strTrapezeRegions = properties[Keys.TrapezeRegions];

                if (!string.IsNullOrEmpty(strTrapezeRegions))
                {
                    trapezeRegions.AddRange(strTrapezeRegions.Split(','));
                }
            }
            catch
            {
                // Log the error when in verbose mode
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose,
                    string.Format("Property [{0}] failed to parse into trapezeRegions array", Keys.TrapezeRegions)));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PublicJourneyDetail()
            : base()
        {
        }

        /// <summary>
        /// Takes a CJP leg and creates a new instance of this 
        /// class populated with the leg information
        /// </summary>
        public PublicJourneyDetail(ICJP.Leg cjpLeg)
            : this(cjpLeg, null, null)
        {
        }

        /// <summary>
        /// Takes a CJP leg and creates a new instance of this 
        /// class populated with the leg information
        /// </summary>
        public PublicJourneyDetail(ICJP.Leg cjpLeg, ICJP.Leg cjpPreviousLeg, ICJP.Leg cjpSubsequentLeg)
            : base()
        {
            #region Mode

            // SJPModeType of this detail
            mode = PopulateMode(cjpLeg.mode);

            #endregion

            #region Vehicle features

            // Vehicle features
            if (cjpLeg.vehicleFeatures != null)
            {
                foreach (ICJP.VehicleFeature vf in cjpLeg.vehicleFeatures)
                {
                    vehicleFeatures.Add(vf.id);
                }
            }

            #endregion

            #region Region

            // Region
            if (!string.IsNullOrEmpty(cjpLeg.region))
            {
                region = cjpLeg.region.Trim().ToUpper();
            }

            // Set the trapeze region flag, to ensure the Display notes get formatted correctly when accessed
            if (trapezeRegions != null)
            {
                isTrapezeRegion = (trapezeRegions.Contains(region));
            }

            #endregion

            #region Service details

            // Populate the services property
            serviceDetails = PopulateServiceDetails(cjpLeg.services);

            #endregion

            #region Display notes

            displayNotes = PopulateDisplayNotes(cjpLeg.notes);
                       
            #endregion

            #region Accessibility details

            // Leg accessibility details for board and alight stops
            if ((cjpLeg.board != null) && (cjpLeg.board.stop != null) && (cjpLeg.board.stop.accessibility != null))
            {
                ICJP.Accessibility cjpAccessibilityBoard = cjpLeg.board.stop.accessibility;

                stopBoardAccessibility = PopulateStopAccessibility(cjpAccessibilityBoard, true);
            }

            if ((cjpLeg.alight != null) && (cjpLeg.alight.stop != null) && (cjpLeg.alight.stop.accessibility != null))
            {
                ICJP.Accessibility cjpAccessibilityAlight = cjpLeg.alight.stop.accessibility;

                stopAlightAccessibility = PopulateStopAccessibility(cjpAccessibilityAlight, true);
            }

            // Leg accessibility details for service
            if (cjpLeg.serviceAccessibility != null)
            {
                ICJP.ServiceAccessibility cjpServiceAccessibility = cjpLeg.serviceAccessibility;

                serviceAccessibility = PopulateServiceAccessibility(cjpServiceAccessibility);
            }
                        
            #endregion

            #region CheckIn and Exit times used for flight specific legs

            // If previous leg is provided and is of the correct type, then use it for check in time
            if ((cjpPreviousLeg != null) && (cjpPreviousLeg.mode == ICJP.ModeType.CheckIn))
            {
                checkInTime = DateTimeHelper.GetDateTime(cjpPreviousLeg.board.departTime);
            }

            // If subsequent leg is provided and is of the correct type, then use it for exit time
            if ((cjpSubsequentLeg != null) &&
                ((cjpSubsequentLeg.mode == ICJP.ModeType.CheckOut) || (cjpSubsequentLeg.mode == ICJP.ModeType.Transfer)))
            {
                exitTime = DateTimeHelper.GetDateTime(cjpSubsequentLeg.alight.arriveTime);

                if (cjpSubsequentLeg.mode == ICJP.ModeType.Transfer)
                {
                    transferDescription = cjpSubsequentLeg.description;
                }
            }

            #endregion

            // SJP is not interested where the service origin or destination is

            // SJP is not interested in the service calling points before, intermediate, or after 
            // where the user boards or alights
        }

        /// <summary>
        /// Takes a CJP StopEvent and creates a new instance of this 
        /// class populated with the StopEvent information
        /// </summary>
        public PublicJourneyDetail(ICJP.StopEvent cjpStopEvent)
        {
            #region Mode

            // SJPModeType of this detail
            mode = PopulateMode(cjpStopEvent.mode);

            #endregion

            #region Service details

            // Populate the services property
            serviceDetails = PopulateServiceDetails(cjpStopEvent.services);

            #endregion

            #region Display notes

            displayNotes = PopulateDisplayNotes(cjpStopEvent.notes);

            #endregion
        }


        #endregion

        #region Public Static methods

        /// <summary>
        /// Factory method that creates an appropriate subclass of PublicJourneyDetail
        /// based on the type of leg supplied.
        /// </summary>
        /// <param name="leg">The journey leg details passed back from the CJP</param>
        /// <param name="previousLeg">The previous leg in the list. This is necessary for flights only.</param>
        /// <param name="subsequentLeg">The subsequent leg in the list. This is necessary for flights only.</param>
        /// <returns></returns>
        public static PublicJourneyDetail Create(ICJP.Leg cjpLeg, ICJP.Leg cjpPreviousLeg, ICJP.Leg cjpSubsequentLeg)
        {
            if (cjpLeg is ICJP.FrequencyLeg)
            {
                return new PublicJourneyFrequencyDetail(cjpLeg);
            }
            // InterchangeLeg inherits from ContinuousLeg, so check before
            else if (cjpLeg is ICJP.InterchangeLeg)
            {
                return new PublicJourneyInterchangeDetail(cjpLeg);
            }
            else if (cjpLeg is ICJP.ContinuousLeg)
            {
                return new PublicJourneyContinuousDetail(cjpLeg);
            }
            else if (cjpLeg is ICJP.TimedLeg)
            {
                return new PublicJourneyTimedDetail(cjpLeg, cjpPreviousLeg, cjpSubsequentLeg);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Factory method that creates a PublicJourneyTimedDetail
        /// based for the CJP StopEvent
        /// </summary>
        /// <param name="cjpStopEvent">The CJP StopEvent</param>
        /// <returns></returns>
        public static PublicJourneyDetail Create(ICJP.StopEvent cjpStopEvent, SJPLocation startLocation, SJPLocation endLocation)
        {
            if (cjpStopEvent != null)
            {
                return new PublicJourneyTimedDetail(cjpStopEvent, startLocation, endLocation);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. Region for this journey detail
        /// </summary>
        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        /// <summary>
        /// Read/Write. Details of the services used in this journey detail
        /// </summary>
        public List<ServiceDetail> Services
        {
            get { return serviceDetails; }
            set { serviceDetails = value; }
        }

        /// <summary>
        /// Read/Write. Vehicle features for this journey detail
        /// </summary>
        public List<int> VehicleFeatures
        {
            get { return vehicleFeatures; }
            set { vehicleFeatures = value; }
        }

        /// <summary>
        /// Read/Write. Display notes
        /// </summary>
        public List<string> DisplayNotes
        {
            get { return displayNotes; }
            set { displayNotes = value; }
        }

        /// <summary>
        /// Read/Write. Gets the check-in datetime (the time a user 
        /// must arrive at the airport for an Air leg).
        /// </summary>
        public DateTime CheckInTime
        {
            get { return checkInTime; }
            set { checkInTime = value; }
        }

        /// <summary>
        /// Read/Write. Gets the exit datetime (the time a user 
        /// can actually leave the airport for an Air leg).
        /// </summary>
        public DateTime ExitTime
        {
            get { return exitTime; }
            set { exitTime = value; }
        }

        /// <summary>
        /// Read/Write. Transfer description (text describing the 
        /// airport transfer stage that immediately follows this leg).  
        /// </summary>
        public string TransferDescription
        {
            get { return transferDescription; }
            set { transferDescription = value; }
        }

        /// <summary>
        /// Read/Write. Overall accessibility details for the services used in this journey detail
        /// </summary>
        public List<SJPAccessibilityType> ServiceAccessibility
        {
            get { return serviceAccessibility; }
            set { serviceAccessibility = value; }
        }

        /// <summary>
        /// Read/Write. Accessibility details for the Board stop of this journey detail
        /// </summary>
        public List<SJPAccessibilityType> BoardAccessibility
        {
            get { return stopBoardAccessibility; }
            set { stopBoardAccessibility = value; }
        }

        /// <summary>
        /// Read/Write. Accessibility details for the Alight stop of this journey detail
        /// </summary>
        public List<SJPAccessibilityType> AlightAccessibility
        {
            get { return stopAlightAccessibility; }
            set { stopAlightAccessibility = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns true if this PublicJourneyDetail contains "check constraints" which 
        /// requires an instruction to be displayed in the UI.
        /// May only be true for PublicJourneyInterchangeDetail detail types
        /// </summary>
        public virtual bool HasCheckConstraint()
        {
            return false;
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Updates the Display Notes, removing "invalid" characters 
        /// if notes were returned from a trapeze region
        /// </summary>
        private List<string> UpdateDisplayNotes(List<string> displayNotes)
        {
            if (isTrapezeRegion)
            {
                for (int i = 0; i < displayNotes.Count; i++)
                {
                    displayNotes[i] = displayNotes[i].ToString().Replace("O:", string.Empty);
                    displayNotes[i] = displayNotes[i].ToString().Replace("D:", string.Empty);
                    displayNotes[i] = displayNotes[i].ToString().Replace(";", "\n");
                }
            }

            return displayNotes;
        }

        #region Populate helper methods

        /// <summary>
        /// Returns the SJPModeType for a CJP ModeType
        /// </summary>
        /// <returns></returns>
        private SJPModeType PopulateMode(ICJP.ModeType cjpMode)
        {
            SJPModeType mode = SJPModeTypeHelper.GetSJPModeType(cjpMode);

            // Overide ModeType.Drt with Bus
            if (mode == SJPModeType.Drt)
            {
                mode = SJPModeType.Bus;
            }

            return mode;
        }

        /// <summary>
        /// Returns a list of ServiceDetail for an array of CJP Service
        /// </summary>
        /// <param name="cjpServices"></param>
        /// <returns></returns>
        private List<ServiceDetail> PopulateServiceDetails(ICJP.Service[] cjpServices)
        {
            List<ServiceDetail> serviceDetails = new List<ServiceDetail>();

            if (cjpServices != null)
            {
                foreach (ICJP.Service service in cjpServices)
                {
                    // Check there are some valid details, prevents an empty service detail from being added
                    if ((!string.IsNullOrEmpty(service.operatorCode))
                        || (!string.IsNullOrEmpty(service.operatorName))
                        || (!string.IsNullOrEmpty(service.destinationBoard))
                        || (!string.IsNullOrEmpty(service.serviceNumber)))
                    {
                        ServiceDetail sd = new ServiceDetail(
                            service.operatorCode,
                            service.operatorName,
                            service.serviceNumber,
                            service.destinationBoard,
                            service.direction,
                            service.privateID,
                            service.retailId);

                        serviceDetails.Add(sd);
                    }
                }
            }

            return serviceDetails;
        }

        /// <summary>
        /// Returns a list of string display notes for an array of CJP Note
        /// </summary>
        /// <returns></returns>
        private List<string> PopulateDisplayNotes(ICJP.Note[] notes)
        {
            List<string> displayNotes = new List<string>();

            // Leg display notes
            if (notes != null)
            {
                foreach (ICJP.Note note in notes)
                {
                    displayNotes.Add(note.message);
                }

                displayNotes = UpdateDisplayNotes(displayNotes);
            }

            return displayNotes;
        }

        /// <summary>
        /// Populates the service accessibility list using the CJP ServiceAccessibility detail
        /// </summary>
        /// <param name="cjpServiceAccessibility"></param>
        private List<SJPAccessibilityType> PopulateServiceAccessibility(ICJP.ServiceAccessibility cjpServiceAccessibility)
        {
            List<SJPAccessibilityType> accessibility = new List<SJPAccessibilityType>();

            if (cjpServiceAccessibility.lowFloor)
            {
                // Only show for Bus and Tram mode types
                if ((mode == SJPModeType.Bus) ||
                    (mode == SJPModeType.Tram))
                {
                    accessibility.Add(SJPAccessibilityType.ServiceLowFloor);
                }
            }
            if (cjpServiceAccessibility.mobilityImpairedAccess == ICJP.AccessibilityType.True)
            {
                // Only show for Ferry and Underground mode types
                if ((mode == SJPModeType.Ferry) ||
                    (mode == SJPModeType.Underground))
                {
                    accessibility.Add(SJPAccessibilityType.MobilityImpairedAccess);
                }
            }

            if (cjpServiceAccessibility.wheelchairBookingRequired)
            {
                // Only show for Rail and Coach mode types
                if ((mode == SJPModeType.Rail) ||
                    (mode == SJPModeType.Coach))
                {
                    accessibility.Add(SJPAccessibilityType.ServiceWheelchairBookingRequired);
                }
            }

            // Assistance available for the service
            if (cjpServiceAccessibility.assistanceServices != null)
            {
                foreach (ICJP.AssistanceServiceType ast in cjpServiceAccessibility.assistanceServices)
                {
                    accessibility.Add(SJPAccessibilityTypeHelper.GetSJPAccessibilityType(ast));
                }
            }

            return accessibility;
        }

        /// <summary>
        /// Populates the stop accessibility list using the CJP Accessibility detail,
        /// and filters out accessibility types not required for output
        /// </summary>
        /// <param name="cjpAccessibility"></param>
        /// <param name="isForStop">true if CJP Accessibility is for a Board/Alight stop, 
        /// false if for a Leg (e.g. InterchangeLeg accessibility details) </param>
        protected List<SJPAccessibilityType> PopulateStopAccessibility(ICJP.Accessibility cjpAccessibility, bool isForStop)
        {
            // NOTE: Commented out lines are not required for output in SJP

            List<SJPAccessibilityType> accessibility = new List<SJPAccessibilityType>();

            // Only want to parse Access types for Board/Alight stops, not for legs
            if (isForStop)
            {
                if (cjpAccessibility.escalatorFreeAccess == ICJP.AccessibilityType.True)
                {
                    //accessibility.Add(SJPAccessibilityType.EscalatorFreeAccess);
                }
                if (cjpAccessibility.liftFreeAccess == ICJP.AccessibilityType.True)
                {
                    //accessibility.Add(SJPAccessibilityType.LiftFreeAccess);
                }
                if (cjpAccessibility.mobilityImpairedAccess == ICJP.AccessibilityType.True)
                {
                    //accessibility.Add(SJPAccessibilityType.MobilityImpairedAccess);
                }
                if (cjpAccessibility.stepFreeAccess == ICJP.AccessibilityType.True)
                {
                    //accessibility.Add(SJPAccessibilityType.StepFreeAccess);
                }
                if (cjpAccessibility.wheelchairAccess == ICJP.AccessibilityType.True)
                {
                    accessibility.Add(SJPAccessibilityType.WheelchairAccess);
                }
            }

            // Only want to parse Access Features for legs, not Board/Alight stops
            if (!isForStop)
            {
                if (cjpAccessibility.accessSummary != null)
                {
                    foreach (ICJP.AccessSummary accessSummary in cjpAccessibility.accessSummary)
                    {
                        SJPAccessibilityType sat = SJPAccessibilityTypeHelper.GetSJPAccessibilityType(accessSummary);

                        // Only keep those wanted for UI output
                        if (sat == SJPAccessibilityType.AccessLiftDown
                            || sat == SJPAccessibilityType.AccessLiftUp
                            || sat == SJPAccessibilityType.AccessEscalatorDown
                            || sat == SJPAccessibilityType.AccessEscalatorUp
                            || sat == SJPAccessibilityType.AccessStairsDown
                            || sat == SJPAccessibilityType.AccessStairsUp
                            || sat == SJPAccessibilityType.AccessRampDown
                            || sat == SJPAccessibilityType.AccessRampUp)
                        {
                            // Display all even if there are duplicates
                            accessibility.Add(sat);
                        }
                    }
                }
            }

            return accessibility;
        }

        #endregion

        #endregion
    }
}
