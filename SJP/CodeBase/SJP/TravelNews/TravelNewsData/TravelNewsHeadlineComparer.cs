// *********************************************** 
// NAME             : TravelNewsHeadlineComparer.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 26 Jul 2011
// DESCRIPTION  	: Implementation of IComparer to sort the headline items using regions supplied in comma delimited string
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SJP.UserPortal.TravelNews.TravelNewsData
{
    /// <summary>
    /// Implementation of IComparer to sort the headline items using regions supplied in comma delimited string
    /// </summary>
    public class TravelNewsHeadlineComparer : IComparer<HeadlineItem>
    {
        #region Private Fields
        private List<string> sortOrder = new List<string>();
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sortOrderString">Comma delimited string specifying the regions sort order</param>
        public TravelNewsHeadlineComparer(string sortOrderString)
        {
            if (!string.IsNullOrEmpty(sortOrderString))
            {
                sortOrder.AddRange(sortOrderString.Split(new char[] { ',' }));
            }
        }
        #endregion

        #region IComparer Implementation
        /// <summary>
        /// Compares the two travel news headlines item with respect to the sort order specified 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(HeadlineItem x, HeadlineItem y)
        {
            int diff = x.SeverityLevel.CompareTo(y.SeverityLevel);

            if (diff == 0)
            {
                diff = GetSortRank(x).CompareTo(GetSortRank(y));
            }
            return diff;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the sort rank for the headline item
        /// The sort rank is the index of the item found in the region sort order string passed.
        /// </summary>
        /// <param name="item">Travel news headline item</param>
        /// <returns></returns>
        private int GetSortRank(HeadlineItem item)
        {
            int sortRank = sortOrder.Count;

            for (int rank = 0; rank < sortOrder.Count; rank++)
            {
                if (item.Regions.Contains(sortOrder[rank]))
                {
                    sortRank = rank;
                    break;
                }
            }

            return sortRank;
        }
        #endregion
    }
}