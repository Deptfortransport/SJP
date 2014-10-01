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
    ///This is a test class for SJPGNATLocationsDataImporterTest and is intended
    ///to contain all SJPGNATLocationsDataImporterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPGNATLocationsDataImporterTest
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
        ///A test for SJPGNATLocationsDataImporter Constructor
        ///</summary>
        [TestMethod()]
        public void SJPGNATLocationsDataImporterConstructorTest()
        {
            string feed = "mkw489";  
            string params1 = string.Empty;  
            string params2 = string.Empty;  
            string utility = string.Empty;
            DirectoryInfo di = new DirectoryInfo("DataImporters");
            string processingDirectory = di.FullName;  
            SJPGNATLocationsDataImporter target = new SJPGNATLocationsDataImporter(feed, params1, params2, utility, processingDirectory);
            string filename = "110613 GNAT STATION LIST VERSION 8FN.csv";  
            int expected = 0;  
            int actual;
            actual = target.Run(filename);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for SJPGNATLocationsDataImporter Constructor with wrong feed name and file name
        ///</summary>
        [TestMethod()]
        public void SJPGNATLocationsDataImporterConstructorTestWrongFileAndFeed()
        {
            string datafeed = "110613 GNAT STATION LIST VERSION 8FN.csv1";

            string feed = "mkw4891";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            string processingDirectory = "DataImporters";
            SJPGNATLocationsDataImporter target = new SJPGNATLocationsDataImporter(feed, params1, params2, utility, processingDirectory);
            int actual = target.Run(datafeed);
            Assert.IsTrue(0 != actual);
        }
        
        /// <summary>
        ///A test for Run when invalid File specified
        ///</summary>
        [TestMethod()]
        public void SJPGNATLocationsDataImporterConstructorInvalidfile()
        {
            string feed = "mkw489";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            DirectoryInfo di = new DirectoryInfo("DataImporters");
            string processingDirectory = di.FullName;
            SJPGNATLocationsDataImporter target = new SJPGNATLocationsDataImporter(feed, params1, params2, utility, processingDirectory);
            string filename = "110613 GNAT STATION LIST VERSION 8FN1.csv";
            int actual;
            actual = target.Run(filename);
            Assert.IsTrue(0 != actual);

        }

        /// <summary>
        ///A test for Run when no feed file specified
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void SJPGNATLocationsDataImporterConstructorNoFeedfile()
        {
            string feed = "mkw489";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            DirectoryInfo di = new DirectoryInfo("DataImporters");
            string processingDirectory = di.FullName;
            SJPGNATLocationsDataImporter target = new SJPGNATLocationsDataImporter(feed, params1, params2, utility, processingDirectory);
            string filename = null;
            int actual;
            actual = target.Run(filename);
            Assert.IsTrue(0 != actual);

        }
    }
}
