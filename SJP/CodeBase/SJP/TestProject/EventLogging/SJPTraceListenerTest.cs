using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using System.Collections.Generic;
using System.Diagnostics;
using SJP.TestProject.EventLogging.MockObjects;
using System.IO;
using System.Collections;
using SJP.Common;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for SJPTraceListenerTest and is intended
    ///to contain all SJPTraceListenerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPTraceListenerTest
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
        [ClassInitialize()]
        public static void SetUp(TestContext testContext)
        {
            Trace.Listeners.Remove("SJPTraceListener");

            // delete file publisher dirs
            IPropertyProvider MockPropertiesGood = new MockPropertiesGood();

            DirectoryInfo di1 = new DirectoryInfo(MockPropertiesGood[String.Format(Keys.FilePublisherDirectory, "File1")]);
            di1.Delete(true);

            DirectoryInfo di2 = new DirectoryInfo(MockPropertiesGood[String.Format(Keys.FilePublisherDirectory, "File2")]);
            di2.Delete(true);
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void Cleanup()
        {
            Trace.Listeners.Remove("SJPTraceListener");

            // delete file publisher dirs
            IPropertyProvider MockPropertiesGood = new MockPropertiesGood();

            DirectoryInfo di1 = new DirectoryInfo(MockPropertiesGood[String.Format(Keys.FilePublisherDirectory, "File1")]);
            di1.Delete(true);

            DirectoryInfo di2 = new DirectoryInfo(MockPropertiesGood[String.Format(Keys.FilePublisherDirectory, "File2")]);
            di2.Delete(true);

        }
        
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Trace.Listeners.Remove("SJPTraceListener");
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void SJPTraceListenerUnsupportedPrototypes()
        {
            Cleanup();

            IPropertyProvider MockPropertiesGoodMinimumProperties = new MockPropertiesGoodMinimumProperties();
            IEventPublisher[] customPublishers = new IEventPublisher[0];
            List<string> errors = new List<string>();

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(MockPropertiesGoodMinimumProperties, customPublishers, errors));
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }

            Assert.IsTrue(errors.Count == 0);

            // call each unsupported prototype - no exceptions should result
            try
            {
                Exception testObject = new Exception("test object");
                Trace.Write("My call to Write(string)");
                Trace.Write((object)testObject, "My call to Write(object,string)");
                Trace.Write("My message for Write(string,string)", "My category for Write(string, string)");
                Trace.WriteLine("My call to WriteLine(string)");
                Trace.WriteLine((object)testObject, "My call to WriteLine(object,string)");
                Trace.WriteLine("My message for WriteLine(string,string)", "My category for WriteLine(string, string)");
                Trace.WriteLine((object)testObject);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }


            DirectoryInfo publisherDir = new DirectoryInfo(MockPropertiesGoodMinimumProperties[String.Format(Keys.FilePublisherDirectory, "File1")] + "\\");
            FileInfo[] fileInfoArray = publisherDir.GetFiles("*.txt");

            Assert.IsTrue(fileInfoArray.Length > 0);

            FileInfo tempFile = fileInfoArray[0];
            using (FileStream fileStream = tempFile.OpenRead())
            {
                StreamReader streamReader = new StreamReader(fileStream);

                int count = 0;
                while (streamReader.ReadLine() != null)
                    count++;

                
                Assert.IsTrue(count == 7); // one per prototype call
            }

        }


        [TestMethod()]
        public void SJPTraceListenerEmptyProperties()
        {
            bool exceptionThrown = false;
            IEventPublisher[] customPublishers = new IEventPublisher[0];
            List<string> errors = new List<string>();

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(new MockPropertiesEmpty(), customPublishers, errors));
            }
            catch (SJPException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
                Assert.IsTrue(false);

            // display the errors for visual comfort factor
            foreach (string error in errors)
            {
                string message = "NUNIT:TestSJPTraceListener. " + error;
                Console.WriteLine(message);
            }

            Assert.IsTrue(errors.Count == 6);
        }

        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void SJPTraceListenerEmptyValueProperties()
        {
            IEventPublisher[] customPublishers = new IEventPublisher[0];
            List<string> errors = new List<string>();

            
            Trace.Listeners.Add(new SJPTraceListener(new MockPropertiesEmptyValues(), customPublishers, errors));
           
            Assert.IsTrue(errors.Count > 0);

        }

        [TestMethod()]
        public void SJPTraceListenerMinimumProperties()
        {
            IPropertyProvider MockPropertiesGoodMinimumProperties = new MockPropertiesGoodMinimumProperties();
            IEventPublisher[] customPublishers = new IEventPublisher[0];
            List<string> errors = new List<string>();

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(MockPropertiesGoodMinimumProperties,
                                                        customPublishers, errors));
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }

            Assert.IsTrue(errors.Count == 0);
        }

        [TestMethod()]
        public void SJPTraceListenerAllGood()
        {
            // NB. In proper use, an instance of Properties would be
            // retrieved from the Service Discovery (? or other means)
            // and then IPropertyProvider goodProperties = Properties.Current;

            IPropertyProvider goodProperties = new MockPropertiesGood();

            IEventPublisher[] customPublishers = new IEventPublisher[2];

            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");

            List<string> errors = new List<string>();

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
        }

        [TestMethod()]
        public void SJPTraceListenerPublisherNotPassed()
        {
            bool exceptionThrown = false;

            IPropertyProvider goodProperties = new MockPropertiesGood();
            List<string> errors = new List<string>();
            IEventPublisher[] customPublishers = new IEventPublisher[1];

            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            // do not pass a TDPublisher2 even though it is defined in goodProperties

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties, customPublishers, errors));
            }
            catch (SJPException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
                Assert.IsTrue(false);

            // display the errors for visual comfort factor
            foreach (string error in errors)
            {
                string message = "NUNIT:TestSJPTraceListener. " + error;
                Console.WriteLine(message);
            }

            Assert.IsTrue(errors.Count == 1);
        }

        [TestMethod()]
        public void SJPTraceListenerUnknownCustomPublisherClassPassed()
        {
            bool exceptionThrown = false;
            List<string> errors = new List<string>();
            IEventPublisher[] customPublishers = new IEventPublisher[3];

            IPropertyProvider goodProperties = new MockPropertiesGood();

            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");
            // pass in a custom publisher that does not appear in good properties
            customPublishers[2] = new SJPPublisher3("CustomPublisher3");

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties, customPublishers, errors));
            }
            catch (SJPException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
                Assert.IsTrue(false);

            Assert.IsTrue(errors.Count == 1);

            // display the errors for visual comfort factor
            foreach (string error in errors)
            {
                string message = "NUNIT:TestSJPTraceListener. " + error;
                Console.WriteLine(message);
            }

        }

        [TestMethod()]
        public void SJPTraceListenerUnknownCustomPublisherIDPassed()
        {
            bool exceptionThrown = false;
            List<string> errors = new List<string>();
            IEventPublisher[] customPublishers = new IEventPublisher[2];

            IPropertyProvider goodProperties = new MockPropertiesGood();

            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            // intialise with a different ID to that specified in good properties
            customPublishers[1] = new SJPPublisher2("DifferentIDToProperties");

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties, customPublishers, errors));
            }
            catch (SJPException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
                Assert.IsTrue(false);

            Assert.IsTrue(errors.Count == 1);

            // display the errors for visual comfort factor
            foreach (string error in errors)
            {
                string message = "NUNIT:TestSJPTraceListener. " + error;
                Console.WriteLine(message);
            }

        }


        [TestMethod()]
        public void SJPTraceListenerUnknownCustomPublisherClassKnownIDPassed()
        {
            bool exceptionThrown = false;
            List<string> errors = new List<string>();
            IEventPublisher[] customPublishers = new IEventPublisher[2];

            IPropertyProvider goodProperties = new MockPropertiesGood();

            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            // provide unknown class with an id that IS specified in good properties
            customPublishers[1] = new SJPPublisher3("CustomPublisher2");

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties, customPublishers, errors));
            }
            catch (SJPException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
                Assert.IsTrue(false);

            Assert.IsTrue(errors.Count == 1);

            // display the errors for visual comfort factor
            foreach (string error in errors)
            {
                string message = "NUNIT:TestSJPTraceListener. " + error;
                Console.WriteLine(message);
            }

        }

        [TestMethod()]
        public void SJPTraceListenerNoCustomPublishersPassed()
        {
            bool exceptionThrown = false;
            List<string> errors = new List<string>();
            IPropertyProvider goodProperties = new MockPropertiesGood();
            IEventPublisher[] customPublishers = new IEventPublisher[0];

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties, customPublishers, errors));
            }
            catch (SJPException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
                Assert.IsTrue(false);

            Assert.IsTrue(errors.Count == 2);

            // display the errors for visual comfort factor
            foreach (string error in errors)
            {
                string message = "NUNIT:TestSJPTraceListener. " + error;
                Console.WriteLine(message);
            }

        }

        [TestMethod()]
        public void SJPTraceListenerNullArrayCustomPublishersPassed()
        {
            bool exceptionThrown = false;
            List<string> errors = new List<string>();
            IPropertyProvider goodProperties = new MockPropertiesGood();

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties, null, errors));
            }
            catch (SJPException)
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
                Assert.IsTrue(false);

            Assert.IsTrue(errors.Count == 1);

            // display the errors for visual comfort factor
            foreach (string error in errors)
            {
                string message = "NUNIT:TestSJPTraceListener. " + error;
                Console.WriteLine(message);
            }

        }


        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void SJPTraceListenerBadCustomEvents()
        {
            List<string> errors = new List<string>();
            IPropertyProvider badCustomEvents = new MockPropertiesGoodPublishersBadEvents();
            IEventPublisher[] customPublishers = new IEventPublisher[2];
            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");

            
            Trace.Listeners.Add(new SJPTraceListener(badCustomEvents, customPublishers, errors));
            Assert.IsTrue(errors.Count>0);


            
        }


        [TestMethod()]
        public void SJPTraceListenerEventWriting()
        {
            MockPropertiesGood goodProperties = new MockPropertiesGood();

            Trace.Listeners.Clear();

            IEventPublisher[] customPublishers = new IEventPublisher[2];

            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");

            List<string> errors = new List<string>();

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

            OperationalEvent oe1 = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "Message", "11");
            oe1.AuditPublishersOff = false; // so we can perform test below
            Trace.Write(oe1);
            Assert.AreEqual(oe1.PublishedBy.Trim(),"SJP.Common.EventLogging.QueuePublisher SJP.Common.EventLogging.EventLogPublisher");

           
            // test operational event level switch works -  following event should not be published because level is Error
            OperationalEvent oe2 = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Warning, "Message", "11");
            oe2.AuditPublishersOff = false; // so we can perform test below
            Trace.Write(oe2);
            Assert.IsTrue(oe2.PublishedBy == "");
        }


        [TestMethod()]
        public void SJPTraceListenerParallelUsage()
        {
            MockPropertiesGood goodProperties = new MockPropertiesGood();

            IEventPublisher[] customPublishers = new IEventPublisher[2];

            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");

            List<string> errors = new List<string>();

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

            bool badCall = false;

            try
            {
                // call Write overload that is not implemented in SJPTraceListener
                string test = "TestSJPTraceListener.SJPTraceListenerParallelUsage. Test message - called using Write overload that is not implemented in SJPTraceListener, but is implemented in default listener.\n";
                Trace.Write(test);
            }
            catch (SJPException)
            {
                badCall = true;
            }

            Assert.IsTrue(!badCall);

            badCall = false;

            try
            {
                // call WriteLine - not implemented in SJPTraceListener
                string test = "TestSJPTraceListener.SJPTraceListenerParallelUsage. Test Message - called using WriteLine - this is not implemented in SJPTraceListener, but is implemented in default listener.";
                Trace.WriteLine(test);
            }
            catch (SJPException)
            {
                badCall = true;
            }

            Assert.IsTrue(!badCall);

            // ensure that SJPTraceListener does not effect any Debug calls
            try
            {
                string test = "TestSJPTraceListener.SJPTraceListenerParallelUsage. Test message (using Debug, with SJPTraceListener registered).";
                Debug.WriteLine(test);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }


            // remove SJPTraceListener - above calls should now work without exception being throw
            Trace.Listeners.Remove("SJPTraceListener");

            badCall = false;

            try
            {
                // call Write overload that is not implemented in SJPTraceListener
                string test = "TestSJPTraceListener.SJPTraceListenerParallelUsage. Test message (SJPTraceListener not registered).";
                Trace.Write(test);
            }
            catch (SJPException)
            {
                badCall = true;
            }

            Assert.IsTrue(!badCall);
        }

        /// <summary>
        /// Test handling when an unknown object is logged.
        /// </summary>
        [TestMethod()]
        public void SJPTraceListenerUnknownObject()
        {
            // set up SJPTraceListener with valid properties
            IPropertyProvider goodProperties = new MockPropertiesGood();
            IEventPublisher[] customPublishers = new IEventPublisher[2];
            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");
            List<string> errors = new List<string>();
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

            bool badEventFound = false;
            try
            {
                // try logging SJPEvent3 - this does not derive from CustomEvent
                Trace.Write(new SJPEvent3());
            }
            catch (SJPException)
            {
                badEventFound = true;
            }

            Assert.IsTrue(badEventFound);
        }

        /// <summary>
        /// Test handling when a custom event is logged, which is unknown to the SJPTraceListener
        /// </summary>
        [TestMethod()]
        public void SJPTraceListenerUnknownCustomEvent()
        {
            // set up SJPTraceListener with valid properties
            IPropertyProvider goodProperties = new MockPropertiesGood();
            IEventPublisher[] customPublishers = new IEventPublisher[2];
            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");
            List<string> errors = new List<string>();
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

            // attempt to log an event that the SJPTraceListener knows nothing about:
            bool badEventFound = false;
            try
            {
                // try logging SJPEvent4 - this derives from CustomEvent but is
                // not known by SJPTraceListener
                Trace.Write(new SJPEvent4());
            }
            catch (SJPException)
            {
                badEventFound = true;
            }

            Assert.IsTrue(badEventFound);
        }

       
    }
}
