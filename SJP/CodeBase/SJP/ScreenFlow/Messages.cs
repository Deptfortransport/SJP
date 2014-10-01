// *********************************************** 
// NAME             : Messages.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Mar 2011
// DESCRIPTION  	: Messages class used by the exception handling in ScreenFlow.
// ************************************************
// 

namespace SJP.UserPortal.ScreenFlow
{
    /// <summary>
    /// Messages class used by the exception handling in ScreenFlow
    /// </summary>
    public class Messages
    {
        public const string PageTransferDataCacheConstructor =
            "PageTransferDataCacheConstructor constructor.";

        public const string PageTransferDataValidationError =
            "Exception when attempting to validate the Sitemap. ";

        public const string EnumConversionFailed =
            PageTransferDataValidationError + "A conversion to an enumeration has failed. The values that may " +
            "have caused the exception are:\n{0}";

    }
}
