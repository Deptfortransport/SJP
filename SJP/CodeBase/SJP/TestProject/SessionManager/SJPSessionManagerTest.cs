using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;
using System.Web.SessionState;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPSessionManagerTest and is intended
    ///to contain all SJPSessionManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPSessionManagerTest
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
        //
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
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                HttpContext.Current = new HttpContext(new HttpRequest("", "http://localhost/", ""), new HttpResponse(sw));
            }

            System.Web.SessionState.SessionStateUtility.AddHttpSessionStateToContext(HttpContext.Current,
                new HttpSessionStateContainer("SessionId", new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 20000, true, HttpCookieMode.UseCookies, SessionStateMode.Off, false));

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            HttpContext.Current = null;
        }
        //
        #endregion


        /// <summary>
        ///A test for SJPSessionManager Constructor
        ///</summary>
        [TestMethod()]
        public void SJPSessionManagerConstructorTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                Assert.IsNotNull(target, "Null object returned");
            }
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.sessionmanager.dll")]
        public void DisposeTest()
        {
            SJPSessionFactory factory = new SJPSessionFactory();
            SJPSessionManager_Accessor target = new SJPSessionManager_Accessor(factory);
            bool disposing = true;
            target.Dispose(disposing);
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest1()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            SJPSessionManager target = new SJPSessionManager(sessFactory);
            target.Dispose();
        }

        /// <summary>
        ///A test for Finalize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.sessionmanager.dll")]
        public void FinalizeTest()
        {
            SJPSessionFactory factory = new SJPSessionFactory();
            using (SJPSessionManager_Accessor target = new SJPSessionManager_Accessor(factory))
            {
                target.Finalize();
            }
        }

        /// <summary>
        ///A test for GetData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.sessionmanager.dll")]
        public void GetDataTest()
        {
            SJPSessionFactory factory = new SJPSessionFactory();
            using (SJPSessionManager_Accessor target = new SJPSessionManager_Accessor(factory))
            {
                IKey key = new StringKey("keyName");

                // Get non-existant key (force manager to go to session state server
                object notThere = target.GetData(key);
                Assert.IsNull(notThere, "Expected null as no key was uploaded");

                string expected = "keyValue";
                target.SetData(key, expected);
                string actual = (string)target.GetData(key);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for OnFormShift
        ///</summary>
        [TestMethod()]
        public void OnFormShiftTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                target.OnFormShift();
            }
        }

        /// <summary>
        ///A test for OnLoad
        ///</summary>
        [TestMethod()]
        public void OnLoadTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                target.OnLoad();
            }
        }

        /// <summary>
        ///A test for OnPreUnload
        ///</summary>
        [TestMethod()]
        public void OnPreUnloadTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                target.OnPreUnload();
            }
        }

        /// <summary>
        ///A test for OnUnload
        ///</summary>
        [TestMethod()]
        public void OnUnloadTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                target.OnUnload();
            }
        }

        /// <summary>
        ///A test for SaveData
        ///</summary>
        [TestMethod()]
        public void SaveDataTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager_Accessor targetAccessor = new SJPSessionManager_Accessor(sessFactory))
            {
                // Create a couple of keys to save then set one to null to go down the other code path
                StringKey key1 = new StringKey("key1");
                targetAccessor.SetData(key1, "keyValue1");
                StringKey key2 = new StringKey("key2");
                targetAccessor.SetData(key2, null);

                targetAccessor.SaveData();
            }
        }

        /// <summary>
        ///A test for SetData
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.sessionmanager.dll")]
        public void SetDataTest()
        {
            SJPSessionFactory factory = new SJPSessionFactory();
            using (SJPSessionManager_Accessor target = new SJPSessionManager_Accessor(factory))
            {
                IKey key = new StringKey("keyValue");
                string expected = "keyValue";
                target.SetData(key, expected);
                Assert.AreEqual(expected, target.GetData(key), "Stored value differs from expected");
            }
        }

        /// <summary>
        ///A test for Current
        ///</summary>
        [TestMethod()]
        public void CurrentTest()
        {
            ISJPSessionManager actual = SJPSessionManager.Current;
            Assert.IsNotNull(actual, "Null object returned");
        }

        /// <summary>
        ///A test for JourneyState
        ///</summary>
        [TestMethod()]
        public void JourneyStateTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                JourneyViewState expected = new JourneyViewState();
                JourneyViewState actual;
                target.JourneyState = expected;
                actual = target.JourneyState;
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for PageState
        ///</summary>
        [TestMethod()]
        public void PageStateTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                InputPageState expected = new InputPageState();
                InputPageState actual;
                target.PageState = expected;
                actual = target.PageState;
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for RequestManager
        ///</summary>
        [TestMethod()]
        public void RequestManagerTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                SJPRequestManager expected = new SJPRequestManager();
                SJPRequestManager actual;
                target.RequestManager = expected;
                actual = target.RequestManager;
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for ResultManager
        ///</summary>
        [TestMethod()]
        public void ResultManagerTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                SJPResultManager expected = new SJPResultManager();
                SJPResultManager actual;
                target.ResultManager = expected;
                actual = target.ResultManager;
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for Session
        ///</summary>
        [TestMethod()]
        public void SessionTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                ISJPSession actual;
                actual = target.Session;
                Assert.IsNotNull(target, "Null object returned");
            }
        }

        /// <summary>
        ///A test for StopEventRequestManager
        ///</summary>
        [TestMethod()]
        public void StopEventRequestManagerTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                SJPStopEventRequestManager expected = new SJPStopEventRequestManager();
                SJPStopEventRequestManager actual;
                target.StopEventRequestManager = expected;
                actual = target.StopEventRequestManager;
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for StopEventResultManager
        ///</summary>
        [TestMethod()]
        public void StopEventResultManagerTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                SJPStopEventResultManager expected = new SJPStopEventResultManager();
                SJPStopEventResultManager actual;
                target.StopEventResultManager = expected;
                actual = target.StopEventResultManager;
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for TravelNewsPageState
        ///</summary>
        [TestMethod()]
        public void TravelNewsPageStateTest()
        {
            ISJPSessionFactory sessFactory = new SJPSessionFactory();
            using (SJPSessionManager target = new SJPSessionManager(sessFactory))
            {
                TravelNewsPageState expected = new TravelNewsPageState();
                TravelNewsPageState actual;
                target.TravelNewsPageState = expected;
                actual = target.TravelNewsPageState;
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
