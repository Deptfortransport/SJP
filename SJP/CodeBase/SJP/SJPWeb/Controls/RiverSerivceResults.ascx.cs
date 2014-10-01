// *********************************************** 
// NAME             : RiverServiceResults.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 08 May 2011
// DESCRIPTION  	: User control representing river service departure results
// ************************************************


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SJP.Common;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.ResourceManager;
using SJP.Common.Web;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// User control representing river service departure results
    /// </summary>
    public partial class RiverSerivceResults : System.Web.UI.UserControl
    {
        #region Events

        // Selected journey event declaration
        public event OnSelectedJourneyChange SelectedJourneyHandler;

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

        protected string TXT_Select = string.Empty;
        protected string TXT_Departure = string.Empty;
        protected string TXT_Arrive = string.Empty;
        protected string TXT_Service = string.Empty;
        protected string TXT_JourneyTime = string.Empty;

        private string TXT_Hours = string.Empty;
        private string TXT_Hour = string.Empty;
        private string TXT_Minutes = string.Empty;
        private string TXT_Minute = string.Empty;

        private string TXT_EarlierJourney_Outward = string.Empty;
        private string TXT_EarlierJourney_Return = string.Empty;
        private string TXT_LaterJourney_Outward = string.Empty;
        private string TXT_LaterJourney_Return = string.Empty;

        #endregion

        private ISJPJourneyResult journeyResult = null;
        private ISJPJourneyRequest journeyRequest = null;
        private bool isReturn = false;
        // -1 indicates no journey set, 0 indicates user has intentionally not selected any journey (i.e all collapsed)
        private int selectedJourneyId = -1;

        // Indicates whether to show the earlier/later links
        private bool showEarlierLink = false;
        private bool showLaterLink = false;

        // Inidicates if any debug details should be shown
        private bool showDebug = false;

        // Indicates if the control should be rendered in a printer friendly mode
        private bool isPrinterFriendly = false;
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write. The selected river journey id
        /// </summary>
        public int SelectedJourneyId
        {
            get { return selectedJourneyId; }
            set { selectedJourneyId = value; }
        }
        
        #endregion

        #region  Page_Init, Page_Load, Page_PreRender

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
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
        /// StopEventResult repeater data bound event - populates the various controls with the stop event leg details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StopEventResult_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                #region Controls

                Label hdrSelect = e.Item.FindControlRecursive<Label>("hdrSelect");
                Label hdrDeparture = e.Item.FindControlRecursive<Label>("hdrDeparture");
                Label hdrArrive = e.Item.FindControlRecursive<Label>("hdrArrive");
                Label hdrService = e.Item.FindControlRecursive<Label>("hdrService");

                #endregion

                #region Set header text

                hdrSelect.Text = TXT_Select;
                hdrDeparture.Text = TXT_Departure;
                hdrArrive.Text = TXT_Arrive;
                hdrService.Text = TXT_Service;

                #endregion
            }

            else if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Journey journey = e.Item.DataItem as Journey;

                #region Controls
                HiddenField journeyId = e.Item.FindControlRecursive<HiddenField>("journeyId");

                GroupRadioButton radioSelect = e.Item.FindControlRecursive<GroupRadioButton>("select");
                Label departure = e.Item.FindControlRecursive<Label>("departure");
                Label service = e.Item.FindControlRecursive<Label>("service");
                Label arrive = e.Item.FindControlRecursive<Label>("arrive");

                #endregion

                #region Set journey summary values

                journeyId.Value = journey.JourneyId.ToString();

                radioSelect.GroupName = isReturn ? "return" : "outward";

                // Select the first one by default
                if (selectedJourneyId == 0 && e.Item.ItemIndex == 0)
                {
                    selectedJourneyId = journey.JourneyId;

                    radioSelect.Checked = true;

                    if (SelectedJourneyHandler != null)
                    {
                        SelectedJourneyHandler(sender, new JourneyEventArgs(selectedJourneyId));
                    }
                }
                else if (journey.JourneyId == selectedJourneyId)
                {
                    selectedJourneyId = journey.JourneyId;
                    radioSelect.Checked = true;

                    if (SelectedJourneyHandler != null)
                    {
                        SelectedJourneyHandler(sender, new JourneyEventArgs(selectedJourneyId));
                    }
                }

                departure.Text = GetTime(journey.StartTime,
                    isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime);

                arrive.Text = GetTime(journey.EndTime,
                    isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime);

                JourneyLeg leg = journey.JourneyLegs.FirstOrDefault();

                #region Service info
                if (leg != null)
                {
                    PublicJourneyTimedDetail legDetail = leg.JourneyDetails.FirstOrDefault() as PublicJourneyTimedDetail;

                    if (legDetail != null)
                    {
                        ServiceDetail serviceDetail = legDetail.Services.FirstOrDefault();

                        if (serviceDetail != null)
                        {
                            service.Text = serviceDetail.ServiceNumber;

                            if (showDebug)
                            {
                                service.Text += string.Format("<span class=\"debug\"> opCode[{0}] opName[{1}] privId[{2}] retId[{3}] direc[{4}] destBoard[{5}]</span>",
                                    serviceDetail.OperatorCode,
                                    serviceDetail.OperatorName,
                                    serviceDetail.PrivateId,
                                    serviceDetail.RetailId,
                                    serviceDetail.Direction,
                                    serviceDetail.DestinationBoard);
                            }
                        }
                    }

                }
                #endregion

                #endregion
            }
        }

        /// <summary>
        /// Stop Event result item radio button checked change event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StopEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepeaterItem row = ((Control)sender).NamingContainer as RepeaterItem;

            if (row != null)
            {
                GroupRadioButton radioSelect = row.FindControlRecursive<GroupRadioButton>("select");
                HiddenField journeyId = row.FindControlRecursive<HiddenField>("journeyId");
                if (radioSelect.Checked)
                {
                    int.TryParse(journeyId.Value, out selectedJourneyId);
                    if (SelectedJourneyHandler != null)
                    {
                        SelectedJourneyHandler(sender, new JourneyEventArgs(selectedJourneyId));
                    }
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
        /// <param name="isReturn">true or false determins wether the outward or return summary required</param>
        /// <param name="journeyId">Id of journey to be selected</param>
        /// <param name="printerFriendly">If control should be rendered in printer friendly mode</param>
        public void Initialise(ISJPJourneyRequest journeyRequest, ISJPJourneyResult journeyResult, bool isReturn,
            int journeyId, bool showEarlierLink, bool showLaterLink, bool printerFriendly)
        {
            this.journeyRequest = journeyRequest;
            this.journeyResult = journeyResult;
            this.isReturn = isReturn;
            this.selectedJourneyId = journeyId;
            this.showEarlierLink = showEarlierLink;
            this.showLaterLink = showLaterLink;
            this.isPrinterFriendly = printerFriendly;
            this.showDebug = DebugHelper.ShowDebug;

            InitialiseText();

            returnJourney.Value = isReturn.ToString();

            if (journeyResult != null)
            {
                journeyResult.SortJourneys();

                List<Journey> journeys = isReturn ? journeyResult.ReturnJourneys : journeyResult.OutwardJourneys;
                
                stopEventResult.DataSource = journeys;
                stopEventResult.DataBind();
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
            Language language = CurrentLanguage.Value;

            TXT_JourneyHeaderOutward = RM.GetString(language, RG, RC, "StopEventOutput.JourneyHeader.Outward.Text");
            TXT_JourneyHeaderReturn = RM.GetString(language, RG, RC, "StopEventOutput.JourneyHeader.Return.Text");

            TXT_JourneyDirectionOutward = RM.GetString(language, RG, RC, "StopEventOutput.JourneyDirection.Outward.Text");
            TXT_JourneyDirectionReturn = RM.GetString(language, RG, RC, "StopEventOutput.JourneyDirection.Return.Text");

            TXT_JourneyArriveBy = RM.GetString(language, RG, RC, "StopEventOutput.JourneyHeader.ArriveBy.Text");
            TXT_JourneyLeavingAt = RM.GetString(language, RG, RC, "StopEventOutput.JourneyHeader.LeavingAt.Text");

            TXT_Select = RM.GetString(language, RG, RC, "StopEventOutput.HeaderRow.Select.Text");
            TXT_Departure = RM.GetString(language, RG, RC, "StopEventOutput.HeaderRow.Departure.Text");
            TXT_Service = RM.GetString(language, RG, RC, "StopEventOutput.HeaderRow.Service.Text");
            TXT_Arrive = RM.GetString(language, RG, RC, "StopEventOutput.HeaderRow.Arrive.Text");
            TXT_JourneyTime = RM.GetString(language, RG, RC, "StopEventOutput.HeaderRow.JourneyTime.Text");

            TXT_Hours = RM.GetString(language, RG, RC, "JourneyOutput.Text.Hours");
            TXT_Hour = RM.GetString(language, RG, RC, "JourneyOutput.Text.Hour");
            TXT_Minutes = RM.GetString(language, RG, RC, "JourneyOutput.Text.Minutes");
            TXT_Minute = RM.GetString(language, RG, RC, "JourneyOutput.Text.Minute");

            TXT_EarlierJourney_Outward = RM.GetString(language, RG, RC, "JourneyOutput.EarlierService.Outward.Text");
            TXT_LaterJourney_Outward = RM.GetString(language, RG, RC, "JourneyOutput.LaterService.Outward.Text");
            TXT_EarlierJourney_Return = RM.GetString(language, RG, RC, "JourneyOutput.EarlierService.Return.Text");
            TXT_LaterJourney_Return = RM.GetString(language, RG, RC, "JourneyOutput.LaterService.Return.Text");
        }

        /// <summary>
        /// Returns the time
        /// </summary>
        public string GetTime(DateTime dateTime, DateTime requestDateTime)
        {
            if (dateTime.Second >= 30)
                dateTime = dateTime.AddMinutes(1);

            // Check to see if the date is different from the request date.
            // For example, if the user has searched for a journey commencing on
            // a Sunday, but the first available train is on a Monday
            if (dateTime.Day != requestDateTime.Day)
            {
                // Days are different, return the time with the dates appended
                return string.Format("{0}<br />({1})",
                    dateTime.ToShortTimeString(),
                    dateTime.ToShortDateString());
            }
            else
            {
                // Dates are the same, simply return the time.
                return dateTime.ToShortTimeString();
            }
        }

        /// <summary>
        /// Returns the journey time (duration)
        /// </summary>
        /// <returns></returns>
        private string GetJourneyTime(TimeSpan duration)
        {
            // The result journey time to return
            StringBuilder journeyTime = new StringBuilder();

            #region Determine hours/minutes

            //Get duration hours and minutes
            int durationHours = 0;
            int durationMinutes = 0;

            //Greater than 24 hours case
            if (duration.Days > 0)
            {
                // For each day, there are 24 hours
                durationHours = 24 * duration.Days + duration.Hours;
            }
            else if (duration.Hours != 0)
            {
                durationHours = duration.Hours;
            }

            durationMinutes = duration.Minutes;

            // Round up if necessary for consistency with start/end times.
            if (duration.Seconds >= 30)
                durationMinutes += 1;

            // If the rounding up of minutes takes it to 60,
            // then increment hours by 1 and set mins to zero
            if (durationMinutes == 60)
            {
                durationHours++;
                durationMinutes = 0;
            }

            #endregion

            #region Build journey time string

            if (durationHours > 0)
                journeyTime.Append(durationHours.ToString(CultureInfo.CurrentCulture.NumberFormat));

            if (durationHours == 1)
                journeyTime.Append(TXT_Hour);
            else if (durationHours > 1)
                journeyTime.Append(TXT_Hours);

            if (durationMinutes != 0)
            {
                // if hour was not equal to 0 then add a space
                if (durationHours != 0)
                    journeyTime.Append(" ");

                // Check to see if minutes requires a 0 padding.
                // Pad with 0 only if an hour was present and minute is a single digit.
                if (durationMinutes < 10 & durationHours != 0)
                    journeyTime.Append("0");

                journeyTime.Append(durationMinutes.ToString(CultureInfo.CurrentCulture.NumberFormat));

                if (durationMinutes > 1)
                    journeyTime.Append(TXT_Minutes);
                else
                    journeyTime.Append(TXT_Minute);

            }
            else if (durationHours == 0 && durationMinutes == 0)
            {
                // This leg has 0 hours 0 minutes, e.g. a journey to itself.
                // Should never really happen, but still required otherwise
                // no duration will be displayed.
                journeyTime.Append("0");
                journeyTime.Append(TXT_Minute);
            }

            #endregion

            return journeyTime.ToString();
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

                bool arriveBy = isReturn ? journeyRequest.ReturnArriveBefore : journeyRequest.OutwardArriveBefore;

                string resourceDateTime = arriveBy ? TXT_JourneyArriveBy : TXT_JourneyLeavingAt;

                stopEventHeader.Text = string.Format(
                        resource,
                        origin, destination);

                stopEventDateTime.Text = string.Format(resourceDateTime, journeyTime.ToString("dd/MM/yyyy HH:mm"));

                stopEventDirection.Text = isReturn ? TXT_JourneyDirectionReturn : TXT_JourneyDirectionOutward;
            }
        }

        /// <summary>
        /// Sets up the earlier/later replan links
        /// </summary>
        private void SetupReplanLinks()
        {
            // Only show links for River journeys
            if (journeyResult != null && journeyRequest != null
                && journeyRequest.PlannerMode == SJPJourneyPlannerMode.RiverServices
                && !isPrinterFriendly)
            {
                #region Earlier replan

                if (btnEarlierJourney != null)
                {
                    btnEarlierJourney.Text = isReturn ? TXT_EarlierJourney_Return : TXT_EarlierJourney_Outward;

                    #region Determine if button should be shown

                    // Initialise method should have set the show button flag (default is false)
                    bool show = showEarlierLink;

                    // Don't show the link if any journey departs before 00:00 of the request date
                    if (show)
                    {
                        List<Journey> journeys = isReturn ? journeyResult.ReturnJourneys : journeyResult.OutwardJourneys;

                        DateTime requestLeaveTime = isReturn ? journeyRequest.ReturnDateTime : journeyRequest.OutwardDateTime;

                        if ((journeys != null) && (journeys.Count > 0))
                        {
                            journeys.Sort(JourneyComparer.SortJourneyLeaveAfter);

                            // Leaving before request datetime (i.e. 00:00)
                            if (journeys[0].StartTime.Date < requestLeaveTime.Date)
                            {
                                show = false;
                            }
                        }
                    }

                    // Don't show the link if result contains error code indicating previous replan request failed
                    if (show)
                    {
                        if (journeyResult.Messages.Find(delegate(SJPMessage m){ return (m.MajorMessageNumber == Codes.SJPNoEarlierServicesOutward);}) 
                            != null)
                        {
                            if (!isReturn)
                            {
                                show = false;
                            }
                        }
                        else if (journeyResult.Messages.Find(delegate(SJPMessage m) { return (m.MajorMessageNumber == Codes.SJPNoEarlierServicesReturn); })
                            != null)
                        {
                            if (isReturn)
                            {
                                show = false;
                            }
                        }
                    }

                    // Don't show the link if more than configured number of journeys shown
                    if (show)
                    {
                        int maxJourneys = Properties.Current["RiverServiceResults.Replan.EarlierLink.MaxJourneys.Visible"].Parse(0);

                        if ((isReturn ? journeyResult.ReturnJourneys.Count : journeyResult.OutwardJourneys.Count) >= maxJourneys)
                        {
                            show = false;
                        }
                    }

                    // Don't show the button if properties have them turned off
                    if (show)
                    {
                        show = isReturn ?
                            Properties.Current["RiverServiceResults.Replan.EarlierLink.Visible.Return.Switch"].Parse(false) :
                            Properties.Current["RiverServiceResults.Replan.EarlierLink.Visible.Outward.Switch"].Parse(false);
                    }

                    #endregion

                    btnEarlierJourney.Visible = show;
                }

                #endregion

                #region Later replan

                if (btnLaterJourney != null)
                {
                    btnLaterJourney.Text = isReturn ? TXT_LaterJourney_Return : TXT_LaterJourney_Outward;

                    #region Determine if button should be shown

                    // Initialise method should have set the show button flag (default is false)
                    bool show = showLaterLink;
                    
                    // Don't show the link if any journey arrives after 03:00 of the next day of the request date
                    if (show)
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
                                show = false;
                            }
                        }
                    }

                    // Don't show the link if result contains error code indicating previous replan request failed
                    if (show)
                    {
                        if (journeyResult.Messages.Find(delegate(SJPMessage m) { return (m.MajorMessageNumber == Codes.SJPNoLaterServicesOutward); })
                            != null)
                        {
                            if (!isReturn)
                            {
                                show = false;
                            }
                        }
                        else if (journeyResult.Messages.Find(delegate(SJPMessage m) { return (m.MajorMessageNumber == Codes.SJPNoLaterServicesReturn); })
                            != null)
                        {
                            if (isReturn)
                            {
                                show = false;
                            }
                        }
                    }

                    // Don't show the link if more than configured number of journeys shown
                    if (show)
                    {
                        int maxJourneys = Properties.Current["RiverServiceResults.Replan.LaterLink.MaxJourneys.Visible"].Parse(0);

                        if ((isReturn ? journeyResult.ReturnJourneys.Count : journeyResult.OutwardJourneys.Count) >= maxJourneys)
                        {
                            show = false;
                        }
                    }

                    // Don't show the button if properties have them turned off
                    if (show)
                    {
                        show = isReturn ?
                            Properties.Current["RiverServiceResults.Replan.LaterLink.Visible.Return.Switch"].Parse(false) :
                            Properties.Current["RiverServiceResults.Replan.LaterLink.Visible.Outward.Switch"].Parse(false);
                    }

                    #endregion

                    btnLaterJourney.Visible = show;
                }

                #endregion
            }
        }

        #endregion
    }
}