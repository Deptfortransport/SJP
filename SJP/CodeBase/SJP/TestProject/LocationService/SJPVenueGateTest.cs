// *********************************************** 
// NAME             : SJPVenueGateTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueGate
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueGateTest and is intended
    ///to contain all SJPVenueGateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueGateTest
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
        ///A test for SJPVenueGate 
        ///</summary>
        [TestMethod()]
        public void SJPVenueGateClassTest()
        {
            SJPVenueGate target = new SJPVenueGate();

            target.AvailableFrom = new DateTime(2012, 07, 07);
            target.AvailableTo = new DateTime(2012, 07, 28);
            target.GateGridRef = new OSGridReference(567434, 323532);
            target.GateName = "Gate";
            target.GateNaPTAN = "8100TSTG01";

            Assert.AreEqual(new DateTime(2012, 07, 07), target.AvailableFrom);
            Assert.AreEqual(new DateTime(2012, 07, 28), target.AvailableTo);
            Assert.AreEqual(567434, target.GateGridRef.Easting);
            Assert.AreEqual("Gate", target.GateName);
            Assert.AreEqual("8100TSTG01", target.GateNaPTAN);

        }

        
    }
}
