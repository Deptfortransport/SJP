// *********************************************** 
// NAME             : W3CWebLogReaderTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: W3CWebLogReader unit tests
// ************************************************
                
                
using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.WebLogReader;

namespace SJP.TestProject.WebLogReader
{
    
    
    /// <summary>
    ///This is a test class for W3CWebLogReaderTest and is intended
    ///to contain all W3CWebLogReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class W3CWebLogReaderTest
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
            

            WebLogReaderInitialisation initialisation = new WebLogReaderInitialisation();

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

            SJPServiceDiscovery.Init(initialisation);

            ResetLogFiles();
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
        ///A test for GetActiveWebLogFileNames
        ///</summary>
        [TestMethod()]
        public void GetActiveWebLogFileNamesTest()
        {
            W3CWebLogReader target = new W3CWebLogReader();
            string[] actual;
            actual = target.GetActiveWebLogFileNames();

            string logDir = Properties.Current["WebLogReader.W3SVC1.LogDirectory"];

            string fileCurrentHourName = string.Format("ex{0}.log", DateTime.Now.AddHours(-1).ToString("yyMMddHH"));

            string fileNextHourName = string.Format("ex{0}.log", DateTime.Now.ToString("yyMMddHH"));

            string fileCurrentDayName = string.Format("ex{0}.log", DateTime.Now.AddHours(-1).ToString("yyMMdd"));

            Assert.AreEqual(2, actual.Length);
            Assert.IsTrue(actual.Contains(fileCurrentHourName));
            Assert.IsTrue(actual.Contains(fileNextHourName));

            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));

            // Rotation set to daily
            accessor.propertyDictionary[Keys.WebLogReaderRolloverPeriod] = "Daily";
            actual = target.GetActiveWebLogFileNames();
            Assert.AreEqual(2, actual.Length); // the count is still two as the other file is null
            Assert.IsTrue(actual.Contains(fileCurrentDayName));
        }

        /// <summary>
        ///A test for GetFieldNames
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void GetFieldNamesTest()
        {
            W3CWebLogReader logReader = new W3CWebLogReader();
            
            W3CWebLogReader_Accessor target = new W3CWebLogReader_Accessor(new PrivateObject(logReader));
            string webLogLine = "2011-04-27 13:17:37 W3SVC1 OVS3 ::1 GET /SJPWeb/Pages/JourneyPlannerInput.aspx";
            char splitOn = ' ';
            string[] actual;
            actual = target.GetFieldNames(webLogLine, splitOn);
            // first field gets ignored
            Assert.AreEqual(6, actual.Length);
            Assert.IsTrue(actual.Contains("W3SVC1"));
            Assert.IsTrue(actual.Contains("OVS3"));
            Assert.IsTrue(actual.Contains("GET"));
            Assert.IsTrue(actual.Contains("/SJPWeb/Pages/JourneyPlannerInput.aspx"));
        }

        /// <summary>
        ///A test for GetFieldPositions
        /// As the actual fiels differ from the expected fields - exception gets thrown
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        [ExpectedException(typeof(SJPException))]
        public void GetFieldPositionsTest()
        {
            W3CWebLogReader logReader = new W3CWebLogReader();

            W3CWebLogReader_Accessor target = new W3CWebLogReader_Accessor(new PrivateObject(logReader));
            string[] actualFields = new string[]{"a","b","c"};
            string[] expectedFields = new string[] { "c", "d", "e" };
            int[] fieldPositions = new int[3];
            // as the actual fiels differ from the expected fields - exception gets thrown
            target.GetFieldPositions(actualFields, expectedFields, fieldPositions);
            
        }

        /// <summary>
        ///A test for ProcessWorkload
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void ProcessWorkloadTest()
        {
            W3CWebLogReader target = new W3CWebLogReader();
            string filePath = "WebLogReader/W3SVC1/u_ex11042613.log";
            WebLogDataSpecification dataSpecification = new WebLogDataSpecification();
            int expected = 0;
            int actual;
            actual = target.ProcessWorkload(filePath, dataSpecification);
            Assert.AreEqual(expected, actual);

            // Filepath is null
            filePath = null;
            actual = target.ProcessWorkload(filePath, dataSpecification);
            Assert.IsTrue(actual>0);
        }

        /// <summary>
        ///A test for HourlyRotation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void HourlyRotationTest()
        {
            W3CWebLogReader logReader = new W3CWebLogReader();

            W3CWebLogReader_Accessor target = new W3CWebLogReader_Accessor(new PrivateObject(logReader));
            bool actual;
            actual = target.HourlyRotation;
            Assert.IsTrue(actual);

            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));
            
            // Rotation set to daily
            accessor.propertyDictionary[Keys.WebLogReaderRolloverPeriod] = "Daily";
            actual = target.HourlyRotation;
            Assert.IsFalse(actual);

            // Rotation set to no value -- in this case rollover period is hourly
            accessor.propertyDictionary[Keys.WebLogReaderRolloverPeriod] = "";
            actual = target.HourlyRotation;
            Assert.IsTrue(actual);

            // Rotation set to invalid value -- in this case rollover period is hourly
            accessor.propertyDictionary[Keys.WebLogReaderRolloverPeriod] = "Invalid";
            actual = target.HourlyRotation;
            Assert.IsTrue(actual);

        }

        /// <summary>
        ///A test for UseLocalTime
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void UseLocalTimeTest()
        {
            W3CWebLogReader logReader = new W3CWebLogReader();

            W3CWebLogReader_Accessor target = new W3CWebLogReader_Accessor(new PrivateObject(logReader));
            bool actual;
            actual = target.UseLocalTime;
            Assert.IsFalse(actual);

            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));

            // the WebLogReaderUseLocalTime property set to no value 
            accessor.propertyDictionary[Keys.WebLogReaderUseLocalTime] = "";
            actual = target.UseLocalTime;
            Assert.IsFalse(actual);

            // the WebLogReaderUseLocalTime property set to true
            accessor.propertyDictionary[Keys.WebLogReaderUseLocalTime] = "true";
            actual = target.UseLocalTime;
            Assert.IsTrue(actual);

            // the WebLogReaderUseLocalTime property set to invalid value 
            accessor.propertyDictionary[Keys.WebLogReaderUseLocalTime] = "abc";
            actual = target.UseLocalTime;
            Assert.IsFalse(actual);
        }

        #region Private Helper Methods
        // Resets the log files to be in the log directory rather than archive directory
        private void ResetLogFiles()
        {
            string logDir = Properties.Current["WebLogReader.W3SVC1.LogDirectory"];
            string archiveDir = Properties.Current["WebLogReader.W3SVC1.ArchiveDirectory"];

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
