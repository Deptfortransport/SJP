// *********************************************** 
// NAME             : StopAccessibilityLinksFactoryTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for StopAccessibilityLinksFactory
// ************************************************
                
                
using SJP.Common.DataServices.StopAccessibilityLinks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for StopAccessibilityLinksFactoryTest and is intended
    ///to contain all StopAccessibilityLinksFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StopAccessibilityLinksFactoryTest
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
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());

            StopAccessibilityLinksFactory target = new StopAccessibilityLinksFactory(); 
            object actual;
            actual = target.Get();
            Assert.IsInstanceOfType(actual, typeof(StopAccessibilityLinks));
        }
    }
}
