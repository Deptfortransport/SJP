using SJP.WebService.CoordinateConvertorService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace SJP.TestProject
{

    /// <summary>
    ///This is a test class for CoordinateConverterLatitudeLongitudeTest and is intended
    ///to contain all CoordinateConverterLatitudeLongitudeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CoordinateConverterLatitudeLongitudeTest
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
        /// A test method for the LatitudeLongitude constructor
        /// </summary>
        [TestMethod()]
        public void LatitudeLongitudeConstructorTest()
        {
            LatitudeLongitude target = new LatitudeLongitude();
            Assert.IsNotNull(target, "LatitudeLongitude object not returned by constructor");
        }

        /// <summary>
        /// A test method for the LatitudeLongitude properties
        /// </summary>
        [TestMethod()]
        public void LatitudeLongitudePropertiesTest()
        {
            LatitudeLongitude target = new LatitudeLongitude();

            double latitude = 1234.56;
            target.Latitude = latitude;
            Assert.AreEqual(latitude, target.Latitude, "Latitude property not returning the correct value");

            double longitude = 3456.78;
            target.Longitude = longitude;
            Assert.AreEqual(longitude, target.Longitude, "Latitude property not returning the correct value");
        }
    }
}
