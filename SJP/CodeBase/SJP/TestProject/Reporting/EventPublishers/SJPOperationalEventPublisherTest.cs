// *********************************************** 
// NAME             : SJPOperationalEventPublisherTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: SJPOperationalEventPublisherTest test class
// ************************************************
// 
                
using SJP.Reporting.EventPublishers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.ServiceDiscovery;
using SJP.Common;
using SJP.Reporting.Events;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPOperationalEventPublisherTest and is intended
    ///to contain all SJPOperationalEventPublisherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPOperationalEventPublisherTest
    {
        private static TestDataManager testDataManager;

        private TestContext testContextInstance;

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
        public static void MyClassInitialize(TestContext testContext)
        {
            SJPServiceDiscovery.Init(new TestInitialisation());

            string test_data = string.Empty;
            string setup_script = string.Empty;
            string clearup_script = @"Reporting\SJPCustomEventPublisherCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPReportStaging;Trusted_Connection=true";

            // Test data
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.ReportStagingDB);
            testDataManager.Setup();

        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            testDataManager.ClearData();
        }
        //
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
        ///A test for SJPOperationalEventPublisher Constructor
        ///</summary>
        [TestMethod()]
        public void SJPOperationalEventPublisherConstructorTest()
        {
            string identifier = "OPDB";
            SqlHelperDatabase targetDatabase = SqlHelperDatabase.ReportStagingDB;

            try
            {
                SJPOperationalEventPublisher target = new SJPOperationalEventPublisher(identifier, targetDatabase);

                Assert.IsNotNull(target, "Expected SJPOperationalEventPublisher object to have been created");
                Assert.IsTrue(target.Identifier == identifier, "Expected SJPOperationalEventPublisher identifier to equal the id passed in");
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Exception thrown in SJPOperationalEventPublisher constructor {0}", ex.Message));
            }

            // For code coverage
            string message = string.Format(SJP.Reporting.EventPublishers.Messages.ConstructorFailed, "TEST");

            Assert.IsNotNull(message, "Expected EventPublishers.Message to not be null");
        }

        /// <summary>
        ///A test for WriteEvent
        ///</summary>
        [TestMethod()]
        public void SJPOperationalEventPublisherWriteEventTest()
        {
            string identifier = "OPDB";
            SqlHelperDatabase targetDatabase = SqlHelperDatabase.ReportStagingDB;
            SJPOperationalEventPublisher target = new SJPOperationalEventPublisher(identifier, targetDatabase);

            // Test publishing 

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "TestOperational");
            target.WriteEvent(oe);

            // Extra long field to enforce truncate
            OperationalEvent oe1 = new OperationalEvent(SJPEventCategory.Business, "TestSessionIdTestSessionIdTestSessionIdTestSessionIdTestSessionIdTestSessionIdTestSessionIdTestSessionId", 
                SJPTraceLevel.Error, "TestOperational");
            target.WriteEvent(oe1);
            
            ReceivedOperationalEvent roe = new ReceivedOperationalEvent(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "TestOperational"));
            target.WriteEvent(roe);

            // Test publishing LogEvent
            try
            {
                LogEventTest le = new LogEventTest("TestSessionId");
                target.WriteEvent(le);

                Assert.Fail("Expected SJPOperationalEventPublisher to throw exception for unknown LogEvent type");
            }
            catch (SJPException sjpEx)
            {
                // Expect exception to be thrown
                Assert.IsTrue(sjpEx.Identifier == SJPExceptionIdentifier.RDPUnsupportedSJPOperationalEventPublisherEvent,
                    "Expected SJPOperationalEventPublisher to thrown unknown custom event error with SJPExceptionIdentifier.RDPUnsupportedSJPOperationalEventPublisherEvent");
            }
        }
    }
}
