// *********************************************** 
// NAME             : IPageTransferCache.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Mar 2011
// DESCRIPTION  	: IPageTransferCache interface
// ************************************************
// 

using SJP.Common;

namespace SJP.UserPortal.ScreenFlow
{
    /// <summary>
    /// Interface for PageTransferCache
    /// </summary>
    public interface IPageTransferCache
    {
        /// <summary>
        /// Returns the PageTransferDetail object for the given pageId.
        /// </summary>
        PageTransferDetail GetPageTransferDetails(PageId pageId);
    }
}
