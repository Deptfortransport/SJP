// *********************************************** 
// NAME             : W3CWebLogDataTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: W3CWebLogData unit tests
// ************************************************
                
                
using SJP.Reporting.WebLogReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SJP.TestProject.WebLogReader
{
    
    
    /// <summary>
    ///This is a test class for W3CWebLogDataTest and is intended
    ///to contain all W3CWebLogDataTest Unit Tests
    ///</summary>
    [TestClass()]
    public class W3CWebLogDataTest
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
        ///A test for W3CWebLogData Constructor
        ///</summary>
        [TestMethod()]
        public void W3CWebLogDataConstructorTest()
        {
            
            string[] webPageFileExtensions = new string[]{"t1", "t2"};
            List<StatusCode> protocolStatusCodes = new List<StatusCode>();
            protocolStatusCodes.Add(new StatusCode(200, 299));
            int minNonWebPageSize = 500;
            string[] clientIPExcludes = new string[]{};
            string queryIgnoreMarker = "test1";
            string noFileExtensionMarker = "testnoextension";
            string userExperienceVisitorUserAgent = "test";
            string cookieSessionIdStartMarker = "start";
            string cookieSessionIdEndMarker = "end";
            WebLogDataSpecification dataSpecification = new WebLogDataSpecification(webPageFileExtensions, protocolStatusCodes, minNonWebPageSize, clientIPExcludes, queryIgnoreMarker, noFileExtensionMarker,
                userExperienceVisitorUserAgent, cookieSessionIdStartMarker, cookieSessionIdEndMarker);
            
            string uriStem = "Test uri stem string.t1"; 
            int bytesSent = 50; 
            int protocolStatus = 200; 
            string clientIP = "10.93.128.23"; 
            string uriQuery = "Test uri query string"; 
            string date = "2011/06/25"; 
            string time = "13:45"; 
            string host = "localhost";
            string userAgent = "test";
            string cookie = "teststartsessionend";
            W3CWebLogData target = new W3CWebLogData(uriStem, bytesSent, protocolStatus, clientIP, uriQuery, date, time, host, userAgent, cookie);

            DateTime dateTimeLogged = new DateTime(2011, 06, 25, 13, 45, 0);
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dateTimeLogged))
            {
                // Web log entry made in BST so add an hour: 
                dateTimeLogged = dateTimeLogged.AddHours(1);
            }
            Assert.AreEqual(dateTimeLogged, target.DateTimeLogged);
            Assert.AreEqual(0, target.PartnerId);
            Assert.IsTrue(target.MeetsSpecification(dataSpecification));

        }

        /// <summary>
        ///A test for CheckBytesSent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void CheckBytesSentTest()
        {

            string uriStem = "Test uri stem string.t1";
            int bytesSent = 50;
            int protocolStatus = 200;
            string clientIP = "10.93.128.23";
            string uriQuery = "Test uri query string";
            string date = "2011/06/25";
            string time = "13:45";
            string host = "localhost";
            string userAgent = "test";
            string cookie = "teststartsessionend";
            W3CWebLogData weblogData = new W3CWebLogData(uriStem, bytesSent, protocolStatus, clientIP, uriQuery, date, time, host, userAgent, cookie);

            W3CWebLogData_Accessor target = new W3CWebLogData_Accessor(new PrivateObject(weblogData)); 
            int minSize = 30; 
            bool expected = true; 
            bool actual;
            actual = target.CheckBytesSent(minSize);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CheckClientIP
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void CheckClientIPTest()
        {
            string uriStem = "Test uri stem string.t1";
            int bytesSent = 50;
            int protocolStatus = 200;
            string clientIP = "10.93.128.23";
            string uriQuery = "Test uri query string";
            string date = "2011/06/25";
            string time = "13:45";
            string host = "localhost";
            string userAgent = "test";
            string cookie = "teststartsessionend";
            W3CWebLogData weblogData = new W3CWebLogData(uriStem, bytesSent, protocolStatus, clientIP, uriQuery, date, time, host, userAgent, cookie);

            W3CWebLogData_Accessor target = new W3CWebLogData_Accessor(new PrivateObject(weblogData)); 

            string[] clientIPExcludes = new string[]{"10.93.128.23"}; 
            bool expected = false; 
            bool actual;
            actual = target.CheckClientIP(clientIPExcludes);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for CheckFileType
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void CheckFileTypeTest()
        {
            string uriStem = "Test uri stem string.t1";
            int bytesSent = 50;
            int protocolStatus = 200;
            string clientIP = "10.93.128.23";
            string uriQuery = "Test uri query string";
            string date = "2011/06/25";
            string time = "13:45";
            string host = "localhost";
            string userAgent = "test";
            string cookie = "teststartsessionend";
            W3CWebLogData weblogData = new W3CWebLogData(uriStem, bytesSent, protocolStatus, clientIP, uriQuery, date, time, host, userAgent, cookie);

            W3CWebLogData_Accessor target = new W3CWebLogData_Accessor(new PrivateObject(weblogData)); 
            string[] fileExtensions = new string[]{"t1","t2"}; 
            string noFileExtensionMarker = "t3"; 
            bool allowNoFileExtension = false; 
            bool expected = true; 
            bool actual;
            actual = target.CheckFileType(fileExtensions, noFileExtensionMarker, allowNoFileExtension);
            Assert.AreEqual(expected, actual);

            // AllowNoFileExtension is true
            allowNoFileExtension = true;
            target.uriStem = "Test uri stem string";
            actual = target.CheckFileType(fileExtensions, noFileExtensionMarker, allowNoFileExtension);
            Assert.AreEqual(expected, actual);

            // Extension matches to the noFileExtensionMarker
            allowNoFileExtension = false;
            expected = false;
            target.uriStem = "Test uri stem string.t3";
            actual = target.CheckFileType(fileExtensions, noFileExtensionMarker, allowNoFileExtension);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CheckProtocolStatus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void CheckProtocolStatusTest()
        {
            string uriStem = "Test uri stem string.t1";
            int bytesSent = 50;
            int protocolStatus = 605;
            string clientIP = "10.93.128.23";
            string uriQuery = "Test uri query string";
            string date = "2011/06/25";
            string time = "13:45";
            string host = "localhost";
            string userAgent = "test";
            string cookie = "teststartsessionend";
            W3CWebLogData weblogData = new W3CWebLogData(uriStem, bytesSent, protocolStatus, clientIP, uriQuery, date, time, host, userAgent, cookie);
            W3CWebLogData_Accessor target = new W3CWebLogData_Accessor(new PrivateObject(weblogData)); 

            List<StatusCode> statusCodes = new List<StatusCode>();

            statusCodes.Add(new StatusCode(600, 699));

            bool expected = true; 
            bool actual;
            actual = target.CheckProtocolStatus(statusCodes);
            Assert.AreEqual(expected, actual);

            // Protocol status not in a range
            target.protocolStatus = 200;
            expected = false;
            actual = target.CheckProtocolStatus(statusCodes);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CheckQueryString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void CheckQueryStringTest()
        {
            string uriStem = "Test uri stem string.t1";
            int bytesSent = 50;
            int protocolStatus = 605;
            string clientIP = "10.93.128.23";
            string uriQuery = "Test uri query string";
            string date = "2011/06/25";
            string time = "13:45";
            string host = "localhost";
            string userAgent = "test";
            string cookie = "teststartsessionend";
            W3CWebLogData weblogData = new W3CWebLogData(uriStem, bytesSent, protocolStatus, clientIP, uriQuery, date, time, host, userAgent, cookie);
            W3CWebLogData_Accessor target = new W3CWebLogData_Accessor(new PrivateObject(weblogData)); 

            string queryIgnoreMarker = "query"; 
            bool expected = false; 
            bool actual;
            actual = target.CheckQueryString(queryIgnoreMarker);
            Assert.AreEqual(expected, actual);
           
        }

       
    }
}
