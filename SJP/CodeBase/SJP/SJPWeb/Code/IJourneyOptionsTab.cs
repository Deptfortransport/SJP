// *********************************************** 
// NAME             : IJourneyOptionsTab.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Interface to be implemented by journey options tab
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SJP.UserPortal.JourneyControl;

namespace SJP.UserPortal.SJPWeb
{

    public delegate void PlanJourney(object sender, EventArgs e);
    /// <summary>
    /// Interface to be implemented by journey options tab
    /// </summary>
    public interface IJourneyOptionsTab
    {
        SJPJourneyPlannerMode PlannerMode { get; }

        bool Disabled { get; set; }

        event PlanJourney OnPlanJourney;
    }
}