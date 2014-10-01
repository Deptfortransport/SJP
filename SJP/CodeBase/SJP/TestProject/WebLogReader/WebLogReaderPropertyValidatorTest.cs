// *********************************************** 
// NAME             : WebLogReaderPropertyValidatorTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: WebLogReaderPropertyValidator unit tests
// ************************************************


using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.WebLogReader;

namespace SJP.TestProject.WebLogReader
{
    
    
    /// <summary>
    ///This is a test class for WebLogReaderPropertyValidatorTest and is intended
    ///to contain all WebLogReaderPropertyValidatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WebLogReaderPropertyValidatorTest
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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            WebLogReaderInitialisation initialisation = new WebLogReaderInitialisation();

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

            SJPServiceDiscovery.Init(initialisation);
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
        //
        #endregion

                

        /// <summary>
        ///A test for ValidateArchiveDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void ValidateArchiveDirectoryTest()
        {
            PrivateObject param0 = new PrivateObject(new WebLogReaderPropertyValidator(Properties.Current)); 
            WebLogReaderPropertyValidator_Accessor target = new WebLogReaderPropertyValidator_Accessor(param0); 
            string key = "WebLogReader.W3SVC1.ArchiveDirectory"; 
            List<string> errors = new List<string>(); 
            bool expected = true; 
            bool actual;
            actual = target.ValidateArchiveDirectory(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count == 0);

            key = "WebLogReader.W3SVC1.ArchiveDirectory.NegativeTest";
            expected = false;
            actual = target.ValidateArchiveDirectory(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count > 0);
        }

        /// <summary>
        ///A test for ValidateClientIPExclusions
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void ValidateClientIPExclusionsTest()
        {
            PrivateObject param0 = new PrivateObject(new WebLogReaderPropertyValidator(Properties.Current));
            WebLogReaderPropertyValidator_Accessor target = new WebLogReaderPropertyValidator_Accessor(param0);

            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));

            string key = "WebLogReader.ClientIPExcludes";
            accessor.propertyDictionary[key] = "30.125.125.254";
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateClientIPExclusions(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count == 0);

            accessor.propertyDictionary[key] = "900.145.1475.57"; // invalid ip address
            expected = false;
            actual = target.ValidateClientIPExclusions(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count > 0);
                        
        }

       

        /// <summary>
        ///A test for ValidateLocalTimeZone
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void ValidateLocalTimeZoneTest()
        {
            PrivateObject param0 = new PrivateObject(new WebLogReaderPropertyValidator(Properties.Current));
            WebLogReaderPropertyValidator_Accessor target = new WebLogReaderPropertyValidator_Accessor(param0);
            List<string> errors = new List<string>(); 
            bool expected = true; 
            bool actual;
            actual = target.ValidateLocalTimeZone(errors);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for ValidateLogDirectory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void ValidateLogDirectoryTest()
        {
            PrivateObject param0 = new PrivateObject(new WebLogReaderPropertyValidator(Properties.Current));
            WebLogReaderPropertyValidator_Accessor target = new WebLogReaderPropertyValidator_Accessor(param0);
            string key = "WebLogReader.W3SVC1.LogDirectory";
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateLogDirectory(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count == 0);

            key = "WebLogReader.W3SVC1.LogDirectory.NegativeTest";
            expected = false;
            actual = target.ValidateLogDirectory(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count > 0);
        }

        /// <summary>
        ///A test for ValidateNonPageMinimumBytes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void ValidateNonPageMinimumBytesTest()
        {
            PrivateObject param0 = new PrivateObject(new WebLogReaderPropertyValidator(Properties.Current));
            WebLogReaderPropertyValidator_Accessor target = new WebLogReaderPropertyValidator_Accessor(param0);

            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));

            string key = "WebLogReader.NonPageMinimumBytes";
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateNonPageMinimumBytes(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count == 0);

            accessor.propertyDictionary[key] = "-5"; // invalid property value
            expected = false;
            actual = target.ValidateNonPageMinimumBytes(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count > 0);
        }

        /// <summary>
        ///A test for ValidateProperty
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void ValidatePropertyTest()
        {
            PrivateObject param0 = new PrivateObject(new WebLogReaderPropertyValidator(Properties.Current));
            WebLogReaderPropertyValidator_Accessor target = new WebLogReaderPropertyValidator_Accessor(param0);
            string key = Keys.WebLogReaderArchiveDirectory;
            List<string> errors = new List<string>();
            bool expected = true; 
            bool actual;
            actual = target.ValidateProperty(key, errors);
            Assert.AreEqual(expected, actual);
            

            // Exception condition when the key supplied is not valid
            key = "WebLogReader.NonPageMinimumBytes.123";
            actual = target.ValidateProperty(key, errors);
            
        }

        /// <summary>
        ///A test for ValidateWebLogFolders
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void ValidateWebLogFoldersTest()
        {
            PrivateObject param0 = new PrivateObject(new WebLogReaderPropertyValidator(Properties.Current));
            WebLogReaderPropertyValidator_Accessor target = new WebLogReaderPropertyValidator_Accessor(param0);

            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));

            string key = "WebLogReader.WebLogFolders";
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateWebLogFolders(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count == 0);

            key = "undefined";
            expected = false;
            actual = target.ValidateWebLogFolders(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count > 0);
        }

       
    }
}
