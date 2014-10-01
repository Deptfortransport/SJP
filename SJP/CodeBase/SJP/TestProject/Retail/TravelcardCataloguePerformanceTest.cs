// *********************************************** 
// NAME             : TravelcardCataloguePerformanceTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 24 Jan 2012
// DESCRIPTION  	: TravelcardCataloguePerformanceTest test class
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

namespace SJP.TestProject.Retail
{
    [TestClass]
    public class TravelcardCataloguePerformanceTest
    {
        private TestContext testContextInstance;

        private static TravelcardCatalogue travelcardCatalogue;
        
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

            // Initialise the caches used in test
            travelcardCatalogue = new TravelcardCatalogue();
            
            SJPLocationCache_Accessor.PopulateLocationsData();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
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

        #endregion

        /// <summary>
        /// Returns a default journey leg used for HasTravelcard check (no locations)
        /// </summary>
        /// <returns></returns>
        private JourneyLeg BuildJourneyLeg()
        {
            // Build journey leg
            JourneyLeg journeyLeg = new JourneyLeg();
            journeyLeg.Mode = SJPModeType.Rail;
            journeyLeg.LegStart = new JourneyCallingPoint();
            journeyLeg.LegStart.ArrivalDateTime = DateTime.MinValue;
            journeyLeg.LegStart.DepartureDateTime = new DateTime(2012, 8, 1, 10, 0, 0);
            journeyLeg.LegStart.Type = JourneyCallingPointType.OriginAndBoard;
            journeyLeg.LegEnd = new JourneyCallingPoint();
            journeyLeg.LegEnd.ArrivalDateTime = new DateTime(2012, 8, 1, 11, 0, 0);
            journeyLeg.LegEnd.DepartureDateTime = DateTime.MinValue;
            journeyLeg.LegEnd.Type = JourneyCallingPointType.DestinationAndAlight;

            return journeyLeg;
        }

        /// <summary>
        /// Uses the SJPLocationCache_Accessor.locations to populate a JourneyLeg to 
        /// call the TravelcardCatalogue.HasTravelcard method, repeating the specified iterations
        /// </summary>
        /// <param name="maxIteration"></param>
        /// <returns></returns>
        private int DoHasTravelcardPerformanceTest(int maxIteration)
        {
            // Build journey leg
            JourneyLeg journeyLeg = BuildJourneyLeg();

            SJPLocation startLocation = null;
            SJPLocation endLocation = null;
            bool hasTravelcard = false;

            int iteration = 0;
            int maxIndex = SJPLocationCache_Accessor.locations.Count - 1; //count less one because we do a + 1 in loop to get next locations
            int index = 0;

            bool finished = false;

            // Only do performance test if locations loaded
            if (maxIndex > 0)
            {
                while (!finished)
                {
                    startLocation = SJPLocationCache_Accessor.locations[index];
                    endLocation = SJPLocationCache_Accessor.locations[index + 1];

                    journeyLeg.LegStart.Location = startLocation;
                    journeyLeg.LegEnd.Location = endLocation;

                    hasTravelcard = travelcardCatalogue.HasTravelcard(journeyLeg);

                    index++;
                    iteration++;

                    if (index == maxIndex)
                        index = 0;

                    if (iteration == maxIteration)
                        finished = true;
                }
            }

            return iteration;
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest01a()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(5);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest01b()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(5);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest02a()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(10);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest02b()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(10);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest03a()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(100);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest03b()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(100);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest04a()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(1000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest04b()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(1000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest05a()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(5000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest05b()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(5000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest06a()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(10000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest06b()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(10000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest07a()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(100000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }

        /// <summary>
        ///A test for TravelcardCatalogue HasTravelcard Performance test
        /// </summary>
        [TestMethod()]
        public void TravelcardCataloguePerformanceTest07b()
        {
            DateTime dtStarted = DateTime.Now;

            int iterations = DoHasTravelcardPerformanceTest(100000);

            DateTime dtFinished = DateTime.Now;

            TimeSpan duration = dtFinished.Subtract(dtStarted);

            Assert.IsTrue(duration.TotalSeconds < 60,
                string.Format("HasTravelcard performance test: Duration[{0}ms] Iterations[{1}] Started[{2}] Finished[{3}]",
                duration.TotalMilliseconds,
                iterations,
                dtStarted,
                dtFinished));
        }
    }
}
