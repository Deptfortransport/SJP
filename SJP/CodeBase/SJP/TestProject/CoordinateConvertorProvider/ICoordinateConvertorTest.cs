// *********************************************** 
// NAME             : ICoordinateConvertorTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: ICoordinateConvertorTest class
// ************************************************
// 
                
using SJP.UserPortal.CoordinateConvertorProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.TestProject;

namespace SJP.TestProject
{
    /// <summary>
    ///This is a test class for ICoordinateConvertorTest and is intended
    ///to contain all ICoordinateConvertorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ICoordinateConvertorTest
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


        internal virtual ICoordinateConvertor CreateICoordinateConvertor()
        {
            // Create a mock coordinate convertor
            ICoordinateConvertor target = new MockCoordinateConvertor();
            return target;
        }

        /// <summary>
        ///A test for GetLatitudeLongitude
        ///</summary>
        [TestMethod()]
        public void ICoordinateConvertorGetLatitudeLongitudeTest()
        {
            ICoordinateConvertor target = CreateICoordinateConvertor();
            OSGridReference[] osgrs = new OSGridReference[0];
            LatitudeLongitude[] expected = new LatitudeLongitude[0];
            LatitudeLongitude[] actual;
            actual = target.GetLatitudeLongitude(osgrs);

            // Mock class doesnt return valid data, just check object was returned
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for GetLatitudeLongitude
        ///</summary>
        [TestMethod()]
        public void ICoordinateConvertorGetLatitudeLongitudeTest1()
        {
            ICoordinateConvertor target = CreateICoordinateConvertor();
            OSGridReference osgr = new OSGridReference();
            LatitudeLongitude expected = new LatitudeLongitude();
            LatitudeLongitude actual;
            actual = target.GetLatitudeLongitude(osgr);

            // Mock class doesnt return valid data, just check object was returned
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for GetOSGridReference
        ///</summary>
        [TestMethod()]
        public void ICoordinateConvertorGetOSGridReferenceTest()
        {
            ICoordinateConvertor target = CreateICoordinateConvertor();
            LatitudeLongitude[] latlongs = new LatitudeLongitude[0];
            OSGridReference[] expected = new OSGridReference[0];
            OSGridReference[] actual;
            actual = target.GetOSGridReference(latlongs);
            
            // Mock class doesnt return valid data, just check object was returned
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for GetOSGridReference
        ///</summary>
        [TestMethod()]
        public void ICoordinateConvertorGetOSGridReferenceTest1()
        {
            ICoordinateConvertor target = CreateICoordinateConvertor();
            LatitudeLongitude latlong = new LatitudeLongitude();
            OSGridReference expected = new OSGridReference();
            OSGridReference actual;
            actual = target.GetOSGridReference(latlong);

            // Mock class doesnt return valid data, just check object was returned
            Assert.IsNotNull(actual);
        }
    }
}
