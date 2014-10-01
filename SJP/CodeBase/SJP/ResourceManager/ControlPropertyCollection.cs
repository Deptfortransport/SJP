// *********************************************** 
// NAME             : ControlPropertyCollection.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Contains control property names and values in a fashion that enables control looping
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.ResourceManager
{
    /// <summary>
    /// Contains control property names and values in a fashion that enables control looping
    /// </summary>
    public class ControlPropertyCollection
    {
        #region Private Fields

        /// <summary>
        /// Field to store all controls, properties and values. Although the field looks
        /// nasty and complex, it must be this way in order to provide the required
        /// functionality. The outer dictionary is keyed on control name, and has
        /// an inner dictionary as its values. The inner dictionary is keyed on 
        /// property name. Thus, contentDictionary["txtTest"] will return a 
        /// dictionary of all property names and values available for a control 
        /// called txtTest. Going one step further, 
        /// contentDictionary["txtTest"]["Text"] will return the string value
        /// of the property Text for control txtTest.
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, string>> contentDictionary = new Dictionary<string, Dictionary<string, string>>();
        private readonly DateTime creationDateTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor in which all required data is set for the collection. The
        /// data is stored in a "double dictionary". See comments against the
        /// field contentDictionary for more information.
        /// </summary>
        /// <param name="contentDictionary">All data needed to set up the object. 
        /// See comments against the field contentDictionary for more 
        /// information as to why a "double dictionary" has been used. </param>
        internal ControlPropertyCollection(Dictionary<string, Dictionary<string, string>> contentDictionary)
        {
            this.contentDictionary = contentDictionary;
            creationDateTime = DateTime.Now;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of controls handled by the collection.
        /// </summary>
        /// <returns></returns>
        public int GetControlCount()
        {
            return contentDictionary.Keys.Count;
        }

        /// <summary>
        /// Gets the date and time the collection was created
        /// </summary>
        public DateTime CreationDateTime
        {
            get
            {
                return creationDateTime;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Given a control name, the name of all available properties in the collection 
        /// for the control are returned, as a string array.
        /// </summary>
        public string[] GetPropertyNames(string controlName)
        {
            //Make the input lower:
            controlName = controlName.ToLower();

            //If the inner dictionary contains nothing against the control, then
            //the count is zero:
            if (!contentDictionary.ContainsKey(controlName))
            {
                return new string[] { };
            }

            //Make an array to return:
            string[] returnArray = new string[contentDictionary[controlName].Keys.Count];

            //Now copy the key names to the return array:
            contentDictionary[controlName].Keys.CopyTo(returnArray, 0);

            return returnArray;
        }
        
        /// <summary>
        /// Given a control name and property name, the relevant value is
        /// returned. If a matching value cannot be found, null is returned
        /// </summary>
        public string GetPropertyValue(string controlName, string propertyName)
        {
            //Make the inputs lower:
            controlName = controlName.ToLower();
            propertyName = propertyName.ToLower();

            try
            {
                return contentDictionary[controlName][propertyName];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
        #endregion
    }
}
