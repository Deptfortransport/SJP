using SJP.Reporting.EventReceiver;
using SJP.Common.ServiceDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Messaging;

namespace SJP.TestProject.EventReceiver
{
    
    
    /// <summary>
    ///This is a test class for EventReceiverServiceTest and is intended
    ///to contain all EventReceiverServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventReceiverServiceTest
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
        ///A test for EventReceiverService Constructor
        ///</summary>
        [TestMethod()]
        public void EventReceiverServiceConstructorTest()
        {
            using (EventReceiverService target = new EventReceiverService())
            {
                Assert.IsNotNull(target, "Expected an object to be returned");
            }
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void DisposeTest()
        {
            PurgeQueues();

            EventReceiverService_Accessor target = new EventReceiverService_Accessor();
            string[] args = new string[] { "/blah" };
            target.OnStart(args);
            bool disposing = true; 
            target.Dispose(disposing);
        }

        /// <summary>
        ///A test for InitializeComponent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void InitializeComponentTest()
        {
            // Inherited method
            EventReceiverService_Accessor target = new EventReceiverService_Accessor(); 
            target.InitializeComponent();
        }

        /// <summary>
        ///A test for OnStart
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void OnStartTest()
        {
            PurgeQueues();

            EventReceiverService_Accessor target = new EventReceiverService_Accessor(); 
            string[] args = new string[]{"/blah"};
            target.OnStart(args);
            Assert.IsNotNull(target.eventReceiver, "Expected event receiver to be initialised");
        }

        /// <summary>
        ///A test for OnStart
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void OnStartTest1()
        {
            PurgeQueues();

            EventReceiverService_Accessor target = new EventReceiverService_Accessor();
            string[] args = new string[] {"/test"};
            target.OnStart(args);
            Assert.IsNull(target.eventReceiver, "Expected event receiver to not be initialised due to test arg");
        }

        /// <summary>
        ///A test for OnStop
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void OnStopTest()
        {
            PurgeQueues();

            EventReceiverService_Accessor target = new EventReceiverService_Accessor();
            string[] args = new string[] { };
            target.OnStart(args);
            target.OnStop();
            Assert.IsNull(target.eventReceiver, "Expected event receiver to have been removed");
        }
    }
}
