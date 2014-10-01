// *********************************************** 
// NAME             : SJPLocationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPLocation
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TransportDirect.JourneyPlanning.CJPInterface;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPLocationTest and is intended
    ///to contain all SJPLocationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPLocationTest
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
        ///A test for SJPLocation Constructor
        ///</summary>
        [TestMethod()]
        public void SJPLocationConstructorTest()
        {
            string locationName = "Leicester Rail";  
            string locationDisplayName = "Leicester Rail Staion";  
            string locationLocality = "E456789";  
            List<string> toids = new List<string>(new string[]{ "420042004200"});
            List<string> naptans = new List<string>(new string[] { "9100LECSTER" });    
            string locationParent = string.Empty;
            SJPLocationType locationType = SJPLocationType.Station;
            SJPLocationType locationTypeActual = SJPLocationType.StationRail;  
            OSGridReference gridReference = new OSGridReference(564785,364732);
            OSGridReference cycleGridReference = new OSGridReference(564785.34F, 364732.43F);
            LatitudeLongitude latLong = new LatitudeLongitude(1, 1);
            bool isGNATStation = true;  
            bool useNaptan = false;
            int adminAreaID = 123;
            int districtID = 321;
            string sourceID = "Test";  
            SJPLocation target = new SJPLocation(locationName, locationDisplayName, locationLocality, toids, naptans, 
                locationParent, locationType, locationTypeActual, gridReference, cycleGridReference, isGNATStation, useNaptan, adminAreaID, districtID, sourceID);
            
            Assert.AreEqual(naptans[0], target.ID);
            Assert.AreEqual(locationName, target.Name);
            Assert.AreEqual(locationDisplayName, target.DisplayName);
            Assert.AreEqual(locationLocality, target.Locality);
            Assert.AreEqual(1, target.Toid.Count);
            Assert.AreEqual(1, target.Naptan.Count);
            Assert.AreEqual(locationParent, target.Parent);
            Assert.AreEqual(locationType, target.TypeOfLocation);
            Assert.AreEqual(locationTypeActual, target.TypeOfLocationActual);
            Assert.AreEqual(gridReference.Easting, target.GridRef.Easting);
            Assert.AreEqual(gridReference.Northing, target.GridRef.Northing);
            Assert.AreEqual(true, target.GridRef.IsValid);
            Assert.IsTrue(target.IsGNAT);
            Assert.AreEqual(adminAreaID, target.AdminAreaCode);
            Assert.AreEqual(districtID, target.DistrictCode);

            int hashCode = naptans[0].GetHashCode() ^ locationName.GetHashCode() ^ locationDisplayName.GetHashCode() ^
                locationLocality.GetHashCode() ^ locationParent.GetHashCode() ^ isGNATStation.GetHashCode() ^
                useNaptan.GetHashCode() ^ adminAreaID.GetHashCode() ^ districtID.GetHashCode() ^ sourceID.GetHashCode() ^ locationType.GetHashCode();

            // list object returns different instance hashcodes, so manually add
            foreach (string naptan in naptans)
            {
                hashCode = hashCode ^ naptan.GetHashCode();
            }
            foreach (string toid in toids)
            {
                hashCode = hashCode ^ toid.GetHashCode();
            }

            // OSGR object returns different instance hashcodes, so manually add
            hashCode = hashCode ^ gridReference.GetSJPHashCode() ^ cycleGridReference.GetSJPHashCode();

            Assert.AreEqual(hashCode, target.GetSJPHashCode());

            // Update test
            locationName = "Leicester Rail updated";
            locationDisplayName = "Leicester Rail Staion updated";
            locationLocality = "E123456";
            toids = new List<string>(new string[] { "123456" });
            naptans = new List<string>(new string[] { "9100XXXXXX" });
            locationParent = "123456";
            locationType = SJPLocationType.StationAirport;
            locationTypeActual = SJPLocationType.StationAirport;
            gridReference = new OSGridReference(123456, 123456);
            cycleGridReference = new OSGridReference(123456.34F, 123456.43F);
            latLong = new LatitudeLongitude(2, 2);
            isGNATStation = false;
            useNaptan = true;
            adminAreaID = 789;
            districtID = 987;
            sourceID = "Test updated";

            target.Name = locationName;
            target.DisplayName = locationDisplayName;
            target.Locality = locationLocality;
            target.Toid = toids;
            target.Naptan = naptans;
            target.Parent = locationParent;
            target.TypeOfLocation = locationType;
            target.TypeOfLocationActual = locationTypeActual;
            target.GridRef = gridReference;
            target.CycleGridRef = cycleGridReference;
            target.LatitudeLongitudeCoordinate = latLong;
            target.IsGNAT = isGNATStation;
            target.UseNaPTAN = useNaptan;
            target.AdminAreaCode = adminAreaID;
            target.DistrictCode = districtID;
            target.DataSetID = sourceID;

            Assert.AreEqual(locationName, target.Name);
            Assert.AreEqual(locationDisplayName, target.DisplayName);
            Assert.AreEqual(locationLocality, target.Locality);
            Assert.AreEqual(1, target.Toid.Count);
            Assert.AreEqual(1, target.Naptan.Count);
            Assert.AreEqual(locationParent, target.Parent);
            Assert.AreEqual(locationType, target.TypeOfLocation);
            Assert.AreEqual(locationTypeActual, target.TypeOfLocationActual);
            Assert.AreEqual(gridReference.Easting, target.GridRef.Easting);
            Assert.AreEqual(gridReference.Northing, target.GridRef.Northing);
            Assert.AreEqual(latLong.Latitude, target.LatitudeLongitudeCoordinate.Latitude);
            Assert.AreEqual(latLong.Longitude, target.LatitudeLongitudeCoordinate.Longitude);
            Assert.AreEqual(true, target.GridRef.IsValid);
            Assert.AreEqual(isGNATStation, target.IsGNAT);
            Assert.AreEqual(adminAreaID, target.AdminAreaCode);
            Assert.AreEqual(districtID, target.DistrictCode);

            // Default test - for code coverage
            locationType = SJPLocationType.Unknown;
            target = new SJPLocation(locationName, locationDisplayName, locationLocality, toids, naptans,
                locationParent, locationType, locationTypeActual, gridReference, cycleGridReference, isGNATStation, useNaptan, adminAreaID, districtID, sourceID);
        
            Assert.AreEqual(locationType, target.TypeOfLocation);

            // UseNaptan test - for code coverage
            locationType = SJPLocationType.Venue;
            target = new SJPLocation(locationName, locationDisplayName, locationLocality, toids, naptans,
                locationParent, locationType, locationTypeActual, gridReference, cycleGridReference, isGNATStation, useNaptan, adminAreaID, districtID, sourceID);

            Assert.IsTrue(target.UseNaPTAN);

        }

        /// <summary>
        ///A test for SJPLocation Constructor
        ///</summary>
        [TestMethod()]
        public void SJPLocationConstructorTest1()
        {
            Event cjpLegEvent = new Event() ;
            cjpLegEvent.stop = new Stop();
            cjpLegEvent.stop.name = "test";
            cjpLegEvent.stop.NaPTANID = "9100TEST";
            cjpLegEvent.stop.coordinate = new Coordinate();
            cjpLegEvent.stop.coordinate.easting = 567812;
            cjpLegEvent.stop.coordinate.northing = 323216;
            
            SJPLocation target = new SJPLocation(cjpLegEvent);
            
            Assert.AreEqual(cjpLegEvent.stop.name, target.Name);
            Assert.AreEqual(cjpLegEvent.stop.NaPTANID, target.Naptan[0]);
            Assert.AreEqual(cjpLegEvent.stop.coordinate.easting, target.GridRef.Easting);

            // Null test
            target = new SJPLocation(null);
            Assert.AreEqual(SJPLocationType.Unknown, target.TypeOfLocation);
            
            cjpLegEvent = new Event();
            cjpLegEvent.stop = null;
            target = new SJPLocation(cjpLegEvent);
            Assert.AreEqual(SJPLocationType.Unknown, target.TypeOfLocation);

            // No stop name test
            // No naptan test
            cjpLegEvent.stop = new Stop();
            cjpLegEvent.stop.name = string.Empty;
            cjpLegEvent.stop.NaPTANID = string.Empty;
            target = new SJPLocation(cjpLegEvent);
            
            Assert.AreEqual(cjpLegEvent.stop.name, target.Name);
            Assert.AreEqual(0, target.Naptan.Count);

            // Origin/Destination naptan test
            cjpLegEvent.stop.NaPTANID = SJPLocation_Accessor.ORIGIN_NAPTAN;
            target = new SJPLocation(cjpLegEvent);
            Assert.AreEqual(0, target.Naptan.Count);

            cjpLegEvent.stop.NaPTANID = SJPLocation_Accessor.DESTINATION_NAPTAN;
            target = new SJPLocation(cjpLegEvent);
            Assert.AreEqual(0, target.Naptan.Count);
        }

        /// <summary>
        ///A test for SJPLocation Constructor - Empty default constructor
        ///</summary>
        [TestMethod()]
        public void SJPLocationConstructorTest2()
        {
            SJPLocation target = new SJPLocation();

            Assert.AreEqual(string.Empty,target.ID);
            Assert.AreEqual(string.Empty, target.Name);
            Assert.AreEqual(string.Empty, target.DisplayName);
            Assert.AreEqual(string.Empty, target.Locality);
            Assert.AreEqual(0, target.Toid.Count);
            Assert.AreEqual(0, target.Naptan.Count);
            Assert.AreEqual(string.Empty, target.Parent);
            Assert.AreEqual(SJPLocationType.Unknown, target.TypeOfLocation);
            Assert.AreEqual(0, target.GridRef.Easting);
            Assert.AreEqual(0, target.GridRef.Northing);
            Assert.AreEqual(false, target.GridRef.IsValid);
            Assert.AreEqual(0, target.CycleGridRef.Easting);
            Assert.AreEqual(0, target.CycleGridRef.Northing);
            Assert.AreEqual(false, target.CycleGridRef.IsValid);
            Assert.AreEqual(0, target.LatitudeLongitudeCoordinate.Latitude);
            Assert.AreEqual(0, target.LatitudeLongitudeCoordinate.Longitude);
            Assert.IsFalse(target.IsGNAT);
            Assert.IsFalse(target.UseNaPTAN);
            Assert.AreEqual(string.Empty, target.DataSetID);

        }

        /// <summary>
        ///A test for SJPLocation Constructor
        ///</summary>
        [TestMethod()]
        public void SJPLocationConstructorTest3()
        {
            string locationDisplayName = "Leicester";  
            SJPLocationType locationType = SJPLocationType.Locality;  
            string identifier = "E475689";
            SJPLocation target = new SJPLocation(locationDisplayName, locationType, locationType, identifier);
            Assert.AreEqual(locationDisplayName, target.DisplayName);
            Assert.AreEqual(locationType, target.TypeOfLocation);
            Assert.AreEqual(identifier, target.ID);

            // Postcode
            locationDisplayName = "LE1 1AB";
            locationType = SJPLocationType.Postcode;
            identifier = "LE11AB";
            target = new SJPLocation(locationDisplayName, locationType, locationType, identifier);
            Assert.AreEqual(locationDisplayName, target.DisplayName);
            Assert.AreEqual(locationType, target.TypeOfLocation);
            Assert.AreEqual(identifier, target.ID);

            // Station Group
            locationDisplayName = "London";
            locationType = SJPLocationType.StationGroup;
            identifier = "G1";
            target = new SJPLocation(locationDisplayName, locationType, locationType, identifier);
            Assert.AreEqual(locationDisplayName, target.DisplayName);
            Assert.AreEqual(locationType, target.TypeOfLocation);
            Assert.AreEqual(identifier, target.ID);

            // Default test - for coverage
            locationType = SJPLocationType.Unknown;
            target = new SJPLocation(locationDisplayName, locationType, locationType, identifier);
            Assert.AreEqual(locationType, target.TypeOfLocation);
        }

                        
        
    }
}
