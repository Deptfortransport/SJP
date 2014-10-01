// *********************************************** 
// NAME             : IPageController.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Mar 2011
// DESCRIPTION  	: IPageController interface 
// ************************************************
// 

using SJP.Common;

namespace SJP.UserPortal.ScreenFlow
{
    /// <summary>
    /// Interface for PageController.
    /// </summary>
    public interface IPageController
    {
        /// <summary>
        /// Returns the PageTransferDetails object associated with the pageId.
        /// </summary>
        PageTransferDetail GetPageTransferDetails(PageId pageId);
    }
}
