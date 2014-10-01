// *********************************************** 
// NAME             : TransferTask.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: The TransferTask class is a base class used by other transfer classes
// ************************************************
// 


using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;

namespace SJP.DataLoader
{
    /// <summary>
    /// TransferTask
    /// </summary>
    public class TransferTask : LogTask
    {
        #region Private members

        // Public properties
        private string dataName;
        private string dataDirectory;
        private string dataTransfer;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TransferTask(string dataName, string dataDirectory)
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
        /// Directory of the file to be loaded
        /// </summary>
        protected string DataDirectory
        {
            get { return dataDirectory; }
            set { dataDirectory = value; }
        }

        /// <summary>
        /// Data transfer name used in logging
        /// </summary>
        protected string DataTransferName
        {
            get { return dataTransfer; }
            set { dataTransfer = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
		/// Run class to call the transfer and logging methods in sequence
		/// </summary>
        public override int Run()
		{
            int result = 0;
            
            // Call base Run
            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info,
                string.Format("{0} transfering file for {1}", dataTransfer, dataName));
            Logger.Write(oe);

            result = base.Run();

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
                string.Format ("{0} transfer started for {1}", dataTransfer, dataName));
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
                    string.Format("{0} transfer completed successfully for {1} with result code[{2}]", dataTransfer, dataName, result));
			}
			else
			{
                oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Info,
                    string.Format("{0} transfer failed for {1} with result code[{2}]", dataTransfer, dataName, result));
			}
            
            Logger.Write(oe);
		}

        #endregion
	}
}