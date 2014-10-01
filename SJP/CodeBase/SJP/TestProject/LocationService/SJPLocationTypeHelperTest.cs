// *********************************************** 
// NAME             : SJPLocationTypeHelperTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: SJPLocationTypeHelperTest test class
// ************************************************
// 
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPLocationTypeHelperTest and is intended
    ///to contain all SJPLocationTypeHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPLocationTypeHelperTest
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
        ///A test for GetSJPLocationType
        ///</summary>
        [TestMethod()]
        public void GetSJPLocationTypeTest()
        {
            string dbLocationType = "COACH";
            SJPLocationType expected = SJPLocationType.Station;
            SJPLocationType actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "RAIL STATION";
            expected = SJPLocationType.Station;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "TMU";
            expected = SJPLocationType.Station;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "AIRPORT";
            expected = SJPLocationType.Station;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "FERRY";
            expected = SJPLocationType.Station;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "EXCHANGE GROUP";
            expected = SJPLocationType.StationGroup;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "LOCALITY";
            expected = SJPLocationType.Locality;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "VENUEPOI";
            expected = SJPLocationType.Venue;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "POSTCODE";
            expected = SJPLocationType.Postcode;
            actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));


            try
            {
                dbLocationType = "abcdef";

                actual = SJPLocationTypeHelper.GetSJPLocationType(dbLocationType);

                Assert.Fail("Expected exception to be thrown parsing an invalid location type");
            }
            catch
            {
                // Exception should be thrown, pass
            }
        }

        /// <summary>
        ///A test for GetSJPLocationTypeActual
        ///</summary>
        [TestMethod()]
        public void GetSJPLocationTypeActualTest()
        {
            string dbLocationType = "COACH";
            SJPLocationType expected = SJPLocationType.StationCoach;
            SJPLocationType actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "RAIL STATION";
            expected = SJPLocationType.StationRail;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "TMU";
            expected = SJPLocationType.StationTMU;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "AIRPORT";
            expected = SJPLocationType.StationAirport;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "FERRY";
            expected = SJPLocationType.StationFerry;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "EXCHANGE GROUP";
            expected = SJPLocationType.StationGroup;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "LOCALITY";
            expected = SJPLocationType.Locality;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "VENUEPOI";
            expected = SJPLocationType.Venue;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));

            dbLocationType = "POSTCODE";
            expected = SJPLocationType.Postcode;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", dbLocationType, expected));


            try
            {
                dbLocationType = "abcdef";

                actual = SJPLocationTypeHelper.GetSJPLocationTypeActual(dbLocationType);

                Assert.Fail("Expected exception to be thrown parsing an invalid location type");
            }
            catch
            {
                // Exception should be thrown, pass
            }
        }

        /// <summary>
        /// A test for GetSJPLocationTypeQS method
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void GetSJPLocationTypeQSTest()
        {
            string queryLocationType = "S";
            SJPLocationType expected = SJPLocationType.Station;
            SJPLocationType actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(queryLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", queryLocationType, expected));

            queryLocationType = "L";
            expected = SJPLocationType.Locality;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(queryLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", queryLocationType, expected));

            queryLocationType = "U";
            expected = SJPLocationType.Unknown;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(queryLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", queryLocationType, expected));

            queryLocationType = "V";
            expected = SJPLocationType.Venue;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(queryLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", queryLocationType, expected));

            queryLocationType = "SG";
            expected = SJPLocationType.StationGroup;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(queryLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", queryLocationType, expected));

            queryLocationType = "P";
            expected = SJPLocationType.Postcode;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(queryLocationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", queryLocationType, expected));

            // Exception thrown
            queryLocationType = "AB";
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(queryLocationType);

        }

        /// <summary>
        /// A test for GetSJPLocationTypeQS method to parse location type in to query string values
        /// </summary>
        [TestMethod()]
        public void GetSJPLocationTypeQS1Test()
        {
            string expected = "S";
            SJPLocationType locationType = SJPLocationType.Station;
            string actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(locationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", locationType, expected));

            expected = "L";
            locationType = SJPLocationType.Locality;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(locationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", locationType, expected));

            expected = "V";
            locationType = SJPLocationType.Venue;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(locationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", locationType, expected));

            expected = "SG";
            locationType = SJPLocationType.StationGroup;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(locationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", locationType, expected));

            expected = "P";
            locationType = SJPLocationType.Postcode;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(locationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", locationType, expected));

            expected = "U";
            locationType = SJPLocationType.Unknown;
            actual = SJPLocationTypeHelper.GetSJPLocationTypeQS(locationType);

            Assert.AreEqual(expected, actual, string.Format("Expected location type[{0}] to be parsed into [{1}]", locationType, expected));

        }
    }
}
