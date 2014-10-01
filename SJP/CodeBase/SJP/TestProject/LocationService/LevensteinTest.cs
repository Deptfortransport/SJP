// *********************************************** 
// NAME             : LevensteinTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for Levenstein class
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for LevensteinTest and is intended
    ///to contain all LevensteinTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LevensteinTest
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
        ///A test for MinOf3
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.locationservice.dll")]
        public void MinOf3Test()
        {
            Levenstein_Accessor target = new Levenstein_Accessor(); 
            double firstNumber = 3.0F; 
            double secondNumber = 2.86F; 
            double thirdNumber = 4.12F; 
            double expected = 2.86F; 
            double actual;
            actual = target.MinOf3(firstNumber, secondNumber, thirdNumber);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for GetUnnormalisedSimilarity
        ///</summary>
        [TestMethod()]
        public void GetUnnormalisedSimilarityTest()
        {
            Levenstein target = new Levenstein(); 
            string firstWord ="Leicester"; 
            string secondWord = "Lecester"; 
            double expected = 1F; 
            double actual;
            actual = target.GetUnnormalisedSimilarity(firstWord, secondWord);
            Assert.AreEqual(expected, actual);

            firstWord = string.Empty; // first word is empty
            actual = target.GetUnnormalisedSimilarity(firstWord, secondWord);
            Assert.AreEqual(8, actual);

            firstWord = "Leicester";
            secondWord = string.Empty; // Second word is empty
            actual = target.GetUnnormalisedSimilarity(firstWord, secondWord);
            Assert.AreEqual(9, actual);

            // Null test
            firstWord = "Leicester";
            secondWord = null;
            actual = target.GetUnnormalisedSimilarity(firstWord, secondWord);
            Assert.AreEqual(0, actual);

            firstWord = null;
            secondWord = null;
            actual = target.GetUnnormalisedSimilarity(firstWord, secondWord);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///A test for GetSimilarity
        ///</summary>
        [TestMethod()]
        public void GetSimilarityTest()
        {
            Levenstein target = new Levenstein(); 
            string firstWord = "Leicester"; 
            string secondWord = "Leicecter";
            double actual;
            actual = target.GetSimilarity(firstWord, secondWord);
            Assert.IsTrue(actual > 0 && actual < 1);


            firstWord = string.Empty; // first word is empty
            actual = target.GetSimilarity(firstWord, secondWord);
            Assert.AreEqual(0, actual);

            firstWord = "Leicester";
            secondWord = string.Empty; // Second word is empty
            actual = target.GetSimilarity(firstWord, secondWord);
            Assert.AreEqual(0, actual);

            firstWord = string.Empty; // Both empty, should match
            secondWord = string.Empty; 
            actual = target.GetSimilarity(firstWord, secondWord);
            Assert.AreEqual(1, actual);

            // Null test
            firstWord = "Leicester";
            secondWord = null; // Second word is null
            actual = target.GetSimilarity(firstWord, secondWord);
            Assert.AreEqual(0, actual);

            firstWord = null; // First word is null
            secondWord = null; 
            actual = target.GetSimilarity(firstWord, secondWord);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///A test for GetCost
        ///</summary>
        [TestMethod()]
        public void GetCostTest()
        {
            Levenstein target = new Levenstein();
            string firstWord = "Leicester"; 
            int firstWordIndex = 1;
            string secondWord = "Lecester"; 
            int secondWordIndex = 1; 
            double expected = 0F; 
            double actual;
            actual = target.GetCost(firstWord, firstWordIndex, secondWord, secondWordIndex);
            Assert.AreEqual(expected, actual);
            
            // char mismatch
            firstWordIndex = 2;
            secondWordIndex = 2;
            expected = 1F;
            actual = target.GetCost(firstWord, firstWordIndex, secondWord, secondWordIndex);
            Assert.AreEqual(expected, actual);

            // Null test
            firstWord = null;
            secondWord = null; 
            expected = 0;
            actual = target.GetCost(firstWord, firstWordIndex, secondWord, secondWordIndex);
            Assert.AreEqual(expected, actual);

            firstWord = "Leicester";
            secondWord = null;
            expected = 0;
            actual = target.GetCost(firstWord, firstWordIndex, secondWord, secondWordIndex);
            Assert.AreEqual(expected, actual);
        }

        
    }
}
