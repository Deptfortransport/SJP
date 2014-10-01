// *********************************************** 
// NAME             : JourneyComparer.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 19 June 2011
// DESCRIPTION  	: JourneyComparer used to sort journeys
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// JourneyComparer used to sort journeys
    /// </summary>
    public static class JourneyComparer
    {
        /// <summary>
        /// Compares journeys using the Journey.StartTime. 
        /// Journeys ordered by those starting earlier (leave after)
        /// </summary>
        public static int SortJourneyLeaveAfter(Journey x, Journey y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // journey start times (leave after)
                    return x.StartTime.CompareTo(y.StartTime);
                }
            }
        }

        /// <summary>
        /// Compares journeys using the Journey.EndTime. 
        /// Journeys ordered by those arriving later (arriving by)
        /// </summary>
        public static int SortJourneyArriveBy(Journey x, Journey y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // journey end times (arriving by)
                    return y.EndTime.CompareTo(x.EndTime);
                }
            }
        }
    }
}
