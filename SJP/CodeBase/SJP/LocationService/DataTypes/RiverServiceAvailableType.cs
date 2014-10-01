// *********************************************** 
// NAME             : RiverServiceAvailableType.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Dec 2011
// DESCRIPTION  	: Define the type of river service availability for a venue
// ************************************************

using System;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Type of river service availability for a venue
    /// </summary>
    [Serializable]
    public enum RiverServiceAvailableType
    {
        No,
        Yes,
        Maybe
    }
}
