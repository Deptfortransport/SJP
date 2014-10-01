// *********************************************** 
// NAME             : MonitoringItem.cs      
// AUTHOR           : Rich Broddle
// DATE CREATED     : 11th April 2011
// DESCRIPTION  	: Implementation of abstract MonitoringItem class
//                    Monitoring Items inherit from this for core functionality
// ************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SJP.Common.Extenders;
using SJP.Common;
using SJP.Common.DatabaseInfrastructure;
using SJP.Common.EventLogging;
using SJP.Common.PropertyManager;
using System.Data.SqlClient;
using Logger = System.Diagnostics.Trace;
using System.Security;
using System.Security.Permissions;

namespace SJP.UserPortal.CCAgent
{

    [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.ControlThread)]
    public abstract class MonitoringItem
    {

        #region Private Fields

        protected int monitoringItemID;
        protected int checkInterval;
        protected DateTime lastCheckTime;
        protected bool checkRequired;
        protected string description;
        protected string valueAtLastCheck;
        protected bool isInRed;
        protected string redCondition;

        #endregion 

        /// <summary>
        /// Time (in seconds) to leave between checks for this monitoring item, as configured in the database.
        /// </summary>
        /// <remarks></remarks>
        public int CheckInterval
        {
            get
            {
                return checkInterval;
            }
            set
            {
                checkInterval = value;
            }
        }

        /// <summary>
        /// ID of this monitoring item, as configured in the database.
        /// </summary>
        /// <remarks></remarks>
        public int MonitoringItemID
        {
            get
            {
                return monitoringItemID;
            }
            set
            {
                monitoringItemID = value;
            }
        }

        /// <summary>
        /// Time the last check for this monitoring item was carried out
        /// </summary>
        /// <remarks></remarks>
        public DateTime LastCheckTime
        {
            get
            {
                return lastCheckTime;
            }
            set
            {
                lastCheckTime = value;
            }
        }

        /// <summary>
        /// Read-Only. Indicates wether a check on this monitoring item is now required, according to CheckInterval and LastCheckTime.
        /// </summary>
        /// <remarks></remarks>
        public bool CheckRequired
        {
            get
            {
                return !(DateTime.Now.CompareTo(lastCheckTime.AddSeconds(checkInterval)) < 0);
            }
        }

        /// <summary>
        /// Indicates the appropriate text description to accompany this monitoring item when displayed.
        /// </summary>
        /// <remarks></remarks>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Result of the latest monitoring item check, formatted as a string which can be diplayed alongside the accompanying Description property.
        /// </summary>
        /// <remarks></remarks>
        public string ValueAtLastCheck
        {
            get
            {
                return valueAtLastCheck;
            }
            set
            {
                valueAtLastCheck = value;
            }
        }

        /// <summary>
        /// Indicates if the result of the latest monitoring item is considered to require flagging up when displayed on a dashboard, according to its configured RedCondition
        /// </summary>
        public bool IsInRed
        {
            get
            {
                try
                {
                //just in case there's any spaces in there
                string redCondNoSpaces = redCondition.Replace(" ", "");
                string redCondNoSpacesOrOperator = redCondition.Replace(" ", "").Substring(1);
                long redCondNumeric, lastCheckValNumeric;

                if (redCondNoSpacesOrOperator.Length > 0)
                    //Some redCond defined - check against the actual value according to the operator required
                    {
                        switch (redCondNoSpaces.Substring(0, 1))
                        {
                            case "=":
                                //we are checking if it equals a value so just treat as string
                                return (valueAtLastCheck == redCondition.Substring(1));
                            case "!":
                                //we are checking if it does not equal a value so just treat as string
                                return (valueAtLastCheck != redCondition.Substring(1));
                            default:
                            // Must be testing for greater than or less than 
                            // - strip the operator off & treat as number
                            redCondNumeric = (redCondNoSpacesOrOperator).Parse<long>();
                            lastCheckValNumeric = valueAtLastCheck.Parse<long>();
                            if (redCondNoSpaces.StartsWith("<"))
                            {
                                return (lastCheckValNumeric < redCondNumeric);
                            }
                            else
                            {
                                if (redCondNoSpaces.StartsWith(">"))
                                {
                                    return (lastCheckValNumeric > redCondNumeric); ;
                                }
                                else
                                {
                                    //unrecognised operator - return red if in doubt
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    //no redCond defined so cant be in red - return false
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    // An error has occurred. Flag up as in red if in doubt
                    return true;
                }
            }

            set
            {
                isInRed = value;
            }

        }

        /// <summary>
        /// String representing a valid C# conditional operator which if evaluating to true indicates a possible problem for this monitoring item.
        /// </summary>
        public string RedCondition
        {
            get
            {
                return redCondition;
            }
            set
            {
                redCondition = value;
            }
        }

        /// <summary>
        /// Checks this monitoring item and updates the "ValueAtLastCheck" property with the result
        /// </summary>
        /// <remarks></remarks>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.ControlThread)]
        abstract public void ReCheck(SqlHelper helper);

    }
}
