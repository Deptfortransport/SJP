// *********************************************** 
// NAME             : PageControllerFactory.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Mar 2011
// DESCRIPTION  	: PageControllerFactory class that allows the
// ServiceDiscovery to create an instance of the PageController class.
// ************************************************
// 
                
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.UserPortal.ScreenFlow
{
    /// <summary>
    /// PageControllerFactory class that allows the
    /// ServiceDiscovery to create an instance of the PageController class.
    /// </summary>
    public class PageControllerFactory : IServiceFactory
    {
        #region Private members

        private IPageController current;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PageControllerFactory()
        {
            IPageTransferCache pageTransferCache = new PageTransferCache();
            current = new PageController(pageTransferCache);
        }

        #endregion

        #region IServiceFactory methods

        /// <summary>
        ///  Method used by the ServiceDiscovery to get the
        ///  instance of the PageController.
        /// </summary>
        /// <returns>The current instance of the PageController.</returns>
        public Object Get()
        {
            return current;
        }

        #endregion
    }
}
