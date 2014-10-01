// *********************************************** 
// NAME             : StatusCodeTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: StatusCodeTest Unit tests
// ************************************************
                
                
using SJP.Reporting.WebLogReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.WebLogReader
{
    
    
    /// <summary>
    ///This is a test class for StatusCodeTest and is intended
    ///to contain all StatusCodeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StatusCodeTest
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
        ///A test for StatusCode Constructor
        ///</summary>
        [TestMethod()]
        public void StatusCodeConstructorTest()
        {
            int minStatusCode = 200; 
            int maxStatusCode = 300;
            StatusCode target = new StatusCode(minStatusCode, maxStatusCode);
            Assert.AreEqual(minStatusCode, target.MinProtocolStatusCode);
            Assert.AreEqual(maxStatusCode, target.MaxProtocolStatusCode);
        }

       
    }
}
