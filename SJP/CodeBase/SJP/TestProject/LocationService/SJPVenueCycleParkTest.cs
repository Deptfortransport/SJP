// *********************************************** 
// NAME             : SJPVenueCycleParkTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueCyclePark
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueCycleParkTest and is intended
    ///to contain all SJPVenueCycleParkTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueCycleParkTest
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
        ///A test for SJPVenueCyclePark
        ///</summary>
        [TestMethod()]
        public void SJPVenueCycleParkClassTest()
        {
            SJPVenueCyclePark target = new SJPVenueCyclePark();

            target.Availability = new List<SJPParkAvailability>();

            DateTime fromDate = new DateTime(2012, 07, 1);
            DateTime toDate = new DateTime(2012, 09, 14);
            TimeSpan dailyOpeningTime = new TimeSpan(8, 55, 53);
            TimeSpan dailyClosingTime = new TimeSpan(18, 30, 30);
            DaysOfWeek daysOpen = DaysOfWeek.Weekday;
            SJPParkAvailability availability = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);

            target.Availability.Add(availability);

            target.CycleFromGridReference = new OSGridReference(564556, 342344);
            target.CycleParkMapUrl = "http://www.transportdirect.info";
            target.CycleToGridReference = new OSGridReference(564540, 342323);
            target.ID = "Test";
            target.Name = "TestCyclePark";
            target.NumberOfSpaces = 30;
            target.StorageType = CycleStorageType.Lockers;
            target.VenueGateEntranceNaPTAN = "8100TSTCY01";
            target.VenueGateExitNaPTAN = "8100TSTCY02";
            target.VenueNaPTAN = "8100TST";
            target.WalkFromGateDuration = new TimeSpan(0,15,0);
            target.WalkToGateDuration = new TimeSpan(0,5,0);

            Assert.AreEqual(564556, target.CycleFromGridReference.Easting);
            Assert.AreEqual(342344, target.CycleFromGridReference.Northing);
            Assert.AreEqual("http://www.transportdirect.info", target.CycleParkMapUrl);
            Assert.AreEqual(564540, target.CycleToGridReference.Easting);
            Assert.AreEqual(342323, target.CycleToGridReference.Northing);
            Assert.AreEqual("Test", target.ID);
            Assert.AreEqual("TestCyclePark", target.Name);
            Assert.AreEqual(30, target.NumberOfSpaces);
            Assert.AreEqual(CycleStorageType.Lockers, target.StorageType);
            Assert.AreEqual("8100TSTCY01", target.VenueGateEntranceNaPTAN);
            Assert.AreEqual("8100TSTCY02", target.VenueGateExitNaPTAN);
            Assert.AreEqual("8100TST", target.VenueNaPTAN);
            Assert.AreEqual(15, target.WalkFromGateDuration.Minutes);
            Assert.AreEqual(5, target.WalkToGateDuration.Minutes);

            // Cycle park is open for the time requested
            Assert.IsTrue(target.IsOpenForTime(new DateTime(2012, 07, 13, 13, 14, 14)));

            // Cycle par is closed for the time requested
            Assert.IsFalse(target.IsOpenForTime(new DateTime(2012, 07, 13, 19, 14, 14)));

        }

    }
}
