// *********************************************** 
// NAME             : WebLogReaderInitialisationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: WebLogReaderInitialisation unit tests
// ************************************************
                
                
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.WebLogReader;

namespace SJP.TestProject.WebLogReader
{
    
    
    /// <summary>
    ///This is a test class for WebLogReaderInitialisationTest and is intended
    ///to contain all WebLogReaderInitialisationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WebLogReaderInitialisationTest
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
        ///A test for Populate
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void PopulateTest()
        {
            WebLogReaderInitialisation target = new WebLogReaderInitialisation();

            SJPServiceDiscovery_Accessor.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery_Accessor.current = new SJPServiceDiscovery();

            SJPServiceDiscovery_Accessor accessor = new SJPServiceDiscovery_Accessor(new PrivateObject(SJPServiceDiscovery_Accessor.current));

            accessor.serviceCache = new Dictionary<string, IServiceFactory>();

            using (PropertyServiceFactory ps = new PropertyServiceFactory())
            {
                accessor.SetServiceForTest(ServiceDiscoveryKey.PropertyService, ps);
            }

            Trace.Listeners.Clear();

            Dictionary<string, IServiceFactory> serviceCache = new Dictionary<string,IServiceFactory>();

            target.Populate(serviceCache);
                        
            Assert.IsTrue(serviceCache.ContainsKey(ServiceDiscoveryKey.PropertyService.ToString()));

            Assert.AreEqual(1, Trace.Listeners.Count);

            Assert.IsInstanceOfType(Trace.Listeners[0], typeof(SJPTraceListener));
        }
    }
}
