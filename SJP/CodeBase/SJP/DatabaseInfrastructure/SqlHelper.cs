// *********************************************** 
// NAME             : SqlHelper.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: Helper class to provide methods to interact with databases
// ************************************************
// 


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using SJP.Common.PropertyManager;

namespace SJP.Common.DatabaseInfrastructure
{
    /// <summary>
    /// Helper class to provide methods to interact with databases
    /// </summary>
    public sealed class SqlHelper : IDisposable
    {


        /// <summary>
        /// Used internally to maintain a connection. It needs to be opened
        /// explicitly and closed explicitly.
        /// </summary>
        private SqlConnection sqlConn = new SqlConnection();

        private static int refNumberCur, refNumberMax;
        private static string qryUpdate = "UPDATE ReferenceNum SET RefID = RefID + 100";
        private static string qrySelect = "SELECT MAX(RefID) AS RefID FROM ReferenceNum";

        /// <summary>
        /// Lock to allow thread safe interaction.
        /// </summary>
        private static readonly object lockObj = new object();

        #region Constructor and Destroy

        /// <summary>
        /// Constructor, does nothing yet.
        /// </summary>
        public SqlHelper()
        {
        }

        /// <summary>
        /// Ensures that the connection is closed.
        /// </summary>
        public void Dispose()
        {
            if (sqlConn != null)
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                sqlConn.Dispose();
                sqlConn = null;
            }
        }

        #endregion

        #region Reference Numbers

        /// <summary>
        /// Obtains the next number in sequence, updates the
        /// database if necessary.
        /// </summary>
        /// <returns></returns>
        public static int GetRefNumInt()
        {
            lock (lockObj)  // Lock SqlHelper Type at this stage.
            {
                // Read next batch if required...
                if ((refNumberCur == 0) || (refNumberCur == refNumberMax))
                {
                    using (SqlHelper sql = new SqlHelper())
                    {
                        SqlTransaction tx = null;

                        refNumberMax = refNumberCur = -2; // For completeness in case of exceptions.

                        try
                        {
                            sql.ConnOpen(SqlHelperDatabase.DefaultDB);
                            tx = sql.sqlConn.BeginTransaction(IsolationLevel.ReadCommitted);
                            sql.Execute(qryUpdate, tx); // Update and lock then read the new value.
                            refNumberMax = (int)sql.GetScalar(qrySelect, tx);
                            refNumberCur = refNumberMax - 100;
                            tx.Commit();
                            tx = null;
                        }
                        finally
                        {
                            if (tx != null)
                                tx.Rollback();
                            sql.ConnClose();
                        }
                    }
                } // if allocating a new seuquence block

                return ++refNumberCur;
            } // lock(typeof(SqlHelper))
        }

        /// <summary>
        /// Gets formatted reference number string
        /// </summary>
        /// <returns>Reference number as formatted string</returns>
        public static string GetRefNumStr()
        {
            return FormatRef(GetRefNumInt());
        }

        /// <summary>
        /// Converts the number, int, to the required string format.
        /// </summary>
        /// <param name="numberToFormat"></param>
        /// <returns></returns>
        public static string FormatRef(int numberToFormat)
        {
            string nullFormat = "0000-0000-0000-0000", refNumber, refLeft;
            NumberFormatInfo nfi = new NumberFormatInfo();

            nfi.NumberGroupSeparator = "-";
            nfi.NumberGroupSizes = new int[] { 4 };
            nfi.NumberDecimalDigits = 0;

            refNumber = numberToFormat.ToString("n", nfi);
            refLeft = nullFormat.Substring(0, nullFormat.Length - refNumber.Length);
            return refLeft + refNumber;
        }

