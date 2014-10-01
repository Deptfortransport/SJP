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
using SJP.Common.EventLogging;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ReferenceTransactionEventTest and is intended
    ///to contain all ReferenceTransactionEventTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReferenceTransactionEventTest
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
        ///A test for ReferenceTransactionEvent Constructor
        ///</summary>
        [TestMethod()]
        public void ReferenceTransactionEventConstructorTest()
        {
            string category = string.Empty; 
            bool serviceLevelAgreement = false;
            DateTime submitted = DateTime.Now;
            string sessionId = string.Empty; 
            bool successful = false; 
            string machineName = string.Empty; 
            ReferenceTransactionEvent target = new ReferenceTransactionEvent(category, serviceLevelAgreement, submitted, sessionId, successful, machineName);

            Assert.IsTrue(category == target.EventType);
            Assert.IsTrue(machineName == target.MachineName);
            Assert.IsTrue(serviceLevelAgreement == target.ServiceLevelAgreement);
            Assert.IsTrue(submitted == target.Submitted);
            Assert.IsTrue(successful == target.Successful);
        }
        
        /// <summary>
        ///A test for FileFormatter
        ///</summary>
        [TestMethod()]
        public void ReferenceTransactionEventFileFormatterTest()
        {
            string category = string.Empty; 
            bool serviceLevelAgreement = false;
            DateTime submitted = DateTime.Now;
            string sessionId = string.Empty; 
            bool successful = false; 
            string machineName = string.Empty; 
            ReferenceTransactionEvent target = new ReferenceTransactionEvent(category, serviceLevelAgreement, submitted, sessionId, successful, machineName);

            IEventFormatter actual = target.FileFormatter;

            string eventAsString = actual.AsString(target);

            Assert.IsNotNull(eventAsString);
        }
        
    }
}
