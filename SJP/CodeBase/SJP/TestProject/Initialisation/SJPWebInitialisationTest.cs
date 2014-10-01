// *********************************************** 
// NAME             : SJPWebInitialisationTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: SJPWebInitialisationTest test
// ************************************************
// 
                
using SJP.Common.ServiceDiscovery.Initialisation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPWebInitialisationTest and is intended
    ///to contain all SJPWebInitialisationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPWebInitialisationTest
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
        ///A test for Populate
        ///</summary>
        [TestMethod()]
        public void PopulateTest()
        {
            SJPWebInitialisation target = new SJPWebInitialisation();
            Dictionary<string, IServiceFactory> serviceCache = new Dictionary<string, IServiceFactory>();

            target.Populate(serviceCache);

            Assert.IsTrue(serviceCache.Count > 0, "Expected services to be added to the cache");
        }

        /// <summary>
        ///A test for SJPWebInitialisation Constructor
        ///</summary>
        [TestMethod()]
        public void SJPWebInitialisationConstructorTest()
        {
            SJPWebInitialisation target = new SJPWebInitialisation();

            Assert.IsNotNull(target);
        }
    }
}
