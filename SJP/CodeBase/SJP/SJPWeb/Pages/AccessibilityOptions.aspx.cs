// *********************************************** 
// NAME             : AccessibilityOptions.aspx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 07 Jun 2011
// DESCRIPTION  	: The page represents the find nearest for GNAT stations
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.DataServices;
using SJP.Common.DataServices.NPTG;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SJPWeb.Adapters;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Pages
{
    /// <summary>
    /// Page to represent the find nearest for GNAT stations
    /// </summary>
    public partial class AccessibilityOptions : SJPPage
    {
        #region Variables
        private NPTGData nptgData = null;
        private SJPAccessiblePreferences accessiblePreferences= null;
        private ISJPJourneyRequest journeyRequest = null;
        private bool isForOriginLocation = true;

        private bool showDebug = false;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AccessibilityOptions()
            : base(Global.SJPResourceManager)
        {
            pageId = PageId.AccessibilityOptions;
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
            nptgData = SJPServiceDiscovery.Current.Get<NPTGData>(ServiceDiscoveryKey.NPTGData);
            SessionHelper sessionHelper = new SessionHelper();
            journeyRequest = sessionHelper.GetSJPJourneyRequest();
            accessiblePreferences = journeyRequest.AccessiblePreferences;

            // To show debug information when in debug mode
            showDebug = DebugHelper.ShowDebug;

            // Determine if page is for the origin location (assume it is)
            isForOriginLocation = ((journeyRequest.Destination != null) && (journeyRequest.Destination.TypeOfLocation == SJPLocationType.Venue));

            SetupResources();
            SetupJourneyInfo(journeyRequest);
                        
            if (!IsPostBack)
            {
                SetupStopTypeList();
                
                PopulateCountryList(isForOriginLocation ? journeyRequest.Origin : journeyRequest.Destination);
            }

            AddStyleSheet("jquery-ui-1.8.13.css");
            AddJavascript("jquery-ui-1.8.13.min.js");
            AddJavascript("Common.js");
            AddJavascript("AccessibilityOptions.js");
            AddJavascript("jquery.ui.selectmenu.js");
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupDebugInformation();
        }
        
        #endregion

        #region Controls Event Handlers
        /// <summary>
        /// Drop Down selected index changed event handler for the Countrylist drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CountryList_Changed(object sender, EventArgs e)
        {
            RefreshAreaList(null);
            // reset the districtList and gnatList
            districtRow.Visible = false;
            districtList.Visible = false;
            gnatList.Enabled = false;
            btnPlanJourney.Enabled = false;
        }

        /// <summary>
        /// Drop Down selected index changed event handler for the AdminAreaList drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminAreaList_Changed(object sender, EventArgs e)
        {
            btnPlanJourney.Enabled = false;
            RefreshDistrictList(null);
        }

        /// <summary>
        /// Drop Down selected index changed event handler for the AdminAreaList drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAdminAreaListGo_Click(object sender, EventArgs e)
        {
            RefreshDistrictList(null);
            btnPlanJourney.Enabled = gnatList.Enabled;
        }

        /// <summary>
        /// Drop Down selected index changed event handler for the DistrictList drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DistrictList_Changed(object sender, EventArgs e)
        {
            btnPlanJourney.Enabled = false;
            RefreshGNATList();
        }

        /// <summary>
        /// Drop Down selected index changed event handler for the DistrictList drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDistrictListGo_Click(object sender, EventArgs e)
        {
           RefreshGNATList();
           btnPlanJourney.Enabled = gnatList.Enabled;
        }

        /// <summary>
        /// Drop Down selected index changed event handler for the GNATList drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GNATList_Changed(object sender, EventArgs e)
        {
            btnPlanJourney.Enabled = !string.IsNullOrEmpty(gnatList.SelectedValue);
        }

        /// <summary>
        /// Drop Down selected index changed event handler for the GNATList drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnGNATListGo_Click(object sender, EventArgs e)
        {
            btnPlanJourney.Enabled = !string.IsNullOrEmpty(gnatList.SelectedValue);
        }

        /// <summary>
        /// Stop type check boxed changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StopTypeList_Changed(object sender, EventArgs e)
        {
            btnPlanJourney.Enabled = false;
            
            RefreshGNATList();
            
            if (!jsEnabled.Value.Parse(true))
            {
                btnPlanJourney.Enabled = gnatList.Enabled;
            }
        }
                
        /// <summary>
        /// Event handler for Back button click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            SetPageTransfer(PageId.JourneyPlannerInput);
        }

        /// <summary>
        /// Event handler for plan journey button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPlanJourney_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gnatList.SelectedValue))
            {
                SubmitRequest();
            }
            else
            {
                DisplayMessage(new SJPMessage("AccessiblilityOptions.NoStopSelected.Text", SJPMessageType.Error));
            }
        }

        #endregion

        #region Private Methods

        #region Refresh lists

        /// <summary>
        /// Refreshes the Admin area drop down list
        /// </summary>
        private void RefreshAreaList(SJPLocation location)
        {
            adminAreaList.ClearSelection();

            if (!string.IsNullOrEmpty(countryList.SelectedValue))
            {
                adminAreaList.DataSource = nptgData.GetAdminAreas(countryList.SelectedValue.Trim()).OrderBy(x=> x.AreaName);
            }
            else
            {
                adminAreaList.DataSource = nptgData.GetAllAdminAreas().OrderBy(x => x.AreaName);
            }
            adminAreaList.DataTextField = (showDebug) ? "AreaNameDebug" : "AreaName";
            adminAreaList.DataValueField = "AdministrativeAreaCode";

            adminAreaList.DataBind();

            string defaultItemText = GetResource("AccessibilityOptions.AdminAreaList.DefaultItem.Text");
            if(!string.IsNullOrEmpty(defaultItemText))
            {
                ListItem defaultItem = new ListItem(defaultItemText, "");
                defaultItem.Selected = true;
                adminAreaList.Items.Insert(0, defaultItem);
            }

            // Set the selected value.
            // Assume if location is provided then set otherwise not
            if (location != null)
            {
                if (adminAreaList.Items.Count > 1)
                {
                    try
                    {
                        adminAreaList.SelectedValue = location.AdminAreaCode.ToString();
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
            string adminAreaCode = adminAreaList.SelectedValue;
            string londonAdminAreaCode = Properties.Current["AccessibilityOptions.DistrictList.AdminAreaCode.London"];

            districtList.ClearSelection();
            districtList.Items.Clear();

            districtRow.Visible = false;
            districtList.Visible = false;

            if (Properties.Current["AccessibilityOptions.DistrictList.Visible.LondonOnly"].Parse(true) && adminAreaCode != londonAdminAreaCode)
            {
                RefreshGNATList();
                return;
            }

            if (!string.IsNullOrEmpty(adminAreaList.SelectedValue))
            {
                districtList.DataSource = nptgData.GetDistricts(adminAreaList.SelectedValue.Trim().Parse(0)).OrderBy(x => x.DistrictName);

                districtList.DataTextField = (showDebug) ? "DistrictNameDebug" : "DistrictName";
                districtList.DataValueField = "DistrictCode";

                districtList.DataBind();

                string defaultItemText = GetResource("AccessibilityOptions.DistrictList.DefaultItem.Text");
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

                districtRow.Visible = true;
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
            gnatList.ClearSelection();
                       
            if (!string.IsNullOrEmpty(adminAreaList.SelectedValue))
            {
                List<SJPGNATLocation> gnatStopList = GetFilteredGNATList();

                gnatList.DataSource = gnatStopList;
                gnatList.DataTextField = "DisplayName";
                gnatList.DataValueField = "ID";

                gnatList.DataBind();

                gnatList.Enabled = gnatStopList.Count > 0 && !string.IsNullOrEmpty(adminAreaList.SelectedValue);

                if (gnatStopList.Count == 0)
                {
                    DisplayMessage(new SJPMessage("AccessibilityOptions.NoGNATStopFound.Text", SJPMessageType.Error));
                    noStopsFound = true;
                }

                SetupDebugInformationGnatList(gnatStopList);
            }
            else
            {
                gnatList.Enabled = false;
            }

            string defaultItemText = GetResource("AccessibilityOptions.GNATList.DefaultItem.Text");

            if (noStopsFound)
            {
                defaultItemText = GetResource("AccessibilityOptions.GNATList.NoStopsFound.Text");
                noStopsFound = false;
            }

            if (!string.IsNullOrEmpty(defaultItemText))
            {
                ListItem defaultItem = new ListItem(defaultItemText, "");
                defaultItem.Selected = true;
                gnatList.Items.Insert(0, defaultItem);
            }
        }

        #endregion

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
            foreach (ListItem item in stopTypeList.Items)
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
            if (!string.IsNullOrEmpty(adminAreaList.SelectedValue))
            {
                gnatStopList = gnatStopList.Where(gnat => gnat.AdminAreaCode == adminAreaList.SelectedValue.Trim().Parse(0)).ToList();
            }

            // Filter the gnat list by Borough (District)
            if (districtList.Visible && !string.IsNullOrEmpty(districtList.SelectedValue))
            {
                gnatStopList = gnatStopList.Where(gnat => gnat.DistrictCode == districtList.SelectedValue.Trim().Parse(0)).ToList();
            }

            return gnatStopList;
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
        /// Setup resources i.e. text, etc. for the controls
        /// </summary>
        private void SetupResources()
        {
            lblFrom.Text = GetResource("AccessibilityOptions.LblFrom.Text");
            lblTo.Text = GetResource("AccessibilityOptions.LblTo.Text");
            lblDateTime.Text = GetResource("AccessibilityOptions.LblDateTime.Text");

            accessibilityPlanJourneyInfo.Text = GetResource("AccessibilityOptions.AccessibilityPlanJourneyInfo.Text");
            stopTypeSelect.Text = isForOriginLocation ?
                GetResource("AccessibilityOptions.StopTypeSelect.Origin.Text") :
                GetResource("AccessibilityOptions.StopTypeSelect.Destination.Text");
            stopSelectInfo.Text = isForOriginLocation ?
                GetResource("AccessibilityOptions.StopSelectInfo.Origin.Text") :
                GetResource("AccessibilityOptions.StopSelectInfo.Destination.Text");

            lblCountry.Text = GetResource("AccessibilityOptions.LblCountry.Text");
            journeyFrom.Text = GetResource("AccessibilityOptions.JourneyFrom.Text");
            lblAdminArea.Text = GetResource("AccessibilityOptions.LblAdminArea.Text");
            lblDistrict.Text = GetResource("AccessibilityOptions.LblDistrict.Text");

            lblMapInfo.Text = isForOriginLocation ?
                GetResource("AccessibilityOptions.LblMapInfo.Origin.Text") :
                GetResource("AccessibilityOptions.LblMapInfo.Destination.Text");

            btnAdminAreaListGo.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnAdminAreaListGo.Text"));
            btnAdminAreaListGo.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnAdminAreaListGo.ToolTip"));
            btnCountryListGo.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnCountryListGo.Text"));
            btnCountryListGo.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnCountryListGo.ToolTip"));
            btnDistrictListGo.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnDistrictListGo.Text"));
            btnDistrictListGo.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnDistrictListGo.ToolTip"));
            btnStopTypeListGo.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnStopTypeListGo.Text"));
            btnStopTypeListGo.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnStopTypeListGo.ToolTip"));
            btnGNATListGo.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnGNATListGo.Text"));
            btnGNATListGo.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.BtnGNATListGo.ToolTip"));

            btnBack.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.Back.Text"));
            btnBack.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.Back.ToolTip"));
            btnPlanJourney.Text = Server.HtmlDecode(GetResource("AccessibilityOptions.PlanJourney.Text"));
            btnPlanJourney.ToolTip = Server.HtmlDecode(GetResource("AccessibilityOptions.PlanJourney.ToolTip"));

            if (accessiblePreferences.RequireStepFreeAccess && accessiblePreferences.RequireSpecialAssistance)
            {
                lblRequestJourney.Text = GetResource("AccessibilityOptions.LblRequestJourney.RequireStepFreeAccessAndAssistance.Text");
                lblAccessibility.Text = isForOriginLocation ?
                    GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Origin.Text") :
                    GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccessAndAssistance.Destination.Text");
            }
            else if (accessiblePreferences.RequireStepFreeAccess)
            {
                lblRequestJourney.Text = GetResource("AccessibilityOptions.LblRequestJourney.RequireStepFreeAccess.Text");
                lblAccessibility.Text = isForOriginLocation ?
                    GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccess.Origin.Text") :
                    GetResource("AccessibilityOptions.Accessibility.RequireStepFreeAccess.Destination.Text");
            }
            else
            {
                lblRequestJourney.Text = GetResource("AccessibilityOptions.LblRequestJourney.RequireSpecialAssistance.Text");
                lblAccessibility.Text = isForOriginLocation ?
                    GetResource("AccessibilityOptions.Accessibility.RequireSpecialAssistance.Origin.Text") :
                    GetResource("AccessibilityOptions.Accessibility.RequireSpecialAssistance.Destination.Text");
            }
        }

        /// <summary>
        /// Sets up the journey information 
        /// </summary>
        private void SetupJourneyInfo(ISJPJourneyRequest journeyRequest)
        {
            from.Text = journeyRequest.Origin.DisplayName;
            to.Text = journeyRequest.Destination.DisplayName;
            journeyDateTime.Text = journeyRequest.OutwardDateTime.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Sets up the GNAT stop type list
        /// </summary>
        private void SetupStopTypeList()
        {
            stopTypeList.Items.Clear();

            ListItem railListItem = new ListItem();
            railListItem.Text = GetResource("AccessibilityOptions.GNATStopTypeList.Rail.Text");
            railListItem.Value = SJPGNATLocationType.Rail.ToString();
            railListItem.Selected = true;
            stopTypeList.Items.Add(railListItem);
            
            ListItem ferryListItem = new ListItem();
            ferryListItem.Text = GetResource("AccessibilityOptions.GNATStopTypeList.Ferry.Text");
            ferryListItem.Value = SJPGNATLocationType.Ferry.ToString();
            ferryListItem.Selected = true;
            stopTypeList.Items.Add(ferryListItem);

            ListItem undergroundListItem = new ListItem();
            undergroundListItem.Text = GetResource("AccessibilityOptions.GNATStopTypeList.Underground.Text");
            undergroundListItem.Value = SJPGNATLocationType.Underground.ToString();
            undergroundListItem.Selected = true;
            stopTypeList.Items.Add(undergroundListItem);

            ListItem dlrListItem = new ListItem();
            dlrListItem.Text = GetResource("AccessibilityOptions.GNATStopTypeList.DLR.Text");
            dlrListItem.Value = SJPGNATLocationType.DLR.ToString();
            dlrListItem.Selected = true;
            stopTypeList.Items.Add(dlrListItem);

            ListItem coachListItem = new ListItem();
            coachListItem.Text = GetResource("AccessibilityOptions.GNATStopTypeList.Coach.Text");
            coachListItem.Value = SJPGNATLocationType.Coach.ToString();
            coachListItem.Selected = true;
            stopTypeList.Items.Add(coachListItem);

            ListItem tramListItem = new ListItem();
            tramListItem.Text = GetResource("AccessibilityOptions.GNATStopTypeList.Tram.Text");
            tramListItem.Value = SJPGNATLocationType.Tram.ToString();
            tramListItem.Selected = true;
            stopTypeList.Items.Add(tramListItem);
        }

        /// <summary>
        /// Displays a message
        /// </summary>
        private void DisplayMessage(SJPMessage sjpMessage)
        {
            ((SJPWeb)this.Master).DisplayMessage(sjpMessage);

            // Display the message seperator div (just a line seperator image)
            messageSeprator.Visible = true;
        }

        /// <summary>
        /// Submits the request.
        /// </summary>
        private void SubmitRequest()
        {
            JourneyPlannerInputAdapter adapter = new JourneyPlannerInputAdapter();

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            List<SJPGNATLocation> gnatStopList = locationService.GetGNATLocations().OrderBy(stop => stop.DisplayName).ToList();

            SJPGNATLocation selectedLocation = gnatStopList.SingleOrDefault(stop => stop.ID == gnatList.SelectedValue.Trim());
            

            if (journeyRequest != null && selectedLocation != null)
            {
                bool validRequest = false;
                                
                validRequest = adapter.ValidateAndUpdateSJPRequestForAccessiblePublicTransport(selectedLocation);

                JourneyPlannerHelper journeyPlannerHelper = new JourneyPlannerHelper();

                if (validRequest && journeyPlannerHelper.SubmitRequest(SJPJourneyPlannerMode.PublicTransport, true))
                {
                    // Set transfer to Journey Options page
                    SetPageTransfer(PageId.JourneyOptions);

                    // Set the query string values for the JourneyOptions page,
                    // this allows the result for the correct request to be loaded
                    AddQueryStringForPage(PageId.JourneyOptions);
                }
                else
                {
                    DisplayMessage(new SJPMessage("AccessiblilityOptions.ValidationError.Text", SJPMessageType.Error));
                }
            }
        }

        #region Debug

        /// <summary>
        /// Adds debug information to page (where possible)
        /// </summary>
        private void SetupDebugInformation()
        {
            if (DebugHelper.ShowDebug)
            {
                // Journey request
                if (journeyRequest != null)
                {
                    from.Text += string.Format("<span class=\"debug\"> ad[{0}] dis[{1}]</span>", 
                        journeyRequest.Origin.AdminAreaCode,
                        journeyRequest.Origin.DistrictCode);

                    to.Text += string.Format("<span class=\"debug\"> ad[{0}] dis[{1}]</span>",
                        journeyRequest.Destination.AdminAreaCode,
                        journeyRequest.Destination.DistrictCode);
                }

                // Selected values
                if (!string.IsNullOrEmpty(countryList.SelectedValue))
                    lblCountry.Text += string.Format("<span class=\"debug\"> country[{0}]</span>", countryList.SelectedValue);

                if (!string.IsNullOrEmpty(adminAreaList.SelectedValue))
                    lblAdminArea.Text += string.Format("<span class=\"debug\"> adminarea[{0}]</span>", adminAreaList.SelectedValue);

                if (districtList.Visible && !string.IsNullOrEmpty(districtList.SelectedValue))
                    lblDistrict.Text += string.Format("<span class=\"debug\"> district[{0}]</span>", districtList.SelectedValue);

                if (!string.IsNullOrEmpty(gnatList.SelectedValue))
                    journeyFrom.Text += string.Format("<span class=\"debug\"> id[{0}]</span>", gnatList.SelectedValue);
            }
        }

        /// <summary>
        /// Adds debug information to page (where possible)
        /// </summary>
        private void SetupDebugInformationGnatList(List<SJPGNATLocation> gnatStopList)
        {
            if (DebugHelper.ShowDebug)
            {
                // Gnat List
                if (gnatStopList != null && gnatStopList.Count > 0)
                {
                    debugGnatListRptr.DataSource = gnatStopList;
                    debugGnatListRptr.DataBind();

                    debugInfoDiv.Visible = true;
                }
                else
                {
                    debugInfoDiv.Visible = false;
                }

            }
        }

        /// <summary>
        /// GnatListRptr_DataBound repeater data bound event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GnatListRptr_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SJPGNATLocation location = e.Item.DataItem as SJPGNATLocation;

                HtmlGenericControl debugGnatStation = (HtmlGenericControl)e.Item.FindControl("debugGnatStation");

                if (debugGnatStation != null)
                {
                    string naptans = 

                    debugGnatStation.InnerHtml = string.Format("<span class=\"debug\">n[{0}] name[{1}] t[{2}] op[{3}] ad[{4}] dis[{5}] w[{6}] a[{7}]</span>",
                        location.Naptan.FirstOrDefault(),
                        location.DisplayName,
                        location.GNATStopType,
                        location.OperatorCode,
                        location.AdminAreaCode,
                        location.DistrictCode,
                        (location.StepFreeAccess) ? "1" : "0",
                        (location.AssistanceAvailable) ? "1" : "0");
                }
            }
        }

        #endregion

        #endregion
    }
}