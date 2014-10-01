// *********************************************** 
// NAME             : CycleAttributeType.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 19 Apr 2011
// DESCRIPTION  	: Enum to specify type of cycle attribute
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DataServices.CycleAttributes
{
    /// <summary>
    /// Enum to specify type of cycle attribute
    /// </summary>
    public enum CycleAttributeType
    {
        None,
        Link,
        Node,
        Stopover
    }
}
