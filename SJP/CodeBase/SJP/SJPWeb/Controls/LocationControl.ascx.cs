// *********************************************** 
// NAME             : LocationControl.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 1 Apr 2011
// DESCRIPTION  	: Location User control
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Location user control
    /// </summary>
    public partial class LocationControl : System.Web.UI.UserControl
    {
        #region Private Fields

        private SJPLocationType locationType = SJPLocationType.Unknown;
        private LocationStatus status = LocationStatus.Unspecified;
        private SJPLocation location = null;
        private LocationService locationService = null;
        private bool resolveLocation = true;
        #region Resource Strings
        private string ambiguity = string.Empty;
        private string invalidPostcodeText = string.Empty;
        private string noLocationFoundText = string.Empty;
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
            get { return status; }
        }

        /// <summary>
        /// Read/Write propterty determining type of location represented by location control
        /// </summary>
        public SJPLocationType TypeOfLocation
        {
            get
            {
                if(IsPostBack && locationDropdown.Visible  && !ambiguityRow.Visible)
                    return SJPLocationType.Venue;
                else
                    return locationType;
            }
            set { locationType = value; }
        }

        /// <summary>
        /// Read only property returns the id of the internal control to associate label to
        /// </summary>
        public string ControlToAssociateLabel
        {
            get
            {
                if (locationDropdown.Visible || locationType == SJPLocationType.Venue)
                    return  string.Format("{0}:{1}", this.ID,"locationDropdown");
                else
                    return string.Format("{0}:{1}", this.ID, "locationInput"); 
            }
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
            SetupResources();
            RegisterJavascripts();
            locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
        }
        
        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowHideControls();

            if (locationType == SJPLocationType.Venue)
            {
                SetForVenueLocations();
            }
            else
            {
                SetForInputLocation();
            }
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialises the locaiton control
        /// </summary>
        /// <param name="searchString">Location to be search</param>
        /// <param name="locationType">Type of location</param>
        public void Initialise(string searchString, SJPLocationType locationType)
        {
            locationInput.Text = searchString;
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
            locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
            if (locationType == SJPLocationType.Venue)
            {
                #region Validate Venue location

                if (resolveLocation)
                {
                    if (locationDropdown.SelectedIndex == 0)
                        return false;

                    location = locationService.GetSJPLocation(locationDropdown.SelectedValue, SJPLocationType.Venue);

                    status = LocationStatus.Resolved;
                    return true;
                }
                else
                {   
                    // Location was supplied to control and was considered resolved, so check it is
                    if (IsValidLocation(location))
                    {
                        status = LocationStatus.Resolved;
                        return true;
                    }
                }

                #endregion
            }
            else
            {
                #region Validate location

                if (resolveLocation)
                {
                    string searchString = locationInput.Text.Trim();
                    string searchId = locationInput_hdnValue.Value.Trim();
                    SJPLocationType searchType = searchString.IsValidPostcode() ? SJPLocationType.Postcode : SJPLocationType.Unknown;

                    // if location is set from javascript dropdown
                    if (!string.IsNullOrEmpty(locationInput_hdnType.Value.Trim()) &&
                        !string.IsNullOrEmpty(searchId)
                        && jsEnabled.Value.Parse(true))
                    {

                        searchType = (SJPLocationType)Enum.Parse(typeof(SJPLocationType), locationInput_hdnType.Value.Trim());

                        location = locationService.GetSJPLocation(searchId, searchType);

                    }
                    // if location is already in ambiguity state
                    else if (!ambiguityRow.Visible && !locationDropdown.Visible)
                    {
                        location = locationService.GetSJPLocation(searchString, searchType);
                    }
                    else // possible user input the location name without picking from list
                    {
                        if (locationDropdown.SelectedValue.Trim() != "-1")
                        {
                            string[] locationIds = locationDropdown.SelectedValue.Split(new char[] { ':' });
                            if (locationIds.Length == 2)
                            {
                                searchType = (SJPLocationType)Enum.Parse(typeof(SJPLocationType), locationIds[1].Trim());

                                location = locationService.GetSJPLocation(locationIds[0].Trim(), searchType);
                            }

                        }
                    }
                }
                
                // Check location is valid
                if (IsValidLocation(location))
                {
                    status = LocationStatus.Resolved;
                    return true;
                }
                else
                {
                    status = LocationStatus.Ambiguous;
                }

                #endregion
            }

            return false;
        }
        
        /// <summary>
        /// Reset location to type of location specified
        /// </summary>
        /// <param name="typeOfLocation">Type of location</param>
        public void Reset(SJPLocationType typeOfLocation)
        {
            locationType = typeOfLocation;
            if (typeOfLocation == SJPLocationType.Venue)
            {
                PopulateVenueDropDown();
            }
            else
            {
                locationDropdown.Visible = false;
            }
            status = LocationStatus.Unspecified;
            location = null;
            locationInput.Text = string.Empty;
            locationInput_hdnValue.Value = string.Empty;
            locationInput_hdnType.Value = string.Empty;
            ambiguityRow.Visible = false;
            ambiguityText.Text = string.Empty;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Show/Hides controls based on various state of the location control
        /// </summary>
        private void ShowHideControls()
        {
            if (TypeOfLocation == SJPLocationType.Venue)
            {
                locationDropdown.Visible = true;
                locationInput.Visible = false;
                ambiguityRow.Visible = false;
                locationInput_hdnValue.Visible = false;
                locationInput_hdnType.Visible = false;
                reset.Visible = false;
            }
            else
            {
                bool ambiguity = false;

                if (status == LocationStatus.Ambiguous)
                    ambiguity = true;

                ambiguityRow.Visible = ambiguity;
                locationDropdown.Visible = ambiguity;
                locationInput.Visible = !ambiguity;
                locationInput_hdnValue.Visible = !ambiguity;
                locationInput_hdnType.Visible = !ambiguity;
                reset.Visible = ambiguity;
            }
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
            if (status == LocationStatus.Ambiguous)
            {
                SetForAmbiguousLocation();
            }
            else if (status == LocationStatus.Resolved)
            {
                locationInput.Text = location.DisplayName;
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
        }

        /// <summary>
        /// Sets location for ambiguity situation
        /// </summary>
        private void SetForAmbiguousLocation()
        {
            string search = locationInput.Text.Trim();

            bool isDebug = DebugHelper.ShowDebug;

            if (!IsPossiblePostCode(search))
            {
                // Only perform ambiguity search if user has entered text
                if (!string.IsNullOrEmpty(search))
                {
                    List<SJPLocation> alternateLocations = locationService.GetAlternateLocations(search);

                    if (alternateLocations.Count > 0)
                    {
                        ambiguityText.Text = string.Format(ambiguity, search);

                        if (isDebug)
                        {
                            ambiguityText.Text += string.Format("<br /><span class=\"debug\">Total[{0}]<br />Grp[{1}] Rail[{2}] Loc[{3}] Coach[{4}] TMU[{5}] Ferry[{6}] Air[{7}] Pcde[{8}] Other[{9}]</span>",
                                alternateLocations.Count,
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationGroup; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationRail; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.Locality; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationCoach; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationTMU; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationFerry; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.StationAirport; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return loc.TypeOfLocationActual == SJPLocationType.Postcode; }),
                                alternateLocations.Count(delegate(SJPLocation loc) { return (loc.TypeOfLocationActual == SJPLocationType.Unknown)
                                                                                            || (loc.TypeOfLocationActual == SJPLocationType.Venue)
                                                                                            || (loc.TypeOfLocationActual == SJPLocationType.Station); })
                                );
                        }

                        locationDropdown.Items.Clear();

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
                            locationDropdown.Items.Add(alterLocationItem);
                        }

                        locationDropdown.Items.Insert(0, new ListItem(locationDropDownDefaultItem, "-1"));

                        // Set ambiguous locations specific reset text
                        SJPPage currentPage = (SJPPage)Page;
                        reset.Text = currentPage.GetResource("LocationControl.LocationInput.Reset.Ambiguous.Text");
                    }
                    else
                    {
                        ambiguityText.Text = noLocationFoundText;
                        locationDropdown.Visible = false;
                    }
                }
                else
                {
                    ambiguityText.Text = noLocationFoundText;
                    locationDropdown.Visible = false;
                }
            }
            else
            {
                ambiguityText.Text = invalidPostcodeText;
                locationDropdown.Visible = false;
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
        /// Sets location control to show venue dropdowns
        /// </summary>
        private void SetForVenueLocations()
        {
            if (!IsPostBack)
            {
                PopulateVenueDropDown();
            }

            if (location != null && status == LocationStatus.Resolved)
            {
                locationDropdown.SelectedIndex = locationDropdown.Items.IndexOf(locationDropdown.Items.FindByValue(location.ID.Trim()));
            }
        }

        /// <summary>
        /// Populates the Venue dropdown
        /// </summary>
        private void PopulateVenueDropDown()
        {
            List<SJPLocation> venues = locationService.GetSJPVenueLocations();
           
            locationDropdown.Items.Clear();

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

                        locationDropdown.Items.Add(vItem);
                    }
                }
            }
            else
            {
                locationDropdown.DataSource = venues;
                locationDropdown.DataTextField = "DisplayName";
                locationDropdown.DataValueField = "Id";
                locationDropdown.DataBind();
            }

            locationDropdown.Items.Insert(0, new ListItem(venueDropDownDefaultItem));
        }

        /// <summary>
        /// Registers the javascripts required to show auto suggest functionality
        /// </summary>
        private void RegisterJavascripts()
        {
            SJPPage currentPage = (SJPPage)Page;

            // Javascript settings
            scriptPath.Value = currentPage.ResolveClientUrl(currentPage.JavascriptPath);

            scriptId.Value = Properties.Current["LocationControl.LocationSuggest.ScriptId"];

            currentPage.AddJavascript("common.js");
            currentPage.AddJavascript("locationSuggest.js");
            currentPage.AddJavascript("jquery.scrollTo.js");
            currentPage.AddJavascript("jquery.serialScroll.js");
        }

        /// <summary>
        /// Sets up resource string from content database
        /// </summary>
        private void SetupResources()
        {
            SJPPage currentPage = (SJPPage)Page;

            ambiguity = currentPage.GetResource("LocationControl.ambiguityText.Text");
            invalidPostcodeText = currentPage.GetResource("LocationControl.ambiguityText.invalidPostcode.Text");
            noLocationFoundText = currentPage.GetResource("LocationControl.ambiguityText.noLocationFound.Text");
            locationDropDownDefaultItem = currentPage.GetResource("LocationControl.locationDropdown.DefaultItem.Text");
            venueDropDownDefaultItem = currentPage.GetResource("LocationControl.VenueDropdown.DefaultItem.Text");
            locationInput_Discription.Text = currentPage.GetResource("LocationControl.LocationInput.Discription.Text");
            reset.Text = currentPage.GetResource("LocationControl.LocationInput.Reset.Text");
        }

        #endregion
    }
}