// *********************************************** 
// NAME             : SJPResultManager.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 Mar 2011
// DESCRIPTION  	: SJPResultManager class to hold multiple journey results for a session.
// The class uses the hash of the request used when planning the journey as the key, therefore
// does rely on the caller ensuring proper use of the request hash value.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SessionManager
{
    /// <summary>
    /// SJPResultManager class to hold multiple journey results for a session
    /// </summary>
    [Serializable()]
    public class SJPResultManager : ISJPSessionAware
    {
        #region Private members

        // Max number of results allowed to retain in manager (hard coded value)
        private int maxResults = 5;

        // Holds SJPJourneyRequests
        private Dictionary<string, ISJPJourneyResult> results = null;

        // Maintains a history of results, used to drop the oldest results when maxResults is exceeded
        private Queue<string> resultsQueue = null;

        // Session aware
        private bool isDirty = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SJPResultManager()
        {
            results = new Dictionary<string, ISJPJourneyResult>(maxResults);
            resultsQueue = new Queue<string>(maxResults);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the SJPJourneyResult to the collection within this manager.
        /// </summary>
        /// <param name="sjpJourneyResult"></param>
        public void AddSJPJourneyResult(ISJPJourneyResult sjpJourneyResult)
        {
            if (sjpJourneyResult != null)
            {
                // Use the request hash property as the key. 
                // This is used when retrieving the result by caller
                string key = sjpJourneyResult.JourneyRequestHash;

                // Delete the old(est) result if needed
                if ((resultsQueue.Count >= maxResults) && (!resultsQueue.Contains(key)))
                {
                    string oldResultKey = resultsQueue.Peek();

                    resultsQueue.Dequeue();
                    results.Remove(oldResultKey);

                    // Flag class as changed
                    isDirty = true;
                }


                // Only add if the result doesnt already exist (prevents unnecessary session save)
                if (!results.ContainsKey(key))
                {
                    // Add this new result and queue to keep track of it
                    resultsQueue.Enqueue(key);
                    results.Add(key, sjpJourneyResult);

                    // Flag class as changed
                    isDirty = true;
                }
            }
        }

        /// <summary>
        /// Removes an SJPJourneyResult for the journey request hash if it exists. 
        /// This should only be called where the result is "bad", i.e. it was submitted for a request
        /// but no journeys were planned. This is to allow the same request to be submitted to the journey planners
        /// and allow the journey options page to wait for the "new" result to arrive rather than re-displayinh the
        /// "bad" result
        /// </summary>
        /// <param name="requestHash"></param>
        public void RemoveSJPJourneyResult(string requestHash)
        {
            if (results.ContainsKey(requestHash))
            {
                // This could be optimised, but should be ok
                List<string> queuedKeys = new List<string>(resultsQueue.ToArray());

                // Create a new queue without the requestHash, persisting the queue order
                Queue<string> amendedResultsQueue = new Queue<string>(maxResults);

                foreach(string key in queuedKeys)
                {
                    if (!key.Equals(requestHash))
                    {
                        amendedResultsQueue.Enqueue(key);
                    }
                }

                // Delete the result
                results.Remove(requestHash);

                // Switch the queue to be the amended results queue
                lock (resultsQueue)
                {
                    resultsQueue = amendedResultsQueue;
                }

                // Flag class as changed
                isDirty = true;
            }
        }
        
        /// <summary>
        /// Checks if the manager contains an SJPJourneyResult corresponding to the requestHash value. 
        /// Returns true if it exists within the manager
        /// </summary>
        public bool DoesResultExist(string requestHash)
        {
            return results.ContainsKey(requestHash);
        }

        /// <summary>
        /// Retrieves the SJPJourneyResult corresponding to the requestHash value
        /// Returns null if it doesnt exist
        /// </summary>
        public ISJPJourneyResult GetSJPJourneyResult(string requestHash)
        {
            if (results.ContainsKey(requestHash))
            {
                return results[requestHash];
            }

            return null;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. SJPJourneyResults contained within this manager.
        /// Added for serialization only.
        /// </summary>
        public Dictionary<string, ISJPJourneyResult> SJPJourneyResults
        {
            get { return results; }
            set { results = value; }
        }

        /// <summary>
        /// Read/Write. Queue to keep track of results, old results are removed 
        /// when new results are added if max capacity is exceeded.
        /// Added for serialization only.
        /// </summary>
        public Queue<string> SJPJourneyResultsQueue
        {
            get { return resultsQueue; }
            set { resultsQueue = value; }
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
