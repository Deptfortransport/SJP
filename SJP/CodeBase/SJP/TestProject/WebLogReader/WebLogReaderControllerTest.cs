// *********************************************** 
// NAME             : WebLogReaderControllerTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 24 Jun 2011
// DESCRIPTION  	: WebLogReaderController unit tests
// ************************************************
                
                
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJP.Common.PropertyManager;
using SJP.Common.ServiceDiscovery;
using SJP.Reporting.WebLogReader;

namespace SJP.TestProject.WebLogReader
{
    
    
    /// <summary>
    ///This is a test class for WebLogReaderControllerTest and is intended
    ///to contain all WebLogReaderControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WebLogReaderControllerTest
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
        ///A test for ArchiveWebLog
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void ArchiveWebLogTest()
        {
            ResetLogFiles();
            PrivateObject param0 = new PrivateObject(new WebLogReaderController(Properties.Current));
            WebLogReaderController_Accessor target = new WebLogReaderController_Accessor(param0); 
            string fileName = "sample.txt";
            string logDir = Properties.Current["WebLogReader.W3SVC1.LogDirectory"];
            string archiveDir = Properties.Current["WebLogReader.W3SVC1.ArchiveDirectory"];

            // create the file in the logDir
            FileInfo fi = new FileInfo(logDir + "//" + fileName);

            using (StreamWriter sw = fi.CreateText()) { };

            target.ArchiveWebLog(fi.FullName, logDir, archiveDir);


            Assert.IsTrue(File.Exists(archiveDir + "//" + fileName));
            Assert.IsFalse(File.Exists(logDir + "//" + fileName));

        }

        
        /// <summary>
        ///A test for GetLogFileNames
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void GetLogFileNamesTest()
        {
            ResetLogFiles();
            PrivateObject param0 = new PrivateObject(new WebLogReaderController(Properties.Current));
            WebLogReaderController_Accessor target = new WebLogReaderController_Accessor(param0);
            string logDir = Properties.Current["WebLogReader.W3SVC1.LogDirectory"];

            // In real run the log files gets copied to another location first and then renamed to be without "u_" at start
            string fileCurrentHourName = logDir + "\\" + string.Format("u_ex{0}.log", DateTime.Now.AddHours(-1).ToString("yyMMddHH"));

            string fileNextHourName = logDir + "\\" + string.Format("u_ex{0}.log", DateTime.Now.ToString("yyMMddHH"));

            using (FileStream fr = File.Create(fileCurrentHourName)) { }

            using (FileStream fr = File.Create(fileNextHourName)) { }

            ArrayList actual;
            actual = target.GetLogFileNames(logDir);
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains("weblogreader/W3SVC1\\u_ex11042613.log"));
            Assert.IsTrue(actual.Contains("weblogreader/W3SVC1\\u_ex11042713.log"));

            Assert.IsTrue(actual.Contains(fileCurrentHourName));

