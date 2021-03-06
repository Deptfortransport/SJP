﻿using SJP.UserPortal.JourneyPlanRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;
using JC = SJP.UserPortal.JourneyControl;
using JPR = SJP.UserPortal.JourneyPlanRunner;
using Logger = System.Diagnostics.Trace;
using SJP.Common.ServiceDiscovery;
using SJP.Common.EventLogging;
using System.Collections.Generic;
using SJP.Common;
using SJP.Common.PropertyManager;
using SJP.Common.LocationService;
using SJP.Common.Extenders;
using SJP.UserPortal.StateServer;

namespace SJP.TestProject.JourneyPlanRunner
{
    
    
    /// <summary>
    ///This is a test class for JourneyPlanRunnerCallerTest and is intended
    ///to contain all JourneyPlanRunnerCallerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JourneyPlanRunnerCallerTest
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

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.CJP, new CJPFactory());

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.CJPManager, new CjpManagerFactory());
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
        ///A test for CallCJP
        ///</summary>
        [TestMethod()]
        public void CallCJPTest()
        {
            JourneyPlanRunnerCaller target = new JourneyPlanRunnerCaller();
            ISJPJourneyRequest journeyRequest = InitialiseValidJourneyRequest();
            string sessionID = MockSJPSessionFactory.mockSessionId;
            string lang = "en"; 
            target.CallCJP(journeyRequest, sessionID, lang);
            using (SJPStateServer stateServer = new SJPStateServer())
            {

                // Get the SJPResultManager
                object objResultManager = stateServer.Read(sessionID, SessionManagerKey.KeyResultManager.ID);

                // result manager should have never been created for this test
                Assert.IsNotNull(objResultManager);

                Assert.IsInstanceOfType(objResultManager, typeof(SJPResultManager));

                SJPResultManager resultManager = (SJPResultManager)objResultManager;

                ISJPJourneyResult result = resultManager.GetSJPJourneyResult(journeyRequest.JourneyRequestHash);

                Assert.IsNotNull(result);

            } // StateServer will be disposed, any out

        }

        /// <summary>
        ///A test for InvokeCJP
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        [ExpectedException(typeof(NullReferenceException))]
        public void InvokeCJPTestt_NullJourneyRequest()
        {
            JourneyPlanRunnerCaller_Accessor target = new JourneyPlanRunnerCaller_Accessor();
            ISJPJourneyRequest journeyRequest = null;
            string sessionID = MockSJPSessionFactory.mockSessionId;
            string lang = "en";
            target.CallCJP(journeyRequest, sessionID, lang);
                     
        }

        
        #region Private Helper Methods
        /// <summary>
        /// Initialises a journey request with standard request values
        /// for an outward and return journey
        /// </summary>
        /// <returns></returns>
        private ISJPJourneyRequest InitialiseValidJourneyRequest()
        {
            IPropertyProvider pp = Properties.Current;

            ISJPJourneyRequest request = new SJPJourneyRequest();

            request.JourneyRequestHash = "Test";

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            request.Origin = locationService.GetSJPLocation("9100EDINBUR", SJPLocationType.Station);
            request.Destination = locationService.GetSJPLocation("8100OPK", SJPLocationType.Venue);

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

            // Public
            request.PlannerMode = SJPJourneyPlannerMode.PublicTransport;
            request.Modes = new System.Collections.Generic.List<SJPModeType>(
                new SJPModeType[8] 
                { SJPModeType.Rail, SJPModeType.Bus, SJPModeType.Coach, SJPModeType.Metro, SJPModeType.Underground, 
                  SJPModeType.Tram, SJPModeType.Ferry, SJPModeType.Air});

            request.OutwardJourneyPart = new Journey();
            request.ReturnJourneyPart = new Journey();

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
        /// Initialises a journey request with standard request values
        /// for an outward and return journey
        /// </summary>
        /// <returns></returns>
        private ISJPJourneyRequest InitialiseInValidJourneyRequest()
        {
            IPropertyProvider pp = Properties.Current;

            ISJPJourneyRequest request = new SJPJourneyRequest();

            request.JourneyRequestHash = "Test";

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            request.Origin = locationService.GetSJPLocation("9100EDINBUR", SJPLocationType.Station);
            request.Destination = locationService.GetSJPLocation("9100EDINBUR", SJPLocationType.Station);

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

            // Public
            request.PlannerMode = SJPJourneyPlannerMode.PublicTransport;
            request.Modes = new System.Collections.Generic.List<SJPModeType>(
                new SJPModeType[8] 
                { SJPModeType.Rail, SJPModeType.Bus, SJPModeType.Coach, SJPModeType.Metro, SJPModeType.Underground, 
                  SJPModeType.Tram, SJPModeType.Ferry, SJPModeType.Air});

            request.OutwardJourneyPart = new Journey();
            request.ReturnJourneyPart = new Journey();

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

        /// <summary>
        /// Returns true if list of journeys contains a journey with SJPModeType.Car
        /// </summary>
        /// <param name="journeys"></param>
        /// <returns></returns>
        private bool HasCarJourney(List<Journey> journeys)
        {
            List<SJPModeType> journeyModes;

            foreach (Journey journey in journeys)
            {
                if (journey.IsCarJourney())
                {
                    return true;
                }

                journeyModes = new List<SJPModeType>(journey.GetUsedModes());

                if (journeyModes.Contains(SJPModeType.Car))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
