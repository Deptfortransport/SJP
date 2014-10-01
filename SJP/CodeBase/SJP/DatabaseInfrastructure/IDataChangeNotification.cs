// *********************************************** 
// NAME             : IDataChangeNotification.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 21 Mar 2011
// DESCRIPTION  	: Interface for DataChangeNotification
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DatabaseInfrastructure
{
    /// <summary>
    /// Interface for DataChangeNotification
    /// </summary>
    public interface IDataChangeNotification
    {
        event ChangedEventHandler Changed;
    }

    #region ChangedEventArgs delegate

    /// <summary>
    /// ChangedEventHandler delegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ChangedEventHandler(object sender, ChangedEventArgs e);

    #endregion

    #region ChangedEventArgs class
    /// <summary>
    /// Class to store data for the Changed event.
    /// </summary>
    public class ChangedEventArgs : EventArgs
    {
        #region Private Fields
        private string groupId = string.Empty;
        #endregion

        #region Public Properties
        /// <summary>
        /// GroupId
        /// </summary>
        public string GroupId
        {
            get { return groupId; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// ChangedEventArgs
        /// </summary>
        /// <param name="groupId"></param>
        public ChangedEventArgs(string groupId)
        {
            this.groupId = groupId;
        }
        #endregion
    }
    #endregion
}
