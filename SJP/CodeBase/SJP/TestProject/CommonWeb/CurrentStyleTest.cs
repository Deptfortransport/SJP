using System.Web;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common.Web;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for CurrentStyleTest and is intended
    ///to contain all CurrentStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CurrentStyleTest
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
        ///A test for GetAccessibleStyleValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetAccessibleStyleValueTest()
        {
            HttpContext context = HttpContext.Current;
            
            // Test retrieval from cookie
            HttpCookie cookie = new HttpCookie(cookieName);
            AccessibleStyle expected = AccessibleStyle.Dyslexia;
            cookie[PersistentCookie_Accessor.KeyCurrentAccessibleStyle] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            AccessibleStyle actual = CurrentStyle_Accessor.GetAccessibleStyleValue(context);
            Assert.AreEqual(expected, actual, "Unexpected accessibility style returned from cookie");

            // Retrieval from session
            expected = AccessibleStyle.HighVis;
            context.Session[CurrentStyle_Accessor.siteAccessibleStyleKeyName] = expected;
            actual = CurrentStyle_Accessor.GetAccessibleStyleValue(context);
            Assert.AreEqual(expected, actual, "Unexpected accessibility style returned from session");
        }

        /// <summary>
        ///A test for GetFontSizeValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetFontSizeValueTest()
        {
            HttpContext context = HttpContext.Current; 

            // Test retrieval from cookie
            FontSize expected = FontSize.Large;
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie[PersistentCookie_Accessor.KeyCurrentFontSize] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            FontSize actual = CurrentStyle_Accessor.GetFontSizeValue(context);
            Assert.AreEqual(expected, actual);

            // Test retrieval from session
            context.Request.Cookies.Remove(cookieName);
            actual = CurrentStyle_Accessor.GetFontSizeValue(context);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ParseAccessibleStyle
        ///</summary>
        [TestMethod()]
        public void ParseAccessibleStyleTest()
        {
            // Test default
            string accessibleStyle = "blah";
            AccessibleStyle expected = AccessibleStyle.Normal;
            AccessibleStyle actual = CurrentStyle.ParseAccessibleStyle(accessibleStyle);
            Assert.AreEqual(expected, actual);

            // Test high vis and spaces
            accessibleStyle = "   h  ";
            expected = AccessibleStyle.HighVis;
            actual = CurrentStyle.ParseAccessibleStyle(accessibleStyle);
            Assert.AreEqual(expected, actual);

            // Dyslexia no spaces
            accessibleStyle = "d";
            expected = AccessibleStyle.Dyslexia;
            actual = CurrentStyle.ParseAccessibleStyle(accessibleStyle);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ParseFontSize
        ///</summary>
        [TestMethod()]
        public void ParseFontSizeTest()
        {
            // Test default
            string fontSize = "blah";
            FontSize expected = FontSize.Normal;
            FontSize actual = CurrentStyle.ParseFontSize(fontSize);
            Assert.AreEqual(expected, actual);

            // medium with spaces
            fontSize = "    m    ";
            expected = FontSize.Medium;
            actual = CurrentStyle.ParseFontSize(fontSize);
            Assert.AreEqual(expected, actual);

            // Test large
            fontSize = "l";
            expected = FontSize.Large;
            actual = CurrentStyle.ParseFontSize(fontSize);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetAccessibleStyleValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetAccessibleStyleValueTest()
        {
            HttpContext context = HttpContext.Current;
            AccessibleStyle expected = AccessibleStyle.Dyslexia;
            CurrentStyle_Accessor.SetAccessibleStyleValue(context, expected);

            AccessibleStyle actual = PersistentCookie.CurrentAccessibleStyle;
            Assert.AreEqual(expected, actual, "Unexpected accessibility style returned from cookie");

            actual = (AccessibleStyle)context.Session[CurrentStyle_Accessor.siteAccessibleStyleKeyName];
            Assert.AreEqual(expected, actual, "Unexpected accessibility style returned from cookie");
        }

        /// <summary>
        ///A test for SetFontSizeValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetFontSizeValueTest()
        {
            HttpContext context = HttpContext.Current;
            FontSize expected = FontSize.Large;
            CurrentStyle_Accessor.SetFontSizeValue(context, expected);

            FontSize actual = PersistentCookie.CurrentFontSize;
            Assert.AreEqual(expected, actual, "Unexpected font size returned from cookie");

            actual = (FontSize)context.Session[CurrentStyle_Accessor.siteFontSizeKeyName];
            Assert.AreEqual(expected, actual, "Unexpected font size returned from session");
        }

        /// <summary>
        ///A test for SetSessionValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetSessionValueTest()
        {
            HttpContext context = HttpContext.Current;
            AccessibleStyle expected = AccessibleStyle.HighVis;
            CurrentStyle_Accessor.SetSessionValue(context, expected);
            HttpCookie cookie = context.Response.Cookies[cookieName];
            AccessibleStyle actual = (AccessibleStyle)context.Session[CurrentStyle_Accessor.siteAccessibleStyleKeyName];
            Assert.AreEqual(expected, actual, "Unexpected accessibility style returned from session");
        }

        /// <summary>
        ///A test for SetSessionValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetSessionValueTest1()
        {
            HttpContext context = HttpContext.Current;
            FontSize expected = FontSize.Large;
            CurrentStyle_Accessor.SetSessionValue(context, expected);
            FontSize actual = (FontSize)context.Session[CurrentStyle_Accessor.siteFontSizeKeyName];
            Assert.AreEqual(expected, actual, "Unexpected font size returned from session");
        }

        /// <summary>
        ///A test for AccessibleStyleDefault
        ///</summary>
        [TestMethod()]
        public void AccessibleStyleDefaultTest()
        {
            AccessibleStyle expected = AccessibleStyle.Normal;
            AccessibleStyle actual = CurrentStyle.AccessibleStyleDefault;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for AccessibleStyleSheet
        ///</summary>
        [TestMethod()]
        public void AccessibleStyleSheetTest()
        {
            string expected = string.Empty;
            string actual = CurrentStyle.AccessibleStyleSheet;
            Assert.AreEqual(expected, actual, "Unexpected default stylesheet returned");

            expected = "dyslexia.css";
            CurrentStyle.AccessibleStyleValue = AccessibleStyle.Dyslexia;
            actual = CurrentStyle.AccessibleStyleSheet;
            Assert.AreEqual(expected, actual, "Unexpected dyslexia stylesheet returned");

            expected = "high-vis.css";
            CurrentStyle.AccessibleStyleValue = AccessibleStyle.HighVis;
            actual = CurrentStyle.AccessibleStyleSheet;
            Assert.AreEqual(expected, actual, "Unexpected high-vis stylesheet returned");
        }

        /// <summary>
        ///A test for AccessibleStyleValue
        ///</summary>
        [TestMethod()]
        public void AccessibleStyleValueTest()
        {
            AccessibleStyle expected = AccessibleStyle.HighVis;
            AccessibleStyle actual;
            CurrentStyle.AccessibleStyleValue = expected;
            actual = CurrentStyle.AccessibleStyleValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FontSizeDefault
        ///</summary>
        [TestMethod()]
        public void FontSizeDefaultTest()
        {
            FontSize expected = FontSize.Normal;
            FontSize actual = CurrentStyle.FontSizeDefault;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FontSizeStyleSheet
        ///</summary>
        [TestMethod()]
        public void FontSizeStyleSheetTest()
        {
            string expected = "font-small.css";
            string actual = CurrentStyle.FontSizeStyleSheet;
            Assert.AreEqual(expected, actual, "Unexpected default font size style sheet returned");

            expected = "font-medium.css";
            CurrentStyle.FontSizeValue = FontSize.Medium;
            actual = CurrentStyle.FontSizeStyleSheet;
            Assert.AreEqual(expected, actual, "Unexpected medium font size style sheet returned");

            expected = "font-large.css";
            CurrentStyle.FontSizeValue = FontSize.Large;
            actual = CurrentStyle.FontSizeStyleSheet;
            Assert.AreEqual(expected, actual, "Unexpected large font size style sheet returned");
        }

        /// <summary>
        ///A test for FontSizeValue
        ///</summary>
        [TestMethod()]
        public void FontSizeValueTest()
        {
            FontSize expected = FontSize.Medium;
            CurrentStyle.FontSizeValue = expected;
            FontSize actual = CurrentStyle.FontSizeValue;
            Assert.AreEqual(expected, actual);
        }
    }
}
