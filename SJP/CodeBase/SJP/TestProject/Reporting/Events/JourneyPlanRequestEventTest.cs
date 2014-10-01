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
using System.Collections.Generic;
using SJP.Common.EventLogging;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for JourneyPlanRequestEventTest and is intended
    ///to contain all JourneyPlanRequestEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JourneyPlanRequestEventTest
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
        ///A test for JourneyPlanRequestEvent Constructor
        ///</summary>
        [TestMethod()]
        public void JourneyPlanRequestEventConstructorTest()
        {
            string journeyPlanRequestId = string.Empty;
            List<SJPModeType> modes = new List<SJPModeType>(1) { SJPModeType.Rail };
            bool userLoggedOn = false;
            string sessionId = string.Empty;
            JourneyPlanRequestEvent target = new JourneyPlanRequestEvent(journeyPlanRequestId, modes, userLoggedOn, sessionId);

            Assert.IsTrue(journeyPlanRequestId == target.JourneyPlanRequestId);
            Assert.IsTrue(modes[0] == target.Modes[0]);
            
        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void JourneyPlanRequestEventFileFormatterTest()
        {
            string journeyPlanRequestId = string.Empty;
            List<SJPModeType> modes = new List<SJPModeType>(1) { SJPModeType.Rail };
            bool userLoggedOn = false;
            string sessionId = string.Empty;
            JourneyPlanRequestEvent target = new JourneyPlanRequestEvent(journeyPlanRequestId, modes, userLoggedOn, sessionId);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }

    }
}
