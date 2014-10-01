// *********************************************** 
// NAME             : DetailsSummaryControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2012
// DESCRIPTION  	: Journey details summary control
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.Common.LocationService;

namespace SJP.UserPortal.SJPMobile.Controls
{
    #region Public Events

    // Delegate for show details of journey
    public delegate void OnShowJourney(object sender, JourneyEventArgs e);

    // Delegate for replan journey
    public delegate void OnReplanJourney(object sender, ReplanJourneyEventArgs e);

    #region Public Event classes

    /// <summary>
    /// EventsArgs class for passing journey id in OnShowJourney event
    /// </summary>
    public class JourneyEventArgs : EventArgs
    {
        private readonly int journeyId;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="journeyId"></param>
        public JourneyEventArgs(int journeyId)
        {
            this.journeyId = journeyId;
        }

        /// <summary>
        /// Journey Id
        /// </summary>
        public int JourneyId
        {
            get { return journeyId; }
        }
    }
        
    /// <summary>
    /// EventsArgs class for passing replan event detail in OnReplanJourney event
    /// </summary>
    public class ReplanJourneyEventArgs : EventArgs
    {
        private readonly bool isEarlier = true;

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="isEarlier"></param>
        public ReplanJourneyEventArgs(bool isEarlier)
        {
            this.isEarlier = isEarlier;
        }

        /// <summary>
        /// Is Earlier
        /// </summary>
        public bool IsEarlier
        {
            get { return isEarlier; }
        }
    }

    #endregion

    #endregion

    /// <summary>
    /// Journey details summary control
    /// </summary>
    public partial class DetailsSummaryControl : System.Web.UI.UserControl
    {
        #region Public Events

        // Show journey event declaration
        public event OnShowJourney ShowJourneyHandler;

        // Replan journey event declaration
        public event OnReplanJourney ReplanJourneyHandler;

        #endregion

        #region Private Fields
        
        #region Resources

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private static string RG_Mobile = SJPResourceManager.GROUP_MOBILE;
        private static string RC_Mobile = SJPResourceManager.COLLECTION_DEFAULT;
        private SJPResourceManager RM = Global.SJPResourceManager;

        // Resource strings
        protected string TXT_Transport = string.Empty;
        protected string TXT_Changes = string.Empty;
        protected string TXT_Leave = string.Empty;
        protected string TXT_Arrive = string.Empty;
        protected string TXT_JourneyTime = string.Empty;

        protected string TXT_JourneysCount = string.Empty;
        protected string TXT_JourneyCount = string.Empty;
        protected string TXT_Show = string.Empty;
        protected string TXT_Show_ToolTip = string.Empty;

        private string TXT_EarlierJourney_Outward = string.Empty;
        private string TXT_EarlierJourney_Outward_ToolTip = string.Empty;
        private string TXT_LaterJourney_Outward = string.Empty;
        private string TXT_LaterJourney_Outward_ToolTip = string.Empty;

        #endregion

        private SummaryInstructionAdapter adapter = null;

        private ISJPJourneyResult journeyResult = null;
        private ISJPJourneyRequest journeyRequest = null;

        #endregion

        #region  Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Adapter for summary instructions
            adapter = new SummaryInstructionAdapter(Global.SJPResourceManager, true);

            SetupResources();
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupControls();

            SetupSummaryHead();

            SetupReplanLinks();
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// JourneySummary repeater data bound event - populates the various controls with the journey leg details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void JourneySummary_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Journey journey = e.Item.DataItem as Journey;

                #region Controls

                Label transport = e.Item.FindControlRecursive<Label>("transport");
                Label changes = e.Item.FindControlRecursive<Label>("changes");
                Label journeyTime = e.Item.FindControlRecursive<Label>("journeyTime");
                Label leave = e.Item.FindControlRecursive<Label>("leave");
                Label arrive = e.Item.FindControlRecursive<Label>("arrive");

