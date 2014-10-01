using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using SJP.Common;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for ConsolePublisherTest and is intended
    ///to contain all ConsolePublisherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConsolePublisherTest
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
        ///A test for WriteEvent
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTest()
        {
            // ---------------------------------------------

            // create test data - 16 operational events

            int numberOfEvents = 16;

            OperationalEvent[] operationalEvents =
                new OperationalEvent[numberOfEvents];

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

            // for this event add a complex data type as the target parameter
            ArrayList target = new ArrayList();
            target.Add("My target string.");
            operationalEvents[15] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Info,
                "Test Error 16: D123 failed",
                target
                );

            // ---------------------------------------------

            // create two console publishers, one where the
            // the streamSetting is set to "Out", and one
            // where the stream setting is set to "Error"

            ConsolePublisher outPublisher = new ConsolePublisher("id", "Out");
            ConsolePublisher errorPublisher = new ConsolePublisher("id2", "Error");

            // write 8 of the operational events to the Out and 8 to Error

            for (int i = 0; i < 8; i++)
                outPublisher.WriteEvent(operationalEvents[i]);

            for (int i = 8; i < 16; i++)
                errorPublisher.WriteEvent(operationalEvents[i]);

            //-----------------------------------------------

            // create custom events

            CustomEventOne customEventOne = new CustomEventOne
                (SJPEventCategory.Business, SJPTraceLevel.Warning,
                "A custom event one message", Environment.UserName, 12345);

            CustomEventTwo customEventTwo = new CustomEventTwo
                (SJPEventCategory.ThirdParty, SJPTraceLevel.Error,
                "A custom event two message", Environment.UserName, 3343);

            // CustomEventOne has a console formatter defined, therefore
            // log event should be written in the format specified
            // in CustomEventOneConsoleFormatter
            outPublisher.WriteEvent(customEventOne);

            // CustomEventTwo has no consolel formatter defined, therefore
            // log event should be written in the format specified
            // by the DefaultFormatter.
            outPublisher.WriteEvent(customEventTwo);

            // Testing for null reference exception by passing null
            outPublisher.WriteEvent(null);
        }

        /// <summary>
        ///A test for Identifier
        ///</summary>
        [TestMethod()]
        public void IdentifierTest()
        {
            string identifier = "abc"; 
            string streamSetting = "def"; 
            ConsolePublisher target = new ConsolePublisher(identifier, streamSetting); 
            string actual;
            actual = target.Identifier;

            Assert.AreEqual(identifier, actual);
        }
    }
}
