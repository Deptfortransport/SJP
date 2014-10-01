using SJP.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SJP.TestProject.Common
{
    
    
    /// <summary>
    ///This is a test class for SJPMessageTest and is intended
    ///to contain all SJPMessageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPMessageTest
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
        ///A test for SJPMessage Constructor
        ///</summary>
        [TestMethod()]
        public void SJPMessageConstructorTest()
        {
            string messageText = "The message";
            string messageResourceId = "MessageResourceId";
            int majorNumber = 6;
            int minorNumber = 3;
            SJPMessageType type = SJPMessageType.Error;
            SJPMessage target = new SJPMessage(messageText, messageResourceId, majorNumber, minorNumber, type);

            Assert.AreEqual(messageText, target.MessageText, "Unexpected message text");
            Assert.AreEqual(messageResourceId, target.MessageResourceId, "Unexpected message resource id");
            Assert.AreEqual(majorNumber, target.MajorMessageNumber, "Unexpected major number");
            Assert.AreEqual(minorNumber, target.MinorMessageNumber, "Unexpected minor number");
            Assert.AreEqual(type, target.Type, "Unexpected message type");
            Assert.AreEqual(string.Empty, target.MessageResourceCollection, "Unexpected message resource collection");
            Assert.AreEqual(string.Empty, target.MessageResourceGroup, "Unexpected message resource group");
            Assert.AreEqual(0, target.MessageArgs.Count, "Unexpected number of message args");
        }

        /// <summary>
        ///A test for SJPMessage Constructor
        ///</summary>
        [TestMethod()]
        public void SJPMessageConstructorTest1()
        {
            string messageText = "The message";
            string messageResourceId = "MessageResourceId";
            int majorNumber = 6;
            int minorNumber = 3;
            SJPMessageType type = SJPMessageType.Warning;
            string messageResourceCollection = "MessageResourceCollection";
            string messageResourceGroup = "MessageResourceGroup";
            List<string> messageArgs = new List<string>(){"ArgOne", "ArgTwo"};
            SJPMessage target = new SJPMessage(messageText, messageResourceId, messageResourceCollection, messageResourceGroup, messageArgs, majorNumber, minorNumber, type);

            Assert.AreEqual(messageText, target.MessageText, "Unexpected message text");
            Assert.AreEqual(messageResourceId, target.MessageResourceId, "Unexpected message resource id");
            Assert.AreEqual(majorNumber, target.MajorMessageNumber, "Unexpected major number");
            Assert.AreEqual(minorNumber, target.MinorMessageNumber, "Unexpected minor number");
            Assert.AreEqual(type, target.Type, "Unexpected message type");
            Assert.AreEqual(messageResourceCollection, target.MessageResourceCollection, "Unexpected message resource collection");
            Assert.AreEqual(messageResourceGroup, target.MessageResourceGroup, "Unexpected message resource group");
            Assert.AreEqual(messageArgs, target.MessageArgs, "Unexpected message args");
        }

        /// <summary>
        ///A test for SJPMessage Constructor
        ///</summary>
        [TestMethod()]
        public void SJPMessageConstructorTest2()
        {
            SJPMessage target = new SJPMessage();

            Assert.AreEqual(string.Empty, target.MessageText, "Unexpected message text");
            Assert.AreEqual(string.Empty, target.MessageResourceId, "Unexpected message resource id");
            Assert.AreEqual(0, target.MajorMessageNumber, "Unexpected major number");
            Assert.AreEqual(0, target.MinorMessageNumber, "Unexpected minor number");
            Assert.AreEqual(SJPMessageType.Info, target.Type, "Unexpected message type");
            Assert.AreEqual(string.Empty, target.MessageResourceCollection, "Unexpected message resource collection");
            Assert.AreEqual(string.Empty, target.MessageResourceGroup, "Unexpected message resource group");
            Assert.AreEqual(0, target.MessageArgs.Count, "Unexpected number of message args");
        }

        /// <summary>
        ///A test for SJPMessage Constructor
        ///</summary>
        [TestMethod()]
        public void SJPMessageConstructorTest3()
        {
            string messageResourceId = "MessageResourceId";
            SJPMessageType type = SJPMessageType.Error;
            SJPMessage target = new SJPMessage(messageResourceId, type);

            Assert.AreEqual(string.Empty, target.MessageText, "Unexpected message text");
            Assert.AreEqual(messageResourceId, target.MessageResourceId, "Unexpected message resource id");
            Assert.AreEqual(0, target.MajorMessageNumber, "Unexpected major number");
            Assert.AreEqual(0, target.MinorMessageNumber, "Unexpected minor number");
            Assert.AreEqual(type, target.Type, "Unexpected message type");
            Assert.AreEqual(string.Empty, target.MessageResourceCollection, "Unexpected message resource collection");
            Assert.AreEqual(string.Empty, target.MessageResourceGroup, "Unexpected message resource group");
            Assert.AreEqual(0, target.MessageArgs.Count, "Unexpected number of message args");
        }

        /// <summary>
        ///A test for MajorMessageNumber
        ///</summary>
        [TestMethod()]
        public void MajorMessageNumberTest()
        {
            SJPMessage target = new SJPMessage(); 
            int expected = 3;
            int actual;
            target.MajorMessageNumber = expected;
            actual = target.MajorMessageNumber;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MessageArgs
        ///</summary>
        [TestMethod()]
        public void MessageArgsTest()
        {
            SJPMessage target = new SJPMessage();
            List<string> expected = new List<string>() { "ArgOne", "ArgTwo" };
            List<string> actual;
            target.MessageArgs = expected;
            actual = target.MessageArgs;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MessageResourceCollection
        ///</summary>
        [TestMethod()]
        public void MessageResourceCollectionTest()
        {
            SJPMessage target = new SJPMessage();
            string expected = "MessageResourceCollection";
            string actual;
            target.MessageResourceCollection = expected;
            actual = target.MessageResourceCollection;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MessageResourceGroup
        ///</summary>
        [TestMethod()]
        public void MessageResourceGroupTest()
        {
            SJPMessage target = new SJPMessage();
            string expected = "MessageResourceGroup";
            string actual;
            target.MessageResourceGroup = expected;
            actual = target.MessageResourceGroup;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MessageResourceId
        ///</summary>
        [TestMethod()]
        public void MessageResourceIdTest()
        {
            SJPMessage target = new SJPMessage();
            string expected = "MessageResourceId";
            string actual;
            target.MessageResourceId = expected;
            actual = target.MessageResourceId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MessageText
        ///</summary>
        [TestMethod()]
        public void MessageTextTest()
        {
            SJPMessage target = new SJPMessage();
            string expected = "MessageText";
            string actual;
            target.MessageText = expected;
            actual = target.MessageText;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MinorMessageNumber
        ///</summary>
        [TestMethod()]
        public void MinorMessageNumberTest()
        {
            SJPMessage target = new SJPMessage();
            int expected = 9;
            int actual;
            target.MinorMessageNumber = expected;
            actual = target.MinorMessageNumber;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        public void TypeTest()
        {
            SJPMessage target = new SJPMessage();
            SJPMessageType expected = SJPMessageType.Warning;
            SJPMessageType actual;
            target.Type = expected;
            actual = target.Type;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SJPMessage Clone
        ///</summary>
        [TestMethod()]
        public void SJPMessageCloneTest()
        {
            string messageResourceId = "MessageResourceId";
            SJPMessageType type = SJPMessageType.Error;
            SJPMessage target = new SJPMessage(messageResourceId, type);

            SJPMessage clone = target.Clone();

            Assert.AreEqual(clone.MessageText, target.MessageText, "Unexpected message text");
            Assert.AreEqual(clone.MessageResourceId, target.MessageResourceId, "Unexpected message resource id");
            Assert.AreEqual(clone.MajorMessageNumber, target.MajorMessageNumber, "Unexpected major number");
            Assert.AreEqual(clone.MinorMessageNumber, target.MinorMessageNumber, "Unexpected minor number");
            Assert.AreEqual(clone.Type, target.Type, "Unexpected message type");
            Assert.AreEqual(clone.MessageResourceCollection, target.MessageResourceCollection, "Unexpected message resource collection");
            Assert.AreEqual(clone.MessageResourceGroup, target.MessageResourceGroup, "Unexpected message resource group");
            Assert.AreEqual(clone.MessageArgs.Count, target.MessageArgs.Count, "Unexpected number of message args");
        }
    }
}
