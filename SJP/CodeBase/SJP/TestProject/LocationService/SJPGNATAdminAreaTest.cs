// *********************************************** 
// NAME             : SJPGNATAdminArea.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 30 Now 2011
// DESCRIPTION  	: Unit tests for SJPGNATAdminArea
// ************************************************

using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPGNATAdminAreaTest and is intended
    ///to contain all SJPGNATAdminAreaTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPGNATAdminAreaTest
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
        ///A test for SJPGNATAdminArea Constructor
        ///</summary>
        [TestMethod()]
        public void SJPGNATAdminAreaConstructorTest()
        {
            int administrativeAreaCode = 123;
            int districtCode = 321;
            bool stepFreeAccess = true;
            bool assistanceAvailable = true;
            SJPGNATAdminArea target = new SJPGNATAdminArea(administrativeAreaCode, districtCode, stepFreeAccess, assistanceAvailable);
            Assert.AreEqual(administrativeAreaCode, target.AdministrativeAreaCode);
            Assert.AreEqual(districtCode, target.DistrictCode);
            Assert.AreEqual(stepFreeAccess, target.StepFreeAccess);
            Assert.AreEqual(assistanceAvailable, target.AssistanceAvailable);
        }

        /// <summary>
        ///A test for SJPGNATAdminArea Constructor
        ///</summary>
        [TestMethod()]
        public void SJPGNATAdminAreaConstructorTest1()
        {
            SJPGNATAdminArea target = new SJPGNATAdminArea();
            Assert.IsNotNull(target, "Expected SJPGNATAdminArea to be created");
        }
    }
}
