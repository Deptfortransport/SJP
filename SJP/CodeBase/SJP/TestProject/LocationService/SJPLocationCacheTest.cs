// *********************************************** 
// NAME             : SJPLocationCacheTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: SJPLocationCacheTest test class
// ************************************************
// 
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;
using System.Collections.Generic;

namespace SJP.TestProject
{
    /// <summary>
    ///This is a test class for SJPLocationCacheTest and is intended
    ///to contain all SJPLocationCacheTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPLocationCacheTest
    {
        private TestContext testContextInstance;

        private string unknownToFind = "Arbroath Rail Station";
        private string unknownToFindPostcode = "SW1A 1AA";
        private string naptanToFind = "9100LESTER";
        private string groupToFind = "G1";
        private string localityToFind = "E0000326";
        private string postcodeToFind = "NG9 1AL";
        private string alternativeToFind1 = "Glasgow";
        private string alternativeToFind2 = "London";

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

        #region Location tests

        #region Unknown

        /// <summary>
        ///A test for GetUnknownLocation
        ///</summary>
        [TestMethod()]
        public void GetUnknownLocationTest()
        {
            // Check can get an unknown location (station)
            SJPLocation actual = SJPLocationCache.GetUnknownLocation(unknownToFind);

            Assert.IsNotNull(actual, "Expected GetUnknownLocation to have returned a location");
            Assert.IsTrue(actual.Naptan.Count > 0, "Expected unknown station search to have naptans");
            Assert.IsTrue(!string.IsNullOrEmpty(actual.DisplayName));
            Assert.IsTrue(actual.DisplayName.ToUpper() == unknownToFind.ToUpper(), "Expected unknown station search DisplayName to match search string");
            Assert.IsTrue(!string.IsNullOrEmpty(actual.ID));
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Name));
            Assert.IsTrue(actual.Toid.Count > 0);

        }

        /// <summary>
        ///A test for PopulateUnknownData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulateUnknownDataTest()
        {
            // Tests if can load an unknown location (station) from the database
            SJPLocation actual = SJPLocationCache_Accessor.PopulateUnknownData(unknownToFind);

            Assert.IsNotNull(actual, "Expected PopulateUnknownData to have loaded unknown station location");
            Assert.IsTrue(actual.Naptan.Count > 0, "Expected unknown station search to have naptans");
            Assert.IsTrue(actual.DisplayName.ToUpper() == unknownToFind.ToUpper(), "Expected unknown station search DisplayName to match search string");

            // Tests if can load an unknown location (postcode) from the database
            actual = SJPLocationCache_Accessor.PopulateUnknownData(unknownToFindPostcode);

            Assert.IsNotNull(actual, "Expected PopulateUnknownData to have loaded unknown postcode location");
            Assert.IsTrue(actual.Name.ToUpper() == unknownToFindPostcode.ToUpper().Replace(" ", string.Empty), "Expected unknown postcode search Name to match search string (without spaces)");           
        }

        #endregion

        #region NaPTAN

        /// <summary>
        ///A test for GetNaptanLocation
        ///</summary>
        [TestMethod()]
        public void GetNaptanLocationTest()
        {
            // Check can get a naptan location
            SJPLocation actual = SJPLocationCache.GetNaptanLocation(naptanToFind);

            Assert.IsNotNull(actual, "Expected GetNaptanLocation to have returned a naptan location");
            Assert.IsTrue(actual.Naptan.Count > 0, "Expected station to have naptans");
            Assert.IsTrue(actual.Naptan[0] == naptanToFind, "Expected location Naptan to equal naptan searched for");
            Assert.IsTrue(!string.IsNullOrEmpty(actual.ID));
            Assert.IsTrue(!string.IsNullOrEmpty(actual.DisplayName));
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Name));
            Assert.IsTrue(actual.Toid.Count > 0);

        }

        /// <summary>
        ///A test for PopulateNaptanData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulateNaptanDataTest()
        {
            // Tests if can load a naptan location from the database
            SJPLocation actual = SJPLocationCache_Accessor.PopulateNaPTANData(naptanToFind);

            Assert.IsNotNull(actual, "Expected PopulateNaPTANData to have loaded naptan location");
            Assert.IsTrue(actual.Naptan.Count > 0, "Expected station to have naptans");
            Assert.IsTrue(actual.Naptan[0] == naptanToFind, "Expected location Naptan to equal naptan searched for");
            Assert.IsTrue(actual.ID == naptanToFind, "Expected location ID to equal naptan searched for");
        }

        #endregion

        #region Group

        /// <summary>
        ///A test for GetGroupLocation
        ///</summary>
        [TestMethod()]
        public void GetGroupLocationTest()
        {
            // Check can get a group location
            SJPLocation actual = SJPLocationCache.GetGroupLocation(groupToFind);

            Assert.IsNotNull(actual, "Expected GetGroupLocation to have returned a group location");
            Assert.IsTrue(actual.DataSetID == groupToFind, "Expected location DataSetID to equal group searched for");
            Assert.IsTrue(!string.IsNullOrEmpty(actual.ID));
            Assert.IsTrue(actual.IsGNAT == false);
            Assert.IsTrue(!string.IsNullOrEmpty(actual.DisplayName));
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Name));
            Assert.IsTrue(actual.Toid.Count > 0);
            Assert.IsTrue(actual.Naptan.Count > 0); // Group station should have no naptans

        }

        /// <summary>
        ///A test for PopulateGroupData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulateGroupDataTest()
        {
            // Tests if can load a group location from the database
            SJPLocation actual = SJPLocationCache_Accessor.PopulateGroupData(groupToFind);

            Assert.IsNotNull(actual, "Expected PopulateGroupData to have loaded group location");
            Assert.IsTrue(actual.DataSetID == groupToFind, "Expected location DataSetID to equal group searched for");
            Assert.IsTrue(actual.ID == groupToFind, "Expected location ID to equal group searched for");
        }

        #endregion

        #region Locality

        /// <summary>
        ///A test for GetLocalityLocation
        ///</summary>
        [TestMethod()]
        public void GetLocalityLocationTest()
        {
            // Check can get a locality location
            SJPLocation actual = SJPLocationCache.GetLocalityLocation(localityToFind);

            Assert.IsNotNull(actual, "Expected GetLocalityLocation to have returned a locality location");
            Assert.IsTrue(actual.Locality == localityToFind, "Expected location Locality to equal locality searched for");
            Assert.IsTrue(!string.IsNullOrEmpty(actual.ID));
            Assert.IsTrue(actual.IsGNAT == false);
            Assert.IsTrue(!string.IsNullOrEmpty(actual.DisplayName));
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Name));
            Assert.IsTrue(actual.Toid.Count > 0);
            Assert.IsTrue(actual.Naptan.Count == 0); // Locality should have no naptans
            
        }

        /// <summary>
        ///A test for PopulateLocalityData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulateLocalityDataTest()
        {
            // Tests if can load a locality location from the database
            SJPLocation actual = SJPLocationCache_Accessor.PopulateLocalityData(localityToFind);

            Assert.IsNotNull(actual, "Expected PopulateLocalityData to have loaded locality location");
            Assert.IsTrue(actual.Locality == localityToFind, "Expected location Locality to equal locality searched for");
            Assert.IsTrue(actual.ID == localityToFind, "Expected location ID to equal locality searched for");
        }

        #endregion

        /// <summary>
        ///A test for LoadLocations
        ///</summary>
        [TestMethod()]
        public void LoadLocationsTest()
        {
            SJPLocationCache.LoadLocations();

            // Check can get a location (e.g. check for locality)
            SJPLocation actual = SJPLocationCache.GetLocalityLocation(localityToFind);

            Assert.IsNotNull(actual, "Expected LoadLocations to have loaded locations");
        }

        /// <summary>
        ///A test for PopulateLocationsData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulateLocationsDataTest()
        {
            // Tests if all locations have been loaded from the database
            SJPLocationCache_Accessor.PopulateLocationsData();

            Assert.IsTrue(SJPLocationCache_Accessor.locations.Count > 0, "Expected locations data to have been populated");
        }

        /// <summary>
        ///A test for PopulateLocationsData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void GetCachedLocationTest()
        {
            // Tests if all locations have been loaded from the database
            SJPLocationCache_Accessor.PopulateLocationsData();
            SJPLocationCache_Accessor.PopulatePostcodesData();

            // Set flag for to use the cache
            SJPLocationCache_Accessor.useLocationCache = true;
            SJPLocationCache_Accessor.usePostcodeCache = true;

            SJPLocation actual1 = null;
            List<SJPLocation> actual2 = null;

            actual1 = SJPLocationCache.GetUnknownLocation(unknownToFind);
            Assert.IsNotNull(actual1, "Expected GetUnknownLocation using cache to have returned a location");

            actual1 = SJPLocationCache.GetNaptanLocation(naptanToFind);
            Assert.IsNotNull(actual1, "Expected GetNaptanLocation using cache to have returned a naptan location");

            actual1 = SJPLocationCache.GetGroupLocation(groupToFind);
            Assert.IsNotNull(actual1, "Expected GetGroupLocation using cache to have returned a group location");

            actual1 = SJPLocationCache.GetLocalityLocation(localityToFind);
            Assert.IsNotNull(actual1, "Expected GetLocalityLocation using cache to have returned a locality location");

            actual1 = SJPLocationCache.GetPostcodeLocation(postcodeToFind);
            Assert.IsNotNull(actual1, "Expected GetPostcodeLocation using cache to have returned a postcode location");

            actual2 = SJPLocationCache.GetAlternativeSJPLocations(alternativeToFind1);
            Assert.IsNotNull(actual2, "Expected GetAlternativeSJPLocations using cache to have returned locations");

            // Reset flag for any other tests using the SJPLocationCache
            SJPLocationCache_Accessor.useLocationCache = false;
            SJPLocationCache_Accessor.usePostcodeCache = false;

        }

        #endregion

        #region Postcode tests

        /// <summary>
        ///A test for GetPostcodeLocation
        ///</summary>
        [TestMethod()]
        public void GetPostcodeLocationTest()
        {
            // Check can get a postcode
            SJPLocation actual = SJPLocationCache.GetPostcodeLocation(postcodeToFind);

            Assert.IsNotNull(actual, "Expected GetPostcodeLocation to have returned a postcode location");
            Assert.IsTrue(actual.DisplayName == postcodeToFind, "Expected location DisplayName to equal postcode searched for");
            Assert.IsTrue(!string.IsNullOrEmpty(actual.ID));
            Assert.IsTrue(actual.IsGNAT == false);
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Locality));
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Name));
            Assert.IsTrue(actual.Toid.Count > 0);
            Assert.IsTrue(actual.Naptan.Count == 0); // Should be no naptans for postcode
        }

        /// <summary>
        ///A test for LoadPostcodes
        ///</summary>
        [TestMethod()]
        public void LoadPostcodesTest()
        {
            SJPLocationCache.LoadPostcodes();

            // Check can get a postcode
            SJPLocation actual = SJPLocationCache.GetPostcodeLocation(postcodeToFind);

            Assert.IsNotNull(actual, "Expected LoadPostcodes to have loaded postcodes");
        }

        /// <summary>
        ///A test for PopulatePostcodeData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulatePostcodeDataTest()
        {
            // Tests if can load a postcode location from the database
            SJPLocation actual = SJPLocationCache_Accessor.PopulatePostcodeData(postcodeToFind);

            Assert.IsNotNull(actual, "Expected PopulatePostcodeData to have loaded postcode");
            Assert.IsTrue(actual.DisplayName == postcodeToFind, "Expected location DisplayName to equal postcode searched for");
            Assert.IsTrue(actual.ID == postcodeToFind.Replace(" ", string.Empty), "Expected location ID to equal postcode searched for");
        }

        /// <summary>
        ///A test for PopulatePostcodesData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulatePostcodesDataTest()
        {
            // Tests if all postcode locations have been loaded from the database
            SJPLocationCache_Accessor.PopulatePostcodesData();

            Assert.IsTrue(SJPLocationCache_Accessor.postcodeLocations.Count > 0, "Expected postcodes data to have been populated");
        }

        #endregion

        #region Alternative Location tests

        /// <summary>
        ///A test for GetAlternativeSJPLocationsFromCache
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void GetAlternativeSJPLocationsFromCacheTest()
        {
            List<SJPLocation> actual = SJPLocationCache_Accessor.GetAlternativeSJPLocationsFromCache(alternativeToFind1, 1000);

            Assert.IsNotNull(actual, "Expected GetAlternativeSJPLocationsFromCache to have returned a list");

            // If test run seperately, and use location cache is false (likely as this will be run in Dev),
            // then no locations will exist in cache and so result is false - which is ok
            if ((!SJPLocationCache_Accessor.useLocationCache) && (actual.Count == 0 ))
            {
                Assert.IsTrue(actual.Count == 0, "Expected locations to not have been found");
            }
            // Otherwise test for true
            else
            {
                Assert.IsTrue(actual.Count > 0, "Expected locations to have been found");
            }
        }

        /// <summary>
        ///A test for GetAlternativeSJPLocationsFromDB
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void GetAlternativeSJPLocationsFromDBTest()
        {
            // Tests if can load a naptan location from the database
            List<SJPLocation> actual = SJPLocationCache_Accessor.GetAlternativeSJPLocationsFromDB(alternativeToFind1, 1000);

            Assert.IsNotNull(actual, "Expected GetAlternativeSJPLocationsFromDB to have returned a list");
            Assert.IsTrue(actual.Count > 0, "Expected locations to have been found");

            // Group location in result
            actual = SJPLocationCache_Accessor.GetAlternativeSJPLocationsFromDB(alternativeToFind2, 1000);

            Assert.IsNotNull(actual, "Expected GetAlternativeSJPLocationsFromDB to have returned a list");
            Assert.IsTrue(actual.Count > 0, "Expected locations to have been found");

            // Sort and filter
            actual = SJPLocationCache_Accessor.SortAndFilterAlternativeSJPLocations(actual, 20);
            Assert.IsNotNull(actual, "Expected GetAlternativeSJPLocationsFromDB to have returned a list");
            Assert.IsTrue(actual.Count > 0, "Expected locations to have been found");
            Assert.IsTrue(actual.Count <= 20, "Expected locations to have been found");
        }

        #endregion
    }
}
