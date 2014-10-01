using SJP.Reporting.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.EventLogging;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPCustomEventTest and is intended
    ///to contain all SJPCustomEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPCustomEventTest
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
        ///A test for SJPCustomEvent Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.events.dll")]
        public void SJPCustomEventConstructorTest()
        {
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            SJPCustomEvent_Accessor target = new SJPCustomEvent_Accessor(sessionId, userLoggedOn);

            Assert.IsTrue(sessionId == target.SessionId);
            Assert.IsTrue(userLoggedOn == target.UserLoggedOn);
        }

        /// <summary>
        ///A test for ConsoleFormatter
        ///</summary>
        [TestMethod()]
        public void SJPCustomEventConsoleFormatterTest()
        {
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            SJPCustomEvent_Accessor target = new SJPCustomEvent_Accessor(sessionId, userLoggedOn);

            IEventFormatter actual = target.ConsoleFormatter;

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "message");

            string eventAsString = actual.AsString(oe);

            Assert.IsNotNull(eventAsString);
        }

        /// <summary>
        ///A test for EmailFormatter
        ///</summary>
        [TestMethod()]
        public void SJPCustomEventEmailFormatterTest()
        {
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            SJPCustomEvent_Accessor target = new SJPCustomEvent_Accessor(sessionId, userLoggedOn);

            IEventFormatter actual = target.EmailFormatter;

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "message");

            string eventAsString = actual.AsString(oe);

            Assert.IsNotNull(eventAsString);
        }

        /// <summary>
        ///A test for EventLogFormatter
        ///</summary>
        [TestMethod()]
        public void SJPCustomEventEventLogFormatterTest()
        {
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            SJPCustomEvent_Accessor target = new SJPCustomEvent_Accessor(sessionId, userLoggedOn);

            IEventFormatter actual = target.EventLogFormatter;

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "message");

            string eventAsString = actual.AsString(oe);

            Assert.IsNotNull(eventAsString);
        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void SJPCustomEventFileFormatterTest()
        {
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            SJPCustomEvent_Accessor target = new SJPCustomEvent_Accessor(sessionId, userLoggedOn);

            IEventFormatter actual = target.FileFormatter;

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "message");

            string eventAsString = actual.AsString(oe);

            Assert.IsNotNull(eventAsString);
        }
    }
}
