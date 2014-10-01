// *********************************************** 
// NAME             : Global.asax.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 31 Mar 2011
// DESCRIPTION  	: Global.asax class for methods which occur at Application start and end
// ************************************************
// 
                
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.WebService.CoordinateConvertorService
{
    /// <summary>
    /// Global SJP web application class
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        #region Application events

        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialise Services required by Coordinate Convertor Service.
            SJPServiceDiscovery.Init(new CoordinateConvertorInitialisation());
        }

        #endregion
    }
}