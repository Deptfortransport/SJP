using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.TestProject.EventLogging.MockObjects;
using SJP.Common.PropertyManager;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Diagnostics;
using SJP.Common;
using System.Configuration;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for CustomEventFilterTest and is intended
    ///to contain all CustomEventFilterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomEventFilterTest
    {

        private string singleEmailAddress = ConfigurationManager.AppSettings["singleEmailAddress"];
        private string multiEmailAddress = ConfigurationManager.AppSettings["multiEmailAddress"];
        private string attachmentPath = ConfigurationManager.AppSettings["attachmentPath"];
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
        ///A test for ShouldLog
        ///</summary>
        [TestMethod()]
        public void ShouldLogTest()
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

            Assert.IsTrue(errors.Count == 0);

            // test sending to single email address
            CustomEmailEvent e1 = new CustomEmailEvent(singleEmailAddress, "Hello!", "UNIT_TEST:PublishWithoutError");
            e1.AuditPublishersOff = false;

            CustomEventFilter target = new CustomEventFilter();

            Assert.IsTrue(target.ShouldLog(e1));

            // turning off the CustomEmailEvent 
            CustomEventSwitch_Accessor.individualLevels["CustomEmailEvent"] = CustomEventLevel.Off;
                     
            Assert.IsFalse(target.ShouldLog(e1));

            // Remove working directory for next test
            Directory.Delete(workingDirectory);

            // ensure working directory has been tidied up (ie removed)
            bool dirs = true;
            dirs = Directory.Exists(workingDirectory);
            Assert.IsFalse(dirs, "Working Directory not removed");

        }
    }
}
