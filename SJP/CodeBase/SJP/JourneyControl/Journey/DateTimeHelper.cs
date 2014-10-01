// *********************************************** 
// NAME             : DateTimeHelper.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 27 Mar 2011
// DESCRIPTION  	: DateTimeHelper class providing helper methods
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// DateTimeHelper class providing helper methods
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// Enumeration of date time conversion
        /// </summary>
        public enum DateTimeType
        {
            Date,
            DateTime,
            DateTimeMillisecond
        }

        /// <summary>
        /// Returns a new DateTime object from the supplied datetime.
        /// Uses Year, Month, Day, Hour, Minute, and Second parts
        /// </summary>
        public static DateTime GetDateTime(DateTime dateTime)
        {
            return GetDateTime(dateTime, DateTimeType.DateTime);
        }
                
        /// <summary>
        /// Returns a new DateTime object from the supplied datetime.
        /// Uses Year, Month, Day, Hour, Minute, Second, and Millisecond parts (if required)
        /// </summary>
        public static DateTime GetDateTime(DateTime dateTime, DateTimeType dateTimeType)
        {
            if (dateTime == null)
            {
                return DateTime.MinValue;
            }

            switch (dateTimeType)
            {
                case DateTimeType.Date:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                case DateTimeType.DateTime:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                case DateTimeType.DateTimeMillisecond:
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
                default:
                    return DateTime.MinValue;
            }
        }
    }
}