// *********************************************** 
// NAME             : SJPParkAvailabilityTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPParkAvailability
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPParkAvailabilityTest and is intended
    ///to contain all SJPParkAvailabilityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPParkAvailabilityTest
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
        ///A test for SJPParkAvailability Constructor
        ///</summary>
        [TestMethod()]
        public void SJPParkAvailabilityConstructorTest()
        {
            DateTime fromDate = new DateTime(2012,07,1);  
            DateTime toDate = new DateTime(2012,09,14);  
            TimeSpan dailyOpeningTime = new TimeSpan(8,55,53);  
            TimeSpan dailyClosingTime = new TimeSpan(18,30,30);  
            DaysOfWeek daysOpen = DaysOfWeek.Everyday;  
            SJPParkAvailability target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);

            Assert.AreEqual(fromDate, target.FromDate);
            Assert.AreEqual(toDate, target.ToDate);
            Assert.AreEqual(dailyOpeningTime, target.DailyOpeningTime);
            Assert.AreEqual(dailyClosingTime, target.DailyClosingTime);
            Assert.AreEqual(daysOpen, target.DaysOpen);

            // Update tests
            fromDate = new DateTime(2012, 07, 10);
            toDate = new DateTime(2012, 09, 24);
            dailyOpeningTime = new TimeSpan(10, 00, 00);
            dailyClosingTime = new TimeSpan(18, 00, 00);
            daysOpen = DaysOfWeek.Everyday;
            
            target.FromDate = fromDate;
            target.ToDate = toDate;
            target.DailyOpeningTime = dailyOpeningTime;
            target.DailyClosingTime = dailyClosingTime;
            target.DaysOpen = daysOpen;

            Assert.AreEqual(fromDate, target.FromDate);
            Assert.AreEqual(toDate, target.ToDate);
            Assert.AreEqual(dailyOpeningTime, target.DailyOpeningTime);
            Assert.AreEqual(dailyClosingTime, target.DailyClosingTime);
            Assert.AreEqual(daysOpen, target.DaysOpen);
        }

        /// <summary>
        ///A test for GetDaysOfWeek
        ///</summary>
        [TestMethod()]
        public void GetDaysOfWeekTest()
        {
            DateTime fromDate = new DateTime(2012, 07, 1);
            DateTime toDate = new DateTime(2012, 09, 14);
            TimeSpan dailyOpeningTime = new TimeSpan(8, 55, 53);
            TimeSpan dailyClosingTime = new TimeSpan(18, 30, 30);
            DaysOfWeek daysOpen = DaysOfWeek.Weekday;
            SJPParkAvailability target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            
            List<DayOfWeek> actual;
            actual = target.GetDaysOfWeek();
            Assert.AreEqual(5, actual.Count);
            Assert.IsTrue(actual.Contains(DayOfWeek.Tuesday));
            Assert.IsFalse(actual.Contains(DayOfWeek.Saturday));

            daysOpen = DaysOfWeek.Monday;
            target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            actual = target.GetDaysOfWeek();
            Assert.IsTrue(actual.Contains(DayOfWeek.Monday));

            daysOpen = DaysOfWeek.Tuesday;
            target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            actual = target.GetDaysOfWeek();
            Assert.IsTrue(actual.Contains(DayOfWeek.Tuesday));

            daysOpen = DaysOfWeek.Wednesday;
            target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            actual = target.GetDaysOfWeek();
            Assert.IsTrue(actual.Contains(DayOfWeek.Wednesday));

            daysOpen = DaysOfWeek.Thursday;
            target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            actual = target.GetDaysOfWeek();
            Assert.IsTrue(actual.Contains(DayOfWeek.Thursday));

            daysOpen = DaysOfWeek.Friday;
            target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            actual = target.GetDaysOfWeek();
            Assert.IsTrue(actual.Contains(DayOfWeek.Friday));

            daysOpen = DaysOfWeek.Saturday;
            target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            actual = target.GetDaysOfWeek();
            Assert.IsTrue(actual.Contains(DayOfWeek.Saturday));

            daysOpen = DaysOfWeek.Sunday;
            target = new SJPParkAvailability(fromDate, toDate, dailyOpeningTime, dailyClosingTime, daysOpen);
            actual = target.GetDaysOfWeek();
            Assert.IsTrue(actual.Contains(DayOfWeek.Sunday));

            
        }

        
    }
}
