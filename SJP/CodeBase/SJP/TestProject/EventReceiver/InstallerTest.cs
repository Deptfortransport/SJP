using SJP.Reporting.EventReceiver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject.EventReceiver
{
    
    
    /// <summary>
    ///This is a test class for InstallerTest and is intended
    ///to contain all InstallerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InstallerTest
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
        ///A test for Installer Constructor
        ///</summary>
        [TestMethod()]
        public void InstallerConstructorTest()
        {
            using (Installer target = new Installer())
            {
                Assert.IsNotNull(target, "Expected an object to be returned");
            }
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void DisposeTest()
        {
            // Inherited method
            Installer_Accessor target = new Installer_Accessor();
            bool disposing = false; 
            target.Dispose(disposing);
        }

        /// <summary>
        ///A test for InitializeComponent
        ///</summary>
        [TestMethod()]
        [DeploymentItem("EventReceiver.exe")]
        public void InitializeComponentTest()
        {
            // Inherited method
            Installer_Accessor target = new Installer_Accessor();
            target.InitializeComponent();
        }
    }
}
