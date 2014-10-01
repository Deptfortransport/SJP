// *********************************************** 
// NAME             : LegControlAdapter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 13 Feb 2012
// DESCRIPTION  	: Helper class for the DetailsLegControl to apply logic to the UI controls 
// when building the leg to display a Journey Leg
// ************************************************

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common.DataServices.StopAccessibilityLinks;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;

namespace SJP.Common.Web
{
    /// <summary>
    /// Helper class for the DetailsLegControl
    /// </summary>
    public class LegControlAdapter
    {
        #region Private members

        #region Constructor values

        private ISJPJourneyRequest journeyRequest; 

        private JourneyLeg previousJourneyLeg; 
        private JourneyLeg currentJourneyLeg; 
        private JourneyLeg nextJourneyLeg; 
                
        private bool showAccessibleFeatures; 
        private bool showAccessibleInfo; 
        
        private bool isPrinterFriendly; 
        private bool isAccessibleFriendly; 
        private bool isReturn;
        private bool isMobile;

        #endregion

        #region Resources

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private SJPResourceManager RM = null;
        private string Page_ImagePath = string.Empty;

        // Resource strings
        private string URL_TravelNotesLink = string.Empty;
        private string ALTTXT_TravelNotesLink = string.Empty;
        private string URL_OpenInNewWindowImage = string.Empty;
        private string URL_OpenInNewWindowImage_Blue = string.Empty;
        private string ALTTXT_OpenInNewWindow = string.Empty;
        private string TOOLTIP_OpenInNewWindow = string.Empty;

        private string TXT_TravelNotesLink = string.Empty;
        private string TXT_AccessibleLink = string.Empty;
        private string TXT_AccessibleInfo = string.Empty;
        private string TXT_VenueMapLink = string.Empty;
        private string TXT_VenueMapLinkToolTip = string.Empty;

        #endregion

        #region Image

        #region Web/Mobile constants

        // Node/Line image dimensions - these should be set to actual image dimensions
        private const int IMG_Web_Node_Width = 20;
        private const int IMG_Web_Node_Height = 40;
        private const int IMG_Web_Node_Medium_Height = 58;
        private const int IMG_Web_Node_Start_Medium_Height = 56;
        private const int IMG_Web_Node_End_Medium_Height = 56;
        private const int IMG_Web_Node_Long_Height = 68;
        private const int IMG_Web_Node_ExtraLong_Height = 78;
        private const int IMG_Web_Line_Width = 20;
        private const int IMG_Web_Line_Height = 51;

        private const int IMG_Mobile_Node_Width = 20;
        private const int IMG_Mobile_Node_Height = 54;
        private const int IMG_Mobile_Node_Medium_Height = 68;
        private const int IMG_Mobile_Node_Start_Medium_Height = 66;
        private const int IMG_Mobile_Node_End_Medium_Height = 66;
        private const int IMG_Mobile_Node_Long_Height = 78;
        private const int IMG_Mobile_Node_ExtraLong_Height = 88;
        private const int IMG_Mobile_Line_Width = 20;
        private const int IMG_Mobile_Line_Height = 60;

        #endregion

        private int IMG_Node_Width;
        private int IMG_Node_Height;
        private int IMG_Node_Medium_Height;
        private int IMG_Node_Start_Medium_Height;
        private int IMG_Node_End_Medium_Height;
        private int IMG_Node_Long_Height;
        private int IMG_Node_ExtraLong_Height;
        private int IMG_Line_Width;
        private int IMG_Line_Height;

        // Enum for setting image node size
        private enum NodeImageSize
        {
            Default,
            DefaultStart,
            DefaultEnd,
            Medium,
            MediumStart,
            MediumEnd,
            Long,
            ExtraLong,
        }

        #endregion
        
        private LegInstructionAdapter legInstructionAdapter = null;

        private SJPModeType currentMode = SJPModeType.Unknown;

        private bool firstLeg = false;
        private bool lastLeg = false;

        private bool fromVenue = false;
        private bool toVenue = false;

        private bool isPJFrequencyLeg = false;
        private bool isPJContinuousLeg = false;
        private bool isPJInterchangeLeg = false;
        private bool isPJTimedLeg = false;
        private bool isCarLeg = false;
        private bool isCycleLeg = false;
        private bool isPreviousPJTimedLeg = false;
        private bool isPreviousCarLeg = false;
        private bool isPreviousCycleLeg = false;

        private bool showAccessibleAssistanceFeatures = true;
        private bool showInterchange = false;
        private bool showWalkInterchange = false;
        private bool showCheckConstraint = false;
        private bool showQueueMode = false;
        private bool showServiceNumber = false;

