// *********************************************** 
// NAME             : CyclePlannerManagerTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 04 Apr 2011
// DESCRIPTION  	: CyclePlannerManagerTest test class
// ************************************************
// 

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common;
using SJP.Common.LocationService;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.JourneyControl;
using JC = SJP.UserPortal.JourneyControl;

namespace SJP.TestProject
{   
    /// <summary>
    ///This is a test class for CyclePlannerManagerTest and is intended
    ///to contain all CyclePlannerManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CyclePlannerManagerTest
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
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region Tests

        /// <summary>
        ///A test for CyclePlannerManager Constructor
        ///</summary>
        [TestMethod()]
        public void CyclePlannerManagerConstructorTest()
        {
            CyclePlannerManager target = new CyclePlannerManager();

            Assert.IsNotNull(target, "CyclePlannerManager is null, failed to create CyclePlannerManager object");

            CyclePlannerManagerFactory factory = new CyclePlannerManagerFactory();

            CyclePlannerManager manager = (CyclePlannerManager)factory.Get();

            Assert.IsNotNull(manager, "CyclePlannerManager is null, failed to get CyclePlannerManager using Factory object");
        }

        /// <summary>
        ///A test for CallCyclePlanner
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeycontrol.dll")]
        public void CallCyclePlannerTest()
        {
            CyclePlannerManager_Accessor target = new CyclePlannerManager_Accessor();

            target.sessionId = "CyclePlannerTest_CallCyclePlannerTest";
            target.referenceTransaction = false;
            target.userType = 0;
            target.language = "en";

            // Initialise a cycle journey request
            ISJPJourneyRequest request = InitialiseCycleJourneyRequest();

            // Perform test
            ISJPJourneyResult actual = target.CallCyclePlanner(request);

            // Check for journeys
            bool hasJourneys = (actual.OutwardJourneys.Count > 0);
            bool hasCorrectNumOfJourneysOutward = (actual.OutwardJourneys.Count == request.Sequence);
            bool hasCorrectNumOfJourneysReturn = (request.IsReturnRequired) ? (actual.ReturnJourneys.Count == request.Sequence) : true;
            bool hasCycleJourneyOutward = (request.Modes.Contains(SJPModeType.Cycle)) ? (HasCycleJourney(actual.OutwardJourneys)) : true;
            bool hasCycleJourneyReturn = ((request.Modes.Contains(SJPModeType.Cycle)) && (request.IsReturnRequired)) ? (HasCycleJourney(actual.ReturnJourneys)) : true;

            // Check test resuls
            Assert.IsNotNull(actual);
            Assert.IsTrue(hasJourneys, "Excepted at least 1 journey in the result");
            Assert.IsTrue(hasCorrectNumOfJourneysOutward,
                            string.Format("Excepted [{0}] outward journeys in the result, result contained [{1}]", request.Sequence, actual.OutwardJourneys.Count));
            Assert.IsTrue(hasCorrectNumOfJourneysReturn,
                            string.Format("Excepted [{0}] return journeys in the result, result contained [{1}]", request.Sequence, actual.ReturnJourneys.Count));
            Assert.IsTrue(hasCycleJourneyOutward, "Expect [1] outward cycle journey in the result, result contained [0]");
            Assert.IsTrue(hasCycleJourneyReturn, "Expect [1] return cycle journey in the result, result contained [0]");
        }

        /// <summary>
        ///A test for CallCyclePlanner
        ///</summary>
        [TestMethod()]
        public void CallCyclePlannerTest1()
        {
            CyclePlannerManager target = new CyclePlannerManager();

            // Initialise a cycle journey request
            ISJPJourneyRequest request = InitialiseCycleJourneyRequest();

            // Only test for outward journey
            request.IsReturnRequired = false;

            string sessionId = "CyclePlannerTest_CallCyclePlannerTest1";
            bool referenceTransaction = false;
            string language = "en";
            
            // Perform test
            ISJPJourneyResult actual = target.CallCyclePlanner(request, sessionId, referenceTransaction, language);

            // Check for journeys
            bool hasJourneys = (actual.OutwardJourneys.Count > 0);
            bool hasCorrectNumOfJourneysOutward = (actual.OutwardJourneys.Count == request.Sequence);
            bool hasCorrectNumOfJourneysReturn = (request.IsReturnRequired) ? (actual.ReturnJourneys.Count == request.Sequence) : true;
            bool hasCycleJourneyOutward = (request.Modes.Contains(SJPModeType.Cycle)) ? (HasCycleJourney(actual.OutwardJourneys)) : true;
            bool hasCycleJourneyReturn = ((request.Modes.Contains(SJPModeType.Cycle)) && (request.IsReturnRequired)) ? (HasCycleJourney(actual.ReturnJourneys)) : true;

           // Check test resuls
            Assert.IsNotNull(actual);
            Assert.IsTrue(hasJourneys, "Excepted at least 1 journey in the result");
            Assert.IsTrue(hasCorrectNumOfJourneysOutward,
                            string.Format("Excepted [{0}] outward journeys in the result, result contained [{1}]", request.Sequence, actual.OutwardJourneys.Count));
            Assert.IsTrue(hasCorrectNumOfJourneysReturn,
                            string.Format("Excepted [{0}] return journeys in the result, result contained [{1}]", request.Sequence, actual.ReturnJourneys.Count));
            Assert.IsTrue(hasCycleJourneyOutward, "Expect [1] outward cycle journey in the result, result contained [0]");
            Assert.IsTrue(hasCycleJourneyReturn, "Expect [1] return cycle journey in the result, result contained [0]");
        
        }

