using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for OperationalEventFileFormatterTest and is intended
    ///to contain all OperationalEventFileFormatterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OperationalEventFileFormatterTest
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
            OperationalEventFileFormatter target = OperationalEventFileFormatter.Instance;
            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Test message",123,"123");
            

            string expected = "SJP-OP\t" +
                    oe.Time.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "\t" +
                    oe.Message + "\t" +
                    oe.Category + "\t" +
                    oe.Level + "\t" +
                    oe.MachineName + "\t" +
                    oe.TypeName + "\t" +
                    oe.MethodName + "\t" +
                    oe.AssemblyName + "\t" +
                    oe.Target + "\t" +
                    oe.SessionId;

            string actual;
            actual = target.AsString(oe);
            Assert.AreEqual(expected, actual);
           
        }

       
    }
}
