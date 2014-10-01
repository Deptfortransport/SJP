// *********************************************** 
// NAME             : MessagesTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: MessagesTest test
// ************************************************
// 
                
using SJP.UserPortal.ScreenFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for MessagesTest and is intended
    ///to contain all MessagesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MessagesTest
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
        ///A test for Messages
        ///</summary>
        [TestMethod()]
        public void ScreenFlowMessagesTest()
        {
            Assert.IsTrue(!string.IsNullOrEmpty(Messages.PageTransferDataValidationError));
            Assert.IsTrue(!string.IsNullOrEmpty(Messages.PageTransferDataCacheConstructor));
            Assert.IsTrue(!string.IsNullOrEmpty(string.Format(Messages.EnumConversionFailed, "1")));
        }
    }
}
