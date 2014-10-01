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
    ///This is a test class for LandingPageEntryEventTest and is intended
    ///to contain all LandingPageEntryEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LandingPageEntryEventTest
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
        ///A test for LandingPageEntryEvent Constructor
        ///</summary>
        [TestMethod()]
        public void LandingPageEntryEventConstructorTest()
        {
            string partnerId = string.Empty;
            LandingPageService serviceId = LandingPageService.SJP;
            string sessionID = string.Empty;
            bool userLoggedOn = false;
            LandingPageEntryEvent target = new LandingPageEntryEvent(partnerId, serviceId, sessionID, userLoggedOn);

            Assert.IsTrue(partnerId == target.PartnerID);
            Assert.IsTrue(serviceId == target.ServiceID);
        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void LandingPageEntryEventFileFormatterTest()
        {
            string partnerId = string.Empty;
            LandingPageService serviceId = new LandingPageService();
            string sessionID = string.Empty;
            bool userLoggedOn = false;
            LandingPageEntryEvent target = new LandingPageEntryEvent(partnerId, serviceId, sessionID, userLoggedOn);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }
    }
}
