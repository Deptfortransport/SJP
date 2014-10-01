// *********************************************** 
// NAME             : CycleAttributeCategory.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Enum to identify various cycle attribute categories
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DataServices.CycleAttributes
{
    /// <summary>
    /// Enum to identify various cycle attribute categories
    /// </summary>
    public enum CycleAttributeCategory
    {
        None,
        Crossing,
        Type,
        Barrier,
        Characteristic,
        Manoeuvrability,
        Roundabout
    }

}