            Assert.IsFalse(actual.Contains(fileNextHourName));
        }

        /// <summary>
        ///A test for GetStatusCodes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.reporting.weblogreader.exe")]
        public void GetStatusCodesTest()
        {
            ResetLogFiles();
            PrivateObject param0 = new PrivateObject(new WebLogReaderController(Properties.Current));
            WebLogReaderController_Accessor target = new WebLogReaderController_Accessor(param0);

            Properties_Accessor accessor = new Properties_Accessor(new PrivateObject(Properties.Current));

            // More than one range specified
            List<StatusCode> actual;
            actual = target.GetStatusCodes();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(100, actual[0].MinProtocolStatusCode);
            Assert.AreEqual(399, actual[0].MaxProtocolStatusCode);
            Assert.AreEqual(500, actual[1].MinProtocolStatusCode);
            Assert.AreEqual(599, actual[1].MaxProtocolStatusCode);

            // More than one range specified - second range values are empty
            accessor.propertyDictionary["WebLogReader.ValidStatusCode.RANGE2.Max"] = "";
            accessor.propertyDictionary["WebLogReader.ValidStatusCode.RANGE2.Min"] = "";
            actual = target.GetStatusCodes();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(100, actual[0].MinProtocolStatusCode);
            Assert.AreEqual(399, actual[0].MaxProtocolStatusCode);
            Assert.AreEqual(100, actual[1].MinProtocolStatusCode);
            Assert.AreEqual(599, actual[1].MaxProtocolStatusCode);


            // Single range specified
            accessor.propertyDictionary["WebLogReader.ValidStatusCode.Ranges"] = "RANGE1";
            actual = target.GetStatusCodes();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(100, actual[0].MinProtocolStatusCode);
            Assert.AreEqual(399, actual[0].MaxProtocolStatusCode);
        }

        /// <summary>
        ///A test for Run
        ///</summary>
        [TestMethod()]
        public void RunTest()
        {
            ResetLogFiles();
            string logDir = Properties.Current["WebLogReader.W3SVC1.LogDirectory"];

            string fileCurrentHourName = logDir + "\\" + string.Format("ex{0}.log", DateTime.Now.AddHours(-1).ToString("yyMMddHH"));

            string fileNextHourName = logDir + "\\" + string.Format("ex{0}.log", DateTime.Now.ToString("yyMMddHH"));

            using (FileStream fr = File.Create(fileCurrentHourName)) 
            {
                using (StreamWriter sw = new StreamWriter(fr))
                {
                    sw.WriteLine("#Software: Microsoft Internet Information Services 7.5");
                    sw.WriteLine("#Version: 1.0");
                    sw.WriteLine("#Date: 2011-04-26 13:02:40");
                    sw.WriteLine("#Fields: date time s-sitename s-computername s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs-version cs(User-Agent) cs(Cookie) cs(Referer) cs-host sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken");
                    sw.WriteLine("2011-04-26 13:02:40 W3SVC1 OVS3 10.96.38.83 GET /SJPWeb/Version/London2012/images/icons/header-drop-down-icon-over.gif - 80 - 10.93.154.10 HTTP/1.1 Mozilla/5.0+(Macintosh;+Intel+Mac+OS+X+10.5;+rv:2.0)+Gecko/20100101+Firefox/4.0 ASP.NET_SessionId=v2vhrh45pwy2qwbesv231m3l;+__utma=72248141.1379406566.1303820142.1303820142.1303820142.1;+__utmc=72248141;+__utmz=72248141.1303820142.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none);+SJP=O=NG9+1LA&TYO=Postcode&D=8100NGA&TYD=Venue&DTO=20110426T1230Z&DTR=20110426T1600Z&R=False&PM=PublicTransport&UID=v2vhrh45pwy2qwbesv231m3l&LP=JourneyOptions&DT=20110426T1226Z http://ovs3/SJPWeb/Version/London2012/Styles/main.css ovs3 200 0 0 1236 841 171");
                }
            }

            using (FileStream fr = File.Create(fileNextHourName))
            {
                using (StreamWriter sw = new StreamWriter(fr))
                {
                    sw.WriteLine("#Software: Microsoft Internet Information Services 7.5");
                    sw.WriteLine("#Version: 1.0");
                    sw.WriteLine("#Date: 2011-04-26 13:02:40");
                    sw.WriteLine("#Fields: date time s-sitename s-computername s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs-version cs(User-Agent) cs(Cookie) cs(Referer) cs-host sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken");
                    sw.WriteLine("2011-04-26 13:02:40 W3SVC1 OVS3 10.96.38.83 GET /SJPWeb/Version/London2012/images/icons/header-drop-down-icon-over.gif - 80 - 10.93.154.10 HTTP/1.1 Mozilla/5.0+(Macintosh;+Intel+Mac+OS+X+10.5;+rv:2.0)+Gecko/20100101+Firefox/4.0 ASP.NET_SessionId=v2vhrh45pwy2qwbesv231m3l;+__utma=72248141.1379406566.1303820142.1303820142.1303820142.1;+__utmc=72248141;+__utmz=72248141.1303820142.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none);+SJP=O=NG9+1LA&TYO=Postcode&D=8100NGA&TYD=Venue&DTO=20110426T1230Z&DTR=20110426T1600Z&R=False&PM=PublicTransport&UID=v2vhrh45pwy2qwbesv231m3l&LP=JourneyOptions&DT=20110426T1226Z http://ovs3/SJPWeb/Version/London2012/Styles/main.css ovs3 200 0 0 1236 841 171");
                }
            }

            WebLogReaderController target = new WebLogReaderController(Properties.Current);
            int expected = 0;
            int actual;
            actual = target.Run();
            Assert.AreEqual(expected, actual);
            
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
