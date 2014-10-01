// *********************************************** 
// NAME             : DataGatewayEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: File formatter class for datagateway custom events.
// ************************************************
// 
                
using System;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// File formatter class for datagateway custom events.
    /// </summary>
    public class DataGatewayEventFileFormatter : IEventFormatter
    {
        #region Constructor

        /// <summary>
		/// Default constructor.
		/// </summary>
		public DataGatewayEventFileFormatter()
		{
		}

        #endregion

        #region Public methods

        /// <summary>
		/// Formats the given log event.
		/// </summary>
		/// <param name="logEvent">Log Event to format.</param>
		/// <returns>A formatted string representing the log event.</returns>
		public string AsString(LogEvent logEvent)
		{
			string output = String.Empty;

			if(logEvent is DataGatewayEvent)
			{
				DataGatewayEvent dge = (DataGatewayEvent)logEvent;
				string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

				string outputFormat = "SJP-DGE	FeedId:[{0}]	Filepath:[{1}]	TimeStarted:[{2}]	TimeFinished:[{3}]	SuccessFlag:[{4}]	ErrorCode:[{5}]";
				
				output = String.Format(outputFormat, dge.FeedId, dge.FileName, dge.TimeStarted.ToString(dateTimeFormat), dge.TimeFinished.ToString(dateTimeFormat), dge.SuccessFlag.ToString(), dge.ErrorCode);
			}
			return output;
        }

        #endregion
    }
}
