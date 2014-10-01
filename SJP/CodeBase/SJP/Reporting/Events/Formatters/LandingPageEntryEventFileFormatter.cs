// *********************************************** 
// NAME             : LandingPageEntryEventFileFormatter.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: File formatter for the LandingPageEntryEvent
// ************************************************
// 

using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events.Formatters
{
    /// <summary>
    /// File formatter for the LandingPageEntryEvent
    /// </summary>
    public class LandingPageEntryEventFileFormatter : IEventFormatter
    {
        #region Private members

        // Custom datetime pattern based on ISO 8601, to resolution of milliseconds
        private readonly string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

        #endregion

        #region Constructor
        
        /// <summary>
        /// default constructor
        /// </summary>
        public LandingPageEntryEventFileFormatter()
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
            StringBuilder output = new StringBuilder();

            LandingPageEntryEvent lpee = logEvent as LandingPageEntryEvent;

            if (logEvent != null)
            {
                string tab = "\t";

                output.Append("SJP-LPEE" + tab);
                output.Append(lpee.Time.ToString(dateTimeFormat) + tab);
                output.Append("PartnerId: ");
                output.Append(lpee.PartnerID + tab);
                output.Append("ServiceId: ");
                output.Append(lpee.ServiceID);
            }

            return output.ToString();
        }

        #endregion
    }
}
