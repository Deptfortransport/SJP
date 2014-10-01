// *********************************************** 
// NAME             : PageEntryEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: Defines a custom event for logging page entry event data.
// ************************************************
// 
                
using System;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Reporting.Events.Formatters;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Defines a custom event for logging page entry event data.
    /// </summary>
    [Serializable()]
    public class PageEntryEvent : BasePageEntryEvent
    {
        #region Private members

        private PageId page;
        private int themeId;

        private static PageEntryEventFileFormatter fileFormatter = new PageEntryEventFileFormatter();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for a <c>PageEntryEvent</c> class. 
        /// A <c>PageEntryEvent</c> is used
        /// to log page entry transaction data using the Event Service.
        /// This class must be serializable to allow logging to MSMQs.
        /// </summary>
        /// <param name="sessionId">The session id on which the page was entered.</param>
        /// <param name="eventType">The page identifier of the page entered.</param>
        /// <param name="userLoggedOn">Flag indicating whether a registered user is logged on (true) or not (false).</param>
        public PageEntryEvent(PageId page,
                              string sessionId,
                              bool userLoggedOn)
            : base(sessionId, userLoggedOn)
        {
            this.page = page;
            this.themeId = 0;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the page identifier
        /// </summary>
        public PageId Page
        {
            get { return page; }
        }
        
        /// <summary>
        /// Read-Only property to return the page name as string
        /// </summary>
        override public string PageName
        {
            get { return page.ToString(); }
        }

        /// <summary>
        /// Read-only property returning ThemeId
        /// </summary>
        override public int ThemeId
        {
            get { return themeId; }
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
