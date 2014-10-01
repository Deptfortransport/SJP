// *********************************************** 
// NAME             : SqlHelperTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 15 Feb 2011
// DESCRIPTION  	: Unit test class for SqlHelper
// ************************************************
// 
                
                
using SJP.Common.DatabaseInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using SJP.Common.ServiceDiscovery;
using SJP.Common.PropertyManager;
using System.Configuration;
using System.IO;

namespace SJP.TestProject.DatabaseInfrastructure
{
    
    
    /// <summary>
    ///This is a test class for SqlHelperTest and is intended
    ///to contain all SqlHelperTest Unit Tests
    ///</summary>
    ///<remarks>
    /// The test requires SJPConfiguration database to be built and deployed with properties table and related stored procedures
    /// </remarks>
    [TestClass()]
    public class SqlHelperTest
    {
        #region Private Fields

        private TestContext testContextInstance;
        private static TestDataManager testDataManager;

        /// <summary>
        /// Used to test RefNumber functions of SqlHelper.
        /// </summary>
        private static string qryRefSelect = "SELECT MAX(RefID) AS MaxRefID FROM ReferenceNum";
               
      
        #endregion


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [TestInitialize()]
        public void TestInitialize()
        {
            string test_data = @"DatabaseInfrastructure\SqlHelperTestData.xml";
            string setup_script = @"DatabaseInfrastructure\SqlHelperTestSetup.sql";
            string clearup_script = @"DatabaseInfrastructure\SqlHelperTestCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPConfiguration;Trusted_Connection=true";

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

            SJPServiceDiscovery.Init(new TestInitialisation());
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.DefaultDB);
            testDataManager.Setup();
 
        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        [TestCleanup()]
        public void TestCleanup()
        {
            testDataManager.ClearData();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
        
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region Test Methods
        /// <summary>
        ///A test for ConnClose
        ///</summary>
        [TestMethod()]
        public void ConnCloseTest()
        {
            using (SqlHelper target = new SqlHelper())
            {
                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                Assert.IsTrue(target.ConnIsOpen);
                target.ConnClose();
                Assert.IsFalse(target.ConnIsOpen);
            }
        }

        /// <summary>
        ///A test for ConnOpen
        ///</summary>
        [TestMethod()]
        public void ConnOpenTest()
        {
            using (SqlHelper target = new SqlHelper())
            {
                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                Assert.IsTrue(target.ConnIsOpen);
                target.ConnClose();
            }

        }

        /// <summary>
        ///A test for ConnOpen passing connection string to internal constructor
        ///</summary>
        [TestMethod()]
        public void ConnOpenTest1()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["propertyservice.providers.databaseprovider.connectionstring"].ConnectionString;

            using (SqlHelper_Accessor target = new SqlHelper_Accessor())
            {

                target.ConnOpen(connectionString);

                Assert.IsTrue(target.ConnIsOpen);

                target.ConnClose();
            }
            
            
        }

        /// <summary>
        ///A test for Execute method accepting sql query string
        ///</summary>
        [TestMethod()]
        public void ExecuteTest()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string sqlQuery = "SELECT * FROM TestTable";

                //For UPDATE, INSERT, and DELETE statements, the return value is the number of rows 
                //affected by the command. When a trigger exists on a table being inserted or updated, 
                //the return value includes the number of rows affected by both the insert or update 
                //operation and the number of rows affected by the trigger or triggers. 
                //For all other types of statements, the return value is -1. 
                //If a rollback occurs, the return value is also -1.
                int expected = -1;

