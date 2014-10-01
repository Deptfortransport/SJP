// *********************************************** 
// NAME             : SJPEventCategory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Enumeration containing the categories that can be assicated with an OperationalEvent.
// ************************************************

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Enumeration containing the categories that can be assicated with
    /// an <c>OperationalEvent</c>.
    /// The first element is given a value of 1 so that a non-default
    /// value appears when publishing to Windows Event Logs.
    /// </summary>
    public enum SJPEventCategory
    {
        Business = 1,
        CJP,
        Database,
        ThirdParty,
        Infrastructure
    }
}
