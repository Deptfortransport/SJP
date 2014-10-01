// *********************************************** 
// NAME             : LocationControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Feb 2012
// DESCRIPTION  	: Location User control
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPMobile.Controls
{
    #region Public Event Definition

    /// <summary>
    /// Delegate for Toggle location button click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ToggleLocation(object sender, EventArgs e);

    #endregion

    /// <summary>
    /// Location user control
    /// </summary>
    public partial class LocationControl : System.Web.UI.UserControl
    {
        #region Public Events

        public event ToggleLocation OnToggleLocation;

        #endregion

        #region Private Fields

        private SJPLocationType locationType = SJPLocationType.Unknown;
        private LocationStatus status = LocationStatus.Unspecified;
        private SJPLocation location = null;
        private LocationService locationService = null;
        private bool resolveLocation = true;
        private bool? isVenueOnly = null;
        
        private bool addInvalidStyle = false;

        private string validationMessage = string.Empty;

        // Used when toggling locations.
        // Must match string javascript toggle locations method
        private const string updatingString = "Updating...";
        private const string mylocationString = "My Location";

        #region Resource Strings

        private string ambiguousText = string.Empty;
        private string invalidPostcodeText = string.Empty;
        private string noLocationFoundText = string.Empty;
        private string noLocationUKFoundText = string.Empty;
        private string noLocationLocalityFoundText = string.Empty;
        private string chooseVenueLocationText = string.Empty;
        private string locationDropDownDefaultItem = string.Empty;
        private string venueDropDownDefaultItem = string.Empty;

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write property indicates if the location should be resolved when Validate is called.
        /// This is to allow a pre-resolved location set during a landing page to bypass resolution when
        /// autoplan is in effect (as the control will not have gone through its life cycle and therefore not 
        /// fully setup).
        /// Default is true.
        /// </summary>
        public bool ResolveLocation
        {
            get { return resolveLocation; }
            set { resolveLocation = value; }
        }

        /// <summary>
        /// Get/Sets the SJPLocation represented by the location control
        /// </summary>
        public SJPLocation Location
        {
            get { return location; }
            set
            {
                location = value;
                if (location != null)
                {
                    status = LocationStatus.Resolved;
                    locationType = location.TypeOfLocation;
                }
            }
        }

        /// <summary>
        /// Read only property determining status fo the location control
        /// </summary>
        public LocationStatus Status
        {
            get {

                // If ambiguous and no amibiguous locations found then let controls know 
                // location is not resolved
                if ((status == LocationStatus.Ambiguous)
                    && (ambiguityDropdown.Items.Count == 0))
                {
                    return LocationStatus.Unspecified;
                }
                else
                {
                    return status;
                }
            }
        }

        /// <summary>
        /// Read/Write property determining type of location represented by location control
        /// </summary>
        public SJPLocationType TypeOfLocation
        {
            get
            {
                // Location type could have been updated by js
                if (IsPostBack
                    && (locationInput_hdnType.Value != string.Empty)
                    && !ambiguityRow.Visible
                    && (locationType == SJPLocationType.Unknown))
                {
                    locationType = (SJPLocationType)Enum.Parse(typeof(SJPLocationType), locationInput_hdnType.Value);
                }
                // For non-js users, when venue dropdown is used that indicates its a venue location type
                else if (IsPostBack
                    && (!(jsEnabled.Value.Parse(true)))
                    && (venueDropdownNonJS.Items.Count > 0)
                    && (venueDropdownNonJS.Visible))
                {
                    locationType = SJPLocationType.Venue;
                }

                return locationType;
            }
            set { locationType = value; }
        }

        /// <summary>
        /// Read/Write. Label shown against location input
        /// </summary>
        public Label LocationDirectionLabel
        {
            get { return locationDirectionLbl; }
            set { locationDirectionLbl = value; }
        }

        /// <summary>
        /// Read/Write. TextBox for location input
        /// </summary>
        public TextBox LocationInputTextBox
        {
            get { return locationInput; }
            set { locationInput = value; }
        }

        /// <summary>
        /// Sets this control so that it only allows venues to be selected
        /// </summary>
        public bool IsVenueOnly
        {
            get 
            {
                if (!isVenueOnly.HasValue)
                {
                    isVenueOnly = true;

                    // If current location button is shown,
                    // or ambiguity is shown (venues should never be ambiguous),
                    // then is not a venue only
                    if (currentLocationDiv.Visible || ambiguityRow.Visible)
                    {
                        isVenueOnly = false;
                    }
                }

                return isVenueOnly.Value;
            }
            set { isVenueOnly = value; }
        }

        /// <summary>
        /// Read only. Contains any error or validation message returned by the location resolution proces
        /// </summary>
        public string ValidationMessage
        {
            get { return validationMessage; }
        }

        #endregion

        #region Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Init
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupResources();
            RegisterJavascripts();
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowHideControls();

            PopulateVenueDropDown();

            if (IsVenueOnly)
            {
                SetForVenueLocations();
            }
            else
            {
                SetForInputLocation();
            }

            SetupResourcesForControls();

            SetInvalidStyle();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Reset button event handler
        /// Resets the location in the event of reset button clicked when ambiguity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ResetLocation(object sender, EventArgs e)
        {
            Reset(locationType);
        }

        /// <summary>
        /// Event handler for the toggle locations click
        /// </summary>
        protected void TravelFromToToggle_Click(object sender, EventArgs e)
        {
            if (OnToggleLocation != null)
            {
                OnToggleLocation(this, null);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialises the location control
        /// </summary>
        /// <param name="searchString">Location to be search</param>
        /// <param name="locationType">Type of location</param>
        public void Initialise(string searchString, SJPLocationType locationType)
        {
            // Reset search string if it matches the "updating" string
            if (IsIgnoreSearchString(searchString))
            {
                searchString = string.Empty;
            }

            locationInput.Text = searchString;
            locationInputNonJS.Text = searchString;
            this.locationType = locationType;

            Validate();
        }

        /// <summary>
        /// Validates the location control and determines the current status of it
        /// </summary>
        /// <returns>True if the location gets resolved</returns>
        public bool Validate()
        {
            return Validate(TypeOfLocation);
        }

        /// <summary>
        /// Validates the location control and determines the current status of it
        /// </summary>
        /// <returns>True if the location gets resolved</returns>
        public bool Validate(SJPLocationType locationType)
        {
            bool valid = false;

            LocationHelper locationHelper = new LocationHelper();

            if (locationType == SJPLocationType.Venue)
            {
                #region Validate Venue location

                if (resolveLocation)
                {
                    if (locationInput_hdnValue.Value != "" && (jsEnabled.Value.Parse(true)))
                    {
                        location = locationHelper.GetSJPLocation(locationInput_hdnValue.Value, string.Empty, SJPLocationType.Venue);

                        status = LocationStatus.Resolved;
                        valid = true;
                    }
                    else if (venueDropdownNonJS.Items.Count > 0 && (!jsEnabled.Value.Parse(true)))
                    {
                        if (venueDropdownNonJS.SelectedIndex == 0)
                        {
                            valid = false;
                        }
                        else
                        {
                            if (locationService != null)
                            {
                                location = locationService.GetSJPLocation(venueDropdownNonJS.SelectedValue, SJPLocationType.Venue);

                                status = LocationStatus.Resolved;
                                valid = true;
                            }
                        }
                    }
                }
                else
                {
                    // Location was supplied to control and was considered resolved, so check it is
                    if (IsValidLocation(location))
                    {
                        status = LocationStatus.Resolved;
                        valid = true;
                    }
                }

                #endregion
            }
            else
            {
                #region Validate location

                SJPLocationType searchType = SJPLocationType.Unknown;

                if (resolveLocation)
                {
                    // Reset location text if it matches the "updating" string
                    if (IsIgnoreSearchString(locationInput.Text.Trim()))
                    {
                        locationInput.Text = string.Empty;
                        locationInputNonJS.Text = string.Empty;
                    }

                    // Read js location input first
                    string searchString = locationInput.Text.Trim();

                    if (!jsEnabled.Value.Parse(true))
                    {
                        // If js disabled, then read from the non-js location input
                        searchString = locationInputNonJS.Text.Trim();
                    }

                    string searchId = locationInput_hdnValue.Value.Trim();
                    searchType = searchString.IsValidPostcode() ? SJPLocationType.Postcode : SJPLocationType.Unknown;

                    // if location is set from javascript dropdown (or used the current location button)
                    if (!string.IsNullOrEmpty(locationInput_hdnType.Value.Trim()) &&
                        !string.IsNullOrEmpty(searchId)
                        && jsEnabled.Value.Parse(true))
                    {
                        // Any errors parsing, allow application to capture as that would indicate javascript may have been tampered with
                        searchType = (SJPLocationType)Enum.Parse(typeof(SJPLocationType), locationInput_hdnType.Value.Trim());

                        // Fix for toggle locations when My location was selected but was not resolved (page was not postback before toggle selected)
                        if ((searchType == SJPLocationType.CoordinateLL || (searchType == SJPLocationType.CoordinateEN))
                            && string.IsNullOrEmpty(searchString))
                        {
                            searchString = mylocationString;
                        }

                        location = locationHelper.GetSJPLocation(searchId, searchString, searchType);
                    }
                    // if user input the location name without picking from list (or javascript disabled)
                    else if (!ambiguityRow.Visible)
                    {
                        location = locationHelper.GetSJPLocation(searchString, string.Empty, searchType);
                    }
                    else // if user is selecting location from ambiguity state
                    {
                        if (ambiguityDropdown.SelectedValue.Trim() != "-1")
                        {
                            string[] locationIds = ambiguityDropdown.SelectedValue.Split(new char[] { ':' });
                            if (locationIds.Length == 2)
                            {
                                searchType = (SJPLocationType)Enum.Parse(typeof(SJPLocationType), locationIds[1].Trim());

                                location = locationHelper.GetSJPLocation(locationIds[0].Trim(), string.Empty, searchType);
                            }
                        }
                    }
                }

                // Check location is valid
                if (IsValidLocation(location))
                {
                    status = LocationStatus.Resolved;
                    valid = true;
                }
                else
                {
                    status = LocationStatus.Ambiguous;

                    // If coordinate location and if no location was found for it, 
                    // then assume it's not able to be resolved.
                    if (searchType == SJPLocationType.CoordinateLL)
                    {
                        // Assume couldnt resolve the coordiante to an easting northing
                        validationMessage = noLocationUKFoundText;
                    }
                    else if (searchType == SJPLocationType.CoordinateEN)
                    {
                        // Assume couldnt resolve a locality for the coordinate
                        validationMessage = noLocationLocalityFoundText;
                    }
                    else if (IsVenueOnly)
                    {
                        // Is in unknown mode for venue only input and a venue wasnt selected
                        validationMessage = chooseVenueLocationText;
                    }

                    SetForAmbiguousLocation();
                }

                #endregion
            }

            // Let control know invalid css style should be applied to the input
            this.addInvalidStyle = !valid;

            return valid;
        }

        /// <summary>
        /// Reset location to type of location specified
        /// </summary>
        /// <param name="typeOfLocation">Type of location</param>
        public void Reset(SJPLocationType typeOfLocation)
        {
            locationType = typeOfLocation;
    
            PopulateVenueDropDown();
            status = LocationStatus.Unspecified;
            location = null;
            locationInput.Visible = true;
            locationInputNonJS.Visible = true;
            locationInput.Text = string.Empty;
            locationInputNonJS.Text = string.Empty;
            locationInput_hdnValue.Visible = true;
            locationInput_hdnValue.Value = string.Empty;
            locationInput_hdnType.Visible = true;
            locationInput_hdnType.Value = string.Empty;
            locationDescriptionDiv.Visible = true;
            venueInputPageLnk.Visible = true;
            venueSelectControl.Visible = true;
            venueDropdownNonJS.Visible = false;
            currentLocationDiv.Visible = true;
            ambiguityRow.Visible = false;
            resetButton.Visible = false;
            resetButtonNonJS.Visible = false;

            if (typeOfLocation == SJPLocationType.Venue)
            {
                isVenueOnly = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Show/Hides controls based on various state of the location control
        /// </summary>
        private void ShowHideControls()
        {
            if (IsVenueOnly)
            {
                currentLocationDiv.Visible = false;
                ambiguityRow.Visible = false;
                resetButton.Visible = false;
                resetButtonNonJS.Visible = false;
                venueDropdownNonJS.Visible = true && (!jsEnabled.Value.Parse(true)); // Only shown for non-js users
                locationInputNonJS.Visible = false;
            }
            else
            {
                bool ambiguity = false;

                // If ambiguous and amibiguous locations found, display them,
                // otherwise continue to display the input
                if ((status == LocationStatus.Ambiguous)
                    && (ambiguityDropdown.Items.Count > 0))
                {
                    ambiguity = true;
                }
                
                ambiguityRow.Visible = ambiguity;
                resetButton.Visible = ambiguity;
                resetButtonNonJS.Visible = ambiguity;

                locationInput.Visible = !ambiguity;
                locationInputNonJS.Visible = !ambiguity;
                currentLocationDiv.Visible = !ambiguity;
                locationInput_hdnValue.Visible = !ambiguity;
                locationInput_hdnType.Visible = !ambiguity;
                locationDescriptionDiv.Visible = !ambiguity;
                venueInputPageLnk.Visible = !ambiguity;
                venueSelectControl.Visible = !ambiguity;
                venueDropdownNonJS.Visible = false;
            }

            locationDirectionLbl.AssociatedControlID = (ambiguityRow.Visible) ?
                ambiguityDropdown.ID : locationInput.ID;

        }

        /// <summary>
        /// Checks for valid location attributes
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private bool IsValidLocation(SJPLocation location)
        {
            return (location != null
                        && !String.IsNullOrEmpty(location.DisplayName)
                        && (!string.IsNullOrEmpty(location.Locality)
                            || location.Naptan.Count > 0
                            || (location.GridRef.Easting > 0 && location.GridRef.Northing > 0)));
        }

        /// <summary>
        /// Sets location for Input 
        /// </summary>
        private void SetForInputLocation()
        {
            if (status == LocationStatus.Resolved)
            {
                locationInput.Text = location.DisplayName;
                locationInputNonJS.Text = location.DisplayName;
            }

            if (status != LocationStatus.Ambiguous && location != null)
            {
                // Set the hidden location type and value settings 
                // check if js enabled on postback 
                if (!IsPostBack || jsEnabled.Value.Parse(true))
                {
                    locationInput_hdnValue.Value = location.ID;
                    locationInput_hdnType.Value = location.TypeOfLocation.ToString();
                }
            }
            
            if (!locationInput.CssClass.Contains("locationBox"))
            {
                locationInput.CssClass += " locationBox";
            }
        }

        /// <summary>
        /// Sets location for ambiguity situation
        /// </summary>
        private void SetForAmbiguousLocation()
        {
            #region Read search string from location input

            // Reset location text if it matches the "updating" string
            if (IsIgnoreSearchString(locationInput.Text.Trim()))
            {
                locationInput.Text = string.Empty;
            }

            // Read js location in put first
            string searchString = locationInput.Text.Trim();

            if (!jsEnabled.Value.Parse(true))
            {
                // If js disabled, then read from the non-js location input
                searchString = locationInputNonJS.Text.Trim();
            }

            // Reset search string if it matches the "updating" string
            if (IsIgnoreSearchString(searchString))
            {
                searchString = string.Empty;
            }

            #endregion

            ambiguityDropdown.Items.Clear();
            ambiguityDropdown.Visible = false;

            bool isDebug = DebugHelper.ShowDebug;

            if (!IsPossiblePostCode(searchString))
            {
                // Only perform ambiguity search if user has entered text
                if (!string.IsNullOrEmpty(searchString))
                {
                    List<SJPLocation> alternateLocations = locationService.GetAlternateLocations(searchString);

                    if (alternateLocations.Count > 0)
                    {
                        validationMessage = string.Format(ambiguousText, searchString);

                        if (isDebug)
                        {
                            validationMessage += string.Format("<br /><span class=\"debug\">Total[{0}]<br />Grp[{1}] Rail[{2}] Loc[{3}] Coach[{4}] TMU[{5}] Ferry[{6}] Air[{7}] Pcde[{8}] Other[{9}]</span>",
                                alternateLocations.Count,
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationGroup; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationRail; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.Locality; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationCoach; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationTMU; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationFerry; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationAirport; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.Postcode; }),
                                alternateLocations.Count(delegate(SJPLocation loc)
                            {
                                return (loc.TypeOfLocationActual == SJPLocationType.Unknown)
                                       || (loc.TypeOfLocationActual == SJPLocationType.Venue)
                                       || (loc.TypeOfLocationActual == SJPLocationType.Station);
                            })
                                );
                        }

                        foreach (SJPLocation alterlocation in alternateLocations)
                        {
                            string displayName = alterlocation.DisplayName;

                            if (isDebug)
                            {
                                displayName += string.Format(" - t[{0}] id[{1}]",
                                    alterlocation.TypeOfLocationActual.ToString(),
                                    alterlocation.ID);
                            }

                            ListItem alterLocationItem = new ListItem(displayName, string.Format("{0}:{1}", alterlocation.ID, alterlocation.TypeOfLocation));
                            ambiguityDropdown.Items.Add(alterLocationItem);
                        }

                        ambiguityDropdown.Items.Insert(0, new ListItem(locationDropDownDefaultItem, "-1"));

                        ambiguityDropdown.Visible = true;
                    }
                    else
                    {
                        validationMessage = noLocationFoundText;
                    }
                }
                else
                {
                    // In case validation message was set already by the Valiate method
                    if (string.IsNullOrEmpty(validationMessage))
                        validationMessage = noLocationFoundText;
                }
            }
            else
            {
                validationMessage = invalidPostcodeText;
            }
        }

        /// <summary>
        /// Checks if the search string is possible postcode
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private bool IsPossiblePostCode(string search)
        {
            return search.IsValidPostcode();
        }

        /// <summary>
        /// Checks if the search string is an ignore string, e.g. an updating, or a watermark
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private bool IsIgnoreSearchString(string search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    if ((search.Equals(updatingString)) || (search.Equals(mylocationString)))
                    {
                        return true;
                    }
                    else
                    {
                        SJPPageMobile page = (SJPPageMobile)Page;

                        if (search.Equals(page.GetResourceMobile("JourneyInput.Location.From.Watermark")))
                        {
                            return true;
                        }
                        else if (search.Equals(page.GetResourceMobile("JourneyInput.Location.To.Watermark")))
                        {
                            return true;
                        }
                        else if (search.Equals(page.GetResource("LocationControl.LocationInput.Tooltip.Venue")))
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                // Ignore exceptions, only checking a string
            }

            return false;
        }

        /// <summary>
        /// Sets location control to show venue dropdowns
        /// </summary>
        private void SetForVenueLocations()
        {
            if (location != null && status == LocationStatus.Resolved)
            {
                venueSelectControl.List.SelectedIndex = venueSelectControl.List.Items.IndexOf(venueSelectControl.List.Items.FindByValue(location.ID.Trim()));

                if (venueDropdownNonJS.Items.Count > 0)
                {
                    venueDropdownNonJS.SelectedIndex = venueDropdownNonJS.Items.IndexOf(venueDropdownNonJS.Items.FindByValue(location.ID.Trim()));
                }

                locationInput.Text = location.DisplayName;
                locationInputNonJS.Text = location.DisplayName;
                locationInput_hdnValue.Value = location.ID;
                locationInput_hdnType.Value = location.TypeOfLocation.ToString();
            }
        }

        /// <summary>
        /// Adds the invalid styling to the input control(s)
        /// </summary>
        private void SetInvalidStyle()
        {
            string invalidStyle = "locationError";

            if ((addInvalidStyle) && (!IsValidLocation(Location)))
            {
                // Input text box
                if (!locationInput.CssClass.Contains(invalidStyle))
                {
                    locationInput.CssClass += " " + invalidStyle;
                }

                // Dropdown if shown for ambiguous
                if (!ambiguityDropdown.CssClass.Contains(invalidStyle))
                {
                    ambiguityDropdown.CssClass += " " + invalidStyle;
                }
            }
            else
            {
                // Input text box
                if (locationInput.CssClass.Contains(invalidStyle))
                {
                    locationInput.CssClass = locationInput.CssClass.Replace(invalidStyle, string.Empty);
                }

                // Dropdown if shown for ambiguous
                if (!ambiguityDropdown.CssClass.Contains(invalidStyle))
                {
                    ambiguityDropdown.CssClass = ambiguityDropdown.CssClass.Replace(invalidStyle, string.Empty);
                }
            }
        }

        /// <summary>
        /// Populates the Venue dropdown
        /// </summary>
        private void PopulateVenueDropDown()
        {
            // Ensures venue dropdown is loaded during the page lifecycle only 
            // (e.g. this method could be called during a Reset call before control is ready) 
            venueSelectControl.Populate(locationService);


            if ((!jsEnabled.Value.Parse(true)) && (locationService != null))
            {
                List<SJPLocation> venues = locationService.GetSJPVenueLocations();

                venueDropdownNonJS.Items.Clear();

                if (Properties.Current["LocationControl.VenueGrouping.Switch"].Parse(true))
                {
                    Dictionary<string, List<SJPVenueLocation>> groupedVenues = new Dictionary<string, List<SJPVenueLocation>>();

                    // To group items for which there is no group specified.
                    // These items will be shown last in the list
                    string defaultGroupName = "NOGROUP";

                    groupedVenues.Add(defaultGroupName, new List<SJPVenueLocation>());

                    foreach (SJPLocation loc in venues)
                    {
                        SJPVenueLocation venue = (SJPVenueLocation)loc;

                        if (string.IsNullOrEmpty(venue.VenueGroupName) && !string.IsNullOrEmpty(venue.Parent))
                        {
                            // If the group name is empty but parent is not empty get the group name from the parent

                            SJPLocation parentVenue = venues.SingleOrDefault(v => v.ID == venue.Parent);

                            if (parentVenue != null)
                            {
                                SJPVenueLocation pvl = (SJPVenueLocation)parentVenue;
                                if (!groupedVenues.Keys.Contains(pvl.VenueGroupName))
                                {
                                    groupedVenues.Add(pvl.VenueGroupName, new List<SJPVenueLocation>());
                                }
                                groupedVenues[pvl.VenueGroupName].Add(venue);
                            }
                            else // no suitable parent found add it as no group
                            {
                                groupedVenues[defaultGroupName].Add(venue);
                            }
                        }
                        else if (!string.IsNullOrEmpty(venue.VenueGroupName))
                        {
                            // group name specified add it will group name
                            if (!groupedVenues.Keys.Contains(venue.VenueGroupName))
                            {
                                groupedVenues.Add(venue.VenueGroupName, new List<SJPVenueLocation>());
                            }
                            groupedVenues[venue.VenueGroupName].Add(venue);
                        }
                        else // No grouping specified add it as no group
                        {
                            groupedVenues[defaultGroupName].Add(venue);
                        }
                    }

                    foreach (string venueGroupName in groupedVenues.Keys)
                    {
                        foreach (SJPVenueLocation vl in groupedVenues[venueGroupName].OrderBy(v => v.DisplayName))
                        {
                            ListItem vItem = new ListItem();
                            vItem.Text = vl.DisplayName;
                            vItem.Value = vl.ID;

                            if (venueGroupName != defaultGroupName)
                                vItem.Attributes.Add("OptionGroup", venueGroupName);

                            venueDropdownNonJS.Items.Add(vItem);
                        }
                    }
                }
                else
                {
                    venueDropdownNonJS.DataSource = venues;
                    venueDropdownNonJS.DataTextField = "DisplayName";
                    venueDropdownNonJS.DataValueField = "Id";
                    venueDropdownNonJS.DataBind();
                }

                venueDropdownNonJS.Items.Insert(0, new ListItem(venueDropDownDefaultItem));
            }
        }

        /// <summary>
        /// Registers the javascripts required to show auto suggest functionality
        /// </summary>
        private void RegisterJavascripts()
        {
            SJPPageMobile currentPage = (SJPPageMobile)Page;

            // Javascript settings
            scriptPath.Value = currentPage.ResolveClientUrl(currentPage.JavascriptPath);

            scriptId.Value = Properties.Current["LocationControl.LocationSuggest.ScriptId"];
            
            currentPage.AddJavascript("LocationSuggest.js");
        }

        /// <summary>
        /// Sets up resource string from content database
        /// </summary>
        private void SetupResources()
        {
            SJPPageMobile currentPage = (SJPPageMobile)Page;

            ambiguousText = currentPage.GetResource("LocationControl.ambiguityText.Text");
            invalidPostcodeText = currentPage.GetResource("LocationControl.ambiguityText.invalidPostcode.Text");
            noLocationFoundText = currentPage.GetResource("LocationControl.ambiguityText.noLocationFound.Text");
            noLocationUKFoundText = currentPage.GetResource("LocationControl.ambiguityText.noLocationUKFound.Text");
            noLocationLocalityFoundText = currentPage.GetResource("LocationControl.ambiguityText.noLocationLocalityFound.Text");
            chooseVenueLocationText = currentPage.GetResource("LocationControl.ambiguityText.chooseVenueLocation.Text");
            locationDropDownDefaultItem = currentPage.GetResource("LocationControl.locationDropdown.DefaultItem.Text");
            venueDropDownDefaultItem = currentPage.GetResource("LocationControl.VenueDropdown.DefaultItem.Text");
        }

        /// <summary>
        /// Sets up controls with resources from the content database
        /// </summary>
        private void SetupResourcesForControls()
        {
            SJPPageMobile currentPage = (SJPPageMobile)Page;

            if (IsVenueOnly)
            {
                locationInput.ToolTip = currentPage.GetResource("LocationControl.LocationInput.Tooltip.Venue");
                locationInput_Discription.Text = currentPage.GetResource("LocationControl.LocationInput.Discription.Text.Venue");
                locationInput.Attributes.Add("data-sjpdefaultvalue", currentPage.GetResource("LocationControl.LocationInput.Tooltip.Venue"));
            }
            else
            {
                locationInput.ToolTip = currentPage.GetResource("LocationControl.LocationInput.Tooltip.All");
                locationInput_Discription.Text = currentPage.GetResource("LocationControl.LocationInput.Discription.Text.All");
                locationInput.Attributes.Add("data-sjpdefaultvalue",
                    LocationDirectionLabel.Text == currentPage.GetResourceMobile("JourneyInput.Location.From.Text") ?
                    currentPage.GetResourceMobile("JourneyInput.Location.From.Watermark") : 
                    currentPage.GetResourceMobile("JourneyInput.Location.To.Watermark"));
            }

            locationInputNonJS.ToolTip = locationInput.ToolTip;

            venueInputPageLnk.Text = currentPage.GetResourceMobile("JourneyInput.Location.Venues.Text");
            venueInputPageLnk.ToolTip = currentPage.GetResourceMobile("JourneyInput.Location.Venues.ToolTip");
            resetButton.Text = currentPage.GetResource("LocationControl.LocationInput.Reset.Clear.Text");
            resetButton.ToolTip = currentPage.GetResource("LocationControl.LocationInput.Reset.Clear.Text");
            resetButtonNonJS.Text = resetButton.Text;
            currentLocationButton.ToolTip = currentPage.GetResource("LocationControl.CurrentLocation.Tooltip");
        }

        #endregion
    }
}
