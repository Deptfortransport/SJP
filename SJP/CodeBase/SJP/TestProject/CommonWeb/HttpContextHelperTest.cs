using System.Web;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common.Web;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for HttpContextHelperTest and is intended
    ///to contain all HttpContextHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HttpContextHelperTest
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

                System.Web.SessionState.SessionStateUtility.AddHttpSessionStateToContext(HttpContext.Current,
                    new HttpSessionStateContainer("SessionId", new SessionStateItemCollection(), new HttpStaticObjectsCollection(), 20000, true, HttpCookieMode.UseCookies, SessionStateMode.Off, false));
            }
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
        ///A test for GetCurrent
        ///</summary>
        [TestMethod()]
        public void GetCurrentTest()
        {
            HttpContext actual = HttpContextHelper.GetCurrent();
            HttpContext expected = HttpContext.Current;
            Assert.AreEqual(expected, actual);
        }
    }
}
