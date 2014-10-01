using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using System.Reflection;
using System.Collections.Generic;
using SJP.TestProject.EventLogging.MockObjects;
using SJP.Common;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for LoggingPropertyValidatorTest and is intended
    ///to contain all LoggingPropertyValidatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LoggingPropertyValidatorTest
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
        public void ValidatePropertyTest()
        {
            IPropertyProvider properties = new MockPropertiesGood();
           
            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);


            target.ValidateProperty(Keys.QueuePublishers, errors);

            target.ValidateProperty(Keys.EmailPublishers, errors);

            target.ValidateProperty(Keys.FilePublishers, errors);

            target.ValidateProperty(Keys.EventLogPublishers, errors);

            target.ValidateProperty(Keys.CustomPublishers, errors);

            target.ValidateProperty(Keys.ConsolePublishers, errors);

            target.ValidateProperty(Keys.OperationalTraceLevel, errors);

            target.ValidateProperty(Keys.DefaultPublisher, errors);

            target.ValidateProperty(Keys.CustomEventsLevel, errors);

            target.ValidateProperty(Keys.CustomEvents, errors);

            Assert.AreEqual(0, errors.Count);

           
        }

        /// <summary>
        ///A test for ValidateProperty with errors gets generated during validation
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void ValidatePropertyTestErrors()
        {
            MockPropertiesEmptyValues properties = new MockPropertiesEmptyValues();

            properties.SetPropertiesForValidationErrors();

            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);


            target.ValidateProperty(Keys.QueuePublishers, errors);

            target.ValidateProperty(Keys.EmailPublishers, errors);

            target.ValidateProperty(Keys.FilePublishers, errors);

            target.ValidateProperty(Keys.EventLogPublishers, errors);

            target.ValidateProperty(Keys.CustomPublishers, errors);

            target.ValidateProperty(Keys.ConsolePublishers, errors);

            target.ValidateProperty(Keys.OperationalTraceLevel, errors);

            target.ValidateProperty(Keys.DefaultPublisher, errors);

            target.ValidateProperty(Keys.CustomEventsLevel, errors);

            target.ValidateProperty(Keys.CustomEvents, errors);

            Assert.AreNotEqual(0, errors.Count);

            errors.Clear();

            target.ValidateProperty("abc", errors);

           

        }

        /// <summary>
        ///A test for ValidateProperty with errors gets generated during validating
        ///operation event trace level due to trace level not defined in properties
        ///</summary>
        [TestMethod()]
        public void ValidateOperationalEventTraceLevelUndefined()
        {
            MockPropertiesGood properties = new MockPropertiesGood();
            
            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);

            properties.PropStore.Remove(Keys.OperationalTraceLevel);
            
            target.ValidateProperty(Keys.OperationalTraceLevel, errors);


            Assert.AreEqual(1, errors.Count);

            errors.Clear();

           

        }

        /// <summary>
        ///A test for ValidateProperty with errors gets generated during validating
        ///queue publishers with bad MSMQ defined in properties
        ///</summary>
        [TestMethod()]
        public void ValidateQueuePublishersBadQueue()
        {
            MockPropertiesGood properties = new MockPropertiesGood();

           
            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);

            // Creating bad queue path
            properties.PropStore[String.Format(Keys.QueuePublisherPath, "Queue1")] = Environment.MachineName + @"\Private$\TestQueueABC$";

            // Setting the delivery for Queue2 to wrong value
            properties.PropStore[String.Format(Keys.QueuePublisherDelivery, "Queue2")] = "dfd";

            target.ValidateProperty(Keys.QueuePublishers, errors);


            Assert.AreEqual(2, errors.Count);

            errors.Clear();

        }


        /// <summary>
        ///A test for ValidateProperty with errors gets generated during validating
        ///console publishers with bad bad output and error streams defined in properties
        ///</summary>
        [TestMethod()]
        public void ValidateConsolePublishersErrors()
        {
            MockPropertiesGood properties = new MockPropertiesGood();


            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);

            // Creating bad stream for Console1 publisher
            properties.PropStore[String.Format(Keys.ConsolePublisherStream, "Console1")] = Environment.MachineName + @"\Private$\TestQueueABC$";

            // Creating bad stream for Console2 publisher
            properties.PropStore[String.Format(Keys.ConsolePublisherStream, "Console2")] = "dfd";

            target.ValidateProperty(Keys.ConsolePublishers, errors);


            Assert.AreEqual(2, errors.Count);

            errors.Clear();

        }

        /// <summary>
        ///A test for ValidateProperty with errors gets generated during validating
        ///email publishers with invalid from and to email address defined in properties
        ///</summary>
        [TestMethod()]
        public void ValidateEmailPublishersErrors()
        {
            MockPropertiesGood properties = new MockPropertiesGood();


            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);

            // Creating bad stream for Console1 publisher
            properties.PropStore[String.Format(Keys.EmailPublisherTo, "Email1")] = Environment.MachineName + @"\Private$\TestQueueABC$";

            // Creating bad stream for Console2 publisher
            properties.PropStore[String.Format(Keys.EmailPublisherFrom, "Email2")] = "dfd";

            target.ValidateProperty(Keys.EmailPublishers, errors);


            Assert.AreEqual(2, errors.Count);

            errors.Clear();

        }


        /// <summary>
        ///A test for ValidateProperty with errors gets generated during validating
        ///custom events
        ///</summary>
        [TestMethod()]
        public void ValidateCustomEventErrors()
        {
            MockPropertiesGood properties = new MockPropertiesGood();


            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);

            // setting the event class name such that the base type is not custom event
            properties.PropStore[String.Format(Keys.CustomEventName, "CustomEvent1")] = "MockPropertiesEmpty";

            // removing the trace level property for CustomEvent2
            properties.PropStore.Remove(String.Format(Keys.CustomEventLevel, "CustomEvent2"));

            target.ValidateProperty(Keys.CustomEvents, errors);


            Assert.AreEqual(2, errors.Count);

            errors.Clear();

        }

        /// <summary>
        ///A test for ValidateProperty with errors gets generated during validating
        ///file publisher properties
        ///</summary>
        [TestMethod()]
        public void ValidateFilePublishersErrors()
        {
            MockPropertiesGood properties = new MockPropertiesGood();


            List<string> errors = new List<string>();

            LoggingPropertyValidator target = new LoggingPropertyValidator(properties);

            // setting the invalid directory
            properties.PropStore[String.Format(Keys.FilePublisherDirectory, "File1")] = "MockPropertiesEmpty";

            // setting the negative invalid file rotation
            properties.PropStore[String.Format(Keys.FilePublisherRotation, "File2")] = "-100";

            target.ValidateProperty(Keys.FilePublishers, errors);


            Assert.AreEqual(2, errors.Count);

            errors.Clear();

        }


    }
}
