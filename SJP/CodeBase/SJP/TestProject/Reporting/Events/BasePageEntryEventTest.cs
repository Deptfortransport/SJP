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
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for BasePageEntryEventTest and is intended
    ///to contain all BasePageEntryEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BasePageEntryEventTest
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
        ///A test for BasePageEntryEvent Constructor
        ///</summary>
        [TestMethod()]
        public void BasePageEntryEventConstructorTest()
        {
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            BasePageEntryEvent target = new BasePageEntryEvent(sessionId, userLoggedOn);
        }

        /// <summary>
        ///A test for BasePageEntryEvent Constructor
        ///</summary>
        [TestMethod()]
        public void BasePageEntryEventConstructorTest1()
        {
            string sessionId = string.Empty;
            bool userLoggedOn = false;
            string partnerName = string.Empty;
            BasePageEntryEvent target = new BasePageEntryEvent(sessionId, userLoggedOn, partnerName);

            Assert.IsTrue(0 == target.ThemeId);
            Assert.IsTrue(partnerName == target.PartnerName);

            BasePageEntryEvent_Accessor accessor = new BasePageEntryEvent_Accessor(sessionId, userLoggedOn);
            
            Assert.IsNotNull(accessor.PageName);

            accessor.page = PageId.Empty;

            Assert.IsNotNull(accessor.PageName);
        }

    }
}
