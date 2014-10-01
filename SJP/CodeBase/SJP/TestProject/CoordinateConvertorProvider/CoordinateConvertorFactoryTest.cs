// *********************************************** 
// NAME             : CoordinateConvertorFactoryTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: CoordinateConvertorFactoryTest test class
// ************************************************
// 
                
using SJP.UserPortal.CoordinateConvertorProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{   
    /// <summary>
    ///This is a test class for CoordinateConvertorFactoryTest and is intended
    ///to contain all CoordinateConvertorFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CoordinateConvertorFactoryTest
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
        ///A test for CoordinateConvertorFactory Constructor
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorFactoryConstructorTest()
        {
            using (CoordinateConvertorFactory target = new CoordinateConvertorFactory())
            {
                Assert.IsNotNull(target, "Failed to create CoordinateConvertorFactory object");
            }
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.coordinateconvertorprovider.dll")]
        public void CoordinateConvertorFactoryDisposeTest()
        {
            CoordinateConvertorFactory_Accessor target = new CoordinateConvertorFactory_Accessor();
            
            // Perform test
            bool disposing = true;
            target.Dispose(disposing);

            // Dispose should have cleared its CoordinateConvertor instance
            object cc = target.Get();
            Assert.IsNull(cc, "Expected CoordinateConvertorFactory to return a null CoordinateConvertor object after dispose was called");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorFactoryDisposeTest1()
        {
            CoordinateConvertorFactory target = new CoordinateConvertorFactory();
            
            // Perform test
            target.Dispose();

            // Dispose should have cleared its CoordinateConvertor instance
            object cc = target.Get();
            Assert.IsNull(cc, "Expected CoordinateConvertorFactory to return a null CoordinateConvertor object after dispose was called");
        }

        /// <summary>
        ///A test for Finalize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.coordinateconvertorprovider.dll")]
        public void CoordinateConvertorFactoryFinalizeTest()
        {
            using (CoordinateConvertorFactory_Accessor target = new CoordinateConvertorFactory_Accessor())
            {
                target.Finalize();
            }
            // Method that does not return a value, therefore cannot be verified
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void CoordinateConvertorFactoryGetTest()
        {
            using (CoordinateConvertorFactory target = new CoordinateConvertorFactory())
            {
                // Perform test
                object actual = target.Get();

                Assert.IsNotNull(actual, "Expected CoordinateConvertorFactory to return an object");

                // Check object returned is a CoordinateConvertor
                CoordinateConvertor cc = null;

                if (actual is CoordinateConvertor)
                {
                    cc = (CoordinateConvertor)actual;
                }

                Assert.IsNotNull(actual, "Expected CoordinateConvertorFactory to return a CoordinteConvertor object");
            }
        }
    }
}
