using System;
using System.Globalization;
using System.Web;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common;
using SJP.Common.Web;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for CurrentLanguageTest and is intended
    ///to contain all CurrentLanguageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CurrentLanguageTest
    {


        private string cookieName = "SJP";
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
        ///A test for GetLanguageString
        ///</summary>
        [TestMethod()]
        public void GetLanguageStringTest()
        {
            string expected = "en";  // english
            string actual;
            actual = LanguageHelper.GetLanguageString(Language.English);

            Assert.AreEqual(expected, actual);

            expected = "fr"; // french
            actual = LanguageHelper.GetLanguageString(Language.French);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetValueTest()
        {
            // Get the language from the cookie
            Language expected = Language.French;
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie[PersistentCookie_Accessor.KeyCurrentLanguage] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            Language actual = CurrentLanguage_Accessor.GetValue(context);
            Assert.AreEqual(expected, actual, "Unexpected language value returned");

            // Get the language from the session
            expected = Language.English;
            context.Session[CurrentLanguage_Accessor.siteLanguageKeyName] = expected;
            actual = CurrentLanguage_Accessor.GetValue(context);
            Assert.AreEqual(expected, actual, "Unexpected language value returned");
        }

        /// <summary>
        ///A test for ParseLanguage
        ///</summary>
        [TestMethod()]
        public void ParseLanguageTest()
        {
            string cultureName = "en"; // english
            Language expected = Language.English;
            Language actual;
            actual = LanguageHelper.ParseLanguage(cultureName);
            Assert.AreEqual(expected, actual);

            cultureName = "fr"; // french
            expected = Language.French;
            actual = LanguageHelper.ParseLanguage(cultureName);
            Assert.AreEqual(expected, actual);

            // error condition
            cultureName = "en-GB";

            try
            {
                actual = LanguageHelper.ParseLanguage(cultureName);
                Assert.Fail("Exception expected.");
            }
            catch (SJPException ex)
            {
                if (!string.Equals("Language en-GB not handled", ex.Message, StringComparison.CurrentCultureIgnoreCase))
                {
                    Assert.Fail(string.Format("Unexpected exception received: {0}", ex.ToString()));
                }
            }
        }

        /// <summary>
        ///A test for SetSessionValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetSessionValueTest()
        {
            HttpContext context = HttpContext.Current;
            Language expected = Language.French;
            CurrentLanguage_Accessor.SetSessionValue(context, expected);
            Language actual = CurrentLanguage_Accessor.GetValue(context);
            Assert.AreEqual(expected, actual, "Unexpected language returned");
        }

        /// <summary>
        ///A test for SetValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetValueTest()
        {
            HttpContext context = HttpContext.Current; 
            Language expected = Language.French; 
            CurrentLanguage_Accessor.SetValue(context, expected);
            Assert.AreEqual(expected, CurrentLanguage_Accessor.GetValue(context), "Unexpected language value returned from session");
            Assert.AreEqual(expected, PersistentCookie.CurrentLanguage, "Unexpected language value in cookie"); 
        }

        /// <summary>
        ///A test for Culture
        ///</summary>
        [TestMethod()]
        public void CultureTest()
        {
            string expected = "fr";
            CurrentLanguage.Value = Language.French;
            string actual = CurrentLanguage.Culture;
            Assert.AreEqual(expected, actual, "Unexpected language value returned");

            expected = "en";
            CurrentLanguage.Value = Language.English;
            actual = CurrentLanguage.Culture;
            Assert.AreEqual(expected, actual, "Unexpected language value returned");
        }

        /// <summary>
        ///A test for CurrentCultureInfo
        ///</summary>
        [TestMethod()]
        public void CurrentCultureInfoTest()
        {
            CultureInfo expected = new CultureInfo("en-GB");
            CultureInfo actual = CurrentLanguage.CurrentCultureInfo;
            Assert.AreEqual(expected, actual, "Unexpected default language returned");

            expected = new CultureInfo("fr-FR");
            CurrentLanguage.Value = Language.French;
            actual = CurrentLanguage.CurrentCultureInfo;
            Assert.AreEqual(expected, actual, "Unexpected language returned");
        }

        /// <summary>
        ///A test for Default
        ///</summary>
        [TestMethod()]
        public void DefaultTest()
        {
            Language expected = Language.English;
            Language actual = LanguageHelper.Default;
            Assert.AreEqual(expected, actual, "Unexpected default language returned");
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        public void ValueTest()
        {
            Language expected = Language.French; 
            CurrentLanguage.Value = expected;
            Language actual = CurrentLanguage.Value;
            Assert.AreEqual(expected, actual, "Unexpected language returend");
        }
    }
}
