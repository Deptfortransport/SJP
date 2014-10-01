using SJP.UserPortal.StateServer;
using SJP.Common.ServiceDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SJP.TestProject
{
    
    
    /// <summary>
    ///This is a test class for ISJPStateServerTest and is intended
    ///to contain all ISJPStateServerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ISJPStateServerTest
    {


        private TestContext testContextInstance;
        private string sessionID = "TestSessionID";
        private string key = "TestKey";
        private string keyValue = "TestKeyValue";

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
            SJPServiceDiscovery.ResetServiceDiscoveryForTest();

            SJPServiceDiscovery.Init(new TestInitialisation());
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        internal virtual ISJPStateServer CreateISJPStateServer()
        {
            ISJPStateServer target = new SJPStateServer("TestApp");
            return target;
        }

        internal virtual void DisposeISJPStateServer(ISJPStateServer target)
        {
            if (target != null)
            {
                SJPStateServer realTarget = (SJPStateServer)target;
                realTarget.Dispose();
            }
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void IDeleteTest()
        {
            ISJPStateServer target = CreateISJPStateServer();

            try
            {
                target.Save(sessionID, key, keyValue);

                target.Delete(sessionID, key);

                object actual = target.Read(sessionID, key);
                Assert.IsNull(actual, "Key not deleted");
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Unexpected exception returned: {0}", ex.ToString()));
            }
            finally
            {
                DisposeISJPStateServer(target);
            }
        }

        /// <summary>
        ///A test for Lock
        ///</summary>
        [TestMethod()]
        public void ILockTest()
        {
            ISJPStateServer target = CreateISJPStateServer();

            // Create a separate state server to check the key is locked
            ISJPStateServer otherServer = CreateISJPStateServer();

            try
            {
                target.Save(sessionID, key, keyValue);

                target.Lock(sessionID, new string[]{key});

                try
                {
                    otherServer.Read(sessionID, key);
                    Assert.Fail("Expected exception not raised");
                }
                catch (Exception ex)
                {
                    // Check it's the correct exception!
                    string message = ex.Message;
                    string combinedKey = sessionID + key;

                    if (!message.Equals(string.Format("Error reading object with key[{0}] from StateServer: Failed locking object with key[{0}].", combinedKey), StringComparison.InvariantCultureIgnoreCase))
                    {
                        // wrong exception
                        Assert.Fail(string.Format("Unepxected exception received: {0}", ex.ToString()));
                    }
                }

                // Remove the original server then check lock is lifted
                DisposeISJPStateServer(target);

                try
                {
                    otherServer.Read(sessionID, key);
                }
                catch (Exception ex)
                {
                    // Didn't want this exception
                    Assert.Fail(string.Format("Unexpected exception received: {0}", ex.ToString()));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Unexpected exception returned: {0}", ex.ToString()));
            }
            finally
            {
                DisposeISJPStateServer(target);
                DisposeISJPStateServer(otherServer);
            }
        }

        /// <summary>
        ///A test for Read
        ///</summary>
        [TestMethod()]
        public void IReadTest()
        {
            ISJPStateServer target = CreateISJPStateServer();

            try
            {
                target.Save(sessionID, key, keyValue);

                string actual;
                actual = (string)target.Read(sessionID, key);
                Assert.AreEqual(keyValue, actual, "Different key value returned");
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Unexpected exception returned: {0}", ex.ToString()));
            }
            finally
            {
                DisposeISJPStateServer(target);
            }
        }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void ISaveTest()
        {
            ISJPStateServer target = CreateISJPStateServer();

            try
            {
                target.Save(sessionID, key, keyValue);
            }
            catch (Exception ex)
            {
                Assert.Fail(string.Format("Unexpected exception received: {0}", ex.ToString()));
            }
            finally
            {
                DisposeISJPStateServer(target);
            }
        }
    }
}
