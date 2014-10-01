// *********************************************** 
// NAME             : Global.asax.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Global application site class 
// ************************************************
// 

using System;
using System.IO;
using System.Runtime.Remoting;
using System.Web;
using SJP.Common;
using SJP.Common.EventLogging;
using SJP.Common.ResourceManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.ServiceDiscovery.Initialisation;
using SJP.UserPortal.SessionManager;
using Logger = System.Diagnostics.Trace;

namespace SJP.UserPortal.SJPWeb
{
    /// <summary>
    /// Global SJP web application class
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        #region Private members

        // Resource manager
        private static SJPResourceManager sjpResourceManager = null;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Read/write. SJPResourceManager to be used throughout application
        /// </summary>
        public static SJPResourceManager SJPResourceManager
        {
            get { return sjpResourceManager; }
            set { sjpResourceManager = value; }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        ///  Method to set up the resource manager, if needed
        /// </summary>
        public static void InstantiateResourceManager()
        {
            if (sjpResourceManager == null)
            {
                // Instantiate a resource manager
                sjpResourceManager = new SJPResourceManager();
            }
        }

        #endregion

        #region Application events

        /// <summary>
        /// Application Start - Only fires once
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            #if DEBUG
            //System.Diagnostics.Debugger.Break();
            #endif

            // Initialise the service discovery
            SJPServiceDiscovery.Init(new SJPWebInitialisation());

            // Initialise resource manager
            Global.InstantiateResourceManager();

            // Setup remoting configuration
            string applicationPath = Context.Server.MapPath("~");
            string configPath = Path.Combine(applicationPath, "Remoting.config");
            if (File.Exists(configPath))
            {
                RemotingConfiguration.Configure(configPath, false);
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "Loaded remoting configuration file: " + configPath));
            }
            else
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Could not find remoting configuration file: " + configPath));	
        
        }

        /// <summary>
        /// Session Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            // Initialise the intial page entry
            SJPSessionManager.Current.Session[SessionKey.NextPageId] = PageId.Empty;
            SJPSessionManager.Current.Session[SessionKey.Transferred] = false;
        }

        /// <summary>
        /// Application BeginRequest - Fires for every request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetNoStore();

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            HttpContext.Current.Response.Cache.AppendCacheExtension("no-cache=\"set-cookie\"");
        }

        /// <summary>
        /// Application AuthenticateRequest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Application Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            // Server.GetLastError() normally returns an HttpUnhandledException 
            // object that wraps the exception we actually want to log ...
            Exception ex = Server.GetLastError().InnerException;

            if (ex == null)
            {
                ex = Server.GetLastError();
            }

            string message = "Unhandled Exception on page: " + Request.Path
                + "\n\nMessage:\n " + ex.Message
                + "\n\nStack trace:\n" + ex.StackTrace;

            Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, message));
        }

        /// <summary>
        /// Session End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Application End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {

        }

        #endregion
    }
}