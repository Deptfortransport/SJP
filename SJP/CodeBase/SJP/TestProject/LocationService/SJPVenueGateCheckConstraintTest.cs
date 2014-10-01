// *********************************************** 
// NAME             : SJPVenueGateCheckConstraintTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueGateCheckConstraint
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueGateCheckConstraintTest and is intended
    ///to contain all SJPVenueGateCheckConstraintTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueGateCheckConstraintTest
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
        ///A test for SJPVenueGateCheckConstraint 
        ///</summary>
        [TestMethod()]
        public void SJPVenueGateCheckConstraintClassTest()
        {
            SJPVenueGateCheckConstraint target = new SJPVenueGateCheckConstraint();
            target.AverageDelay = new TimeSpan(0, 12, 0);
            target.CheckConstraintID = "TID";
            target.CheckConstraintName = "Test";
            target.Congestion = "not available";
            target.GateNaPTAN = "8100TSTG01";
            target.IsEntry = true;
            target.Process = "Test Process";

            Assert.AreEqual("TID", target.CheckConstraintID);
            Assert.AreEqual("Test", target.CheckConstraintName);
            Assert.AreEqual("not available", target.Congestion);
            Assert.AreEqual("8100TSTG01", target.GateNaPTAN);
            Assert.IsTrue(target.IsEntry);
            Assert.AreEqual("Test Process", target.Process);
            Assert.AreEqual(12, target.AverageDelay.Minutes);
        }

        


    }
}
