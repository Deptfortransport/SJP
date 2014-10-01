// *********************************************** 
// NAME             : DetailsLegControl.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 06 Apr 2011
// DESCRIPTION  	: Displays a high level view of the leg mode (e.g. walk, ferry, etc),
//                    it also adds a detailed view of the leg depend on the leg mode
// ************************************************


using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;
using SJP.UserPortal.ScreenFlow;

namespace SJP.UserPortal.SJPWeb.Controls
{
    #region Public events

    /// <summary>
    /// EventsArgs class for passing journey id in OnSelectedJourneyLegChange event
    /// </summary>
    public class JourneyLegEventArgs : EventArgs
    {
        private readonly bool expanded = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public JourneyLegEventArgs(bool expanded)
        {
            this.expanded = expanded;
        }

        /// <summary>
        /// Leg Detail Expanded
        /// </summary>
        public bool Expanded
        {
            get { return expanded; }
        }
    }

    // Delegate for selected leg detail being expanded/collapsed
    public delegate void OnSelectedJourneyLegDetailChange(object sender, JourneyLegEventArgs e);

    #endregion

    /// <summary>
    /// Displays a high level view of the leg mode (e.g. walk, ferry, etc),
    /// it also adds a detailed view of the leg depend on the leg mode
    /// </summary>
    public partial class DetailsLegControl : System.Web.UI.UserControl
    {
        #region Public Events

        // Selected journey detail leg event declaration
        public event OnSelectedJourneyLegDetailChange SelectedJourneyLegDetailHandler;

        #endregion

        #region Private Fields
        
        /// <summary>
        /// Leg control adapter to load controls and labels common to both SJPWeb and SJPMobile
        /// </summary>
        protected LegControlAdapter LCA = null;

        #region Resources

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private SJPResourceManager RM = Global.SJPResourceManager;

        // Resource strings
        private string TXT_Drive = string.Empty;
        private string TXT_DriveExpand = string.Empty;
        private string TXT_Cycle = string.Empty;
        private string TXT_CycleExpand = string.Empty;

        private string TXT_GPXDownLoad = string.Empty;
        private string TXT_GPXInfo = string.Empty;
        private string TXT_GPXInfoAlternateText = string.Empty;
        private string IMG_GPXInfoImageUrl = string.Empty;

        private string URL_ShowDetail_Close = string.Empty;
        private string URL_ShowDetail_Open = string.Empty;
        private string ALTTXT_ShowDetail_Close = string.Empty;
        private string TOOLTIP_ShowDetail_Close = string.Empty;
        private string ALTTXT_ShowDetail_Open = string.Empty;
        private string TOOLTIP_ShowDetail_Open = string.Empty;
        
        private string URL_BookTicketLinkImage = string.Empty;
        private string ALTTXT_BookTicketLink = string.Empty;
        private string ALTTXT_BookTicketLinkAccessible = string.Empty;

        private string URL_BookTicketInfo = string.Empty;
        private string URL_BookTicketInfoImage = string.Empty;
        private string ALTTXT_BookTicketInfo = string.Empty;

        private string URL_TravelcardLink = string.Empty;

        private string URL_OpenInNewWindowImage = string.Empty;
        private string URL_OpenInNewWindowImage_Blue = string.Empty;
        private string ALTTXT_OpenInNewWindow = string.Empty;
        private string TOOLTIP_OpenInNewWindow = string.Empty;

        private string TXT_TravelNotesLink = string.Empty;
        private string TXT_BookTicketLink = string.Empty;
        private string TXT_BookTicketLinkAccessible = string.Empty;
        private string TXT_BookTicketLinkCoachAndRail = string.Empty;
        private string TXT_BookTicketInfo = string.Empty;
        private string TXT_TravelcardLink = string.Empty;

        private string TXT_DirectionsMapLink_Cycle = string.Empty;
        private string TXT_DirectionsMapLinkToolTip_Cycle = string.Empty;
        private string TXT_DirectionsMapLink_Walk = string.Empty;
        private string TXT_DirectionsMapLinkToolTip_Walk = string.Empty;

        #endregion

        private ISJPJourneyRequest journeyRequest = null;

        private JourneyLeg previousJourneyLeg;
        private JourneyLeg currentJourneyLeg;
        private JourneyLeg nextJourneyLeg;
        private string journeyRequestHash;

        // Retailers which apply to the current leg
        private List<Retailer> retailers = null;
        private bool isCoachAndRailRetailer = false; // Used for showing the "Book combined" text on the booking link
        private bool isTravelcardRetailer = false; // Used to show leg "covered by Games Travelcard" link

        // Track outward/return journey Id for cycle GPX link
        private int journeyId = -1;
        private int journeyLegIndex = -1;

        // Used in updating image dimensions
        private bool vehicleFeaturesAvailable = false;
        private bool accessibleFeaturesLocationAvailable = false;
        private bool accessibleFeaturesLocationEndAvailable = false;
        private bool accessibleFeaturesAvailable = false;
        private bool bookTicketAvailable = false;
        private bool bookTicketTravelcardAvailable = false;
        private bool gpxLinkAvailable = false;
        private bool mapDirectionsLinkAvailable = false;

