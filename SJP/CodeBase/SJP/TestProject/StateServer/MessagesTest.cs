using SJP.UserPortal.StateServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SJP.TestProject
{


    /// <summary>
    ///This is a test class for ISJPStateServerTest and is intended
    ///to contain all ISJPStateServerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StateServerMessagesTest
    {

        /// <summary>
        ///A test for Messages
        ///</summary>
        [TestMethod()]
        public void MessagesTest()
        {
            Messages messages = new Messages();

            Assert.IsNotNull(messages, "No object returned");
        }
    }
}
