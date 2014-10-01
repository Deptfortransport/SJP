// *********************************************** 
// NAME             : ISJPStateServer.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: ISJPStateServer interface
// ************************************************
// 

using System;

namespace SJP.UserPortal.StateServer
{
    /// <summary>
    /// Interface for SJPStateServer
    /// </summary>
    public interface ISJPStateServer : IDisposable
    {
        /// <summary>
        /// Saves the supplied deferree object to the state server. 
        /// The session id with the deferred key are joined to form the key to save the object.
        /// Deferree object must be serialiseable
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKey">Key for the deferree object</param>
        /// <param name="deferree">Object to be saved, must be serialiseable</param>
        void Save(string sessionId, string deferredKey, object deferree);

        /// <summary>
        /// Reads a deferree object from the state server.
        /// The session id with the deferred key are joined to form the key to read the object.
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKey">Key for the deferree obect</param>
        /// <returns>Object from state server</returns>
        object Read(string sessionId, string deferredKey);

        /// <summary>
        /// Deletes a deferree object from the state server.
        /// The session id with the deferred key are joined to form the key to delete the object.
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKey">Key for the deferree object</param>
        void Delete(string sessionId, string deferredKey);

        /// <summary>
        /// Locks the derree object(s) in the state server.
        /// The session id with the deferred key(s) are joined to form the key for the object to lock
        /// </summary>
        /// <param name="sessionId">Current session id</param>
        /// <param name="deferredKeys">Keys for the deferree objects</param>
        void Lock(string sessionId, string[] deferredKeys);
    }
}