        /// <summary>
        /// Used internally by GetReference methods. This function was required
        /// to support database transactions..
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        private Object GetScalar(string sqlQuery, SqlTransaction tx)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
            {
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Transaction = tx;
                return sqlCmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Used internally by GetReference methods. This function was required
        /// to support database transactions..
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        private int Execute(string sqlQuery, SqlTransaction tx)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
            {
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Transaction = tx;
                return sqlCmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// Opens a connection to the database.
        /// </summary>
        public void ConnOpen(SqlHelperDatabase dbToOpen)
        {
            // string strPropName = DBPropertyServiceNames[(int) dbToOpen];
            string strPropName = dbToOpen.ToString();
            IPropertyProvider currProps = Properties.Current;
            string connectionString = currProps[strPropName];
            ConnOpen(connectionString);
        }

        /// <summary>
        /// Opens a connection to the database using supplied connection string
        /// </summary>
        /// <param name="connectionString">Connection string to connect database</param>
        internal void ConnOpen(string connectionString)
        {
            sqlConn.ConnectionString = connectionString;
            sqlConn.Open();
        }

        /// <summary>
        /// Opens connection and returns the transaction object back to the caller.
        /// </summary>
        /// <param name="dbToOpen">Database to Open</param>
        /// <returns></returns>
        public SqlTransaction GetTransaction(SqlHelperDatabase dbToOpen)
        {
            ConnOpen(dbToOpen);
            return sqlConn.BeginTransaction();
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        public void ConnClose()
        {
            sqlConn.Close();
        }

        /// <summary>
        /// Read only property determines if the sql database connection is open or close
        /// </summary>
        public bool ConnIsOpen
        {
            get { return sqlConn.State != ConnectionState.Closed; }
        }

        #endregion

        #region Query Methods

        /// <summary>
        /// Executes an SQL statement.
        /// You must close the Reader when you have finished with it.
        /// </summary>
        /// <param name="sqlQuery">The query to execute.</param>
        /// <returns>SqlDataReader, could have zero rows in it.</returns>
        public SqlDataReader GetReader(string sqlQuery)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
            {
                sqlCmd.CommandType = CommandType.Text;
                return sqlCmd.ExecuteReader(CommandBehavior.SingleResult);
            }
        }

        /// <summary>
        /// Executes a stored procedure that returns rows.
        /// You must close the Reader when you have finished with it.
        /// </summary>
        /// <param name="storedProcName">The name of the store procedure to execute.</param>
        /// <param name="sqlParameters">Associative sql parameters.</param>
        /// <returns>SqlDataReader, could have zero rows in it.</returns>
        public SqlDataReader GetReader(string storedProcName, List<SqlParameter> sqlParameters)
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedProcName, sqlConn))
            {
               sqlCmd.CommandType = CommandType.StoredProcedure;

               if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCmd.Parameters.AddRange(sqlParameters.ToArray());
                

                return sqlCmd.ExecuteReader();
            }
        }
               

        /// <summary>
        /// Executes a stored procedure and uses a data adapter to return a dataset.
        /// </summary>
        /// <param name="storedProcName">The name of the store procedure to execute.</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string storedProcName)
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedProcName, sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                
                //create a new dataset
                using (DataSet dataSet = new DataSet())
                {

                    //Create an SqlDataAdapter using the command object
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        //fill the dataset
                        dataAdapter.Fill(dataSet, storedProcName);
                    }

                    //Return the dataset
                    return dataSet;
                }
            }
        }


        /// <summary>
        /// Executes a stored procedure and uses a data adapter to return a dataset.
        /// </summary>
        /// <param name="storedProcName">The name of the store procedure to execute.</param>
        /// <param name="sqlParameters">Associative sql parameters.</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string storedProcName, List<SqlParameter> sqlParameters)
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedProcName, sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCmd.Parameters.AddRange(sqlParameters.ToArray());
                
                //create a new dataset
                using (DataSet dataSet = new DataSet())
                {

                    //Create an SqlDataAdapter using the command object
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        //fill the dataset
                        dataAdapter.Fill(dataSet, storedProcName);
                    }

                    //Return the dataset
                    return dataSet;
                }
            }
        }
                

        /// <summary>
        /// Executes an SQL statement that returns a scalar, eg COUNT(*)
        /// </summary>
        /// <param name="sqlQuery">The query to execute.</param>
        /// <returns>Object that will need to be cast to appropriate type.</returns>
        public Object GetScalar(string sqlQuery)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
            {
                sqlCmd.CommandType = CommandType.Text;
                return sqlCmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes an stored procedure that returns a scalar, eg COUNT(*)
        /// </summary>
        /// <param name="storedProcName">Stored procedure name.</param>
        /// <param name="sqlParameters">Associative sql parameters.</param>
        /// <returns>Object that will need to be cast to appropriate type.</returns>
        public Object GetScalar(string storedProcName, List<SqlParameter> sqlParameters)
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedProcName, sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCmd.Parameters.AddRange(sqlParameters.ToArray());

                return sqlCmd.ExecuteScalar();
            }
        }
        

        /// <summary>
        /// Executes an update query.
        /// </summary>
        /// <param name="sqlQuery">A query that updates the database. Usually INSERT or UPDATE statement.</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(string sqlQuery)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
            {
                sqlCmd.CommandType = CommandType.Text;
                return sqlCmd.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="storedProcName">Stored procedure name.</param>
        /// <param name="sqlParameters">Associative sql parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(string storedProcName, List<SqlParameter> sqlParameters)
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedProcName, sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCmd.Parameters.AddRange(sqlParameters.ToArray());

                return sqlCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a stored procedure with given types.
        /// </summary>
        /// <param name="storedProcName">Stored procedure name.</param>
        /// <param name="sqlParameters">Associative sqly parameters.</param>
        /// <param name="commandTimeOut">Command time out value</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(string storedProcName, List<SqlParameter> sqlParameters, int commandTimeOut)
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedProcName, sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.CommandTimeout = commandTimeOut;

                if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCmd.Parameters.AddRange(sqlParameters.ToArray());

                return sqlCmd.ExecuteNonQuery();
            }
        }

        
        /// <summary>
        /// Executes SQL Command
        /// </summary>
        /// <param name="sqlCmd">SQL Command object to execute.</param>
        /// <param name="sqlParameters">Associative sql parameters.</param>
        /// <returns>The number of rows affected.</returns>
        private int ExecuteCmd(SqlCommand sqlCmd, List<SqlParameter> sqlParameters)
        {

            using (sqlCmd)
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCmd.Parameters.AddRange(sqlParameters.ToArray());

                return sqlCmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a stored procedure with given types and within the given transaction.
        /// </summary>
        /// <param name="storedProcName">Stored procedure name.</param>
        /// <param name="sqlParameters">Associative sql parameters.</param>
        /// <param name="sqlTransaction">Transaction object</param>
        /// <returns>The number of rows affected/status.</returns>
        public int Execute(string storedProcName, List<SqlParameter> sqlParameters, SqlTransaction sqlTransaction)
        {
            using (SqlCommand sqlCmd = new SqlCommand(storedProcName, sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null && sqlParameters.Count > 0)
                    sqlCmd.Parameters.AddRange(sqlParameters.ToArray());

                sqlCmd.Transaction = sqlTransaction;

                return sqlCmd.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
