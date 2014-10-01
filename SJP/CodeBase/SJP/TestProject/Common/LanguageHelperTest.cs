// *********************************************** 
// NAME             : LanguageHelperTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Feb 2012
// DESCRIPTION  	: LanguageHelper tests
// ************************************************
// 
                
using SJP.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for LanguageHelperTest and is intended
    ///to contain all LanguageHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LanguageHelperTest
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
        ///A test for GetLanguageString
        ///</summary>
        [TestMethod()]
        public void LanguageHelperGetLanguageStringTest()
        {
            Language language = Language.English;
            string expected = "en";
            string actual;
            actual = LanguageHelper.GetLanguageString(language);
            Assert.AreEqual(expected, actual);

            language = Language.French;
            expected = "fr";
            actual = LanguageHelper.GetLanguageString(language);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ParseLanguage
        ///</summary>
        [TestMethod()]
        public void LanguageHelperParseLanguageTest()
        {
            string cultureName = "en";
            Language expected = Language.English;
            Language actual;
            actual = LanguageHelper.ParseLanguage(cultureName);
            Assert.AreEqual(expected, actual);

            cultureName = "fr";
            expected = Language.French;
            actual = LanguageHelper.ParseLanguage(cultureName);
            Assert.AreEqual(expected, actual);

            try
            {
                cultureName = "xx";
                actual = LanguageHelper.ParseLanguage(cultureName);
                Assert.Fail("Expected exception to be thrown for unrecognised language");
            }
            catch (SJPException sjpEx)
            {
                Assert.AreEqual(SJPExceptionIdentifier.RMLanguageNotHandled, sjpEx.Identifier);
            }
        }

        /// <summary>
        ///A test for Default
        ///</summary>
        [TestMethod()]
        public void LanguageHelperDefaultTest()
        {
            Language expected = Language.English;
            Language actual = LanguageHelper.Default;
            Assert.AreEqual(expected, actual);
        }
    }
}
