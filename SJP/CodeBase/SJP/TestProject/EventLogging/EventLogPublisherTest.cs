using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for EventLogPublisherTest and is intended
    ///to contain all EventLogPublisherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventLogPublisherTest
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
        ///A test for Identifier
        ///</summary>
        [TestMethod()]
        public void IdentifierTest()
        {
            string logName = "ELTest";
            string logSource = "ELSource";
            string machineName = "."; // uses current machine

            using (EventLogPublisher target = new EventLogPublisher("SJPIdentifier1", logName, logSource, machineName))
            {
                string actual;
                actual = target.Identifier;

                Assert.AreEqual("SJPIdentifier1", actual);
            }
        }

        /// <summary>
        /// Test for WriteEvent.
        /// </summary>
        [TestMethod()]
        public void TestWriteEvent()
        {
            string logName = "ELTest";
            string logSource = "ELSource";
            string machineName = "."; // uses current machine

            // intialise event log sink
            if (!EventLog.Exists(logName))
            {
                EventSourceCreationData sourceData = new EventSourceCreationData(logSource, logName);
                EventLog.CreateEventSource(sourceData);
            }

            using (EventLog log = new EventLog(logName, machineName, logSource))
            {
                log.Source = logSource;
                log.Clear();
            

                // instantiate a new instance of the EventLogPublisher
                using (EventLogPublisher eventLogPublisher =
                    new EventLogPublisher("SJPIdentifier1", logName, logSource, machineName))
                {
                    bool logExists = EventLog.Exists(logName);

                    Assert.AreEqual(true, logExists, "The log '" + logName + "' " + " was not found on machine '" + machineName + "'");
           
                    // -------------------------------------------------

                    string errorMessage = "Error connecting to database";

                    Object target = new Object();
                    string sessionId = "12345";

                    // Create an operational event and call the
                    // WriteEvent object for the Event Log Publisher

                    OperationalEvent operationalEvent =
                        new OperationalEvent(SJPEventCategory.Database,
                                             SJPTraceLevel.Error,
                                             errorMessage,
                                             target,
                                             sessionId);


                    eventLogPublisher.WriteEvent(operationalEvent);

                    // the number of entries in the log should be equal to 1
                    Assert.AreEqual(1, log.Entries.Count, "The number of entries in the log is incorrect.");

           


                    // -------------------------------------------------

                    // create another operational event and write it to the log

                    string errorMessage2 = "Warning: problem with file XYZ";
                    Object target2 = new Object();

                    // Create an operational event and call the
                    // WriteEvent object for the Event Log Publisher

                    OperationalEvent operationalEvent2 =
                        new OperationalEvent(SJPEventCategory.ThirdParty,
                                             SJPTraceLevel.Warning,
                                             errorMessage2,
                                             target2
                                            );


                    eventLogPublisher.WriteEvent(operationalEvent2);

                    // the number of entries in the log should now be equal to 2
                    Assert.AreEqual(2, log.Entries.Count, "The number of entries in the log is incorrect.");


                    // -------------------------------------------------

                    // create custom events

                    CustomEventOne customEventOne = new CustomEventOne
                        (SJPEventCategory.Business, SJPTraceLevel.Warning,
                        "A custom event one message", Environment.UserName, 12345);

                    // CustomEventOne has an event log formatter defined, therefore
                    // log event should be written in the format specified
                    // in CustomEventOneEventLogFormatter
                    eventLogPublisher.WriteEvent(customEventOne);

                    // the number of entries in the log should now be equal to 3
                    Assert.AreEqual(3, log.Entries.Count, "The number of entries in the log is incorrect.");


                    CustomEventTwo customEventTwo = new CustomEventTwo
                        (SJPEventCategory.ThirdParty, SJPTraceLevel.Error,
                        "A custom event two message", Environment.UserName, 3343);

                    eventLogPublisher.WriteEvent(customEventTwo);

                    // the number of entries in the log should now be equal to 4
                    Assert.AreEqual(4, log.Entries.Count, "The number of entries in the log is incorrect.");

                }
            }

            // Test that Application Event Log can be written to :
            using (EventLogPublisher eventLogPublisherApp =
                new EventLogPublisher("App1", "Application", "TDTestSource", machineName))
            {


                OperationalEvent oe =
                    new OperationalEvent(SJPEventCategory.Database,
                    SJPTraceLevel.Error,
                    "Test error message by TD Event Logging Service");


                eventLogPublisherApp.WriteEvent(oe);
            }


        }
    }
}
