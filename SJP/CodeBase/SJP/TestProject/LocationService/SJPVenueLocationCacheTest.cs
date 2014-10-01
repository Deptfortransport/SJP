// *********************************************** 
// NAME             : SJPVenueLocationCacheTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: SJPVenueLocationCacheTest test class
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
    ///This is a test class for SJPVenueLocationCacheTest and is intended
    ///to contain all SJPVenueLocationCacheTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueLocationCacheTest
    {
        private TestContext testContextInstance;

        private string venueToFind = "8100STA";

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
            SJPServiceDiscovery.Init(new LocationServiceInitialisation());
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
        ///A test for GetVenueLocation
        ///</summary>
        [TestMethod()]
        public void GetVenueLocationTest()
        {
            SJPLocation actual = SJPVenueLocationCache.GetVenueLocation(venueToFind);

            Assert.IsNotNull(actual, "Expected GetVenueLocation to have found venue");
            Assert.IsTrue(actual.Naptan.Contains(venueToFind), "Expected GetVenueLocation location to have returned the requested venue");
        }

        /// <summary>
        ///A test for GetVenuesList
        ///</summary>
        [TestMethod()]
        public void GetVenuesListTest()
        {
            List<SJPLocation> actual = SJPVenueLocationCache.GetVenuesList();

            bool hasVenues = actual.Count > 0;

            bool hasVenue = false;

            foreach (SJPLocation location in actual)
            {
                if (location.Naptan.Contains(venueToFind))
                {
                    hasVenue = true;
                    break;
                }
            }

            Assert.IsTrue(hasVenues, "Expected GetVenuesList to return a list containing at least 1 venue");
            Assert.IsTrue(hasVenue, string.Format("Expected GetVenuesList to return a list containing the venue with NaPTAN[{0}]", venueToFind));
        }

        /// <summary>
        ///A test for LoadVenues
        ///</summary>
        [TestMethod()]
        public void LoadVenuesTest()
        {
            SJPVenueLocationCache.LoadVenues();

            // Check can get a venue
            SJPLocation actual = SJPVenueLocationCache.GetVenueLocation(venueToFind);

            Assert.IsNotNull(actual, "Expected LoadVenues to have loaded venues");
        }

        /// <summary>
        ///A test for PopulateData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void PopulateVenuesDataTest()
        {
            SJPVenueLocationCache_Accessor.PopulateVenuesData();

            Assert.IsTrue(SJPVenueLocationCache_Accessor.venuesList.Count > 0, "Expected venues data to have been populated");
            Assert.IsTrue(SJPVenueLocationCache_Accessor.venueLocations.Count > 0, "Expected venues data to have been populated");
        }
    }
}
