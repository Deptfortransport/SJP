// *********************************************** 
// NAME             : JourneyOptionTabContainer.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 1 Apr 2011
// DESCRIPTION  	: Container for journey option tabs
// ************************************************


using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Container for journey option tabs
    /// </summary>
    public partial class JourneyOptionTabContainer : System.Web.UI.UserControl
    {
        #region Private Fields

        private IJourneyOptionsTab activeTab;
        private bool isOriginVenue = false;
        private SJPLocation venue;
        private DateTime outwardDateTime;
        private DateTime returnDateTime;

        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write property returns the current active journey option tab
        /// </summary>
        public IJourneyOptionsTab ActiveTab
        {
            get { return activeTab; }
            set { activeTab = value; }
        }

        /// <summary>
        /// Read/Write only property determining the planner mode of the active tab
        /// </summary>
        public SJPJourneyPlannerMode PlannerMode
        {
            get { return activeTab.PlannerMode; }
            set
            {
                switch (value)
                {
                    case SJPJourneyPlannerMode.ParkAndRide:
                        activeTab = parkandrideTab;
                        break;

                    case SJPJourneyPlannerMode.RiverServices:
                        activeTab = riverServicesTab;
                        break;

                    case SJPJourneyPlannerMode.BlueBadge:
                        activeTab = blueBadgeTab;
                        break;

                    case SJPJourneyPlannerMode.Cycle:
                        activeTab = cycleTab;
                        break;

                    default:
                        activeTab = publicJourneyTab;
                        break;

                }
            }
        }

        /// <summary>
        /// Read/Write. PublicJourneyOptionsTab containing the options selected
        /// </summary>
        public PublicJourneyOptionsTab PublicJourneyTab
        {
            get { return publicJourneyTab; }
            set { publicJourneyTab = value; }
        }

        /// <summary>
        /// Read/Write. Is the Venue the origin venue (false be default)
        /// </summary>
        public bool IsOriginVenue
        {
            get { return isOriginVenue; }
            set 
            { 
                isOriginVenue = value;
                riverServicesTab.IsOriginVenue = isOriginVenue;
                //parkandrideTab and blueBadgeTab have no direction specific venue text so don't need to pass flag through
                cycleTab.IsOriginVenue = isOriginVenue;
            }
        }

        /// <summary>
        /// Read/Write. SJP venue location
        /// </summary>
        public SJPLocation Venue
        {
            get { return venue; }
            set { 
                venue = value;
                riverServicesTab.Venue = venue;
                parkandrideTab.Venue = venue;
                blueBadgeTab.Venue = venue;
                cycleTab.Venue = venue;
            }
        }

        /// <summary>
        /// Read/Write outward date time selected for the journey
        /// </summary>
        public DateTime OutwardDateTime 
        {
            get { return outwardDateTime; }
            set
            {
                outwardDateTime = value;
                riverServicesTab.OutwardDateTime = value;
                parkandrideTab.OutwardDateTime = value;
                blueBadgeTab.OutwardDateTime = value;
                cycleTab.OutwardDateTime = value;

            }
             
        }

        /// <summary>
        /// Read/Write. Return date time selected for the journey
        /// </summary>
        public DateTime ReturnDateTime 
        {
            get { return returnDateTime; }
            set
            {
                returnDateTime = value;
                riverServicesTab.ReturnDateTime = value;
                parkandrideTab.ReturnDateTime = value;
                blueBadgeTab.ReturnDateTime = value;
                cycleTab.ReturnDateTime = value;

            } 
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
            publicJourneyTab.OnPlanJourney +=new PlanJourney(PlanJourney);
            riverServicesTab.OnPlanJourney += new PlanJourney(PlanJourney);
            parkandrideTab.OnPlanJourney +=new PlanJourney(PlanJourney);
            blueBadgeTab.OnPlanJourney  +=new PlanJourney(PlanJourney);
            cycleTab.OnPlanJourney += new UserPortal.SJPWeb.PlanJourney(PlanJourney);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupResourceStrings();
            if (IsPostBack)
            {
                SyncToClientSideTab();
            }
        }
 
        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupTabs();
        }
                
        #endregion

        #region Control event handlers

        /// <summary>
        /// Tab clicked event handler
        /// </summary>
        /// <remarks>
        /// This will be handled only when javascript disabled.
        /// When javascript enabled the tab switching will be made client side using javascript 
        /// to save server round trips
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabClicked(object sender, EventArgs e)
        {
            if(sender != null && sender is Button)
            {
                Button source = (Button)sender;
                if (source == blueBadge)
                    activeTab = blueBadgeTab;
                else if (source == riverServices)
                    activeTab = riverServicesTab;
                else if (source == parkAndRide)
                    activeTab = parkandrideTab;
                else if (source == cycle)
                    activeTab = cycleTab;
                else
                    activeTab = publicJourneyTab;
            }
        }

        /// <summary>
        /// Handles the PlanJourney event raised by individual tabs
        /// The handler just let the event to be bubbled up so it can be handled by the parent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlanJourney(object sender, EventArgs e)
        {
            if (sender is IJourneyOptionsTab)
            {
                RaiseBubbleEvent(sender, e);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets active and inactive tabs
        /// </summary>
        private void SetupTabs()
        {
            HideOrShowTabs();

            if (activeTab == null)
                activeTab = publicJourneyTab;

            switch (activeTab.PlannerMode)
            {
                case  SJPJourneyPlannerMode.ParkAndRide:
                    hdnActiveTab.Value = parkAndRide.Text.Trim();
                    SetInActiveTab(pjLink, pjTab);
                    SetActiveTab(prLink, prTab);
                    SetInActiveTab(rsLink, rsTab);
                    SetInActiveTab(bbLink, bbTab);
                    SetInActiveTab(cyLink, cyTab);
                    break;

                case SJPJourneyPlannerMode.RiverServices:
                    hdnActiveTab.Value = riverServices.Text.Trim();
                    SetInActiveTab(pjLink, pjTab);
                    SetInActiveTab(prLink, prTab);
                    SetActiveTab(rsLink, rsTab);
                    SetInActiveTab(bbLink, bbTab);
                    SetInActiveTab(cyLink, cyTab);
                    break;

                case SJPJourneyPlannerMode.BlueBadge:
                    hdnActiveTab.Value = blueBadge.Text.Trim();
                    SetInActiveTab(pjLink, pjTab);
                    SetInActiveTab(prLink, prTab);
                    SetInActiveTab(rsLink, rsTab);
                    SetActiveTab(bbLink, bbTab);
                    SetInActiveTab(cyLink, cyTab);
                    break;

                case SJPJourneyPlannerMode.Cycle:
                    hdnActiveTab.Value = cycle.Text.Trim();
                    SetInActiveTab(pjLink, pjTab);
                    SetInActiveTab(prLink, prTab);
                    SetInActiveTab(rsLink, rsTab);
                    SetInActiveTab(bbLink, bbTab);
                    SetActiveTab(cyLink, cyTab);
                    break;

                default:
                    hdnActiveTab.Value = publicJourney.Text.Trim();
                    SetActiveTab(pjLink, pjTab);
                    SetInActiveTab(prLink, prTab);
                    SetInActiveTab(rsLink, rsTab);
                    SetInActiveTab(bbLink, bbTab);
                    SetInActiveTab(cyLink, cyTab);
                    break;
            }

            SetTabsEnableOrDisable();         
        }

        /// <summary>
        /// Enables or Disables the tabs  - property driven
        /// </summary>
        private void SetTabsEnableOrDisable()
        {
            publicJourneyTab.Disabled =  Properties.Current["JourneyOptionTabContainer.Tabs.PublicTransport.Disabled"].Parse(false);
            riverServicesTab.Disabled = Properties.Current["JourneyOptionTabContainer.Tabs.RiverServices.Disabled"].Parse(false);
            parkandrideTab.Disabled =  Properties.Current["JourneyOptionTabContainer.Tabs.ParkAndRide.Disabled"].Parse(false);
            blueBadgeTab.Disabled =  Properties.Current["JourneyOptionTabContainer.Tabs.BlueBadge.Disabled"].Parse(false);
            cycleTab.Disabled = Properties.Current["JourneyOptionTabContainer.Tabs.Cycle.Disabled"].Parse(false);

            if (publicJourneyTab.Disabled)
            {
                if (!pjTab.Attributes["class"].Contains("disabled"))
                {
                    pjTab.Attributes["class"] += " disabled";
                }
            }
            if (riverServicesTab.Disabled)
            {
                if (!rsTab.Attributes["class"].Contains("disabled"))
                {
                    rsTab.Attributes["class"] += " disabled";
                }
            }
            if (parkandrideTab.Disabled)
            {
                if (!prTab.Attributes["class"].Contains("disabled"))
                {
                    prTab.Attributes["class"] += " disabled";
                }
            }
            if (blueBadgeTab.Disabled)
            {
                if (!bbTab.Attributes["class"].Contains("disabled"))
                {
                    bbTab.Attributes["class"] += " disabled";
                }
            }
            if (cycleTab.Disabled)
            {
                if (!cyTab.Attributes["class"].Contains("disabled"))
                {
                    cyTab.Attributes["class"] += " disabled";
                }
            }
        }

        /// <summary>
        /// Hides or shows tabs - property driven
        /// </summary>
        private void HideOrShowTabs()
        {
            pjLink.Visible = pjTab.Visible = Properties.Current["JourneyOptionTabContainer.Tabs.PublicTransport.Visible"].Parse(true);
            rsLink.Visible = rsTab.Visible = Properties.Current["JourneyOptionTabContainer.Tabs.RiverServices.Visible"].Parse(true);
            prLink.Visible = prTab.Visible = Properties.Current["JourneyOptionTabContainer.Tabs.ParkAndRide.Visible"].Parse(true);
            bbLink.Visible = bbTab.Visible = Properties.Current["JourneyOptionTabContainer.Tabs.BlueBadge.Visible"].Parse(true);
            cyLink.Visible = cyTab.Visible = Properties.Current["JourneyOptionTabContainer.Tabs.Cycle.Visible"].Parse(true);
        }

        /// <summary>
        /// Sets the css to represent active tab(Selected)
        /// </summary>
        /// <param name="headerLink">Header link for the tab to be set active</param>
        /// <param name="tab">Tab content to be set as active</param>
        private void SetActiveTab(HtmlGenericControl headerLink, HtmlGenericControl tab)
        {
            if (!headerLink.Attributes["class"].Contains("active"))
            {
                headerLink.Attributes["class"] = headerLink.Attributes["class"] + " active";
            }
            if(!tab.Attributes["class"].Contains("active"))
            {
                tab.Attributes["class"] = tab.Attributes["class"] + " active";
            }

            // Set the tab section highlight 
            // (use the same style class as the link, assume they are all consistently 5 chars and styles defined correctly)
            divHighlight.Attributes["class"] = headerLink.Attributes["class"].Substring(0, 5);
        }

        /// <summary>
        /// Sets the css to set specific tab as inactive(deselected)
        /// </summary>
        /// <param name="headerLink"></param>
        /// <param name="headerTab"></param>
        private void SetInActiveTab(HtmlGenericControl headerLink, HtmlGenericControl headerTab)
        {
            headerLink.Attributes["class"] = headerLink.Attributes["class"].Replace(" active", "");
            headerTab.Attributes["class"] = headerTab.Attributes["class"].Replace(" active","");
        }

        /// <summary>
        /// Synch server controls to the client side state of the tabs
        /// </summary>
        private void SyncToClientSideTab()
        {
            string active = hdnActiveTab.Value.Trim();

            if (active == parkAndRide.Text.Trim())
                activeTab = parkandrideTab;
            else if (active == blueBadge.Text.Trim())
                activeTab = blueBadgeTab;
            else if (active == riverServices.Text.Trim())
                activeTab = riverServicesTab;
            else if (active == cycle.Text.Trim())
                activeTab = cycleTab;
            else
                activeTab = publicJourneyTab;
        }

        /// <summary>
        /// Sets Control Culture aware resource strings
        /// </summary>
        private void SetupResourceStrings()
        {
            SJPPage page = (SJPPage)Page;

            lblJourneyOptions.Text = page.GetResource("JourneyOptionTabContainer.JourneyOptions.Text");

            publicJourney.Text = page.GetResource("JourneyOptionTabContainer.publiJourney.Text");
            riverServices.Text = page.GetResource("JourneyOptionTabContainer.riverServices.Text");
            parkAndRide.Text = page.GetResource("JourneyOptionTabContainer.parkAndRide.Text");
            blueBadge.Text = page.GetResource("JourneyOptionTabContainer.blueBadge.Text");
            cycle.Text = page.GetResource("JourneyOptionTabContainer.cycle.Text");

            publicJourney.ToolTip = page.GetResource("JourneyOptionTabContainer.publiJourney.ToolTip");
            riverServices.ToolTip = page.GetResource("JourneyOptionTabContainer.riverServices.ToolTip");
            parkAndRide.ToolTip = page.GetResource("JourneyOptionTabContainer.parkAndRide.ToolTip");
            blueBadge.ToolTip = page.GetResource("JourneyOptionTabContainer.blueBadge.ToolTip");
            cycle.ToolTip = page.GetResource("JourneyOptionTabContainer.cycle.ToolTip");
        }

        #endregion
    }
}