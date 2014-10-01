// *********************************************** 
// NAME             : PageController.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Mar 2011
// DESCRIPTION  	: PageController class provides methods to return the page transfer details
// ************************************************
// 

using SJP.Common;

namespace SJP.UserPortal.ScreenFlow
{
    /// <summary>
    /// PageController class provides methods to return the page transfer details
    /// </summary>
    public class PageController : IPageController
    {
        #region Private members

        private IPageTransferCache pageTransferCache;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for PageController
        /// </summary>
        /// <param name="pageTransferCache">
        /// The PageTransferCache to use for retrieving PageTransferDetails
        /// and PageIds.</param>
        public PageController(IPageTransferCache pageTransferCache)
        {
            this.pageTransferCache = pageTransferCache;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the PageTransferDetail object associated with the
        /// given pageId.
        /// </summary>
        /// <param name="pageId">Id of the page in which to get the
        /// PageTransferDetails object for.</param>
        /// <returns>The PageTransferDetail object associated
        /// with the pageId.</returns>
        public PageTransferDetail GetPageTransferDetails(PageId pageId)
        {
            return (pageTransferCache.GetPageTransferDetails(pageId));
        }

        #endregion
    }
}
