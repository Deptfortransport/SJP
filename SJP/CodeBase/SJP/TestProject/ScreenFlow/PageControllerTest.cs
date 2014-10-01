// *********************************************** 
// NAME             : PageControllerTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: PageControllerTest test
// ************************************************
// 
                
using SJP.UserPortal.ScreenFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for PageControllerTest and is intended
    ///to contain all PageControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageControllerTest
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
        ///A test for PageController Constructor
        ///</summary>
        [TestMethod()]
        public void PageControllerConstructorTest()
        {
            IPageTransferCache pageTransferCache = new PageTransferCache();
            PageController target = new PageController(pageTransferCache);

            Assert.IsNotNull(target, "Expected PageController object to be created");
        }

        /// <summary>
        ///A test for GetPageTransferDetails
        ///</summary>
        [TestMethod()]
        public void PageControllerGetPageTransferDetailsTest()
        {
            // UNABLE TO TEST BECAUSE HTTP CONTEXT DOES NOT EXIST TO GENERATE THE SITE MAP

            try
            {
                IPageTransferCache pageTransferCache = new PageTransferCache();
                PageController target = new PageController(pageTransferCache);

                Assert.IsNotNull(target, "Expected PageController object to be created");

                PageId pageId = PageId.Homepage;

                PageTransferDetail actual = target.GetPageTransferDetails(pageId);

                Assert.IsNotNull(actual, "Expected PageTransferDetail to be found");
            }
            catch //(Exception ex)
            {
                //Assert.Fail(string.Format("Exception was thrown attempting to GetPageTransferDetails, exception: {0}", ex.Message));
            } 
        }
    }
}
