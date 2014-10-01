using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for OperationalEventEventLogFormatterTest and is intended
    ///to contain all OperationalEventEventLogFormatterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OperationalEventEventLogFormatterTest
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
            OperationalEventEventLogFormatter target = OperationalEventEventLogFormatter.Instance;
            OperationalEvent operationalEvent = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Test message",123,"123");
            string expected = operationalEvent.Category + Environment.NewLine +
                         "SJP OPERATIONAL EVENT" + Environment.NewLine +
                         "Time: " + operationalEvent.Time.ToString("yyyy-MM-ddTHH:mm:ss.fff") + Environment.NewLine +
                         "Category: " + operationalEvent.Category + Environment.NewLine +
                         "Level: " + operationalEvent.Level + Environment.NewLine +
                         "Message: " + operationalEvent.Message + Environment.NewLine +
                         "Machine: " + operationalEvent.MachineName + Environment.NewLine +
                         "Class logged: " + operationalEvent.TypeName + Environment.NewLine +
                         "Method logged: " + operationalEvent.MethodName + Environment.NewLine +
                         "Assembly logged: " + operationalEvent.AssemblyName + Environment.NewLine +
                         "Target: " + operationalEvent.Target.ToString() + Environment.NewLine +
                         "Session Id: " + operationalEvent.SessionId + Environment.NewLine;

            string actual;
            actual = target.AsString(operationalEvent);
            Assert.AreEqual(expected, actual);
        }

       
    }
}
