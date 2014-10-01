// *********************************************** 
// NAME             : LoadTask.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: The LoadTask class is a base class used by other load classes
// ************************************************
// 

using System.IO;
using SJP.Common;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;

namespace SJP.DataLoader
{
    /// <summary>
    /// LoadTask 
    /// </summary>
    public class LoadTask : LogTask
    {
        #region Private members

        // Public properties
        private string dataName;
        private string dataFile;
        private string dataDirectory;
        private string dataLoader;

        // Protected properties
        protected string file;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LoadTask(string dataName, string dataDirectory)
		{
            // If the constructor signature changes, then ensure DataLoaderController.cs is updated
            // to pass in the correct arguments in the reflection instance
            this.dataName = dataName;
            this.dataDirectory = dataDirectory;
		}

        #endregion

        #region Properties

        /// <summary>
        /// The name of the data being loaded
        /// </summary>
        protected string DataName
        {
            get { return dataName; }
            set { dataName = value; }
        }

        /// <summary>
        /// Name of file to be loaded
        /// </summary>
        protected string DataFile
        {
            get { return dataFile; }
            set { dataFile = value; }
        }

        /// <summary>
        /// Directory of the file to be loaded
        /// </summary>
        protected string DataDirectory
        {
            get { return dataDirectory; }
            set { dataDirectory = value; }
        }

        /// <summary>
        /// Data loader name used in logging
        /// </summary>
        protected string DataLoaderName
        {
            get { return dataLoader; }
            set { dataLoader = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
		/// Run class to call the load and logging methods in sequence
		/// </summary>
        /// <param name="dataFile">The data file name (not including path)</param>
        public virtual int Run(string dataFile)
		{
            this.dataFile = dataFile;

            int result = 0;

            // Set the file name to load
            file = dataFile;

            //Check to see if the file exists (path shouldn't have been specified but check anyway)
            if (!File.Exists(file))
            {
                file = dataDirectory + dataFile;

                if (!File.Exists(file))
                {
                    result = (int)SJPExceptionIdentifier.DLDataLoaderFileNotFound;

                    OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                        string.Format ("{0} file [{2}{3}] could not be found for {1}", dataLoader, dataName, dataDirectory, dataFile));
                    Logger.Write(oe);
                }
            }
            
            // Call base Run
            if (result == 0)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info,
                    string.Format("{0} loading file [{1}]", dataLoader, dataFile));
                Logger.Write(oe);

                result = base.Run();
            }

            return result;
		}

        #endregion

        #region Protected methods

        /// <summary>
        /// Performs the actual proccessing of a data file load
        /// </summary>
        protected override int PerformTask()
        {
            return 0;
        }

		/// <summary>
		/// Logs a start operational event to the event logging service
		/// </summary>
		protected override void LogStart()
		{
            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info, 
                string.Format ("{0} load started for {1}", dataLoader, dataName));
			Logger.Write(oe);
		}

		/// <summary>
		/// Log a finish operational event to the event logging service
		/// </summary>
		/// <param name="result">Result status code</param>
		protected override void LogFinish(int result)
		{
            OperationalEvent oe = null;

			if(result == 0)
			{
                oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info,
                    string.Format("{0} load completed successfully for {1} with result code[{2}]", dataLoader, dataName, result));
			}
			else
			{
                oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info,
                    string.Format("{0} load failed for {1} with result code[{2}]", dataLoader, dataName, result));
			}
            
            Logger.Write(oe);
		}

        #endregion
	}
}