// *********************************************** 
// NAME             : SJPParkAvailability.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: SJPParkAvailability class represents the availability conditions
//                    of SJP cycle and car parks 
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// This class represents the availability conditions of SJP cycle and car parks 
    /// </summary>
    [Serializable]
    public class SJPParkAvailability
    {
        #region Private Fields

        private DateTime fromDate;
        private DateTime toDate;
        private TimeSpan dailyOpeningTime;
        private TimeSpan dailyClosingTime;
        private DaysOfWeek daysOpen = DaysOfWeek.Everyday;

        #endregion

        #region Constructor

        /// <summary>
        /// SJPParkAvailability constructor
        /// </summary>
        /// <param name="fromDate">From date time</param>
        /// <param name="toDate">To date time</param>
        /// <param name="dailyOpeningTime">Daily opening time</param>
        /// <param name="dailyClosingTime">Daily closing time</param>
        /// <param name="daysOpen">Days of the week park is open</param>
        public SJPParkAvailability(DateTime fromDate, DateTime toDate, 
            TimeSpan dailyOpeningTime, TimeSpan dailyClosingTime, DaysOfWeek daysOpen)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.dailyOpeningTime = dailyOpeningTime;
            this.dailyClosingTime = dailyClosingTime;
            this.daysOpen = daysOpen;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Date from when the park is open
        /// </summary>
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        /// <summary>
        /// DateTime when the park is closing down
        /// </summary>
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }

        /// <summary>
        /// Daily opening time for the park
        /// </summary>
        public TimeSpan DailyOpeningTime
        {
            get { return dailyOpeningTime; }
            set { dailyOpeningTime = value; }
        }

        /// <summary>
        /// Daily closing time for the park
        /// </summary>
        public TimeSpan DailyClosingTime
        {
            get { return dailyClosingTime; }
            set { dailyClosingTime = value; }
        }

        /// <summary>
        /// Days of the week park is open
        /// </summary>
        public DaysOfWeek DaysOpen
        {
            get { return daysOpen; }
            set { daysOpen = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DayOfWeek> GetDaysOfWeek()
        {
            List<DayOfWeek> days = new List<DayOfWeek>();

            switch (daysOpen)
            {
                case DaysOfWeek.Monday:
                    days.Add(DayOfWeek.Monday);
                    break;
                case DaysOfWeek.Tuesday:
                    days.Add(DayOfWeek.Tuesday);
                    break;
                case DaysOfWeek.Wednesday:
                    days.Add(DayOfWeek.Wednesday);
                    break;
                case DaysOfWeek.Thursday:
                    days.Add(DayOfWeek.Thursday);
                    break;
                case DaysOfWeek.Friday:
                    days.Add(DayOfWeek.Friday);
                    break;
                case DaysOfWeek.Saturday:
                    days.Add(DayOfWeek.Saturday);
                    break;
                case DaysOfWeek.Sunday:
                    days.Add(DayOfWeek.Sunday);
                    break;
                case DaysOfWeek.Weekday:
                    days.Add(DayOfWeek.Monday);
                    days.Add(DayOfWeek.Tuesday);
                    days.Add(DayOfWeek.Wednesday);
                    days.Add(DayOfWeek.Thursday);
                    days.Add(DayOfWeek.Friday);
                    break;
                default:
                    days.Add(DayOfWeek.Monday);
                    days.Add(DayOfWeek.Tuesday);
                    days.Add(DayOfWeek.Wednesday);
                    days.Add(DayOfWeek.Thursday);
                    days.Add(DayOfWeek.Friday);
                    days.Add(DayOfWeek.Saturday);
                    days.Add(DayOfWeek.Sunday);
                    break;

            }

            return days;
        }

        #endregion
    }
}
