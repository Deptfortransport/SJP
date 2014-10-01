// *********************************************** 
// NAME             : ContentGroup.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Class to allow content for a particular content group (usually a page) to be stored and managed in isolation.
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;

namespace SJP.Common.ResourceManager
{
    /// <summary>
    /// Class to allow content for a particular content group (usually a page) 
    /// to be stored and managed in isolation. Many of these will be contained
    /// in the ContentProvider class.
    /// </summary>
    public class ContentGroup
    {
        #region Private Fields

        private readonly ControlPropertyCollectionProvider controlPropertyCollectionProvider = new ControlPropertyCollectionProvider();
        private readonly object getControlPropertiesLock = new object();
        private readonly string groupName;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="groupName">The group name to be handled</param>
        internal ContentGroup(string groupName)
        {
            this.groupName = groupName;
        }

        #endregion

        #region Public Methods
                
        /// <summary>
        /// Gets the current set of content (property names and values). The class
        /// uses the language provided, and manages the caching  of the values.
        /// </summary>
        /// <returns></returns>
        public ControlPropertyCollection GetControlProperties(Language language)
        {
            lock (getControlPropertiesLock)
            {
                //Use the language provided, and get the current theme
                Language currentLanguage = language;

               
                ControlPropertyCollection controlPropertyCollection;

                //First attempt to get the content dictionary for the
                //specified language:
                bool canGetControlPropertyCollection = controlPropertyCollectionProvider.CanGetControlPropertyCollection(currentLanguage, out controlPropertyCollection);

                // Creating expiry date from the data refresh time set up in properties table
                DateTime newExpiryDate = DateTime.MinValue;
                DateTime currentExpiryDate;

                if (canGetControlPropertyCollection)
                {
                    currentExpiryDate = controlPropertyCollection.CreationDateTime;
                }
                else
                {
                    currentExpiryDate = DateTime.MinValue;
                }

                try
                {
                    //Get the time from the database when the settings will expire...
                    string dataRefreshTimeText = Properties.Current["Content.DailyDataRefreshTime"];

                    //Convert the time to an int:
                    int dataRefreshTimeNumber = int.Parse(dataRefreshTimeText);
                    int hours = int.Parse(dataRefreshTimeNumber.ToString("0000").Substring(0, 2));
                    int minutes = int.Parse(dataRefreshTimeNumber.ToString("0000").Substring(2, 2));
                    newExpiryDate = new DateTime(currentExpiryDate.Year, currentExpiryDate.Month, currentExpiryDate.Day, hours, minutes, 0);
                }
                catch
                {
                    // set default expiry date to be 4:00 a.m.
                    DateTime now = DateTime.Now;
                    newExpiryDate = new DateTime(now.Year, now.Month, now.Day, 4, 0, 0);
                }

                //Make sure we use the next day:
                newExpiryDate = newExpiryDate.AddDays(1);

                if (!canGetControlPropertyCollection || newExpiryDate < DateTime.Now)
                {
                    //We couldn't get it, so create a new one:
                    controlPropertyCollection = ContentProvider.GetControlPropertyCollection(GroupName, currentLanguage);
                    controlPropertyCollectionProvider.SetControlPropertyCollection(currentLanguage, controlPropertyCollection);
                }

                return controlPropertyCollection;
            }
        }

        /// <summary>
        /// Safely clears the cached set of content (property names and values)
        /// </summary>
        public void ClearControlProperties()
        {
            lock (getControlPropertiesLock)
            {
                controlPropertyCollectionProvider.ClearControlPropertyCollection();
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the name of the group managed by the instance.
        /// </summary>
        protected string GroupName
        {
            get
            {
                return groupName;
            }
        }

        #endregion
    }
}
