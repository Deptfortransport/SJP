
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;
using SJP.Common.PropertyManager;
using SJP.Common.EventLogging;
using SJP.TestProject.EventLogging.MockObjects;
using System.Messaging;
using ER = SJP.Reporting.EventReceiver;
using SJP.Reporting.Events;

namespace SJP.TestProject.EventReceiver
{
    
    
    /// <summary>
    ///This is a test class for EventReceiverTest and is intended
    ///to contain all EventReceiverTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventReceiverTest
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
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());

            string queueName = string.Format(@"{0}\Private$\ERTestQueue1$", Environment.MachineName);

            if (!MessageQueue.Exists(queueName))
            {
                using (MessageQueue newQueue1 = MessageQueue.Create(queueName, false)) { }
            }

            queueName = string.Format(@"{0}\Private$\ERTestQueue2$", Environment.MachineName);

            if (!MessageQueue.Exists(queueName))
            {
                using (MessageQueue newQueue1 = MessageQueue.Create(queueName, false)) { }
            }
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            PurgeQueues();
        }
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
        /// Purges messages in the test queues
        /// </summary>
        public static void PurgeQueues()
        {
            string queueName = string.Format(@"{0}\Private$\ERTestQueue1$", Environment.MachineName);

            if (MessageQueue.Exists(queueName))
            {
                using (MessageQueue queue1 = new MessageQueue(queueName))
                {
                    queue1.Purge();
                }
            }

            queueName = string.Format(@"{0}\Private$\ERTestQueue2$", Environment.MachineName);

            if (MessageQueue.Exists(queueName))
            {
                using (MessageQueue queue2 = new MessageQueue(queueName))
                {
                    queue2.Purge();
                }
            }
        }

        /// <summary>
        ///A test for EventReceiver Constructor
        ///</summary>
        [TestMethod()]
        public void EventReceiverConstructorTest()
        {
            using (ER.EventReceiver target = new ER.EventReceiver())
            {
                Assert.IsNotNull(target, "Expected an object to be returned");

                try
                {
                    // need to run or dispose fails
                    target.Run();
                }
                catch
                {
                    // Ignore exceptions
                }
            }
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void DisposeTest()
        {
            ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor();
            bool disposing = true;

            try
            {
                target.Run(new MockPropertiesGoodProperties());
            }
            finally
            {
                target.Dispose(disposing);
            }
            
            Assert.IsTrue(target.messageQueueList.Count == 0, "Expected message queues to be removed");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest1()
        {
            // Method doesn't do anything so can't test for a result
            ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor();

            try
            {
                target.Run(new MockPropertiesGoodProperties());
            }
            finally
            {
                target.Dispose();
            }
            
            Assert.IsTrue(target.messageQueueList.Count == 0, "Expected message queues to be removed");
        }

        /// <summary>
        ///A test for Finalize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void FinalizeTest()
        {
            // Nothing to check
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                target.Finalize();
            }
        }

        /// <summary>
        ///A test for InitQueueList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void InitQueueListTest()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                IPropertyProvider properties = new MockPropertiesGoodProperties();
                target.InitQueueList(properties);
                Assert.IsTrue(target.messageQueueList.Count > 0, "Expected the message queue list to have been initialised");
            }
        }

        /// <summary>
        ///A test for OnDefaultPublisherCalled
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void OnDefaultPublisherCalledTest()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                target.Run(new MockPropertiesGoodProperties());
                DefaultPublisherCalledEventArgs e = new DefaultPublisherCalledEventArgs(new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "Event message"));
                object sender = null;
                target.OnDefaultPublisherCalled(sender, e);
                Assert.IsTrue(target.messageQueueList.Count == 0, "Expected message queues to be removed");
            }
        }

        /// <summary>
        ///A test for Receive
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void ReceiveTest()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                target.Run(new MockPropertiesGoodProperties());

                JourneyPlanRequestEvent je = new JourneyPlanRequestEvent("RequestId", new System.Collections.Generic.List<SJP.Common.SJPModeType> { SJP.Common.SJPModeType.Rail }, true, "SessionId");
                OperationalEvent oe = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "Operational event message");
                string queueName = string.Format(@"{0}\Private$\ERTestQueue1$", Environment.MachineName);

                // empty the queue in case any existing events exist
                using (MessageQueue queue = new MessageQueue(queueName))
                {
                    queue.Formatter = new BinaryMessageFormatter();
                    queue.Purge();

                    // create a new queue publisher
                    using (QueuePublisher queuePublisher =
                         new QueuePublisher("Identifier", MessagePriority.Normal, queueName, true))
                    {
                        queuePublisher.WriteEvent(je);
                        queuePublisher.WriteEvent(oe);
                    }

                    System.Threading.Thread.Sleep(5000);

                    Assert.IsTrue(queue.GetAllMessages().Length == 0, "Expected the message queue to be empty.");
                }
            }
        }

        /// <summary>
        ///A test for RecoverFromRemoteException
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void RecoverFromRemoteExceptionTest()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                target.Run(new MockPropertiesGoodProperties());
                target.RecoverFromRemoteException();
                Assert.IsTrue(target.messageQueueList.Count > 0, "Expected queues to have been re-initialised");
            }
        }

        /// <summary>
        ///A test for Run
        ///</summary>
        [TestMethod()]
        public void RunTest()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                target.Run();
                Assert.IsTrue(target.messageQueueList.Count > 0, "Expected the queues to have been initialised");
            }
        }

        /// <summary>
        ///A test for Run
        ///</summary>
        [TestMethod()]
        public void RunTest1()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                IPropertyProvider pp = new MockPropertiesGoodProperties();
                target.Run(pp);
                Assert.IsTrue(target.messageQueueList.Count > 0, "Expected message queues to be initialised");
            }
        }

        /// <summary>
        ///A test for SetupQueues
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void SetupQueuesTest()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                target.InitQueueList(new MockPropertiesGoodProperties());
                target.SetupQueues();
                Assert.IsNotNull(target.messageQueueList[0].Formatter, "Expected queues to be setup");
            }
        }

        /// <summary>
        ///A test for StopMessageQueues
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void StopMessageQueuesTest()
        {
            using (ER.EventReceiver_Accessor target = new ER.EventReceiver_Accessor())
            {
                target.Run(new MockPropertiesGoodProperties());
                target.StopMessageQueues();
                Assert.IsTrue(target.messageQueueList.Count == 0, "Expected message queues to have been removed");
            }
        }
    }
}