                System.Web.UI.WebControls.LinkButton showDetailsBtn = e.Item.FindControlRecursive<System.Web.UI.WebControls.LinkButton>("showDetailsBtn");
                System.Web.UI.WebControls.Button showDetailsBtnNonJS = e.Item.FindControlRecursive<System.Web.UI.WebControls.Button>("showDetailsBtnNonJS");
                                                
                #endregion

                #region Set journey summary values

                transport.Text = adapter.GetTransport(journey.GetUsedModes());

                changes.Text = string.Format("{0} {1}", 
                    journey.InterchangeCount.ToString(),
                    TXT_Changes);

                journeyTime.Text = adapter.GetJourneyTime(journey.StartTime, journey.EndTime);

                leave.Text = string.Format("{0}: {1}", 
                    TXT_Leave,
                    adapter.GetTime(journey.StartTime, journeyRequest.OutwardDateTime));

                arrive.Text = string.Format("{0}: {1}",
                    TXT_Arrive,
                    adapter.GetTime(journey.EndTime, journeyRequest.OutwardDateTime));

                #endregion

                #region Show journey details button

                // Above labels are shown in the link button as the text
                //showDetailsBtn.Text = TXT_Show;
                showDetailsBtn.ToolTip = TXT_Show_ToolTip;
                showDetailsBtnNonJS.Text = TXT_Show;
                showDetailsBtnNonJS.ToolTip = TXT_Show_ToolTip;

                // Set the journey id for the button click event handler 
                showDetailsBtn.CommandArgument = journey.JourneyId.ToString();
                showDetailsBtnNonJS.CommandArgument = journey.JourneyId.ToString();

