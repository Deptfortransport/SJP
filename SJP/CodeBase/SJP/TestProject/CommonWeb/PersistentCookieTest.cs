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
    ///This is a test class for PersistentCookieTest and is intended
    ///to contain all PersistentCookieTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PersistentCookieTest
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
        ///A test for GetCookieValueAccessibleStyle
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetCookieValueAccessibleStyleTest()
        {
            // Test using default value
            string key = "AcessibleStyleTest";
            AccessibleStyle defaultValue = AccessibleStyle.Dyslexia;
            AccessibleStyle expected = AccessibleStyle.HighVis;
            AccessibleStyle actual;
            actual = PersistentCookie_Accessor.GetCookieValueAccessibleStyle(key, defaultValue);
            Assert.AreEqual(defaultValue, actual);

            // Test a pre-existing value
            HttpContext context = HttpContext.Current;
            HttpCookie newCookie = new HttpCookie(cookieName);
            newCookie.Expires = DateTime.Now.AddMonths(6);
            newCookie[key] = expected.ToString();
            context.Request.Cookies.Add(newCookie);
            actual = PersistentCookie_Accessor.GetCookieValueAccessibleStyle(key, defaultValue);
            Assert.AreEqual(expected, actual);

            // Bad value test
            expected = defaultValue;
            newCookie[key] = "bad";
            actual = PersistentCookie_Accessor.GetCookieValueAccessibleStyle(key, defaultValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetCookieValueBool
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetCookieValueBoolTest()
        {
            // Test using default value
            string key = "BoolTest";
            bool defaultValue = false; 
            bool expected = true; 
            bool actual;
            actual = PersistentCookie_Accessor.GetCookieValueBool(key, defaultValue);
            Assert.AreEqual(defaultValue, actual);

            // Test a pre-existing value
            HttpContext context = HttpContext.Current;
            HttpCookie newCookie = new HttpCookie(cookieName);
            newCookie.Expires = DateTime.Now.AddMonths(6);
            newCookie[key] = expected.ToString();
            context.Request.Cookies.Add(newCookie);
            actual = PersistentCookie_Accessor.GetCookieValueBool(key, defaultValue);
            Assert.AreEqual(expected, actual);

            // Bad value test
            expected = defaultValue; 
            newCookie[key] = "bad";
            actual = PersistentCookie_Accessor.GetCookieValueBool(key, defaultValue);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetCookieValueDateTime
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetCookieValueDateTimeTest()
        {
            // Test using default value
            string key = "DateTimeTest";
            DateTime defaultValue = DateTime.Now;
            DateTime expected = DateTime.Now.AddDays(4);
            defaultValue = defaultValue.AddSeconds(-defaultValue.Second); // Seconds not used
            expected = expected.AddSeconds(-expected.Second);
            DateTime actual;
            actual = PersistentCookie_Accessor.GetCookieValueDateTime(key, defaultValue.ToUniversalTime());
            Assert.AreEqual(defaultValue.ToString(), actual.ToString());

            // Test a pre-existing value
            HttpContext context = HttpContext.Current;
            HttpCookie newCookie = new HttpCookie(cookieName);
            newCookie.Expires = DateTime.Now.AddMonths(6);
            newCookie[key] = expected.ToUniversalTime().ToString(PersistentCookie_Accessor.dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
            context.Request.Cookies.Add(newCookie);
            actual = PersistentCookie_Accessor.GetCookieValueDateTime(key, defaultValue.ToUniversalTime());
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for GetCookieValueFontSize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetCookieValueFontSizeTest()
        {
            // Test using default value
            string key = "FontSizeTest";
            FontSize defaultValue = FontSize.Large;
            FontSize expected = FontSize.Medium;
            FontSize actual;
            actual = PersistentCookie_Accessor.GetCookieValueFontSize(key, defaultValue);
            Assert.AreEqual(defaultValue, actual);

            // Test a pre-existing value
            HttpContext context = HttpContext.Current;
            HttpCookie newCookie = new HttpCookie(cookieName);
            newCookie.Expires = DateTime.Now.AddMonths(6);
            newCookie[key] = expected.ToString();
            context.Request.Cookies.Add(newCookie);
            actual = PersistentCookie_Accessor.GetCookieValueFontSize(key, defaultValue);
            Assert.AreEqual(expected, actual);

            // Bad value test
            expected = defaultValue;
            newCookie[key] = "bad";
            actual = PersistentCookie_Accessor.GetCookieValueFontSize(key, defaultValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetCookieValueInt
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetCookieValueIntTest()
        {
            // Test using default value
            string key = "IntTest";
            int defaultValue = 3; 
            int expected = 7;
            int actual;
            actual = PersistentCookie_Accessor.GetCookieValueInt(key, defaultValue);
            Assert.AreEqual(defaultValue, actual);

            // Test a pre-existing value
            HttpContext context = HttpContext.Current;
            HttpCookie newCookie = new HttpCookie(cookieName);
            newCookie.Expires = DateTime.Now.AddMonths(6);
            newCookie[key] = expected.ToString();
            context.Request.Cookies.Add(newCookie);
            actual = PersistentCookie_Accessor.GetCookieValueInt(key, defaultValue);
            Assert.AreEqual(expected, actual);

            // Bad value test
            expected = defaultValue;
            newCookie[key] = "bad";
            actual = PersistentCookie_Accessor.GetCookieValueInt(key, defaultValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetCookieValueLanguage
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetCookieValueLanguageTest()
        {
            // Test using default value
            string key = "LanguageTest";
            Language defaultValue = Language.French;
            Language expected = Language.English;
            Language actual;
            actual = PersistentCookie_Accessor.GetCookieValueLanguage(key, defaultValue);
            Assert.AreEqual(defaultValue, actual);

            // Test a pre-existing value
            HttpContext context = HttpContext.Current;
            HttpCookie newCookie = new HttpCookie(cookieName);
            newCookie.Expires = DateTime.Now.AddMonths(6);
            newCookie[key] = expected.ToString();
            context.Request.Cookies.Add(newCookie);
            actual = PersistentCookie_Accessor.GetCookieValueLanguage(key, defaultValue);
            Assert.AreEqual(expected, actual);

            // Bad value test
            expected = defaultValue;
            newCookie[key] = "bad";
            actual = PersistentCookie_Accessor.GetCookieValueLanguage(key, defaultValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetCookieValueString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void GetCookieValueStringTest()
        {
            // Test using default value
            string key = "StringTest";
            string defaultValue = "default";
            string expected = "expected";
            string actual;
            actual = PersistentCookie_Accessor.GetCookieValueString(key, defaultValue);
            Assert.AreEqual(defaultValue, actual);

            // Test a pre-existing value
            HttpContext context = HttpContext.Current;
            HttpCookie newCookie = new HttpCookie(cookieName);
            newCookie.Expires = DateTime.Now.AddMonths(6);
            newCookie[key] = expected.ToString();
            context.Request.Cookies.Add(newCookie);
            actual = PersistentCookie_Accessor.GetCookieValueString(key, defaultValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetCookieValueAccessibleStyle
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetCookieValueAccessibleStyleTest()
        {
            string key = "AccessibleStyleTest";
            AccessibleStyle expected = AccessibleStyle.HighVis;
            PersistentCookie_Accessor.SetCookieValueAccessibleStyle(key, expected);

            // Check the response cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Response.Cookies[cookieName];
            Assert.AreEqual(expected.ToString(), cookie[key]);
        }

        /// <summary>
        ///A test for SetCookieValueBool
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetCookieValueBoolTest()
        {
            string key = "BoolTest";
            bool expected = true;
            PersistentCookie_Accessor.SetCookieValueBool(key, expected);

            // Check the response cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Response.Cookies[cookieName];
            Assert.AreEqual(expected.ToString(), cookie[key]);
        }

        /// <summary>
        ///A test for SetCookieValueDateTime
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetCookieValueDateTimeTest()
        {
            string key = "DateTimeTest";
            DateTime expected = DateTime.Now;
            expected = expected.AddSeconds(-expected.Second); // seconds not used
            PersistentCookie_Accessor.SetCookieValueDateTime(key, expected);

            // Check the response cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Response.Cookies[cookieName];
            DateTime actual = DateTime.ParseExact(cookie[key], PersistentCookie_Accessor.dateTimeFormat, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for SetCookieValueFontSize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetCookieValueFontSizeTest()
        {
            string key = "FontSizeTest";
            FontSize expected = FontSize.Medium;
            PersistentCookie_Accessor.SetCookieValueFontSize(key, expected);

            // Check the response cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Response.Cookies[cookieName];
            Assert.AreEqual(expected.ToString(), cookie[key]);
        }

        /// <summary>
        ///A test for SetCookieValueInt
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetCookieValueIntTest()
        {
            string key = "IntTest";
            int expected = 7;
            PersistentCookie_Accessor.SetCookieValueInt(key, expected);

            // Check the response cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Response.Cookies[cookieName];
            Assert.AreEqual(expected.ToString(), cookie[key]);
        }

        /// <summary>
        ///A test for SetCookieValueLanguage
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetCookieValueLanguageTest()
        {
            string key = "LanguageTest";
            Language expected = Language.French;
            PersistentCookie_Accessor.SetCookieValueLanguage(key, expected);

            // Check the response cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Response.Cookies[cookieName];
            Assert.AreEqual(expected.ToString(), cookie[key]);
        }

        /// <summary>
        ///A test for SetCookieValueString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void SetCookieValueStringTest()
        {
            string key = "StringTest";
            string expected = "String Test";
            PersistentCookie_Accessor.SetCookieValueString(key, expected);

            // Check the response cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = context.Response.Cookies[cookieName];
            Assert.AreEqual(expected.ToString(), cookie[key]);
        }

        /// <summary>
        ///A test for Cookie
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dll")]
        public void CookieTest()
        {
            // Test get with no existing cookie
            HttpCookie expected = new HttpCookie(cookieName);
            HttpCookie actual;
            actual = PersistentCookie_Accessor.Cookie;
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Values.Count, actual.Values.Count);

            // Test get with existing cookie
            HttpContext context = HttpContext.Current;
            expected = new HttpCookie(cookieName);
            expected.Expires = DateTime.Now.AddDays(7);
            context.Request.Cookies.Add(expected);
            actual = PersistentCookie_Accessor.Cookie;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.Expires, actual.Expires);

            // Test setting the cookie
            PersistentCookie_Accessor.Cookie = expected;
            actual = context.Response.Cookies[cookieName];
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected.Expires, actual.Expires);
        }

        /// <summary>
        ///A test for CurrentAccessibleStyle
        ///</summary>
        [TestMethod()]
        public void CurrentAccessibleStyleTest()
        {
            // Test the get default
            AccessibleStyle expected = AccessibleStyle.Normal;
            AccessibleStyle actual;
            actual = PersistentCookie.CurrentAccessibleStyle;
            Assert.AreEqual(expected, actual);

            // Test the get from cookie
            expected = AccessibleStyle.Dyslexia;
            string key = PersistentCookie_Accessor.KeyCurrentAccessibleStyle;
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie[key] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.CurrentAccessibleStyle;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = AccessibleStyle.HighVis;
            PersistentCookie.CurrentAccessibleStyle = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = (AccessibleStyle)Enum.Parse(typeof(AccessibleStyle), cookie[key]);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CurrentFontSize
        ///</summary>
        [TestMethod()]
        public void CurrentFontSizeTest()
        {
            // Test the get default
            FontSize expected = FontSize.Normal;
            FontSize actual;
            actual = PersistentCookie.CurrentFontSize;
            Assert.AreEqual(expected, actual);

            // Test the get from cookie
            expected = FontSize.Large;
            string key = PersistentCookie_Accessor.KeyCurrentFontSize;
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie[key] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.CurrentFontSize;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = FontSize.Medium;
            PersistentCookie.CurrentFontSize = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = (FontSize)Enum.Parse(typeof(FontSize), cookie[key]);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CurrentLanguage
        ///</summary>
        [TestMethod()]
        public void CurrentLanguageTest()
        {
            // Test the get default
            Language expected = Language.English;
            Language actual;
            actual = PersistentCookie.CurrentLanguage;
            Assert.AreEqual(expected, actual);

            // Test the get from cookie
            expected = Language.French;
            string key = PersistentCookie_Accessor.KeyCurrentLanguage;
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie[key] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.CurrentLanguage;
            Assert.AreEqual(expected, actual);
            
            // Test the set
            expected = Language.French;
            PersistentCookie.CurrentLanguage = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = (Language)Enum.Parse(typeof(Language), cookie[key]);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Domain
        ///</summary>
        [TestMethod()]
        public void DomainTest()
        {
            // Test the get (no default)
            string expected  = "TestDomain1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie.Domain = expected;
            context.Request.Cookies.Add(cookie);
            string actual = PersistentCookie.Domain;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "TestDomain2";

            // Due to the setting of the domain actually happening on the request collection
            // we have to set up a response cookie by setting a different cookie key item
            PersistentCookie.CurrentFontSize = FontSize.Large;

            PersistentCookie.Domain = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie.Domain;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Expires
        ///</summary>
        [TestMethod()]
        public void ExpiresTest()
        {
            // Test the get (no default)
            DateTime expected = DateTime.Now.AddDays(7);
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie.Expires = expected;
            context.Request.Cookies.Add(cookie);
            DateTime actual = PersistentCookie.Expires;
            Assert.AreEqual(expected, actual);


            // Test the set
            expected = DateTime.Now.AddDays(14);

            // Due to the setting of the expiry actually happening on the request collection
            // we have to set up a response cookie by setting a different cookie key item
            PersistentCookie.CurrentFontSize = FontSize.Large;

            PersistentCookie.Expires = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie.Expires;
            Assert.AreEqual(expected, actual);

            context.Response.AppendCookie(cookie);
        }

        /// <summary>
        ///A test for IsPersistentCookieAvailable
        ///</summary>
        [TestMethod()]
        public void IsPersistentCookieAvailableTest()
        {
            // No Context exception test
            MyTestCleanup();

            bool expected = false;
            bool actual = PersistentCookie.IsPersistentCookieAvailable;
            Assert.AreEqual(expected, actual);

            MyTestInitialize();

            // Test no cookie
            expected = false;
            actual = PersistentCookie.IsPersistentCookieAvailable;
            Assert.AreEqual(expected, actual);

            // Test cookie
            expected = true;
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie[PersistentCookie_Accessor.KeyCurrentFontSize] = FontSize.Large.ToString();
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.IsPersistentCookieAvailable;
            Assert.AreEqual(expected, actual);   
        }

        /// <summary>
        ///A test for JourneyDateTimeOutward
        ///</summary>
        [TestMethod()]
        public void JourneyDateTimeOutwardTest()
        {
            // Test the get (from cookie)
            DateTime expected = DateTime.Now.AddDays(7);
            expected = expected.AddSeconds(-expected.Second); // seconds aren't used
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyDateTimeOutward] = expected.ToUniversalTime().ToString(PersistentCookie_Accessor.dateTimeFormat, CultureInfo.InvariantCulture);
            context.Request.Cookies.Add(cookie);
            DateTime actual = PersistentCookie.JourneyDateTimeOutward;
            Assert.AreEqual(expected.ToString(), actual.ToString());

            // Test the set
            expected = expected.AddDays(7);
            PersistentCookie.JourneyDateTimeOutward = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = DateTime.ParseExact(cookie[PersistentCookie_Accessor.KeyJourneyDateTimeOutward], PersistentCookie_Accessor.dateTimeFormat, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for JourneyDateTimeReturn
        ///</summary>
        [TestMethod()]
        public void JourneyDateTimeReturnTest()
        {
            // Test the get (from cookie)
            DateTime expected = DateTime.Now.AddDays(7);
            expected = expected.AddSeconds(-expected.Second); // seconds aren't used
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyDateTimeReturn] = expected.ToUniversalTime().ToString(PersistentCookie_Accessor.dateTimeFormat, CultureInfo.InvariantCulture);
            context.Request.Cookies.Add(cookie);
            DateTime actual = PersistentCookie.JourneyDateTimeReturn;
            Assert.AreEqual(expected.ToString(), actual.ToString());

            // Test the set
            expected = expected.AddDays(7);
            PersistentCookie.JourneyDateTimeReturn = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = DateTime.ParseExact(cookie[PersistentCookie_Accessor.KeyJourneyDateTimeReturn], PersistentCookie_Accessor.dateTimeFormat, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for JourneyDestinationId
        ///</summary>
        [TestMethod()]
        public void JourneyDestinationIdTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.JourneyDestinationId;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "JourneyDestinationIdTest1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyDestinationId] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyDestinationId;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "JourneyDestinationIdTest2";
            PersistentCookie.JourneyDestinationId = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyJourneyDestinationId];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyDestinationType
        ///</summary>
        [TestMethod()]
        public void JourneyDestinationTypeTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.JourneyDestinationType;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "JourneyDestinationTypeTest1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyDestinationType] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyDestinationType;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "JourneyDestinationTypeTest2";
            PersistentCookie.JourneyDestinationType = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyJourneyDestinationType];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyOriginId
        ///</summary>
        [TestMethod()]
        public void JourneyOriginIdTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.JourneyOriginId;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "JourneyOriginIdTest1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyOriginId] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyOriginId;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "JourneyOriginIdTest2";
            PersistentCookie.JourneyOriginId = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyJourneyOriginId];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyOriginType
        ///</summary>
        [TestMethod()]
        public void JourneyOriginTypeTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.JourneyOriginType;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "JourneyOriginTypeTest1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyOriginType] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyOriginType;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "JourneyOriginTypeTest2";
            PersistentCookie.JourneyOriginType = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyJourneyOriginType];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyPlannerMode
        ///</summary>
        [TestMethod()]
        public void JourneyPlannerModeTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.JourneyPlannerMode;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "JourneyPlannerMode1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyPlannerMode] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyPlannerMode;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "JourneyPlannerMode2";
            PersistentCookie.JourneyPlannerMode = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyJourneyPlannerMode];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyAccessibleOption
        ///</summary>
        [TestMethod()]
        public void JourneyAccessibleOptionTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.JourneyAccessibleOption;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "JourneyAccessibleOption1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyAccessibleOption] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyAccessibleOption;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "JourneyAccessibleOption2";
            PersistentCookie.JourneyAccessibleOption = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyJourneyAccessibleOption];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyOutwardRequired
        ///</summary>
        [TestMethod()]
        public void JourneyOutwardRequiredTest()
        {
            // Test the get (default)
            bool expected = true;
            bool actual = PersistentCookie.JourneyOutwardRequired;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = true;
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyOutwardRequired] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyOutwardRequired;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = true;
            PersistentCookie.JourneyOutwardRequired = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = bool.Parse(cookie[PersistentCookie_Accessor.KeyJourneyOutwardRequired]);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyReturnRequired
        ///</summary>
        [TestMethod()]
        public void JourneyReturnRequiredTest()
        {
            // Test the get (default)
            bool expected = false;
            bool actual = PersistentCookie.JourneyReturnRequired;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = true;
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyReturnRequired] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyReturnRequired;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = true;
            PersistentCookie.JourneyReturnRequired = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = bool.Parse(cookie[PersistentCookie_Accessor.KeyJourneyReturnRequired]);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyReturnRequired
        ///</summary>
        [TestMethod()]
        public void JourneyFewerChangesTest()
        {
            // Test the get (default)
            bool expected = false;
            bool actual = PersistentCookie.JourneyFewerInterchanges;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = true;
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyJourneyFewerInterchanges] = expected.ToString();
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.JourneyFewerInterchanges;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = true;
            PersistentCookie.JourneyFewerInterchanges = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = bool.Parse(cookie[PersistentCookie_Accessor.KeyJourneyFewerInterchanges]);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LastPageVisited
        ///</summary>
        [TestMethod()]
        public void LastPageVisitedTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.LastPageVisited;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "LastPageVisited1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyLastPageVisited] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.LastPageVisited;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "LastPageVisited2";
            PersistentCookie.LastPageVisited = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyLastPageVisited];
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LastVisitedDateTime
        ///</summary>
        [TestMethod()]
        public void LastVisitedDateTimeTest()
        {
            // Test the get (from cookie)
            DateTime expected = DateTime.Now.AddDays(7);
            expected = expected.AddSeconds(-expected.Second); // seconds not used
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyLastVisitedDateTime] = expected.ToUniversalTime().ToString(PersistentCookie_Accessor.dateTimeFormat, CultureInfo.InvariantCulture);
            context.Request.Cookies.Add(cookie);
            DateTime actual = PersistentCookie.LastVisitedDateTime;
            Assert.AreEqual(expected.ToString(), actual.ToString());

            // Test the set
            expected = expected.AddDays(7);
            PersistentCookie.LastVisitedDateTime = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = DateTime.ParseExact(cookie[PersistentCookie_Accessor.KeyLastVisitedDateTime], PersistentCookie_Accessor.dateTimeFormat, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            // Test default
            string expected = cookieName;
            string actual = PersistentCookie.Name;
            Assert.AreEqual(expected, actual);

            // Test from cookie
            HttpContext context = HttpContext.Current;
            HttpCookie cookie = new HttpCookie(cookieName);
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.Name;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for UniqueId
        ///</summary>
        [TestMethod()]
        public void UniqueIdTest()
        {
            // Test the get (default)
            string expected = string.Empty;
            string actual = PersistentCookie.UniqueId;
            Assert.AreEqual(expected, actual);

            // Test the get (from cookie)
            expected = "UniqueId1";
            HttpCookie cookie = new HttpCookie(cookieName);
            HttpContext context = HttpContext.Current;
            cookie[PersistentCookie_Accessor.KeyUniqueId] = expected;
            context.Request.Cookies.Add(cookie);
            actual = PersistentCookie.UniqueId;
            Assert.AreEqual(expected, actual);

            // Test the set
            expected = "UniqueId2";
            PersistentCookie.UniqueId = expected;
            cookie = context.Response.Cookies[cookieName];
            actual = cookie[PersistentCookie_Accessor.KeyUniqueId];
            Assert.AreEqual(expected, actual);
        }
    }
}
