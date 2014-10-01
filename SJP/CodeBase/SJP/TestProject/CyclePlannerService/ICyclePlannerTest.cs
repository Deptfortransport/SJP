using SJP.UserPortal.CyclePlannerService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.CyclePlannerService.CyclePlannerWebService;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ICyclePlannerTest and is intended
    ///to contain all ICyclePlannerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ICyclePlannerTest
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


        internal virtual ICyclePlanner CreateICyclePlanner()
        {
            ICyclePlanner target = null;
            return target;
        }

        /// <summary>
        ///A test for CycleJourneyPlan
        ///</summary>
        //[TestMethod()]
        //public void CycleJourneyPlanTest()
        //{
        //    ICyclePlanner target = CreateICyclePlanner(); // TODO: Initialize to an appropriate value
        //    CyclePlannerRequest cyclePlannerRequest = null; // TODO: Initialize to an appropriate value
        //    CyclePlannerResult expected = null; // TODO: Initialize to an appropriate value
        //    CyclePlannerResult actual;
        //    actual = target.CycleJourneyPlan(cyclePlannerRequest);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
    }
}
