// *********************************************** 
// NAME             : DaysOfWeek.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Enum to specify days of week the SJP parks are available
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Enum to specify days of week the SJP parks are available
    /// </summary>
    [Serializable]
    public enum DaysOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
        Everyday,
        Weekday
    }
}
