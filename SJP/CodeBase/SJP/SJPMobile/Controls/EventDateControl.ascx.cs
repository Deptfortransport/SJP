// *********************************************** 
// NAME             : EventDateControl.ascx.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 06 Feb 2012
// DESCRIPTION  	: Event Date User control
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;
using System.Web.UI.HtmlControls;

namespace SJP.UserPortal.SJPMobile.Controls
{
    /// <summary>
    /// Event Date User control
    /// </summary>
    public partial class EventDateControl : System.Web.UI.UserControl
    {
        #region Private Fields

        private const string DATE_FORMAT = "dd/MM/yyyy";

        private DateTime outwardDateTime = DateTime.MinValue;
        private bool forceUpdate = false;
        private bool validInputDateChange = true;
        private bool arriveBy = true;
        private bool isNow = false;
        private bool isToVenue = false;
        private bool resetTime = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write. Outward datetime
        /// </summary>
        public DateTime OutwardDateTime
        {
            get { return outwardDateTime = GetEventDateTime(); }
            set { outwardDateTime = value; }
        }

        /// <summary>
        /// Read/Write property indicates if the datetime should be updated
        /// (e.g. if in the past)
        /// </summary>
        public bool ForceUpdate
        {
            get { return forceUpdate; }
            set { forceUpdate = value; }
        }

        /// <summary>
        /// Read/Write. Flag used to control if the date time is arrive by or leave at.
        /// Default is arrive by
        /// </summary>
        public bool ArriveBy
        {
            get
            {
                if (isNow)
                    return false;
                else
                    return arriveBy;
            }
            set { arriveBy = value; }
        }

        /// <summary>
        /// Read/Write. Flag to indicate if the event date is being shown for a ToVenue location input mode
        /// </summary>
        public bool IsToVenue
        {
            get { return isToVenue; }
            set { isToVenue = value; }
        }

        public bool ResetTime
        {
            get { return resetTime; }
            set { resetTime = true; }
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
            InitCalendar();

            // attempt to get the date time from the control, will be Date.MinValue if the control doesn't contain a date.
            outwardDateTime = GetEventDateTime();

            if (IsPostBack)
            {
                // Read flags.
                // Not reading isToVenueFlag as that is only required by the javascript as when
                // changing the date/time the now flag must be reset and the arrive by flag updated
                // depending on the location input "to venue" mode
                arriveBy = isArriveByFlag.Value.Parse(true);
                isNow = isNowFlag.Value.Parse(false);
            }
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetupResourceStrings();

            SetupNowLink();

            SetupDateTimeStyle();

            SetJourneyDateTime();

            // Persist flags for next postback
            isArriveByFlag.Value = ArriveBy.ToString().ToLower();
            isNowFlag.Value = isNow.ToString().ToLower();
            isToVenueFlag.Value = isToVenue.ToString().ToLower();
        }

        #endregion

        #region Control Event Handlers
        
        /// <summary>
        /// Outward date input box text change event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OutwardDate_Changed(object sender, EventArgs e)
        {
            DateTime outDate = DateTime.MinValue;

            if (!DateTime.TryParse(outwardDate.Text.Trim(), out outDate))
            {
                validInputDateChange = false;
            }
            else
            {
                outwardDateTime = GetEventDateTime();
            }
        }

        /// <summary>
        /// Now link click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void nowLink_Click(object sender, EventArgs e)
        {
            outwardDateTime = DateTime.Now;

            // Update flags, when now is selected, the date time is a leave at
            arriveBy = false;
            isNow = true;
        }
                
        #endregion

        #region Private Methods

