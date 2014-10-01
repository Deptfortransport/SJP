using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SJP.TestProject.CommandAndControl
{
    [TestClass]
    public class SJPDashboardTest
    {
        [TestMethod]
        public void SJPDashboardWebsiteTest()
        {
            // NB1: Unfortunately code coverage will not pick this up as it tests the website directly

            // NB2: To run this test you will need to add an application to the default website that points at the SJPDashboard project

            // Call out using web classes
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost/SJPDashboard/Default.aspx");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader srResponse = new StreamReader(responseStream);
            string theResponse = srResponse.ReadToEnd();

            // Check for the titles
            if (!theResponse.Contains("SJP Dashboard"))
            {
                Assert.Fail("Dashboard main page not returned 1");
            }

            if (!theResponse.Contains("Latest Monitoring Data"))
            {
                Assert.Fail("Dashboard main page not returned 2");
            }

            if (!theResponse.Contains("Historical Monitoring Data"))
            {
                Assert.Fail("Dashboard main page not returned 3");
            }


            request = (HttpWebRequest)WebRequest.Create("http://localhost/SJPDashboard/SJPChecksumMonitoringResults/List.aspx");
            response = (HttpWebResponse)request.GetResponse();
            responseStream = response.GetResponseStream();
            srResponse = new StreamReader(responseStream);
            theResponse = srResponse.ReadToEnd();

            if (!theResponse.Contains("SJPChecksumMonitoringResults"))
            {
                Assert.Fail("SJPChecksumMonitoringResults page not returned");
            }


            request = (HttpWebRequest)WebRequest.Create("http://localhost/SJPDashboard/SJPDatabaseMonitoringResults/List.aspx");
            response = (HttpWebResponse)request.GetResponse();
            responseStream = response.GetResponseStream();
            srResponse = new StreamReader(responseStream);
            theResponse = srResponse.ReadToEnd();

            if (!theResponse.Contains("SJPDatabaseMonitoringResults"))
            {
                Assert.Fail("SJPDatabaseMonitoringResults page not returned");
            }


            request = (HttpWebRequest)WebRequest.Create("http://localhost/SJPDashboard/SJPFileMonitoringResults/List.aspx");
            response = (HttpWebResponse)request.GetResponse();
            responseStream = response.GetResponseStream();
            srResponse = new StreamReader(responseStream);
            theResponse = srResponse.ReadToEnd();

            if (!theResponse.Contains("SJPFileMonitoringResults"))
            {
                Assert.Fail("SJPFileMonitoringResults page not returned");
            }


            request = (HttpWebRequest)WebRequest.Create("http://localhost/SJPDashboard/SJPWMIMonitoringResults/List.aspx");
            response = (HttpWebResponse)request.GetResponse();
            responseStream = response.GetResponseStream();
            srResponse = new StreamReader(responseStream);
            theResponse = srResponse.ReadToEnd();

            if (!theResponse.Contains("SJPWMIMonitoringResults"))
            {
                Assert.Fail("SJPWMIMonitoringResults page not returned");
            }
        }
    }
}
