// *********************************************** 
// NAME             : UndergroundNewsHandler.cs
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 05 Mar 2012
// DESCRIPTION  	: Interface for classes that make underground news available to clients
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.UndergroundNews
{
    /// <summary>
    /// Interface for classes that make underground news available to clients
    /// </summary>
    public interface IUndergroundNewsHandler
    {
        /// <summary>
        /// Returns if underground news is available
        /// </summary>
        bool IsUndergroundNewsAvaliable { get; }

        /// <summary>
        /// Returns the underground status last updated date time 
        /// </summary>
        DateTime UndergroundStatusLastUpdated { get; }

        /// <summary>
        /// Returns underground status data
        /// </summary>
        List<UndergroundStatusItem> GetUndergroundStatusItems();
    }
}
