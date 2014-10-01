using SJP.UserPortal.ScreenFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;
using System.Web;
using System.Collections.Specialized;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for PageTransferCacheTest and is intended
    ///to contain all PageTransferCacheTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageTransferCacheTest
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
        ///A test for PageTransferCache Constructor
        ///</summary>
        [TestMethod()]
        public void PageTransferCacheConstructorTest()
        {
            PageTransferCache target = new PageTransferCache();

            Assert.IsNotNull(target, "Expected PageTransferCache to be created");
        }

        /// <summary>
        ///A test for GetPageTransferDetails
        ///</summary>
        [TestMethod()]
        public void PageTransferCacheGetPageTransferDetailsTest()
        {
            try
            {
                // Because no HttpContext exists, manually set up the transfer cache, otherwise exception will be thrown
                PageTransferCache_Accessor target = new PageTransferCache_Accessor();

                PageId pageId = PageId.Default;

                PageTransferDetail pageTransferDetail = new PageTransferDetail(pageId, "testurl");

                target.pageTransferDetails.Add(pageId, pageTransferDetail);
                
                
                PageTransferDetail actual = target.GetPageTransferDetails(pageId);

                Assert.IsNotNull(actual, "Expected PageTransferDetail to be found");
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Exception was thrown attempting to GetPageTransferDetails, exception: {0}", ex.Message));
            } 
        }

        /// <summary>
        ///A test for ParseSiteMapNode
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.screenFlow.dll")]
        public void ParseSiteMapNodeTest()
        {
            PageTransferCache_Accessor target = new PageTransferCache_Accessor();

            // Create a Test SiteMapProvider
            using (XmlSiteMapProvider smp = new XmlSiteMapProvider())
            {

                NameValueCollection providerAttributes = new NameValueCollection(1);
                providerAttributes.Add("siteMapFile", "Web.sitemap");

                smp.Initialize("TestProvider", providerAttributes);

                // Do not build because no HttpContext exists, this would throw an exception
                //smp.BuildSiteMap();

                // Valid page id
                try
                {
                    string pageId = PageId.Default.ToString();

                    NameValueCollection nodeAttributes = new NameValueCollection(1);
                    nodeAttributes.Add("pageId", pageId);

                    SiteMapNode smn = new SiteMapNode(smp, pageId, "~/Default.aspx", pageId, pageId, null, nodeAttributes, null, "Pages.Default");

                    target.ParseSiteMapNode(smn);
                }
                catch (Exception ex)
                {
                    Assert.Fail(string.Format("Exception was thrown parsing a SiteMapNote, exception: {0}", ex.Message));
                }

                // Invalid page id
                try
                {
                    string pageId = null;

                    NameValueCollection nodeAttributes = new NameValueCollection(1);
                    nodeAttributes.Add("pageId", pageId);

                    SiteMapNode smn = new SiteMapNode(smp, "Default", "~/Default.aspx", pageId, pageId, null, nodeAttributes, null, "Pages.Default");

                    target.ParseSiteMapNode(smn);
                }
                catch (SJPException s)
                {
                    // Expect null argument exception to be thrown
                    Assert.IsTrue(s.Identifier == SJPExceptionIdentifier.SFMScreenFlowPageIdError, "Unexpected SJPException was thrown parsing a SiteMapNote");
                }
                catch (Exception ex)
                {
                    Assert.Fail(string.Format("Unexpected ecception was thrown parsing a SiteMapNote, exception: {0}", ex.Message));
                }

                // Invalid page id
                try
                {
                    string pageId = "test";

                    NameValueCollection nodeAttributes = new NameValueCollection(1);
                    nodeAttributes.Add("pageId", pageId);

                    SiteMapNode smn = new SiteMapNode(smp, pageId, "~/Default.aspx", pageId, pageId, null, nodeAttributes, null, "Pages.Default");

                    target.ParseSiteMapNode(smn);
                }
                catch (SJPException s)
                {
                    // Expect argument exception to be thrown
                    Assert.IsTrue(s.Identifier == SJPExceptionIdentifier.SFMScreenFlowPageIdError, "Unexpected SJPException was thrown parsing a SiteMapNote");
                }
                catch (Exception ex)
                {
                    Assert.Fail(string.Format("Unexpected ecception was thrown parsing a SiteMapNote, exception: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        ///A test for PopulatePageTransferDetails
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.screenFlow.dll")]
        public void PopulatePageTransferDetailsTest()
        {
            // UNABLE TO TEST BECAUSE HTTP CONTEXT DOES NOT EXIST TO GENERATE THE SITE MAP

            PageTransferCache_Accessor target = new PageTransferCache_Accessor();

            try
            {
                target.PopulatePageTransferDetails();
            }
            catch //(Exception ex)
            {
                //Assert.Fail(string.Format("Exception was throw attempting to PopulatePageTransferDetails, exception: {0}", ex.Message));
            }
        }
    }
}