        // Used in updating image dimensions
        private List<string> notes = null;
        private bool accessibleLinkAvailable = false;
        private bool accessibleLinkEndAvailable = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LegControlAdapter(ISJPJourneyRequest journeyRequest, JourneyLeg previousJourneyLeg, JourneyLeg currentJourneyLeg, JourneyLeg nextJourneyLeg,
            bool showAccessibleFeatures, bool showAccessibleInfo,
            bool isPrinterFriendly, bool isAccessibleFriendly, bool isReturn, bool isMobile,
            SJPResourceManager resourceManager, string imagePath)
        {
            this.journeyRequest = journeyRequest;
            this.previousJourneyLeg = previousJourneyLeg;
            this.currentJourneyLeg = currentJourneyLeg;
            this.nextJourneyLeg = nextJourneyLeg;
            this.showAccessibleFeatures = showAccessibleFeatures;
            this.showAccessibleInfo = showAccessibleInfo;
            this.isPrinterFriendly = isPrinterFriendly;
            this.isAccessibleFriendly = isAccessibleFriendly;
            this.isReturn = isReturn;
            this.RM = resourceManager;
            this.Page_ImagePath = imagePath;
            this.isMobile = isMobile;

            InitialiseFlags();
            InitialiseText();
            InitialiseImageDimensions();

            legInstructionAdapter = new LegInstructionAdapter(currentJourneyLeg, previousJourneyLeg, nextJourneyLeg, 
                isPrinterFriendly, resourceManager, isMobile);
        }
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Read only
        /// </summary>
        public SJPModeType CurrentMode
        {
            get { return currentMode; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool FirstLeg
        {
            get { return firstLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool LastLeg
        {
            get { return lastLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPJFrequencyLeg
        {
            get { return isPJFrequencyLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPJContinuousLeg
        {
            get { return isPJContinuousLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPJInterchangeLeg
        {
            get { return isPJInterchangeLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPJTimedLeg
        {
            get { return isPJTimedLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsCarLeg
        {
            get { return isCarLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsCycleLeg
        {
            get { return isCycleLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPreviousPJTimedLeg
        {
            get { return isPreviousPJTimedLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPreviousCarLeg
        {
            get { return isPreviousCarLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPreviousCycleLeg
        {
            get { return isPreviousCycleLeg; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ShowAccessibleFeatures
        {
            get { return showAccessibleFeatures; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ShowAccessibleInfo
        {
            get { return showAccessibleInfo; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ShowAccessibleAssistanceFeatures
        {
            get { return showAccessibleAssistanceFeatures; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ShowInterchange
        {
            get { return showInterchange; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ShowWalkInterchange
        {
            get { return showWalkInterchange; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ShowCheckConstraint
        {
            get { return showCheckConstraint; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool ShowServiceNumber
        {
            get { return showServiceNumber; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsPrinterFriendly
        {
            get { return isPrinterFriendly; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsAccessibleFriendly
        {
            get { return isAccessibleFriendly; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsReturn
        {
            get { return isReturn; }
        }

        /// <summary>
        /// Read only
        /// </summary>
        public List<string> DisplayNotes
        {
            get { return notes; }
        }

        /// <summary>
        /// Returns true if the detail car leg should be visible
        /// </summary>
        public bool ShowCarLeg
        {
            get
            {
                bool show = false;

                // Show if its a car leg
                if (currentJourneyLeg != null)
                {
                    if (currentJourneyLeg.Mode == SJPModeType.Car)
                    {
                        // And it contains RoadJourneyDetails
                        if ((currentJourneyLeg.JourneyDetails != null) && (currentJourneyLeg.JourneyDetails.Count > 0))
                        {
                            if (currentJourneyLeg.JourneyDetails[0] is RoadJourneyDetail)
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
        public bool ShowCycleLeg
        {
            get
            {
                bool show = false;

                // Show if its a cycle leg
                if (currentJourneyLeg != null)
                {
                    if (currentJourneyLeg.Mode == SJPModeType.Cycle)
                    {
                        // And it contains CycleJourneyDetails
                        if ((currentJourneyLeg.JourneyDetails != null) && (currentJourneyLeg.JourneyDetails.Count > 0))
                        {
                            if (currentJourneyLeg.JourneyDetails[0] is CycleJourneyDetail)
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
        /// Returns the PublicJourneyDetail if exists in the current journey leg
        /// </summary>
        public PublicJourneyDetail GetPublicJourneyDetail()
        {
            PublicJourneyDetail pjd = null;

                if (currentJourneyLeg.JourneyDetails.Count > 0)
                {
                    // If journey leg is for a public journey, then there will only be one public journey detail,
                    // so check and use only the first for vehicle features
                    if (currentJourneyLeg.JourneyDetails[0] is PublicJourneyDetail)
                    {
                        pjd = (PublicJourneyDetail)currentJourneyLeg.JourneyDetails[0];
                    }
                }

                return pjd;
        }

        /// <summary>
        /// Returns the first RoadJourneyDetail if exists in the current journey leg
        /// </summary>
        public RoadJourneyDetail GetRoadJourneyDetail()
        {
            RoadJourneyDetail rjd = null;

            if (currentJourneyLeg.JourneyDetails.Count > 0)
            {
                // If journey leg is for a road journey, then there may be multiple road journey details
                if (currentJourneyLeg.JourneyDetails[0] is RoadJourneyDetail)
                {
                    rjd = (RoadJourneyDetail)currentJourneyLeg.JourneyDetails[0];
                }
            }

            return rjd;
        }

        /// <summary>
        /// Returns the journey leg NaPTAN
        /// </summary>
        /// <param name="legStart">True is for leg start, or False for leg end</param>
        /// <returns>Naptan (upper) or string.empty </returns>
        public string GetJourneyLegNaPTAN(bool legStart)
        {
            string naptan = string.Empty;

            if (legStart)
            {
                if ((currentJourneyLeg.LegStart.Location.Naptan != null)
                    && (currentJourneyLeg.LegStart.Location.Naptan.Count > 0))
                {
                    naptan = currentJourneyLeg.LegStart.Location.Naptan[0].ToUpper();
                }
            }
            else
            {
                if ((currentJourneyLeg.LegEnd.Location.Naptan != null)
                    && (currentJourneyLeg.LegEnd.Location.Naptan.Count > 0))
                {
                    naptan = currentJourneyLeg.LegEnd.Location.Naptan[0].ToUpper();
                }
            }

            return naptan;
        }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Sets up the leg instruction, with location and times
        /// </summary>
        public void SetupLegInstruction(Label locationLabel, Label instructionLabel, Label instructionTimeLabel, Label modeDetailLabel,
            Label arriveLabel, Label arriveTimeLabel, Label departLabel, Label departTimeLabel)
        {
            // Set location and instruction text values
            locationLabel.Text = legInstructionAdapter.GetLegStartLocation();
            instructionLabel.Text = legInstructionAdapter.GetLegDetail();
            modeDetailLabel.Text = legInstructionAdapter.GetLegModeDetail(firstLeg, lastLeg, isPJFrequencyLeg, showAccessibleFeatures);

            // Set instruction time value where needed (for frequency legs only)
            if (isPJFrequencyLeg)
                instructionTimeLabel.Text = legInstructionAdapter.GetLegTimeDetail(isPJFrequencyLeg);
            if (string.IsNullOrEmpty(instructionTimeLabel.Text))
                instructionTimeLabel.Visible = false;

            // Set depart/arrive times (arrive time is for previous leg)
            arriveLabel.Text = (!showInterchange) ? legInstructionAdapter.GetArriveLabelText(firstLeg, false, isPreviousPJTimedLeg, isPreviousCarLeg, isPreviousCycleLeg, false) : string.Empty;
            arriveTimeLabel.Text = (!showInterchange) ? legInstructionAdapter.GetPreviousArriveTime(firstLeg, isPreviousPJTimedLeg, (isPreviousCarLeg || isPreviousCycleLeg)) : string.Empty;
            departLabel.Text = legInstructionAdapter.GetDepartLabelText(firstLeg, isPJTimedLeg, isPJFrequencyLeg, isPJContinuousLeg, isCarLeg, isCycleLeg);
            departTimeLabel.Text = legInstructionAdapter.GetDepartTime(firstLeg, lastLeg, isPJTimedLeg, (isCarLeg || isCycleLeg));
        }

        /// <summary>
        /// Sets up the leg mode image based on journey leg mode
        /// </summary>
        public void SetupLegMode(Image modeImage, SJPModeType mode)
        {
            Language language = CurrentLanguage.Value;

            string modeImageKey = string.Format("TransportMode.{0}.ImageUrl", mode);
            string modeAltTextKey = string.Format("TransportMode.{0}", mode);
            string modeToolTipKey = string.Format("TransportMode.{0}", mode);

            string imageResource = RM.GetString(language, modeImageKey);

            // Only display the mode image if a resource string has been defined, e.g. for Transfer mode no image should be shown
            if (!string.IsNullOrEmpty(imageResource))
            {
                modeImage.ImageUrl = Page_ImagePath + RM.GetString(language, modeImageKey);
                modeImage.AlternateText = RM.GetString(language, modeAltTextKey);
                modeImage.ToolTip = RM.GetString(language, modeToolTipKey);
                modeImage.Visible = true;
            }
            else
            {
                modeImage.Visible = false;
            }
        }

        /// <summary>
        /// Sets up the leg location point icon and connecting line image
        /// </summary>
        public void SetupLegLineImages(Image nodeImage, Image lineImage, Label locationLabel, HyperLink accessibleLink, bool isForEndRow)
        {
            Language language = CurrentLanguage.Value;

            string imageUrl = string.Empty;
            string altText = string.Empty;

            #region Node image

            if (nodeImage != null)
            {
                #region Determine node size

                // Default size
                NodeImageSize nodeSize = NodeImageSize.Default;

                #region End leg conditions
                // Check for lastLeg and endRow in the event it is a "one leg" journey, 
                // because the leg will be true for both "firstLeg" and "lastLeg"
                if ((lastLeg) && (isForEndRow) && (accessibleLinkEndAvailable) && (!isPrinterFriendly))
                {
                    nodeSize = NodeImageSize.MediumEnd;
                }
                else if ((lastLeg) && (isForEndRow))
                {
                    nodeSize = NodeImageSize.DefaultEnd;
                }
                #endregion
                #region Start leg conditions
                // Check if first leg has accessble station link shown, if so use the medium image
                else if ((firstLeg) && (accessibleLinkAvailable) && (!isPrinterFriendly))
                {
                    nodeSize = NodeImageSize.MediumStart;
                }
                else if (firstLeg)
                {
                    nodeSize = NodeImageSize.DefaultStart;
                }
                #endregion
                #region Other leg conditions
                // Check if node location text will go over 3 lines and is for accessible friendly page,
                // if so use longer node image. The value is a safe approximation
                else if ((isAccessibleFriendly) && (!isForEndRow) && (locationLabel.Text.Length > 78))
                {
                    nodeSize = NodeImageSize.Long;
                }
                // Check if node location text will go over 3 lines, if so use longer node image. The value is
                // a safe approximation
                else if ((!isForEndRow) && (locationLabel.Text.Length > 90))
                {
                    nodeSize = NodeImageSize.Medium;
                }
                // Check if is for an accessible station link, if so use longer node image.
                else if ((!isForEndRow) && (accessibleLinkAvailable) && (!isPrinterFriendly))
                {
                    nodeSize = NodeImageSize.Medium;

                    // Check if node location text will go over 2 lines, if so use longer node image.
                    if (locationLabel.Text.Length > 40)
                    {
                        nodeSize = NodeImageSize.Long;

                        // Accessible link row may span over two rows as it contains the stop name
                        if (accessibleLink != null && accessibleLink.Text.Length > 200)
                        {
                            nodeSize = NodeImageSize.ExtraLong;
                        }
                    }
                }
                // Check if is for accessible friendly page, text font size is larger so use so use longer node image
                else if (isAccessibleFriendly)
                {
                    nodeSize = NodeImageSize.Medium;
                }
                #endregion

                #endregion

                #region Set node image based on node size to use
                // Default
                nodeImage.Width = IMG_Node_Width;
                nodeImage.AlternateText = RM.GetString(language, RG, RC, "JourneyOutput.Image.Node.AltText");

                switch (nodeSize)
                {
                    case NodeImageSize.MediumStart:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.Start.Medium.ImageURL");
                        nodeImage.Height = IMG_Node_Start_Medium_Height;
                        nodeImage.AlternateText = RM.GetString(language, RG, RC, "JourneyOutput.Image.Start.AltText");
                        break;
                    case NodeImageSize.DefaultStart:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.Start.ImageURL");
                        nodeImage.Height = IMG_Node_Height;
                        nodeImage.AlternateText = RM.GetString(language, RG, RC, "JourneyOutput.Image.Start.AltText");
                        break;
                    case NodeImageSize.MediumEnd:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.End.Medium.ImageURL");
                        nodeImage.Height = IMG_Node_End_Medium_Height;
                        nodeImage.AlternateText = RM.GetString(language, RG, RC, "JourneyOutput.Image.End.AltText");
                        break;
                    case NodeImageSize.DefaultEnd:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.End.ImageURL");
                        nodeImage.Height = IMG_Node_Height;
                        nodeImage.AlternateText = RM.GetString(language, RG, RC, "JourneyOutput.Image.End.AltText");
                        break;
                    case NodeImageSize.Medium:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.Node.Medium.ImageURL");
                        nodeImage.Height = IMG_Node_Medium_Height;
                        break;
                    case NodeImageSize.Long:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.Node.Long.ImageURL");
                        nodeImage.Height = IMG_Node_Long_Height;
                        break;
                    case NodeImageSize.ExtraLong:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.Node.ExtraLong.ImageURL");
                        nodeImage.Height = IMG_Node_ExtraLong_Height;
                        break;
                    default:
                        nodeImage.ImageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.Node.ImageURL");
                        nodeImage.Height = IMG_Node_Height;
                        break;
                }

                // Tool tip is same as alt text
                nodeImage.ToolTip = nodeImage.AlternateText;
                #endregion
            }

            #endregion

            #region Line image

            if (lineImage != null)
            {
                // All modes now use the solid line image, resource ids left here for future use if needed
                // "JourneyOutput.Image.LineDotted.ImageURL"
                // "JourneyOutput.Image.LineDotted.AltText"
                imageUrl = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Image.LineSolid.ImageURL");
                altText = RM.GetString(language, RG, RC, "JourneyOutput.Image.LineSolid.AltText");

                lineImage.ImageUrl = imageUrl;
                lineImage.AlternateText = altText;
                lineImage.ToolTip = altText;
            }

            #endregion
        }

        /// <summary>
        /// Sets up the check constraint instrucion for the journey leg
        /// </summary>
        public void SetupCheckConstraint(Image modeImage, Label modeDetailLabel, Label instructionLabel)
        {
            if (showCheckConstraint && showQueueMode)
            {
                // Currently only show check constraint interchange leg for two types, one for outward and one for return.
                // This flag will need to be updated in future if the assumptions change
                instructionLabel.Text = legInstructionAdapter.GetLegCheckConstraintDetail(isReturn);

                // Update the leg mode image to be queue (rather than walk)
                SetupLegMode(modeImage, SJPModeType.Queue);
            }
            else if (currentMode == SJPModeType.Walk)
            {
                // Otherwise, not showing queue, for walk leg and if entering or exiting venue,
                // update the text with allow time for delays, and show mode icon if needed
                if ((firstLeg && fromVenue)
                    || (lastLeg && toVenue))
                {
                    bool isDoNotUseUnderground = (journeyRequest != null 
                             && journeyRequest.AccessiblePreferences != null
                             && journeyRequest.AccessiblePreferences.DoNotUseUnderground);

                    // If accessible journey and "not underground" selected, then display standard allow delay text
                    instructionLabel.Text = legInstructionAdapter.GetLegAllowTimeDetail(isReturn,
                        firstLeg, lastLeg, fromVenue, toVenue, (showAccessibleFeatures && !isDoNotUseUnderground));

                    int showDurationSecs = Properties.Current["DetailsLegControl.AllowTime.ShowDelay.MinimumDuration.Seconds"].Parse(121);

                    // For accessible journeys (excluding "not underground") do not display the mode image
                    // or for all other journey types where the duration of the leg is less than two mins
                    if (showAccessibleFeatures 
                        && !isDoNotUseUnderground)
                    {
                        modeImage.Visible = false;
                    }
                    else if (currentJourneyLeg.Duration.TotalSeconds < showDurationSecs)
                    {
                        modeImage.Visible = false;
                    }
                }
            }
        }
        
        /// <summary>
        /// Sets up the stop accessible link
        /// </summary>
        public void SetupLegAccessibleLink(HyperLink accessibleLink, SJPLocation location, DateTime date, List<JourneyDetail> journeyDetails,
            bool isForEndRow, string newWindowImageUrl)
        {
            accessibleLink.Visible = false;

            if (showAccessibleFeatures)
            {
                #region Set naptan, date, and operator code

                string naptan = string.Empty;

                if (location.Naptan.Count > 0)
                    naptan = location.Naptan[0];

                // For walk legs, there may not be a time set, so try using the previous or next leg date
                if ((date == DateTime.MinValue) && (previousJourneyLeg != null))
                {
                    date = previousJourneyLeg.EndTime;
                }
                if ((date == DateTime.MinValue) && (nextJourneyLeg != null))
                {
                    date = nextJourneyLeg.StartTime;
                }

                string operatorCode = string.Empty;

                if ((journeyDetails != null)
                    && (journeyDetails.Count > 0)
                    && (journeyDetails[0] is PublicJourneyDetail))
                {
                    PublicJourneyDetail pjd = (PublicJourneyDetail)journeyDetails[0];

                    if ((pjd.Services != null)
                        && (pjd.Services.Count > 0))
                    {
                        foreach (ServiceDetail sd in pjd.Services)
                        {
                            if (!string.IsNullOrEmpty(sd.OperatorCode))
                            {
                                operatorCode = sd.OperatorCode;
                                break;
                            }
                        }
                    }
                }

                #endregion

                // Get accessible url
                StopAccessibilityLinks stopAccessibilityLinksService = SJPServiceDiscovery.Current.Get<StopAccessibilityLinks>(ServiceDiscoveryKey.StopAccessibilityLinks);

                string accessibleURL = stopAccessibilityLinksService.GetAccessibilityURL(naptan, operatorCode, date);

                if (!string.IsNullOrEmpty(accessibleURL))
                {
                    #region Set up link/image text

                    accessibleLink.Text = string.Format("{0} <img src=\"{1}\" alt=\"{2}\" title=\"{3}\" />",
                        string.Format(TXT_AccessibleLink, location.DisplayName),
                        newWindowImageUrl, //ResolveClientUrl(URL_OpenInNewWindowImage_Blue),
                        ALTTXT_OpenInNewWindow,
                        TOOLTIP_OpenInNewWindow);
                    accessibleLink.ToolTip = string.Format(TXT_AccessibleLink, location.DisplayName);
                    accessibleLink.NavigateUrl = accessibleURL;
                    accessibleLink.Visible = true;

                    // Set flag for images
                    if (!isForEndRow)
                    {
                        accessibleLinkAvailable = true;
                    }
                    else
                    {
                        accessibleLinkEndAvailable = true;
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// Sets up the accessible info message
        /// </summary>
        public void SetupLegAccessibleInfo(Label accessibleInfoTextLabel)
        {
            accessibleInfoTextLabel.Visible = false;

            if (showAccessibleFeatures)
            {
                #region Set up info text

                if (showAccessibleInfo)
                {
                    accessibleInfoTextLabel.Text = TXT_AccessibleInfo;
                    accessibleInfoTextLabel.Visible = true;
                }

                #endregion
            }
        }

        /// <summary>
        /// Displays the accessible interchange text
        /// </summary>
        public void SetupLegAccessibleInterchange(Image modeImage, Label instructionLabel)
        {
            if (showWalkInterchange)
            {
                // Show "interchange to location" rather than "walk"
                instructionLabel.Text = legInstructionAdapter.GetLegDetail(SJPModeType.WalkInterchange);

                // Update the leg mode image to be interchange (rather than walk)
                SetupLegMode(modeImage, SJPModeType.WalkInterchange);
            }
        }

        /// <summary>
        /// Displays the cable car is if the leg starts and ends at cable car naptans
        /// </summary>
        /// <param name="modeImage"></param>
        public void SetupLegCableCar(Image modeImage)
        {
            if (JourneyLeg.IsLegModeCableCar(currentJourneyLeg))
            {
                // If the journey leg starts and ends at any of the cable car naptans, 
                // then display the cable car icon
                SetupLegMode(modeImage, SJPModeType.CableCar);
            }
        }

        /// <summary>
        /// Sets up the display notes for the journey leg, only if it contains a PublicJourneyDetail
        /// </summary>
        public void SetupDisplayNotes(HtmlGenericControl notesDiv, HtmlAnchor travelNotesLink, Label travelNotesLinkLabel, Image travelNotesLinkImage)
        {
            List<string> displayNotes = null;

            // Notes for journey
            PublicJourneyDetail pjd = GetPublicJourneyDetail();
            if ((pjd != null) && (pjd.DisplayNotes.Count > 0))
            {
                displayNotes = pjd.DisplayNotes;
            }
            else
            {
                RoadJourneyDetail rjd = GetRoadJourneyDetail();
                if ((rjd != null) && (rjd.DisplayNotes.Count > 0))
                {
                    displayNotes = rjd.DisplayNotes;
                }
            }

            if ((displayNotes != null) && (displayNotes.Count > 0))
            {
                NotesDisplayAdapter nda = new NotesDisplayAdapter();

                // Store in control variable (used later in UpdateImageDimensions)
                notes = nda.GetDisplayableNotes(displayNotes);

                #region Travel news addition notes js link

                if (travelNotesLink != null)
                {
                    // Link used by javascript to display note as dialog rather than in line
                    travelNotesLink.Visible = notes.Count > 0;

                    // Set up resources for js links for additional travel notes and the booking link
                    travelNotesLink.Title = TXT_TravelNotesLink;
                    travelNotesLinkImage.ImageUrl = URL_TravelNotesLink;
                    travelNotesLinkImage.ToolTip = ALTTXT_TravelNotesLink;
                    travelNotesLinkImage.AlternateText = ALTTXT_TravelNotesLink;

                    if (travelNotesLinkLabel == null)
                    {
                        // mobile site
                        travelNotesLink.InnerHtml += TXT_TravelNotesLink;
                    }
                    else
                    {
                        travelNotesLinkLabel.Text = TXT_TravelNotesLink;
                    }
                }
                #endregion

                foreach (string note in notes)
                {
                    using (Panel pnlNote = new Panel())
                    {
                        pnlNote.CssClass = "detailNote";
                        pnlNote.EnableViewState = true;

                        using (Label lblNote = new Label())
                        {
                            lblNote.Text = note;
                            lblNote.EnableViewState = true;

                            pnlNote.Controls.Add(lblNote);
                            notesDiv.Controls.Add(pnlNote);

                        }
                    }
                }
            }
            else
            {
                notesDiv.Visible = false;

                if (travelNotesLink != null)
                    travelNotesLink.Visible = false;
            }
        }

        /// <summary>
        /// Sets up the venue map link for the journey leg (if request origin or destination is a venue location)
        /// </summary>
        public void SetupVenueMapLink(HyperLink locationMapLink, HyperLink endLocationMapLink)
        {
            if (firstLeg && fromVenue)
            {
                // Set the venue map url
                locationMapLink.NavigateUrl = GetVenueMapURL((SJPVenueLocation)journeyRequest.Origin, currentJourneyLeg.StartTime.Date);
                
                // Set the venue map link text
                locationMapLink.Text = TXT_VenueMapLink;
                locationMapLink.ToolTip = string.Format(TXT_VenueMapLinkToolTip, journeyRequest.Origin.DisplayName);

                // Append "pdf" if needed, allows mobile user to know it could be a large file
                if ((!string.IsNullOrEmpty(locationMapLink.NavigateUrl))
                    && (locationMapLink.NavigateUrl.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase)))
                {
                    locationMapLink.Text = TXT_VenueMapLink + " (pdf)";
                }
            }

            if (lastLeg && toVenue)
            {
                // Set the venue map url
                endLocationMapLink.NavigateUrl = GetVenueMapURL(((SJPVenueLocation)journeyRequest.Destination), currentJourneyLeg.EndTime.Date);

                // Set the venue map link text
                endLocationMapLink.Text = TXT_VenueMapLink;
                endLocationMapLink.ToolTip = string.Format(TXT_VenueMapLinkToolTip, journeyRequest.Destination.DisplayName);
                
                // Append "pdf" if needed, allows mobile user to know it could be a large file
                if ((!string.IsNullOrEmpty(endLocationMapLink.NavigateUrl))
                    && (endLocationMapLink.NavigateUrl.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase)))
                {
                    endLocationMapLink.Text = TXT_VenueMapLink + " (pdf)";
                }
            }
        }
        
        /// <summary>
        /// Sets up and displays the interchange leg if required
        /// </summary>
        public void SetupInterchangeLeg(Panel interchangeRowPanel, Image interchangeNodeImage,
            Label interchangeLocationLabel, Label interchangeArriveLabel, Label interchangeArriveTimeLabel)
        {
            if (showInterchange)
            {
                interchangeLocationLabel.Text = legInstructionAdapter.GetPreviousEndLocation();
                interchangeArriveTimeLabel.Text = legInstructionAdapter.GetPreviousArriveTime(firstLeg, isPreviousPJTimedLeg, (isPreviousCarLeg || isPreviousCycleLeg));
                if (!string.IsNullOrEmpty(interchangeArriveTimeLabel.Text))
                {
                    interchangeArriveLabel.Text = legInstructionAdapter.GetArriveLabelText(firstLeg, false, isPreviousPJTimedLeg, isPreviousCarLeg, isPreviousCycleLeg, showInterchange);
                }

                SetupLegLineImages(interchangeNodeImage, null, interchangeLocationLabel, null, false);
            }

            interchangeRowPanel.Visible = (showInterchange);
        }

        /// <summary>
        /// Sets up and displays the last end location leg row if required
        /// </summary>
        public void SetupEndLocationLeg(Panel endLocationRowPanel, Label endLocationLabel,
            Label endArriveLabel, Label endArriveTimeLabel,
            HyperLink endLocationAccessibleLink, Image endNodeImage, string newWindowImageUrl)
        {
            if (lastLeg)
            {
                endLocationRowPanel.Visible = true;

                endLocationLabel.Text = legInstructionAdapter.GetLegEndLocation();
                endArriveLabel.Text = legInstructionAdapter.GetArriveLabelText(firstLeg, lastLeg, isPreviousPJTimedLeg, isPreviousCarLeg, isPreviousCycleLeg, false);
                endArriveTimeLabel.Text = legInstructionAdapter.GetArriveTime(firstLeg, lastLeg);

                if (endLocationAccessibleLink != null)
                {
                    SetupLegAccessibleLink(endLocationAccessibleLink, currentJourneyLeg.LegEnd.Location, currentJourneyLeg.EndTime, currentJourneyLeg.JourneyDetails, true, newWindowImageUrl);
                }

                SetupLegLineImages(endNodeImage, null, null, null, true);
            }
            else
            {
                endLocationRowPanel.Visible = false;
            }
        }

        /// <summary>
        /// Increases the height of the line image connecting legs 
        /// if it detects there is a lot of text for the leg
        /// </summary>
        /// <param name="lineImage"></param>
        /// <returns></returns>
        public void UpdateImageDimensions(Image imageNode, Image lineImage,
            Label instructionLabel, bool bookTicketAvailable, bool bookTicketTravelcardAvailable,
            bool vehicleFeaturesAvailable, bool accessibleFeaturesAvailable,
            bool gpxLinkAvailable, bool mapDirectionsLinkAvailable)
        {
            // Line image for modes is alterted based on amount of text/controls shown
            int width = IMG_Line_Width; // Doesn't need to be updated
            int height = IMG_Line_Height;

            // Used as the default height if the calculated height is less
            int heightToCompare = IMG_Line_Height;

            if (isAccessibleFriendly)
            {
                heightToCompare = IMG_Line_Height + 16;
            }
            
            // Each text row requires about 18px
            decimal rowHeight = 16.7M;
            decimal rowChars = 39;
            decimal rows = 0;

            if (isAccessibleFriendly)
            {
                rowHeight = 18;
                rowChars = 32;
            }
            if (isMobile)
            {
                rowHeight = 18;
                rowChars = 28;
            }

            #region Determine "text rows"

            // 1) Leg instruction text, a line here will typically hold about 40chars
            if (instructionLabel.Text.Length > 0)
            {
                rows = (decimal)instructionLabel.Text.Length / rowChars;

                // Because check constraint is a block of text, the above doesnt quite work, adjust the rows value
                if ((showCheckConstraint) && (!isAccessibleFriendly))
                {
                    rows -= 0.5M;
                }
            }

            // 2) Vehicle features, if shown then it'll be displaying icons, 
            // assume line can hold 10 icons (never seen it go above 5 icons!)
            // 3) Accessible features, if shown then it'll be displaying icons, 
            // assume line can hold 10 icons (never seen it go above 0 icons!)
            if ((vehicleFeaturesAvailable) || (accessibleFeaturesAvailable))
            {
                rows += 1;

                // Rows need to be at least this height by now to allow height to be greater than min needed 
                // to show at least one line of leg instruction and features image
                if (rows < 3.7M)
                {
                    rows = 3.7M;
                }
            }

            // 4a) Book travel info/handoff link, if shown then it will be showing hyperlink
            // with symbol and text, only one out of the two links visible for the leg
            if (bookTicketAvailable && !isPrinterFriendly)
            {
                // Book ticket row is a little more than 2 rows
                rows = rows + 2.3M;
            }

            // 4b) Travelcard info link, if shown then it will be showing hyperlink
            // with symbol and text
            if (bookTicketTravelcardAvailable && !isPrinterFriendly)
            {
                // Book ticket travelcard row is a little less than 2 rows
                rows = rows + 2.2M;
            }

            //// 5) Accessible link, if shown then it will be shwoing hyperlink
            //// with symbol and text
            //if (accessibleLinkAvailable)
            //{
            //    rows = rows + 1.4M;

            //    // Accessible link row may span over two rows as it contains the stop name
            //    if (accessibleLink.Text.Length > 54)
            //    {
            //        rows = rows + 0.5M;
            //    }
            //}

            // 6) Accessible info text, is shown this text will span two rows
            if (showAccessibleInfo)
            {
                rows = rows + 2;
            }

            // 7) Display notes text, can be spread accross a number of lines, 
            // therefore check each note's text amount. Plus add a bit extra for spacing
            if (notes != null)
            {
                decimal noteRows = 0;

                // a line here will typically hold about 40chars
                foreach (string note in notes)
                {
                    if (note.Length > 0)
                    {
                        noteRows = noteRows + ((decimal)note.Length / rowChars);
                    }
                }

                // Add extra because of any padding (assume for every 3 notes add 1)
                noteRows += Math.Ceiling(((decimal)notes.Count / 3));

                rows += noteRows;
            }

            // 8) If GPX link is visible
            if (gpxLinkAvailable && !isPrinterFriendly)
            {
                rows += 2;
            }

            // 9) Frequency/Bus legs add an extra line for duration (i.e. max duration and typical duration),
            // which requires the leg to have a little extra height. Check current row height and change
            if (((isPJFrequencyLeg) || (currentMode == SJPModeType.Bus)) && (rows < (showServiceNumber ? 6 : 5)))
            {
                rows = (showServiceNumber ? 6 : 5);

                if (isAccessibleFriendly)
                {
                    rows = (showServiceNumber ? 8 : 7);
                }
            }

            // Directions map link
            if (mapDirectionsLinkAvailable && !isPrinterFriendly)
            {
                rows += 1.5M;
            }

            #endregion

            // Calculate height (retaining default height if less)
            height = Convert.ToInt32(Math.Ceiling(rowHeight * rows));

            if (height < heightToCompare)
            {
                height = heightToCompare;
            }

            // Set the custom dimensions
            lineImage.Width = width;
            lineImage.Height = height;
        }
        
        #endregion

        #region Public Helper methods

        /// <summary>
        /// Returns if accessible details can be shown for the supplied naptan.
        /// If the naptan is in the suppress naptans list, false is returned
        /// If the naptan begins with the suppress naptans in list, false is returned
        /// </summary>
        /// <param name="naptan"></param>
        /// <param name="suppressNaPTANsList"></param>
        /// <returns></returns>
        public bool ShowAccessibleForNaptan(string naptan, List<string> suppressNaPTANsList)
        {
            bool show = true;

            if (!string.IsNullOrEmpty(naptan))
            {
                foreach (string suppressNaptan in suppressNaPTANsList)
                {
                    if (suppressNaptan.Equals(naptan, StringComparison.CurrentCultureIgnoreCase))
                    {
                        show = false;
                    }
                    else if (naptan.StartsWith(suppressNaptan, StringComparison.CurrentCultureIgnoreCase))
                    {
                        show = false;
                    }
                }
            }

            return show;
        }

        /// <summary>
        /// Removes any SJPAccessibilityType based on accessiblity flags set
        /// </summary>
        /// <returns></returns>
        public List<SJPAccessibilityType> FilterAccessibilityTypes(List<SJPAccessibilityType> accessibilityFeatures)
        {
            if (accessibilityFeatures != null)
            {
                // If flag set to not show any features considered as assistance, remove from list
                if (!ShowAccessibleAssistanceFeatures)
                {
                    accessibilityFeatures.Remove(SJPAccessibilityType.ServiceAssistanceBoarding);
                }
            }

            return accessibilityFeatures;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method which initialises the flags/values used by the various setup methods in this control
        /// </summary>
        private void InitialiseFlags()
        {
            firstLeg = (previousJourneyLeg == null);
            lastLeg = (nextJourneyLeg == null);

            if (isReturn)
            {
                fromVenue = (journeyRequest != null && journeyRequest.ReturnOrigin != null && journeyRequest.ReturnOrigin is SJPVenueLocation);
                toVenue = (journeyRequest != null && journeyRequest.ReturnDestination != null && journeyRequest.ReturnDestination is SJPVenueLocation);
            }
            else
            {
                fromVenue = (journeyRequest != null && journeyRequest.Origin != null && journeyRequest.Origin is SJPVenueLocation);
                toVenue = (journeyRequest != null && journeyRequest.Destination != null && journeyRequest.Destination is SJPVenueLocation);
            }

            if (currentJourneyLeg != null)
            {
                // Set current leg detail flags
                currentMode = currentJourneyLeg.Mode;

                if (currentJourneyLeg.JourneyDetails != null)
                {
                    #region Determine type of PJ leg

                    // Public Jouney Detail will only have one "JourneyDetail" per leg
                    if (currentJourneyLeg.JourneyDetails.Count > 0)
                    {
                        JourneyDetail detail = currentJourneyLeg.JourneyDetails[0];

                        if (detail is PublicJourneyFrequencyDetail)
                        {
                            isPJFrequencyLeg = true;
                        }
                        else if (detail is PublicJourneyContinuousDetail)
                        {
                            isPJContinuousLeg = true;
                        }
                        else if (detail is PublicJourneyInterchangeDetail)
                        {
                            isPJInterchangeLeg = true;
                        }
                        else if (detail is PublicJourneyTimedDetail)
                        {
                            isPJTimedLeg = true;
                        }
                        else if (detail is RoadJourneyDetail)
                        {
                            isCarLeg = true;
                        }
                        else if (detail is CycleJourneyDetail)
                        {
                            isCycleLeg = true;
                        }
                    }

                    #endregion
                }
            }

            if (previousJourneyLeg != null)
            {
                if (previousJourneyLeg.JourneyDetails != null)
                {
                    #region Determine type of leg

                    // Public Jouney Detail will only have one "JourneyDetail" per leg
                    if (previousJourneyLeg.JourneyDetails.Count > 0)
                    {
                        JourneyDetail detail = previousJourneyLeg.JourneyDetails[0];

                        if (detail is PublicJourneyTimedDetail)
                        {
                            isPreviousPJTimedLeg = true;
                        }
                        else if (detail is RoadJourneyDetail)
                        {
                            isPreviousCarLeg = true;
                        }
                        else if (detail is CycleJourneyDetail)
                        {
                            isPreviousCycleLeg = true;
                        }
                    }

                    #endregion
                }
            }

            showInterchange = InterchangeRequired();
            showWalkInterchange = WalkInterchangeRequired();
            showCheckConstraint = CheckConstraintRequired();
            showQueueMode = QueueModeRequired();
            showAccessibleAssistanceFeatures = AccessibleAssistanceFeaturesRequired();
            showServiceNumber = Properties.Current["DetailsLegControl.ShowServiceNumber.Switch"].Parse(true);
        }

        /// <summary>
        /// Method which initialises the text string used by this control
        /// </summary>
        private void InitialiseText()
        {
            Language language = CurrentLanguage.Value;

            TXT_TravelNotesLink = RM.GetString(language, RG, RC, "JourneyOutput.Text.TravelNotesLink");
            TXT_AccessibleLink = RM.GetString(language, RG, RC, "JourneyOutput.Text.AccessibleLink");
            TXT_AccessibleInfo = RM.GetString(language, RG, RC, "JourneyOutput.Text.AccessibleInfo");
            TXT_VenueMapLink = RM.GetString(language, RG, RC, "JourneyOutput.Text.VenueMapLink");
            TXT_VenueMapLinkToolTip = RM.GetString(language, RG, RC, "JourneyOutput.Text.VenueMapLinkToolTip");

            URL_TravelNotesLink = Page_ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.TravelNotesLink.ImageUrl");
            ALTTXT_TravelNotesLink = RM.GetString(language, RG, RC, "JourneyOutput.TravelNotesLink.AlternateText");

            URL_OpenInNewWindowImage = Page_ImagePath + RM.GetString(language, "OpenInNewWindow.URL");
            URL_OpenInNewWindowImage_Blue = Page_ImagePath + RM.GetString(language, "OpenInNewWindow.Blue.URL");
            ALTTXT_OpenInNewWindow = RM.GetString(language, "OpenInNewWindow.AlternateText");
            TOOLTIP_OpenInNewWindow = RM.GetString(language, "OpenInNewWindow.Text");
        }

        /// <summary>
        /// Method which initialises the image dimensions used by this control
        /// </summary>
        private void InitialiseImageDimensions()
        {
            // Set the default image values based on the mode
            IMG_Node_Width = isMobile ? IMG_Mobile_Node_Width : IMG_Web_Node_Width;
            
            IMG_Node_Height = isMobile ? IMG_Mobile_Node_Height : IMG_Web_Node_Height;
            IMG_Node_Medium_Height = isMobile ? IMG_Mobile_Node_Medium_Height : IMG_Web_Node_Medium_Height;
            IMG_Node_Start_Medium_Height = isMobile ? IMG_Mobile_Node_Start_Medium_Height: IMG_Web_Node_Start_Medium_Height;
            IMG_Node_End_Medium_Height = isMobile ? IMG_Mobile_Node_End_Medium_Height : IMG_Web_Node_End_Medium_Height;
            IMG_Node_Long_Height = isMobile ? IMG_Mobile_Node_Long_Height: IMG_Web_Node_Long_Height;
            IMG_Node_ExtraLong_Height = isMobile ? IMG_Mobile_Node_ExtraLong_Height : IMG_Web_Node_ExtraLong_Height;

            IMG_Line_Width = isMobile ? IMG_Mobile_Line_Width : IMG_Web_Line_Width;
            IMG_Line_Height = isMobile ? IMG_Mobile_Line_Height : IMG_Web_Line_Height;
        }

        /// <summary>
        /// Method which returns the venue map url to use, either retrieving from resource manager, or from the 
        /// venue location itself
        /// </summary>
        /// <param name="venue"></param>
        /// <param name="journeyDateTime"></param>
        /// <returns></returns>
        private string GetVenueMapURL(SJPVenueLocation venue, DateTime journeyDateTime)
        {
            string mapUrl = string.Empty;
            Language language = CurrentLanguage.Value;
            
            // Get map image url from resource manager, for the journey date
            mapUrl = RM.GetString(language, RG, RC, string.Format("Venue.VenueMapImage.{0}.Url.{1}",
                venue.Naptan.FirstOrDefault(),
                journeyDateTime.ToString("yyyyMMdd")));

            // Otherwise get default map image url from resource manager
            if (string.IsNullOrEmpty(mapUrl))
            {
                mapUrl = RM.GetString(language, RG, RC, string.Format("Venue.VenueMapImage.{0}.Url",
                venue.Naptan.FirstOrDefault()));
            }

            // Not in resource manager, use the map url in the venue object
            if (string.IsNullOrEmpty(mapUrl))
            {
                mapUrl = (venue.VenueMapUrl);
            }
            else
            {
                // Append the server image path to the url from resource manager
                mapUrl = Page_ImagePath + mapUrl;
            }

            return mapUrl;
        }

        #region Helpers

        /// <summary>
        /// Determines whether an interchange "pseudo-leg" needs to be displayed, 
        /// by checking if the start of the current leg matches the end of the previous one 
        /// - if not, a short walk has been removed by the CJP and so we need an interchange.    
        /// </summary>
        /// <returns>True if interchange pseudo-leg is needed.</returns>
        private bool InterchangeRequired()
        {
            if (firstLeg)
            {
                return false;
            }

            // if either location does not have a valid naptan, can only compare descriptions ...
            if ((currentJourneyLeg.LegStart.Location.Naptan == null)
                || (currentJourneyLeg.LegStart.Location.Naptan.Count == 0)
                || (previousJourneyLeg.LegEnd.Location.Naptan == null)
                || (previousJourneyLeg.LegEnd.Location.Naptan.Count == 0))
            {
                if (currentJourneyLeg.LegStart.Location.Name.Equals(
                    previousJourneyLeg.LegEnd.Location.Name))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // both locations have a valid naptan, so compare those ...
            string currentNaptan = currentJourneyLeg.LegStart.Location.Naptan[0];
            string previousNaptan = previousJourneyLeg.LegEnd.Location.Naptan[0];
            if (currentNaptan.Equals(previousNaptan))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether for an accessible journey, the walk leg should be
        /// an accessible interchange walk leg
        /// </summary>
        /// <returns>True if interchange pseudo-leg is needed.</returns>
        private bool WalkInterchangeRequired()
        {
            // Where its walk, we need to show interchange for accessible journeys 
            if ((showAccessibleFeatures) && (currentMode == SJPModeType.Walk))
            {
                // For first and last leg, leave the mode as it is,
                // prevents showing a walk interchange mode when it is not meaningful, 
                // e.g. going from a stop outside station and onto platform
                if (!firstLeg && !lastLeg)
                {
                    // If walk is between naptans (i.e. not to/from a postcode, and not to/from locality)
                    // and it not to/from a venue/gate, then set to the accessible interchange mode
                    string naptanLegStart = GetJourneyLegNaPTAN(true);
                    string naptanLegEnd = GetJourneyLegNaPTAN(false);

                    if (!string.IsNullOrEmpty(naptanLegStart) && !string.IsNullOrEmpty(naptanLegEnd))
                    {
                        string venueNaptanPrefix = Properties.Current["JourneyDetails.Location.Venue.Naptan.Prefix"];

                        // Check not a venue/gate
                        if (!string.IsNullOrEmpty(venueNaptanPrefix) &&
                            !naptanLegStart.StartsWith(venueNaptanPrefix) && !naptanLegEnd.StartsWith(venueNaptanPrefix))
                        {
                            // Accessible interchange mode should be shown
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether an check constraint needs to be displayed, 
        /// by checking if the current leg includes a "CheckConstraint"
        /// </summary>
        private bool CheckConstraintRequired()
        {
            // Assume method InitialiseFlags has set this flag correctly, no point duplicating the check
            if (isPJInterchangeLeg)
            {
                PublicJourneyInterchangeDetail pjid = (PublicJourneyInterchangeDetail)currentJourneyLeg.JourneyDetails[0];

                return pjid.HasCheckConstraint();
            }

            return false;
        }

        /// <summary>
        /// Determines whether the current leg has check contraints duration total more than the configured duration,
        /// which indicates the queue mode should be displayed
        /// </summary>
        /// <returns></returns>
        private bool QueueModeRequired()
        {
            if (isPJInterchangeLeg)
            {
                PublicJourneyInterchangeDetail pjid = (PublicJourneyInterchangeDetail)currentJourneyLeg.JourneyDetails[0];

                int showDurationSecs = Properties.Current["DetailsLegControl.CheckConstraint.ShowQueue.MinimumDuration.Seconds"].Parse(120);

                // Queue only shown for check constraints with duration of 2mins or more
                return pjid.DurationCheckConstraints >= showDurationSecs;
            }

            return false;
        }

        /// <summary>
        /// Determines whether to show accessible assistance feature icons when displaying the
        /// accessible/assistance details
        /// </summary>
        private bool AccessibleAssistanceFeaturesRequired()
        {
            if (journeyRequest != null)
            {
                if (journeyRequest.AccessiblePreferences != null)
                {
                    if (!journeyRequest.AccessiblePreferences.RequireSpecialAssistance)
                    {
                        // Assistance was not required in the accessible journey request, so don't show
                        return false;
                    }
                }
            }

            // Default to true, show
            return true;
        }
        
        #endregion

        #endregion
    }
}
