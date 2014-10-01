// *********************************************** 
// NAME             : ContentGroupTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for ContentGroup
// ************************************************
                
                
using SJP.Common.ResourceManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.Web;
using SJP.Common.ServiceDiscovery;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ContentGroupTest and is intended
    ///to contain all ContentGroupTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContentGroupTest
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
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            testDataManager.ClearData();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
        //
        
        #endregion

                

        /// <summary>
        ///A test for ClearControlProperties
        ///</summary>
        [TestMethod()]
        public void ClearControlPropertiesTest()
        {
            ControlPropertyCollectionProvider_Accessor controlPropertyCollectionProvider = new ControlPropertyCollectionProvider_Accessor();
            string groupName = "General"; 
            ContentGroup target = new ContentGroup(groupName);
            target.GetControlProperties(Language.English);
            target.ClearControlProperties();
            Assert.AreEqual(0, controlPropertyCollectionProvider.dictionary.Count);
            
            
        }

        /// <summary>
        ///A test for GetControlProperties
        ///</summary>
        [TestMethod()]
        public void GetControlPropertiesTest()
        {
            string groupName = "General";
            ContentGroup target = new ContentGroup(groupName);

            ControlPropertyCollection actual;
            actual = target.GetControlProperties(Language.English);

            Assert.AreEqual(1, actual.GetControlCount());

            Assert.IsNotNull(actual.GetPropertyNames("General"));
            Assert.AreEqual(10, actual.GetPropertyNames("General").Length);

            Assert.IsNotNull(actual.GetPropertyNames("TEST"));
            Assert.AreEqual(0, actual.GetPropertyNames("TEST").Length);
        }

        /// <summary>
        ///A test for GetControlProperties
        ///</summary>
        [TestMethod()]
        public void GetControlPropertiesTest1()
        {
            string groupName = "General"; 
            ContentGroup target = new ContentGroup(groupName); 
            Language language = Language.French; 
            ControlPropertyCollection actual;
            actual = target.GetControlProperties(language);
            Assert.AreEqual(1, actual.GetControlCount());
            Assert.IsNotNull(actual.GetPropertyNames("General"));
            // property names will be still 10 event though there is  only 1 row for french in test data
            // as the resource manger loads english content by default for those where french content not found
            Assert.AreEqual(10, actual.GetPropertyNames("General").Length);
            // Check that we loaded the french content
            Assert.AreEqual("Le To", actual.GetPropertyValue("General", "AccessibilityOpitons.LblTo.Text"));
        }

        /// <summary>
        ///A test for GroupName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.resourcemanager.dll")]
        public void GroupNameTest()
        {
            PrivateObject param0 = new PrivateObject(new ContentGroup("General")); 
            ContentGroup_Accessor target = new ContentGroup_Accessor(param0); 
            string actual;
            actual = target.GroupName;
            Assert.AreEqual("General", actual);
        }
    }
}
