// *********************************************** 
// NAME             : SqlTypeHelperTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SqlTypeHelper
// ************************************************
                
                
using SJP.Common.DatabaseInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.DatabaseInfrastructure
{
    
    
    /// <summary>
    ///This is a test class for SqlTypeHelperTest and is intended
    ///to contain all SqlTypeHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SqlTypeHelperTest
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
        ///A test for IsSqlDateTimeCompatible
        ///</summary>
        [TestMethod()]
        public void IsSqlDateTimeCompatibleTest()
        {
            // check with min datatime value
            DateTime dateTime = DateTime.MinValue; 
            bool expected = false; 
            bool actual;
            actual = SqlTypeHelper.IsSqlDateTimeCompatible(dateTime);
            Assert.AreEqual(expected, actual);

            // check with max datatime value
            dateTime = DateTime.MaxValue;
            expected = false;
            actual = SqlTypeHelper.IsSqlDateTimeCompatible(dateTime);
            Assert.AreEqual(expected, actual);

            // check with valid datatime value
            dateTime = new DateTime(2012,7,31);
            expected = true;
            actual = SqlTypeHelper.IsSqlDateTimeCompatible(dateTime);
            Assert.AreEqual(expected, actual);
            

        }
    }
}
