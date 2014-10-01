// *********************************************** 
// NAME             : TestService.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 27 Jun 2011
// DESCRIPTION  	: Test service to help testing Service Discovery
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.TestProject.ServiceDiscovery
{
    /// <summary>
    /// Test service to help testing Service Discovery
    /// </summary>
    public class TestService
    {
        #region Private Fields
        private List<int> testData = new List<int>();
        #endregion

        #region Public Properties
        /// <summary>
        /// Exposes the test data
        /// </summary>
        public List<int> TestData
        {
            get
            {
                return testData;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TestService()
        {
            LoadTestData();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Load the test data for the servic
        /// </summary>
        private void LoadTestData()
        {
            testData.Add(1);
            testData.Add(2);
            testData.Add(3);
            testData.Add(4);
            testData.Add(5);
        }
        #endregion

    }
}
