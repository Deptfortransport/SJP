// *********************************************** 
// NAME             : TransitShuttleTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for TransitShuttle
// ************************************************
                
                
using SJP.Common.LocationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for TransitShuttleTest and is intended
    ///to contain all TransitShuttleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TransitShuttleTest
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
        ///A test for TransitShuttle Constructor
        ///</summary>
        [TestMethod()]
        public void TransitShuttleConstructorTest()
        {
            TransitShuttle target = new TransitShuttle();
            target.ID = "TID";
            target.IsPRMOnly = false;
            target.IsScheduledService = true;
            target.ModeOfTransport = ParkingInterchangeMode.Shuttlebus;
            target.ServiceEndTime = new TimeSpan(19, 45, 0);
            target.ServiceFrequency = 30;
            target.ServiceStartTime = new TimeSpan(7, 15, 0);
            target.ToVenue = true;
            target.TransitDuration = 45;
            target.TransferNotes = new System.Collections.Generic.Dictionary<Language, string>();
            target.AddTransferText("Notes1", Language.English);
            target.AddTransferText("Notes2", Language.English);
            target.AddTransferText("Notes3", Language.French);
            target.VenueGateToUse = "GW03";

            Assert.AreEqual("TID", target.ID);
            Assert.IsFalse(target.IsPRMOnly);
            Assert.IsTrue(target.IsScheduledService);
            Assert.AreEqual(ParkingInterchangeMode.Shuttlebus, target.ModeOfTransport);
            Assert.AreEqual(new TimeSpan(19, 45, 0), target.ServiceEndTime);
            Assert.AreEqual(new TimeSpan(7, 15, 0), target.ServiceStartTime);
            Assert.AreEqual(30, target.ServiceFrequency);
            Assert.IsTrue(target.ToVenue);
            Assert.AreEqual(45, target.TransitDuration);
            Assert.AreEqual("Notes2", target.GetTransferText(Language.English));
            Assert.AreEqual("Notes3", target.GetTransferText(Language.French));
            Assert.AreEqual(2, target.TransferNotes.Count);
            Assert.AreEqual("GW03", target.VenueGateToUse);
        }

       
    }
}
