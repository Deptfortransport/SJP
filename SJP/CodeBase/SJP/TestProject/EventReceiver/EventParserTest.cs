using SJP.Reporting.EventReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TransportDirect.Common.Logging;
using SCEL = SJP.Common.EventLogging;
using SRE = SJP.Reporting.Events;
using TDPEL = TransportDirect.Common.Logging;
using TDPCJPE = TransportDirect.ReportDataProvider.CJPCustomEvents;

namespace SJP.TestProject.EventReceiver
{
    
    
    /// <summary>
    ///This is a test class for EventParserTest and is intended
    ///to contain all EventParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EventParserTest
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
        ///A test for ParseTDPCJPLogEvent
        ///</summary>
        [TestMethod()]
        public void ParseTDPCJPLogEventTest()
        {
            // Parsing JourneyWebRequestEvent
            TDPCJPE.JourneyWebRequestEvent jwre = new TDPCJPE.JourneyWebRequestEvent("test", "a15", DateTime.Now, TDPCJPE.JourneyWebRequestType.Journey, "em", true, false);

            SCEL.LogEvent actual;
            actual = EventParser.ParseTDPCJPLogEvent(jwre);

            Assert.IsInstanceOfType(actual, typeof(SRE.JourneyWebRequestEvent));

            // Parsing LocationRequestEvent
            TDPCJPE.LocationRequestEvent lre = new TDPCJPE.LocationRequestEvent("test", TDPCJPE.JourneyPrepositionCategory.From, "082", "632");

            actual = EventParser.ParseTDPCJPLogEvent(lre);

            Assert.IsInstanceOfType(actual, typeof(SRE.LocationRequestEvent));

            // Parsing InternalRequestEvent
            TDPCJPE.InternalRequestEvent ire = new TDPCJPE.InternalRequestEvent("test", "12", DateTime.Now, TDPCJPE.InternalRequestType.RailTTBO, "ft", true, false);

            actual = EventParser.ParseTDPCJPLogEvent(ire);

            Assert.IsInstanceOfType(actual, typeof(SRE.InternalRequestEvent));

            // Parsing OperationalEvent
            TDPEL.OperationalEvent oe = new TDPEL.OperationalEvent(TDEventCategory.Database, TDTraceLevel.Error, "Test database is not available");

            actual = EventParser.ParseTDPCJPLogEvent(oe);

            Assert.IsInstanceOfType(actual, typeof(SCEL.OperationalEvent));

            // Parsing OperationalEvent
            TDPEL.CustomEmailEvent cee = new TDPEL.CustomEmailEvent("Destination", "Body", "CJP is not available");

            actual = EventParser.ParseTDPCJPLogEvent(cee);

            Assert.IsNull(actual, "Expected null to be returned");
        }
    }
}
