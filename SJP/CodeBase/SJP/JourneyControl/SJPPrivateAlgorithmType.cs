// *********************************************** 
// NAME             : SJPPrivateAlgorithmType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Enumeration of SJPPrivateAlgorithmType
// ************************************************
// 
                
using System;

namespace SJP.UserPortal.JourneyControl
{
    [Serializable()]
    public enum SJPPrivateAlgorithmType
    {
        Fastest = 0,
        Shortest = 1,
        MostEconomical = 2,
        Cheapest = 3,
    }
}
