// *********************************************** 
// NAME             : StopEventRequestEventTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: StopEventRequestEventTest test
// ************************************************
// 
                
using SJP.Reporting.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.EventLogging;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for StopEventRequestEventTest and is intended
    ///to contain all StopEventRequestEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StopEventRequestEventTest
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
        ///A test for StopEventRequestEvent Constructor
        ///</summary>
        [TestMethod()]
        public void StopEventRequestEventConstructorTest()
        {
            string requestId = string.Empty;
            DateTime submitted = DateTime.Now;
            StopEventRequestType requestType = StopEventRequestType.Time;
            bool success = false;
            StopEventRequestEvent target = new StopEventRequestEvent(requestId, submitted, requestType, success);

            Assert.IsTrue(requestId == target.RequestId);
            Assert.IsTrue(requestType == target.RequestType);
            Assert.IsTrue(submitted == target.Submitted);
            Assert.IsTrue(success == target.Success);
        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void StopEventRequestFileFormatterTest()
        {
            string requestId = string.Empty;
            DateTime submitted = DateTime.Now;
            StopEventRequestType requestType = StopEventRequestType.Time;
            bool success = false;
            StopEventRequestEvent target = new StopEventRequestEvent(requestId, submitted, requestType, success);
            
            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }
    }
}
