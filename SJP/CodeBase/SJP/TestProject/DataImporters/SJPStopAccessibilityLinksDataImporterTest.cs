using SJP.DataImporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using System.Configuration;
using TransportDirect.Common.ServiceDiscovery;
using System.IO;
using TransportDirect.Common;
using System.Diagnostics;

namespace SJP.TestProject.DataImporters
{
    
    
    /// <summary>
    ///This is a test class for SJPStopAccessibilityLinksDataImporterTest and is intended
    ///to contain all SJPStopAccessibilityLinksDataImporterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPStopAccessibilityLinksDataImporterTest
    {
        private TestContext testContextInstance;
        private string propertyservice_providerassembly;
        private string propertyservice_providerclass;
        private string propertyservice_applicationid;
        private string propertyservice_groupid;
        private string propertyservice_providers_databaseprovider_connectionstring;

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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            propertyservice_applicationid = config.AppSettings.Settings["propertyservice.applicationid"].Value;
            propertyservice_groupid = config.AppSettings.Settings["propertyservice.groupid"].Value;
            propertyservice_providerassembly = config.AppSettings.Settings["propertyservice.providerassembly"].Value;
            propertyservice_providerclass = config.AppSettings.Settings["propertyservice.providerclass"].Value;
            propertyservice_providerclass = config.AppSettings.Settings["propertyservice.providerclass"].Value;
            propertyservice_providers_databaseprovider_connectionstring = config.AppSettings.Settings["propertyservice.providers.databaseprovider.connectionstring"].Value;

            config.AppSettings.Settings["propertyservice.providerassembly"].Value = "td.common.propertyservice.databasepropertyprovider";
            config.AppSettings.Settings["propertyservice.providerclass"].Value = "TransportDirect.Common.PropertyService.DatabasePropertyProvider.DatabasePropertyProvider";
            config.AppSettings.Settings["propertyservice.applicationid"].Value = "DataGatewayImport";
            config.AppSettings.Settings["propertyservice.groupid"].Value = "DataGateway";
            config.AppSettings.Settings["propertyservice.providers.databaseprovider.connectionstring"].Value = @"Integrated Security=SSPI;Initial Catalog=PermanentPortal;Data Source=.\SQLExpress;Connect Timeout=30";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            TDServiceDiscovery.ResetServiceDiscoveryForTest();
            TDServiceDiscovery.Init(new TestTDInitialisation());
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["propertyservice.providerassembly"].Value = propertyservice_providerassembly;
            config.AppSettings.Settings["propertyservice.providerclass"].Value = propertyservice_providerclass;
            config.AppSettings.Settings["propertyservice.applicationid"].Value = propertyservice_applicationid;
            config.AppSettings.Settings["propertyservice.groupid"].Value = propertyservice_groupid;
            config.AppSettings.Settings["propertyservice.providers.databaseprovider.connectionstring"].Value = propertyservice_providers_databaseprovider_connectionstring;


            config.Save();

            ConfigurationManager.RefreshSection("appSettings");
            TDServiceDiscovery.ResetServiceDiscoveryForTest();

            // Need to remove TD trace listeners as they will cause errors on other tests
            for (int i = Trace.Listeners.Count - 1; i > -1; i--)
            {
                Trace.Listeners.RemoveAt(i);
            }
        }
        //
        #endregion



        /// <summary>
        ///A test for Run
        ///</summary>
        [TestMethod()]
        public void RunTest()
        {
            string feed = "mnb765";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            DirectoryInfo di = new DirectoryInfo("DataImporters");
            string processingDirectory = di.FullName;  
            SJPStopAccessibilityLinksDataImporter target = new SJPStopAccessibilityLinksDataImporter(feed, params1, params2, utility, processingDirectory);
            string filename = "StopLinks_v0.2a.csv";
            int expected = 0;
            int actual;
            actual = target.Run(filename);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Run when invalid feed specified
        ///</summary>
        [TestMethod()]
        public void RunTestInvalidFeed()
        {
            string feed = "mnb7651";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            DirectoryInfo di = new DirectoryInfo("DataImporters");
            string processingDirectory = di.FullName;
            SJPStopAccessibilityLinksDataImporter target = new SJPStopAccessibilityLinksDataImporter(feed, params1, params2, utility, processingDirectory);
            string filename = "StopLinks_v0.2.csv";
            int actual;
            actual = target.Run(filename);
            Assert.IsTrue(0 != actual);
        }

        /// <summary>
        ///A test for Run when invalid File specified
        ///</summary>
        [TestMethod()]
        public void RunTestInvalidfile()
        {
            string feed = "mnb765";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            DirectoryInfo di = new DirectoryInfo("DataImporters");
            string processingDirectory = di.FullName;
            SJPStopAccessibilityLinksDataImporter target = new SJPStopAccessibilityLinksDataImporter(feed, params1, params2, utility, processingDirectory);
            string filename = "StopLinks_v0.21.csv";
            int actual;
            actual = target.Run(filename);
            Assert.IsTrue(0 != actual);

        }

        /// <summary>
        ///A test for Run when no feed file specified
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void RunTestNoFeedfile()
        {
            string feed = "mnb765";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            DirectoryInfo di = new DirectoryInfo("DataImporters");
            string processingDirectory = di.FullName;
            SJPStopAccessibilityLinksDataImporter target = new SJPStopAccessibilityLinksDataImporter(feed, params1, params2, utility, processingDirectory);
            string filename = null;
            int actual;
            actual = target.Run(filename);
            Assert.IsTrue(0 != actual);

        }
    }
}