        /// <summary>
        /// Sets resource strings
        /// </summary>
        private void SetupResourceStrings()
        {
            SJPPageMobile page = (SJPPageMobile)Page;

            outwardDateLabel.Text = page.GetResourceMobile("JourneyInput.Date.Outward.Text");
            outwardTimeLabel.Text = page.GetResourceMobile("JourneyInput.Time.Outward.Text");
            timeSelectorLabel.InnerText = page.GetResourceMobile("JourneyInput.TimePage.SelectTime.Text");
            dateSelectorLabel.InnerText = page.GetResourceMobile("JourneyInput.DatePage.SelectDay.Text");
            closetime.Text = page.GetResourceMobile("JourneyInput.Back.Text");
            closedate.Text = page.GetResourceMobile("JourneyInput.Back.Text");
            nowLink.Text = page.GetResourceMobile("JourneyInput.Now.Link.Text");
            nowLink.ToolTip = page.GetResourceMobile("JourneyInput.Now.Link.ToolTip");
            nowLinkNonJS.Text = nowLink.Text;
            nowLinkNonJS.ToolTip = nowLink.ToolTip;

            string arriveByText = page.GetResourceMobile("JourneyInput.ArriveBy.Text");
            string leaveAtText = page.GetResourceMobile("JourneyInput.LeaveAt.Text");

            // Show the correct text, the javascript may update displayed text as appropriate if it changes date times
            eventDateLabel.Text = ArriveBy ?
                string.Format("<span class=\"show\">{0}</span><span class=\"hide\">{1}</span>", arriveByText, leaveAtText) :
                string.Format("<span class=\"show\">{0}</span><span class=\"hide\">{1}</span>", leaveAtText, arriveByText);

            // Set up default display text for the date and time text boxes when not set
            outwardDate.Text = page.GetResourceMobile("JourneyInput.Date.SetDate.Text");
            outwardDate.ToolTip = page.GetResourceMobile("JourneyInput.Date.SetDate.ToolTip");

            outwardTime.Text = ArriveBy ?
                page.GetResourceMobile("JourneyInput.Time.ArrivalTime.Text") :
                page.GetResourceMobile("JourneyInput.Time.DepartureTime.Text");

            outwardTime.ToolTip = ArriveBy ?
                page.GetResourceMobile("JourneyInput.Time.ArrivalTime.ToolTip") :
                page.GetResourceMobile("JourneyInput.Time.DepartureTime.ToolTip");
        }

        /// <summary>
        /// Sets up the now date and time link
        /// </summary>
        private void SetupNowLink()
        {
            if (Properties.Current["EventDateControl.Now.Link.Switch"].Parse(false))
            {
                nowSelectDiv.Visible = true;
            }
            else
            {
                nowSelectDiv.Visible = false;
            }
        }

        /// <summary>
        /// Adds the depart or arrive style the date time inputs
        /// </summary>
        private void SetupDateTimeStyle()
        {
            if (ArriveBy)
            {
                outwardDate.CssClass = outwardDate.CssClass.Replace("arrivalDate", "");
                outwardDate.CssClass += outwardDate.CssClass.Contains(" departureDate") ? "" : " departureDate";
                outwardTime.CssClass = outwardTime.CssClass.Replace("arrivalTime", "");
                outwardTime.CssClass += outwardTime.CssClass.Contains(" departureTime") ? "" : " departureTime";
            }
            else
            {
                outwardDate.CssClass = outwardDate.CssClass.Replace("departureDate", "");
                outwardDate.CssClass += outwardDate.CssClass.Contains(" arrivalDate") ? "" : " arrivalDate";
                outwardTime.CssClass = outwardTime.CssClass.Replace("departureTime", "");
                outwardTime.CssClass += outwardTime.CssClass.Contains(" arrivalTime") ? "" : " arrivalTime";
            }
        }

        /// <summary>
        /// Populated the arrive and leave time dropdowns
        /// </summary>
        private void PopulateJourneyTimeDropDowns()
        {
            if (timeRadioList.Items.Count == 0)
            {
                List<ListItem> timeList = new List<ListItem>();
                DateTime startTime = DateTime.Now.Date;

                int interval = Properties.Current["EventDateControl.DropDownTime.IntervalMinutes"].Parse(15);

                var times = from hour in Enumerable.Range(0, 24) // each hour in 0 -23
                            from minute in Enumerable.Range(0, 59) // each minute in 0 - 59
                            where minute % interval == 0 // get the minute with the interval required
                            // format the time and return the ListItem populated with it
                            select new ListItem(string.Format("{0:00}:{1:00}", hour, minute),
                                string.Format("{0:00}:{1:00}", hour, minute));

                // create time lists of ListItems
                timeList = times.ToList();

                // Add the ListItems to the dropdowns
                timeRadioList.Items.AddRange(timeList.ToArray());

                timeRadioList.Items.FindByValue(Properties.Current["EventDateControl.DropDownTime.Outward.Default"]).Selected = true;
            }
        }

