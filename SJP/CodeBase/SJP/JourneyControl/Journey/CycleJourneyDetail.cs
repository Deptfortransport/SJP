// *********************************************** 
// NAME             : CycleJourneyDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: CycleJourneyDetail class containing details specific for a cycle journey leg
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPWS = SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;
using Logger = System.Diagnostics.Trace;
using SJP.Common.EventLogging;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// CycleJourneyDetail class containing details specific for a cycle journey leg
    /// </summary>
    [Serializable()]
    public class CycleJourneyDetail : JourneyDetail
    {
        #region Private members
        
        private int congestionlevel;
        private char coordinateSeperator;
        private char eastingNorthingSeperator;
        
        // private members
        private bool driveSection = false;
        private bool stopoverSection = false;
        private bool junctionSection = false;

        private int sectionId;
        
        private string roadName;
        private string roadNumber;
        private string placeName;

        private string junctionNumber;
        private CPWS.JunctionType junctionType;
        private bool junctionDriveSectionWithoutJunctionNo;

        private int cost; // in 10000th pence
        private int congestionZoneCost;
        private int tollCost;
        private int ferryCost;

        private bool roadSplits = false;
        private bool slipRoad = false;
        private bool ferry = false;
        private bool congestion = false;

        private int turnCount;
        private CPWS.TurnDirection turnDirection;
        private CPWS.TurnAngle turnAngle;

        private Dictionary<int, bool> interpolateGradient;
        private OSGridReference startOSGR;
        private string[] toid;
        private string nodeToid;

        private string instructionText;
        private string manoeuvreText;
        private string namedAccessRestrictionText;

        private Dictionary<string, string> cycleRoutes; // the cycle routes this section applies to
        private bool cycleRoute = false;

        private uint[] joiningSignificantLinkAttributes = new uint[0]; // cycle specific attributes
        private uint[] leavingSignificantLinkAttributes = new uint[0];
        private uint[] interestingLinkAttributes = new uint[0];
        private uint[] significantNodeAttributes = new uint[0];
        private uint[] sectionFeatureAttributes = new uint[0];

        private string companyUrl;

        private string congestionZoneEntry;
        private string congestionZoneExit;
        private string congestionZoneEnd;
        private bool congestionEntry = false;
        private bool congestionExit = false;
        private bool congestionEnd = false;

        private string ferryCheckInName;
        private bool ferryCheckIn = false;
        private bool ferryCheckOut = false;
        private bool displayFerryIcon = false;

        private string tollEntryName;
        private string tollExitName;
        private bool tollEntry = false;
        private bool tollExit = false;
        private bool displayTollIcon = false;

        private bool viaLocation = false;

        private bool waitSection = false;
        private bool undefinedWaitSection = false;

        private bool complexManoeuvre = false;
        private bool namedAccessRestriction = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CycleJourneyDetail()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CycleJourneyDetail(CPWS.Section ctpSection)
            : base()
        {
            // populate helper values
            congestionlevel = Properties.Current[Keys.CongestionLevel].Parse<int>();
            coordinateSeperator = CyclePlannerRequestPopulator.GetCoordinateSeperator();
            eastingNorthingSeperator = CyclePlannerRequestPopulator.GetEastingNorthingSeperator();
                        
            sectionId = ctpSection.sectionID;
            durationSecs = Convert.ToInt32(ctpSection.time.Ticks / TimeSpan.TicksPerSecond);


            // Is this a stop over section, drive section or a junction drive section?
            Type sectionType = ctpSection.GetType();

            if (sectionType == typeof(CPWS.StopoverSection))
            {
                #region StopoverSection

                stopoverSection = true;
                driveSection = false;
                junctionSection = false;

                // Stop Over section information
                CPWS.StopoverSection thisSection = ctpSection as CPWS.StopoverSection;

                roadName = (thisSection.name != null ? thisSection.name.Trim() : string.Empty);

                // Get the toid and geometry
                if (thisSection.node == null || thisSection.node.TOID == null)
                {
                    nodeToid = string.Empty;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                        string.Format(Messages.ROAD_NoNodeToid, thisSection.name, thisSection.type)));
                }
                else
                {
                    nodeToid = thisSection.node.TOID;

                    // Populate the geometry for this stopover section
                    if (!string.IsNullOrEmpty(thisSection.node.geometry))
                    {
                        PopulateGeometry(0, thisSection.node.geometry, false);
                    }
                }

                tollCost = thisSection.toll;

                // Need to establish what type of Stopover it is and handle accordingly

                #region ComplexManoeuvre
                if (thisSection.type == CPWS.StopoverSectionType.ComplexManoeuvre)
                {
                    complexManoeuvre = true;

                    manoeuvreText = thisSection.name.Trim();

                    #region Populate the Section feature attributes
                    // Section Features (e.g. information on details of an operator, contact details for a ferry...).
                    // For a stopover section type of ComplexManoeuvre, there will be a SectionFeatureAttribute defined
                    if ((thisSection.sectionFeatures != null) && (thisSection.sectionFeatures.Length != 0))
                    {
                        // Initialise the attributes array, assuming we will only ever have upto 4 attribute integers
                        if (sectionFeatureAttributes.Length == 0)
                        {
                            sectionFeatureAttributes = new uint[4] { 0, 0, 0, 0 };
                        }

                        foreach (CPWS.SectionFeature feature in thisSection.sectionFeatures)
                        {
                            // The feature.id indicates whether the value is for ITN attribute (id = 0), or User attribuutes (id > 0)
                            if (feature.GetType() == typeof(CPWS.SectionFeatureAttributes))
                            {
                                CPWS.SectionFeatureAttributes featureAttribute = feature as CPWS.SectionFeatureAttributes;

                                if (featureAttribute.id < 4) // Assume only ever get up to 4 attribute integers
                                {
                                    if (featureAttribute.value >= 0)
                                    {
                                        sectionFeatureAttributes[featureAttribute.id] = Convert.ToUInt32(featureAttribute.value);
                                    }
                                    else
                                    {
                                        // Should never reach here
                                        Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                                            String.Format("CycleJourneyDetail ComplexManoeuvre contains an unsupported UInt32 feature attribute value, {0}, for the section {1}, {2}",
                                                featureAttribute.value, thisSection.name, thisSection.type)));

                                        sectionFeatureAttributes[featureAttribute.id] = 0;
                                    }
                                }
                            }

                        }
                    }
                    #endregion
                }

                #endregion

                #region NamedAccessRestriction
                if (thisSection.type == CPWS.StopoverSectionType.NamedAccessRestriction)
                {
                    namedAccessRestriction = true;

                    namedAccessRestrictionText = thisSection.name;
                }

                #endregion

                #region CongestionZoneEntry
                if (thisSection.type == CPWS.StopoverSectionType.CongestionZoneEntry)
                {
                    congestionEntry = true;
                    congestionZoneEntry = thisSection.name;
                    congestionZoneCost = thisSection.toll;

                    if ((thisSection.sectionFeatures != null) && (thisSection.sectionFeatures.Length != 0))
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (CPWS.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            CPWS.TextSectionFeature tsec = (CPWS.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:	//Extract url of website
                                    companyUrl = tsec.value;
                                    break;

                            }
                        }
                    }
                }
                #endregion

                #region CongestionZoneExit
                if (thisSection.type == CPWS.StopoverSectionType.CongestionZoneExit)
                {
                    congestionExit = true;
                    congestionZoneExit = thisSection.name;
                    congestionZoneCost = thisSection.toll;

                    if ((thisSection.sectionFeatures != null) && (thisSection.sectionFeatures.Length != 0))
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (CPWS.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            CPWS.TextSectionFeature tsec = (CPWS.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:		//Extract url of website
                                    companyUrl = tsec.value;
                                    break;
                            }
                        }
                    }
                }
                #endregion

                #region CongestionZoneEnd
                if (thisSection.type == CPWS.StopoverSectionType.CongestionZoneEnd)
                {
                    congestionEnd = true;
                    congestionZoneEnd = thisSection.name;
                    congestionZoneCost = thisSection.toll;

                    if ((thisSection.sectionFeatures != null) && (thisSection.sectionFeatures.Length != 0))
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (CPWS.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            CPWS.TextSectionFeature tsec = (CPWS.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:		//Extract url of website
                                    companyUrl = tsec.value;
                                    break;
                            }
                        }
                    }
                }
                #endregion

                #region FerryCheckIn
                if (thisSection.type == CPWS.StopoverSectionType.FerryCheckIn)
                {
                    ferryCost = thisSection.toll;
                    ferryCheckInName = thisSection.name;
                    ferryCheckIn = true;
                    displayFerryIcon = true;

                    if ((thisSection.sectionFeatures != null) && (thisSection.sectionFeatures.Length != 0))
                    {
                        string ferryRouteUrl = string.Empty;
                        //need to extract company name and URL from stopoversection section features	
                        foreach (CPWS.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            CPWS.TextSectionFeature tsec = (CPWS.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:		//Extract url of website
                                    companyUrl = tsec.value;
                                    break;
                                case 14:
                                    ferryRouteUrl = tsec.value;
                                    break;
                            }

                            // The companyUrl property actually exposes either the ferry route Url or the company Url.
                            // The ferry route Url takes precedence.
                            if (ferryRouteUrl != null && ferryRouteUrl.Length > 0)
                                companyUrl = ferryRouteUrl;
                        }
                    }
                }
                #endregion

                #region FerryCheckOut
                if (thisSection.type == CPWS.StopoverSectionType.FerryCheckOut)
                {
                    //Cost not displayed on exiting a ferry
                    ferryCheckOut = true;
                    displayFerryIcon = true;
                }
                #endregion

                #region TollEntry
                if (thisSection.type == CPWS.StopoverSectionType.TollEntry)
                {
                    tollCost = thisSection.toll;
                    tollEntryName = thisSection.name;
                    tollEntry = true;
                    displayTollIcon = true;

                    if ((thisSection.sectionFeatures != null) && (thisSection.sectionFeatures.Length != 0))
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (CPWS.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            CPWS.TextSectionFeature tsec = (CPWS.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:		//Extract url
                                    companyUrl = tsec.value;
                                    break;
                            }
                        }
                    }
                }
                #endregion

                #region TollExit
                if (thisSection.type == CPWS.StopoverSectionType.TollExit)
                {
                    tollCost = thisSection.toll;
                    tollExitName = thisSection.name;
                    tollExit = true;
                    displayTollIcon = true;

                    if ((thisSection.sectionFeatures != null) && (thisSection.sectionFeatures.Length != 0))
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (CPWS.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            CPWS.TextSectionFeature tsec = (CPWS.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:		//Extract url
                                    companyUrl = tsec.value;
                                    break;
                            }

                        }
                    }
                }
                #endregion

                #region Via Section
                if (thisSection.type == CPWS.StopoverSectionType.Via)
                {
                    roadName = "VIALOCAION";
                    viaLocation = true;
                }
                #endregion

                #region Wait Section
                if (thisSection.type == CPWS.StopoverSectionType.Wait)
                {
                    //If it is a wait type, we need to add the time to the journey duration
                    //but we don't want to display any journey directions for the wait.
                    roadName = "WAIT";
                    waitSection = true;
                }
                #endregion

                #region Undefined Wait
                if (thisSection.type == CPWS.StopoverSectionType.UndefinedWait)
                {
                    undefinedWaitSection = true;
                }
                #endregion

                #endregion
            }

            else if (sectionType == typeof(CPWS.DriveSection))
            {
                #region DriveSection

                driveSection = true;
                junctionSection = false;
                stopoverSection = false;

                // Drive section
                CPWS.DriveSection thisSection = ctpSection as CPWS.DriveSection;

                roadName = (thisSection.name != null ? thisSection.name.Trim() : string.Empty);
                roadNumber = (thisSection.number != null ? thisSection.number.Trim() : string.Empty);
                placeName = (thisSection.heading != null ? thisSection.heading.Trim() : string.Empty);

                instructionText = (thisSection.instructionText != null ? thisSection.instructionText : string.Empty);

                cost = thisSection.cost;

                // Don't include Ferry Journeys in the total distance, only in
                // total time. Updated to include check for road number in addition to road name. 
                if (roadNumber != Messages.ROAD_NUMBER_FERRY)
                {
                    distance = thisSection.distance;
                    ferry = false;
                }
                else
                {
                    // Section is a ferry section - set flag for formatters
                    ferry = true;
                }

                turnCount = thisSection.turnCount;
                turnDirection = thisSection.turnDirection;
                turnAngle = thisSection.turnAngle;

                roadSplits = thisSection.roadSplits;
                slipRoad = false;

                // Need to loop through every link to check for congestion levels
                toid = new string[thisSection.links.Length];
                for (int i = 0; i < thisSection.links.Length; i++)
                {
                    toid[i] = thisSection.links[i].TOID;

                    // congestion
                    if (thisSection.links[i].congestion > congestionlevel)
                    {
                        congestion = true;
                    }

                    // geometry
                    if (!string.IsNullOrEmpty(thisSection.links[i].geometry))
                    {
                        // Add the geometry to the list for this link
                        PopulateGeometry(i, thisSection.links[i].geometry, thisSection.links[i].interpolateGradient);
                    }
                }

                // Add the cycle routes this section applies to
                if ((thisSection.cycleRoutes != null) && (thisSection.cycleRoutes.Length > 0))
                {
                    if (cycleRoutes == null)
                    {
                        cycleRoutes = new Dictionary<string, string>();
                    }

                    // Cycle route may not have a number, so we need to assign a temporary key/value for the dictionary
                    int tempNumber = -100;

                    foreach (CPWS.CycleRoute cr in thisSection.cycleRoutes)
                    {
                        string cycleRouteNumber = string.Empty;

                        // if null, give it a temp key value
                        if (string.IsNullOrEmpty(cr.number))
                        {
                            cycleRouteNumber = tempNumber.ToString();
                            tempNumber++;
                        }
                        else
                        {
                            cycleRouteNumber = cr.number.Trim();
                        }

                        // Add the cycle route to the dictionary
                        cycleRoutes.Add(cycleRouteNumber, cr.name.Trim());
                    }

                    cycleRoute = true;
                }

                // Add the cycle specific attributes. These are converted into actual instructions by the UI
                joiningSignificantLinkAttributes = thisSection.joiningSignificantLinkAttributes;
                leavingSignificantLinkAttributes = thisSection.leavingSignificantLinkAttributes;
                interestingLinkAttributes = thisSection.interestingLinkAttributes;
                significantNodeAttributes = thisSection.nodeAttributes;
                                
                #endregion
            }

            else if (sectionType == typeof(CPWS.JunctionDriveSection))
            {
                #region JunctionDriveSection

                // A JunctionDriveSection is used to describe turns on motorway junctions, so it should
                // not be used by the results returned by the cycle planner

                junctionSection = true;
                driveSection = false;
                stopoverSection = false;

                // Junction Drive Section
                CPWS.JunctionDriveSection thisSection = ctpSection as CPWS.JunctionDriveSection;

                roadName = (thisSection.name != null ? thisSection.name.Trim() : string.Empty);
                roadNumber = (thisSection.number != null ? thisSection.number.Trim() : string.Empty);
                placeName = (thisSection.heading != null ? thisSection.heading.Trim() : string.Empty);

                instructionText = (thisSection.instructionText != null ? thisSection.instructionText : string.Empty);

                cost = thisSection.cost;

                if (roadNumber != Messages.ROAD_NUMBER_FERRY)
                {
                    distance = thisSection.distance;
                    ferry = false;
                }
                else
                {
                    // Section is a ferry section - set flag for formatters
                    ferry = true;
                }

                // Treat all junction sections as slip roads
                slipRoad = true;

                turnCount = thisSection.turnCount;
                turnDirection = thisSection.turnDirection;
                turnAngle = thisSection.turnAngle;
                roadSplits = thisSection.roadSplits;

                //Need to loop through every link to check for congestion levels

                toid = new string[thisSection.links.Length];
                for (int i = 0; i < thisSection.links.Length; i++)
                {
                    toid[i] = thisSection.links[i].TOID;

                    // congestion
                    if (thisSection.links[i].congestion > congestionlevel)
                    {
                        congestion = true;
                    }

                    // geometry
                    if (!string.IsNullOrEmpty(thisSection.links[i].geometry))
                    {
                        // Add the geometry to the list for this link
                        PopulateGeometry(i, thisSection.links[i].geometry, thisSection.links[i].interpolateGradient);
                    }
                }

                // Although dealing with JunctionDriveSection, if no junction number 
                // present then need to override and treat as normal DriveSection.
                junctionNumber = thisSection.junctionNumber;
                junctionSection = ((junctionNumber != null) && (junctionNumber.Length != 0));
                driveSection = !junctionSection;

                junctionType = thisSection.type;

                junctionDriveSectionWithoutJunctionNo = (junctionNumber == null) || (junctionNumber.Length == 0);

                // Add the cycle routes this section applies to
                if ((thisSection.cycleRoutes != null) && (thisSection.cycleRoutes.Length > 0))
                {
                    if (cycleRoutes == null)
                    {
                        cycleRoutes = new Dictionary<string, string>();
                    }

                    foreach (CPWS.CycleRoute cr in thisSection.cycleRoutes)
                    {
                        cycleRoutes.Add(cr.number, cr.name);
                    }

                    cycleRoute = true;
                }

                // Add the cycle specific attributes. These are converted into actual instructions by the UI
                joiningSignificantLinkAttributes = thisSection.joiningSignificantLinkAttributes;
                leavingSignificantLinkAttributes = thisSection.leavingSignificantLinkAttributes;
                interestingLinkAttributes = thisSection.interestingLinkAttributes;
                significantNodeAttributes = thisSection.nodeAttributes;

                #endregion
            }
        }

        #endregion

        #region Public Static methods

        /// <summary>
        /// Is the CTP Section considered to be for a Ferry. 
        /// Returns true if the road number contains the Ferry identifier
        /// </summary>
        /// <param name="cjpSection"></param>
        /// <returns></returns>
        public static bool IsFerrySection(CPWS.Section cjpSection)
        {
            CPWS.JunctionDriveSection jds = cjpSection as CPWS.JunctionDriveSection;
            CPWS.DriveSection ds = cjpSection as CPWS.DriveSection;

            if (jds != null)
            {
                if (!string.IsNullOrEmpty(jds.number))
                {
                    if (jds.number == Messages.ROAD_NUMBER_FERRY)
                    {
                        // Section is a ferry section
                        return true;
                    }
                }
            }
            else if (ds != null)
            {
                if (!string.IsNullOrEmpty(ds.number))
                {
                    if (ds.number == Messages.ROAD_NUMBER_FERRY)
                    {
                        // Section is a ferry section
                        return true;
                    }
                }
            }

            // Section is not a ferry section
            return false;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. DriveSection flag for this detail
        /// </summary>
        public bool DriveSection
        {
            get { return driveSection; }
            set { driveSection = value; }
        }

        /// <summary>
        /// Read/Write. StopoverSection flag for this detail
        /// </summary>
        public bool StopoverSection
        {
            get { return stopoverSection; }
            set { stopoverSection = value; }
        }

        /// <summary>
        /// Read/Write. JunctionSection flag for this detail
        /// </summary>
        public bool JunctionSection
        {
            get { return junctionSection; }
            set { junctionSection = value; }
        }

        /// <summary>
        /// Read/Write. SectionId for this detail
        /// </summary>
        public int SectionId
        {
            get { return sectionId; }
            set { sectionId = value; }
        }

        /* Defined in base class 
        /// <summary>
        /// Read/Write. Distance for this detail
        /// </summary>
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// Read/Write. Duration for this detail, in seconds
        /// </summary>
        public long Duration
        {
            get { return duration; }
            set { duration = value; }
        }*/

        /// <summary>
        /// Read/Write. RoadName for this detail
        /// </summary>
        public string RoadName
        {
            get { return roadName; }
            set { roadName = value; }
        }

        /// <summary>
        /// Read/Write. RoadNumber for this detail
        /// </summary>
        public string RoadNumber
        {
            get { return roadNumber; }
            set { roadNumber = value; }
        }

        /// <summary>
        /// Read/Write. PlaceName for this detail
        /// </summary>
        public string PlaceName
        {
            get { return placeName; }
            set { placeName = value; }
        }

        /// <summary>
        /// Read/Write. JunctionNumber for this detail
        /// </summary>
        public string JunctionNumber
        {
            get { return junctionNumber; }
            set { junctionNumber = value; }
        }

        /// <summary>
        /// Read/Write. JunctionType for this detail
        /// </summary>
        public CPWS.JunctionType JunctionType
        {
            get { return junctionType; }
            set { junctionType = value; }
        }

        /// <summary>
        /// Read/Write. JunctionDriveSectionWithoutJunctionNo flag for this detail
        /// </summary>
        public bool JunctionDriveSectionWithoutJunctionNo
        {
            get { return junctionDriveSectionWithoutJunctionNo; }
            set { junctionDriveSectionWithoutJunctionNo = value; }
        }

        /// <summary>
        /// Read/Write. Cost for this detail, in 10000th pence
        /// </summary>
        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneCost for this detail
        /// </summary>
        public int CongestionZoneCost
        {
            get { return congestionZoneCost; }
            set { congestionZoneCost = value; }
        }

        /// <summary>
        /// Read/Write. TollCost for this detail
        /// </summary>
        public int TollCost
        {
            get { return tollCost; }
            set { tollCost = value; }
        }

        /// <summary>
        /// Read/Write. FerryCost for this detail
        /// </summary>
        public int FerryCost
        {
            get { return ferryCost; }
            set { ferryCost = value; }
        }

        /// <summary>
        /// Read/Write. RoadSplits flag for this detail
        /// </summary>
        public bool RoadSplits
        {
            get { return roadSplits; }
            set { roadSplits = value; }
        }

        /// <summary>
        /// Read/Write. SlipRoad flag for this detail
        /// </summary>
        public bool SlipRoad
        {
            get { return slipRoad; }
            set { slipRoad = value; }
        }

        /// <summary>
        /// Read/Write. Ferry flag for this detail
        /// </summary>
        public bool Ferry
        {
            get { return ferry; }
            set { ferry = value; }
        }

        /// <summary>
        /// Read/Write. Congestion flag for this detail
        /// </summary>
        public bool Congestion
        {
            get { return congestion; }
            set { congestion = value; }
        }

        /// <summary>
        /// Read/Write. TurnCount for this detail
        /// </summary>
        public int TurnCount
        {
            get { return turnCount; }
            set { turnCount = value; }
        }

        /// <summary>
        /// Read/Write. TurnDirection for this detail
        /// </summary>
        public CPWS.TurnDirection TurnDirection
        {
            get { return turnDirection; }
            set { turnDirection = value; }
        }

        /// <summary>
        /// Read/Write. TurnAngle for this detail
        /// </summary>
        public CPWS.TurnAngle TurnAngle
        {
            get { return turnAngle; }
            set { turnAngle = value; }
        }

        /// <summary>
        /// Read/Write. The interpolateGradient flag dictionary. This should be read in conjunction
        /// with the Geometry dictionary
        /// </summary>
        public Dictionary<int, bool> InterpolateGradient
        {
            get { return interpolateGradient; }
            set { interpolateGradient = value; }
        }

        /// <summary>
        /// Read/Write. The OSGR for the start of this detail
        /// </summary>
        public OSGridReference StartOSGR
        {
            get { return startOSGR; }
            set { startOSGR = value; }
        }

        /// <summary>
        /// Read/Write. Toid array for this detail
        /// </summary>
        public string[] Toid
        {
            get { return toid; }
            set { toid = value; }
        }

        /// <summary>
        /// Read/Write. NodeToid for this detail
        /// </summary>
        public string NodeToid
        {
            get { return nodeToid; }
            set { nodeToid = value; }
        }

        /// <summary>
        /// Read/Write. InstructionText for this detail
        /// </summary>
        public string InstructionText
        {
            get { return instructionText; }
            set { instructionText = value; }
        }

        /// <summary>
        /// Read/Write. ManoeuvreText for this detail
        /// </summary>
        public string ManoeuvreText
        {
            get { return manoeuvreText; }
            set { manoeuvreText = value; }
        }

        /// <summary>
        /// Read/Write. NamedAccessRestrictionText for this detail
        /// </summary>
        public string NamedAccessRestrictionText
        {
            get { return namedAccessRestrictionText; }
            set { namedAccessRestrictionText = value; }
        }

        /// <summary>
        /// Read/Write. The Cycle Routes this detail applies to
        /// </summary>
        public Dictionary<string, string> CycleRoutes
        {
            get { return cycleRoutes; }
            set { CycleRoutes = value; }
        }

        /// <summary>
        /// Read/Write. Flag indicating this section is on a Cycle route
        /// </summary>
        public bool CycleRoute
        {
            get { return cycleRoute; }
            set { cycleRoute = value; }
        }

        /// <summary>
        /// Read/Write. Array of uint representing cycle attributes (Joining) applying to this section
        /// </summary>
        public uint[] JoiningSignificantLinkAttributes
        {
            get { return joiningSignificantLinkAttributes; }
            set { joiningSignificantLinkAttributes = value; }
        }

        /// <summary>
        /// Read/Write. Array of uint representing cycle attributes (Leaving) applying to this section
        /// </summary>
        public uint[] LeavingSignificantLinkAttributes
        {
            get { return leavingSignificantLinkAttributes; }
            set { leavingSignificantLinkAttributes = value; }
        }

        /// <summary>
        /// Read/Write. Array of uint representing cycle attributes (Interesting) applying to this section
        /// </summary>
        public uint[] InterestingLinkAttributes
        {
            get { return interestingLinkAttributes; }
            set { interestingLinkAttributes = value; }
        }

        /// <summary>
        /// Read/Write. Array of uint representing cycle attributes (Node) applying to this section
        /// </summary>
        public uint[] SignificantNodeAttributes
        {
            get { return significantNodeAttributes; }
            set { significantNodeAttributes = value; }
        }

        /// <summary>
        /// Read/Write. Array of uint representing cycle attributes (SectionFeatures) applying to this section
        /// </summary>
        public uint[] SectionFeatureAttributes
        {
            get { return sectionFeatureAttributes; }
            set { sectionFeatureAttributes = value; }
        }

        /// <summary>
        /// Read/Write. CompanyUrl for this detail
        /// </summary>
        public string CompanyUrl
        {
            get { return companyUrl; }
            set { companyUrl = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneEntry for this detail
        /// </summary>
        public string CongestionZoneEntry
        {
            get { return congestionZoneEntry; }
            set { congestionZoneEntry = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneExit for this detail
        /// </summary>
        public string CongestionZoneExit
        {
            get { return congestionZoneExit; }
            set { congestionZoneExit = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneEnd for this detail
        /// </summary>
        public string CongestionZoneEnd
        {
            get { return congestionZoneEnd; }
            set { congestionZoneEnd = value; }
        }

        /// <summary>
        /// Read/Write. CongestionEntry flag for this detail
        /// </summary>
        public bool CongestionEntry
        {
            get { return congestionEntry; }
            set { congestionEntry = value; }
        }

        /// <summary>
        /// Read/Write. CongestionExit flag for this detail
        /// </summary>
        public bool CongestionExit
        {
            get { return congestionExit; }
            set { congestionExit = value; }
        }

        /// <summary>
        /// Read/Write. CongestionEnd flag for this detail
        /// </summary>
        public bool CongestionEnd
        {
            get { return congestionEnd; }
            set { congestionEnd = value; }
        }

        /// <summary>
        /// Read/Write. FerryCheckInName for this detail
        /// </summary>
        public string FerryCheckInName
        {
            get { return ferryCheckInName; }
            set { ferryCheckInName = value; }
        }

        /// <summary>
        /// Read/Write. FerryCheckIn flag for this detail
        /// </summary>
        public bool FerryCheckIn
        {
            get { return ferryCheckIn; }
            set { ferryCheckIn = value; }
        }

        /// <summary>
        /// Read/Write. FerryCheckOut flag for this detail
        /// </summary>
        public bool FerryCheckOut
        {
            get { return ferryCheckOut; }
            set { ferryCheckOut = value; }
        }

        /// <summary>
        /// Read/Write. DisplayFerryIcon flag for this detail
        /// </summary>
        public bool DisplayFerryIcon
        {
            get { return displayFerryIcon; }
            set { displayFerryIcon = value; }
        }

        /// <summary>
        /// Read/Write. TollEntryName for this detail
        /// </summary>
        public string TollEntryName
        {
            get { return tollEntryName; }
            set { tollEntryName = value; }
        }

        /// <summary>
        /// Read/Write. TollExitName for this detail
        /// </summary>
        public string TollExitName
        {
            get { return tollExitName; }
            set { tollExitName = value; }
        }

        /// <summary>
        /// Read/Write. TollEntry flag for this detail
        /// </summary>
        public bool TollEntry
        {
            get { return tollEntry; }
            set { tollEntry = value; }
        }

        /// <summary>
        /// Read/Write. TollExit flag for this detail
        /// </summary>
        public bool TollExit
        {
            get { return tollExit; }
            set { tollExit = value; }
        }

        /// <summary>
        /// Read/Write. DisplayTollIcon flag for this detail
        /// </summary>
        public bool DisplayTollIcon
        {
            get { return displayTollIcon; }
            set { displayTollIcon = value; }
        }

        /// <summary>
        /// Read/Write. ViaLocation flag for this detail
        /// </summary>
        public bool ViaLocation
        {
            get { return viaLocation; }
            set { viaLocation = value; }
        }

        /// <summary>
        /// Read/Write. WaitSection flag for this detail
        /// </summary>
        public bool WaitSection
        {
            get { return waitSection; }
            set { waitSection = value; }
        }

        /// <summary>
        /// Read/Write. UndefinedWaitSection flag for this detail
        /// </summary>
        public bool UndefinedWaitSection
        {
            get { return undefinedWaitSection; }
            set { undefinedWaitSection = value; }
        }

        /// <summary>
        /// Read/Write. ComplexManoeuvre flag for this detail
        /// </summary>
        public bool ComplexManoeuvre
        {
            get { return complexManoeuvre; }
            set { complexManoeuvre = value; }
        }

        /// <summary>
        /// Read/Write. NamedAccessRestriction flag for this detail
        /// </summary>
        public bool NamedAccessRestriction
        {
            get { return namedAccessRestriction; }
            set { namedAccessRestriction = value; }
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Takes a string of coordinates, converts them into an array of OSGRs, and 
        /// then adds it to this objects geometry dictionary at the specified index.
        /// A second dictionary is also populated with the flag of whether the coordinate should be 
        /// interpolated by the gradient profiler
        /// </summary>
        private void PopulateGeometry(int index, string geometryvalue, bool interpolateGradient)
        {
            // Get all the coordinate pairs from the geometry
            string[] coordinates = geometryvalue.Split(coordinateSeperator);

            List<OSGridReference> osgrs = new List<OSGridReference>();

            // Convert each coordinate pair into an OSGRGridReference
            foreach (string coordinate in coordinates)
            {
                // Make sure there is a coordinate before converting
                string coordinateTrimmed = coordinate.Trim();

                if (!string.IsNullOrEmpty(coordinateTrimmed))
                {
                    string[] eastingNorthing = coordinateTrimmed.Split(eastingNorthingSeperator);

                    // Assume coordinates returned are Points (i.e. with decimals)
                    double easting = Math.Round(Convert.ToDouble(eastingNorthing[0]), 0);
                    double northing = Math.Round(Convert.ToDouble(eastingNorthing[1]), 0);

                    // Create the OSGR
                    OSGridReference osgr = new OSGridReference(
                        Convert.ToInt32(easting),
                        Convert.ToInt32(northing));

                    // Add the OSGR to the array
                    osgrs.Add(osgr);

                    // Set up the OSGR for the start of this detail
                    if (startOSGR == null)
                    {
                        startOSGR = osgr;
                    }
                }
            }

            // Finally, add all the OSGRs for this link to the geometry list
            if (this.geometry == null)
            {
                this.geometry = new Dictionary<int, OSGridReference[]>();
            }

            if (this.interpolateGradient == null)
            {
                this.interpolateGradient = new Dictionary<int, bool>();
            }

            this.geometry.Add(index, osgrs.ToArray());
            this.interpolateGradient.Add(index, interpolateGradient);
        }

        #endregion
    }
}
