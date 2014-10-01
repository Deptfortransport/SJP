// *********************************************** 
// NAME             : LocationServiceTest.cs 
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: LocationServiceTest test class
// ************************************************
// 
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for LocationServiceTest and is intended
    ///to contain all LocationServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocationServiceTest
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
        ///A test for LocationService Constructor
        ///</summary>
        [TestMethod()]
        public void LocationServiceConstructorTest()
        {
            LocationService target = new LocationService();

            Assert.IsNotNull(target, "Expected LocationService object to not be null");
        }

        /// <summary>
        ///A test for GetAlternateLocations
        ///</summary>
        [TestMethod()]
        public void GetAlternateLocationsTest()
        {
            LocationService target = new LocationService();

            string searchString = "Bee";
            
            List<SJPLocation> actual = target.GetAlternateLocations(searchString);

            Assert.IsNotNull(actual, "Expected GetAlternateLocations to return an list and not be null");
            Assert.IsTrue(actual.Count > 0, "Expected GetAlternateLocations to return list containing locations");
        }

        /// <summary>
        ///A test for GetGNATLocations
        ///</summary>
        [TestMethod()]
        public void GetGNATLocationsTest()
        {
            LocationService target = new LocationService();
            
            List<SJPGNATLocation> actual = target.GetGNATLocations();

            Assert.IsNotNull(actual, "Expected GetGNATLocations to return an list and not be null");
            Assert.IsTrue(actual.Count > 0, "Expected GetGNATLocations to return list containing GNAT locations");
        }

        /// <summary>
        ///A test for GetSJPLocation
        ///</summary>
        [TestMethod()]
        public void GetSJPLocationTest()
        {
            LocationService target = new LocationService();

            string searchString = string.Empty;
            SJPLocationType locType = SJPLocationType.Unknown;
            SJPLocation actual;

            // Venue test
            searchString = "8100STA";
            locType = SJPLocationType.Venue;
            actual = target.GetSJPLocation(searchString, locType);

            Assert.IsNotNull(actual, "Expected GetSJPLocation to return an location and not be null");
            Assert.IsTrue(actual.ID == searchString, 
                string.Format("Expected search for SJPLocation with type[{0}] and string[{1}] to return location with ID[{2}] but instead is [{3}]", locType, searchString, searchString, actual.ID));

            // Locality test
            searchString = "E0000326";
            locType = SJPLocationType.Locality;
            actual = target.GetSJPLocation(searchString, locType);

            Assert.IsNotNull(actual, "Expected GetSJPLocation to return an location and not be null");
            Assert.IsTrue(actual.ID == searchString,
                string.Format("Expected search for SJPLocation with type[{0}] and string[{1}] to return location with ID[{2}] but instead is [{3}]", locType, searchString, searchString, actual.ID));

            // Postcode test
            searchString = "NG9 1AL";
            locType = SJPLocationType.Postcode;
            actual = target.GetSJPLocation(searchString, locType);

            Assert.IsNotNull(actual, "Expected GetSJPLocation to return an location and not be null");
            Assert.IsTrue(actual.ID == searchString.Replace(" ", string.Empty),
                string.Format("Expected search for SJPLocation with type[{0}] and string[{1}] to return location with ID[{2}] but instead is [{3}]", locType, searchString, searchString, actual.ID));


            // Station test
            searchString = "9100DRBY";
            locType = SJPLocationType.Station;
            actual = target.GetSJPLocation(searchString, locType);

            Assert.IsNotNull(actual, "Expected GetSJPLocation to return an location and not be null");
            Assert.IsTrue(actual.Naptan[0] == searchString,
                string.Format("Expected search for SJPLocation with type[{0}] and string[{1}] to return location with Naptan[{2}] but instead is [{3}]", locType, searchString, searchString, actual.Naptan[0]));

            // Station group test
            searchString = "G1";
            locType = SJPLocationType.StationGroup;
            actual = target.GetSJPLocation(searchString, locType);

            Assert.IsNotNull(actual, "Expected GetSJPLocation to return an location and not be null");
            Assert.IsTrue(actual.ID == searchString,
                string.Format("Expected search for SJPLocation with type[{0}] and string[{1}] to return location with ID[{2}] but instead is [{3}]", locType, searchString, searchString, actual.ID));

            // Unknown test
            searchString = "Chelmsford Rail Station";
            locType = SJPLocationType.Unknown;
            actual = target.GetSJPLocation(searchString, locType);

            Assert.IsNotNull(actual, "Expected GetSJPLocation to return an location and not be null");
            Assert.IsTrue(actual.DisplayName.ToUpper() == searchString.ToUpper(),
                string.Format("Expected search for SJPLocation with type[{0}] and string[{1}] to return location with DisplayName[{2}] but instead is [{3}]", locType, searchString, searchString, actual.DisplayName));
        }

        /// <summary>
        ///A test for GetSJPVenueLocations
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueLocationsTest()
        {
            LocationService target = new LocationService();
            
            List<SJPLocation> actual = target.GetSJPVenueLocations();

            Assert.IsNotNull(actual, "Expected GetSJPVenueLocations to return an list and not be null");
            Assert.IsTrue(actual.Count > 0, "Expected GetSJPVenueLocations to return list containing venue locations");
        }

        /// <summary>
        ///A test for GetSJPVenueAccessData
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueAccessDataTest()
        {
            LocationService target = new LocationService();
            List<string> venueNaPTANs = new List<string>() {"8100MIL"};
            DateTime datetime = new DateTime(2012, 7, 27);
            List<SJPVenueAccess> actual = target.GetSJPVenueAccessData(venueNaPTANs, datetime);
            Assert.IsTrue(actual.Count > 0, "Expected venue access to be returned, update test to use latest data");

            // Should find null car parks
            venueNaPTANs = new List<string>() { "8100XXX" };
            actual = target.GetSJPVenueAccessData(venueNaPTANs, datetime);
            Assert.IsNull(actual, "Expected null venue access to be found");
        }

        /// <summary>
        ///A test for GetSJPVenueBlueBadgeCarParks
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueBlueBadgeCarParksTest1()
        {
            LocationService target = new LocationService();
            List<string> venueNaPTANs = new List<string>() { "8100OPK" };
            DateTime outwardDate = new DateTime(2012, 7, 31, 10, 0, 0); 
            DateTime returnDate = new DateTime(2012, 7, 31, 16, 0, 0);
            List<SJPVenueCarPark> actual = target.GetSJPVenueBlueBadgeCarParks(venueNaPTANs, outwardDate, returnDate);
            Assert.IsNotNull(actual, "Expected car park containing blue badge spaced to be returned");
            Assert.IsTrue(actual.Count > 0, "Expected car park containing blue badge spaced to be returned");
            Assert.IsTrue(actual[0].BlueBadgeSpaces > 0, "Expected car park containing blue badge spaced to be returned");
        }

        /// <summary>
        ///A test for GetSJPVenueCarPark
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueCarParkTest()
        {
            LocationService target = new LocationService();
            string carParkId = string.Empty;
            SJPVenueCarPark actual;
            actual = target.GetSJPVenueCarPark(carParkId);
            Assert.IsNull(actual, "Expected no carpark to be returned");

            carParkId = "OPK_EBB";
            actual = target.GetSJPVenueCarPark(carParkId);
            Assert.IsNotNull(actual, "Expected a carpark to be returned");
        }

        /// <summary>
        ///A test for GetSJPVenueCarParks
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueCarParksTest()
        {
            LocationService target = new LocationService();
            List<string> venueNaPTANs = new List<string>(){"8100OPK"};
            DateTime outwardDate = new DateTime(2012, 7, 31);
            DateTime returnDate = new DateTime(2012, 7, 31);
            List<SJPVenueCarPark> actual;
            actual = target.GetSJPVenueCarParks(venueNaPTANs, outwardDate, returnDate);
            Assert.IsTrue(actual.Count > 0, "Expected car parks to be returned");

            // Should find null car parks
            venueNaPTANs = new List<string>() { "8100XXX" };
            actual = target.GetSJPVenueCarParks(venueNaPTANs, outwardDate, returnDate);
            Assert.IsNull(actual, "Expected null car parks to be found");
        }


        /// <summary>
        ///A test for GetSJPVenueCyclePark
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueCycleParkTest()
        {
            LocationService target = new LocationService();
            string cycleParkId = string.Empty; 
            SJPVenueCyclePark actual = target.GetSJPVenueCyclePark(cycleParkId);
            Assert.IsNull(actual, "Expected no cycle park to be returned");

            cycleParkId = "WEACY01";
            actual = target.GetSJPVenueCyclePark(cycleParkId);
            Assert.IsNotNull(actual, "Expected cycle park to be returned");
        }

        /// <summary>
        ///A test for GetSJPVenueCycleParks
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueCycleParksTest()
        {
            LocationService target = new LocationService();
            List<string> venueNaPTANs = new List<string>() {"8100GRP", "8100OPK"};
            DateTime outwardDate = new DateTime(2012, 8, 1, 10, 0, 0);
            DateTime returnDate = new DateTime(2012, 8, 1, 17, 0, 0);
            List<SJPVenueCyclePark> actual;
            actual = target.GetSJPVenueCycleParks(venueNaPTANs, outwardDate, returnDate);
            Assert.IsTrue(actual.Count > 0, "Expected cycle parks to be returned");

            // Should find null cycle parks
            venueNaPTANs = new List<string>() { "8100XXX" };
            actual = target.GetSJPVenueCycleParks(venueNaPTANs, outwardDate, returnDate);
            Assert.IsNull(actual, "Expected null cycle parks to be found");

            // Overnight availablity check
            venueNaPTANs = new List<string>() { "8100EXL" };
            outwardDate = new DateTime(2012, 8, 1, 0, 5, 0);
            returnDate = new DateTime(2012, 8, 1, 0, 5, 0);
            actual = target.GetSJPVenueCycleParks(venueNaPTANs, outwardDate, returnDate);
            Assert.IsTrue(actual.Count > 0, "Expected cycle park which closes after midnight to be returned, update test to use latest data");
        }
        
        /// <summary>
        ///A test for GetSJPVenueGate
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueGateTest()
        {
            LocationService target = new LocationService();
            string venueGateNaPTAN = "8100HYDg0"; 
            SJPVenueGate actual = target.GetSJPVenueGate(venueGateNaPTAN);
            Assert.IsNotNull(actual, "Expected a venue gate to be returned");

            // Null test
            venueGateNaPTAN = "8100XXX";
            actual = target.GetSJPVenueGate(venueGateNaPTAN);
            Assert.IsNull(actual, "Expected a null venue gate");
        }

        /// <summary>
        ///A test for GetSJPVenueGateCheckConstraints
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueGateCheckConstraintsTest()
        {
            LocationService target = new LocationService();
            string venueGateNaPTAN = "8100HYDg0";
            SJPVenueGate venueGate = target.GetSJPVenueGate(venueGateNaPTAN);
            bool isVenueEntry = true;
            SJPVenueGateCheckConstraint actual = target.GetSJPVenueGateCheckConstraints(venueGate, isVenueEntry);
            Assert.IsNotNull(actual, "Expected a check constraint to be returned");

            // Null test
            List<string> venueGateNaPTANs = new List<string>();
            venueGateNaPTANs.Add("8100XXX");
            List<SJPVenueGateCheckConstraint> actual2 = target.GetSJPVenueGateCheckConstraints(venueGateNaPTANs);
            Assert.IsNull(actual2, "Expected null check constraints to be returned");
        }
        
        /// <summary>
        ///A test for GetSJPVenueGateNavigationPaths
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueGateNavigationPathsTest1()
        {
            // Venue test
            LocationService target = new LocationService();
            string searchString = "8100HYD";
            SJPLocationType locType = SJPLocationType.Venue;
            SJPLocation venue = target.GetSJPLocation(searchString, locType); 
            string venueGateNaPTAN = "8100HYDg0";
            SJPVenueGate venueGate = target.GetSJPVenueGate(venueGateNaPTAN);
            bool isToVenue = true;
            SJPVenueGateNavigationPath actual = target.GetSJPVenueGateNavigationPaths(venue, venueGate, isToVenue);
            Assert.IsNotNull(actual, "Expected a navigation path to be returned");

            isToVenue = false;
            actual = target.GetSJPVenueGateNavigationPaths(venue, venueGate, isToVenue);
            Assert.IsNotNull(actual, "Expected a navigation path to be returned");

            // Null test
            actual = target.GetSJPVenueGateNavigationPaths(venue, null, isToVenue);
            Assert.IsNull(actual, "Expected a navigation path not to be returned");

            // Null test
            List<string> venueGateNaPTANs = new List<string>();
            venueGateNaPTANs.Add("8100XXX");
            List<SJPVenueGateNavigationPath> actual2 = target.GetSJPVenueGateNavigationPaths(venueGateNaPTANs);
            Assert.IsNull(actual2, "Expected a navigation path not to be returned");
        }

        /// <summary>
        ///A test for GetSJPVenuePierNavigationPaths
        ///</summary>
        [TestMethod()]
        public void GetSJPVenuePierNavigationPathsTest1()
        {
            LocationService target = new LocationService();
            List<string> venueNaPTANs = new List<string>() {"8100GRP"};
            string venuePierNaPTAN = "9300GNW1";
            bool isToVenue = true; 
            SJPPierVenueNavigationPath actual = target.GetSJPVenuePierNavigationPaths(venueNaPTANs, venuePierNaPTAN, isToVenue);
            Assert.IsNotNull(actual, "Expected a pier navigation path to be returned");

            isToVenue = false; 
            actual = target.GetSJPVenuePierNavigationPaths(venueNaPTANs, venuePierNaPTAN, isToVenue);
            Assert.IsNotNull(actual, "Expected a pier navigation path to be returned");

            venuePierNaPTAN = "9300XXX";
            actual = target.GetSJPVenuePierNavigationPaths(venueNaPTANs, venuePierNaPTAN, isToVenue);
            Assert.IsNull(actual, "Expected a pier navigation path not to be returned");

            // Null test
            List<string> venueGateNaPTANs = new List<string>();
            venueGateNaPTANs.Add("8100XXX");
            List<SJPPierVenueNavigationPath> actual2 = target.GetSJPVenuePierNavigationPaths(venueGateNaPTANs);
            Assert.IsNull(actual2, "Expected a pier navigation path not to be returned");
        }

        /// <summary>
        ///A test for GetSJPVenueRiverServices
        ///</summary>
        [TestMethod()]
        public void GetSJPVenueRiverServicesTest()
        {
            LocationService target = new LocationService(); 
            List<string> venueNaPTANs = new List<string>(){"8100GRP"};
            List<SJPVenueRiverService> actual = target.GetSJPVenueRiverServices(venueNaPTANs);
            Assert.IsTrue(actual.Count > 7, "Expected many river services to be returned");

            // Null test
            venueNaPTANs = new List<string>() { "8100XXX" };
            actual = target.GetSJPVenueRiverServices(venueNaPTANs);
            Assert.IsNull(actual, "Expected null river services to be returned");
        }

        /// <summary>
        ///A test for IsDateValid
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void IsDateValidTest()
        {
            LocationService_Accessor target = new LocationService_Accessor();

            // Outward only check
            List<DayOfWeek> daysValid = new List<DayOfWeek>() {DayOfWeek.Monday, DayOfWeek.Thursday};
            DateTime fromDate = new DateTime(2012, 7, 7); 
            DateTime toDate = new DateTime(2012, 7, 14);
            DateTime outwardDate = new DateTime(2012, 7, 9);
            DateTime returnDate = new DateTime(2012, 7, 9);
            bool checkOutwardDate = true;
            bool checkReturnDate = false;
            bool expected = true;
            bool actual = target.IsDateValid(daysValid, fromDate, toDate, outwardDate, returnDate, checkOutwardDate, checkReturnDate);
            Assert.AreEqual(expected, actual, "Expected the date to be valid");

            // Return only check
            daysValid = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Thursday };
            fromDate = new DateTime(2012, 7, 7);
            toDate = new DateTime(2012, 7, 14);
            outwardDate = new DateTime(2012, 7, 9);
            returnDate = new DateTime(2012, 7, 9);
            checkOutwardDate = false;
            checkReturnDate = true;
            expected = true;
            actual = target.IsDateValid(daysValid, fromDate, toDate, outwardDate, returnDate, checkOutwardDate, checkReturnDate);
            Assert.AreEqual(expected, actual, "Expected the date to be valid");

            // Outward and return check
            daysValid = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Thursday };
            fromDate = new DateTime(2012, 7, 7);
            toDate = new DateTime(2012, 7, 14);
            outwardDate = new DateTime(2012, 7, 9);
            returnDate = new DateTime(2012, 7, 9);
            checkOutwardDate = true;
            checkReturnDate = true;
            expected = true;
            actual = target.IsDateValid(daysValid, fromDate, toDate, outwardDate, returnDate, checkOutwardDate, checkReturnDate);
            Assert.AreEqual(expected, actual, "Expected the date to be valid");

            // Check neither (weird!)
            daysValid = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Thursday };
            fromDate = new DateTime(2012, 7, 7);
            toDate = new DateTime(2012, 7, 14);
            outwardDate = new DateTime(2012, 7, 9);
            returnDate = new DateTime(2012, 7, 9);
            checkOutwardDate = false;
            checkReturnDate = false;
            expected = true;
            actual = target.IsDateValid(daysValid, fromDate, toDate, outwardDate, returnDate, checkOutwardDate, checkReturnDate);
            Assert.AreEqual(expected, actual, "Expected the date to be valid");
        }

        /// <summary>
        ///A test for IsTimeValid
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void IsTimeValidTest()
        {
            LocationService_Accessor target = new LocationService_Accessor(); 

            // Check outward only not overnight
            TimeSpan openingTime = new TimeSpan(10, 0, 0);
            TimeSpan closingTime = new TimeSpan(17, 0, 0);
            DateTime outwardDate = new DateTime(2012, 7, 8, 11, 0, 0);
            DateTime returnDate = new DateTime(2012, 7, 8, 16, 0, 0); 
            bool checkOutwardDate = true;
            bool checkReturnDate = false;
            bool overnight = false;
            bool expected = true;
            bool actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check outward only overnight
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(17, 0, 0);
            outwardDate = new DateTime(2012, 7, 8, 11, 0, 0);
            returnDate = new DateTime(2012, 7, 8, 16, 0, 0);
            checkOutwardDate = true;
            checkReturnDate = false;
            overnight = true;
            expected = true;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check return only not overnight
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(17, 0, 0);
            outwardDate = new DateTime(2012, 7, 8, 11, 0, 0);
            returnDate = new DateTime(2012, 7, 8, 16, 0, 0);
            checkOutwardDate = false;
            checkReturnDate = true;
            overnight = false;
            expected = true;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check return only overnight
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(17, 0, 0);
            outwardDate = new DateTime(2012, 7, 8, 11, 0, 0);
            returnDate = new DateTime(2012, 7, 8, 16, 0, 0);
            checkOutwardDate = false;
            checkReturnDate = true;
            overnight = true;
            expected = true;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check both only not overnight
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(17, 0, 0);
            outwardDate = new DateTime(2012, 7, 8, 11, 0, 0);
            returnDate = new DateTime(2012, 7, 8, 16, 0, 0);
            checkOutwardDate = true;
            checkReturnDate = true;
            overnight = false;
            expected = true;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check both overnight
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(17, 0, 0);
            outwardDate = new DateTime(2012, 7, 8, 11, 0, 0);
            returnDate = new DateTime(2012, 7, 8, 16, 0, 0);
            checkOutwardDate = true;
            checkReturnDate = true;
            overnight = true;
            expected = true;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check neither
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(17, 0, 0);
            outwardDate = new DateTime(2012, 7, 8, 11, 0, 0);
            returnDate = new DateTime(2012, 7, 8, 16, 0, 0);
            checkOutwardDate = false;
            checkReturnDate = false;
            overnight = false;
            expected = true;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check both at overnight threshold
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(0, 30, 0);
            outwardDate = new DateTime(2012, 7, 8, 0, 5, 0);
            returnDate = new DateTime(2012, 7, 8, 0, 5, 0);
            checkOutwardDate = true;
            checkReturnDate = true;
            overnight = true;
            expected = true;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be valid");

            // Check both invalid
            openingTime = new TimeSpan(10, 0, 0);
            closingTime = new TimeSpan(17, 0, 0);
            outwardDate = new DateTime(2012, 7, 8, 4, 0, 0);
            returnDate = new DateTime(2012, 7, 8, 4, 0, 0);
            checkOutwardDate = true;
            checkReturnDate = true;
            overnight = true;
            expected = false;
            actual = target.IsTimeValid(openingTime, closingTime, outwardDate, returnDate, checkOutwardDate, checkReturnDate, overnight);
            Assert.AreEqual(expected, actual, "Expected time to be invalid");
        }

        /// <summary>
        ///A test for IsGNAT
        ///</summary>
        [TestMethod()]
        public void IsGNATTest()
        {
            LocationService target = new LocationService();
            string naptan = "900010171";
            bool stepFree = true; 
            bool assistanceRequired = true; 
            bool expected = true;
            bool actual = target.IsGNAT(naptan, stepFree, assistanceRequired);
            Assert.AreEqual(expected, actual, "Expected venue to be have wheel chair access and assistance");
        }

        /// <summary>
        /// A test for IsGNATAdminArea
        /// </summary>
        [TestMethod()]
        public void IsGNATAdminAreaTest()
        {
            LocationService target = new LocationService();
            int adminArea = 82; 
            int districtCode = 282;
            bool stepFree = true;
            bool assistanceRequired = true;
            bool expected = true;
            bool actual = target.IsGNATAdminArea(adminArea, districtCode, stepFree, assistanceRequired);
            // Check database for existing record if test fails
            Assert.AreEqual(expected, actual, "Expected admin area to have wheel chair access and assistance");

            adminArea = 9999;
            expected = false;
            actual = target.IsGNATAdminArea(adminArea, districtCode, stepFree, assistanceRequired);
            // Check database for existing record if test fails
            Assert.AreEqual(expected, actual, "Expected admin area not to be found and therefore not have wheel chair access and assistance");
        }

        /// <summary>
        ///A test for LoadData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void LoadDataTest()
        {
            LocationService_Accessor target = new LocationService_Accessor(); 
            target.LoadData();
            Assert.IsTrue(SJPVenueLocationCache.GetVenuesList().Count > 0, "Expected venues to be populated");
            Assert.IsTrue(SJPGNATLocationCache.GetGNATList().Count > 0, "Expected GNATs to be populated");
            Assert.IsNotNull(SJPLocationCache.GetNaptanLocation("8100GRP"), "Expected locations to be populated");
            Assert.IsNotNull(SJPLocationCache.GetPostcodeLocation("AB101AL"), "Expected postcodes to be populated");
        }
    }
}
