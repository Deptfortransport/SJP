using SJP.Reporting.EventReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using System.Collections.Generic;
using SJP.TestProject.EventReceiver;
using System.Collections;
using System.Messaging;
using SJP.Common.ServiceDiscovery;
using SJP.Common;
using System.Threading;


namespace SJP.TestProject.EventReceiver
{
    
    
    /// <summary>
    ///This is a test class for EventReceiverPropertyValidatorTest and is intended
    ///to contain all EventReceiverPropertyValidatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventReceiverPropertyValidatorTest
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
        ///A test for ValidateProperty
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void ValidatePropertyTest()
        {
            IPropertyProvider properties = new MockPropertiesGoodProperties();
            EventReceiverPropertyValidator target = new EventReceiverPropertyValidator(properties);
            //key == Keys.ReceiverQueue
            string key = Keys.ReceiverQueue;
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateProperty(key, errors);
            Assert.AreEqual(expected, actual);

            //key != Keys.ReceiverQueue
            key = Keys.ReceiverQueuePath;
            actual = target.ValidateProperty(key, errors);

        }

        /// <summary>
        ///A test for ValidateReceiverQueue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void ValidateReceiverQueueTest()
        {
            MockPropertiesGoodProperties goodProps = new MockPropertiesGoodProperties();
            PrivateObject param0 = new PrivateObject(new EventReceiverPropertyValidator(goodProps));
            EventReceiverPropertyValidator_Accessor target = new EventReceiverPropertyValidator_Accessor(param0);
            
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateReceiverQueue(errors);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ValidateReceiverQueueCanRead
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void ValidateReceiverQueueCanReadTest()
        {
            MockPropertiesGoodProperties goodProps =  new MockPropertiesGoodProperties();
            PrivateObject param0 = new PrivateObject(new EventReceiverPropertyValidator(goodProps));
            EventReceiverPropertyValidator_Accessor target = new EventReceiverPropertyValidator_Accessor(param0);
            string key = String.Format(Keys.ReceiverQueuePath, "Queue1");
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateReceiverQueueCanRead(key, errors);
            Assert.AreEqual(expected, actual);

            
        }

        /// <summary>
        ///A test for ValidateReceiverQueuePath
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void ValidateReceiverQueuePathTest()
        {
            MockPropertiesGoodProperties goodProps = new MockPropertiesGoodProperties();
            PrivateObject param0 = new PrivateObject(new EventReceiverPropertyValidator(goodProps));
            EventReceiverPropertyValidator_Accessor target = new EventReceiverPropertyValidator_Accessor(param0);
            string key = String.Format(Keys.ReceiverQueuePath, "Queue1");
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;
            actual = target.ValidateReceiverQueuePath(key, errors);
            Assert.AreEqual(expected, actual);

            // Queue path does not exist
            MessageQueue.Delete(goodProps[String.Format(Keys.ReceiverQueuePath, "Queue2")].ToString());
            key = String.Format(Keys.ReceiverQueuePath, "Queue2");
           errors = new List<string>();
            expected = false;
            actual = target.ValidateReceiverQueuePath(key, errors);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(errors.Count > 0);

        }



        /// <summary>
        /// Tests that the validation class can handle a properties object with no message queue specified.
        /// </summary>
        [TestMethod()]
        public void TestGoodPropertiesWithNoQueue()
        {
            MockPropertiesGoodProperties properties = new MockPropertiesGoodProperties();
            EventReceiverPropertyValidator validator = new EventReceiverPropertyValidator(properties);

            // Queue path does not exist
            MessageQueue.Delete(properties[String.Format(Keys.ReceiverQueuePath, "Queue1")].ToString());
            List<string> errors = new List<string>();

            Assert.IsTrue(!validator.ValidateProperty(Keys.ReceiverQueue, errors), "validation failed");
        }

    }
}
