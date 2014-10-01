// *********************************************** 
// NAME             : FileNamePlaceholder      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: DiscriptionPlaceholder
// ************************************************
// 
                
using SJP.Reporting.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.EventLogging;
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ReceivedOperationalEventTest and is intended
    ///to contain all ReceivedOperationalEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReceivedOperationalEventTest
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
        ///A test for ReceivedOperationalEvent Constructor
        ///</summary>
        [TestMethod()]
        public void ReceivedOperationalEventConstructorTest()
        {
            OperationalEvent operationalEvent = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "message");
            ReceivedOperationalEvent target = new ReceivedOperationalEvent(operationalEvent);
        }

        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void ReceivedOperationalEventFileFormatterTest()
        {
            OperationalEvent operationalEvent = new OperationalEvent(
                SJPEventCategory.Business, SJPTraceLevel.Error, "message", 
                new SJPException("ExceptionMessage", false, SJPExceptionIdentifier.RDPUnsupportedSJPOperationalEventPublisherEvent), 
                "TestSession");
            ReceivedOperationalEvent target = new ReceivedOperationalEvent(operationalEvent);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }

        /// <summary>
        ///A test for WrappedOperationalEvent
        ///</summary>
        [TestMethod()]
        public void ReceivedOperationalEventWrappedOperationalEventTest()
        {
            OperationalEvent operationalEvent = new OperationalEvent(SJPEventCategory.Business, SJPTraceLevel.Error, "message");
            ReceivedOperationalEvent target = new ReceivedOperationalEvent(operationalEvent);
            OperationalEvent actual;
            actual = target.WrappedOperationalEvent;

            Assert.IsNotNull(actual);
        }
    }
}
