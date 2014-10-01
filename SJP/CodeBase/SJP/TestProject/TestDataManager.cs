// *********************************************** 
// NAME             : TestDataManager.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 15 Feb 2011
// DESCRIPTION  	: Class to help out with running custom sql scripts during unit test runs
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.DatabaseInfrastructure;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Data.SqlClient;

namespace SJP.TestProject
{
    /// <summary>
    /// Class to help running custom sql scripts during unit test runs
    /// </summary>
    public class TestDataManager
    {
        #region Private Fields

        private string dataFile;
        private string clearDownScript;
        private string setupScript;
        private string conString;
        private const string INSERT_COMMAND = "INSERT INTO {0} VALUES (";
        private const string DELETE_COMMAND = "DELETE FROM {0}";

        private SqlHelperDatabase database = SqlHelperDatabase.TransientPortalDB;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sourceData">Script to run containing the data</param>
        /// <param name="clearDownScript">Script to run after unit test run for clean up</param>
        /// <param name="connectionString">Sql connection string</param>
        public TestDataManager(string sourceData, string clearDownScript, string connectionString)
        {
            if (File.Exists(sourceData))
            {
                dataFile = sourceData;
            }
            else
            {
                throw new FileNotFoundException("Data file '" + DataFile + "' could not be found");
            }
            if (File.Exists(clearDownScript))
            {
                this.clearDownScript = clearDownScript;
            }
            else
            {
                throw new FileNotFoundException("Cleardown script '" + clearDownScript + "' could not be found");
            }
            conString = connectionString;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="sourceData"></param>
        /// <param name="clearDownScript"></param>
        /// <param name="connectionString"></param>
        public TestDataManager(string sourceData, string setupScript, string clearDownScript, string connectionString, SqlHelperDatabase database)
        {
            if (!string.IsNullOrEmpty(sourceData))
            {
                if (File.Exists(sourceData))
                {
                    dataFile = sourceData;
                }
                else
                {
                    throw new FileNotFoundException("Data file '" + DataFile + "' could not be found");
                }
            }
            if (!string.IsNullOrEmpty(setupScript))
            {
                if (File.Exists(setupScript))
                {
                    this.setupScript = setupScript;
                }
                else
                {
                    throw new FileNotFoundException("Setup script '" + setupScript + "' could not be found");
                }
            }
            if (!string.IsNullOrEmpty(clearDownScript))
            {
                if (File.Exists(clearDownScript))
                {
                    this.clearDownScript = clearDownScript;
                }
                else
                {
                    throw new FileNotFoundException("Cleardown script '" + clearDownScript + "' could not be found");
                }
            }
            conString = connectionString;
            this.database = database;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the file containing the test data
        /// </summary>
        public string DataFile
        {
            get { return dataFile; }
            set
            {
                if (File.Exists(value))
                {
                    dataFile = value;
                }
                else
                {
                    throw new FileNotFoundException("Data File '" + value + "' could not be found");
                }
            }
        }

        /// <summary>
        /// Gets or sets the file to be run (optionally) for setting up the database before Loading the data
        /// </summary>
        public string SetupScript
        {
            get { return setupScript; }
            set
            {
                if (File.Exists(value))
                {
                    setupScript = value;
                }
                else
                {
                    throw new FileNotFoundException("Setup script '" + value + "' could not be found");
                }
            }
        }

        /// <summary>
        /// Gets or sets the data file containing the cleardown data
        /// </summary>
        public string ClearDownScript
        {
            get { return clearDownScript; }
            set
            {
                if (File.Exists(value))
                {
                    clearDownScript = value;
                }
                else
                {
                    throw new FileNotFoundException("Cleardown script '" + value + "' could not be found");
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to run the (optional) setup script before data is loaded
        /// </summary>
        /// <returns></returns>
        public bool Setup()
        {
            try
            {
                if (!string.IsNullOrEmpty(setupScript))
                {
                    RunScript(setupScript);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Loads test data into the target database ready for test, will first run the cleardown script
        /// </summary>
        /// <returns>True if the operation completed successfully, false otherwise</returns>
        public bool LoadData()
        {
            return LoadData(true);
        }

        /// <summary>
        /// Loads test data into the target database ready for test, first running the cleardown script if specified
        /// </summary>
        /// <returns>True if the operation completed successfully, false otherwise</returns>
        public bool LoadData(bool runClearDownScript)
        {
            if (runClearDownScript)
            {
                if (!string.IsNullOrEmpty(clearDownScript))
                {
                    RunScript(clearDownScript);
                }
            }

            XmlDocument xmlData = new XmlDocument();
            try
            {
                xmlData.Load(dataFile);
            }
            catch (XmlException e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }

            XmlNodeList tables;
            tables = xmlData.DocumentElement.ChildNodes;
            List<string> parms = new List<string>();
            foreach (XmlNode table in tables)
            {
                XmlNodeList records = table.ChildNodes;
                foreach (XmlNode record in records)
                {
                    parms.Add(table.Name);
                    XmlAttributeCollection data = record.Attributes;
                    foreach (XmlAttribute field in data)
                    {
                        parms.Add(field.Value);
                    }
                    string command = FormatInsertCommand(parms.ToArray());
                    //EXECUTE THE COMMAND
                    ExecuteCommand(command);
                    parms.Clear();
                }
            }
            return true;
        }

        /// <summary>
        /// Executes update command
        /// </summary>
        /// <param name="Command">Sql command to run</param>
        /// <returns></returns>
        public bool ExecuteUpdateCommand(string Command)
        {
            try
            {
                ExecuteCommand(Command);
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Runs custom sql scripts
        /// </summary>
        /// <param name="script"></param>
        private void RunScript(string script)
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
               connection.Open();
               using (StreamReader reader = new StreamReader(script))
               {
                   using (SqlCommand command = new SqlCommand(reader.ReadToEnd()))
                   {
                       command.Connection = connection;
                       command.ExecuteNonQuery();
                   }
                  
               }
            }
        }

        /// <summary>
        /// Executes the clear down script to remove the test data from the database
        /// </summary>
        public void ClearData()
        {
            Debug.WriteLine("Resetting Database");
            if (!string.IsNullOrEmpty(clearDownScript))
            {
                RunScript(clearDownScript);
            }
        }

        /// <summary>
        /// formats insert commands
        /// </summary>
        /// <param name="parms">Array of sql parameters to pass</param>
        /// <returns></returns>
        private string FormatInsertCommand(string[] parms)
        {
            StringBuilder command = new StringBuilder(INSERT_COMMAND);
            for (int i = 1; i < parms.Length; i++)
            {
                try
                {
                    int.Parse(parms[i]);
                }
                catch
                {
                    if (parms[i] != "null")
                    {
                        try
                        {
                            DateTime date = DateTime.Parse(parms[i]);
                            parms[i] = "CONVERT(datetime, '" + parms[i] + "', 121)";
                        }
                        catch
                        {
                            parms[i] = "'" + parms[i] + "'";
                        }
                    }
                }
                AddParameter(command, i);
                if (i != parms.GetUpperBound(0))
                {
                    command.Append(',');
                }
                else
                {
                    command.Append(')');
                }
            }
            return String.Format(command.ToString(), parms);
        }

        /// <summary>
        /// Adds parameters
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        private void AddParameter(StringBuilder s, int i)
        {
            s.Append('{');
            s.Append(i.ToString());
            s.Append('}');
        }

        /// <summary>
        /// Executes sql command
        /// </summary>
        /// <param name="command"></param>
        private void ExecuteCommand(string command)
        {
            Debug.WriteLine("Executing Command: " + command);
            using (SqlHelper helper = new SqlHelper())
            {

                try
                {
                    helper.ConnOpen(database);
                    helper.Execute(command);
                }
                finally
                {
                    helper.ConnClose();
                }
            }
        }

        #endregion
    }
}
