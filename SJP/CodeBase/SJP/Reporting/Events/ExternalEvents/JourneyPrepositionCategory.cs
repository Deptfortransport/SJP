// *********************************************** 
// NAME             : JourneyPrepositionCategory.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 June 2011
// DESCRIPTION  	: Defines categories of Location 
// Request.
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Enumeration containing categories of Location Request
    /// </summary>
    public enum JourneyPrepositionCategory
    {
        From = 1,
        To,
        Via,
        StopEvent,
        FirstLastService
    }
}
