using SJP.UserPortal.JourneyPlanRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using SJP.Common.PropertyManager;
using SJP.Common;
using SJP.Common.ServiceDiscovery;
using JC = SJP.UserPortal.JourneyControl;
using JPR = SJP.UserPortal.JourneyPlanRunner;

using Logger = System.Diagnostics.Trace;
using SJP.Common.LocationService;
using System.Collections.Generic;

namespace SJP.TestProject.JourneyPlanRunner
{
    
    
    /// <summary>
    ///This is a test class for CycleJourneyPlanRunnerTest and is intended
    ///to contain all CycleJourneyPlanRunnerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CycleJourneyPlanRunnerTest
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
        [TestInitialize()]
        public void TestInitialize()
        {
            
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

            SJPServiceDiscovery.Init(new TestInitialisation());

            MockSJPSessionFactory.ClearSession();

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.SessionManager, new MockSJPSessionFactory());

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.JourneyPlanRunnerCaller, new JPR.JourneyPlanRunnerCallerFactory());

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.CyclePlannerManager, new CyclePlannerManagerFactory());

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.CycleJourneyPlanRunnerCaller, new CycleJourneyPlanRunnerCallerFactory());
        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        [TestCleanup()]
        public void TestCleanup()
        {
            MockSJPSessionFactory.ClearSession();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }

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
        ///A test for PerformLocationsDistanceValidation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void PerformLocationsDistanceValidationTest()
        {
            CycleJourneyPlanRunner_Accessor target = new CycleJourneyPlanRunner_Accessor();
            ISJPJourneyRequest journeyRequest = InitialiseCycleJourneyRequest();
           
            target.PerformLocationsDistanceValidation(journeyRequest);
            Assert.AreEqual(0, target.listErrors.Count);
        }

        /// <summary>
        ///A test for PerformLocationsDistanceValidation when from location is venue as well
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void PerformLocationsDistanceValidationTest_FromLocationIsVenue()
        {
            CycleJourneyPlanRunner_Accessor target = new CycleJourneyPlanRunner_Accessor();
            ISJPJourneyRequest journeyRequest = InitialiseCycleJourneyRequest();
            //Change the to location to be venue
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
            journeyRequest.Origin = locationService.GetSJPLocation("8100EXL", SJPLocationType.Venue);
            
            target.PerformLocationsDistanceValidation(journeyRequest);
            Assert.AreEqual(0, target.listErrors.Count);
        }

        /// <summary>
        ///A test for PerformLocationsDistanceValidation when from location is venue as well 
        ///and the distance between the two venue is greater than maximum distance allowed
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void PerformLocationsDistanceValidationTest_FromLocationIsVenue_FromToDistanceGreaterThanLimit()
        {
            CycleJourneyPlanRunner_Accessor target = new CycleJourneyPlanRunner_Accessor();
            ISJPJourneyRequest journeyRequest = InitialiseCycleJourneyRequest();
            //Change the to location to be venue
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
            journeyRequest.Origin = locationService.GetSJPLocation("8100ETD", SJPLocationType.Venue);

            target.PerformLocationsDistanceValidation(journeyRequest);
            Assert.IsTrue(target.listErrors.Count>0);
            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DistanceToVenueLocationTooGreat"));
        }

        /// <summary>
        ///A test for PerformLocationsDistanceValidation where from location is at greater distance than the maximum cycle distance allowed
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void PerformLocationsDistanceValidationTest_FromToDistanceGreaterThanLimit()
        {
            CycleJourneyPlanRunner_Accessor target = new CycleJourneyPlanRunner_Accessor();
            ISJPJourneyRequest journeyRequest = InitialiseCycleJourneyRequest();
            //Change the to location to be venue
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
            journeyRequest.Origin = locationService.GetSJPLocation("NG9 1LA", SJPLocationType.Postcode);

            target.PerformLocationsDistanceValidation(journeyRequest);
            Assert.IsTrue(target.listErrors.Count > 0);
            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DistanceBetweenLocationsTooGreat"));
        }

        /// <summary>
        ///A test for PerformPlannerAvailableValidation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void PerformPlannerAvailableValidationTest()
        {
            CycleJourneyPlanRunner_Accessor target = new CycleJourneyPlanRunner_Accessor();
            
            Assert.IsTrue(target.PerformPlannerAvailableValidation());
            
            // Make the cycle planner available property to false and test the method again
            Properties_Accessor properties = new Properties_Accessor(new PrivateObject(Properties.Current));
            properties.propertyDictionary["JourneyPlanner.Switch.CyclePlanner.Available"] = "false";

            Assert.IsFalse(target.PerformPlannerAvailableValidation());

            Assert.IsTrue(target.listErrors.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.CyclePlannerUnavailableKey"));

        }

        /// <summary>
        ///A test for ValidateAndRun when the request only needs validating and not submitting
        ///</summary>
        [TestMethod()]
        public void ValidateAndRunTestValidRequestSubmitFalse()
        {
            CycleJourneyPlanRunner target = new CycleJourneyPlanRunner();
            ISJPJourneyRequest journeyRequest = InitialiseCycleJourneyRequest();
            bool submitRequest = false;
            bool expected = true;
            bool actual;
            actual = target.ValidateAndRun(journeyRequest, LanguageHelper.GetLanguageString(Language.English), submitRequest);
            Assert.AreEqual(expected, actual);
           
        }


        /// <summary>
        ///A test for ValidateAndRun when the request needs validating and submitting
        ///</summary>
        [TestMethod()]
        public void ValidateAndRunTestValidRequestSubmitTrue()
        {
            CycleJourneyPlanRunner target = new CycleJourneyPlanRunner();
            ISJPJourneyRequest journeyRequest = InitialiseCycleJourneyRequest();
            bool submitRequest = true;
            bool expected = true;
            bool actual;
            actual = target.ValidateAndRun(journeyRequest, LanguageHelper.GetLanguageString(Language.English), submitRequest);
            Assert.AreEqual(expected, actual);

        }

        #region Private methods

        /// <summary>
        /// Initialises a cycle journey request with standard request values
        /// for an outward and return journey
        /// </summary>
        /// <returns></returns>
        private ISJPJourneyRequest InitialiseCycleJourneyRequest()
        {
            IPropertyProvider pp = Properties.Current;

            ISJPJourneyRequest request = new SJPJourneyRequest();

            request.JourneyRequestHash = "Test";

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            request.Origin = locationService.GetSJPLocation("E15 2TF", SJPLocationType.Postcode);
            request.Destination = locationService.GetSJPLocation("8100OPK", SJPLocationType.Venue);

            // Populate a cycle park
            List<SJPVenueCyclePark> cycleParks = locationService.GetSJPVenueCycleParks(request.Destination.Naptan);

            if ((cycleParks != null) && (cycleParks.Count > 0))
            {
                SJPVenueLocation venueLocation = request.Destination as SJPVenueLocation;

                venueLocation.SelectedSJPParkID = cycleParks[0].ID;

                request.Destination = venueLocation;
            }


            // Just to ensure the objects are used (for code coverage)
            request.ReturnOrigin = request.Destination;
            request.ReturnDestination = request.Origin;

            // Fix to ensure planning for 2012
            DateTime dtOutward = DateTime.Now;
            DateTime dtOutward2012 = new DateTime(2012, 8, 1, 12, 0, 0);
            if (dtOutward < dtOutward2012)
            {
                dtOutward = dtOutward2012;
            }

            request.OutwardDateTime = dtOutward;
            request.ReturnDateTime = dtOutward.AddHours(3);
            request.OutwardArriveBefore = true;
            request.ReturnArriveBefore = false;
            request.IsReturnRequired = true;

            request.AccessiblePreferences = new SJPAccessiblePreferences();

            // Cycle only
            request.PlannerMode = SJPJourneyPlannerMode.Cycle;
            request.Modes = new System.Collections.Generic.List<SJPModeType>(
                new SJPModeType[1] { SJPModeType.Cycle });


            // Cycle specific
            request.Sequence = 1; // Cycle journey only 1 required

            #region Penalty function

            request.PenaltyFunction = GetCycleAlgorithm(string.Empty);

            #endregion

            #region User preferences

            // The ID of each user preference must match the IDs specified in the cycle planner configuration file.
            List<SJPUserPreference> userPreferences = new List<SJPUserPreference>();

            // Initialise a userpreference (for code coverage)
            SJPUserPreference sjpUserPreference = new SJPUserPreference();
            sjpUserPreference.PreferenceKey = "-1";
            sjpUserPreference.PreferenceValue = "-1";

            // A property that denotes the size of the array of user preferences expected by the Atkins CTP
            int numOfProperties = Convert.ToInt32(pp[JC.Keys.JourneyRequest_UserPreferences_Count]);

            // Build the actual array of user preferences from properties
            // these are used in the request sent to the Atkins CTP.
            for (int i = 0; i < numOfProperties; i++)
            {
                // Override any preferences by User entered/chosen values
                switch (i)
                {
                    //case 5: // Max Speed
                    //    break;
                    //case 6:  // Avoid Time Based Restrictions
                    //    break;
                    //case 12: // Avoid Steep Climbs
                    //    break;
                    //case 13: // Avoid Unlit Roads
                    //    break;
                    //case 14: // Avoid Walking your bike
                    //    break;
                    default:
                        sjpUserPreference = new SJPUserPreference(i.ToString(),
                            pp[string.Format(JC.Keys.JourneyRequest_UserPreferences_Index, i.ToString())]);
                        break;
                }
                userPreferences.Add(sjpUserPreference);
            }

            request.UserPreferences = userPreferences;

            #endregion

            request.JourneyRequestHash = request.GetSJPHashCode().ToString();

            return request;
        }

        /// <summary>
        /// Uses the provided cycle penalty function algorithm to build up a penalty function algorithm
        /// to use for cycle planning. Values are read from properties for the specified algorithm
        /// </summary>
        /// <param name="algorithm">Algorithm name corresponding to properties value, empty will use default algorithm</param>
        /// <returns></returns>
        private string GetCycleAlgorithm(string algorithm)
        {
            IPropertyProvider pp = Properties.Current;

            // penalty function must be formatted as 
            // "Call <location of penalty function assembly file>,<penalty function type name>"
            // e.g. "Call C:\CyclePlannerService\Services\RoadInterfaceHostingService\atk.cp.PenaltyFunctions.dll,
            // AtkinsGlobal.JourneyPlanning.PenaltyFunctions.Fastest"

            string algorithmToUse = algorithm;

            // Construct penalty function using the properties
            if (string.IsNullOrEmpty(algorithmToUse))
            {
                algorithmToUse = pp[JC.Keys.JourneyRequest_PenaltyFunction_Algorithm];
            }

            string dllPath = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLLPath, algorithmToUse)];
            string dll = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLL, algorithmToUse)];
            string prefix = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Prefix, algorithmToUse)];
            string suffix = pp[string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Suffix, algorithmToUse)];

            #region Validate

            // Validate penalty function values
            if (string.IsNullOrEmpty(dllPath) ||
                string.IsNullOrEmpty(dll) ||
                string.IsNullOrEmpty(prefix) ||
                string.IsNullOrEmpty(suffix))
            {
                throw new SJPException(
                    string.Format("Cycle planner penalty function property values for algorithm[{0}] were missing or invalid, check properties[{1}, {2}, {3}, and {4}] are available.",
                        algorithmToUse,
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLLPath, algorithmToUse),
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_DLL, algorithmToUse),
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Prefix, algorithmToUse),
                        string.Format(JC.Keys.JourneyRequest_PenaltyFunction_Suffix, algorithmToUse)),
                    false,
                    SJPExceptionIdentifier.PSMissingProperty);
            }

            if (!dllPath.EndsWith("\\"))
            {
                dllPath = dllPath + "\\";
            }

            if (!prefix.EndsWith("."))
            {
                prefix = prefix + ".";
            }

            #endregion

            string penaltyFunction = string.Format("Call {0}{1}, {2}{3}", dllPath, dll, prefix, suffix);

            return penaltyFunction;
        }

        /// <summary>
        /// Returns true if list of journeys contains a journey with SJPModeType.Cycle
        /// </summary>
        /// <param name="journeys"></param>
        /// <returns></returns>
        private bool HasCycleJourney(List<Journey> journeys)
        {
            List<SJPModeType> journeyModes;

            foreach (Journey journey in journeys)
            {
                if (journey.IsCycleJourney())
                {
                    return true;
                }

                journeyModes = new List<SJPModeType>(journey.GetUsedModes());

                if (journeyModes.Contains(SJPModeType.Cycle))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
