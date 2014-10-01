using SJP.Common.HashGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HashGeneratorProgramTest
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
        ///A test for Main
        ///</summary>
        public void HashGeneratorMainTest()
        {
            // Test no args supplied (receive help back)
            using (StringWriter sw = new StringWriter()) 
            { 
                Console.SetOut(sw);

                string[] args = new string[0]; 
                Program_Accessor.Main(args);
                Assert.IsTrue(sw.ToString().Contains("Usage:"), "Didn't receive help back when supplying no paramteres");
            }

            // Test /help args supplied (receive help back)
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                string[] args = new string[1]{"/help"};
                Program_Accessor.Main(args);
                Assert.IsTrue(sw.ToString().Contains("Usage:"), "Didn't receive help back when using /help parameter");
            }

            // Test bad algorithm arg supplied (receive help back)
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                string[] args = new string[1] { "/algorithm" };
                Program_Accessor.Main(args);
                Assert.IsTrue(sw.ToString().Contains("Usage:"), "Didn't receive help back when using bad /algorithm parameter");
            }

            // Test unexpected arg supplied (receive help back)
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                string[] args = new string[1] { "/doh:test" };
                Program_Accessor.Main(args);
                Assert.IsTrue(sw.ToString().Contains("Usage:"), "Didn't receive help back when using unexpected parameter");
            }

            // Test good algorithm arg supplied followed by text arg (receive help back)
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                string[] args = new string[2] { "/algorithm:rsa", "text" };
                Program_Accessor.Main(args);
                Assert.IsTrue(sw.ToString().Contains("Usage:"), "Didn't receive help back when using random text parameter");
            }

            // Test root directory arg and good algorithm arg supplied - unfortunately this throws an exception 
            // as it tries to read console input however that is the last line of the program so indicates success
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                string[] args = new string[2] { @"D:\SJP\CodeBase\SJP\Initialisation", "/algorithm|rsa" };

                try
                {
                    Program_Accessor.Main(args);
                    Assert.Fail("Expected exception");
                }
                catch (Exception ex)
                {
                    if (ex.GetType().Equals(typeof(System.InvalidOperationException)))
                    {
                        if (ex.Message == "Cannot read keys when either application does not have a console or when console input has been redirected from a file. Try Console.Read.")
                        {
                            // success, unless error was returned to console
                            Assert.IsFalse(sw.ToString().Contains("Error - unable to verify files:"), "Errored generating hash file");
                        }
                        else
                        {
                            Assert.Fail("Exception thrown: {0}", new string[1] { ex.ToString() });
                        }
                    }
                    else
                    {
                        Assert.Fail("Exception thrown: {0}", new string[1] { ex.ToString() });
                    }
                }
            }
        }

        /// <summary>
        ///A test for displayHelp
        ///</summary>
        [TestMethod()]
        [DeploymentItem("WebSupportHashGenerator.exe")]
        public void HashGeneratorDisplayHelpTest()
        {
            Program_Accessor target = new Program_Accessor(); 
            try
            {
                // Test root directory arg and good algorithm arg supplied 
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);
                    target.displayHelp();

                    Assert.IsTrue(sw.ToString().Contains("Usage:"), "Didn't output help content");
                }
                Assert.IsTrue(true, "Help displayed without error");
            }
            catch(Exception ex)
            {
                Assert.Fail("Display help errored: {0}", new string[]{ex.ToString()});
            }
        }
    }
}
