// *********************************************** 
// NAME             : TravelcardCatalogueTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 19 Jan 2012
// DESCRIPTION  	: TravelcardCatalogueTest test class
// ************************************************

using SJP.UserPortal.Retail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using SJP.Common.ServiceDiscovery;
using System.Collections.Generic;
using System.Drawing;
using SJP.Common;
using SJP.Common.LocationService;
using SJP.Common.DatabaseInfrastructure;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for TravelcardCatalogueTest and is intended
    ///to contain all TravelcardCatalogueTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TravelcardCatalogueTest
    {

        private static TestDataManager testDataManager;

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
            string test_data = @"Retail\TravelcardData.xml";
            string setup_script = @"Retail\TravelcardTestSetup.sql";
            string clearup_script = @"Retail\TravelcardTestCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPTransientPortal;Trusted_Connection=true";

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.TransientPortalDB);
            testDataManager.Setup();

            testDataManager.LoadData(false);

        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            testDataManager.ClearData();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            //SJPServiceDiscovery.Init(new TestInitialisation());
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
            
        //}
        
        #endregion


        /// <summary>
        ///A test for TravelcardCatalogue Constructor
        ///</summary>
        [TestMethod()]
        public void TravelcardCatalogueConstructorTest()
        {
            TravelcardCatalogue target = new TravelcardCatalogue();

            Assert.IsNotNull(target, "Expected TravelcardCatalogue object to be created");
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard
        ///</summary>
        [TestMethod()]
        public void TravelcardCatalogueHasTravelcardTest()
        {
            // NOTE: Data below is tested against test data held in TravelcardData.xml

            TravelcardCatalogue target = new TravelcardCatalogue();
            
            JourneyLeg journeyLeg = null;
            
            bool actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for null leg");

            // Leg
            OSGridReference osgrZone1a = new OSGridReference(100005, 100005);
            OSGridReference osgrZone1b = new OSGridReference(100009, 100009);
            OSGridReference osgrZone2a = new OSGridReference(200005, 100005);
            OSGridReference osgrNoZone = new OSGridReference(100000, 900000);
            string naptanZone1a = "9100TEST1";
            string naptanZone1b = "9100TEST2";
            string naptanNoZone = "9100TEST999";
            string naptanRoute1a = "9100TEST5";
            string naptanRoute1b = "9100TEST6";
            string naptanRoute2a = "9100TEST7";
            string naptanRoute2b = "9100TEST8";

            SJPLocation startLocation = new SJPLocation();
            startLocation.GridRef = osgrZone1a; // Included zone coordinate
            startLocation.Naptan.Add(naptanZone1a); // Included zone naptan

            SJPLocation endLocation = new SJPLocation();
            endLocation.GridRef = osgrZone1b; // Included zone coordinate
            endLocation.Naptan.Add(naptanZone1b); // Included zone naptan

            // Default
            journeyLeg = new JourneyLeg();
            journeyLeg.Mode = SJPModeType.Walk; // Invalid mode (walks are ignored)
            journeyLeg.LegStart = new JourneyCallingPoint();
            journeyLeg.LegStart.Location = startLocation;
            journeyLeg.LegStart.ArrivalDateTime = DateTime.MinValue;
            journeyLeg.LegStart.DepartureDateTime = DateTime.MinValue; // Date outside of travelcard valid from
            journeyLeg.LegStart.Type = JourneyCallingPointType.OriginAndBoard;
            journeyLeg.LegEnd = new JourneyCallingPoint();
            journeyLeg.LegEnd.Location = endLocation;
            journeyLeg.LegEnd.ArrivalDateTime = DateTime.MinValue; // Date outside of travelcard valid to
            journeyLeg.LegEnd.DepartureDateTime = DateTime.MinValue;
            journeyLeg.LegEnd.Type = JourneyCallingPointType.DestinationAndAlight;

            // Set no location
            journeyLeg.LegStart.Location = null;
            journeyLeg.LegEnd.Location = endLocation;
            
            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for nul locations");

            // Set no location
            journeyLeg.LegStart.Location = startLocation;
            journeyLeg.LegEnd.Location = null;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for nul locations");
            
            // Set no location
            journeyLeg.LegStart.Location = null;
            journeyLeg.LegEnd.Location = null;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for nul locations");

            // Set location
            journeyLeg.LegStart.Location = startLocation;
            journeyLeg.LegEnd.Location = endLocation;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for walk leg");

            // Set valid mode
            journeyLeg.Mode = SJPModeType.Underground;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for outside valid travelcard date leg");

            // Set valid date
            journeyLeg.LegStart.DepartureDateTime = DateTime.Now;
            journeyLeg.LegEnd.ArrivalDateTime = DateTime.Now.AddHours(1);

            actual = target.HasTravelcard(journeyLeg); // Leg locations should be in zone (coordinate and stop)
            Assert.AreEqual(true, actual, "Expected HasTravelcard to be true for underground leg");

            // Set end location to be in a different zone
            endLocation.GridRef = osgrZone2a;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(true, actual, "Expected HasTravelcard to be true for locations in different zones");

            // Set end location osgr to be outside zones
            endLocation.GridRef = osgrNoZone;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(true, actual, "Expected HasTravelcard to be true for location outside zones but naptan in zone");

            // Set end location osgr and naptan to be outside zones
            endLocation.GridRef = osgrNoZone;
            endLocation.Naptan.Clear();
            endLocation.Naptan.Add(naptanNoZone);

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for location outside zones");

            // Set start location osgr and naptan to be outside zones
            startLocation.GridRef = osgrNoZone;
            startLocation.Naptan.Clear();
            startLocation.Naptan.Add(naptanNoZone);

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for locations outside zones");

            // Set locations to outside zone but in a route
            startLocation.Naptan.Clear();
            startLocation.Naptan.Add(naptanRoute1a);
            endLocation.Naptan.Clear();
            endLocation.Naptan.Add(naptanRoute1b);
            journeyLeg.Mode = SJPModeType.Bus;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(true, actual, "Expected HasTravelcard to be true for locations on route");

            // Set locations to outside zone but in a route (opposite direction)
            startLocation.Naptan.Clear();
            startLocation.Naptan.Add(naptanRoute1b);
            endLocation.Naptan.Clear();
            endLocation.Naptan.Add(naptanRoute1a);
            
            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(true, actual, "Expected HasTravelcard to be true for locations on route");

            // Set locations to outside zone but in a route but for an exlcuded mode
            journeyLeg.Mode = SJPModeType.Tram;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for locations on route excluded mode");

            // Set locations to outside zone but in an excluded route
            startLocation.Naptan.Clear();
            startLocation.Naptan.Add(naptanRoute2a);
            endLocation.Naptan.Clear();
            endLocation.Naptan.Add(naptanRoute2b);
            journeyLeg.Mode = SJPModeType.Bus;

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for locations on excluded route");

            
            // Reset journey leg...
            startLocation.GridRef = osgrZone1a;
            startLocation.Naptan.Clear();
            startLocation.Naptan.Add(naptanZone1a);
            endLocation.GridRef = osgrZone1b;
            endLocation.Naptan.Clear();
            endLocation.Naptan.Add(naptanZone1b);
            journeyLeg.LegStart.Location = startLocation;
            journeyLeg.LegEnd.Location = endLocation;
            journeyLeg.Mode = SJPModeType.Rail;
            
            // Set date to trigger Travelcard with a zone containing only 2 points test
            journeyLeg.LegStart.DepartureDateTime = new DateTime(2013, 1, 1, 12, 0, 0);
            journeyLeg.LegEnd.ArrivalDateTime = journeyLeg.LegStart.DepartureDateTime.AddHours(1);

            actual = target.HasTravelcard(journeyLeg);
            Assert.AreEqual(false, actual, "Expected HasTravelcard to be false for travelcard with 2 zone points only");
        }

        /// <summary>
        ///A test for TravelcardCatalogue DataTypes
        ///</summary>
        [TestMethod()]
        public void TravelcardCatalogueDataTypesTest()
        {
            #region Zone

            Zone zone = new Zone();

            zone.Id = "Z1";
            zone.Name = "Zone 1";
            zone.OuterZonePolygon = new List<Point>();
            zone.InnerZonePolygon = new List<Point>();
            zone.ModesIncluded = new List<SJPModeType>();
            zone.ModesExcluded = new List<SJPModeType>();
            zone.StopsIncluded = new List<string>();
            zone.StopsExcluded = new List<string>();

            zone.AddZoneMode(SJPModeType.Unknown, false);
            zone.AddZoneMode(SJPModeType.Rail, false);
            zone.AddZoneMode(SJPModeType.Bus, true);
            zone.AddZoneMode(SJPModeType.Rail, false);// Exists in included
            zone.AddZoneMode(SJPModeType.Bus, true);  // Exists in excluded
            zone.AddZoneMode(SJPModeType.Rail, true); // Exists in included
            zone.AddZoneMode(SJPModeType.Bus, false); // Exists in excluded

            zone.AddZonePoint(new Point(123456, 123456), true);
            zone.AddZonePoint(new Point(123453, 123453), true);
            zone.AddZonePoint(new Point(123450, 123450), true);
            zone.AddZonePoint(new Point(456123, 456123), false);
            zone.AddZonePoint(new Point(456126, 456126), false);
            zone.AddZonePoint(new Point(456120, 456120), false);

            zone.AddZoneStop(null, false);
            zone.AddZoneStop(string.Empty, false);
            zone.AddZoneStop("TEST123", false);
            zone.AddZoneStop("TEST987", true);
            zone.AddZoneStop("TEST123", false); // Exists in included
            zone.AddZoneStop("TEST987", true);  // Exists in excluded
            zone.AddZoneStop("TEST123", true);  // Exists in included
            zone.AddZoneStop("TEST987", false); // Exists in excluded
            
            Assert.IsNotNull(zone.Id);
            Assert.IsNotNull(zone.Name);
            Assert.IsNotNull(zone.OuterZonePolygon);
            Assert.IsNotNull(zone.InnerZonePolygon);
            Assert.IsNotNull(zone.ModesIncluded);
            Assert.IsNotNull(zone.ModesExcluded);
            Assert.IsNotNull(zone.StopsIncluded);
            Assert.IsNotNull(zone.StopsExcluded);
            Assert.IsNotNull(zone.ToString());

            #endregion

            #region Route

            Route route = new Route();

            route.Id = "R1";
            route.Name = "Route 1";
            route.StopsEndA = new List<string>();
            route.StopsEndB = new List<string>();
            route.ZonesEndA = new Dictionary<string, Zone>();
            route.ZonesEndB = new Dictionary<string, Zone>();
            route.ModesIncluded = new List<SJPModeType>();
            route.ModesExcluded = new List<SJPModeType>();

            route.AddRouteMode(SJPModeType.Unknown, false);
            route.AddRouteMode(SJPModeType.Rail, false);
            route.AddRouteMode(SJPModeType.Bus, true);
            route.AddRouteMode(SJPModeType.Rail, false);// Exists in included
            route.AddRouteMode(SJPModeType.Bus, true);  // Exists in excluded
            route.AddRouteMode(SJPModeType.Rail, true); // Exists in included
            route.AddRouteMode(SJPModeType.Bus, false); // Exists in excluded

            route.AddRouteEndStop(null, false);
            route.AddRouteEndStop(string.Empty, false);
            route.AddRouteEndStop("TEST123", false);
            route.AddRouteEndStop("TEST987", true);
            route.AddRouteEndStop("TEST123", false); // Exists in included
            route.AddRouteEndStop("TEST987", true);  // Exists in excluded
            route.AddRouteEndStop("TEST123", true);  // Exists in included
            route.AddRouteEndStop("TEST987", false); // Exists in excluded

            Zone zoneB = new Zone("Z2", "Zone 2");

            route.AddRouteEndZone(zone, true);
            route.AddRouteEndZone(zone, true);
            route.AddRouteEndZone(zone, false);
            route.AddRouteEndZone(zoneB, false);
            route.AddRouteEndZone(zoneB, false);
            route.AddRouteEndZone(zoneB, true);
            
            Assert.IsNotNull(route.Id);
            Assert.IsNotNull(route.Name);
            Assert.IsNotNull(route.ModesIncluded);
            Assert.IsNotNull(route.ModesExcluded);
            Assert.IsNotNull(route.StopsEndA);
            Assert.IsNotNull(route.StopsEndB);
            Assert.IsNotNull(route.ZonesEndA);
            Assert.IsNotNull(route.ZonesEndB);
            Assert.IsNotNull(route.ToString());

            #endregion

            #region Travelcard

            Travelcard travelcard = new Travelcard();

            travelcard.Id = "TC1";
            travelcard.Name = "Travelcard 1";
            travelcard.ValidFrom = DateTime.MinValue;
            travelcard.ValidTo = DateTime.MaxValue;
            travelcard.RoutesIncluded = new List<string>();
            travelcard.RoutesExcluded = new List<string>();
            travelcard.ZonesIncluded = new List<string>();
            travelcard.ZonesExcluded = new List<string>();
            
            travelcard.AddRoute(null, false);
            travelcard.AddRoute(string.Empty, false);
            travelcard.AddRoute("R1", false);
            travelcard.AddRoute("R2", true);
            travelcard.AddRoute("R1", false); // Exists in included
            travelcard.AddRoute("R2", true);  // Exists in excluded
            travelcard.AddRoute("R1", true);  // Exists in included
            travelcard.AddRoute("R2", false); // Exists in excluded

            travelcard.AddZone(null, false);
            travelcard.AddZone(string.Empty, false);
            travelcard.AddZone("Z1", false);
            travelcard.AddZone("Z2", true);
            travelcard.AddZone("Z1", false); // Exists in included
            travelcard.AddZone("Z2", true);  // Exists in excluded
            travelcard.AddZone("Z1", true);  // Exists in included
            travelcard.AddZone("Z2", false); // Exists in excluded

            Assert.IsNotNull(travelcard.Id);
            Assert.IsNotNull(travelcard.Name);
            Assert.AreEqual(DateTime.MinValue, travelcard.ValidFrom);
            Assert.AreEqual(DateTime.MaxValue, travelcard.ValidTo);
            Assert.IsNotNull(travelcard.RoutesIncluded);
            Assert.IsNotNull(travelcard.RoutesExcluded);
            Assert.IsNotNull(travelcard.ZonesIncluded);
            Assert.IsNotNull(travelcard.ZonesExcluded);
            Assert.IsNotNull(travelcard.ToString());
            
            #endregion
        }
    }
}
