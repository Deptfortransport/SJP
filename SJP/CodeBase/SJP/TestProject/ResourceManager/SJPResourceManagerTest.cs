// *********************************************** 
// NAME             : SJPResourceManagerTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPResourceManager
// ************************************************
                
                
using SJP.Common.ResourceManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.Web;
using SJP.Common;
using SJP.Common.ServiceDiscovery;
using SJP.Common.DatabaseInfrastructure;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPResourceManagerTest and is intended
    ///to contain all SJPResourceManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPResourceManagerTest
    {


        private TestContext testContextInstance;
        private static TestDataManager testDataManager;

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
        [TestInitialize()]
        public void TestInitialize()
        {
            string test_data = @"ResourceManager\ResourceManagerTestData.xml";
            string setup_script = @"ResourceManager\ResourceManagerTestSetup.sql";
            string clearup_script = @"ResourceManager\ResourceManagerTestCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPContent;Trusted_Connection=true";

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.ContentDB);
            testDataManager.Setup();

            testDataManager.LoadData(false);

        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        [TestCleanup()]
        public void TestCleanup()
        {
            testDataManager.ClearData();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }


        #endregion



        /// <summary>
        ///A test for GetString
        ///</summary>
        [TestMethod()]
        public void GetStringTest()
        {
            SJPResourceManager target = new SJPResourceManager(); 
            string groupName = SJPResourceManager.GROUP_DEFAULT; 
            string collectionName = SJPResourceManager.COLLECTION_DEFAULT; 
            string key = "AccessibilityOpitons.LblTo.Text"; 
            string expected = "To"; 
            string actual;
            actual = target.GetString(Language.English, groupName, collectionName, key);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetString for French content
        ///</summary>
        [TestMethod()]
        public void GetStringTestFrenchContent()
        {
            SJPResourceManager target = new SJPResourceManager(); 
            Language language = Language.French;
            string groupName = SJPResourceManager.GROUP_DEFAULT;
            string collectionName = SJPResourceManager.COLLECTION_DEFAULT;
            string key = "AccessibilityOpitons.LblTo.Text";
            string expected = "Le To"; 
            string actual;
            actual = target.GetString(language, groupName, collectionName, key);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for GetString with only Key specified
        ///</summary>
        [TestMethod()]
        public void GetStringTestByKeyOnly()
        {
            SJPResourceManager target = new SJPResourceManager(); 
            string key = "AccessibilityOptions.Back.ToolTip"; 
            string expected = "Back"; 
            string actual;
            actual = target.GetString(Language.English, key);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetString with collection and key specified
        ///</summary>
        [TestMethod()]
        public void GetStringTestByCollectionAndKey()
        {
            SJPResourceManager target = new SJPResourceManager(); 
            string collectionName = SJPResourceManager.COLLECTION_DEFAULT; 
            string key = "AccessibilityOptions.Back.ToolTip"; 
            string expected = "Back"; 
            string actual;
            actual = target.GetString(Language.English, collectionName, key);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for GetString with invalid key specified
        ///</summary>
        [TestMethod()]
        public void GetStringTestInvalidKey()
        {
            SJPResourceManager target = new SJPResourceManager();
            string collectionName = SJPResourceManager.COLLECTION_DEFAULT;
            string key = "AccessibilityOptions.Back.TolTip";
            string actual;
            actual = target.GetString(Language.English, collectionName, key);
            Assert.IsNull(actual);

        }
    }
}
