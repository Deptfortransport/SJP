using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for CustomEmailEventTest and is intended
    ///to contain all CustomEmailEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomEmailEventTest
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
        ///A test for CustomEmailEvent Constructor
        ///</summary>
        [TestMethod()]
        public void CustomEmailEventConstructorTest()
        {
            string origin = "SJPSupport";
            string destination = "SJPClient";
            string bodyText = "leicester to beeston";
            string subject = "test";
            CustomEmailEvent target = new CustomEmailEvent(origin, destination, bodyText, subject);

            Assert.AreEqual(origin, target.From);
            Assert.AreEqual(destination, target.To);
            Assert.AreEqual(bodyText, target.BodyText);
            Assert.AreEqual(subject, target.Subject);

            Assert.IsNull(target.ConsoleFormatter);
            
            Assert.IsNull(target.EmailFormatter);
            Assert.IsNull(target.EventLogFormatter);
            Assert.IsNull(target.FileFormatter);
            
            Assert.IsNotNull(target.DefaultFormatter);
            Assert.IsInstanceOfType(target.DefaultFormatter, typeof(SJP.Common.EventLogging.DefaultFormatter));

        }

        /// <summary>
        ///A test for CustomEmailEvent Constructor
        ///</summary>
        [TestMethod()]
        public void CustomEmailEventConstructorTest1()
        {
            string origin = "SJPSupport";
            string destination = "SJPClient";
            string bodyText = "leicester to beeston";
            string subject = "test";
            CustomEmailEvent target = null;

            using (Stream attachment = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(attachment);
                
                writer.Write("Attachment");
                

                string attachmentName = string.Empty; 
                target = new CustomEmailEvent(origin, destination, bodyText, subject, attachment, attachmentName);
                Assert.AreEqual(origin, target.From);
                Assert.AreEqual(destination, target.To);
                Assert.AreEqual(bodyText, target.BodyText);
                Assert.AreEqual(subject, target.Subject);


                Assert.IsNotNull(target.AttachmentStream);
                Assert.IsTrue(target.HasAttachment);
                Assert.IsTrue(target.HasAttachmentStream);
            }

            
        }

        /// <summary>
        ///A test for CustomEmailEvent Constructor
        ///</summary>
        [TestMethod()]
        public void CustomEmailEventConstructorTest2()
        {
            string origin = "SJPSupport";
            string destination = "SJPClient";
            string bodyText = "leicester to beeston";
            string subject = "test";
            string attachment = "TestAttachmentPath";
            string attachmentName = "TestAttachment";
            CustomEmailEvent target = new CustomEmailEvent(origin, destination, bodyText, subject, attachment, attachmentName);

            Assert.AreEqual(origin, target.From);
            Assert.AreEqual(destination, target.To);
            Assert.AreEqual(bodyText, target.BodyText);
            Assert.AreEqual(subject, target.Subject);
            Assert.AreEqual(attachment, target.AttachmentFilePath);
            Assert.AreEqual(attachmentName, target.AttachmentName);
            Assert.IsTrue(target.HasAttachmentFilePath);
            Assert.IsTrue(target.HasAttachment);
        }

        /// <summary>
        ///A test for CustomEmailEvent Constructor
        ///</summary>
        [TestMethod()]
        public void CustomEmailEventConstructorTest3()
        {
            string destination = "SJPClient";
            string bodyText = "leicester to beeston";
            string subject = "test";

            CustomEmailEvent target = new CustomEmailEvent(destination, bodyText, subject);
            Assert.AreEqual(destination, target.To);
            Assert.AreEqual(bodyText, target.BodyText);
            Assert.AreEqual(subject, target.Subject);
            Assert.IsFalse(target.HasAttachmentFilePath);
            Assert.IsFalse(target.HasAttachment);
            Assert.IsFalse(target.HasAttachmentStream);
        }

        /// <summary>
        ///A test for CustomEmailEvent Constructor
        ///</summary>
        [TestMethod()]
        public void CustomEmailEventConstructorTest4()
        {
            string destination = "SJPClient";
            string bodyText = "leicester to beeston";
            string subject = "test";
            string attachmentName = "TestAttachment";

            CustomEmailEvent target = null;

            using (Stream attachment = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(attachment);
                
                writer.Write("Attachment");
                writer.Flush();
                

                
                target = new CustomEmailEvent(destination, bodyText, subject, attachment, attachmentName);
                Assert.AreEqual(destination, target.To);
                Assert.AreEqual(bodyText, target.BodyText);
                Assert.AreEqual(subject, target.Subject);

                Assert.IsNotNull(target.AttachmentStream);
                Assert.AreEqual(attachmentName, target.AttachmentName);
                Assert.IsTrue(target.HasAttachment);
                Assert.IsTrue(target.HasAttachmentStream);
            }

           
        }

        /// <summary>
        ///A test for CustomEmailEvent Constructor
        ///</summary>
        [TestMethod()]
        public void CustomEmailEventConstructorTest5()
        {
            string destination = "SJPClient";
            string bodyText = "leicester to beeston";
            string subject = "test";
            string attachment = "TestAttachmentPath";
            string attachmentName = "TestAttachment";
            CustomEmailEvent target = new CustomEmailEvent(destination, bodyText, subject, attachment, attachmentName);

            Assert.AreEqual(destination, target.To);
            Assert.AreEqual(bodyText, target.BodyText);
            Assert.AreEqual(subject, target.Subject);
            Assert.AreEqual(attachment, target.AttachmentFilePath);
            Assert.AreEqual(attachmentName, target.AttachmentName);
            Assert.IsTrue(target.HasAttachmentFilePath);
            Assert.IsTrue(target.HasAttachment);
        }



        

        
    }
}
