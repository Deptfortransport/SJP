// *********************************************** 
// NAME             : CycleAttributeTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for CycleAttribute
// ************************************************
                
                
using SJP.Common.DataServices.CycleAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for CycleAttributeTest and is intended
    ///to contain all CycleAttributeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CycleAttributeTest
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
        ///A test for CycleAttribute Constructor
        ///</summary>
        [TestMethod()]
        public void CycleAttributeConstructorTest()
        {
            int attributeId = 0; 
            CycleAttributeType attributeType = CycleAttributeType.Link; 
            CycleAttributeGroup attributeGroup = CycleAttributeGroup.User0; 
            CycleAttributeCategory attributeCategory = CycleAttributeCategory.Type; 
            string attributeResourceName = "Test"; 
            long attributeMask = 232; 
            CycleAttribute target = new CycleAttribute(attributeId, attributeType, attributeGroup, attributeCategory, attributeResourceName, attributeMask);

            Assert.AreEqual(attributeId, target.CycleAttributeId);
            Assert.AreEqual(attributeType, target.CycleAttributeType);
            Assert.AreEqual(attributeGroup, target.CycleAttributeGroup);
            Assert.AreEqual(attributeCategory, target.CycleAttributeCategory);
            Assert.AreEqual(attributeResourceName, target.CycleAttributeResourceName);
            Assert.AreEqual(attributeMask, target.CycleAttributeMask);
        }

      
    }
}
