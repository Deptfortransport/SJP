// *********************************************** 
// NAME             : SJPVenueAccessStationTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for SJPVenueAccessStation
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SJP.Common.Web;
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for SJPVenueAccessStationTest and is intended
    ///to contain all SJPVenueAccessStationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPVenueAccessStationTest
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
        ///A test for SJPVenueAccessStation Constructor
        ///</summary>
        [TestMethod()]
        public void SJPVenueAccessStationConstructorTest()
        {
            string stationNaPTAN = "9100TST"; 
            string stationName = "Test";
            string transferDesciption = "Test Discription"; 
            SJPVenueAccessStation target = new SJPVenueAccessStation(stationNaPTAN, stationName);

            SJPVenueAccessStation target1 = new SJPVenueAccessStation();
            target1.StationName = stationName;
            target1.StationNaPTAN = stationNaPTAN;
            target1.AddTransferText(transferDesciption, Language.English, true);
            target1.AddTransferText(transferDesciption, Language.English, true); // Repeat for code coverage
            target1.AddTransferText(transferDesciption, Language.English, false);
            target1.AddTransferText(transferDesciption, Language.English, false); // Repeat for code coverage
            
            Assert.AreEqual(target1.StationName, target.StationName);
            Assert.AreEqual(target1.StationNaPTAN, target.StationNaPTAN);
            Assert.AreEqual(target1.GetTransferText(Language.English, true), transferDesciption);
            Assert.AreEqual(target1.GetTransferText(Language.English, false), transferDesciption);          

            // Update for code coverage
            Dictionary<Language, string> transferVenueText = new Dictionary<Language, string>();

            target1.TransferFromVenue = transferVenueText;
            target1.TransferToVenue = transferVenueText;

            // No text found
            Assert.IsTrue(string.IsNullOrEmpty(target1.GetTransferText(Language.English, true)));
            Assert.IsTrue(string.IsNullOrEmpty(target1.GetTransferText(Language.English, false)));
            
            transferVenueText.Add(Language.English, transferDesciption);
            transferVenueText.Add(Language.French, transferDesciption);

            target1.TransferFromVenue = transferVenueText;
            target1.TransferToVenue = transferVenueText;

            Assert.AreEqual(target1.GetTransferText(Language.English, true), transferDesciption);
            Assert.AreEqual(target1.GetTransferText(Language.English, false), transferDesciption);

            Assert.AreEqual(target1.TransferFromVenue, transferVenueText);
            Assert.AreEqual(target1.TransferToVenue, transferVenueText);
        }

        
    }
}