        /// <summary>
        ///A test for CallCyclePlanner
        ///</summary>
        [TestMethod()]
        public void CallCyclePlannerTest2()
        {
            CyclePlannerManager target = new CyclePlannerManager();

            // Initialise a cycle journey request
            ISJPJourneyRequest request = InitialiseCycleJourneyRequest();

            // overwrite default locations
            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);
            request.Origin = locationService.GetSJPLocation("N7 0EE", SJPLocationType.Postcode);
            request.Destination = locationService.GetSJPLocation("8100EAR", SJPLocationType.Venue);

            // Populate a cycle park
            List<SJPVenueCyclePark> cycleParks = locationService.GetSJPVenueCycleParks(request.Destination.Naptan);

            if ((cycleParks != null) && (cycleParks.Count > 0))
            {
                SJPVenueLocation venueLocation = request.Destination as SJPVenueLocation;

                venueLocation.SelectedSJPParkID = cycleParks[0].ID;

                request.Destination = venueLocation;
            }

            // Only test for outward journey
            request.IsReturnRequired = false;

            string sessionId = "CyclePlannerTest_CallCyclePlannerTest1";
            bool referenceTransaction = false;
            string language = "en";

            // Perform test
            ISJPJourneyResult actual = target.CallCyclePlanner(request, sessionId, referenceTransaction, language);

            // Check for journeys
            bool hasJourneys = (actual.OutwardJourneys.Count > 0);
            bool hasCorrectNumOfJourneysOutward = (actual.OutwardJourneys.Count == request.Sequence);
            bool hasCorrectNumOfJourneysReturn = (request.IsReturnRequired) ? (actual.ReturnJourneys.Count == request.Sequence) : true;
            bool hasCycleJourneyOutward = (request.Modes.Contains(SJPModeType.Cycle)) ? (HasCycleJourney(actual.OutwardJourneys)) : true;
            bool hasCycleJourneyReturn = ((request.Modes.Contains(SJPModeType.Cycle)) && (request.IsReturnRequired)) ? (HasCycleJourney(actual.ReturnJourneys)) : true;

            // Check test resuls
            Assert.IsNotNull(actual);
            Assert.IsTrue(hasJourneys, "Excepted at least 1 journey in the result");
            Assert.IsTrue(hasCorrectNumOfJourneysOutward,
                            string.Format("Excepted [{0}] outward journeys in the result, result contained [{1}]", request.Sequence, actual.OutwardJourneys.Count));
            Assert.IsTrue(hasCorrectNumOfJourneysReturn,
                            string.Format("Excepted [{0}] return journeys in the result, result contained [{1}]", request.Sequence, actual.ReturnJourneys.Count));
            Assert.IsTrue(hasCycleJourneyOutward, "Expect [1] outward cycle journey in the result, result contained [0]");
            Assert.IsTrue(hasCycleJourneyReturn, "Expect [1] return cycle journey in the result, result contained [0]");

        }                
        /// <summary>
        ///A test for LogRequest
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeycontrol.dll")]
        public void LogRequestTest()
        {
            CyclePlannerManager_Accessor target = new CyclePlannerManager_Accessor();
            
            // Initialise a request, LogRequest will only use the modes value
            ISJPJourneyRequest request = InitialiseCycleJourneyRequest();

            string requestId = "CyclePlannerTest_LogRequestTest";
            bool isLoggedOn = false;
            string sessionId = "LogRequestTest";

            // Perform test
            target.LogRequest(request, requestId, isLoggedOn, sessionId);

            // Method will write the request event to a log, cannot verify test here
        }

        /// <summary>
        ///A test for LogResponse
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeycontrol.dll")]
        public void LogResponseTest()
        {
            CyclePlannerManager_Accessor target = new CyclePlannerManager_Accessor();
           
            // LogResponse will write a result event to a log, the sucsess value can be Failure, ZeroResults, or Results

            // Initialise a result
            ISJPJourneyResult result = new SJPJourneyResult();

            string requestId = "CyclePlannerTest_LogResponseTest";
            bool isLoggedOn = false;
            bool cyclePlannerFailed = true;
            string sessionId = "LogResponseTest";

            // Perform Test 1 - Failure type
            target.LogResponse(result, requestId, isLoggedOn, cyclePlannerFailed, sessionId);

            // Perform Test 1 - ZeroResults type
            cyclePlannerFailed = false;
            target.LogResponse(result, requestId, isLoggedOn, cyclePlannerFailed, sessionId);

            // Perform Test 1 - Results type
            result.OutwardJourneys.Add(new Journey());
            target.LogResponse(result, requestId, isLoggedOn, cyclePlannerFailed, sessionId);

            // Method will write the result event to a log, cannot verify test here
        }

        #endregion

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
                new SJPModeType[1] 
                { SJPModeType.Cycle});

            
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
