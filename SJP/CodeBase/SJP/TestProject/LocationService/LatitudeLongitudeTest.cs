// *********************************************** 
// NAME             : LatitudeLongitudeTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: LatitudeLongitudeTest test class
// ************************************************
// 
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for LatitudeLongitudeTest and is intended
    ///to contain all LatitudeLongitudeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LatitudeLongitudeTest
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
        ///A test for LatitudeLongitude Constructor
        ///</summary>
        [TestMethod()]
        public void LatitudeLongitudeConstructorTest()
        {
            LatitudeLongitude target = new LatitudeLongitude();

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");
        }

        /// <summary>
        ///A test for LatitudeLongitude Constructor
        ///</summary>
        [TestMethod()]
        public void LatitudeLongitudeConstructorTest1()
        {
            double latitude = 0F;
            double longitude = 0F;
            LatitudeLongitude target = new LatitudeLongitude(latitude, longitude);

            double valLat = target.Latitude;
            double valLon = target.Longitude;

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");
            Assert.IsTrue(latitude == valLat, "Expected Latitude to equal value added in constructor");
            Assert.IsTrue(longitude == valLon, "Expected Longitude to equal value added in constructor");

            target.Latitude = latitude + 1;
            target.Longitude = longitude - 1;

            Assert.IsTrue(target.Latitude == latitude + 1, "Expected LatitudeLongitude Easting to have been updated");
            Assert.IsTrue(target.Longitude == longitude - 1, "Expected LatitudeLongitude Northing to have been updated");
        }

        /// <summary>
        ///A test for LatitudeLongitude Constructor
        ///</summary>
        [TestMethod()]
        public void LatitudeLongitudeConstructorTest2()
        {
            string gridRef = null;
            LatitudeLongitude target = new LatitudeLongitude(gridRef);

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");

            gridRef = string.Empty;
            target = new LatitudeLongitude(gridRef);

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");

            gridRef = "0";
            target = new LatitudeLongitude(gridRef);

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");

            gridRef = "0,0";
            target = new LatitudeLongitude(gridRef);

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");

            gridRef = "a,b";
            target = new LatitudeLongitude(gridRef);

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");

            gridRef = "1,-1";
            target = new LatitudeLongitude(gridRef);

            Assert.IsNotNull(target, "Expected LatitudeLongitude object to not be null");

            gridRef = target.ToString();
            
            Assert.IsNotNull(target, "Expected LatitudeLongitude string to not be null");

            int hashcode = target.GetSJPHashCode();

            Assert.IsTrue(hashcode != 0, "Expected LatitudeLongitude hashcode to not be null");
        }
    }
}
