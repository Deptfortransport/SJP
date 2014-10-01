// *********************************************** 
// NAME             : ProgramTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for LocationJsGeneator program class
// ************************************************
                
                
using SJP.Common.LocationJsGenerator;
using SJP.Common.ServiceDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using SJP.Common;

namespace SJP.TestProject.LocationJSGenerator
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramTest
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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Main
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LocationJsGenerator.exe")]
        public void MainTest()
        {
            string[] args = null; 
            
            int exitCode = Program_Accessor.Main(args);

            Assert.AreEqual(0, exitCode);
            
        }

        /// <summary>
        ///A test for Main
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LocationJsGenerator.exe")]
        public void MainTestWithParameters()
        {
            string[] args = new string[] { "/v", "0" }; 

            int exitCode = Program_Accessor.Main(args);

            Assert.AreEqual(0, exitCode);

        }

        /// <summary>
        ///A test for Main when Exception gets raised
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LocationJsGenerator.exe")]
        public void MainTestWithException()
        {
            // simulating alias file specified but no alias file found
            File.Move("LocationJsGenerator/alias.csv", "LocationJsGenerator/alias1.csv");
            string[] args = new string[] { "/v", "0" };

            int exitCode = Program_Accessor.Main(args);

            Assert.IsTrue(exitCode != 0);

            Assert.AreEqual((int)SJPExceptionIdentifier.LJSGenAliasDataLoadFailed, exitCode);

            File.Move("LocationJsGenerator/alias1.csv", "LocationJsGenerator/alias.csv");

        }
    }
}
