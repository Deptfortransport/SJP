// *********************************************** 
// NAME             : CycleStorageType.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Different types of storage for cycles provided by cycle car park
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.LocationService
{
    /// <summary>
    /// Different types of storage for cycles provided by cycle car park
    /// </summary>
    [Serializable]
    public enum CycleStorageType
    {
        SheffieldStand,
        Lockers,
        Loops
    }
}
