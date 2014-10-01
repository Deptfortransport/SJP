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
    ///This is a test class for RepeatVisitorEventTest and is intended
    ///to contain all RepeatVisitorEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RepeatVisitorEventTest
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
        ///A test for RepeatVisitorEvent Constructor
        ///</summary>
        [TestMethod()]
        public void RepeatVisitorEventConstructorTest()
        {
            RepeatVisitorType repeatVisitorType = RepeatVisitorType.VisitorNew;
            string sessionIdOld = string.Empty; 
            string sessionIdNew = string.Empty;
            DateTime lastVisitedDateTime = DateTime.Now;
            string lastPageVisted = string.Empty; 
            string domain = string.Empty; 
            string userAgent = string.Empty; 
            RepeatVisitorEvent target = new RepeatVisitorEvent(repeatVisitorType, sessionIdOld, sessionIdNew, lastVisitedDateTime, lastPageVisted, domain, userAgent);

            Assert.IsTrue(repeatVisitorType == target.RepeatVisitorType);
            Assert.IsTrue(sessionIdOld == target.SessionIdOld);
            Assert.IsTrue(lastVisitedDateTime == target.LastVisitedDateTime);
            Assert.IsTrue(lastPageVisted == target.LastPageVisted);
            Assert.IsTrue(domain == target.Domain);
            Assert.IsTrue(userAgent == target.UserAgent);
            Assert.IsTrue(0 == target.ThemeId);
        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void RepeatVisitorEventFileFormatterTest()
        {
            RepeatVisitorType repeatVisitorType = RepeatVisitorType.VisitorNew; 
            string sessionIdOld = string.Empty; 
            string sessionIdNew = string.Empty;
            DateTime lastVisitedDateTime = DateTime.Now; 
            string lastPageVisted = string.Empty; 
            string domain = string.Empty; 
            string userAgent = string.Empty; 
            RepeatVisitorEvent target = new RepeatVisitorEvent(repeatVisitorType, sessionIdOld, sessionIdNew, lastVisitedDateTime, lastPageVisted, domain, userAgent);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }
    }
}
