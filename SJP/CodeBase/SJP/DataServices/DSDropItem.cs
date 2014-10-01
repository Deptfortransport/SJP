// *********************************************** 
// NAME             : DSDropItem.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 25 Apr 2011
// DESCRIPTION  	: This class contains info for a single item of a dropdown list.
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DataServices
{
    /// <summary>
    /// This class contains info for a single item of a dropdown list.
    /// </summary>
    public class DSDropItem
    {
        #region Private Fields
        private string resourceID;
        private string itemValue;
        private bool isSelected;
        #endregion

        #region Constructor
        /// <summary>
        /// Private members are set only through the constructor,
        /// provides public read-only properties.
        /// </summary>
        /// <param name="key">Value used by application.</param>
        /// <param name="description">Resource ID for language translation.</param>
        /// <param name="isSelected">Flag indicating of selected by default.</param>
        public DSDropItem(string resourceID, string itemValue, bool isSelected)
        {
            this.resourceID = resourceID;
            this.itemValue = itemValue;
            this.isSelected = isSelected;
        }
        #endregion

        #region Public Properties
        public string ResourceID { get { return resourceID; } }
        public string ItemValue { get { return itemValue; } }
        public bool IsSelected { get { return isSelected; } }
        #endregion
    }
}
