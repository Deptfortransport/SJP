using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for JourneyViewStateTest and is intended
    ///to contain all JourneyViewStateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JourneyViewStateTest
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
        ///A test for JourneyViewState Constructor
        ///</summary>
        [TestMethod()]
        public void JourneyViewStateConstructorTest()
        {
            JourneyViewState target = new JourneyViewState();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for CongestionChargeAdded
        ///</summary>
        [TestMethod()]
        public void CongestionChargeAddedTest()
        {
            JourneyViewState target = new JourneyViewState(); 
            bool expected = true;
            bool actual;
            target.CongestionChargeAdded = expected;
            actual = target.CongestionChargeAdded;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CongestionCostAdded
        ///</summary>
        [TestMethod()]
        public void CongestionCostAddedTest()
        {
            JourneyViewState target = new JourneyViewState(); 
            bool expected = true; 
            bool actual;
            target.CongestionCostAdded = expected;
            actual = target.CongestionCostAdded;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsDirty
        ///</summary>
        [TestMethod()]
        public void IsDirtyTest()
        {
            // Should alwasy come back false no matter what
            JourneyViewState target = new JourneyViewState();
            bool expected = false; 
            bool actual;
            target.IsDirty = true;
            actual = target.IsDirty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for VisitedCongestionCompany
        ///</summary>
        [TestMethod()]
        public void VisitedCongestionCompanyTest()
        {
            JourneyViewState target = new JourneyViewState();
            List<string> expected = new List<string>();
            expected.Add("Company1");
            expected.Add("Company2");
            List<string> actual;
            target.VisitedCongestionCompany = expected;
            actual = target.VisitedCongestionCompany;
            Assert.AreEqual(expected, actual);
        }
    }
}
