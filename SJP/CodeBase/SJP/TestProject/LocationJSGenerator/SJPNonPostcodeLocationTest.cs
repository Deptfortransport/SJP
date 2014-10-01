// *********************************************** 
// NAME             : SJPNonPostcodeLocationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPNonPostcodeLocation
// ************************************************
                
                
using SJP.Common.LocationJsGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.LocationJsGenerator
{
    
    
    /// <summary>
    ///This is a test class for SJPNonPostcodeLocationTest and is intended
    ///to contain all SJPNonPostcodeLocationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPNonPostcodeLocationTest
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
        ///A test for SJPNonPostcodeLocation Constructor
        ///</summary>
        [TestMethod()]
        public void SJPNonPostcodeLocationClassTest()
        {
            SJPNonPostcodeLocation target = new SJPNonPostcodeLocation();
            target.ID = 1;
            target.LocalityID = "test";
            target.Name = "test";
            target.Naptan = "test";
            target.NearestPointE = 1.0;
            target.NearestPointN = 1.0;
            target.NearestTOID = "test";
            target.Northing = 2.0;
            target.Easting = 2.0;
            target.ParentID = "testParent";
            target.Type = "LOCALITY";
            target.DisplayName = "testName";
            target.DATASETID = "test";
            
            Assert.AreEqual(1, target.ID);
            Assert.AreEqual("test", target.LocalityID);
            Assert.AreEqual("test", target.Name);
            Assert.AreEqual("test", target.Naptan);
            Assert.AreEqual(1.0, target.NearestPointE);
            Assert.AreEqual(1.0, target.NearestPointN);
            Assert.AreEqual("test", target.NearestTOID);
            Assert.AreEqual(2.0, target.Northing);
            Assert.AreEqual(2.0, target.Easting);
            Assert.AreEqual("testParent", target.ParentID);
            Assert.AreEqual("LOCALITY", target.Type);
            Assert.AreEqual("testName", target.DisplayName);
            Assert.AreEqual("test", target.DATASETID);


        }
    }
}
