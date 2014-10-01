// *********************************************** 
// NAME             : AdminAreaTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit test for AdminArea class
// ************************************************
                
                
using SJP.Common.DataServices.NPTG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for AdminAreaTest and is intended
    ///to contain all AdminAreaTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdminAreaTest
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
        ///A test for AdminArea Constructor
        ///</summary>
        [TestMethod()]
        public void AdminAreaConstructorTest()
        {
            int administrativeAreaCode = 82; 
            string areaName = "Greater London"; 
            string countryCode = "ENG"; 
            string regionCode = "L"; 
            AdminArea target = new AdminArea(administrativeAreaCode, areaName, countryCode, regionCode);

            Assert.AreEqual(areaName, target.AreaName);
            Assert.AreEqual(countryCode, target.CountryCode);
            Assert.AreEqual(regionCode, target.RegionCode);
            Assert.AreEqual(administrativeAreaCode, target.AdministrativeAreaCode);
        }

       
    }
}
