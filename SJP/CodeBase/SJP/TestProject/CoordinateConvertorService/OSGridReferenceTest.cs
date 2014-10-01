using SJP.WebService.CoordinateConvertorService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace SJP.TestProject
{

    /// <summary>
    ///This is a test class for CoordinateConverterOSGridReferenceTest and is intended
    ///to contain all CoordinateConverterOSGridReferenceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CoordinateConverterOSGridReferenceTest
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
        /// A test method for the OSGridReference constructor
        /// </summary>
        [TestMethod()]
        public void OSGridReferenceConstructorTest()
        {
            OSGridReference target = new OSGridReference();
            Assert.IsNotNull(target, "OSGridReference object not returned by constructor");
        }

        /// <summary>
        /// A test method for the OSGridReference properties
        /// </summary>
        [TestMethod()]
        public void OSGridReferencePropertiesTest()
        {
            OSGridReference target = new OSGridReference();

            int easting = 1234;
            target.Easting = easting;
            Assert.AreEqual(easting, target.Easting, "Easting property not returning the correct value");

            int northing = 3456;
            target.Northing = northing;
            Assert.AreEqual(northing, target.Northing, "Northing property not returning the correct value");
        }
    }
}
