using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for OperationalEventEmailFormatterTest and is intended
    ///to contain all OperationalEventEmailFormatterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OperationalEventEmailFormatterTest
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
            OperationalEventEmailFormatter target = OperationalEventEmailFormatter.Instance;
            OperationalEvent oe = new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Test message",123,"123");
            
            string output =
                    "SJP Operational Event\n\n" +
                    "Time: " + oe.Time + "\n" +
                    "Message: " + oe.Message + "\n" +
                    "Category: " + oe.Category + "\n" +
                    "Level: " + oe.Level + "\n" +
                    "Machine Name: " + oe.MachineName + "\n";
            

            output += "Type Name: ";
            if (oe.TypeName.Length > 0)
            {
                output += (oe.TypeName + "\n");
            }
            else
            {
                output += ("N/A\n");
            }

            output += "Method Name: ";
            if (oe.MethodName.Length > 0)
            {
                output += (oe.MethodName + "\n");
            }
            else
            {
                output += ("N/A\n");
            }

            output += "Assembly Name: ";
            if (oe.AssemblyName.Length > 0)
            {
                output += (oe.AssemblyName + "\n");
            }
            else
            {
                output += ("N/A\n");
            }

            output += "Target: ";
            if (oe.Target != null)
            {
                output += (oe.Target.ToString() + "\n");
            }
            else
            {
                output += ("N/A\n");
            }

            output += "Session: ";
            if (oe.SessionId != OperationalEvent.SessionIdUnassigned)
            {
                output += oe.SessionId;
            }
            else
            {
                output += ("N/A\n");
            }

            string actual;
            actual = target.AsString(oe);
            Assert.AreEqual(output, actual);
        }

        
    }
}
