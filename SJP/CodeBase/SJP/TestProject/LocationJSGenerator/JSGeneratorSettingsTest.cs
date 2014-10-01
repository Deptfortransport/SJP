// *********************************************** 
// NAME             : JSGeneratorSettingsTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for JSGeneratorSettings
// ************************************************
                
                
using SJP.Common.LocationJsGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.LocationJSGenerator
{
    
    
    /// <summary>
    ///This is a test class for JSGeneratorSettingsTest and is intended
    ///to contain all JSGeneratorSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JSGeneratorSettingsTest
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
        
        #endregion


        /// <summary>
        ///A test for GetCommandLindValue
        ///</summary>
        [TestMethod()]
        [DeploymentItem("LocationJsGenerator.exe")]
        public void GetCommandLindValueTest()
        {
            string key = "v";
            string expected = string.Empty;
            string actual;
            // command line arguments are not specified
            actual = JSGeneratorSettings_Accessor.GetCommandLineValue(key);
            Assert.AreEqual(string.Empty, actual);

            JSGeneratorSettings_Accessor.commandLineArgs = new string[] { "/v", "0" };
            actual = JSGeneratorSettings_Accessor.GetCommandLineValue(key);
            Assert.AreEqual("0", actual);
        }

       

        
    }
}
