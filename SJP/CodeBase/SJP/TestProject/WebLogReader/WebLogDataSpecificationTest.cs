// *********************************************** 
// NAME             : WebLogDataSpecificationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: WebLogDataSpecification unit tests
// ************************************************
                
                
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Reporting.WebLogReader;

namespace SJP.TestProject.WebLogReader
{
    
    
    /// <summary>
    ///This is a test class for WebLogDataSpecificationTest and is intended
    ///to contain all WebLogDataSpecificationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WebLogDataSpecificationTest
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
        ///A test for WebLogDataSpecification Constructor
        ///</summary>
        [TestMethod()]
        public void WebLogDataSpecificationConstructorTest()
        {
            string[] webPageFileExtensions = new string[]{"t1", "t2"};
            List<StatusCode> protocolStatusCodes = new List<StatusCode>();
            protocolStatusCodes.Add(new StatusCode(200, 299));
            int minNonWebPageSize = 500;
            string[] clientIPExcludes = new string[]{};
            string queryIgnoreMarker = "test";
            string noFileExtensionMarker = "testExtentionMarker";
            string userExperienceVisitorUserAgent = "test";
            string cookieSessionIdStartMarker = "test";
            string cookieSessionIdEndMarker = "test";
            WebLogDataSpecification target = new WebLogDataSpecification(webPageFileExtensions, protocolStatusCodes, minNonWebPageSize, clientIPExcludes, queryIgnoreMarker, noFileExtensionMarker,
                userExperienceVisitorUserAgent, cookieSessionIdStartMarker, cookieSessionIdEndMarker);
            
            Assert.AreEqual(webPageFileExtensions.Length, target.WebPageFileExtensions.Length);
            Assert.IsTrue(target.WebPageFileExtensions.Contains("t1"));
            Assert.IsTrue(target.WebPageFileExtensions.Contains("t2"));
            Assert.IsTrue(target.ProtocolStatusCode.Count > 0);
            Assert.IsNotNull(target.ProtocolStatusCode[0]);
            Assert.AreEqual(minNonWebPageSize, target.MinNonWebPageSize);
            Assert.IsTrue(target.ClientIPExcludes.Length == 0);
            Assert.AreEqual(queryIgnoreMarker, target.QueryIgnoreMarker);
            Assert.AreEqual(noFileExtensionMarker, target.NoFileExtensionMarker);

        }

        
    }
}
