using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPResultManagerTest and is intended
    ///to contain all SJPResultManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPResultManagerTest
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
        ///A test for SJPResultManager Constructor
        ///</summary>
        [TestMethod()]
        public void SJPResultManagerConstructorTest()
        {
            SJPResultManager target = new SJPResultManager();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for AddSJPJourneyResult
        ///</summary>
        [TestMethod()]
        public void AddSJPJourneyResultTest()
        {
            SJPResultManager target = new SJPResultManager(); 
            ISJPJourneyResult sjpJourneyResult = new SJPJourneyResult();
            string requestHash = "hash5";
            sjpJourneyResult.JourneyRequestHash = requestHash;
            target.AddSJPJourneyResult(sjpJourneyResult);
            Assert.AreEqual(sjpJourneyResult, target.GetSJPJourneyResult(requestHash));
        }

        /// <summary>
        ///A test for DoesResultExist
        ///</summary>
        [TestMethod()]
        public void DoesResultExistTest()
        {
            SJPResultManager target = new SJPResultManager(); 
            string requestHash = "hash6";
            bool expected = false; 
            bool actual;
            actual = target.DoesResultExist(requestHash);
            Assert.AreEqual(expected, actual);

            // Re run test for success response
            ISJPJourneyResult sjpJourneyResult = new SJPJourneyResult();
            sjpJourneyResult.JourneyRequestHash = requestHash;
            target.AddSJPJourneyResult(sjpJourneyResult);
            expected = true;
            actual = target.DoesResultExist(requestHash);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetSJPJourneyResult
        ///</summary>
        [TestMethod()]
        public void GetSJPJourneyResultTest()
        {
            SJPResultManager target = new SJPResultManager(); 
            string requestHash = "hash6";
            ISJPJourneyResult expected = new SJPJourneyResult();
            expected.JourneyRequestHash = requestHash;
            target.AddSJPJourneyResult(expected);
            ISJPJourneyResult actual;
            actual = target.GetSJPJourneyResult(requestHash);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for RemoveSJPJourneyResult
        ///</summary>
        [TestMethod()]
        public void RemoveSJPJourneyResultTest()
        {
            SJPResultManager target = new SJPResultManager(); 
            string requestHash = "hash7";
            SJPJourneyResult journeyResult = new SJPJourneyResult();
            journeyResult.JourneyRequestHash = requestHash;
            target.AddSJPJourneyResult(journeyResult);
            target.RemoveSJPJourneyResult(requestHash);
        }

        /// <summary>
        ///A test for IsDirty
        ///</summary>
        [TestMethod()]
        public void IsDirtyTest()
        {
            SJPResultManager target = new SJPResultManager(); 
            bool expected = true;
            bool actual;
            target.IsDirty = expected;
            actual = target.IsDirty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SJPJourneyResults
        ///</summary>
        [TestMethod()]
        public void SJPJourneyResultsTest()
        {
            SJPResultManager target = new SJPResultManager(); 
            Dictionary<string, ISJPJourneyResult> expected = new Dictionary<string,ISJPJourneyResult>();
            expected.Add("key", new SJPJourneyResult());
            Dictionary<string, ISJPJourneyResult> actual;
            target.SJPJourneyResults = expected;
            actual = target.SJPJourneyResults;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SJPJourneyResultsQueue
        ///</summary>
        [TestMethod()]
        public void SJPJourneyResultsQueueTest()
        {
            SJPResultManager target = new SJPResultManager(); 
            Queue<string> expected = new Queue<string>();
            expected.Enqueue("item");
            Queue<string> actual;
            target.SJPJourneyResultsQueue = expected;
            actual = target.SJPJourneyResultsQueue;
            Assert.AreEqual(expected, actual);
        }
    }
}
