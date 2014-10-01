using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPRequestManagerTest and is intended
    ///to contain all SJPRequestManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPRequestManagerTest
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
        ///A test for SJPRequestManager Constructor
        ///</summary>
        [TestMethod()]
        public void SJPRequestManagerConstructorTest()
        {
            SJPRequestManager target = new SJPRequestManager();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for AddSJPJourneyRequest
        ///</summary>
        [TestMethod()]
        public void AddSJPJourneyRequestTest()
        {
            SJPRequestManager target = new SJPRequestManager();
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
            SJPRequestManager target = new SJPRequestManager(); 
            string requestHash = "hash4";
            ISJPJourneyRequest expected = new SJPJourneyRequest();
            expected.JourneyRequestHash = requestHash;
            target.AddSJPJourneyRequest(expected);
            ISJPJourneyRequest actual;
            actual = target.GetSJPJourneyRequest(requestHash);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsDirty
        ///</summary>
        [TestMethod()]
        public void IsDirtyTest()
        {
            SJPRequestManager target = new SJPRequestManager();
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
            SJPRequestManager target = new SJPRequestManager();
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
            SJPRequestManager target = new SJPRequestManager();
            Queue<string> expected = new Queue<string>();
            expected.Enqueue("item");
            Queue<string> actual;
            target.SJPJourneyRequestsQueue = expected;
            actual = target.SJPJourneyRequestsQueue;
            Assert.AreEqual(expected, actual);
        }
    }
}
