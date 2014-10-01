// *********************************************** 
// NAME             : CustomLevelEventArgs.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Event Data associated with CustomLevelEvent event
// ************************************************
                
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Event Data associated with CustomLevelEvent event
    /// </summary>
    public class CustomLevelEventArgs : EventArgs
    {
        #region Private Fields
        private CustomEventLevel customLevel;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customLevel">CustomEventLevel value</param>
        public CustomLevelEventArgs(CustomEventLevel customLevel)
        {
            this.customLevel = customLevel;
        }
        #endregion              

        #region Public Properties
        /// <summary>
        /// Gets the custom event level held in the event args instance.
        /// </summary>
        public CustomEventLevel CustomLevel
        {
            get { return customLevel; }
        }
        #endregion
    }
}
