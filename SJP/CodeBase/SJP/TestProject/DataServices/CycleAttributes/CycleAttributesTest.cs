// *********************************************** 
// NAME             : CycleAttributesTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for CycleAttributes
// ************************************************
                
                
using SJP.Common.DataServices.CycleAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SJP.Common.ServiceDiscovery;
using SJP.Common.DatabaseInfrastructure;
using System.Linq;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for CycleAttributesTest and is intended
    ///to contain all CycleAttributesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CycleAttributesTest
    {


        private TestContext testContextInstance;
        private static TestDataManager testDataManager;

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
        public static void TestClassInitialize(TestContext testContext)
        {
            string test_data = @"DataServices\CycleAttributeData.xml";
            string setup_script = @"DataServices\DataServicesTestSetup.sql";
            string clearup_script = @"DataServices\DataServicesTestCleanUp.sql";
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SJPTransientPortal;Trusted_Connection=true";

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
            testDataManager = new TestDataManager(
                test_data,
                setup_script,
                clearup_script,
                connectionString,
                SqlHelperDatabase.TransientPortalDB);
            testDataManager.Setup();

            testDataManager.LoadData(false);

        }

        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void TestClassCleanup()
        {
            testDataManager.ClearData();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }

        
        #endregion


        /// <summary>
        ///A test for CycleAttributes Constructor
        ///</summary>
        [TestMethod()]
        public void CycleAttributesConstructorTest()
        {
            CycleAttributes target = new CycleAttributes();
            CycleAttributes_Accessor accessor = new CycleAttributes_Accessor(new PrivateObject(target));
            Assert.AreEqual(59, accessor.cycleAttributesCache.Keys.Count);
            Assert.AreEqual("CycleAttribute.Roundabout", accessor.cycleAttributesCache[13].CycleAttributeResourceName);
        }

        /// <summary>
        ///A test for GetCycleAttributeCategory
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dataservices.dll")]
        public void GetCycleAttributeCategoryTest()
        {
            CycleAttributes_Accessor target = new CycleAttributes_Accessor(); 
            string attributeCategory = "Type"; 
            CycleAttributeCategory expected = CycleAttributeCategory.Type; 
            CycleAttributeCategory actual;
            actual = target.GetCycleAttributeCategory(attributeCategory);
            Assert.AreEqual(expected, actual);

            // Invalid attribute category
            attributeCategory = "Type1";
            expected = CycleAttributeCategory.None;
            actual = target.GetCycleAttributeCategory(attributeCategory);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for GetCycleAttributeGroup
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dataservices.dll")]
        public void GetCycleAttributeGroupTest()
        {
            CycleAttributes_Accessor target = new CycleAttributes_Accessor(); 
            string attributeGroup = "ITN"; 
            CycleAttributeGroup expected = CycleAttributeGroup.ITN; 
            CycleAttributeGroup actual;
            actual = target.GetCycleAttributeGroup(attributeGroup);
            Assert.AreEqual(expected, actual);

            // Invalid attribute group
            attributeGroup = "ITN1";
            expected = CycleAttributeGroup.None;
            actual = target.GetCycleAttributeGroup(attributeGroup);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for GetCycleAttributeType
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.dataservices.dll")]
        public void GetCycleAttributeTypeTest()
        {
            CycleAttributes_Accessor target = new CycleAttributes_Accessor();
            
            string attributeType = "Link"; 
            CycleAttributeType expected = CycleAttributeType.Link; 
            CycleAttributeType actual;
            actual = target.GetCycleAttributeType(attributeType);
            Assert.AreEqual(expected, actual);

            attributeType = "Link1";
            expected = CycleAttributeType.None;
            actual = target.GetCycleAttributeType(attributeType);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for GetCycleAttributes
        ///</summary>
        [TestMethod()]
        public void GetCycleAttributesTest()
        {
            CycleAttributes target = new CycleAttributes();
            
            List<CycleAttribute> actual;
            actual = target.GetCycleAttributes();

            Assert.AreEqual(59, actual.Count);
            Assert.AreEqual("CycleAttribute.Roundabout", actual[0].CycleAttributeResourceName);
 
        }

        /// <summary>
        ///A test for GetCycleAttributes
        ///</summary>
        [TestMethod()]
        public void GetCycleAttributesTest1()
        {
            CycleAttributes target = new CycleAttributes(); 
            CycleAttributeType cycleAttributeType = CycleAttributeType.Link; 
            CycleAttributeGroup cycleAttributeGroup = CycleAttributeGroup.User1; 
            List<CycleAttribute> actual;
            actual = target.GetCycleAttributes(cycleAttributeType, cycleAttributeGroup);
            Assert.AreEqual(23, actual.Count);

            actual = actual.OrderBy(attr => attr.CycleAttributeId).ToList();

            Assert.AreEqual("CycleAttribute.Pelican", actual[0].CycleAttributeResourceName);
            
        }

        /// <summary>
        ///A test for GetCycleInfrastructureAttributes
        ///</summary>
        [TestMethod()]
        public void GetCycleInfrastructureAttributesTest()
        {
            CycleAttributes target = new CycleAttributes(); 
            CycleAttributeGroup cycleAttributeGroup = CycleAttributeGroup.User1; 
            List<CycleAttribute> actual;
            actual = target.GetCycleInfrastructureAttributes(cycleAttributeGroup);
            Assert.AreEqual(6, actual.Count);
            
        }

        /// <summary>
        ///A test for GetCycleRecommendedAttributes
        ///</summary>
        [TestMethod()]
        public void GetCycleRecommendedAttributesTest()
        {
            CycleAttributes target = new CycleAttributes(); 
            CycleAttributeGroup cycleAttributeGroup = CycleAttributeGroup.User2; 
            List<CycleAttribute> actual;
            actual = target.GetCycleRecommendedAttributes(cycleAttributeGroup);
            Assert.AreEqual(4, actual.Count);
            
        }

        
    }
}
