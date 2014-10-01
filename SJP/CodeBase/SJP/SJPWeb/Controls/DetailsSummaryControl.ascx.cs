// *********************************************** 
// NAME             : DetailsSummaryControl.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 04 Apr 2011
// DESCRIPTION  	: Journey details summary control
// ************************************************


using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.Retail;

namespace SJP.UserPortal.SJPWeb.Controls
{
    #region Public Events

    // Delegate for selected journey
    public delegate void OnSelectedJourneyChange(object sender, JourneyEventArgs e);

    // Delegate for selected summary leg details being expanded/collapsed
    public delegate void OnSelectedJourneySummaryLegDetailsChange(object sender, JourneyLegEventArgs e);

    // Delegate for replan journey
    public delegate void OnReplanJourney(object sender, ReplanJourneyEventArgs e);

    #region Public Event classes

    /// <summary>
    /// EventsArgs class for passing journey id in OnSelectedJourneyChange event
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
        
        // Selected journey event declaration
        public event OnSelectedJourneyChange SelectedJourneyHandler;
                
        // Selected journey detail leg event declaration
        public event OnSelectedJourneySummaryLegDetailsChange SelectedJourneySummaryLegDetailsHandler;

        // Replan journey event declaration
        public event OnReplanJourney ReplanJourneyHandler;

        #endregion

        #region Private Fields

        #region Resources

        // Resource manager
        private static string RG = SJPResourceManager.GROUP_JOURNEYOUTPUT;
        private static string RC = SJPResourceManager.COLLECTION_JOURNEY;
        private SJPResourceManager RM = Global.SJPResourceManager;

        // Resource strings
        private string TXT_JourneyDirectionOutward = string.Empty;
        private string TXT_JourneyDirectionReturn = string.Empty;

        private string TXT_JourneyHeaderOutward = string.Empty;
        private string TXT_JourneyHeaderReturn = string.Empty;

        private string TXT_JourneyArriveBy = string.Empty;
        private string TXT_JourneyLeavingAt = string.Empty;

        protected string TXT_Transport = string.Empty;
        protected string TXT_Changes = string.Empty;
        protected string TXT_Leave = string.Empty;
        protected string TXT_Arrive = string.Empty;
        protected string TXT_JourneyTime = string.Empty;
        protected string TXT_Select = string.Empty;

        private string TXT_Close = string.Empty;
        private string TXT_CloseToolTip = string.Empty;

        private string URL_SelectJourney_Close = string.Empty;
        private string URL_SelectJourney_Open = string.Empty;
        private string ALTTXT_SelectJourney = string.Empty;
        private string TOOLTIP_SelectJourney = string.Empty;

        private string TXT_EarlierJourney_Outward = string.Empty;
        private string TXT_EarlierJourney_Return = string.Empty;
        private string TXT_LaterJourney_Outward = string.Empty;
        private string TXT_LaterJourney_Return = string.Empty;

        #endregion

        private SummaryInstructionAdapter adapter = null;

        private ISJPJourneyResult journeyResult = null;
        private ISJPJourneyRequest journeyRequest = null;
        private bool isReturn = false;
        // -1 indicates no journey set, 0 indicates user has intentionally not selected any journey (i.e all collapsed)
        private int selectedJourneyId = -1; 
        private int expandedIndex = -1;

        // Indicates if the journey leg has a "detail" element, whether to expand or collapse
        private bool legDetailExpanded = false;

        // Indicates if the control should be rendered in a printer friendly mode
        private bool isPrinterFriendly = false;

        // Indicates if the control should be rendered in accessible friendly mode
        private bool isAccessibleFriendly = false;
        
        #endregion

        #region Public Properties
        
        #endregion

        #region Constructor
        #endregion
        
