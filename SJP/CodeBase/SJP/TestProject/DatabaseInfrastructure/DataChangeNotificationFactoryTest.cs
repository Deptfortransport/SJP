// *********************************************** 
// NAME             : DataChangeNotificationFactoryTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for DataChangeNotificationFactory class
// ************************************************
                
                
using SJP.Common.DatabaseInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.DatabaseInfrastructure
{
    
    
    /// <summary>
    ///This is a test class for DataChangeNotificationFactoryTest and is intended
    ///to contain all DataChangeNotificationFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataChangeNotificationFactoryTest
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
        #endregion


        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            using (DataChangeNotificationFactory target = new DataChangeNotificationFactory())
            {
                object actual;
                actual = target.Get();
                Assert.IsInstanceOfType(actual, typeof(DataChangeNotification));
                
            }
        }

        
    }
}
