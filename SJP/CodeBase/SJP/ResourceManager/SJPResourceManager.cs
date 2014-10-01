// *********************************************** 
// NAME             : SJPResourceManager.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Manages access to SJP resource content data
// ************************************************


namespace SJP.Common.ResourceManager
{
    
    /// <summary>
    /// Summary description for SJPResourceManager, which inherits from ResourceManager
    /// This class contains look up style methods to retrieve language resources.
    /// </summary>
    public class SJPResourceManager
    {
        #region Constants

        // Define new resource groups here.
        // These must manually be added to the SJPContent.ContentGroup database table
        public static string GROUP_DEFAULT = "General";
        public static string GROUP_SITEMAP = "Sitemap";
        public static string GROUP_HEADERFOOTER = "HeaderFooter";
        public static string GROUP_JOURNEYOUTPUT = "JourneyOutput";
        public static string GROUP_ANALYTICS = "Analytics";
        public static string GROUP_MOBILE = "Mobile";

        // Define new resource collections here (for convienience, e.g. if used in many places)
        public static string COLLECTION_DEFAULT = "General";
        public static string COLLECTION_JOURNEY = "Journey";
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SJPResourceManager class
        /// </summary>
        public SJPResourceManager()
        {
            
        }
        
        #endregion
                
        #region Public Methods

        /// <summary>
        /// Gets resource values for given key, using default collection and group
        /// </summary>
        /// <param name="key">The key of the resource data to retrieve</param>
        /// <returns>String representing the value of the resource</returns>
        public string GetString(Language language, string key)
        {
            return GetString(language, COLLECTION_DEFAULT, key);
        }

        /// <summary>
        /// Gets resource values for given collection and key, using the default group
        /// </summary>
        /// <param name="collectionName">The collection of the resource data</param>
        /// <param name="key">The key of the resource data to retrieve</param>
        /// <returns>String representing the value of the resource</returns>
        public string GetString(Language language, string collectionName, string key)
        {
            return GetString(language, GROUP_DEFAULT, collectionName, key);
        }

        /// <summary>
        /// Gets resource values for given key, collection, and group. 
        /// The language provided is used instead of the global CurrentLanguage value
        /// </summary>
        /// <param name="language">The language to use</param>
        /// <param name="groupName">The group of the resource data</param>
        /// <param name="collectionName">The collection of the resource data</param>
        /// <param name="key">The key of the resource data to retrieve</param>
        /// <returns>String representing the value of the resource</returns>
        public string GetString(Language language, string groupName, string collectionName, string key)
        {
            return ContentProvider.Instance[groupName].GetControlProperties(language).GetPropertyValue(collectionName, key);
        }
                
        #endregion

    }
}
