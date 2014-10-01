// *********************************************** 
// NAME             : LogTask.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: Provides a template for all tasks that need to log to the
// Logging Service. It provides a template method run that controls logical 
// sequence of logging and proccessing
// ************************************************
// 

using System.Collections;

namespace SJP.DataLoader
{
    /// <summary>
    /// Provides a template for all tasks that need to log to the
    /// Logging Service. It provides a template method run that controls logical 
    /// sequence of logging and proccessing
    /// </summary>
    public class LogTask
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LogTask()
        {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// A template for all scheduled tasks. Run() calls the other methods within this
        /// class in a pre-determined order to control the sequence of logging.
        /// Sub-classes will usually create their own version of this method.
        /// </summary>
        public virtual int Run()
        {
            int result = 0;

            LogStart();

            result = PerformTask();

            LogFinish(result);

            return result;
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Basic implemetation of the PerformTask event.
        /// Sub classes should override this method
        /// </summary>
        protected virtual int PerformTask()
        {
            return 0;
        }

        /// <summary>
        /// Basic implemetation of the LogStart event.  
        /// Sub classes should override this method
        /// </summary>
        protected virtual void LogStart()
        {
        }

        /// <summary>
        /// Basic implemetation of the LogFinish event.
        /// Sub classes should override this method
        /// </summary>
        protected virtual void LogFinish(int retCode)
        {
        }

        #endregion
    }
}
