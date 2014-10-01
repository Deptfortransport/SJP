// *********************************************** 
// NAME             : DatabasePropertyProviderTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 17 Feb 2011
// DESCRIPTION  	: Unit tests for DatabasePropertyProvider class
// ************************************************
                
                
using SJP.Common.PropertyManager.PropertyProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.DatabaseInfrastructure;
using System.Configuration;
using SJP.Common;

namespace SJP.TestProject.PropertyProvider
{
    
    
    /// <summary>
    ///This is a test class for DatabasePropertyProviderTest and is intended
    ///to contain all DatabasePropertyProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DatabasePropertyProviderTest
    {


        private TestContext testContextInstance;
        private static TestDataManager testDataManager;
        private const string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPConfiguration;Trusted_Connection=true";


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
        [ClassInitialize()]
        public static void TestClassInitialize(TestContext testContext)
        {
            string setup_script = @"PropertyProviders\PropertyProvidersTestSetup.sql";
            string clearup_script = @"PropertyProviders\PropertyProvidersTestCleanUp.sql";

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
            
            testDataManager = new TestDataManager(
                null,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.DefaultDB);
            
            testDataManager.Setup();

        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void TestClassCleanup()
        {
            testDataManager.ClearData();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
        
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
        ///A test for CreateParam
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.propertyproviders.dll")]
        public void CreateParamTest()
        {
            DatabasePropertyProvider_Accessor target = new DatabasePropertyProvider_Accessor(); 
            string name = "test"; 
            string val = "5"; 
            
            SqlParameter actual;
            actual = target.CreateParam(name, val);

            Assert.AreEqual(name, actual.ParameterName);
            Assert.AreEqual(val, actual.Value);
            Assert.IsTrue(actual.Direction == System.Data.ParameterDirection.Input);
        }

        /// <summary>
        ///A test for ExecuteCommand
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.propertyproviders.dll")]
        public void ExecuteCommandTest()
        {
            DatabasePropertyProvider_Accessor target = new DatabasePropertyProvider_Accessor(); 
            string storedProc = "GetProperty";
            
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlParameter param = target.CreateParam("@pName", "propertyservice.version");

                con.Open();


                using (SqlDataReader actual = target.ExecuteCommand(storedProc, con, param))
                {

                    Assert.IsTrue(actual.HasRows);

                    actual.Read();

                    Assert.AreEqual(actual.GetString(0), "propertyservice.version");
                    Assert.AreEqual(actual.GetString(1), "1");
                }
            }
           
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            DatabasePropertyProvider target = new DatabasePropertyProvider(); 

            IPropertyProvider properties =  target.Load();

            string expApplicationID = ConfigurationManager.AppSettings["propertyservice.applicationid"];
            string expGroupID = ConfigurationManager.AppSettings["propertyservice.groupid"];

            Assert.AreEqual(expApplicationID, properties.ApplicationID);
            Assert.AreEqual(expGroupID, properties.GroupID);
            Assert.AreEqual(1, properties.Version);

            Assert.AreEqual("1000", properties["propertyservice.refreshrate"]);

            Assert.IsFalse(properties.IsSuperseded);

            // no changes are made so loading properties again should return null
            properties = target.Load();

            Assert.IsNull(properties);

            //  updating the version no so properties does get refrehed
            try
            {

                using (SqlHelper helper = new SqlHelper())
                {
                    try
                    {
                        helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                        helper.Execute("DatabasePropertyProviderTestLoad1");

                        properties = target.Load();

                        Assert.AreEqual(2, properties.Version);


                    }
                    finally
                    {
                        helper.ConnClose();
                    }

                }
            }
            finally
            {


                // reset database to original state
                testDataManager.ClearData();

                testDataManager.Setup();
            }


        }

        /// <summary>
        ///A test for Load method raising SJPException when format exception occurs
        ///i.e failed to conver version valud property in to integer value
        ///</summary>
        //////<remarks>
        ///<b>Check the stored proc name for correctness after running thist test as 
        ///it may not changed the stored proc name back if error occurs</b>
        /// </remarks>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void LoadTestWithFormatException()
        {
            DatabasePropertyProvider target = new DatabasePropertyProvider(); 

           
            try
            {

                using (SqlHelper helper = new SqlHelper())
                {
                    try
                    {
                        helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                        helper.Execute("DatabasePropertyProviderTestLoad3");
                                                

                        IPropertyProvider properties = target.Load();


                    }
                    finally
                    {
                        helper.ConnClose();
                    }

                }
            }
            finally
            {


                // reset database to original state
                testDataManager.ClearData();

                testDataManager.Setup();
            }
            


        }


        /// <summary>
        ///A test for Load method raising SJPExeption when sql exception occurs
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void LoadTestWithSqlException()
        {
            DatabasePropertyProvider target = new DatabasePropertyProvider(); 
            
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                    // Renaming sql stored procedure to get sql to raise sql exception
                    // NOTE the PropertyProvidersTestCleanUp also tests if the 'SelectGlobalProperties1' is not changed
                    // back to 'SelectGlobalProperties'
                    helper.Execute("EXEC sp_rename 'SelectGlobalProperties', 'SelectGlobalProperties1'");


                    IPropertyProvider properties = target.Load();


                }
                finally
                {
                    // Resetting the GetVersion stored procedure back to correct name
                    // NOTE the PropertyProvidersTestCleanUp also tests if the 'SelectGlobalProperties1' is not changed
                    // back to 'SelectGlobalProperties'
                    helper.Execute("EXEC sp_rename 'SelectGlobalProperties1', 'SelectGlobalProperties'");
                    helper.ConnClose();
                }

            }
           
        }


        

