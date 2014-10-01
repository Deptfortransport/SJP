// *********************************************** 
// NAME             : FilePropertyProviderTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 17 Feb 2011
// DESCRIPTION  	: Unit tests for FilePropertyProvider class
// ************************************************
                
                
using SJP.Common.PropertyManager.PropertyProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using System.Xml;
using System.Configuration;
using SJP.Common;

namespace SJP.TestProject.PropertyProvider
{
    
    
    /// <summary>
    ///This is a test class for FilePropertyProviderTest and is intended
    ///to contain all FilePropertyProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FilePropertyProviderTest
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
        ///A test for Load method 
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            FilePropertyProvider target = new FilePropertyProvider(); 

            IPropertyProvider actual;

            actual = target.Load();

            Assert.AreEqual(1, actual.Version);

            actual = target.Load();

            Assert.IsNull(actual);
            

        }
        

        /// <summary>
        ///A test for Load method checking for FileNotFoundException
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void LoadTestWithFileNotFoundException()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = "App.config";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {

                FilePropertyProvider target = new FilePropertyProvider(); 

                IPropertyProvider actual;

                actual = target.Load();
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }
            
        }
        

        /// <summary>
        ///A test for Load checking for ArgumentException
        ///</summary>
        [TestMethod()]
        public void LoadTestWithNewVersion()
        {
            FilePropertyProvider target = new FilePropertyProvider(); 
            
            IPropertyProvider actual;
            actual = target.Load();
            
            Assert.AreEqual(1, actual.Version);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"PropertyProviders\TestNewVersion.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {

                actual = target.Load();

                Assert.AreEqual(2, actual.Version);
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }

            
        }


        /// <summary>
        ///A test for Load raising SJPException when New version is provided
        ///The exception gets rethrown as SJPException
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void LoadTestWithNewVersionSJPException()
        {
            FilePropertyProvider target = new FilePropertyProvider(); 

            IPropertyProvider actual;
            actual = target.Load();

            Assert.AreEqual(1, actual.Version);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"PropertyProviders\TestNewVersionException.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {

                actual = target.Load();

                Assert.AreEqual(2, actual.Version);
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();

                ConfigurationManager.RefreshSection("appSettings");
            }

        }

        
        /// <summary>
        ///A test for IsNumeric method
        ///</summary>
        [TestMethod()]
        public void IsNumericTest()
        {
            string toTest = "5"; 
            bool expected = true; 
            bool actual;
            actual = FilePropertyProvider_Accessor.IsNumeric(toTest);
            Assert.AreEqual(expected, actual);

            toTest = "a";
            expected = false;
            actual = FilePropertyProvider_Accessor.IsNumeric(toTest);
            Assert.AreEqual(expected, actual); 

        }


        /// <summary>
        ///A test for IsNewVersion
        ///</summary>
        [TestMethod()]
        public void IsNewVersionTest()
        {
            FilePropertyProvider target = new FilePropertyProvider(); 

            IPropertyProvider actual;
            actual = target.Load();

            Assert.IsFalse(target.IsNewVersion());

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"PropertyProviders\TestNewVersion.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {

                Assert.IsTrue(target.IsNewVersion());
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
        }


        /// <summary>
        ///A test for GetVersion with NullReference Exception get raised
        ///The exception gets rethrown as SJPException
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void GetVersionNullReferenceException()
        {
            FilePropertyProvider_Accessor target = new FilePropertyProvider_Accessor();

            IPropertyProvider actual;
            actual = target.Load();

            Assert.IsFalse(target.IsNewVersion());

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"PropertyProviders\TestNewVersionNullReferenceException.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {

                int version =  target.GetVersion();
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        ///A test for GetVersion with Format Exception get raised
        ///The exception gets rethrown as SJPException
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void GetVersionSJPException()
        {
            FilePropertyProvider_Accessor target = new FilePropertyProvider_Accessor();

            IPropertyProvider actual;
            actual = target.Load();

            Assert.IsFalse(target.IsNewVersion());

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"PropertyProviders\TestNewVersionException.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {

                int version = target.GetVersion();
            }
            finally
            {

                config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        ///A test for GetVersion with xml Exception get raised
        ///The exception gets rethrown as SJPException
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void GetVersionXMLException()
        {
            FilePropertyProvider_Accessor target = new FilePropertyProvider_Accessor();

            IPropertyProvider actual;
            actual = target.Load();

            Assert.IsFalse(target.IsNewVersion());

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = @"PropertyProviders\TestNewVersionXMLException.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            try
            {

                int version = target.GetVersion();
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
