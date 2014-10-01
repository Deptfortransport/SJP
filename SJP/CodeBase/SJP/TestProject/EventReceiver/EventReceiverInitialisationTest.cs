using SJP.Reporting.EventReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;
using System.Collections.Generic;
using SJP.Common.PropertyManager;
using System.Diagnostics;
using SJP.Common.EventLogging;
using SJP.Common;
using System.Configuration;

namespace SJP.TestProject.EventReceiver
{
    
    
    /// <summary>
    ///This is a test class for EventReceiverInitialisationTest and is intended
    ///to contain all EventReceiverInitialisationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventReceiverInitialisationTest
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
        [ExpectedException(typeof(SJPException))]
        public void PopulateTest()
        {
            EventReceiverInitialisation target = new EventReceiverInitialisation();

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

            // Service Cache is null - Exception gets thrown
            SJPServiceDiscovery_Accessor.ResetServiceDiscoveryForTest();

            target.Populate(serviceCache);

        }

        /// <summary>
        ///A test for Populate - when there is an exception initialising custom event publishers
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void PopulateTest_PublisherException()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"EventReceiver\TestProject.Properties.PublisherException.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            EventReceiverInitialisation target = new EventReceiverInitialisation();

            SJPServiceDiscovery_Accessor.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery_Accessor.current = new SJPServiceDiscovery();

            SJPServiceDiscovery_Accessor accessor = new SJPServiceDiscovery_Accessor(new PrivateObject(SJPServiceDiscovery_Accessor.current));

            accessor.serviceCache = new Dictionary<string, IServiceFactory>();

            using (PropertyServiceFactory ps = new PropertyServiceFactory())
            {
                accessor.SetServiceForTest(ServiceDiscoveryKey.PropertyService, ps);
            }


            Trace.Listeners.Clear();

            try
            {

                Dictionary<string, IServiceFactory> serviceCache = new Dictionary<string, IServiceFactory>();

                target.Populate(serviceCache);
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        ///A test for Populate - when the required properties are not supplied or are wrong
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void PopulateTest_PropertyErrors()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"EventReceiver\TestProject.Properties.PublisherException.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            EventReceiverInitialisation target = new EventReceiverInitialisation();

            SJPServiceDiscovery_Accessor.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery_Accessor.current = new SJPServiceDiscovery();

            SJPServiceDiscovery_Accessor accessor = new SJPServiceDiscovery_Accessor(new PrivateObject(SJPServiceDiscovery_Accessor.current));

            accessor.serviceCache = new Dictionary<string, IServiceFactory>();

            using (PropertyServiceFactory ps = new PropertyServiceFactory())
            {
                accessor.SetServiceForTest(ServiceDiscoveryKey.PropertyService, ps);
            }


            Trace.Listeners.Clear();

            try
            {

                Dictionary<string, IServiceFactory> serviceCache = new Dictionary<string, IServiceFactory>();

                target.Populate(serviceCache);
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
