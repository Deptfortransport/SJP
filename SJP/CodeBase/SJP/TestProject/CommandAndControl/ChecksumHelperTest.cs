using SJP.Common.ChecksumUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Specialized;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ChecksumHelperTest and is intended
    ///to contain all ChecksumHelperTest Unit Tests
    ///NB most of the testing of this class is done through the Hash Generator and Validator tests.
    ///</summary>
    [TestClass()]
    public class ChecksumHelperTest
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
        ///A test for ChecksumHelper Constructor
        ///</summary>
        [TestMethod()]
        public void ChecksumHelperConstructorTest()
        {
            try
            {
                ChecksumHelper target = new ChecksumHelper();
            }
            catch (Exception ex)
            {
                Assert.Fail("Constructor errored - {0}", new string[1]{ex.ToString()});
            }
        }

        ///// <summary>
        /////A test for DecryptFile
        /////</summary>
        //[TestMethod()]
        //public void DecryptFileTest()
        //{
        //    ChecksumHelper target = new ChecksumHelper(); // TODO: Initialize to an appropriate value
        //    string sInputFilename = string.Empty; // TODO: Initialize to an appropriate value
        //    string sOutputFilename = string.Empty; // TODO: Initialize to an appropriate value
        //    string sKey = string.Empty; // TODO: Initialize to an appropriate value
        //    target.DecryptFile(sInputFilename, sOutputFilename, sKey);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}


        /// <summary>
        ///A test for GenerateKey
        ///</summary>
        [TestMethod()]
        public void GenerateKeyTest()
        {
            ChecksumHelper target = new ChecksumHelper(); 
            string reply = string.Empty;

            try
            {
                reply = target.GenerateKey();
            }
            catch (Exception ex)
            {
                Assert.Fail("GenerateKey errored - {0}", new string[1] { ex.ToString() });
            }

            if (string.IsNullOrEmpty(reply))
            {
                Assert.Fail("No key returned by GenerateKey");
            }
        }
    }
}
