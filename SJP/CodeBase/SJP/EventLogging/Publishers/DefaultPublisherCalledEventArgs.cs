// *********************************************** 
// NAME             : DefaultPublisherCalledEventArgs.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Event data for DefaultPublisherCalled Event
// ************************************************             
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Event data for DefaultPublisherCalled Event
    /// </summary>
    public class DefaultPublisherCalledEventArgs : EventArgs
    {
        #region Private Fields
        private LogEvent le = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="le">Log Event</param>
        public DefaultPublisherCalledEventArgs(LogEvent le)
        {
            this.le = le;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Read only property returning logging event associated with the DefaultPublisherCalled Event
        /// </summary>
        public LogEvent LogEvent
        {
            get
            {
                return le;
            }
        }
        #endregion
    }
}
