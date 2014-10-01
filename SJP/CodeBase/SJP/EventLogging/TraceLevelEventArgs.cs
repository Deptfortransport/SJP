// *********************************************** 
// NAME             : TraceLevelEventArgs.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Class to store event data for TraceLevelChangeEvent
// ************************************************           
                
using System;

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Class to store event data for TraceLevelChangeEvent
    /// </summary>
    public class TraceLevelEventArgs: EventArgs
    {
        #region Private Fields
        private SJPTraceLevel traceLevel;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="traceLevel"></param>
        public TraceLevelEventArgs(SJPTraceLevel traceLevel)
        {
            this.traceLevel = traceLevel;
        }
        #endregion

        

        #region Public Properties
        /// <summary>
        /// Gets the trace level held in the event args instance.
        /// </summary>
        public SJPTraceLevel TraceLevel
        {
            get { return traceLevel; }
        }
        #endregion
    }
}
