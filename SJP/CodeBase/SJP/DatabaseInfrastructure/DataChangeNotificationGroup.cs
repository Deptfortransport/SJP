// *********************************************** 
// NAME             : DataChangeNotificationGroup.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Feb 2011
// DESCRIPTION  	: DataChangeNotificationGroup class represents
/// a single group that needs to be notified of changes to corresponding database
/// tables. It holds the SQLHelperDatabase that holds the tables, and a list of
/// the tables concerned. These properties are checked by the 
/// DataChangeNotification class in order to determine whether or not to raise
/// a Changed event for the group.
// ************************************************
// 
                
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.Common.DatabaseInfrastructure
{
    /// <summary>
    /// DataChangeNotificationGroup class represents
    /// a single group that needs to be notified of changes to corresponding database
    /// tables. It holds the SQLHelperDatabase that holds the tables, and a list of
    /// the tables concerned. These properties are checked by the 
    /// DataChangeNotification class in order to determine whether or not to raise
    /// a Changed event for the group.
    /// </summary>
    public class DataChangeNotificationGroup
    {
        #region Private members

        /// <summary>
        /// The database that holds the tables that concern this group 
        /// </summary>
        private SqlHelperDatabase dataBase;

        /// <summary>
        /// The specific table(s) that concern this group
        /// </summary>
        private string[] tables;

        /// <summary>
        /// Unique identifier for the group, passed from the DataChangeNotification
        /// groupsArray data received via the Property Service
        /// </summary>
        private string groupId;

        /// <summary>
        /// Flag set by this class's IsAffected mehtod, used by DataChangeNotification
        /// service to determine whether to fire a Changed event for this group
        /// </summary>
        private bool raiseEvent;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor, stores initialisation data passed from the
        /// DataChangeNotification class.
        /// </summary>
        /// <param name="groupId">The ID of this group</param>
        /// <param name="currDatabase">The Database that holds the tables that concern this group</param>
        /// <param name="currTables">The specific tables that concern this group</param>
        public DataChangeNotificationGroup(string groupId, SqlHelperDatabase currDatabase, string[] currTables)
        {
            // SqlHelperDatabase that holds this group's tables
            this.dataBase = currDatabase;
            // The tables that concern this group 
            this.tables = currTables;
            // Unique ID of this group
            this.groupId = groupId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Method to return the GroupID of this class
        /// </summary>
        public string GroupId
        {
            // Returns the string groupID of this class
            get { return groupId; }
        }

        /// <summary>
        /// Method to return the SqlHelperDatabase of this class
        /// </summary>
        public SqlHelperDatabase DataBase
        {
            // Returns the SqlHelperDatabase of this class
            get { return dataBase; }
        }

        /// <summary>
        /// Method to return the string array of tables of this class
        /// </summary>
        public string[] Tables
        {
            // Returns a clone of the array
            get { return (string[])tables.Clone(); }
        }

        /// <summary>
        /// Method to Get/Set the private raiseEvent member of this class,
        /// which is used to determine whether a Changed event should be
        /// fired for this group
        /// </summary>
        public bool RaiseEvent
        {
            // Get the current raiseEvent flag
            get { return this.raiseEvent; }
            // Set the current raiseEvent flag to a value
            set { this.raiseEvent = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method checks the database and tablename passed in against
        /// this class's private members. If this class matches those passed
        /// in, the raiseEvent flag will be set to true. This flag is later evaluated
        /// by calling code in DataChangeNotification class to determine whether an
        /// event should be raised.
        /// </summary>
        /// <param name="database">The database to check</param>
        /// <param name="tableName">The table to check</param>
        /// <returns>True if this group is concerned with changes in the database, otherwise false</returns>
        public bool IsAffected(SqlHelperDatabase database, string tableName)
        {
            bool validToRaidEvent = false;

            if (this.dataBase == database)
            {
                //For each of the tables this group observes, check if the supplied
                //table is in it.
                for (int i = 0; i < tables.Length; i++)
                {
                    if (tables[i].Equals(tableName))
                    {
                        validToRaidEvent = true;
                        break;
                    }
                }
            }

            // Set the raiseEvent property
            raiseEvent = validToRaidEvent;

            return raiseEvent;
        }

        #endregion
    }
}
