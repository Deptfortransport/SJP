// *********************************************** 
// NAME             : SJPServiceDiscoveryTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Jun 2011
// DESCRIPTION  	: This is a test class for SJPServiceDiscovery
// ************************************************
                
                
using SJP.Common.ServiceDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;

namespace SJP.TestProject.ServiceDiscovery
{
    
    
    /// <summary>
    ///This is a test class for SJPServiceDiscoveryTest and is intended
    ///to contain all SJPServiceDiscoveryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPServiceDiscoveryTest
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
        [ExpectedException(typeof(SJPException))]
        public void GetTest()
        {
            SJPServiceDiscovery target = new SJPServiceDiscovery();
            string key = "TestKey";
            TestService actual;
            
            // Initalise the service discovery
            SJPServiceDiscovery.Init(new TestServiceFactoryInitialisation());
            actual = target.Get<TestService>(key);
            Assert.IsNotNull(actual);

            // Making sure that the test service factory has loaded the data
            Assert.AreEqual(5, actual.TestData.Count);

            // When Key not found SJPException gets thrown
            string keyNotFound = "TestKeyNotFound";
            actual = target.Get<TestService>(keyNotFound);


            // set the service discover to null
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

           

           
        }


        /// <summary>
        ///A test for Init
        ///</summary>
        [TestMethod()]
        public void InitTest()
        {
            SJPServiceDiscovery.Init(new TestServiceFactoryInitialisation());
            SJPServiceDiscovery_Accessor accessor = new SJPServiceDiscovery_Accessor(new PrivateObject(SJPServiceDiscovery.Current));

            Assert.IsNotNull(SJPServiceDiscovery.Current);
            Assert.AreEqual(1, accessor.serviceCache.Count);
            Assert.IsInstanceOfType(accessor.serviceCache["TestKey"], typeof(TestServiceFactory));

            // set the service discover to null
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }

        /// <summary>
        ///A test for Initialise
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.servicediscovery.dll")]
        public void InitialiseTest()
        {
            SJPServiceDiscovery_Accessor target = new SJPServiceDiscovery_Accessor();
            IServiceInitialisation initContext = new TestServiceFactoryInitialisation();
            target.Initialise(initContext);

            Assert.AreEqual(1, target.serviceCache.Count);
            Assert.IsInstanceOfType(target.serviceCache["TestKey"], typeof(TestServiceFactory));

            // set the service discover to null
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }

        /// <summary>
        ///A test for ResetServiceDiscoveryForTest
        ///</summary>
        [TestMethod()]
        public void ResetServiceDiscoveryForTestTest()
        {
            SJPServiceDiscovery.Init(new TestServiceFactoryInitialisation());

            Assert.IsNotNull(SJPServiceDiscovery.Current);

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            Assert.IsNull(SJPServiceDiscovery.Current);

            // set the service discover to null
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }

        /// <summary>
        ///A test for SetServiceForTest
        ///</summary>
        [TestMethod()]
        public void SetServiceForTestTest()
        {
            SJPServiceDiscovery target = new SJPServiceDiscovery();
            SJPServiceDiscovery_Accessor accessor = new SJPServiceDiscovery_Accessor(new PrivateObject(target));
            SJPServiceDiscovery_Accessor.current = target;
            string key = "TestKey";
            IServiceFactory serviceFactory = new TestServiceFactory();
            target.SetServiceForTest(key, serviceFactory);

            
            Assert.IsNotNull(SJPServiceDiscovery.Current);
            Assert.AreEqual(1, accessor.serviceCache.Count);
            Assert.IsInstanceOfType(accessor.serviceCache["TestKey"], typeof(TestServiceFactory));

            // Set the service to null
            target.SetServiceForTest(key, null);
            Assert.AreEqual(1, accessor.serviceCache.Count);
            Assert.IsNull(accessor.serviceCache["TestKey"]);

            // set the service discover to null
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
           
        }

        
    }
}
