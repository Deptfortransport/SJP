// *********************************************** 
// NAME             : CoordinateConvertorTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: CoordinateConvertorTest class
// ************************************************
// 
                
using SJP.UserPortal.CoordinateConvertorProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using SJP.Common;

namespace SJP.TestProject
{
    
    /// <summary>
    ///This is a test class for CoordinateConvertorTest and is intended
    ///to contain all CoordinateConvertorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CoordinateConvertorTest
    {
        private TestContext testContextInstance;

        private object coordinateLock = new object();
        
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            SJPServiceDiscovery.Init(new TestInitialisation());
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
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
        ///A test for CoordinateConvertor Constructor
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorConstructorTest()
        {
            using (CoordinateConvertor target = new CoordinateConvertor())
            {
                Assert.IsNotNull(target, "Expected CoordinateConvertor to not be null");
            }
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.coordinateconvertorprovider.dll")]
        public void CoordinateConvertorDisposeTest()
        {
            CoordinateConvertor_Accessor target = new CoordinateConvertor_Accessor();
            bool disposing = true;
            target.Dispose(disposing);

            // Method that does not return a value cannot be verified
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorDisposeTest1()
        {
            CoordinateConvertor target = new CoordinateConvertor();
            target.Dispose();

            // Method that does not return a value cannot be verified
        }

        /// <summary>
        ///A test for Finalize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.coordinateconvertorprovider.dll")]
        public void CoordinateConvertorFinalizeTest()
        {
            using (CoordinateConvertor_Accessor target = new CoordinateConvertor_Accessor())
            {
                target.Finalize();
            }

            // Method that does not return a value cannot be verified
        }

        /// <summary>
        ///A test for GetLatitudeLongitude
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorGetLatitudeLongitudeTest()
        {
            lock (coordinateLock)
            {
                using (CoordinateConvertor target = new CoordinateConvertor())
                {
                    OSGridReference osgr = new OSGridReference(312345, 312345);

                    LatitudeLongitude actual = target.GetLatitudeLongitude(osgr);

                    Assert.IsNotNull(actual, "Expected LatitudeLongitude to have been returned");

                    bool valid = (actual.Latitude != 0) && (actual.Longitude != 0);

                    Assert.IsTrue(valid, "Expected LatitudeLongitude to contain non-zero values");
                }
            }
        }

        /// <summary>
        ///A test for GetLatitudeLongitude
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorGetLatitudeLongitudeTest1()
        {
            lock (coordinateLock)
            {
                using (CoordinateConvertor target = new CoordinateConvertor())
                {
                    OSGridReference[] osgrs = new OSGridReference[2];
                    osgrs[0] = new OSGridReference(312345, 312345);
                    osgrs[1] = new OSGridReference(322345, 322345);

                    LatitudeLongitude[] actual = target.GetLatitudeLongitude(osgrs);

                    Assert.IsNotNull(actual, "Expected LatitudeLongitudes to have been returned");

                    Assert.IsTrue(actual.Length > 0, "Expected at least one LatitudeLongitude to have been returned");

                    bool valid = (actual[0].Latitude != 0) && (actual[0].Longitude != 0)
                                && (actual[1].Latitude != 0) && (actual[1].Longitude != 0);

                    Assert.IsTrue(valid, "Expected all LatitudeLongitudes to contain non-zero values");
                }
            }
        }

        /// <summary>
        ///A test for GetLatitudeLongitude
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorGetLatitudeLongitudeErrorTest()
        {
            using (CoordinateConvertor target = new CoordinateConvertor())
            {
                OSGridReference osgr = new OSGridReference(-1, -1);

                try
                {
                    LatitudeLongitude actual = target.GetLatitudeLongitude(osgr);

                    Assert.Fail("Expected SJPException to have been thrown for test");
                }
                catch (SJPException ex)
                {
                    Assert.IsNotNull(ex, "Expected SJPException to have been thrown for test");
                }
          }
        }

        /// <summary>
        ///A test for GetOSGridReference
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorGetOSGridReferenceTest()
        {
            lock (coordinateLock)
            {
                using (CoordinateConvertor target = new CoordinateConvertor())
                {
                    LatitudeLongitude latlong = new LatitudeLongitude(53.0853736, -2.8157941);

                    OSGridReference actual = target.GetOSGridReference(latlong);

                    Assert.IsNotNull(actual, "Expected OSGridReference to have been returned");

                    bool valid = (actual.Easting != 0) && (actual.Northing != 0);

                    Assert.IsTrue(valid, "Expected OSGridReference to contain non-zero values");
                }
            }
        }

        /// <summary>
        ///A test for GetOSGridReference
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorGetOSGridReferenceTest1()
        {
            lock (coordinateLock)
            {
                using (CoordinateConvertor target = new CoordinateConvertor())
                {
                    LatitudeLongitude[] latlongs = new LatitudeLongitude[2];
                    latlongs[0] = new LatitudeLongitude(53.0853736, -2.8157941);
                    latlongs[1] = new LatitudeLongitude(52.7016695, -3.2986371);

                    OSGridReference[] actual = target.GetOSGridReference(latlongs);

                    Assert.IsNotNull(actual, "Expected OSGridReferences to have been returned");

                    Assert.IsTrue(actual.Length > 0, "Expected at least one OSGridReference to have been returned");

                    bool valid = (actual[0].Easting != 0) && (actual[0].Northing != 0)
                                && (actual[1].Easting != 0) && (actual[1].Northing != 0);

                    Assert.IsTrue(valid, "Expected all OSGridReferences to contain non-zero values");
                }
            }
        }

        /// <summary>
        ///A test for GetOSGridReference
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorGetOSGridReferenceErrorTest()
        {
            using (CoordinateConvertor target = new CoordinateConvertor())
            {
                LatitudeLongitude latlong = new LatitudeLongitude(0, 0);

                try
                {
                    OSGridReference actual = target.GetOSGridReference(latlong);
                 
                    Assert.Fail("Expected SJPException to have been thrown for test");
                }
                catch (SJPException ex)
                {
                    Assert.IsNotNull(ex, "Expected SJPException to have been thrown for test");
                }
            }
        }

        /// <summary>
        ///A test for LogCallEvent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.coordinateconvertorprovider.dll")]
        public void CoordinateConvertorLogCallEventTest()
        {
            using (CoordinateConvertor_Accessor target = new CoordinateConvertor_Accessor())
            {
                string webServiceMethod = "LogCallEventTest";
                DateTime callStartTime = DateTime.Now;
                bool successful = false;

                target.LogCallEvent(webServiceMethod, callStartTime, successful);
            }

            // Method that does not return a value cannot be verified
        }

        /// <summary>
        ///A test for LogException
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.coordinateconvertorprovider.dll")]
        public void CoordinateConvertorLogExceptionTest()
        {
            using (CoordinateConvertor_Accessor target = new CoordinateConvertor_Accessor())
            {
                string webServiceMethod = "LogExceptionTest";
                Exception ex = new Exception("LogExceptionTest");

                target.LogException(webServiceMethod, ex);
            }

            // Method that does not return a value cannot be verified
        }
    }
}
