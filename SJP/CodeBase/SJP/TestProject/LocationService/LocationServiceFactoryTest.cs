// *********************************************** 
// NAME             : LocationServiceFactoryTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: LocationServiceFactoryTest test class
// ************************************************
// 
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for LocationServiceFactoryTest and is intended
    ///to contain all LocationServiceFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocationServiceFactoryTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
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
        ///A test for LocationServiceFactory Constructor
        ///</summary>
        [TestMethod()]
        public void LocationServiceFactoryConstructorTest()
        {
            LocationServiceFactory target = new LocationServiceFactory();

            Assert.IsNotNull(target, "Expected LocationServiceFactory object to not be null");
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void LocationServiceFactoryGetTest()
        {
            LocationServiceFactory target = new LocationServiceFactory();

            object actual = target.Get();

            Assert.IsNotNull(target, "Expected LocationServiceFactory Get object to not be null");

            LocationService locationService = (LocationService)actual;

            Assert.IsNotNull(locationService, "Expected LocationService object to not be null");
        }
    }
}
