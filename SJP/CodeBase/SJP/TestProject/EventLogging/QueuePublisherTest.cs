using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Messaging;
using SJP.Common;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for QueuePublisherTest and is intended
    ///to contain all QueuePublisherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class QueuePublisherTest
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
        [TestCleanup()]
        public void MyTestCleanup()
        {
            string queuePath = Environment.MachineName + "\\Private$\\event_test_queue$";

            // Purge the queue to avoid issues with other tests in the run
            if (MessageQueue.Exists(queuePath))
            {
                using (MessageQueue queue = new MessageQueue(queuePath)) { queue.Purge(); }
            }
        }
        //
        #endregion


       
        /// <summary>
        ///A test for WriteEvent
        ///</summary>
        [TestMethod()]
        public void WriteEventTest()
        {
            // ---------------------------------------------

            // perform initialisation


            string queuePath = Environment.MachineName + "\\Private$\\event_test_queue$";

            // Create the (nontransactional) queue if it does not exist.
            if (!MessageQueue.Exists(queuePath))
            {
                using (MessageQueue newQueue = MessageQueue.Create(queuePath, false)) { }
            }

            // ---------------------------------------------

            // create test data - 16 operational events and 2 custom events

            OperationalEvent[] operationalEvents =
                new OperationalEvent[16];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");

            operationalEvents[1] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Info,
                "Test Error 2: A problem occurred whilst attempting to load G5265",
                "Test Target Number 2", "456");

            operationalEvents[2] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Warning,
                "Test Error 3: Connection XYF failed",
                "Test Target Number 3", "789");

            operationalEvents[3] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Verbose,
                "Test Error 4: Timeout occurred processing H6001.aspx",
                "Test Target Number 4", "012");

            operationalEvents[4] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Error,
                "Test Error 5: Error Number 1234HDJ has occurred",
                "Test Target Number 5", "345");

            operationalEvents[5] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Warning,
                "Test Error 6: Page Index.aspx failed to load",
                "Test Target Number 6", "678");

            operationalEvents[6] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Verbose,
                "Test Error 7: Server SG12323 has failed",
                "Test Target Number 7", "901");

            operationalEvents[7] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Info,
                "Test Error 8: Server SG123 failed to reboot",
                "Test Target Number 8", "234");

            operationalEvents[8] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Info,
                "Test Error 9: Component C123 malfunction",
                "Test Target Number 9", "567");

            operationalEvents[9] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Warning,
                "Test Error 10: Hard disk failure on SG134",
                "Test Target Number 10", "890");

            operationalEvents[10] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Warning,
                "Test Error 11: SG123 has failed to initialise",
                "Test Target Number 11", "123");

            operationalEvents[11] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Verbose,
                "Test Error 12: Database connection error on 'Journeys'",
                "Test Target Number 12", "456");

            operationalEvents[12] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Info,
                "Test Error 13: Unknown Error",
                "Test Target Number 13", "789");

            operationalEvents[13] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Info,
                "Test Error 14: Error writing to audit trail",
                "Test Target Number 14", "012");

            operationalEvents[14] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Warning,
                "Test Error 15: Driver D1 failed initialisation routine",
                "Test Target Number 15");

            operationalEvents[15] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Info,
                "Test Error 16: D123 failed",
                "Test Target Number 16");


            // create custom events
            CustomEventOne customEventOne = new CustomEventOne
                (SJPEventCategory.Business, SJPTraceLevel.Warning,
                "A custom event one message", Environment.UserName, 12345);

            CustomEventTwo customEventTwo = new CustomEventTwo
                (SJPEventCategory.ThirdParty, SJPTraceLevel.Error,
                "A custom event two message", Environment.UserName, 3343);

            // empty the queue in case any existing events exist
            using(MessageQueue queue = new MessageQueue(queuePath))
            {
                queue.Formatter = new BinaryMessageFormatter();
                queue.Purge();



                // ---------------------------------------------

                // create a new queue publisher
                using (QueuePublisher queuePublisher =
                     new QueuePublisher("Identifier", MessagePriority.Normal, queuePath, true))
                {
                    // call queuePublisher's WriteEvent() for each of the operational events
                    foreach (OperationalEvent oe in operationalEvents)
                    {
                        queuePublisher.WriteEvent(oe);
                    }

                    // write custom events to queue
                    queuePublisher.WriteEvent(customEventOne);
                    queuePublisher.WriteEvent(customEventTwo);
                }

                // ---------------------------------------------

                // expect the queue to have 18 messages
                Assert.AreEqual(18, queue.GetAllMessages().Length, "Queue does not have 18 messages");

                // test each message from the queue
                OperationalEvent actualEvent, expectedEvent;

                for (int i = 0; i < 16; i++)
                {
                    expectedEvent = operationalEvents[i];
                    actualEvent = (OperationalEvent)queue.Receive().Body;

                    // Assert that there is one less item in the queue
                    Assert.AreEqual((18 - (i + 1)), queue.GetAllMessages().Length, "Number of messages in queue is incorrect.");

                    // compare the OperationalEvent from the queue with the expected event
                    Assert.AreEqual(expectedEvent.Category.ToString(), actualEvent.Category.ToString(), "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.ConsoleFormatter, actualEvent.ConsoleFormatter, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.EmailFormatter, actualEvent.EmailFormatter, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.EventLogFormatter, actualEvent.EventLogFormatter, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.FileFormatter, actualEvent.FileFormatter, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.Filter, actualEvent.Filter, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.Level, actualEvent.Level, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.Message, actualEvent.Message, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.MethodName, actualEvent.MethodName, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.SessionId, actualEvent.SessionId, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.Target, actualEvent.Target, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.Time, actualEvent.Time, "Message Number:" + (i + 1));
                    Assert.AreEqual(expectedEvent.TypeName, actualEvent.TypeName, "");
                }

                // expect the queue to have 2 messages left
                // i.e., the two custom events
                Assert.AreEqual(2, queue.GetAllMessages().Length, "Queue does not have 2 messages");



                CustomEventOne actualCustomEventOne =
                         (CustomEventOne)queue.Receive().Body;

                CustomEventTwo actualCustomEventTwo =
                        (CustomEventTwo)queue.Peek().Body;

                // test custom event one



                Assert.AreEqual
                    (customEventOne.Category.ToString(), actualCustomEventOne.Category.ToString(), "Custom Event One");

                Assert.AreEqual
                    (customEventOne.ConsoleFormatter, actualCustomEventOne.ConsoleFormatter, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.DefaultFormatter, actualCustomEventOne.DefaultFormatter, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.EmailFormatter, actualCustomEventOne.EmailFormatter, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.EventLogFormatter, actualCustomEventOne.EventLogFormatter, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.FileFormatter, actualCustomEventOne.FileFormatter, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.Filter, actualCustomEventOne.Filter, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.Level, actualCustomEventOne.Level, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.Message, actualCustomEventOne.Message, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.ReferenceNumber, actualCustomEventOne.ReferenceNumber, "Custom Event One");

                Assert.AreEqual
                    (customEventOne.Time, actualCustomEventOne.Time, "Custom Event One");


                // test custom event two


                Assert.AreEqual
                    (customEventTwo.Category.ToString(), actualCustomEventTwo.Category.ToString(), "Custom Event Two");

                Assert.AreEqual
                    (actualCustomEventTwo.ConsoleFormatter, customEventTwo.ConsoleFormatter, "Custom Event Two");

                Assert.AreEqual
                    (customEventTwo.DefaultFormatter, customEventTwo.DefaultFormatter, "Custom Event Two");

                Assert.AreEqual
                    (actualCustomEventTwo.EmailFormatter, customEventTwo.EmailFormatter, "Custom Event Two");

                Assert.AreEqual
                    (actualCustomEventTwo.EventLogFormatter, customEventTwo.EventLogFormatter, "Custom Event Two");

                Assert.AreEqual
                    (actualCustomEventTwo.FileFormatter, customEventTwo.FileFormatter, "Custom Event Two");

                Assert.AreEqual
                    (customEventTwo.Filter, actualCustomEventTwo.Filter, "Custom Event Two");

                               

            }
            
        }

        
        /// <summary>
        /// Tests MessageQueueException exception raised in queue publisher write event method
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestMessageQueueException()
        {
            // ---------------------------------------------

            // perform initialisation


            string queuePath =string.Empty;

           
            using (QueuePublisher queuePublisher =
                    new QueuePublisher("Identifier", MessagePriority.Normal, queuePath, true))
            {
                // create custom events
                CustomEventOne customEventOne = new CustomEventOne
                    (SJPEventCategory.Business, SJPTraceLevel.Warning,
                    "A custom event one message", Environment.UserName, 12345);

                queuePublisher.WriteEvent(customEventOne);
            }
           

        }

        /// <summary>
        /// Tests exception raised in queue publisher write event method
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestException()
        {
            // ---------------------------------------------

            // perform initialisation


            string queuePath = Environment.MachineName + "\\Private$\\event_test_queue$";

            if (MessageQueue.Exists(queuePath))
            {
                MessageQueue.Delete(queuePath);
            }

            // Create the (nontransactional) queue if it does not exist.
            if (!MessageQueue.Exists(queuePath))
            {
                using (MessageQueue newQueue = MessageQueue.Create(queuePath, false)) { }

            }

            // empty the queue in case any existing events exist
            using (MessageQueue queue = new MessageQueue(queuePath, false))
            {
                queue.Formatter = new BinaryMessageFormatter();
                queue.Purge();

                // ---------------------------------------------
                try
                {
                    using (QueuePublisher queuePublisher =
                         new QueuePublisher("Identifier", MessagePriority.Normal, queuePath, true))
                    {

                        queuePublisher.WriteEvent(null);
                    }
                }
                finally
                {
                    if (MessageQueue.Exists(queuePath))
                    {
                        MessageQueue.Delete(queuePath);
                    }
                }
            }

        }
        
    }
}
