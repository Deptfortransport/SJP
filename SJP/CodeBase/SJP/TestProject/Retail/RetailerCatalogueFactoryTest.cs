// *********************************************** 
// NAME             : RetailerCatalogueFactoryTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 09 Apr 2011
// DESCRIPTION  	: RetailerCatalogueFactoryTest test class
// ************************************************
// 

using SJP.Common.ServiceDiscovery;                
using SJP.UserPortal.Retail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for RetailerCatalogueFactoryTest and is intended
    ///to contain all RetailerCatalogueFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RetailerCatalogueFactoryTest
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
        ///A test for RetailerCatalogueFactory Constructor
        ///</summary>
        [TestMethod()]
        public void RetailerCatalogueFactoryConstructorTest()
        {
            RetailerCatalogueFactory target = new RetailerCatalogueFactory();

            Assert.IsNotNull(target, "Expected RetailerCatalogueFactory object to be created");
        }

        /// <summary>
        ///A test for RetailerCatalogueFactory Get
        ///</summary>
        [TestMethod()]
        public void RetailerCatalogueFactoryGetTest()
        {
            RetailerCatalogueFactory target = new RetailerCatalogueFactory();
            
            object actual = target.Get();

            Assert.IsNotNull(target, "Expected RetailerCatalogueFactory object to be created");

            IRetailerCatalogue retailerCatalogue = (RetailerCatalogue)actual;

            Assert.IsNotNull(retailerCatalogue, "Expected Get to return an IRetailerCatalogue object");
        }
    }
}
