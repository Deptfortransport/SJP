using SJP.WebService.CoordinateConvertorService;
using SJP.Common.ServiceDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GIQ60Lib;
using System.Collections.Generic;


namespace SJP.TestProject
{

    /// <summary>
    ///This is a test class for CoordinateConvertorTest and is intended
    ///to contain all CoordinateConvertorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CoordinateConvertorWSTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

            SJPServiceDiscovery.Init(new TestInitialisation());
        }
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


        // Test cannot work asthe GIQ60Lib library needs to run in a 32 bit app pool
        // this functionality is tested by the other tests below (likewise for CoordinateConvertorInitialisation).
        ///// <summary>
        ///// A test method for the CoordinateConvertor constructor
        ///// </summary>
        //[TestMethod()]
        //public void CoordinateConvertorConstructorTest()
        //{
        //    CoordinateConvertor target = new CoordinateConvertor();
        //    Assert.IsNotNull(target, "CoordinateConvertor object not returned by constructor");
        //}

        /// <summary>
        /// A test method for the CoordinateConvertor properties
        /// </summary>
        [TestMethod()]
        public void ConvertLatitudeLongitudetoOSGRTest()
        {
            SJP.UserPortal.CoordinateConvertorProvider.CoordinateConvertor target = new SJP.UserPortal.CoordinateConvertorProvider.CoordinateConvertor();

            try
            {
                SJP.Common.LocationService.LatitudeLongitude latLong = new SJP.Common.LocationService.LatitudeLongitude(51.5005125781194, -0.670664930180879);

                SJP.Common.LocationService.LatitudeLongitude[] latLongs = new SJP.Common.LocationService.LatitudeLongitude[1] { latLong };
                SJP.Common.LocationService.OSGridReference[] osGRs = target.GetOSGridReference(latLongs);

                bool pass = (osGRs[0].Easting != 0) && (osGRs[0].Northing != 0);

                Assert.IsTrue(pass, "Non-zero values for easting and northing expected");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Access to the path"))
                {
                    Assert.Inconclusive("Access to the file CoordinateConvertorService.txt was denied, the CoordinateConvertorService may already have been started on another web process. Please stop these, delete the txt file, and re-run test");
                }
                else
                {
                    Assert.Fail(string.Format("Unexpected exception received: {0}", ex.ToString()));
                }
            }
            finally
            {
                target.Dispose();
            }
        }
        
        /// <summary>
        /// A test method for the CoordinateConvertor properties
        /// </summary>
        [TestMethod()]
        public void ConvertOSGRtoLatitudeLongitudeTest()
        {
            SJP.UserPortal.CoordinateConvertorProvider.CoordinateConvertor target = new SJP.UserPortal.CoordinateConvertorProvider.CoordinateConvertor();

            try
            {
                SJP.Common.LocationService.OSGridReference osGR = new SJP.Common.LocationService.OSGridReference();

                osGR.Easting = 492368;
                osGR.Northing = 178790;
                SJP.Common.LocationService.OSGridReference[] osGRs = new SJP.Common.LocationService.OSGridReference[1] { osGR };
                SJP.Common.LocationService.LatitudeLongitude[] latLongs = target.GetLatitudeLongitude(osGRs);

                bool pass = (latLongs[0].Latitude != 0) && (latLongs[0].Longitude != 0);

                Assert.IsTrue(pass, "Non-zero values for latitude and longitude expected");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Access to the path"))
                {
                    Assert.Inconclusive("Access to the file CoordinateConvertorService.txt was denied, the CoordinateConvertorService may already have been started on another web process. Please stop these, delete the txt file, and re-run test");
                }
                else
                {
                    Assert.Fail(string.Format("Unexpected exception received: {0}", ex.ToString()));
                }
            }
            finally
            {
                target.Dispose();
            }
        }

        /// <summary>
        /// A test method for the CoordinateConvertorService Convert
        /// </summary>
        [TestMethod()]
        public void ConvertCoordinatesUsingWebServiceTest()
        {
            try
            {
                // New instance of the Web Service CoordinateConvertor
                using (SJP.WebService.CoordinateConvertorService.CoordinateConvertor cc = new CoordinateConvertor())
                {
                    #region ConvertLatitudeLongitudetoOSGRUsingWebServiceTest

                    Assert.IsTrue(cc.TestActive());

                    SJP.WebService.CoordinateConvertorService.OSGridReference[] osgrs1 = null;

                    // Null test
                    try
                    {
                        osgrs1 = cc.ConvertLatitudeLongitudetoOSGR(null);
                        Assert.Fail("Exception was not thrown when invalid coordinate parameter supplied");
                    }
                    catch
                    {
                        // Exception should be thrown
                    }

                    SJP.WebService.CoordinateConvertorService.LatitudeLongitude latLong = new LatitudeLongitude();
                    latLong.Latitude = 53.081510614491009;
                    latLong.Longitude = -2.8206923400850492;

                    List<LatitudeLongitude> latLongs1 = new List<LatitudeLongitude>();

                    // No coordinates test
                    try
                    {
                        osgrs1 = cc.ConvertLatitudeLongitudetoOSGR(latLongs1.ToArray());
                        Assert.Fail("Exception was not thrown when invalid coordinate parameter supplied");
                    }
                    catch
                    {
                        // Exception should be thrown
                    }

                    latLongs1.Add(latLong);

                    // Coordinates test
                    osgrs1 = cc.ConvertLatitudeLongitudetoOSGR(latLongs1.ToArray());
                    Assert.IsNotNull(osgrs1);
                    Assert.IsTrue(osgrs1.Length > 0);
                    Assert.IsTrue(osgrs1[0].Easting != 0);
                    Assert.IsTrue(osgrs1[0].Northing != 0);

                    #endregion

                    #region ConvertOSGRtoLatitudeLongitudeUsingWebServiceTest

                    Assert.IsTrue(cc.TestActive());

                    SJP.WebService.CoordinateConvertorService.LatitudeLongitude[] latLongs2 = null;

                    // Null test
                    try
                    {
                        latLongs2 = cc.ConvertOSGRtoLatitudeLongitude(null);
                        Assert.Fail("Exception was not thrown when invalid coordinate parameter supplied");
                    }
                    catch
                    {
                        // Exception should be thrown
                    }

                    SJP.WebService.CoordinateConvertorService.OSGridReference osgr2 = new OSGridReference();
                    osgr2.Easting = 345123;
                    osgr2.Northing = 354123;

                    List<OSGridReference> osgrs2 = new List<OSGridReference>();

                    // No coordinates test
                    try
                    {
                        latLongs2 = cc.ConvertOSGRtoLatitudeLongitude(osgrs2.ToArray());
                        Assert.Fail("Exception was not thrown when invalid coordinate parameter supplied");
                    }
                    catch
                    {
                        // Exception should be thrown
                    }

                    osgrs2.Add(osgr2);

                    // Coordinates test
                    latLongs2 = cc.ConvertOSGRtoLatitudeLongitude(osgrs2.ToArray());
                    Assert.IsNotNull(latLongs2);
                    Assert.IsTrue(latLongs2.Length > 0);
                    Assert.IsTrue(latLongs2[0].Latitude != 0);
                    Assert.IsTrue(latLongs2[0].Longitude != 0);

                    #endregion

                    #region ConvertOSGRtoLatitudeLongitudeUsingWebServiceTest1

                    Assert.IsTrue(cc.TestActive());

                    SJP.WebService.CoordinateConvertorService.OSGridReference osgr3 = new OSGridReference();
                    osgr3.Easting = 345123;
                    osgr3.Northing = 354123;

                    SJP.WebService.CoordinateConvertorService.LatitudeLongitude latLong3 = cc.TestConvertOSGRToLatitudeLongitude(osgr3.Easting, osgr3.Northing);

                    Assert.IsNotNull(latLong3);
                    Assert.IsTrue(latLong3.Latitude != 0);
                    Assert.IsTrue(latLong3.Longitude != 0);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Access to the path"))
                {
                    Assert.Inconclusive("Access to the file CoordinateConvertorService.txt was denied, the CoordinateConvertorService may already have been started on another web process. Please stop these, delete the txt file, and re-run test");
                }
                else if ((ex.Message.Contains("COM")) || (ex.InnerException != null && ex.InnerException.Message.Contains("COM")))
                {
                    Assert.Inconclusive("COM exception encountered in CoordinateConvertorService. Please attempt running this test seperately");
                }
                else
                {
                    Assert.Fail(string.Format("Unexpected exception received: {0}", ex.ToString()));
                }
            }
        }
    }
}
