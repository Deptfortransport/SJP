// *********************************************** 
// NAME             : ParkingInterchangeMode.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Apr 2011
// DESCRIPTION  	: Specifies the parking interchange types
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Specifies the parking interchange types
    /// </summary>
    [Serializable]
    public enum ParkingInterchangeMode
    {
        Shuttlebus,
        Rail,
        Cycle,
        Metro,
        Walk
    }
}
