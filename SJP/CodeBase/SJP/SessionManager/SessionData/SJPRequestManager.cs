// *********************************************** 
// NAME             : SJPRequestManager.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: SJPRequestManager class to hold multiple journey requests for a session.
// The class uses the hash property of the request as the key to ensure "identical" requests do not fill
// the historical requests made. This allows the SJPResultManager to serve the already planned
// journey result rather than making a duplicate call to the CJP (removes wasted performance overhead).
// This therefore does rely on the caller ensuring proper use of the request hash value.
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// SJPRequestManager class to hold multiple journey requests for a session
    /// </summary>
    [Serializable()]
    public class SJPRequestManager : ISJPSessionAware
    {
        #region Private members

        // Max number of requests allowed to retain in manager (hard coded value)
        private int maxRequests = 5;

        // Holds SJPJourneyRequests
        private Dictionary<string, ISJPJourneyRequest> requests = null;
        
        // Maintains a history of requests, used to drop the oldest request when maxRequests is exceeded
        private Queue<string> requestQueue = null;

        // Session aware
        private bool isDirty = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPRequestManager()
        {
            requests = new Dictionary<string, ISJPJourneyRequest>(maxRequests);
            requestQueue = new Queue<string>(maxRequests);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the SJPJourneyRequest to the collection within this manager.
        /// </summary>
        /// <param name="sjpJourneyRequest"></param>
        public void AddSJPJourneyRequest(ISJPJourneyRequest sjpJourneyRequest)
        {
            if (sjpJourneyRequest != null)
            {
                // Use the request hash property as the key.
                string key = sjpJourneyRequest.JourneyRequestHash;

                // Delete the old(est) request if needed
                if ((requestQueue.Count >= maxRequests) && (!requestQueue.Contains(key)))
                {
                    string oldRequestKey = requestQueue.Peek();

                    requestQueue.Dequeue();
                    requests.Remove(oldRequestKey);
                }

                
                // Check if the request already exists
                if (!requests.ContainsKey(key))
                {
                    // Add this new request and queue to keep track of it
                    requestQueue.Enqueue(key);
                    requests.Add(key, sjpJourneyRequest);
                }
                else
                {
                    // Already exists, update it
                    requests[key] = sjpJourneyRequest;
                }


                // Flag class as changed
                isDirty = true;
            }
        }

        /// <summary>
        /// Retrieves the SJPJourneyRequest from the manager. 
        /// Returns null if it doesnt exist
        /// </summary>
        /// <param name="RequestId"></param>
        public ISJPJourneyRequest GetSJPJourneyRequest(string requestHash)
        {
            if (requests.ContainsKey(requestHash))
            {
                return requests[requestHash];
            }
            
            return null;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. SJPJourneyRequests contained within this manager.
        /// Added for serialization only.
        /// </summary>
        public Dictionary<string, ISJPJourneyRequest> SJPJourneyRequests
        {
            get { return requests; }
            set { requests = value; }
        }

        /// <summary>
        /// Read/Write. Queue to keep track of requests, old requests are removed 
        /// when new requests are added if max capacity is exceeded.
        /// Added for serialization only.
        /// </summary>
        public Queue<string> SJPJourneyRequestsQueue
        {
            get { return requestQueue; }
            set { requestQueue = value; }
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
