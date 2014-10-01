// *********************************************** 
// NAME             : UndergroundStatusItem.cs      
// AUTHOR           : Mitesh Modi
// DATE CREATED     : 10 Mar 2012
// DESCRIPTION  	: UndergroundStatusItem class for information about the status of an underground line
// ************************************************
// 
                
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SJP.UserPortal.UndergroundNews
{
    /// <summary>
    /// UndergroundStatusItem class for information about the status of an underground line
    /// </summary>
    [Serializable()]
    public class UndergroundStatusItem
    {
        #region Private members

        string lineStatusId = string.Empty;
        string lineStatusDetails = string.Empty;
        string lineId = string.Empty;
        string lineName = string.Empty;
        string statusId = string.Empty;
        string statusDescription = string.Empty;
        bool statusIsActive = false;
        string statusCssClass = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UndergroundStatusItem()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Read/Write. LineStatusID
        /// </summary>
        public string LineStatusID 
        { 
            get { return lineStatusId; } 
            set { lineStatusId = value; } 
        }

        /// <summary>
        /// Read/Write. LineStatusDetails
        /// </summary>
        public string LineStatusDetails
        {
            get { return lineStatusDetails; }
            set { lineStatusDetails = value; }
        }

        /// <summary>
        /// Read/Write. LineId
        /// </summary>
        public string LineId
        {
            get { return lineId; }
            set { lineId = value; }
        }

        /// <summary>
        /// Read/Write. LineName
        /// </summary>
        public string LineName
        {
            get { return lineName; }
            set { lineName = value; }
        }

        /// <summary>
        /// Read/Write. StatusId
        /// </summary>
        public string StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }

        /// <summary>
        /// Read/Write. StatusDescription
        /// </summary>
        public string StatusDescription
        {
            get { return statusDescription; }
            set { statusDescription = value; }
        }

        /// <summary>
        /// Read/Write. StatusIsActive
        /// </summary>
        public bool StatusIsActive
        {
            get { return statusIsActive; }
            set { statusIsActive = value; }
        }

        /// <summary>
        /// Read/Write. StatusCssClass
        /// </summary>
        public string StatusCssClass
        {
            get { return statusCssClass; }
            set { statusCssClass = value; }
        }

        #endregion
    }
}
