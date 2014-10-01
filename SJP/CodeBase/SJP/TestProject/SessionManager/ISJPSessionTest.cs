using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;
using System.Web;
using System.Web.SessionState;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ISJPSessionTest and is intended
    ///to contain all ISJPSessionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ISJPSessionTest
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


        internal virtual ISJPSession CreateISJPSession()
        {
            ISJPSession target = new SJPSession();
            return target;
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest()
        {
            ISJPSession target = CreateISJPSession();
            DateKey key = new DateKey("keyName");
            DateTime expected = DateTime.Now;
            DateTime actual;
            target[key] = expected;
            actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest1()
        {
            ISJPSession target = CreateISJPSession();
            BoolKey key = new BoolKey("keyName");
            bool expected = true;
            bool actual;
            target[key] = expected;
            actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest2()
        {
            ISJPSession target = CreateISJPSession();
            PageIdKey key = new PageIdKey("keyName");
            PageId expected = PageId.Homepage;
            PageId actual;
            target[key] = expected;
            actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest3()
        {
            ISJPSession target = CreateISJPSession();
            IntKey key = new IntKey("keyName");
            int expected = 7;
            int actual;
            target[key] = expected;
            actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest4()
        {
            ISJPSession target = CreateISJPSession();
            StringKey key = new StringKey("keyName");
            string expected = "keyValue";
            string actual;
            target[key] = expected;
            actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest5()
        {
            ISJPSession target = CreateISJPSession();
            DoubleKey key = new DoubleKey("keyName");
            double expected = 3.4E+8;
            double actual;
            target[key] = expected;
            actual = target[key];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SessionID
        ///</summary>
        [TestMethod()]
        public void SessionIDTest()
        {
            ISJPSession target = CreateISJPSession(); 
            string actual;
            actual = target.SessionID;
            Assert.IsTrue(!string.IsNullOrEmpty(actual), "Null or empty session Id returned");
        }
    }
}
