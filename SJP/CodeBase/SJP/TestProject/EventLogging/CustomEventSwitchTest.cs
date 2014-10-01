using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using SJP.TestProject.EventLogging.MockObjects;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Diagnostics;
using SJP.Common;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for CustomEventSwitchTest and is intended
    ///to contain all CustomEventSwitchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomEventSwitchTest
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
        ///A test for CheckLevel when global event level is undefined
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.eventlogging.dll")]
        public void CheckLevelTestGlobalLevelOff()
        {
            CustomEventSwitch ces = new CustomEventSwitch();
            string eventClassName = "UnknownCustomEvent";
            bool expected = false;
            bool actual;
            CustomEventSwitch_Accessor.globalLevel = CustomEventLevel.Off;
            actual = CustomEventSwitch_Accessor.CheckLevel(CustomEventLevel.On, eventClassName);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for CheckLevel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.eventlogging.dll")]
        [ExpectedException(typeof(SJPException))]
        public void CheckLevelTestException()
        {
            IPropertyProvider goodProperties = new MockPropertiesCoreCustomPublisher();

            IEventPublisher[] customPublishers = new IEventPublisher[1];

            string workingDirectory = Environment.CurrentDirectory + "\\EmailAttachWorkDir";

            Directory.CreateDirectory(workingDirectory);

            List<string> errors = new List<string>();

            customPublishers[0] =
                new CustomEmailPublisher("EMAIL",
                                         "FromSomeone@slb.com",
                                         MailPriority.Normal,
                                         "localhost",
                                         workingDirectory,
                                         errors);

            Assert.IsTrue(errors.Count == 0);

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties,
                                                        customPublishers,
                                                        errors));
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }

            bool expected = true; 
            bool actual;
            CustomEventSwitch_Accessor.globalLevel = CustomEventLevel.Undefined;
            actual = CustomEventSwitch_Accessor.CheckLevel();
            Assert.AreEqual(expected, actual);
            
        }
              

        /// <summary>
        ///A test for CheckLevel when individual level for unknown event class name not defined
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.eventlogging.dll")]
        [ExpectedException(typeof(SJPException))]
        public void CheckLevelTestExceptionUnknownEventClass()
        {
            IPropertyProvider goodProperties = new MockPropertiesCoreCustomPublisher();

            IEventPublisher[] customPublishers = new IEventPublisher[1];

            string workingDirectory = Environment.CurrentDirectory + "\\EmailAttachWorkDir";

            Directory.CreateDirectory(workingDirectory);

            List<string> errors = new List<string>();

            customPublishers[0] =
                new CustomEmailPublisher("EMAIL",
                                         "FromSomeone@slb.com",
                                         MailPriority.Normal,
                                         "localhost",
                                         workingDirectory,
                                         errors);

            Assert.IsTrue(errors.Count == 0);

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties,
                                                        customPublishers,
                                                        errors));
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }


            string eventClassName = "UnknownCustomEvent";
            bool expected = false;
            bool actual;
            actual = CustomEventSwitch_Accessor.CheckLevel(CustomEventLevel.On, eventClassName);
            Assert.AreEqual(expected, actual);

        }
                

        /// <summary>
        ///A test for Off
        ///</summary>
        [TestMethod()]
        public void OffTest()
        {
            IPropertyProvider goodProperties = new MockPropertiesCoreCustomPublisher();

            IEventPublisher[] customPublishers = new IEventPublisher[1];

            string workingDirectory = Environment.CurrentDirectory + "\\EmailAttachWorkDir";

            Directory.CreateDirectory(workingDirectory);

            List<string> errors = new List<string>();

            customPublishers[0] =
                new CustomEmailPublisher("EMAIL",
                                         "FromSomeone@slb.com",
                                         MailPriority.Normal,
                                         "localhost",
                                         workingDirectory,
                                         errors);

            Assert.IsTrue(errors.Count == 0);

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties,
                                                        customPublishers,
                                                        errors));
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }


            string eventClassName = "CustomEmailEvent"; 
            bool expected = false; 
            bool actual;
            actual = CustomEventSwitch.Off(eventClassName);

            Assert.AreEqual(expected, actual);

            // turning off the CustomEmailEvent 
            CustomEventSwitch_Accessor.individualLevels[eventClassName] = CustomEventLevel.Off;
            expected = true;
            actual = CustomEventSwitch.Off(eventClassName);

            Assert.AreEqual(expected, actual);



           
        }

        /// <summary>
        ///A test for On
        ///</summary>
        [TestMethod()]
        public void OnTest()
        {
            IPropertyProvider goodProperties = new MockPropertiesCoreCustomPublisher();

            IEventPublisher[] customPublishers = new IEventPublisher[1];

            string workingDirectory = Environment.CurrentDirectory + "\\EmailAttachWorkDir";

            Directory.CreateDirectory(workingDirectory);

            List<string> errors = new List<string>();

            customPublishers[0] =
                new CustomEmailPublisher("EMAIL",
                                         "FromSomeone@slb.com",
                                         MailPriority.Normal,
                                         "localhost",
                                         workingDirectory,
                                         errors);

            Assert.IsTrue(errors.Count == 0);

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties,
                                                        customPublishers,
                                                        errors));
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }


            string eventClassName = "CustomEmailEvent";
            bool expected = true;
            bool actual;
            actual = CustomEventSwitch.On(eventClassName);

            Assert.AreEqual(expected, actual);

            // turning off the CustomEmailEvent 
            CustomEventSwitch_Accessor.individualLevels[eventClassName] = CustomEventLevel.Off;
            expected = false;
            actual = CustomEventSwitch.On(eventClassName);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GlobalOn
        ///</summary>
        [TestMethod()]
        public void GlobalOnTest()
        {
            IPropertyProvider goodProperties = new MockPropertiesCoreCustomPublisher();

            IEventPublisher[] customPublishers = new IEventPublisher[1];

            string workingDirectory = Environment.CurrentDirectory + "\\EmailAttachWorkDir";

            Directory.CreateDirectory(workingDirectory);

            List<string> errors = new List<string>();

            customPublishers[0] =
                new CustomEmailPublisher("EMAIL",
                                         "FromSomeone@slb.com",
                                         MailPriority.Normal,
                                         "localhost",
                                         workingDirectory,
                                         errors);

            Assert.IsTrue(errors.Count == 0);

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties,
                                                        customPublishers,
                                                        errors));
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }
            
            bool actual;
            actual = CustomEventSwitch.GlobalOn;

            Assert.IsTrue(actual);
            
        }
    }
}
