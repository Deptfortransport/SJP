// *********************************************** 
// NAME             : CustomEventLevel.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Enumeration containing levels that a CustomEvent may take
// ************************************************

namespace SJP.Common.EventLogging
{
    /// <summary>
    /// Enumeration containing levels that a <c>CustomEvent</c> may take.
    /// </summary>
    public enum CustomEventLevel : int
    {
        Undefined,
        Off,
        On
    }
}
