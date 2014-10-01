// *********************************************** 
// NAME             : SJPPark.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Apr 2011
// DESCRIPTION  	: Class represent sjp cycle/car park common attributes
// ************************************************
                
using System;
using System.Collections.Generic;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Class represent sjp cycle/car park common attributes
    /// </summary>
    [Serializable]
    public class SJPPark
    {
        #region Private Fields

        private string venueNaPTAN = string.Empty;
        private string id;
        private string name;
        private List<SJPParkAvailability> availabilities = new List<SJPParkAvailability>();

        #endregion

        #region Public Properties
        /// <summary>
        /// The NaPTAN code for the Olympic Venue with which this cycle/car park is associated
        /// </summary>
        public string VenueNaPTAN
        {
            get { return venueNaPTAN; }
            set { venueNaPTAN = value; }
        }

        /// <summary>
        /// The name of the cycle/car park
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Unique identifier for the cycle/car park
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The availablity conditions for the cycle/car park
        /// </summary>
        public List<SJPParkAvailability> Availability
        {
            get { return availabilities; }
            set { availabilities = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if this cycle/car park is open for the specified time (ignores dates/days)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsOpenForTime(DateTime time)
        {
            return IsOpenForDateAndTime(time, false);
        }

        /// <summary>
        /// Checks if this cycle/car park is open for the specified date and time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsOpenForDateAndTime(DateTime datetime, bool checkDate)
        {
            bool isOpen = false;

            if ((availabilities != null) && (datetime != DateTime.MinValue))
            {
                TimeSpan tsTime = new TimeSpan(datetime.Hour, datetime.Minute, 0);
                TimeSpan overnightMidnight = new TimeSpan(23, 59, 0);
                bool isOvernight = false;

                foreach (SJPParkAvailability availability in availabilities)
                {
                    if (checkDate && 
                        !(availability.FromDate.Date <= datetime.Date && datetime.Date <= availability.ToDate))
                    {
                        // Availability not valid for the date
                        continue;
                    }

                    isOvernight = (availability.DailyClosingTime < availability.DailyOpeningTime);

                    if ((availability.DailyOpeningTime <= tsTime && tsTime <= availability.DailyClosingTime)
                        || (isOvernight && TimeSpan.Zero <= tsTime && tsTime <= availability.DailyClosingTime)
                        || (isOvernight && availability.DailyOpeningTime <= tsTime && tsTime <= overnightMidnight))
                    {
                        isOpen = true;
                        break;
                    }

                    // Assume if availability opening and closing times are both midnight, then that 
                    // park is open all day
                    if ((availability.DailyOpeningTime == TimeSpan.Zero) &&
                        (availability.DailyClosingTime == TimeSpan.Zero))
                    {
                        isOpen = true;
                        break;
                    }
                }
            }
            else
            {
                // Assume open
                isOpen = true;
            }

            return isOpen;
        }

        /// <summary>
        /// Returns the opening or closing time for the date.
        /// Default is zero.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="isOpeningTime"></param>
        /// <returns></returns>
        public TimeSpan GetAvailabilityTimeForDate(DateTime date, bool isOpeningTime)
        {
            TimeSpan ts = TimeSpan.Zero;

            if ((availabilities != null) && (date != DateTime.MinValue))
            {
                foreach (SJPParkAvailability availability in availabilities)
                {
                    if (date.Date >= availability.FromDate && date.Date <= availability.ToDate)
                    {
                        if (isOpeningTime)
                            ts = availability.DailyOpeningTime;
                        else
                            ts = availability.DailyClosingTime;
                        break;
                    }
                }
            }

            return ts;
        }

        #endregion
    }
}