        // Indicates if the journey leg has a "detail" element, whether to expand or collapse
        private bool legDetailExpanded = false;
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or Sets the individual journey leg detail displayed using DetailsLegControl
        /// </summary>
        public JourneyLeg JourneyLegDetail
        {
            get { return currentJourneyLeg; }
            set { currentJourneyLeg = value; }
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialiseImageResource();

            legInstructionBefore.Click += new EventHandler(legInstructionBeforeCar_Click);
            legInstructionBefore.Click += new EventHandler(legInstructionBeforeCycle_Click);

            showDetail.Click += new ImageClickEventHandler(legInstructionBeforeCar_Click);
            showDetail.Click += new ImageClickEventHandler(legInstructionBeforeCycle_Click);
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (LCA != null)
            {
                // Set visibility of book ticket link panel, should only be visibile when links are visible,
                // otherwise unneccessary "space" is added which can break the leg node/connector line
                bookTicketDiv.Visible = (bookTicketLink.Visible || bookTicketInfo.Visible || travelcardLink.Visible) && !LCA.IsPrinterFriendly;

                vehicleFeaturesDiv.Visible = (vehicleFeaturesAvailable || accessibleFeaturesAvailable);

                travelNotesLinkDiv.Visible = ((LCA.DisplayNotes != null) && (LCA.DisplayNotes.Count > 0));

                accessibleLinkDiv.Visible = accessibleLink.Visible && !LCA.IsPrinterFriendly;
                accessibleInfoDiv.Visible = LCA.ShowAccessibleInfo;
                endLocationAccessibleLinkDiv.Visible = (LCA.LastLeg && endLocationAccessibleLink.Visible && !LCA.IsPrinterFriendly);

                directionsMapLinkDiv.Visible = (!string.IsNullOrEmpty(directionsMapLinkText.Text));
            }
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// Click event for legInstructionBefore click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void legInstructionBeforeCar_Click(object sender, EventArgs e)
        {
            bool expanded = false;

            if (legCar.Visible && carLeg.Visible)
            {
                if (legCar.Attributes["class"].Contains("collapsed"))
                {
                    legCar.Attributes["class"] = legCar.Attributes["class"].Replace("collapsed", "expanded");

                    expanded = true;
                }
                else
                {
                    legCar.Attributes["class"] = legCar.Attributes["class"].Replace("expanded", "collapsed");
                }

                // Raise event to tell subscribers selected journey leg detail has changed
                if (SelectedJourneyLegDetailHandler != null)
                {
                    SelectedJourneyLegDetailHandler(sender, new JourneyLegEventArgs(expanded));
                }

                showDetail.ImageUrl = expanded ? URL_ShowDetail_Open : URL_ShowDetail_Close;
                showDetail.AlternateText = expanded ? ALTTXT_ShowDetail_Open : ALTTXT_ShowDetail_Close;
                showDetail.ToolTip = expanded ? TOOLTIP_ShowDetail_Open : TOOLTIP_ShowDetail_Close;
            }
        }

        /// <summary>
        /// Click event for legInstructionBefore click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void legInstructionBeforeCycle_Click(object sender, EventArgs e)
        {
            bool expanded = false;

            if (legCycle.Visible && cycleLeg.Visible)
            {
                if (legCycle.Attributes["class"].Contains("collapsed"))
                {
                    legCycle.Attributes["class"] = legCycle.Attributes["class"].Replace("collapsed", "expanded");



                    expanded = true;
                }
                else
                {
                    legCycle.Attributes["class"] = legCycle.Attributes["class"].Replace("expanded", "collapsed");
                }

                // Raise event to tell subscribers selected journey leg detail has changed
                if (SelectedJourneyLegDetailHandler != null)
                {
                    SelectedJourneyLegDetailHandler(sender, new JourneyLegEventArgs(expanded));
                }

                showDetail.ImageUrl = expanded ? URL_ShowDetail_Open : URL_ShowDetail_Close;
                showDetail.AlternateText = expanded ? ALTTXT_ShowDetail_Open : ALTTXT_ShowDetail_Close;
                showDetail.ToolTip = expanded ? TOOLTIP_ShowDetail_Open : TOOLTIP_ShowDetail_Close;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise
        /// </summary>
        /// <param name="journeyLegDetail"></param>
        public void Initialise(ISJPJourneyRequest journeyRequest, JourneyLeg previousJourneyLeg, JourneyLeg currentJourneyLeg, JourneyLeg nextJourneyLeg,
            List<Retailer> retailers, bool isCoachAndRailRetailer, bool isTravelcardRetailer,
            bool showAccessibleFeatures, bool showAccessibleInfo,
            bool legDetailExpanded, bool isPrinterFriendly, bool isAccessibleFriendly, 
            int journeyId, int journeyLegIndex, bool isReturn, string journeyRequestHash)
        {
            this.journeyRequest = journeyRequest;
            this.previousJourneyLeg = previousJourneyLeg;
            this.currentJourneyLeg = currentJourneyLeg;
            this.nextJourneyLeg = nextJourneyLeg;

            // Values specific to this control
            this.retailers = retailers;
            this.isCoachAndRailRetailer = isCoachAndRailRetailer;
            this.isTravelcardRetailer = isTravelcardRetailer;
            this.journeyId = journeyId;
            this.journeyLegIndex = journeyLegIndex;
            this.journeyRequestHash = journeyRequestHash;
            this.legDetailExpanded = legDetailExpanded;

            SJPPage page = (SJPPage)Page;

            LCA = new LegControlAdapter(journeyRequest, 
                previousJourneyLeg, currentJourneyLeg, nextJourneyLeg,
                showAccessibleFeatures, showAccessibleInfo,
                isPrinterFriendly, isAccessibleFriendly, isReturn, false,
                Global.SJPResourceManager, page.ImagePath);

            InitialiseText();
            InitialiseImageResource();

            SetupLeg();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets up all the leg details
        /// </summary>
        private void SetupLeg()
        {
            if (currentJourneyLeg != null)
            {
                // Setup this control using the LegControlAdapter
                LCA.SetupLegInstruction(legLocation, legInstruction, legInstructionTime, legModeDetail, legArrive, legArriveTime, legDepart, legDepartTime);
                LCA.SetupLegMode(legMode, LCA.CurrentMode);
                LCA.SetupCheckConstraint(legMode, legModeDetail, legInstruction);
                LCA.SetupLegAccessibleLink(accessibleLink, 
                    currentJourneyLeg.LegStart.Location, currentJourneyLeg.StartTime,
                    currentJourneyLeg.JourneyDetails, false, ResolveClientUrl(URL_OpenInNewWindowImage_Blue));
                LCA.SetupLegAccessibleInfo(accessibleInfoText);
                LCA.SetupLegAccessibleInterchange(legMode, legInstruction);
                LCA.SetupLegCableCar(legMode);
                LCA.SetupDisplayNotes(divNotes, travelNotesLink, travelNotesLinkText, travelNotesLinkSymbol);
                LCA.SetupLegLineImages(legNodeImage, legLineImage, legLocation, accessibleLink, false);
                LCA.SetupInterchangeLeg(pnlInterchangeRow, interchangeNodeImage, interchangeLocation,
                    interchangeArrive, interchangeArriveTime);
                LCA.SetupEndLocationLeg(pnlEndLocationRow, endLocation, endArrive, endArriveTime,
                    endLocationAccessibleLink, endNodeImage, ResolveClientUrl(URL_OpenInNewWindowImage_Blue));

                // Setup elements directly in this control
                SetupLegVehicleFeatures();
                SetupLegAccessibleFeatures();
                SetupBookTravelLink();
                SetupDirectionsMapLink();

                // Expandable controls
                if (LCA.ShowCycleLeg)
                {
                    SetupCycleLeg(true);
                }

                if (LCA.ShowCarLeg)
                {
                    SetupCarLeg(true);
                }

                SetupDebugInformation();

                // Adjust image dimensions where required
                LCA.UpdateImageDimensions(legNodeImage, legLineImage, legInstruction, 
                    bookTicketAvailable, bookTicketTravelcardAvailable, 
                    vehicleFeaturesAvailable, accessibleFeaturesAvailable, gpxLinkAvailable, mapDirectionsLinkAvailable);
            }

        }

        /// <summary>
        /// Method which initialises the text string used by this control
        /// </summary>
        private void InitialiseText()
        {
            SJPPage page = (SJPPage)Page;

            Language language = CurrentLanguage.Value;

            TXT_Drive = RM.GetString(language, RG, RC, "JourneyOutput.Text.Drive");
            TXT_DriveExpand = RM.GetString(language, RG, RC, "JourneyOutput.Text.DriveExpand");
            TXT_Cycle = RM.GetString(language, RG, RC, "JourneyOutput.Text.Cycle");
            TXT_CycleExpand = RM.GetString(language, RG, RC, "JourneyOutput.Text.CycleExpand");
            TXT_GPXDownLoad = RM.GetString(language, RG, RC, "JourneyOutput.Text.GPXDownLoad");
            TXT_GPXInfo = RM.GetString(language, RG, RC, "JourneyOutput.Text.GPXInfo");
            TXT_GPXInfoAlternateText = RM.GetString(language, RG, RC, "JourneyOptions.GPXInfoImage.AlternateText");
            
            TXT_BookTicketLink = RM.GetString(language, RG, RC, "JourneyOutput.Text.BookTicketLink");
            TXT_BookTicketLinkAccessible = RM.GetString(language, RG, RC, "JourneyOutput.Text.BookTicketLink.Accessible");
            TXT_BookTicketLinkCoachAndRail = RM.GetString(language, RG, RC, "JourneyOutput.Text.BookTicketLink.CoachAndRail");
            TXT_BookTicketInfo = RM.GetString(language, RG, RC, "JourneyOutput.Text.BookTicketInfo");
            TXT_TravelcardLink = RM.GetString(language, RG, RC, "JourneyOutput.Text.TravelcardLink");
            
            URL_BookTicketInfo = RM.GetString(language, RG, RC, "JourneyOutput.Url.BookTicketInfo");
            URL_TravelcardLink = RM.GetString(language, RG, RC, "JourneyOutput.Url.TravelcardLink");

            TXT_DirectionsMapLink_Cycle = RM.GetString(language, RG, RC, "JourneyOutput.Text.DirectionsMapLink.Cycle");
            TXT_DirectionsMapLinkToolTip_Cycle = RM.GetString(language, RG, RC, "JourneyOutput.Text.DirectionsMapLinkToolTip.Cycle");
            TXT_DirectionsMapLink_Walk = RM.GetString(language, RG, RC, "JourneyOutput.Text.DirectionsMapLink.Walk");
            TXT_DirectionsMapLinkToolTip_Walk = RM.GetString(language, RG, RC, "JourneyOutput.Text.DirectionsMapLinkToolTip.Walk");

            IMG_GPXInfoImageUrl = page.ImagePath + RM.GetString(language, RG, RC, "JourneyOptions.GPXInfoImage.Imageurl");
        }

        /// <summary>
        /// Method which initialises the resource strings used by the image controls
        /// </summary>
        private void InitialiseImageResource()
        {
            SJPPage page = (SJPPage)Page;

            Language language = CurrentLanguage.Value;

            URL_ShowDetail_Close = page.ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Detail.Close.ImageUrl");
            URL_ShowDetail_Open = page.ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.Detail.Open.ImageUrl");
            ALTTXT_ShowDetail_Close = RM.GetString(language, RG, RC, "JourneyOutput.Detail.Close.AlternateText");
            TOOLTIP_ShowDetail_Close = RM.GetString(language, RG, RC, "JourneyOutput.Detail.Close.ToolTip");
            ALTTXT_ShowDetail_Open = RM.GetString(language, RG, RC, "JourneyOutput.Detail.Close.AlternateText");
            TOOLTIP_ShowDetail_Open = RM.GetString(language, RG, RC, "JourneyOutput.Detail.Close.ToolTip");
                        
            URL_BookTicketLinkImage = page.ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.BookTicketLink.ImageUrl");
            ALTTXT_BookTicketLink = RM.GetString(language, RG, RC, "JourneyOutput.BookTicketLink.AlternateText");
            ALTTXT_BookTicketLinkAccessible = RM.GetString(language, RG, RC, "JourneyOutput.BookTicketLink.Accessible.AlternateText");

            URL_BookTicketInfoImage = page.ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.BookTicketInfo.ImageUrl");
            ALTTXT_BookTicketInfo = RM.GetString(language, RG, RC, "JourneyOutput.BookTicketInfo.AlternateText");

            URL_OpenInNewWindowImage = page.ImagePath + Global.SJPResourceManager.GetString(language, "OpenInNewWindow.URL");
            URL_OpenInNewWindowImage_Blue = page.ImagePath + Global.SJPResourceManager.GetString(language, "OpenInNewWindow.Blue.URL");
            ALTTXT_OpenInNewWindow = Global.SJPResourceManager.GetString(language, "OpenInNewWindow.AlternateText");
            TOOLTIP_OpenInNewWindow = Global.SJPResourceManager.GetString(language, "OpenInNewWindow.Text");
        }

        #region Expandable legs

        /// <summary>
        /// Sets up addition information i.e. cycleleg to be shown
        /// </summary>
        /// <param name="active">True if cycleleg and other relevant detail to be shown</param>
        private void SetupCycleLeg(bool active)
        {
            cycleLeg.Visible = active;
            legCycle.Visible = active;
            legInstructionBefore.Visible = active;
            arrow_btn.Visible = active;
            showDetail.Visible = active;

            if (active)
            {
                // Set up show detail image button
                showDetail.ImageUrl = URL_ShowDetail_Close;
                showDetail.AlternateText = ALTTXT_ShowDetail_Close;
                showDetail.ToolTip = TOOLTIP_ShowDetail_Close;

                // Set up the link
                legInstructionBefore.Text = TXT_Cycle;
                legInstructionBefore.ToolTip = TXT_CycleExpand;

                // Set id to be used by javascript for client side
                legInstructionBeforeId.Value = legCycle.ClientID;

                // Expand if required
                if (legDetailExpanded)
                {
                    if (legCycle.Attributes["class"].Contains("collapsed"))
                    {
                        legCycle.Attributes["class"] = legCycle.Attributes["class"].Replace("collapsed", "expanded");
                        showDetail.ImageUrl = URL_ShowDetail_Open;
                        showDetail.AlternateText = ALTTXT_ShowDetail_Open;
                        showDetail.ToolTip = TOOLTIP_ShowDetail_Open;
                    }
                }

                // Initialise the cycle leg details
                cycleLeg.Initialise(journeyRequest, currentJourneyLeg);

                #region GPX Download link

                gpxLink.Text = TXT_GPXDownLoad;
                gpxLink.ToolTip = TXT_GPXDownLoad;

                gpxLink.NavigateUrl = GetGPXDownLoadLink();
                gpxLink.Visible = Properties.Current["DetailsLegControl.GPXLink.Available.Switch"].Parse(true);
                gpxLinkAvailable = gpxLink.Visible;

                gpxTooltipInfo.Title = TXT_GPXInfo;
                gpxTooltipInfo.Visible = Properties.Current["DetailsLegControl.GPXLink.Available.Switch"].Parse(true);
                gpxInfoImage.AlternateText = TXT_GPXInfoAlternateText;
                gpxInfoImage.ImageUrl = IMG_GPXInfoImageUrl;
                gpxInfoImage.Visible = Properties.Current["DetailsLegControl.GPXLink.Available.Switch"].Parse(true);

                #endregion
            }

            // Hide link for printer friendly
            if (LCA.IsPrinterFriendly)
            {
                legInstructionBefore.Visible = false;
                gpxLink.Visible = false;
                gpxTooltipInfo.Visible = false;
                gpxInfoImage.Visible = false;
                gpxLinkAvailable = false;
                showDetail.Visible = false;
                arrow_btn.Visible = false;
            }
        }

        /// <summary>
        /// Sets up additional information i.e. carleg to be shown
        /// </summary>
        /// <param name="active">True if carleg and other relevan detail to be shown</param>
        private void SetupCarLeg(bool active)
        {
            carLeg.Visible = active;
            legCar.Visible = active;
            legInstructionBefore.Visible = active;
            arrow_btn.Visible = active;
            showDetail.Visible = active;

            if (active)
            {
                // Set up show detail image button
                showDetail.ImageUrl = URL_ShowDetail_Close;
                showDetail.AlternateText = ALTTXT_ShowDetail_Close;
                showDetail.ToolTip = TOOLTIP_ShowDetail_Close;

                // Set up the link
                legInstructionBefore.Text = TXT_Drive;
                legInstructionBefore.ToolTip = TXT_DriveExpand;

                // Set id to be used by javascript for client side
                legInstructionBeforeId.Value = legCar.ClientID;

                // Expand if required
                if (legDetailExpanded)
                {
                    if (legCar.Attributes["class"].Contains("collapsed"))
                    {
                        legCar.Attributes["class"] = legCar.Attributes["class"].Replace("collapsed", "expanded");
                        showDetail.ImageUrl = URL_ShowDetail_Open;
                        showDetail.AlternateText = ALTTXT_ShowDetail_Open;
                        showDetail.ToolTip = TOOLTIP_ShowDetail_Open;
                    }
                }

                // Initialise the car leg details
                carLeg.Initialise(currentJourneyLeg);
            }

            // Hide link for printer friendly
            if (LCA.IsPrinterFriendly)
            {
                legInstructionBefore.Visible = false;
                showDetail.Visible = false;
                arrow_btn.Visible = false;
            }
        }

        #endregion
                
        /// <summary>
        /// Sets up the vehicle features for the journey leg, only if it contains a PublicJourneyDetail
        /// </summary>
        private void SetupLegVehicleFeatures()
        {
            // Vehicle Features
            PublicJourneyDetail pjd = LCA.GetPublicJourneyDetail();

            if (pjd != null)
            {
                legVehicleFeatures.Initialise(pjd.VehicleFeatures);

                vehicleFeaturesAvailable = (pjd.VehicleFeatures.Count > 0);
            }

            // Display if features found
            legVehicleFeatures.Visible = vehicleFeaturesAvailable;
        }

        /// <summary>
        /// Sets up the accessible features for the journey leg, only if it contains a PublicJourneyDetail
        /// </summary>
        private void SetupLegAccessibleFeatures()
        {
            // Accessible Features
            if (LCA.ShowAccessibleFeatures)
            {
                PublicJourneyDetail pjd = LCA.GetPublicJourneyDetail();

                if (pjd != null)
                {
                    List<string> suppressForNaPTANsPrefix = new List<string>();

                    string suppressForNaPTANPrefix = Properties.Current["JourneyDetails.Accessibility.Icons.SuppressForNaPTANs"];

                    if (!string.IsNullOrEmpty(suppressForNaPTANPrefix))
                    {
                        suppressForNaPTANsPrefix.AddRange(suppressForNaPTANPrefix.ToUpper().Split(','));
                    }

                    #region Board location

                    if (LCA.ShowAccessibleForNaptan(LCA.GetJourneyLegNaPTAN(true), suppressForNaPTANsPrefix))
                    {
                        // Accessibility features for location (board)
                        legLocationAccessibleFeatures.Initialise(pjd.BoardAccessibility);
                        accessibleFeaturesLocationAvailable = (pjd.BoardAccessibility.Count > 0);
                    }

                    #endregion

                    #region Leg/Service

                    // Accessibility features for leg/service
                    List<SJPAccessibilityType> accessibilityFeatures = new List<SJPAccessibilityType>();


                    if (pjd is PublicJourneyInterchangeDetail)
                    {
                        // Interchange detail can contain additional accessibility details
                        PublicJourneyInterchangeDetail pjid = (PublicJourneyInterchangeDetail)pjd;

                        accessibilityFeatures.AddRange(pjid.InterchangeLegAccessibility);
                    }

                    accessibilityFeatures.AddRange(pjd.ServiceAccessibility);

                    // Filter out any accessible icons that shouldn't be shown
                    accessibilityFeatures = LCA.FilterAccessibilityTypes(accessibilityFeatures);

                    legAccessibleFeatures.Initialise(accessibilityFeatures);
                    accessibleFeaturesAvailable = (accessibilityFeatures.Count > 0);

                    #endregion

                    #region Alight location

                    if (LCA.LastLeg)
                    {
                        if (LCA.ShowAccessibleForNaptan(LCA.GetJourneyLegNaPTAN(false), suppressForNaPTANsPrefix))
                        {
                            // Accessibility features for location (alight)
                            endLocationAccessibleFeatures.Initialise(pjd.AlightAccessibility);
                            accessibleFeaturesLocationEndAvailable = (pjd.AlightAccessibility.Count > 0);
                        }
                    }

                    #endregion
                }
            }

            // Display if features found
            legLocationAccessibleFeatures.Visible = accessibleFeaturesLocationAvailable;
            legAccessibleFeatures.Visible = accessibleFeaturesAvailable;
            endLocationAccessibleFeatures.Visible = accessibleFeaturesLocationEndAvailable;
        }
        
        /// <summary>
        /// Sets up book travel retailer handoff / info link
        /// </summary>
        private void SetupBookTravelLink()
        {
            string handoffUrl = string.Empty;

            string retailerHandoffUrl = RetailHandoffPageURL(retailers.Count);

            RetailerHelper retailerHelper = new RetailerHelper();

            // If only 1 retailer found, send the user to retailer handoff page
            if (retailers.Count == 1)
            {
                if (!string.IsNullOrEmpty(retailerHandoffUrl))
                {
                    // Get the handoff url, this will load the aspx to build the handoff xml and post to retailer
                    handoffUrl = retailerHelper.BuildRetailerHandoffURL(retailerHandoffUrl, retailers[0],
                        journeyRequestHash, journeyId, LCA.IsReturn);
                }
            }
            // more than 1 retailers found, send user to retailers page
            else if (retailers.Count > 1)
            {
                handoffUrl = retailerHelper.BuildRetailersURL(retailerHandoffUrl, journeyRequestHash, journeyId, LCA.IsReturn);
            }

            bookTicketLink.Visible = false;
            bookTicketInfo.Visible = false;
            travelcardLink.Visible = false;

            #region Set up link/image text

            // Ticket hyperlink
            bookTicketLinkText.Text = (!LCA.ShowAccessibleInfo) ?
                    string.Format(TXT_BookTicketLink, currentJourneyLeg.Mode.ToString().ToLower()) :
                    string.Format(TXT_BookTicketLinkAccessible, currentJourneyLeg.Mode.ToString().ToLower());
            if (isCoachAndRailRetailer)
                bookTicketLinkText.Text = TXT_BookTicketLinkCoachAndRail;
            bookTicketLinkText.ToolTip = bookTicketLinkText.Text;

            bookTicketLinkSymbol.ImageUrl = URL_BookTicketLinkImage;
            bookTicketLinkSymbol.AlternateText = (!LCA.ShowAccessibleInfo) ?
                    string.Format(ALTTXT_BookTicketLink, currentJourneyLeg.Mode.ToString().ToLower()) :
                    string.Format(ALTTXT_BookTicketLinkAccessible, currentJourneyLeg.Mode.ToString().ToLower());
            bookTicketLinkSymbol.ToolTip = bookTicketLinkSymbol.AlternateText;

            openInNewWindow_Link.ImageUrl = URL_OpenInNewWindowImage;
            openInNewWindow_Link.AlternateText = ALTTXT_OpenInNewWindow;
            openInNewWindow_Link.ToolTip = TOOLTIP_OpenInNewWindow;

            // Information hyperlink
            bookTicketInfoText.Text = string.Format(TXT_BookTicketInfo, currentJourneyLeg.Mode.ToString().ToLower());
            bookTicketInfoText.ToolTip = bookTicketInfoText.Text;

            bookTicketInfoSymbol.ImageUrl = URL_BookTicketInfoImage;
            bookTicketInfoSymbol.AlternateText = string.Format(ALTTXT_BookTicketInfo, currentJourneyLeg.Mode.ToString().ToLower());
            bookTicketInfoSymbol.ToolTip = bookTicketInfoSymbol.AlternateText;

            openInNewWindow_Info.ImageUrl = URL_OpenInNewWindowImage_Blue;
            openInNewWindow_Info.AlternateText = ALTTXT_OpenInNewWindow;
            openInNewWindow_Info.ToolTip = TOOLTIP_OpenInNewWindow;

            // Travelcard hyperlink
            travelcardLinkText.Text = TXT_TravelcardLink;
            travelcardLinkText.ToolTip = travelcardLinkText.Text;

            openInNewWindow_Travelcard.ImageUrl = URL_OpenInNewWindowImage;
            openInNewWindow_Travelcard.AlternateText = ALTTXT_OpenInNewWindow;
            openInNewWindow_Travelcard.ToolTip = TOOLTIP_OpenInNewWindow;

            #endregion

            if (isTravelcardRetailer)
            {
                travelcardLink.NavigateUrl = URL_TravelcardLink;
                travelcardLink.Visible = true;

                // Add a space to the end to force a gap with the open in new window image
                travelcardLinkText.Text = travelcardLinkText.Text.Trim() + " ";

                // Set flag for adjusting line image
                bookTicketTravelcardAvailable = true;
            }
            
            if (!string.IsNullOrEmpty(handoffUrl) && retailers.Count > 0)
            {
                // If travelcard is available and booking handoff available 
                // and is for Rail or Coach, then don't display the booking link
                if (!(isTravelcardRetailer && 
                    (currentJourneyLeg.Mode == SJPModeType.Rail || currentJourneyLeg.Mode == SJPModeType.Coach)))
                {
                    bookTicketLink.NavigateUrl = handoffUrl;
                    bookTicketLink.Visible = true;
                    bookTicketInfo.Visible = false;
                    
                    // Set flag for adjusting line image
                    bookTicketAvailable = true;

                    // more than 1 retailer found 
                    // hide the open in new window and make the book ticket link to open in the same window
                    if (retailers.Count > 1)
                    {
                        openInNewWindow_Link.Visible = false;
                        bookTicketLink.Target = "_self";
                    }
                    else
                    {
                        // Add a space to the end to force a gap with the open in new window image
                        bookTicketLinkText.Text = bookTicketLinkText.Text.Trim() + " ";
                    }
                }
            }
            else if (currentJourneyLeg.Mode == SJPModeType.Rail
                    || currentJourneyLeg.Mode == SJPModeType.Ferry
                    || currentJourneyLeg.Mode == SJPModeType.Coach
                    || currentJourneyLeg.Mode == SJPModeType.Air)
            {
                bookTicketInfo.NavigateUrl = URL_BookTicketInfo;
                bookTicketLink.Visible = false;
                bookTicketInfo.Visible = true;
                
                // Add a space to the end to force a gap with the open in new window image
                bookTicketInfoText.Text = bookTicketInfoText.Text.Trim() + " ";

                // Set flag for adjusting line image
                bookTicketAvailable = true;
            }

        }

        /// <summary>
        /// Sets up the link for going to the maps page to show the journey directions on a map
        /// </summary>
        private void SetupDirectionsMapLink()
        {
            // Show the map link for cycle leg
            if (LCA.CurrentMode == SJPModeType.Cycle)
            {
                #region Set up map link for cycle leg

                if (Properties.Current["Map.Journey.Cycle.Enabled.Switch"].Parse(true))
                {
                    MapHelper mapHelper = new MapHelper(Global.SJPResourceManager);

                    // Cycle link goes to the Map (Bing) page
                    directionsMapLink.NavigateUrl = mapHelper.BuildMapURL(
                        MapPageURL(SJPModeType.Cycle), journeyRequestHash, journeyId, -1, LCA.IsReturn);

                    directionsMapLinkText.Text = TXT_DirectionsMapLink_Cycle;
                    directionsMapLinkText.ToolTip = TXT_DirectionsMapLinkToolTip_Cycle;

                    // Opens in current window, so hide the new window image
                    openInNewWindow_MapLink.ImageUrl = string.Empty;
                    openInNewWindow_MapLink.Visible = false;

                    mapDirectionsLinkAvailable = true;
                }

                #endregion
            }
            // Show the map link for walk leg (accessible journeys only)
            else if ((LCA.CurrentMode == SJPModeType.Walk) && (LCA.ShowAccessibleFeatures))
            {
                #region Set up map link for walk leg

                if (Properties.Current["Map.Journey.Walk.Enabled.Switch"].Parse(true))
                {
                    // Display only for first or last walk leg, but NOT for 
                    // - a walk only journey
                    // - leg starts at a naptan (first leg)
                    // - leg ends at a naptan (last leg)
                    if (((!LCA.IsReturn && LCA.FirstLeg) || (LCA.IsReturn && LCA.LastLeg))
                        && !(LCA.FirstLeg && LCA.LastLeg))
                    {
                        if ((!LCA.IsReturn && currentJourneyLeg.LegStart.Location.Naptan.Count == 0)
                            || (LCA.IsReturn && currentJourneyLeg.LegEnd.Location.Naptan.Count == 0))
                        {
                            MapHelper mapHelper = new MapHelper(Global.SJPResourceManager);

                            // Walk link goes to the Map (Google) page, displayed in new window
                            directionsMapLink.NavigateUrl = mapHelper.BuildMapURL(
                                MapPageURL(SJPModeType.Walk), journeyRequestHash, journeyId, -1, LCA.IsReturn);
                            directionsMapLink.Target = "_blank";

                            // Add a space to the end to force a gap with the open in new window image
                            directionsMapLinkText.Text = TXT_DirectionsMapLink_Walk + " ";
                            directionsMapLinkText.ToolTip = TXT_DirectionsMapLinkToolTip_Walk;

                            // Opens in new window, so display the new window image
                            openInNewWindow_MapLink.Visible = true;
                            openInNewWindow_MapLink.ImageUrl = URL_OpenInNewWindowImage;
                            openInNewWindow_MapLink.AlternateText = ALTTXT_OpenInNewWindow;
                            openInNewWindow_MapLink.ToolTip = TOOLTIP_OpenInNewWindow;

                            mapDirectionsLinkAvailable = true;
                        }
                    }
                }

                #endregion
            }

            // Hide link for printer friendly
            if (LCA.IsPrinterFriendly)
            {
                directionsMapLink.Visible = false;
                directionsMapLinkText.Visible = false;
                openInNewWindow_MapLink.Visible = false;
                mapDirectionsLinkAvailable = false;
            }
        }

        #region Helpers
        
        /// <summary>
        /// Build and returns a cycle journey GPX download link
        /// </summary>
        /// <returns></returns>
        private string GetGPXDownLoadLink()
        {
            IPageController pageController = SJPServiceDiscovery.Current.Get<IPageController>(ServiceDiscoveryKey.PageController);

            PageTransferDetail pageTransferDetail = pageController.GetPageTransferDetails(Common.PageId.CycleJourneyGPXDownload);

            URLHelper helper = new URLHelper();

            string url = helper.AddQueryStringPart(pageTransferDetail.PageUrl,
                LCA.IsReturn ? QueryStringKey.JourneyIdReturn : QueryStringKey.JourneyIdOutward,
                journeyId.ToString());

            return url;

        }

        /// <summary>
        /// Returns the retailer page transfer url based of wether retailer count is 1 or more than 1
        /// If retailer count is 0 or less the method returns empty string
        /// </summary>
        /// <param name="retailerCount">Retailer found for the journey leg</param>
        /// <returns></returns>
        private string RetailHandoffPageURL(int retailerCount)
        {
            SJPPage page = (SJPPage)Page;
            PageTransferDetail ptd = null;

            if (retailerCount == 1)
            {
                ptd = page.GetPageTransferDetail(PageId.RetailerHandoff);
            }
            else if (retailerCount > 1)
            {
                ptd = page.GetPageTransferDetail(PageId.Retailers);
            }

            if (ptd != null)
            {
                return ResolveClientUrl(ptd.PageUrl);
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns the map page transfer url based on the SJPModeType
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private string MapPageURL(SJPModeType mode)
        {
            SJPPage page = (SJPPage)Page;
            PageTransferDetail ptd = null;

            if (mode == SJPModeType.Walk)
            {
                ptd = page.GetPageTransferDetail(PageId.MapGoogle);
            }
            else
            {
                ptd = page.GetPageTransferDetail(PageId.MapBing);
            }

            if (ptd != null)
            {
                return ResolveClientUrl(ptd.PageUrl);
            }

            return string.Empty;
        }

        /// <summary>
        /// Adds debug information to controls (where possible)
        /// </summary>
        private void SetupDebugInformation()
        {
            if (DebugHelper.ShowDebug)
            {
                // Location
                legLocation.Text += string.Format("<span class=\"debug\"> napt[{0}] osgr[{1},{2}]</span>",
                    LCA.GetJourneyLegNaPTAN(true),
                    currentJourneyLeg.LegStart.Location.GridRef.Easting,
                    currentJourneyLeg.LegStart.Location.GridRef.Northing);

                if (LCA.LastLeg)
                {
                    endLocation.Text += string.Format("<span class=\"debug\"> napt[{0}] osgr[{1},{2}]</span>",
                        LCA.GetJourneyLegNaPTAN(false),
                        currentJourneyLeg.LegEnd.Location.GridRef.Easting,
                        currentJourneyLeg.LegEnd.Location.GridRef.Northing);
                }

                // Interchange location
                if (LCA.ShowInterchange)
                {
                    if (previousJourneyLeg != null)
                    {
                        interchangeLocation.Text += string.Format("<span class=\"debug\"> napt[{0}] osgr[{1},{2}]</span>",
                            (previousJourneyLeg.LegEnd.Location.Naptan.Count > 0) ? previousJourneyLeg.LegEnd.Location.Naptan[0] : string.Empty,
                            previousJourneyLeg.LegEnd.Location.GridRef.Easting,
                            previousJourneyLeg.LegEnd.Location.GridRef.Northing);
                    }
                }

                // Leg duration and mode
                if (string.IsNullOrEmpty(legModeDetail.Text))
                {
                    double mins = currentJourneyLeg.Duration.TotalMinutes;

                    legModeDetail.Text += string.Format("<span class=\"debug\">{0}mins</span>",
                    Math.Round(mins, 2).ToString());
                }
                legModeDetail.Text += string.Format("<span class=\"debug\"><br />[{0}]</span>",
                    currentJourneyLeg.Mode.ToString());


                // Times
                if ((string.IsNullOrEmpty(legArriveTime.Text)) && (previousJourneyLeg != null))
                {
                    legArriveTime.Text = string.Format("<span class=\"debug\">[{0}]</span>",
                        previousJourneyLeg.EndTime.TimeOfDay.ToString());
                }

                if (string.IsNullOrEmpty(legDepartTime.Text))
                {
                    legDepartTime.Text = string.Format("<span class=\"debug\">[{0}]</span>",
                        currentJourneyLeg.StartTime.TimeOfDay.ToString());
                }

                // Instruction
                PublicJourneyDetail pjd = LCA.GetPublicJourneyDetail();
                if (pjd != null)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("<span class=\"debug\">");

                    sb.Append(string.Format(" duration[{0}s]", pjd.Duration));

                    if (!string.IsNullOrEmpty(pjd.Region))
                        sb.Append(string.Format(" region[{0}]", pjd.Region));

                    // Service Details
                    foreach (ServiceDetail sd in pjd.Services)
                    {
                        sb.Append("<br />");
                        sb.Append("service: ");
                        sb.Append(sd.ToString());
                    }

                    // Retailers
                    if (bookTicketAvailable)
                    {
                        foreach (Retailer r in retailers)
                        {
                            sb.Append("<br />");
                            sb.Append("retailer: ");
                            sb.Append(string.Format("id[{0}]", r.Id));
                            sb.Append(string.Format("name[{0}]", r.Name));
                        }
                    }

                    sb.Append("</span>");


                    legInstruction.Text += sb.ToString();
                }

                if (!mapDirectionsLinkAvailable || string.IsNullOrEmpty(directionsMapLink.NavigateUrl))
                {
                    MapHelper mapHelper = new MapHelper(Global.SJPResourceManager);

                    // Walk link goes to the Map (Google) page, displayed in new window
                    directionsMapLink.NavigateUrl = mapHelper.BuildMapURL(
                        MapPageURL(SJPModeType.Walk), journeyRequestHash, journeyId, journeyLegIndex, LCA.IsReturn);
                    directionsMapLink.Target = "_blank";
                    
                    // Add a space to the end to force a gap with the open in new window image
                    directionsMapLinkText.Text = TXT_DirectionsMapLink_Walk + " ";
                    directionsMapLinkText.ToolTip = TXT_DirectionsMapLinkToolTip_Walk;
                    
                    // Opens in new window, so display the new window image
                    openInNewWindow_MapLink.Visible = true;
                    openInNewWindow_MapLink.ImageUrl = URL_OpenInNewWindowImage;
                    openInNewWindow_MapLink.AlternateText = ALTTXT_OpenInNewWindow;
                    openInNewWindow_MapLink.ToolTip = TOOLTIP_OpenInNewWindow;

                    mapDirectionsLinkAvailable = true;

                    directionsMapLinkText.CssClass += " debug";
                }
            }
        }

        #endregion

        #endregion
    }
}