        /// <summary>
        /// Builds a datetime object out of the date control and time dropdowns.
        /// </summary>
        /// <returns>Returns a DateTime object, set to MinValue if the control is not set</returns>
        private DateTime GetEventDateTime()
        {
            // If there was a landing page with auto plan true, then need to ensure this control is initialised 
            // (otherwise values can be empty and cause errors here)
            if (forceUpdate)
            {
                PopulateJourneyTimeDropDowns();
                SetJourneyDateTime();
            }

            DateTime date = DateTime.MinValue;

            // check if the date is set
            DateTime.TryParseExact(outwardDate.Text.Trim(),
                    DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            RadioButtonList timeList = timeRadioList;

            string[] timeparts = timeList.SelectedValue.Split(new char[] { ':' });
            int minute = 0;
            int hour = 0;

            if (int.TryParse(timeparts[0], out hour) && int.TryParse(timeparts[1], out minute))
            {
                date = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
            }

            return date;
        }

        /// <summary>
        /// Sets the date control and time dropdown to represent the journey date
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isReturn"></param>
        private void SetJourneyDateTime()
        {
            if (ValidJourneyDateTime(outwardDateTime, forceUpdate))
            {
                outwardDate.Text = outwardDateTime.ToString(DATE_FORMAT);

                timeRadioList.SelectedItem.Selected = false;

                string dropDownTime = GetDropDownTime(outwardDateTime);

                timeRadioList.Items.FindByValue(dropDownTime).Selected = true;
                outwardTime.Text = dropDownTime;
            }

            //if (validInputDateChange)
            //{
            //    outwardDate.Text = outwardDateTime.ToString(DATE_FORMAT);
            //}
        }

        /// <summary>
        /// Gets the dropdown time string based on the date and time specified
        /// </summary>
        /// <param name="dropdownTime">DateTime object of which time needs setting in drop down</param>
        /// <returns>string in HH:mm format</returns>
        private string GetDropDownTime(DateTime dropdownTime)
        {
            int interval = Properties.Current["EventDateControl.DropDownTime.IntervalMinutes"].Parse(15);

            int dropDownTime_Minute_Itervarl_Multiplier = (int)(dropdownTime.Minute / interval);

            int dropDownTime_Minutes = 0;

            if ((dropdownTime.Minute % interval) > 0)
            {
                dropDownTime_Minutes = (dropDownTime_Minute_Itervarl_Multiplier + 1) * interval;
            }
            else
            {
                dropDownTime_Minutes = dropDownTime_Minute_Itervarl_Multiplier * interval;
            }
            
            return string.Format("{0:00}:{1:00}",
                dropDownTime_Minutes >= 60 ?
                    (((dropdownTime.Hour + 1) >= 24) ? 00 : dropdownTime.Hour + 1) : dropdownTime.Hour, 
                dropDownTime_Minutes >= 60 ? 00 : dropDownTime_Minutes);
        }

        /// <summary>
        /// Validates the journey date times
        /// </summary>
        /// <param name="selectedOutwardDateTime"></param>
        /// <param name="forceUpdate">If true, the selected datetime is updated to now if it is in the past</param>
        /// <returns></returns>
        private bool ValidJourneyDateTime(DateTime selectedDateTime, bool forceUpdate)
        {
            bool validDate = true;

            if (selectedDateTime == DateTime.MinValue)
                validDate = false;
            // Date part could be invalid (01/01/0001)
            else if (selectedDateTime.Date == DateTime.MinValue.Date)
                validDate = false;
            // Caller might have reset time part only
            else if (resetTime && selectedDateTime.TimeOfDay == DateTime.MinValue.TimeOfDay)
            {
                validDate = false;

                // Ensure date input displays the date part
                outwardDate.Text = selectedDateTime.ToString(DATE_FORMAT);
            }

            if (validDate && selectedDateTime < DateTime.Now && forceUpdate)
            {
                outwardDateTime = DateTime.Now;
            }
            
            return validDate;
        }

        /// <summary>
        /// Sets up caledar control default dates
        /// </summary>
        private void InitCalendar()
        {
            if (!IsPostBack)
            {
                // first populate the dropdown items
                PopulateJourneyTimeDropDowns();
            }

            #region Set calendar dates

            // Set the start to end dates to display for the calendar. This will be the period for which
            // journeys can be planned for the games
            DateTime startDate = Properties.Current["JourneyPlanner.Validate.Games.StartDate"].Parse(DateTime.Now.Date);
            DateTime endDate = Properties.Current["JourneyPlanner.Validate.Games.EndDate"].Parse(DateTime.Now.Date);

            calendarStartDate.Value = startDate.ToString(DATE_FORMAT);
            calendarEndDate.Value = endDate.ToString(DATE_FORMAT);

            #endregion

            if (!IsPostBack)
            {
                #region Set the default selected outward and return datetimes

                DateTime calendarSelectedDate = outwardDateTime;
                // Set the initial selected outward date value
                if (calendarSelectedDate == DateTime.MinValue)
                {
                    // Get the default outward selected time value
                    string[] timeparts = Properties.Current["EventDateControl.DropDownTime.Outward.Default"].Split(new char[] { ':' });
                    int minute = timeparts[1].Parse(0);
                    int hour = timeparts[0].Parse(0);

                    DateTime outwardStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, hour, minute, 0);

                    calendarSelectedDate = DateTime.Now >= outwardStartDate ? DateTime.Now : outwardStartDate;

                    // Ensure calendar start date is Now or later
                    calendarStartDate.Value = calendarSelectedDate.ToString(DATE_FORMAT);
                }
                // Setup the selectable calendar
                SetupCalendar(startDate, endDate, calendarSelectedDate);

                #endregion
            }
        }

