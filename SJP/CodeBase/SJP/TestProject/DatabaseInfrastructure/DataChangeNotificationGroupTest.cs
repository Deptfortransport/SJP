// *********************************************** 
// NAME             : DataChangeNotificationGroupTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 20 Jun 2011
// DESCRIPTION  	: Unit tests for DataChangeNotificationGroup class
// ************************************************
                
                
using SJP.Common.DatabaseInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SJP.TestProject.DatabaseInfrastructure
{
    
    
    /// <summary>
    ///This is a test class for DataChangeNotificationGroupTest and is intended
    ///to contain all DataChangeNotificationGroupTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataChangeNotificationGroupTest
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

        #region Test Methods
        /// <summary>
        ///A test for IsAffected
        ///</summary>
        [TestMethod()]
        public void IsAffectedTest()
        {
            string groupId = "testGroup";
            SqlHelperDatabase currDatabase = SqlHelperDatabase.DefaultDB;
            string[] currTables = new string[] {"A","B","C"}; 
            DataChangeNotificationGroup target = new DataChangeNotificationGroup(groupId, currDatabase, currTables); 
            target.RaiseEvent = false;
            Assert.IsFalse(target.RaiseEvent);

            Assert.AreEqual(groupId, target.GroupId);
            foreach(string table in target.Tables)
            {
                Assert.IsTrue(currTables.Contains(table));
            }
            Assert.AreEqual(target.DataBase, currDatabase);
           

            // Database and table name is a match
            bool expected = true; 
            bool actual;
            actual = target.IsAffected(currDatabase, "A");
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(target.RaiseEvent);
            
            // Database is a match but not the table
            expected = false; 
            actual = target.IsAffected(currDatabase, "D");
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(target.RaiseEvent);

            // Table is a match but not the database
            expected = false; 
            actual = target.IsAffected(SqlHelperDatabase.ContentDB, "B");
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(target.RaiseEvent);

        }
        #endregion


    }
}
