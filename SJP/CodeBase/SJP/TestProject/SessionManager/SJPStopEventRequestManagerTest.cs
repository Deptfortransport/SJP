using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPStopEventRequestManagerTest and is intended
    ///to contain all SJPStopEventRequestManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPStopEventRequestManagerTest
    {


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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
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
        ///A test for SJPStopEventRequestManager Constructor
        ///</summary>
        [TestMethod()]
        public void SJPStopEventRequestManagerConstructorTest()
        {
            SJPStopEventRequestManager target = new SJPStopEventRequestManager();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for AddSJPJourneyRequest
        ///</summary>
        [TestMethod()]
        public void AddSJPJourneyRequestTest()
        {
            SJPStopEventRequestManager target = new SJPStopEventRequestManager();
            ISJPJourneyRequest sjpJourneyRequest = new SJPJourneyRequest();
            target.AddSJPJourneyRequest(sjpJourneyRequest);
            Assert.IsTrue(target.SJPJourneyRequests.ContainsValue(sjpJourneyRequest), "Journey request not added");
        }

        /// <summary>
        ///A test for GetSJPJourneyRequest
        ///</summary>
        [TestMethod()]
        public void GetSJPJourneyRequestTest()
        {
            SJPStopEventRequestManager target = new SJPStopEventRequestManager(); 
            string requestHash = "hash3"; 
            ISJPJourneyRequest expected = new SJPJourneyRequest();
            expected.JourneyRequestHash = requestHash; 
            ISJPJourneyRequest actual;
            target.AddSJPJourneyRequest(expected);
            actual = target.GetSJPJourneyRequest(requestHash);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsDirty
        ///</summary>
        [TestMethod()]
        public void IsDirtyTest()
        {
            SJPStopEventRequestManager target = new SJPStopEventRequestManager(); 
            bool expected = true; 
            bool actual;
            target.IsDirty = expected;
            actual = target.IsDirty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SJPJourneyRequests
        ///</summary>
        [TestMethod()]
        public void SJPJourneyRequestsTest()
        {
            SJPStopEventRequestManager target = new SJPStopEventRequestManager(); 
            Dictionary<string, ISJPJourneyRequest> expected = new Dictionary<string,ISJPJourneyRequest>();
            expected.Add("key", new SJPJourneyRequest());
            Dictionary<string, ISJPJourneyRequest> actual;
            target.SJPJourneyRequests = expected;
            actual = target.SJPJourneyRequests;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SJPJourneyRequestsQueue
        ///</summary>
        [TestMethod()]
        public void SJPJourneyRequestsQueueTest()
        {
            SJPStopEventRequestManager target = new SJPStopEventRequestManager(); 
            Queue<string> expected = new Queue<string>();
            expected.Enqueue("item");
            Queue<string> actual;
            target.SJPJourneyRequestsQueue = expected;
            actual = target.SJPJourneyRequestsQueue;
            Assert.AreEqual(expected, actual);
        }
    }
}
