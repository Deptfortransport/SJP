// *********************************************** 
// NAME             : DataServicesTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for DataServices
// ************************************************
                
                
using SJP.Common.DataServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.DatabaseInfrastructure;
using System.Collections;
using System.Web.UI.WebControls;
using SJP.Common.ResourceManager;
using System.Data.SqlClient;
using System.Collections.Generic;
using SJP.Common.ServiceDiscovery;
using SJP.Common;
using SJP.Common.PropertyManager;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for DataServicesTest and is intended
    ///to contain all DataServicesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataServicesTest
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
            string test_data = @"DataServices\DataServicesData.xml";
            string setup_script = @"DataServices\DataServicesTestSetup.sql";
            string clearup_script = @"DataServices\DataServicesTestCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPTransientPortal;Trusted_Connection=true";

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.TransientPortalDB);
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
        ///A test for FindDatabase
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dataservices.dll")]
        public void FindDatabaseTest()
        {
            string DBName = "TransientPortalDB"; 
            SqlHelperDatabase expected = SqlHelperDatabase.TransientPortalDB; 
            SqlHelperDatabase actual;
            actual = DataServices_Accessor.FindDatabase(DBName);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for GetList
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void GetListTestIlligalType()
        {
            DataServices_Accessor target = new DataServices_Accessor(); 

            DataServices_Accessor.cache.Add(DataServiceType.DataServiceTypeEnd, new string[]{"test"});

            DataServiceType item = DataServiceType.DataServiceTypeEnd;
            ArrayList actual;
            
            actual = target.GetList(item);   
        }

        /// <summary>
        ///A test for LoadDataCache
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dataservices.dll")]
        [ExpectedException(typeof(SJPException))]
        public void LoadDataCacheTest()
        {
            DataServices_Accessor target = new DataServices_Accessor();
            DataServices_Accessor.cache.Clear();
            target.LoadDataCache();
            Assert.AreEqual(4, DataServices_Accessor.cache.Keys.Count);
            
            // database property is not set - Exception gets thrown
            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));
            string dbprop = accessor.propertyDictionary["SJP.UserPortal.DataServices.CountryDrop.db"];
            accessor.propertyDictionary["SJP.UserPortal.DataServices.CountryDrop.db"] = string.Empty;
            DataServices_Accessor.cache.Clear();
            target.LoadDataCache();
            // reset the property back
            accessor.propertyDictionary["SJP.UserPortal.DataServices.CountryDrop.db"] = dbprop;


            // query property is not set - Exception gets thrown
            accessor = new Properties_Accessor(new PrivateObject(Properties.Current));
            string queryProp = accessor.propertyDictionary["SJP.UserPortal.DataServices.CycleRouteType.query"];
            accessor.propertyDictionary["SJP.UserPortal.DataServices.CycleRouteType.query"] = string.Empty;
            DataServices_Accessor.cache.Clear();
            target.LoadDataCache();
            // reset the property back
            accessor.propertyDictionary["SJP.UserPortal.DataServices.CycleRouteType.query"] = queryProp;

            // wrong datatype defined - Exception gets thrown
            accessor = new Properties_Accessor(new PrivateObject(Properties.Current));
            string datatype = accessor.propertyDictionary["SJP.UserPortal.DataServices.NewsRegionDrop.type"];
            accessor.propertyDictionary["SJP.UserPortal.DataServices.NewsRegionDrop.type"] = "6";
            DataServices_Accessor.cache.Clear();
            target.LoadDataCache();
            // reset the property back
            accessor.propertyDictionary["SJP.UserPortal.DataServices.NewsRegionDrop.type"] = datatype;


        }

        /// <summary>
        ///A test for LoadListControl
        ///</summary>
        ///<remarks>This class assumes that the resource content database has content for the dropdown resource Ids</remarks>
        [TestMethod()]
        public void LoadListControlTest()
        {
            DataServices target = new DataServices(); 
            DataServiceType dataSet = DataServiceType.CycleRouteType;

            using (DropDownList control = new DropDownList())
            {
                SJPResourceManager rm = new SJPResourceManager();
                target.LoadListControl(dataSet, control, rm, Language.English);
                Assert.AreEqual(3, control.Items.Count);
                Assert.IsTrue(control.Items[0].Text.Contains("Quietest"));
                Assert.IsTrue(control.Items[1].Text.Contains("Quickest"));
                Assert.IsTrue(control.Items[2].Text.Contains("Recreational"));
                Assert.AreEqual(true, control.Items[0].Selected);
            }
        }

        
    }
}
