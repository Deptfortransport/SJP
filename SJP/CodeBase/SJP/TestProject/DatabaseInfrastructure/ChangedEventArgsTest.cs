// *********************************************** 
// NAME             : ChangedEventArgsTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit test for ChangedEventArgs class
// ************************************************
                
                
using SJP.Common.DatabaseInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.DatabaseInfrastructure
{
    
    
    /// <summary>
    ///This is a test class for ChangedEventArgsTest and is intended
    ///to contain all ChangedEventArgsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ChangedEventArgsTest
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
        ///A test for ChangedEventArgs Constructor
        ///</summary>
        [TestMethod()]
        public void ChangedEventArgsConstructorTest()
        {
            string groupId = "test"; 
            ChangedEventArgs target = new ChangedEventArgs(groupId);
            Assert.AreEqual(groupId, target.GroupId);
        }
    }
}
