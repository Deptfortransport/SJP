// *********************************************** 
// NAME             : CycleJourneyLocations.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 25 Apr 2011
// DESCRIPTION  	: User Control to represent intermediate selection of venue cycle park. It shows venue cycle park map 
//                    to the user and enable them to pick cycle park and plan a journey to those cycle parks
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using SJP.Common;
using SJP.Common.DataServices;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// User Control to represent intermediate selection of venue cycle park. It shows venue cycle park map 
    /// to the user and enable them to pick cycle park and plan a journey to those cycle parks
    /// </summary>
    public partial class CycleJourneyLocations : System.Web.UI.UserControl
    {
        #region Private Fields

        private SJPVenueLocation venue;
        private DateTime dateTimeOutward = DateTime.MinValue;
        private DateTime dateTimeReturn = DateTime.MinValue;
        private List<SJPVenueCyclePark> cycleParkList;
        private string cycleRoute = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// SJP venue location for which Cycle park options required
        /// </summary>
        public SJPVenueLocation Venue
        {
            get { return venue; }
            set { venue = value; }
        }

        /// <summary>
        /// Read/Write. Outward date time to use for filtering cycle parks
        /// </summary>
        public DateTime OutwardDateTime
        {
            get { return dateTimeOutward; }
            set { dateTimeOutward = value; }
        }

        /// <summary>
        /// Read/Write. Return date time to use for filtering cycle parks
        /// </summary>
        public DateTime ReturnDateTime
        {
            get { return dateTimeReturn; }
            set { dateTimeReturn = value; }
        }

        /// <summary>
        /// Returns the Cycle penalty function 
        /// </summary>
        /// <example>
        /// Call C:\CyclePlanner\td.cp.CyclePenaltyFunctions.v2.dll, TransportDirect.JourneyPlanning.CyclePenaltyFunctions.QuietestV912
        /// </example>
        public string CycleRouteType
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Read only property to represent sjp venue location object with only selected cycle park.
        /// The property returns a clone of SJP venue location with all the other cycle parks removed from the list.
        /// </summary>
        public SJPVenueCyclePark SelectedCyclePark
        {
            get
            {
                SJPVenueCyclePark selectedCyclePark = null;

                if (cycleParkList != null)
                {
                    selectedCyclePark = cycleParkList.SingleOrDefault(cp => cp.ID == preferredParksOptions.SelectedValue);
                }

                return selectedCyclePark;
            }
        }

        /// <summary>
        /// Read only property to return the selected outward date time updated with any cycle park transit times
        /// </summary>
        public DateTime SelectedOutwardDateTime
        {
            get
            {
                DateTime selectedOutwardDateTime = dateTimeOutward;

                // Update the outward time to be the outward datetime - any transit times from 
                // venue to the car park
                if (selectedOutwardDateTime != DateTime.MinValue)
                    selectedOutwardDateTime = selectedOutwardDateTime.Subtract(GetTransitTime(false));
                
                return selectedOutwardDateTime;
            }
        }

        /// <summary>
        /// Read only property to return the selected return date time updated with any cycle park transit times
        /// </summary>
        public DateTime SelectedReturnDateTime
        {
            get
            {
                // Update the return time to be the return datetime + any transit times from 
                // venue to the car park
                DateTime selectedReturnDateTime = dateTimeReturn;

                if (selectedReturnDateTime != DateTime.MinValue)
                    selectedReturnDateTime = selectedReturnDateTime.Add(GetTransitTime(true));

                return selectedReturnDateTime;
            }
        }

        /// <summary>
        /// Read only property to return the selected cycle route type
        /// </summary>
        public string SelectedCycleRouteType
        {
            get
            {
                if (typeOfRouteOptions != null && typeOfRouteOptions.Items.Count > 0)
                {
                    cycleRoute = typeOfRouteOptions.SelectedValue;
                }

                return cycleRoute;
            }
            set { cycleRoute = value; }
        }

        /// <summary>
        /// Read only. Returns true is cycle parks found for venue
        /// </summary>
        public bool IsCycleParksAvailable
        {
            get { return cycleParkList != null; }
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
            SetupCycleParks();

            DisplayMessages();
        }

        /// <summary>
        /// Page PreRender event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResources();

            if (!IsPostBack)
            {
                SetupVenueMap();
                SetupVenueDropDown();
                SetupCycleRouteTypeDropDown();
            }

            // Only display dropdowns/labels if cycle parks found
            if (cycleParkList != null && cycleParkList.Count > 0)
            {
                pnlCycleParks.Visible = true;
            }
            else
            {
                pnlCycleParks.Visible = false;
            }

            SetupDebug();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets any messages to show based on validation of car parks and travel dates/times
        /// </summary>
        private void DisplayMessages()
        {
            // Display message if no cycle parks found, must be placed in Load method 
            // so Master page can display it
            if (venue != null && cycleParkList == null)
            {
                List<string> messageArgs = new List<string>();
                messageArgs.Add(venue.Name);

                SJPMessage sjpMessage = new SJPMessage(string.Empty, "CycleJourneyLocations.CycleParkNoneFound.Text", string.Empty, string.Empty,
                    messageArgs, 0, 0, SJPMessageType.Error);

                ((SJPWeb)this.Page.Master).DisplayMessage(sjpMessage);
            }
            // Display message if outward and return dates are different
            else if ((dateTimeOutward != DateTime.MinValue && dateTimeReturn != DateTime.MinValue) &&
                    (!dateTimeOutward.Date.Equals(dateTimeReturn.Date)))
            {
                TimeSpan overnightThreshold = new TimeSpan(3, 0, 0);
                if (dateTimeReturn.TimeOfDay < overnightThreshold)
                {
                    DateTime adjustedReturnDateTime = dateTimeReturn.AddDays(-1);
                    if (!dateTimeOutward.Date.Equals(adjustedReturnDateTime.Date))
                    {
                        SJPMessage sjpMessage = new SJPMessage("CycleJourneyLocations.CycleReturnDateDifferent.Text", SJPMessageType.Warning);

                        ((SJPWeb)this.Page.Master).DisplayMessage(sjpMessage);
                    }
                }
                else
                {
                    SJPMessage sjpMessage = new SJPMessage("CycleJourneyLocations.CycleReturnDateDifferent.Text", SJPMessageType.Warning);

                    ((SJPWeb)this.Page.Master).DisplayMessage(sjpMessage);
                }
            }
        }

        /// <summary>
        /// Sets up resource content for the controls
        /// </summary>
        private void SetupResources()
        {
            Language language = CurrentLanguage.Value;

            preferredParksHeading.InnerText = Global.SJPResourceManager.GetString(language, "CycleJourneyLocations.PreferredParksHeading.Text");
            typeOfRouteHeading.Text = Global.SJPResourceManager.GetString(language, "CycleJourneyLocations.TypeOfRouteHeading.Text");
            usetheMap.Text = Global.SJPResourceManager.GetString(language, "CycleJourneyLocations.UseTheMap.Text");

            string venueName = string.Empty;
            string venueUrl = string.Empty;

            if (venue != null)
            {
                venueName = venue.DisplayName;
                venueUrl = venue.VenueMapUrl;
            }

            if (!string.IsNullOrEmpty(venueUrl))
            {
                preferredParksInfo.Text = string.Format(
                    Global.SJPResourceManager.GetString(language, "CycleJourneyLocations.PreferredParksInfo.Text"),
                    venueUrl, venueName);
            }
            else
            {
                preferredParksInfo.Text = 
                    Global.SJPResourceManager.GetString(language, "CycleJourneyLocations.PreferredParksInfo.NoMapLink.Text");
            }
        }

        /// <summary>
        /// Sets up venue map url using resource content
        /// </summary>
        private void SetupVenueMap()
        {
            if (venue != null)
            {
                SJPPage page = (SJPPage)Page;

                Language language = CurrentLanguage.Value;

                string venueMapUrl_ResourceId = string.Format("JourneyLocations.{0}.CyclePark.Url", venue.Naptan[0]);
                string venueMapAlternateText_ResourceId = string.Format("JourneyLocations.{0}.CyclePark.AlternateText", venue.Naptan[0]);

                string venueMapURL = Global.SJPResourceManager.GetString(language, venueMapUrl_ResourceId);
                string venueMapAltText = Global.SJPResourceManager.GetString(language, venueMapAlternateText_ResourceId);

                // If venue has no map, then use parents map
                if (string.IsNullOrEmpty(venueMapURL))
                {
                    venueMapUrl_ResourceId = string.Format("JourneyLocations.{0}.CyclePark.Url", venue.Parent);
                    venueMapURL = Global.SJPResourceManager.GetString(language, venueMapUrl_ResourceId);
                }
                if (string.IsNullOrEmpty(venueMapAltText))
                {
                    venueMapAlternateText_ResourceId = string.Format("JourneyLocations.{0}.CyclePark.AlternateText", venue.Parent);
                    venueMapAltText = Global.SJPResourceManager.GetString(language, venueMapAlternateText_ResourceId);
                }

                venueMap.ImageUrl = page.ImagePath + venueMapURL;
                venueMap.AlternateText = venueMapAltText;
                venueMap.ToolTip = venueMap.AlternateText;
            }
        }

        /// <summary>
        /// Sets up the cycle park for the provided venue
        /// </summary>
        private void SetupCycleParks()
        {
            // Venue should have been set by now
            if (venue != null)
            {
                // Check if the users journey travel dates should be used in determining validaty of car parks
                bool validateDate = Properties.Current["CycleJourneyLocations.JouneyDate.Validate.Switch"].Parse(true);
                if (!validateDate)
                {
                    dateTimeOutward = DateTime.MinValue;
                    dateTimeReturn = DateTime.MinValue;
                }

                // Get car parks
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                // Get cycle parks for the venue and its parent
                List<string> naptans = new List<string>(venue.Naptan);
                if (!string.IsNullOrEmpty(venue.Parent))
                {
                    naptans.Add(venue.Parent);
                }

                // Cycle parks
                cycleParkList = locationService.GetSJPVenueCycleParks(naptans, dateTimeOutward, dateTimeReturn);

                if (Properties.Current["CycleJourneyLocations.VenueMap.Clickable.Switch"].Parse(true))
                {
                    SetupClickableMapBullets(cycleParkList);
                }
            }
        }

        /// <summary>
        /// Sets up cycle park map bullet links
        /// The links rendered as absolute position style set in content database
        /// </summary>
        /// <param name="cycleParkList"></param>
        private void SetupClickableMapBullets(List<SJPVenueCyclePark> cycleParkList)
        {
            if (cycleParkList != null)
            {
                mapBulletTarget.Value = preferredParksOptions.ClientID;

                Language language = CurrentLanguage.Value;

                foreach (SJPVenueCyclePark cyclePark in cycleParkList)
                {
                    using (HtmlAnchor bullet = new HtmlAnchor())
                    {
                        string bulletStyle_ResourceId = string.Format("JourneyLocations.CyclePark.{0}.Style", cyclePark.ID);

                        string bulletStyle = Global.SJPResourceManager.GetString(language, bulletStyle_ResourceId);
                        if (!string.IsNullOrEmpty(bulletStyle))
                        {
                            bullet.Title = cyclePark.Name;
                            bullet.Attributes["style"] = bullet.Attributes["style"] + bulletStyle + "display:none";
                            bullet.Attributes["class"] = "bullet cycleBullet ";
                            bullet.HRef = "#";
                            bullet.InnerHtml = "&nbsp;";
                            bullet.Name = cyclePark.ID;

                            cycleParkBullets.Controls.Add(bullet);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Binds venue cycle parks to preferredParkOptions dropdown
        /// </summary>
        private void SetupVenueDropDown()
        {
            List<SJPVenueCyclePark> cycleParks = cycleParkList;

            if (cycleParks != null)
            {
                preferredParksOptions.DataSource = cycleParks;
                preferredParksOptions.DataTextField = "Name";
                preferredParksOptions.DataValueField = "ID";

                preferredParksOptions.DataBind();
            }
        }

        /// <summary>
        /// Populates cycle penalty functions using dataservices
        /// </summary>
        private void SetupCycleRouteTypeDropDown()
        {
            IDataServices dataServices = SJPServiceDiscovery.Current.Get<IDataServices>(ServiceDiscoveryKey.DataServices);

            dataServices.LoadListControl(DataServiceType.CycleRouteType, typeOfRouteOptions, Global.SJPResourceManager, CurrentLanguage.Value);

            // Set the selected value to be the specified value (allows previous choice to be retained when returning
            // to the locations page)
            if (!string.IsNullOrEmpty(cycleRoute))
            {
                if (typeOfRouteOptions != null && typeOfRouteOptions.Items.Count > 0)
                {
                    string selected = typeOfRouteOptions.SelectedValue;
                    // Place in a try just to be safe
                    try
                    {
                        typeOfRouteOptions.SelectedValue = cycleRoute;
                    }
                    catch
                    {
                        // Ignore exceptions and set back to its previous selected value
                        typeOfRouteOptions.SelectedValue = selected;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the total transit time between the Venue and Cycle park
        /// </summary>
        /// <returns></returns>
        private TimeSpan GetTransitTime(bool isReturn)
        {
            TimeSpan time = new TimeSpan();

            SJPVenueCyclePark cyclePark = SelectedCyclePark;

            if (cyclePark != null)
            {
                #region Cycle park to/from the venue interchange

                // Use Walk To gate duration, or Walk From gate for the return journey if it exists
                TimeSpan duration = cyclePark.WalkToGateDuration;

                if ((isReturn) && (cyclePark.WalkFromGateDuration.TotalMinutes > 0))
                {
                    duration = cyclePark.WalkFromGateDuration;
                }
                
                time = time.Add(duration);

                #endregion

                #region Venue gate and check constraints
                
                LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

                // Get the venue gate and path details for cycle park

                // Use Entrance naptan always, or Exit naptan for the return journey if it exists
                string gateNaptan = cyclePark.VenueGateEntranceNaPTAN;

                if ((isReturn) && (!string.IsNullOrEmpty(cyclePark.VenueGateExitNaPTAN))) 
                {
                    gateNaptan = cyclePark.VenueGateExitNaPTAN;
                }

                SJPVenueGate gate = locationService.GetSJPVenueGate(gateNaptan);
                SJPVenueGateCheckConstraint gateCheckConstraint = locationService.GetSJPVenueGateCheckConstraints(gate, !isReturn);
                SJPVenueGateNavigationPath gateNavigationPath = locationService.GetSJPVenueGateNavigationPaths(venue, gate, !isReturn);

                if (gateCheckConstraint != null)
                {
                    time = time.Add(gateCheckConstraint.AverageDelay);
                }

                if (gateNavigationPath != null)
                {
                    time = time.Add(gateNavigationPath.TransferDuration);
                }

                #endregion
            }

            return time;
        }

        /// <summary>
        /// Sets up the debug information
        /// </summary>
        private void SetupDebug()
        {
            if (DebugHelper.ShowDebug)
            {
                if (venue != null)
                {
                    usetheMap.Text = string.Format("{0}<br /><span class=\"debug\">venue[{1}] parent[{2}]<br />{3}</span>",
                        usetheMap.Text,
                        venue.Naptan.FirstOrDefault(),
                        venue.Parent,
                        venueMap.ImageUrl);
                }
            }
        }

        #endregion

    }
}
