using SJP.DataImporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using TransportDirect.Common.ServiceDiscovery;
using System.Diagnostics;

namespace SJP.TestProject.DataImporters
{


    /// <summary>
    ///This is a test class for SJPVenueAdditionalDataImporterTest and is intended
    ///to contain all SJPVenueAdditionalDataImporterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPTravelcardDataImporterTest
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
        ///A test for SJPTravelcardDataImporter Constructor
        ///</summary>
        [TestMethod()]
        public void SJPTravelcardDataImporterConstructorTest()
        {
            string datafeed = "SJPTravelcardData.xml";

            string feed = "mbt156";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            string processingDirectory = "DataImporters";
            SJPTravelcardDataImporter target = new SJPTravelcardDataImporter(feed, params1, params2, utility, processingDirectory);
            int actual = target.Run(datafeed);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///A test for SJPTravelcardDataImporter Constructor with wrong feed name and file name
        ///</summary>
        [TestMethod()]
        public void SJPTravelcardDataImporterConstructorTestWrongFileAndFeed()
        {
            string datafeed = "SJPTravelcardData.xml1";

            string feed = "mbt1561";
            string params1 = string.Empty;
            string params2 = string.Empty;
            string utility = string.Empty;
            string processingDirectory = "DataImporters";
            SJPTravelcardDataImporter target = new SJPTravelcardDataImporter(feed, params1, params2, utility, processingDirectory);
            int actual = target.Run(datafeed);
            Assert.IsTrue(0 != actual);
        }
    }
}
