// *********************************************** 
// NAME             : HttpContextHelper.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Mar 2011
// DESCRIPTION  	: Helper class to access http context
// ************************************************


using System.Web;

namespace SJP.Common.Web
{
    /// <summary>
    /// Helper for HttpContext
    /// </summary>
    public static class HttpContextHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Gets the current context. If it is not possible to get the context, 
        /// a HttpException exception is thrown.
        /// </summary>
        /// <returns></returns>
        public static HttpContext GetCurrent()
        {
            HttpContext context = HttpContext.Current;

            //Make sure the context isn't null:
            if (context == null)
            {
                throw new System.Web.HttpException("Cannot get the HttpContext for the current application domain");
            }

            return context;
        }

        #endregion
    }
}
