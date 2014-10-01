// *********************************************** 
// NAME             : Calendar.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 29 Mar 2011
// DESCRIPTION  	: User control to display calendar inside tabs 
// ************************************************


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    ///  User control to display calendar inside tabs 
    /// </summary>
    public partial class Calendar : System.Web.UI.UserControl
    {
        #region Private Fields
        private DateTime startDate = new DateTime(2012, 6, 1);
        private DateTime endDate = new DateTime(2012, 8, 31);
        private DateTime selectedDate = DateTime.Now;
        private string activeHeaderLinkCss = "current";
        private const string DATE_PATTERN = "ddMMyyyy";
        private List<DateTime> datesEnabled = new List<DateTime>();
        private bool checkForEnabledDates = true;
        #endregion

        #region Events
        public event SelectedDateChange OnDateChange;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets/Sets the selected date for the calendar
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                DateTime.TryParseExact(calendar_SelectedDate.Value, DATE_PATTERN, CultureInfo.InvariantCulture,
                   DateTimeStyles.None, out selectedDate);
                return selectedDate;
            }
            set
            {
                selectedDate = value;
                calendarMonth1.SelectedDate = selectedDate;
                calendarMonth2.SelectedDate = selectedDate;
                calendarMonth3.SelectedDate = selectedDate;
                calendar_SelectedDate.Value = selectedDate.ToString(DATE_PATTERN);
            }

        }

        /// <summary>
        /// Sets the starting date for the calendar display
        /// The calendar shows only 3 months starting from the start date month
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
            }
            
        }

        /// <summary>
        /// Sets the end date for the calendar control
        /// The end date sets the css so the date after end date show as disabled
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
            }
        }

        /// <summary>
        /// List of dates tracking the selectable calendar dates
        /// </summary>
        public List<DateTime> EnabledDates
        {
            set
            {
                datesEnabled = value;
            }
        }

        /// <summary>
        /// Boolean value determining if calendar needs to check for enabled dates
        /// </summary>
        public bool CheckForEnabledDates
        {
            get
            {
                return checkForEnabledDates;
            }
            set
            {
                checkForEnabledDates = value;
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
            calendarMonth1.OnDateChange += new SelectedDateChange(Calendar_DateChange);
            calendarMonth2.OnDateChange += new SelectedDateChange(Calendar_DateChange);
            calendarMonth3.OnDateChange += new SelectedDateChange(Calendar_DateChange);
        }

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetupResources();


            if (!IsPostBack)
            {
                
               
                calendarMonth1.IsActive = true;
                calendarMonth2.IsActive = false;
                calendarMonth3.IsActive = false;

            }

            SetupCalendar();

            DateTime.TryParseExact(calendar_SelectedDate.Value, DATE_PATTERN, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out selectedDate);
            calendarMonth1.SelectedDate = selectedDate;
            calendarMonth2.SelectedDate = selectedDate;
            calendarMonth3.SelectedDate = selectedDate;

           
        
        }


        /// <summary>
        /// Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetActiveTab();
        }
        #endregion

        #region Control Event Handlers
        /// <summary>
        /// Handler for the individual CalendarMonth control's date change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Calendar_DateChange(object sender, EventArgs e)
        {
            if (sender is CalendarMonth)
            {
                CalendarMonth currentCalendar = (CalendarMonth)sender;
                SelectedDate = currentCalendar.SelectedDate;
                if (OnDateChange != null)
                {
                    OnDateChange(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Event handler for month tab changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void monthTab_Command(object sender, CommandEventArgs e)
        {
            int month = e.CommandArgument.ToString().Parse(calendarMonth1.Month);
            if (month == calendarMonth2.Month)
            {
                calendarMonth1.IsActive = false;
                calendarMonth2.IsActive = true;
                calendarMonth3.IsActive = false;
            }
            else if (month == calendarMonth3.Month)
            {
                calendarMonth1.IsActive = false;
                calendarMonth2.IsActive = false;
                calendarMonth3.IsActive = true;
            }
            else
            {
                calendarMonth1.IsActive = true;
                calendarMonth2.IsActive = false;
                calendarMonth3.IsActive = false;
            }

            calendarMonth1.MonthChanging = true;
            calendarMonth2.MonthChanging = true;
            calendarMonth3.MonthChanging = true;
                   
            
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Setsup the 3 calendar months represented by this control
        /// </summary>
        private void SetupCalendar()
        {
            calendarMonth1.Month = startDate.Month;
            calendarMonth1.Year = startDate.Year;

            calendarMonth2.Month = startDate.AddMonths(1).Month;
            calendarMonth2.Year = startDate.AddMonths(1).Year;

            calendarMonth3.Month = startDate.AddMonths(2).Month;
            calendarMonth3.Year = startDate.AddMonths(2).Year;
            
            
            calendarMonth1.StartDate = startDate;
            calendarMonth2.StartDate = startDate;
            calendarMonth3.StartDate = startDate;

            calendarMonth1.EndDate = endDate;
            calendarMonth2.EndDate = endDate;
            calendarMonth3.EndDate = endDate;

            calendarMonth1.EnabledDates = datesEnabled;
            calendarMonth2.EnabledDates = datesEnabled;
            calendarMonth3.EnabledDates = datesEnabled;

            calendarMonth1.CheckForEnabledDates = checkForEnabledDates;
            calendarMonth2.CheckForEnabledDates = checkForEnabledDates;
            calendarMonth3.CheckForEnabledDates = checkForEnabledDates;

            PopulateHeaderLinks();

        }

        /// <summary>
        /// Set up resources from the resource content database
        /// </summary>
        private void SetupResources()
        {
            SJPPage page = (SJPPage)Page;

            // The following two resource content if changed it will require Calendar Javascript and css updating to reflect 
            // the class name changes
            activeHeaderLinkCss = page.GetResource("EventCalendar.monthHeaderLink.CssClass");
           
        }

        /// <summary>
        /// Sets the active tab of the calendar
        /// </summary>
        private void SetActiveTab()
        {
            SetCurrent(monthHeader1, calendarMonth1.IsActive);
            SetCurrent(monthHeader2, calendarMonth2.IsActive);
            SetCurrent(monthHeader3, calendarMonth3.IsActive);
        }

        /// <summary>
        /// Sets the css for tab to be active in the calendar tabs
        /// </summary>
        /// <param name="monthHeaderLink">Tab header link</param>
        /// <param name="active">True if active</param>
        private void SetCurrent(HtmlGenericControl monthHeaderLink, bool active)
        {
            if (active)
            {
                if(string.IsNullOrEmpty(monthHeaderLink.Attributes["class"]))
                {
                    monthHeaderLink.Attributes["class"] = activeHeaderLinkCss;
                }

                if (!monthHeaderLink.Attributes["class"].Trim().Contains(activeHeaderLinkCss))
                {
                    monthHeaderLink.Attributes["class"] = monthHeaderLink.Attributes["class"] + " " + activeHeaderLinkCss;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(monthHeaderLink.Attributes["class"]))
                {
                    monthHeaderLink.Attributes["class"] = monthHeaderLink.Attributes["class"].Replace(activeHeaderLinkCss, "").Trim();
                }
            }
        }

        /// <summary>
        /// Populates header buttons - this is to make the tabs working without javascript
        /// </summary>
        private void PopulateHeaderLinks()
        {
            month1Button.Text = startDate.ToString("MMM", CurrentLanguage.CurrentCultureInfo);
            month1Button.CommandArgument = calendarMonth1.Month.ToString(CurrentLanguage.CurrentCultureInfo);
            month1Button.ToolTip = startDate.ToString("MMM", CurrentLanguage.CurrentCultureInfo);

            month2Button.Text = startDate.AddMonths(1).ToString("MMM", CurrentLanguage.CurrentCultureInfo);
            month2Button.CommandArgument = calendarMonth2.Month.ToString(CurrentLanguage.CurrentCultureInfo);
            month2Button.ToolTip = startDate.ToString("MMM", CurrentLanguage.CurrentCultureInfo);

            month3Button.Text = startDate.AddMonths(2).ToString("MMM", CurrentLanguage.CurrentCultureInfo);
            month3Button.CommandArgument = calendarMonth3.Month.ToString(CurrentLanguage.CurrentCultureInfo);
            month3Button.ToolTip = startDate.ToString("MMM", CurrentLanguage.CurrentCultureInfo);
        }

        
        #endregion

       
    }
}