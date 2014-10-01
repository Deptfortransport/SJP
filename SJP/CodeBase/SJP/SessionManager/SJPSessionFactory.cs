// *********************************************** 
// NAME             : SJPSessionFactory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: SJPSessionFactory handles the life-cycle and allocation of SJPSessionManager objects
// through the service discovery
// ************************************************
// 

using System.Collections;
using System.Web;

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// SJPSessionFactory class
    /// </summary>
    public class SJPSessionFactory : ISJPSessionFactory
    {
        #region Private variables

        /// <summary>
        /// Keeps track of all the sessions currently in use for this factory
        /// </summary>
        private Hashtable SessionManagers = new Hashtable();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPSessionFactory()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Factory method to get an appropriate instance of the ISJPSessionManager object.
        /// </summary>
        /// <returns>The correct ISJPSessionManager object</returns>
        public object Get()
        {
            // If a session manager already exists for the current session.
            if (SessionManagers.ContainsKey(HttpContext.Current.Session.SessionID))
            {
                return (SJPSessionManager)SessionManagers[HttpContext.Current.Session.SessionID];
            }
            else
            {
                // If not create new session manager.
                SJPSessionManager sm = new SJPSessionManager(this);
                SessionManagers.Add(HttpContext.Current.Session.SessionID, sm);
                return sm;
            }

        }

        /// <summary>
        /// Removes the session manager for the current session
        /// </summary>
        public void Remove()
        {
            // Remove the session manager for the current session
            if (SessionManagers.ContainsKey(HttpContext.Current.Session.SessionID))
            {
                SessionManagers.Remove(HttpContext.Current.Session.SessionID);
            }
        }

        #endregion
    }
}
