// *********************************************** 
// NAME             : AcessibleOptionsControl.ascx.cs      
// AUTHOR           : David Lane
// DATE CREATED     : 10 Mar 2012
// DESCRIPTION  	: Accessible stops control
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.DataServices;
using SJP.Common.DataServices.NPTG;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;
using SJP.Common.PropertyManager;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SJPMobile.Controls
{
    #region Public Event Definition

    /// <summary>
    /// EventsArgs class for displaying messages
    /// </summary>
    public class DisplayMessageEventArgs : EventArgs
    {
        private SJPMessage message;

        /// <summary>
        /// Constructor
        /// </summary>
        public DisplayMessageEventArgs(SJPMessage message)
        {
            this.message = message;
        }

        /// <summary>
        /// SJPJourneyPlannerMode
        /// </summary>
        public SJPMessage Message
        {
            get { return message; }
        }
    }

    /// <summary>
    /// Delegate for displaying message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DisplayMessage(object sender, DisplayMessageEventArgs e);

    #endregion

    /// <summary>
    /// Accessible mobility options control
    /// </summary>
    public partial class AccessibleStopsControl : System.Web.UI.UserControl
    {
        private NPTGData nptgData = null;
        private SJPAccessiblePreferences accessiblePreferences = null;
        private ISJPJourneyRequest journeyRequest = null;
        private bool isForOriginLocation = true;
        private SJPJourneyPlannerMode plannerMode = SJPJourneyPlannerMode.PublicTransport;
        private SJPGNATLocation theLocation;

        #region Public Events

        public event PlanJourney OnPlanJourney;
        public event DisplayMessage OnDisplayMessage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Read only, the location (could be origin or destination)
        /// </summary>
        public SJPGNATLocation TheLocation
        {
            get { return theLocation; }
        }

        /// <summary>
        /// Read/Write. Determines the options and functionality enabled in this control
        /// </summary>
        public SJPJourneyPlannerMode PlannerMode
        {
            get { return plannerMode; }
            set
            {
                plannerMode = value;

                // SJP Mobile only supports PT and Cycle
                switch (plannerMode)
                {
                    case SJPJourneyPlannerMode.Cycle:
                    case SJPJourneyPlannerMode.PublicTransport:
                        break;
                    default:
                        plannerMode = SJPJourneyPlannerMode.PublicTransport;
                        break;
                }
            }
        }

        /// <summary>
        /// Read only. GNAT stop is origin
        /// </summary>
        public bool IsForOriginLocation
        {
            get { return isForOriginLocation; }
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            nptgData = SJPServiceDiscovery.Current.Get<NPTGData>(ServiceDiscoveryKey.NPTGData);
            SessionHelper sessionHelper = new SessionHelper();
            journeyRequest = sessionHelper.GetSJPJourneyRequest();
            accessiblePreferences = journeyRequest.AccessiblePreferences;

            // Determine if page is for the origin location (assume it is)
            isForOriginLocation = ((journeyRequest.Destination != null) && (journeyRequest.Destination.TypeOfLocation == SJPLocationType.Venue));

            SetupResources();

            if (!IsPostBack)
            {
                SetupStopTypeLists();

                PopulateCountryList(isForOriginLocation ? journeyRequest.Origin : journeyRequest.Destination);
            }
        }

        /// <summary>
        /// Page_PreRender 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// Drop Down selected index changed event handler for the DistrictList drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void District_Click(object sender, EventArgs e)
        {
            RefreshGNATList();
        }

        /// <summary>
        /// Event handler for stop type post back event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StopTypeList_Changed(object sender, EventArgs e)
        {
            RefreshGNATList();
        }

        /// <summary>
        /// Event handler for country post back event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Country_Click(object sender, EventArgs e)
        {
            RefreshAreaList(null);
            stopList.Items.Clear();
            stopList.Enabled = false;
        }

        /// <summary>
        /// Event handler for county post back event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void County_Click(object sender, EventArgs e)
        {
            RefreshDistrictList(null);
        }

        /// <summary>
        /// Event handler for stop post back event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Stop_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Event handler for the update stops button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void updateBtn_Click(object sender, EventArgs e)
        {
            RefreshGNATList();
        }

        /// <summary>
        /// Event handler for plan journey button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void planJourneyBtn_Click(object sender, EventArgs e)
        {
            // Set the selected GNAT location
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
            List<SJPGNATLocation> gnatStopList = locationService.GetGNATLocations().OrderBy(stop => stop.DisplayName).ToList();
            SJPGNATLocation selectedLocation = gnatStopList.SingleOrDefault(stop => stop.ID == stopList.SelectedValue.Trim());

            if (selectedLocation != null)
            {
                theLocation = selectedLocation;

                // Raise event to let page submit journey
                if (OnPlanJourney != null)
                {
                    OnPlanJourney(this, new PlanJourneyEventArgs(this.plannerMode));
                }
            }
            else
            {
                if (stopList.Items.Count > 1)
                {
                    // No stop selected
                    DisplayMessage(new SJPMessage("AccessiblilityOptions.NoStopSelected.Text", SJPMessageType.Error));
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            // Accessible stops headings
            accessibleStopsHeading.InnerText = page.GetResource("AccessibilityOptions.Heading.Text");
            if (accessiblePreferences.RequireStepFreeAccess && accessiblePreferences.RequireSpecialAssistance)
            {
                accessibleStopsInfo.Text = isForOriginLocation ?
                    page.GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Origin.Text") :
                    page.GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Destination.Text");
            }
            else if (accessiblePreferences.RequireStepFreeAccess)
            {
                accessibleStopsInfo.Text = isForOriginLocation ?
                    page.GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccess.Origin.Text") :
                    page.GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccess.Destination.Text");
            }
            else
            {
                accessibleStopsInfo.Text = isForOriginLocation ?
                    page.GetResource("AccessibilityOptions.Accessibility.RequireSpecialAssistance.Origin.Text") :
                    page.GetResource("AccessibilityOptions.Accessibility.RequireSpecialAssistance.Destination.Text");
            }

            // Stop types
            stopTypeHeading.InnerText = isForOriginLocation ?
                page.GetResource("AccessibilityOptions.StopTypeSelect.Origin.Text") :
                page.GetResource("AccessibilityOptions.StopTypeSelect.Destination.Text");

            // Countries
            countryHeading.InnerText = page.GetResource("AccessibilityOptions.LblCountry.Text");

            // Counties
            countyHeading.InnerText = page.GetResource("AccessibilityOptions.LblAdminArea.Text");

            // Districts
            districtHeading.InnerText = page.GetResource("AccessibilityOptions.LblDistrict.Text");

            // Stops
            stopHeading.InnerText = page.GetResource("AccessibilityOptions.JourneyFrom.Text");

            // Plan journey button
            planJourneyBtn.Text = Server.HtmlDecode(page.GetResourceMobile("JourneyInput.PlanJourney.Text"));
            planJourneyBtn.ToolTip = Server.HtmlDecode(page.GetResourceMobile("JourneyInput.PlanJourney.ToolTip"));
            planJourneyBtnNonJS.Text = Server.HtmlDecode(page.GetResourceMobile("JourneyInput.PlanJourney.Text"));
            planJourneyBtnNonJS.ToolTip = Server.HtmlDecode(page.GetResourceMobile("JourneyInput.PlanJourney.ToolTip"));

            // Update stops button
            updateBtnNonJS.Text = page.GetResource("AccessibilityOptions.Update.Text");
            updateBtnNonJS.ToolTip = page.GetResource("AccessibilityOptions.Update.ToolTip");
        }

        /// <summary>
        /// Sets up the GNAT stop type list
        /// </summary>
        private void SetupStopTypeLists()
        {
            stopTypeListLeft.Items.Clear();
            stopTypeListRight.Items.Clear();
            SJPPageMobile page = (SJPPageMobile)Page;

            ListItem railListItem = new ListItem();
            railListItem.Text = page.GetResource("AccessibilityOptions.GNATStopTypeList.Rail.Text");
            railListItem.Value = SJPGNATLocationType.Rail.ToString();
            railListItem.Selected = true;
            railListItem.Attributes["class"] = "accessibilityCheckbox";
            stopTypeListLeft.Items.Add(railListItem);

            ListItem ferryListItem = new ListItem();
            ferryListItem.Text = page.GetResource("AccessibilityOptions.GNATStopTypeList.Ferry.Text");
            ferryListItem.Value = SJPGNATLocationType.Ferry.ToString();
            ferryListItem.Selected = true;
            ferryListItem.Attributes["class"] = "accessibilityCheckbox";
            stopTypeListLeft.Items.Add(ferryListItem);

            ListItem undergroundListItem = new ListItem();
            undergroundListItem.Text = page.GetResource("AccessibilityOptions.GNATStopTypeList.Underground.Text");
            undergroundListItem.Value = SJPGNATLocationType.Underground.ToString();
            undergroundListItem.Selected = true;
            undergroundListItem.Attributes["class"] = "accessibilityCheckbox";
            stopTypeListLeft.Items.Add(undergroundListItem);

            ListItem dlrListItem = new ListItem();
            dlrListItem.Text = page.GetResource("AccessibilityOptions.GNATStopTypeList.DLR.Text");
            dlrListItem.Value = SJPGNATLocationType.DLR.ToString();
            dlrListItem.Selected = true;
            dlrListItem.Attributes["class"] = "accessibilityCheckbox";
            stopTypeListRight.Items.Add(dlrListItem);

            ListItem coachListItem = new ListItem();
            coachListItem.Text = page.GetResource("AccessibilityOptions.GNATStopTypeList.Coach.Text");
            coachListItem.Value = SJPGNATLocationType.Coach.ToString();
            coachListItem.Selected = true;
            coachListItem.Attributes["class"] = "accessibilityCheckbox";
            stopTypeListRight.Items.Add(coachListItem);

            ListItem tramListItem = new ListItem();
            tramListItem.Text = page.GetResource("AccessibilityOptions.GNATStopTypeList.Tram.Text");
            tramListItem.Value = SJPGNATLocationType.Tram.ToString();
            tramListItem.Selected = true;
            tramListItem.Attributes["class"] = "accessibilityCheckbox";
            stopTypeListRight.Items.Add(tramListItem);
        }

        
        /// <summary>
        /// Populates the country list
        /// </summary>
        private void PopulateCountryList(SJPLocation location)
        {
            // load regions drop down
            IDataServices dataServices = SJPServiceDiscovery.Current.Get<IDataServices>(ServiceDiscoveryKey.DataServices);

            dataServices.LoadListControl(DataServiceType.CountryDrop, countryList, Global.SJPResourceManager, CurrentLanguage.Value);

            // Set the selected value.
            // Assume if location is provided then set otherwise not
            if (location != null)
            {
                // Identify the country
                AdminArea adminArea = nptgData.GetAdminArea(location.AdminAreaCode);

                if ((adminArea != null) && (countryList.Items.Count > 1))
                {
                    try
                    {
                        countryList.SelectedValue = adminArea.CountryCode;
                    }
                    catch
                    {
                        // Ignore exception if value does not exist in list
                    }
                }
            }

            RefreshAreaList(location);
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            if (OnDisplayMessage != null)
            {
                OnDisplayMessage(this, new DisplayMessageEventArgs(sjpMessage));
            }
        }

        #region Refresh lists

        /// <summary>
        /// Refreshes the Admin area drop down list
        /// </summary>
        private void RefreshAreaList(SJPLocation location)
        {
            countyList.ClearSelection();

            if (!string.IsNullOrEmpty(countryList.SelectedValue))
            {
                countyList.DataSource = nptgData.GetAdminAreas(countryList.SelectedValue.Trim()).OrderBy(x => x.AreaName);
            }
            else
            {
                countyList.DataSource = nptgData.GetAllAdminAreas().OrderBy(x => x.AreaName);
            }
            countyList.DataTextField = "AreaName";
            countyList.DataValueField = "AdministrativeAreaCode";

            countyList.DataBind();

            string defaultItemText = ((SJPPageMobile)Page).GetResource("AccessibilityOptions.AdminAreaList.DefaultItem.Text");
            if (!string.IsNullOrEmpty(defaultItemText))
            {
                ListItem defaultItem = new ListItem(defaultItemText, "");
                defaultItem.Selected = true;
                countyList.Items.Insert(0, defaultItem);
            }

            // Set the selected value.
            // Assume if location is provided then set otherwise not
            if (location != null)
            {
                if (countyList.Items.Count > 1)
                {
                    try
                    {
                        countyList.SelectedValue = location.AdminAreaCode.ToString();
                    }
                    catch
                    {
                        // Ignore exception if value does not exist in list
                    }
                }
            }

            RefreshDistrictList(location);
        }

        /// <summary>
        /// Refreshes and populates the district list drop down
        /// </summary>
        private void RefreshDistrictList(SJPLocation location)
        {
            string adminAreaCode = countyList.SelectedValue;
            string londonAdminAreaCode = Properties.Current["AccessibilityOptions.DistrictList.AdminAreaCode.London"];

            districtList.ClearSelection();
            districtList.Items.Clear();

            district.Visible = false;
            districtList.Visible = false;

            if (Properties.Current["AccessibilityOptions.DistrictList.Visible.LondonOnly"].Parse(true) && adminAreaCode != londonAdminAreaCode)
            {
                RefreshGNATList();
                return;
            }

            if (!string.IsNullOrEmpty(countyList.SelectedValue))
            {
                districtList.DataSource = nptgData.GetDistricts(countyList.SelectedValue.Trim().Parse(0)).OrderBy(x => x.DistrictName);

                districtList.DataTextField = "DistrictName";
                districtList.DataValueField = "DistrictCode";

                districtList.DataBind();

                string defaultItemText = ((SJPPageMobile)Page).GetResource("AccessibilityOptions.DistrictList.DefaultItem.Text");
                if (!string.IsNullOrEmpty(defaultItemText))
                {
                    ListItem defaultItem = new ListItem(defaultItemText, "");
                    defaultItem.Selected = true;
                    districtList.Items.Insert(0, defaultItem);
                }

                // Set the selected value.
                // Assume if location is provided then set otherwise not
                if (location != null)
                {
                    if (districtList.Items.Count > 1)
                    {
                        try
                        {
                            districtList.SelectedValue = location.DistrictCode.ToString();
                        }
                        catch
                        {
                            // Ignore exception if value does not exist in list
                        }
                    }
                }

                district.Visible = true;
                districtList.Visible = true;
                RefreshGNATList();
            }
        }

        /// <summary>
        /// Refresh and populate the standard GNAT list which will be used in postback scenario
        /// </summary>
        private void RefreshGNATList()
        {
            bool noStopsFound = false;
            stopList.ClearSelection();

            if (!string.IsNullOrEmpty(countyList.SelectedValue))
            {
                List<SJPGNATLocation> gnatStopList = GetFilteredGNATList();

                stopList.DataSource = gnatStopList;
                stopList.DataTextField = "DisplayName";
                stopList.DataValueField = "ID";

                stopList.DataBind();

                stopList.Enabled = gnatStopList.Count > 0 && !string.IsNullOrEmpty(countyList.SelectedValue);

                if (gnatStopList.Count == 0)
                {
                    DisplayMessage(new SJPMessage("AccessibilityOptions.NoGNATStopFound.Text", SJPMessageType.Error));
                    noStopsFound = true;
                }
            }
            else
            {
                stopList.Enabled = false;
            }

            SJPPageMobile page = (SJPPageMobile)Page;
            string defaultItemText = page.GetResource("AccessibilityOptions.GNATList.DefaultItem.Text");

            if (noStopsFound)
            {
                defaultItemText = page.GetResource("AccessibilityOptions.GNATList.NoStopsFound.Text");
                noStopsFound = false;
            }

            if (!string.IsNullOrEmpty(defaultItemText))
            {
                ListItem defaultItem = new ListItem(defaultItemText, "");
                defaultItem.Selected = true;
                stopList.Items.Insert(0, defaultItem);
            }
        }

        /// <summary>
        /// Filters the GNAT stop list based on the user selection
        /// </summary>
        /// <returns></returns>
        private List<SJPGNATLocation> GetFilteredGNATList()
        {
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            // Get all the gnat locations
            List<SJPGNATLocation> gnatStopList = locationService.GetGNATLocations();

            List<SJPGNATLocationType> selectedStopType = new List<SJPGNATLocationType>();

            // Filter the user selected gnat type
            if (accessiblePreferences.RequireStepFreeAccess && accessiblePreferences.RequireSpecialAssistance)
            {
                // Select the stations where both step free access and assistance available
                gnatStopList = gnatStopList.Where(gnat => (gnat.StepFreeAccess == true && gnat.AssistanceAvailable == true)).ToList();
            }
            else if (accessiblePreferences.RequireStepFreeAccess)
            {
                // Select the stations where step free access
                gnatStopList = gnatStopList.Where(gnat => gnat.StepFreeAccess == true).ToList();
            }
            else
            {
                // Select the stations where assistance available
                gnatStopList = gnatStopList.Where(gnat => gnat.AssistanceAvailable == true).ToList();
            }


            // Filter the gnat list data by Stop types selected
            foreach (ListItem item in stopTypeListLeft.Items)
            {
                if (item.Selected)
                {
                    selectedStopType.Add((SJPGNATLocationType)Enum.Parse(typeof(SJPGNATLocationType), item.Value));
                }
            }

            foreach (ListItem item in stopTypeListRight.Items)
            {
                if (item.Selected)
                {
                    selectedStopType.Add((SJPGNATLocationType)Enum.Parse(typeof(SJPGNATLocationType), item.Value));
                }
            }

            gnatStopList = gnatStopList.FindAll(stop => selectedStopType.Contains(stop.GNATStopType));


            // Filter the gnat list by Country
            if (!string.IsNullOrEmpty(countryList.SelectedValue))
            {
                gnatStopList = gnatStopList.Where(gnat => gnat.CountryCode == countryList.SelectedValue.Trim()).ToList();
            }

            // Filter the gnat list by County (Admin Area)
            if (!string.IsNullOrEmpty(countyList.SelectedValue))
            {
                gnatStopList = gnatStopList.Where(gnat => gnat.AdminAreaCode == countyList.SelectedValue.Trim().Parse(0)).ToList();
            }

            // Filter the gnat list by Borough (District)
            if (districtList.Visible && !string.IsNullOrEmpty(districtList.SelectedValue))
            {
                gnatStopList = gnatStopList.Where(gnat => gnat.DistrictCode == districtList.SelectedValue.Trim().Parse(0)).ToList();
            }

            return gnatStopList;
        }

        #endregion

        #endregion
    }
}