// *********************************************** 
// NAME             : JourneyViewState.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Class to manage variables between multiple outward and/or return journeys
//                    The class inteded to be used for single page life cycle and not between pages.
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// Class to manage variables between multiple outward and/or return journeys
    /// </summary>
    public class JourneyViewState : ISJPSessionAware
    {
        #region Private Fields
        private bool congestionChargeAdded = false;
        private bool congestionCostAdded = false;
        private List<string> visitedcongestionCompany = new List<string>();
        #endregion

        #region ISJPSessionAware Members
        /// <summary>
        /// Gets/Sets if the session aware object considers itself to have changed or not
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return false;
            }
            set
            {
                // Intentionally left blank as the JourneyViewState object
                // inteded to live only for the single Page life cycle 
                // and not intended to share the variables between different
                // pages
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Read/write property indicating whether a congestion charge has already been applied 
        /// to the complete journey 
        /// <remarks> 
        /// Used to track congestion charge display for cycle and car journeys
        /// Its assumed here that only one outward journey and only one return journey exists for cycle and car
        /// </remarks>
        /// </summary>
        public bool CongestionChargeAdded
        {
            get
            {
                return congestionChargeAdded;
            }
            set
            {
                congestionChargeAdded = value;
            }
        }

        /// <summary>
        /// Read/write property indicating whether a congestion charge has already been applied 
        /// to the complete journey 
        /// <remarks> 
        /// Used to track congestion charge display for cycle and car journeys
        /// Its assumed here that only one outward journey and only one return journey exists for cycle and car
        /// </remarks>
        /// </summary>
        public bool CongestionCostAdded
        {
            get
            {
                return congestionCostAdded;
            }
            set
            {
                congestionCostAdded = value;
            }
        }

        /// <summary>
        /// Read/write property indicating whether a congestion charge company's charge has already  
        /// been applied to the complete journey 
        /// <remarks> 
        /// Used to track congestion charge display for cycle and car journeys
        /// Its assumed here that only one outward journey and only one return journey exists for cycle and car
        /// </remarks>
        /// </summary>
        public List<string> VisitedCongestionCompany
        {
            get
            {
                return visitedcongestionCompany;
            }
            set
            {
                visitedcongestionCompany = value;
            }

        }

        #endregion
    }
}
