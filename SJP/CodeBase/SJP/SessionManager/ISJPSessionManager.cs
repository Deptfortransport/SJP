// *********************************************** 
// NAME             : ISJPSessionManager.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: An interface that lists the Properties and 
// Methods that must be provided by the SJPSessionManager class
// ************************************************
// 

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// Interface for SJPSessionManager
    /// </summary>
    public interface ISJPSessionManager
    {
        #region Public Properties
        
        /// <summary>
        /// Read only property that returns a Session wrapper object for the current session.
        /// </summary>
        ISJPSession Session { get; }

        #region Page state

        /// <summary>
        /// State of the input page
        /// </summary>
        InputPageState PageState { get; set;}

        /// <summary>
        /// State of the Journey results - object to share information between
        /// multiple outward and/or return journeys
        /// </summary>
        JourneyViewState JourneyState { get; set; }
        
        #endregion

        #region Journey managers

        /// <summary>
        /// SJPRequestManager containing journey requests for the session
        /// </summary>
        SJPRequestManager RequestManager { get; set;}
        
        /// <summary>
        /// SJPResultManager containing journey results for the session
        /// </summary>
        /// <remarks>UI elements of the solution MUST NOT use the setter on this property (or change
        /// any objects within the SJPResultManager object). 
        /// Only the Journey Planner elements should update, this is to prevent losing 
        /// journey results in possible race conditions.</remarks>
        SJPResultManager ResultManager { get; set;}
        
        #endregion

        #region Stop Event managers

        /// <summary>
        /// SJPStopEventRequestManager containing stop event requests for the session
        /// </summary>
        SJPStopEventRequestManager StopEventRequestManager { get; set;}
        
        /// <summary>
        /// SJPStopEventResultManager containing stop event journey results for the session
        /// </summary>
        /// <remarks>UI elements of the solution MUST NOT use the setter on this property (or change
        /// any objects within the SJPStopEventResultManager object). 
        /// Only the Journey Planner elements should update, this is to prevent losing 
        /// results in possible race conditions.</remarks>
        SJPStopEventResultManager StopEventResultManager { get; set;}
        
        #endregion

        #region Travel News state

        /// <summary>
        /// State of the travel news page
        /// </summary>
        TravelNewsPageState TravelNewsPageState { get; set; }

        #endregion

        #endregion

        #region Lifecycle Event Methods

        /// <summary>
		/// OnLoad event executes the first time the SessionManager 
		/// is requested via the property 'Current'
		/// </summary>
		void OnLoad();

		/// <summary>
		/// OnFormShift event executes when shift of a form has occurred
		/// but only after the new page's OnLoad event has executed.
		/// </summary>
		/// </summary>
		void OnFormShift();
		
		/// <summary>
		/// OnPreUnload event executes when the page renders.
		/// </summary>
		void OnPreUnload();

		/// <summary>
		/// OnUnload is the last event to occur and any outside access 
		/// should be avoided at this point.
		/// </summary>
		void OnUnload();

		#endregion
    }
}
