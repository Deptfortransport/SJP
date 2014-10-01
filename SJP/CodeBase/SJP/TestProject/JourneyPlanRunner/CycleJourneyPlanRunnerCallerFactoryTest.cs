using SJP.UserPortal.JourneyPlanRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.ServiceDiscovery;
using SJP.Common;
using JPR = SJP.UserPortal.JourneyPlanRunner;

using Logger = System.Diagnostics.Trace;
using SJP.UserPortal.JourneyControl;

namespace SJP.TestProject.JourneyPlanRunner
{
    
    
    /// <summary>
    ///This is a test class for CycleJourneyPlanRunnerCallerFactoryTest and is intended
    ///to contain all CycleJourneyPlanRunnerCallerFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CycleJourneyPlanRunnerCallerFactoryTest
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

                       
        /// <summary>
        ///A test for Get method 
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            CycleJourneyPlanRunnerCallerFactory target = new CycleJourneyPlanRunnerCallerFactory();
            object actual;
            actual = target.Get();
            Assert.IsInstanceOfType(actual, typeof(CycleJourneyPlanRunnerCaller));
           
        }
               
    }
}
