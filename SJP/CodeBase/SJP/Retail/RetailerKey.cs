// *********************************************** 
// NAME             : RetailerKey.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Mar 2011
// DESCRIPTION  	: RetailerKey class implments a key that identifies retailers capable of 
// selling tickets for the operator code and travel mode defined by the key value.
// ************************************************
// 

using SJP.Common;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// RetailerKey class implments a key that identifies retailers capable of 
    /// selling tickets for the operator code and travel mode defined by the key value.
    /// </summary>
    internal class RetailerKey
    {
        #region Private members

        private SJPModeType mode;
        private string operatorCode;

        // Value to indicate operator code is not significant for lookup of retailer
        public const string IgnoreOperatorCode = "NONE";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a composite key based on the supplied parameters.
        /// </summary>
        /// <param name="operatorCode">The operator code</param>
        /// <param name="mode">The travel mode</param>
        public RetailerKey(string operatorCode, SJPModeType mode)
        {
            this.mode = mode;
            this.operatorCode = operatorCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The operator code of the composite key.
        /// </summary>
        public string OperatorCode
        {
            get { return operatorCode; }
        }

        /// <summary>
        /// The travel mode of the composite key.
        /// </summary>
        public SJPModeType Mode
        {
            get { return mode; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines the equality of two key instances.
        /// </summary>
        /// <returns>True if supplied object is of the same type as this instance and
        /// operator code and travel modes are equal, false otherwise.</returns>
        public override bool Equals(object o)
        {
            if (o is RetailerKey)
            {
                RetailerKey retailerKey = (RetailerKey)o;
                return (retailerKey.OperatorCode.Equals(operatorCode) &&
                        retailerKey.Mode.Equals(mode));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a hash value for this instance.
        /// </summary>
        /// <returns>A hash value of this instance</returns>
        public override int GetHashCode()
        {
            // Given that it is anticipated we will have a small number of retailers
            // we can produce a perfect hash
            return (operatorCode.GetHashCode() + mode.GetHashCode());
        }

        #endregion
    }
}
