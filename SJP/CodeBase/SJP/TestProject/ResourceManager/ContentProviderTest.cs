// *********************************************** 
// NAME             : ContentProviderTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for ContentProvider
// ************************************************
                
                
using SJP.Common.ResourceManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.Web;
using System.Data.SqlClient;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ContentProviderTest and is intended
    ///to contain all ContentProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContentProviderTest
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
        ///A test for DataChangedNotificationReceived
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.resourcemanager.dll")]
        public void DataChangedNotificationReceivedTest()
        {
            
            ContentProvider_Accessor target = new ContentProvider_Accessor(new PrivateObject(ContentProvider.Instance));
            ContentGroup contentGroup = ContentProvider.Instance["General"];
            contentGroup.GetControlProperties(Language.English);
            InsertTestContentForDataChangeNotificationTest();
            object sender = null; 
            ChangedEventArgs e = new ChangedEventArgs("Content"); 
            target.DataChangedNotificationReceived(sender, e);
            // content gets cleared upon datachange notification
            contentGroup.GetControlProperties(Language.English);
            SJPResourceManager resourceManger = new SJPResourceManager();
            Assert.IsNotNull(resourceManger.GetString(Language.English, "ChangeNotificationTest"));
            Assert.AreEqual("ChangeNotificationTestValue", resourceManger.GetString(Language.English, "ChangeNotificationTest"));
            
        }

        

        /// <summary>
        ///A test for GetControlPropertyCollection When exception gets raised due to invalid group name and/or language specified
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SqlException))]
        public void GetControlPropertyCollectionTestWIthException()
        {
            string groupName = string.Empty; 
            Language language = new Language(); 
           ControlPropertyCollection actual;
            actual = ContentProvider.GetControlPropertyCollection(groupName, language);
           
            
        }

        
        /// <summary>
        ///A test for RegisterForChangeNotification when the datachange notification not initialised and 
        ///Exception gets thrown
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.resourcemanager.dll")]
        [ExpectedException(typeof(NullReferenceException))]
        public void RegisterForChangeNotificationTestException()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            ContentProvider_Accessor target = new ContentProvider_Accessor(); 
            bool actual;
            actual = target.RegisterForChangeNotification();

        }

        #region Private Helper Methods
        /// <summary>
        /// Inserts text for testing datachange notifiction
        /// </summary>
        private void InsertTestContentForDataChangeNotificationTest()
        {
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.ContentDB);

                    helper.Execute("EXEC AddContent 'General', 'en', 'General', 'ChangeNotificationTest', 'ChangeNotificationTestValue'");
                }
                finally
                {
                    helper.ConnClose();
                }
            }
        }
        #endregion

    }
}
