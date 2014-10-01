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
    ///This is a test class for CyclePlannerResultEventTest and is intended
    ///to contain all CyclePlannerResultEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CyclePlannerResultEventTest
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
        ///A test for CyclePlannerResultEvent Constructor
        ///</summary>
        [TestMethod()]
        public void CyclePlannerResultEventConstructorTest()
        {
            string cyclePlannerRequestId = string.Empty;
            JourneyPlanResponseCategory responseCategory = new JourneyPlanResponseCategory();
            bool userLoggedOn = false;
            string sessionId = string.Empty;
            CyclePlannerResultEvent target = new CyclePlannerResultEvent(cyclePlannerRequestId, responseCategory, userLoggedOn, sessionId);

            Assert.IsTrue(cyclePlannerRequestId == target.CyclePlannerRequestId);
            Assert.IsTrue(responseCategory == target.ResponseCategory);

        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void CyclePlannerResultEventFileFormatterTest()
        {
            string cyclePlannerRequestId = string.Empty;
            JourneyPlanResponseCategory responseCategory = new JourneyPlanResponseCategory();
            bool userLoggedOn = false;
            string sessionId = string.Empty;
            CyclePlannerResultEvent target = new CyclePlannerResultEvent(cyclePlannerRequestId, responseCategory, userLoggedOn, sessionId);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }
    }
}
