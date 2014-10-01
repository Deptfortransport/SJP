// *********************************************** 
// NAME             : CustomLevelsEventArgs.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Event Data associated with CustomLevelsEvent
// ************************************************
                
using System;
using System.Collections.Generic;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Event Data associated with CustomLevelsEvent
    /// </summary>
    public class CustomLevelsEventArgs : EventArgs
    {
        #region Private Fields
        private Dictionary<string, CustomEventLevel> customLevels;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customLevels"></param>
        public CustomLevelsEventArgs(Dictionary<string, CustomEventLevel> customLevels)
        {
            this.customLevels = customLevels;
        }
        #endregion

       
        #region Public Properties
        /// <summary>
        /// Gets the custom event levels held in the event args instance.
        /// </summary>
        public Dictionary<string, CustomEventLevel> CustomLevels
        {
            get { return customLevels; }
        }
        #endregion
    }
}
