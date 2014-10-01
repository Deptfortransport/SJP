// *********************************************** 
// NAME             : IDataServices.cs      
// AUTHOR           : Amit Patel
// DATE CREATED     : 07 Apr 2011
// DESCRIPTION  	: Data Services Interface - Implements data services similar to TD
// ************************************************
                
                    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SJP.Common.ResourceManager;
using System.Web.UI.WebControls;

namespace SJP.Common.DataServices
{
    /// <summary>
    /// Data Services Interface - Implements data services similar to TD
    /// </summary>
    public interface IDataServices
    {

        /// <summary>
        /// Returns an Array-List type object from cache, performs type checking.
        /// </summary>
        /// <param name="item">Enumeration must refer to Array-List type.</param>
        /// <returns>Structure could contain a list of date and dropdown items.</returns>
        ArrayList GetList(DataServiceType item);

        /// <summary>
        /// Method that populates a list control with items using dataservices.
        /// </summary>
        /// <param name="dataSet">The dataset that contains dropdown items.</param>
        /// <param name="control">The control, eg DropDownList, to be populated.</param>
        /// <param name="rm">The resource manager to use for looking up lang strings</param>
        void LoadListControl(DataServiceType dataSet, ListControl control, SJPResourceManager rm, Language language);

        /// <summary>
        /// Returns the display text for a data service type value
        /// </summary>
        string GetText(DataServiceType type, string value, SJPResourceManager rm, Language language);
    }
}
