// *********************************************** 
// NAME             : StopAccessibilityLinkTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for StopAccessibilityLink
// ************************************************
                
                
using SJP.Common.DataServices.StopAccessibilityLinks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for StopAccessibilityLinkTest and is intended
    ///to contain all StopAccessibilityLinkTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StopAccessibilityLinkTest
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
        ///A test for StopAccessibilityLink Constructor
        ///</summary>
        [TestMethod()]
        public void StopAccessibilityLinkConstructorTest()
        {
            string stopNaPTAN = "9000LECSTER"; 
            string stopOperator = "East Midlands"; 
            string stopAccessibilityURL = "www.transportdirect.info"; 
            DateTime dateWEF = new DateTime(2012,07,01); 
            DateTime dateWEU = new DateTime(2012,09,14); 
            StopAccessibilityLink target = new StopAccessibilityLink(stopNaPTAN, stopOperator, stopAccessibilityURL, dateWEF, dateWEU);
            Assert.AreEqual(stopNaPTAN, target.StopNaPTAN);
            Assert.AreEqual(stopOperator, target.StopOperator);
            Assert.AreEqual(stopAccessibilityURL, target.StopAccessibilityURL);
            Assert.AreEqual(dateWEF, target.DateWEF);
            Assert.AreEqual(dateWEU, target.DateWEU);
        }

        

        /// <summary>
        ///A test for IsValidForDate
        ///</summary>
        [TestMethod()]
        public void IsValidForDateTest()
        {
            string stopNaPTAN = "9000LECSTER";
            string stopOperator = "East Midlands";
            string stopAccessibilityURL = "www.transportdirect.info";
            DateTime dateWEF = new DateTime(2012, 07, 01);
            DateTime dateWEU = new DateTime(2012, 09, 14);
            StopAccessibilityLink target = new StopAccessibilityLink(stopNaPTAN, stopOperator, stopAccessibilityURL, dateWEF, dateWEU);
            
            DateTime date = new DateTime(2012, 08, 31); // valid date
            bool expected = true; 
            bool actual;
            actual = target.IsValidForDate(date);
            Assert.AreEqual(expected, actual);

            date = new DateTime(2012, 10, 31); // invalid date
            expected = false;
            actual = target.IsValidForDate(date);
            Assert.AreEqual(expected, actual);
           
        }

        

        /// <summary>
        ///A test for DateWEU
        ///</summary>
        [TestMethod()]
        public void DateWEUTest()
        {
            StopAccessibilityLink target = new StopAccessibilityLink(); 
            DateTime actual;
            actual = target.DateWEU;
            Assert.AreEqual(DateTime.MaxValue, actual);
        }

       
    }
}
