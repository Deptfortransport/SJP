using SJP.Common.ServiceDiscovery;
using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;
using System.Web.SessionState;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPSessionFactoryTest and is intended
    ///to contain all SJPSessionFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPSessionFactoryTest
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
        ///A test for SJPSessionFactory Constructor
        ///</summary>
        [TestMethod()]
        public void SJPSessionFactoryConstructorTest()
        {
            SJPSessionFactory target = new SJPSessionFactory();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            SJPSessionFactory target = new SJPSessionFactory(); 
            object actual;
            actual = target.Get();
            Assert.IsNotNull(actual, "Null object returned");
            
            // Run Get again as this time it will come from the cache
            Assert.AreEqual(actual, target.Get(), "Second get returned different object");
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            SJPSessionFactory target = new SJPSessionFactory(); 

            // Make sure an object exists first
            target.Get();
            target.Remove();
        }
    }
}
