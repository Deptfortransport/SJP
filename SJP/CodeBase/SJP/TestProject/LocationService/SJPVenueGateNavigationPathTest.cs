// *********************************************** 
// NAME             : SJPVenueGateNavigationPathTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueGateNavigationPath
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueGateNavigationPathTest and is intended
    ///to contain all SJPVenueGateNavigationPathTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueGateNavigationPathTest
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
        ///A test for SJPVenueGateNavigationPath Constructor
        ///</summary>
        [TestMethod()]
        public void SJPVenueGateNavigationPathConstructorTest()
        {
            SJPVenueGateNavigationPath target = new SJPVenueGateNavigationPath();
            target.FromNaPTAN = "8100TST1";
            target.ToNaPTAN = "8100TST2";
            target.GateNaPTAN = "8100TSTG01";
            target.NavigationPathID = "TID";
            target.NavigationPathName = "Test";
            target.TransferDistance = 90;
            target.TransferDuration = new TimeSpan(0, 13, 0);

            Assert.AreEqual("8100TST1", target.FromNaPTAN);
            Assert.AreEqual("8100TST2", target.ToNaPTAN);
            Assert.AreEqual("8100TSTG01", target.GateNaPTAN);
            Assert.AreEqual("TID", target.NavigationPathID);
            Assert.AreEqual("Test", target.NavigationPathName);
            Assert.AreEqual(90, target.TransferDistance);
            Assert.AreEqual(13, target.TransferDuration.Minutes);
        }

        
    }
}
