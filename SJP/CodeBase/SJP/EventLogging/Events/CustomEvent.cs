// *********************************************** 
// NAME             : CustomEvent.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Abstract class that is used by clients to define their own event types
// ************************************************
                
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Abstract class that is used by clients to define their own event types.
    /// </summary>
    [Serializable]
    public abstract class CustomEvent : LogEvent
    {

        #region Private Static Fields
        
        /// <summary>
        /// The filter class that is used to determine whether the custom event should
        /// be logged or not.
        /// </summary>
        private static IEventFilter filter = new CustomEventFilter();

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a new CustomEvent
        /// </summary>
        protected CustomEvent()
            : base()
        {
            // set class name - used as an id to associate events to publishers, and also in config properties
            this.ClassName = this.GetType().Name;
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the default event formatter.
        /// </summary>
        public IEventFormatter DefaultFormatter
        {
            get { return SJP.Common.EventLogging.DefaultFormatter.Instance; }
        }

        /// <summary>
        /// Gets the event filter.
        /// </summary>
        public override IEventFilter Filter
        {
            get { return filter; }
        }

        #endregion


    }
}
