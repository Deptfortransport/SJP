using SJP.UserPortal.SessionManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for StringKeyTest and is intended
    ///to contain all StringKeyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringKeyTest
    {


        private string keyName = "keyName";
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
        ///A test for StringKey Constructor
        ///</summary>
        [TestMethod()]
        public void StringKeyConstructorTest()
        {
            StringKey target = new StringKey(keyName);
            Assert.AreEqual("str@" + keyName, target.ToString());
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            StringKey target = new StringKey(keyName);
            string keyConst = "str@" + keyName;
            int expected = keyConst.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            StringKey target = new StringKey(keyName);
            string actual;
            actual = target.ToString();
            Assert.AreEqual("str@" + keyName, actual);
        }

        /// <summary>
        ///A test for ID
        ///</summary>
        [TestMethod()]
        public void IDTest()
        {
            StringKey target = new StringKey(keyName); 
            string actual = target.ID;
            Assert.AreEqual("str@" + keyName, actual, "Incorrect ID returned");
        }
    }
}
