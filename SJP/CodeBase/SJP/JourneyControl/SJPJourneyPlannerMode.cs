// *********************************************** 
// NAME             : SJPPlannerMode.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Enum use to track UI journey plan mode
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.JourneyControl
{
    /// <summary>
    /// Represents the UI journey plan modes
    /// </summary>
    public enum SJPJourneyPlannerMode
    {
        PublicTransport,
        RiverServices,
        ParkAndRide,
        BlueBadge,
        Cycle
    }
}
