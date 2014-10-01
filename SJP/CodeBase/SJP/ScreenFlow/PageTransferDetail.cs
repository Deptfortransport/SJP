// *********************************************** 
// NAME             : PageTransferDetails.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Mar 2011
// DESCRIPTION  	: PageTransferDetails class holds all the data
// required during page transfers
// ************************************************
// 

using SJP.Common;

namespace SJP.UserPortal.ScreenFlow
{
    /// <summary>
    /// PageTransferDetails class holds all the data
    /// required during page transfers
    /// </summary>
    public class PageTransferDetail
    {
        #region Private members

        private PageId pageId;
        private string pageUrl;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pageId">Id of this page.</param>
        /// <param name="pageUrl">Url of the page.</param>
        public PageTransferDetail(PageId pageId, string pageUrl)
        {
            this.pageId = pageId;
            this.pageUrl = pageUrl;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read Property. Returns the id of the page.
        /// </summary>
        public PageId PageId
        {
            get { return pageId; }
        }

        /// <summary>
        /// Read Property. Returns the Url of the page.
        /// </summary>
        public string PageUrl
        {
            get { return pageUrl; }
        }

        #endregion
    }
}
