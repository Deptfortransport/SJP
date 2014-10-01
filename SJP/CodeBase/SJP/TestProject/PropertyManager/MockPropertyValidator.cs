// *********************************************** 
// NAME             : MockPropertyValidator.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 18 Feb 2011
// DESCRIPTION  	: Mock PropertyValidator class to help testing protected methods of PropertyValidator
// ************************************************
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.PropertyManager;

namespace SJP.TestProject.PropertyManager
{
    /// <summary>
    /// Mock PropertyValidator class to help testing protected methods of PropertyValidator
    /// </summary>
    class MockPropertyValidator : PropertyValidator
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propProvider"></param>
        public MockPropertyValidator(IPropertyProvider propProvider)
            : base(propProvider)
        {
            
        }
        #endregion

        /// <summary>
        /// Implementation of abstract method 
        /// The method just returns true as the class is written to help testing 
        /// protected methods of PropertyValidator class
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public override bool ValidateProperty(string key, List<string> errors)
        {
            return true;
        }
    }
}
