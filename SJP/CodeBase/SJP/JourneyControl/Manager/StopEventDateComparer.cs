// *********************************************** 
// NAME             : StopEventDateComparer.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Comparer class to do StopEvnet arrival/departure time comparison 
// ************************************************
// 
                
using System;
using System.Collections;
using System.Collections.Generic;
using ICJP = TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.UserPortal.JourneyControl
{
    public class StopEventDateComparer : IComparer<ICJP.StopEvent>
    {
        #region Public enum

        public enum SortOrder { Ascending, Descending };

        #endregion

        #region Private members

        private SortOrder sortOrder;
        private bool showDepartures;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public StopEventDateComparer()
        { 
            sortOrder = SortOrder.Ascending; 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="showDepartures"></param>
        public StopEventDateComparer(SortOrder sortOrder, bool showDepartures)
        {
            this.sortOrder = sortOrder;
            this.showDepartures = showDepartures;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="showDepartures"></param>
        public StopEventDateComparer(bool showDepartures)
        {
            this.sortOrder = SortOrder.Ascending;
            this.showDepartures = showDepartures;
        }

        #endregion

        #region Public methods

        // Note: Always sorts null entries in the front.
        /// <summary>
        /// Implamentation of IComparer.Compare method
        /// </summary>
        /// <param name="x">StopEvent object 1</param>
        /// <param name="y">StopEvent object 2</param>
        /// <returns>Return 1 if greater, 0 if smaller and -1 if null</returns>
        public int Compare(ICJP.StopEvent x, ICJP.StopEvent y)
        {
            int retval = 0;

            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    retval = 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    retval = -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    retval = 1;
                }
                else
                {
                    ICJP.StopEvent stopEvent1 = (ICJP.StopEvent)x;
                    ICJP.StopEvent stopEvent2 = (ICJP.StopEvent)y;

                    DateTime datetime1 = DateTime.MinValue;
                    DateTime datetime2 = DateTime.MinValue;

                    if (showDepartures)
                    {
                        datetime1 = stopEvent1.stop.departTime;
                        datetime2 = stopEvent2.stop.departTime;
                    }
                    else
                    {
                        datetime1 = stopEvent1.stop.arriveTime;
                        datetime2 = stopEvent2.stop.arriveTime;
                    }

                    if (sortOrder == SortOrder.Ascending)
                    {
                        retval = DateTime.Compare(datetime1, datetime2);
                    }
                    else
                    {
                        retval = DateTime.Compare(datetime2, datetime1);
                    }
                }
            }

            return retval;
        }

        #endregion
    }
}
