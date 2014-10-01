// *********************************************** 
// NAME             : PageTransferDetailTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: PageTransferDetailTest test
// ************************************************
// 
                
using SJP.UserPortal.ScreenFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for PageTransferDetailTest and is intended
    ///to contain all PageTransferDetailTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageTransferDetailTest
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
        ///A test for PageTransferDetail Constructor
        ///</summary>
        [TestMethod()]
        public void PageTransferDetailConstructorTest()
        {
            PageId pageId = PageId.Empty;
            string pageUrl = string.Empty;
            PageTransferDetail target = new PageTransferDetail(pageId, pageUrl);

            Assert.IsNotNull(target, "Expected PageTransferDetail to be created");
        }

        /// <summary>
        ///A test for PageId
        ///</summary>
        [TestMethod()]
        public void PageIdTest()
        {
            PageId pageId = PageId.Empty;
            string pageUrl = "test";
            PageTransferDetail target = new PageTransferDetail(pageId, pageUrl);
            PageId actual = target.PageId;

            Assert.IsTrue(actual == pageId, "Expected PageId to correct");
        }

        /// <summary>
        ///A test for PageUrl
        ///</summary>
        [TestMethod()]
        public void PageUrlTest()
        {
            PageId pageId = PageId.Empty;
            string pageUrl = "test";
            PageTransferDetail target = new PageTransferDetail(pageId, pageUrl);
            string actual = target.PageUrl;

            Assert.IsTrue(actual == pageUrl, "Expected PageUrl to correct");
        }
    }
}