        /// <summary>
        /// Sets up the selectable calendar dates
        /// </summary>
        private void SetupCalendar(DateTime start, DateTime end, DateTime selected)
        {
            DateTime currentMonth = start;

            List<object> monthList = new List<object>();

            // The following code builds up complete "months" with Mon to Sun dates, 
            // which can include dates from previous month and next which are "disabled"
            while (currentMonth <= end)
            {
                DateTime monthStart = currentMonth.AddDays(1 - (int)currentMonth.DayOfWeek);
                DateTime monthEnd = new DateTime(currentMonth.Year, currentMonth.Month, 1).AddMonths(1).AddDays(-1);

                List<object> dateList = new List<object>();

                if (monthEnd > end)
                {
                    monthEnd = end;
                }

                monthEnd = monthEnd.AddDays(7 - (int)monthEnd.DayOfWeek);

                DateTime currentDay = monthStart;

                while (currentDay <= monthEnd)
                {
                    dateList.Add(new { date = currentDay.ToString("dd"), 
                                       month = currentDay.ToString("MM"), 
                                       year = currentDay.ToString("yyyy"), 
                                       disabled = (((currentDay < currentMonth) 
                                                    || (currentDay.Month != currentMonth.Month) 
                                                    || (currentDay > end)) ? " disabled=\"disabled\" " : ""), 
                                       selected = ((currentDay.Date == selected.Date) ? " checked=\"checked\" " : "") 
                                     });
                    currentDay = currentDay.AddDays(1);
                }

                monthList.Add(new { monthName = currentMonth.ToString("MMMM"), year = currentMonth.Year, dates = dateList });

                currentMonth = currentMonth.AddMonths(1).AddDays(1 - currentMonth.Day);
            }

            outwardDateMonths.DataSource = monthList;
            outwardDateMonths.DataBind();
        }

        /// <summary>
        /// Used by the selectable calendar to show as expanded for the currently selected date
        /// </summary>
        protected string GetCollapsed(object monthName)
        {
            if (outwardDateTime != DateTime.MinValue)
            {
                try
                {
                    DateTime month = DateTime.ParseExact((string)monthName, "MMMM", CultureInfo.InvariantCulture);

                    if (outwardDateTime.Month == month.Month)
                    {
                        // Expand the selected month 
                        return false.ToString().ToLower();
                    }
                }
                catch
                {
                    // Ignore exceptions
                }

            }

            // Collapse month by default
            return true.ToString().ToLower();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validates the outward and return times
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return validInputDateChange;
        }

        #endregion
    }
}
