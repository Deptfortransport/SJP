using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for OperationalEventConsoleFormatterTest and is intended
    ///to contain all OperationalEventConsoleFormatterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OperationalEventConsoleFormatterTest
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
        ///A test for AsString
        ///</summary>
        [TestMethod()]
        public void AsStringTest()
        {
            OperationalEventConsoleFormatter target = OperationalEventConsoleFormatter.Instance;
            OperationalEvent logEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Test message");
            string expected = "SJP-OP" + " " +
                    logEvent.Time + " " +
                    logEvent.Message + " " +
                    logEvent.Category + " " +
                    logEvent.Level + " " +
                    logEvent.MachineName + " " +
                    logEvent.TypeName + " " +
                    logEvent.MethodName + " " +
                    logEvent.AssemblyName;
            string actual;
            actual = target.AsString(logEvent);
            Assert.AreEqual(expected, actual);
        }

        
    }
}
