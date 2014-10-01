// *********************************************** 
// NAME             : SJPPublicAlgorithmType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Enumeration for SJPPublicAlgorithmType
// ************************************************
// 
                
using System;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Enumeration for SJPPublicAlgorithmType
    /// </summary>
    [Serializable()]
    public enum SJPPublicAlgorithmType
    {
        Default = 0,
        Fastest = 1,
        NoChanges = 2,
        Max1Change = 3,
        Max2Changes = 4,
        Max3Changes = 5,
        MinChanges = 6
    }
}
