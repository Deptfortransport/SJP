using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.TravelNews.SessionData;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for TravelNewsPageStateTest and is intended
    ///to contain all TravelNewsPageStateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TravelNewsPageStateTest
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
        ///A test for TravelNewsPageState Constructor
        ///</summary>
        [TestMethod()]
        public void TravelNewsPageStateConstructorTest()
        {
            TravelNewsPageState target = new TravelNewsPageState();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for SetDefaultState
        ///</summary>
        [TestMethod()]
        public void SetDefaultStateTest()
        {
            TravelNewsPageState target = new TravelNewsPageState(); 
            target.SetDefaultState();
            Assert.AreEqual("All", target.TravelNewsState.SelectedRegion, "State not initialised");
        }

        /// <summary>
        ///A test for IsDirty
        ///</summary>
        [TestMethod()]
        public void IsDirtyTest()
        {
            TravelNewsPageState target = new TravelNewsPageState();
            bool expected = true; 
            bool actual;
            target.IsDirty = expected;
            actual = target.IsDirty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for TravelNewsState
        ///</summary>
        [TestMethod()]
        public void TravelNewsStateTest()
        {
            TravelNewsPageState target = new TravelNewsPageState();
            TravelNewsState expected = new TravelNewsState();
            expected.SetDefaultState();
            TravelNewsState actual;
            target.TravelNewsState = expected;
            actual = target.TravelNewsState;
            Assert.AreEqual(expected, actual);
        }
    }
}
