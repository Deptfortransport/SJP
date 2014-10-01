// *********************************************** 
// NAME             : CalendarMonth.ascx.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 1 Apr 2011
// DESCRIPTION  	: Represents each separate month of the calendar control
// ************************************************


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SJP.Common.Extenders;
using SJP.Common.Web;

namespace SJP.UserPortal.SJPWeb.Controls
{
    /// <summary>
    /// Delegate to raise DateChange event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SelectedDateChange(object sender, EventArgs e);

    /// <summary>
    /// Represents each separate month of the calendar control
    /// </summary>
    public partial class CalendarMonth : System.Web.UI.UserControl
    {
        #region Private Fields
        private List<string> monthDates = new List<string>();

        private int month;
        private int year;
        private int selectedDay;
        private DateTime selectedDate = DateTime.Now;
        private bool visibleMonth = false;

        private DateTime startDate;
        private DateTime endDate;
        private List<DateTime> datesEnabled = new List<DateTime>();
        private bool checkForEnabledDates;
        private bool monthChanging=false;
        #endregion

        #region Events
        public event SelectedDateChange OnDateChange;
        #endregion

        #region Public Properties
        /// <summary>
        /// Month of the calendar month 
        /// </summary>
        public int Month
        {
            set { month = value; }
            get { return month; }
        }

        /// <summary>
        /// Year of the calendar month
        /// </summary>
        public int Year
        {
            set { year = value; }
        }

        /// <summary>
        /// Read only returning the selected day of the month of the calendar month
        /// </summary>
        public int SelectedDay
        {
            get { return selectedDay; }
        }

        /// <summary>
        /// Gets/Sets the date selected on the calendar month
        /// </summary>
        public DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }

            set
            {
                selectedDate = value;
                // Only set the calendar active using selected date when user not clicked top tabs to change months
                if (!monthChanging)
                {
                    if (selectedDate.Month == month)
                    {
                        IsActive = true;
                    }
                    else
                    {
                        IsActive = false;
                    }
                }
                
            }
        }

        /// <summary>
        /// Determines if the current active month is in process of changing.
        /// </summary>
        public bool MonthChanging
        {
            get
            {
                return monthChanging;
            }
            set
            {
                monthChanging = value;
            }
        }

        /// <summary>
        /// Read/Write property determines if the calendar is active calendar
        /// </summary>
        public bool IsActive
        {
            get
            {
                return visibleMonth;
            }
            set
            {
                visibleMonth = value;

                string activeMonthCss = ((SJPPage)Page).GetResource("CalendarMonth.CssClass");

                if (string.IsNullOrEmpty(activeMonthCss))
                {
                    activeMonthCss = "current";
                }

                if (value)
                {
                    if (!calendarMonth.Attributes["class"].Trim().Contains(activeMonthCss))
                    {
                        calendarMonth.Attributes["class"] = calendarMonth.Attributes["class"].Trim() + " " + activeMonthCss;
                    }
                }
                else
                {
                    calendarMonth.Attributes["class"] = calendarMonth.Attributes["class"].Replace(activeMonthCss, "").Trim();
                }
            }
           
        }

        /// <summary>
        /// Start date of calendar
        /// The date may not be in the month represented by the calendar month instance
        /// </summary>
        public DateTime StartDate
        {
            set
            {
                startDate = value;
            }
        }

        /// <summary>
        /// End date of calendar
        ///  The date may not be in the month represented by the calendar month instance
        /// </summary>
        public DateTime EndDate
        {
            set
            {
                endDate = value;
            }
        }

        /// <summary>
        /// Sets the list of dates for which calendar should show dates as selectable
        /// </summary>
        public List<DateTime> EnabledDates
        {
            set
            {
                datesEnabled = value;
            }
        }

        /// <summary>
        /// List of dates tracking the selectable calendar dates
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

            SetupCalendar();
            // reset the monthChanging flag
            monthChanging = false;
        }

        #endregion

        #region Control Event Handlers
        /// <summary>
        /// Event Handler for each day button of the calendar clicked
        /// Raises DateChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Day_Command(object sender, CommandEventArgs e)
        {
            int day = e.CommandArgument.ToString().Parse(0);

            if (day > 0)
            {
                selectedDay = day;
                selectedDate = new DateTime(year, month, selectedDay);
                if (OnDateChange != null)
                {
                    OnDateChange(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// CalendarMonthView repeater data bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void calendarMonthView_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item
               || e.Item.ItemType == ListItemType.SelectedItem)
            {
                if (e.Item.DataItem != null)
                {
                    Repeater calendarMonthRow = e.Item.FindControl("calendarMonthRow") as Repeater;

                    calendarMonthRow.DataSource = e.Item.DataItem;
                    calendarMonthRow.DataBind();
                }
            }
        }

        /// <summary>
        /// CalendarMonthRow repeater data bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void calendarMonthRow_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.SelectedItem)
            {
                string item = e.Item.DataItem.ToString().Trim();

                HtmlTableCell dayCell = e.Item.FindControl("dayCell") as HtmlTableCell;

                if (dayCell != null)
                {

                    if (string.IsNullOrEmpty(item))
                    {
                        dayCell.Controls.Clear();
                        dayCell.Attributes["class"] = string.Empty;
                    }
                    else
                    {
                        DateTime cellDate = new DateTime(year, month, item.Parse(1));
                        if (cellDate < DateTime.Now.Date)
                        {
                            dayCell.Attributes["class"] = "disabled";
                        }
                        if (cellDate < startDate || cellDate > endDate || (!datesEnabled.Contains(cellDate) && checkForEnabledDates))
                        {
                            dayCell.Attributes["class"] = "disabled";
                        }
                            
                        if (selectedDate.Day.ToString("00") == item
                                    && selectedDate.Month == month
                                    && selectedDate.Year == year)
                        {

                            dayCell.Attributes["class"] = dayCell.Attributes["class"] + " selected";
                        }

                    }
                }
            }
            
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Setup calendar
        /// </summary>
        private void SetupCalendar()
        {
            monthDates = GetDates(year, month);

            calendarMonth_Month.Value = month.ToString();
            calendarMonth_Year.Value = year.ToString();


            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            if (!calendarMonth.Attributes["class"].Contains(firstDayOfMonth.ToString("MMM")))
            {
                calendarMonth.Attributes["class"] = calendarMonth.Attributes["class"] + " " + firstDayOfMonth.ToString("MMM");


            }

            for (int i = 7; i > (7 - (int)firstDayOfMonth.DayOfWeek); i--)
            {
                monthDates.Insert(0, string.Empty);
            }

            List<List<string>> weekInMonth = new List<List<string>>();

            int countToSkip = 0;
            do
            {
                weekInMonth.Add(monthDates.Skip(countToSkip)
                .Take(7).ToList());
                countToSkip += 7;
            } while (countToSkip < monthDates.Count);



            calendarMonthView.DataSource = weekInMonth;
            calendarMonthView.DataBind();
        }
        
        /// <summary>
        /// Gets all the dates as list in the month and year specified
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <returns></returns>
        private List<string> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day).ToString("dd")) // Map each day to a date
                             .ToList(); // Load dates into a list
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Returns the day string i.e. S, M, T, etc. based on the day of the week
        /// </summary>
        /// <param name="day">DayOfWeek enum value</param>
        /// <returns></returns>
        protected string GetDayResourceString(DayOfWeek day)
        {
            SJPPage page = (SJPPage)Page;
            string resourceKey = string.Format("CalendarMonth.{0}",day);

            return page.GetResource(resourceKey);
        }
        #endregion

    }
}