                #endregion
            }
        }

        /// <summary>
        /// JourneySummary repeater item command event handler - identifies the selected journey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void JourneySummary_Command(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("select"))
            {
                int journeyId = ((IButtonControl)e.CommandSource).CommandArgument.Parse(1);
            }
        }

        /// <summary>
        /// Handler for the show journey button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void showDetailsBtn_Click(object sender, EventArgs e)
        {
            if (sender is System.Web.UI.WebControls.LinkButton)
            {
                int journeyId = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument.Parse(1);

                // Raise event to tell subscribers to that show journey button has been selected
                if (ShowJourneyHandler != null)
                {
                    ShowJourneyHandler(sender, new JourneyEventArgs(journeyId));
                }
            }
        }

        /// <summary>
        /// Handler for the show journey button non js click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void showDetailsBtnNonJS_Click(object sender, EventArgs e)
        {
            if (sender is System.Web.UI.WebControls.Button)
            {
                int journeyId = ((System.Web.UI.WebControls.Button)sender).CommandArgument.Parse(1);

                // Raise event to tell subscribers to that show journey button has been selected
                if (ShowJourneyHandler != null)
                {
                    ShowJourneyHandler(sender, new JourneyEventArgs(journeyId));
                }
            }
        }

        /// <summary>
        /// Handler for the earlier journey button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnEarlierJourney_Click(object sender, EventArgs e)
        {
            // Raise event to tell subscribers earlier journey button has been selected
            if (ReplanJourneyHandler != null)
            {
                ReplanJourneyHandler(sender, new ReplanJourneyEventArgs(true));
            }
        }

        /// <summary>
        /// Handler for the later journey button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnLaterJourney_Click(object sender, EventArgs e)
        {
            // Raise event to tell subscribers later journey button has been selected
            if (ReplanJourneyHandler != null)
            {
                ReplanJourneyHandler(sender, new ReplanJourneyEventArgs(false));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialises the detail summary control
        /// </summary>
        /// <param name="journeyRequest">SJP Journey request object</param>
        /// <param name="journeyResult">SJP Journey result object</param>
        public void Initialise(ISJPJourneyRequest journeyRequest, ISJPJourneyResult journeyResult)
        {
            this.journeyRequest = journeyRequest;
            this.journeyResult = journeyResult;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method which initialises the text string used by this control
        /// </summary>
        private void SetupResources()
        {
            Language language = CurrentLanguage.Value;

            TXT_Transport = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Transport.Text");
            TXT_Changes = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Changes.Text");
            TXT_Leave = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Leave.Text");
            TXT_Arrive = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Arrive.Text");
            TXT_JourneyTime = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.JourneyTime.Text");

            TXT_JourneysCount = RM.GetString(language, RG, RC, "JourneyOutput.SummaryRow.JourneysCount.Text");
            TXT_JourneyCount = RM.GetString(language, RG, RC, "JourneyOutput.SummaryRow.JourneyCount.Text");

            TXT_Show = RM.GetString(language, RG, RC, "JourneyOutput.SelectJourney.Show.Text");

            // Mobile specific
            TXT_Show_ToolTip = RM.GetString(language, RG_Mobile, RC_Mobile, "JourneyOutput.SelectJourney.Show.ToolTip");
            TXT_EarlierJourney_Outward = RM.GetString(language, RG_Mobile, RC_Mobile, "JourneySummary.EarlierJourney.Outward.Text");
            TXT_EarlierJourney_Outward_ToolTip = RM.GetString(language, RG_Mobile, RC_Mobile, "JourneySummary.EarlierJourney.Outward.ToolTip");
            TXT_LaterJourney_Outward = RM.GetString(language, RG_Mobile, RC_Mobile, "JourneySummary.LaterJourney.Outward.Text");
            TXT_LaterJourney_Outward_ToolTip = RM.GetString(language, RG_Mobile, RC_Mobile, "JourneySummary.LaterJourney.Outward.ToolTip");
        }

        /// <summary>
        /// Method which setup the controls
        /// </summary>
        private void SetupControls()
        {
            if (journeyResult != null && journeyRequest != null)
            {
                // Get journeys
                List<Journey> journeys = journeyResult.OutwardJourneys;

                // Sort:
                // journeys to venue: show latest arrival first
                // or journeys from venue: show earliest depart first.
                // (If the sort is changed here, ensure JourneyPageControl.ascx sort is updated as well)
                if (journeyRequest.Destination is SJPVenueLocation)
                {
                    journeys.Sort(JourneyComparer.SortJourneyArriveBy);
                }
                else
                {
                    journeys.Sort(JourneyComparer.SortJourneyLeaveAfter);                    
                }

                journeySummary.DataSource = journeys;
                journeySummary.DataBind();
            }
        }

        /// <summary>
        /// Method to setup the summary head
        /// </summary>
        private void SetupSummaryHead()
        {
            if (journeyResult != null)
            {
                lblJourneyCount.Text = string.Format(
                    (journeyResult.OutwardJourneys.Count > 1) ? TXT_JourneysCount : TXT_JourneyCount, 
                    journeyResult.OutwardJourneys.Count);
            }
        }

        /// <summary>
        /// Sets up the earlier/later replan links
        /// </summary>
        private void SetupReplanLinks()
        {
            // Only show links for Public Transport journeys, others could be added in future
            if (journeyResult != null && journeyRequest != null
                && journeyRequest.PlannerMode == SJPJourneyPlannerMode.PublicTransport)
            {
                #region Earlier replan

                if (btnEarlierJourney != null)
                {
                    btnEarlierJourney.Text = TXT_EarlierJourney_Outward;
                    btnEarlierJourney.ToolTip = TXT_EarlierJourney_Outward_ToolTip;
                    
                    btnEarlierJourney.Visible = Properties.Current["DetailsSummaryControl.Replan.EarlierLink.Visible.Outward.Switch"].Parse(false);

                    // Don't show the link if more than configured number of journeys shown
                    if (btnEarlierJourney.Visible)
                    {
                        int maxJourneys = Properties.Current["DetailsSummaryControl.Replan.EarlierLink.MaxJourneys.Visible"].Parse(0);

                        if (journeyResult.OutwardJourneys.Count >= maxJourneys)
                        {
                            btnEarlierJourney.Visible = false;
                        }
                    }

                    // Don't show the link if any journey departs before 00:00 of the request date
                    if (btnEarlierJourney.Visible)
                    {
                        List<Journey> journeys = journeyResult.OutwardJourneys;

                        DateTime requestLeaveTime = journeyRequest.OutwardDateTime;

                        if ((journeys != null) && (journeys.Count > 0))
                        {
                            journeys.Sort(JourneyComparer.SortJourneyLeaveAfter);

                            // Leaving before request datetime (i.e. 00:00)
                            if (journeys[0].StartTime.Date < requestLeaveTime.Date)
                            {
                                btnEarlierJourney.Visible = false;
                            }
                        }
                    }

                    earlierJourneyDiv.Visible = btnEarlierJourney.Visible;
                }

                #endregion

                #region Later replan

                if (btnLaterJourney != null)
                {
                    btnLaterJourney.Text = TXT_LaterJourney_Outward;
                    btnLaterJourney.ToolTip = TXT_LaterJourney_Outward_ToolTip;

                    btnLaterJourney.Visible = Properties.Current["DetailsSummaryControl.Replan.LaterLink.Visible.Outward.Switch"].Parse(false);

                    // Don't show the link if more than configured number of journeys shown
                    if (btnLaterJourney.Visible)
                    {
                        int maxJourneys = Properties.Current["DetailsSummaryControl.Replan.LaterLink.MaxJourneys.Visible"].Parse(0);

                        if (journeyResult.OutwardJourneys.Count >= maxJourneys)
                        {
                            btnLaterJourney.Visible = false;
                        }
                    }

                    // Don't show the link if any journey arrives after 03:00 of the next day of the request date
                    if (btnLaterJourney.Visible)
                    {
                        List<Journey> journeys = journeyResult.OutwardJourneys;

                        DateTime requestLeaveTime = journeyRequest.OutwardDateTime;

                        if ((journeys != null) && (journeys.Count > 0))
                        {
                            journeys.Sort(JourneyComparer.SortJourneyArriveBy);

                            requestLeaveTime = requestLeaveTime.AddDays(1);
                            requestLeaveTime = new DateTime(requestLeaveTime.Year, requestLeaveTime.Month, requestLeaveTime.Day, 3, 0, 0);

                            // Arrives after 03:00 day after request datetime (i.e. 00:00)
                            if (journeys[0].EndTime > requestLeaveTime)
                            {
                                btnLaterJourney.Visible = false;
                            }
                        }
                    }

                    laterJouneyDiv.Visible = btnLaterJourney.Visible;
                }

                #endregion
            }
            else if (journeyRequest.PlannerMode != SJPJourneyPlannerMode.PublicTransport)
            {
                // Always hide for non-PT journey planner mode
                btnEarlierJourney.Visible = false;
                btnLaterJourney.Visible = false;

                earlierJourneyDiv.Visible = btnEarlierJourney.Visible;
                laterJouneyDiv.Visible = btnLaterJourney.Visible;
            }
            else
            {
                // Journey request/result may only be populated on first load, so button visibility is already set
                if (!IsPostBack)
                {
                    btnEarlierJourney.Visible = false;
                    btnLaterJourney.Visible = false;

                    earlierJourneyDiv.Visible = btnEarlierJourney.Visible;
                    laterJouneyDiv.Visible = btnLaterJourney.Visible;
                }
            }

            btnEarlierJourneyNonJS.Text = btnEarlierJourney.Text;
            btnEarlierJourneyNonJS.ToolTip = btnEarlierJourney.ToolTip;
            btnEarlierJourneyNonJS.Visible = btnEarlierJourney.Visible;

            btnLaterJourneyNonJS.Text = btnLaterJourney.Text;
            btnLaterJourneyNonJS.ToolTip = btnLaterJourney.ToolTip;
            btnLaterJourneyNonJS.Visible = btnLaterJourney.Visible;
        }

        #endregion
    }
}
