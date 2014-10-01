using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;
using System.Collections.Generic;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for InputPageStateTest and is intended
    ///to contain all InputPageStateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InputPageStateTest
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
        ///A test for InputPageState Constructor
        ///</summary>
        [TestMethod()]
        public void InputPageStateConstructorTest()
        {
            InputPageState target = new InputPageState();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for AddMessage
        ///</summary>
        [TestMethod()]
        public void AddMessageTest()
        {
            InputPageState target = new InputPageState(); 
            SJPMessage message = new SJPMessage("ResourceId", SJPMessageType.Info);
            target.AddMessage(message);
            List<SJPMessage> messages = target.Messages;
            Assert.IsTrue(messages.Contains(message), "Message not stored");
        }

        /// <summary>
        ///A test for AddMessages
        ///</summary>
        [TestMethod()]
        public void AddMessagesTest()
        {
            InputPageState target = new InputPageState(); 
            List<SJPMessage> messagesToAdd = new List<SJPMessage>();
            SJPMessage message = new SJPMessage("ResourceId", SJPMessageType.Info);
            messagesToAdd.Add(message);
            target.ClearMessages();
            target.AddMessages(messagesToAdd);
            Assert.IsTrue(target.Messages.Contains(message), "Messages not stored");
        }

        /// <summary>
        ///A test for ClearMessages
        ///</summary>
        [TestMethod()]
        public void ClearMessagesTest()
        {
            InputPageState target = new InputPageState(); 
            List<SJPMessage> messagesToAdd = new List<SJPMessage>();
            SJPMessage message = new SJPMessage("ResourceId", SJPMessageType.Info);
            messagesToAdd.Add(message);
            target.AddMessages(messagesToAdd);
            target.ClearMessages();
            Assert.IsTrue(!target.Messages.Contains(message), "Messages not cleared");
        }

        /// <summary>
        ///A test for IsDirty
        ///</summary>
        [TestMethod()]
        public void IsDirtyTest()
        {
            InputPageState target = new InputPageState();
            bool expected = true; 
            bool actual;
            target.IsDirty = expected;
            actual = target.IsDirty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyIdOutward
        ///</summary>
        [TestMethod()]
        public void JourneyIdOutwardTest()
        {
            InputPageState target = new InputPageState(); 
            int expected = 7; 
            int actual;
            target.JourneyIdOutward = expected;
            actual = target.JourneyIdOutward;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyIdReturn
        ///</summary>
        [TestMethod()]
        public void JourneyIdReturnTest()
        {
            InputPageState target = new InputPageState(); 
            int expected = 3;
            int actual;
            target.JourneyIdReturn = expected;
            actual = target.JourneyIdReturn;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyLegDetailExpandedOutward
        ///</summary>
        [TestMethod()]
        public void JourneyLegDetailExpandedOutwardTest()
        {
            InputPageState target = new InputPageState(); 
            bool expected = true; 
            bool actual;
            target.JourneyLegDetailExpandedOutward = expected;
            actual = target.JourneyLegDetailExpandedOutward;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyLegDetailExpandedReturn
        ///</summary>
        [TestMethod()]
        public void JourneyLegDetailExpandedReturnTest()
        {
            InputPageState target = new InputPageState();
            bool expected = true; 
            bool actual;
            target.JourneyLegDetailExpandedReturn = expected;
            actual = target.JourneyLegDetailExpandedReturn;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for JourneyRequestHash
        ///</summary>
        [TestMethod()]
        public void JourneyRequestHashTest()
        {
            InputPageState target = new InputPageState(); 
            string expected = "hash";
            string actual;
            target.JourneyRequestHash = expected;
            actual = target.JourneyRequestHash;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Messages
        ///</summary>
        [TestMethod()]
        public void MessagesTest()
        {
            InputPageState target = new InputPageState(); 
            List<SJPMessage> expected = new List<SJPMessage>();
            expected.Add(new SJPMessage("SessionId", SJPMessageType.Info));
            List<SJPMessage> actual;
            target.Messages = expected;
            actual = target.Messages;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for StopEventJourneyIdOutward
        ///</summary>
        [TestMethod()]
        public void StopEventJourneyIdOutwardTest()
        {
            InputPageState target = new InputPageState();
            int expected = 8; 
            int actual;
            target.StopEventJourneyIdOutward = expected;
            actual = target.StopEventJourneyIdOutward;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for StopEventJourneyIdReturn
        ///</summary>
        [TestMethod()]
        public void StopEventJourneyIdReturnTest()
        {
            InputPageState target = new InputPageState(); 
            int expected = 4; 
            int actual;
            target.StopEventJourneyIdReturn = expected;
            actual = target.StopEventJourneyIdReturn;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for StopEventRequestHash
        ///</summary>
        [TestMethod()]
        public void StopEventRequestHashTest()
        {
            InputPageState target = new InputPageState(); 
            string expected = "hash2";
            string actual;
            target.StopEventRequestHash = expected;
            actual = target.StopEventRequestHash;
            Assert.AreEqual(expected, actual);
        }
    }
}
