// *********************************************** 
// NAME             : StopEventRequestType.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: Enumeration containing the types of StopEventRequests
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Reporting.Events
{
    /// <summary>
    /// Enumeration containing the types of StopEventRequests
    /// </summary>
    public enum StopEventRequestType
    {
        First = 1,
        Last,
        Time
    }
}