        #region  Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in journeySummary.Items)
            {
                DetailsLegsControl legsControl = item.FindControlRecursive<DetailsLegsControl>("legsDetails");
                if (legsControl != null)
                {
                    legsControl.SelectedJourneyLegDetailsHandler += new OnSelectedJourneyLegDetailsChange(SelectedJourneyLegDetailsEventHandler);
                }

                HtmlTableRow detailsRow = item.FindControlRecursive<HtmlTableRow>("detailsRow");
                if (detailsRow != null)
                {
                    if (detailsRow.Attributes["class"].Contains("expanded"))
                    {
                        expandedIndex = item.ItemIndex;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
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
            if (e.Item.ItemType == ListItemType.Header)
            {
                #region Controls

                Label hdrTransport = e.Item.FindControlRecursive<Label>("hdrTransport");
                Label hdrChanges = e.Item.FindControlRecursive<Label>("hdrChanges");
                Label hdrLeave = e.Item.FindControlRecursive<Label>("hdrLeave");
                Label hdrArrive = e.Item.FindControlRecursive<Label>("hdrArrive");
                Label hdrJourneyTime = e.Item.FindControlRecursive<Label>("hdrJourneyTime");
                Label hdrSelect = e.Item.FindControlRecursive<Label>("hdrSelect");

                #endregion

                #region Set header text

                hdrTransport.Text = TXT_Transport;
                hdrChanges.Text = TXT_Changes;
                hdrLeave.Text = TXT_Leave;
                hdrArrive.Text = TXT_Arrive;
                hdrJourneyTime.Text = TXT_JourneyTime;
                hdrSelect.Text = TXT_Select;

                #endregion
            }

            else if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Journey journey = e.Item.DataItem as Journey;

                #region Controls

                Label lblTransport = e.Item.FindControlRecursive<Label>("lblTransport");

                Label changes = e.Item.FindControlRecursive<Label>("changes");
                Label journeyTime = e.Item.FindControlRecursive<Label>("journeyTime");
                Label leave = e.Item.FindControlRecursive<Label>("leave");
                Label arrive = e.Item.FindControlRecursive<Label>("arrive");
                DetailsLegsControl legsControl = e.Item.FindControlRecursive<DetailsLegsControl>("legsDetails");
                ImageButton selectJourney = e.Item.FindControlRecursive<ImageButton>("selectJourney");

                GroupRadioButton radioSelect = e.Item.FindControlRecursive<GroupRadioButton>("select");
                HtmlTableRow summaryRow = e.Item.FindControlRecursive<HtmlTableRow>("summaryRow");
                #endregion

                #region Close button

                Button btnClose = e.Item.FindControlRecursive<Button>("btnClose");

                // Hide the close button (retain code to allow reinsert if required in future)
                btnClose.Visible = false;
                btnClose.Text = TXT_Close;
                btnClose.ToolTip = TXT_CloseToolTip;

                #endregion

                #region Select button
                selectJourney.ImageUrl = URL_SelectJourney_Close;
                selectJourney.AlternateText = ALTTXT_SelectJourney;
                selectJourney.ToolTip = TOOLTIP_SelectJourney;
                #endregion

                #region Set journey summary values

                lblTransport.Visible = true;
                lblTransport.Text = adapter.GetTransport(journey.GetUsedModes());
                if (isPrinterFriendly)
                {
                    selectJourney.Visible = false;
                }

                changes.Text = journey.InterchangeCount.ToString();

                journeyTime.Text = adapter.GetJourneyTime(journey.StartTime, journey.EndTime);

                leave.Text = adapter.GetTime(journey.StartTime,
                    isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime);

                arrive.Text = adapter.GetTime(journey.EndTime,
                    isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime);

                legsControl.Initialise(journeyRequest, journey.JourneyLegs, journey.JourneyRoutingDetail, 
                    journey.AccessibleJourney, legDetailExpanded,
                    isPrinterFriendly, isAccessibleFriendly, journey.JourneyId, 
                    isReturn, journeyRequest.JourneyRequestHash);

                #endregion

                #region Set selected/default journey and expand it
                List<Journey> journeys = isReturn ? journeyResult.ReturnJourneys : journeyResult.OutwardJourneys;

                if (selectedJourneyId < 0)
                {

                    if (journeys.Count > 0)
                    {
                        selectedJourneyId = journeys[0].JourneyId;

                        // Raise event to tell subscribers selected journey has changed
                        if (SelectedJourneyHandler != null)
                        {
                            SelectedJourneyHandler(this, new JourneyEventArgs(selectedJourneyId));
                        }
                    }
                }

                // Expand the selected one
                HtmlTableRow detailsRow = e.Item.FindControlRecursive<HtmlTableRow>("detailsRow");

                if (selectedJourneyId == journey.JourneyId)
                {
                    if (detailsRow.Attributes["class"].Contains("collapsed"))
                    {
                        detailsRow.Attributes["class"] = detailsRow.Attributes["class"].Replace("collapsed", "expanded");
                        selectJourney.ImageUrl = URL_SelectJourney_Open;
                    }

                }



                #endregion

                #region Radio button
                radioSelect.ToolTip = TOOLTIP_SelectJourney;
                radioSelect.GroupName = isReturn ? "return" : "outward";
                JourneyHelper journeyHelper = new JourneyHelper();
                int radioJourneySelectedId = journeyHelper.GetJourneySelected(!isReturn);
                if (radioJourneySelectedId == journey.JourneyId)
                {
                    radioSelect.Checked = true;
                    summaryRow.Attributes["class"] = summaryRow.Attributes["class"] + " selected";
                }
                // If there is only one journey hide the radio button.
                radioSelect.Visible = journeys.Count > 1 && !isPrinterFriendly;


                #endregion

                #region Retailers Info (for client side)

                // retailers info for client side working of the retailer handoff
                HiddenField journeyRetailers = e.Item.FindControlRecursive<HiddenField>("journeyRetailers");
                HiddenField journeyId = e.Item.FindControlRecursive<HiddenField>("journeyId");

                journeyId.Value = journey.JourneyId.ToString();

                RetailerHelper helper = new RetailerHelper();
                List<Retailer> retailers = helper.GetRetailersForJourneys(isReturn ? null : journey, isReturn ? journey : null);
                foreach (Retailer retailer in retailers)
                {
                    journeyRetailers.Value += retailer.Id + ',';
                }

                journeyRetailers.Value = journeyRetailers.Value.Trim().TrimEnd(new char[] { ',' });

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
                int journeyId =  ((IButtonControl)e.CommandSource).CommandArgument.Parse(1);
                
                // Close all the current open detail rows
                foreach (RepeaterItem item in journeySummary.Items)
                {
                    if (item.ItemIndex != e.Item.ItemIndex)
                    {
                        HtmlTableRow detailItemRow =item.FindControlRecursive<HtmlTableRow>("detailsRow");

                        detailItemRow.Attributes["class"] = detailItemRow.Attributes["class"].Replace("expanded", "collapsed");
                    }
                }

                // Default to no journey selected
                selectedJourneyId = 0;

                // expand the select one
                HtmlTableRow detailsRow = e.Item.FindControlRecursive<HtmlTableRow>("detailsRow");
                HtmlTableRow summaryRow = e.Item.FindControlRecursive<HtmlTableRow>("summaryRow");

                if (expandedIndex != e.Item.ItemIndex)
                {
                    if (detailsRow.Attributes["class"].Contains("collapsed"))
                    {
                        detailsRow.Attributes["class"] = detailsRow.Attributes["class"].Replace("collapsed", "expanded");

                        // Journey selected
                        selectedJourneyId = journeyId;

                        summaryRow.Attributes["class"] = summaryRow.Attributes["class"] + " selected";
                    }
                    else if (detailsRow.Attributes["class"].Contains("expanded"))
                    {
                        detailsRow.Attributes["class"] = detailsRow.Attributes["class"].Replace("expanded", "collapsed");
                        summaryRow.Attributes["class"] = summaryRow.Attributes["class"].Replace("selected","").Trim();
                    }
                }
                else
                {
                    detailsRow.Attributes["class"] = detailsRow.Attributes["class"].Replace("expanded", "collapsed");
                    summaryRow.Attributes["class"] = summaryRow.Attributes["class"].Replace("selected", "").Trim();
                }

                

                // Raise event to tell subscribers selected journey has changed
                if (SelectedJourneyHandler != null)
                {
                    SelectedJourneyHandler(sender, new JourneyEventArgs(selectedJourneyId));
                }
            }
        }

        /// <summary>
        /// Handler for checked changed event of the radio button in the repeater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DetailSummary_Selected(object sender, EventArgs e)
        {
            RepeaterItem row = ((Control)sender).NamingContainer as RepeaterItem;

            if (row != null)
            {
                GroupRadioButton radioSelect = row.FindControlRecursive<GroupRadioButton>("select");
                HiddenField journeyId = row.FindControlRecursive<HiddenField>("journeyId");

                HtmlTableRow summaryRow = row.FindControlRecursive<HtmlTableRow>("summaryRow");
               

                if (!int.TryParse(journeyId.Value, out selectedJourneyId))
                {
                    selectedJourneyId = 0;
                }

                //remove selected from all the rows

                foreach (RepeaterItem item in journeySummary.Items)
                {
                    HtmlTableRow summaryRowItem = item.FindControlRecursive<HtmlTableRow>("summaryRow");

                    if (item != null)
                    {
                        summaryRowItem.Attributes["class"] = summaryRowItem.Attributes["class"].Replace("selected", "").Trim();
                    }
                }

                summaryRow.Attributes["class"] = summaryRow.Attributes["class"] + " selected";

                // Raise event to tell subscribers selected journey has changed
                if (SelectedJourneyHandler != null)
                {
                    SelectedJourneyHandler(sender, new JourneyEventArgs(selectedJourneyId));
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
        
        /// <summary>
        /// Handler for the expanded detail row's close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnClose_Click(object sender, EventArgs e)
        {
            if (expandedIndex >= 0 && expandedIndex < journeySummary.Items.Count)
            {
                RepeaterItem expandedItem = journeySummary.Items[expandedIndex];

                HtmlTableRow detailsRow = expandedItem.FindControlRecursive<HtmlTableRow>("detailsRow");

                detailsRow.Attributes["class"] = detailsRow.Attributes["class"].Replace("expanded", "collapsed");
            }

            // No journey selected
            selectedJourneyId = 0;

            // Raise event to tell subscribers selected journey has changed
            if (SelectedJourneyHandler != null)
            {
                SelectedJourneyHandler(sender, new JourneyEventArgs(selectedJourneyId));
            }
        }

        /// <summary>
        /// Event handler for SelectedJourneyLegHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedJourneyLegDetailsEventHandler(object sender, JourneyLegEventArgs e)
        {
            // Raise event to tell subscribers selected journey leg detail has changed
            if (SelectedJourneySummaryLegDetailsHandler != null)
            {
                SelectedJourneySummaryLegDetailsHandler(sender, e);
            }
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Initialises the detail summary control
        /// </summary>
        /// <param name="journeyRequest">SJP Journey request object</param>
        /// <param name="journeyResult">SJP Journey result object</param>
        /// <param name="isReturn">true or false determins wether the outward or return summary required</param>
        /// <param name="journeyId">Id of journey to be selected/expanded, 
        /// -1 to indicate let this control identify and select,
        /// 0 to indicate none shoule be selected/expanded </param>
        /// <param name="legDetailExpanded">If the journey leg has a "detail" element, whether to expand or collapse</param>
        /// <param name="printerFriendly">If control should be rendered in printer friendly mode</param>
        public void Initialise(ISJPJourneyRequest journeyRequest, ISJPJourneyResult journeyResult, bool isReturn,
            int journeyId, bool legDetailExpanded, bool printerFriendly, bool accessibleFriendly)
        {
            this.journeyRequest = journeyRequest;
            this.journeyResult = journeyResult;
            this.isReturn = isReturn;
            this.selectedJourneyId = journeyId;
            this.legDetailExpanded = legDetailExpanded;
            this.isPrinterFriendly = printerFriendly;
            this.isAccessibleFriendly = accessibleFriendly;

            this.adapter = new SummaryInstructionAdapter(Global.SJPResourceManager, false);

            InitialiseText();

            returnJourney.Value = isReturn.ToString();

            if (journeyResult != null)
            {
                List<Journey> journeys = isReturn ? journeyResult.ReturnJourneys : journeyResult.OutwardJourneys;

                journeySummary.DataSource = journeys;
                journeySummary.DataBind();
            }

            SetupJourneySummaryHeader();
        }
       
        #endregion

        #region Private Methods

        /// <summary>
        /// Method which initialises the text string used by this control
        /// </summary>
        private void InitialiseText()
        {
            SJPPage page = (SJPPage)Page;

            Language language = CurrentLanguage.Value;

            TXT_JourneyDirectionOutward = RM.GetString(language, RG, RC, "JourneyOutput.JourneyDirection.Outward.Text");
            TXT_JourneyDirectionReturn = RM.GetString(language, RG, RC, "JourneyOutput.JourneyDirection.Return.Text");

            TXT_JourneyHeaderOutward = RM.GetString(language, RG, RC, "JourneyOutput.JourneyHeader.Outward.Text");
            TXT_JourneyHeaderReturn = RM.GetString(language, RG, RC, "JourneyOutput.JourneyHeader.Return.Text");

            TXT_JourneyArriveBy = RM.GetString(language, RG, RC, "JourneyOutput.JourneyHeader.ArriveBy.Text");
            TXT_JourneyLeavingAt = RM.GetString(language, RG, RC, "JourneyOutput.JourneyHeader.LeavingAt.Text");

            TXT_Transport = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Transport.Text");
            TXT_Changes = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Changes.Text");
            TXT_Leave = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Leave.Text");
            TXT_Arrive = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Arrive.Text");
            TXT_JourneyTime = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.JourneyTime.Text");
            TXT_Select = RM.GetString(language, RG, RC, "JourneyOutput.HeaderRow.Select.Text");

            TXT_Close = RM.GetString(language, RG, RC, "JourneyOutput.DetailsRow.BtnClose.Text").ToLower();
            TXT_CloseToolTip = RM.GetString(language, RG, RC, "JourneyOutput.DetailsRow.BtnClose.ToolTip").ToLower();
                        
            URL_SelectJourney_Close = page.ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.SelectJourney.Close.ImageUrl");
            URL_SelectJourney_Open = page.ImagePath + RM.GetString(language, RG, RC, "JourneyOutput.SelectJourney.Open.ImageUrl");
            ALTTXT_SelectJourney = RM.GetString(language, RG, RC, "JourneyOutput.SelectJourney.AlternateText");
            TOOLTIP_SelectJourney = RM.GetString(language, RG, RC, "JourneyOutput.SelectJourney.ToolTip");

            TXT_EarlierJourney_Outward = RM.GetString(language, RG, RC, "JourneyOutput.EarlierJourney.Outward.Text");
            TXT_LaterJourney_Outward = RM.GetString(language, RG, RC, "JourneyOutput.LaterJourney.Outward.Text");
            TXT_EarlierJourney_Return = RM.GetString(language, RG, RC, "JourneyOutput.EarlierJourney.Return.Text");
            TXT_LaterJourney_Return = RM.GetString(language, RG, RC, "JourneyOutput.LaterJourney.Return.Text");
        }
        
        /// <summary>
        /// Sets the journey summary header text
        /// </summary>
        private void SetupJourneySummaryHeader()
        {
            if (journeyResult != null && journeyRequest != null)
            {
                string origin = isReturn ? journeyRequest.ReturnOrigin.DisplayName : journeyRequest.Origin.DisplayName;
                string destination = isReturn ? journeyRequest.ReturnDestination.DisplayName : journeyRequest.Destination.DisplayName;
                DateTime journeyTime = isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime;

                string resource = isReturn ? TXT_JourneyHeaderReturn : TXT_JourneyHeaderOutward;

                string resourceDateTime = isReturn ? TXT_JourneyLeavingAt : TXT_JourneyArriveBy;
                
                journeyHeader.Text = string.Format(
                        resource,
                        origin, destination);

                journeyDateTime.Text = string.Format(resourceDateTime, journeyTime.ToString("dd/MM/yyyy HH:mm"));

                journeyDirection.Text = isReturn ? TXT_JourneyDirectionReturn : TXT_JourneyDirectionOutward;
            }
        }

        /// <summary>
        /// Sets up the earlier/later replan links
        /// </summary>
        private void SetupReplanLinks()
        {
            // Hide all buttons by default
            btnEarlierJourney.Visible = false;
            btnLaterJourney.Visible = false;

            // Only show links for Public Transport journeys, others could be added in future
            if (journeyResult != null && journeyRequest != null
                && journeyRequest.PlannerMode == SJPJourneyPlannerMode.PublicTransport
                && !isPrinterFriendly)
            {
                #region Earlier replan

                if (btnEarlierJourney != null)
                {
                    btnEarlierJourney.Text = isReturn ? TXT_EarlierJourney_Return : TXT_EarlierJourney_Outward;

                    btnEarlierJourney.Visible = isReturn ?
                        Properties.Current["DetailsSummaryControl.Replan.EarlierLink.Visible.Return.Switch"].Parse(false) :
                        Properties.Current["DetailsSummaryControl.Replan.EarlierLink.Visible.Outward.Switch"].Parse(false);

                    // Don't show the link if more than configured number of journeys shown
                    if (btnEarlierJourney.Visible)
                    {
                        int maxJourneys = Properties.Current["DetailsSummaryControl.Replan.EarlierLink.MaxJourneys.Visible"].Parse(0);

                        if ((isReturn ? journeyResult.ReturnJourneys.Count : journeyResult.OutwardJourneys.Count) >= maxJourneys)
                        {
                            btnEarlierJourney.Visible = false;
                        }
                    }

                    // Don't show the link if any journey departs before 00:00 of the request date
                    if (btnEarlierJourney.Visible)
                    {
                        List<Journey> journeys = isReturn ? journeyResult.ReturnJourneys : journeyResult.OutwardJourneys;

                        DateTime requestLeaveTime = isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime;

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
                }

                #endregion

                #region Later replan

                if (btnLaterJourney != null)
                {
                    btnLaterJourney.Text = isReturn ? TXT_LaterJourney_Return : TXT_LaterJourney_Outward;

                    btnLaterJourney.Visible = isReturn ?
                        Properties.Current["DetailsSummaryControl.Replan.LaterLink.Visible.Return.Switch"].Parse(false) :
                        Properties.Current["DetailsSummaryControl.Replan.LaterLink.Visible.Outward.Switch"].Parse(false);

                    // Don't show the link if more than configured number of journeys shown
                    if (btnLaterJourney.Visible)
                    {
                        int maxJourneys = Properties.Current["DetailsSummaryControl.Replan.LaterLink.MaxJourneys.Visible"].Parse(0);

                        if ((isReturn ? journeyResult.ReturnJourneys.Count : journeyResult.OutwardJourneys.Count) >= maxJourneys)
                        {
                            btnLaterJourney.Visible = false;
                        }
                    }

                    // Don't show the link if any journey arrives after 03:00 of the next day of the request date
                    if (btnLaterJourney.Visible)
                    {
                        List<Journey> journeys = isReturn ? journeyResult.ReturnJourneys : journeyResult.OutwardJourneys;

                        DateTime requestLeaveTime = isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime;

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
                }

                #endregion
            }
        }
        
        #endregion
    }
}
