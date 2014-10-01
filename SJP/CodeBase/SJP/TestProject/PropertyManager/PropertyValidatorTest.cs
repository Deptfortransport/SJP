// *********************************************** 
// NAME             : PropertyValidatorTest.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 18 Feb 2011
// DESCRIPTION  	: Unit test for PropertyValidator abstract class
// ************************************************
                
                
using SJP.Common.PropertyManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Collections.Generic;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common;
using SJP.Common.PropertyManager.PropertyProviders;
using System.Configuration;
using SJP.TestProject.PropertyManager;
using SJP.Common.ServiceDiscovery;

namespace SJP.TestProject.PropertyManager
{
    
    
    /// <summary>
    ///This is a test class for PropertyValidatorTest and is intended
    ///to contain all PropertyValidatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PropertyValidatorTest
    {


        private TestContext testContextInstance;
        private static MockPropertyValidator mockPropertyProvider;
        private static FilePropertyProvider propProvider;
        private static string oldValue;

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
        [TestInitialize()]
        public void TestInitialize()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            oldValue = config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value;

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = "PropertyManager/PropertyManagerTest.Properties.xml";

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            propProvider = new FilePropertyProvider();

            propProvider.Load();

            mockPropertyProvider = new MockPropertyValidator(propProvider); 

        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [TestCleanup()]
        public void TestCleanup()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["propertyservice.providers.fileprovider.filepath"].Value = oldValue;

            config.Save();

            ConfigurationManager.RefreshSection("appSettings");

            SJPServiceDiscovery.ResetServiceDiscoveryForTest();
        }
        
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


        internal virtual PropertyValidator_Accessor CreatePropertyValidator_Accessor()
        {
            PropertyValidator_Accessor target = null;
            return target;
        }

