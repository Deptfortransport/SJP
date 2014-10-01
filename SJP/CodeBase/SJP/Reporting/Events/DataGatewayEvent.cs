// *********************************************** 
// NAME             : DataGatewayEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging data gateway event data.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging data gateway event data.
    /// </summary>
    [Serializable()]
    public class DataGatewayEvent : SJPCustomEvent
    {
        #region Private members

        private string feedId;
		private string fileName;
		private DateTime timeStarted;
		private DateTime timeFinished;
		private bool successFlag;
		private int errorCode;

		private static DataGatewayEventFileFormatter fileFormatter = new DataGatewayEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
		/// Constructor for a <c>DataGatewayEvent</c> class. 
		/// A <c>DataGatewayEvent</c> is used
		/// to log file movements in the data gateway 
		/// using the Event Service.
		/// This class must be serializable to allow logging to MSMQs.
		/// </summary>
		/// <param name="sessionId">The session id on which the page was entered.</param>
		/// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or not (false).</param>
		public DataGatewayEvent(string feedId,
									string sessionId, 
									string fileName,
									DateTime timeStarted,
									DateTime timeFinished,
									bool successFlag,
									int errorCode): 
            base(sessionId, false)
		{
			this.feedId = feedId;
			this.fileName = fileName;
			this.timeStarted = timeStarted;
			this.timeFinished = timeFinished;
			this.successFlag = successFlag;
			this.errorCode = errorCode;
		}

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the feed id
        /// </summary>
        public string FeedId
        {
            get { return feedId; }
        }
        /// <summary>
        /// Gets the fileName
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }
        /// <summary>
        /// Gets the timeStarted
        /// </summary>
        public DateTime TimeStarted
        {
            get { return timeStarted; }
        }
        /// <summary>
        /// Gets the timeFinished
        /// </summary>
        public DateTime TimeFinished
        {
            get { return timeFinished; }
        }
        /// <summary>
        /// Gets the successFlag
        /// </summary>
        public bool SuccessFlag
        {
            get { return successFlag; }
        }
        /// <summary>
        /// Gets the ErrorCode
        /// </summary>
        public int ErrorCode
        {
            get { return errorCode; }
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
