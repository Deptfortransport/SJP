// *********************************************** 
// NAME             : EventDateControl.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Represents Event date selection user control on journey input page
// ************************************************


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.PropertyManager;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Represents Event date selection user control on journey input page
    /// </summary>
    public partial class EventDateControl : System.Web.UI.UserControl
    {
        #region Private Fields

        private const string DATE_FORMAT = "dd/MM/yyyy";

        private bool forceUpdate = false;
        private DateTime selectedOutwardDateTime = DateTime.MinValue;
        private DateTime selectedReturnDateTime = DateTime.MinValue;
        private bool validInputDateChange = true;
        private bool isOutwardRequired = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write property indicates if the outward and return date times should be updated
        /// if they are in the past
        /// </summary>
        public bool ForceUpdate
        {
            get { return forceUpdate; }
            set { forceUpdate = value; }
        }

        /// <summary>
        /// Read/Write property gets/sets the event outward date time
        /// </summary>
        public DateTime OurwardDateTime
        {
            get
            {
                selectedOutwardDateTime = GetEventDateTime(false);
                return selectedOutwardDateTime;
            }

            set
            {
                selectedOutwardDateTime = value;
            }

        }

        /// <summary>
        /// Read/Write property gets/sets the event return date time
        /// </summary>
        /// <remarks>
        /// Only time of the return date time will be considered when setting the datetime
        /// as the event date should be the single day the outward journey date will be considered always 
        /// </remarks>
        public DateTime ReturnDateTime
        {
            get
            {
                selectedReturnDateTime = GetEventDateTime(true);
                return selectedReturnDateTime;
            }

            set
            {
                selectedReturnDateTime = value;
            }

        }

        /// <summary>
        /// Read/Write property determines wether the outward journey required or not
        /// </summary>
        public bool IsOutwardRequired
        {
            get { return isOutwardRequired; }
            set { isOutwardRequired = value; }
        }

        /// <summary>
        /// Read/Write property determines wether the return journey required or not
        /// </summary>
        public bool IsReturnRequired
        {
            get { return (isReturnJourney.Checked || !isOutwardRequired); }
            set { isReturnJourney.Checked = value; }
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
            SetupResourceStrings();

            InitCalendar();

            InitOutwardRequired();
        }

        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetJourneyDateTime();
        }

        #endregion

        #region Control Event Handlers

        /// <summary>
        /// Calendar date change event handler
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected void Calendar_DateChange(object source, EventArgs args)
        {
            selectedOutwardDateTime = GetEventDateTime(false);
            selectedReturnDateTime = GetEventDateTime(true);

            // Ensure return date is not before outward
            if (isOutwardRequired && (selectedOutwardDateTime > selectedReturnDateTime))
            {
                selectedReturnDateTime = selectedOutwardDateTime;
            }
        }

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
                selectedOutwardDateTime = GetEventDateTime(false);
            }
        }

        /// <summary>
        /// Return date input box text change event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReturnDate_Changed(object sender, EventArgs e)
        {
            DateTime retDate = DateTime.MinValue;

            if (!DateTime.TryParse(returnDate.Text.Trim(), out retDate))
            {
                if (isReturnJourney.Checked || !isOutwardRequired)
                {
                    validInputDateChange = false;
                }
            }
            else
            {
                selectedReturnDateTime = GetEventDateTime(true);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets Control Culture aware resource strings
        /// </summary>
        private void SetupResourceStrings()
        {
            SJPPage page = (SJPPage)Page;

            outwardDateLabel.Text = page.GetResource("EventDateControl.outwardDateLabel.Text");
            returnDateLabel.Text = page.GetResource("EventDateControl.returnDateLabel.Text");
            arriveTimeLabel.Text = page.GetResource("EventCalendar.arriveTimeLabel.Text");
            leaveTimeLabel.Text = page.GetResource("EventCalendar.leaveTimeLabel.Text");
            isReturnJourney.Text = page.GetResource("EventCalendar.isReturnJourney.Text");

            outward_Information.ImageUrl = page.ImagePath + page.GetResource("EventDateControl.OutwardInformation.ImageUrl");
            outward_Information.AlternateText = page.GetResource("EventDateControl.OutwardInformation.AlternateText");
            outward_Information.ToolTip = page.GetResource("EventDateControl.OutwardInformation.ToolTip");

            return_Information.ImageUrl = page.ImagePath + page.GetResource("EventDateControl.ReturnInformation.ImageUrl");
            return_Information.AlternateText = page.GetResource("EventDateControl.ReturnInformation.AlternateText");
            return_Information.ToolTip = page.GetResource("EventDateControl.ReturnInformation.ToolTip");
            
            tooltip_information_outward.Title = page.GetResource("EventDateControl.OutwardInformation.ToolTip");
            tooltip_information_return.Title = page.GetResource("EventDateControl.ReturnInformation.ToolTip");
        }

        /// <summary>
        /// Populated the arrive and leave time dropdowns
        /// </summary>
        private void PopulateJourneyTimeDropDowns()
        {
            if ((arriveTime.Items.Count == 0) || (leaveTime.Items.Count == 9))
            {
                List<ListItem> arriveTimeList = new List<ListItem>();
                List<ListItem> leaveTimeList = new List<ListItem>();
                DateTime startTime = DateTime.Now.Date;

                int interval = Properties.Current["EventDateControl.DropDownTime.IntervalMinutes"].Parse(15);

                var times = from hour in Enumerable.Range(0, 24) // each hour in 0 -23
                            from minute in Enumerable.Range(0, 59) // each minute in 0 - 59
                            where minute % interval == 0 // get the minute with the interval required
                            // format the time and return the ListItem populated with it
                            select new ListItem(string.Format("{0:00}:{1:00}", hour, minute),
                                string.Format("{0:00}:{1:00}", hour, minute));

                // create arrive and leave time lists of ListItems
                arriveTimeList = times.ToList();

                leaveTimeList = times.ToList();

                // Add the ListItems to the dropdowns
                arriveTime.Items.AddRange(arriveTimeList.ToArray());

                leaveTime.Items.AddRange(leaveTimeList.ToArray());

                arriveTime.Items.FindByValue(Properties.Current["EventDateControl.DropDownTime.Outward.Default"]).Selected = true;
                leaveTime.Items.FindByValue(Properties.Current["EventDateControl.DropDownTime.Return.Default"]).Selected = true;
            }
        }

        /// <summary>
        /// Builds a datetime object out of the calendar control and time dropdowns
        /// </summary>
        /// <param name="isReturn"></param>
        /// <returns></returns>
        private DateTime GetEventDateTime(bool isReturn)
        {
            // If there was a landing page with auto plan true, then need to ensure this control is initialised 
            // (otherwise values can be empty and cause errors here)
            if (forceUpdate)
            {
                PopulateJourneyTimeDropDowns();
                SetJourneyDateTime();
            }

            DateTime calendarDate = DateTime.MinValue;

            if (isReturn)
            {
                DateTime.TryParseExact(returnDate.Text.Trim(),
                    DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out calendarDate);
            }
            else
            {
                DateTime.TryParseExact(outwardDate.Text.Trim(),
                    DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out calendarDate);
            }

            DropDownList timeList = isReturn ? leaveTime : arriveTime;

            string[] timeparts = timeList.SelectedValue.Split(new char[] { ':' });
            int minute = timeparts[1].Parse(0);
            int hour = timeparts[0].Parse(0);

            return new DateTime(calendarDate.Year, calendarDate.Month, calendarDate.Day, hour, minute, 0);
        }

        /// <summary>
        /// Sets the calendar control and time dropdown to represent the outward and return journey date
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isReturn"></param>
        private void SetJourneyDateTime()
        {
            if (ValidJourneyDateTime(selectedOutwardDateTime, true, forceUpdate))
            {
                outwardDate.Text = selectedOutwardDateTime.ToString(DATE_FORMAT);

                arriveTime.SelectedItem.Selected = false;

                string dropDownTime = GetDropDownTime(selectedOutwardDateTime);

                arriveTime.Items.FindByValue(dropDownTime).Selected = true;
            }

            if (ValidJourneyDateTime(selectedReturnDateTime, false, forceUpdate))
            {
                returnDate.Text = selectedReturnDateTime.ToString(DATE_FORMAT);

                leaveTime.SelectedItem.Selected = false;

                string dropDownTime = GetDropDownTime(selectedReturnDateTime);

                leaveTime.Items.FindByValue(dropDownTime).Selected = true;
            }

            if (validInputDateChange)
            {
                outwardDate.Text = selectedOutwardDateTime.ToString(DATE_FORMAT);
                returnDate.Text = selectedReturnDateTime.ToString(DATE_FORMAT);
            }
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

            return string.Format("{0:00}:{1:00}", dropdownTime.Hour, dropDownTime_Minutes >= 60 ? 00 : dropDownTime_Minutes);
        }

        /// <summary>
        /// Validates the outward and return journey date times
        /// </summary>
        /// <param name="selectedOutwardDateTime"></param>
        /// <param name="forceUpdate">If true, the selected datetime is updated to now if it is inthe past</param>
        /// <returns></returns>
        private bool ValidJourneyDateTime(DateTime selectedDateTime, bool isOutward, bool forceUpdate)
        {
            bool validDate = true;

            if (selectedDateTime == DateTime.MinValue)
                validDate = false;
            // Date part could be invalid (01/01/0001)
            else if (selectedDateTime.Date == DateTime.MinValue.Date)
                validDate = false;

            if (validDate && isOutward && selectedDateTime < DateTime.Now && forceUpdate)
            {
                selectedOutwardDateTime = DateTime.Now;
            }

            if (validDate && !isOutward && selectedDateTime < DateTime.Now && forceUpdate)
            {
                selectedReturnDateTime = DateTime.Now;
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

                // Set the initial selected outward date value
                if (selectedOutwardDateTime == DateTime.MinValue)
                {
                    // Outward event date

                    // Get the default outward selected time value
                    string[] timeparts = Properties.Current["EventDateControl.DropDownTime.Outward.Default"].Split(new char[] { ':' });
                    int minute = timeparts[1].Parse(0);
                    int hour = timeparts[0].Parse(0);

                    DateTime outwardStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, hour, minute, 0);

                    selectedOutwardDateTime = DateTime.Now >= outwardStartDate ? DateTime.Now : outwardStartDate;

                    // Ensure calendar start date is Now or later
                    calendarStartDate.Value = selectedOutwardDateTime.ToString(DATE_FORMAT);
                }
                // Else the selectedOutwardDateTime has already been populated

                // Update inputs to the selected datetime
                outwardDate.Text = selectedOutwardDateTime.ToString(DATE_FORMAT);
                arriveTime.SelectedItem.Selected = false;
                arriveTime.Items.FindByValue(GetDropDownTime(selectedOutwardDateTime)).Selected = true;

                if (selectedReturnDateTime == DateTime.MinValue)
                {
                    // Return event date

                    // Get the default return selected time value
                    string[] timeparts = Properties.Current["EventDateControl.DropDownTime.Return.Default"].Split(new char[] { ':' });
                    int minute = timeparts[1].Parse(0);
                    int hour = timeparts[0].Parse(0);

                    DateTime returnStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, hour, minute, 0);

                    selectedReturnDateTime = DateTime.Now >= returnStartDate ? DateTime.Now : returnStartDate;
                }
                // Else the selectedReturnDateTime has already been populated

                // Update inputs to the selected datetime
                returnDate.Text = selectedReturnDateTime.ToString(DATE_FORMAT);
                leaveTime.SelectedItem.Selected = false;
                leaveTime.Items.FindByValue(GetDropDownTime(selectedReturnDateTime)).Selected = true;

                #endregion

                // Check if need to update the return date time based on the outward date time.
                // If landing page, then the return date time value may have been set already - this needs to be 
                // validated and updated here

                #region Validate and update return datetime

                // Get the SJP default return datetime, e.g. 24/05/2010 17:00
                DateTime returnDateTime = GetEventDateTime(true);
                if (returnDateTime <= selectedOutwardDateTime)
                {
                    // The return time might have been set to be later than the SJP 
                    // default return time, so retain that part

                    // Update the date part, retaining the time part and check again
                    DateTime updatedReturnDateTime = new DateTime(
                        selectedOutwardDateTime.Year, selectedOutwardDateTime.Month, selectedOutwardDateTime.Day,
                        returnDateTime.Hour, returnDateTime.Minute, 0);

                    if (updatedReturnDateTime < selectedOutwardDateTime)
                    {
                        // Update both the date part and time part and check again, because the 
                        // return datetime may not have been set in the landing page
                        updatedReturnDateTime = new DateTime(
                            selectedOutwardDateTime.Year, selectedOutwardDateTime.Month, selectedOutwardDateTime.Day,
                            returnDateTime.Hour, returnDateTime.Minute, 0);

                        if (updatedReturnDateTime < selectedOutwardDateTime)
                        {
                            selectedReturnDateTime = selectedOutwardDateTime.AddHours(Properties.Current["EventDateControl.DropDownTime.OutwardReturnIntervalHours"].Parse(1));
                        }
                        else
                        {
                            selectedReturnDateTime = updatedReturnDateTime;
                        }
                    }
                    else
                    {
                        selectedReturnDateTime = updatedReturnDateTime;
                    }
                }

                #endregion
            }
            else
            {
                selectedOutwardDateTime = GetEventDateTime(false);
                selectedReturnDateTime = GetEventDateTime(true);
            }
        }

        /// <summary>
        /// Sets the outward required flag in control
        /// </summary>
        private void InitOutwardRequired()
        {
            // Read the hidden value, this will be empty on first page load, 
            // and should only be set on a postbacks if set by parent
            if (!string.IsNullOrEmpty(returnOnly.Value))
            {
                isOutwardRequired = !returnOnly.Value.Parse(true);
            }
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

        /// <summary>
        /// Switches the date control into showing/hiding the outward date entry
        /// </summary>
        /// <param name="showReturnDateOnly"></param>
        /// <returns></returns>
        public void ShowReturnDateOnly(bool showReturnDateOnly)
        {
            isOutwardRequired = !showReturnDateOnly;
            
            // Show/hide the outward date controls and the return date checkbox
            outwardDateDiv.Visible = !showReturnDateOnly;
            returnDateCheckBoxDiv.Visible = !showReturnDateOnly;

            // Update hidden field for javascript
            returnOnly.Value = showReturnDateOnly.ToString();
        }

        #endregion
    }
}
