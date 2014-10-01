using SJP.Common.EventLogging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using SJP.Common;
using System.Security.Permissions;

namespace SJP.TestProject.EventLogging
{
    
    
    /// <summary>
    ///This is a test class for FilePublisherTest and is intended
    ///to contain all FilePublisherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FilePublisherTest
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
        ///A test for WriteEvent
        ///</summary>
        [TestMethod()]
        public void WriteEventTest()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = @".\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;

            // make a reference to a directory
            DirectoryInfo di = new DirectoryInfo(root);

            // create a subdirectory in the directory just created
            DirectoryInfo dis = di.CreateSubdirectory(testDirectoryName);

            // check to make sure that directory has been successfully
            // created before continuing
            DirectoryInfo testDirectory =
                new DirectoryInfo(root + testDirectoryName + "\\");
            Assert.AreEqual(true, testDirectory.Exists, "Could not create the test directory!");

            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            string baseFilepath = root + testDirectoryName + "\\" + "testfile";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

            OperationalEvent[] operationalEvents =
                new OperationalEvent[16];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");

            operationalEvents[1] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Info,
                "Test Error 2: A problem occurred whilst attempting to load G5265",
                "Test Target Number 2", "456");

            operationalEvents[2] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Warning,
                "Test Error 3: Connection XYF failed",
                "Test Target Number 3", "789");

            operationalEvents[3] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Verbose,
                "Test Error 4: Timeout occurred processing H6001.aspx",
                "Test Target Number 4", "012");

            operationalEvents[4] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Error,
                "Test Error 5: Error Number 1234HDJ has occurred",
                "Test Target Number 5", "345");

            operationalEvents[5] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Warning,
                "Test Error 6: Page Index.aspx failed to load",
                "Test Target Number 6", "678");

            operationalEvents[6] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Verbose,
                "Test Error 7: Server SG12323 has failed",
                "Test Target Number 7", "901");

            operationalEvents[7] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Info,
                "Test Error 8: Server SG123 failed to reboot",
                "Test Target Number 8", "234");

            operationalEvents[8] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Info,
                "Test Error 9: Component C123 malfunction",
                "Test Target Number 9", "567");

            operationalEvents[9] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Warning,
                "Test Error 10: Hard disk failure on SG134",
                "Test Target Number 10", "890");

            operationalEvents[10] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Warning,
                "Test Error 11: SG123 has failed to initialise",
                "Test Target Number 11", "123");

            operationalEvents[11] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Verbose,
                "Test Error 12: Database connection error on 'Journeys'",
                "Test Target Number 12", "456");

            operationalEvents[12] = new OperationalEvent(
                SJPEventCategory.Business,
                SJPTraceLevel.Info,
                "Test Error 13: Unknown Error",
                "Test Target Number 13", "789");

            operationalEvents[13] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Info,
                "Test Error 14: Error writing to audit trail",
                "Test Target Number 14", "012");

            operationalEvents[14] = new OperationalEvent(
                SJPEventCategory.ThirdParty,
                SJPTraceLevel.Warning,
                "Test Error 15: Driver D1 failed initialisation routine",
                "Test Target Number 15");

            operationalEvents[15] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Info,
                "Test Error 16: D123 failed",
                "Test Target Number 16");

            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }

            // create custom events
            CustomEventOne customEventOne = new CustomEventOne
                (SJPEventCategory.Business, SJPTraceLevel.Warning,
                "A custom event one message", Environment.UserName, 12345);

            CustomEventTwo customEventTwo = new CustomEventTwo
                (SJPEventCategory.ThirdParty, SJPTraceLevel.Error,
                "A custom event two message", Environment.UserName, 3343);

            // CustomEventOne has a file formatter defined, therefore
            // log event should be written in the format specified
            // in CustomEventOneFileFormatter
            filePublisher.WriteEvent(customEventOne);

            // CustomEventTwo has no file formatter defined, therefore
            // file should be written in the format specified
            // by the DefaultFormatter.
            filePublisher.WriteEvent(customEventTwo);

            // --------------------------------------------------

            // recreate the expected lines in the files and store
            // in a string array

            string head = "SJP-OP";
            string tab = "\t";
            string[] expectedLines = new String[18];
            OperationalEvent ope;

            for (int i = 0; i < 16; i++)
            {
                ope = operationalEvents[i];

                expectedLines[i] =
                    head + tab + ope.Time.ToString("yyyy-MM-ddTHH:mm:ss.fff") + tab + ope.Message + tab +
                    ope.Category + tab + ope.Level + tab + ope.MachineName + tab +
                    ope.TypeName + tab + ope.MethodName + tab +
                    ope.AssemblyName + tab + ope.Target;

                // check to see if session id exists and concat
                // if it does otherwise ignore
                if (ope.SessionId != OperationalEvent.SessionIdUnassigned)
                {
                    expectedLines[i] += tab + ope.SessionId;
                }
            }

            // add in the expected lines for the custom events

            // customEventOne
            expectedLines[16] =
                "SJP-CustomEventOne" + "\t" +
                customEventOne.Time + "\t" +
                customEventOne.Message + "\t" +
                customEventOne.Category + "\t" +
                customEventOne.Level + "\t" +
                customEventOne.ReferenceNumber;

            expectedLines[17] = String.Format(SJP.Common.EventLogging.Messages.DefaultFormatterOutput, customEventTwo.Time, customEventTwo.ClassName);


            // --------------------------------------------------

            // read the files that were created

            FileInfo[] fileInfoArray = testDirectory.GetFiles("*.txt");

            // Expect 4 files to be written
            Assert.AreEqual(4, fileInfoArray.Length, "Number of files created is incorrect.");

            // Open each file in turn, read each line and store in actualResults.
            // Open files in order of time created (default ordering of the array)

            string[] actualLines = new String[18];

            // read the first file
            FileInfo tempFile1 = fileInfoArray[0];
            using (FileStream fileStream1 = tempFile1.OpenRead())
            {
                StreamReader streamReader = new StreamReader(fileStream1);
                
                for (int i = 0; i < 5; i++)
                {
                    actualLines[i] = streamReader.ReadLine();
                }
               
            }
            

            // read the second file
            FileInfo tempFile2 = fileInfoArray[1];
            using (FileStream fileStream2 = tempFile2.OpenRead())
            {
                StreamReader streamReader = new StreamReader(fileStream2);
                
                for (int i = 5; i < 10; i++)
                {
                    actualLines[i] = streamReader.ReadLine();
                }
                
            }

           
            // read the third file
            FileInfo tempFile3 = fileInfoArray[2];
            using (FileStream fileStream3 = tempFile3.OpenRead())
            {
                StreamReader streamReader = new StreamReader(fileStream3);
                
                for (int i = 10; i < 15; i++)
                {
                    actualLines[i] = streamReader.ReadLine();
                }
                
            }

            
            // read the fourth (final) file
            FileInfo tempFile4 = fileInfoArray[3];
            using (FileStream fileStream4 = tempFile4.OpenRead())
            {
                StreamReader streamReader = new StreamReader(fileStream4);
                
                actualLines[15] = streamReader.ReadLine();

                // the two custom events
                actualLines[16] = streamReader.ReadLine();
                actualLines[17] = streamReader.ReadLine();
                
            }

            // --------------------------------------------------

            // perform the assertions

            for (int i = 0; i < 18; i++)
            {
                Assert.AreEqual(expectedLines[i], actualLines[i], "Line " + (i + 1) + " does not match.");
            }

            //-----------------------------------------------

        }

        /// <summary>
        ///A test for WriteEvent with UnauthorizedAccessException exception gets raised
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestUnauthorizedAccessException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = string.Empty; // +@"\FilePublisherTests\";
            string testDirectoryName = Environment.SystemDirectory; //"TestFilePublisher-" + currentTime;
                       

            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            string baseFilepath = root + testDirectoryName + "\\" + "testfile";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

            OperationalEvent[] operationalEvents =
                new OperationalEvent[16];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");


            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }
        }

        /// <summary>
        ///A test for WriteEvent with DirectoryNotFoundException exception gets raised
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestDirectoryNotFoundException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root =  @"\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;
            
            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            string baseFilepath = root + testDirectoryName + "\\" + "testfile";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

            OperationalEvent[] operationalEvents =
                new OperationalEvent[16];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");


            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }
        }

        /// <summary>
        ///A test for WriteEvent with PathTooLongException exception gets raised
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestPathTooLongException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = @".\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;

            // make a reference to a directory
            DirectoryInfo di = new DirectoryInfo(root);

            // create a subdirectory in the directory just created
            DirectoryInfo dis = di.CreateSubdirectory(testDirectoryName);

            // check to make sure that directory has been successfully
            // created before continuing
            DirectoryInfo testDirectory =
                new DirectoryInfo(root + testDirectoryName + "\\");
            Assert.AreEqual(true, testDirectory.Exists, "Could not create the test directory!");

            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            // Creating a big file name 
            // Full paths must not exceed 260 characters to maintain compatibility with Windows operating systems
            // This should generate pathtoolong exception
            string baseFilepath = root + testDirectoryName + "\\" + "testfile1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabceefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabceefghijklmnopqrstuvwxyz";

            for(int i=0;i < 10; i++)
            {
                baseFilepath += "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabceefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabceefghijklmnopqrstuvwxyz";
            }

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

            OperationalEvent[] operationalEvents =
                new OperationalEvent[16];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");


            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }
        }

        /// <summary>
        ///A test for WriteEvent with exception gets raised which is not handled using specific exception catch blocks
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = @".\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;

            // make a reference to a directory
            DirectoryInfo di = new DirectoryInfo(root);

            // create a subdirectory in the directory just created
            DirectoryInfo dis = di.CreateSubdirectory(testDirectoryName);

            // check to make sure that directory has been successfully
            // created before continuing
            DirectoryInfo testDirectory =
                new DirectoryInfo(root + testDirectoryName + "\\");
            Assert.AreEqual(true, testDirectory.Exists, "Could not create the test directory!");

            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            // Creating a big file name 
            // Full paths must not exceed 260 characters to maintain compatibility with Windows operating systems
            // This should generate pathtoolong exception
            string baseFilepath = root + testDirectoryName + "\\" + "testfile1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabceefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabceefghijklmnopqrstuvwxyz";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

            OperationalEvent[] operationalEvents =
                new OperationalEvent[16];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");


            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }
        }

        /// <summary>
        ///A test for WriteEvent with ArgumentException exception gets raised
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestArgumentException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = @"\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;

            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            string baseFilepath = root + testDirectoryName + "\\" + "testfile";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

            FilePublisher_Accessor accessor = new FilePublisher_Accessor(new PrivateObject(filePublisher));

            OperationalEvent[] operationalEvents =
                new OperationalEvent[1];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");

            accessor.rotationFilepath = string.Empty;

            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }
        }

        /// <summary>
        ///A test for WriteEvent with ArgumentNullException exception gets raised
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestArgumentNullException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = @"\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;

            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            string baseFilepath = root + testDirectoryName + "\\" + "testfile";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

            FilePublisher_Accessor accessor = new FilePublisher_Accessor(new PrivateObject(filePublisher));

            OperationalEvent[] operationalEvents =
                new OperationalEvent[1];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");

            accessor.rotationFilepath = null;

            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }
        }


        /// <summary>
        ///A test for WriteEvent with SecurityException exception gets raised
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestSecurityException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = @".\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;

            // make a reference to a directory
            DirectoryInfo di = new DirectoryInfo(root);

            // create a subdirectory in the directory just created
            DirectoryInfo dis = di.CreateSubdirectory(testDirectoryName);

            // check to make sure that directory has been successfully
            // created before continuing
            DirectoryInfo testDirectory =
                new DirectoryInfo(root + testDirectoryName + "\\");
            Assert.AreEqual(true, testDirectory.Exists, "Could not create the test directory!");
            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            string baseFilepath = root + testDirectoryName + "\\" + "testfile";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);

           
            OperationalEvent[] operationalEvents =
                new OperationalEvent[1];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");

            FilePublisher_Accessor accessor = new FilePublisher_Accessor(new PrivateObject(filePublisher));

            FileInfo finfo = new FileInfo(accessor.rotationFilepath);

            // Creates declarative security to disable access to the publisher file.
            FileIOPermission filePermission = new FileIOPermission(FileIOPermissionAccess.NoAccess, finfo.FullName);
            filePermission.PermitOnly();

            // call the write event procedure to write
            // the events to the file

            foreach (OperationalEvent oe in operationalEvents)
            {
                filePublisher.WriteEvent(oe);
            }
        }

        /// <summary>
        ///A test for WriteEvent with IOException exception gets raised
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void WriteEventTestIOException()
        {
            // --------------------------------------------------
            // set-up a directory for the test files

            string currentTime = DateTime.Now.ToString
                ("yyMMddHHmmssff", System.Globalization.CultureInfo.InvariantCulture);
            string root = @".\FilePublisherTests\";
            string testDirectoryName = "TestFilePublisher-" + currentTime;

            // make a reference to a directory
            DirectoryInfo di = new DirectoryInfo(root);

            // create a subdirectory in the directory just created
            DirectoryInfo dis = di.CreateSubdirectory(testDirectoryName);

            // check to make sure that directory has been successfully
            // created before continuing
            DirectoryInfo testDirectory =
                new DirectoryInfo(root + testDirectoryName + "\\");
            Assert.AreEqual(true, testDirectory.Exists, "Could not create the test directory!");
            // --------------------------------------------------

            // create 16 operational events and write these to files
            // in the specified directory.

            string identifier = "Identifier123";
            int rotation = 5;
            string baseFilepath = root + testDirectoryName + "\\" + "testfile";

            FilePublisher filePublisher = new FilePublisher
                (identifier, rotation, baseFilepath);


            OperationalEvent[] operationalEvents =
                new OperationalEvent[1];

            operationalEvents[0] = new OperationalEvent(
                SJPEventCategory.Database,
                SJPTraceLevel.Error,
                "Test Error 1: There was a connection error with Database 123XY",
                "Test Target Number 1", "123");

            FilePublisher_Accessor accessor = new FilePublisher_Accessor(new PrivateObject(filePublisher));

            // opening a stream to restrict access to the file
            using (FileStream stream = new FileStream(accessor.rotationFilepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {

                // call the write event procedure to write
                // the events to the file

                foreach (OperationalEvent oe in operationalEvents)
                {
                    filePublisher.WriteEvent(oe);
                }
            }
        }

        
    }
}
