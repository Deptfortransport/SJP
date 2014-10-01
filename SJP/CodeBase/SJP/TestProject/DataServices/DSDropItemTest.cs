// *********************************************** 
// NAME             : DSDropItemTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for DSDropItem
// ************************************************
                
                
using SJP.Common.DataServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for DSDropItemTest and is intended
    ///to contain all DSDropItemTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DSDropItemTest
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
        ///A test for DSDropItem Constructor
        ///</summary>
        [TestMethod()]
        public void DSDropItemConstructorTest()
        {
            string resourceID = "testResourceId"; 
            string itemValue = "testValue"; 
            bool isSelected = true; 
            DSDropItem target = new DSDropItem(resourceID, itemValue, isSelected);

            Assert.AreEqual(resourceID, target.ResourceID);
            Assert.AreEqual(itemValue, target.ItemValue);
            Assert.AreEqual(isSelected, target.IsSelected);
        }

        
    }
}
