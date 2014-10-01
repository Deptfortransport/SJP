// *********************************************** 
// NAME             : RetailerCatalogueTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 09 Apr 2011
// DESCRIPTION  	: RetailerCatalogueTest test class
// ************************************************
// 

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common;
using SJP.UserPortal.Retail;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for RetailerCatalogueTest and is intended
    ///to contain all RetailerCatalogueTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RetailerCatalogueTest
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
        }
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
        //    SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        //    SJPServiceDiscovery.Init(new TestInitialisation());
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
        ///A test for RetailerCatalogue Constructor
        ///</summary>
        [TestMethod()]
        public void RetailerCatalogueConstructorTest()
        {
            RetailerCatalogue target = new RetailerCatalogue();

            Assert.IsNotNull(target, "Expected RetailerCatalogue object to be created");
        }

        /// <summary>
        ///A test for FindRetailer
        ///</summary>
        [TestMethod()]
        public void FindRetailerTest()
        {
            RetailerCatalogue target = new RetailerCatalogue();

            // Test invalid retailer
            Retailer actual = target.FindRetailer(null);
            Assert.IsNull(actual, "Expected null Retailer to have been returned");

            actual = target.FindRetailer(string.Empty);
            Assert.IsNull(actual, "Expected null Retailer to have been returned");

            actual = target.FindRetailer("Invalid");
            Assert.IsNull(actual, "Expected null Retailer to have been returned");


            // Identify a retailer to find
            IList<Retailer> retailers = target.GetRetailers();
            Assert.IsTrue(retailers.Count > 0, "Expected retailers to exist in RetailerCatalogue, check retailers exist in database");
                        
            Retailer expected = retailers[0];

            string id = expected.Id;

            // Test valid 
            actual = target.FindRetailer(id);

            Assert.IsNotNull(actual, "Expected Retailer to have been returned");
            Assert.IsTrue(actual.Id == id, "Expected requested Retailer to have been returned");
            
        }

        /// <summary>
        ///A test for FindRetailers
        ///</summary>
        [TestMethod()]
        public void FindRetailersTest()
        {
            RetailerCatalogue target = new RetailerCatalogue();

            string operatorCode = string.Empty;
            SJPModeType mode = SJPModeType.Rail;

            IList<Retailer> actual = null;

            // Test no operator code
            actual = target.FindRetailers(operatorCode, mode);

            Assert.IsNotNull(actual, "Expected Retailers to have been returned");
            Assert.IsTrue(actual.Count > 0, "Expected retailers to exist in Retailers list returned");

            // Test operator code starts/ends with *

            operatorCode = "*NoRetailer*";
            actual = target.FindRetailers(operatorCode, mode);

            Assert.IsNull(actual, "Expected no Retailers to have been returned");

            // Test operator code starts/ends with :
            operatorCode = ":NoRetailer:";
            actual = target.FindRetailers(operatorCode, mode);

            Assert.IsNull(actual, "Expected no Retailers to have been returned");
        }

        /// <summary>
        ///A test for GetRetailers
        ///</summary>
        [TestMethod()]
        public void GetRetailersTest()
        {
            RetailerCatalogue target = new RetailerCatalogue();

            IList<Retailer> actual = target.GetRetailers();

            Assert.IsTrue(actual.Count > 0, "Expected retailers to exist in RetailerCatalogue");

            // Sort retailers for code coverage
            List<Retailer> retailers = new List<Retailer>(actual);
            retailers.Sort(new Retailer());
        }

        /// <summary>
        ///A test for Retailer object
        ///</summary>
        [TestMethod()]
        public void RetailerTest()
        {
            // Test retailer get/set properties
            Retailer retailer = new Retailer();
            retailer.Id = "Test";
            retailer.Name = "Test";
            retailer.DisplayUrl = "Test";
            retailer.WebsiteUrl = "Test";
            retailer.HandoffUrl = "Test";
            retailer.ResourceKey = "Test";
            retailer.Modes = new List<SJPModeType>(new SJPModeType[1] { SJPModeType.Rail });
            retailer.PhoneNumber = "Test";
            retailer.PhoneNumberDisplay = "Test";

            Assert.IsTrue(retailer.Id != null, "Expected Retailer Id to not be null");
            Assert.IsTrue(retailer.Name != null, "Expected Retailer Name to not be null");
            Assert.IsTrue(retailer.DisplayUrl != null, "Expected Retailer DisplayUrl to not be null");
            Assert.IsTrue(retailer.WebsiteUrl != null, "Expected Retailer WebsiteUrl to not be null");
            Assert.IsTrue(retailer.HandoffUrl != null, "Expected Retailer HandoffUrl to not be null");
            Assert.IsTrue(retailer.ResourceKey != null, "Expected Retailer ResourceKey to not be null");
            Assert.IsTrue(retailer.Modes.Count > 0, "Expected Retailer Modes to not be null");
            Assert.IsTrue(retailer.HandoffSupported, "Expected Retailer HandoffSupported to not be null");
            Assert.IsTrue(retailer.PhoneNumber != null, "Expected Retailer Name to not be null");
            Assert.IsTrue(retailer.PhoneNumberDisplay != null, "Expected Retailer Name to not be null");

        }

        /// <summary>
        ///A test for RetailerKey object
        ///</summary>
        [TestMethod()]
        public void RetailerKeyTest()
        {
            // Test retailer key get/set properties
            RetailerKey_Accessor retailerKey = new RetailerKey_Accessor("Test", SJPModeType.Rail);

            Assert.IsTrue(retailerKey.OperatorCode != null, "Expected RetailerKey OperatorCode to not be null");
            Assert.IsTrue(retailerKey.Mode == SJPModeType.Rail, "Expected RetailerKey Mode to not be null");
            Assert.IsTrue(retailerKey.GetHashCode() != 0, "Expected Retailer GetHashCode() to not equal 0");

            // Equal test
            RetailerKey_Accessor retailerKeyMatch = new RetailerKey_Accessor("Test", SJPModeType.Rail);

            Assert.IsTrue(retailerKey.Equals(retailerKeyMatch), "Expected RetailerKeys to equal");

            // No Equal test
            RetailerKey_Accessor retailerKeyNoMatch = new RetailerKey_Accessor("TestNoMatch", SJPModeType.Rail);

            Assert.IsTrue(!retailerKey.Equals(retailerKeyNoMatch), "Expected RetailerKeys to not equal");

            // Not RetailKey test
            string notRetailKey = "NotRetailKey";

            Assert.IsTrue(!retailerKey.Equals(notRetailKey), "Expected object to not equal RetailKey");
        }

        /// <summary>
        ///A test for Retailer object comparison
        ///</summary>
        [TestMethod()]
        public void RetailerCompareTest()
        {
            // Test retailer get/set properties
            Retailer retailer = new Retailer();
            retailer.Id = "Test";
            retailer.Name = "Test";
            retailer.DisplayUrl = "Test";
            retailer.WebsiteUrl = "Test";
            retailer.HandoffUrl = "Test";
            retailer.ResourceKey = "Test";
            retailer.Modes = new List<SJPModeType>(new SJPModeType[1] { SJPModeType.Rail });

            Retailer retailerB = new Retailer();
            retailerB.Id = "Test";
            retailerB.Name = "Test";
            retailerB.DisplayUrl = "Test";
            retailerB.WebsiteUrl = "Test";
            retailerB.HandoffUrl = "Test";
            retailerB.ResourceKey = "Test";
            retailerB.Modes = new List<SJPModeType>(new SJPModeType[1] { SJPModeType.Rail });

            Assert.IsTrue(retailer.Compare(null, null) == 0, "Expected compare value 0");
            Assert.IsTrue(retailer.Compare(null, retailerB) < 0, "Expected compare value > 0");
            Assert.IsTrue(retailer.Compare(retailer, null) > 0, "Expected compare value > 0");
            Assert.IsTrue(retailer.Compare(retailer, retailerB) == 0, "Expected compare value 0");

            retailerB.Id = "NotTest";

            Assert.IsTrue(retailer.Compare(retailer, retailerB) > 0, "Expected compare value > 0");
        }
    }
}
