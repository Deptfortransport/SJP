using SJP.UserPortal.JourneyPlanRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.JourneyControl;
using SJP.UserPortal.SessionManager;
using SJP.Common.ServiceDiscovery;

using JC = SJP.UserPortal.JourneyControl;
using JPR = SJP.UserPortal.JourneyPlanRunner;
using Logger = System.Diagnostics.Trace;
using SJP.Common.PropertyManager;
using SJP.Common.LocationService;
using SJP.Common;
using SJP.UserPortal.StateServer;

namespace SJP.TestProject.JourneyPlanRunner
{
    
    
    /// <summary>
    ///This is a test class for StopEventRunnerCallerTest and is intended
    ///to contain all StopEventRunnerCallerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StopEventRunnerCallerTest
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

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.StopEventRunnerCaller, new StopEventRunnerCallerFactory());

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.StopEventManager, new StopEventManagerFactory());
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
        ///A test for CallStopEvent
        ///</summary>
        [TestMethod()]
        public void CallStopEventTest()
        {
            StopEventRunnerCaller target = new StopEventRunnerCaller();
            ISJPJourneyRequest journeyRequest = InitialiseValidStopEventRequest();
            string sessionID = MockSJPSessionFactory.mockSessionId;
            string lang = "en";
            target.CallStopEvent(journeyRequest, sessionID, lang);
            
            using (SJPStateServer stateServer = new SJPStateServer())
            {

                // Get the SJPResultManager
                object objResultManager = stateServer.Read(sessionID, SessionManagerKey.KeyStopEventResultManager.ID);

                // result manager should have never been created for this test
                Assert.IsNotNull(objResultManager);

                Assert.IsInstanceOfType(objResultManager, typeof(SJPStopEventResultManager));

                SJPStopEventResultManager resultManager = (SJPStopEventResultManager)objResultManager;

                ISJPJourneyResult result = resultManager.GetSJPJourneyResult(journeyRequest.JourneyRequestHash);

                Assert.IsNotNull(result);

            } // StateServer will be disposed, any out
        }

        /// <summary>
        ///A test for InvokeStopEvent with journey request passed as null
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.journeyplanrunner.dll")]
        public void InvokeStopEventTest_NullJourneyRequest()
        {
            StopEventRunnerCaller_Accessor target = new StopEventRunnerCaller_Accessor();
            ISJPJourneyRequest journeyRequest = null;
            string sessionID = MockSJPSessionFactory.mockSessionId;
            string lang = "en";
            target.InvokeStopEvent(journeyRequest, sessionID, lang);


            using (SJPStateServer stateServer = new SJPStateServer())
            {

                // Get the SJPResultManager
                object objResultManager = stateServer.Read(sessionID, SessionManagerKey.KeyStopEventResultManager.ID);

                // result manager should have never been created for this test
                Assert.IsNull(objResultManager);


            } // StateServer will be disposed, any out
           
        }

        #region Private methods

        /// <summary>
        /// Initialises a stop event request with standard request values
        /// for an outward and return journey
        /// </summary>
        /// <returns></returns>
        private ISJPJourneyRequest InitialiseValidStopEventRequest()
        {
            IPropertyProvider pp = Properties.Current;

            ISJPJourneyRequest request = new SJPJourneyRequest();

            request.JourneyRequestHash = "Test";

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            SJPLocation origin = new SJPLocation("Tower Millennium Pier", SJPLocationType.Station, SJPLocationType.Unknown, "9300TMP");

            SJPLocation destination = new SJPLocation("Greenwich Pier ", SJPLocationType.Station, SJPLocationType.Unknown, "9300GNW");
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

        

        #endregion

        
    }
}
