// *********************************************** 
// NAME             : RoadJourneyDetail.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 25 Mar 2011
// DESCRIPTION  	: RoadJourneyDetail class containing details specific for a road journey leg
// ************************************************
// 

using System;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;
using Logger = System.Diagnostics.Trace;
using System.Collections.Generic;
using SJP.Common.LocationService;
using SJP.Common;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// RoadJourneyDetail class containing details specific for a road journey leg
    /// </summary>
    [Serializable()]
    public class RoadJourneyDetail : JourneyDetail
    {
        #region Private members

        private string roadName;
        private string roadNumber;
        private int turnCount;
        private ICJP.TurnDirection turnDirection;
        private ICJP.TurnAngle turnAngle;
        private bool roundabout;
        private bool throughRoute;
        private bool congestionLevel = false;
        private string[] toid;
        private bool stopOver;
        private bool junctionSection;
        private string junctionNumber = string.Empty;
        private ICJP.JunctionType junctionType;
        private string placeName = string.Empty;
        private bool ferry;
        private bool slipRoad;
        private int tollCost;
        private int congestionZoneCost;
        private int ferryCost;
        private string tollEntry;
        private string congestionZoneEntry;
        private string companyUrl;
        private string congestionZoneExit;
        private string congestionZoneEnd;
        private string ferryCheckIn;
        private bool ferryCheckOut = false;
        private int carCost;
        private bool viaLocation;
        private bool wait;
        private bool congestionExit = false;
        private bool congestionEntry = false;
        private bool congestionEnd = false;
        private bool undefindedWait;
        private string nodeToid;
        private bool displayTollIcon = false;
        private bool displayFerryIcon = false;
        private bool ferryEntry = false;
        private string tollExit;
        private bool roadSplits;
        private bool limitedAccessRestriction = false;
        private string restriction;


        private bool junctionDriveSectionWithoutJunctionNo;

        private List<string> displayNotes = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RoadJourneyDetail()
            : base()
        {
        }

        /// <summary>
        /// Takes a CJP Section and creates a new instance of this 
        /// class populated with the section information
        /// </summary>
        public RoadJourneyDetail(ICJP.Section cjpSection, SJPVenueCarPark carPark, Language language)
            : base()
        {
            // Set values needed before doing the work
            string nullTollLink = Properties.Current[Keys.NullTollLink];
            int congestionlevel = Properties.Current[Keys.CongestionLevel].Parse(30);
            

            durationSecs = Convert.ToInt32(cjpSection.time.Ticks / TimeSpan.TicksPerSecond);
            

            // Is this a stop over section, drive section or a junction drive section?
            Type sectionType = cjpSection.GetType();

            if (sectionType == typeof(ICJP.StopoverSection))
            {
                #region StopoverSection

                // Stop Over section information
                ICJP.StopoverSection thisSection = cjpSection as ICJP.StopoverSection;

                roadName = (thisSection.name != null ? thisSection.name : string.Empty);

                if (thisSection.node == null || thisSection.node.TOID == null)
                {
                    nodeToid = string.Empty;

                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error,
                        string.Format(Messages.ROAD_NoNodeToid, thisSection.name, thisSection.type)));
                }
                else
                {
                    nodeToid = thisSection.node.TOID;
                }

                stopOver = true;
                junctionSection = false;
                ferry = false;
                slipRoad = false;

                tollCost = thisSection.toll;

                // Need to establish what type of Stopover it is

                #region CongestionZoneEntry

                //CongestionZoneEntry
                if (thisSection.type == ICJP.StopoverSectionType.CongestionZoneEntry)
                {
                    congestionZoneCost = thisSection.toll;
                    congestionZoneEntry = thisSection.name;
                    congestionEntry = true;

                    if (thisSection.sectionFeatures.Length != 0)
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (ICJP.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            ICJP.TextSectionFeature tsec = (ICJP.TextSectionFeature)sec;
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

                //CongestionZoneExit
                if (thisSection.type == ICJP.StopoverSectionType.CongestionZoneExit)
                {

                    congestionExit = true;

                    congestionZoneExit = thisSection.name;
                    congestionZoneCost = thisSection.toll;

                    if (thisSection.sectionFeatures.Length != 0)
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (ICJP.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            ICJP.TextSectionFeature tsec = (ICJP.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    //congestionZoneExit = tsec.value;
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

                //CongestionZoneEnd
                if (thisSection.type == ICJP.StopoverSectionType.CongestionZoneEnd)
                {

                    CongestionEnd = true;

                    congestionZoneEnd = thisSection.name;
                    congestionZoneCost = thisSection.toll;

                    if (thisSection.sectionFeatures.Length != 0)
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (ICJP.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            ICJP.TextSectionFeature tsec = (ICJP.TextSectionFeature)sec;
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

                //FerryCheckIn
                if (thisSection.type == ICJP.StopoverSectionType.FerryCheckIn)
                {
                    ferryCost = thisSection.toll;
                    ferryCheckIn = thisSection.name;
                    displayFerryIcon = true;
                    ferryEntry = true;

                    if (thisSection.sectionFeatures.Length != 0)
                    {
                        string ferryRouteUrl = string.Empty;
                        //need to extract company name and URL from stopoversection section features	
                        foreach (ICJP.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            ICJP.TextSectionFeature tsec = (ICJP.TextSectionFeature)sec;
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

                //FerryCheckOut
                if (thisSection.type == ICJP.StopoverSectionType.FerryCheckOut)
                {
                    //Cost not displayed on exiting a ferry
                    ferryCheckOut = true;
                    displayFerryIcon = true;

                }

                #endregion

                #region TollEntry

                //TollEntry
                if (thisSection.type == ICJP.StopoverSectionType.TollEntry)
                {
                    tollCost = thisSection.toll;
                    tollEntry = thisSection.name;
                    displayTollIcon = true;

                    if (thisSection.sectionFeatures.Length != 0)
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (ICJP.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            ICJP.TextSectionFeature tsec = (ICJP.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:		//Extract url
                                    if (tsec.value == nullTollLink)
                                    {
                                        companyUrl = string.Empty;
                                    }
                                    else
                                    {
                                        companyUrl = tsec.value;
                                    }
                                    break;
                            }
                        }
                    }
                }

                #endregion

                #region TollExit

                //TollExit
                if (thisSection.type == ICJP.StopoverSectionType.TollExit)
                {
                    tollCost = thisSection.toll;
                    tollExit = thisSection.name;
                    displayTollIcon = true;

                    if (thisSection.sectionFeatures.Length != 0)
                    {
                        //need to extract company name and URL from stopoversection section features	
                        foreach (ICJP.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            ICJP.TextSectionFeature tsec = (ICJP.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 0:		//Extract company name
                                    break;
                                case 13:		//Extract url
                                    if (tsec.value == nullTollLink)
                                    {
                                        companyUrl = string.Empty;
                                    }
                                    else
                                    {
                                        companyUrl = tsec.value;
                                    }
                                    break;
                            }

                        }
                    }
                }

                #endregion

                #region Via

                //If it is a Via Section
                if (thisSection.type == ICJP.StopoverSectionType.Via)
                {

                    roadName = "VIALOCAION";
                    viaLocation = true;

                }

                #endregion

                #region Wait

                if (thisSection.type == ICJP.StopoverSectionType.Wait)
                {
                    //If it is a wait type, we need to add the time to the journey duration
                    //but we don't want to display any journey directions for the wait.
                    roadName = "WAIT";
                    wait = true;

                }

                #endregion

                #region UndefinedWait

                if (thisSection.type == ICJP.StopoverSectionType.UndefinedWait)
                {
                    undefindedWait = true;
                }

                #endregion

                if (thisSection.type == ICJP.StopoverSectionType.LimitedAccessRestriction)
                {
                    limitedAccessRestriction = true;

                    if (thisSection.sectionFeatures != null 
                        && thisSection.sectionFeatures.Length > 0)
                    {
                        // Extract the limited access restriction text
                        foreach (ICJP.SectionFeature sec in thisSection.sectionFeatures)
                        {
                            ICJP.TextSectionFeature tsec = (ICJP.TextSectionFeature)sec;
                            switch (tsec.id)
                            {
                                case 15:
                                    restriction = tsec.value;
                                    break;
                                default: break;
                            }
                        }
                    }
                }

                #endregion
            }

            else if (sectionType == typeof(ICJP.DriveSection))
            {
                #region DriveSection
                
                // Drive section
                ICJP.DriveSection thisSection = cjpSection as ICJP.DriveSection;

                roadName = (thisSection.name != null ? thisSection.name : string.Empty);
                roadNumber = (thisSection.number != null ? thisSection.number : string.Empty);

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

                slipRoad = CheckSlipRoad(roadName);

                turnCount = thisSection.turnCount;
                turnDirection = thisSection.turnDirection;
                turnAngle = thisSection.turnAngle;
                roundabout = thisSection.roundabout;
                throughRoute = thisSection.throughRoute;

                //flag for ambiguous junction
                roadSplits = thisSection.roadSplits;

                //Need to loop through every link to check for congestion levels
                int i;
                toid = new string[thisSection.links.Length];
                for (i = 0; i < thisSection.links.Length; i++)
                {
                    toid[i] = thisSection.links[i].TOID;

                    if (thisSection.links[i].congestion > congestionlevel)
                    {
                        congestionLevel = true;
                    }
                }

                placeName = thisSection.heading;
                carCost = thisSection.cost;

                junctionSection = false;
                stopOver = false;

                #endregion
            }

            else if (sectionType == typeof(ICJP.JunctionDriveSection))
            {
                #region JunctionDriveSection
                
                // Junction Drive Section
                ICJP.JunctionDriveSection thisSection = cjpSection as ICJP.JunctionDriveSection;

                roadName = (thisSection.name != null ? thisSection.name : string.Empty);
                roadNumber = (thisSection.number != null ? thisSection.number : string.Empty);

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
                roundabout = thisSection.roundabout;
                throughRoute = thisSection.throughRoute;

                //flag for ambiguous junction
                roadSplits = thisSection.roadSplits;

                //Need to loop through every link to check for congestion levels
                int i;
                toid = new string[thisSection.links.Length];
                
                for (i = 0; i < thisSection.links.Length; i++)
                {
                    toid[i] = thisSection.links[i].TOID;

                    if (thisSection.links[i].congestion > congestionlevel)
                    {
                        congestionLevel = true;
                    }
                }
                
                placeName = thisSection.heading;
                carCost = thisSection.cost;

                // Although dealing with JunctionDriveSection, if no junction number 
                // present then need to override and treat as normal DriveSection.
                junctionNumber = thisSection.junctionNumber;
                junctionSection = ((junctionNumber != null)
                    && (junctionNumber.Length != 0));

                junctionType = thisSection.type;

                junctionDriveSectionWithoutJunctionNo = (junctionNumber == null) || (junctionNumber.Length == 0);

                stopOver = false;

                #endregion
            }

            #region Display notes

            displayNotes = PopulateDisplayNotes(carPark, language);

            #endregion
        }

        #endregion

        #region Public Static methods

        /// <summary>
        /// Is the CJP Section considered to be for a Ferry. 
        /// Returns true if the road number contains the Ferry identifier
        /// </summary>
        /// <param name="cjpSection"></param>
        /// <returns></returns>
        public static bool IsFerrySection(ICJP.Section cjpSection)
        {
            ICJP.JunctionDriveSection jds = cjpSection as ICJP.JunctionDriveSection;
            ICJP.DriveSection ds = cjpSection as ICJP.DriveSection;

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
        /// Read/Write. RoadName
        /// </summary>
        public string RoadName
        {
            get { return roadName; }
            set { roadName = value; }
        }

        /// <summary>
        /// Read/Write. RoadNumber
        /// </summary>
        public string RoadNumber
        {
            get { return roadNumber; }
            set { roadNumber = value; }
        }

        /* Defined in base class 
        /// <summary>
        /// Read/Write. Distance (metres)
        /// </summary>
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// Read/Write. Duration (seconds)
        /// </summary>
        public long Duration 
        {
            get { return durationSecs; }
            set { durationSecs = value; }
        }*/

        /// <summary>
        /// Read/Write. TurnCount
        /// </summary>
        public int TurnCount
        {
            get { return turnCount; }
            set { turnCount = value; }
        }

        /// <summary>
        /// Read/Write. Direction
        /// </summary>
        public ICJP.TurnDirection Direction
        {
            get { return turnDirection; }
            set { turnDirection = value; }
        }

        /// <summary>
        /// Read/Write. Angle
        /// </summary>
        public ICJP.TurnAngle Angle
        {
            get { return turnAngle; }
            set { turnAngle = value; }
        }

        /// <summary>
        /// Read/Write. Roundabout
        /// </summary>
        public bool Roundabout
        {
            get { return roundabout; }
            set { roundabout = value; }
        }

        /// <summary>
        /// Read/Write. ThroughRoute
        /// </summary>
        public bool ThroughRoute
        {
            get { return throughRoute; }
            set { throughRoute = value; }
        }

        /// <summary>
        /// Read/Write. CongestionLevel
        /// </summary>
        public bool CongestionLevel
        {
            get { return congestionLevel; }
            set { congestionLevel = value; }
        }

        /// <summary>
        /// Read/Write. Toid
        /// </summary>
        public string[] Toid
        {
            get { return toid; }
            set { toid = value; }
        }

        /// <summary>
        /// Read/Write. IsStopOver
        /// </summary>
        public bool IsStopOver
        {
            get { return stopOver; }
            set { stopOver = value; }
        }

        /// <summary>
        /// Read/Write. IsFerry
        /// </summary>
        public bool IsFerry
        {
            get { return ferry; }
            set { ferry = value; }
        }

        /// <summary>
        /// Read/Write. IsSlipRoad
        /// </summary>
        public bool IsSlipRoad
        {
            get { return slipRoad; }
            set { slipRoad = value; }
        }

        /// <summary>
        /// Read/Write. IsJunctionSection
        /// </summary>
        public bool IsJunctionSection
        {
            get { return junctionSection; }
            set { junctionSection = value; }
        }

        /// <summary>
        /// Read/Write. JunctionNumber
        /// </summary>
        public string JunctionNumber
        {
            get { return junctionNumber; }
            set { junctionNumber = value; }
        }

        /// <summary>
        /// Read/Write. JunctionAction
        /// </summary>
        public ICJP.JunctionType JunctionAction
        {
            get { return junctionType; }
            set { junctionType = value; }
        }

        /// <summary>
        /// Read/Write. PlaceName
        /// </summary>
        public string PlaceName
        {
            get { return placeName; }
            set { placeName = value; }
        }

        /// <summary>
        /// Read/Write. TollCost
        /// </summary>
        public int TollCost
        {
            get { return tollCost; }
            set { tollCost = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneCost
        /// </summary>
        public int CongestionZoneCost
        {
            get { return congestionZoneCost; }
            set { congestionZoneCost = value; }
        }

        /// <summary>
        /// Read/Write. FerryCost
        /// </summary>
        public int FerryCost
        {
            get { return ferryCost; }
            set { ferryCost = value; }
        }

        /// <summary>
        /// Read/Write. TollEntry
        /// </summary>
        public string TollEntry
        {
            get { return tollEntry; }
            set { tollEntry = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneEntry
        /// </summary>
        public string CongestionZoneEntry
        {
            get { return congestionZoneEntry; }
            set { congestionZoneEntry = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneExit
        /// </summary>
        public string CongestionZoneExit
        {
            get { return congestionZoneExit; }
            set { congestionZoneExit = value; }
        }

        /// <summary>
        /// Read/Write. CongestionZoneEnd
        /// </summary>
        public string CongestionZoneEnd
        {
            get { return congestionZoneEnd; }
            set { congestionZoneEnd = value; }
        }

        /// <summary>
        /// Read/Write. FerryCheckIn
        /// </summary>
        public string FerryCheckIn
        {
            get { return ferryCheckIn; }
            set { ferryCheckIn = value; }
        }

        /// <summary>
        /// Read/Write. FerryCheckOut
        /// </summary>
        public bool FerryCheckOut
        {
            get { return ferryCheckOut; }
            set { ferryCheckOut = value; }
        }

        /// <summary>
        /// Read/Write. CompanyUrl
        /// </summary>
        public string CompanyUrl
        {
            get { return companyUrl; }
            set { companyUrl = value; }
        }

        /// <summary>
        /// Read/Write. CarCost
        /// </summary>
        public int CarCost
        {
            get { return carCost; }
            set { carCost = value; }
        }

        /// <summary>
        /// Read/Write. ViaLocation
        /// </summary>
        public bool ViaLocation
        {
            get { return viaLocation; }
            set { viaLocation = value; }
        }

        /// <summary>
        /// Read/Write. Wait
        /// </summary>
        public bool Wait
        {
            get { return wait; }
            set { wait = value; }
        }

        /// <summary>
        /// Read/Write. CongestionExit
        /// </summary>
        public bool CongestionExit
        {
            get { return congestionExit; }
            set { congestionExit = value; }
        }

        /// <summary>
        /// Read/Write. CongestionEntry
        /// </summary>
        public bool CongestionEntry
        {
            get { return congestionEntry; }
            set { congestionEntry = value; }
        }

        /// <summary>
        /// Read/Write. CongestionEnd
        /// </summary>
        public bool CongestionEnd
        {
            get { return congestionEnd; }
            set { congestionEnd = value; }
        }

        /// <summary>
        /// Read/Write. UndefindedWait
        /// </summary>
        public bool UndefindedWait
        {
            get { return undefindedWait; }
            set { undefindedWait = value; }
        }

        /// <summary>
        /// Read/Write. NodeToid
        /// </summary>
        public string NodeToid
        {
            get { return nodeToid; }
            set { nodeToid = value; }
        }

        /// <summary>
        /// Read/Write. DisplayTollIcon
        /// </summary>
        public bool DisplayTollIcon
        {
            get { return displayTollIcon; }
            set { displayTollIcon = value; }
        }

        /// <summary>
        /// Read/Write. DisplayFerryIcon
        /// </summary>
        public bool DisplayFerryIcon
        {
            get { return displayFerryIcon; }
            set { displayFerryIcon = value; }
        }

        /// <summary>
        /// Read/Write. TollExit 
        /// </summary>
        public string TollExit
        {
            get { return tollExit; }
            set { tollExit = value; }
        }

        /// <summary>
        /// Read/Write. FerryEntry
        /// </summary>
        public bool FerryEntry
        {
            get { return ferryEntry; }
            set { ferryEntry = value; }
        }

        /// <summary>
        /// Read/Write. Flag for ambiguous junction.		
        /// </summary>
        public bool RoadSplits
        {
            get { return roadSplits; }
            set { roadSplits = value; }
        }

        /// <summary>
        /// Read/Write. Flag for junction section without junction no.		
        /// </summary>
        public bool JunctionDriveSectionWithoutJunctionNo
        {
            get { return junctionDriveSectionWithoutJunctionNo; }
            set { junctionDriveSectionWithoutJunctionNo = value; }
        }

        /// <summary>
        /// Read/Write. LimitedAccessRestriction 
        /// </summary>
        public bool LimitedAccessRestriction 
        {
            get { return limitedAccessRestriction; }
            set { limitedAccessRestriction = value; }
        }

        /// <summary>
        /// Read/Write. Restriction text
        /// </summary>
        public string Restriction
        {
            get { return restriction; }
            set { restriction = value; }
        }

        /// <summary>
        /// Read/Write. Display notes
        /// </summary>
        public List<string> DisplayNotes
        {
            get { return displayNotes; }
            set { displayNotes = value; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Function parses the road number string to see if it 
        /// contains the "slip road" string. Returns bool value 
        /// accordingly.
        /// </summary>
        /// <param name="roadNumber">string to be parsed</param>
        /// <returns>true if road number contains "slip road"</returns>
        private bool CheckSlipRoad(string roadName)
        {
            // Convert to upper case for consistency
            string s = roadName.ToUpper();
            // Check if string starts with "Slip Road". Need to avoid situations like "Ruislip Road"
            if (s.StartsWith(Messages.ROAD_NAME_SLIP_ROAD))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns a list of string display notes for an array of CJP Note
        /// </summary>
        /// <returns></returns>
        private List<string> PopulateDisplayNotes(SJPVenueCarPark carPark, Language language)
        {
            List<string> displayNotes = new List<string>();

            // Leg display notes
            if (carPark != null && carPark.InformationText.Count > 0)
            {
                foreach (KeyValuePair<Language,string> kvp in carPark.InformationText)
                {
                    if (language == kvp.Key)
                    {
                        displayNotes.Add(kvp.Value);
                    }
                }
            }

            return displayNotes;
        }

        #endregion
    }
}
