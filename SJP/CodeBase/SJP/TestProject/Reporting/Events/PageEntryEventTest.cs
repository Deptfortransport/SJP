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
using SJP.Common;
using SJP.Common.EventLogging;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for PageEntryEventTest and is intended
    ///to contain all PageEntryEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageEntryEventTest
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
        ///A test for PageEntryEvent Constructor
        ///</summary>
        [TestMethod()]
        public void PageEntryEventConstructorTest()
        {
            PageId page = PageId.Empty;
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            PageEntryEvent target = new PageEntryEvent(page, sessionId, userLoggedOn);

            Assert.IsTrue(page == target.Page);
            Assert.IsTrue(page.ToString() == target.PageName);
            Assert.IsTrue(0 == target.ThemeId);
        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void PageEntryEventFileFormatterTest()
        {
            PageId page = new PageId();
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            PageEntryEvent target = new PageEntryEvent(page, sessionId, userLoggedOn);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }
    }
}
