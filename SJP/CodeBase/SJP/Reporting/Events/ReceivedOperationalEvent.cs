// *********************************************** 
// NAME             : ReceivedOperationalEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: ReceivedOperationalEvent class for wrapping 
// Operational Events as a custom event. Used by the
// Event Receiver to log 'received' Operational Events
// as opposed to Operational Events it generates.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a wrapper class for storing a reference to an
    /// Operational Event.
    /// </summary>
    [Serializable()]
    public class ReceivedOperationalEvent : SJPCustomEvent
    {
        #region Private members

        private static ReceivedOperationalEventFileFormatter fileFormatter = new ReceivedOperationalEventFileFormatter();

        private OperationalEvent operationalEvent;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="operationalEvent">
        /// A reference to this event is stored in the class.
        /// </param>
        /// <remarks>
        /// It is only necessary to store a reference to the operational event, 
        /// rather than a clone, since the Operational Event properties
        /// cannot be changed once set on constrution.
        /// </remarks>
        public ReceivedOperationalEvent(OperationalEvent operationalEvent)
            : base("NoSession", false)
        {
            this.operationalEvent = operationalEvent;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the wrapped Operational Event.
        /// </summary>
        public OperationalEvent WrappedOperationalEvent
        {
            get { return operationalEvent; }
        }

        /// <summary>
        /// Provides an event formatter for publishing to files.
        /// </summary>
        override public IEventFormatter FileFormatter
        {
            get { return fileFormatter; }
        }

        #endregion


    }
}
