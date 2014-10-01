// *********************************************** 
// NAME             : IPropertyProvider.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 16 Feb 2011
// DESCRIPTION  	: Interface definition for the PropertyService
// ************************************************
                
                
using System;

namespace SJP.Common.PropertyManager
{
    public delegate void SupersededEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Interface definition for the PropertyService
    /// </summary>
    public interface IPropertyProvider
    {
        /// <summary>
        /// Read only IsSuperseded Property
        /// </summary>
        bool IsSuperseded
        {
            get;
        }

        event SupersededEventHandler Superseded;

        /// <summary>
        /// Read-only Version property
        /// </summary>
        int Version
        {
            get;
        }

        /// Read only Indexer property
        string this[string key]
        {
            get;
        }

              
        /// <summary>
        /// Read only ApplicationID Property
        /// </summary>
        string ApplicationID
        {
            get;
        }

        /// <summary>
        /// Read only GroupID property
        /// </summary>
        string GroupID
        {
            get;
        }
    }
}
