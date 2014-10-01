// *********************************************** 
// NAME             : Installer.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Performs installtion tasks for the EventReceiver service.
// ************************************************
// 
                
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace SJP.Reporting.EventReceiver
{
    /// <summary>
    /// Performs installtion tasks for the EventReceiver service
    /// </summary>
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        #region Private members

        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Installer()
        {
            InitializeComponent();

            this.serviceProcessInstaller = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();

            // 
            // serviceProcessInstaller
            // 

            // Commenting out the following line forces user and password to be entered on installation.
            // If this line is uncommented then appropriate priveleges to resources (eg MSMQ) must be granted.
            //this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;			
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;			

            // 
            // serviceInstaller
            // 
            this.serviceInstaller.Description = "Service to run on SJP servers and to monitor for reporting events in message queues";
            this.serviceInstaller.ServiceName = "EventReceiver";
            this.serviceInstaller.DisplayName = "EventReceiver";

            this.serviceInstaller.StartType = ServiceStartMode.Automatic;

            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                this.serviceProcessInstaller,
                this.serviceInstaller});

        }

        #endregion
    }
}