        /// <summary>
        ///A test for IsNewVersion
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void IsNewVersionTest()
        {
            DatabasePropertyProvider target = new DatabasePropertyProvider(); 

            IPropertyProvider properties = target.Load();

            bool actual;
            actual = target.IsNewVersion();

            Assert.IsFalse(actual);

            try
            {

                using (SqlHelper helper = new SqlHelper())
                {
                    try
                    {
                        helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                        helper.Execute("DatabasePropertyProviderTestLoad1");

                        actual = target.IsNewVersion();

                        Assert.IsTrue(actual);

                        //Test InvalidOperationException
                        helper.Execute("DatabasePropertyProviderTestLoad2");

                        actual = target.IsNewVersion();

                    }
                    finally
                    {
                        helper.ConnClose();
                    }

                }
            }
            finally
            {


                // reset database to original state
                testDataManager.ClearData();

                testDataManager.Setup();
            }
        }

        /// <summary>
        ///A test for IsNewVersion when sql exception gets raised
        ///</summary>
        ///<remarks>
        ///<b>Check the stored proc name for correctness after running thist test as 
        ///it may not changed the stored proc name back if error occurs</b>
        /// </remarks>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void IsNewVersionWithSqlExceptionTest()
        {
            DatabasePropertyProvider target = new DatabasePropertyProvider(); 

            IPropertyProvider properties = target.Load();

            bool actual;
            actual = target.IsNewVersion();

            Assert.IsFalse(actual);

            
            using (SqlHelper helper = new SqlHelper())
            {
                try
                {
                    helper.ConnOpen(SqlHelperDatabase.DefaultDB);

                    // Renaming sql stored procedure to get sql to raise sql exception
                    // NOTE the PropertyProvidersTestCleanUp also tests if the 'GetVersion1' is not changed
                    // back to 'GetVersion'
                    helper.Execute("EXEC sp_rename 'GetVersion', 'GetVersion1'");


                    actual = target.IsNewVersion();

                    Assert.IsTrue(actual);


                }
                finally
                {
                    // NOTE the PropertyProvidersTestCleanUp also tests if the 'GetVersion1' is not changed
                    // back to 'GetVersion'
                    helper.Execute("EXEC sp_rename 'GetVersion1', 'GetVersion'");

                    helper.ConnClose();
                }

            }
            
        }

       
    }
}
