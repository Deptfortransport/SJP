using SJP.UserPortal.StateServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Soss.Client;
using System.Runtime.Serialization;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for DataAccessorStringTest and is intended
    ///to contain all DataAccessorStringTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataAccessorStringTest
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
        ///A test for CreateStateServerKey
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.stateserver.dll")]
        public void CreateStateServerKeyTest()
        {
            string applicationName = "TestApp";
            string key = "TestAppKey";
            StateServerKey actual = DataAccessorString_Accessor.CreateStateServerKey(applicationName, key);

            Assert.AreNotEqual(0, actual.AppId, "Expected a non-zero AppID if the key had created successfully");
        }

        /// <summary>
        ///A test for DataAccessorString Constructor
        ///</summary>
        [TestMethod()]
        public void DataAccessorStringConstructorTest()
        {
            string applicationName = "TestApp";
            string key = "TestAppKey";
            bool lockWhenReading = false;

            using (DataAccessorString target = new DataAccessorString(applicationName, key, lockWhenReading))
            {
                Assert.AreNotEqual(0, target.Key.AppId, "Expected a non-zero AppID if the key had created successfully");
            }
        }

        // Can't see what to provide as parameter values!
        ///// <summary>
        /////A test for DataAccessorString Constructor
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("sjp.userportal.stateserver.dll")]
        //public void DataAccessorStringConstructorTest1()
        //{
        //    SerializationInfo info = null; // TODO: Initialize to an appropriate value
        //    StreamingContext context = new StreamingContext(); // TODO: Initialize to an appropriate value
        //    DataAccessorString_Accessor target = new DataAccessorString_Accessor(info, context);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}
    }
}
