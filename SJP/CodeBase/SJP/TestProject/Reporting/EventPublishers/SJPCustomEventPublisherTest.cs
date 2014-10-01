// *********************************************** 
// NAME             : SJPCustomEventPublisherTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: SJPCustomEventPublisherTest test class
// ************************************************
// 

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.EventPublishers;
using SJP.Reporting.Events;
using SJP.Common.EventLogging;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPCustomEventPublisherTest and is intended
    ///to contain all SJPCustomEventPublisherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPCustomEventPublisherTest
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
        ///A test for SJPCustomEventPublisher Constructor
        ///</summary>
        [TestMethod()]
        public void SJPCustomEventPublisherConstructorTest()
        {
            string identifier = "SJPDB";
            SqlHelperDatabase targetDatabase = SqlHelperDatabase.ReportStagingDB;

            try
            {
                SJPCustomEventPublisher target = new SJPCustomEventPublisher(identifier, targetDatabase);

                Assert.IsNotNull(target, "Expected SJPCustomEventPublisher object to have been created");
                Assert.IsTrue(target.Identifier == identifier, "Expected SJPCustomEventPublisher identifier to equal the id passed in");
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Exception thrown in SJPCustomEventPublisher constructor {0}", ex.Message));
            }

            // For code coveragae, test for Default database
            targetDatabase = SqlHelperDatabase.DefaultDB;

            try
            {
                SJPCustomEventPublisher target = new SJPCustomEventPublisher(identifier, targetDatabase);

                Assert.IsNotNull(target, "Expected SJPCustomEventPublisher object to have been created");
                Assert.IsTrue(target.Identifier == identifier, "Expected SJPCustomEventPublisher identifier to equal the id passed in");
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Exception thrown in SJPCustomEventPublisher constructor {0}", ex.Message));
            }
        }

        /// <summary>
        ///A test for WriteEvent
        ///</summary>
        [TestMethod()]
        public void SJPCustomEventPublisherWriteEventTest()
        {
            string identifier = "SJPDB";
            SqlHelperDatabase targetDatabase = SqlHelperDatabase.ReportStagingDB;
            SJPCustomEventPublisher target = new SJPCustomEventPublisher(identifier, targetDatabase);

            // Test publishing 
            
            CyclePlannerRequestEvent cprqe = new CyclePlannerRequestEvent("TestRequestId", false, "TestSession");
            target.WriteEvent(cprqe);

            CyclePlannerResultEvent cprse = new CyclePlannerResultEvent("TestRequestId", JourneyPlanResponseCategory.Results, false, "TestSession");
            target.WriteEvent(cprse);

            DataGatewayEvent dge = new DataGatewayEvent("TestFeed", "TestSession", "TestFile", DateTime.Now, DateTime.Now, true, 0);
            target.WriteEvent(dge);

            JourneyPlanRequestEvent jprqe = new JourneyPlanRequestEvent("TestRequestId", new System.Collections.Generic.List<SJPModeType>(1) { SJPModeType.Rail }, false, "TestSession");
            target.WriteEvent(jprqe);

            JourneyPlanResultsEvent jprse = new JourneyPlanResultsEvent("TestRequestId", JourneyPlanResponseCategory.Results, false, "TestSession");
            target.WriteEvent(jprse);

            LandingPageEntryEvent lpee = new LandingPageEntryEvent("TestPartner", LandingPageService.SJP, "TestSession", false);
            target.WriteEvent(lpee);

            PageEntryEvent pee = new PageEntryEvent(PageId.Empty, "TestSession", false);
            target.WriteEvent(pee);
            
            ReferenceTransactionEvent rte = new ReferenceTransactionEvent("TestCategory", false, DateTime.Now, "TestSession", true, "TestMachine");
            target.WriteEvent(rte);

            RepeatVisitorEvent rve = new RepeatVisitorEvent(RepeatVisitorType.VisitorNew, "TestSessionIdOld", "TestSessionIdNew", DateTime.Now.Subtract(new TimeSpan(1, 0, 0)), "JourneyPlannerInput", "localhost", "Firefox");
            target.WriteEvent(rve);

            RetailerHandoffEvent rhe = new RetailerHandoffEvent("TestRetailer", "TestSession", false);
            target.WriteEvent(rhe);

            StopEventRequestEvent sere = new StopEventRequestEvent("TestRequestId", DateTime.Now, StopEventRequestType.Time, true);
            target.WriteEvent(sere);

            NoResultsEvent nre = new NoResultsEvent(DateTime.Now, "TestSession",false);
            target.WriteEvent(nre);
                     
            WorkloadEvent wle = new WorkloadEvent(DateTime.Now, 1, -999);
            target.WriteEvent(wle);

            // Test publishing LogEvent
            try
            {
                LogEventTest le = new LogEventTest("TestSessionId");
                target.WriteEvent(le);

                Assert.Fail("Expected SJPCustomEventPublisher to throw exception for unknown LogEvent type");
            }
            catch (SJPException sjpEx)
            {
                // Expect exception to be thrown
                Assert.IsTrue(sjpEx.Identifier == SJPExceptionIdentifier.RDPUnsupportedSJPCustomEventPublisherEvent,
                    "Expected SJPCustomEventPublisher to thrown unknown custom event error with SJPExceptionIdentifier.RDPUnsupportedSJPCustomEventPublisherEvent");
            }
        }

    }

    #region Protected LogEventTest class

    /// <summary>
    /// LogEventTest class
    /// </summary>
    [Serializable()]
    public class LogEventTest : SJPCustomEvent
    {
        public LogEventTest(string sessionIdNew)
            : base(sessionIdNew, false)
        {
        }
    }

    #endregion
}
