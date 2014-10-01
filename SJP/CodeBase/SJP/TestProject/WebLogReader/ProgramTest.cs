// *********************************************** 
// NAME             : ProgramTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: Unit test for Program class of WebLogReader
// ************************************************


using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.WebLogReader;

namespace SJP.TestProject.WebLogReader
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
            ResetLogFiles();
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
        //
        #endregion


        
        /// <summary>
        ///A test for Main
        ///</summary>
        [TestMethod()]
        public void MainTest()
        {
            // args passed for help
            string[] args = new string[]{"/help"};
            int expected = 0; 
            int actual;
            actual = Program.Main(args);
            Assert.AreEqual(expected, actual);

            // args passed as test
            args = new string[] { "/test" };
            expected = 0;
            actual = Program.Main(args);
            Assert.AreEqual(expected, actual);

            // run the weblog reader - no args supplied
            args = new string[] { };
            expected = 0;
            actual = Program.Main(args);
            Assert.AreEqual(expected, actual);
            
        }

        ///// <summary>
        /////A test for RunReader
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("sjp.reporting.weblogreader.exe")]
        //[ExpectedException(typeof(SJPException))]
        //public void RunReaderTest()
        //{
        //    WebLogReaderInitialisation initialisation = new WebLogReaderInitialisation();

        //    SJPServiceDiscovery.Init(initialisation);

        //    Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));

        //    int actual;
        //    actual = Program_Accessor.RunReader();
           
        //}


        #region Private Helper Methods
        // Resets the log files to be in the log directory rather than archive directory
        private void ResetLogFiles()
        {
            string logDir = "weblogreader/W3SVC1";
            string archiveDir = "weblogreader/W3SVC1/Archive";

            DirectoryInfo logDirInfo = new DirectoryInfo(logDir);
            DirectoryInfo archiveDirInfo = new DirectoryInfo(archiveDir);
            foreach (FileInfo file in archiveDirInfo.GetFiles())
            {
                file.MoveTo(logDirInfo + "//" + file.Name);
            }
        }

        #endregion
    }
}
