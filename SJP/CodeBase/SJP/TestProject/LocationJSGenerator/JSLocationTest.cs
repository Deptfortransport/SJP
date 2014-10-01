// *********************************************** 
// NAME             : JSLocationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for JSLocation
// ************************************************
                
                
using SJP.Common.LocationJsGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.LocationService;

namespace SJP.TestProject.LocationJSGenerator
{
    
    
    /// <summary>
    ///This is a test class for JSLocationTest and is intended
    ///to contain all JSLocationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JSLocationTest
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
        ///A test for GetJSArrayString when alias is not defined
        ///</summary>
        [TestMethod()]
        public void GetJSArrayStringTestWithoutAlias()
        {
            string displayName = "Test"; 
            SJPLocationType locType = SJPLocationType.Locality; 
            string dataId = "E00Test"; 
            JSLocation target = new JSLocation(displayName, locType, dataId);
            string expected = "[\"Test\",\"E00Test\",\"Locality\"]";
            string actual;
            actual = target.GetJSArrayString();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(displayName, target.DisplayName);
            Assert.AreEqual(locType, target.LocationType);
            Assert.AreEqual(dataId, target.DataId);
           
        }

        /// <summary>
        ///A test for GetJSArrayString when alias is defined
        ///</summary>
        [TestMethod()]
        public void GetJSArrayStringTestWithAlias()
        {
            string displayName = "Test";
            SJPLocationType locType = SJPLocationType.Locality;
            string dataId = "E00Test";
            string alias = "TestAlias";
            JSLocation target = new JSLocation(displayName, locType, dataId,alias);
            string expected = "[\"Test\",\"E00Test\",\"Locality\",\"TestAlias\"]";
            string actual;
            actual = target.GetJSArrayString();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(alias, target.Alias);
        }
    }
}
