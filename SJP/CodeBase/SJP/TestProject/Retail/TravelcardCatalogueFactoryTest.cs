// *********************************************** 
// NAME             : TravelcardCatalogueFactoryTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 19 Jan 2012
// DESCRIPTION  	: TravelcardCatalogueFactoryTest test class
// ************************************************
// 

using SJP.UserPortal.Retail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for TravelcardCatalogueFactoryTest and is intended
    ///to contain all TravelcardCatalogueFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TravelcardCatalogueFactoryTest
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
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //    SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        //    SJPServiceDiscovery.Init(new TestInitialisation());
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
        ///A test for TravelcardCatalogueFactory Constructor
        ///</summary>
        [TestMethod()]
        public void TravelcardCatalogueFactoryConstructorTest()
        {
            TravelcardCatalogueFactory target = new TravelcardCatalogueFactory();

            Assert.IsNotNull(target, "Expected TravelcardCatalogueFactory object to be created");
        }

        /// <summary>
        ///A test for TravelcardCatalogueFactory Get
        ///</summary>
        [TestMethod()]
        public void TravelcardCatalogueFactoryGetTest()
        {
            TravelcardCatalogueFactory target = new TravelcardCatalogueFactory();
            
            object actual = target.Get();

            Assert.IsNotNull(target, "Expected TravelcardCatalogueFactory object to be created");

            ITravelcardCatalogue travelcardCatalogue = (TravelcardCatalogue)actual;

            Assert.IsNotNull(travelcardCatalogue, "Expected Get to return an ITravelcardCatalogue object");
        }
    }
}
