// *********************************************** 
// NAME             : DataChangeNotificationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for DataChangeNotification
// ************************************************
                
                
using SJP.Common.DatabaseInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Timers;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common;

namespace SJP.TestProject.DatabaseInfrastructure
{
    
    
    /// <summary>
    ///This is a test class for DataChangeNotificationTest and is intended
    ///to contain all DataChangeNotificationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataChangeNotificationTest
    {


        #region Private Fields

        private TestContext testContextInstance;
        private static TestDataManager testDataManager;
                
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


        /// <summary>
        ///A test for PollInterval
        ///</summary>
        [TestMethod()]
        public void PollIntervalTest()
        {
            using (DataChangeNotification target = new DataChangeNotification())
            {
                int actual;
                actual = target.PollInterval;

                // polling interval is in milliseconds so need to multiply property by 1000
                Assert.AreEqual(int.Parse(Properties.Current["DataNotification.PollingInterval.Seconds"]) * 1000, actual);
            }
        }

        /// <summary>
        ///A test for PollInterval
        ///</summary>
        [TestMethod()]
        public void ChangeGroupTest()
        {
            using (DataChangeNotification target = new DataChangeNotification())
            {
                ArrayList actual;
                actual = target.ChangeGroups;

                Assert.AreEqual(4, actual.Count);

                Assert.IsInstanceOfType(actual[0], typeof(DataChangeNotificationGroup));

                Assert.AreEqual(((DataChangeNotificationGroup)actual[0]).GroupId, "Configuration");
            }
        }
               

        /// <summary>
        ///A test for Poll
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.databaseinfrastructure.dll")]
        public void PollTest()
        {
            using (DataChangeNotification_Accessor target = new DataChangeNotification_Accessor())
            {
                object sender = this;
                bool isTestPass = false;

                IncrementChangeNotificationVersionNo("TestTable");


                target.add_Changed(new ChangedEventHandler(delegate(System.Object o, ChangedEventArgs e)
                       {
                           isTestPass = ("Configuration" == e.GroupId);
                           Assert.AreEqual("Configuration", e.GroupId);
                       }));

                target.Poll(sender, null);

                Assert.AreEqual(4, target.ChangeNotificationTables.Count);

                Assert.IsInstanceOfType(target.ChangeNotificationTables[SqlHelperDatabase.DefaultDB], typeof(Hashtable));

                Assert.IsTrue(isTestPass);
            }
        }

        /// <summary>
        ///A test for ParseDatabase
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.databaseinfrastructure.dll")]
        [ExpectedException(typeof(SJPException))]
        public void ParseDatabaseTest()
        {
            using (DataChangeNotification_Accessor target = new DataChangeNotification_Accessor())
            {
                string databaseName = "DefaultDB";
                SqlHelperDatabase expected = SqlHelperDatabase.DefaultDB;
                SqlHelperDatabase actual;
                actual = target.ParseDatabase(databaseName);
                Assert.AreEqual(expected, actual);

                // the database name is null which should raise an ArgumentNullException
                actual = target.ParseDatabase(null);
            }
        }

        /// <summary>
        ///A test for ParseDatabase when the database name is wrong and the ArgumentException gets raised
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.databaseinfrastructure.dll")]
        [ExpectedException(typeof(SJPException))]
        public void ParseDatabaseTestArgumentException()
        {
            using (DataChangeNotification_Accessor target = new DataChangeNotification_Accessor())
            {
                string databaseName = "DefaultDB";
                SqlHelperDatabase expected = SqlHelperDatabase.DefaultDB;
                SqlHelperDatabase actual;
                actual = target.ParseDatabase(databaseName);
                Assert.AreEqual(expected, actual);

                // the database name is wrong which should raise an Argument exception
                actual = target.ParseDatabase("DefaultDB1");
            }
        }

        
        
        /// <summary>
        ///A test for ChangeNotificationData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.databaseinfrastructure.dll")]
        public void ChangeNotificationDataTest()
        {
            using (DataChangeNotification dcn = new DataChangeNotification())
            {

                using (DataChangeNotification_Accessor target = new DataChangeNotification_Accessor(new PrivateObject(dcn)))
                {

                    SqlHelperDatabase db = SqlHelperDatabase.DefaultDB;
                    Hashtable actual;
                    using (SqlHelper sql = new SqlHelper())
                    {
                        sql.ConnOpen(db);

                        actual = target.ChangeNotificationData(sql, db);
                    }

                    Assert.IsTrue(actual.ContainsKey("TestTable"));

                    Assert.AreEqual(actual["TestTable"], TestGetTableChangeNotificationVersionNo("TestTable"));
                }
            }
        }

        #region Private Helper Methods
        /// <summary>
        /// Gets the current change notification version no for a specified table from database
        /// </summary>
        /// <returns></returns>
        private int TestGetTableChangeNotificationVersionNo(string tableName)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                    return (int)helper.GetScalar(string.Format("SELECT version FROM ChangeNotification WHERE [Table] = '{0}'" , tableName));
                }
                finally
                {
                    helper.ConnClose();
                }
            }
        }

        /// <summary>
        /// Increments the current change notification version no for a specified table from database by 1
        /// </summary>
        /// <returns></returns>
        private void IncrementChangeNotificationVersionNo(string tableName)
        {
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                    helper.Execute(string.Format("UPDATE ChangeNotification SET [Version]= [Version]+1 WHERE [Table] = '{0}'", tableName));
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