                int actual;

                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = target.Execute(sqlQuery);
                target.ConnClose();
                Assert.AreEqual(expected, actual);
            }
            
        }

        /// <summary>
        ///A test for overloaded Execute method accepting stored proc name and parameters to pass.
        ///</summary>
        [TestMethod()]
        public void ExecuteTest1()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string storedProcName = "GetTestData";
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@RefId", 3));

                //For UPDATE, INSERT, and DELETE statements, the return value is the number of rows 
                //affected by the command. When a trigger exists on a table being inserted or updated, 
                //the return value includes the number of rows affected by both the insert or update 
                //operation and the number of rows affected by the trigger or triggers. 
                //For all other types of statements, the return value is -1. 
                //If a rollback occurs, the return value is also -1.
                int expected = -1;
                int actual;
                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = target.Execute(storedProcName, sqlParameters);
                target.ConnClose();
                Assert.AreEqual(expected, actual);
            }
            
        }

        /// <summary>
        ///A test for overloaded execute method with command timeout value passed as parameter
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void ExecuteTestWithCommandTimeOut()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string storedProcName = "LongRunningProc";
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@RefId", 3));

                int commandTimeOut = 15;

                int actual;
                try
                {
                    target.ConnOpen(SqlHelperDatabase.DefaultDB);

                    actual = target.Execute(storedProcName, sqlParameters, commandTimeOut);
                }
                finally
                {
                    target.ConnClose();
                }
            }
            
        }


        /// <summary>
        ///A test for overloaded Execute method accepting stored proc name and parameters to pass.
        ///</summary>
        [TestMethod()]
        public void ExecuteTestTransaction()
        {
            using (SqlHelper target = new SqlHelper())
            {
                
                string storedProcName = "GetTestData";
                
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@RefId", 3));

                //For UPDATE, INSERT, and DELETE statements, the return value is the number of rows 
                //affected by the command. When a trigger exists on a table being inserted or updated, 
                //the return value includes the number of rows affected by both the insert or update 
                //operation and the number of rows affected by the trigger or triggers. 
                //For all other types of statements, the return value is -1. 
                //If a rollback occurs, the return value is also -1.
                int expected = -1;
                int actual;
                using (SqlTransaction transaction = target.GetTransaction(SqlHelperDatabase.DefaultDB))
                {
                    actual = target.Execute(storedProcName, sqlParameters, transaction);
                    transaction.Commit();
                    target.ConnClose();
                }
                Assert.AreEqual(expected, actual);
            }

        }

        /// <summary>
        ///A test for ExecuteCmd method
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.databaseinfrastructure.dll")]
        public void ExecuteCmdTest()
        {
            using (SqlHelper_Accessor target = new SqlHelper_Accessor())
            {
                using (SqlCommand sqlCmd = new SqlCommand())
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "GetALLTestData";
                    sqlCmd.Connection = target.sqlConn;

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    int expected = -1;
                    int actual;
                    target.ConnOpen(SqlHelperDatabase.DefaultDB);

                    actual = target.ExecuteCmd(sqlCmd, sqlParameters);
                    Assert.AreEqual(expected, actual);
                    target.ConnClose();
                }
            }
        }

        /// <summary>
        ///A test for GetDataSet method accepting stored procedure name and parameters
        ///</summary>
        [TestMethod()]
        public void GetDataSetTest()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string storedProcName = "GetTestData";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@RefId", 3));

                DataSet actual;
                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = target.GetDataSet(storedProcName, sqlParameters);
                target.ConnClose();
                Assert.AreEqual(1, actual.Tables.Count);
                Assert.AreEqual(1, actual.Tables[0].Rows.Count);
            }
            
        }

        /// <summary>
        ///A test for overloaded GetDataSet method which just accepts stored procedure name
        ///</summary>
        [TestMethod()]
        public void GetDataSetTest1()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string storedProcName = "GetALLTestData";

                DataSet actual;
                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = target.GetDataSet(storedProcName);
                target.ConnClose();
                Assert.AreEqual(1, actual.Tables.Count);
                Assert.AreEqual(5, actual.Tables[0].Rows.Count);
            }
        }

        /// <summary>
        ///A test for GetReader
        ///</summary>
        [TestMethod()]
        public void GetReaderTest()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string sqlQuery = "SELECT * FROM TestTABLE";

                SqlDataReader actual;

                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = target.GetReader(sqlQuery);
               

                Assert.IsTrue(actual.HasRows);

                int expectedRefid = 0;

                while (actual.Read())
                {
                    Assert.AreEqual(expectedRefid, actual.GetInt32(0));
                    Assert.AreEqual(expectedRefid + 1, actual.GetInt32(1));

                    expectedRefid++;
                } 
                
                target.ConnClose();
            }
            
        }

        /// <summary>
        ///A test for overloaded GetReader method which accepts stored procedure name and sql parameters
        ///</summary>
        [TestMethod()]
        public void GetReaderTest1()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string storedProcName = "GetTestData";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@RefId", 3));

                SqlDataReader actual;
                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = target.GetReader(storedProcName,sqlParameters);
                

                Assert.IsTrue(actual.HasRows);

                int expectedRefValue = 4;

                while (actual.Read())
                {
                    Assert.AreEqual(expectedRefValue, actual.GetInt32(0));

                    expectedRefValue++;
                }
                
                target.ConnClose();

              
            }
        }

        /// <summary>
        ///A test for GetRefNumInt
        ///</summary>
        [TestMethod()]
        public void GetRefNumIntTest()
        {
            int refOld, refNew, refNo1, refNo2;
            
            ResetReferenceNumber();
            refOld = TestGetReferenceGetRegister();
            refNo1 = SqlHelper.GetRefNumInt();
            refNew = TestGetReferenceGetRegister();

            // On first execution, reference register should increament.
            Assert.IsTrue((refNo1 != 0) && (refNo1 != -1));
            if(!(refNew == refOld + 100))
            {
                Assert.Inconclusive("New reference number is not 100 higher than old one. Old = " + refOld + ", new = " + refNew);
            }

            // Reference register should not increament the 2nd time.
            refNo2 = SqlHelper.GetRefNumInt(); // Get 2nd reference.
            Assert.IsTrue(refNew == TestGetReferenceGetRegister());
            Assert.IsTrue(refNo2 == refNo1 + 1);

            // Check string ref num is working
            string refStr = SqlHelper.GetRefNumStr();
            Assert.IsNotNull(refStr);

            // Check number format function is working.
            Assert.IsTrue(SqlHelper.FormatRef(1) == "0000-0000-0000-0001");
            Assert.IsTrue(SqlHelper.FormatRef(12345) == "0000-0000-0001-2345");
            Assert.IsTrue(SqlHelper.FormatRef(123456789) == "0000-0001-2345-6789");

        }

        /// <summary>
        ///A test for GetScalar method which accepts stored procedure name and sql parameter list
        ///</summary>
        [TestMethod()]
        public void GetScalarTest()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string storedProcName = "GetTestData";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@RefId", 3));

                int expected = 4;
                int actual;

                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = (int)target.GetScalar(storedProcName, sqlParameters);
                target.ConnClose();

                Assert.AreEqual(expected, actual);
            }
            
        }

        /// <summary>
        ///A test for overloaded GetScalar method which accepts sql query statement
        ///</summary>
        [TestMethod()]
        public void GetScalarTest1()
        {
            using (SqlHelper target = new SqlHelper())
            {
                string sqlQuery = "SELECT Count(*) FROM TestTable";

                int expected = 5;
                int actual;

                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                actual = (int)target.GetScalar(sqlQuery);
                target.ConnClose();

                Assert.AreEqual(expected, actual);
            }
        }
        #endregion




        /// <summary>
        ///A test for ConnIsOpen
        ///</summary>
        [TestMethod()]
        public void ConnIsOpenTest()
        {
            using (SqlHelper target = new SqlHelper())
            {
                target.ConnOpen(SqlHelperDatabase.DefaultDB);
                Assert.IsTrue(target.ConnIsOpen);
                target.ConnClose();
                Assert.IsFalse(target.ConnIsOpen);
            }
        }


        #region Private Helper Methods
        /// <summary>
        /// Called by TestGetReference() to red register the the database.
        /// </summary>
        /// <returns></returns>
        private int TestGetReferenceGetRegister()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                    return (int)helper.GetScalar(qryRefSelect);
                }
                finally
                {
                    helper.ConnClose();
                }
            }
        }

        /// <summary>
        /// Resets the reference number
        /// </summary>
        private void ResetReferenceNumber()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                    helper.Execute("UPDATE ReferenceNum SET RefID = 0");
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
