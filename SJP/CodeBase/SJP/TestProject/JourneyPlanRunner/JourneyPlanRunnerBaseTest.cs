using SJP.UserPortal.JourneyPlanRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using System.Collections.Generic;
using SJP.Common;
using JC = SJP.UserPortal.JourneyControl;
using JPR = SJP.UserPortal.JourneyPlanRunner;
using Logger = System.Diagnostics.Trace;
using SJP.Common.ServiceDiscovery;
using SJP.Common.PropertyManager;
using SJP.Common.LocationService;
using SJP.Common.EventLogging;
using SJP.Common.Extenders;

namespace SJP.TestProject.JourneyPlanRunner
{
    
    
    /// <summary>
    ///This is a test class for JourneyPlanRunnerBaseTest and is intended
    ///to contain all JourneyPlanRunnerBaseTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JourneyPlanRunnerBaseTest
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
        ///A test for PerformDateValidations
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void PerformDateValidationsTest()
        {
            PrivateObject param0 = new PrivateObject(new JPR.JourneyPlanRunner());
            JourneyPlanRunnerBase_Accessor target = new JourneyPlanRunnerBase_Accessor(param0);
            ISJPJourneyRequest journeyRequest = InitialiseValidJourneyRequest();
            target.PerformDateValidations(journeyRequest);

            Assert.AreEqual(0, target.Messages.Count);
            DateTime origOutwardDate = journeyRequest.OutwardDateTime;

            //------------------------------ OUTWARD DATE -------------------------------

            // Invalid outward journey date - date time is not set
            journeyRequest.OutwardDateTime = DateTime.MinValue;
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateNotValid"));
            
            // Invalid outward journey date - date time is less that current date
            journeyRequest.OutwardDateTime = DateTime.Now.AddDays(-1);
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count>0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateTimeIsInThePast"));

            // Invalid outward journey date - date time is less that event start date
            journeyRequest.OutwardDateTime = new DateTime(2012, 07, 17);
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            // In case the test is run after the games start date, then do not fail
            if (journeyRequest.OutwardDateTime > DateTime.Now)
                Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateTimeIsBeforeEvent"));

            // Invalid outward journey date - date time is greater that event start date
            journeyRequest.OutwardDateTime = new DateTime(2012, 12, 1);
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateTimeIsAfterEvent"));

            // As this date is greater than the return date set in the journey request
            // There will be an additional message about outward date is greater than return date
            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.OutwardDateIsAfterReturnDate"));

            // reset the outward date
            journeyRequest.OutwardDateTime = origOutwardDate;

            //------------------------------ RETURN DATE -------------------------------

            // Invalid return journey date - date time is not set
            journeyRequest.ReturnDateTime = DateTime.MinValue;
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateNotValid"));

            // Invalid return journey date - date time is less that current date
            journeyRequest.ReturnDateTime = DateTime.Now.AddDays(-1);
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateTimeIsInThePast"));

            // Invalid return journey date - date time is less that event start date
            journeyRequest.ReturnDateTime = new DateTime(2012, 07, 17);
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            // In case the test is run after the games start date, then do not fail
            if (journeyRequest.ReturnDateTime > DateTime.Now)
                Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateTimeIsBeforeEvent"));

            // Invalid return journey date - date time is greater that event start date
            journeyRequest.ReturnDateTime = new DateTime(2012, 12, 1);
            target.PerformDateValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.DateTimeIsAfterEvent"));

           
        }

        /// <summary>
        ///A test for PerformLocationValidations
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void PerformLocationValidationsTest()
        {
            PrivateObject param0 = new PrivateObject(new JPR.JourneyPlanRunner());
            JourneyPlanRunnerBase_Accessor target = new JourneyPlanRunnerBase_Accessor(param0);
            ISJPJourneyRequest journeyRequest = InitialiseValidJourneyRequest();
            target.PerformLocationValidations(journeyRequest);

            Assert.AreEqual(0, target.Messages.Count);

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            SJPLocation orig_Origin = journeyRequest.Origin;
            SJPLocation orig_Destination = journeyRequest.Destination;

            // No venue supplied - both locations are not venue
            journeyRequest.Destination = locationService.GetSJPLocation("NG9 1LA", SJPLocationType.Postcode);
            target.PerformLocationValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.AtleastOneLocationShouleBeVenue"));

            // Both locations are venue and are the same venues
            journeyRequest.Destination = orig_Destination;
            journeyRequest.Origin = locationService.GetSJPLocation("8100OPK", SJPLocationType.Venue);
            target.PerformLocationValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.OriginAndDestinationAreSame"));

            // Both locations are venue and are Parent - Child
            journeyRequest.Destination = locationService.GetSJPLocation("8100AQC", SJPLocationType.Venue);
            journeyRequest.Origin = locationService.GetSJPLocation("8100OPK", SJPLocationType.Venue);
            target.PerformLocationValidations(journeyRequest);

            Assert.IsTrue(target.Messages.Count > 0);

            Assert.IsTrue(target.listErrors.ContainsKey("ValidateAndRun.OriginAndDestinationOverlaps"));
            
            
            
        }

        /// <summary>
        ///A test for SetValidationError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void SetValidationErrorTest1()
        {
            PrivateObject param0 = new PrivateObject(new JPR.JourneyPlanRunner());
            JourneyPlanRunnerBase_Accessor target = new JourneyPlanRunnerBase_Accessor(param0);
            string msgResourceID = "Test";
            List<string> msgArgs = new List<string>(new string[]{"ta1","ta2"});
            target.SetValidationError(msgResourceID, msgArgs);
            Assert.IsTrue(target.listErrors.ContainsKey(msgResourceID));
            Assert.IsNotNull(target.listErrors[msgResourceID]);
            Assert.IsTrue(target.listErrors[msgResourceID].MessageArgs.Count == 2);
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
