using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using System.Collections.Generic;
using SJP.TestProject.EventLogging.MockObjects;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for EventPublisherGroupTest and is intended
    ///to contain all EventPublisherGroupTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventPublisherGroupTest
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
        ///A test for CreatePublishers
        ///</summary>
        [TestMethod()]
        public void CreatePublishersTest()
        {
            IPropertyProvider properties = new MockPropertiesGood();
            EventPublisherGroup target = new EventPublisherGroup(properties); 
            List<string> errors = new List<string>(); 
            
            target.CreatePublishers(errors);

            Assert.AreEqual(0, errors.Count);
        }

        /// <summary>
        ///A test for CreatePublishers - no publishers defined in properties
        ///</summary>
        [TestMethod()]
        public void CreatePublishersTestNoPublishers()
        {
            IPropertyProvider properties = new MockPropertiesEmptyValues();

            EventPublisherGroup target = new EventPublisherGroup(properties);
            List<string> errors = new List<string>();

            target.CreatePublishers(errors);

            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(0,target.Publishers.Count);
        }
        
    }
}
