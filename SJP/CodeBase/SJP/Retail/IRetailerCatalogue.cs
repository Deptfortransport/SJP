// *********************************************** 
// NAME             : IRetailerCatalogue.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 23 Mar 2011
// DESCRIPTION  	: Interface for the RetailerCatalogue class
// ************************************************
// 

using System.Collections.Generic;
using SJP.Common;

namespace SJP.UserPortal.Retail
{
    /// <summary>
    /// Interface for the RetailerCatalogue
    /// </summary>
    public interface IRetailerCatalogue
    {
        /// <summary>
        /// Returns an immutable arraylist containing all Retailer instances
        /// </summary>
        /// <return>An IList of Retailer instances</return>
        IList<Retailer> GetRetailers();

        /// <summary>
        /// Returns an immutable arraylist containing 1 or more Retailer instances that are capable
        /// of selling tickets for the suppplied operator code and travel mode.
        /// Null is returned if no retailers are found or an invalid combination of parameters
        /// is used.
        /// </summary>
        /// <param name="operatorCode">The operator code. A null value indicates the operator
        /// code is not significant</param>
        /// <param name="mode">The mode of transport</param>
        /// <return>An IList of Retailer instances</return>
        IList<Retailer> FindRetailers(string operatorCode, SJPModeType mode);

        /// <summary>
        /// Returns a retailer for the supplied id
        /// </summary>
        /// <param name="id">The id of the required retailer</param>
        /// <return>A retailer for the supplied id or null if not found</return>
        Retailer FindRetailer(string id);
    }
}
