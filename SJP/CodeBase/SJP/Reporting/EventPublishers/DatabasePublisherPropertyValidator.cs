// *********************************************** 
// NAME             : DatabasePublisherPropertyValidator.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 18 Apr 2011
// DESCRIPTION  	: Validates properties for database publishers class
// ************************************************
// 

using System.Collections.Generic;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.PropertyManager;

namespace SJP.Reporting.EventPublishers
{
    /// <summary>
    /// Validates properties for database publishers class
    /// </summary>
    public class DatabasePublisherPropertyValidator : PropertyValidator
    {
        #region Constructor

        /// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="properties">Properties.</param>
        public DatabasePublisherPropertyValidator(IPropertyProvider properties)
            : base(properties)
		{
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Validates the given property.
        /// </summary>
        /// <param name="key">Property name to validate.</param>
        /// <param name="errors">List to append any errors.</param>
        /// <returns>true on success, false on validation failure.</returns>
        public override bool ValidateProperty(string key, List<string> errors)
        {
            if (key == SqlHelperDatabase.DefaultDB.ToString())
                return ValidateTargetDB(errors);
            else
            {
                // by default validate the key exists
                return ValidateExistence(key, Optionality.Mandatory, errors);
            }

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks target database key containing connection string is valid
        /// </summary>
        /// <param name="errors">Error list.</param>
        /// <returns>true on success, false on validation failure.</returns>
        private bool ValidateTargetDB(List<string> errors)
        {
            int errorsBefore = errors.Count;

            ValidateExistence(SqlHelperDatabase.DefaultDB.ToString(), Optionality.Mandatory, errors);

            return (errorsBefore == errors.Count);
        }

        #endregion
    }
}
