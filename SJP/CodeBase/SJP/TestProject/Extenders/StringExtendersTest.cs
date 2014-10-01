// *********************************************** 
// NAME             : StringExtendersTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for StringExtenders
// ************************************************
                
                
using SJP.Common.Extenders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace SJP.TestProject.Extenders
{
    
    
    /// <summary>
    ///This is a test class for StringExtendersTest and is intended
    ///to contain all StringExtendersTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringExtendersTest
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
        ///A test for IsValidEmailAddress
        ///</summary>
        [TestMethod()]
        public void IsValidEmailAddressTest()
        {
            string address = "abc@test.com";  
            bool expected = true; 
            bool actual;
            actual = StringExtenders.IsValidEmailAddress(address);
            Assert.AreEqual(expected, actual);

            // Invalid email address
            address = "abc@test"; 
            expected = false;
            actual = StringExtenders.IsValidEmailAddress(address);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for IsValidPostcode
        ///</summary>
        [TestMethod()]
        public void IsValidPostcodeTest()
        {
            string postcode = "SE9 1AB"; 
            bool expected = true; 
            bool actual;
            actual = StringExtenders.IsValidPostcode(postcode);
            Assert.AreEqual(expected, actual);

            // Partial Postcode
            postcode = "SE9";
            expected = true;
            actual = StringExtenders.IsValidPostcode(postcode);
            Assert.AreEqual(expected, actual);

            //Invalid Postcode
            postcode = "SE9ABC";
            expected = false;
            actual = StringExtenders.IsValidPostcode(postcode);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for LowercaseFirst
        ///</summary>
        [TestMethod()]
        public void LowercaseFirstTest()
        {
            string text = "ABCDE"; 
            string expected = "aBCDE"; 
            string actual;
            actual = StringExtenders.LowercaseFirst(text);
            Assert.AreEqual(expected, actual);

            // empty string
            actual = StringExtenders.LowercaseFirst(string.Empty);
            Assert.AreEqual(string.Empty, actual);
        }

        /// <summary>
        ///A test for MatchesRegex
        ///</summary>
        [TestMethod()]
        public void MatchesRegexTest()
        {
            string text = "567"; 
            string regex = @"^\d{3}$"; 
            bool expected = true; 
            bool actual;
            actual = StringExtenders.MatchesRegex(text, regex);
            Assert.AreEqual(expected, actual);

            // regex not matched
            text = "5678"; 
            expected = false;
            actual = StringExtenders.MatchesRegex(text, regex);
            Assert.AreEqual(expected, actual);
            
            // no regex
            actual = StringExtenders.MatchesRegex(text, null);
            Assert.AreEqual(false, actual);

            // no text
            actual = StringExtenders.MatchesRegex(null, regex);
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        public void ParseTest()
        {
            string value = "23"; 
            int defaultValue = 5;
            int expected = 23; 
            int actual;
            actual = StringExtenders.Parse<int>(value, defaultValue);
            Assert.AreEqual(expected, actual);

            // Default Value
            value = "ab";
            expected = 5;
            actual = StringExtenders.Parse<int>(value, defaultValue);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse where no default value specified
        ///</summary>
        [TestMethod()]
        public void ParseTest1()
        {
            string value = "23";
            int expected = 23;
            int actual;
            actual = StringExtenders.Parse<int>(value);
            Assert.AreEqual(expected, actual);

            // Default Value
            value = "ab";
            expected = 0;
            actual = StringExtenders.Parse<int>(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse where culture info specified
        ///</summary>
        [TestMethod()]
        public void ParseTest2()
        {
            CultureInfo cultureInfo = new CultureInfo("en-GB"); 
            string value = "23"; 
            int defaultValue = 5;
            int expected = 23; 
            int actual;
            actual = StringExtenders.Parse<int>(value, defaultValue, cultureInfo);
            Assert.AreEqual(expected, actual);

            // Default Value
            value = "ab";
            expected = 5;
            actual = StringExtenders.Parse<int>(value, defaultValue, cultureInfo);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SubstringFirst
        ///</summary>
        [TestMethod()]
        public void SubstringFirst1()
        {
            string text = "ABCDE";
            string expected = "ABC";

            string actual;
            actual = StringExtenders.SubstringFirst(text, 3);
            Assert.AreEqual(expected, actual);

            // empty string
            actual = StringExtenders.SubstringFirst(string.Empty, 3);
            Assert.AreEqual(string.Empty, actual);

            // short string
            actual = StringExtenders.SubstringFirst(text, 100);
            Assert.AreEqual(text, actual);
        }

        /// <summary>
        ///A test for SubstringFirst
        ///</summary>
        [TestMethod()]
        public void SubstringFirst2()
        {
            string text = "ABCDE";
            string expected = "ABC";

            string actual;
            actual = StringExtenders.SubstringFirst(text, 'D');
            Assert.AreEqual(expected, actual);

            // empty string
            actual = StringExtenders.SubstringFirst(string.Empty, 'X');
            Assert.AreEqual(string.Empty, actual);

            // short string
            actual = StringExtenders.SubstringFirst(text, 'X');
            Assert.AreEqual(text, actual);
        }
    }
}
