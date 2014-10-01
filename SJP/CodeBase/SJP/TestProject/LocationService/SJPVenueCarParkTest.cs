// *********************************************** 
// NAME             : SJPVenueCarParkTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueCarPark
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueCarParkTest and is intended
    ///to contain all SJPVenueCarParkTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueCarParkTest
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
        ///A test for SJPVenueCarPark Constructor
        ///</summary>
        [TestMethod()]
        public void SJPVenueCarParkConstructorTest()
        {
            SJPVenueCarPark target = new SJPVenueCarPark();
            target.BlueBadgeSpaces = 34;
            target.CarSpaces = 120;
            target.CoachSpaces = 3;
            target.DisabledSpaces = 32;
            target.DriveFromToid = "4000042000";
            target.DriveToToid = "4000043000";
            target.ID = "Test";
            target.InterchangeDuration = 30;
            target.MapOfSiteUrl = "http://www.transportdirect.info";
            target.Name = "TestCarPark";
            target.VenueNaPTAN = "8100TST";
            target.Availability = new List<SJPParkAvailability>();


            DateTime fromDate = new DateTime(2012, 07, 1);
            DateTime toDate = new DateTime(2012, 09, 14);
            TimeSpan dailyOpeningTime = new TimeSpan(8, 55, 53);
            TimeSpan dailyClosingTime = new TimeSpan(18, 30, 30);
            DaysOfWeek daysOpen = DaysOfWeek.Weekday;
            SJPParkAvailability availability = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);

            target.Availability.Add(availability);

            TransitShuttle shuttle = new TransitShuttle();
            shuttle.ID = "TestShuttle";

            target.TransitShuttles = new List<TransitShuttle>();
            target.TransitShuttles.Add(shuttle);

            Assert.AreEqual(34, target.BlueBadgeSpaces);
            Assert.AreEqual(120, target.CarSpaces);
            Assert.AreEqual(3, target.CoachSpaces);
            Assert.AreEqual(32, target.DisabledSpaces);
            Assert.AreEqual("4000042000", target.DriveFromToid);
            Assert.AreEqual("4000043000", target.DriveToToid);
            Assert.AreEqual("Test", target.ID);
            Assert.AreEqual(30, target.InterchangeDuration);
            Assert.AreEqual("http://www.transportdirect.info", target.MapOfSiteUrl);
            Assert.AreEqual("TestCarPark", target.Name);
            Assert.AreEqual("8100TST", target.VenueNaPTAN);
            Assert.AreEqual(1, target.TransitShuttles.Count);
            Assert.AreEqual("TestShuttle", target.TransitShuttles[0].ID);

            // Car park is open for the time requested
            Assert.IsTrue(target.IsOpenForTime(new DateTime(2012, 07, 13, 13, 14,14)));

            // Car par is closed for the time requested
            Assert.IsTrue(target.IsOpenForTime(new DateTime(2012, 07, 14, 14, 14,14)));
        }

    }
}
