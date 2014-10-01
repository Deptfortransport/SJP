using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for DeferredKeyTest and is intended
    ///to contain all DeferredKeyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DeferredKeyTest
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
        ///A test for DeferredKey Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.sessionmanager.dll")]
        public void DeferredKeyConstructorTest()
        {
            DeferredKey_Accessor target = new DeferredKey_Accessor();
            Assert.IsNotNull(target, "Null object returned");
        }

        /// <summary>
        ///A test for DeferredKey Constructor
        ///</summary>
        [TestMethod()]
        public void DeferredKeyConstructorTest1()
        {
            string keyName = "keyName";
            DeferredKey target = new DeferredKey(keyName);
            Assert.AreEqual("def@" + keyName, target.ToString());
        }
    }
}
