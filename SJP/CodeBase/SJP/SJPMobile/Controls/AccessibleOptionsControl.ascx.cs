// *********************************************** 
// NAME             : AcessibleOptionsControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 15 Mar 2012
// DESCRIPTION  	: Accessible mobility options control
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.Web;
using SJP.Common.PropertyManager;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// Accessible mobility options control
    /// </summary>
    public partial class AccessibleOptionsControl : System.Web.UI.UserControl
    {
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

            // Accessible options functionality turned off
            if (!Properties.Current["PublicJourneyOptions.AccessibilityOptions.Visible"].Parse(true))
            {
                accessibilityOptions.Visible = false;

                ExcludeUnderGround = false;
                StepFree = false;
                Assistance = false;
                StepFreeAndAssistance = false;
                noMobilityNeeds.Checked = true;
            }
            
            SJPPageMobile page = (SJPPageMobile)Page;

            // Update accessible option heading to be the selected text
            // If mobile summary page, then show selected text,
            // Otherwise, only show it if user has manually selected
            if ((page.PageId == Common.PageId.MobileSummary)
                || (Page.IsPostBack && !string.IsNullOrEmpty(assistanceOptionSelected.Value)))
            {
                if (ExcludeUnderGround)
                {
                    mobiltyOptionsHeading.InnerText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Short.Text");
                    accessibilityOptionsBtn.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Short.Text");
                }
                else if (Assistance)
                {
                    mobiltyOptionsHeading.InnerText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Short.Text");
                    accessibilityOptionsBtn.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Short.Text");
                }
                else if (StepFree)
                {
                    mobiltyOptionsHeading.InnerText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Short.Text");
                    accessibilityOptionsBtn.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Short.Text");
                }
                else if (StepFreeAndAssistance)
                {
                    mobiltyOptionsHeading.InnerText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Short.Text");
                    accessibilityOptionsBtn.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Short.Text");
                }
                else
                {
                    mobiltyOptionsHeading.InnerText = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Short.Text");
                    accessibilityOptionsBtn.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Short.Text");
                }
            }
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// Event handler for AdditionalMobilityNeeds image button click event, should only be fired for non-js users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdditionalMobilityNeeds_Click(object sender, EventArgs e)
        {
            ShowMobilityOptions(true);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads resource strings for labels/controls
        /// </summary>
        private void SetupResources()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            // Accessible options headings
            mobiltyOptionsHeading.InnerText = page.GetResourceMobile("PublicJourneyOptions.MobiltyOptionsHeading.Text");
            mobiltyOptionsHeading.Attributes["title"] = page.GetResourceMobile("PublicJourneyOptions.MobiltyOptionsHeading.Text");

            accessibilityOptionsBtn.Text = page.GetResourceMobile("PublicJourneyOptions.MobiltyOptionsHeading.Text");
            accessibilityOptionsBtn.ToolTip = page.GetResourceMobile("PublicJourneyOptions.MobiltyOptionsHeading.Text");

            // Accessible options - Reuse existing SJPWeb text for consistency
            excludeUnderground.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Text");
            assistance.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Text");
            stepFree.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Text");
            stepFreeAndAssistance.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Text");
            noMobilityNeeds.Text = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Text");

            // Set tooltips which are used by the page javascript to update the Accessible options heading
            excludeUnderground.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.ExcludeUnderground.Short.Text");
            assistance.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.Assistance.Short.Text");
            stepFree.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFree.Short.Text");
            stepFreeAndAssistance.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.StepFreeAndAssistance.Short.Text");
            noMobilityNeeds.ToolTip = page.GetResource("JourneyOptionTabContainer.PublicJourneyOptions.NoMobilityNeeds.Short.Text");
        }

        /// <summary>
        /// Toggles the visibility of the mobility options, should only be fired for non-js users
        /// </summary>
        private void ShowMobilityOptions(bool hideHeadding)
        {
            if (mobilityOptions.Attributes["class"].Contains("jshide"))
            {
                mobilityOptions.Attributes["class"] = mobilityOptions.Attributes["class"].Replace("jshide", "jsshow");
            }
            else
            {
                mobilityOptions.Attributes["class"] = mobilityOptions.Attributes["class"].Replace("jsshow", "jshide");
            }

            // Hide the heading
            if (hideHeadding && !mobilityOptions.Attributes["class"].Contains("jshide"))
            {
                mobiltyOptionsHeading.Attributes["class"] = "jshide";
            }
        }

        #endregion
    }
}