using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for OperationalEventFilterTest and is intended
    ///to contain all OperationalEventFilterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OperationalEventFilterTest
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
        ///A test for ShouldLog
        ///</summary>
        [TestMethod()]
        public void ShouldLogTest()
        {
            OperationalEventFilter target = new OperationalEventFilter();

            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "Test Message");

            SJPTraceSwitch_Accessor accessor = new SJPTraceSwitch_Accessor();

            SJPTraceSwitch_Accessor.currentLevel = SJPTraceLevel.Verbose;

            Assert.IsTrue(target.ShouldLog(oe));

            oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning, "Test Message");

            SJPTraceSwitch_Accessor.currentLevel = SJPTraceLevel.Warning;

            Assert.IsTrue(target.ShouldLog(oe));

            oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Test Message");

            SJPTraceSwitch_Accessor.currentLevel = SJPTraceLevel.Error;

            Assert.IsTrue(target.ShouldLog(oe));

            oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, "Test Message");

            SJPTraceSwitch_Accessor.currentLevel = SJPTraceLevel.Info;

            Assert.IsTrue(target.ShouldLog(oe));

            oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, "Test Message");

            SJPTraceSwitch_Accessor.currentLevel = SJPTraceLevel.Off;

            Assert.IsFalse(target.ShouldLog(oe));

            CustomEventOne cEvent = new CustomEventOne(SJPEventCategory.Infrastructure, SJPTraceLevel.Info, "Test Message", "user", 21213232);

            Assert.IsFalse(target.ShouldLog(cEvent));
        }
    }
}
