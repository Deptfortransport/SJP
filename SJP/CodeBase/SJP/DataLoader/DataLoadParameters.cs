// *********************************************** 
// NAME             : DataLoadParameters.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: DataLoadParameters class to hold parameters supplied through command line arguments and 
// read from properties configuration
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.DatabaseInfrastructure;
using System.Data.SqlClient;
using SJP.Common.PropertyManager;
using SJP.Common.Extenders;

namespace SJP.DataLoader
{
    /// <summary>
    /// DataLoadParameters class to hold parameters supplied through command line arguments and 
    /// read from properties configuration
    /// </summary>
    public class DataLoadParameters
    {
        #region Private members

        private string dataName;

        private bool dataTransfer;
        private string dataTransferClassName;
        private string dataTransferClassAssembly;

        private bool dataLoad;
        private string dataLoadClassName;
        private string dataLoadClassAssembly;

        private string directory;
        private bool directoryClean;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataName"></param>
        public DataLoadParameters(string dataName, bool dataTransfer, bool dataLoad)
		{
            this.dataName = dataName;
            this.dataTransfer = dataTransfer;
            this.dataLoad = dataLoad;
		}

        #endregion

        #region Properties

        /// <summary>
        /// DataName is the unique data name that provides the key for obtaining
        /// other parameters from the properties configuration.
        /// </summary>
        public string DataName
        {
            get { return dataName; }
        }

        /// <summary>
        /// DateTransfer indicates if data should be transfered to the processing directory
        /// from the configured incoming locations
        /// </summary>
        public bool DataTransfer
        {
            get { return dataTransfer; }
        }

        /// <summary>
        /// DataTransferClass is the subclass of TransferTask that needs to be instantiated
        /// to perform the data transfer
        /// </summary>
        public string DataTransferClass
        {
            get { return dataTransferClassName; }
        }

        /// <summary>
        /// DataTransferClassAssembly is the name of the assembly that the sub-class can be found in
        /// </summary>
        public string DataTransferClassAssembly
        {
            get { return dataTransferClassAssembly; }
        }

        /// <summary>
        /// DataLoad indicates if data should be loaded from the processing directory
        /// into its data store
        /// </summary>
        public bool DataLoad
        {
            get { return dataLoad; }
        }

        /// <summary>
        /// DataLoadClass is the subclass of LoadTask that needs to be instantiated
		/// to perform the data load
		/// </summary>
		public string DataLoadClass
		{
			get { return dataLoadClassName; }
		}

		/// <summary>
        /// DataLoadClassAssembly is the name of the assembly that the sub-class can be found in
		/// </summary>
        public string DataLoadClassAssembly
        {
            get { return dataLoadClassAssembly; }
        }
        
		/// <summary>
		/// DirectoryName is used to indicate any particular directory a file must be copied to
		/// for processing. An empty field indicates that the current directory should be used.
		/// </summary>
		public string Directory
		{
			get { return directory; }
        }

        /// <summary>
        /// DirectoryClean is used to indicate if the processing directory should be cleaned (files deleted)
        /// after the data load has completed
        /// </summary>
        public bool DirectoryClean
        {
            get { return directoryClean; }
        }

		#endregion

		#region Public Methods

		/// <summary>
		/// Reads the properties configuration for this data load assigning to the 
        /// corresponding properties of this class.
		/// </summary>
		public bool SetData()
		{
			bool dataSetOK = false;

            // No dataname supplied
            if (!string.IsNullOrEmpty(dataName))
            {
                string Prop_Class_Transfer = "DataLoader.Configuration.{0}.Class.Transfer";
                string Prop_ClassAssembly_Transfer = "DataLoader.Configuration.{0}.ClassAssembly.Transfer";
                string Prop_Class_Load = "DataLoader.Configuration.{0}.Class.Load";
                string Prop_ClassAssembly_Load = "DataLoader.Configuration.{0}.ClassAssembly.Load";
                string Prop_Directory = "DataLoader.Configuration.{0}.Directory";
                string Prop_DirectoryClean = "DataLoader.Configuration.{0}.Directory.Clean";

                // Load the properties, if any are missing then data not set ok

                dataTransferClassName = Properties.Current[string.Format(Prop_Class_Transfer, dataName)];
                dataTransferClassAssembly = Properties.Current[string.Format(Prop_ClassAssembly_Transfer, dataName)];
                dataLoadClassName = Properties.Current[string.Format(Prop_Class_Load, dataName)];
                dataLoadClassAssembly = Properties.Current[string.Format(Prop_ClassAssembly_Load, dataName)];
                directory = Properties.Current[string.Format(Prop_Directory, dataName)];
                directoryClean = Properties.Current[string.Format(Prop_DirectoryClean, dataName)].Parse(false);

                if (string.IsNullOrEmpty(directory))
                {
                    directory = ".\\" + dataName;
                }
                if (!directory.EndsWith("\\"))
                {
                    directory += "\\";
                }

                // If data transfer required and class not specified, or data load required and class not specified, then error
                if (
                    (dataLoad && (string.IsNullOrEmpty(dataLoadClassName) || string.IsNullOrEmpty(dataLoadClassAssembly)))
                    || (dataTransfer && (string.IsNullOrEmpty(dataTransferClassName) || string.IsNullOrEmpty(dataTransferClassAssembly)))
                   )
                {
                    // Missing properties
                    dataSetOK = false;
                }
                else
                {
                    dataSetOK = true;
                }
            }

			return dataSetOK;
		}
		#endregion
	}
}
