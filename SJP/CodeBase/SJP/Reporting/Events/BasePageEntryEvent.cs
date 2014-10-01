// *********************************************** 
// NAME             : BasePageEntryEvent.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 20 Apr 2011
// DESCRIPTION  	: Base class for Page Entry Event types
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.EventLogging;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Base class for Page Entry Event types
    /// </summary>
    [Serializable()]
    public class BasePageEntryEvent : SJPCustomEvent
    {
        #region Private members

        private object page = null;
        private int themeId = 0;
        private string partnerName = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="sessionId">Id of the users session</param>
        /// <param name="userLoggedOn">boolean</param>
        public BasePageEntryEvent(string sessionId, bool userLoggedOn)
            : base(sessionId, userLoggedOn)
        {

        }

        /// <summary>
        /// OverLoaded Constructor for the BasePageEntryEvent
        /// </summary>
        /// <param name="sessionId">Id of the users session</param>
        /// <param name="userLoggedOn">boolean</param>
        /// <param name="partnerName">DisplayName of the Partner</param>
        public BasePageEntryEvent(string sessionId, bool userLoggedOn, string partnerName)
            : base(sessionId, userLoggedOn)
        {
            this.partnerName = partnerName;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read-Only property to return the page name as string
        /// </summary>
        virtual public string PageName
        {
            get
            {
                if (page != null)
                {
                    return page.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Read-only property returning ThemeId
        /// </summary>
        virtual public int ThemeId
        {
            get { return themeId; }
        }
                
        /// <summary>
        /// read-only property returning the partner's displayname
        /// </summary>
        public string PartnerName
        {
            get
            {
                return partnerName;
            }
        }

        #endregion
    }
}
