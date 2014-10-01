// *********************************************** 
// NAME             : CommandAndControlAgentInitialisation.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Installer for the Command&ControlAgent Service
// ************************************************
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace SJP.UserPortal.CCAgent
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
