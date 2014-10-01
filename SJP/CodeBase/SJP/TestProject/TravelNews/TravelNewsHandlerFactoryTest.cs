// *********************************************** 
// NAME             : TravelNewsHandlerFactoryTest.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 02 May 2011
// DESCRIPTION  	: TravelNewsHandlerFactoryTest test
// ************************************************
// 
                
using SJP.UserPortal.TravelNews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for TravelNewsHandlerFactoryTest and is intended
    ///to contain all TravelNewsHandlerFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TravelNewsHandlerFactoryTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
            SJPServiceDiscovery.Init(new TestInitialisation());
        }
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
        ///A test for TravelNewsHandlerFactory Constructor
        ///</summary>
        [TestMethod()]
        public void TravelNewsHandlerFactoryConstructorTest()
        {
            TravelNewsHandlerFactory target = new TravelNewsHandlerFactory();

            Assert.IsNotNull(target, "Expected TravelNewsHandlerFactory to be not null");
        }

        /// <summary>
        ///A test for DataChangedNotificationReceived
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.travelnews.dll")]
        public void TravelNewsDataChangedNotificationReceivedTest()
        {
            TravelNewsHandlerFactory_Accessor target = new TravelNewsHandlerFactory_Accessor();
            object sender = null;
            ChangedEventArgs e = new ChangedEventArgs("TravelNews");

            try
            {
                target.DataChangedNotificationReceived(sender, e);
            }
            catch (Exception ex)
            {
                Assert.Fail(
                    string.Format("Exception was thrown when performing the DataChangedNotificationReceivedTest, exception: {0}", ex));
            }
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        public void TravelNewsGetTest()
        {
            TravelNewsHandlerFactory target = new TravelNewsHandlerFactory();
            
            object actual = target.Get();

            Assert.IsTrue(actual is TravelNewsHandler, "Expected TravelNewsHandler to be returned");
        }

        /// <summary>
        ///A test for RegisterForChangeNotification
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.travelnews.dll")]
        public void TravelNewsRegisterForChangeNotificationTest()
        {
            TravelNewsHandlerFactory_Accessor target = new TravelNewsHandlerFactory_Accessor();

            try
            {
                bool actual = target.RegisterForChangeNotification();

                Assert.IsTrue(actual, "RegisterForChangeNotification failed");
            }
            catch (Exception ex)
            {
                Assert.Fail(
                    string.Format("Exception was thrown when performing the RegisterForChangeNotification, exception: {0}", ex));
            }
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.userportal.travelnews.dll")]
        public void TravelNewsUpdateTest()
        {
            TravelNewsHandlerFactory_Accessor target = new TravelNewsHandlerFactory_Accessor();

            try
            {
                target.Update();
            }
            catch (Exception ex)
            {
                Assert.Fail(
                    string.Format("Exception was thrown when performing the TravelNews Update, exception: {0}", ex));
            }
        }
    }
}
