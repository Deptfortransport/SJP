// *********************************************** 
// NAME             : StopEventManagerTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 26 Apr 2011
// DESCRIPTION  	: StopEventManagerTest test class
// ************************************************
// 
                
using SJP.UserPortal.JourneyControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using SJP.Common.LocationService;
using SJP.Common.ServiceDiscovery;
using System.Runtime.Remoting;
using System.IO;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;

using Logger = System.Diagnostics.Trace;
using SJP.Common;
using JC = SJP.UserPortal.JourneyControl;
using System.Collections.Generic;



namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for StopEventManagerTest and is intended
    ///to contain all StopEventManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StopEventManagerTest
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
            SJPServiceDiscovery.Init(new TestInitialisation());

            try
            {
                //Configure the hosted remoting objects to be a remote object
                string configPath = AppDomain.CurrentDomain.BaseDirectory + @"\Remoting.config";
                if (File.Exists(configPath))
                {
                    RemotingConfiguration.Configure(configPath, false);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "Loaded remoting configuration from " + configPath));
                }
                else
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Could not find remoting configuration file: " + configPath));
            }
            catch 
            {
                // Ignore as it may already have been done
            }
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


        /// <summary>
        ///A test for CallCJP
        ///</summary>
        [TestMethod()]
        public void StopEventManagerCallCJPTest()
        {
            StopEventManagerFactory factory = new StopEventManagerFactory();
            StopEventManager manager = (StopEventManager)factory.Get();
            Assert.IsNotNull(manager, "StopEventManager is null, failed to get StopEventManager using Factory object");

            StopEventManager target = new StopEventManager();
            ISJPJourneyRequest request = InitialiseStopEventRequest();

            string sessionId = "StopEventManagerTest_CallCJPTest";
            bool referenceTransaction = false;
            string language = "en";

            // Perform test
            ISJPJourneyResult actual = target.CallCJP(request, sessionId, referenceTransaction, language);

            // Check journeys were created for the stop event results
            bool hasJourneys = (actual.OutwardJourneys.Count > 0);
            bool hasCorrectNumOfJourneysOutward = (actual.OutwardJourneys.Count == request.Sequence);
            bool hasCorrectNumOfJourneysReturn = (request.IsReturnRequired) ? (actual.ReturnJourneys.Count == request.Sequence) : true;

            // Check test resuls
            Assert.IsNotNull(actual);
            Assert.IsTrue(hasJourneys, "Excepted at least 1 journey in the result");
            Assert.IsTrue(hasCorrectNumOfJourneysOutward,
                            string.Format("Excepted [{0}] outward journeys in the result, result contained [{1}]", request.Sequence, actual.OutwardJourneys.Count));
            Assert.IsTrue(hasCorrectNumOfJourneysReturn,
                            string.Format("Excepted [{0}] return journeys in the result, result contained [{1}]", request.Sequence, actual.ReturnJourneys.Count));
        }

        /// <summary>
        ///A test for CallCJP
        ///</summary>
        [TestMethod()]
        public void StopEventAndCJPTest()
        {
            #region Get Stop Event journeys

            StopEventManager target = new StopEventManager();
            ISJPJourneyRequest request = InitialiseStopEventRequest();

            string sessionId = "StopEventManagerTest_CallCJPTest";
            bool referenceTransaction = false;
            string language = "en";

            // Perform test
            ISJPJourneyResult actual = target.CallCJP(request, sessionId, referenceTransaction, language);

            // Check journeys were created for the stop event results
            bool hasJourneys = (actual.OutwardJourneys.Count > 0);
            bool hasCorrectNumOfJourneysOutward = (actual.OutwardJourneys.Count == request.Sequence);
            bool hasCorrectNumOfJourneysReturn = (request.IsReturnRequired) ? (actual.ReturnJourneys.Count == request.Sequence) : true;

            // Check test resuls
            Assert.IsNotNull(actual);
            Assert.IsTrue(hasJourneys, "Excepted at least 1 journey in the result");
            Assert.IsTrue(hasCorrectNumOfJourneysOutward,
                            string.Format("Excepted [{0}] outward journeys in the result, result contained [{1}]", request.Sequence, actual.OutwardJourneys.Count));
            Assert.IsTrue(hasCorrectNumOfJourneysReturn,
                            string.Format("Excepted [{0}] return journeys in the result, result contained [{1}]", request.Sequence, actual.ReturnJourneys.Count));

            #endregion

            #region Get CJP journeys, passing in the Stop Event journeys

            CJPManager cjpManager = new CJPManager();

            // Initialise a journey request
            ISJPJourneyRequest cjpRequest = InitialiseJourneyRequest();
            
            // Add the stop event journeys
            cjpRequest.OutwardJourneyPart = actual.OutwardJourneys[0];
            cjpRequest.ReturnJourneyPart = actual.ReturnJourneys[0];

            string cjpSessionId = "CJPManagerTest_CallCJPTest1";

            // Perform test, result should contain the journey parts we added
            ISJPJourneyResult cjpResult = cjpManager.CallCJP(cjpRequest, cjpSessionId, referenceTransaction, language);

            hasJourneys = (cjpResult.OutwardJourneys.Count > 0);
            hasCorrectNumOfJourneysOutward = (cjpResult.OutwardJourneys.Count == cjpRequest.Sequence);
            hasCorrectNumOfJourneysReturn = (cjpRequest.IsReturnRequired) ? (cjpResult.ReturnJourneys.Count == cjpRequest.Sequence) : true;

            // Check test resuls
            Assert.IsNotNull(actual);
            Assert.IsTrue(hasJourneys, "Excepted at least 1 journey in the cjp result");
            Assert.IsTrue(hasCorrectNumOfJourneysOutward,
                            string.Format("Excepted [{0}] outward journeys in the cjp result, result contained [{1}]", request.Sequence, actual.OutwardJourneys.Count));
            Assert.IsTrue(hasCorrectNumOfJourneysReturn,
                            string.Format("Excepted [{0}] return journeys in the cjp result, result contained [{1}]", request.Sequence, actual.ReturnJourneys.Count));
            
            #endregion
        }

        #region Private methods

        /// <summary>
        /// Initialises a stop event request with standard request values
        /// for an outward and return journey
        /// </summary>
        /// <returns></returns>
        private ISJPJourneyRequest InitialiseStopEventRequest()
        {
            IPropertyProvider pp = Properties.Current;

            ISJPJourneyRequest request = new SJPJourneyRequest();

            request.JourneyRequestHash = "Test";

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            SJPLocation origin = new SJPLocation("Tower Millennium Pier", SJPLocationType.Station, SJPLocationType.Unknown, "9300TMP");

            SJPLocation destination = new SJPLocation("Greenwich Pier ", SJPLocationType.Station, SJPLocationType.Unknown, "9300GNW1");
            // Set the Locality as it needed by the CJP
            destination.Locality = "E0034328";

            request.Origin = origin;
            request.Destination = destination;

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

            // Ferry 
            request.PlannerMode = SJPJourneyPlannerMode.RiverServices;
            request.Modes = new System.Collections.Generic.List<SJPModeType>(
                new SJPModeType[1] { SJPModeType.Ferry });

            // Stop event specific
            request.Sequence = 3;

            request.JourneyRequestHash = request.GetSJPHashCode().ToString();

            return request;
        }

        /// <summary>
        /// Initialises a journey request with standard request values
        /// for an outward and return journey
        /// </summary>
        /// <returns></returns>
        private ISJPJourneyRequest InitialiseJourneyRequest()
        {
            IPropertyProvider pp = Properties.Current;

            ISJPJourneyRequest request = new SJPJourneyRequest();

            request.JourneyRequestHash = "Test";

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            request.Origin = locationService.GetSJPLocation("9100CAMBDGE", SJPLocationType.Station);
            request.Destination = locationService.GetSJPLocation("8100GRP", SJPLocationType.Venue);

            // Fix to ensure planning for 2012
            DateTime dtOutward = DateTime.Now;
            DateTime dtOutward2012 = new DateTime(2012, 8, 1, 11, 0, 0);
            if (dtOutward < dtOutward2012)
            {
                dtOutward = dtOutward2012;
            }

            request.OutwardDateTime = dtOutward;
            request.ReturnDateTime = dtOutward.AddHours(4);
            request.OutwardArriveBefore = true;
            request.ReturnArriveBefore = false;
            request.IsReturnRequired = true;

            request.AccessiblePreferences = new SJPAccessiblePreferences();

            // Public
            request.PlannerMode = SJPJourneyPlannerMode.PublicTransport;
            request.Modes = new System.Collections.Generic.List<SJPModeType>(
                new SJPModeType[8] 
                { SJPModeType.Rail, SJPModeType.Bus, SJPModeType.Coach, SJPModeType.Metro, SJPModeType.Underground, 
                  SJPModeType.Tram, SJPModeType.Ferry, SJPModeType.Air});
            
            // Public specific
            request.PublicAlgorithm = GetPublicAlgorithm(pp[JC.Keys.JourneyRequest_AlgorithmPublic]);

            request.Sequence = pp[JC.Keys.JourneyRequest_Sequence].Parse(3);
            request.InterchangeSpeed = pp[JC.Keys.JourneyRequest_InterchangeSpeed].Parse(0);
            request.WalkingSpeed = pp[JC.Keys.JourneyRequest_WalkingSpeed].Parse(80);
            request.MaxWalkingTime = pp[JC.Keys.JourneyRequest_MaxWalkingTime].Parse(30);
            request.RoutingGuideInfluenced = pp[JC.Keys.JourneyRequest_RoutingGuideInfluenced].Parse(false);
            request.RoutingGuideCompliantJourneysOnly = pp[JC.Keys.JourneyRequest_RoutingGuideCompliantJourneysOnly].Parse(false);
            request.RouteCodes = pp[JC.Keys.JourneyRequest_RouteCodes];
            request.OlympicRequest = pp[JC.Keys.JourneyRequest_OlympicRequest].Parse(true);

            request.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanOutward];
            request.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanReturn];
            request.RemoveAwkwardOvernight = pp[JC.Keys.JourneyRequest_RemoveAwkwardOvernight].Parse(false);
            
            // Car specific
            request.PrivateAlgorithm = GetPrivateAlgorithm(pp[JC.Keys.JourneyRequest_AlgorithmPrivate]);

            request.AvoidMotorways = pp[JC.Keys.JourneyRequest_AvoidMotorways].Parse(false);
            request.AvoidFerries = pp[JC.Keys.JourneyRequest_AvoidFerries].Parse(false);
            request.AvoidTolls = pp[JC.Keys.JourneyRequest_AvoidTolls].Parse(false);
            request.AvoidRoads = new List<string>();
            request.IncludeRoads = new List<string>();
            request.DrivingSpeed = pp[JC.Keys.JourneyRequest_DrivingSpeed].Parse(112);
            request.DoNotUseMotorways = pp[JC.Keys.JourneyRequest_DoNotUseMotorways].Parse(false);
            request.FuelConsumption = pp[JC.Keys.JourneyRequest_FuelConsumption];
            request.FuelPrice = pp[JC.Keys.JourneyRequest_FuelPrice];

            // Hash
            request.JourneyRequestHash = request.GetSJPHashCode().ToString();

            return request;
        }

        /// <summary>
        /// Converts a string into a SJPPublicAlgorithmType. If unable to parse, Default is returned 
        /// and a warning logged
        /// </summary>
        private SJPPublicAlgorithmType GetPublicAlgorithm(string algorithm)
        {
            SJPPublicAlgorithmType algorithmType = SJPPublicAlgorithmType.Default;
            try
            {
                algorithmType = (SJPPublicAlgorithmType)Enum.Parse(typeof(SJPPublicAlgorithmType), algorithm, true);
            }
            catch
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                    string.Format("Failed to parse algorithm string[{0}] into an SJPPublicAlgorithmType, check property[{1}] contains a valid value for this algorithm type.",
                                   algorithm,
                                   JC.Keys.JourneyRequest_AlgorithmPublic)));
            }

            return algorithmType;
        }

        /// <summary>
        /// Converts a string into a SJPPrivateAlgorithmType. If unable to parse, Fastest is returned 
        /// and a warning logged
        /// </summary>
        private SJPPrivateAlgorithmType GetPrivateAlgorithm(string algorithm)
        {
            SJPPrivateAlgorithmType algorithmType = SJPPrivateAlgorithmType.Fastest;
            try
            {
                algorithmType = (SJPPrivateAlgorithmType)Enum.Parse(typeof(SJPPrivateAlgorithmType), algorithm, true);
            }
            catch
            {
                Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Warning,
                    string.Format("Failed to parse algorithm string[{0}] into an SJPPrivateAlgorithmType, check property[{1}] contains a valid value for this algorithm type.",
                                   algorithm,
                                   JC.Keys.JourneyRequest_AlgorithmPrivate)));
            }

            return algorithmType;
        }

        #endregion
    }
}
