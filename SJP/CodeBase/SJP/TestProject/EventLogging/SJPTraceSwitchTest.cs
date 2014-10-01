using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.PropertyManager;
using SJP.TestProject.EventLogging.MockObjects;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using SJP.Common;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for SJPTraceSwitchTest and is intended
    ///to contain all SJPTraceSwitchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SJPTraceSwitchTest
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
        ///A test for CheckLevel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.eventlogging.dll")]
        [ExpectedException(typeof(SJPException))]
        public void CheckLevelTest()
        {
            
            IEventPublisher[] customPublishers = new IEventPublisher[2];
            customPublishers[0] = new SJPPublisher1("CustomPublisher1");
            customPublishers[1] = new SJPPublisher2("CustomPublisher2");
            List<string> errors = new List<string>();
            bool expected = true;
            bool actual;

            SJPServiceDiscovery_Accessor.current = new SJPServiceDiscovery();

            MockPropertiesGoodServiceFactory factory = new MockPropertiesGoodServiceFactory(new MockPropertiesGood());
            SJPServiceDiscovery.Current.SetServiceForTest(ServiceDiscoveryKey.PropertyService, factory);

            MockPropertiesGood goodProperties = (MockPropertiesGood)Properties.Current;

            try
            {
                Trace.Listeners.Add(new SJPTraceListener(goodProperties, customPublishers, errors));

                actual = SJPTraceSwitch_Accessor.CheckLevel(SJPTraceLevel.Error);
                Assert.AreEqual(expected, actual);
                Assert.IsTrue(SJPTraceSwitch.TraceError);

                factory.MockLevelChange1();
                actual = SJPTraceSwitch_Accessor.CheckLevel(SJPTraceLevel.Off);
                Assert.AreEqual(expected, actual);
                Assert.IsFalse(SJPTraceSwitch.TraceError);

                factory.MockLevelChange2();
                actual = SJPTraceSwitch_Accessor.CheckLevel(SJPTraceLevel.Warning);
                Assert.AreEqual(expected, actual);
                Assert.IsTrue(SJPTraceSwitch.TraceWarning);

                factory.MockLevelChange3();
                actual = SJPTraceSwitch_Accessor.CheckLevel(SJPTraceLevel.Info);
                Assert.AreEqual(expected, actual);
                Assert.IsTrue(SJPTraceSwitch.TraceInfo);

                
            }
            catch (SJPException)
            {
                Assert.IsTrue(false);
            }

            SJPTraceSwitch_Accessor.currentLevel = SJPTraceLevel.Undefined;
            actual = SJPTraceSwitch_Accessor.CheckLevel(SJPTraceLevel.Info);
            Assert.AreEqual(expected, actual);
           
            
        }

       
    }
}
