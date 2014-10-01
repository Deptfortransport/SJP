// *********************************************** 
// NAME             : SessionManagerKey.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: SessionManagerKey class containing definitions of keys used in SessionManager class
// ************************************************
// 

using System.Collections.Generic;
namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// Global session key repository where each key must be defined 
    /// prior to being used by the SessionManager class.
    /// </summary>
    public class SessionManagerKey
    {
        // Key Definitions
        // To add new key definitions, follow the example format, and add to the keys list:
        //      public static readonly DeferredKey KeyAsyncCallstate = new DeferredKey("AsyncCallState");
        public static readonly DeferredKey KeyPageState = new DeferredKey("InputPageState");
        public static readonly DeferredKey KeyRequestManager = new DeferredKey("RequestManager");
        public static readonly DeferredKey KeyResultManager = new DeferredKey("ResultManager");
        public static readonly DeferredKey KeyStopEventRequestManager = new DeferredKey("StopEventRequestManager");
        public static readonly DeferredKey KeyStopEventResultManager = new DeferredKey("StopEventResultManager");
        public static readonly DeferredKey KeyTravelNewsPageState = new DeferredKey("TravelNewsPageState");
        
        /// <summary>
        /// Journey View State key is intended for single page life cycle only. 
        /// Hence, the object associated with the key should not be persisted in the defferred session
        /// </summary>
        public static readonly DeferredKey KeyJourneyViewState = new DeferredKey("JourneyViewState");
        
        // Identifies all session manager keys
        private static List<string> allSessionManagerKeys = new List<string>();
            
        /// <summary>
        /// Static constructor
        /// </summary>
        static SessionManagerKey()
        {
            // Add key definitions to the keys list:
            //      allSessionManagerKeys.Add(KeyAsyncCallstate.ID);
            allSessionManagerKeys.Add(KeyPageState.ID);
            allSessionManagerKeys.Add(KeyRequestManager.ID);
            allSessionManagerKeys.Add(KeyResultManager.ID);
            allSessionManagerKeys.Add(KeyStopEventRequestManager.ID);
            allSessionManagerKeys.Add(KeyStopEventResultManager.ID);
            allSessionManagerKeys.Add(KeyTravelNewsPageState.ID);
        }

        /// <summary>
        /// Method to return a string array of all deferred keys saved in the session manager
        /// </summary>
        /// <returns></returns>
        public static string[] AllSessionManagerKeys()
        {
            return allSessionManagerKeys.ToArray();
        }
    }
}
