// *********************************************** 
// NAME             : NPTGDataFactoryTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for NPTGDataFactory
// ************************************************
                
                
using SJP.Common.DataServices.NPTG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;
using SJP.Common.DatabaseInfrastructure;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for NPTGDataFactoryTest and is intended
    ///to contain all NPTGDataFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NPTGDataFactoryTest
    {


        private TestContext testContextInstance;
        private static TestDataManager testDataManager;

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
        [ClassInitialize()]
        public static void TestClassInitialize(TestContext testContext)
        {
            string test_data = @"DataServices\NPTGTestData.xml";
            string setup_script = @"DataServices\DataServicesTestSetup.sql";
            string clearup_script = @"DataServices\DataServicesTestCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPTransientPortal;Trusted_Connection=true";

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.TransientPortalDB);
            testDataManager.Setup();

            testDataManager.LoadData(false);

        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void TestClassCleanup()
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
            NPTGDataFactory target = new NPTGDataFactory(); 
            object actual;
            actual = target.Get();
            Assert.IsInstanceOfType(actual, typeof(NPTGData));
           
        }
    }
}
