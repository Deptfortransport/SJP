// *********************************************** 
// NAME             : FileNamePlaceholder      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: DiscriptionPlaceholder
// ************************************************
// 
                
using SJP.Reporting.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.EventLogging;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for DataGatewayEventTest and is intended
    ///to contain all DataGatewayEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataGatewayEventTest
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
        ///A test for DataGatewayEvent Constructor
        ///</summary>
        [TestMethod()]
        public void DataGatewayEventConstructorTest()
        {
            string feedId = string.Empty;
            string sessionId = string.Empty;
            string fileName = string.Empty;
            DateTime timeStarted = DateTime.Now;
            DateTime timeFinished = DateTime.Now;
            bool successFlag = false;
            int errorCode = 0;
            DataGatewayEvent target = new DataGatewayEvent(feedId, sessionId, fileName, timeStarted, timeFinished, successFlag, errorCode);

            Assert.IsTrue(errorCode == target.ErrorCode);
            Assert.IsTrue(feedId == target.FeedId);
            Assert.IsTrue(fileName == target.FileName);
            Assert.IsTrue(successFlag == target.SuccessFlag);
            Assert.IsTrue(timeFinished == target.TimeFinished);
            Assert.IsTrue(timeStarted == target.TimeStarted);

        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void DataGatewayEventFileFormatterTest()
        {
            string feedId = string.Empty;
            string sessionId = string.Empty;
            string fileName = string.Empty;
            DateTime timeStarted = new DateTime();
            DateTime timeFinished = new DateTime();
            bool successFlag = false;
            int errorCode = 0;
            DataGatewayEvent target = new DataGatewayEvent(feedId, sessionId, fileName, timeStarted, timeFinished, successFlag, errorCode);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }
        
    }
}
