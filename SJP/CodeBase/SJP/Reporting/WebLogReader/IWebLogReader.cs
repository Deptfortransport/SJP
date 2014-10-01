// *********************************************** 
// NAME             : IWebLogReader      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Apr 2011
// DESCRIPTION  	: Interface for web log readers
// ************************************************
// 


namespace SJP.Reporting.WebLogReader
{
    /// <summary>
    /// Interface for web log readers
    /// </summary>
    public interface IWebLogReader
    {
        /// <summary>
        /// Processes the workload for a given web log file. 
        /// Logs WorkLoadEvent events for each entry read from web log
        /// that meets the specification passed.
        /// </summary>
        /// <param name="filePath">
        /// Full filepath to web log to process.
        /// </param>
        /// <param name="dataSpecification">
        /// Specification that must be met for web log data to be given a workload event.
        /// </param>
        /// <returns>
        /// Number of workload events logged for the file processed. This is used for informational purposes.
        /// </returns>
        /// <exception cref="TDException">
        /// Thrown if error when processing the web log.
        /// </exception>
        int ProcessWorkload(string filePath, WebLogDataSpecification dataSpecification);

        /// <summary>
        /// Returns the filenames of web logs that should be treated
        /// as active and not processed.
        /// </summary>
        /// <returns>Filenames of active web logs.</returns>
        string[] GetActiveWebLogFileNames();
    }
}
