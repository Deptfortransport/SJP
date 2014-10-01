// *********************************************** 
// NAME             : ControlPropertyCollectionProvider.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Acts as a manager for ControlPropertyCollections, determining when a collection has expired
// ************************************************


using System.Collections.Generic;

namespace SJP.Common.ResourceManager
{
    /// <summary>
    /// Serves non-expired ControlPropertyCollections
    /// </summary>
    internal sealed class ControlPropertyCollectionProvider
    {
        #region Private Fields

        /// <summary>
        /// Private collection used to stored active ControlPropertyCollections
        /// </summary>
        private Dictionary<Language, ControlPropertyCollection> dictionary = new Dictionary<Language, ControlPropertyCollection>();
        private readonly object dictionaryLock = new object();

        #endregion

        #region Default Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        internal ControlPropertyCollectionProvider()
        {
            //No implementation
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Determines whether a control collection for a specified language and
        /// request uri (related to partner) exists. If it does, the required 
        /// collection is returned as out parameter.
        /// </summary>
        /// <param name="key">Represents the language and theme name to allow the correct
        /// values to be loaded</param>
        /// <param name="controlPropertyCollection">An out parameter that contains the 
        /// required collection if the method return is true.</param>
        /// <returns>Returns true if a collection for the required language exists. 
        /// False otherwise.</returns>
        internal bool CanGetControlPropertyCollection(Language key, out ControlPropertyCollection controlPropertyCollection)
        {
            lock (dictionaryLock)
            {
                controlPropertyCollection = null;

                if (dictionary.ContainsKey(key))
                {
                    controlPropertyCollection = dictionary[key];
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Allows a control collection to be added to the internal collection
        /// </summary>
        /// <param name="key">The language and theme of the collection being added</param>
        /// <param name="controlPropertyCollection">The collection to add</param>
        internal void SetControlPropertyCollection(Language key, ControlPropertyCollection controlPropertyCollection)
        {
            lock (dictionaryLock)
            {
                dictionary[key] = controlPropertyCollection;
            }
        }

        /// <summary>
        /// Allows safe clearing of the internal control collection
        /// </summary>
        internal void ClearControlPropertyCollection()
        {
            lock (dictionaryLock)
            {
                dictionary.Clear();
            }
        }

        #endregion
    }
}
