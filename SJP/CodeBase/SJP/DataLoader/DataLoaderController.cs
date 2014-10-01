// *********************************************** 
// NAME             : DataLoaderController.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 29 Feb 2012
// DESCRIPTION  	: DataLoaderController class for performing the data load work for the dataname:
// reads configuration properties,
// transfers files,
// instantiates the loader class to do the load
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;
using System.Reflection;
using System.IO;

namespace SJP.DataLoader
{
    /// <summary>
    /// DataLoaderController class for performing the data load work
    /// </summary>
    public class DataLoaderController
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DataLoaderController()
        {
        }

        #endregion

        #region Public methods

        public int Load(string dataName, bool dataTransfer, bool dataLoad)
        {
            int statusCode = 0;

            DataLoadParameters dataLoadParameters = new DataLoadParameters(dataName, dataTransfer, dataLoad);

            if (dataLoadParameters.SetData())
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("DataName [{0}]", dataName));
                
                #region Transfer data files

                // Transfer files to the processing directory if required (default is true)
                if (dataLoadParameters.DataTransfer)
                {
                    // Use refleciton to establish the class that needs to be instantiated for 
                    // this data transfer and then create an instance of that class.

                    object[] initArguments = { dataName, dataLoadParameters.Directory };

                    Assembly importAssembly = Assembly.LoadFrom(dataLoadParameters.DataTransferClassAssembly);
                    Type reflectionClass = importAssembly.GetType(dataLoadParameters.DataTransferClass);
                    object reflectionInstance = Activator.CreateInstance(reflectionClass, initArguments);

                    statusCode = TransferDataFile(dataName, dataLoadParameters, reflectionInstance);

                    if (statusCode != 0)
                    {
                        OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                            string.Format("{0}:Data file transfer failed for {1}",
                                statusCode.ToString(), dataName));
                        Logger.Write(oe);

                        return statusCode;
                    }
                }

                #endregion

                #region Load data files

                // Load data from the processing directory if required (default is true)
                if (dataLoadParameters.DataLoad)
                {
                    try
                    {
                        // Use refleciton to establish the class that needs to be instantiated for 
                        // this data load and then create an instance of that class.

                        object[] initArguments = { dataName, dataLoadParameters.Directory };

                        Assembly importAssembly = Assembly.LoadFrom(dataLoadParameters.DataLoadClassAssembly);
                        Type reflectionClass = importAssembly.GetType(dataLoadParameters.DataLoadClass);
                        object reflectionInstance = Activator.CreateInstance(reflectionClass, initArguments);

                        statusCode = LoadDataFile(dataName, dataLoadParameters, reflectionInstance);

                        if (statusCode != 0)
                        {
                            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                                string.Format("{0}:Data file load was not completed for {1}",
                                statusCode.ToString(), dataName));
                            Logger.Write(oe);

                            return statusCode;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Throw exception to let main program log and handle
                        string message = string.Format("{0}:Unexpected exception during data file load for {1}. Message:{2}",
                                    statusCode.ToString(), dataName, ex.Message);

                        throw new SJPException(message, false, SJPExceptionIdentifier.DLDataLoaderUnexpectedException, ex);
                    }
                }

                #endregion

                CleanUp(dataName, dataLoadParameters);
            }
            else // If params were not read
            {
                statusCode = (int)SJPExceptionIdentifier.DLDataLoaderInvalidConfiguration;

                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                    string.Format("{0}:Unable to read configuration properties for {1}",
                        statusCode.ToString(), dataName));
                Logger.Write(oe);
            }

            return statusCode;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Identifies the file paths to transfer the file from, and calls the data transfer class to perform the transfer
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="dataLoadParameters"></param>
        /// <param name="reflectionInstance"></param>
        /// <returns></returns>
        private int TransferDataFile(string dataName, DataLoadParameters dataLoadParameters, Object reflectionInstance)
        {
            Console.WriteLine();

            int statusCode = 1;

            try
            {
                // Call the class to perform the load
                Type reflectionClass = reflectionInstance.GetType();
                object[] methodArguments = { };

                Console.WriteLine(string.Format("Calling {0}", dataLoadParameters.DataTransferClass));

                statusCode = (int)reflectionClass.InvokeMember("Run", BindingFlags.InvokeMethod, null, reflectionInstance, methodArguments);
            }
            catch (Exception ex)
            {
                statusCode = (int)SJPExceptionIdentifier.DLDataLoaderInvokingTransferTaskFailed;

                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                        string.Format("{0}:Unexpected exception invoking transfer task for {1}. Message:{2}",
                            statusCode.ToString(), dataName, ex.Message));
                Logger.Write(oe);
            }

            return statusCode;
        }

        /// <summary>
        /// Identifies the date file to load, and calls the data load class to perform the load
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="dataLoadParameters"></param>
        /// <param name="reflectionInstance"></param>
        /// <returns></returns>
        private int LoadDataFile(string dataName, DataLoadParameters dataLoadParameters, Object reflectionInstance)
        {
            Console.WriteLine();

            int statusCode = 1;
                        
            try
            {
                // Get data file to load
                FileInfo fileInfo = GetDataFile(dataLoadParameters);

                if ((fileInfo != null) && (fileInfo.Exists))
                {
                    // Call the class to perform the load
                    Type reflectionClass = reflectionInstance.GetType();
                    object[] methodArguments = { fileInfo.Name };

                    Console.WriteLine(string.Format("Calling {0}", dataLoadParameters.DataLoadClass));
                    
                    statusCode = (int)reflectionClass.InvokeMember("Run", BindingFlags.InvokeMethod, null, reflectionInstance, methodArguments);
                }
                else
                {
                    Console.WriteLine(string.Format("Not called {0}, data file was not found", dataLoadParameters.DataLoadClass));
                    statusCode = 0;
                }
            }
            catch (Exception ex)
            {
                statusCode = (int)SJPExceptionIdentifier.DLDataLoaderInvokingLoadTaskFailed;
                
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                        string.Format("{0}:Unexpected exception invoking load task for {1}. Message:{2}",
                            statusCode.ToString(), dataName, ex.Message));
                Logger.Write(oe);
            }

            return statusCode;
        }

        /// <summary>
        /// Gets the data file info to load
        /// </summary>
        /// <param name="dataLoadParameters"></param>
        /// <returns></returns>
        private FileInfo GetDataFile(DataLoadParameters dataLoadParameters)
        {
            FileInfo file = null;

            DirectoryInfo directory = new DirectoryInfo(dataLoadParameters.Directory);
            FileInfo[] files = directory.GetFiles();

            if (files != null) 
            {
                // Only one file in the processing directory
                if (files.Length == 1)
                {
                    file = files[0];
                }
                else
                {
                    DateTime lastUpdated = DateTime.MinValue;

                    // Identify the latest file
                    foreach (FileInfo f in files)
                    {
                        if (f.LastWriteTimeUtc > lastUpdated)
                        {
                            lastUpdated = f.LastWriteTimeUtc;
                            file = f;
                        }
                    }
                }
            }

            return file;
        }

        /// <summary>
        /// Cleans up the directory for the data name
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="dataLoadParameters"></param>
        private void CleanUp(string dataName, DataLoadParameters dataLoadParameters)
        {
            if (dataLoadParameters.DirectoryClean)
            {
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error,
                                string.Format("Cleaning up directory [{0}]", dataLoadParameters.Directory));
                Logger.Write(oe);

                // Delete the transfered/loaded files from the directory
                DirectoryInfo directory = new DirectoryInfo(dataLoadParameters.Directory);
                FileInfo[] files = directory.GetFiles();

                if (files != null) 
                {
                    foreach (FileInfo f in files)
                    {
                        try
                        {
                            oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                                string.Format("Deleting file [{0}]", f.FullName));
                            Logger.Write(oe);

                            f.Delete();
                        }
                        catch (Exception ex)
                        {
                            oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, 
                                string.Format("Error deleting file [{0}] for {1}. Message: {2}",
                                    f.FullName, dataName, ex.Message));
                            Logger.Write(oe);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
