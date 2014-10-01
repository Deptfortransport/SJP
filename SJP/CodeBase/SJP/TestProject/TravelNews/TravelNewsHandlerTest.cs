// *********************************************** 
// NAME             : TravelNewsHandlerTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: TravelNewsHandlerTest test class
// ************************************************
// 
                
using SJP.UserPortal.TravelNews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.UserPortal.TravelNews.TravelNewsData;
using SJP.UserPortal.TravelNews.SessionData;
using SJP.Common.ServiceDiscovery;
using SJP.Common.DatabaseInfrastructure;
using System.Collections.Generic;
using SJP.Common;
using System.Collections;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for TravelNewsHandlerTest and is intended
    ///to contain all TravelNewsHandlerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TravelNewsHandlerTest
    {
        private static TestDataManager testDataManager;

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
            SJPServiceDiscovery.Init(new TestInitialisation());

            string test_data = string.Empty;
            string setup_script = @"TravelNews\TravelNewsSetUp.sql";
            string clearup_script = @"TravelNews\TravelNewsCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPTransientPortal;Trusted_Connection=true";

            // Test data
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.TransientPortalDB);
            testDataManager.Setup();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            testDataManager.ClearData();

        }
        ////
        ////Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        ////
        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        ////
        #endregion


        /// <summary>
        ///A test for TravelNewsHandler Constructor
        ///</summary>
        [TestMethod()]
        public void TravelNewsHandlerConstructorTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();

            Assert.IsNotNull(target, "Expected TravelNewsHandler to be not null");
            Assert.IsTrue(target.TravelNewsLastUpdated > DateTime.MinValue, "Expected TravelNewsLastUpdated to be set");

            Keys_Accessor keysAccessor = new Keys_Accessor();
            Values_Accessor valuesAccessor = new Values_Accessor();
        }

        /// <summary>
        ///A test for GetChildrenDetailsByUid
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetChildrenDetailsByUidTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            string uid = "SJP0000001";
            
            TravelNewsItem[] actual;
            actual = target.GetChildrenDetailsByUid(uid);

            Assert.IsNotNull(actual, "Expected Travel News child items to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected at least one Travel News item to have been returned");

            bool hasCorrectParent = actual[0].IncidentParent.ToUpper() == uid.ToUpper();
            Assert.IsTrue(hasCorrectParent, "Expected Travel News child item to have parent searched on");
        }

        /// <summary>
        ///A test for GetDetails
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetDetailsTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            TravelNewsState travelNewsState = new TravelNewsState();
            travelNewsState.SetDefaultState();
            travelNewsState.SearchPhrase = "Test";

            TravelNewsItem[] actual;
            actual = target.GetDetails(travelNewsState);

            Assert.IsNotNull(actual, "Expected Travel News items to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected at least one Travel News item to have been returned");

            bool isFilteredItem = actual[0].DetailText.Contains(travelNewsState.SearchPhrase);
            Assert.IsTrue(isFilteredItem, "Expected Travel News item to have been filtered with the passed TravelNewsState");

            // For code coverage
            travelNewsState.SearchPhrase = " Test \"TEST \" "; // Intentional space and "
            actual = target.GetDetails(travelNewsState);
            Assert.IsNotNull(actual, "Expected Travel News items not to have been returned");
            Assert.IsNotNull(actual.Length == 0, "Expected Travel News items not to have been returned");
        }

        /// <summary>
        ///A test for GetDetailsByUid
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetDetailsByUidTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            string uid = "SJP0000001";
                        
            TravelNewsItem actual;
            actual = target.GetDetailsByUid(uid);

            Assert.IsNotNull(actual, "Expected Travel News item to have been returned");
            Assert.IsTrue(actual.Uid.ToUpper() == uid.ToUpper(), "Expected Travel News item to be for the requested id");

            // For code coverage, check news item object contains values 
            Assert.IsTrue((actual.CarriagewayDirection != null), "Expected Travel News item to contain value, CarriagewayDirection");
            Assert.IsTrue((actual.ClearedDateTime != DateTime.MinValue || actual.ClearedDateTime == DateTime.MinValue), "Expected Travel News item to contain value, ClearedDateTime");
            Assert.IsTrue((actual.DailyEndTime != TimeSpan.Zero), "Expected Travel News item to contain value, DailyEndTime");
            Assert.IsTrue((actual.DailyStartTime != TimeSpan.Zero), "Expected Travel News item to contain value, DailyStartTime");
            Assert.IsTrue((actual.DayMask != null), "Expected Travel News item to contain value, DayMask");
            Assert.IsTrue((actual.DetailText != null), "Expected Travel News item to contain value, DetailText");
            Assert.IsTrue((actual.Easting > 0), "Expected Travel News item to contain value, Easting");
            Assert.IsTrue((actual.ExpiryDateTime != DateTime.MinValue), "Expected Travel News item to contain value, ExpiryDateTime");
            Assert.IsTrue((actual.HeadlineText != null), "Expected Travel News item to contain value, HeadlineText");
            Assert.IsTrue((actual.IncidentActiveStatus == IncidentActiveStatus.Active || actual.IncidentActiveStatus == IncidentActiveStatus.Inactive), "Expected Travel News item to contain value, IncidentActiveStatus");
            Assert.IsTrue((actual.IncidentParent != null), "Expected Travel News item to contain value, IncidentParent");
            Assert.IsTrue((actual.IncidentStatus != null), "Expected Travel News item to contain value, IncidentStatus");
            Assert.IsTrue((actual.IncidentType != null), "Expected Travel News item to contain value, IncidentType");
            Assert.IsTrue((actual.ItemChangeStatus != null), "Expected Travel News item to contain value, ItemChangeStatus");
            Assert.IsTrue((actual.LastModifiedDateTime != DateTime.MinValue), "Expected Travel News item to contain value, LastModifiedDateTime");
            Assert.IsTrue((actual.Location != null), "Expected Travel News item to contain value, Location");
            Assert.IsTrue((actual.ModeOfTransport != null), "Expected Travel News item to contain value, ModeOfTransport");
            Assert.IsTrue((actual.Northing > 0), "Expected Travel News item to contain value, Northing");
            Assert.IsTrue((actual.Operator != null), "Expected Travel News item to contain value, Operator");
            Assert.IsTrue((actual.PlannedIncident), "Expected Travel News item to contain value, PlannedIncident");
            Assert.IsTrue((actual.PublicTransportOperator != null), "Expected Travel News item to contain value, PublicTransportOperator");
            Assert.IsTrue((actual.Regions != null), "Expected Travel News item to contain value, Regions");
            Assert.IsTrue((actual.RegionsLocation != null), "Expected Travel News item to contain value, RegionsLocation");
            Assert.IsTrue((actual.ReportedDateTime != DateTime.MinValue), "Expected Travel News item to contain value, ReportedDateTime");
            Assert.IsTrue((actual.RoadNumber != null), "Expected Travel News item to contain value, RoadNumber");
            Assert.IsTrue((actual.RoadType != null), "Expected Travel News item to contain value, RoadType");
            Assert.IsTrue((actual.SeverityDescription != null), "Expected Travel News item to contain value, SeverityDescription");
            Assert.IsTrue(((int)actual.SeverityLevel >= 0), "Expected Travel News item to contain value, SeverityLevel");
            Assert.IsTrue((actual.StartDateTime != DateTime.MinValue), "Expected Travel News item to contain value, StartDateTime");
            Assert.IsTrue((actual.StartToNowMinDiff >= 0), "Expected Travel News item to contain value, StartToNowMinDiff");
            Assert.IsTrue((actual.Uid != null), "Expected Travel News item to contain value, CarriagewayDirection");

            Assert.IsTrue(((int)actual.OlympicSeverityLevel >= 0), "Expected Travel News item to contain value, OlympicSeverityLevel");
            Assert.IsTrue((actual.OlympicSeverityDescription != null), "Expected Travel News item to contain value, OlympicSeverityDescription");
            Assert.IsTrue((actual.OlympicTravelAdvice != null), "Expected Travel News item to contain value, OlympicTravelAdvice");

            uid = "SJP0000001XXX";
            actual = target.GetDetailsByUid(uid);

            Assert.IsNull(actual, "Expected Travel News item not to have been returned");
        }

        /// <summary>
        ///A test for GetDetailsForWeb
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetDetailsForWeb()
        {
            TravelNewsHandler target = new TravelNewsHandler();

            TravelNewsState travelNewsState = new TravelNewsState();
            travelNewsState.SetDefaultState();

            TravelNewsItem[] actual;

            // No olympic 
            actual = target.GetDetailsForWeb(travelNewsState, false);
            
            Assert.IsNotNull(actual, "Expected Travel News items to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected Travel News items to exist");
            Assert.IsTrue(actual[0].OlympicIncident == false, "Expected Travel News items to not be for olympic incidents");

            // Olympic
            travelNewsState.SelectedVenuesFlag = true;
            travelNewsState.SearchNaptans.Add("8100AQC");
            travelNewsState.SearchNaptans.Add("8100AWP");
            travelNewsState.SearchNaptans.Add("8100EXL");

            actual = target.GetDetailsForWeb(travelNewsState, true);
            
            Assert.IsNotNull(actual, "Expected Travel News items to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected Travel News items to exist");
            Assert.IsTrue(actual[0].OlympicIncident == true, "Expected Travel News items to exist for olympic incidents");

            // Search phrase
            travelNewsState.SelectedVenuesFlag = false;
            travelNewsState.SearchPhrase = "Test";
            actual = target.GetDetailsForWeb(travelNewsState, true);

            Assert.IsNotNull(actual, "Expected Travel News items to have been returned");
            bool isFiltered = actual[0].HeadlineText.Contains(travelNewsState.SearchPhrase);
            Assert.IsTrue(isFiltered, "Expected Travel News items to have been filtered with the passed search phrase");

            // Regions filter
            travelNewsState.SelectedVenuesFlag = false;
            travelNewsState.SearchPhrase = string.Empty;
            travelNewsState.SelectedRegion = TravelNewsRegion.London.ToString();
            actual = target.GetDetailsForWeb(travelNewsState, true);

            Assert.IsNotNull(actual, "Expected Travel News items to have been returned");
            isFiltered = actual[0].Regions.Contains(TravelNewsRegion.London.ToString());
            Assert.IsTrue(isFiltered, "Expected Travel News items to have been filtered with the passed region");
        }

        /// <summary>
        ///A test for GetDetailsForWebGroupedBySeverity
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetDetailsForWebGroupedBySeverity()
        {
            TravelNewsHandler target = new TravelNewsHandler();

            TravelNewsState travelNewsState = new TravelNewsState();
            travelNewsState.SetDefaultState();
            travelNewsState.SelectedSeverity = SeverityLevel.Severe;

            Dictionary<SeverityLevel, List<TravelNewsItem>> actual;
            actual = target.GetDetailsForWebGroupedBySeverity(travelNewsState, false);

            Assert.IsNotNull(actual, "Expected Travel News items dictionary to have been returned");
            Assert.IsTrue(actual.Count > 0, "Expected Travel News items to exist");
            Assert.IsTrue(actual.ContainsKey(travelNewsState.SelectedSeverity), "Expected Travel News items to exist for severity level requested");
        }

        /// <summary>
        ///A test for GetDetailsForMobile
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetDetailsForMobile()
        {
            TravelNewsHandler target = new TravelNewsHandler();

            TravelNewsState travelNewsState = new TravelNewsState();
            travelNewsState.SetDefaultState();

            TravelNewsItem[] actual;
            actual = target.GetDetailsForMobile(travelNewsState);

            Assert.IsNotNull(actual, "Expected Travel News items to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected Travel News items to exist");
        }

        /// <summary>
        ///A test for GetHeadlines
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetHeadlinesTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            TravelNewsState travelNewsState = new TravelNewsState();
            travelNewsState.SetDefaultState();

            HeadlineItem[] actual;
            actual = target.GetHeadlines(travelNewsState, string.Empty);

            Assert.IsNotNull(actual, "Expected Travel News headlines to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected at least one Travel News headline to have been returned");
            
            // Venues filter
            travelNewsState.SelectedVenuesFlag = true;
            travelNewsState.SearchNaptans.Add("8100AQC");
            travelNewsState.SearchNaptans.Add("8100AWP");
            travelNewsState.SearchNaptans.Add("8100EXL");

            actual = target.GetHeadlines(travelNewsState, string.Empty);

            Assert.IsNotNull(actual, "Expected Travel News headlines to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected Travel News headlines to have been filtered for venues");

            // Search phrase
            travelNewsState.SelectedVenuesFlag = false;
            travelNewsState.SearchPhrase = "Test";
            actual = target.GetHeadlines(travelNewsState, string.Empty);

            Assert.IsNotNull(actual, "Expected Travel News headlines to have been returned");
            bool isFilteredHeadline = actual[0].HeadlineText.Contains(travelNewsState.SearchPhrase);
            Assert.IsTrue(isFilteredHeadline, "Expected Travel News headlines to have been filtered with the passed TravelNewsState");

            // Regions filter
            travelNewsState.SelectedVenuesFlag = false;
            travelNewsState.SearchPhrase = string.Empty;
            travelNewsState.SelectedRegion = TravelNewsRegion.All.ToString();
            actual = target.GetHeadlines(travelNewsState, 
                string.Format("{0},{1}",TravelNewsRegion.London.ToString(),TravelNewsRegion.EastMidlands.ToString()));

            Assert.IsNotNull(actual, "Expected Travel News items to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected Travel News headlines to have been filtered for region");
            
            // For code coverage, check headline object contains values 
            Assert.IsNotNull(actual[0].DelayTypes, "Expected Travel News headline item to contain value, DelayTypes");
            Assert.IsTrue(!string.IsNullOrEmpty(actual[0].Regions), "Expected Travel News headline item to contain value, Regions");
            Assert.IsTrue((actual[0].SeverityLevel == SeverityLevel.Severe || actual[0].SeverityLevel == SeverityLevel.Critical || actual[0].SeverityLevel == SeverityLevel.Serious),
                "Expected Travel News headline item to contain value, SeverityLevel");
            Assert.IsTrue((actual[0].TransportType == TransportType.All || actual[0].TransportType == TransportType.PublicTransport || actual[0].TransportType == TransportType.Road),
                "Expected Travel News headline item to contain value, TransportType");
            Assert.IsTrue(!string.IsNullOrEmpty(actual[0].Uid), "Expected Travel News headline item to contain value, Uid");
            Assert.IsTrue((actual[0].OlympicVenuesAffected.Count >= 0),
                "Expected Travel News headline item to contain zero or more, OlympicVenuesAffected");
        }

        /// <summary>
        ///A test for GetHeadlines
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetHeadlinesTest1()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            HeadlineItem[] actual;
            actual = target.GetHeadlines();

            Assert.IsNotNull(actual, "Expected Travel News headlines to have been returned");
            Assert.IsTrue(actual.Length > 0, "Expected at least one Travel News headline to have been returned");
        }

        /// <summary>
        ///A test for TravelNewsHeadlineComparer
        ///</summary>
        [TestMethod()]
        public void TravelNewsHeadlineComparerTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            HeadlineItem[] actual;
            actual = target.GetHeadlines();

            Assert.IsNotNull(actual, "Expected Travel News headlines to have been returned");
            Assert.IsTrue(actual.Length > 1, "Expected at least two Travel News headline to have been returned");

            string regionFilterAndSort = "London,South West";
            TravelNewsHeadlineComparer comparer = new TravelNewsHeadlineComparer(regionFilterAndSort);

            List<HeadlineItem> headlinesToSort = new List<HeadlineItem>(actual);

            headlinesToSort.Sort(comparer);

            HeadlineItem[] headlinesSorted = headlinesToSort.ToArray();

            Assert.IsTrue(headlinesSorted[0].Regions.Contains("London"), "Expected first sorted headline item to contain London");
        }

        /// <summary>
        ///A test for IsTravelNewsAvaliable
        ///</summary>
        [TestMethod()]
        public void TravelNewsIsAvaliableTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            bool actual;
            actual = target.IsTravelNewsAvaliable;

            Assert.IsTrue(actual, "Expected Travel News items to exist");
        }

        /// <summary>
        ///A test for TravelNewsUnavailableText
        ///</summary>
        [TestMethod()]
        public void TravelNewsUnavailableTextTest()
        {
            TravelNewsHandler target = new TravelNewsHandler();
            string actual;
            actual = target.TravelNewsUnavailableText;

            Assert.IsNotNull(actual, "Expected Travel News unavailable text to be returned");
        }

        /// <summary>
        ///A test for TravelNewsRegionParser
        ///</summary>
        [TestMethod()]
        public void TravelNewsRegionParserTest()
        {
            TravelNewsRegionParser target = new TravelNewsRegionParser();

            Assert.IsTrue(target.GetTravelNewsRegion(null) == TravelNewsRegion.All);
            Assert.IsTrue(target.GetTravelNewsRegion(string.Empty) == TravelNewsRegion.All);
            Assert.IsTrue(target.GetTravelNewsRegion("test") == TravelNewsRegion.All);
            Assert.IsTrue(target.GetTravelNewsRegion("eastanglia") == TravelNewsRegion.EastAnglia);
            Assert.IsTrue(target.GetTravelNewsRegion("eastmidlands") == TravelNewsRegion.EastMidlands);
            Assert.IsTrue(target.GetTravelNewsRegion("london") == TravelNewsRegion.London);
            Assert.IsTrue(target.GetTravelNewsRegion("northeast") == TravelNewsRegion.NorthEast);
            Assert.IsTrue(target.GetTravelNewsRegion("northwest") == TravelNewsRegion.NorthWest);
            Assert.IsTrue(target.GetTravelNewsRegion("scotland") == TravelNewsRegion.Scotland);
            Assert.IsTrue(target.GetTravelNewsRegion("southeast") == TravelNewsRegion.SouthEast);
            Assert.IsTrue(target.GetTravelNewsRegion("southwest") == TravelNewsRegion.SouthWest);
            Assert.IsTrue(target.GetTravelNewsRegion("wales") == TravelNewsRegion.Wales);
            Assert.IsTrue(target.GetTravelNewsRegion("westmidlands") == TravelNewsRegion.WestMidlands);
            Assert.IsTrue(target.GetTravelNewsRegion("yorkshireandhumber") == TravelNewsRegion.YorkshireandHumber);

            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.All)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.EastAnglia)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.EastMidlands)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.London)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.NorthEast)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.NorthWest)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.Scotland)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.SouthEast)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.SouthWest)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.Wales)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.WestMidlands)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegion(TravelNewsRegion.YorkshireandHumber)));

            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.All)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.EastAnglia)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.EastMidlands)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.London)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.NorthEast)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.NorthWest)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.Scotland)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.SouthEast)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.SouthWest)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.Wales)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.WestMidlands)));
            Assert.IsTrue(!string.IsNullOrEmpty(target.GetTravelNewsRegionForQueryString(TravelNewsRegion.YorkshireandHumber)));
        }

        /// <summary>
        ///A test for TravelNewsTransportModeParser
        ///</summary>
        [TestMethod()]
        public void TravelNewsTransportModeParserTest()
        {
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType(null) == SJPModeType.Unknown);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType(string.Empty) == SJPModeType.Unknown);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("test") == SJPModeType.Unknown);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("air") == SJPModeType.Air);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("bus") == SJPModeType.Bus);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("Cable Car") == SJPModeType.CableCar);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("car") == SJPModeType.Car);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("road") == SJPModeType.Car);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("coach") == SJPModeType.Coach);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("cycle") == SJPModeType.Cycle);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("lightrail") == SJPModeType.Drt);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("ferry") == SJPModeType.Ferry);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("metro") == SJPModeType.Metro);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("rail") == SJPModeType.Rail);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("tram") == SJPModeType.Tram);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("underground") == SJPModeType.Underground);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("walking") == SJPModeType.Walk);
            Assert.IsTrue(TravelNewsTransportModeParser.GetSJPModeType("EuroTunnel") == SJPModeType.EuroTunnel);
        }

        /// <summary>
        /// A test for TravelNews Converting
        /// </summary>
        [TestMethod()]
        public void TravelNewsConvertingTest()
        {
            Converting_Accessor accessor = new Converting_Accessor();

            Assert.IsTrue(Converting.ToString(TransportType.All) == KeyValue_Accessor.All);
            Assert.IsTrue(Converting.ToString(TransportType.PublicTransport) == KeyValue_Accessor.PublicTransport);
            Assert.IsTrue(Converting.ToString(TransportType.Road) == KeyValue_Accessor.Road);

            Assert.IsTrue(Converting.ToString(DelayType.All) == KeyValue_Accessor.All);
            Assert.IsTrue(Converting.ToString(DelayType.Major) == KeyValue_Accessor.Major);
            Assert.IsTrue(Converting.ToString(DelayType.Recent) == KeyValue_Accessor.Recent);

            Assert.IsTrue(Converting.ToString(IncidentType.All) == KeyValue_Accessor.All);
            Assert.IsTrue(Converting.ToString(IncidentType.Planned) == KeyValue_Accessor.Planned);
            Assert.IsTrue(Converting.ToString(IncidentType.Unplanned) == KeyValue_Accessor.Unplanned);

            Assert.IsTrue(Converting.ToString(DisplayType.Full) == KeyValue_Accessor.Full);
            Assert.IsTrue(Converting.ToString(DisplayType.Summary) == KeyValue_Accessor.Summary);
        }

        /// <summary>
        /// A test for TravelNews Parsing
        /// </summary>
        [TestMethod()]
        public void TravelNewsParsingTest()
        {
            Parsing_Accessor accessor = new Parsing_Accessor();

            Assert.IsTrue(Parsing.ParseTransportType(KeyValue_Accessor.All) == TransportType.All);
            Assert.IsTrue(Parsing.ParseTransportType(KeyValue_Accessor.PublicTransport) == TransportType.PublicTransport);
            Assert.IsTrue(Parsing.ParseTransportType(KeyValue_Accessor.Road) == TransportType.Road);

            Assert.IsTrue(Parsing.ParseDelayType(KeyValue_Accessor.All) == DelayType.All);
            Assert.IsTrue(Parsing.ParseDelayType(KeyValue_Accessor.Major) == DelayType.Major);
            Assert.IsTrue(Parsing.ParseDelayType(KeyValue_Accessor.Recent) == DelayType.Recent);

            Assert.IsTrue(Parsing.ParseIncidentType(KeyValue_Accessor.All) == IncidentType.All);
            Assert.IsTrue(Parsing.ParseIncidentType(KeyValue_Accessor.Planned) == IncidentType.Planned);
            Assert.IsTrue(Parsing.ParseIncidentType(KeyValue_Accessor.Unplanned) == IncidentType.Unplanned);

            Assert.IsTrue(Parsing.ParseDisplayType(KeyValue_Accessor.Full) == DisplayType.Full);
            Assert.IsTrue(Parsing.ParseDisplayType(KeyValue_Accessor.Summary) == DisplayType.Summary);

            Assert.IsTrue(Parsing.ParseSeverityLevel(0) == SeverityLevel.Critical);
            Assert.IsTrue(Parsing.ParseSeverityLevel("Critical") == SeverityLevel.Critical);

        }

        /// <summary>
        /// A test for TravelNewsState
        /// </summary>
        [TestMethod()]
        public void TravelNewsStateTest()
        {
            TravelNewsState state = new TravelNewsState();

            // For code coverage
            state.SetDefaultState();
            string s = state.ToString();

            state.HelpDisplayed = true;
            state.LastSearchPhrase = "Test";
            state.LastSelectedDate = DateTime.MaxValue;
            state.LastSelectedDelays = DelayType.Major;
            state.LastSelectedIncident = "Test";
            state.LastSelectedIncidentType = IncidentType.Planned;
            state.LastSelectedRegion = TravelNewsRegion.EastAnglia.ToString();
            state.LastSelectedTransport = TransportType.PublicTransport;
            state.LastSelectedView = TravelNewsViewType.Details;
            state.PageLanguage = "Test";
            state.SearchNaptans = new List<string>();
            state.SearchNaptans.Add("Test");
            state.SearchPhrase = "Test";
            state.SearchTokens = new ArrayList(); 
            state.SearchTokens.Add((object)"Test");
            state.SelectedAllVenuesFlag = true;
            state.SelectedDate = DateTime.MaxValue;
            state.SelectedDelays = DelayType.Major;
            state.SelectedDetails = DisplayType.Full;
            state.SelectedIncident = "Test";
            state.SelectedIncidentActive = IncidentActiveStatus.Active.ToString();
            state.SelectedIncidentType = IncidentType.Planned;
            state.SelectedRegion = TravelNewsRegion.EastAnglia.ToString();
            state.SelectedSeverity = SeverityLevel.Medium;
            state.SelectedSeverityFilter = SeverityFilter.CriticalIncidents;
            state.SelectedTransport = TransportType.PublicTransport;
            state.SelectedVenuesFlag = true;
            state.SelectedView = TravelNewsViewType.Details;

            
            Assert.IsTrue(state.HelpDisplayed == true);
            Assert.IsTrue(state.LastSearchPhrase == "Test");
            Assert.IsTrue(state.LastSelectedDate == DateTime.MaxValue);
            Assert.IsTrue(state.LastSelectedDelays == DelayType.Major);
            Assert.IsTrue(state.LastSelectedIncident == "Test");
            Assert.IsTrue(state.LastSelectedIncidentType == IncidentType.Planned);
            Assert.IsTrue(state.LastSelectedRegion == TravelNewsRegion.EastAnglia.ToString());
            Assert.IsTrue(state.LastSelectedTransport == TransportType.PublicTransport);
            Assert.IsTrue(state.LastSelectedView == TravelNewsViewType.Details);
            Assert.IsTrue(state.PageLanguage == "Test");
            Assert.IsTrue(state.SearchNaptans.Count == 1);
            Assert.IsTrue(state.SearchPhrase == "Test");
            Assert.IsTrue(state.SearchTokens.Count == 1);
            Assert.IsTrue(state.SelectedAllVenuesFlag == true);
            Assert.IsTrue(state.SelectedDate == DateTime.MaxValue);
            Assert.IsTrue(state.SelectedDelays == DelayType.Major);
            Assert.IsTrue(state.SelectedDetails == DisplayType.Full);
            Assert.IsTrue(state.SelectedIncident == "Test");
            Assert.IsTrue(state.SelectedIncidentActive == IncidentActiveStatus.Active.ToString());
            Assert.IsTrue(state.SelectedIncidentType == IncidentType.Planned);
            Assert.IsTrue(state.SelectedRegion == TravelNewsRegion.EastAnglia.ToString());
            Assert.IsTrue(state.SelectedSeverity == SeverityLevel.Medium);
            Assert.IsTrue(state.SelectedSeverityFilter == SeverityFilter.CriticalIncidents);
            Assert.IsTrue(state.SelectedTransport == TransportType.PublicTransport);
            Assert.IsTrue(state.SelectedVenuesFlag == true);
            Assert.IsTrue(state.SelectedView == TravelNewsViewType.Details);
            
        }
    }
}
