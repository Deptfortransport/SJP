// *********************************************** 
// NAME             : OSGridReferenceTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 Apr 2011
// DESCRIPTION  	: OSGridReferenceTest test class
// ************************************************
// 
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for OSGridReferenceTest and is intended
    ///to contain all OSGridReferenceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OSGridReferenceTest
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
        ///A test for OSGridReference Constructor
        ///</summary>
        [TestMethod()]
        public void OSGridReferenceConstructorTest()
        {
            float gridRefEasting = 312345F;
            float gridRefNorthing = 312345F;

            OSGridReference target = new OSGridReference(gridRefEasting, gridRefNorthing);

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            Assert.IsTrue(target.IsValid, "Expected OSGridReference IsValid to return true");

            target.Easting = gridRefEasting + 1;
            target.Northing = gridRefNorthing + 1;

            Assert.IsTrue(target.Easting == gridRefEasting + 1, "Expected OSGridReference Easting to have been updated");
            Assert.IsTrue(target.Northing == gridRefNorthing + 1, "Expected OSGridReference Northing to have been updated");

            Assert.AreEqual(target.Easting.GetHashCode() ^ target.Northing.GetHashCode(), target.GetSJPHashCode());

            Assert.AreEqual(14, target.DistanceFrom(new OSGridReference(target.Easting+10, target.Northing+10)));
        }

        /// <summary>
        ///A test for OSGridReference Constructor
        ///</summary>
        [TestMethod()]
        public void OSGridReferenceConstructorTest1()
        {
            OSGridReference target = new OSGridReference();

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            Assert.IsFalse(target.IsValid);
        }

        /// <summary>
        ///A test for OSGridReference Constructor
        ///</summary>
        [TestMethod()]
        public void OSGridReferenceConstructorTest2()
        {
            string gridRef = null;
            OSGridReference target = new OSGridReference(gridRef);

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            gridRef = string.Empty;
            target = new OSGridReference(gridRef);

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            gridRef = "0";
            target = new OSGridReference(gridRef);

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            gridRef = "0,0";
            target = new OSGridReference(gridRef);

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            gridRef = "a,b";
            target = new OSGridReference(gridRef);

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            gridRef = "1,-1";
            target = new OSGridReference(gridRef);

            Assert.IsNotNull(target, "Expected OSGridReference object to not be null");

            gridRef = target.ToString();

            Assert.IsNotNull(target, "Expected OSGridReference string to not be null");

            int hashcode = target.GetSJPHashCode();

            Assert.IsTrue(hashcode != 0, "Expected OSGridReference hashcode to not be null");
        }
    }
}