        /// <summary>
        ///A test for ValidateClassExists positive test
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateClassExistsTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider); 
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "CurrentTestClass"; 
            Assembly myAssembly = Assembly.GetExecutingAssembly(); 
            List<string> errors = new List<string>(); 
            bool expected = true; 
            bool actual;
            actual = target.ValidateClassExists(key, myAssembly, errors);
           
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(0, errors.Count);
           
        }

        /// <summary>
        ///A test for ValidateClassExists negative test
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateClassExistsNegativeTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "CurrentTestClassNegative";
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            List<string> errors = new List<string>();
            bool expected = false;
            bool actual;
            actual = target.ValidateClassExists(key, myAssembly, errors);

            string message = String.Format(SJP.Common.Messages.ClassNotFoundInAssembly,
                           propProvider[key],
                           key,
                           myAssembly.FullName);

            Assert.AreEqual(expected, actual);

            Assert.AreEqual(1, errors.Count);

            Assert.AreEqual(message.Trim(), errors[0].Trim());

        }

        /// <summary>
        ///A test for ValidateClassExists test raising exception
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateClassExistsExceptionTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "CurrentTestClass1";
            Assembly myAssembly =null;
            List<string> errors = new List<string>();
            bool expected = false;
            bool actual;
            actual = target.ValidateClassExists(key, myAssembly, errors);
                      
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(1, errors.Count);

            myAssembly = Assembly.GetExecutingAssembly();

            errors.Clear();

            actual = target.ValidateClassExists(key, myAssembly, errors);

            Assert.AreEqual(expected, actual);
            
            Assert.AreEqual(1, errors.Count);

        }
        

        /// <summary>
        ///A test for ValidateAssembly
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateAssemblyTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "ProviderAssembly"; 
            List<string> errors = new List<string>();
            Assembly expected = Assembly.GetExecutingAssembly();
            Assembly actual;
            actual = target.ValidateAssembly(key, errors);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.FullName, actual.FullName);
            Assert.AreEqual(0, errors.Count);
        }


        /// <summary>
        ///A test for ValidateAssembly
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateAssemblyTestFileNotFoundException()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "ProviderAssemblyNotFound";
            List<string> errors = new List<string>();
            Assembly actual;
            actual = target.ValidateAssembly(key, errors);

            Assert.IsNull(actual);
            Assert.AreEqual(1, errors.Count);
        }

        /// <summary>
        ///A test for ValidateAssembly
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateAssemblyTestArgumentNullException()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "ProviderAssemblyABC";
            List<string> errors = new List<string>();
            Assembly actual;
            actual = target.ValidateAssembly(key, errors);

            Assert.IsNull(actual);
            Assert.AreEqual(1, errors.Count);
        }
                

        /// <summary>
        ///A test for ValidateLength validating againse min and max boundary values
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateLengthTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);
            string key = "LengthTest"; 
            int min = 4; 
            int max = 7; 
            List<string> errors = new List<string>(); 
            
            bool expected = true; 
            bool actual;
            actual = target.ValidateLength(key, min, max, errors);
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(0, errors.Count);
        }

        /// <summary>
        ///A test for ValidateLength validating againse min and max boundary values negative test
        ///Test failure when key value length is less then the min boundary
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateLengthNegativeTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);
            string key = "LengthTest";
            int min = 6;
            int max = 9;
            List<string> errors = new List<string>();

            bool expected = false;
            bool actual;
            actual = target.ValidateLength(key, min, max, errors);
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(1, errors.Count);

            string message = String.Format(Messages.InvalidPropertyLength,
                    propProvider[key], key, min, max);

            Assert.AreEqual(message, errors[0]);
        }

        /// <summary>
        ///A test for ValidateLength validating againse min and max boundary values negative test
        ///Test failure when key value length is greater then the max boundary
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateLengthNegativeTest1()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);
            string key = "LengthTest";
            int min = 1;
            int max = 4;
            List<string> errors = new List<string>();

            bool expected = false;
            bool actual;
            actual = target.ValidateLength(key, min, max, errors);
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(1, errors.Count);

            string message = String.Format(Messages.InvalidPropertyLength,
                    propProvider[key], key, min, max);

            Assert.AreEqual(message, errors[0]);
        }

        /// <summary>
        ///A test for ValidateLength validating againse only min  boundary values
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateLengthTestMinBoundary()
        {

            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);
            string key = "LengthTest";
            int min = 4;
            List<string> errors = new List<string>();

            bool expected = true;
            bool actual;
            actual = target.ValidateLength(key, min, errors);
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(0, errors.Count);
           
        }

        /// <summary>
        ///A test for ValidateLength validating againse only min  boundary values negative test
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateLengthNegativeTestMinBoundary()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);
            string key = "LengthTest";
            int min = 6;
            List<string> errors = new List<string>();

            bool expected = false;
            bool actual;
            actual = target.ValidateLength(key, min, errors);
           
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(1, errors.Count);

            string message = String.Format(Messages.InvalidPropertyLengthMin,
                    propProvider[key], key, min);

            Assert.AreEqual(message, errors[0]);
        }

        /// <summary>
        ///A test for ValidateExistence
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateExistenceTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "TestExistMandatory";
            
            List<string> errors = new List<string>();

            bool expected = true;
            bool actual;
            actual = target.ValidateExistence(key, PropertyValidator_Accessor.Optionality.Mandatory, errors);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(0, errors.Count);

            key = "TestExistUndefined";

            actual = target.ValidateExistence(key, PropertyValidator_Accessor.Optionality.Undefined, errors);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(0, errors.Count);
        }

        /// <summary>
        ///A test for ValidateExistence
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateExistenceNegativeTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "TestExistMandatoryError";

            List<string> errors = new List<string>();

            bool expected = false;
            bool actual;
            actual = target.ValidateExistence(key, PropertyValidator_Accessor.Optionality.Mandatory, errors);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, errors.Count);

            errors.Clear();

            key = "TestExistUndefinedError";

            actual = target.ValidateExistence(key, PropertyValidator_Accessor.Optionality.Undefined, errors);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, errors.Count);
        }

        

        /// <summary>
        ///A test for ValidateEnumProperty
        ///</summary>
        [TestMethod()]
        [DeploymentItem("sjp.common.propertymanager.dll")]
        public void ValidateEnumPropertyTest()
        {
            PrivateObject param0 = new PrivateObject(mockPropertyProvider);
            PropertyValidator_Accessor target = new PropertyValidator_Accessor(param0);

            string key = "EnumProperty"; 

            Type t = typeof(SqlHelperDatabase); 
           
            List<string> errors = new List<string>();
            
            // positive test
            bool expected = true; 
            bool actual;
            actual = target.ValidateEnumProperty(key, t, PropertyValidator_Accessor.Optionality.Mandatory, errors);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(0, errors.Count);

            // negative test - Property is null
            key = "DefaultDB1";

            errors = new List<string>();

            
            expected = false;
            actual = target.ValidateEnumProperty(key, t, PropertyValidator_Accessor.Optionality.Mandatory, errors);
           
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, errors.Count);

            // negative test - Property is null
            key = "LengthTest";

            errors = new List<string>();


            expected = false;
            actual = target.ValidateEnumProperty(key, t, PropertyValidator_Accessor.Optionality.Mandatory, errors);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, errors.Count);
           
        }

        /// <summary>
        ///A test for StringToEnum
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(SJPException))]
        public void StringToEnumTest()
        {
            Type t = typeof(SqlHelperDatabase); 
            string strValue = "DefaultDB"; 
            object expected = SqlHelperDatabase.DefaultDB; 
            object actual;
            actual = PropertyValidator.StringToEnum(t, strValue);

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, t);
            Assert.AreEqual(SqlHelperDatabase.DefaultDB, (SqlHelperDatabase)actual);

            strValue = "DefaultDB1";
            actual = PropertyValidator.StringToEnum(t, strValue);

        }

        /// <summary>
        ///A test for IsWholeNumber
        ///</summary>
        [TestMethod()]
        public void IsWholeNumberTest()
        {
            string strNumber = "5"; 
            bool expected = true; 
            bool actual;
            actual = PropertyValidator.IsWholeNumber(strNumber);
            Assert.AreEqual(expected, actual);

            strNumber = "5.64";
            expected = false;
            actual = PropertyValidator.IsWholeNumber(strNumber);
            Assert.AreEqual(expected, actual);
        }
    }
}
