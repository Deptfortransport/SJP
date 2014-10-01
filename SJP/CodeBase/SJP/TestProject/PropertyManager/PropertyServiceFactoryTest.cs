// *********************************************** 
// NAME             : PropertyServiceFactoryTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 18 Feb 2011
// DESCRIPTION  	: Test class for PropertyServiceFactory
// ************************************************
                
                
using SJP.Common.PropertyManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager.PropertyProviders;
using SJP.Common;
using System.Configuration;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.PropertyManager
{
    
    
    /// <summary>
    ///This is a test class for PropertyServiceFactory and is intended
    ///to contain all PropertyServiceFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PropertyServiceFactoryTest
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
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            using (PropertyServiceFactory target = new PropertyServiceFactory())
            {

                object actual;
                actual = target.Get();

                Assert.IsNotNull(actual);

                Assert.IsInstanceOfType(actual, typeof(IPropertyProvider));
            }
            
        }

        /// <summary>
        ///A test for Register
        ///</summary>
        [TestMethod()]
        public void RegisterTest()
        {
            using (PropertyServiceFactory target = new PropertyServiceFactory())
            {
                IPropertyProvider provider = new DatabasePropertyProvider();
                target.Register(provider);

                Assert.IsNotNull(target.Get());

                Assert.IsInstanceOfType(target.Get(), typeof(DatabasePropertyProvider));
            }

        }


        /// <summary>
        ///A test for PropertyServiceFactory throwing SJPException 
        ///when FileNotFoundException gets raised in method
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void PropertyServiceFactoryTestFileNotFoundException()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providerassembly"].Value;

            config.AppSettings.Settings["propertyservice.providerassembly"].Value = @"abc";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {
                using (PropertyServiceFactory target = new PropertyServiceFactory())
                {
                    IPropertyProvider provider = new DatabasePropertyProvider();
                    target.Register(provider);

                    Assert.IsNotNull(target.Get());

                    Assert.IsInstanceOfType(target.Get(), typeof(DatabasePropertyProvider));
                }
            }
            finally
            {
                config.AppSettings.Settings["propertyservice.providerassembly"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }

        }


        /// <summary>
        ///A test for PropertyServiceFactory throwing SJPException 
        ///when ArgumentNullException gets raised in method
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void PropertyServiceFactoryTestArgumentException()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providerassembly"].Value;

            config.AppSettings.Settings["propertyservice.providerassembly"].Value = null;

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {
                using (PropertyServiceFactory target = new PropertyServiceFactory())
                {
                    IPropertyProvider provider = new DatabasePropertyProvider();
                    target.Register(provider);

                    Assert.IsNotNull(target.Get());

                    Assert.IsInstanceOfType(target.Get(), typeof(DatabasePropertyProvider));
                }
            }
            finally
            {
                config.AppSettings.Settings["propertyservice.providerassembly"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }

        }

        /// <summary>
        ///A test for PropertyServiceFactory throwing SJPException 
        ///when ArgumentNullException gets raised in method when loading provider class
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void PropertyServiceFactoryTestArgumentException1()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providerclass"].Value;

            config.AppSettings.Settings["propertyservice.providerclass"].Value = null;

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {
                using (PropertyServiceFactory target = new PropertyServiceFactory())
                {
                    IPropertyProvider provider = new DatabasePropertyProvider();
                    target.Register(provider);

                    Assert.IsNotNull(target.Get());

                    Assert.IsInstanceOfType(target.Get(), typeof(DatabasePropertyProvider));
                }
            }
            finally
            {
                config.AppSettings.Settings["propertyservice.providerclass"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }

        }


        /// <summary>
        ///A test to check PropertyServiceFactory's timer elapsed event gets called 
        ///and superseded value gets set
        ///</summary>
        [TestMethod()]
        public void PropertyServiceTimerElapsedTest()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            try
            {
                using (PropertyServiceFactory_Accessor target = new PropertyServiceFactory_Accessor())
                {
                    IPropertyProvider propProvider = (IPropertyProvider)target.Get();

                    config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"PropertyProviders\TestNewVersion.xml";

                    config.Save();

                    ConfigurationManager.RefreshSection("appSettings");
                    int count = 0;
                    while (!propProvider.IsSuperseded && count < 3)
                    {
                        count++;
                        System.Threading.Thread.Sleep(500);
                    }
                }
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
