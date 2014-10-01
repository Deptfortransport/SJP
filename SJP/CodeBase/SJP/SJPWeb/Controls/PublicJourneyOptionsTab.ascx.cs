// *********************************************** 
// NAME             : PublicJourneyOptionsTab.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: User control to represent public journey options
// ************************************************
                
                
using System;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// User control to represent public journey options
    /// </summary>
    public partial class PublicJourneyOptionsTab : System.Web.UI.UserControl, IJourneyOptionsTab
    {
        #region Private Fields
        private bool disabled = false;
        #endregion

        #region Events
        public event PlanJourney OnPlanJourney;
        #endregion

        #region Public Properties
        
        /// <summary>
        /// Read/Write property to determine if journey planning should exclude under grounds
        /// </summary>
        public bool ExcludeUnderGround
        {
            get { return excludeUnderground.Checked; }
            set { excludeUnderground.Checked = value; }
        }

        /// <summary>
        /// Read/Write property to determine if special assistance required during journey
        /// </summary>
        public bool Assistance
        {
            get { return assistance.Checked; }
            set { assistance.Checked = value; }
        }

        /// <summary>
        /// Read/Write property determining if step free required in journey
        /// </summary>
        public bool StepFree
        {
            get { return stepFree.Checked; }
            set { stepFree.Checked = value; }
        }

        /// <summary>
        /// Read/Write property determining if step free and assistance required in journey
        /// </summary>
        public bool StepFreeAndAssistance
        {
            get { return stepFreeAndAssistance.Checked; }
            set { stepFreeAndAssistance.Checked = value; }
        }

        /// <summary>
        /// Read/Write property to determine if journey planning should use fewer interchanges
        /// Returns true only if the "no accessible" option is not checked
        /// </summary>
        public bool FewerInterchanges
        {
            get { return (fewerInterchanges.Checked && !noMobilityNeeds.Checked); }
            set { fewerInterchanges.Checked = value; }
        }

        /// <summary>
        /// Read only property determining the planner mode represented by journey obtions tab
        /// </summary>
        public JourneyControl.SJPJourneyPlannerMode PlannerMode
        {
            get { return JourneyControl.SJPJourneyPlannerMode.PublicTransport; }
        }

        /// <summary>
        /// Read/Write property if the tab is disabled
        /// </summary>
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
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
            // default the option to no mobility options selected
            if (!IsPostBack && !ExcludeUnderGround && !StepFree && !Assistance && !StepFreeAndAssistance)
            {
                noMobilityNeeds.Checked = true;

                // Fewer changes default is unchecked
                fewerInterchanges.Checked = false;
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

            if (!Properties.Current["JourneyOptionTabContainer.PublicJourneyOptions.AccessibilityOptions.Visible"].Parse(true))
            {
                accessibilityOptions.Visible = false;
                ExcludeUnderGround = false;
                StepFree = false;
                Assistance = false;
                StepFreeAndAssistance = false;
                noMobilityNeeds.Checked = true;
                FewerInterchanges = false;
            }
            else
            {
                // Force show of mobility options (e.g. when javascript is disabled, then expand if option selected)
                if (!IsPostBack && (ExcludeUnderGround || StepFree || Assistance || StepFreeAndAssistance)
                    && mobilityOptions.Attributes["class"].Contains("collapsed"))
                {
                    ShowMobilityOptions();
                }
            }
        }

        #endregion
                
        #region Control Event Handlers

        /// <summary>
        /// Event handler for plan journey button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PublicJourneyPlanned(object sender, EventArgs e)
        {
            if (OnPlanJourney != null)
            {
                OnPlanJourney(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event handler for AdditionalMobilityNeeds image button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdditionalMobilityNeeds_Click(object sender, EventArgs e)
        {
            ShowMobilityOptions();
        }
                
        #endregion

        #region Private Methods

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            SJPPage page = (SJPPage)Page;

            if (disabled)
            {
                publicJourneyOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Disabled.ImageUrl");
                publicJourneyOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Disabled.AlternateText");
                publicJourneyOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Disabled.ToolTip");
            }
            else
            {
                publicJourneyOptions.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ImageUrl");
                publicJourneyOptions.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.AlternateText");
                publicJourneyOptions.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ToolTip");
                venueContent.InnerHtml = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Available.Information");
            }

            // Options
            excludeUnderground.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Text");
            assistance.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Text");
            stepFree.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Text");
            stepFreeAndAssistance.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Text");
            noMobilityNeeds.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Text");
            fewerInterchanges.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.FewerInterchanges.Text");

            // Information tooltips
            tooltipInfoImg_StepFreeAndAssistance.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.ImageUrl");
            tooltipInfoImg_StepFreeAndAssistance.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.AlternateText");
            tooltipInfoImg_StepFreeAndAssistance.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Information.ToolTip");
            tooltipInfo_StepFreeAndAssistance.Title = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Information.ToolTip");

            tooltipInfoImg_StepFree.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.ImageUrl");
            tooltipInfoImg_StepFree.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.AlternateText");
            tooltipInfoImg_StepFree.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Information.ToolTip");
            tooltipInfo_StepFree.Title = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Information.ToolTip");

            tooltipInfoImg_Assistance.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.ImageUrl");
            tooltipInfoImg_Assistance.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.AlternateText");
            tooltipInfoImg_Assistance.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Information.ToolTip");
            tooltipInfo_Assistance.Title = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Information.ToolTip");

            tooltipInfoImg_ExcludeUnderground.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.ImageUrl");
            tooltipInfoImg_ExcludeUnderground.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.AlternateText");
            tooltipInfoImg_ExcludeUnderground.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Information.ToolTip");
            tooltipInfo_ExcludeUnderground.Title = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Information.ToolTip");

            tooltipInfoImg_NoMobilityNeeds.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.ImageUrl");
            tooltipInfoImg_NoMobilityNeeds.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Information.AlternateText");
            tooltipInfoImg_NoMobilityNeeds.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Information.ToolTip");
            tooltipInfo_NoMobilityNeeds.Title = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Information.ToolTip");

            additionalMobilityNeeds.ImageUrl = page.ImagePath + page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.ImageUrl");
            additionalMobilityNeeds.AlternateText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.AlternateText");
            additionalMobilityNeeds.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.AdditionalMobilityNeeds.ToolTip");

            mobilityNeedsLabel.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.MobilityNeedsLabel.Text");

            lnkAccessibleTravel.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.AccessibleTravelLink.Text");
            lnkAccessibleTravel.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.AccessibleTravelLink.Text");
            lnkAccessibleTravel.NavigateUrl = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.AccessibleTravelLink.URL");

            PlanPublicJourney.Text = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.PlanPublicJourney.Text"));
            PlanPublicJourney.ToolTip = Server.HtmlDecode(page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.PlanPublicJourney.ToolTip"));


            PlanPublicJourney.Visible = true;
            PlanPublicJourney.Enabled = !disabled;

            if (disabled)
            {
                // Fully hide the submit button if the functionality is set to disabled
                PlanPublicJourney.Visible = false;
            }

        }

        /// <summary>
        /// Toggles the visibility of the mobility options
        /// </summary>
        private void ShowMobilityOptions()
        {
            if (mobilityOptions.Attributes["class"].Contains("collapsed"))
            {
                mobilityOptions.Attributes["class"] = mobilityOptions.Attributes["class"].Replace("collapsed", "expanded");
            }
            else
            {
                mobilityOptions.Attributes["class"] = mobilityOptions.Attributes["class"].Replace("expanded", "collapsed");
            }
        }

        #endregion



        
    }
}