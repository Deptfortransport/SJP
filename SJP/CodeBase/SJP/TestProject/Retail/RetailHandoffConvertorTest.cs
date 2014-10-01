// *********************************************** 
// NAME             : RetailHandoffConvertorTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 09 Apr 2011
// DESCRIPTION  	: RetailHandoffConvertorTest test class
// ************************************************
// 
                
using SJP.UserPortal.Retail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml;
using SJP.UserPortal.JourneyControl;
using SJP.Common.LocationService;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Xml.Schema;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Common.Extenders;
using JC = SJP.UserPortal.JourneyControl;
using System.Runtime.Remoting;
using System.IO;
using SJP.Common.EventLogging;
using Logger = System.Diagnostics.Trace;
using SJP.Common;

namespace SJP.TestProject
{
    /// <summary>
    ///This is a test class for RetailHandoffConvertorTest and is intended
    ///to contain all RetailHandoffConvertorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RetailHandoffConvertorTest
    {
        private TestContext testContextInstance;

        private static object journeyLock = new object();
        private static Journey journeyOutward = null;
        private static Journey journeyReturn = null;

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

            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.TravelcardCatalogue, new TravelcardCatalogueFactory());

            try
            {
                //Configure the hosted remoting objects to be a remote object
                string configPath = AppDomain.CurrentDomain.BaseDirectory + @"\Remoting.config";
                if (File.Exists(configPath))
                {
                    RemotingConfiguration.Configure(configPath, false);
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Verbose, "Loaded remoting configuration from " + configPath));
                }
                else
                    Logger.Write(new OperationalEvent(SJPEventCategory.Infrastructure, SJPTraceLevel.Error, "Could not find remoting configuration file: " + configPath));
            }
            catch
            {
                // Ignore as it remoting may have been setup in another test
            }
        }
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
        ///A test for RetailHandoffConvertor Constructor
        ///</summary>
        [TestMethod()]
        public void RetailHandoffConvertorConstructorTest()
        {
            RetailHandoffConvertor target = new RetailHandoffConvertor();

            Assert.IsNotNull(target, "Expected RetailHandoffConvertor object to be created");
        }

        /// <summary>
        ///A test for AddReference
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.retail.dll")]
        public void AddReferenceTest()
        {
            XmlDocument doc = new XmlDocument();
            string referenceNumber = "AddReferenceTest";
            
            XmlElement actual = RetailHandoffConvertor_Accessor.AddReference(doc, referenceNumber);

            bool hasCorrectName = (actual.Name == RetailHandoffConvertor_Accessor.ELE_Reference);
            bool hasCorrectInnerText = (actual.InnerText == referenceNumber);

            Assert.IsNotNull(actual, "Expected XmlElement to be returned by method");
            Assert.IsTrue(hasCorrectName, string.Format("Expected XmlElement Name to be {0}", RetailHandoffConvertor_Accessor.ELE_Reference));
            Assert.IsTrue(hasCorrectInnerText, "Expected XmlElement InnerText to be supplied value");
        }

        /// <summary>
        ///A test for AddDateTimeGenerated
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.retail.dll")]
        public void AddDateTimeGeneratedTest()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement actual = RetailHandoffConvertor_Accessor.AddDateTimeGenerated(doc);

            bool hasCorrectName = (actual.Name == RetailHandoffConvertor_Accessor.ELE_Generated);
            bool hasCorrectInnerText = (actual.InnerText.StartsWith(DateTime.Now.Year.ToString())); // Cursory check theres a datetime

            Assert.IsNotNull(actual, "Expected XmlElement to be returned by method");
            Assert.IsTrue(hasCorrectName, string.Format("Expected XmlElement Name to be {0}", RetailHandoffConvertor_Accessor.ELE_Generated));
            Assert.IsTrue(hasCorrectInnerText, "Expected XmlElement InnerText to be a datetime value");
        }
                
        /// <summary>
        ///A test for AddJourneyLeg
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.retail.dll")]
        public void AddJourneyLegTest()
        {
            // Get some journeys to work with
            PopulateJourneys();

            if ((journeyOutward != null) && (journeyOutward.JourneyLegs.Count > 0))
            {
                // Prevent thread conflicts with other tests which use PopulateJourneys
                lock (journeyLock)
                {
                    XmlDocument doc = new XmlDocument();
                    JourneyLeg leg = journeyOutward.JourneyLegs[0];
                    int legCount = 0;

                    SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.TravelcardCatalogue, new TravelcardCatalogueFactory());

                    XmlElement actual = RetailHandoffConvertor_Accessor.AddJourneyLeg(doc, leg, legCount, false, false,
                        string.Empty, string.Empty);

                    bool hasCorrectName = (actual.Name == RetailHandoffConvertor_Accessor.ELE_JourneyLeg);
                    bool hasCorrectChild = (actual.FirstChild.Name == RetailHandoffConvertor_Accessor.ELE_LegId); // Cursory check there is a child leg id element
                    bool hasCorrectInnerText = (actual.FirstChild.InnerText == legCount.ToString()); // Cursory check the child leg id has correct value

                    Assert.IsNotNull(actual, "Expected XmlElement to be returned by method");
                    Assert.IsTrue(hasCorrectName, string.Format("Expected XmlElement Name to be {0}", RetailHandoffConvertor_Accessor.ELE_JourneyLeg));
                    Assert.IsTrue(hasCorrectChild, string.Format("Expected XmlElement to have First Child XmlElement Name with {0}", RetailHandoffConvertor_Accessor.ELE_LegId));
                    Assert.IsTrue(hasCorrectInnerText, "Expected XmlElement to have First Child XmlElement value to be supplied value");
                }
            }
            else
            {
                Assert.Fail("Journeys were not created, unable to perform test");
            }
        }

        /// <summary>
        ///A test for AddJourneys
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.retail.dll")]
        public void AddJourneysTest()
        {
            // Get some journeys to work with
            PopulateJourneys();

            if ((journeyOutward != null) && (journeyReturn != null))
            {
                // Prevent thread conflicts with other tests which use PopulateJourneys
                lock (journeyLock)
                {
                    XmlDocument doc = new XmlDocument();

                    // Make accessible for code coverage
                    journeyOutward.AccessibleJourney = true;
                    journeyReturn.AccessibleJourney = true;

                    SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.TravelcardCatalogue, new TravelcardCatalogueFactory());

                    XmlElement actual = RetailHandoffConvertor_Accessor.AddJourneys(doc, journeyOutward, journeyReturn,
                        string.Empty, string.Empty, string.Empty, string.Empty);

                    bool hasCorrectName = (actual.Name == RetailHandoffConvertor_Accessor.ELE_Journeys);
                    bool hasCorrectChild = (actual.FirstChild.Name == RetailHandoffConvertor_Accessor.ELE_Accesible); // Cursory check there is a child element

                    Assert.IsNotNull(actual, "Expected XmlElement to be returned by method");
                    Assert.IsTrue(hasCorrectName, string.Format("Expected XmlElement Name to be {0}", RetailHandoffConvertor_Accessor.ELE_Accesible));
                    Assert.IsTrue(hasCorrectChild, string.Format("Expected XmlElement to have First Child XmlElement Name with {0}", RetailHandoffConvertor_Accessor.ELE_LegId));

                    // For code coverage
                    journeyOutward.AccessibleJourney = false;
                    journeyReturn.AccessibleJourney = false;

                    // Outward (not accessible), Null
                    actual = RetailHandoffConvertor_Accessor.AddJourneys(doc, journeyOutward, null,
                        string.Empty, string.Empty, string.Empty, string.Empty);
                    Assert.IsNotNull(actual, "Expected XmlElement to be returned by method");

                    // Null, Return (not accessible)
                    actual = RetailHandoffConvertor_Accessor.AddJourneys(doc, null, journeyReturn,
                        string.Empty, string.Empty, string.Empty, string.Empty);
                    Assert.IsNotNull(actual, "Expected XmlElement to be returned by method");

                    // Null, Return (accessible)
                    journeyReturn.AccessibleJourney = true;
                    actual = RetailHandoffConvertor_Accessor.AddJourneys(doc, null, journeyReturn,
                        string.Empty, string.Empty, string.Empty, string.Empty);
                    Assert.IsNotNull(actual, "Expected XmlElement to be returned by method");
                }
            }
            else
            {
                Assert.Fail("Journeys were not created, unable to perform test");
            }
        }

        /// <summary>
        ///A test for ParseNaPTAN
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.retail.dll")]
        public void ParseNaPTANTest()
        {
            List<string> naptans = null;

            string actual = null;

            actual = RetailHandoffConvertor_Accessor.ParseNaPTAN(naptans);
            Assert.IsTrue(
                ((actual == string.Empty) || (actual.Contains("XXX"))), 
                "Expected ParseNaPTAN to return string.empty or dummy naptan");

            naptans = new List<string>();
            actual = RetailHandoffConvertor_Accessor.ParseNaPTAN(naptans);
            Assert.IsTrue(((actual == string.Empty) || (actual.Contains("XXX"))),
                "Expected ParseNaPTAN to return string.empty or dummy naptan");

            naptans.Add(string.Empty);
            actual = RetailHandoffConvertor_Accessor.ParseNaPTAN(naptans);
            Assert.IsTrue(((actual == string.Empty) || (actual.Contains("XXX"))),
                "Expected ParseNaPTAN to return string.empty or dummy naptan");

            naptans.Clear();
            naptans.Add(RetailHandoffConvertor_Accessor.ORIGIN_NAPTAN);
            actual = RetailHandoffConvertor_Accessor.ParseNaPTAN(naptans);
            Assert.IsTrue(((actual == string.Empty) || (actual.Contains("XXX"))),
                "Expected ParseNaPTAN to return string.empty or dummy naptan");

            naptans.Clear();
            naptans.Add(RetailHandoffConvertor_Accessor.DESTINATION_NAPTAN);
            actual = RetailHandoffConvertor_Accessor.ParseNaPTAN(naptans);
            Assert.IsTrue(((actual == string.Empty) || (actual.Contains("XXX"))),
                "Expected ParseNaPTAN to return string.empty or dummy naptan");

            naptans.Clear();
            naptans.Add("9100NTNG");
            actual = RetailHandoffConvertor_Accessor.ParseNaPTAN(naptans);
            Assert.IsTrue(actual == "9100NTNG", "Expected ParseNaPTAN to return 9100NTNG");
        }

        /// <summary>
        ///A test for ValidationHandler
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.retail.dll")]
        public void ValidationHandlerTest()
        {
            // Get the schema to validate against
            RetailHandoffSchema handoffSchema = SJPServiceDiscovery.Current.Get<RetailHandoffSchema>(ServiceDiscoveryKey.RetailerHandoffSchema);

            // Attach to the object ValidationHandler method (to test)
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.Schemas.Add(handoffSchema.Schema);
            //settings.ConformanceLevel = ConformanceLevel.Document;
            settings.ValidationEventHandler += new ValidationEventHandler(RetailHandoffConvertor_Accessor.ValidationHandler);
            
            try
            {
                // Perform validation test
                string xml = "<test>error</test>";

                using (StringReader stringReader = new StringReader(xml))
                {
                    XmlReader reader = XmlReader.Create(stringReader, settings);

                    // Load and Validate the xml
                    XmlDocument document = new XmlDocument();
                    document.Load(reader);
                    document.Validate(RetailHandoffConvertor_Accessor.ValidationHandler);
                }

                Assert.Fail("Expected ValidationHandler to throw exception");
            }
            catch (SJPException)
            {
                // Expect expception to be thrown
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Expected SJPException to be thrown, error: {0}", ex.Message));
            }
        }

        /// <summary>
        ///A test for GenerateHandoffXml
        ///</summary>
        [TestMethod()]
        public void GenerateHandoffXmlTest()
        {
            // Get some journeys to work with
            PopulateJourneys();

            if ((journeyOutward != null) && (journeyReturn != null))
            {
                // Prevent thread conflicts with other tests which use PopulateJourneys
                lock (journeyLock)
                {
                    string sessionId = "GenerateHandoffXmlTest";

                    SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.TravelcardCatalogue, new TravelcardCatalogueFactory());

                    IXPathNavigable actual = RetailHandoffConvertor.GenerateHandoffXml(sessionId, journeyOutward, journeyReturn, "9999XXXXXX", "9999XXXXXX", "9999XXXXXX", "9999XXXXXX");

                    Assert.IsNotNull(actual, "Expected IXPathNavigable to be returned by method");
                }
            }
            else
            {
                Assert.Fail("Journeys were not created, unable to perform test");
            }
        }
        

        #region Private methods

        /// <summary>
        /// Method to populate journeys using the CJPManager
        /// </summary>
        private void PopulateJourneys()
        {
            if (journeyOutward == null || journeyReturn == null)
            {
                lock (journeyLock)
                {
                    if (journeyOutward == null || journeyReturn == null)
                    {
                        CJPManager manager = new CJPManager();

                        // Initialise a journey request
                        ISJPJourneyRequest request = InitialiseJourneyRequest();

                        string sessionId = "Test";
                        bool referenceTransaction = false;
                        string language = "en";

                        // Perform test
                        ISJPJourneyResult result = manager.CallCJP(request, sessionId, referenceTransaction, language);

                        // Check for journeys
                        if ((result.OutwardJourneys.Count > 0) && (result.ReturnJourneys.Count > 0))
                        {
                            journeyOutward = result.OutwardJourneys[0];
                            journeyReturn = result.ReturnJourneys[0];
                        }
                        else
                        {
                            // In case of journeys failing, e.g. could be CJP slow to start up, try again
                            request.JourneyRequestHash = "TestAgain";
                            result = manager.CallCJP(request, sessionId, referenceTransaction, language);

                            if ((result.OutwardJourneys.Count > 0) && (result.ReturnJourneys.Count > 0))
                            {
                                journeyOutward = result.OutwardJourneys[0];
                                journeyReturn = result.ReturnJourneys[0];
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initialises a journey request with standard request values
        /// for an outward and return journey
        /// </summary>
        /// <returns></returns>
        private ISJPJourneyRequest InitialiseJourneyRequest()
        {
            IPropertyProvider pp = Properties.Current;

            ISJPJourneyRequest request = new SJPJourneyRequest();

            request.JourneyRequestHash = "Test";

            LocationService locationService = SJPServiceDiscovery.Current.Get<LocationService>(ServiceDiscoveryKey.LocationService);

            request.Origin = locationService.GetSJPLocation("9100CAMBDGE", SJPLocationType.Station);
            request.Destination = locationService.GetSJPLocation("8100OPK", SJPLocationType.Venue);

            // Fix to ensure planning for 2012
            DateTime dtOutward = DateTime.Now;
            DateTime dtOutward2012 = new DateTime(2012, 8, 1, 12, 0, 0);
            if (dtOutward < dtOutward2012)
            {
                dtOutward = dtOutward2012;
            }

            request.OutwardDateTime = dtOutward;
            request.ReturnDateTime = dtOutward.AddHours(3);
            request.OutwardArriveBefore = true;
            request.ReturnArriveBefore = false;
            request.IsReturnRequired = true;

            request.AccessiblePreferences = new SJPAccessiblePreferences();

            // Public
            request.PlannerMode = SJPJourneyPlannerMode.PublicTransport;
            request.Modes = new System.Collections.Generic.List<SJPModeType>(
                new SJPModeType[8] 
                { SJPModeType.Rail, SJPModeType.Bus, SJPModeType.Coach, SJPModeType.Metro, SJPModeType.Underground, 
                  SJPModeType.Tram, SJPModeType.Ferry, SJPModeType.Air});

            request.OutwardJourneyPart = new Journey();
            request.ReturnJourneyPart = new Journey();

            // Public specific
            request.PublicAlgorithm = SJPPublicAlgorithmType.Default;

            request.Sequence = pp[JC.Keys.JourneyRequest_Sequence].Parse(3);
            request.InterchangeSpeed = pp[JC.Keys.JourneyRequest_InterchangeSpeed].Parse(0);
            request.WalkingSpeed = pp[JC.Keys.JourneyRequest_WalkingSpeed].Parse(80);
            request.MaxWalkingTime = pp[JC.Keys.JourneyRequest_MaxWalkingTime].Parse(30);
            request.RoutingGuideInfluenced = pp[JC.Keys.JourneyRequest_RoutingGuideInfluenced].Parse(false);
            request.RoutingGuideCompliantJourneysOnly = pp[JC.Keys.JourneyRequest_RoutingGuideCompliantJourneysOnly].Parse(false);
            request.RouteCodes = pp[JC.Keys.JourneyRequest_RouteCodes];
            request.OlympicRequest = pp[JC.Keys.JourneyRequest_OlympicRequest].Parse(true);

            request.TravelDemandPlanOutward = pp[JC.Keys.JourneyRequest_TravelDemandPlanOutward];
            request.TravelDemandPlanReturn = pp[JC.Keys.JourneyRequest_TravelDemandPlanReturn];
            request.RemoveAwkwardOvernight = pp[JC.Keys.JourneyRequest_RemoveAwkwardOvernight].Parse(false);


            // Hash
            request.JourneyRequestHash = request.GetSJPHashCode().ToString();

            return request;
        }
        
        #endregion
    }
}
