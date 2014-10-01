// *********************************************** 
// NAME             : SJPVenueRiverServiceTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueRiverService
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueRiverServiceTest and is intended
    ///to contain all SJPVenueRiverServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueRiverServiceTest
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
        ///A test for SJPVenueRiverService
        ///</summary>
        [TestMethod()]
        public void SJPVenueRiverServiceClassTest()
        {
            SJPVenueRiverService target = new SJPVenueRiverService();
            target.VenueNaPTAN = "8100TST";
            target.VenuePierName = "TestVenuePier";
            target.VenuePierNaPTAN = "9300TST1";
            target.RemotePierName = "TestRemotePier";
            target.RemotePierNaPTAN = "9300TST2";

            Assert.AreEqual("8100TST", target.VenueNaPTAN);
            Assert.AreEqual("TestVenuePier", target.VenuePierName);
            Assert.AreEqual("9300TST1", target.VenuePierNaPTAN);
            Assert.AreEqual("TestRemotePier", target.RemotePierName);
            Assert.AreEqual("9300TST2", target.RemotePierNaPTAN);
        }

       
    }
}
