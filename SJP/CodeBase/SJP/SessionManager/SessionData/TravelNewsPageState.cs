// *********************************************** 
// NAME             : TravelNewsPageState.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: TravelNewsPageState is used to hold session data for the displayed data on
// TravelNews.aspx. This session data can then be accessed by other forms,
// in particular PrintableTravelNews.aspx.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SJP.UserPortal.TravelNews.SessionData;

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// TravelNewsState is used to hold session data for the displayed data on
    /// TravelNews.aspx
    /// </summary>
    [Serializable()]
    public class TravelNewsPageState : ISJPSessionAware
    {
        #region Private members

        //private properties
        private TravelNewsState travelNewsState = null;

        // Session aware
        private bool isDirty = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TravelNewsPageState()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set default state
        /// </summary>
        public void SetDefaultState()
        {
            travelNewsState = new TravelNewsState();
            travelNewsState.SetDefaultState();

            isDirty = true;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. TravelNewsState
        /// </summary>
        public TravelNewsState TravelNewsState
        {
            get { return travelNewsState; }
            set 
            { 
                travelNewsState = value; 
                isDirty = true; 
            }
        }

        #endregion

        #region ISJPSessionAware methods

        /// <summary>
        /// Gets/Sets if the session aware object considers itself to have changed or not
        /// </summary>
        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }

        #endregion
    }
}
