// *********************************************** 
// NAME             : SJPVenueLocationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueLocation
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueLocationTest and is intended
    ///to contain all SJPVenueLocationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueLocationTest
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
        ///A test for SJPVenueLocation
        ///</summary>
        [TestMethod()]
        public void SJPVenueLocationClassTest()
        {
            string locationName = "Leicester Rail";  
            string locationDisplayName = "Leicester Rail Station";  
            string locationLocality = "E570563";  
            List<string> toids = new List<string>(new string[] { "4000042000"});
            List<string> naptans = new List<string>(new string[] { "9100LESTER" });   
            string locationParent = string.Empty;  
            SJPLocationType locationType = SJPLocationType.Station;  
            OSGridReference gridReference = new OSGridReference(567456,324512);
            OSGridReference cycleGridReference = new OSGridReference(567456.3F, 324512.12F); 
            bool isGNATStation = true;  
            bool useNaptan = false;
            int adminAreaID = 123;
            int districtID = 321;
            string sourceID = "TESTID";  
            SJPVenueLocation target = new SJPVenueLocation(locationName, locationDisplayName, locationLocality, toids, naptans, 
                locationParent, locationType, gridReference, cycleGridReference, isGNATStation, useNaptan, adminAreaID, districtID, sourceID);
            target.AccessibleNaptans = new List<string>(new string[] { "9100LESTER" });
            target.CycleToVenueDistance = 30;
            target.SelectedGridReference = new OSGridReference(567456, 324512);
            target.SelectedName = "Leicester Station";
            target.SelectedOutwardDateTime = new DateTime(2012, 07, 14, 09, 05, 00);
            target.SelectedReturnDateTime = new DateTime(2012, 07, 14, 17, 05, 00);
            target.SelectedPierNaptan = "9300TST";
            target.SelectedSJPParkID = "TESTPARK01";
            target.VenueGroupID = "VGOOL";
            target.VenueGroupName = "Out of London";
            target.VenueJourneyMode = SJPVenueJourneyMode.CyclePark;
            target.VenueMapUrl = "http://www.transportdirect.info";
            target.VenueTravelNewsRegion = "EastMidlands";
            target.VenueWalkingRoutesUrl = "http://www.transportdirect.info";

            Assert.AreEqual(naptans[0], target.ID);
            Assert.AreEqual(locationName, target.Name);
            Assert.AreEqual(locationDisplayName, target.DisplayName);
            Assert.AreEqual(locationLocality, target.Locality);
            Assert.AreEqual(1, target.Toid.Count);
            Assert.AreEqual(1, target.Naptan.Count);
            Assert.AreEqual(locationParent, target.Parent);
            Assert.AreEqual(SJPLocationType.Station, target.TypeOfLocation);
            Assert.AreEqual(567456, target.GridRef.Easting);
            Assert.AreEqual(324512, target.GridRef.Northing);
            Assert.AreEqual(true, target.GridRef.IsValid);
            Assert.IsTrue(target.IsGNAT);
            Assert.IsFalse(target.UseNaPTAN);
            Assert.AreEqual(adminAreaID, target.AdminAreaCode);
            Assert.AreEqual(districtID, target.DistrictCode);

            Assert.AreEqual("9100LESTER", target.AccessibleNaptans[0]);
            Assert.AreEqual(30, target.CycleToVenueDistance);
            Assert.AreEqual(567456, target.SelectedGridReference.Easting);
            Assert.AreEqual(324512, target.SelectedGridReference.Northing);
            Assert.AreEqual(true, target.SelectedGridReference.IsValid);
            Assert.AreEqual("Leicester Station", target.SelectedName);
            Assert.AreEqual(new DateTime(2012, 07, 14, 09, 05, 00), target.SelectedOutwardDateTime);
            Assert.AreEqual(new DateTime(2012, 07, 14, 17, 05, 00), target.SelectedReturnDateTime);
            Assert.AreEqual("9300TST", target.SelectedPierNaptan);
            Assert.AreEqual("TESTPARK01", target.SelectedSJPParkID);
            Assert.AreEqual("VGOOL", target.VenueGroupID);
            Assert.AreEqual("Out of London", target.VenueGroupName);
            Assert.AreEqual(SJPVenueJourneyMode.CyclePark, target.VenueJourneyMode);
            Assert.AreEqual("http://www.transportdirect.info", target.VenueMapUrl);
            Assert.AreEqual("EastMidlands", target.VenueTravelNewsRegion);
            Assert.AreEqual("http://www.transportdirect.info", target.VenueWalkingRoutesUrl);

            Assert.IsNotNull(target.ToString(true));
            Assert.IsNotNull(target.ToString(false));
            Assert.IsNotNull(target.GetSJPHashCode());
        }

       
    }
}
